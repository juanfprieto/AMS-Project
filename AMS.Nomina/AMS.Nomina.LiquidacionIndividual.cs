using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;



namespace AMS.Nomina
{
	public class Liquidacionnomina2 : System.Web.UI.UserControl
	{
		
		protected Label registros,lbperipago;
		protected Button BTNCONFIRMACION;
		protected DataGrid DataGrid1,DataGrid2;
		protected TextBox TXTIDENTI,TXTCODIGOQUIN,TXTFECHA1;
		protected DataTable DataTable1 ,DataTable2;
		protected DataSet empleados,liquidador,quincena,tsalarionomina,cnomina;
		protected DropDownList DDLANO,DDLQUIN,DDLMES,DDLTIPOPAGO;
		protected Label lb,lb2,lb3,lbmas1,lbdocref,lbliquidador,prueba,prueba2,lbpag;
		double acumuladoeps,acumuladocesantia,acumuladoprima,acumuladovacaciones,acumuladoretefuente;
		double acumuladoprovisiones,acumuladoliqdefinitiva,acumuladohorasextras,diasexceptosauxtransp;
		protected System.Web.UI.WebControls.Label titulo;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Button consulta;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label lbpag2;
		
		
		
		
	
		
		protected void realizar_consulta(Object  Sender, EventArgs e)
		{
		
		int i;
		
		
		
		empleados= new DataSet();
		liquidador= new DataSet();
		quincena= new DataSet();
		tsalarionomina= new DataSet();
		cnomina= new DataSet();
		
		//Traigo el ano,mes,y quincena vigente de CNOMINA.
		DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi from dbxschema.cnomina");	
		
		lbdocref.Text=cnomina.Tables[0].Rows[0][0]+"-"+cnomina.Tables[0].Rows[0][1]+"-"+cnomina.Tables[0].Rows[0][2];
		
		//Valido los datos escogidos por el usuario contra los de CNOMINA. 
		if (DDLQUIN.SelectedValue==cnomina.Tables[0].Rows[0][2].ToString() && DDLMES.SelectedValue==cnomina.Tables[0].Rows[0][1].ToString() && DDLANO.SelectedValue==cnomina.Tables[0].Rows[0][0].ToString())
		{
            Utils.MostrarAlerta(Response, "se selecciono periodo QUINCENA correcto! ,MES CORRECTO!,ANO CORRECTO! ");
			//borrar la tabla de dquincena 
			DataSet mquincenas0= new DataSet();
			DBFunctions.Request(mquincenas0,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			//DBFunctions.NonQuery("DELETE from dquincena where mqui_codiquin="+mquincenas0.Tables[0].Rows[0][0]+"");
			mquincenas0.Clear();
			//****** Si el usuario escogio procesar TIPO PAGO QUINCENAL********
			//Se incluiran los empleados VIGENTES pago QUINCENAL.
									
			if (DDLTIPOPAGO.SelectedValue=="1")
			{
        Utils.MostrarAlerta(Response, "se selecciono quincena");
		
		//Traigo todos los empleados vigentes.
		DBFunctions.Request(empleados,IncludeSchema.NO,"Select MEMP_CODIEMPL as Codigo,MEMP_NOMBRE1 as Nombre,MEMP_APELLIDO1 as Apellido, MEMP_APELLIDO2 as SegundoApellido,MEMP_SUELACTU as SueldoQuincena, memp_suelactu as SueldoMes,memp_fechingreso as fecha,memp_nombre2,tsub_codigo from DBXSCHEMA.mempleado ,DBXSCHEMA.CNOMINA  where DBXSCHEMA.mempleado.test_estado='1' AND DBXSCHEMA.mempleado.memp_peripago=1 ORDER BY memp_codiempl;");
		//Traigo los tipos de salario:no integral,integral,variable
		DBFunctions.Request(tsalarionomina,IncludeSchema.NO,"select * from dbxschema.tsalarionomina;");
		
		//SI ESCOGIO PAGO PRIMERA QUINCENA
		if (DDLQUIN.SelectedValue=="1")
		{
		//conversion fecha de cnomina para la quincena vigente yyyy-mm-dd a yyyy-mm-dd 
		if(Convert.ToInt32(cnomina.Tables[0].Rows[0][1].ToString())>=1 && Convert.ToInt32(cnomina.Tables[0].Rows[0][1].ToString())<=9)
		{
			lb.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-0"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"01";
			lb2.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-0"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"15";
			lbmas1.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-0"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"16";
		}
		else
		{
			lb.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"01";
			lb2.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"15";
			lbmas1.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"16";
		}
			
		}
		//SI ESCOGIO PAGO SEGUNDA QUINCENA
		if (DDLQUIN.SelectedValue=="2")
		{
		//conversion fecha de cnomina para la quincena vigente yyyy-mm-dd a yyyy-mm-dd 
		if(Convert.ToInt32(cnomina.Tables[0].Rows[0][1].ToString())>=1 && Convert.ToInt32(cnomina.Tables[0].Rows[0][1].ToString())<=9)
		{
			lb.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-0"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"15";
			lb2.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-0"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"30";
			lbmas1.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-0"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"31";
		}
		else
		{
			lb.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"15";
			lb2.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"30";
			lbmas1.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"31";
		}	
			
		}
		
			}
		if (DDLTIPOPAGO.SelectedValue=="2")
			{
				
				DBFunctions.Request(empleados,IncludeSchema.NO,"Select MEMP_CODIEMPL as Codigo,MEMP_NOMBRE1 as Nombre,MEMP_APELLIDO1 as Apellido, MEMP_APELLIDO2 as SegundoApellido,MEMP_SUELACTU as SueldoQuincena, memp_suelactu as SueldoMes,memp_fechingreso as fecha,memp_nombre2,tsub_codigo from DBXSCHEMA.mempleado ,DBXSCHEMA.CNOMINA  where DBXSCHEMA.mempleado.test_estado='1' AND DBXSCHEMA.mempleado.memp_peripago=2 ORDER BY memp_codiempl;");
				DBFunctions.Request(tsalarionomina,IncludeSchema.NO,"select * from dbxschema.tsalarionomina;");
				DataSet nomensuales= new DataSet();
				DBFunctions.Request(nomensuales,IncludeSchema.NO,"Select MEMP_CODIEMPL,MEMP_NOMBRE1,MEMP_APELLIDO1, MEMP_APELLIDO2 ,memp_nombre2,tper_descp from DBXSCHEMA.mempleado E,DBXSCHEMA.CNOMINA C,dbxschema.tperipago T  where E.test_estado='1' AND (E.memp_peripago=1 or E.memp_peripago=3) and memp_peripago=tper_peri ORDER BY memp_codiempl");
				if(Convert.ToInt32(cnomina.Tables[0].Rows[0][1].ToString())>=1 && Convert.ToInt32(cnomina.Tables[0].Rows[0][1].ToString())<=9)
		{
			lb.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-0"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"01";
			lb2.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-0"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"30";
			
		}
		else
		{
			lb.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"01";
			lb2.Text=cnomina.Tables[0].Rows[0][0].ToString()+"-"+cnomina.Tables[0].Rows[0][1].ToString()+"-"+"30";
		}		
		if (nomensuales.Tables[0].Rows.Count>0)
			{
                Utils.MostrarAlerta(Response, "Se encontro empleados a los cuales se les paga quincenal o jornal, ud esta procesando liquidacion mensual estos empleados no seran tenidos en cuenta, porfavor Revise el listado posterior al de la liquidacion.. ");
				this.preparargrilla_empleadosdiferenteperipago();
				for(i=0;i<nomensuales.Tables[0].Rows.Count;i++)
				{
					this.ingresar_datos_empleadosdiferenteperipago(nomensuales.Tables[0].Rows[i][0].ToString(),nomensuales.Tables[0].Rows[i][2].ToString(),nomensuales.Tables[0].Rows[i][3].ToString(),nomensuales.Tables[0].Rows[i][1].ToString(),nomensuales.Tables[0].Rows[i][4].ToString(),nomensuales.Tables[0].Rows[i][5].ToString());
				}
				
				lbperipago.Visible=true;			
			}
		}
		
		
		
		DataSet mquincenas= new DataSet();
		//Traigo el ultimo registro del maestro de Quincenas
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			//funcion para llenar columnas en grilla
		this.preparargrilla_empleadospago();
		//Recorro cada empleado vigente
		for(i=0;i<empleados.Tables[0].Rows.Count;i++)
		{
			
			acumuladoeps=0;
			diasexceptosauxtransp=0;		
			
			this.validardiasdepago(empleados.Tables[0].Rows[i][6].ToString(),lb.Text,lb2.Text,empleados.Tables[0].Rows[i][0].ToString(),cnomina.Tables[0].Rows[0][3].ToString(),empleados.Tables[0].Rows[i][4].ToString(),lbdocref.Text,empleados.Tables[0].Rows[i][2].ToString(),empleados.Tables[0].Rows[i][3].ToString(),empleados.Tables[0].Rows[i][1].ToString(),empleados.Tables[0].Rows[i][7].ToString());
										
			this.actualiza_novedades(empleados.Tables[0].Rows[i][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[i][5]),lbdocref.Text,lbmas1.Text,empleados.Tables[0].Rows[i][2].ToString(),empleados.Tables[0].Rows[i][3].ToString(),empleados.Tables[0].Rows[i][1].ToString(),empleados.Tables[0].Rows[i][7].ToString());
						
			this.actualiza_registro_msusplicencias(empleados.Tables[0].Rows[i][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[i][5]),lbdocref.Text,lbmas1.Text,empleados.Tables[0].Rows[i][2].ToString(),empleados.Tables[0].Rows[i][3].ToString(),empleados.Tables[0].Rows[i][1].ToString(),empleados.Tables[0].Rows[i][7].ToString());
				
			this.actuliza_registro_mpagosydtosper(empleados.Tables[0].Rows[i][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[i][5]),lbdocref.Text,lbmas1.Text,empleados.Tables[0].Rows[i][2].ToString(),empleados.Tables[0].Rows[i][3].ToString(),empleados.Tables[0].Rows[i][1].ToString(),empleados.Tables[0].Rows[i][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString());
			
			this.actuliza_prestamoempledos(empleados.Tables[0].Rows[i][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[i][5]),lbdocref.Text,lbmas1.Text,empleados.Tables[0].Rows[i][2].ToString(),empleados.Tables[0].Rows[i][3].ToString(),empleados.Tables[0].Rows[i][1].ToString(),empleados.Tables[0].Rows[i][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString());
			
			this.liquidar_epsfondo(empleados.Tables[0].Rows[i][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[i][5]),lbdocref.Text,lbmas1.Text,empleados.Tables[0].Rows[i][2].ToString(),empleados.Tables[0].Rows[i][3].ToString(),empleados.Tables[0].Rows[i][1].ToString(),empleados.Tables[0].Rows[i][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString());
			
			this.liquidar_apropiaciones(empleados.Tables[0].Rows[i][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[i][5]),lbdocref.Text,lbmas1.Text,empleados.Tables[0].Rows[i][2].ToString(),empleados.Tables[0].Rows[i][3].ToString(),empleados.Tables[0].Rows[i][1].ToString(),empleados.Tables[0].Rows[i][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString());
			
			this.liquidar_subsidiodetransporte(empleados.Tables[0].Rows[i][0].ToString(),lb.Text,lb2.Text,mquincenas.Tables[0].Rows[0][0].ToString(),Convert.ToDouble(empleados.Tables[0].Rows[i][5]),lbdocref.Text,lbmas1.Text,empleados.Tables[0].Rows[i][2].ToString(),empleados.Tables[0].Rows[i][3].ToString(),empleados.Tables[0].Rows[i][1].ToString(),empleados.Tables[0].Rows[i][7].ToString(),cnomina.Tables[0].Rows[0][9].ToString(),diasexceptosauxtransp.ToString(),empleados.Tables[0].Rows[i][8].ToString());						
		}
		Session["DataTable1"]=DataTable1;//data table secundario
		Session["DataTable2"]=DataTable2;//data table principal 
		
		BTNCONFIRMACION.Visible= true;	
		
		if (DDLTIPOPAGO.SelectedValue=="3")
			{
                Utils.MostrarAlerta(Response, "se seleeciono jornal bajo construccion");
		}
		}
		
		
		else
        Utils.MostrarAlerta(Response, "Esta tratando de liquidar otra quincena");
		
		
		}
		
	
		protected void liquidar_subsidiodetransporte(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string diasexceptosauxtransp,string periodosubt) 
		{
		
			
			
		DataSet mquincenas= new DataSet();
		DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
		DataSet cnomina= new DataSet();
		DataSet valorepsprimeraquinanterior= new DataSet();
		DataSet valorepssegundaquinanterior= new DataSet();
		DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_topepagoauxitran,cnom_salaminiactu from dbxschema.cnomina");
		double auxiliotransporte;
		
		//double valortope=0;
		double valortope=double.Parse(cnomina.Tables[0].Rows[0][8].ToString())* double.Parse(cnomina.Tables[0].Rows[0][9].ToString());
		double valordiatransporte=(double.Parse(cnomina.Tables[0].Rows[0][6].ToString())/30);
		double valoradescontar=valordiatransporte*int.Parse(diasexceptosauxtransp.ToString());
		double valorsalarioprimeraq,valorsalariosegundaq,valortotalmesanterior;
		//liquidar subsidio de transporte
			//si se le paga al empleado el subsidio en ambas quincenas
			if  (cnomina.Tables[0].Rows[0][5].ToString()=="1")
			   
			{
				
				//si al empleado se le paga el auxilio legal o subsidio igual.
				
				if (periodosubt=="1"|| periodosubt=="2" )
				{
					//mirar que no sobrepase el tope
										
					if (acumuladoeps<=valortope)
					{
						//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
						auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0][7].ToString())-valoradescontar;
						this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,auxiliotransporte,auxiliotransporte,0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
					}
				}		     
			}
			//si se le paga al empleado el subsidio en la segunda quincena
			if  (cnomina.Tables[0].Rows[0][5].ToString()=="2")
			{
				//si al empleado se le paga el auxilio legal o subsidio igual.
				if (periodosubt=="1"|| periodosubt=="2" )
					//mirar que no sobrepase el tope
					if (acumuladoeps<=valortope)
					{
						//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
						auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0][6].ToString())-valoradescontar;
						this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,auxiliotransporte,auxiliotransporte,0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
					}
			}
		       
			//si se le paga al empleado el subsidio en ambas quincenas mes anterior
			
			if  (cnomina.Tables[0].Rows[0][5].ToString()=="3")
			{
				if (DDLQUIN.SelectedValue=="1" || DDLQUIN.SelectedValue=="2")
				{
					//Sumar devengado 1era Q + 2da Q
				DBFunctions.Request(valorepsprimeraquinanterior,IncludeSchema.NO,"select max(D.mqui_codiquin-1), D.dqui_adescontar from dbxschema.dquincena D,dbxschema.pconceptonomina P,dbxschema.cnomina C where D.pcon_concepto=C.cnom_concepscodi group by D.dqui_adescontar");
				DBFunctions.Request(valorepssegundaquinanterior,IncludeSchema.NO,"select max(D.mqui_codiquin-2), D.dqui_adescontar from dbxschema.dquincena D,dbxschema.pconceptonomina P,dbxschema.cnomina C where D.pcon_concepto=C.cnom_concepscodi group by D.dqui_adescontar");	
				valorsalarioprimeraq=double.Parse(valorepsprimeraquinanterior.Tables[0].Rows[0][1].ToString())*100/double.Parse(cnomina.Tables[0].Rows[0][10].ToString());
				valorsalariosegundaq=double.Parse(valorepssegundaquinanterior.Tables[0].Rows[0][1].ToString())*100/double.Parse(cnomina.Tables[0].Rows[0][10].ToString());
				valortotalmesanterior=valorsalarioprimeraq+valorsalarioprimeraq;
				
				//si al empleado se le paga el auxilio legal o subsidio igual.
				if (periodosubt=="1"|| periodosubt=="2" )
				//mirar que no sobrepase el tope
					if (valortotalmesanterior<=valortope)
			{
					// ojo mirar como sacar las ausencias del mes pasado..
					auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0][7].ToString());
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,auxiliotransporte,auxiliotransporte,0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
					
			}
				}		
			}			
		}
		
		protected void actuliza_prestamoempledos(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps) 
		{
			
			DataSet mquincenas= new DataSet();
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			double valorcuota,intereses,capital,saldo=0,parte2;
			DataSet prestamoempledos= new DataSet();
			int i;
			// traigo los prestamos para el empleado que afecten al periodo de pago elegido y que esten  activos,descuento primera o ambas quincenas
			if (DDLQUIN.SelectedValue=="1")
			{
				DBFunctions.Request(prestamoempledos,IncludeSchema.NO,"SELECT M.pcon_concepto,MPRE_SECUENCIA,MPRE_NUMELIBR,MPRE_TPERNOMI,MPRE_FECHPREST,MPRE_NUMECUOT,MPRE_VALORPRES,MPRE_CUOTPAGA,MPRE_PORCINTE,MPRE_VALOPAGA FROM DBXSCHEMA.MPRESTAMOEMPLEADOS M,DBXSCHEMA.PCONCEPTONOMINA P WHERE M.pcon_concepto=P.pcon_concepto and M.memp_codiempl='"+codigoempleado+"' and P.pcon_claseconc='N' and mpre_estdpres=1 and (mpre_tpernomi=1 or mpre_tpernomi=3) AND (MPRE_FECHPREST BETWEEN '"+lb.ToString()+"' and '"+lb2.ToString()+"') ");//ppuntop
			}
			
			if (DDLQUIN.SelectedValue=="2")
			{
				DBFunctions.Request(prestamoempledos,IncludeSchema.NO,"SELECT M.pcon_concepto,MPRE_SECUENCIA,MPRE_NUMELIBR,MPRE_TPERNOMI,MPRE_FECHPREST,MPRE_NUMECUOT,MPRE_VALORPRES,MPRE_CUOTPAGA,MPRE_PORCINTE,MPRE_VALOPAGA FROM DBXSCHEMA.MPRESTAMOEMPLEADOS M,DBXSCHEMA.PCONCEPTONOMINA P WHERE M.pcon_concepto=P.pcon_concepto and M.memp_codiempl='"+codigoempleado+"' and P.pcon_claseconc='N' and mpre_estdpres=1 and mpre_tpernomi=2  AND (MPRE_FECHPREST BETWEEN '"+lb.ToString()+"' and '"+lb2.ToString()+"') ");
			}
			
			for (i=0;i<prestamoempledos.Tables[0].Rows.Count;i++)
			{

			valorcuota=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())/int.Parse(prestamoempledos.Tables[0].Rows[i][5].ToString());
			parte2=double.Parse(prestamoempledos.Tables[0].Rows[i][7].ToString())*valorcuota;
			//parte2=	(double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())-(double.Parse(prestamoempledos.Tables[0].Rows[i][7].ToString())*valorcuota));
			prueba2.Text=parte2.ToString();
			intereses=parte2*double.Parse(prestamoempledos.Tables[0].Rows[i][8].ToString());
			capital = valorcuota;
			saldo=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())-capital;
			//saldo=parte2;
			this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,prestamoempledos.Tables[0].Rows[0][0].ToString(),1,valorcuota,0,valorcuota,"PESOS M/CTE",saldo,prestamoempledos.Tables[0].Rows[0][2].ToString(),apellido1,apellido2,nombre1,nombre2);
				
			}
			
		}
		
		
		protected void actuliza_registro_mpagosydtosper(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps) 
		{
			DataSet mpagosydtosper = new DataSet();
			DataSet mpagosydtosper2 = new DataSet();
			DataSet mquincenas= new DataSet();
			DateTime mpagfechainicio = new DateTime();
			DateTime fechainicioliquidacion = new DateTime(); 
			DateTime fechafinalliquidacion = new DateTime();
			DateTime mpagfechainicio2 = new DateTime();
			fechainicioliquidacion=Convert.ToDateTime(lb.ToString());
			fechafinalliquidacion=Convert.ToDateTime(lb2.ToString());
			
			int i;
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			
			if (DDLQUIN.SelectedValue=="1")
			{
			//Traigo los pagos y descuentos permanentes para cada empleado vigente entre las fechas de la quincena vigente ,descuento en la primera quincena o en ambas
						
			DBFunctions.Request(mpagosydtosper,IncludeSchema.NO,"select distinct M.pcon_concepto, M.mpag_fechinic,M.mpag_fechfinmes,M.mpag_fechfinquin,M.mpag_fechfinano,P.pcon_signoliq,M.mpag_valor,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from dbxschema.mpagosydtosper M,dbxschema.pconceptonomina P where M.memp_codiempl='"+codigoempleado+"' and M.pcon_concepto=P.pcon_concepto and (M.mpag_tperpag=1 or M.mpag_tperpag=3)");
			}
			
			if (DDLQUIN.SelectedValue=="2")
			{
				DBFunctions.Request(mpagosydtosper,IncludeSchema.NO,"select distinct M.pcon_concepto, M.mpag_fechinic,M.mpag_fechfinmes,M.mpag_fechfinquin,M.mpag_fechfinano,P.pcon_signoliq,M.mpag_valor,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from dbxschema.mpagosydtosper M,dbxschema.pconceptonomina P where M.memp_codiempl='"+codigoempleado+"' and M.pcon_concepto=P.pcon_concepto and M.mpag_tperpag=2");
			}
			
			//conversion fecha de mpagosydtosper para ver si aplica a la quincena vigente
			for (i=0;i<mpagosydtosper.Tables[0].Rows.Count;i++)
			{
			
				if(Convert.ToInt32(mpagosydtosper.Tables[0].Rows[i][2].ToString())>=1 && Convert.ToInt32(mpagosydtosper.Tables[0].Rows[i][2].ToString())<=9)
		{
					if (mpagosydtosper.Tables[0].Rows[i][3].ToString()=="1")
						lbpag.Text=mpagosydtosper.Tables[0].Rows[i][4].ToString()+"-0"+mpagosydtosper.Tables[0].Rows[i][2].ToString()+"-"+"15";
					
					if (mpagosydtosper.Tables[0].Rows[i][3].ToString()=="2")
						lbpag.Text=mpagosydtosper.Tables[0].Rows[i][4].ToString()+"-0"+mpagosydtosper.Tables[0].Rows[i][2].ToString()+"-"+"30";
					
			
		}
		else
		{
			if (mpagosydtosper.Tables[0].Rows[i][3].ToString()=="1")
			lbpag.Text=mpagosydtosper.Tables[0].Rows[i][4].ToString()+"-"+mpagosydtosper.Tables[0].Rows[i][2].ToString()+"-"+"15";
			
			if (mpagosydtosper.Tables[0].Rows[i][3].ToString()=="2")
			lbpag.Text=mpagosydtosper.Tables[0].Rows[i][4].ToString()+"-"+mpagosydtosper.Tables[0].Rows[i][2].ToString()+"-"+"30";
		}
		mpagfechainicio=Convert.ToDateTime(lbpag.Text);
		mpagfechainicio2=Convert.ToDateTime(mpagosydtosper.Tables[0].Rows[i][1].ToString());		
		
		if ((mpagfechainicio2>=fechainicioliquidacion && mpagfechainicio2<=fechafinalliquidacion) || mpagfechainicio2<= fechainicioliquidacion && mpagfechainicio>=fechafinalliquidacion )
		{
			this.validaciondebitaacredita(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString());
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
		 
		 
		protected void liquidar_apropiaciones(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps)
		{
			
			double apropiacionARP,apropiacionICBF,apropiacionSENA,apropiacionCAJACOMPENSACION;
			DataSet mquincenas= new DataSet();
			DataSet cnomina= new DataSet();
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concarpcodi,cnom_concicbfcodi,cnom_concsenacodi,cnom_conccajacompcodi,cnom_porcarp,cnom_porcicbf,cnom_porcsena,cnom_porccajacomp from dbxschema.cnomina");
			apropiacionARP=(acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][7].ToString()))/100;
			apropiacionICBF=(acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][8].ToString()))/100;
			apropiacionSENA=(acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][9].ToString()))/100;
			apropiacionCAJACOMPENSACION=(acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][10].ToString()))/100;
			this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][3].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionARP,docref,apellido1,apellido2,nombre1,nombre2);
			this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][4].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionICBF,docref,apellido1,apellido2,nombre1,nombre2);	
			this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][5].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionSENA,docref,apellido1,apellido2,nombre1,nombre2);	
			this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][6].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionCAJACOMPENSACION,docref,apellido1,apellido2,nombre1,nombre2);	
		}
		
		protected void liquidar_epsfondo(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps)
		{
			
			DataSet mquincenas= new DataSet();	
			DataSet valorepsquinanterior= new DataSet();	
			DataSet valorfondoquinanterior= new DataSet();	
			DataSet cnomina= new DataSet();
			double acumulado1,acumulado3,valorepsempleado,valorepsempresa,valorfondopensionempleado,valorfondopensionempresa ;
			//Traigo el ultimo registro del maestro de Quincenas
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			DBFunctions.Request(valorfondoquinanterior,IncludeSchema.NO,"select max(D.mqui_codiquin-1), D.dqui_adescontar from dbxschema.dquincena D,dbxschema.pconceptonomina P,dbxschema.cnomina C where D.pcon_concepto=C.cnom_concfondcodi group by D.dqui_adescontar");
			DBFunctions.Request(valorepsquinanterior,IncludeSchema.NO,"select max(D.mqui_codiquin-1), D.dqui_adescontar from dbxschema.dquincena D,dbxschema.pconceptonomina P,dbxschema.cnomina C where D.pcon_concepto=C.cnom_concepscodi group by D.dqui_adescontar");
		//	DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_porcepsempl,cnom_porcepsempr,cnom_concepscodi,cnom_epsperinomi,cnom_concfondcodi,cnom_porcfondempl,cnom_porcfondempr from dbxschema.cnomina");	
			DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_porcepsempl,                 cnom_concepscodi,cnom_epsperinomi,cnom_concfondcodi,cnom_porcfondempl                   from dbxschema.cnomina");	
			prueba.Text=acumuladoeps.ToString();
			//validar si se descuenta en ambas quincenas =1
			if (cnomina.Tables[0].Rows[0][13].ToString()=="1")
			{
				//averiguar en que quincena estoy, primera calculo normal
				
				if (cnomina.Tables[0].Rows[0][2].ToString()=="1")
				{
					//prueba2.Text=cnomina.Tables[0].Rows[0][14].ToString();
					valorfondopensionempleado=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][15].ToString())/100;
					valorepsempleado=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][10].ToString())/100;
					valorepsempresa=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][11].ToString())/100;
					valorfondopensionempresa=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][16].ToString())/100;
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][12].ToString(),1,0,0,valorepsempleado,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][14].ToString(),1,0,0,valorfondopensionempleado,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
				}
				//si estoy en la segunda acumulo el valor acumulado de la perimera mas la segunda,calculo y resto el resultado de la primera
				if  (cnomina.Tables[0].Rows[0][2].ToString()=="2")
				{
					if (valorepsquinanterior.Tables[0].Rows.Count<=0)
					{
                    Utils.MostrarAlerta(Response, "No se encontro registro de anteriores liquidaciones");
					valorfondopensionempleado=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][15].ToString())/100;
					valorepsempleado=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][10].ToString())/100;
					valorepsempresa=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][11].ToString())/100;
					valorfondopensionempresa=acumuladoeps*double.Parse(cnomina.Tables[0].Rows[0][16].ToString())/100;
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][12].ToString(),1,0,0,valorepsempleado,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][14].ToString(),1,0,0,valorfondopensionempleado,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
					}
					else
					{
					acumulado1=double.Parse(valorepsquinanterior.Tables[0].Rows[0][1].ToString())*100/double.Parse(cnomina.Tables[0].Rows[0][10].ToString());
			//h		acumulado3=acumulado1+acumuladoeps;
					acumulado3=acumuladoeps;
					valorepsempleado=(acumulado3*double.Parse(cnomina.Tables[0].Rows[0][10].ToString()))-double.Parse(valorepsquinanterior.Tables[0].Rows[0][1].ToString());
					valorfondopensionempleado=(acumulado3*double.Parse(cnomina.Tables[0].Rows[0][15].ToString()))-double.Parse(valorfondoquinanterior.Tables[0].Rows[0][1].ToString());
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][12].ToString(),1,0,0,valorepsempleado,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][12].ToString(),1,0,0,valorfondopensionempleado,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);					
					}
					
				}
			}
			//validar si se descuenta en la segunda =2
			if (cnomina.Tables[0].Rows[0][13].ToString()=="2")
			{
				acumulado1=double.Parse(valorepsquinanterior.Tables[0].Rows[0][1].ToString())*100/double.Parse(cnomina.Tables[0].Rows[0][10].ToString());
	// h		acumulado3=acumulado1+acumuladoeps;
				acumulado3=acumuladoeps;
				valorepsempleado=acumulado3*double.Parse(cnomina.Tables[0].Rows[0][10].ToString())/100;
				valorfondopensionempleado=acumulado3*double.Parse(cnomina.Tables[0].Rows[0][15].ToString());
				this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][12].ToString(),1,0,0,valorepsempleado,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
				this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,cnomina.Tables[0].Rows[0][12].ToString(),1,0,0,valorfondopensionempleado,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);					
			}
			
			//validar si se descuenta en comisiones (primera quincenas+segunda+comisiones)
			if (cnomina.Tables[0].Rows[0][13].ToString()=="3")
			{
				//mriara cuando liquide comisiones.
			}
		}
		
		
		
		protected void actualiza_novedades(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2)
		{
		int j;
		double valordiatrabajo=0;
		double valorhoratrabajonormal=0;
		double valorminuto=0;
		double valorhorasextras,valorevento,valorpagado;
		DataSet afectahorasextras= new DataSet();
		DBFunctions.Request(afectahorasextras,IncludeSchema.NO,"select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and P.tres_afechoraextr='S' and (M.mnov_fecha between '"+lb.ToString()+"' and '"+lb2.ToString()+"')");
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
			DBFunctions.Request(novedades,IncludeSchema.NO,"select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_claseconc='N' and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and (M.mnov_fecha between '"+lb.ToString()+"' and '"+lb2.ToString()+"')");
			//validar si existen licencias en novedades
			DBFunctions.Request(licenciasennovedades,IncludeSchema.NO,"select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_claseconc='L' and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and (M.mnov_fecha between '"+lb.ToString()+"' and '"+lb2.ToString()+"')");
		    //mostrar mensaje de adevertencia
		    for(j=0;j<licenciasennovedades.Tables[0].Rows.Count;j++)
		    {
                Utils.MostrarAlerta(Response, "Apreciado Usuario,se detecto el ingreso de un concepto tipo Licencia en Novedades,este concepto no sera tenido en cuenta para la liquidacion,porfavor corrija e ingrese este concepto en Licencias..");
		    }
			
			
			//Recorro cada novedad
			
		     for(j=0;j<novedades.Tables[0].Rows.Count;j++)
			{
				
				
		     	//si es novedad de horas=2
				
		     	if (novedades.Tables[0].Rows[j][5].ToString()=="2")
		     	{
					
					valorevento=valorhoratrabajonormal*double.Parse(novedades.Tables[0].Rows[j][6].ToString());
					valorhorasextras= valorhoratrabajonormal*double.Parse(novedades.Tables[0].Rows[j][4].ToString())* double.Parse(novedades.Tables[0].Rows[j][6].ToString());
					this.validaciondebitaacredita(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString());
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad en Horas de descuento que afecta EPS? posible error..",valorhorasextras,0,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad en Horas de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,valorhorasextras);											
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad en Horas de descuento que afecta Primas? posible error..",0,0,valorhorasextras,0,0,0,0,0);											
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad en Horas de descuento que afecta Vacaciones? posible error..",0,0,0,valorhorasextras,0,0,0,0);
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad en Horas de descuento que afecta Cesantias? posible error..",0,valorhorasextras,0,0,0,0,0,0);
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad en Horas de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,valorhorasextras,0,0,0);
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad en Horas de descuento que afecta Provisiones? posible error..",0,0,0,0,0,valorhorasextras,0,0);
		     		this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad en Horas de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,valorhorasextras,0);
		     		
		     	}
				//si es novedad de dias=1
				if (novedades.Tables[0].Rows[j][5].ToString()=="1")
				{
					valorpagado=valordiatrabajo*int.Parse(novedades.Tables[0].Rows[j][4].ToString());
					this.validaciondebitaacredita(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString());
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad de dias de descuento que afecta EPS? posible error..",valorpagado,0,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad de dias de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,valorpagado);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad de dias de descuento que afecta Primas? posible error..",0,0,valorpagado,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad de dias de descuento que afecta Vacaciones? posible error..",0,0,0,valorpagado,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad de dias de descuento que afecta Cesantias?  posible error..",0,valorpagado,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad de dias de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,valorpagado,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad de dias de descuento que afecta Provisiones? posible error..",0,0,0,0,0,valorpagado,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad de dias de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,valorpagado,0);
					
						
				}
		     	
				//si es novedad de minutos=3
				if (novedades.Tables[0].Rows[j][5].ToString()=="3")
				{
					valorpagado=valorminuto*int.Parse(novedades.Tables[0].Rows[j][4].ToString());
					this.validaciondebitaacredita(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString());
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad de minutos de descuento que afecta EPS? posible error..",valorpagado,0,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad de minutos de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,valorpagado);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad de minutos de descuento que afecta Primas? posible error..",0,0,valorpagado,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad de minutos de descuento que afecta Vacaciones? posible error..",0,0,0,valorpagado,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad de minutos de descuento que afecta Cesantias? posible error..",0,valorpagado,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad de minutos de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,valorpagado,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad de minutos de descuento que afecta Provisiones? posible error..",0,0,0,0,0,valorpagado,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),int.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad de minutos de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,valorpagado,0);
					
				}
				
				//si es novedad en pesos=4
				if (novedades.Tables[0].Rows[j][5].ToString()=="4")
				{
					this.validaciondebitaacredita(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString());
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
		                                   
		}
		protected void validaciondebitaacredita(string codquincena,string codempleado,string concepto,int canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2,string camposumaresta)
		                                        {
		                                        	if (camposumaresta=="D")
		                                        	this.ingresar_datos_datatable(codquincena,codempleado,concepto,canteventos,valorevento,apagar,0,descripcion,saldo,docref,apellido1,apellido2,nombre1,nombre2);	
		                                        	else
		                                        	this.ingresar_datos_datatable(codquincena,codempleado,concepto,canteventos,valorevento,0,adescontar,descripcion,saldo,docref,apellido1,apellido2,nombre1,nombre2);
		                                        		
		                                        }
		protected void validacionafectaciones(string codquincena,string codempleado,string concepto,int canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2,string camposumaresta,string campocomparacion,string mensaje,double valorpagadoeps,double valorpagadocesantia,double valorpagadoprima,double valorpagadovacaciones,double valorpagadoretefuente,double valorpagadoprovisiones,double valorpagadoliqdefinitiva,double valorpagadohorasextras)
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
			else 
				if(camposumaresta=="H" && campocomparacion=="S" )
		      	{
                    Utils.MostrarAlerta(Response, "Empleado : " + codempleado + " MSG " + mensaje + "");
			    //	errores +=1;
			    }  			
			    					
		}
		
		protected void actualiza_registro_msusplicencias(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2)
		{
			int i;
						
			double valordiatrabajo=sueldo/30;
			
			double valormsusplicencias;
			DataSet mquincenas= new DataSet();
			
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			//Traigo el ultimo registro del maestro de Quincenas			
			DateTime fecha1 = new DateTime();
			DateTime fecha2 = new DateTime();
			//mirar si ahy suspension,licencias o incapacidades entre la quincena exacta y de cuantos dias
			
			DataSet msusplicencias= new DataSet();
			DataSet msusplicencias2= new DataSet();
			DataSet msusplicencias3= new DataSet();
			DBFunctions.Request(msusplicencias,IncludeSchema.NO, "select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,(M.msus_hasta-M.msus_desde)+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion, p.pcon_factorliq  from msusplicencias M,pconceptonomina P,tdesccantidad T  where M.memp_codiempl='" + codigoempleado+"'and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) and (M.msus_desde between '"+lb.ToString()+"' and '"+lb2.ToString()+"')and (M.msus_hasta between '"+lb.ToString()+"' and '"+lb2.ToString()+"')");
			
			for (i=0;i<msusplicencias.Tables[0].Rows.Count;i++)
				{
				//asi la licencia sea remunerada se le descuenta el subsidio de transporte
				diasexceptosauxtransp+=int.Parse(msusplicencias.Tables[0].Rows[i][4].ToString());
				valormsusplicencias=int.Parse(msusplicencias.Tables[0].Rows[i][4].ToString()) * valordiatrabajo * Convert.ToDouble(msusplicencias.Tables[0].Rows[i][8].ToString());
            if (msusplicencias.Tables[0].Rows[i][6].ToString()=="H" && (msusplicencias.Tables[0].Rows[i][5].ToString()=="1" || msusplicencias.Tables[0].Rows[i][5].ToString()=="4"))
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,msusplicencias.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),0,Math.Round(valormsusplicencias, 0),msusplicencias.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2);
				else if (msusplicencias.Tables[0].Rows[i][6].ToString()=="D" && (msusplicencias.Tables[0].Rows[i][5].ToString()=="2" || msusplicencias.Tables[0].Rows[i][5].ToString()=="3"))
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,msusplicencias.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valormsusplicencias, 0),0,msusplicencias.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2);
				else
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,"Error de incongruencia en susplic",0,0,0,0,"0",0,docref,apellido1,apellido2,nombre1,nombre2);
					
				}
			
			//mirar si la suspension,licencias o incapacidades inicia en el periodo, y se pasa del periodo, cuantos dias le debo descontar 
			DateTime lb2mas1 = new DateTime();
			string lb2mas11;
			lb2mas1=Convert.ToDateTime(lb2.ToString());
			lb2mas1=lb2mas1.AddDays(1);
			lb2mas11=lb2mas1.Date.ToString("yyyy-MM-dd");
			
			//prueba2.Text="select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,('"+lb2.ToString()+"'-M.msus_desde)+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion from msusplicencias M,pconceptonomina P,tdesccantidad T  ,msusplicencias M2 where M.memp_codiempl='"+codigoempleado+"' and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) and (M.msus_desde between '"+lb.ToString()+"' and '"+lb2.ToString()+"') and (M.msus_hasta between '"+lb2mas11.ToString()+"' and M2.msus_hasta)";
			DBFunctions.Request(msusplicencias2,IncludeSchema.NO,"select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,('"+lb2.ToString()+ "'-M.msus_desde)+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion, p.pcon_factorliq  from msusplicencias M,pconceptonomina P,tdesccantidad T  ,msusplicencias M2 where M.memp_codiempl='" + codigoempleado+"' and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) and (M.msus_desde between '"+lb.ToString()+"' and '"+lb2.ToString()+"') and (M.msus_hasta between '"+lb2mas11.ToString()+"' and M2.msus_hasta)");
			
			for (i=0;i<msusplicencias2.Tables[0].Rows.Count;i++)
			{
				diasexceptosauxtransp+=int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString());
				valormsusplicencias=int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString()) * valordiatrabajo * Convert.ToDouble(msusplicencias.Tables[0].Rows[i][8].ToString());
				if (msusplicencias2.Tables[0].Rows[i][6].ToString()=="H" && (msusplicencias2.Tables[0].Rows[i][5].ToString()=="1" || msusplicencias2.Tables[0].Rows[i][5].ToString()=="4"))
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,msusplicencias2.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),0,Math.Round(valormsusplicencias, 0),msusplicencias2.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2);
				else if (msusplicencias2.Tables[0].Rows[i][6].ToString()=="D" && (msusplicencias2.Tables[0].Rows[i][5].ToString()=="2" || msusplicencias2.Tables[0].Rows[i][5].ToString()=="3"))
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,msusplicencias2.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valormsusplicencias, 0),0,msusplicencias2.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2);
				else
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,"Error de incongruencia en susplic",0,0,0,0,"0",0,docref,apellido1,apellido2,nombre1,nombre2);
							
			}
				
			//mirar si la suspension inicia antes del periodo y se termina en el periodo
			DBFunctions.Request(msusplicencias3,IncludeSchema.NO,"distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,(M.msus_hasta-'"+lb.ToString()+ "')+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion, p.pcon_factorliq from msusplicencias M,pconceptonomina P,tdesccantidad T,msusplicencias M2 where M.memp_codiempl='" + codigoempleado+"'and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad) and (M.msus_desde between M.msus_desde and '"+lb.ToString()+"') and (M.msus_hasta between '"+lb.ToString()+"' and '"+lb2.ToString()+"')");
			for (i=0;i<msusplicencias3.Tables[0].Rows.Count;i++)
			{
				diasexceptosauxtransp+=int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString());
				valormsusplicencias=int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()) * valordiatrabajo * Convert.ToDouble(msusplicencias.Tables[0].Rows[i][8].ToString());
                if (msusplicencias3.Tables[0].Rows[i][6].ToString()=="H" && (msusplicencias3.Tables[0].Rows[i][5].ToString()=="1" || msusplicencias3.Tables[0].Rows[i][5].ToString()=="4"))
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),0,Math.Round(valormsusplicencias, 0),msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2);
				else if (msusplicencias3.Tables[0].Rows[i][6].ToString()=="D" && (msusplicencias3.Tables[0].Rows[i][5].ToString()=="2" || msusplicencias3.Tables[0].Rows[i][5].ToString()=="3"))
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valormsusplicencias, 0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2);
				else
					this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,"Error de incongruencia en susplic",0,0,0,0,"0",0,docref,apellido1,apellido2,nombre1,nombre2);
												
			}
		
		}
		
		protected void validardiasdepago( string fecha,string lb,string lb2,string codigoempleado,string concepto,string sueldomes,string docref,string apellido1,string apellido2,string nombre1,string nombre2)
		{
			DataSet afectacionessueldo= new DataSet();
			DBFunctions.Request(afectacionessueldo,IncludeSchema.NO,"select P.pcon_signoliq,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from dbxschema.cnomina C,dbxschema.pconceptonomina P where C.cnom_concsalacodi=P.pcon_concepto");
			TimeSpan diastrabajados = new TimeSpan();
			TimeSpan diasnotrabajados = new TimeSpan();
			DateTime fechaingresoempleado = new DateTime();
			DateTime fechainicioliquidacion = new DateTime(); 
			DateTime fechafinalliquidacion = new DateTime();
			DataSet mquincenas= new DataSet();	
			int dias,numerodias=0;
			double valordiatrabajo=double.Parse(sueldomes)/30;
			double sueldodias,sueldoquincena,sueldoapagarmes;
			string sueldopagado="";
			sueldoquincena=valordiatrabajo*15;
			sueldoapagarmes=valordiatrabajo*30;
			if (DDLTIPOPAGO.SelectedValue=="1")
			{
				sueldopagado=sueldoquincena.ToString();
				numerodias=15;
			}
			if (DDLTIPOPAGO.SelectedValue=="2")
			{
				sueldopagado=sueldoapagarmes.ToString();
				numerodias=30;
			}
			
					
			//Traigo el ultimo registro del maestro de Quincenas
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
									
			//validar la fecha de ingreso
			fechaingresoempleado=Convert.ToDateTime(fecha);
			fechainicioliquidacion=Convert.ToDateTime(lb.ToString());
			fechafinalliquidacion=Convert.ToDateTime(lb2.ToString());
			
			if (fechaingresoempleado<= fechainicioliquidacion)
			{
				
				//insertar pago de sueldo
				this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
				//this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad pesos m/cte de descuento que afecta EPS? posible error..",Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0,0,0,0,0);
					
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][1].ToString(),"Salario credito que afecta EPS? posible error..",double.Parse(sueldopagado),0,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][2].ToString(),"Salario credito que afecta Horas extras? posible error..",0,0,0,0,0,0,0,double.Parse(sueldopagado));
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][3].ToString(),"Salario credito que afecta Primas? posible error..",0,0,double.Parse(sueldopagado),0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][4].ToString(),"Salario credito que afecta Vacaciones? posible error..",0,0,0,double.Parse(sueldopagado),0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][5].ToString(),"Salario credito que afecta Cesantias? posible error..",0,double.Parse(sueldopagado),0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][6].ToString(),"Salario credito que afecta Retencion en la fuente? posible error..",0,0,0,0,double.Parse(sueldopagado),0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][7].ToString(),"Salario credito que afecta Provisiones? posible error..",0,0,0,0,0,double.Parse(sueldopagado),0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][8].ToString(),"Salario credito que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,double.Parse(sueldopagado),0);
					
			}
				
				
			if (fechaingresoempleado> fechainicioliquidacion && fechaingresoempleado<=fechafinalliquidacion)
				{
				//ingreso despues de la fecha de inicio de pago,pagarle los dias que trabajo.
				diastrabajados=(fechafinalliquidacion-fechaingresoempleado);
				diasnotrabajados=(fechafinalliquidacion-fechainicioliquidacion)-(fechafinalliquidacion-fechaingresoempleado);
				
				diasexceptosauxtransp+=double.Parse(diasnotrabajados.Days.ToString());
				
			
				prueba.Text=diastrabajados.Days.ToString();
				dias=int.Parse(prueba.Text)+1;
				sueldodias=dias*valordiatrabajo;
				this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2);
				
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][1].ToString(),"Salario credito que afecta EPS? posible error..",sueldodias,0,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][2].ToString(),"Salario credito que afecta Horas extras? posible error..",0,0,0,0,0,0,0,sueldodias);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][3].ToString(),"Salario credito que afecta Primas? posible error..",0,0,sueldodias,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][4].ToString(),"Salario credito que afecta Vacaciones? posible error..",0,0,0,sueldodias,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][5].ToString(),"Salario credito que afecta Cesantias? posible error..",0,sueldodias,0,0,0,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][6].ToString(),"Salario credito que afecta Retencion en la fuente? posible error..",0,0,0,0,sueldodias,0,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][7].ToString(),"Salario credito que afecta Provisiones? posible error..",0,0,0,0,0,sueldodias,0,0);
					this.validacionafectaciones(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(sueldodias,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][8].ToString(),"Salario credito que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,sueldodias,0);
					
			
			}
			
			if (fechaingresoempleado> fechafinalliquidacion)
			{
				
				//ingreso despues de la fecha de finalizacion de pago, error en el ingreso.
                Utils.MostrarAlerta(Response, "Fecha Ingreso de Empleado " + codigoempleado + " Erronea..");
				this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,"Error Fecha de ingreso futura",0,0,0,0,"0",0,docref,apellido1,apellido2,nombre1,nombre2);
			
							
			
			}
		}
			
		protected void ingresar_datos_datatable(string codquincena,string codempleado,string concepto,int canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2)
			 {
			    DataRow fila = DataTable1.NewRow();
			    fila["COD QUINCENA"]=codquincena;
				fila["CODIGOEMPLEADO"]=codempleado+" "+apellido1+" "+apellido2+" "+nombre1+" "+nombre2;
				fila["CONCEPTO"]=concepto;
				fila["CANT EVENTOS"]=canteventos;
				fila["VALOR EVENTO"]=valorevento;
				fila["A PAGAR"]=apagar;
				fila["A DESCONTAR"]=adescontar;
				fila["TIPO EVENTO"]=descripcion;
				fila["SALDO"]=saldo;
				fila["DOC REFERENCIA"]=docref; 
			    DataTable1.Rows.Add(fila);
			    
				DataGrid2.DataSource = DataTable1;
				DataGrid2.DataBind();
				DatasToControls.Aplicar_Formato_Grilla(DataGrid2);
				
			 }
		
		protected void ingresar_datos_empleadosdiferenteperipago(string codempleado,string apellido1,string apellido2,string nombre1,string nombre2,string periodopago)
			 {
			    DataRow fila = DataTable2.NewRow();
			    fila["EMPLEADO"]=codempleado+" "+apellido1+" "+apellido2+" "+nombre1+" "+nombre2;
				fila["PERIODO DE PAGO"]=periodopago;
				DataTable2.Rows.Add(fila);
				DataGrid1.DataSource = DataTable2;
				DataGrid1.DataBind();
				DatasToControls.Aplicar_Formato_Grilla(DataGrid1);
				
			 }
		
		
		
		protected void preparargrilla_empleadospago()
		{
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("COD QUINCENA",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("CODIGOEMPLEADO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("CONCEPTO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("CANT EVENTOS",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("VALOR EVENTO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("A PAGAR",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("A DESCONTAR",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("TIPO EVENTO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("SALDO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("DOC REFERENCIA",System.Type.GetType("System.String")));
		}
		
		protected void preparargrilla_empleadosdiferenteperipago()
		{
			DataTable2 = new DataTable();
			DataTable2.Columns.Add(new DataColumn("EMPLEADO",System.Type.GetType("System.String")));
			DataTable2.Columns.Add(new DataColumn("PERIODO DE PAGO",System.Type.GetType("System.String")));
				
			
		}
		
		protected void Page_Load(object Sender,EventArgs e)
		{
		if(!IsPostBack)
			{
				liquidador= new DataSet();
				cnomina= new DataSet();
				string quincena;
				string mes;
				string ano;
				DBFunctions.Request(cnomina,IncludeSchema.NO,"SELECT CNOM_NOMBPAGA,PANO_DETALLE,TMES_NOMBRE,TPER_DESCRIPCION FROM DBXSCHEMA.PANO P, DBXSCHEMA.CNOMINA C,DBXSCHEMA.TMES  T,DBXSCHEMA.TPERIODONOMINA N WHERE C.CNOM_ANO=P.PANO_ANO AND C.CNOM_MES=T.TMES_MES AND C.CNOM_QUINCENA=N.TPER_PERIODO");
				if (cnomina.Tables[0].Rows.Count==0)
				{
                    Utils.MostrarAlerta(Response, "Estimado Usuario no ha configurado su nomina aun..");
				}
				else {
					quincena=cnomina.Tables[0].Rows[0][3].ToString();
					mes=cnomina.Tables[0].Rows[0][2].ToString();
					ano=cnomina.Tables[0].Rows[0][1].ToString();
					DBFunctions.Request(liquidador,IncludeSchema.NO,"SELECT MNIT_NOMBRES,MNIT_APELLIDOS FROM MNIT WHERE MNIT_NIT='"+cnomina.Tables[0].Rows[0][0].ToString()+"'");
					DatasToControls bind = new DatasToControls();
					bind.PutDatasIntoDropDownList(DDLQUIN,"SELECT TPER_PERIODO, TPER_DESCRIPCION FROM TPERIODONOMINA");
					DatasToControls.EstablecerDefectoDropDownList(DDLQUIN,quincena);
					bind.PutDatasIntoDropDownList(DDLMES,"Select TMES_MES, TMES_NOMBRE from TMES");
					DatasToControls.EstablecerDefectoDropDownList(DDLMES,mes);
					bind.PutDatasIntoDropDownList(DDLANO,"SELECT PANO_ANO, PANO_DETALLE FROM PANO");
					DatasToControls.EstablecerDefectoDropDownList(DDLANO,ano);
					bind.PutDatasIntoDropDownList(DDLTIPOPAGO,"select tper_peri,tper_descp from dbxschema.tperipago");
					lbliquidador.Text=liquidador.Tables[0].Rows[0][0].ToString()+" "+liquidador.Tables[0].Rows[0][1].ToString();
					
				}
				
		}
		else 
		{
			if (Session["DataTable1"]!=null)
				DataTable1=(DataTable)Session["DataTable1"];
			
			if (Session["DataTable2"]!=null)
				DataTable2=(DataTable)Session["DataTable2"];
		}
		}
		protected void Grid_Change(Object sender, DataGridPageChangedEventArgs e) 
      {
 
         // For the DataGrid control to navigate to the correct page when
         // paging is allowed, the CurrentPageIndex property must be updated
         // programmatically. This process is usually accomplished in the
         // event-handling method for the PageIndexChanged event.

         // Set CurrentPageIndex to the page the user clicked.
         DataGrid2.CurrentPageIndex = e.NewPageIndex;

         // Rebind the data to refresh the DataGrid control. 
         DataGrid2.DataSource = DataTable1;
         DataGrid2.DataBind();
      
      }

		protected void Grid_Change2(Object sender, DataGridPageChangedEventArgs e) 
      {
 
         // For the DataGrid control to navigate to the correct page when
         // paging is allowed, the CurrentPageIndex property must be updated
         // programmatically. This process is usually accomplished in the
         // event-handling method for the PageIndexChanged event.

         // Set CurrentPageIndex to the page the user clicked.
         DataGrid1.CurrentPageIndex = e.NewPageIndex;

         // Rebind the data to refresh the DataGrid control. 
         DataGrid1.DataSource = DataTable2;
         DataGrid1.DataBind();
      
      }
		
		
		protected void realizar_liquidacion(Object  Sender, EventArgs e)
		{
			
			
			int i;
			DataTable1= new DataTable();
			DataTable1=(DataTable)Session["DataTable1"];

            Utils.MostrarAlerta(Response, "liquidando.. ");
			ArrayList EmpleadosRevisados= new ArrayList();
			for (i=0;i<DataTable1.Rows.Count;i++)
			{
				if (EmpleadosRevisados.BinarySearch(DataTable1.Rows[i][1].ToString())<0)
				{
				EmpleadosRevisados.Add(DataTable1.Rows[i][1].ToString());
				DataRow[]seleccion= DataTable1.Select("CODIGOEMPLEADO='"+DataTable1.Rows[i][1].ToString()+"'");
				
				
					for (i=0;i<seleccion.Length;i++)
					{
                        Utils.MostrarAlerta(Response, "liquidando.. " + seleccion[i][5].ToString() + "  ");
					}
				}
			}
			
		}
		
		
		
		//inserta campos de cnomina a maestro de quincenas
		
		//DBFunctions.NonQuery("insert into mquincenas values (default,"+cnomina.Tables[0].Rows[0][0]+","+cnomina.Tables[0].Rows[0][1]+","+cnomina.Tables[0].Rows[0][2]+","+cnomina.Tables[0].Rows[0][2]+")");
		
		
		
		
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

 
