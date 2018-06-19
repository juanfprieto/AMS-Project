using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;
using System.ComponentModel;
using System.Globalization;
using System.Web;
//using System.Web.Mail;
using System.Data.Odbc;






namespace AMS.Nomina
{
		public class CesantiasGenerales : System.Web.UI.UserControl 
		{
			protected DropDownList DDLANO;
			protected DataTable DataTable1;
			protected DataGrid DATAGRIDCESANTIAS;
			protected DataSet empleadosvariable,empleadosnointegral,diassuspensiones;
			protected Button BTNLIQUIDARDEFINITIVAMENTE,BTNLIQUIDAR;
			protected Label lbfechainicio,lbfechafinal;
            double suma, promedio, diastrabajados, salariominimo, subsidiotransporte = 0, subsidiotransporteAnter = 0; // sueldobasico,
			string mainpage=ConfigurationManager.AppSettings["MainIndexPage"];
            string fechainicio, fechafinal, fechainiciocesantia, codigoEmpleado;  // fechasueldoactual,
            string conc_intecesantias;
            int    quincena;
            protected System.Web.UI.WebControls.Label LBMESINICIAL;
			protected System.Web.UI.WebControls.Label LBMESFINAL;
            protected System.Web.UI.WebControls.HyperLink hl;
            protected System.Web.UI.WebControls.ImageButton BtnImprimirExcel;
          
		
			protected void LiquidacionCesantiasGenerales( object sender, EventArgs e)
		{
			
				double valorliquidacion=0,interesescesantia=0;
				int i,j,ultimospagos,diaspreliquidados=0;
				this.preparargrilla_cesantias();
				empleadosvariable   = new DataSet();
				empleadosnointegral = new DataSet();
				this.armarfechaliqcesantias();
				
				string quinomens   = DBFunctions.SingleData("select cnom_opciquinomens from dbxschema.cnomina");
                conc_intecesantias = DBFunctions.SingleData("select cnom_concintecesacodi from dbxschema.cnomina");
                quincena           = Convert.ToInt32(DBFunctions.SingleData("select mqui_codiquin from mquincenas m, cnomina cn where m.mqui_anoquin  = cn.cnom_ano and m.mqui_mesquin = cn.cnom_mes and m.mqui_tpernomi = cn.cnom_quincena;"));
                subsidiotransporte = double.Parse(DBFunctions.SingleData("select CASE WHEN PC.TRES_Afeccesantia = 'S' THEN 0 ELSE coalesce(cnom_substranante,0) END from dbxschema.cnomina cn, dbxschema.pconceptonomina pc where cn.CNOM_concsubtcodi = pc.pcon_concepto;"));
                subsidiotransporteAnter = double.Parse(DBFunctions.SingleData("select CASE WHEN CNOM_ANO > " + DDLANO.SelectedValue + " THEN coalesce(cnom_substranANTE,0) ELSE coalesce(cnom_substransACTU,0) END from dbxschema.cnomina ;"));
                salariominimo      = double.Parse(DBFunctions.SingleData("select CASE WHEN CNOM_ANO > " + DDLANO.SelectedValue + " THEN coalesce(cnom_salaminiANTE,0) ELSE coalesce(cnom_salaminiACTU,0) END from dbxschema.cnomina ;"));
		 	    
				DBFunctions.Request(empleadosvariable,  IncludeSchema.NO,
                    @"select empl.memp_codiempl, mnit.mnit_nombres||' '||coalesce(mnit.mnit_nombre2,''), mnit.mnit_apellidos||' '||coalesce(mnit.mnit_apellido2,''), memp_fechingreso, memp_fechsuelactu, memp_suelactu,'','',empl.mnit_nit, 'VARIABLE'                          
                    from dbxschema.mempleado EMPL, dbxschema.MNIT mnit 
                    where tsal_salario='2' and test_estado in ('1','5') and tcon_contrato not in ('3') and empl.mnit_nit=mnit.mnit_nit  
                    order by memp_codiempl ");
				DBFunctions.Request(empleadosnointegral,IncludeSchema.NO,
                    @"select empl.memp_codiempl, mnit.mnit_nombres||' '||coalesce(mnit.mnit_nombre2,''), mnit.mnit_apellidos||' '||coalesce(mnit.mnit_apellido2,''), memp_fechingreso, memp_fechsuelactu, memp_suelactu, memp_fecsuelanter, memp_suelanter,empl.mnit_nit, 'FIJO' 
                    from dbxschema.mempleado EMPL, dbxschema.MNIT mnit 
                    where tsal_salario='3' and test_estado in ('1','5') and tcon_contrato not in ('3') and empl.mnit_nit=mnit.mnit_nit
                    order by memp_codiempl ");

				// reviso que este periodo no haya sido liquidado antes.
				int liqsino=int.Parse(DBFunctions.SingleData("select count( mces_secuencia) from dbxschema.mcesantias where mces_fechinic='"+DDLANO.SelectedValue+"' concat '-01-01' and mces_fechfina='"+DDLANO.SelectedValue+"' concat '-12-31' "));
				//sacar los pagos que afecten las cesantias por cada empleado, variable promedio de lo que trabajo.
				if (liqsino>0)
				{
                    Utils.MostrarAlerta(Response, "El periodo de liquidación de Cesantías escogido ya fue liquidado!, porfavor revise los periodos de liquidacion");
				}
				else
				{
					for (i=0;i<empleadosvariable.Tables[0].Rows.Count;i++)
					{
						suma=0;
						diaspreliquidados=0;
						promedio=0;
						diastrabajados=0;
						valorliquidacion=0;interesescesantia=0;
						DataSet preliquidado= new DataSet();
						DataSet cesantias= new DataSet();
						DataSet resumencesantias= new DataSet();
						//este query trae los pagos mes por mes
                        DBFunctions.Request(cesantias, IncludeSchema.NO, @"SELECT sum(DQUI.dqui_apagar - DQUI.DQUI_ADESCONTAR),MQUI.mqui_mesquin 
                                FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI,dbxschema.tmes T, CNOMINA CN 
                                WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin  AND DQUI.pcon_concepto=PCON.pcon_concepto  AND PCON.tres_afeccesantia='S' 
                                AND MQUI.mqui_anoquin=" + DDLANO.SelectedValue + @" AND MQUI.mqui_mesquin BETWEEN 1 AND 12 AND T.tmes_mes=mqui.mqui_mesquin AND PCON.PCON_CONCEPTO <> CN.CNOM_CONCSUBTCODI AND DQUI.memp_codiempl='" + empleadosvariable.Tables[0].Rows[i][0].ToString() + @"' group by mqui.mqui_mesquin ");
						//este query trae quincena por quincena resumencesantias
                        DBFunctions.Request(resumencesantias, IncludeSchema.NO, @"SELECT sum(DQUI.dqui_apagar - DQUI.DQUI_ADESCONTAR),MQUI.mqui_mesquin,MQUI.mqui_codiquin 
                                FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI,dbxschema.tmes T, CNOMINA CN  
                                WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin AND DQUI.pcon_concepto=PCON.pcon_concepto AND PCON.tres_afeccesantia='S'  
                                AND MQUI.mqui_anoquin=" + DDLANO.SelectedValue + @"   AND MQUI.mqui_mesquin BETWEEN 1 AND 12 AND T.tmes_mes=mqui.mqui_mesquin AND PCON.PCON_CONCEPTO <> CN.CNOM_CONCSUBTCODI AND DQUI.memp_codiempl='" + empleadosvariable.Tables[0].Rows[i][0].ToString() + @"' group by mqui.mqui_mesquin,MQUI.mqui_codiquin");
						// averiguo si ya le pague alguna cesantias e int en el año que estoy liquidando,si es asi sumo los dias liquidados.
						DBFunctions.Request(preliquidado,IncludeSchema.NO,@"SELECT distinct MCESA.MCES_FECHINIC ,DCES_DIASTRAB, MCESA.MCES_FECHFINA, DCESA.DCES_VALOCESA,DCESA.DCES_INTECESA,DCESA.MEMP_CODIEMP 
                                FROM DBXSCHEMA.DCESANTIAS DCESA , DBXSCHEMA.MCESANTIAS MCESA  
                                WHERE DCESA.MEMP_CODIEMP='"+empleadosvariable.Tables[0].Rows[i][0].ToString()+@"'   and mcesa.mces_secuencia=dcesa.mces_secuencia and '"+fechainicio+@"' between mcesa.mces_fechinic and mcesa.mces_fechfina" );
						if (preliquidado.Tables[0].Rows.Count>0)
						{
							for (j=0;j<preliquidado.Tables[0].Rows.Count;j++)
							{
								diaspreliquidados+=int.Parse(preliquidado.Tables[0].Rows[j][1].ToString());
							}
						}
					
						for (j=0;j<cesantias.Tables[0].Rows.Count;j++)
						{
							suma+= double.Parse(cesantias.Tables[0].Rows[j][0].ToString());
						}
					
			//	h		promedio= (suma/cesantias.Tables[0].Rows.Count);
						/*
						//mirar si se le esta pagando por quincenas=1 o mensualidades=2
						if (quinomens=="1")
						{
							diastrabajados=int.Parse(resumencesantias.Tables[0].Rows.Count.ToString())*15;
							diastrabajados=diastrabajados-diaspreliquidados;
						}
						if (quinomens=="2")
						{
							diastrabajados=int.Parse(resumencesantias.Tables[0].Rows.Count.ToString())*30;
							diastrabajados=diastrabajados-diaspreliquidados;
						}
						*/
						codigoEmpleado=empleadosvariable.Tables[0].Rows[i][0].ToString();
						fechainiciocesantia=empleadosvariable.Tables[0].Rows[i][3].ToString();
						this.calculardiastrabajados();
						diastrabajados=diastrabajados-diaspreliquidados;
						
						promedio = (suma/(diastrabajados/30));
                        if (promedio < salariominimo*2)
                            promedio += subsidiotransporte;

						valorliquidacion=Math.Round((promedio*diastrabajados)/360,0);
						interesescesantia=Math.Round((valorliquidacion*diastrabajados*0.12)/360,0);
					
						if (cesantias.Tables[0].Rows.Count>0)
						{
							this.ingresardatos_cesantias(empleadosvariable.Tables[0].Rows[i][0].ToString(),valorliquidacion,interesescesantia,diastrabajados,empleadosvariable.Tables[0].Rows[i][1].ToString(),empleadosvariable.Tables[0].Rows[i][2].ToString(),promedio);
						}
					}
					
					//cesantias empleados con sueldo no integral
					for (i=0;i<empleadosnointegral.Tables[0].Rows.Count;i++)
					{
						//para estos empleados miro la condicion, si vario el sueldo en los ultimos 3 meses	
						suma=0;
						promedio=0;
						diastrabajados=0;
						valorliquidacion=0;interesescesantia=0;
						diaspreliquidados=0;
						DataSet preliquidado= new DataSet();	
						DataSet cesantias= new DataSet();
						DataSet resumencesantias= new DataSet();

                        DBFunctions.Request(cesantias, IncludeSchema.NO, @"SELECT sum(DQUI.dqui_apagar - DQUI.dqui_adescontar),MQUI.mqui_mesquin, 
                                CASE WHEN YEAR(MEMP.MEMP_FECHSUELACTU) > " + DDLANO.SelectedValue + @" THEN MEMP.MEMP_SUELANTER ELSE MEMP.MEMP_SUELACTU END, 
                                CASE WHEN YEAR(MEMP.MEMP_FECHSUELACTU) > " + DDLANO.SelectedValue + @" THEN MEMP.MEMP_FECSUELANTER ELSE MEMP.MEMP_FECHSUELACTU END  
                                FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI,dbxschema.tmes T, dbxschema.mEMPLEADO MEMP 
                                WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin  AND DQUI.pcon_concepto=PCON.pcon_concepto  AND PCON.tres_afeccesantia='S' AND DQUI.MEMP_CODIEMPL = MEMP.MEMP_CODIEMPL
                                AND MQUI.mqui_anoquin=" + DDLANO.SelectedValue + @" AND MQUI.mqui_mesquin BETWEEN 1 AND 12 AND T.tmes_mes=mqui.mqui_mesquin AND DQUI.memp_codiempl='" + empleadosnointegral.Tables[0].Rows[i][0].ToString() + @"' group by mqui.mqui_mesquin, MEMP.MEMP_SUELACTU, MEMP.MEMP_FECHSUELACTU, MEMP.MEMP_SUELANTER, MEMP.MEMP_FECSUELANTER  ");

                        DBFunctions.Request(resumencesantias, IncludeSchema.NO, @"SELECT sum(DQUI.dqui_apagar - DQUI.dqui_adescontar),MQUI.mqui_mesquin,MQUI.mqui_codiquin 
                                FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI,dbxschema.tmes T  
                                WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin AND DQUI.pcon_concepto=PCON.pcon_concepto AND PCON.tres_afeccesantia='S'  
                                AND MQUI.mqui_anoquin=" + DDLANO.SelectedValue + @"   AND MQUI.mqui_mesquin BETWEEN 1 AND 12 AND T.tmes_mes=mqui.mqui_mesquin AND DQUI.memp_codiempl='" + empleadosnointegral.Tables[0].Rows[i][0].ToString() + @"' group by mqui.mqui_mesquin,MQUI.mqui_codiquin");
						
                        DBFunctions.Request(preliquidado,IncludeSchema.NO,@"SELECT distinct MCESA.MCES_FECHINIC ,DCES_DIASTRAB, MCESA.MCES_FECHFINA, DCESA.DCES_VALOCESA,DCESA.DCES_INTECESA,DCESA.MEMP_CODIEMP 
                                FROM DBXSCHEMA.DCESANTIAS DCESA, DBXSCHEMA.MCESANTIAS MCESA  
                                WHERE DCESA.MEMP_CODIEMP='"+empleadosnointegral.Tables[0].Rows[i][0].ToString()+@"'   and mcesa.mces_secuencia=dcesa.mces_secuencia and '"+fechainicio+@"' between mcesa.mces_fechinic and mcesa.mces_fechfina" );
						if (preliquidado.Tables[0].Rows.Count>0)
						{
							for (j=0;j<preliquidado.Tables[0].Rows.Count;j++)
							{
								diaspreliquidados+=int.Parse(preliquidado.Tables[0].Rows[j][1].ToString());
                                Utils.MostrarAlerta(Response, "dias que ya se liquidaron " + diaspreliquidados + " para el empelado  " + empleadosnointegral.Tables[0].Rows[i][0].ToString() + " ");
							}
						}
					
					
					//sacar los 3 ultimos pagos mensuales
					ultimospagos= cesantias.Tables[0].Rows.Count-3;
					if (ultimospagos<0)
					{
						ultimospagos=0;
						for (j=0;j<cesantias.Tables[0].Rows.Count;j++)
						{
							suma+= double.Parse(cesantias.Tables[0].Rows[j][0].ToString());
						}
				//		promedio= (suma/cesantias.Tables[0].Rows.Count);
					}
					else
					{
						for (j=0;j<cesantias.Tables[0].Rows.Count;j++)
						{
							suma+= double.Parse(cesantias.Tables[0].Rows[j][0].ToString());
						}
					}
				
				codigoEmpleado      = empleadosnointegral.Tables[0].Rows[i][0].ToString();
				fechainiciocesantia = empleadosnointegral.Tables[0].Rows[i][3].ToString();
				this.calculardiastrabajados();

                if (diastrabajados < 90)
                {
                    promedio = (suma / (diastrabajados / 30));
                    if (promedio < salariominimo * 2)
                        promedio += subsidiotransporte;
                }
                else
                    if (cesantias.Tables[0].Rows.Count == 12)
                    {
                        DateTime fechaSueldo = Convert.ToDateTime(cesantias.Tables[0].Rows[11][3].ToString());
                        DateTime fechaLiquid = Convert.ToDateTime(fechafinal.ToString());
                        if (fechaSueldo < (fechaLiquid).AddDays(-90) )
                            // Si le devengado no cambia en los ultimos tres (3) MESES
                            promedio = double.Parse(cesantias.Tables[0].Rows[11][2].ToString());
                        else
                        {
                            promedio = (suma / (diastrabajados / 30)); // se promedia sobre lo devengado en el año
                            /*
                            if (Convert.ToDateTime(empleadosvariable.Tables[0].Rows[i][3]).Year = int.Parse(DDLANO.SelectedValue))
                                sueldobasico = double.Parse(empleadosvariable.Tables[0].Rows[i][4].ToString());
                            else
                                sueldobasico = double.Parse(empleadosvariable.Tables[0].Rows[i][6].ToString());
                            if(sueldobasico<=(salariominimo*2))
                                sueldobasico = sueldobasico + subsidiotransporte;
                             */
                        }
                        if (promedio < salariominimo * 2)
                            promedio += subsidiotransporteAnter;
                    }
                    else
                        promedio = (suma / (diastrabajados / 30)); // se promedia sobre lo devengado en el año
				
					diastrabajados   = diastrabajados - diaspreliquidados;

                promedio = Math.Round(promedio*1, 0);
                valorliquidacion =Math.Round((promedio*diastrabajados)/360,0);
				interesescesantia=Math.Round((valorliquidacion*diastrabajados*0.12)/360,0);
					
				if (cesantias.Tables[0].Rows.Count>0)
					{
						this.ingresardatos_cesantias(empleadosnointegral.Tables[0].Rows[i][0].ToString(),valorliquidacion,interesescesantia,diastrabajados,empleadosnointegral.Tables[0].Rows[i][1].ToString(),empleadosnointegral.Tables[0].Rows[i][2].ToString(),promedio);
						
					}
									
				}
				BTNLIQUIDARDEFINITIVAMENTE.Visible=true;
                //hl.Visible = true;
                BtnImprimirExcel.Visible = true;
                //hl.Text = "Descargar Archivo";
                    
                Session["DataTable1"] = DataTable1;
                Session["quincena"] = quincena;
                Session["conc_intecesantias"] = conc_intecesantias;
                
                }
            }


            protected void ImprimirExcelGrid(Object Sender, EventArgs e)
            {
                try
                {
                    DateTime fecha = DateTime.Now;
                    string nombreArchivo = "Cesantias e Intereses" + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
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
                    //LBPRUEBAS.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
                    return;
                }
            }	

		protected void armarfechaliqcesantias()
		{
			fechainicio=DDLANO.SelectedValue+"-01"+"-01";
		 	fechafinal=DDLANO.SelectedValue+"-12"+"-30";
			Session["fechainicio"]=fechainicio;
			Session["fechafinal"]=fechafinal;
			lbfechainicio.Text=fechainicio;
			lbfechafinal.Text=fechafinal;
		}
			
		protected void calculardiastrabajados()
		{
			string fechainicial="";
			if   (Convert.ToDateTime(fechainiciocesantia) > Convert.ToDateTime(fechainicio))
				{
				fechainicial = Convert.ToDateTime(fechainiciocesantia).Year +"-"+ Convert.ToDateTime(fechainiciocesantia).Month +"-"+ Convert.ToDateTime(fechainiciocesantia).Day;					 ; 
				}
			else fechainicial = Convert.ToString(fechainicio);

			diastrabajados = ((Convert.ToDateTime(fechafinal).Year*360)
					+(Convert.ToDateTime(fechafinal).Month*30)
					+Convert.ToDateTime(fechafinal).Day)
					-((Convert.ToDateTime(fechainicial).Year*360)
					+(Convert.ToDateTime(fechainicial).Month*30)
					+(Convert.ToDateTime(fechainicial).Day))
					+1;
					 
							
			if (diastrabajados >= 360)
				diastrabajados = 360;
			 
//            string diassuspension=DBFunctions.SingleData(@"select coalesce(sum((M.msus_hasta-M.msus_desde)+1),0) as diasLic
//							 from dbxschema.msusplicencias M, dbxschema.pconceptonomina P, dbxschema.tdesccantidad T
//							 where M.memp_codiempl='"+codigoEmpleado+@"' and P.pcon_claseconc='L' and m.ttip_coditipo in (1,4)
//							 and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) 
//							 and (M.msus_desde between '"+fechainicial+@"' and '"+fechafinal+@"')
//							 and (M.msus_hasta between '"+fechainicial+@"' and '"+fechafinal+@"')
//							 group by M.memp_codiempl,M.msus_desde,M.msus_hasta
//							 order by M.memp_codiempl,M.msus_desde,M.msus_hasta");
            string diassuspension = DBFunctions.SingleData(@"select sum(dqui_canteventos) as diasLic
							 from dbxschema.dquincena D, dbxschema.mquincenas M, dbxschema.pconceptonomina P, dbxschema.tdesccantidad T
							 where d.memp_codiempl='" + codigoEmpleado + @"' and P.pcon_claseconc='L' 
							 and dqui_adescontar > 0
							 and (d.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) 
							 and M.mQUI_ANOQUIN = "+DDLANO.SelectedValue+@"
							 AND D.MQUI_CODIQUIN = M.MQUI_CODIQUIN
						     group by d.memp_codiempl,M.mQUI_ANOQUIN 
							 order by d.memp_codiempl,M.mQUI_ANOQUIN ");
            if (diassuspension == "")   // SI TRAJO NULO
                diassuspension = "0";   
			diastrabajados = diastrabajados - double.Parse(diassuspension.ToString());
		}	
										
					
		protected void grabarcesantiasgenerales(object sender ,EventArgs e)
		{
			int i;
			DataTable1= new DataTable();
			DataTable1=(DataTable)Session["DataTable1"];
            conc_intecesantias = Session["conc_intecesantias"].ToString();
            quincena  = Convert.ToInt32(Session["quincena"].ToString());

            // TODO ESTO ES NECESARIO insertarlo en un una sola transaccion porque si se revienta, que no grabe ningun registro.

			DBFunctions.NonQuery("insert into dbxschema.mcesantias values (default,'"+fechainicio+"','"+fechafinal+"')");
			for (i=0;i<DataTable1.Rows.Count;i++)
			{
				string secuencia=DBFunctions.SingleData("select max(mces_secuencia) from dbxschema.mcesantias");
                DBFunctions.NonQuery("insert into dbxschema.dcesantias values (" + DataTable1.Rows[i][2] + "," + DataTable1.Rows[i][3] + ",'" + DataTable1.Rows[i][0] + "',default," + secuencia + "," + DataTable1.Rows[i][4] + "," + DataTable1.Rows[i][5] + ")");
                DBFunctions.NonQuery("insert into dbxschema.dquincena  values (DEFAULT," + quincena + ",'" + DataTable1.Rows[i][0] + "','" + conc_intecesantias + "',1," + DataTable1.Rows[i][3] + "," + DataTable1.Rows[i][3] + ",0,4,0,'" + secuencia +"' ,'I')");
            }
			Response.Redirect(mainpage+"?process=Nomina.CesantiasGenerales");
		}
		

        protected void ingresardatos_cesantias(string codigoempleado,double Cesantias,double interesescesantia,double diastrabajados,string nombres,string apellidos,double sueldopromedio)
		{
			DataRow fila=DataTable1.NewRow();
			fila["CODIGO EMPLEADO"]=codigoempleado;
			fila["NOMBRE"]=nombres+" "+apellidos;   // insertar celdas de cesantias retiradas y otra para intereses retirados
			fila["CESANTIAS"]=Cesantias;
			fila["INTERESES DE CESANTIA"]=interesescesantia;
			fila["DIAS TRABAJADOS"]=diastrabajados;
			fila["SUELDO PROMEDIO"]=sueldopromedio;
			DataTable1.Rows.Add(fila);
			DATAGRIDCESANTIAS.DataSource=DataTable1;
			DATAGRIDCESANTIAS.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DATAGRIDCESANTIAS);
			DatasToControls.JustificacionGrilla(DATAGRIDCESANTIAS,DataTable1);
						
		}
		
			
		protected void preparargrilla_cesantias()
		{
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("CODIGO EMPLEADO",System.Type.GetType("System.String")));
            DataTable1.Columns.Add(new DataColumn("NOMBRE", System.Type.GetType("System.String")));   // insertar celdas de cesantias retiradas y otra para intereses retirados
			DataTable1.Columns.Add(new DataColumn("CESANTIAS",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("INTERESES DE CESANTIA",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("DIAS TRABAJADOS",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("SUELDO PROMEDIO",System.Type.GetType("System.Double")));
		}


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
                DatasToControls param = new DatasToControls();
                param.PutDatasIntoDropDownList(DDLANO, "SELECT PANO_ANO,PANO_DETALLE FROM PANO ORDER BY 1 DESC");
            }
            else
            {
                if (Session["fechainicio"] != null)
                    fechainicio = (string)Session["fechainicio"];

                if (Session["fechafinal"] != null)
                    fechafinal = (string)Session["fechafinal"];

                if (Session["DataTable1"] != null)
                    DataTable1 = (DataTable)Session["DataTable1"];
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	
		
		
		}
}

