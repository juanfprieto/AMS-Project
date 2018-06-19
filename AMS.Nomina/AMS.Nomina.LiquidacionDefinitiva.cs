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
using AMS.Tools;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Mail;

namespace AMS.Nomina
{
	public class LiquidacionDefinitiva : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected DropDownList DDLEMPLEADO,DDLMESRETI,DDLANOLIQ,DDLDIARETI,DDLMESLIQ;
		protected DropDownList DDLANORETI,DDLDIALIQ,DDLMOTRETIRO,DLLConceptoLNR;
		protected Panel     panelbaseliquidacion,panelcesantias;
        protected Label     LBFECHAINGRESO, LBIDENTIFICACION, LBCARGO, LBDEPENDENCIA, lbtipopago, LBDTPRIMAS, LBPRIMAAPAGAR, LBBASELIQPRIMA, LabelEmpresa, LabelNitEmpresa, LabelDigitoEmpresa;
		protected Label     lb,lb2,lb3,lbmas1,lbdocref,prueba,lbpag,LBSUELDOPROMEDIO,LBBASELIQUIDACION,LBDIASTRABAJADOS,LBCESAAPAGAR,LBINTAPAGAR,LBPRUEBAS;
		protected Label     LBDTVACACIONES,LBVACAAPAGAR,LBDECUENTOSEMPLEA,LBBASELIQVACA,LBINDEMNIZACION,LBSUELDOCARGO,LBFECHARETIRO,LBNOMBREEMPLEADO, LBFIRMAEMPL, LBIDENTFIRMA, lbindentpaga, LBFIRMAPAGO;
		protected DataGrid  DataGrid2;
		protected DataGrid  DATAGRIDCESAANTERIORES;
		protected DataTable DataTable1,DataTable2;
		string    fechainicio, fechafinal,fechainiciocesaanterior,fechafinalcesaanterior;
		string    DDLQUIN,fechaliquidacion,tipopago,tipoPromedio;
		protected int diasTrabajados;
		protected System.Web.UI.WebControls.Button Button2;
		protected System.Web.UI.WebControls.Button BTNLIQUIDAR;
	
		protected string table;
		protected System.Web.UI.WebControls.Label LBVALORTOTAL;
		protected System.Web.UI.WebControls.PlaceHolder phGrilla;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected Label lbInfo;

		// arreglos  
		protected CNomina o_CNomina = new CNomina();
		protected Varios o_Varios = new Varios();
		protected string mest;
		protected string anot;
		protected string diat;
        protected string tipoContrato;  // cotrato de trabajo
        protected int    mesv, diav, anov;
		protected double diasPrima;
        protected int    diaps, mesps, anops;
        protected DateTime fechaing = new DateTime();
        protected DateTime fechaSalario = new DateTime();
		
		
	
	//	protected int diasVacaciones = 0;
			
	//	protected System.Web.UI.WebControls.TextBox DiasPrimer;
	//	protected System.Web.UI.WebControls.TextBox DiasSegundo;
		protected System.Web.UI.WebControls.PlaceHolder toolsHolder;
		protected System.Web.UI.WebControls.TextBox tbEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.ImageButton ibMail;
		protected System.Web.UI.WebControls.CheckBox CheckBox1;
		protected System.Web.UI.WebControls.Label LBDESCUENTONOVEDADES, LBEPSVACACIONES, LBFONDOVACACIONES;
		
		protected void LiquidacionParcial( object sender , EventArgs e)
		{
            DataGrid2.PageSize  = 20;
			this.preparargrilla_empleadoliquidacion();
			DateTime fechaliq   = new DateTime();
			DateTime fecharet   = new DateTime();
			DateTime fechainices= new DateTime();
			DateTime fechainiprima= new DateTime();
			DateTime fechavaca  = new DateTime();
			Empleado o_Empleado = new Empleado("='" +DDLEMPLEADO.SelectedValue.ToString() +"'");
		
	//		o_Empleado.p_AgregarAFormulario = false;
			double diferenciavacaciones=0;
			o_Empleado.p_DDLANORETI = DDLANORETI;
			o_Empleado.p_DDLDIARETI = DDLDIARETI;
			o_Empleado.p_DDLMESRETI = DDLMESRETI;
			o_Empleado.AsignarParametrosNomina = o_CNomina;
			o_Empleado.AsignarVarios = o_Varios;
			o_Empleado.AsignarDataGrid = this.DataGrid2;
			o_Empleado.AsignarDataTable = DataTable1;
						
			BTNLIQUIDAR.Visible=true;
			fechaing      = Convert.ToDateTime(o_Empleado.p_MEMP_FECHINGRESO);
			fecharet      = Convert.ToDateTime(o_Empleado.p_FechaRetiro);
            fechaSalario  = Convert.ToDateTime(o_Empleado.p_MEMP_FECHSUELACTU);
			DataSet fechaEnLiquidacion = new DataSet();
			
			string query  = @"Select MAX(MPRI_FECHFINA) from DBXSCHEMA.MPRIMAS";
			string auxTime= DBFunctions.SingleData(query);
            try
            {
                fechaliq = Convert.ToDateTime(auxTime);
            }
            catch
            {
                fechaliq = fecharet;
            }

			DateTime fechafinal2= new DateTime();
			DateTime fechaliquidacion2= new DateTime();
			//validacion de fechas 
			fechafinal2   = Convert.ToDateTime(o_Empleado.p_FechaRetiro);
			fechaliquidacion2=Convert.ToDateTime(fechaliquidacion=DDLANOLIQ.SelectedValue+"-"+DDLMESLIQ.SelectedValue+"-"+DDLDIALIQ.SelectedValue);
			fechaing      = Convert.ToDateTime(o_Empleado.p_MEMP_FECHINGRESO);
			fechainices   = new DateTime(fecharet.Year,1,1);

			if (fecharet.Month < 7)
				fechainiprima=new DateTime(fecharet.Year,1,1);
			else 
				fechainiprima=new DateTime(fecharet.Year,7,1);

			if (fechainices < fechaing)
				fechainices = fechaing;
			if (fechainiprima < fechaing)
				fechainiprima = fechaing;

            if (fechafinal2 <= fechaliquidacion2)
            {
                panelbaseliquidacion.Visible = true;
                panelcesantias.Visible = true;
                DDLQUIN = o_CNomina.CNOM_QUINCENA;
                if (o_Varios.p_NumeroQuincena == "")
                {
                    Utils.MostrarAlerta(Response, "No se ha encontrado Registros de ningun pago,porfavor verifique.");
                }

                // se realizan todas las liquidaciones
                int anov = fecharet.Year;
                int anoi = anov;
                if (fechaing.Year < anov) anoi = anov - 1;
                //JFSC 21042008 validación para liquidar cesantías sólo del año
                if (this.CheckBox1.Checked == true)
                {
                    o_Empleado.LiquidoCesantias = true;
                    fechaliq = new DateTime(fecharet.Year, 1, 1);
                }
                else
                {
                    fechaliq = new DateTime(fecharet.Year - 1, 1, 1);
                    if (fechaliq < fechaing)
                        fechaliq = fechaing;
                }
                //	o_Empleado.PromedioUltimoAno(anoi,anov,diav,mesv);

                diaps = Convert.ToInt32(DDLDIARETI.SelectedValue);
                mesps = Convert.ToInt32(DDLMESRETI.SelectedValue);
                anops = Convert.ToInt32(DDLANORETI.SelectedValue);

                diav = diaps;
                mesv = mesps;
                anov = anops;

                o_Empleado.PromedioUltimoAno(anoi, anov, diav, mesv);

                int dianm = 1;

                if (o_CNomina.CNOM_FORMPAGO == "3" && o_CNomina.CNOM_QUINCENA.ToString().Trim() == "2")
                    dianm = 01;
                else
                    if (o_CNomina.CNOM_FORMPAGO != "1" && o_CNomina.CNOM_QUINCENA.ToString().Trim() == "2")
                        dianm = 16;
                int mesnm = Convert.ToInt32(o_CNomina.CNOM_MES);
                int anonm = Convert.ToInt32(o_CNomina.CNOM_ANO);

                double liquDiasTrabajados = 0;
                string liquidaNomina = "Definitiva";
                if ((anonm * 360 + mesnm * 30 + dianm) <= (anov * 360 + mesv * 30 + diav + 1))
                {
                    //if (!o_Empleado.EsPeriodoLiquidado)
                    {
                        if ((Convert.ToDouble(o_Empleado.p_MEMP_SUELACTU)) >= ((Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU)) * 2))
                            liquDiasTrabajados = o_Empleado.LiquidarMesEnCurso(mesv, anov, diav, false, liquidaNomina);
                        else
                            liquDiasTrabajados = o_Empleado.LiquidarMesEnCurso(mesv, anov, diav, true, liquidaNomina);
                    }
                }
                //     o_Empleado.LiquidacionCesantias(Int32.Parse(DiasPrimer.Text), Int32.Parse(DiasSegundo.Text), diav);

                // 1 = Indefinido,  2 = Termino Fijo,  3 = Aprendiz SENA,  4 = Pensionado
                tipoContrato = DBFunctions.SingleData("Select tcon_contrato from mempleado where MEMP_CODIEMPL = '" + o_Empleado.p_MEMP_CODIEMPL.ToString().Trim() + "' ");

                int diasVacaciones = o_Empleado.LiquidacionVacaciones(diaps, mesps, anops);
                double cesantiasanteriores = 0;
                double intcesantiasanteriores = 0;
                DataSet cesaanteriores = new DataSet();
                string Sql = "SELECT MCESA.MCES_FECHINIC, MCESA.MCES_FECHFINA, DCESA.DCES_VALOCESA, DCESA.DCES_INTECESA, DCESA.MEMP_CODIEMP, DCESA.DCES_DIASTRAB FROM DBXSCHEMA.DCESANTIAS DCESA , DBXSCHEMA.MCESANTIAS MCESA WHERE DCESA.MEMP_CODIEMP='" + o_Empleado.p_MEMP_CODIEMPL + "' and mcesa.mces_secuencia=dcesa.mces_secuencia";

                if (tipoContrato != "3" && tipoContrato != "5")
                    DBFunctions.Request(cesaanteriores, IncludeSchema.NO, Sql);

                if (cesaanteriores.Tables.Count > 0 && cesaanteriores.Tables[0].Rows.Count > 0)
                {
                    preparargrilla_cesaanteriores();
                    for (int i = 0; i < cesaanteriores.Tables[0].Rows.Count; i++)
                    {
                        DateTime fechainiciocesaanterior2 = new DateTime();
                        DateTime fechafinalcesaanterior2 = new DateTime();
                        fechainiciocesaanterior2 = Convert.ToDateTime(cesaanteriores.Tables[0].Rows[i][0].ToString());
                        fechafinalcesaanterior2 = Convert.ToDateTime(cesaanteriores.Tables[0].Rows[i][1].ToString());
                        fechainiciocesaanterior = fechainiciocesaanterior2.Date.ToString("yyyy-MM-dd");
                        fechafinalcesaanterior = fechafinalcesaanterior2.Date.ToString("yyyy-MM-dd");
                        this.ingresardatos_cesaanteriores(fechainiciocesaanterior, fechafinalcesaanterior, double.Parse(cesaanteriores.Tables[0].Rows[i][2].ToString()), double.Parse(cesaanteriores.Tables[0].Rows[i][3].ToString()), double.Parse(cesaanteriores.Tables[0].Rows[i][5].ToString()));
                        cesantiasanteriores = cesantiasanteriores + double.Parse(cesaanteriores.Tables[0].Rows[i][2].ToString());
                        intcesantiasanteriores = intcesantiasanteriores + double.Parse(cesaanteriores.Tables[0].Rows[i][3].ToString());
                    }
                }
                diasTrabajados =
                    ((fecharet.Year * 360 + fecharet.Month * 30 + fecharet.Day) - (fechaing.Year * 360 + fechaing.Month * 30 + fechaing.Day) + 1);
                if(fecharet.Month == 2)
                {
                    if (fecharet.Day == 29)
                        diasTrabajados += 1;
                    else if (fecharet.Day == 28)
                            diasTrabajados += 2;
                }
                int diferenciaDias = diasTrabajados;
                int diasDespuesPagarCesantias = 0;
                int diferenciaDias2 = 0;

                if (this.CheckBox1.Checked == false)
                { 
                    diasDespuesPagarCesantias =
                        (fecharet.Year * 360 + fecharet.Month * 30 + fecharet.Day)
                       - (fechaliq.Year * 360 + fechaliq.Month * 30 + fechaliq.Day) + 1;
                    diferenciaDias2 = diasDespuesPagarCesantias;
                }
                else diferenciaDias2 =
                    ((fecharet.Year * 360 + fecharet.Month * 30 + fecharet.Day) - (fechainices.Year * 360 + fechainices.Month * 30 + fechainices.Day) + 1);
		  		diasPrima =
		  			((fecharet.Year*360 + fecharet.Month*30 + fecharet.Day)-(fechainiprima.Year*360 + fechainiprima.Month*30 + fechainiprima.Day)+ 1);
                if (fecharet.Month == 2)
                {
                    if (fecharet.Day == 29)
                    {
                        diasDespuesPagarCesantias += 1;
                        diferenciaDias2 += 1;
                        diasPrima += 1;
                    }
                    else if (fecharet.Day == 28)
                    {
                        diasDespuesPagarCesantias += 2;
                        diferenciaDias2 += 2;
                        diasPrima += 2;
                    }
                }

                if (diferenciaDias>diferenciaDias2)diferenciaDias =diferenciaDias2;
				diferenciaDias2  =diferenciaDias;
                // LOS DIAS DE SUSPENSION se deben descontar de los dias de cesantias
                DataSet dss = new DataSet(); 
                String codEmpleado = o_Empleado.p_MEMP_CODIEMPL;
                Empleado empleadoLiqu = new Empleado("='" + codEmpleado + "'");
                dss =  empleadoLiqu.cargarSuspensiones(codEmpleado);
                Int16 diasSuspencion = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    if (dss.Tables[0].Rows[i][4].ToString() == "H" && Convert.ToInt16(dss.Tables[0].Rows[i][2].ToString()) >= Convert.ToInt16(fechainices.Year.ToString()) ) 
                        diasSuspencion += Convert.ToInt16(dss.Tables[0].Rows[i]["DIA"].ToString());
                }

                diasDespuesPagarCesantias = diferenciaDias - diasSuspencion;
                
				double vlrcesantias = 0;
				double prr          = Convert.ToDouble(o_Empleado.p_MEMP_SUELACTU);
				double prr2         = ((Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU))*2);
				double intersXCesantias=0;
				
				fechafinal          = fechafinal2.ToString();
				tipoPromedio        = "Cesantias";
				string tipoSalario  = o_Empleado.p_TSAL_SALARIO;  // 1=INTEGRAL    2=VARIABLE    3=FIJO NO INTEGRAL
                if(tipoContrato != "3" && tipoContrato != "5") //sena
				    vlrcesantias        = salario_promedio (o_Empleado.p_MEMP_CODIEMPL, fechafinal, tipoSalario, tipoPromedio);
                
                string nombreEmpresa= DBFunctions.SingleData("select cemp_nombre from dbxschema.cempresa");
                string nitEmpresa   = DBFunctions.SingleData("select mnit_nit from dbxschema.cempresa");
                string digitoEmpresa= DBFunctions.SingleData("select cemp_digito from dbxschema.cempresa");
		      
                LabelEmpresa.Text   = nombreEmpresa;
                LabelNitEmpresa.Text= nitEmpresa;
                LabelDigitoEmpresa.Text = digitoEmpresa;
             
                LBBASELIQUIDACION.Text = vlrcesantias.ToString("C");


                if (tipoContrato != "3" && tipoContrato != "5") //sena
                {
                    vlrcesantias = o_Empleado.liquidaCesantiasEMP(diasDespuesPagarCesantias, vlrcesantias.ToString());
                    vlrcesantias = Math.Round(vlrcesantias, 0);
                    intersXCesantias = o_Empleado.interesCesantiaEMP(vlrcesantias, diasDespuesPagarCesantias);
                }

                LBCESAAPAGAR.Text   = vlrcesantias.ToString("C");
             
				double primaPagar   = 0;
				diasPrima    = Math.Round(diasPrima * 15 / 180,8);
				double salBasico=Convert.ToDouble(o_Empleado.p_MEMP_SUELACTU);
 
				fechafinal   = fechafinal2.ToString();
				tipoPromedio = "PrimaServicios";
				tipoSalario  = o_Empleado.p_TSAL_SALARIO;  // 1=INTEGRAL    2=VARIABLE    3=FIJO NO INTEGRAL
				salBasico    = salario_promedio (o_Empleado.p_MEMP_CODIEMPL, fechafinal, tipoSalario, tipoPromedio);
				LBBASELIQPRIMA.Text = salBasico.ToString("C");
                if(salBasico == 0)
					salBasico=Convert.ToDouble(o_Empleado.p_MEMP_SUELACTU);
                if (tipoContrato != "3" && tipoContrato != "5") //sena
                {
                    if (o_Empleado.p_TSUB_CODIGO == "1" && salBasico <= prr2)
                    {
                        primaPagar = Math.Round(salBasico / 30 * diasPrima, 0);
                    }
                    else
                    {
                        primaPagar = Math.Round(salBasico / 30 * diasPrima, 0);
                    }
                }

                string consultaMestotal = @" Select coalesce(SUM(dpri_valorprima),0)  
                      from  dbxschema.dprimas d, dbxschema.mprimaS m    
                      Where d.dpri_secuencia = m.mpri_secuencia   
                     	AND d.dpri_CODIEMP = '" + o_Empleado.p_MEMP_CODIEMPL + @"'  
                    	and " + anov + "  = Year(date(m.mpri_fechfina)) and " + mesv + " = Month(date(m.mpri_fechfina)); ";

                double valorPrimayapagada = Double.Parse(DB.DBFunctions.SingleData(consultaMestotal));

                primaPagar = primaPagar - valorPrimayapagada;
				 
				int diasQueTrabajo=0;
		 		
				if(o_Empleado.detectarQuincena(o_Empleado.p_FactorCesantias)=="1")
				{
					diasQueTrabajo    = o_Empleado.p_FactorCesantias;
				}
				else 
					diasQueTrabajo    = o_Empleado.p_FactorCesantias-15;
				LBDIASTRABAJADOS.Text = diasDespuesPagarCesantias.ToString();

				double dtoYpagosPer   = o_Empleado.pagosDescuentosEMP(Convert.ToInt16(DDLMESLIQ.SelectedValue.ToString()),Convert.ToInt16(DDLANORETI.SelectedValue.ToString()),Convert.ToInt16(DDLDIALIQ.SelectedValue.ToString()));
				double descuentoEMP   = o_Empleado.prestamosEMP_total_EMP();//DESCUENTOS ppuntop 

				LBPRIMAAPAGAR.Text    = primaPagar.ToString("C");//ppuntop
				LBFECHAINGRESO.Text   = fechaing.ToString("yyyy-MM-dd");
				LBIDENTIFICACION.Text = o_Empleado.p_MEMP_CODIEMPL;	
                LBIDENTFIRMA.Text     = o_Empleado.p_MEMP_CODIEMPL;
                LBCARGO.Text          = o_Empleado.p_PCAR_NOMBCARG;
				LBDEPENDENCIA.Text    = o_Empleado.p_PDEP_NOMBDPTO;
				LBSUELDOCARGO.Text    = Convert.ToDouble(o_Empleado.p_MEMP_SUELACTU).ToString("C");
				LBFECHARETIRO.Text    = fecharet.ToString("yyyy-MM-dd");
				LBNOMBREEMPLEADO.Text = o_Empleado.p_MNIT_APELLIDOS.Trim() + " " + o_Empleado.p_MNIT_APELLIDO2.Trim()+ " " + o_Empleado.p_MNIT_NOMBRES.Trim() + " " + o_Empleado.p_MNIT_NOMBRE2;
                LBFIRMAEMPL.Text      = o_Empleado.p_MNIT_APELLIDOS.Trim() + " " + o_Empleado.p_MNIT_APELLIDO2.Trim()+ " " + o_Empleado.p_MNIT_NOMBRES.Trim() + " " + o_Empleado.p_MNIT_NOMBRE2;
                lbindentpaga.Text     = DBFunctions.SingleData("SELECT CNOM_NOMBPAGA FROM CNOMINA");
                LBFIRMAPAGO.Text      = DBFunctions.SingleData("SELECT VM.NOMBRE FROM VMNIT VM, CNOMINA C WHERE VM.MNIT_NIT = C.CNOM_NOMBPAGA");
                this.lbtipopago.Text  = o_Empleado.p_MEMP_FORMPAGO;
				
				string ultimaVacacacion=o_Varios.vacacionesDias(o_Empleado.p_MEMP_CODIEMPL);
				if(ultimaVacacacion!=null)
				{
					fechavaca = Convert.ToDateTime(ultimaVacacacion);
					TimeSpan diasDespuesvacaciones = fecharet - fechavaca;
					diferenciavacaciones = diasDespuesvacaciones.Days;
				}
				else  
					diferenciavacaciones = diasTrabajados;
				double copydiferenciavacaciones= diferenciavacaciones;
				copydiferenciavacaciones = diasVacaciones;
				diferenciavacaciones     = diasVacaciones;
				 
				if(diferenciavacaciones>360)
				{					
					diferenciavacaciones=(diferenciavacaciones/30);
					Math.Round(Convert.ToDouble(diferenciavacaciones),0);
					diferenciavacaciones=diferenciavacaciones*30;					
				}
				
				else if(diferenciavacaciones<360)
				{
					    diferenciavacaciones=copydiferenciavacaciones;	
				}

                //           Se restan las vacaciones que el empleado ha disfrutado

                double liquidacionDeVacaciones = 0, valorepsVacaciones = 0, valorfondoVacaciones = 0; ;
                if (tipoContrato != "3" && tipoContrato != "5") //sena
                {
                    liquidacionDeVacaciones = Convert.ToDouble(o_Empleado.p_MEMP_SUELACTU);
				    double diasVacacionesDisfrutados = Convert.ToDouble(DBFunctions.SingleData("select coalesce(sum(mvac_diasvacadisf),0) from dbxschema.mvacaciones where memp_codiemp = '"+o_Empleado.p_MEMP_CODIEMPL+"' "));
		
				    fechafinal          = fechafinal2.ToString();
				    tipoPromedio        = "Vacaciones";
                    if ((tipoPromedio == "Vacaciones" || tipoPromedio == "VacacionesDefinitivas") && tipoSalario!="2") // salario fijo o integral
                    {
                        salBasico       = Convert.ToDouble(o_Empleado.p_MEMP_SUELACTU);  // art 192 c.laboral si es fijo se toma el salario actual sin extras
                    }
                    else 
 		                salBasico       = salario_promedio (o_Empleado.p_MEMP_CODIEMPL, fechafinal, tipoSalario, tipoPromedio);
				    diferenciavacaciones = Math.Round(((Convert.ToDouble(diasTrabajados) * 15) / 360) - (diasVacacionesDisfrutados),2);
                    liquidacionDeVacaciones = Math.Round(salBasico / 30 * diferenciavacaciones, 0);
                    valorepsVacaciones  = Math.Round(liquidacionDeVacaciones * Double.Parse(o_CNomina.CNOM_PORCEPSEMPL) * 0.01,0);
                    valorfondoVacaciones = Math.Round(liquidacionDeVacaciones * Double.Parse(o_CNomina.CNOM_PORCFONDEMPL) * 0.01, 0);
                }

                LBBASELIQVACA.Text  = salBasico.ToString("C");
				LBDTVACACIONES.Text = diferenciavacaciones.ToString();
				LBVACAAPAGAR.Text   = liquidacionDeVacaciones.ToString("C");
                LBEPSVACACIONES.Text = valorepsVacaciones.ToString("C");
                LBFONDOVACACIONES.Text = valorfondoVacaciones.ToString("C");

                o_Varios.vacacionesDias(o_Empleado.p_MEMP_CODIEMPL);
				
				intersXCesantias    = Math.Round(intersXCesantias,0) ;
				LBINTAPAGAR.Text    = intersXCesantias.ToString("C");
				diasPrima           = Math.Round(diasPrima,0);
				LBDTPRIMAS.Text     = diasPrima.ToString();

                double valorindemnizacion = 0;
				if (DDLMOTRETIRO.SelectedValue.ToString() == "2")
				{
					o_Empleado.LiquidarIndemnizacion();
					valorindemnizacion = o_Empleado.p_VIndemnizacion;
					if (valorindemnizacion == -1)
					{
                        Utils.MostrarAlerta(Response, "Error en fechas de terminación y fecha de retiro del empleado, por favor verifique.");
					}
				}
                double vlrNovedades = 0;
              //  vlrNovedades = o_Empleado.novedades_total_EMP(o_Empleado.p_FechaRetiro);
              		
               	double totalGeneral   = vlrcesantias+intersXCesantias+primaPagar+liquidacionDeVacaciones-dtoYpagosPer-descuentoEMP+liquDiasTrabajados+vlrNovedades-valorepsVacaciones - valorfondoVacaciones + valorindemnizacion;
				  
				double descontare     = dtoYpagosPer+descuentoEMP;

                LBDESCUENTONOVEDADES.Text = vlrNovedades.ToString("C");
				LBDECUENTOSEMPLEA.Text= descontare.ToString("C");
				LBVALORTOTAL.Text     = totalGeneral.ToString("C");
				LBINDEMNIZACION.Text  = valorindemnizacion.ToString("C");
				Session["DataTable1"] = DataTable1;
			}
			else
			{
                Utils.MostrarAlerta(Response, "La fecha de liquidación no puede ser menor a la fecha de retiro del empleado, porfavor  verifique.");
			}
			Session["rep"]=RenderHtml();
			Session["codigoempleado"] = o_Empleado.p_MEMP_CODIEMPL;
		}

				
		protected double salario_promedio (string Empleado, string fechafinal, string tipoSalario, string tipoPromedio)
		{
			DataSet  valorsueldopromedio = new DataSet();
			DateTime fechares            = new DateTime();
			DateTime fecharesx           = new DateTime();
			Double   sueldoPromedio      = 0;
					
			fechares  = Convert.ToDateTime(fechafinal);
         //   fechares  = new DateTime(fechares.Year, fechares.Month, 1);
			fecharesx = Convert.ToDateTime(fechafinal);

			if (tipoPromedio == "Vacaciones" || tipoPromedio == "VacacionesDefinitivas")  //  12 meses salario varible y 3 meses salario fijo
			{
                if (tipoSalario == "2")
                {
                    fechares = fechares.AddMonths(-12);
                    fechares = fechares.AddDays(1);
                }
                else
                    fechares = fechares.AddMonths(-3);
			}
            else if(tipoPromedio=="PrimaServicios") //  desde 1 de enero/julio salario varible y 3 meses SEMESTRE salario fijo 
				 {
				//	 if(tipoSalario=="2")
					 {
						 if  (fechares.Month >= 7)
						      fechares = new DateTime(fechares.Year,7,1);
						 else fechares = new DateTime(fechares.Year,1,1);
					 }
				//	 else
				//	 {
				//		 if  (fechares.Month >= 10 || (fechares.Month >= 04 && fechares.Month <= 06 )) 
				//		     fechares=fechares.AddMonths(-3);
				//		 else if  (fechares.Month >= 07)
				//				   fechares = new DateTime(fechares.Year,7,1);
				//			  else fechares = new DateTime(fechares.Year,1,1);
				//	 }
				 }
			else if(tipoPromedio=="Cesantias") //  desde 1 de enero/julio salario varible y 3 meses salario fijo 
				 {
                    /*
					 if (tipoSalario=="2")
					 {
                        if (this.CheckBox1.Checked == false && fecharesx < Convert.ToDateTime(fecharesx.Year + "-02-14"))
                        {
                            fechares = fechares.AddYears(-1);
                            fechares = new DateTime(fechares.Year, 1, 1);
                        }
                        else
                            fechares = fechares.AddYears(-1);
                     }
					 else
					 {
                        if (fechaSalario >= fecharesx.AddMonths(-3))
                        {
                             //if (fechaSalario.Year == fecharesx.Year && fechaSalario.Month == 1 && this.CheckBox1.Checked == true)
                             //   fechares = fechaSalario;
                             //else 
                             //   fechares = fecharesx.AddYears(-1);
                             fechares = fecharesx.AddMonths(-3);  // SI EL SALAIRO CAMBIO EN LOS 3 ULTIMOS MESES SE PROMEDIA
                        }
                        else
                        {
                             if (this.CheckBox1.Checked == false)
                                 fechares = fecharesx.AddMonths(-3);
                             else
                                 if (fechares.Month >= 04)
                                     fechares = fechares.AddMonths(-3);
                                 else
                                     fechares = new DateTime(fechares.Year,1,1);
                                     //fechares = fechares.AddMonths(-3);
                         }
                         fechares = new DateTime(fechares.Year, fechares.Month, 1); 
					 }
                     */
                     fechares = new DateTime(fechares.Year, 1, 1); // PARA TODOS LOS TIPOS DE SALARIO PORQUE LA CESANTIA SE CONSIGNO EN EL FONDO
                     if (this.CheckBox1.Checked == false && fecharesx < Convert.ToDateTime(fecharesx.Year + "-02-14"))
                     {
                          fechares = fechares.AddYears(-1);  // por si NO se ha consignado la censantia y aun no es la fecha se toma desde el año anterior
                     }
                }
                if (fechares < fechaing)
                    fechares = fechaing;
			string fechares1 = fechares.Date.ToString("yyyy-MM-dd");
			string fechares2 = fecharesx.Date.ToString("yyyy-MM-dd");
			string subsidioTransporte = DBFunctions.SingleData("select cnom_concsubtcodi from dbxschema.cnomina");
		 					
			string tipoProm  = "";
			if(tipoPromedio.Equals("Vacaciones"))tipoProm=" AND PCON.tres_afecVACACION = 'S' ";
         //   else if (tipoPromedio.Equals("VacacionesDefinitivas")) tipoProm = " AND PCON.tres_afecLIQUIDDEFINITIVA = 'S' ";
		    else if(tipoPromedio.Equals("PrimaServicios"))tipoProm=" AND PCON.tres_afecPRIMA = 'S' ";
			else if(tipoPromedio.Equals("Cesantias"))tipoProm=" AND PCON.tres_afecCESANTIA = 'S' ";
            string query = @"SELECT * FROM (SELECT sum(D.DQUI_APAGAR-D.DQUI_ADESCONTAR),sum(dqui_canteventos),
	   	 	  						D.MEMP_CODIEMPL, pcon_desccant,
									M.MQUI_ANOQUIN,
									m.MQUI_MESQUIN,
									CASE WHEN M.MQUI_MESQUIN<=9 
					  					THEN CAST((CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-0' 
						   							CONCAT RTRIM(CAST(MQUI_MESQUIN AS CHAR(2))) 
													CONCAT '-' CONCAT CASE WHEN M.MQUI_TPERNOMI = 1 THEN '15' 
														WHEN M.MQUI_TPERNOMI = 2 THEN CASE WHEN M.MQUI_MESQUIN=2 THEN '28' ELSE '30' END END) AS DATE) 
										ELSE CAST((CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-' 
						   							CONCAT RTRIM(CAST(MQUI_MESQUIN AS CHAR(2))) 
													CONCAT '-' CONCAT CASE WHEN M.MQUI_TPERNOMI = 1 THEN '15' 
														WHEN M.MQUI_TPERNOMI = 2 THEN '30' END) AS DATE) END 
									AS FECHA,
									D.pcon_concepto, pcon_nombconc 
									FROM dbxschema.DQUINCENA D,
										 dbxschema.MQUINCENAS M,
										 dbxschema.pconceptonomina PCON,
										 dbxschema.cnomina cn,
										 dbxschema.mempleado me 
									WHERE D.MQUI_CODIQUIN=M.MQUI_CODIQUIN 
									  AND D.pcon_concepto = PCON.pcon_concepto " + tipoProm +@"
									  AND D.memp_codiempl = '"+Empleado+ @"' AND MQUI_MESQUIN > 0
									  and d.memp_codiempl = me.memp_codiempl
									  and ((me.tsal_salario = 2) or (cn.cnom_concsalacodi <> d.pcon_concepto and me.tsal_salario <> 2))

                                  --    and D.DQUI_APAGAR > 0 
									group by  D.MEMP_CODIEMPL,pcon_desccant,
						  					M.MQUI_ANOQUIN,
											m.MQUI_MESQUIN,
											M.MQUI_TPERNOMI,
											D.pcon_concepto, pcon_nombconc
											) as sources 
				WHERE sources.fecha >= '" + fechares1+@"' 
				  AND sources.fecha <= '"+fechares2+@"'";
	     
			DBFunctions.Request(valorsueldopromedio,IncludeSchema.NO,query);

			DateTime mesante    = new DateTime();
            double valorPagado  = 0;
            double diasPagados  = 0;
            double diasPagoQna  = 0;
            string aux          = "";
            string siPromedio   = "";

            diasPagados =
                    ((Convert.ToDateTime(fechares2).Year * 360 + Convert.ToDateTime(fechares2).Month * 30 + Convert.ToDateTime(fechares2).Day) - (Convert.ToDateTime(fechares1).Year * 360 + Convert.ToDateTime(fechares1).Month * 30 + Convert.ToDateTime(fechares1).Day) + 1);
		    if(Convert.ToDateTime(fechares2).Month == 2)
            {
                if (Convert.ToDateTime(fechares2).Day == 29)
                    diasPagados += 1;
                else if (Convert.ToDateTime(fechares2).Day == 28)
                        diasPagados += 2;
            }
            if (DataTable1.Columns.Count == 9)
            {
                if (DataTable1.Rows.Count > 1)
                {
                    for (int i = 0; i <= DataTable1.Rows.Count - 2; i++)
                    {
                        aux = DataTable1.Rows[i][4].ToString().Replace("$", "");
                        aux = aux.Replace(",", "");
                        siPromedio = DBFunctions.SingleData("select pcon.pcon_concepto, pcon_desccant from dbxschema.pconceptonomina pcon where pcon.pcon_concepto = '" + DataTable1.Rows[i][0].ToString().Trim() + "' "+ tipoProm + " and ((me.tsal_salario = 2) or (cn.cnom_concsalacodi <> d.pcon_concepto and me.tsal_salario <> 2));");
		                if(DataTable1.Rows[i][0].ToString().Trim() == siPromedio)
                        {
                           valorPagado += Convert.ToDouble(aux); 
                            if(DataTable1.Rows[i][1].ToString().Trim() == "1")
                                 diasPagoQna += Convert.ToDouble(aux);
                        }
                    }
                }
            }
       				
            // se deben acumular los dias pagados desde el inicio de la quincena hasta la fecha de liquidacion

			if(valorsueldopromedio.Tables[0].Rows.Count!=0)
			{
				for (int i=0;i<valorsueldopromedio.Tables[0].Rows.Count;i++)
				{
					sueldoPromedio += double.Parse(valorsueldopromedio.Tables[0].Rows[i][0].ToString());
			 		string TipoEvento = valorsueldopromedio.Tables[0].Rows[i][3].ToString();
			 //		if (TipoEvento == "4" && valorsueldopromedio.Tables[0].Rows[i][7].ToString() != subsidioTransporte)
			 //			CantidadEventosDias += int.Parse(valorsueldopromedio.Tables[0].Rows[i][1].ToString());
				}
           	}
            sueldoPromedio = Math.Round(((sueldoPromedio + valorPagado) / (diasPagados + diasPagoQna)) * 30, 0);
            if (tipoSalario != "2")
            {
                sueldoPromedio += Convert.ToDouble(DBFunctions.SingleData("select memp_suelactU from mempleado where memp_codiempL = '" + Empleado + "' "));
            }
            if (sueldoPromedio < Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU.ToString())) // nadie puede ganar menos que el Minimo.
            {
                sueldoPromedio = Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU.ToString());
                if (sueldoPromedio < Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU) * Convert.ToDouble(o_CNomina.CNOM_TOPEPAGOAUXITRAN))
                    sueldoPromedio += Convert.ToDouble(o_CNomina.CNOM_SUBSTRANSACTU);
            }
		    return sueldoPromedio;
		}
 
		protected void preparargrilla_cesaanteriores()
		{			
			DataTable2 = new DataTable();
			DataTable2.Columns.Add(new DataColumn("FECHA INICIO",System.Type.GetType("System.String")));
			DataTable2.Columns.Add(new DataColumn("FECHA FINAL",System.Type.GetType("System.String")));
			DataTable2.Columns.Add(new DataColumn("CESANTIAS",System.Type.GetType("System.Double")));
			DataTable2.Columns.Add(new DataColumn("INTERESES DE CESANTIA",System.Type.GetType("System.Double")));
			DataTable2.Columns.Add(new DataColumn("DIAS TRABAJADOS",System.Type.GetType("System.Double")));
		}

		protected void ingresardatos_cesaanteriores(string fechaInicio,string fechaFinal,double Cesantias,double interesescesantia,double diastrabajados)
		{
			DataRow fila=DataTable2.NewRow();
			fila["FECHA INICIO"]=fechaInicio;
			fila["FECHA FINAL"] =fechaFinal;
			fila["CESANTIAS"]   =Cesantias;
			fila["INTERESES DE CESANTIA"]=interesescesantia;
			fila["DIAS TRABAJADOS"]=diastrabajados;
			DataTable2.Rows.Add(fila);
			DATAGRIDCESAANTERIORES.DataSource=DataTable2;
			DATAGRIDCESAANTERIORES.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DATAGRIDCESAANTERIORES);
			DatasToControls.JustificacionGrilla(DATAGRIDCESAANTERIORES,DataTable2);						
		}

		protected void preparargrilla_empleadoliquidacion()
		{
			DataTable1 = new DataTable();
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

		protected void Page_Load(object sender , EventArgs e)
		{
			DATAGRIDCESAANTERIORES.Visible = true;
			if (!IsPostBack)
			{
				DateTime diaActual = new DateTime();
				mest = o_CNomina.TMES_NOMBRE;
				anot = o_CNomina.PANO_DETALLE;
				diat = diaActual.Day.ToString();
				mesv = Int32.Parse(o_CNomina.CNOM_MES);
				anov = Int32.Parse(o_CNomina.CNOM_ANO);
				diav = diaActual.Day;

				tipopago=o_CNomina.TOPCI_DESCRIPCION;
				o_Varios.llenarListaEmpleados(DDLEMPLEADO);
				o_Varios.llenarListaAños(DDLANORETI,anot);
				o_Varios.llenarListaAños(DDLANOLIQ,anot);
				o_Varios.llenarListaMeses(DDLMESRETI,mest);
				o_Varios.llenarListaMeses(DDLMESLIQ,mest);
				o_Varios.llenarListaDias(DDLDIARETI,diat);
				o_Varios.llenarListaDias(DDLDIALIQ,diat);
				o_Varios.llenarMotivoRetiro(DDLMOTRETIRO);
				BTNLIQUIDAR.Visible=false;
				this.LBINTAPAGAR.Text="$0";
				this.LBPRIMAAPAGAR.Text="$0";
				this.LBVACAAPAGAR.Text="$0";
				this.LBCESAAPAGAR.Text="$0";
				if(Request.QueryString["el"]!=null)
                Utils.MostrarAlerta(Response, "Empleado Liquidado Satisfactoriamente");
			}
			else
			{
				mesv = Int32.Parse(DDLMESRETI.SelectedValue);
				anov = Int32.Parse(DDLANORETI.SelectedValue);
				diav = Int32.Parse(DDLDIARETI.SelectedValue);

				if (Session["fechainicio"]!=null)
					fechainicio=(string)Session["fechainicio"];
				if (Session["fechafinal"]!=null)
					fechafinal=(string)Session["fechafinal"];
			}
		}

		protected string RenderHtml()
		{
			StringBuilder SB= new StringBuilder();
			StringWriter  SW= new StringWriter(SB);
			HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			phGrilla.RenderControl(htmlTW);
			return SB.ToString();
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			MailMessage MyMail = new MailMessage();
			MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
			MyMail.To   = tbEmail.Text;
			MyMail.Subject = "Proceso : "+DBFunctions.SingleData("SELECT remarks FROM SYSIBM.SYSTABLES WHERE name='"+table+"'");
			MyMail.Body = (RenderHtml());
			MyMail.BodyFormat = MailFormat.Html;
			try
			{
				SmtpMail.Send(MyMail);
			}
			catch(Exception e)
			{
				lbInfo.Text = e.ToString();
			}
		}
	
		protected void realizar_liquidacion(Object  Sender, EventArgs e)
		{
			DataTable1  = new DataTable();
			DataTable1  =(DataTable)Session["DataTable1"];
			ArrayList sql = new ArrayList();
			string codigoempleado= (string)Session["codigoempleado"];
            Utils.MostrarAlerta(Response, "codigo empleado " + codigoempleado + " ");
			int i,desccantidad=0;
			DataSet cnomina= new DataSet();
			
			DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");	
			string lbdocref=cnomina.Tables[0].Rows[0][0]+"-"+cnomina.Tables[0].Rows[0][1]+"-"+cnomina.Tables[0].Rows[0][2];
			double codquincena=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+cnomina.Tables[0].Rows[0][0].ToString()+" and MQUI.mqui_mesquin="+cnomina.Tables[0].Rows[0][1].ToString()+" and MQUI.mqui_tpernomi="+cnomina.Tables[0].Rows[0][2].ToString()+" "));
			
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
					try
					{
						int valor=int.Parse(DataTable1.Rows[i][6].ToString()==string.Empty?"0":DataTable1.Rows[i][6].ToString());
						if(valor<=4)
							desccantidad=valor;
					}
					catch(Exception e1)
					{
						desccantidad=0;
					}
					if (DataTable1.Rows[i][5].ToString()=="DIAS")
						desccantidad=1;
					else if (DataTable1.Rows[i][5].ToString()=="HORAS")
						    desccantidad=2;    
					else if (DataTable1.Rows[i][5].ToString()=="MINUTOS")
						    desccantidad=3;
				//	if (DataTable1.Rows[i][5].ToString()=="PESOS M/CTE")
					else 
                        desccantidad=4;					
						
					//inserto en la tabla de detalles de quincena.
					sql.Add("insert into dquincena values(DEFAULT,"+codquincena+",'"+codigoempleado+"','"+DataTable1.Rows[i][0].ToString()+"',"+DataTable1.Rows[i][2].ToString()+","+DataTable1.Rows[i][3].ToString()+","+DataTable1.Rows[i][4].ToString()+","+DataTable1.Rows[i][5].ToString()+","+desccantidad+" ,"+DataTable1.Rows[i]["SALDO"].ToString()+",'"+DataTable1.Rows[i][8].ToString()+"','L')");
					string borrar7689="insert into dquincena values(DEFAULT,"+codquincena+",'"+codigoempleado+"','"+DataTable1.Rows[i][0].ToString()+"',"+DataTable1.Rows[i][2].ToString()+","+DataTable1.Rows[i][3].ToString()+","+DataTable1.Rows[i][4].ToString()+","+DataTable1.Rows[i][5].ToString()+","+desccantidad+" ,"+DataTable1.Rows[i]["SALDO"].ToString()+",'"+DataTable1.Rows[i][8].ToString()+"','L');";
					
					//valido si el concepto es un prestamo.
					if (DataTable1.Rows[i][8].ToString()!= lbdocref && DataTable1.Rows[i][8].ToString()!="--")
					{
						//actualizar las cuotas pagadas de mprestamo
						sql.Add("update mprestamoempleados set mpre_cuotpaga =mpre_cuotpaga+1 ,mpre_valopaga=mpre_valopaga+"+DataTable1.Rows[i]["A PAGAR"].ToString()+" where mpre_numelibr="+DataTable1.Rows[i][6].ToString()+" ");
					}					
				}				
			}
            sql.Add("update dbxschema.mempleado set test_estado='4', memp_fechretiro = '" + LBFECHARETIRO.Text + "' where memp_codiempl='" + codigoempleado + "'");
			
			DataSet conceptos = new DataSet();
            DBFunctions.Request(conceptos, IncludeSchema.NO, "select cnom_conccesacodi,cnom_concintecesacodi,cnom_concprimnormcodi,cnom_concvacacodi,cnom_concINDEMNIZACION,CNOM_CONCEPSCODI, CNOM_CONCFONDCODI from dbxschema.cnomina");
			// SI EL EMPLEADO NO TIENE DIAS LIQUIDADOS LE PONGO CERO A DATATABLE1
            for (int ih = 0; ih <= conceptos.Tables[0].Rows.Count; ih++)
            {
                if (conceptos.Tables[0].Rows[0][0] == null || conceptos.Tables[0].Rows[0][1] == null || conceptos.Tables[0].Rows[0][2] == null || conceptos.Tables[0].Rows[0][3] == null || conceptos.Tables[0].Rows[0][4] == null)
                {
                    Utils.MostrarAlerta(Response, "Falta parametrizar la configuración de Nómina en los conceptos de Liquidación ..! ");
                    return;
                }
            }
            if (DataTable1.Rows.Count == 0)
            {
                this.preparargrilla_empleadoliquidacion();
                DataRow fila = DataTable1.NewRow();
                fila["DOC REFERENCIA"] = "Liquid Defin";
                DataTable1.Rows.Add(fila);
                DataRow fila1 = DataTable1.NewRow();
                fila1["DOC REFERENCIA"] = "Liquid Defint";
                DataTable1.Rows.Add(fila1);
            }   
				
            
            //Cesantias.
			if (Convert.ToDouble(LBCESAAPAGAR.Text.Substring(1))>0)
				sql.Add("insert into dquincena values(DEFAULT,"+codquincena+",'"+codigoempleado+"','"+conceptos.Tables[0].Rows[0][0].ToString()+"',1,"+Convert.ToDouble(LBCESAAPAGAR.Text.Substring(1))+","+Convert.ToDouble(LBCESAAPAGAR.Text.Substring(1))+",0,"+4+" ,0,'"+DataTable1.Rows[1]["DOC REFERENCIA"].ToString()+"','L')");
			if (Convert.ToDouble(LBINTAPAGAR.Text.Substring(1))>0)
				sql.Add("insert into dquincena values(DEFAULT,"+codquincena+",'"+codigoempleado+"','"+conceptos.Tables[0].Rows[0][1].ToString()+"',1,"+Convert.ToDouble(LBINTAPAGAR.Text.Substring(1))+","+Convert.ToDouble(LBINTAPAGAR.Text.Substring(1))+",0,"+4+" ,0,'"+DataTable1.Rows[1]["DOC REFERENCIA"].ToString()+"','L')");
			// Prima.
            double vrPrima = 0;
            if (LBPRIMAAPAGAR.Text.Substring(0, 1) == "$") 
                vrPrima = Convert.ToDouble(LBPRIMAAPAGAR.Text.Substring(1));
            if (LBPRIMAAPAGAR.Text.Substring(0, 2) == "($")
            {
                LBPRIMAAPAGAR.Text = LBPRIMAAPAGAR.Text.Replace(")", ""); 
                vrPrima = Convert.ToDouble(LBPRIMAAPAGAR.Text.Substring(2))*-1; 
            }
			if (vrPrima != 0)
                sql.Add("insert into dquincena values(DEFAULT,"+codquincena+",'"+codigoempleado+"','"+conceptos.Tables[0].Rows[0][2].ToString()+"',1,"+vrPrima+","+vrPrima+",0,"+4+" ,0,'"+DataTable1.Rows[1]["DOC REFERENCIA"].ToString()+"','L')");
			// Vacaciones
			if (Convert.ToDouble(LBVACAAPAGAR.Text.Substring(1))>0)
				sql.Add("insert into dquincena values(DEFAULT,"+codquincena+",'"+codigoempleado+"','"+conceptos.Tables[0].Rows[0][3].ToString()+"',1,"+Convert.ToDouble(LBVACAAPAGAR.Text.Substring(1))+","+Convert.ToDouble(LBVACAAPAGAR.Text.Substring(1))+",0,"+4+" ,0,'"+DataTable1.Rows[1]["DOC REFERENCIA"].ToString()+"','L')");
			// indemnizacion
			if (Convert.ToDouble(LBINDEMNIZACION.Text.Substring(1))>0)
				sql.Add("insert into dquincena values(DEFAULT,"+codquincena+",'"+codigoempleado+"','"+conceptos.Tables[0].Rows[0][4].ToString()+"',1,"+Convert.ToDouble(LBINDEMNIZACION.Text.Substring(1))+","+Convert.ToDouble(LBINDEMNIZACION.Text.Substring(1))+",0,"+4+" ,0,'"+DataTable1.Rows[1]["DOC REFERENCIA"].ToString()+"','L')");
            if (Convert.ToDouble(LBEPSVACACIONES.Text.Substring(1)) > 0)
                sql.Add("insert into dquincena values(DEFAULT," + codquincena + ",'" + codigoempleado + "','" + conceptos.Tables[0].Rows[0]["CNOM_CONCEPSCODI"].ToString() + "',1," + Convert.ToDouble(LBEPSVACACIONES.Text.Substring(1)) + ",0," + Convert.ToDouble(LBEPSVACACIONES.Text.Substring(1)) + "," + 4 + " ,0,'" + DataTable1.Rows[1]["DOC REFERENCIA"].ToString() + "','L')");
            if (Convert.ToDouble(LBFONDOVACACIONES.Text.Substring(1)) > 0)
                sql.Add("insert into dquincena values(DEFAULT," + codquincena + ",'" + codigoempleado + "','" + conceptos.Tables[0].Rows[0]["CNOM_CONCFONDCODI"].ToString() + "',1," + Convert.ToDouble(LBFONDOVACACIONES.Text.Substring(1)) + ",0," + Convert.ToDouble(LBFONDOVACACIONES.Text.Substring(1)) + "," + 4 + " ,0,'" + DataTable1.Rows[1]["DOC REFERENCIA"].ToString() + "','L')");
            // actualizar detalle de vacaciones
            // validar si las toma en tiempo o en dinero
            if (DBFunctions.Transaction(sql))
                Response.Redirect("" + indexPage + "?process=Nomina.LiquidacionDefinitiva&el=1");
            else
            {
                lb.Text = "No ha sido satisfactorio el proceso..";
                General.mostrarMensaje("No ha sido satisfactorio el proceso");
            }
		}

		
		protected void Grid_Change(Object sender, DataGridPageChangedEventArgs e) 
		{
 			DataGrid2.CurrentPageIndex = e.NewPageIndex;
			DataGrid2.DataSource = DataTable1;
			DataGrid2.DataBind();
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
