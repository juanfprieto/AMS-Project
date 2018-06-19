using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Web;

namespace AMS.Nomina
{
	public class LiquidacionVacaciones : System.Web.UI.UserControl 
	{
		protected DataTable DataTable1 = new DataTable ();
		protected DataGrid DATAGRIDVACACIONESCAUSADAS,DATAGRIDPERESCOGIDO,DataGrid2;
        protected Button BTNINGRESAR, BTNLIQUIDAR, BTNLIQUIDAR2;
		protected Label LBVACAAPAGAR,LBDTVACACIONES,LBBASELIQVACA,LBPERIODO,LBDIASEFECTIVOS;
		protected Label Label1;
		protected Panel Panel1;
		string    lbpag,lbtipopago;
		protected DataTable DataTable2 = new DataTable();
		protected ArrayList Adisponibles = new ArrayList();
        string    fechaIniVaca;
        DateTime  fechaIniQuin = new DateTime();

        double    diapromvacaciones, sueldovacaciones, valormsusplicencias, valorepsempleado = 0;
			
		string    mainpage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		int       disponibles=0;
        string    secuencia, codigoempleado, codquincena;
		int       diasvacas2,diasTrabajados;
		double    acumuladoeps=0,sumadescontado=0;

        double    saldo, valorvacaciones, valorEpsyadescontado, valorfondopensionvoluntaria = 0;
		double    acumuladocesantia=0,acumuladoprima=0,acumuladovacaciones=0,acumuladoretefuente=0,eventoepesfondo=0;
		double    sumapagado=0,sumapagadamasauxt=0,valorfondopensionempleado=0,fondosolidaridadpensional=0;
		double    acumuladoprovisiones=0,acumuladoliqdefinitiva=0,acumuladohorasextras=0,diasexceptosauxtransp=0;
		string    fechafinal,DDLQUIN,fechainicial,formaPagoVac;
		int       errores=0,bandera1=0,diasvacas=0;
		//dias q trabajo en la ultima liquidacion.
		int       diasvacaciones = 0;
		protected System.Web.UI.WebControls.Label lb;
		protected System.Web.UI.WebControls.Label lbmas1;
		protected System.Web.UI.WebControls.Label lb2;
		protected System.Web.UI.WebControls.Button Button1;
		protected CNomina o_CNomina = new CNomina();
		protected Varios o_Varios = new Varios();
		protected int diav;
		protected int mesv;
		protected System.Web.UI.WebControls.Label LBLCEDULA;
		protected System.Web.UI.WebControls.Label LBLEMPLEADO;
		protected int anov;

		
		protected void PagoLiquidacionVacaciones(string codigoempleado, string diasefectivos, string causadas)
		{
			this.preparargrilla_empleadoliquidacion();
			this.validardiasdepago(codigoempleado);
			Panel1.Visible   =true;
			LBPERIODO.Visible=true;
			DataGrid2.Visible=true;
		}
		
		
		//pagarle lo q devengo en los dias antes de salir de vacaciones
		//this.preparargrilla_empleadoliquidacion();
		//this.validardiasdepago(empleados.Tables[0].Rows[0][6].ToString(),lb.Text,lb2.Text,empleados.Tables[0].Rows[0][0].ToString(),cnomina.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][4].ToString(),lbdocref.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString(),empleados.Tables[0].Rows[0][10].ToString(),lb3.Text);
		protected void ingresovacacionesgrilla(string  codigoempleado,string lb ,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string tiposalario)
		{
			string concepto;
			DataSet conceptovaca = new DataSet(); // incluir concepto vacaciones en dinero y grabarlo aparte con su valor
			DBFunctions.Request(conceptovaca,IncludeSchema.NO,"select cnom_concvacacodi from dbxschema.cnomina ");
			concepto=conceptovaca.Tables[0].Rows[0][0].ToString();
			string descripcionconcepto=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+concepto+"' ");
			//double valoreventovacas=int.Parse(valorliquidacion.ToString())/int.Parse(DDLDIASEFECTIVOS.SelectedValue.ToString());
			//this.ingresar_datos_datatable(codempleado,concepto,int.Parse(vacaciones.Tables[0].Rows[i][0].ToString()),valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
		}
		
		protected void liquidar_epsfondo(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string tiposalario)
		{
             DataSet cnom = new DataSet();
				DBFunctions.Request(cnom,IncludeSchema.NO,"SELECT CNOM_NOMBPAGA,PANO_DETALLE,PMES_NOMBRE,TPER_DESCRIPCION,TOPCI_DESCRIPCION FROM DBXSCHEMA.PANO P, DBXSCHEMA.CNOMINA C,DBXSCHEMA.PMES  T,DBXSCHEMA.TPERIODONOMINA N,DBXSCHEMA.TOPCIQUINOMES O WHERE C.CNOM_ANO=P.PANO_ANO AND C.CNOM_MES=T.PMES_MES AND C.CNOM_QUINCENA=N.TPER_PERIODO AND C.CNOM_OPCIQUINOMENS=O.TOPCI_PERIODO");
				lbtipopago=cnom.Tables[0].Rows[0][4].ToString();
				
				//Sacar valor maximo de liquidacion
				string  cantsal  = DBFunctions.SingleData("select cnom_topepagoeps from dbxschema.cnomina");
				string  salminimo= DBFunctions.SingleData("select cnom_salaminiactu from dbxschema.cnomina");
				double  valor    = (double.Parse(cantsal)*double.Parse(salminimo));

                int numquincenaanterior = 0, numquincenaactual = 0;
				DataSet mquincenas= new DataSet();	
				DataSet valorepsquinanterior   = new DataSet();	
				DataSet valorfondoquinanterior = new DataSet();	
				DataSet cnomina = new DataSet();
				DataSet cnomina2= new DataSet();
				DateTime fechaquin= new DateTime();
				valorepsempleado = 0;
				//Traigo el ultimo registro del maestro de Quincenas
				string  fondo                   =DBFunctions.SingleData("select cnom_concfondcodi from dbxschema.cnomina");
				string  eps                     =DBFunctions.SingleData("select cnom_concepscodi from dbxschema.cnomina");
				string  descripcionconceptofondo=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+fondo+"' ");
				string  descripcionconceptoeps  =DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+eps+"' ");
				string  tipocontrato            =DBFunctions.SingleData("select T.tcon_contrato from dbxschema.tcontratonomina T,dbxschema.mempleado M where memp_codiempl='"+codigoempleado+"' and T.tcon_contrato=M.tcon_contrato ");
			
				DBFunctions.Request(cnomina2,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");
			
				//*********************************que solo mire esto para pagos quincenales
				if(tiposalario!="1") // 1-indefinido  2-termino fijo  3-sena  4-pensionado
				{
					if (acumuladoeps>valor)
						acumuladoeps=valor;
                }
				
				fechaquin=Convert.ToDateTime(lb);
				//buscar base mes pasado					
				fechaquin=Convert.ToDateTime(lb);			
	 	//h		fechaquin= fechaquin.AddMonths(-1);
				string quinAnt = DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=1 "); // tenia 2
                string quinAct = DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin=" + fechaquin.Year.ToString() + " and MQUI.mqui_mesquin=" + fechaquin.Month.ToString() + " and MQUI.mqui_tpernomi=2 "); // tenia 2
                numquincenaanterior = Convert.ToInt32(quinAnt == string.Empty ? "0" : quinAnt);
                numquincenaactual = Convert.ToInt32(quinAct == string.Empty ? "0" : quinAct);
						
				DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_porcepsempl,cnom_concepscodi,cnom_epsperinomi,cnom_concfondcodi,cnom_porcfondempl,cnom_baseporcsalainteg from dbxschema.cnomina");	
							
				//averiguar en que quincena estoy, primera no calculo nada, segunda si.
			
				DataSet valepsquinanterior= new DataSet();	
				//JFSC 29042008 deben ser vacaciones
                DBFunctions.Request(valepsquinanterior, IncludeSchema.NO, "select coalesce(sum(dqui_apagar),0) from dbxschema.dquincena D, pconceptonomina p where D.memp_codiempl= '" + codigoempleado + "' and D.mqui_codiquin in (" + numquincenaanterior + "," + numquincenaactual + ") and d.pcon_concepto = p.pcon_concepto  and p.TRES_afec_EPS = 'S'");

                valorEpsyadescontado = Convert.ToDouble(DBFunctions.SingleData("select coalesce(sum(dqui_adescontar),0) from dbxschema.dquincena D where D.memp_codiempl= '" + codigoempleado + "' and D.mqui_codiquin in (" + numquincenaanterior + "," + numquincenaactual + ") and d.pcon_concepto ='" + eps + "' "));
                double valorFondoyadescontado = Convert.ToDouble(DBFunctions.SingleData("select coalesce(sum(dqui_adescontar),0) from dbxschema.dquincena D where D.memp_codiempl= '" + codigoempleado + "' and D.mqui_codiquin in (" + numquincenaanterior + "," + numquincenaactual + ") and d.pcon_concepto ='" + fondo + "' "));		
	
				acumuladoeps = acumuladoeps + double.Parse(valepsquinanterior.Tables[0].Rows[0][0].ToString());
                acumuladoeps += valormsusplicencias;         
				if (tiposalario=="1")
				{
					if (acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0]["cnom_baseporcsalainteg"].ToString())/100>valor)
					{
						valorfondopensionempleado=valor*double.Parse(cnomina.Tables[0].Rows[0]["cnom_porcfondempl"].ToString())/100;
						valorepsempleado=valor*double.Parse(cnomina.Tables[0].Rows[0]["cnom_porcepsempl"].ToString())/100;
					}
					else
					{
						valorfondopensionempleado=(acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0]["cnom_baseporcsalainteg"].ToString())/100)*double.Parse(cnomina.Tables[0].Rows[0]["cnom_porcfondempl"].ToString())/100;
						valorepsempleado=(acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0]["cnom_baseporcsalainteg"].ToString())/100)*double.Parse(cnomina.Tables[0].Rows[0]["cnom_porcepsempl"].ToString())/100;
					}											
				}
				else
				{				
					//JFSC 25042008 Modificando para calcularlo sobre el acumulado 
					//valorepsempleado=sueldo*double.Parse(cnomina.Tables[0].Rows[0]["cnom_porcepsempl"].ToString())/100;
					valorepsempleado=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0]["cnom_porcepsempl"].ToString())/100;
					//valorepsempleado=(valorepsempleado/30)*eventoepesfondo;
					//valorfondopensionempleado=sueldo*double.Parse(cnomina.Tables[0].Rows[0]["cnom_porcfondempl"].ToString())/100;
					valorfondopensionempleado=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0]["cnom_porcfondempl"].ToString())/100;
					//valorfondopensionempleado=(valorfondopensionempleado/30)*eventoepesfondo;
				}

                valorepsempleado          -= valorEpsyadescontado;
                valorfondopensionempleado -= valorFondoyadescontado;

				if (tipocontrato!="3")
				{					
					this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concepscodi"].ToString(),1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,descripcionconceptoeps);
					sumadescontado+=valorepsempleado;
				}
				
                //4=pensionado,3=sena.
				if (tipocontrato!="4" && tipocontrato!="3")
				{
					this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concfondcodi"].ToString(),1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,descripcionconceptofondo);
					sumadescontado+=valorfondopensionempleado;	
				}
		}
		
		protected void validardiasdepago(string codigoempleado)
		{
			eventoepesfondo=0;
            diasvacaciones = 0;
			DataSet     conceptovaca = new DataSet();
			DBFunctions.Request(conceptovaca,IncludeSchema.NO,"select cnom_concvacacodi from dbxschema.cnomina ");
			string      concepto=conceptovaca.Tables[0].Rows[0][0].ToString();
			string      descripcionconceptovacaciones=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+concepto+"' ");
			int         sumDiasE=0,j;
			DataSet     afectacionessueldo= new DataSet();
			DBFunctions.Request(afectacionessueldo,IncludeSchema.NO,"select P.pcon_signoliq,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from dbxschema.cnomina C,dbxschema.pconceptonomina P where C.cnom_concvacacodi=P.pcon_concepto");
			DataSet     cnomina2 = new DataSet();
			DBFunctions.Request(cnomina2,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");
			DataSet     empleados = new DataSet();
			DataSet     empleados2 = new DataSet();
			DBFunctions.Request(empleados2,IncludeSchema.NO,"Select memp_peripago,memp_suelactu from DBXSCHEMA.mempleado where MEMP_CODIEMPL='"+codigoempleado+"'");
			DBFunctions.Request(empleados,IncludeSchema.NO,"Select M.MEMP_CODIEMPL ,N.MNIT_NOMBRES ,N.MNIT_APELLIDOS , N.MNIT_APELLIDOS ,M.MEMP_SUELACTU , M.memp_suelactu ,M.memp_fechingreso ,N.MNIT_NOMBRES,M.tsub_codigo,M.tsal_salario,M.memp_peripago,M.tcon_contrato,M.memp_fechfincontrato from DBXSCHEMA.mempleado M, dbxschema.mnit N  where M.MNIT_NIT=N.MNIT_NIT  AND M.test_estado='1'  and M.memp_codiempl='"+codigoempleado+"' ");
			string      descripcionconcepto=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString()+"' ");
            int         diasmesfin, diasquincena, diasactuales, i;
			DateTime    fechaIniVacaciones= new DateTime();
			DateTime    fechaFinVacaciones= new DateTime();
			DateTime    fechaFinQuin= new DateTime();
			DateTime    fechaIngreso= new DateTime();
			fechaIngreso  = DateTime.Parse(empleados.Tables[0].Rows[0][6].ToString());
			double      sumasueldopromedio=0;
			string      lbdocref,fechaFinVaca;
			string      periodopago=DBFunctions.SingleData("Select memp_peripago from DBXSCHEMA.mempleado where MEMP_CODIEMPL='"+codigoempleado+"'");
            string      periodopagoEmpresa = DBFunctions.SingleData("Select cnom_formpago from DBXSCHEMA.CNOMINA");
            DateTime fechaIniVaca2 = new DateTime();
			int         mesIngreso,diaIngreso,anoIngreso,diasFechaIngreso;
			int         mesquincena,diaquincena,anoquincena,mesvacaciones,diavacaciones,anovacaciones;
			int         diaquincenafin,anoquincenafin,mesquincenafin,diasquincenafin;
			int         mesvacacionesfin,diavacacionesfin,anovacacionesfin,diasvacacionesfin,diasvacacionesapagar;
			//para el calculo de dias en vacaciones
			int         DiasEnVacaciones = 0;
			
			//Miro la quincena
			if (cnomina2.Tables[0].Rows[0]["cnom_quincena"].ToString()=="1")
			{
			//conversion fecha de cnomina para la quincena vigente yyyy-mm-dd a yyyy-mm-dd para cada empleado
                if (periodopago == "1" || periodopago == "4")
				{
					if(Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString())>=1 
                    && Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString())<=9)
					{
						lb.Text=cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+"-0"+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+"-"+"01";
						lb2.Text=cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+"-0"+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+"-"+"15";
						lbmas1.Text=cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+"-0"+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+"-"+"16";
					}
					else
					{
						lb.Text=cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+"-"+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+"-"+"01";
						lb2.Text=cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+"-"+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+"-"+"15";
						lbmas1.Text=cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+"-"+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+"-"+"16";
					}
				}
				if (periodopago=="2")
				{
                    if (Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) >= 1 
                    && Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) <= 9)
                    {
                        if (Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 2)
                        {
                            int diaFinal = validarAnoBisiesto(Convert.ToInt16(cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()));
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "01";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "28";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + diaFinal;
                        }
                        else
                        {
                            if (Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 4
                               || Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 6
                               || Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 9
                               || Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 11)
                            {
                                lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "01";
                                lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                                lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                            }
                            else
                            {
                                lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "01";
                                lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                                lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "31";
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 11)
                        {
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0][1].ToString() + "-" + "01";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0][1].ToString() + "-" + "30";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0][1].ToString() + "-" + "30";
                        }
                        else
                        {
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0][1].ToString() + "-" + "01";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0][1].ToString() + "-" + "30";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0][1].ToString() + "-" + "31";
                        }
                    }
				}
			}
		
		
		//SI SE ESTA PAGANDO LA SEGUNDA QUINCENA
			if (cnomina2.Tables[0].Rows[0]["cnom_quincena"].ToString()=="2")
			{
				//conversion fecha de cnomina para la quincena vigente yyyy-mm-dd a yyyy-mm-dd 
                if (periodopago == "1" || periodopago == "3" || periodopago == "4")
				{
					if(Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString())>=1 
                    && Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString())<=9)
					{
                        if (Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 2)
                        {
                            int diaFinal = validarAnoBisiesto(Convert.ToInt16(cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()));
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "16";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "28";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + diaFinal;
                        }
                        else
                        {
                            if ((Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 4)
                            || (Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 6)
                            || (Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 9))
                            {
                                //lb.Text=cnomina2.Tables[0].Rows[0][0].ToString()+"-0"+cnomina2.Tables[0].Rows[0][1].ToString()+"-"+"15";
                                lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "16";
                                lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                                lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                            }
                            else
                            {
                                //lb.Text=cnomina2.Tables[0].Rows[0][0].ToString()+"-0"+cnomina2.Tables[0].Rows[0][1].ToString()+"-"+"15";
                                lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "16";
                                lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                                lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "31";
                            }
                        }
					}
					else
					{
                        if((Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 11))
                        {
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "16";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                        }
                        else
                        {
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "16";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "31";
                        }
						//lb.Text=cnomina2.Tables[0].Rows[0][0].ToString()+"-"+cnomina2.Tables[0].Rows[0][1].ToString()+"-"+"15";
						
					}	
				}
				//periodo de pago mensual
				if (periodopago=="2")
				{
					if(Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString())>=1 
                    && Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString())<=9)
					{
						if(Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString())==2)
						{
                            //int diafeb = Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()) / 4;
                            //diafeb = diafeb * 4;
                            //if (diafeb == Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()))
                            //    diafeb = 29;
                            //else
                            //    diafeb = 28;
                            int diafeb = validarAnoBisiesto(Convert.ToInt16(cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()));
                            lb.Text=cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+"-0"+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+"-"+"01";
							lb2.Text=cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+"-0"+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+"-"+diafeb;
                      		lbmas1.Text=cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+"-0"+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+"-"+diafeb;
						}

						else if(Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 4 
                             || Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 6 
                             || Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 9)
                        {
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "01";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                        }
                        else
                        {
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "01";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-0" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "31";
                        }
					}
                    else
                    {
                        if (Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()) == 11)
                        {
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "01";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                        }
                        else
                        {
                            lb.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "01";
                            lb2.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "30";
                            lbmas1.Text = cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString() + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString() + "-" + "31";
                        }
                    }
				}
			}
		
		    fechaIniVaca    =((DropDownList)DATAGRIDPERESCOGIDO.Items[0].Cells[7].FindControl("DDLANOINI")).SelectedValue+"-"+((DropDownList)DATAGRIDPERESCOGIDO.Items[0].Cells[7].FindControl("DDLMESINI")).SelectedValue+"-"+((DropDownList)DATAGRIDPERESCOGIDO.Items[0].Cells[7].FindControl("DDLDIAINI")).SelectedValue;
		    fechaIniQuin    =Convert.ToDateTime(lb.Text);
		    fechaIniVaca2   =Convert.ToDateTime(fechaIniVaca);
		    fechaFinVaca    =((DropDownList)DATAGRIDPERESCOGIDO.Items[DATAGRIDPERESCOGIDO.Items.Count-1].Cells[8].FindControl("DDLANOFIN")).SelectedValue+"-"+((DropDownList)DATAGRIDPERESCOGIDO.Items[DATAGRIDPERESCOGIDO.Items.Count-1].Cells[8].FindControl("DDLMESFIN")).SelectedValue+"-"+((DropDownList)DATAGRIDPERESCOGIDO.Items[DATAGRIDPERESCOGIDO.Items.Count-1].Cells[8].FindControl("DDLDIAFIN")).SelectedValue;
		    fechaFinVacaciones=Convert.ToDateTime(fechaFinVaca);
		
			//calcular dias Ingreso a la empresa
			mesIngreso      =int.Parse(fechaIngreso.Month.ToString());
			diaIngreso      =int.Parse(fechaIngreso.Day.ToString());
			anoIngreso      =fechaIngreso.Year;
			diasFechaIngreso=(anoIngreso*360)+(mesIngreso*30)+diaIngreso;

			//calcular dias inicio de la quincena
			mesquincena     =int.Parse(fechaIniQuin.Month.ToString());
			diaquincena     =int.Parse(fechaIniQuin.Day.ToString());
			anoquincena     =fechaIniQuin.Year;

         	//calcular dias fin  de la quincena
			fechaFinQuin    =Convert.ToDateTime(lb2.Text);
			mesquincenafin  =int.Parse(fechaFinQuin.Month.ToString());
			diaquincenafin  =int.Parse(fechaFinQuin.Day.ToString());
			anoquincenafin  =fechaFinQuin.Year;
        
            int diasMes = 30;
            if (mesquincenafin == 2)  // febrero tiene 28 dias ...0 29 si ano es bisiesto
            {
                //diasMes = Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()) / 4;
                //diasMes = diasMes * 4;
                //if (diasMes == Convert.ToInt32(cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()))
                //    diasMes = 29;
                //else
                //    diasMes = 28;
                diasMes = validarAnoBisiesto(Convert.ToInt16(cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()));
            }

            diasquincena = (anoquincena * 360) + (mesquincena * diasMes) + diaquincena;
            diasmesfin   = (anoquincena * 360) + (mesquincena * diasMes) + diasMes;


            diasquincenafin = (anoquincenafin * 360) + (mesquincenafin * diasMes) + diaquincenafin;
			
			//calcular dias del inicio de vacaciones
			mesvacaciones   =int.Parse(fechaIniVaca2.Month.ToString());
			diavacaciones   =int.Parse(fechaIniVaca2.Day.ToString());
			anovacaciones   =fechaIniVaca2.Year;

            diasactuales = (anovacaciones * 360) + (mesvacaciones * diasMes) + diavacaciones;
		
			int totaldiasCalculoPromedio = diasactuales - diasFechaIngreso;
			//Si es Salario Integral se toma como base el mes anterior.
			string tipoSalario=DBFunctions.SingleData("select tsal_salario from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"'");
			//JFSC 22042008	si es salario variable, por 360 dias, de lo contrario, por 90
			if(tipoSalario=="2")
			  {
				if (totaldiasCalculoPromedio >= 360)
				   {
					totaldiasCalculoPromedio = 360;
				   }
			  }
			else
			   {
				if (totaldiasCalculoPromedio >= 90)
				   {
					totaldiasCalculoPromedio = 90;
				   }
			   }

			//calcular los dias de fin de las vacaciones
			mesvacacionesfin=int.Parse(fechaFinVacaciones.Month.ToString());
			diavacacionesfin=int.Parse(fechaFinVacaciones.Day.ToString());
			anovacacionesfin=fechaFinVacaciones.Year;

            diasvacacionesfin = (anovacacionesfin * 360) + (mesvacacionesfin * diasMes) + diavacacionesfin;
		
			int diasadiconales = Int32.Parse(((System.Web.UI.WebControls.TextBox)DATAGRIDPERESCOGIDO.Items[0].Cells[7].FindControl("DiasAjuste")).Text);
				
			diasvacacionesapagar=diasvacacionesfin - diasactuales + diasadiconales;
			diasvacacionesapagar+=1;
			
			//calcular dias excepto aux de transporte
            /*
            if (diasquincenafin >= diasvacacionesfin)
                diasexceptosauxtransp = diasvacacionesfin - diasactuales;
            else
            {
                if (diasmesfin <= diasvacacionesfin)
                    diasexceptosauxtransp = diasmesfin - diasactuales;
                else
                    diasexceptosauxtransp = diasquincenafin - diasactuales;
            }
			diasexceptosauxtransp+=1;
            */
			
			DataSet mquincenas = new DataSet();
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			
			DataSet cnomina = new DataSet();
			DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes,cnom_salaminiactu from dbxschema.cnomina");
			
		    //calcular los dias de trabajo.
		    diasTrabajados  = diasactuales - diasquincena;
            //if (mesquincenafin == 2 && diaquincenafin != 15)  // febrero tiene 28 0 29 dias ...dependiendo año bisiesto
            //{
            //    if(diaquincenafin == 28)
            //        diasTrabajados += 2;
            //    else
            //        diasTrabajados += 1;
            //}
		    string sueldomes= empleados2.Tables[0].Rows[0][1].ToString();
		    double valordiatrabajo=double.Parse(sueldomes)/30;

            //calcular dias de licencias-suspenciones-incapacidades liquidadas en dias y periodos anteriores con continuidad en este periodo  
            int DiasSusLicencias = 0;
            DataSet FechasSusplicencias = new DataSet();
            string sqlLicencias = @"Select msus_desde, msus_hasta, M.pcon_concepto, P.pcon_nombconc, M.ttip_coditipo, P.pcon_signoliq, T.tdes_descripcion
                                from DBXSCHEMA.MSUSPLICENCIAS M, pconceptonomina p,  tdesccantidad T 
                                where memp_codiempl='" + codigoempleado.ToString().Trim() + @"' and  msus_desde <= '" + fechaIniVaca + @"'  and  msus_hasta >= '" + lb.Text + @"' and m.pcon_concepto = p.pcon_concepto and (P.pcon_desccant=T.tdes_cantidad) 
                                 union
                                Select dvac_fechinic, dvac_fechfinal, cn.cnom_concvacacodi, P.pcon_nombconc, 0 AS ttip_coditipo, P.pcon_signoliq, T.tdes_descripcion
                                  from DVACACIONES d, mVACACIONES m, cnomina cn, pconceptonomina p,  tdesccantidad T
                                 where m.mVAC_SECUENCIA = d.mVAC_SECUENCIA and memp_codiemp = '" + codigoempleado.ToString().Trim() + @"' and dvac_fechinic <= '" + Convert.ToDateTime(fechaIniQuin).ToString("yyyy-MM-dd") + @"'  and dvac_fechfinal >= '" + Convert.ToDateTime(fechaIniQuin).ToString("yyyy-MM-dd") + @"'
                                   and cn.cnom_concvacacodi = p.pcon_concepto and(P.pcon_desccant = T.tdes_cantidad);

            "; // and pcon_signoliQ <> 'H' 
            //   D = A PAGAR    H = A DESCONTAR
            /*
             DBFunctions.Request(msusplicencias2,IncludeSchema.NO,"
             * select distinct M.pcon_concepto, M.memp_codiempl, M.msus_desde, M.msus_hasta, ('"+lab2.ToString()+"'-M.msus_desde)+1,
             *        M.ttip_coditipo, P.pcon_signoliq, T.tdes_descripcion, P.pcon_nombconc 
             *   from msusplicencias M, pconceptonomina P, tdesccantidad T, msusplicencias M2 
             *  where M.memp_codiempl='"+codigoempleado+"' 
             *    and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) 
             *    and (M.msus_desde between '"+lab.ToString()+"' and '"+lab2.ToString()+"') and (M.msus_hasta between '"+lab2mas11.ToString()+"' and M2.msus_hasta)");
			
            */

            //string lbmas1;
            lbdocref = cnomina.Tables[0].Rows[0][0] + "-" + cnomina.Tables[0].Rows[0][1] + "-" + cnomina.Tables[0].Rows[0][2];
            string docref = cnomina2.Tables[0].Rows[0]["cnom_ano"] + "-" + cnomina2.Tables[0].Rows[0]["cnom_mes"] + "-" + cnomina2.Tables[0].Rows[0]["cnom_quincena"];

            DBFunctions.Request(FechasSusplicencias, IncludeSchema.NO, sqlLicencias);
		    for (i=0;i<FechasSusplicencias.Tables[0].Rows.Count;i++)
		    {
			    if(DateTime.Compare(Convert.ToDateTime(FechasSusplicencias.Tables[0].Rows[i][0]), fechaIniQuin) < 0)
                    FechasSusplicencias.Tables[0].Rows[i][0] = Convert.ToDateTime(fechaIniQuin).ToString("yyyy-MM-dd"); // si inicia antes del periodo
                if(DateTime.Compare(Convert.ToDateTime(FechasSusplicencias.Tables[0].Rows[i][1]), fechaFinQuin) > 0)
                    FechasSusplicencias.Tables[0].Rows[i][1] = Convert.ToDateTime(fechaFinQuin).ToString("yyyy-MM-dd"); // si finaliza despues del periodo
                TimeSpan ts = Convert.ToDateTime(FechasSusplicencias.Tables[0].Rows[i][1]) - Convert.ToDateTime(FechasSusplicencias.Tables[0].Rows[i][0]);
                int differenceInDays = ts.Days + 1;  // los dias de las licencias o incapacidades son Incluidos los dos topes
                if (FechasSusplicencias.Tables[0].Rows[i][4].ToString() != "0")
                    valormsusplicencias = valordiatrabajo * differenceInDays;
                DiasSusLicencias = DiasSusLicencias + differenceInDays;

                //   AQUI SE INSERTA LA INCAPACIDAD EN LA GRILLA DE LA LIQUIDACION

                if (FechasSusplicencias.Tables[0].Rows[i][5].ToString() == "H" && (FechasSusplicencias.Tables[0].Rows[i][4].ToString() == "1" || FechasSusplicencias.Tables[0].Rows[i][4].ToString() == "4"))
                {
                    this.ingresar_datos_datatable(FechasSusplicencias.Tables[0].Rows[i][2].ToString(), Convert.ToDouble(differenceInDays.ToString()), double.Parse(valordiatrabajo.ToString("N")), 0, Math.Round(valormsusplicencias, 0), FechasSusplicencias.Tables[0].Rows[i][6].ToString(), 0, docref, FechasSusplicencias.Tables[0].Rows[i][3].ToString());
                    sumadescontado += valormsusplicencias;
                }
                else if (FechasSusplicencias.Tables[0].Rows[i][5].ToString() == "D" && (FechasSusplicencias.Tables[0].Rows[i][4].ToString() == "2" || FechasSusplicencias.Tables[0].Rows[i][4].ToString() == "3"))
                {
                    this.ingresar_datos_datatable(FechasSusplicencias.Tables[0].Rows[i][2].ToString(), Convert.ToDouble(differenceInDays.ToString()), double.Parse(valordiatrabajo.ToString("N")), Math.Round(valormsusplicencias, 0), 0, FechasSusplicencias.Tables[0].Rows[i][6].ToString(), 0, docref, FechasSusplicencias.Tables[0].Rows[i][3].ToString());
                    sumapagado += valormsusplicencias;
                }
                else
                {
                    if (FechasSusplicencias.Tables[0].Rows[i][4].ToString() != "0") // VACACIONES YA DISFRUTADAS EN EL PERIODO
                    { 
                        this.ingresar_datos_datatable("Error de incongruencia en SuspLic, Corrija", 0, 0, 0, 0, "0", 0, docref, FechasSusplicencias.Tables[0].Rows[i][8].ToString());
                        errores += 1;
                    }
                }
	        }
            string vacacionesPagadas = DBFunctions.SingleData(@"Select coalesce(sum(dqui_canteventos),0) from DBXSCHEMA.MQUINCENAS M, DQUINCENA D, CNOMINA CN
                                    where memp_codiempl = '"+ codigoempleado + "' and M.MQUI_ANOQUIN = CN.CNOM_ANO and m.mqui_mesquin = cn.cnom_mes and d.mqui_codiquin = m.mqui_codiquin and (d.pcon_concepto = cnom_concsalacodi or d.pcon_concepto = cnom_concvacacodi); ");
            if (vacacionesPagadas == "")
                vacacionesPagadas = "0";
           diasTrabajados = diasTrabajados - DiasSusLicencias - Convert.ToUInt16(vacacionesPagadas.ToString());
            diasexceptosauxtransp = diasexceptosauxtransp + DiasSusLicencias;
            if (diasTrabajados < 0)
                diasTrabajados = 0;

            if (periodopago == "3")  //  EMPLEADO de JORNAL 
                diasTrabajados = 0;  

		    //calula los dias que ha estado en vacaciones
            DiasEnVacaciones = verDiasEnVacaciones(codigoempleado, fechaFinVacaciones);
            
	      //  diasTrabajados = diasTrabajados - DiasEnVacaciones; LOS DIAS TRABAJANDS YA VIENEN NETOS
            if (diasTrabajados < 0) diasTrabajados = 0;

		    double sueldodias=valordiatrabajo*diasTrabajados;
    //h		double diapromvacaciones,sueldovacaciones;
		    DataSet valorsueldopromedio= new DataSet();
		    //sacar el sueldo de vacaciones del promedio de sueldos
		    DateTime fechares= new DateTime();
		    //JFSC 22042008 se validan los meses dependiendo del tipo de salario
		    //si es variable debe ser el último año, sino, los últimos 3 meses
		    if(tipoSalario=="2")
		      {
			    fechares=fechaIniQuin.AddMonths(-12);
		      }
		    else
		       {
			    fechares=fechaIniQuin.AddMonths(-3);
		       }

		    string fechares3= fechares.Date.ToString("yyyy-MM-dd");

            string query = @"SELECT * FROM (SELECT sum(D.DQUI_APAGAR - D.DQUI_ADESCONTAR),D.PCON_CONCEPTO,sum(dqui_canteventos),
	   	 	  						    D.MEMP_CODIEMPL, pcon_desccant,
									    M.MQUI_ANOQUIN,
									    m.MQUI_MESQUIN,
									    CASE WHEN M.MQUI_MESQUIN<=9 
					  					    THEN CAST((CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-0' 
						   							    CONCAT RTRIM(CAST(MQUI_MESQUIN AS CHAR(2))) 
													    CONCAT '-' CONCAT CASE WHEN M.MQUI_TPERNOMI = 1 THEN '15' 
														    WHEN M.MQUI_TPERNOMI = 2 THEN '16' END) AS DATE) 
										    ELSE CAST((CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-' 
						   							    CONCAT RTRIM(CAST(MQUI_MESQUIN AS CHAR(2))) 
													    CONCAT '-' CONCAT CASE WHEN M.MQUI_TPERNOMI = 1 THEN '15' 
														    WHEN M.MQUI_TPERNOMI = 2 THEN '16' END) AS DATE) END 
										    AS FECHA 
									    FROM dbxschema.DQUINCENA D,
						   				     dbxschema.MQUINCENAS M,
										     dbxschema.pconceptonomina PCON, dbxschema.CNOMINA CN  
									   WHERE D.MQUI_CODIQUIN=M.MQUI_CODIQUIN 
										 AND D.pcon_concepto = PCON.pcon_concepto 
										 AND PCON.tres_afecvacacion = 'S' 
                                         AND D.PCON_CONCEPTO <> CN.CNOM_CONCSALACODI
										 AND D.memp_codiempl = '" + codigoempleado+ @"' AND MQUI_MESQUIN>0 
									   group by D.MEMP_CODIEMPL,D.PCON_CONCEPTO,pcon_desccant,M.MQUI_ANOQUIN,m.MQUI_MESQUIN,M.MQUI_TPERNOMI
											    ) as sources 
				    WHERE sources.fecha >= '" + fechares3+@"' 
					    AND sources.fecha <= '"+lb.Text+@"'";

            //     and D.pcon_concepto <> cN.CNOM_concsalaCODI   tenia para descarta el concepto SALARIO, pero lo debe incluir

		    //DBFunctions.Request(valorsueldopromedio,IncludeSchema.NO,"SELECT SUM(D.DQUI_APAGAR),D.MEMP_CODIEMPL,M.MQUI_ANOQUIN,m.MQUI_MESQUIN,CASE WHEN M.MQUI_MESQUIN<9 THEN CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-0' CONCAT CAST(MQUI_MESQUIN AS CHAR(2)) CONCAT '-' CONCAT '15' ELSE CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-' CONCAT CAST(MQUI_MESQUIN AS CHAR(2)) CONCAT '-' CONCAT '15' END AS FECHA FROM DBXSCHEMA.DQUINCENA D,DBXSCHEMA.MQUINCENAS M,dbxschema.pconceptonomina PCON WHERE D.MQUI_CODIQUIN=M.MQUI_CODIQUIN AND D.pcon_concepto=PCON.pcon_concepto  AND PCON.tres_afecvacacion='S'  AND D.memp_codiempl='"+codigoempleado+"' AND CASE WHEN M.MQUI_MESQUIN<9 THEN  CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-0' CONCAT CAST(MQUI_MESQUIN AS CHAR(2)) CONCAT '-' CONCAT '15' ELSE CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-' CONCAT CAST(MQUI_MESQUIN AS CHAR(2)) CONCAT '-' CONCAT '15'  END BETWEEN '"+fechares3+"' AND '"+lb.Text+"'  GROUP BY  D.MEMP_CODIEMPL,M.MQUI_ANOQUIN,m.MQUI_MESQUIN ");
		    DBFunctions.Request(valorsueldopromedio,IncludeSchema.NO,query);
			
		    DateTime mesante= new DateTime();
		    mesante=fechaIniQuin.AddMonths(-1);
   
			
	    //	if(valorsueldopromedio.Tables[0].Rows.Count!=0)
		      {				
		       if (tipoSalario=="1")
		          {
                      int numquincenaanterior = 0;
                      try
                      {
                          Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(MQUI.mqui_codiquin,0) from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin=" + mesante.Year.ToString() + " and MQUI.mqui_mesquin=" + mesante.Month.ToString() + " and MQUI.mqui_tpernomi=2 "));
                      }
                      catch
                      {
                          numquincenaanterior = 0;
                      }

                       string basemesA=DBFunctions.SingleData("select MEMP_SUELACTU from DBXSCHEMA.MEMPLEADO WHERE MEMP_CODIEMPL='"+codigoempleado+"'");//CAMBIO DPS
			       diapromvacaciones=Convert.ToDouble(basemesA)/30;//
			       sueldovacaciones=diapromvacaciones*diasvacacionesapagar;
			      }
		       else
			      if (tipoSalario=="3")
				    {
					    sueldovacaciones=valordiatrabajo*diasvacacionesapagar;
					    diapromvacaciones=valordiatrabajo;
				    }
	 		      else
			       {
			        int CantidadEventosDias = 0;
				    for (i=0;i<valorsueldopromedio.Tables[0].Rows.Count;i++)
				    {
			   		    //this.ingresardatos_cesantias(double.Parse(valorsueldopromedio.Tables[0].Rows[i][0].ToString()),valorsueldopromedio.Tables[0].Rows[i][2].ToString(),int.Parse(valorsueldopromedio.Tables[0].Rows[i][1].ToString()));
			  		    sumasueldopromedio+= double.Parse(valorsueldopromedio.Tables[0].Rows[i][0].ToString());
					    string TipoEvento=valorsueldopromedio.Tables[0].Rows[i][3].ToString();
					    if (TipoEvento == "4")
						    CantidadEventosDias += int.Parse(valorsueldopromedio.Tables[0].Rows[i][1].ToString());
				    }
					    //diapromvacaciones=sumasueldopromedio/(totaldiasCalculoPromedio-diasvacacionesapagar);
		                //diapromvacaciones=sumasueldopromedio/CantidadEventosDias;  // debe dividir sobre los dias trabajados.
                    diapromvacaciones = (sumasueldopromedio / totaldiasCalculoPromedio); //+ valordiatrabajo; ? suman un dia de salario 
					    //diapromvacaciones=Double.Parse(sueldomes)/30;
                    // EL valor del sueldo se calcula por el sueldo diario actual por los dias en vacaciones mas el promedior diario de comisiones por los dias en vacaciones
                    sueldovacaciones = Math.Round((diapromvacaciones + valordiatrabajo) * diasvacacionesapagar,0);
					    //JFSC 23042008 se deben tener en cuenta las vacaciones para eps-pensión
					    //acumuladoeps+=sueldovacaciones;
			       }
			    }
			

		    //llenar la informacion de los labels.
		    LBVACAAPAGAR.Text=sueldovacaciones.ToString("C");
		    LBDTVACACIONES.Text=diasvacacionesapagar.ToString();
		
		    LBPERIODO.Text=((DropDownList)DATAGRIDPERESCOGIDO.Items[0].Cells[7].FindControl("DDLANOINI")).SelectedValue+"/"+((DropDownList)DATAGRIDPERESCOGIDO.Items[0].Cells[7].FindControl("DDLMESINI")).SelectedValue+"/"+((DropDownList)DATAGRIDPERESCOGIDO.Items[0].Cells[7].FindControl("DDLDIAINI")).SelectedValue+"-"+((DropDownList)DATAGRIDPERESCOGIDO.Items[DATAGRIDPERESCOGIDO.Items.Count-1].Cells[8].FindControl("DDLANOFIN")).SelectedValue+"/"+((DropDownList)DATAGRIDPERESCOGIDO.Items[DATAGRIDPERESCOGIDO.Items.Count-1].Cells[8].FindControl("DDLMESFIN")).SelectedValue+"/"+((DropDownList)DATAGRIDPERESCOGIDO.Items[DATAGRIDPERESCOGIDO.Items.Count-1].Cells[8].FindControl("DDLDIAFIN")).SelectedValue;
			    //LBPERIODO.Text=DDLANOINI.SelectedValue.ToString()+"/"+DDLMESINI.SelectedValue.ToString()+"/"+DDLDIAINI.SelectedValue.ToString()+"-"+DDLANOFIN.SelectedValue+"/"+DDLMESFIN.SelectedValue+"/"+DDLDIAFIN.SelectedValue;
		    for (j=0;j<DATAGRIDPERESCOGIDO.Items.Count;j++)
			{
			     sumDiasE+=int.Parse(((DropDownList)DATAGRIDPERESCOGIDO.Items[j].Cells[10].FindControl("DDLDIASEFECTIVOS")).SelectedValue);
			}
			
		    LBDIASEFECTIVOS.Text=sumDiasE.ToString();
		
		    //ingresar a la grilla las vacaciones
		
		    if (diasvacacionesapagar>0)
		    {
			    this.ingresar_datos_datatable(concepto,diasvacacionesapagar,double.Parse(diapromvacaciones.ToString("n")),Math.Round(sueldovacaciones,0),0,"PESOS M/CTE",0,docref,descripcionconceptovacaciones);
			    sumapagado+=sueldovacaciones;
			    this.validacionafectaciones("0",codigoempleado,concepto,diasvacacionesapagar,double.Parse(diapromvacaciones.ToString("n")),Math.Round(sueldovacaciones,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][1].ToString(),"Salario Vacaciones credito que afecta EPS? posible error..",sueldovacaciones,0,0,0,0,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,concepto,diasvacacionesapagar,double.Parse(diapromvacaciones.ToString("n")),Math.Round(sueldovacaciones,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][2].ToString(),"Salario Vacaciones credito que afecta Horas extras? posible error..",0,0,0,0,0,0,0,sueldovacaciones);
			    this.validacionafectaciones("0",codigoempleado,concepto,diasvacacionesapagar,double.Parse(diapromvacaciones.ToString("n")),Math.Round(sueldovacaciones,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][3].ToString(),"Salario Vacaciones credito que afecta Primas? posible error..",0,0,sueldovacaciones,0,0,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,concepto,diasvacacionesapagar,double.Parse(diapromvacaciones.ToString("n")),Math.Round(sueldovacaciones,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][4].ToString(),"Salario Vacaciones credito que afecta Vacaciones? posible error..",0,0,0,sueldovacaciones,0,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,concepto,diasvacacionesapagar,double.Parse(diapromvacaciones.ToString("n")),Math.Round(sueldovacaciones,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][5].ToString(),"Salario Vacaciones credito que afecta Cesantias? posible error..",0,sueldovacaciones,0,0,0,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,concepto,diasvacacionesapagar,double.Parse(diapromvacaciones.ToString("n")),Math.Round(sueldovacaciones,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][6].ToString(),"Salario Vacaciones credito que afecta Retencion en la fuente? posible error..",0,0,0,0,sueldovacaciones,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,concepto,diasvacacionesapagar,double.Parse(diapromvacaciones.ToString("n")),Math.Round(sueldovacaciones,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][7].ToString(),"Salario Vacaciones credito que afecta Provisiones? posible error..",0,0,0,0,0,sueldovacaciones,0,0);
			    this.validacionafectaciones("0",codigoempleado,concepto,diasvacacionesapagar,double.Parse(diapromvacaciones.ToString("n")),Math.Round(sueldovacaciones,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][8].ToString(),"Salario Vacaciones credito que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,sueldovacaciones,0);
		    }		
		
	
		    if (diasTrabajados>0)
		    {
			    DataSet afectacionessueldo2= new DataSet();
			    DBFunctions.Request(afectacionessueldo2,IncludeSchema.NO,"select P.pcon_signoliq,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from dbxschema.cnomina C,dbxschema.pconceptonomina P where C.cnom_concsalacodi=P.pcon_concepto");
			
		        this.ingresar_datos_datatable(cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString(),diasTrabajados,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,descripcionconcepto);
                sumapagado +=sueldodias;
			    this.validacionafectaciones("0",codigoempleado,cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString(),diasTrabajados,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo2.Tables[0].Rows[0][0].ToString(),afectacionessueldo2.Tables[0].Rows[0][1].ToString(),"Salario credito que afecta EPS? posible error..",sueldodias,0,0,0,0,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString(),diasTrabajados,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo2.Tables[0].Rows[0][0].ToString(),afectacionessueldo2.Tables[0].Rows[0][2].ToString(),"Salario credito que afecta Horas extras? posible error..",0,0,0,0,0,0,0,sueldodias);
			    this.validacionafectaciones("0",codigoempleado,cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString(),diasTrabajados,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo2.Tables[0].Rows[0][0].ToString(),afectacionessueldo2.Tables[0].Rows[0][3].ToString(),"Salario credito que afecta Primas? posible error..",0,0,sueldodias,0,0,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString(),diasTrabajados,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo2.Tables[0].Rows[0][0].ToString(),afectacionessueldo2.Tables[0].Rows[0][4].ToString(),"Salario credito que afecta Vacaciones? posible error..",0,0,0,sueldodias,0,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString(),diasTrabajados,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo2.Tables[0].Rows[0][0].ToString(),afectacionessueldo2.Tables[0].Rows[0][5].ToString(),"Salario credito que afecta Cesantias? posible error..",0,sueldodias,0,0,0,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString(),diasTrabajados,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo2.Tables[0].Rows[0][0].ToString(),afectacionessueldo2.Tables[0].Rows[0][6].ToString(),"Salario credito que afecta Retencion en la fuente? posible error..",0,0,0,0,sueldodias,0,0,0);
			    this.validacionafectaciones("0",codigoempleado,cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString(),diasTrabajados,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo2.Tables[0].Rows[0][0].ToString(),afectacionessueldo2.Tables[0].Rows[0][7].ToString(),"Salario credito que afecta Provisiones? posible error..",0,0,0,0,0,sueldodias,0,0);
			    this.validacionafectaciones("0",codigoempleado,cnomina2.Tables[0].Rows[0]["cnom_concsalacodi"].ToString(),diasTrabajados,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,"","","","",afectacionessueldo2.Tables[0].Rows[0][0].ToString(),afectacionessueldo2.Tables[0].Rows[0][8].ToString(),"Salario credito que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,sueldodias,0);
			
		    }

            //Las fechas estan armadas asi que llamo las funciones para calcular todas las novedades en el periodo de tiempo.
            lb.Text = fechaIniVaca.ToString();
            lb2.Text = fechaFinVaca.ToString();

            string periodoNomina = cnomina2.Tables[0].Rows[0]["cnom_quincena"].ToString();	
		    this.actualiza_novedades(periodoNomina, empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref,lbmas1.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString());
		    this.actualiza_registro_msusplicencias(empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref,lbmas1.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString());
		    this.actuliza_registro_mpagosydtosper(empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref, fechaFinVaca.ToString(), empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString());
		    this.actuliza_prestamoempledos(empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref,lbmas1.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString());
		    this.liquidar_epsfondo(empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref,lbmas1.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString(),empleados.Tables[0].Rows[0][9].ToString());
		    this.liquidar_fondosolidaridadpensional(empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref,lbmas1.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString(),empleados.Tables[0].Rows[0][9].ToString(),double.Parse(sueldomes));
		    this.ingresovacacionesgrilla(empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref,lbmas1.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString(),empleados.Tables[0].Rows[0][9].ToString());
            lb.Text = fechaIniVaca.ToString();
            lb2.Text = fechaFinVaca.ToString();
            this.liquidar_subsidiodetransporte(empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref,lbmas1.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString(),diasexceptosauxtransp.ToString(),empleados.Tables[0].Rows[0][8].ToString(),empleados.Tables[0].Rows[0][9].ToString());
		    this.insertarretefuente(empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref,lbmas1.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString(),acumuladoeps,empleados.Tables[0].Rows[0][9].ToString(),diasvacacionesapagar);
		    this.liquidar_apropiaciones(empleados.Tables[0].Rows[0][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[0][5]),lbdocref,lbmas1.Text,empleados.Tables[0].Rows[0][2].ToString(),empleados.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[0][1].ToString(),empleados.Tables[0].Rows[0][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString(),empleados.Tables[0].Rows[0][9].ToString());
		
            this.insertarsaldos(empleados.Tables[0].Rows[0][0].ToString(),sumapagado,sumadescontado,acumuladoeps,mquincenas.Tables[0].Rows[0][0].ToString(),diasexceptosauxtransp,sumapagadamasauxt);

		    Session["codigoempleado"]=empleados.Tables[0].Rows[0][0].ToString();
		    Session["DataTable1"]=DataTable1;
		
	   }

        protected int validarAnoBisiesto(int ano)
        {
            int anoBisiento = ano;
            Double anoBis4 = anoBisiento;
            anoBis4 = Math.Round(anoBis4 / 4, 0);
            anoBisiento = Convert.ToInt16(anoBis4 * 4);
            int diaFinal = 28;
            if (anoBisiento == ano)
                diaFinal = 29; // anoBisiesto tiene 29 dias.
            return diaFinal;
        }

        protected int verDiasEnVacaciones(string codigoempleado, DateTime lb2)
        {
            int DiasEnVacaciones = 0;
            int i;
            DataSet DSFechasVacaciones = new DataSet();
            DateTime FechaInicio = new DateTime();
            DateTime FechaFinal  = new DateTime();
            DateTime FechaInicial = new DateTime();
            int DiasFechaInicio  = 0;
            int DiasFechaFinal   = 0;
            int MesFinal, AnoFinal, DiaFinal, MesInicio, AnoInicio, DiaInicio;
            string fechaIniMes = fechaIniQuin.Year.ToString() + '-' + fechaIniQuin.Month.ToString() + '-' + "01";
            int codiQuin = 1;
            if (fechaIniQuin.Day.ToString() == "16")
                codiQuin = 2;

            DBFunctions.Request(DSFechasVacaciones,IncludeSchema.NO,@"Select DVAC_FECHINIC as FECHA_INICIO,DVAC_FECHFINAL AS FECHA_FINAL from DBXSCHEMA.MVACACIONES MV,DBXSCHEMA.DVACACIONES DV 
		                                                              where MV.MVAC_SECUENCIA = DV.MVAC_SECUENCIA AND memp_codiemp = '"+codigoempleado+"'  AND DVAC_FECHFINAL >= '"+fechaIniMes+"';");
		    for (i=0;i< DSFechasVacaciones.Tables[0].Rows.Count;i++)
		    {
		        FechaInicio=Convert.ToDateTime(DSFechasVacaciones.Tables[0].Rows[0]["FECHA_INICIO"]);
                FechaInicial = Convert.ToDateTime(fechaIniMes);
			    if (FechaInicio < FechaInicial)
			    {
				    FechaInicio = FechaInicial;
			    }
			    MesInicio  = int.Parse(FechaInicio.Month.ToString());
			    DiaInicio  = int.Parse(FechaInicio.Day.ToString());
			    AnoInicio  = FechaInicio.Year;
			    DiasFechaInicio = (AnoInicio*360)+(MesInicio*30)+DiaInicio;

			    FechaFinal = Convert.ToDateTime(DSFechasVacaciones.Tables[0].Rows[0]["FECHA_FINAL"]);
                DateTime FechaCorte = Convert.ToDateTime(lb2);
			    if (FechaFinal > FechaCorte) 
			    {
                    FechaFinal = FechaCorte;
			    }
			    MesFinal   = int.Parse(FechaFinal.Month.ToString());
			    DiaFinal   = int.Parse(FechaFinal.Day.ToString());
			    AnoFinal   = FechaFinal.Year;
			    DiasFechaFinal = (AnoFinal*360)+(MesFinal*30)+DiaFinal;
			    DiasEnVacaciones += DiasEnVacaciones+(DiasFechaFinal-DiasFechaInicio) + 1;
            }
        //  ahora buscamos los dias de salario ya pagado en la quincena
        int diasSalarioYaPagados = Convert.ToInt32(DBFunctions.SingleData(@"Select coalesce(sum(dqui_canteventos),0)
                                                                            from  DBXSCHEMA.dquincena D, mquincenas m, cnomina c 
                                                                            where M.Mqui_codiquin = D.Mqui_codiquin AND d.memp_codiempl = '"+codigoempleado+@"'  
                                                                            and mqui_anoquin = "+fechaIniQuin.Year.ToString()+@" 
                                                                            and mqui_mesquin = "+fechaIniQuin.Month.ToString()+@" 
                                                                            and mqui_tpernomi = " + codiQuin + @"
                                                                            and  c.cnom_concsalacodi = d.pcon_concepto;"));
        DiasEnVacaciones += diasSalarioYaPagados; 
       
        return DiasEnVacaciones; 
        }

        protected void realizar_liquidacion1(Object Sender, EventArgs e)
        {
            formaPagoVac = "V";  // Paga las vacaciones con Cheque
            realizar_liquidacion(null, null);
        }

        protected void realizar_liquidacion2(Object Sender, EventArgs e)
        {
            formaPagoVac = "N";  // Paga las vacaciones en la Planilla del Banco
            realizar_liquidacion(null, null);
        }

        protected void realizar_liquidacion(Object  Sender, EventArgs e)
		{
			DataTable1  = new DataTable();
			DataTable2  = new DataTable();
			DataTable1  = (DataTable)Session["DataTable1"];
			DataTable2  = (DataTable)Session["DataTable2"];
			Adisponibles= (ArrayList)Session["Adisponibles"];
			ArrayList secuencias = new ArrayList();	
			ArrayList FechaVacIni = new ArrayList();	
			ArrayList FechaVacFin = new ArrayList();	
			ArrayList Adiasvacaciones = new ArrayList();
			ArrayList Adiasvacas2 = new ArrayList();	
			Adiasvacaciones= (ArrayList)Session["Adiasvacaciones"];	
			Adiasvacas2 = (ArrayList)Session["Adiasvacas2"];
			secuencias  = (ArrayList)Session["secuencias"];
			ArrayList sql = new ArrayList();
			FechaVacIni = (ArrayList)Session["FechaVacIni"];
			FechaVacFin = (ArrayList)Session["FechaVacFin"];
			string continuidad;
			string codigoempleado= (string)Session["codigoempleado"];
            string codquincena = (string)Session["codquincena"];
			Response.Write("<script language:java>alert('codigo empleado "+codigoempleado+" ');</script>");
			int i,desccantidad=0,p;
			DataSet cnomina= new DataSet();
			
			DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");	
			string lbdocref=cnomina.Tables[0].Rows[0][0]+"-"+cnomina.Tables[0].Rows[0][1]+"-"+cnomina.Tables[0].Rows[0][2];
			
			//recorro el datatable 
			for (i=0;i<DataTable1.Rows.Count;i++)
			{
				//hago una busqueda en el array, si no esta lo ingreso a la lista
				//si esta es porque ya fue liquidado.				
				//valido que el codigo sea de un empleado
				if (DataTable1.Rows[i][0].ToString()!="--")
				{										
					//actualizo el maestro de vacaciones							
					DataSet fechaingreso= new DataSet();
					DBFunctions.Request(fechaingreso,IncludeSchema.NO,"select memp_fechingreso from dbxschema.mempleado where MEMP_CODIEMPL='"+codigoempleado+"' ");	
											
					//segun la desc avergiaru el numero.
					if (DataTable1.Rows[i][5].ToString()=="DIAS")
						    desccantidad=1;
					if (DataTable1.Rows[i][5].ToString()=="HORAS")
						     desccantidad=2;    
					if (DataTable1.Rows[i][5].ToString()=="MINUTOS")
						     desccantidad=3;
					if (DataTable1.Rows[i][5].ToString()=="PESOS M/CTE")
					         desccantidad=4;
						
					//inserto en la tabla de detalles de quincena.
					sql.Add("insert into dquincena values(DEFAULT,"+codquincena+",'"+codigoempleado+"','"+DataTable1.Rows[i][0].ToString()+"',"+DataTable1.Rows[i][1].ToString()+","+DataTable1.Rows[i][2].ToString()+","+DataTable1.Rows[i][3].ToString()+","+DataTable1.Rows[i][4].ToString()+","+desccantidad+" ,"+DataTable1.Rows[i][6].ToString()+",'"+DataTable1.Rows[i][7].ToString()+"','"+formaPagoVac+"')");
					//DBFunctions.NonQuery("insert into dquincena values("+codquincena+",'"+codigoempleado+"','"+DataTable1.Rows[i][0].ToString()+"',"+DataTable1.Rows[i][1].ToString()+","+DataTable1.Rows[i][2].ToString()+","+DataTable1.Rows[i][3].ToString()+","+DataTable1.Rows[i][4].ToString()+","+desccantidad+" ,"+DataTable1.Rows[i][6].ToString()+",'"+DataTable1.Rows[i][7].ToString()+"')");
										
					//valido si el concepto es un prestamo.
					if (DataTable1.Rows[i][7].ToString()!= lbdocref && DataTable1.Rows[i][7].ToString()!="--")
					{
						//actualizar las cuotas pagadas de mprestamo
						sql.Add("update mprestamoempleados set mpre_cuotpaga =mpre_cuotpaga+1 ,mpre_valopaga=mpre_valopaga+"+DataTable1.Rows[i][3].ToString()+" where mpre_numelibr="+DataTable1.Rows[i][6].ToString()+" ");
						//DBFunctions.NonQuery("update mprestamoempleados set mpre_cuotpaga =mpre_cuotpaga+1 ,mpre_valopaga=mpre_valopaga+"+DataTable1.Rows[i][3].ToString()+" where mpre_numelibr="+DataTable1.Rows[i][6].ToString()+" ");
					}					
				}							
			}
					//this.PagoLiquidacionVacaciones(DataTable2.Rows[0][0].ToString(),DDLDIASEFECTIVOS.SelectedValue.ToString(),DataTable2.Rows[0][5].ToString());
						
						//actualizar detalle de vacaciones
						//validar si las toma en tiempo o en dinero
						
			for (p=0;p<DATAGRIDPERESCOGIDO.Items.Count;p++)
			{
				if (DATAGRIDPERESCOGIDO.Items.Count>1)
					continuidad="C";
				else
					continuidad="D";
				if ( ((DropDownList)DATAGRIDPERESCOGIDO.Items[p].Cells[7].FindControl("DDLFORMA")).SelectedValue=="EN TIEMPO")
				{
					Response.Write("<script language:java>alert('Se ingreso un periodo de vacaciones por "+diasvacas+" dias,pagados en tiempo.');</script> ");
					//DBFunctions.NonQuery("insert into dbxschema.dvacaciones values ('"+DATAGRIDPERESCOGIDO.Items[p].Cells[3].Text+"','"+DATAGRIDPERESCOGIDO.Items[p].Cells[4].Text+"',"+Adisponibles[p].ToString()+","+secuencias[p].ToString()+","+Adiasvacaciones[p].ToString()+",0 )");
					sql.Add("insert into dbxschema.dvacaciones values ('"+FechaVacIni[p]+"','"+FechaVacFin[p]+"',"+Adisponibles[p].ToString()+","+secuencias[p].ToString()+","+Adiasvacaciones[p].ToString()+",0,'"+continuidad+"' )");
					//DBFunctions.NonQuery("insert into dbxschema.dvacaciones values ('"+FechaVacIni[p]+"','"+FechaVacFin[p]+"',"+Adisponibles[p].ToString()+","+secuencias[p].ToString()+","+Adiasvacaciones[p].ToString()+",0 )");		
				}
						
				if (((DropDownList)DATAGRIDPERESCOGIDO.Items[p].Cells[9].FindControl("DDLFORMA")).SelectedValue=="EN DINERO")
				{
					//DBFunctions.NonQuery("insert into dbxschema.dvacaciones values ('"+DATAGRIDPERESCOGIDO.Items[p].Cells[3].Text+"','"+DATAGRIDPERESCOGIDO.Items[p].Cells[4].Text+"',"+Adisponibles[p].ToString()+","+secuencias[p].ToString()+",0,"+Adiasvacaciones[p].ToString()+" )");
					sql.Add("insert into dbxschema.dvacaciones values ('"+FechaVacIni[p]+"','"+FechaVacFin[p]+"',"+Adisponibles[p].ToString()+","+secuencias[p].ToString()+",0,"+Adiasvacaciones[p].ToString()+",'"+continuidad+"' )");
					//DBFunctions.NonQuery("insert into dbxschema.dvacaciones values ('"+FechaVacIni[p]+"','"+FechaVacFin[p]+"',"+Adisponibles[p].ToString()+","+secuencias[p].ToString()+",0,"+Adiasvacaciones[p].ToString()+" )");
					Response.Write("<script language:java>alert('Se ingreso un periodo de vacaciones por "+diasvacas+" dias,pagados en dinero.');</script> ");
				}
						
				//actualizar maestro de vacaciones
				sql.Add("update mvacaciones set mvac_diasvacadisf =mvac_diasvacadisf+"+Adiasvacas2[p].ToString()+" where mvac_secuencia="+secuencias[p].ToString()+" ");
				//DBFunctions.NonQuery("update mvacaciones set mvac_diasvacadisf =mvac_diasvacadisf+"+Adiasvacas2[p].ToString()+" where mvac_secuencia="+secuencias[p].ToString()+" ");
					
				//Response.Redirect(mainpage+"?process=Nomina.FormatoComprobantePagoVacaciones");
				//Response.Redirect(mainpage+"?process=Nomina.FormatoComprobantePagoVacaciones&DIAS="+LBDTVACACIONES.Text+"&VALORVACA="+LBVACAAPAGAR.Text+"&FECHA="+LBPERIODO.Text.Replace('/','*')+"&DIASEFECTIVOS="+LBDIASEFECTIVOS.Text+" ");
			
			}
			//el estado del empleado pasa a ser el 5 correspndiente a vacaciones.						
			//DBFunctions.NonQuery("update dbxschema.mempleado set test_estado='5' where memp_codiempl='"+codigoempleado+"'");
			if (((DropDownList)DATAGRIDPERESCOGIDO.Items[0].Cells[9].FindControl("DDLFORMA")).SelectedValue !="EN DINERO")
			{
				sql.Add("update dbxschema.mempleado set test_estado='5' where memp_codiempl='"+codigoempleado+"'");
			}

			if (DBFunctions.Transaction(sql))
			{
				Response.Redirect("" + mainpage + "?process=Nomina.LiquidacionVacaciones&el=1");
			}
			else
				lb.Text="Operación de Liquidacion Errada"+DBFunctions.exceptions;				
		}
		
		
		protected void insertarsaldos(string  codigoempleado, double sumapagado,double sumadescontado,double acumuladoeps,string codquincena,double diasexceptosauxtransp,double sumapagadamasauxt)
		{
			//double saldo;
			saldo = sumapagado - sumadescontado;
			this.ingresar_datos_datatable("--",0,0,Math.Round(sumapagado,0),Math.Round(sumadescontado,0),"SALDO",Math.Round(saldo,0),"--","");
			DBFunctions.NonQuery("insert into dpagaafeceps values ( default,"+codquincena+",'"+codigoempleado+"',"+acumuladoretefuente+","+acumuladoprima+","+acumuladovacaciones+","+acumuladoprovisiones+","+acumuladoliqdefinitiva+","+acumuladohorasextras+","+acumuladocesantia+","+acumuladoeps+","+diasexceptosauxtransp+")");
		}		
		
		
		protected void liquidar_apropiaciones(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string tiposalario)
		{
			string tcontrato=DBFunctions.SingleData("select  tcon_contrato from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"' ");
			
			double aportesena=0;
			string porcEmpresa=DBFunctions.SingleData("select papo_porcapor from dbxschema.paportepatronal where papo_tipoaporte=6");
			string porcEmpleado=DBFunctions.SingleData("select cnom_porcepsempl from dbxschema.cnomina");
			aportesena=double.Parse(porcEmpresa)+double.Parse(porcEmpleado);
			
			int i;	
			double  apropiacionARP,apropiacionICBF,apropiacionSENA,apropiacionCAJACOMPENSACION,apropiacionFONDOPENSIONES,apropiacionEPS=0;
			DataSet mquincenas= new DataSet();
			DataSet cnomina= new DataSet();
			DataSet paportepatronal= new DataSet();
			DBFunctions.Request(paportepatronal,IncludeSchema.NO,"select papo_codiapor,papo_nombapor,mnit_nit,papo_porcapor,papo_tipoaporte from dbxschema.paportepatronal");
			//calculo del porcentaje ARP
			string porcentajeArp=DBFunctions.SingleData("select B.prie_porcentaje from dbxschema.mempleado A,dbxschema.priesgoprofesional B where A.prie_codiries=B.prie_codiries and a.memp_codiempl='"+codigoempleado+"' ");

			if (paportepatronal.Tables[0].Rows.Count==0)
			{
				Response.Write("<script language:javascript>alert('Porfavor Configure la tabla correspondiente a los Aportes Patronales');</script>");
			}
			else
			{
				DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_baseporcsalainteg from dbxschema.cnomina");
				DBFunctions.Request(mquincenas,IncludeSchema.NO,"select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+cnomina.Tables[0].Rows[0][0].ToString()+" and mqui_mesquin="+cnomina.Tables[0].Rows[0][1].ToString()+" and mqui_tpernomi="+cnomina.Tables[0].Rows[0][2].ToString()+" ");
				
				//DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
				
				string descripcionconcepto=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+paportepatronal.Tables[0].Rows[0][1].ToString()+"' ");
				for (i=0;i<paportepatronal.Tables[0].Rows.Count;i++)
				{
					
			//icbf
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="1")
					{
						if (tcontrato!="3")
						{
							if (tiposalario=="1")
							{
								apropiacionICBF=((acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][3].ToString())/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionICBF=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}


							DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionICBF+")");
							string uno="insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionICBF+")";
						}
											
					    //bandera+=1;
						//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionICBF,docref,apellido1,apellido2,nombre1,nombre2);
											
					}
					//SENA
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="2")
					{
						if (tcontrato!="3")
						{
							if (tiposalario=="1")
							{
								apropiacionSENA=((acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][3].ToString())/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionSENA=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}					
							//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionSENA,docref,apellido1,apellido2,nombre1,nombre2);	
													
							DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionSENA+")");
							string dos="insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionSENA+")";
						}									
					}
					//ARP
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="3")
					{					
						
						if (tiposalario=="1")
						{
							apropiacionARP=((acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][3].ToString())/100)*double.Parse(porcentajeArp))/100;
						}
						else
						{
							apropiacionARP=(acumuladoeps*double.Parse(porcentajeArp))/100;
						}
					
						//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionARP,docref,apellido1,apellido2,nombre1,nombre2);
						DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionARP+")");
						string tres="insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionARP+")";

												
					}
					//CAJA
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="4")
					{
						if (tcontrato!="3")
						{
							if (tiposalario=="1")
							{
								apropiacionCAJACOMPENSACION=((acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][3].ToString())/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionCAJACOMPENSACION=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
					
							//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionCAJACOMPENSACION,docref,apellido1,apellido2,nombre1,nombre2);	
							DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionCAJACOMPENSACION+")");
							string cuatro="insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionCAJACOMPENSACION+")";
						}						
					}

					//FONDO DE PENSIONES
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="5")
					{
						if (tcontrato!="3")
						{
							if (tiposalario=="1")
							{
								apropiacionFONDOPENSIONES=((acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][3].ToString())/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionFONDOPENSIONES=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
					
							//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionFONDOPENSIONES,docref,apellido1,apellido2,nombre1,nombre2);	
							DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionFONDOPENSIONES+")");
							string cinco="insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionFONDOPENSIONES+")";
						}					
						
					}
					//EPS

					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="6")
					{
						if (tcontrato=="3")
						{							
							apropiacionEPS=(acumuladoeps*aportesena)/100;
						}
						else
						{							
							if (tiposalario=="1")
							{											
								apropiacionEPS=((acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][3].ToString())/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionEPS=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
						}						
						
						//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionEPS,docref,apellido1,apellido2,nombre1,nombre2);		
						DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionEPS+")");
						string seis="insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionEPS+")";
					}							
				}
			}				
		}
	
		
		protected void insertarretefuente(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,double acumuladoeps,string tiposalario,int diasefec)
		{
            string mensaje = "";
            int quince = Convert.ToInt32(o_CNomina.CNOM_QUINCENA);
            string codigoconceptoRfte  = o_CNomina.CNOM_CONCRFTECODI;
            string descripcionconcepto = o_CNomina.CNOM_CONCRFTEDESC; 

            string unidUVT = o_CNomina.CNOM_UNIDVALOTRIB;      //Sacar la Unidad de Valor Tributario
            //                                                                   fech fech                                                                                                                  1 0 2   MENSUAL          
            //                                                                   ini  fin  Nro qna                                                                                                          period  QUINCENAL
            double valorReteFuente = o_Varios.calcular_ReteFuente(codigoempleado, lb, lb2, codquincena, sueldo, docref, lbmas1, apellido1, apellido2, nombre1, nombre2, acumuladoeps, tiposalario, mensaje, quince, lbtipopago, valorfondopensionempleado, fondosolidaridadpensional, valorepsempleado, valorEpsyadescontado, valorfondopensionvoluntaria, unidUVT, o_CNomina);

            if (mensaje != "")
            {
                Response.Write("<script language='javascript'>alert('" + mensaje + "');</script>");
            }

            if (valorReteFuente != 0)
            {
               this.ingresar_datos_datatable(codigoconceptoRfte, 1, valorReteFuente, 0, valorReteFuente, "PESOS M/CTE", 0, docref, descripcionconcepto);
                sumadescontado += valorReteFuente;
            }
	    }		
		
			
		protected void liquidar_subsidiodetransporte(string codigoempleado, string lb, string lb2, string codquincena, double sueldo, string docref, string lbmas1, string apellido1, string apellido2, string nombre1, string nombre2, string periodoeps, string diasexceptosauxtransp, string periodosubt, string tiposalario) 
		{
            string periodopago = DBFunctions.SingleData("Select memp_peripago from DBXSCHEMA.mempleado where MEMP_CODIEMPL='" + codigoempleado + "'");
            if (periodopago == "3") // || (periodopago == "2" && DateTime.Compare(Convert.ToDateTime(lb2.ToString()), Convert.ToDateTime(lbmas1.ToString())) < 0))  //  EMPLEADO de JORNAL o es pago mensual y las vacaciones ter minan 
                return; 

			DataSet afectaciones= new DataSet();					
			DBFunctions.Request(afectaciones,IncludeSchema.NO,"select M.cnom_concsubtcodi,P.pcon_signoliq,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from dbxschema.cnomina M,dbxschema.pconceptonomina P where M.cnom_concsubtcodi=P.pcon_concepto");			
			double auxiliotransporte=0;
			
			string fpagoEmpleado=DBFunctions.SingleData("select memp_peripago from dbxschema.mempleado A, dbxschema.tperipago B where A.memp_peripago=B.tper_peri and memp_codiempl='"+codigoempleado+"' ");
            string fpagoSTransporte = DBFunctions.SingleData("select tsub_codigo from dbxschema.mempleado A, dbxschema.tperipago B where A.memp_peripago=B.tper_peri and memp_codiempl='" + codigoempleado + "' ");
        
	        // mirar si el empleado viene de vacaciones del periodo anterior y descartar esos dias para el subsidio de transporte
            // o si tomo vacaciones en mismo periodo
            DateTime FechaIniVacaciones = Convert.ToDateTime(lb);
            DateTime FechaFinVacaciones = Convert.ToDateTime(lb2);
            DateTime FechaFinQuincena   = Convert.ToDateTime(lb2);
            DateTime lbmas11;
            try
            {
                lbmas11 = Convert.ToDateTime(lbmas1);
            }catch
            {
                lbmas1 = lbmas1.Replace("-31", "-30");
            }
            
            if (FechaFinQuincena.Date > Convert.ToDateTime(lbmas1))
                FechaFinQuincena = Convert.ToDateTime(lbmas1);
          
            int diaQui = 30;  // por defecto el fin de la quincena
            if (o_CNomina.CNOM_OPCIQUINOMENS == "1" && o_CNomina.CNOM_SUBSTRANPERINOMI == "1")  // si paga quincenal y esta en la primera quincena
                diaQui = 15;
   
            //diasexceptosauxtransp = Convert.ToString(Convert.ToInt32(diasexceptosauxtransp) + verDiasEnVacaciones(codigoempleado, FechaFinVacaciones));
            double diasMes = 30;
            if (FechaFinVacaciones.Month == 02)
                diasMes = 28;
            int anoVac = FechaIniVacaciones.Year;
            int mesVac = FechaIniVacaciones.Month;
            if (FechaFinVacaciones.Day < 30 && FechaFinQuincena.Month == FechaFinQuincena.Month && o_CNomina.CNOM_SUBSTRANPERINOMI == "2" && o_CNomina.CNOM_QUINCENA == "1")
                return;  // las vacaciones se terminan antes del fin de mes y el subsidio de transporte se paga en la segunda quincena y el periodo actual es la primero qna
           
            int transportePagado = Convert.ToInt16(DBFunctions.SingleData("select coalesce(sum(d.dqui_canteventos),0) from dquincena d, mquincenas m, cnomina c where d.mqui_codiquin = m.mqui_codiquin and m.mqui_anoquin = " + anoVac + " and m.mqui_mesquin = " + mesVac + " and (d.pcon_concepto = c.cnom_concSUBTcodi or d.pcon_concepto = c.cnom_concVACAcodi) and d.memp_codiempl = '" + codigoempleado + "';"));
            double baseSubsidioTranporte = (acumuladoeps - valorvacaciones); // *diasMes / (diasMes - Convert.ToInt16(diasexceptosauxtransp));
            if (baseSubsidioTranporte < sueldo)
                baseSubsidioTranporte = sueldo;

            if (tiposalario == "1" || fpagoSTransporte == "3")
            {
                sumapagadamasauxt = sumapagado;
            }
            else
            {
                DataSet mquincenas = new DataSet();
                DBFunctions.Request(mquincenas, IncludeSchema.NO, "select max(mqui_codiquin) from mquincenas");
                DataSet cnomina = new DataSet();
                DataSet cnomina2 = new DataSet();
                DataSet valorepsprimeraquinanterior = new DataSet();
                DataSet valorepssegundaquinanterior = new DataSet();
                DBFunctions.Request(cnomina, IncludeSchema.NO, "select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,case when CNOM_OPCIQUINOMENS = 1 then truncate(decimal(cnom_substransactu/1),4) else truncate(decimal(cnom_substransactu/2),4) end   as subtransactu2,cnom_topepagoauxitran,cnom_salaminiactu from dbxschema.cnomina ");
                //   DBFunctions.Request(cnomina,  IncludeSchema.NO, "select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4)   as subtransactu2,cnom_topepagoauxitran,cnom_salaminiactu from dbxschema.cnomina WHERE CNOM_OPCIQUINOMENS = 2");
                DBFunctions.Request(cnomina2, IncludeSchema.NO, "select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4)   as subtransactu2,cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");
                string descripcionconcepto = DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='" + cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString() + "' ");

                //double valortope=0;
                double valortope = double.Parse(cnomina.Tables[0].Rows[0]["cnom_topepagoauxitran"].ToString()) * double.Parse(cnomina.Tables[0].Rows[0]["cnom_salaminiactu"].ToString());
                double valordiatransporte = (double.Parse(cnomina.Tables[0].Rows[0]["cnom_substransactu"].ToString()) / 30);
                double diasSubTransporte = FechaIniVacaciones.Day - 1 - int.Parse(diasexceptosauxtransp.ToString()) - transportePagado;
                if (diasSubTransporte < 0)
                    diasSubTransporte = 0;
                double valorSubTransporte = valordiatransporte * diasSubTransporte;
                 
                //liquidar subsidio de transporte
                //si se le paga al empleado el subsidio en ambas quincenas
                /*if  (cnomina.Tables[0].Rows[0]["cnom_substranperinomi"].ToString()=="1")
			   
                {*/
                //		if (DDLQUIN=="1" && lbtipopago=="QUINCENAL")
                {
                    //si al empleado se le paga el auxilio legal o subsidio igual.
                    if (periodosubt == "1" || periodosubt == "2")
                    {
                        //mirar que no sobrepase el tope
                        if (baseSubsidioTranporte <= valortope)
                        {
                            //descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
                            auxiliotransporte = Math.Round(valorSubTransporte, 0);
                            if (auxiliotransporte > 0 )
                            {
                                if (sueldo <= valortope)
                                {
                                    this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString(), (diasSubTransporte), double.Parse(valordiatransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, descripcionconcepto);
                                    sumapagadamasauxt = sumapagado + auxiliotransporte;
                                }
                                else
                                {  // auxilio de transporte negativo, cuando YA se le haya pagado anteriormente
                                    this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString(), (diasSubTransporte), double.Parse(valordiatransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, descripcionconcepto);
                                    sumapagadamasauxt = sumapagado + auxiliotransporte;
                                }
                            }
                            //this.ingresar_datos_datatable(codquincena,codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);								

                            //sumapagado+=auxiliotransporte;
                        }
                        else
                        {
                           // sumapagadamasauxt = sumapagado;
                        }
                    }

                    if (periodosubt == "3")
                    {
                      //  sumapagadamasauxt = sumapagado;
                    }
                }
            }
		/*                      HECTOR transporte		
					if (DDLQUIN=="2" && lbtipopago=="QUINCENAL")
					{
						//int numquincenaanterior=int.Parse(codquincena)-1;
						if (cnomina2.Tables[0].Rows[0]["cnom_quincena"].ToString()=="2")
						{
							string aux=DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+" and MQUI.mqui_mesquin="+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+" and MQUI.mqui_tpernomi=1 ");
							numquincenaanterior=Convert.ToInt32(aux==string.Empty?"0":aux);
						}
						else
						{
							fechaquin=Convert.ToDateTime(lb);
							fechaquin= fechaquin.AddMonths(-1);
							numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
						}				    	
				    	
				    	
						DataSet valepsquinanterior= new DataSet();
						//DBFunctions.Request(valepsquinanterior,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanterior+"");
						//JFSC 28042008 se debe generar el cálculo sobre dpaga_vacaciones
						//DBFunctions.Request(valepsquinanterior,IncludeSchema.NO,"select  distinct dpaga_vacaciones,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanterior+"");
						DBFunctions.Request(valepsquinanterior,IncludeSchema.NO,"select  distinct max(dpaga_vacaciones) from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanterior+"");
						//si no tiene valor de eps anterior , es mensual.
						if (valepsquinanterior.Tables[0].Rows.Count==0)
						{
							
							//mensual
							if (fpagoEmpleado=="2" )
							{
								auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0]["cnom_substransactu"].ToString())-valoradescontar;
								//this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
								if (auxiliotransporte>0 && sueldo<=valortope)
								{							
								
                                    this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString(), (double)Math.Round(auxiliotransporte / valordiatransporte, 0), double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, descripcionconcepto);	
									sumapagadamasauxt=sumapagado+auxiliotransporte;
								}
								else
								{
									sumapagadamasauxt=sumapagado;
								}
								
							}
							else
								sumapagadamasauxt=sumapagado;
							bandera1+=1;
							if (bandera1==1)
							{
								//Response.Write("<script language:javascript>alert('Atencion:No se encontro registro de la primera quincena pago Subsidio de Transporte,solo se tomara en cuenta el salario devengado en la segunda quincena,pudiendo afectar el calculo del tope. ');</script>");
							}
							
						}
						else
						{
							//Response.Write("<script language:java>alert(' estoy en elseeeeeeeeeeee');</script>");
							if (periodosubt=="1"|| periodosubt=="2" )
							{
								//mirar que no sobrepase el tope sumando salario 1q+2q(actual)
								//JFSC 29042008 Toca validar si es nulo el valor eps de la quincena anterior
								String quinant=valepsquinanterior.Tables[0].Rows[0][0].ToString();
							//	acumuladoeps+=double.Parse(quinant==string.Empty?"0":quinant);
						        if (baseSubsidioTranporte <= valortope)
								{
									//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
									if(Convert.ToInt16(cnomina.Tables[0].Rows[0]["cnom_substranperinomi"]) == 2)
                                        auxiliotransporte = double.Parse(cnomina.Tables[0].Rows[0]["cnom_substransactu"].ToString())-valoradescontar;
							        else 
                                        auxiliotransporte = double.Parse(cnomina.Tables[0].Rows[0]["subtransactu2"].ToString())-valoradescontar;
									if (auxiliotransporte>0 && sueldo<=valortope)
									{
										//	this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
                                        this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString(), (double)Math.Round(auxiliotransporte / valordiatransporte, 0), double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, descripcionconcepto);
										sumapagadamasauxt=sumapagado+auxiliotransporte;
									}
									else
									{
										sumapagadamasauxt=sumapagado;
									}									
									//sumapagado+=auxiliotransporte;
								}
								else
								{
									sumapagadamasauxt=sumapagado;
									Response.Write("<script language:javascript>alert('Atencion:Al empleado "+apellido1+" "+apellido2+" "+nombre1+" "+nombre2+" se le pago Subsidio de Transporte en la primera quincena, pero en la segunda supero el tope,se recomienda descontarle el valor de la primera quincena. ');</script>");
								}
							}	
							if (periodosubt=="3")
							{
								sumapagadamasauxt=sumapagado;
							}							
                        }
					}
			     
				//}
				//si se le paga al empleado el subsidio en la segunda quincena

                    /*
                     //      calculo de los dias trabajados en la primera quincena
                        DataSet diastrabajadosqnaanterior = new DataSet();
                        string querydt=@"select coalesce(sum(dqui_canteventos),0) 
                                            from dbxschema.dquincena d,
                                                dbxschema.pconceptonomina p, cnomina c
                                            where d.pcon_concepto=p.pcon_concepto and
                                                d.memp_codiempl= '"+codigoempleado+@"' and 
                                                d.mqui_codiquin="+numquincenaanterior+@" and
                                                (p.pcon_concepto=c.cnom_concsalacodi or p.pcon_concepto=c.cnom_concsalasena) ";
                        DBFunctions.Request(diastrabajadosqnaanterior,IncludeSchema.NO,querydt);
                        int diasqnaanter=0;
                        diasqnaanter=Convert.ToInt32(diastrabajadosqnaanterior.Tables[0].Rows[0][0]);
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
	
                   HECTOR transporte aqui iva * /
                if  (cnomina.Tables[0].Rows[0]["cnom_substranperinomi"].ToString()=="2")
				{
					numquincenaanterior=0;
				
					if (codquincena=="")
					{
						numquincenaanterior=0;
					}
					else
					{					
						//numquincenaanterior=int.Parse(codquincena)-1;
						if (cnomina2.Tables[0].Rows[0]["cnom_quincena"].ToString()=="2")
						{
							if (lbtipopago=="QUINCENAL")
							{
								numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+cnomina2.Tables[0].Rows[0]["cnom_ano"].ToString()+" and MQUI.mqui_mesquin="+cnomina2.Tables[0].Rows[0]["cnom_mes"].ToString()+" and MQUI.mqui_tpernomi=1 "));
							}							
						}
						else
						{
							fechaquin=Convert.ToDateTime(lb);
							fechaquin= fechaquin.AddMonths(-1);
							numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
						}
					}
							
				
					DataSet valepsquinanterior= new DataSet();	
					DBFunctions.Request(valepsquinanterior,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanterior+"");
					//sumapagadamasauxt=sumapagado;	
					//aqui esta el problema..
                    if (DDLQUIN == "2" && lbtipopago == "MENSUAL")
					{
						if (valepsquinanterior.Tables[0].Rows.Count==0)
						{
							//necesito averiguar si el pago es mensual o quincenal
							if (lbtipopago=="MENSUAL")
							{
								if (periodosubt=="1"|| periodosubt=="2" )
								{
												
									//mirar que no sobrepase el tope
                                    if (baseSubsidioTranporte <= valortope)
									{
										//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
										//valoradescontar+=valordiatransporte*double.Parse(valepsquinanterior.Tables[0].Rows[0][1].ToString());
										auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0]["cnom_substransactu"].ToString())-valoradescontar;
										//this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
										if (auxiliotransporte>0 && sueldo<=valortope)
										{
                                            this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString(), (double)Math.Round(auxiliotransporte / valordiatransporte, 0), double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, descripcionconcepto);
											sumapagadamasauxt=sumapagado+auxiliotransporte;
										}
										else
										{
											sumapagadamasauxt=sumapagado;
										}
										//sumapagado+=auxiliotransporte;	
								    }
									else
									{
										sumapagadamasauxt=sumapagado;
									}
								}	
								if (periodosubt=="3")
								{
									sumapagadamasauxt=sumapagado;
								}						
							}
							else
							{
								bandera1+=1;
								if (bandera1==1)
								{
									Response.Write("<script language:javascript>alert('Atencion:No se encontro registro de la primera quincena pago Subsidio de Transporte,solo se tomara en cuenta el salario devengado en la segunda quincena,pudiendo afectar el calculo del tope. ');</script>");
								}
								sumapagadamasauxt=sumapagado;	
							}							
						}
						else
						{
							//si al empleado se le paga el auxilio legal o subsidio igual.
							if (periodosubt=="1"|| periodosubt=="2" )
							{
												
								//mirar que no sobrepase el tope
                                if ((baseSubsidioTranporte + double.Parse(valepsquinanterior.Tables[0].Rows[0][0].ToString())) <= valortope)
								{
									//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
									valoradescontar+=valordiatransporte*double.Parse(valepsquinanterior.Tables[0].Rows[0][1].ToString());
									auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0]["cnom_substransactu"].ToString())-valoradescontar;
									//this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
									if (auxiliotransporte>0 && sueldo<=valortope)
									{
                                        this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString(), (double)Math.Round(auxiliotransporte / valordiatransporte, 0), double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, descripcionconcepto);
										sumapagadamasauxt=sumapagado+auxiliotransporte;
									}
									else
									{
										sumapagadamasauxt=sumapagado;
									}
									//sumapagado+=auxiliotransporte;										
								}
								else
								{
                                    sumapagadamasauxt = sumapagado + auxiliotransporte;
								}
							}	
							if (periodosubt=="3")
							{
								sumapagadamasauxt=sumapagado;
							}					
						}				
					}			
				}
				//***********************************
				//si se le paga al empleado el subsidio en ambas quincenas mes anterior
			
				if  (cnomina.Tables[0].Rows[0]["cnom_substranperinomi"].ToString()=="3")
				{
					if (DDLQUIN=="1")
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
						DBFunctions.Request(valepsquinanteriorprimera,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorprimera+"");
						DBFunctions.Request(valepsquinanteriorsegunda,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorsegunda+"");	
				
						if (valepsquinanteriorprimera.Tables[0].Rows.Count==0)
						{
							sumapagadamasauxt=sumapagado;
				
							bandera1+=1;
							if (bandera1==1)
							{
								Response.Write("<script language:javascript>alert('Atencion:No se encontro registro de la primera quincena pago Subsidio de Transporte del mes pasado,no se liquidara Subsidio de Transporte. ');</script>");
							}
						}
						else
						{
							//si al empleado se le paga el auxilio legal o subsidio igual.
							if (periodosubt=="1"|| periodosubt=="2" )
							{
								valortotalmesanterior=double.Parse(valepsquinanteriorprimera.Tables[0].Rows[0][0].ToString())+double.Parse(valepsquinanteriorsegunda.Tables[0].Rows[0][0].ToString());
								valoradescontarpq=valordiatransporte* double.Parse(valepsquinanteriorprimera.Tables[0].Rows[0][1].ToString());
								valoradescontarsq=valordiatransporte* double.Parse(valepsquinanteriorsegunda.Tables[0].Rows[0][1].ToString());
				
								//mirar que no sobrepase el tope
								if (valortotalmesanterior<=valortope)
								{
									// sacar las ausencias del mes pasado..
					
									auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0]["subtransactu2"].ToString())-valoradescontarpq;
									if (auxiliotransporte>0 && sueldo<=valortope)
									{
										//this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,auxiliotransporte,auxiliotransporte,0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
                                        this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString(), (double)Math.Round(auxiliotransporte / valordiatransporte, 0), double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, descripcionconcepto);
										sumapagadamasauxt=sumapagado+auxiliotransporte;
									}									
									else
									{
										sumapagadamasauxt=sumapagado;
									}
								}
								else
								{
									sumapagadamasauxt=sumapagado;
								}
							}	
							
							if (periodosubt=="3")
							{
								sumapagadamasauxt=sumapagado;
							}
				  		}
					}
				
					if (DDLQUIN=="2")
					{
						//int numquincenaanteriorsegunda2=(int.Parse(codquincena)-2);
						int numquincenaanteriorsegunda2=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
					
						//int numquincenaanteriorprimera2=(int.Parse(codquincena)-3);
						int numquincenaanteriorprimera2=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=1 "));
					
						DataSet valepsquinanteriorprimera2= new DataSet();
						DataSet valepsquinanteriorsegunda2= new DataSet();
						DBFunctions.Request(valepsquinanteriorprimera2,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorprimera2+"");
						DBFunctions.Request(valepsquinanteriorsegunda2,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorsegunda2+"");	
						if (valepsquinanteriorsegunda2.Tables[0].Rows.Count==0 || valepsquinanteriorprimera2.Tables[0].Rows.Count==0)
						{
					
							//si al empleado se le paga el auxilio legal o subsidio igual.
						
							bandera1+=1;
							if (bandera1==1)
							{
								Response.Write("<script language:javascript>alert('Atencion:No se encontro registro de la segunda quincena pago Subsidio de Transporte del mes pasado,no se liquidara Subsidio de Transporte. ');</script>");
							}
							sumapagadamasauxt=sumapagado;
						}
						else
						{
							valortotalmesanterior = double.Parse(valepsquinanteriorprimera2.Tables[0].Rows[0][0].ToString())+double.Parse(valepsquinanteriorsegunda2.Tables[0].Rows[0][0].ToString());
							valoradescontarpq = valordiatransporte* double.Parse(valepsquinanteriorprimera2.Tables[0].Rows[0][1].ToString());
							valoradescontarsq = valordiatransporte* double.Parse(valepsquinanteriorsegunda2.Tables[0].Rows[0][1].ToString());
													
							
							if (periodosubt=="1"|| periodosubt=="2" )
								//mirar que no sobrepase el tope
							{
								if (valortotalmesanterior<=valortope)
								{
									auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0]["subtransactu2"].ToString())-valoradescontarsq;
									//this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,auxiliotransporte,auxiliotransporte,0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
									if (auxiliotransporte>0 && sueldo<=valortope)
									{
                                        this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString(), (double)Math.Round(auxiliotransporte / valordiatransporte, 0), double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, descripcionconcepto);
										sumapagadamasauxt=sumapagado+auxiliotransporte;
									}									
									else
									{
										sumapagadamasauxt=sumapagado;
									}					
								}
								else
								{
									sumapagadamasauxt=sumapagado;	
								}
							}
							if (periodosubt=="3")
							{
								sumapagadamasauxt=sumapagado;
							}
				  		}
					}
				}
				//segunda quincena mes anterior
				if  (cnomina.Tables[0].Rows[0]["cnom_substranperinomi"].ToString()=="4")
				{
					if (DDLQUIN=="1")
					{
						sumapagadamasauxt=sumapagado;
					}
				
				
					if (DDLQUIN=="2")
					{
						//int numquincenaanteriorsegunda2=(int.Parse(codquincena)-2);
						int numquincenaanteriorsegunda2=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
					
										
						//int numquincenaanteriorprimera2=(int.Parse(codquincena)-3);
						int numquincenaanteriorprimera2=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=1 "));
					
					
					
						DataSet valepsquinanteriorprimera2= new DataSet();
						DataSet valepsquinanteriorsegunda2= new DataSet();
						DBFunctions.Request(valepsquinanteriorprimera2,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorprimera2+"");
						DBFunctions.Request(valepsquinanteriorsegunda2,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorsegunda2+"");	
						if (valepsquinanteriorsegunda2.Tables[0].Rows.Count==0 || valepsquinanteriorprimera2.Tables[0].Rows.Count==0)
						{
							//si al empleado se le paga el auxilio legal o subsidio igual.
						
							bandera1+=1;
							if (bandera1==1)
							{
								Response.Write("<script language:javascript>alert('Atencion:No se encontro registro de la segunda quincena pago Subsidio de Transporte del mes pasado,no se liquidara Subsidio de Transporte. ');</script>");
							}
							sumapagadamasauxt=sumapagado;
						}
						else
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
									auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0]["cnom_substransactu"].ToString())-valoradescontartotal;
									//this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,auxiliotransporte,auxiliotransporte,0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
									if (auxiliotransporte>0 && sueldo<=valortope)
									{
                                        this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0]["cnom_concsubtcodi"].ToString(), (double)Math.Round(auxiliotransporte / valordiatransporte, 0), double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, descripcionconcepto);
										sumapagadamasauxt=sumapagado+auxiliotransporte;
									}									
									else
									{
										sumapagadamasauxt=sumapagado;
									}	
								}
								else
								{
									sumapagadamasauxt=sumapagado;	
								}
					
							}
							if (periodosubt=="3")
							{
								sumapagadamasauxt=sumapagado;
							}
				  		}
					}
				}
			}
	 */          //   HECTOR transporte
			//validar las afectaciones
            sumapagado+=auxiliotransporte;
        //    sumapagadamasauxt += auxiliotransporte;
            this.validacionafectaciones(codquincena,codigoempleado,afectaciones.Tables[0].Rows[0][0].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][2].ToString(),"Subsidio de Transporte Credito que afecta EPS? posible error..",double.Parse(auxiliotransporte.ToString("N")),0,0,0,0,0,0,0);
			this.validacionafectaciones(codquincena,codigoempleado,afectaciones.Tables[0].Rows[0][0].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][3].ToString(),"Subsidio de Transporte Credito que afecta Horas extras? posible error..",0,0,0,0,0,0,0,double.Parse(auxiliotransporte.ToString("N")));
			this.validacionafectaciones(codquincena,codigoempleado,afectaciones.Tables[0].Rows[0][0].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][4].ToString(),"Subsidio de Transporte Credito que afecta Primas? posible error..",0,0,double.Parse(auxiliotransporte.ToString("N")),0,0,0,0,0);											
			this.validacionafectaciones(codquincena,codigoempleado,afectaciones.Tables[0].Rows[0][0].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][5].ToString(),"Subsidio de Transporte Credito que afecta Vacaciones? posible error..",0,0,0,double.Parse(auxiliotransporte.ToString("N")),0,0,0,0);
			this.validacionafectaciones(codquincena,codigoempleado,afectaciones.Tables[0].Rows[0][0].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][6].ToString(),"Subsidio de Transporte Credito que afecta Cesantias? posible error..",0,double.Parse(auxiliotransporte.ToString("N")),0,0,0,0,0,0);
			this.validacionafectaciones(codquincena,codigoempleado,afectaciones.Tables[0].Rows[0][0].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][7].ToString(),"Subsidio de Transporte Credito que afecta Retencion en la fuente? posible error..",0,0,0,0,double.Parse(auxiliotransporte.ToString("N")),0,0,0);
			this.validacionafectaciones(codquincena,codigoempleado,afectaciones.Tables[0].Rows[0][0].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][8].ToString(),"Subsidio de Transporte Credito que afecta Provisiones? posible error..",0,0,0,0,0,double.Parse(auxiliotransporte.ToString("N")),0,0);
			this.validacionafectaciones(codquincena,codigoempleado,afectaciones.Tables[0].Rows[0][0].ToString(),1,double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),double.Parse(auxiliotransporte.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][9].ToString(),"Subsidio de Transporte Credito que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,double.Parse(auxiliotransporte.ToString("N")),0);

			
		}

		/*
		protected void liquidar_fondosolidaridadpensional(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string tiposalario)
		 {
			DataSet mquincenas= new DataSet();
			DataSet cnomina= new DataSet();
			DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_concfondsolipens,cnom_porcfondsolipens,cnom_topepagofondsolipens,cnom_salaminiactu,cnom_baseporcsalainteg from dbxschema.cnomina");
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			double topesalminimos,salariocompletomes,descuentosolipens;
			topesalminimos=double.Parse(cnomina.Tables[0].Rows[0][3].ToString())*double.Parse(cnomina.Tables[0].Rows[0][2].ToString());
			int numquincenaanterior=0;
			string descripcionconcepto=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+cnomina.Tables[0].Rows[0][0].ToString()+"' ");
			
			if (mquincenas.Tables[0].Rows[0][0].ToString()=="")
				{
				numquincenaanterior=0;
				}
				else
				{
					
					numquincenaanterior=int.Parse(mquincenas.Tables[0].Rows[0][0].ToString())-1;
				}
				
			//Response.Write("<java language:java>alert('qnumero de qincena anterior en solipens es "+numquincenaanterior+"');</script>");
			DataSet valepsquinanterior= new DataSet();
			DBFunctions.Request(valepsquinanterior,IncludeSchema.NO,"select  distinct dpaga_afeps  from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanterior+"");
			
			//solo reviso si se esta liquidando la segunda quincena.
			//if (DDLQUIN=="2" || DDLQUIN=="1" )
			if (DDLQUIN=="1")
			{
				salariocompletomes=acumuladoeps;
			if (salariocompletomes>topesalminimos)
				{
					//ingreso el descuento si es integral sobre el 70%
					if (tiposalario=="1")
					{
						//Response.Write("<script language:javascript>alert('primera q Descuento soli pens "+salariocompletomes+" ..');</script>");
						descuentosolipens=((salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][4].ToString())/100)*double.Parse(cnomina.Tables[0].Rows[0][1].ToString())/100);
						fondosolidaridadpensional=descuentosolipens;
					}
					else
					{
						//Response.Write("<script language:javascript>alert('primera q Descuento soli pens "+salariocompletomes+" ..');</script>");
						descuentosolipens=(salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][1].ToString())/100);
						fondosolidaridadpensional=descuentosolipens;
						
					}
					this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0][0].ToString(),1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,descripcionconcepto);
					sumadescontado+=descuentosolipens;
				
				}
			}
			
			
			
			if (DDLQUIN=="2")
			{
							
								
				if (lbtipopago=="QUINCENAL")
				{
						
			if (valepsquinanterior.Tables[0].Rows.Count==0)
			{
			
			bandera1+=1;
			if (bandera1==1)
			{
			//Response.Write("<script language:javascript>alert('Atencion:No se encontro registro de la primera quincena para liquidar Fondo de Solidaridad Pensional,solo se tomara en cuenta el salario devengado en la segunda quincena,pudiendo afectar el calculo del tope. ');</script>");
			}
			salariocompletomes=acumuladoeps;
			if (salariocompletomes>topesalminimos)
				{
					//ingreso el descuento si es integral sobre el 70%
					if (tiposalario=="1")
					{
						//Response.Write("<script language:javascript>alert('Descuento soli pens "+salariocompletomes+" ..');</script>");
						descuentosolipens=((salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][4].ToString())/100)*double.Parse(cnomina.Tables[0].Rows[0][1].ToString())/100);
						fondosolidaridadpensional=descuentosolipens;
					}
					else
					{
						//Response.Write("<script language:javascript>alert('Descuento soli pens "+salariocompletomes+" ..');</script>");
						descuentosolipens=(salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][1].ToString())/100);
						fondosolidaridadpensional=descuentosolipens;
						
					}
					this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0][0].ToString(),1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,descripcionconcepto);
					sumadescontado+=descuentosolipens;
				
				}
			
			}
				if (valepsquinanterior.Tables[0].Rows.Count>0)
			{
				salariocompletomes=double.Parse(valepsquinanterior.Tables[0].Rows[0][0].ToString())+acumuladoeps;
				
				//si se le pago mas de (lo que diga cnomina) salarios minimos descontar lo que diga cnomina
			
				if (salariocompletomes>topesalminimos)
				{
					//ingreso el descuento si es integral sobre el 70%
					if (tiposalario=="1")
					{
						descuentosolipens=((salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][4].ToString())/100)*double.Parse(cnomina.Tables[0].Rows[0][1].ToString())/100);
						fondosolidaridadpensional=descuentosolipens;
					}
					else
					{
						descuentosolipens=(salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][1].ToString())/100);
						fondosolidaridadpensional=descuentosolipens;
						
					}
					this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0][0].ToString(),1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,descripcionconcepto);
					sumadescontado+=descuentosolipens;
				
				}
			
				
			}
				}
			
			if (lbtipopago=="MENSUAL")
			{
				salariocompletomes=acumuladoeps;
			if (salariocompletomes>topesalminimos)
				{
					//ingreso el descuento si es integral sobre el 70%
					if (tiposalario=="1")
					{
						descuentosolipens=((salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][4].ToString())/100)*double.Parse(cnomina.Tables[0].Rows[0][1].ToString())/100);
						fondosolidaridadpensional=descuentosolipens;
					}
					else
					{
						descuentosolipens=(salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][1].ToString())/100);
						fondosolidaridadpensional=descuentosolipens;
						
					}
					this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0][0].ToString(),1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,descripcionconcepto);
					sumadescontado+=descuentosolipens;
				
				}	
				}
			}
		 }
		*/

		protected void liquidar_fondosolidaridadpensional(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string tiposalario,double sueldomes)
		{
            string tipoContrato = DBFunctions.SingleData("select  tcon_contrato  from dbxschema.mempleado where memp_codiempl= '" + codigoempleado + "' ");
            if (tipoContrato == "4") return;   // LOS PENSIONADOS no aportan a solidaridad
            DataSet cnom= new DataSet();
			DBFunctions.Request(cnom,IncludeSchema.NO,"SELECT CNOM_NOMBPAGA,PANO_DETALLE,PMES_NOMBRE,TPER_DESCRIPCION,TOPCI_DESCRIPCION FROM DBXSCHEMA.PANO P, DBXSCHEMA.CNOMINA C,DBXSCHEMA.PMES  T,DBXSCHEMA.TPERIODONOMINA N,DBXSCHEMA.TOPCIQUINOMES O WHERE C.CNOM_ANO=P.PANO_ANO AND C.CNOM_MES=T.PMES_MES AND C.CNOM_QUINCENA=N.TPER_PERIODO AND C.CNOM_OPCIQUINOMENS=O.TOPCI_PERIODO");
			lbtipopago=cnom.Tables[0].Rows[0][4].ToString();
			//Response.Write("<script language:java>alert('el numero de la quincena   "+codquincena+" ');</script>");
			DateTime fechaquin= new DateTime();
			DataSet mquincenas= new DataSet();
			DataSet cnomina= new DataSet();
			DataSet cnomina2= new DataSet();
			DataSet validar= new DataSet();
			DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_concfondsolipens,cnom_porcfondsolipens,cnom_topepagofondsolipens,cnom_salaminiactu,cnom_baseporcsalainteg from dbxschema.cnomina");
			DBFunctions.Request(cnomina2,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");	
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+cnomina2.Tables[0].Rows[0][0].ToString()+" and mqui_mesquin="+cnomina2.Tables[0].Rows[0][1].ToString()+" and mqui_tpernomi="+cnomina2.Tables[0].Rows[0][2].ToString()+" ");
			DBFunctions.Request(validar, IncludeSchema.NO,"select * from dbxschema.pfondosolipens");
			string descripcionconcepto=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+cnomina.Tables[0].Rows[0][0].ToString()+"' ");
			//DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			double topesalminimos,salariocompletomes,descuentosolipens,baseint;
			topesalminimos=double.Parse(cnomina.Tables[0].Rows[0][3].ToString())*double.Parse(cnomina.Tables[0].Rows[0][2].ToString());
			//Response.Write("<script language:java>alert('el tope de salarios minimos esta en "+topesalminimos+" ');</script>");
			int numquincenaanterior=0;
			string porcentaje;
			//validar que la tabla contenga datos
			if (validar.Tables[0].Rows.Count==0)
			{

				Response.Write("<script language='javascript'>alert('Porfavor Ingrese la tabla de Descuentos de Solidaridad Pensional');</script>");
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

                    if (cnomina2.Tables[0].Rows[0][2].ToString() == "2")
                    {
                        if (lbtipopago == "QUINCENAL")
                        {
                            try
                            {
                                numquincenaanterior = Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin=" + cnomina2.Tables[0].Rows[0][0].ToString() + " and MQUI.mqui_mesquin=" + cnomina2.Tables[0].Rows[0][1].ToString() + " and MQUI.mqui_tpernomi=1 "));
                            }
                            catch
                            {
                                numquincenaanterior = 0;
                                Response.Write("<script language:javascript>alert('Atención: Hay error en la configuración de la nómina contra los períodos actuales de quincenas..');</script>");
                            }
                            //Response.Write("<script language:java>alert('el numero de la quincena anterior es  "+numquincenaanterior+" ');</script>");
                        }
                    }
                    else
                    {
                        DataSet conteo = new DataSet();
                        DBFunctions.Request(conteo, IncludeSchema.NO, "select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");
                        if (conteo.Tables[0].Rows.Count > 1)
                        {
                            fechaquin = Convert.ToDateTime(lb);
                            fechaquin = fechaquin.AddMonths(-1);
                            try
                            {
                                numquincenaanterior = Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin=" + fechaquin.Year.ToString() + " and MQUI.mqui_mesquin=" + fechaquin.Month.ToString() + " and MQUI.mqui_tpernomi=2 "));
                            }
                            catch
                            {
                                numquincenaanterior = 0;
                                Response.Write("<script language:javascript>alert('Atención: Hay error en la configuración de la nómina contra los períodos actuales de liquidación..');</script>");
                            }
                        }
                    }
				}
				
				//Response.Write("<java language:java>alert('qnumero de qincena anterior en solipens es "+numquincenaanterior+"');</script>");
				DataSet valepsquinanterior= new DataSet();
				DBFunctions.Request(valepsquinanterior,IncludeSchema.NO,"select  distinct max(dpaga_vacaciones)  from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanterior+"");
				//Response.Write("<script language:java>alert('select  distinct dpaga_afeps  from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= \\'"+codigoempleado+"\\' and D.mqui_codiquin="+numquincenaanterior+" ');</script>");
				//solo reviso si se esta liquidando la segunda quincena.
				/*if (DDLQUIN=="2")
				{*/
					//validar la liquidacion de la segunda quincena sin registros anteriores				
				
								
					if (lbtipopago=="QUINCENAL")
					{
						
						if (valepsquinanterior.Tables[0].Rows.Count==0)
						{
			
							bandera1+=1;
							if (bandera1==1)
							{
								Response.Write("<script language:javascript>alert('Atencion:No se encontro registro de la primera quincena para liquidar Fondo de Solidaridad Pensional Empleado: "+codigoempleado+" ,solo se tomara en cuenta el salario devengado en la segunda quincena,pudiendo afectar el calculo del tope. ');</script>");
							}
							//salariocompletomes=sueldomes;
							salariocompletomes=acumuladoeps;
							if (sueldomes>topesalminimos)
							{
								//sacar el valor de porcentaje de la tabla PFONDOSOLIPENS.
								if (tiposalario=="1")
								{
									baseint=(salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][4].ToString()))/100;
									//Response.Write("<script language:javascript>alert('base integral no cojio pago anterior "+codigoempleado+" "+baseint+" ');</script>");				
									porcentaje=DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens, dbxschema.CNOMINA where ("+baseint+ "/ CNOM_SALAMINIACTU) between pfon_intinicio and pfon_intfinal");
									//Response.Write("<script language:javascript>alert('SIN QUINCENA ANTERIOR! Empleado "+codigoempleado+" ,integral: SI, Salario Mes: "+salariocompletomes+", base fondo solidaridad: "+baseint+" , porcentaje : "+porcentaje+" ');</script>");				
								}
								else
								{
									baseint=0;
									porcentaje=DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens, dbxschema.CNOMINA where (" + salariocompletomes+ "/ CNOM_SALAMINIACTU) between pfon_intinicio and pfon_intfinal");
									//Response.Write("<script language:javascript>alert('SIN QUINCENA ANTERIOR! Empleado "+codigoempleado+" ,integral: NO, Salario Mes: "+salariocompletomes+", base fondo solidaridad: "+baseint+" , porcentaje : "+porcentaje+" ');</script>");				
								}
					
								
				
								//ingreso el descuento si es integral sobre el 70% variable tomada de cnomina.
								if (tiposalario=="1")
								{
						
									descuentosolipens=((salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][4].ToString())/100)*double.Parse(porcentaje)/100);
									descuentosolipens=Math.Round(descuentosolipens,0);
									descuentosolipens=(descuentosolipens*double.Parse(porcentaje)/100);
									fondosolidaridadpensional=Math.Round(descuentosolipens,0);
									//Response.Write("<script language:javascript>alert(' valor solidaridad descontado : "+fondosolidaridadpensional+"   ');</script>");				
								}
								else
								{
									descuentosolipens=(salariocompletomes*double.Parse(porcentaje)/100);
									descuentosolipens=Math.Round(descuentosolipens,0);
									//descuentosolipens=(descuentosolipens*double.Parse(porcentaje)/100);
									fondosolidaridadpensional=Math.Round(descuentosolipens,0);
									//Response.Write("<script language:javascript>alert(' valor solidaridad descontado : "+fondosolidaridadpensional+"   ');</script>");				
								}
								if (porcentaje!="0")
								{
								
									//this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,cnomina.Tables[0].Rows[0][0].ToString(),1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
									
									
										this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0][0].ToString(),1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,descripcionconcepto);
									
									
										//this.ingresar_datos_datatable(codquincena,"",cnomina.Tables[0].Rows[0][0].ToString(),1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
									
								
								}
								sumadescontado+=descuentosolipens;
				
							}
			
						}
						if (valepsquinanterior.Tables[0].Rows.Count>0)
						{
							string epsAnt=valepsquinanterior.Tables[0].Rows[0][0].ToString();
				//h			salariocompletomes=double.Parse(epsAnt==string.Empty?"0":epsAnt)+acumuladoeps;
							salariocompletomes=acumuladoeps;
							porcentaje=DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens, dbxschema.CNOMINA where ROUND(" + salariocompletomes+ " / CNOM_SALAMINIACTU,2) between pfon_intinicio and pfon_intfinal");
							//si se le pago mas de (lo que diga cnomina) salarios minimos descontar lo que diga cnomina
							//salariocompletomes+=acumuladoeps;
							if (salariocompletomes>topesalminimos)
							{
								//ingreso el descuento si es integral sobre el 70%
								if (tiposalario=="1")
								{
									baseint=(salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][4].ToString()))/100;
									porcentaje=DBFunctions.SingleData("select coalesce(pfon_porcentaje,0) from dbxschema.pfondosolipens, dbxschema.CNOMINA where ROUND(" + baseint+ " / CNOM_SALAMINIACTU,2) between pfon_intinicio and pfon_intfinal");
                                    if (porcentaje == "")
                                        porcentaje = "0.0";
                              	}
								else
								{
									baseint=0;
									porcentaje=DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens, dbxschema.CNOMINA where ROUND(" + salariocompletomes+ " / CNOM_SALAMINIACTU,2) between pfon_intinicio and pfon_intfinal");
                                    if (porcentaje == "")
                                        porcentaje = "0.0";
                              	}
					
								//ingreso el descuento si es integral sobre el 70% variable tomada de cnomina.
								if (tiposalario=="1")
								{
						
									descuentosolipens=((salariocompletomes*double.Parse(cnomina.Tables[0].Rows[0][4].ToString())/100)*double.Parse(porcentaje)/100);
									descuentosolipens=Math.Round(descuentosolipens,0);
									fondosolidaridadpensional=Math.Round(descuentosolipens,0);
								}
								else
								{
									descuentosolipens=(salariocompletomes*double.Parse(porcentaje)/100);
									descuentosolipens=Math.Round(descuentosolipens,0);
									fondosolidaridadpensional=Math.Round(descuentosolipens,0);
					
								}
					
								if (porcentaje!="0")
								{
									this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0][0].ToString(),1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,descripcionconcepto);
								}
					
								sumadescontado+=descuentosolipens;
							}
						}
					}
			
					if (lbtipopago=="MENSUAL")
					{
						salariocompletomes=acumuladoeps;
                        porcentaje = "0";
                        descuentosolipens = 0;

                        //ingreso el descuento si es integral sobre el 70%
                        if (tiposalario == "1")
                            baseint = (salariocompletomes * double.Parse(cnomina.Tables[0].Rows[0][4].ToString())) / 100;
                        else
                            baseint = salariocompletomes;
                        if (baseint > topesalminimos)
                        {
                            double baseRetencion = baseint / Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU);
                            porcentaje = DBFunctions.SingleData("select coalesce(pfon_porcentaje,0) from dbxschema.pfondosolipens, dbxschema.CNOMINA where ROUND(" + baseRetencion + " ) between pfon_intinicio and pfon_intfinal-0.0001");
                            if (porcentaje == string.Empty) porcentaje = "0";
                            descuentosolipens = (baseint * double.Parse(porcentaje) / 100);
                            descuentosolipens = Math.Round(descuentosolipens, 0);
                            fondosolidaridadpensional = Math.Round(descuentosolipens, 0);

                            if (porcentaje != "0")
                            {
                                this.ingresar_datos_datatable(cnomina.Tables[0].Rows[0][0].ToString(), 1, double.Parse(descuentosolipens.ToString("N")), 0, double.Parse(descuentosolipens.ToString("N")), "PESOS M/CTE", 0, docref, descripcionconcepto);
                                sumadescontado += descuentosolipens;
                            }
                        }
					}
			
				//}
			}
		}
		
		
		
		protected void actuliza_prestamoempledos(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps) 
		{
			DataSet mquincenas= new DataSet();
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			double valorcuota,intereses,capital,saldo=0,parte2,cuotamasinteres=0,saldoporpagar=0;
			DataSet prestamoempledos= new DataSet();
			int i,j;
			string descripcionconcepto="";
			DataSet cnomina= new DataSet();
			DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes,cnom_salaminiactu from dbxschema.cnomina");
			DDLQUIN= cnomina.Tables[0].Rows[0][2].ToString();

						
			// traigo los prestamos para el empleado que afecten al periodo de pago elegido y que esten  activos,descuento primera o ambas quincenas
			
			//if (DDLQUIN=="1")
			//{
				
				DBFunctions.Request(prestamoempledos,IncludeSchema.NO, "SELECT M.pcon_concepto,MPRE_SECUENCIA,MPRE_NUMELIBR,MPRE_TPERNOMI,MPRE_FECHPREST,MPRE_NUMECUOT,MPRE_VALORPRES,coalesce(MPRE_CUOTPAGA,0) as MPRE_CUOTPAGA,MPRE_PORCINTE,MPRE_VALOPAGA FROM DBXSCHEMA.MPRESTAMOEMPLEADOS M,DBXSCHEMA.PCONCEPTONOMINA P WHERE M.pcon_concepto=P.pcon_concepto and M.memp_codiempl='" + codigoempleado+ "' and P.pcon_claseconc='F' and mpre_estdpres=1 and (mpre_tpernomi=1 AND "+ DDLQUIN +" = 1 OR mpre_tpernomi=2 AND "+ DDLQUIN + "= 2 or mpre_tpernomi=3) AND mpre_valorpres > mpre_valopaga AND (MPRE_FECHPREST <= '" + lb2.ToString()+ "') AND M.PCON_CONCEPTO||M.MPRE_NUMELIBR||M.MEMP_CODIEMPL NOT IN (SELECT  D.PCON_CONCEPTO||D.DQUI_DOCREFE||D.MEMP_CODIEMPL FROM MQUINCENAS M, DQUINCENA D, CNOMINA CN WHERE M.MQUI_CODIQUIN = D.MQUI_CODIQUIN AND M.MQUI_ANOQUIN = CN.CNOM_ANO AND M.MQUI_MESQUIN = CN.CNOM_MES) ");

            //}

            //if (DDLQUIN=="2")
            //{

            //	DBFunctions.Request(prestamoempledos,IncludeSchema.NO,"SELECT M.pcon_concepto,MPRE_SECUENCIA,MPRE_NUMELIBR,MPRE_TPERNOMI,MPRE_FECHPREST,MPRE_NUMECUOT,MPRE_VALORPRES,coalesce(MPRE_CUOTPAGA,0) as MPRE_CUOTPAGA,MPRE_PORCINTE,MPRE_VALOPAGA FROM DBXSCHEMA.MPRESTAMOEMPLEADOS M,DBXSCHEMA.PCONCEPTONOMINA P WHERE M.pcon_concepto=P.pcon_concepto and M.memp_codiempl='"+codigoempleado+"' and P.pcon_claseconc='F' and mpre_estdpres=1 and (mpre_tpernomi=2 or mpre_tpernomi=3)  AND (MPRE_FECHPREST <= '"+lb2.ToString()+"') ");

            //}
            if (prestamoempledos.Tables[0].Rows.Count!=0)
			{
				for (i=0;i<prestamoempledos.Tables[0].Rows.Count;i++)
				{
                    descripcionconcepto = DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='" + prestamoempledos.Tables[0].Rows[i][0].ToString() + "' ");
					valorcuota=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())/int.Parse(prestamoempledos.Tables[0].Rows[i][5].ToString());
					parte2=double.Parse(prestamoempledos.Tables[0].Rows[i][7].ToString())*valorcuota;
			
					if (parte2==0)
					{
						intereses=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())*double.Parse(prestamoempledos.Tables[0].Rows[i][8].ToString())/100;
						cuotamasinteres=valorcuota+intereses;
						capital = valorcuota;
						saldo=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())-capital;
					
						this.ingresar_datos_datatable(prestamoempledos.Tables[0].Rows[i][0].ToString(),1,cuotamasinteres,0,cuotamasinteres,"PESOS M/CTE",saldo,prestamoempledos.Tables[0].Rows[i][2].ToString(),descripcionconcepto);
					
						DBFunctions.NonQuery("insert into dprestamoempleados values ("+codquincena+","+prestamoempledos.Tables[0].Rows[i][2].ToString()+",1,"+valorcuota+","+intereses+","+capital+","+saldo+",default)");
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
							this.ingresar_datos_datatable(prestamoempledos.Tables[0].Rows[i][0].ToString(),1,cuotamasinteres,0,cuotamasinteres,"PESOS M/CTE",saldo,prestamoempledos.Tables[0].Rows[i][2].ToString(),descripcionconcepto);
											
							DBFunctions.NonQuery("insert into dprestamoempleados values ("+codquincena+","+prestamoempledos.Tables[0].Rows[i][2].ToString()+",1,"+valorcuota+","+intereses+","+capital+","+saldo+",default)");
							sumadescontado+=cuotamasinteres;
						}	
					}
				}
			}
		}
		
		
		
		protected void actuliza_registro_mpagosydtosper(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string FechaFinVacaciones,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps) 
		{
            DataSet mpagosydtosper = new DataSet();
            DataSet mquincenas = new DataSet();
            DBFunctions.Request(mquincenas, IncludeSchema.NO, "select max(mqui_codiquin) from mquincenas");

            o_Varios.Actualiza_mpagosydtosper(codigoempleado, lb, lb2, codquincena, sueldo, docref, lb2, apellido1, apellido2, nombre1, nombre2, periodoeps, mpagosydtosper);

            if (mpagosydtosper.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < mpagosydtosper.Tables[0].Rows.Count; i++)
                {
                    docref = mpagosydtosper.Tables[0].Rows[i]["MPAG_SECUENCIA"].ToString();
                    this.validaciondebitaacredita(mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,mpagosydtosper.Tables[0].Rows[i][5].ToString(), mpagosydtosper.Tables[0].Rows[i]["PCON_NOMBCONC"].ToString());
						this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE" ,0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][7].ToString(),"Novedad de Descuento Permanente que afecta EPS? posible error..",double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0,0,0,0,0);
						this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][8].ToString(),"Novedad de descuento Permanente que afecta Horas extras? posible error..",0,0,0,0,0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()));
						this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][9].ToString(),"Novedad de descuento Permanente que afecta Primas? posible error..",0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0,0,0);											
						this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][10].ToString(),"Novedad de descuento Permanente que afecta Vacaciones? posible error..",0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0,0);
						this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][11].ToString(),"Novedad de descuento Permanente que afecta Cesantias? posible error..",0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0,0,0,0);
						this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][12].ToString(),"Novedad de descuento Permanente que afecta Retencion en la fuente? posible error..",0,0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0);
						this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][13].ToString(),"Novedad de descuento Permanente que afecta Provisiones? posible error..",0,0,0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0);
						this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][14].ToString(),"Novedad de descuento Permanente que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0);
				}
			}			
	    }
		

	protected void actualiza_registro_msusplicencias(string  codigoempleado,string lab,string lab2,string codquincena, double sueldo,string docref,string labmas1,string apellido1,string apellido2,string nombre1,string nombre2)
		{

            return;   // YA SE LIQUIDO EN LA PARTE DE LOS DIAS

            int i;
						
			double   valordiatrabajo = sueldo/30;
			
			valormsusplicencias = 0;
			DataSet  mquincenas= new DataSet();
			
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			//mirar si ahy suspension,licencias o incapacidades entre la quincena exacta y de cuantos dias
			
			DataSet  msusplicencias= new DataSet();
			DataSet  msusplicencias2= new DataSet();
			DataSet  msusplicencias3= new DataSet();
            string   sqllic;

            // FechaI  = Fecha de Inicio de la licencia o de la quincena o mes segun sea el pago quincenal o mensual si el incio de la licencia es menor al periodo
            // FechaF  = Fecha de Inicio de la salida a vacaciones
            string   fechaIQ = fechaIniQuin.ToString("yyyy-MM-dd");


     //     unificar estas sentencias en una sola invocandola a las rutinas generales

            sqllic = "select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,"
    //      +"(M.msus_hasta-M.msus_desde)+1, "
            +"CASE WHEN M.msus_desde < '"+ fechaIQ +"' THEN (M.msus_hasta-'"+ fechaIQ +"')+1 ELSE (M.msus_hasta-M.msus_desde)+1 END AS DIAS,"
            +"M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion,P.pcon_nombconc from msusplicencias M,pconceptonomina P,tdesccantidad T " 
            +"where M.memp_codiempl='"+codigoempleado+"' and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) "
   //         + "and (M.msus_desde between '" + fechaIniQuin + "' and '" + fechaIQ + "') 
            + " and M.msus_desde < '" + fechaIQ + "' AND (M.msus_hasta between '" + fechaIQ + "' and '" + fechaIniVaca + "') ";
	 
            DBFunctions.Request(msusplicencias, IncludeSchema.NO, sqllic);
			for (i=0;i<msusplicencias.Tables[0].Rows.Count;i++)
			   {
				//asi la licencia sea remunerada se le descuenta el subsidio de transporte
				diasexceptosauxtransp+=int.Parse(msusplicencias.Tables[0].Rows[i][4].ToString());
				valormsusplicencias=int.Parse(msusplicencias.Tables[0].Rows[i][4].ToString())*valordiatrabajo;
				if (msusplicencias.Tables[0].Rows[i][6].ToString()=="H" && (msusplicencias.Tables[0].Rows[i][5].ToString()=="1" || msusplicencias.Tables[0].Rows[i][5].ToString()=="4"))
				   {
					this.ingresar_datos_datatable(msusplicencias.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),0,Math.Round(valormsusplicencias, 0),msusplicencias.Tables[0].Rows[i][7].ToString(),0,docref,msusplicencias.Tables[0].Rows[i][8].ToString());
					sumadescontado+=valormsusplicencias;
				   }
				else if (msusplicencias.Tables[0].Rows[i][6].ToString()=="D" && (msusplicencias.Tables[0].Rows[i][5].ToString()=="2" || msusplicencias.Tables[0].Rows[i][5].ToString()=="3"))
				        {
					     this.ingresar_datos_datatable(msusplicencias.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valormsusplicencias, 0),0,msusplicencias.Tables[0].Rows[i][7].ToString(),0,docref,msusplicencias.Tables[0].Rows[i][8].ToString());
					     sumapagado+=valormsusplicencias;
				        }
					 else
				        {
					     this.ingresar_datos_datatable("Error de incongruencia en SuspLic, Corrija",0,0,0,0,"0",0,docref,msusplicencias.Tables[0].Rows[i][8].ToString());
					     errores+=1;
				        }
			   }
			
			//mirar si la suspension,licencias o incapacidades inicia en el periodo, y se pasa del periodo, cuantos dias le debo descontar 
			DateTime lab2mas1 = new DateTime();
			string   lab2mas11;
			lab2mas1=Convert.ToDateTime(lab2.ToString());
			lab2mas1=lab2mas1.AddDays(1);
			lab2mas11=lab2mas1.Date.ToString("yyyy-MM-dd");
			
			//prueba2.Text="select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,('"+lb2.ToString()+"'-M.msus_desde)+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion from msusplicencias M,pconceptonomina P,tdesccantidad T  ,msusplicencias M2 where M.memp_codiempl='"+codigoempleado+"' and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) and (M.msus_desde between '"+lb.ToString()+"' and '"+lb2.ToString()+"') and (M.msus_hasta between '"+lb2mas11.ToString()+"' and M2.msus_hasta)";
			DBFunctions.Request(msusplicencias2,IncludeSchema.NO,"select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,('"+lab2.ToString()+"'-M.msus_desde)+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion,P.pcon_nombconc from msusplicencias M,pconceptonomina P,tdesccantidad T  ,msusplicencias M2 where M.memp_codiempl='"+codigoempleado+"' and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) and (M.msus_desde between '"+lab.ToString()+"' and '"+lab2.ToString()+"') and (M.msus_hasta between '"+lab2mas11.ToString()+"' and M2.msus_hasta)");
			
			for (i=0;i<msusplicencias2.Tables[0].Rows.Count;i++)
		    {
				diasexceptosauxtransp+=int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString());
				valormsusplicencias=int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString())*valordiatrabajo;
				if (msusplicencias2.Tables[0].Rows[i][6].ToString()=="H" && (msusplicencias2.Tables[0].Rows[i][5].ToString()=="1" || msusplicencias2.Tables[0].Rows[i][5].ToString()=="4"))
				   {
					this.ingresar_datos_datatable(msusplicencias2.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),0,Math.Round(valormsusplicencias, 0),msusplicencias2.Tables[0].Rows[i][7].ToString(),0,docref,msusplicencias2.Tables[0].Rows[i][8].ToString());
					sumadescontado+=valormsusplicencias;
				   }
				else if (msusplicencias2.Tables[0].Rows[i][6].ToString()=="D" && (msusplicencias2.Tables[0].Rows[i][5].ToString()=="2" || msusplicencias2.Tables[0].Rows[i][5].ToString()=="3"))
				       {
					    this.ingresar_datos_datatable(msusplicencias2.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valormsusplicencias, 0),0,msusplicencias2.Tables[0].Rows[i][7].ToString(),0,docref,msusplicencias2.Tables[0].Rows[i][8].ToString());
					    sumapagado+=valormsusplicencias;
				       }
					 else
				        {
					     this.ingresar_datos_datatable("Error de incongruencia en susplic",0,0,0,0,"0",0,docref,msusplicencias2.Tables[0].Rows[i][8].ToString());
					     errores+=1;
				        }
			}
				
			//mirar si la suspension inicia antes del periodo y se termina en el periodo
			DBFunctions.Request(msusplicencias3,IncludeSchema.NO,"select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,(M.msus_hasta-'"+lab.ToString()+"')+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion,P.pcon_nombconc from msusplicencias M,pconceptonomina P,tdesccantidad T,msusplicencias M2 where M.memp_codiempl='"+codigoempleado+"'and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) and (M.msus_desde between M.msus_desde and '"+lab.ToString()+"') and (M.msus_hasta between '"+lab.ToString()+"' and '"+lab2.ToString()+"')");
			for (i=0;i<msusplicencias3.Tables[0].Rows.Count;i++)
			{
				diasexceptosauxtransp+=int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString());
				valormsusplicencias =int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString())*valordiatrabajo;
				if (msusplicencias3.Tables[0].Rows[i][6].ToString()=="H" && (msusplicencias3.Tables[0].Rows[i][5].ToString()=="1" || msusplicencias3.Tables[0].Rows[i][5].ToString()=="4"))
				   {
					this.ingresar_datos_datatable(msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),0,Math.Round(valormsusplicencias, 0),msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,msusplicencias3.Tables[0].Rows[i][8].ToString());
					sumadescontado  += valormsusplicencias;
				   }
				else if (msusplicencias3.Tables[0].Rows[i][6].ToString()=="D" && (msusplicencias3.Tables[0].Rows[i][5].ToString()=="2" || msusplicencias3.Tables[0].Rows[i][5].ToString()=="3"))
				       {
            		    this.ingresar_datos_datatable(msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valormsusplicencias, 0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,msusplicencias3.Tables[0].Rows[i][8].ToString());
					    sumapagado  += valormsusplicencias;
				       }
				     else
				        {
					     this.ingresar_datos_datatable("Error de incongruencia en susplic",0,0,0,0,"0",0,docref,msusplicencias3.Tables[0].Rows[i][8].ToString());
					     errores+=1;
				        }
			}
		}
		
		
		
		protected void actualiza_novedades(string periodoNOmina, string  codigoempleado,string lab,string lab2,string codquincena, double sueldo,string docref,string labmas1,string apellido1,string apellido2,string nombre1,string nombre2)
		{
		int j;
		double valordiatrabajo=0;
		double valorhoratrabajonormal=0;
		double valorminuto=0;
		double valorhorasextras,valorevento,valorpagado;
		DataSet afectahorasextras= new DataSet();

        //if (periodoNOmina.ToString() == "2" && lab.Substring(8,2) != "16" )
        //    lab = lab.Substring(0,8)+"16";
		
		DBFunctions.Request(afectahorasextras,IncludeSchema.NO,"select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and P.tres_afechoraextr='S' and (M.mnov_fecha between '"+lab.ToString()+"' and '"+lab2.ToString()+"')");
		if (afectahorasextras.Tables[0].Rows.Count==0)
		{
		    valordiatrabajo=sueldo/30;
		    valorhoratrabajonormal=sueldo/240;
		    valorminuto=sueldo/14400;
		}
		else {
			for (j=0;j<afectahorasextras.Tables[0].Rows.Count;j++)
				sueldo=sueldo+double.Parse(afectahorasextras.Tables[0].Rows[j][1].ToString());
			
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
			DBFunctions.Request(novedades,IncludeSchema.NO,"select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva,P.pcon_nombconc  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_claseconc='N' and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and (M.mnov_fecha between '"+lab.ToString()+"' and '"+lab2.ToString()+"') ");
			//DBFunctions.Request(novedades,IncludeSchema.NO,"select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva,P.pcon_nombconc  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_claseconc='N' and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and (M.mnov_fecha between '"+lab.ToString()+"' and '"+lab2.ToString()+"')");
			//validar si existen licencias en novedades
			DBFunctions.Request(licenciasennovedades,IncludeSchema.NO,"select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva,P.pcon_nombconc  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_claseconc='L' and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and (M.mnov_fecha between '"+lab.ToString()+"' and '"+lab2.ToString()+"')");
		    //mostrar mensaje de adevertencia
		    for(j=0;j<licenciasennovedades.Tables[0].Rows.Count;j++)
		    {
		    	Response.Write("<script language:javascript>alert('Apreciado Usuario,se detecto el ingreso de un concepto tipo Licencia en Novedades,este concepto no sera tenido en cuenta para la liquidacion,porfavor corrija e ingrese este concepto en Licencias..');</script>");
		    	errores+=1;
		    }
			
			
			//Recorro cada novedad
			
		     for(j=0;j<novedades.Tables[0].Rows.Count;j++)
			{
				
				
		     	//si es novedad de horas=2
				
		     	if (novedades.Tables[0].Rows[j][5].ToString()=="2")
		     	{
					
					valorevento=valorhoratrabajonormal*double.Parse(novedades.Tables[0].Rows[j][6].ToString());
					valorhorasextras= valorhoratrabajonormal*double.Parse(novedades.Tables[0].Rows[j][4].ToString())* double.Parse(novedades.Tables[0].Rows[j][6].ToString());
					this.validaciondebitaacredita(novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][16].ToString());
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad en Horas de descuento que afecta EPS? posible error..",valorhorasextras,0,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad en Horas de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,valorhorasextras);											
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad en Horas de descuento que afecta Primas? posible error..",0,0,valorhorasextras,0,0,0,0,0);											
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad en Horas de descuento que afecta Vacaciones? posible error..",0,0,0,valorhorasextras,0,0,0,0);
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad en Horas de descuento que afecta Cesantias? posible error..",0,valorhorasextras,0,0,0,0,0,0);
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad en Horas de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,valorhorasextras,0,0,0);
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad en Horas de descuento que afecta Provisiones? posible error..",0,0,0,0,0,valorhorasextras,0,0);
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad en Horas de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,valorhorasextras,0);
		     		
		     	}
				//si es novedad de dias=1
				if (novedades.Tables[0].Rows[j][5].ToString()=="1")
				{
					valorpagado=valordiatrabajo*double.Parse(novedades.Tables[0].Rows[j][4].ToString());
					this.validaciondebitaacredita(novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][16].ToString());
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad de dias de descuento que afecta EPS? posible error..",valorpagado,0,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad de dias de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,valorpagado);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad de dias de descuento que afecta Primas? posible error..",0,0,valorpagado,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad de dias de descuento que afecta Vacaciones? posible error..",0,0,0,valorpagado,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad de dias de descuento que afecta Cesantias?  posible error..",0,valorpagado,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad de dias de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,valorpagado,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad de dias de descuento que afecta Provisiones? posible error..",0,0,0,0,0,valorpagado,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad de dias de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,valorpagado,0);
					
						
				}
		     	
				//si es novedad de minutos=3
				if (novedades.Tables[0].Rows[j][5].ToString()=="3")
				{
					valorpagado=valorminuto*double.Parse(novedades.Tables[0].Rows[j][4].ToString());
					this.validaciondebitaacredita(novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][16].ToString());
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad de minutos de descuento que afecta EPS? posible error..",valorpagado,0,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad de minutos de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,valorpagado);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad de minutos de descuento que afecta Primas? posible error..",0,0,valorpagado,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad de minutos de descuento que afecta Vacaciones? posible error..",0,0,0,valorpagado,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad de minutos de descuento que afecta Cesantias? posible error..",0,valorpagado,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad de minutos de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,valorpagado,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad de minutos de descuento que afecta Provisiones? posible error..",0,0,0,0,0,valorpagado,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad de minutos de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,valorpagado,0);
					
				}
				
				//si es novedad en pesos=4
				if (novedades.Tables[0].Rows[j][5].ToString()=="4")
				{
					
					this.validaciondebitaacredita(novedades.Tables[0].Rows[j][0].ToString(),1,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),novedades.Tables[0].Rows[j][7].ToString(),0,docref,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][16].ToString());
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad pesos m/cte de descuento que afecta EPS? posible error..",Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad pesos m/cte de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())));
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad pesos m/cte de descuento que afecta Primas? posible error..",0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad pesos m/cte de descuento que afecta Vacaciones?  posible error..",0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad pesos m/cte de descuento que afecta Cesantias? posible error..",0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad pesos m/cte de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad pesos m/cte de descuento que afecta Provisiones? posible error..",0,0,0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad pesos m/cte de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0);
					
				}
				
			}
		 //Response.Write("<script language:java>alert('sali de novedades voy en "+sumapagado+ " ');</script>");                                  
		}
		protected void validaciondebitaacredita(string concepto,double canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string camposumaresta,string descripcionconcepto)
		                                        {
		                                        	if (camposumaresta=="D")
		                                        	{
		                                        		//Response.Write("<script language:java>alert('novedades tengo  "+sumapagado+ " ');</script>");                                  
		                                        		this.ingresar_datos_datatable(concepto,canteventos,valorevento,apagar,0,descripcion,saldo,docref,descripcionconcepto);
		                                        		sumapagado+=Math.Round(apagar,0);
		                                        		//Response.Write("<script language:java>alert('le sumo   "+apagar+ " ');</script>");                                  
		                                        		//Response.Write("<script language:java>alert('novedades quecdo en   "+sumapagado+ " ');</script>");                                  
		                                        	}
		                                        	else
		                                        	{
		                                        		this.ingresar_datos_datatable(concepto,canteventos,valorevento,0,adescontar,descripcion,saldo,docref,descripcionconcepto);
		                                        		sumadescontado+=Math.Round(adescontar,0);
		                                        	}
		                                        }
		
		
		
		protected void validacionafectaciones(string codquincena,string codempleado,string concepto,double canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2,string camposumaresta,string campocomparacion,string mensaje,double valorpagadoeps,double valorpagadocesantia,double valorpagadoprima,double valorpagadovacaciones,double valorpagadoretefuente,double valorpagadoprovisiones,double valorpagadoliqdefinitiva,double valorpagadohorasextras)
		{
		//Clasifico cada novedad si suma o resta al empleado
		if (camposumaresta=="D" && campocomparacion=="S")
		{
		 	acumuladoeps          += Math.Round(valorpagadoeps,0);
			acumuladocesantia     += Math.Round(valorpagadocesantia,0);
			acumuladoprima        += Math.Round(valorpagadoprima,0);
			acumuladovacaciones   += Math.Round(valorpagadovacaciones,0);
			acumuladoretefuente   += Math.Round(valorpagadoretefuente,0);
			acumuladoprovisiones  += Math.Round(valorpagadoprovisiones,0);
			acumuladoliqdefinitiva+= Math.Round(valorpagadoliqdefinitiva,0);
			acumuladohorasextras  += Math.Round(valorpagadohorasextras,0);			
		}
		else if(camposumaresta=="H" && campocomparacion=="S" )
		     {
			    Response.Write("<script language:javascript>alert('"+mensaje+"');</script>");
			    errores+=1;
		     }
		}
		
		
		protected void imprimirGrilla(Object sender, EventArgs e)
		{
			Response.Redirect(mainpage+"?process=Nomina.FormatoComprobantePagoVacaciones&DIAS="+LBDTVACACIONES.Text+"&VALORVACA="+LBVACAAPAGAR.Text+"&FECHA="+LBPERIODO.Text.Replace('/','*')+"&DIASEFECTIVOS="+LBDIASEFECTIVOS.Text+" ");
			
		}
 		
		
		protected void preparargrilla_empleadoliquidacion()
		{
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("CONCEPTO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("CANT EVENTOS",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("VALOR EVENTO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("A PAGAR",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("A DESCONTAR",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("TIPO EVENTO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("SALDO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("DOC REFERENCIA",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));
		}
		
		protected void ingresar_datos_datatable(string concepto,double canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string descripcionconcepto)
		{
			bool pasar = false;			
			if (descripcionconcepto == "VACACIONES" )
			{
				pasar = true;
				valorvacaciones = apagar;
				eventoepesfondo+=canteventos;
			}
			else
			{
				if (descripcionconcepto == "SUELDO" )eventoepesfondo+=canteventos;
				//if (descripcionconcepto == "AUX. TRANSPORTE" )eventoepesfondo+=apagar;
				if (((DropDownList)DATAGRIDPERESCOGIDO.Items[0].Cells[9].FindControl("DDLFORMA")).SelectedValue !="EN DINERO")
				{
					pasar = true;
				}
				else
				{
					if (concepto == "--")
					{
						pasar = true;
						adescontar = 0;
						apagar = valorvacaciones;
						saldo = apagar;
					}
				}
			}
			if (pasar)
			{
               
				DataRow fila = DataTable1.NewRow();
				fila["CONCEPTO"]=concepto;
				fila["DESCRIPCION"]=descripcionconcepto;
				fila["CANT EVENTOS"]=canteventos;
				fila["VALOR EVENTO"]=Math.Round(valorevento,0);
				fila["A PAGAR"]=Math.Round(apagar,0);
				fila["A DESCONTAR"]=Math.Round(adescontar,0);
				fila["TIPO EVENTO"]=descripcion;
				fila["SALDO"]=Math.Round(saldo,0);
				fila["DOC REFERENCIA"]=docref; 
			    
				DataTable1.Rows.Add(fila);
                if (concepto == o_CNomina.CNOM_CONCFONDPENSVOLU)
                    valorfondopensionvoluntaria += adescontar;
			    
				DataGrid2.DataSource = DataTable1;
				DataGrid2.DataBind();
				DatasToControls.Aplicar_Formato_Grilla(DataGrid2);
				DatasToControls.JustificacionGrilla(DataGrid2,DataTable1);
			}
		}
		
		protected void liquidar_provisiones(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,double acumuladoprovisiones,string tiposalario)
		{
		int i;
		double provcesantias,provinteresescesantia,provprimadeservicios,provvacaciones,provextralegal;
		DataSet pprovisionnomina= new DataSet();
		DataSet cnomina= new DataSet();	
		DBFunctions.Request(pprovisionnomina,IncludeSchema.NO,"select ppro_codiprov,ppro_nombprov,ppro_porcprov,ppro_ttipo from dbxschema.pprovisionnomina");
		DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_substransactu,cnom_baseporcsalainteg from dbxschema.cnomina");			
		
		for (i=0;i<pprovisionnomina.Tables[0].Rows.Count;i++)
			{
				//provision cesantias
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="1")
				{
					if (tiposalario=="1")
					{
						provcesantias=0;
					}
					else
					{
						provcesantias= acumuladoprovisiones * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						provcesantias=Math.Round(provcesantias,0);
					}
									
					DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provcesantias+")");
					//TXTPRUEBA.Text+="insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"',"+pprovisionnomina.Tables[0].Rows[0][0].ToString()+","+provcesantias+" ";
				}
				//provision intereses de cesantia
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="2")
				{
					if (tiposalario=="1")
					{
						provinteresescesantia=0;
					}
					else
					{
						provcesantias= acumuladoprovisiones * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						provcesantias=Math.Round(provcesantias,0);
						provinteresescesantia=provcesantias* double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						provinteresescesantia=Math.Round(provinteresescesantia,0);
					}
					
					DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provinteresescesantia+")");
					
				}
				//provision prima de servicios
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="3")
				{
					if (tiposalario=="1")
					{
						provprimadeservicios=0;
					}
					else
					{
						provprimadeservicios=acumuladoprovisiones * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						provprimadeservicios=Math.Round(provprimadeservicios,0);
					}
					
					DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provprimadeservicios+")");
				}
				//provision vacaciones
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="4")
				{
					if (tiposalario=="1")
					{
						provvacaciones=(acumuladoprovisiones*double.Parse(cnomina.Tables[0].Rows[0][1].ToString())/100) * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
					}
					else
					{
						provvacaciones=(acumuladoprovisiones-double.Parse(cnomina.Tables[0].Rows[0][0].ToString())) * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						provvacaciones=Math.Round(provvacaciones,0);
					}
					
					DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provvacaciones+")");
				}
				//provision extralegal
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="5")
				{
					if (tiposalario=="1")
					{
						provextralegal=0;
					}
					else
					{
						provextralegal=acumuladoprovisiones *double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						provextralegal=Math.Round(provextralegal,0);
					}
					
					DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provextralegal+")");
				}
			}
		}
			
			
		protected void ingresardatos_vacacionescausadas(string codigoempleado,string nombres,string apellidos,int periodo,string fechainicial,string fechafinal,int causadas,int disfrutadas)
		{
			DataRow fila=DataTable1.NewRow();
			fila["CODIGO EMPLEADO"] =codigoempleado;
			fila["NOMBRE"]          =nombres+" "+apellidos;
			fila["PERIODO"]         =periodo;
			fila["FECHA INICIAL"]   =fechainicial;
			fila["FECHA FINAL"]     =fechafinal;
			fila["CAUSADAS"]        =causadas;
			fila["DISFRUTADAS"]     =disfrutadas;
			DataTable1.Rows.Add(fila);
			DATAGRIDVACACIONESCAUSADAS.DataSource=DataTable1;
			DATAGRIDVACACIONESCAUSADAS.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DATAGRIDVACACIONESCAUSADAS);
			DatasToControls.JustificacionGrilla(DATAGRIDVACACIONESCAUSADAS,DataTable1);
			for(int i=0;i<DataTable1.Rows.Count;i++)
			{
				if(Convert.ToDouble(DataTable1.Rows[i][6]) < 15)
					((Button)DATAGRIDVACACIONESCAUSADAS.Items[i].Cells[7].Controls[1]).Enabled = true;
			}
		}
		
		protected void ingresardatos_perescogido(string codigoempleado,string nombres,int periodo,string fechainicial,string fechafinal,int causadas,int disfrutadas)
		{
			DataRow fila=DataTable2.NewRow();
			fila["CODIGO EMPLEADO"]=codigoempleado;
			fila["NOMBRE"]=nombres;
			fila["PERIODO"]=periodo;
			fila["FECHA INICIAL"]=fechainicial;
			fila["FECHA FINAL"]=fechafinal;
			fila["CAUSADAS"]=causadas;
			fila["DISFRUTADAS"]=disfrutadas;
			DataTable2.Rows.Add(fila);
			DATAGRIDPERESCOGIDO.DataSource=DataTable2;
			DATAGRIDPERESCOGIDO.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DATAGRIDPERESCOGIDO);
			DatasToControls.JustificacionGrilla(DATAGRIDPERESCOGIDO,DataTable2);
		}
		
				
		protected void preparargrilla_vacacionescausadas()
		{
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("CODIGO EMPLEADO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("PERIODO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("FECHA INICIAL",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("FECHA FINAL",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("CAUSADAS",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("DISFRUTADAS",System.Type.GetType("System.Double")));		
			
		}
		
		protected void preparargrilla_perescogido()
		{
			DataTable2 = new DataTable();
			DataTable2.Columns.Add(new DataColumn("CODIGO EMPLEADO",System.Type.GetType("System.String")));
			DataTable2.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			DataTable2.Columns.Add(new DataColumn("PERIODO",System.Type.GetType("System.Double")));
			DataTable2.Columns.Add(new DataColumn("FECHA INICIAL",System.Type.GetType("System.String")));
			DataTable2.Columns.Add(new DataColumn("FECHA FINAL",System.Type.GetType("System.String")));
			DataTable2.Columns.Add(new DataColumn("CAUSADAS",System.Type.GetType("System.Double")));
			DataTable2.Columns.Add(new DataColumn("DISFRUTADAS",System.Type.GetType("System.Double")));		
			Session["DataTable2"]=DataTable2;
		}
		
		protected void cargar_ddl(object sender,DataGridItemEventArgs e)
		{
			if (e.Item.ItemType==ListItemType.Item || e.Item.ItemType==ListItemType.AlternatingItem)
			{
				DatasToControls param = new DatasToControls();
				param.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[7].FindControl("DDLANOINI")), "SELECT PANO_ANO              FROM PANO order by 1 desc");
                param.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[7].FindControl("DDLMESINI")), "Select PMES_MES, PMES_NOMBRE from PMES order by 1");
                param.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[7].FindControl("DDLDIAINI")), "Select TDIA_DIA, TDIA_NOMBRE from TDIA order by 1");
                param.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[8].FindControl("DDLANOFIN")), "SELECT PANO_ANO              FROM PANO order by 1 desc");
                param.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[8].FindControl("DDLMESFIN")), "Select PMES_MES, PMES_NOMBRE from PMES order by 1");
                param.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[8].FindControl("DDLDIAFIN")), "Select TDIA_DIA, TDIA_NOMBRE from TDIA order by 1");
                param.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[10].FindControl("DDLDIASEFECTIVOS")), "Select TDIA_DIA, TDIA_NOMBRE from TDIA order by 1");				
			}
		}
		
		protected void Page_Load(object sender , EventArgs e)
		{
			
			if (!IsPostBack)
			{

				DateTime diaActual = new DateTime();
				mesv = Int32.Parse(o_CNomina.CNOM_MES);
				anov = Int32.Parse(o_CNomina.CNOM_ANO);
				diav = diaActual.Day;

			DataSet vacausadas= new DataSet();
			int i;
			DatasToControls param = new DatasToControls();
										
				this.preparargrilla_vacacionescausadas();
				this.preparargrilla_perescogido();
                string query = "select M.mvac_periodo,M.memp_codiemp,M.mvac_diascaus,M.mvac_diasvacadisf,M.mvac_desde,M.mvac_hasta,N.mnit_nombres ||' '|| coalesce(N.mnit_nombre2,''),N.mnit_apellidos ||' '|| coalesce(N.mnit_apellido2,'') from dbxschema.mvacaciones M,dbxschema.mempleado E,dbxschema.mnit N where M.memp_codiemp=E.memp_codiempl and E.mnit_nit=N.mnit_nit AND MVAC_DIASCAUS - MVAC_DIASVACADISF > 0 and e.test_estado <> '4' order by memp_codiemp, M.mvac_periodo";
				DBFunctions.Request(vacausadas,IncludeSchema.NO,query);	
				for (i=0;i<vacausadas.Tables[0].Rows.Count;i++)
				{
					this.ingresardatos_vacacionescausadas(vacausadas.Tables[0].Rows[i][1].ToString(),vacausadas.Tables[0].Rows[i][6].ToString(),vacausadas.Tables[0].Rows[i][7].ToString(),int.Parse(vacausadas.Tables[0].Rows[i][0].ToString()),Convert.ToDateTime(vacausadas.Tables[0].Rows[i][4].ToString()).ToString("yyyy-MM-dd"),Convert.ToDateTime(vacausadas.Tables[0].Rows[i][5].ToString()).ToString("yyyy-MM-dd"),int.Parse(vacausadas.Tables[0].Rows[i][2].ToString()),int.Parse(vacausadas.Tables[0].Rows[i][3].ToString()));
				}
					
				Session["vacausadas"]=DataTable1;
				if (Request.QueryString["el"]=="1")
					Response.Write("<script language:javascript>alert('Vacaciones Liquidadas Satisfactoriamente');</script>");

			}
			else
			{
	
				if (Session["secuencia"]!=null)
				{
					secuencia=(string)Session["secuencia"];
				}
				
				if (Session["disponibles"]!=null)
				{
					disponibles=(int)Session["disponibles"];
				}
				if (Session["vacausadas"]!=null)
				{
					DataTable1=(DataTable)Session["vacausadas"];
				}
				if (Session["periodoescogido"]!=null)
				{
					DataTable2=(DataTable)Session["periodoescogido"];
				}
				if (Session["codigoempleado"]!=null)
				{
					codigoempleado=(string)Session["codigoempleado"];
				}
				if (Session["DataTable2"]!=null)
				{
					DataTable2=(DataTable)Session["DataTable2"];
				}
				if (Session["diasvacas2"]!=null)
				{
					diasvacas2=(int)Session["diasvacas2"];
				}
				if (Session["fechainicial"]!=null)
				{
					fechainicial=(string)Session["fechainicial"];
				}
				if (Session["fechafinal"]!=null)
				{
					fechafinal=(string)Session["fechafinal"];
				}
				if (Session["diasvacaciones"]!=null)
				{
					diasvacaciones=(int)Session["diasvacaciones"];
				}
				if (Session["codigoempleado"]!=null)
				{
					codigoempleado=(string)Session["codigoempleado"];
				}					

			}
			
		}
		
		protected void Seleccion_Periodo(object sender, DataGridCommandEventArgs e)
		{
			DataTable DataTable1 = new DataTable();
			DataTable1=(DataTable)Session["vacausadas"];
			DataTable2=(DataTable)Session["DataTable2"];
			if(Session["Adisponibles"]!=null)
				Adisponibles=(ArrayList)Session["Adisponibles"];
			
			
			string codigoempleado= DataTable1.Rows[e.Item.ItemIndex][0].ToString();
			if(DataTable2.Rows.Count>0)
			{
				string codigoempleado2=DataTable2.Rows[0][0].ToString();
				//El periodo tiene q ser del mismo empleado.
				if(codigoempleado!=codigoempleado2)
				{
					Response.Write("<script language:javascript>alert('PorFavor Escoja un periodo del mismo empleado.');</script>");
				}
				else
				{
					//averiguar si el empleado esta en vacaciones.
					string estado=DBFunctions.SingleData("select test_estado from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"'");
					if (int.Parse(estado)==1)
					{
						string nombre=      DataTable1.Rows[e.Item.ItemIndex][1].ToString();
						int periodo=        int.Parse(DataTable1.Rows[e.Item.ItemIndex][2].ToString());
						string fechainicial=DataTable1.Rows[e.Item.ItemIndex][3].ToString();
						string fechafinal=  DataTable1.Rows[e.Item.ItemIndex][4].ToString();
						int causadas=       int.Parse(DataTable1.Rows[e.Item.ItemIndex][5].ToString());
						int disfrutadas=    int.Parse(DataTable1.Rows[e.Item.ItemIndex][6].ToString());
						disponibles+=       causadas-disfrutadas;
						Adisponibles.Add(disponibles);
						this.ingresardatos_perescogido(codigoempleado,nombre,periodo,fechainicial,fechafinal,causadas,disfrutadas);
						Session["Adisponibles"]=Adisponibles;
						//sacar la secuencia de este periodo
						string secuencia=DBFunctions.SingleData("select mvac_secuencia from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"' and mvac_desde='"+fechainicial+"' and mvac_hasta='"+fechafinal+"' ");
						Session["periodoescogido"]=DataTable2;
						BTNINGRESAR.Visible=true;

						//DataRow[] docs=tablaNC.Select("PREFIJO='"+prefijo+"' AND NUMERO='"+numero+"'");
						//tablaNC.Clear();
						//tablaNC.AcceptChanges();

						//sacar los periodos del empleado 
						/*
						DataRow[] peris=DataTable1.Select("CODIGO EMPLEADO='""'");
						for(i=0;i<DataTable1.Rows.Count;i++)
						{
							if(DataTable1.Rows[i][0].ToString()!=codigoempleado)
							{
								DataTable1.Rows[i].Delete();
							}

						}
						*/
						DATAGRIDVACACIONESCAUSADAS.DataSource=DataTable1;
						DATAGRIDVACACIONESCAUSADAS.DataBind();
						Session["vacausadas"]=DataTable1;
					
				
					}	
				
					else
					{
						Response.Write("<script language:javascript>alert('El empleado escogido se encuentra en vacaciones.');</script>");			
					}
			
				}
			
			}
			else
			{
				//averiguar si el empleado esta en vacaciones.
				string estado=DBFunctions.SingleData("select test_estado from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"'");
				if (int.Parse(estado)==1)
				{
					string nombre=DataTable1.Rows[e.Item.ItemIndex][1].ToString();
					int periodo=int.Parse(DataTable1.Rows[e.Item.ItemIndex][2].ToString());
					string fechainicial=DataTable1.Rows[e.Item.ItemIndex][3].ToString();
					string fechafinal=DataTable1.Rows[e.Item.ItemIndex][4].ToString();
					int causadas=int.Parse(DataTable1.Rows[e.Item.ItemIndex][5].ToString());
					int disfrutadas=int.Parse(DataTable1.Rows[e.Item.ItemIndex][6].ToString());
					disponibles+=causadas-disfrutadas;
					Adisponibles.Add(disponibles);
					this.ingresardatos_perescogido(codigoempleado,nombre,periodo,fechainicial,fechafinal,causadas,disfrutadas);
					Session["Adisponibles"]=Adisponibles;
					//sacar la secuencia de este periodo
					string secuencia=DBFunctions.SingleData("select mvac_secuencia from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"' and mvac_desde='"+fechainicial+"' and mvac_hasta='"+fechafinal+"' ");
					Session["periodoescogido"]=DataTable2;
					BTNINGRESAR.Visible=true;

					//DataRow[] docs=tablaNC.Select("PREFIJO='"+prefijo+"' AND NUMERO='"+numero+"'");
					//tablaNC.Clear();
					//tablaNC.AcceptChanges();

					//sacar los periodos del empleado 
					/*
						DataRow[] peris=DataTable1.Select("CODIGO EMPLEADO='""'");
						for(i=0;i<DataTable1.Rows.Count;i++)
						{
							if(DataTable1.Rows[i][0].ToString()!=codigoempleado)
							{
								DataTable1.Rows[i].Delete();
							}

						}
						*/
					DATAGRIDVACACIONESCAUSADAS.DataSource=DataTable1;
					DATAGRIDVACACIONESCAUSADAS.DataBind();
					Session["vacausadas"]=DataTable1;
					
				
				}	
				
				else
				{
					Response.Write("<script language:javascript>alert('El empleado escogido se encuentra en vacaciones.');</script>");			
				}		

			}
			for(int y=0;y<DataTable1.Rows.Count;y++)
			{
				if(Convert.ToDouble(DataTable1.Rows[y][6]) < 15)
					((Button)DATAGRIDVACACIONESCAUSADAS.Items[y].Cells[7].Controls[1]).Enabled = true;
			}
            DATAGRIDVACACIONESCAUSADAS.Visible = false;
        }
//ncodigo para montar , por rAFAEL ANGEL
		protected void ingresar_vacaciones_new(object sender, EventArgs e)
		{
			

			this.preparargrilla_empleadoliquidacion();
			for (int i=0;i<DATAGRIDPERESCOGIDO.Items.Count;i++)
			{
				//armar la primera fecha
				string ANOINICIAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[7].FindControl("DDLANOINI")).SelectedValue;
				string MESINICIAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[7].FindControl("DDLMESINI")).SelectedValue;
				string DIAINICIAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[7].FindControl("DDLDIAINI")).SelectedValue;
				fechainicial=ANOINICIAL+"-"+MESINICIAL+"-"+DIAINICIAL;

				//armar la segunda fecha
				string ANOFINAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLANOFIN")).SelectedValue;
				string MESFINAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLMESFIN")).SelectedValue;
				string DIAFINAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLDIAFIN")).SelectedValue;
				string fechafinal=ANOFINAL+"-"+MESFINAL+"-"+DIAFINAL;

				Empleado o_Empleado = new Empleado("='" + DATAGRIDPERESCOGIDO.Items[i].Cells[0].Text + "'");
				o_Empleado.p_DDLANORETI = (DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLANOFIN");
				o_Empleado.p_DDLDIARETI = (DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLDIAFIN");
				o_Empleado.p_DDLMESRETI = (DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLMESFIN");
			
				anov =Int32.Parse(((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLANOFIN")).SelectedValue);
				diav = Int32.Parse(((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLDIAFIN")).SelectedValue);
				mesv = Int32.Parse(((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLMESFIN")).SelectedValue);
			

				o_Empleado.AsignarParametrosNomina = o_CNomina;
				o_Empleado.AsignarVarios = o_Varios;
				o_Empleado.AsignarDataGrid = this.DataGrid2;
				o_Empleado.AsignarDataTable = DataTable1;
				o_Empleado.PromedioUltimoAno(anov,anov,diav,mesv);
				o_Empleado.LiquidarMesEnCurso(mesv,anov,diav,true,"Vacaciones");
				o_Empleado.LiquidacionVacaciones(diav,mesv,anov);

				LBLCEDULA.Text = o_Empleado.p_MEMP_CODIEMPL;
				LBLEMPLEADO.Text = o_Empleado.p_MNIT_APELLIDOS.Trim() + " " + o_Empleado.p_MNIT_APELLIDO2.Trim() + " " + o_Empleado.p_MNIT_NOMBRES.Trim() + " " + o_Empleado.p_MNIT_NOMBRE2.Trim();
				LBDTVACACIONES.Text = diav.ToString();
				LBVACAAPAGAR.Text = o_Empleado.p_VVacaciones.ToString("C");	
				LBPERIODO.Text = fechainicial + " - " + fechafinal;
				LBDIASEFECTIVOS.Text = o_Empleado.p_FactorVacaciones.ToString();
				Panel1.Visible=true;
				LBPERIODO.Visible=true;
				DataGrid2.Visible=true;
			}
		}

		
		protected void ingresar_vacaciones(object sender, EventArgs e)
		{
            codquincena = DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI, CNOMINA wHERE MQUI.mqui_anoquin=cnom_ano and MQUI.mqui_mesquin=cnom_mes and MQUI.mqui_tpernomi=cnom_quincena ");
            Session["codquincena"] = codquincena;       
            int i = 0, diaspervacaciones = 0, diasvacaciones = 0;
			int p;
			ArrayList secuencias = new ArrayList();
			ArrayList FechaVacIni= new ArrayList();
			ArrayList FechaVacFin= new ArrayList();
			ArrayList Adiasvacaciones = new ArrayList();
			ArrayList Adiasvacas2 = new ArrayList();
			
			string secuencia;
			//cargo el datatable con la variable de session que contiene la segunda grilla generada.
			DataTable2=(DataTable)Session["periodoescogido"];
			Adisponibles=(ArrayList)Session["Adisponibles"];
			TimeSpan diasvacacionescalendario = new TimeSpan();
			//diasvacaciones=0;
			
			for (i=0;i<DATAGRIDPERESCOGIDO.Items.Count;i++)
			{
				//sacar la secuencia de este periodo
				secuencia=DBFunctions.SingleData("select mvac_secuencia from dbxschema.mvacaciones where memp_codiemp='"+DATAGRIDPERESCOGIDO.Items[i].Cells[0].Text+"' and mvac_desde='"+DATAGRIDPERESCOGIDO.Items[i].Cells[3].Text+"' and mvac_hasta='"+DATAGRIDPERESCOGIDO.Items[i].Cells[4].Text+"' ");
				secuencias.Add(secuencia);	
				//armar la primera fecha
				string ANOINICIAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[7].FindControl("DDLANOINI")).SelectedValue;
				string MESINICIAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[7].FindControl("DDLMESINI")).SelectedValue;
				string DIAINICIAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[7].FindControl("DDLDIAINI")).SelectedValue;
				fechainicial=ANOINICIAL+"-"+MESINICIAL+"-"+DIAINICIAL;
				FechaVacIni.Add(fechainicial);
				DateTime fechaini=Convert.ToDateTime(fechainicial);
			

				//armar la segunda fecha
				string ANOFINAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLANOFIN")).SelectedValue;
				string MESFINAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLMESFIN")).SelectedValue;
				string DIAFINAL=((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[8].FindControl("DDLDIAFIN")).SelectedValue;
				string fechafinal=ANOFINAL+"-"+MESFINAL+"-"+DIAFINAL;
				FechaVacFin.Add(fechafinal);
				DateTime fechafin=Convert.ToDateTime(fechafinal);
			
				//restar las dos fechas
				diasvacacionescalendario=fechafin-fechaini;
				Adiasvacaciones.Add(int.Parse(((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[10].FindControl("DDLDIASEFECTIVOS")).SelectedValue));
				diasvacaciones+=int.Parse(((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[10].FindControl("DDLDIASEFECTIVOS")).SelectedValue);
				diaspervacaciones=int.Parse(((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[10].FindControl("DDLDIASEFECTIVOS")).SelectedValue);
				diasvacas = int.Parse(diasvacacionescalendario.Days.ToString());
				//diasvacas2+=diasvacaciones;
				diasvacas2=diaspervacaciones;
				Adiasvacas2.Add(diaspervacaciones);
				disponibles=Convert.ToInt32(Adisponibles[Adisponibles.Count - 1]);
			
				//validar fechas,la fecha de inicio de vacaciones no puede ser menor a la fecha final
				//del periodo liquidado.

				if (int.Parse(((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[10].FindControl("DDLDIASEFECTIVOS")).SelectedValue) >disponibles)
				{
					Response.Write("<script language:java>alert('Estimado Usuario: Sólo tiene "+disponibles+" días disponibles, recalcule los dias efectivos por favor. ');</script>");
				}
				else   //permitir que se liquiden mas dias de los disponibles
				{
					//la fecha final de vacaciones no puede ser menor a la inicial
					if (fechafin<fechaini)
					{
						Response.Write("<script language:java>alert('Fechas Incorrectas,la fecha final del periodo de vacaciones es menor a la fecha inicial.. por favor verifique ');</script> ");
					}
					else
					{
						
						disponibles-=diaspervacaciones;
						
						//ingresar a base de datos
						//validar si las toma en tiempo o en dinero
						  
						if (((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[9].FindControl("DDLFORMA")).SelectedValue=="EN TIEMPO")
						{
							Response.Write("<script language:java>alert('Se ingreso un periodo de vacaciones por "+diasvacaciones+" dias,pagados en tiempo.');</script> ");
							//DBFunctions.NonQuery("insert into dbxschema.dvacaciones values ('"+fechainicial+"','"+fechafinal+"',"+disponibles+","+secuencia+","+diaspervacaciones+",0 )");
						}
						
						if (((DropDownList)DATAGRIDPERESCOGIDO.Items[i].Cells[9].FindControl("DDLFORMA")).SelectedValue=="EN DINERO")
						{
								//JFSC 20052008 se debe adicionar un dia al cálculo
							diasvacas++;
							//DBFunctions.NonQuery("insert into dbxschema.dvacaciones values ('"+fechainicial+"','"+fechafinal+"',"+disponibles+","+secuencia+",0,"+diaspervacaciones+" )");
							Response.Write("<script language:java>alert('Se ingreso un periodo de vacaciones por "+diasvacas+" dias,pagados en dinero.');</script> ");
						}
						
						 
						//Cargar datos a la session para posterior actualizacion
						Session["DataTable2"]=DataTable2;
						Session["diasvacas2"]=diasvacas2;
						Session["secuencia"]=secuencia;
						
						Session["fechainicial"]=fechainicial;
						Session["fechafinal"]=fechafinal;
						Session["diasvacaciones"]=diasvacaciones;
						Session["Adiasvacaciones"]=Adiasvacaciones;
						Session["Adiasvacas2"]=Adiasvacas2;

						Session["FechaVacIni"]=FechaVacIni;
						Session["FechaVacFin"]=FechaVacFin;
						
						LBLCEDULA.Text = DATAGRIDPERESCOGIDO.Items[i].Cells[0].Text;
						LBLEMPLEADO.Text = DATAGRIDPERESCOGIDO.Items[i].Cells[1].Text;
						//actualizar maestro de vacaciones
						//DBFunctions.NonQuery("update mvacaciones set mvac_diasvacadisf =mvac_diasvacadisf+"+diasvacas2+" where mvac_secuencia="+secuencia+" ");
							
						//Response.Redirect(mainpage+"?process=Nomina.LiquidacionVacaciones");
					}
                    this.PagoLiquidacionVacaciones(DataTable2.Rows[0][0].ToString(), ((DropDownList)DATAGRIDPERESCOGIDO.Items[0].Cells[10].FindControl("DDLDIASEFECTIVOS")).SelectedValue, DataTable2.Rows[0][5].ToString());
                }

				/*
				if (Convert.ToDateTime(DataTable2.Rows[i][4].ToString())>=fechaini)
				{
					Response.Write("<script language:java>alert('Estimado Usuario la fecha escogida como inicio del periodo de vacaciones  "+fechaini+" no es valida,seleccione las fechas adecuadas ');</script> ");
				}
				else
				{
					
				
			
				}
			    */
			Session["secuencias"]=secuencias;
			}
			

			//for(p=0;p<secuencias.Count;p++)
			//{
			//	//Response.Write("<script language:java>alert(' secuencia "+secuencias[p].ToString()+"   ');</script> ");
			//}

			
			//validar tiempo dado de vacaciones, no puede superar a los dias que tiene permitidos.
		}
		
		
		
		
		
		
		
		//protected HtmlInputFile fDocument;
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		private void InitializeComponent()
		{    
			this.DATAGRIDPERESCOGIDO.SelectedIndexChanged += new System.EventHandler(this.DATAGRIDPERESCOGIDO_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button2_Click(object sender, System.EventArgs e)
		{
		
		}

		private void DATAGRIDPERESCOGIDO_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
	
		
		
		
		
	}
	
	
	
}
