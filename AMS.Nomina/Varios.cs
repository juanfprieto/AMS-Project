using System;
using System.Data;

using AMS.DB;
using AMS.Forms;


namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de Varios.
	/// </summary>
	public class Varios
	{
		private string NumeroQuincena;
		private DatasToControls DTC = new DatasToControls();
		private CNomina o_CNomina;
		
		public CNomina AsignarParametrosNomina
		{
			set
			{
				o_CNomina = value;
			}
		}

		public string p_NumeroQuincena
		{
			get
			{
				NumeroQuincena = DBFunctions.SingleData("select coalesce(max(mqui_codiquin),0) from mquincenas");
                return NumeroQuincena; 
			}
		}

		public Varios()
		{
		}
        
		public void IniciarSistemaNomina()
		{
			int retorno = DBFunctions.NonQuery("insert into mquincenas values (default,"+o_CNomina.CNOM_ANO+","+o_CNomina.CNOM_MES+"," +o_CNomina.CNOM_QUINCENA+",1");
		}
        

		public void llenarListaEmpleados (System.Web.UI.WebControls.DropDownList LB)
		{
	        DTC.PutDatasIntoDropDownList(LB, "SELECT M.MEMP_CODIEMPL,trim(N.MNIT_APELLIDOS) CONCAT ' ' CONCAT COALESCE(N.MNIT_APELLIDO2,'') CONCAT ' ' CONCAT trim(N.MNIT_NOMBRES) CONCAT ' ' CONCAT COALESCE(N.MNIT_NOMBRE2,'') concat ' - ' concat M.MEMP_CODIEMPL FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT and M.test_estado <> '4' ORDER BY N.MNIT_APELLIDOS, N.MNIT_NOMBRES");
        }

		public void llenarListaAños (System.Web.UI.WebControls.DropDownList LB,string ano)
		{
			DTC.PutDatasIntoDropDownList(LB,"SELECT PANO_ANO,PANO_DETALLE FROM PANO order by 1 desc");
			DatasToControls.EstablecerDefectoDropDownList(LB,ano);
		}

		public void llenarListaMeses (System.Web.UI.WebControls.DropDownList LB,string mes)
		{
			DTC.PutDatasIntoDropDownList(LB,"Select PMES_MES, PMES_NOMBRE from PMES order by 1");
			DatasToControls.EstablecerDefectoDropDownList(LB,mes);
		}

		public void llenarListaDias (System.Web.UI.WebControls.DropDownList LB,string dia)
		{
			DTC.PutDatasIntoDropDownList(LB,"Select TDIA_DIA, TDIA_NOMBRE from TDIA order by 1");
			DatasToControls.EstablecerDefectoDropDownList(LB,dia);
		}

		public void llenarMotivoRetiro (System.Web.UI.WebControls.DropDownList LB)
		{
			DTC.PutDatasIntoDropDownList(LB,"Select pmot_estado,pmot_descripcion from PMOTIVORETIRO");
		}

		public double PorcentageFondoSolidaridad(string valor)
		{
			double retorno= 0;
			string valorretorno = DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens, dbxschema.CNOMINA where (" + valor+ "/ CNOM_SALAMINIACTU) between pfon_intinicio and pfon_intfinal AND " + valor + " >= (CNOM_SALAMINIACTU * CNOM_TOPEPAGOFONDSOLIPENS);");
			if (valorretorno != "")
			{
                retorno = Double.Parse(valorretorno);
			}
			else
			{
				retorno = 0;
			}
			return retorno; 
		}

		public int DiasVacaciones(string empleado)
		{
			int retorno= 0;
			string valorretorno = DBFunctions.SingleData("Select sum(mvac_diascaus)-sum(mvac_diasvacadisf) as Totaldias from MVACACIONES where memp_codiemp = '" + empleado + "'");
			if (valorretorno != "")
			{
				retorno = Int32.Parse(valorretorno);
			}
			else
			{
				retorno = 0;
			}
			return retorno; 
		}



		public string vacacionesDias(string empleado)
		{			
			string valorretorno = DBFunctions.SingleData("Select MVAC_HASTA from DBXSCHEMA.MVACACIONES where memp_codiemp = '" + empleado + "'  and mvac_diasvacadisf > 0  ORDER BY mvac_hasta DESC FETCH FIRST 1 ROWS ONLY OPTIMIZE FOR 1 ROWS");
			if (valorretorno != "")
			{
				return valorretorno;				
			}
			else
			{
				valorretorno=null;
			}
			return valorretorno; 
		}

		public bool ProcesarEmpleado(string Estado,string CodigoEmpleado,string fechafinal)
		{
			bool retorno = true;
			if (Estado == "5")
			{
				DataSet fechavacaciones = new DataSet();
				string Sql = "select DVAC.mvac_secuencia,DVAC.dvac_fechinic,DVAC.dvac_fechfinal,MVAC.memp_codiemp,DVAC.dvac_continuidad from dbxschema.dvacaciones DVAC,dbxschema.mvacaciones MVAC where DVAC.mvac_secuencia=MVAC.mvac_secuencia and MVAC.memp_codiemp='"+CodigoEmpleado+"' and DVAC.dvac_fechfinal>='"+fechafinal+"' order by dvac_fechfinal  ";
				DBFunctions.Request(fechavacaciones,IncludeSchema.NO,Sql ); 
				if (fechavacaciones.Tables[0].Rows.Count > 0)
				{
					if (fechavacaciones.Tables[0].Rows[0][4].ToString()=="C")
					{
						if (Convert.ToDateTime(fechavacaciones.Tables[0].Rows[fechavacaciones.Tables[0].Rows.Count-1][2].ToString())>Convert.ToDateTime(fechafinal))
							retorno = false;
					}
					else
					{
						if (Convert.ToDateTime(fechavacaciones.Tables[0].Rows[0][2].ToString())>Convert.ToDateTime(fechafinal))
							retorno = false;
					}
				}
				//if (o_CNomina.CNOM_LIQCOMBINADA=="S")
				//{
					//					if (o_CNomina.=="2" && DDLQUIN.SelectedValue=="2")
					//					{
					//						this.revisarempleadosenvacaciones(empleados.Tables[0].Rows[i][0].ToString(),lb.Text,lb2.Text,empleados.Tables[0].Rows[i][10].ToString());
					//						entra+=1;	
					//					}
				//}
				//else
				//{
					//					if (fechavacaciones.Tables[0].Rows.Count>0 && ent==true)
					 
					//						this.revisarempleadosenvacaciones(empleados.Tables[0].Rows[i][0].ToString(),lb.Text,lb2.Text,empleados.Tables[0].Rows[i][10].ToString());
					//						entra+=1;				
					//					}
				//}
			}
			else if (Estado != "1")
			{
				retorno = false;
			}
			return retorno;
		}

        // metodo de PAGOS Y DESCUENTOS PERMANENTES
        public void Actualiza_mpagosydtosper(string codigoempleado, string lb, string lb2, string codquincena, double sueldo, string docref, string lbmas1, string apellido1, string apellido2, string nombre1, string nombre2, string periodoeps, DataSet mpagosydtosper)
        {
            DataSet mquincenas = new DataSet();
            DateTime fechainicioliquidacion = new DateTime();
            DateTime fechafinalliquidacion = new DateTime();
            fechainicioliquidacion = Convert.ToDateTime(lb.ToString());  
            fechafinalliquidacion = Convert.ToDateTime(lb2.ToString());
  
            int i;
            int quincenaProc = Convert.ToInt32(codquincena);
 
            //Traigo los pagos y descuentos permanentes para cada empleado vigente entre las fechas de la quincena vigente ,descuento en la primera quincena o en ambas
            DBFunctions.Request(mpagosydtosper, IncludeSchema.NO, "select * from (select distinct M.pcon_concepto, M.mpag_fechinic,M.mpag_fechfinmes,M.mpag_fechfinquin,M.mpag_fechfinano,P.pcon_signoliq,COALESCE(M.mpag_valor,0),P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva, MPAG_SECUENCIA, pcon_nombconc, m.mpag_fechfinano ||'-'||m.mpag_fechfinmes ||'-'|| case when m.mpag_fechfinquin = 1 then 15 else case when m.mpag_fechfinmes = 2 then 28 else 30 end end as fecha_vence  from dbxschema.mpagosydtosper M,dbxschema.pconceptonomina P, dbxschema.cnomina cn where M.memp_codiempl='" + codigoempleado + "' and M.pcon_concepto=P.pcon_concepto and (M.mpag_tperpag = 1 AND CN.cnom_quincena = 1 OR M.mpag_tperpag=2 AND CN.cnom_quincena = 2 or M.mpag_tperpag=3) and M.mpag_valor not in (select d.dqui_valevento from dquincena d where p.pcon_concepto=d.pcon_concepto and M.memp_codiempl=d.memp_codiempl and M.mpag_valor=d.dqui_valevento and d.mqui_codiquin=" + quincenaProc + ") AND   M.PCON_CONCEPTO||M.MPAG_SECUENCIA||M.MEMP_CODIEMPL NOT IN (SELECT  D.PCON_CONCEPTO||D.DQUI_DOCREFE||D.MEMP_CODIEMPL FROM MQUINCENAS M, DQUINCENA D, CNOMINA CN WHERE M.MQUI_CODIQUIN = D.MQUI_CODIQUIN AND M.MQUI_ANOQUIN = CN.CNOM_ANO AND M.MQUI_MESQUIN = CN.CNOM_MES) ) as a where fecha_vence >= '" + fechafinalliquidacion.ToString("yyyy-MM-dd") + "'");

            //conversion fecha de mpagosydtosper para ver si aplica a la quincena vigente
            if (mpagosydtosper.Tables[0].Rows.Count != 0)
            {

                for (i = 0; i < mpagosydtosper.Tables[0].Rows.Count; i++)
                {
                }
            }
        }

        public double calcular_ReteFuente(string codigoempleado, string lb, string lb2, string codquincena, double sueldo, string docref, string lbmas1, string apellido1, string apellido2, string nombre1, string nombre2, double acumuladoeps, string tiposalario, string mensaje, int quincena, string tipoPago, double valorfondopensionempleado, double fondosolidaridadpensional, double valorepsempleado, double valorepsquinanteriorA, double valorfondopensionvoluntaria, string unidT, CNomina o_CNomina)
        {
            //Averiguar los pagos extemporaneos (fuera de nomina) que tiene el empleado.
            double  pagado = 0;
            double  valoradescontar2 = 0;
            DataSet pagosfnomina = new DataSet();

            DBFunctions.Request(pagosfnomina, IncludeSchema.NO, "select M.pcon_concepto,M.MPAGO_valrtotl,M.memp_codiempl,P.pcon_signoliq,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva,P.pcon_nombconc from dbxschema.MPAGOSFUERANOMINA M,dbxschema.pconceptonomina P,dbxschema.tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_claseconc='N' and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='" + codigoempleado + "' and (M.mpago_fecha between '" + lb.ToString() + "' and '" + lb2.ToString() + "')");
            int j;
        
            if (pagosfnomina.Tables[0].Rows.Count != 0)
            {
                for (j = 0; j < pagosfnomina.Tables[0].Rows.Count; j++)
                {
                    if (pagosfnomina.Tables[0].Rows[j][1].ToString() + "" == "")
                    {
                        mensaje += "Por favor ingrese un valor para los Pagos extemporaneos del empleado " + codigoempleado + "  \\n";
                    }
                    else
                    {
                        pagado += double.Parse(pagosfnomina.Tables[0].Rows[j][1].ToString());
                    }
                }
                pagado = Math.Round(pagado, 0);
            }

            DataSet estadorfte = new DataSet();
            DataSet valrftequinanterior = new DataSet();

            int     numquincenaanterior = 0;
            double  retefuenteDescontada = 0;

            DBFunctions.Request(estadorfte, IncludeSchema.NO, "select memp_testrete, coalesce(memp_porcrete,0), tcon_contrato, tSAL_SALARIO from dbxschema.mempleado where memp_codiempl='" + codigoempleado + "'");
            DataSet mquincenas = new DataSet();
            DataSet primertope = new DataSet();

            if(estadorfte.Tables[0].Rows[0][0].ToString() == "3")  // empleados definidos SIN RETENCION, el liquidador la debe definir por Pagos y Descuentos permanentes
                return 0;

            DBFunctions.Request(mquincenas, IncludeSchema.NO, "select max(mqui_codiquin) from mquincenas");

            DBFunctions.Request(primertope, IncludeSchema.NO, "select pret_intfinal from pretefuente ");

            if (primertope.Tables[0].Rows.Count == 0)
            {
                mensaje += "Por favor Ingrese la Tabla de Retención en la Fuente \\n";
            }
            else
            {
                string retencion = o_CNomina.CNOM_CONCRFTECODI;
                if (tipoPago == "QUINCENAL" && quincena == 2)
                {
                    try
                    {
                        numquincenaanterior = Convert.ToInt32(DBFunctions.SingleData("SELECT coalesce(MQUI.mqui_codiquin,0) from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin=" + o_CNomina.CNOM_ANO + " and MQUI.mqui_mesquin=" + o_CNomina.CNOM_MES + " and MQUI.mqui_tpernomi=1 and mqui_estado = 2"));
                    }
                    catch
                    {
                        numquincenaanterior = 0;
                    }
                }

                DataSet descuentos = new DataSet();

                DBFunctions.Request(descuentos, IncludeSchema.NO, "select coalesce(memp_vrexceafc,0),coalesce(memp_vrexcesalud,0),coalesce(memp_vrexcevivi,0), coalesce(memp_perscargo,0) from dbxschema.mempleado where memp_codiempl='" + codigoempleado + "'  ");

                double acumuladoretefuente2 = 0, retefuentemensual = 0;
                try
                {
                    retefuenteDescontada = Convert.ToInt32(DBFunctions.SingleData("SELECT coalesce(SUM(dqui_adescontar),0) from dbxschema.dquincena MQUI wHERE (mqui_codiquin=" + numquincenaanterior + " OR mqui_codiquin=" + codquincena + ") and memp_codiempl='" + codigoempleado + "' and pcon_concepto = '" + o_CNomina.CNOM_CONCRFTECODI + "' "));
                }
                catch
                {
                    retefuenteDescontada = 0;
                }
           
                //  la retencion en la fuente se debe causar en todas las liquidaciones sobre el total de los ingresos del mes y restarle lo ya causado en el mismo mes
               
                        //validar si el empleado esta por porcentaje
                        double deduAfc      = double.Parse(descuentos.Tables[0].Rows[0][0].ToString()); //afc
                        double deduSalud    = double.Parse(descuentos.Tables[0].Rows[0][1].ToString()); //salud
                        double deduVivienda = double.Parse(descuentos.Tables[0].Rows[0][2].ToString()); //vivienda
                        double personasAcargo = double.Parse(descuentos.Tables[0].Rows[0][3].ToString()); //personas a cargo

                        string query = @"SELECT coalesce(SUM(DQUI.DQUI_APAGAR),0) FROM DBXSCHEMA.DQUINCENA DQUI," +
                                        " DBXSCHEMA.PCONCEPTONOMINA PCON WHERE MQUI_CODIQUIN=" + numquincenaanterior + " AND " +
                                        " MEMP_CODIEMPL='" + codigoempleado + "' AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO " +
                                        " AND PCON.tres_afecretefte='S'";
                        // string quinanterior = DBFunctions.SingleData(query);
                        double descquinanterior = 0; // Convert.ToDouble(DBFunctions.SingleData(query));

                        string queryD = @"SELECT coalesce(SUM(DQUI.DQUI_APAGAR - DQUI.DQUI_ADESCONTAR),0)FROM DBXSCHEMA.DQUINCENA DQUI," +
                                        " DBXSCHEMA.PCONCEPTONOMINA PCON WHERE MQUI_CODIQUIN=" + codquincena + " AND " +
                                        " MEMP_CODIEMPL='" + codigoempleado + "' AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO " +
                                        " AND PCON.tres_afecretefte='S'";
                        descquinanterior = Convert.ToDouble(DBFunctions.SingleData(queryD));
                  
                        double ingresosLaborales = acumuladoeps + descquinanterior;

                        if (estadorfte.Tables[0].Rows[0][2].ToString() == "4") // pensionado
                            valorfondopensionempleado = 0;

                        // 1. pagos brutos al empleado
                        // 2. menos: ingresos del mes no gravables para el trabajador
                        // double ingresosNogravables = 0;  // ver como se selecciona esa información y acumularla aqui

                        // 3. menos: deducciones

                        // vivienda
                        if (deduVivienda > 100 * Convert.ToDouble(unidT))   // máximo 100 UVTs
                            deduVivienda = 100 * Convert.ToDouble(unidT);
                        // medicina prepagada
                        if (deduSalud > 16 * Convert.ToDouble(unidT))       // máximo 16 UVTs
                            deduSalud = 16 * Convert.ToDouble(unidT);
                        // personas a cargo
                        double deduPnasCargo = 0;
                        if (personasAcargo > 0)
                        {
                            if (retefuentemensual * 0.10 > 32 * Convert.ToDouble(unidT))
                                deduPnasCargo = 32 * Convert.ToDouble(unidT);
                            else
                                deduPnasCargo = Math.Round(retefuentemensual * 0.10, 0);
                        }
                        // aportes obligatorios a salud del año gravable anterior
                        // se toma el pago a la eps del mes presente, porque la del año anterior no es segura si el empleado no trabajo en la empresa todo el tiempo
                        //  el acumuladoEPS ya lo tiene descontado de las base 

                        // double deduAfctotal = valorfondopensionempleado + fondosolidaridadpensional + valorepsempleado + deduAfc + descquinanterior + acumuladoretefuenteQnaAct ;
                        //                  vivienda medici  pnasCgo     salud
                        double deduEpsPens = valorepsempleado + valorepsquinanteriorA + valorfondopensionempleado + fondosolidaridadpensional + descquinanterior;
                        double subTotal1 = ingresosLaborales - deduEpsPens;
                        double deducciones = deduVivienda + deduSalud + deduPnasCargo ;
                        double subTotal2 = subTotal1 -deducciones;
                        // 4. Rentas Exentas
                        // AFC  =  Ahorro para el Fomento de la Construcción
                        if (deduAfc > (30 / 100) * retefuentemensual)  // maximo el 30% de ingreso bruto
                            deduAfc = (30 / 100) * retefuentemensual;
                        if (deduAfc > (3800 / 12) * Convert.ToDouble(unidT))  // maximo 3.800 uvs anuales en meses
                            deduAfc = (3800 / 12) * Convert.ToDouble(unidT);
                        //                    afc         pension
                        double rentaExenta = deduAfc + valorfondopensionvoluntaria;
                        double subTotal3 = subTotal2 - rentaExenta;
                        double base25 = Math.Round(subTotal3 * 0.25,0);   // 25% del deduAfcible
                        if (base25 > 240 * Convert.ToDouble(unidT))
                            base25 = 240 * Convert.ToDouble(unidT);

                        double subTotal4 = Math.Round(subTotal3 - base25,0);
                        double deductotal = deducciones + rentaExenta + base25;
                        double topededu40porc = Math.Round(subTotal1* 0.40,0); // 40% es el maximo tope para deducciones
                        double totalDeduRentExen = deducciones + rentaExenta + base25;

                        if (totalDeduRentExen > topededu40porc)
                            acumuladoretefuente2 = subTotal1 - topededu40porc;
                        else
                            acumuladoretefuente2 = subTotal4;  // base para cálculo de retención

                        ///    REGIMEN ORDINARIO
                        /// procedimiento 1   -    se trabaja con las uvt en la tabla  R
                        double valorRetencionOrdinaria = calcular_reteFuente(estadorfte, "R", unidT, acumuladoretefuente2, codigoempleado, mensaje);

                        ///    REGIMEN IMAN = Impuesto Mínimo Alternativo Nacional
                        /// procedimiento 1   -    se trabaja con las uvt en la tabla  I
                        acumuladoretefuente2 = acumuladoeps - descquinanterior - valorfondopensionempleado - fondosolidaridadpensional - valorepsempleado;
                        double valorRetencionIman = calcular_reteFuente(estadorfte, "I", unidT, acumuladoretefuente2, codigoempleado, mensaje);

                        if (valorRetencionOrdinaria > valorRetencionIman)
                            valoradescontar2 = valorRetencionOrdinaria;
                        else
                            valoradescontar2 = valorRetencionIman;

                        valoradescontar2 = valoradescontar2 - retefuenteDescontada;
                        valoradescontar2 = Math.Round(valoradescontar2 / 1000, 0) * 1000;
            }
            return valoradescontar2;
        }

        protected double calcular_reteFuente(DataSet estadorfte, string tipoRegimen, string unidT, double acumuladoretefuente2, string codigoempleado, string mensaje)
        {
            DataSet valoradescontar = new DataSet();
            double valoradescontar2 = 0;
            if (estadorfte.Tables[0].Rows[0][0].ToString() == "1" || tipoRegimen == "I")
            {
                double uvt = Double.Parse(unidT);

                if (uvt < acumuladoretefuente2)
                {
                    double valorBaseUVT = 0;
                    if (tipoRegimen == "I")
                        valorBaseUVT = (acumuladoretefuente2 / uvt);
                    else
                        valorBaseUVT = acumuladoretefuente2 / uvt;

                    DBFunctions.Request(valoradescontar, IncludeSchema.NO, "select pret_intinicio,pret_tarimarginal,pret_sumafinal from dbxschema.pretefuente where  (" + valorBaseUVT.ToString() + " between pret_intinicio +0.01 and pret_intfinal) and ttip_retefuen = '" + tipoRegimen + "' ");
                    if (valoradescontar.Tables[0].Rows.Count > 0)
                    {
                        if (tipoRegimen == "I")
                            valoradescontar2 = double.Parse(valoradescontar.Tables[0].Rows[0][1].ToString());
                        else
                            valoradescontar2 = ((valorBaseUVT - double.Parse(valoradescontar.Tables[0].Rows[0][0].ToString())) * double.Parse(valoradescontar.Tables[0].Rows[0][1].ToString()) / 100) + double.Parse(valoradescontar.Tables[0].Rows[0][2].ToString());
                        valoradescontar2 = valoradescontar2 * uvt;
                    }
                    else
                    {
                        valoradescontar2 = 0;
                        mensaje += "No HA definido los intervalos de Retención en la Fuente REGIMEN ORDINARIO para el empleado " + codigoempleado + "  ";
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
                        mensaje += "Al empleado " + codigoempleado + " le falta definirle el % de retención, se asume 0  !!! Corrijalo  ";
                    }
                    else valoradescontar2 = acumuladoretefuente2 * double.Parse(estadorfte.Tables[0].Rows[0][1].ToString()) / 100;
                }
                else
                {
                    valoradescontar2 = 0;
                }
            }
            return valoradescontar2;
        }
	}
}
