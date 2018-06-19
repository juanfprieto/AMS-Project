using System;
using System.IO;
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
using System.Web.UI;
using System.Web.Mail;
using System.Text;
using AMS.Tools;


namespace AMS.Nomina
{

	public class PrimaNormal : System.Web.UI.UserControl 
	{
		protected string table;
		protected Label LBMESFINAL;
		protected DropDownList DDLMESINICIAL,DDLANO,DDLTIPOPRIMA;
		protected DataTable DataTable1;
		protected DataGrid DATAGRIDPRIMA;
		protected Button BTNLIQUIDARDEFINITIVAMENTE,BTNLIQUIDAR;
		protected Label lbfechainicio,lbfechafinal,LBPRUEBAS;
		string fechainicio,fechafinal;
        double promedio, diasT, valorliquidacione, promedioPrima;
		double ValDescuentos = 0;
        int mesinicial, mesfinal, anoAnter;
		protected System.Web.UI.WebControls.Label LBTOTALPRIMA;
		protected System.Web.UI.WebControls.Button Button2;
		protected System.Web.UI.WebControls.TextBox tbEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.ImageButton ibMail;
		protected System.Web.UI.WebControls.PlaceHolder toolsHolder;
		protected System.Web.UI.WebControls.PlaceHolder phGrilla;
		string mainpage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected Label lbInfo;
       

		
		protected void LiquidacionPrimas( object Sender,EventArgs e)
		{
			int i,j;
			
			double diastrabajados=0,valorliquidacion=0,valortotal=0;

			int divisor=0;
			this.preparargrilla_prima();
			DataSet empleadosprima= new DataSet();
			this.armarfechaliqcesantias();

            if (DDLTIPOPRIMA.SelectedValue == "2")  // prima ajusta NO trae empleados salario fijo (tipo 3)
                DBFunctions.Request(empleadosprima, IncludeSchema.NO, "select empl.memp_codiempl, mnit.mnit_apellidos concat ' ' concat coalesce (mnit.mnit_apellido2,''), mnit.mnit_nombres concat ' ' concat coalesce(mnit.mnit_nombre2,''), empl.tsal_salario, EMPL.tcon_contrato, EMPL.MEMP_SUELACTU, EMPL.test_estado, EMPL.memp_fechingreso from dbxschema.mempleado EMPL, dbxschema.MNIT mnit where (tsal_salario='2'                    ) and (test_estado='1' or test_estado='5') and tcon_contrato <> '3' and empl.mnit_nit=mnit.mnit_nit order by empl.memp_codiempl");
            else
                DBFunctions.Request(empleadosprima, IncludeSchema.NO, "select empl.memp_codiempl, mnit.mnit_apellidos concat ' ' concat coalesce (mnit.mnit_apellido2,''), mnit.mnit_nombres concat ' ' concat coalesce(mnit.mnit_nombre2,''), empl.tsal_salario, EMPL.tcon_contrato, EMPL.MEMP_SUELACTU, EMPL.test_estado, EMPL.memp_fechingreso from dbxschema.mempleado EMPL, dbxschema.MNIT mnit where (tsal_salario='2' or tsal_salario='3') and (test_estado='1' or test_estado='5') and tcon_contrato <> '3' and empl.mnit_nit=mnit.mnit_nit order by empl.memp_codiempl");

            //  DBFunctions.Request(empleadosprima, IncludeSchema.NO, "select empl.memp_codiempl,  mnit.mnit_apellidos concat ' ' concat coalesce (mnit.mnit_apellido2,''), mnit.mnit_nombres concat ' ' concat coalesce(mnit.mnit_nombre2,''), empl.tsal_salario, EMPL.tcon_contrato, EMPL.MEMP_SUELACTU, EMPL.test_estado, EMPL.memp_fechingreso from dbxschema.mempleado EMPL, dbxschema.MNIT mnit where (tsal_salario='2' or tsal_salario='3') and (test_estado='1' or test_estado='5') and tcon_contrato <> '3' and empl.mnit_nit=mnit.mnit_nit and empl.memp_codiempl = '52235475' order by empl.memp_codiempl");

            int liqsino = int.Parse(DBFunctions.SingleData("select count( mpri_secuencia) from dbxschema.mprimas where mpri_fechinic='"+fechainicio+"'  and mpri_fechfina='"+fechafinal+"' "));
			double topeSubsidioTransporte = Convert.ToDouble(DBFunctions.SingleData("select CNOM_SALAMINIACTU*2 from cnomina").ToString());
			DataSet conceptosS = new DataSet();
			DBFunctions.Request(conceptosS,IncludeSchema.NO,"select cnom_concsalacodi, cnom_concsalaint, cnom_substransactu, cnom_topepagoauxitran*cnom_salaminiactu, cnom_concvacacodi, cnom_substransactu from dbxschema.cnomina");
			
			if (DDLTIPOPRIMA.SelectedValue=="1" && liqsino>0)
			{
                Utils.MostrarAlerta(Response, "El periodo de liquidación de Primas escogido ya fue liquidado!, por favor revise los periodos de liquidación");
			}
			else if (DDLTIPOPRIMA.SelectedValue=="2" && liqsino>1)	
			    {
                    Utils.MostrarAlerta(Response, "El periodo de liquidación de Prima Ajustada escogido ya fue liquidado!, por favor revise los periodos de liquidación");
			    }
				 else if (DDLTIPOPRIMA.SelectedValue=="2" && liqsino<1)	
			         {
                        Utils.MostrarAlerta(Response, "El período de liquidación de Prima Ajustada escogido no se puede liquidar, antes debe liquidar la prima de forma normal.porfavor revise los períodos de liquidación");
			         }
					else
					{
						for (i=0;i<empleadosprima.Tables[0].Rows.Count;i++)
						{
							bool banderaF=false;
							string quinomens= DBFunctions.SingleData("select memp_peripago from dbxschema.mempleado A, dbxschema.tperipago B where A.memp_peripago=B.tper_peri and memp_codiempl='"+empleadosprima.Tables[0].Rows[i][0].ToString()+"'");			        
					//Response.Write("<script language:java>alert('inicio for de empleado: "+empleadosprima.Tables[0].Rows[i][0].ToString()+" ');</script>");			
                
					
					promedio=0;
					DataSet primas= new DataSet();
					DataSet resumenprimas= new DataSet();
					DataSet primerpago= new DataSet();
                 	
                        if (DDLMESINICIAL.SelectedValue=="Enero")
                        {
                            mesinicial=1;
                            mesfinal=6;
                            anoAnter = Convert.ToInt16(DDLANO.SelectedValue.ToString()) - 1 ;
                         }
				
                        if (DDLMESINICIAL.SelectedValue=="Julio")
                        {
                            mesinicial=7;
                            mesfinal=12;
                            anoAnter = Convert.ToInt16(DDLANO.SelectedValue.ToString());
                        }

                        DBFunctions.Request(primas,        IncludeSchema.NO, "SELECT coalesce(sum(DQUI.dqui_apagar),0),MQUI.mqui_mesquin FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI,dbxschema.tmes T WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin  AND DQUI.pcon_concepto=PCON.pcon_concepto  AND PCON.tres_afecprima='S'  AND MQUI.mqui_anoquin="+ DDLANO.SelectedValue +" AND MQUI.mqui_mesquin BETWEEN " + mesinicial + " AND " + mesfinal + " AND T.tmes_mes=mqui.mqui_mesquin AND DQUI.memp_codiempl='" + empleadosprima.Tables[0].Rows[i][0].ToString() + "' group by mqui.mqui_mesquin  ");
                        DBFunctions.Request(resumenprimas, IncludeSchema.NO, "SELECT coalesce(sum(DQUI.dqui_apagar),0),MQUI.mqui_mesquin,MQUI.mqui_codiquin FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI,dbxschema.tmes T  WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin AND DQUI.pcon_concepto=PCON.pcon_concepto  AND PCON.tres_afecprima='S'   AND MQUI.mqui_anoquin="+ DDLANO.SelectedValue +" AND MQUI.mqui_mesquin BETWEEN " + mesinicial + " AND " + mesfinal + " AND T.tmes_mes=mqui.mqui_mesquin AND DQUI.memp_codiempl='" + empleadosprima.Tables[0].Rows[i][0].ToString() + "'  group by mqui.mqui_mesquin,MQUI.mqui_codiquin");
                        string sqlpromedioPrima = "";
                        if (empleadosprima.Tables[0].Rows[i][3].ToString() == "3")  // salario fijo se toma el sueldo basico + transporte si aplica
                        {
                            promedioPrima = Convert.ToDouble(empleadosprima.Tables[0].Rows[i][5].ToString());
                            if (promedioPrima <= topeSubsidioTransporte)
                                promedioPrima += Convert.ToDouble(conceptosS.Tables[0].Rows[0][5].ToString());
                            
                        }
                        else
                        {
                            sqlpromedioPrima =
                           "select sum(a.pagos) from ( " +
                            "Select coalesce(sum(mpago_valrtotl),0) as pagos " +
                            " from mpagosfueranomina mpfn,dbxschema.pconceptonomina PCON " +
                            " WHERE mpfn.pcon_concepto=PCON.pcon_concepto " +
                            " AND PCON.tres_afecprima='S'  AND year(Mpfn.mpago_fecha)=" + DDLANO.SelectedValue + " AND Month(Mpfn.mpago_fecha) " +
                            " BETWEEN " + mesinicial + " AND " + mesfinal + " AND mpfn.memp_codiempl='" + empleadosprima.Tables[0].Rows[i][0].ToString() + "' " +
                            "union " +
                            "SELECT coalesce(sum(DQUI.dqui_apagar - DQUI.dqui_adescontar),0) as pagos " +
                            " FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI,dbxschema.tmes T " +
                            " WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin  AND DQUI.pcon_concepto=PCON.pcon_concepto AND PCON.tres_afecprima='S' " +
                            " AND (MQUI.mqui_anoquin=" + DDLANO.SelectedValue + " AND MQUI.mqui_mesquin BETWEEN " + mesinicial + " AND " + mesfinal + ") " +
                           //"  OR (MQUI.mqui_anoquin=" + anoAnter + " AND MQUI.mqui_mesquin = " + mesfinal + " and mqui_tperNomi = 1) " +
                           // "  OR (MQUI.mqui_anoquin=" + anoAnter + " AND MQUI.mqui_mesquin = " + mesAnter + " and mqui_tperNomi = 2)) " +
                              " AND T.tmes_mes=mqui.mqui_mesquin AND DQUI.memp_codiempl='" + empleadosprima.Tables[0].Rows[i][0].ToString() + "' " +
                            //"union " +
                            //"SELECT coalesce(cnom_substransactu,0) as pagos FROM dbxschema.cnomina,dbxschema.MEMPLEADO MEMP " +
                            //" WHERE MEMP.memp_codiempl='" + empleadosprima.Tables[0].Rows[i][0].ToString() + "' AND CNOM_SUBSTRANPERINOMI = 2 AND MEMP.MEMP_SUELACTU <= CNOM_SALAMINIACTU*2 AND CNOM_QUINCENA = 2 "+
                            ") as a; ";

                            promedioPrima = Convert.ToDouble(DBFunctions.SingleData(sqlpromedioPrima));
                            promedio = promedioPrima;
                            divisor = int.Parse(DBFunctions.SingleData("select coalesce(count(mqui_codiquin),0) from dbxschema.mquincenas where mqui_estado=2 AND mqui_anoquin=" + DDLANO.SelectedValue + " and mqui_mesquin between " + mesinicial + " AND " + mesfinal + " "));
                        }
                 	//diasNOtrabajados
					diastrabajados=0; 
					diastrabajados = ((Convert.ToDateTime(fechafinal).Year*360)
									+(Convert.ToDateTime(fechafinal).Month*30)
									+Convert.ToDateTime(fechafinal).Day)
									-((Convert.ToDateTime(empleadosprima.Tables[0].Rows[i][7]).Year*360)
									+(Convert.ToDateTime(empleadosprima.Tables[0].Rows[i][7]).Month*30)
									+(Convert.ToDateTime(empleadosprima.Tables[0].Rows[i][7]).Day))
									+1;
					 
							
					if (diastrabajados >= 180)
							diastrabajados = 180;

                    //if ((empleadosprima.Tables[0].Rows[i][3].ToString() == "2" && DDLTIPOPRIMA.SelectedValue == "2") || ((empleadosprima.Tables[0].Rows[i][3].ToString() == "3" || empleadosprima.Tables[0].Rows[i][3].ToString() == "2") && DDLTIPOPRIMA.SelectedValue == "1"))
                    if ((empleadosprima.Tables[0].Rows[i][3].ToString() == "2" && DDLTIPOPRIMA.SelectedValue == "2") )  // Lo realmente pagado en le semtre real
                        //             tipo de contrato
					{
						int    ano_actual   = Convert.ToInt16(DDLANO.SelectedValue);
						String cod_emple    = empleadosprima.Tables[0].Rows[i][0].ToString();
						string promedioDevengado = DBFunctions.SingleData
                           ("select sum(a.pagos) from (Select coalesce(SUM(DQ.DQUI_APAGAR - DQUI_ADESCONTAR),0) as pagos from DBXSCHEMA.DQUINCENA DQ,DBXSCHEMA.MQUINCENAS MQ, DBXSCHEMA.PCONCEPTONOMINA PCON where DQ.memp_codiempl='"+cod_emple+"'AND DQ.MQUI_CODIQUIN = MQ.MQUI_CODIQUIN AND MQ.mqui_anoquin = "+ano_actual+" and MQ.mqui_mesquin >="+mesinicial+" and MQ.mqui_mesquin <="+mesfinal+" AND DQ.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND PCON.TRES_AFECPRIMA!='N' "+
                           " union "+
                           "  Select coalesce(sum(mpago_valrtotl),0) as pagos from mpagosfueranomina mpfn,dbxschema.pconceptonomina PCON "+
                            " WHERE mpfn.pcon_concepto=PCON.pcon_concepto "+
                            " AND PCON.tres_afecprima='S'  AND year(Mpfn.mpago_fecha)=" + DDLANO.SelectedValue + " AND Month(Mpfn.mpago_fecha) "+
                            " BETWEEN " + mesinicial + " AND " + mesfinal + " AND mpfn.memp_codiempl='" + empleadosprima.Tables[0].Rows[i][0].ToString() + "' "+
                            ") as a; ");
                        string transporteCausado = DBFunctions.SingleData("SELECT coalesce(cnom_substransactu*0.5,0) as pagos FROM dbxschema.cnomina,dbxschema.MEMPLEADO MEMP WHERE MEMP.memp_codiempl='" + empleadosprima.Tables[0].Rows[i][0].ToString() + "' AND CNOM_SUBSTRANPERINOMI = 2 AND MEMP.MEMP_SUELACTU <= CNOM_SALAMINIACTU*2 AND CNOM_QUINCENA = 2 " ) ;
                        if (promedioDevengado == "")
                            promedioDevengado = "0.00";
                        if (transporteCausado == "")  // corresponde a calcular el valor causado de subsidio de transporte en la primera qna de dic para que promedie exactamente sobre todo lo devengado
                            transporteCausado = "0.00";
                        string concSalario  = Convert.ToString(conceptosS.Tables[0].Rows[0][0]);
						string concVacaciones = Convert.ToString(conceptosS.Tables[0].Rows[0][4]);
						string diasTrabajadosPromedio = DBFunctions.SingleData("Select coalesce(sum(dqui_canteventos),0) from DBXSCHEMA.DQUINCENA DQ,DBXSCHEMA.MQUINCENAS MQ, DBXSCHEMA.PCONCEPTONOMINA PCON where dq.pcon_concepto in ('"+concSalario+"','"+concVacaciones+"') and DQ.memp_codiempl='"+cod_emple+"' AND DQ.MQUI_CODIQUIN = MQ.MQUI_CODIQUIN AND MQ.mqui_anoquin = "+ano_actual+" and MQ.mqui_mesquin >="+mesinicial+" and MQ.mqui_mesquin <="+mesfinal+" AND DQ.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND PCON.TRES_AFECPRIMA!='N'");
					
						diasT    = Convert.ToDouble(diasTrabajadosPromedio);

                        promedio = Convert.ToDouble(promedioDevengado); 
                        if ((promedio/diasT)*30 <= topeSubsidioTransporte)
                            promedio += Convert.ToDouble(transporteCausado);
						
						valorliquidacion = Math.Round(promedio/diasT*30,0);
				 		valorliquidacion = (((valorliquidacion/2)*diastrabajados)/180);
						valorliquidacione=valorliquidacion;

						banderaF = true;
						//this.ingresardatos_prima(empleadosprima.Tables[0].Rows[i][0].ToString(),valorliquidacion,diasT,empleadosprima.Tables[0].Rows[i][1].ToString(),empleadosprima.Tables[0].Rows[i][2].ToString(),promedio,double.Parse(empleadosprima.Tables[0].Rows[i][5].ToString()),ValDescuentos);
						
						
						/*suma=0;					
						//si el empleado tiene sueldo variable
						for (j=0;j<primas.Tables[0].Rows.Count;j++)
							{
								suma+= double.Parse(primas.Tables[0].Rows[j][0].ToString());
							}
						if (empleadosprima.Tables[0].Rows[i][3].ToString()=="2" && DDLTIPOPRIMA.SelectedValue=="1")
							
							promedio= suma/(diastrabajados-30)*30;
						else promedio= suma/diastrabajados*30;*/
					}
					else
					{
                        if ((empleadosprima.Tables[0].Rows[i][3].ToString() == "3" || empleadosprima.Tables[0].Rows[i][3].ToString() == "2") && DDLTIPOPRIMA.SelectedValue == "1")
						{
							//si el empleado tiene sueldo no integral
							//sacar los 3 ultimos pagos mensuales
							 
						//	promedio=Convert.ToDouble(empleadosprima.Tables[0].Rows[i][5]);
						//	if (promedio <= Convert.ToDouble(conceptosS.Tables[0].Rows[0][3])) 
						//		promedio=promedio+Convert.ToDouble(conceptosS.Tables[0].Rows[0][2]);
                            if (divisor == 0)
                                promedio = 0;
                            else
                                promedio = Math.Round(promedioPrima / divisor,0)*2;

                            promedio = Math.Round(promedioPrima,0);

                            if (empleadosprima.Tables[0].Rows[i][3].ToString() == "2")
                                promedioPrima = Math.Round (promedioPrima / diastrabajados * 30,0);  // Obtiene el salario promedio mes
                            
                            valorliquidacion = (promedioPrima / 180) * diastrabajados;
							valorliquidacion = (((valorliquidacion/2) * diastrabajados)/180);
							valorliquidacione=valorliquidacion;
						}
					}
							
					//sacar los dias trabajados
					

                
					//AJUSTADA=2 SACO EL VALOR QUE LE PAGUE LA PRIMERA VEZ Y SACO LA DIFERENCIA.ESTO ES LO QUE SE LE DEBE.
					if (DDLTIPOPRIMA.SelectedValue=="2")
						{	
							DBFunctions.Request(primerpago,IncludeSchema.NO,"select coalesce(dpri_valorprima,0),coalesce(dpri_diastrab,0) from dbxschema.dprimas DPRI, dbxschema.mprimas MPRI where DPRI.dpri_secuencia=MPRI.mpri_secuencia and mpri.mpri_fechinic='"+fechainicio+"' and mpri.mpri_fechfina='"+fechafinal+"'  and dpri.dpri_codiemp='"+empleadosprima.Tables[0].Rows[i][0].ToString()+"' " );
							//LBPRUEBAS.Text+=primerpago.Tables[0].Rows.Count.ToString();
			   	//H			valorliquidacion=(promedio*diastrabajados)/360;
							if (primerpago.Tables.Count > 0)
							{
								if (primerpago.Tables[0].Rows.Count > 0)
								{
									valorliquidacion = valorliquidacion - double.Parse(primerpago.Tables[0].Rows[0][0].ToString());
									diastrabajados-=int.Parse(primerpago.Tables[0].Rows[0][1].ToString());
									valorliquidacione = valorliquidacion;								
								}
							}
						}
					else
						{
							valorliquidacion=(promedioPrima*diastrabajados)/360;
						}
// DESCUENTOS PERMANENTES AL EMPLEADO EN VACACIONES
//						if (empleadosprima.Tables[0].Rows[i][6].ToString()=="5")
//						{
//							string descuento = "";
//							if (DDLMESINICIAL.SelectedValue=="Enero")
//							{
//								descuento =  DBFunctions.SingleData("Select coalesce(SUM(MPAG_VALOR),0) from DBXSCHEMA.MPAGOSYDTOSPER where memp_codiempl='" + empleadosprima.Tables[0].Rows[i][0].ToString() + "' and ((mpag_fechfinmes >= 6 and mpag_fechfinano=" + DDLANO.SelectedValue + ") or mpag_fechfinano>" + DDLANO.SelectedValue + ")");
//							}
//							else
//							{
//								descuento =  DBFunctions.SingleData("Select coalesce(SUM(MPAG_VALOR),0) from DBXSCHEMA.MPAGOSYDTOSPER where memp_codiempl='" + empleadosprima.Tables[0].Rows[i][0].ToString() + "' and ((mpag_fechfinmes = 12 and mpag_fechfinano=" + DDLANO.SelectedValue + ") or mpag_fechfinano>" + DDLANO.SelectedValue + ")");
//							}
//							ValDescuentos =  Double.Parse(descuento);
//						}
//			
//						valorliquidacion = valorliquidacion - ValDescuentos;
			
						if (primas.Tables[0].Rows.Count>0)
						{
							//Response.Write("<script language:java>alert('ingreso datos: "+empleadosprima.Tables[0].Rows[i][0].ToString()+" ');</script>");									
							//si trabajo 90 dias tiene derecho a prima, solo hacemos esta validacion cuando es prima normal
							if (DDLTIPOPRIMA.SelectedValue=="1")
							{
								//en caso de que sea indefinido el sueldo
								if (empleadosprima.Tables[0].Rows[i][4].ToString()=="3")
								{
									//Response.Write("<script language:java>alert('dias trabajados: "+diastrabajados+" ');</script>");									
									//if (diastrabajados>=90)
									//{
									//Response.Write("<script language:java>alert(' mayor de 90 dias, y sueldo indefinido');</script>");
									//this.ingresardatos_prima(empleadosprima.Tables[0].Rows[i][0].ToString(),valorliquidacion,diastrabajados,empleadosprima.Tables[0].Rows[i][1].ToString(),empleadosprima.Tables[0].Rows[i][2].ToString(),promedio,double.Parse(empleadosprima.Tables[0].Rows[i][5].ToString()));
									//}
								
								}
								else
								{
									if(banderaF)
                                    {
									    this.ingresardatos_prima(empleadosprima.Tables[0].Rows[i][0].ToString(),valorliquidacione,diastrabajados,empleadosprima.Tables[0].Rows[i][1].ToString(),empleadosprima.Tables[0].Rows[i][2].ToString(),promedio,double.Parse(empleadosprima.Tables[0].Rows[i][5].ToString()),ValDescuentos);
									}
                                    else 
                                        this.ingresardatos_prima(empleadosprima.Tables[0].Rows[i][0].ToString(),valorliquidacion,diastrabajados,empleadosprima.Tables[0].Rows[i][1].ToString(),empleadosprima.Tables[0].Rows[i][2].ToString(),promedio,double.Parse(empleadosprima.Tables[0].Rows[i][5].ToString()),ValDescuentos);
								}
							}
							else 
							{	if(banderaF)
                                {
								    this.ingresardatos_prima(empleadosprima.Tables[0].Rows[i][0].ToString(),valorliquidacione,diastrabajados,empleadosprima.Tables[0].Rows[i][1].ToString(),empleadosprima.Tables[0].Rows[i][2].ToString(),promedio,double.Parse(empleadosprima.Tables[0].Rows[i][5].ToString()),ValDescuentos);
							    }   
                                else 
                                    this.ingresardatos_prima(empleadosprima.Tables[0].Rows[i][0].ToString(),valorliquidacion,diastrabajados,empleadosprima.Tables[0].Rows[i][1].ToString(),empleadosprima.Tables[0].Rows[i][2].ToString(),promedio,double.Parse(empleadosprima.Tables[0].Rows[i][5].ToString()),ValDescuentos);
							}
						}
						valortotal = valortotal + Math.Round(valorliquidacion,0);
					}
				}
				BTNLIQUIDARDEFINITIVAMENTE.Visible=true;
				LBTOTALPRIMA.Text   = (valortotal).ToString("C");
				BTNLIQUIDAR.Visible =false;
				Session["DataTable1"]=DataTable1;
				Session["rep"]=RenderHtml();
			}
	
		
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
			MailMessage MyMail = new MailMessage();
			MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
			MyMail.To = tbEmail.Text;
			MyMail.Subject = "Proceso : "+DBFunctions.SingleData("SELECT remarks FROM SYSIBM.SYSTABLES WHERE name='"+table+"'");
			MyMail.Body = (RenderHtml());
			MyMail.BodyFormat = MailFormat.Html;
			try
			{
				SmtpMail.Send(MyMail);}
			catch(Exception e)
			{
				lbInfo.Text = e.ToString();
			}
		}

        protected void ImprimirExcelGrid(Object Sender, EventArgs e)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                string nombreArchivo = "LiquidacionPrimas" + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
                base.Response.Clear();
                base.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xls");
                base.Response.Charset = "Unicode";
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.Response.ContentType = "application/vnd.xls";
                StringWriter stringWrite = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

                DataGrid dgAux = new DataGrid();
                dgAux.DataSource = (DataTable)Session["DataTable1"];
                dgAux.DataBind();
                dgAux.RenderControl(htmlWrite);

                base.Response.Write(stringWrite.ToString());
                base.Response.End();
            }
            catch (Exception ex)
            {
                LBPRUEBAS.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
                return;
            }
        }





		protected void grabarPrimas( object Sender,EventArgs e)
		{
			int i;
			DataTable1= new DataTable();
			DataTable1=(DataTable)Session["DataTable1"];
			DBFunctions.NonQuery("insert into dbxschema.mprimas values (default,'"+fechainicio+"','"+fechafinal+"','"+DDLTIPOPRIMA.SelectedItem.Text+"')");
			for (i=0;i<DataTable1.Rows.Count;i++)
			{
				string secuencia=DBFunctions.SingleData("select max(mpri_secuencia) from dbxschema.mprimas");
				DBFunctions.NonQuery("insert into dbxschema.dprimas values ("+secuencia+",'"+DataTable1.Rows[i][0]+"',"+DataTable1.Rows[i][2]+","+DataTable1.Rows[i][3]+" )");
			}
			Response.Redirect(mainpage+"?process=Nomina.PrimaNormal");
		}
		
		
		protected void ingresardatos_prima(string codigoempleado,double Cesantias,double diastrabajados,string nombres,string apellidos,double sueldopromedio,double sueldo,double descuentos)
		{
			DataRow fila=DataTable1.NewRow();
			fila["CODIGO EMPLEADO"]=codigoempleado;
			fila["NOMBRE"]=nombres+" "+apellidos;
			double tem = Math.Round(Cesantias,0);
			fila["VALOR PRIMA"]=tem;
			fila["DIAS TRABAJADOS"]=diastrabajados;
			fila["SUELDO PROMEDIO"]=sueldopromedio;
			fila["SUELDO ACTUAL"]=sueldo;
			fila["DESCUENTOS"]=descuentos;

			DataTable1.Rows.Add(fila);
			DATAGRIDPRIMA.DataSource=DataTable1;
			DATAGRIDPRIMA.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DATAGRIDPRIMA);
			DatasToControls.JustificacionGrilla(DATAGRIDPRIMA,DataTable1);
		}
		
			protected void armarfechaliqcesantias()
		{
				if (DDLMESINICIAL.SelectedValue=="Enero")
					fechainicio=DDLANO.SelectedValue+"-01"+"-01";
				
				if (DDLMESINICIAL.SelectedValue=="Julio")
					fechainicio=DDLANO.SelectedValue+"-07"+"-01";
				
				if (LBMESFINAL.Text=="Junio")
					fechafinal=DDLANO.SelectedValue+"-06"+"-30";
		 	
				if (LBMESFINAL.Text=="Diciembre")
					fechafinal=DDLANO.SelectedValue+"-12"+"-30";
				
			Session["fechainicio"]=fechainicio;
			Session["fechafinal"]=fechafinal;
			lbfechainicio.Text=fechainicio;
			lbfechafinal.Text=fechafinal;
		}
			
		protected void preparargrilla_prima()
		{
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("CODIGO EMPLEADO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("VALOR PRIMA",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("DIAS TRABAJADOS",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("SUELDO PROMEDIO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("SUELDO ACTUAL",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("DESCUENTOS",System.Type.GetType("System.Double")));
		}
		
		protected void Page_Load(object sender , EventArgs e)
			{
			if (!IsPostBack)
				{
					LBMESFINAL.Text="Junio";
					Session.Clear();
					DatasToControls param = new DatasToControls();
					param.PutDatasIntoDropDownList(DDLANO,"SELECT PANO_ANO,PANO_DETALLE FROM PANO order by 1 desc");
				
				}
			else
				{
					if (Session["DataTable1"]!=null)
						DataTable1=(DataTable)Session["DataTable1"];
					
					if (Session["fechainicio"]!=null)
					    fechainicio=(string)Session["fechainicio"];
				
					if (Session["fechafinal"]!=null)
					    fechafinal=(string)Session["fechafinal"];
				
						
				}
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
			this.DATAGRIDPRIMA.SelectedIndexChanged += new System.EventHandler(this.DATAGRIDPRIMA_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	
		protected void cambiomes (Object sender, EventArgs e )
		{
			
			
			if (DDLMESINICIAL.SelectedValue=="Enero")
			    LBMESFINAL.Text="Junio";
			if (DDLMESINICIAL.SelectedValue=="Julio")
			    LBMESFINAL.Text="Diciembre";
		}

		private void DATAGRIDPRIMA_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
		
	}
	
}
