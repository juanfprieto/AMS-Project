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
//using AMS.Reports;
using AMS.DB;
using System.Data.Odbc;
using System.Configuration;
using AMS.Tools;



namespace AMS.Nomina
{


	public class AMS_Nomina_InterfaseContableDOS : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox txtVigencia;
		protected System.Web.UI.WebControls.TextBox txtCompDiario;
		protected System.Web.UI.WebControls.TextBox txtNumeroDocumento;
		protected System.Web.UI.WebControls.TextBox txtCodUsuario;
		protected System.Web.UI.WebControls.HyperLink hl1,hl2,hl3,hl4;

			
		protected string nombreArchivo = "InterfaseNomina"+""+DateTime.Now.ToString("yyyyMMdd")+".txt";
		protected string nombreArchivo2 = "InterfaseARP"+""+DateTime.Now.ToString("yyyyMMdd")+".txt";
		protected string nombreArchivo3 = "InterfaseProvisiones"+""+DateTime.Now.ToString("yyyyMMdd")+".txt";
		protected string nombreArchivo4 = "InterfaseParafiscales"+""+DateTime.Now.ToString("yyyyMMdd")+".txt";
		protected	string directorioArchivo = ConfigurationManager.AppSettings["PathToDownloads"];
		protected	StreamWriter sw;
		protected	StreamWriter sw2;
		protected	StreamWriter sw3;
		protected	StreamWriter sw4;
		NumberFormatInfo info;
		protected string path=ConfigurationManager.AppSettings["VirtualPathToDownloads"];
		 
		string  outputDatosInterfase;
		string  outputDatosInterfaseARP;
		string  outputDatosInterfaseProvisiones;
		string  outputDatosInterfaseParafiscales;
		protected System.Web.UI.WebControls.Button Generar;
		protected System.Web.UI.WebControls.DropDownList DDLANO;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList DDLMES;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList DDLQUIN;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtFechaComprobante;
		protected System.Web.UI.WebControls.Label lb2;
		protected System.Web.UI.WebControls.Label lb;
		protected PlanillaPago comparacion = new PlanillaPago();
		
		
		protected void iniciarproceso()
		{
			sw = File.CreateText(directorioArchivo+nombreArchivo);
			sw2 = File.CreateText(directorioArchivo+nombreArchivo2);
			sw3 = File.CreateText(directorioArchivo+nombreArchivo3);
			sw4 = File.CreateText(directorioArchivo+nombreArchivo4);
			this.EscribirArchivo();
			this.EscribirArchivoARP();
			this.EscribirArchivoProvisiones();
			this.EscribirArchivoParafiscales();
			
			
			hl1.NavigateUrl=path+nombreArchivo;
			hl1.Visible=true;
			hl1.Text="Descargar Interfaz de Nómina";

			hl2.NavigateUrl=path+nombreArchivo2;
			hl2.Visible=true;
			hl2.Text="Descargar Interfaz de Arp";

            
			hl3.NavigateUrl=path+nombreArchivo3;
			hl3.Visible=true;
			hl3.Text="Descargar Interfaz de Provisiones";

			hl4.NavigateUrl=path+nombreArchivo4;
			hl4.Visible=true;
			hl4.Text="Descargar Interfaz de Parafiscales";

		}
		protected void EscribirArchivoParafiscales()
		{
			int i=0;
			int registros=0;
			double sumapagado=0,sumapagadoemp=0,sumadescontado=0;
			double sumatotalcredito=0;
			double sumatotaldebito=0;
			string codempleado="";
			string nit;
			int x,y;
			double valorcentrocosto=0;
			string numQuincena=DBFunctions.SingleData("select  mqui_codiquin from dbxschema.mquincenas where mqui_mesquin="+DDLMES.SelectedValue+" and mqui_tpernomi="+DDLQUIN.SelectedValue+" and mqui_anoquin="+DDLANO.SelectedValue+"");			
			
			DataSet empleados=new DataSet();
//			DBFunctions.Request(empleados,IncludeSchema.NO,"select  memp.memp_codiempl,memp.peps_codieps,peps.peps_nombeps,peps.mnit_nit,MEMP.pfon_codipens,PFP.pfon_nombpens,PFP.mnit_nit,memp.pfon_codipensvolu,PFP2.pfon_nombpens,PFP2.mnit_nit from dbxschema.mempleado memp,dbxschema.peps peps,dbxschema.pfondopension PFP ,dbxschema.pfondopension PFP2 where memp.peps_codieps=peps.peps_codieps and PFP.pfon_codipens=MEMP.pfon_codipens and PFP2.pfon_codipens=memp.pfon_codipensvolu and test_estado='1'");
			DBFunctions.Request(empleados,IncludeSchema.NO,"select  memp.memp_codiempl,memp.peps_codieps,peps.peps_nombeps,peps.mnit_nit,MEMP.pfon_codipens,PFP.pfon_nombpens,PFP.mnit_nit,memp.pfon_codipensvolu,PFP2.pfon_nombpens,PFP2.mnit_nit from dbxschema.mempleado memp,dbxschema.peps peps,dbxschema.pfondopension PFP ,dbxschema.pfondopension PFP2 where memp.peps_codieps=peps.peps_codieps and PFP.pfon_codipens=MEMP.pfon_codipens and PFP2.pfon_codipens=memp.pfon_codipensvolu");
			if (empleados.Tables[0].Rows.Count!=0)
			{
			
				for (i=0;i<empleados.Tables[0].Rows.Count;i++)
				{
					DataSet numcuentas=new DataSet();
					//sacar las cuentas asociadas a los conceptos
					//Response.Write("<script language:javascript>alert(' empleado  "+empleados.Tables[0].Rows[i][0].ToString()+" ');</script>");
					//Averiguar el centro de costo con mayor porcentaje
					string centCosto=DBFunctions.SingleData("Select PCEN_CODIGO from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE mecc_PORCENTAJE=(Select MAX(MECC_PORCENTAJE) from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"') AND MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"'");
								
					DBFunctions.Request(numcuentas,IncludeSchema.NO,"select DPROV.mqui_codiquin,DPROV.memp_codiemp,DPROV.papo_codiapor,DPROV.valor,PAPOR.papo_tipoaporte,PPUCAPAT.MCUE_CODIPUCDEBIAPOR,PPUCAPAT.MCUE_CODIPUCCREDAPOR,PPUCAPAT.pdep_codidpto,PAPOR.mnit_nit,PAPOR.papo_nombapor  from dbxschema.dprovapropiaciones DPROV,dbxschema.paportepatronal PAPOR,dbxschema.ppucaportepatronal PPUCAPAT where memp_codiemp='"+empleados.Tables[0].Rows[i][0].ToString()+"' and mqui_codiquin="+numQuincena+" and DPROV.papo_codiapor=PAPOR.papo_codiapor and papo_tipoaporte<>3 and DPROV.papo_codiapor=PPUCAPAT.papo_codiapor and PPUCAPAT.pdep_codidpto=(select pdep_codidpto from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"') ");
				
					codempleado=empleados.Tables[0].Rows[i][0].ToString();
					if (numcuentas.Tables[0].Rows.Count!=0)
					{
						for (x=0;x<numcuentas.Tables[0].Rows.Count;x++)
						{
							//PROCESO DEBITO
							string clasecuenta=DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][5].ToString()+"' ");
							if (clasecuenta=="N")
							{
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, "ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][1].ToString() + " no se le han definido porcentajes.Interfase Parafiscales");
								}
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									//debito***************
									outputDatosInterfaseParafiscales="";
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
						
									//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									//Validar nit para Fpens. y EPs
									if(numcuentas.Tables[0].Rows[x][4].ToString()=="5")
										nit=empleados.Tables[0].Rows[i][6].ToString();
									else if (numcuentas.Tables[0].Rows[x][4].ToString()=="6")
										nit=empleados.Tables[0].Rows[i][3].ToString();
									else
										nit=numcuentas.Tables[0].Rows[x][8].ToString();
									
									//nit
									//nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(nit,12,"0",false);
									outputDatosInterfaseParafiscales+=",";
									//cuenta(10)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][5].ToString(),10,"0",true);
									outputDatosInterfaseParafiscales+=",";
									//vigencia(06)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
									outputDatosInterfaseParafiscales+=",";
									//comprobante(2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									outputDatosInterfaseParafiscales+=",";
									//numero comprobante(6)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseParafiscales+=",";
									//numerodocumento(6)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseParafiscales+=",";
									//fecha(8)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
									outputDatosInterfaseParafiscales+=",";
									//detalle(30)
									//outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][9].ToString()+txtVigencia.Text,30," ",true);
									outputDatosInterfaseParafiscales+=",";
									//vrDebito(11.2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
									outputDatosInterfaseParafiscales+=",";
									//vrcredito(11.2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseParafiscales+=",";
									//CCosto(3)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(porcentajes.Tables[0].Rows[y][1].ToString(),3," ",true);
									outputDatosInterfaseParafiscales+=",";
									//Tipo(1)
									outputDatosInterfaseParafiscales+=1;
									outputDatosInterfaseParafiscales+=",";
									//VrBase(11.2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseParafiscales+=",";
									//Fec_Proceso(8)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
									outputDatosInterfaseParafiscales+=",";
									//CodUser(4)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
									outputDatosInterfaseParafiscales+=",";
									//prefijodocumento(2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
									sw4.WriteLine(outputDatosInterfaseParafiscales);
																		
									

								}

							}
							else
							{
								//debito***************
								
								outputDatosInterfaseParafiscales="";
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
						
								//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
								//Validar nit para Fpens. y EPs
								if(numcuentas.Tables[0].Rows[x][4].ToString()=="5")
									nit=empleados.Tables[0].Rows[i][6].ToString();
								else if (numcuentas.Tables[0].Rows[x][4].ToString()=="6")
									nit=empleados.Tables[0].Rows[i][3].ToString();
								else
									nit=numcuentas.Tables[0].Rows[x][8].ToString();
								
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(nit,12,"0",false);
								outputDatosInterfaseParafiscales+=",";
								//cuenta(10)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][6].ToString(),10,"0",true);
								outputDatosInterfaseParafiscales+=",";
								//vigencia(06)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
								outputDatosInterfaseParafiscales+=",";
								//comprobante(2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								outputDatosInterfaseParafiscales+=",";
								//numero comprobante(6)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseParafiscales+=",";
								//numerodocumento(6)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseParafiscales+=",";
								//fecha(8)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
								outputDatosInterfaseParafiscales+=",";
								//detalle(30)
								//outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][9].ToString()+txtVigencia.Text,30," ",true);
								outputDatosInterfaseParafiscales+=",";
								//vrDebito(11.2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
								outputDatosInterfaseParafiscales+=",";
								//vrcredito(11.2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseParafiscales+=",";
								//CCosto(3)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(centCosto,3," ",true);
								outputDatosInterfaseParafiscales+=",";
								//Tipo(1)
								outputDatosInterfaseParafiscales+=1;
								outputDatosInterfaseParafiscales+=",";
								//VrBase(11.2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseParafiscales+=",";
								//Fec_Proceso(8)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
								outputDatosInterfaseParafiscales+=",";
								//CodUser(4)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
								outputDatosInterfaseParafiscales+=",";
								//prefijodocumento(2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
								sw4.WriteLine(outputDatosInterfaseParafiscales);
									
								
							}//fin else

							//proceso credito
							clasecuenta=DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][6].ToString()+"' ");
							if (clasecuenta=="N")
							{
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, "ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][1].ToString() + "no se le han definido porcentajes.Interfase Parafiscales");
								}
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									
									//credito***********************
									outputDatosInterfaseParafiscales="";
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
									//Validar nit para Fpens. y EPs
									if(numcuentas.Tables[0].Rows[x][4].ToString()=="5")
										nit=empleados.Tables[0].Rows[i][6].ToString();
									else if (numcuentas.Tables[0].Rows[x][4].ToString()=="6")
										nit=empleados.Tables[0].Rows[i][3].ToString();
									else
										nit=numcuentas.Tables[0].Rows[x][8].ToString();
									//nit
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(nit,12,"0",false);
									outputDatosInterfaseParafiscales+=",";
									//cuenta(10)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][6].ToString(),10,"0",true);
									outputDatosInterfaseParafiscales+=",";
									//vigencia(06)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
									outputDatosInterfaseParafiscales+=",";
									//comprobante(2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									outputDatosInterfaseParafiscales+=",";
									//numero comprobante(6)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseParafiscales+=",";
									//numerodocumento(6)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseParafiscales+=",";
									//fecha(8)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
									outputDatosInterfaseParafiscales+=",";
									//detalle(30)
									//outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][9].ToString()+txtVigencia.Text,30," ",true);
									outputDatosInterfaseParafiscales+=",";
									//vrDebito(11.2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseParafiscales+=",";
									//vrcredito(11.2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
									outputDatosInterfaseParafiscales+=",";
									//CCosto(3)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(porcentajes.Tables[0].Rows[y][1].ToString(),3," ",true);
									outputDatosInterfaseParafiscales+=",";
									//Tipo(1)
									outputDatosInterfaseParafiscales+=1;
									outputDatosInterfaseParafiscales+=",";
									//VrBase(11.2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseParafiscales+=",";
									//Fec_Proceso(8)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
									outputDatosInterfaseParafiscales+=",";
									//CodUser(4)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
									outputDatosInterfaseParafiscales+=",";
									//prefijodocumento(2)
									outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
									sw4.WriteLine(outputDatosInterfaseParafiscales);


								}
							}
							else
							{
								
								//credito***********************
								outputDatosInterfaseParafiscales="";
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
								//Validar nit para Fpens. y EPs
								if(numcuentas.Tables[0].Rows[x][4].ToString()=="5")
									nit=empleados.Tables[0].Rows[i][6].ToString();
								else if (numcuentas.Tables[0].Rows[x][4].ToString()=="6")
									nit=empleados.Tables[0].Rows[i][3].ToString();
								else
									nit=numcuentas.Tables[0].Rows[x][8].ToString();
								//nit
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(nit,12,"0",false);
								outputDatosInterfaseParafiscales+=",";
								//cuenta(10)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][6].ToString(),10,"0",true);
								outputDatosInterfaseParafiscales+=",";
								//vigencia(06)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
								outputDatosInterfaseParafiscales+=",";
								//comprobante(2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								outputDatosInterfaseParafiscales+=",";
								//numero comprobante(6)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseParafiscales+=",";
								//numerodocumento(6)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseParafiscales+=",";
								//fecha(8)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
								outputDatosInterfaseParafiscales+=",";
								//detalle(30)
								//outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][9].ToString()+txtVigencia.Text,30," ",true);
								outputDatosInterfaseParafiscales+=",";
								//vrDebito(11.2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseParafiscales+=",";
								//vrcredito(11.2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
								outputDatosInterfaseParafiscales+=",";
								//CCosto(3)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(centCosto,3," ",true);
								outputDatosInterfaseParafiscales+=",";
								//Tipo(1)
								outputDatosInterfaseParafiscales+=1;
								outputDatosInterfaseParafiscales+=",";
								//VrBase(11.2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseParafiscales+=",";
								//Fec_Proceso(8)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
								outputDatosInterfaseParafiscales+=",";
								//CodUser(4)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
								outputDatosInterfaseParafiscales+=",";
								//prefijodocumento(2)
								outputDatosInterfaseParafiscales+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
								sw4.WriteLine(outputDatosInterfaseParafiscales);



							}
							
							

						}

					}
						
						

				}
			}
			sw4.Close();


		}
		
		protected void iniciarTabla()
		{
			//DataTable2 = new DataTable();
			//DataTable2.Columns.Add(new DataColumn("EMPLEADO",System.Type.GetType("System.String")));
			//DataTable2.Columns.Add(new DataColumn("PERIODO DE PAGO",System.Type.GetType("System.String")));
			
			
			//DataTable1 = new DataTable();
			//dt.Columns.Add(xxx,System.Type"String");
		}

		
		protected void EscribirArchivoARP()
		{
			int i=0;
			int registros=0;
			double sumapagado=0,sumapagadoemp=0,sumadescontado=0;
			double sumatotalcredito=0;
			double sumatotaldebito=0;
			string codempleado="";
			string nit;
			int x,y;
			double valorcentrocosto=0;
			
			string numQuincena=DBFunctions.SingleData("select  mqui_codiquin from dbxschema.mquincenas where mqui_mesquin="+DDLMES.SelectedValue+" and mqui_tpernomi="+DDLQUIN.SelectedValue+" and mqui_anoquin="+DDLANO.SelectedValue+"");			
			
			DataSet empleados=new DataSet();
		//	DBFunctions.Request(empleados,IncludeSchema.NO,"select memp_codiempl from dbxschema.mempleado  where test_estado='1'");
			DBFunctions.Request(empleados,IncludeSchema.NO,"select memp_codiempl from dbxschema.mempleado");
			if (empleados.Tables[0].Rows.Count!=0)
			{
			
				for (i=0;i<empleados.Tables[0].Rows.Count;i++)
				{
					DataSet numcuentas=new DataSet();
					//sacar las cuentas asociadas a los conceptos
					//Response.Write("<script language:javascript>alert(' empleado  "+empleados.Tables[0].Rows[i][0].ToString()+" ');</script>");
					//Averiguar el centro de costo con mayor porcentaje
					string centCosto=DBFunctions.SingleData("Select PCEN_CODIGO from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE mecc_PORCENTAJE=(Select MAX(MECC_PORCENTAJE) from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"') AND MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"'");
					
					DBFunctions.Request(numcuentas,IncludeSchema.NO,"select DPROV.mqui_codiquin,DPROV.memp_codiemp,DPROV.papo_codiapor,DPROV.valor,PAPOR.papo_tipoaporte,MEMP.parp_codiarp,ARP.mnit_nit,mcue_codipucdebiarp,mcue_codipuccredarp,PPUCARP.pdep_codidpto,PAPOR.papo_nombapor  from dbxschema.dprovapropiaciones DPROV,dbxschema.paportepatronal PAPOR,dbxschema.mempleado MEMP,dbxschema.parp ARP,dbxschema.ppucarp PPUCARP where DPROV.papo_codiapor=PAPOR.papo_codiapor and papo_tipoaporte=3 and MEMP.memp_codiempl=DPROV.memp_codiemp and ARP.parp_codiarp=MEMP.parp_codiarp and mqui_codiquin="+numQuincena+" and MEMP.parp_codiarp=PPUCARP.parp_codiarp and memp_codiemp='"+empleados.Tables[0].Rows[i][0].ToString()+"' and PPUCARP.pdep_codidpto=(select pdep_codidpto from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"') ");
					codempleado=empleados.Tables[0].Rows[i][0].ToString();
					if (numcuentas.Tables[0].Rows.Count!=0)
					{
						
						
						
						for (x=0;x<numcuentas.Tables[0].Rows.Count;x++)
						{
							//PROCESO DEBITO
							string clasecuenta=DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][7].ToString()+"' ");
							if (clasecuenta=="N")
							{
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, "no se le han definido porcentajes.Interfase ARP");
								}
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									//debito***************
									outputDatosInterfaseARP="";
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
						
									//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									
									//nit
									//nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
									nit=numcuentas.Tables[0].Rows[x][6].ToString();
									outputDatosInterfaseARP+=comparacion.Completar_Campos(nit,12,"0",false);
									outputDatosInterfaseARP+=",";
									//cuenta(10)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][7].ToString(),10,"0",true);
									outputDatosInterfaseARP+=",";
									//vigencia(06)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
									outputDatosInterfaseARP+=",";
									//comprobante(2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									outputDatosInterfaseARP+=",";
									//numero comprobante(6)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseARP+=",";
									//numerodocumento(6)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseARP+=",";
									//fecha(8)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
									outputDatosInterfaseARP+=",";
									//detalle(30)
									//outputDatosInterfaseARP+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
									outputDatosInterfaseARP+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][10].ToString()+txtVigencia.Text,30," ",true);
									outputDatosInterfaseARP+=",";
									//vrDebito(11.2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
									outputDatosInterfaseARP+=",";
									//vrcredito(11.2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseARP+=",";
									//CCosto(3)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(porcentajes.Tables[0].Rows[y][1].ToString(),3," ",true);
									outputDatosInterfaseARP+=",";
									//Tipo(1)
									outputDatosInterfaseARP+=1;
									outputDatosInterfaseARP+=",";
									//VrBase(11.2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseARP+=",";
									//Fec_Proceso(8)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
									outputDatosInterfaseARP+=",";
									//CodUser(4)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
									outputDatosInterfaseARP+=",";
									//prefijodocumento(2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
									sw2.WriteLine(outputDatosInterfaseARP);
									
									//sumapagado=sumapagado+valorcentrocosto;
									//Response.Write("<script language:javascript>alert(' SUMA pagado EMPLEADO "+numcuentas.Tables[0].Rows[x][0].ToString()+"VA EN : "+sumapagado+" ');</script>");
									//sumatotaldebito+=valorcentrocosto;
								
									
								}


							}//ACABO EL IF
							else
							{
								//debito***************
								outputDatosInterfaseARP="";
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
						
								//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									
								//nit
								//nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								nit=numcuentas.Tables[0].Rows[x][6].ToString();
								outputDatosInterfaseARP+=comparacion.Completar_Campos(nit,12,"0",false);
								outputDatosInterfaseARP+=",";
								//cuenta(10)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][7].ToString(),10,"0",true);
								outputDatosInterfaseARP+=",";
								//vigencia(06)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
								outputDatosInterfaseARP+=",";
								//comprobante(2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								outputDatosInterfaseARP+=",";
								//numero comprobante(6)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseARP+=",";
								//numerodocumento(6)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseARP+=",";
								//fecha(8)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
								outputDatosInterfaseARP+=",";
								//detalle(30)
								//outputDatosInterfaseARP+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
								outputDatosInterfaseARP+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][10].ToString()+txtVigencia.Text,30," ",true);
								outputDatosInterfaseARP+=",";
								//vrDebito(11.2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
								outputDatosInterfaseARP+=",";
								//vrcredito(11.2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseARP+=",";
								//CCosto(3)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(centCosto,3," ",true);
								outputDatosInterfaseARP+=",";
								//Tipo(1)
								outputDatosInterfaseARP+=1;
								outputDatosInterfaseARP+=",";
								//VrBase(11.2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseARP+=",";
								//Fec_Proceso(8)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
								outputDatosInterfaseARP+=",";
								//CodUser(4)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
								outputDatosInterfaseARP+=",";
								//prefijodocumento(2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
								sw2.WriteLine(outputDatosInterfaseARP);
									
								//sumapagado=sumapagado+valorcentrocosto;
								//Response.Write("<script language:javascript>alert(' SUMA pagado EMPLEADO "+numcuentas.Tables[0].Rows[x][0].ToString()+"VA EN : "+sumapagado+" ');</script>");
								//sumatotaldebito+=valorcentrocosto;
																

							}

							//proceso credito
							clasecuenta=DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][8].ToString()+"' ");
							if (clasecuenta=="N")
							{
								
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, "ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][1].ToString() + " no se le han definido porcentajes.Interfase ARP");
								}
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									//sumapagado=sumapagado+valorcentrocosto;
									//Response.Write("<script language:javascript>alert(' SUMA pagado EMPLEADO "+numcuentas.Tables[0].Rows[x][0].ToString()+"VA EN : "+sumapagado+" ');</script>");
									//sumatotaldebito+=valorcentrocosto;
								
									//credito***********************
									outputDatosInterfaseARP="";
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
						
									//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									
									//nit
									nit=numcuentas.Tables[0].Rows[x][6].ToString();
									outputDatosInterfaseARP+=comparacion.Completar_Campos(nit,12,"0",false);
									outputDatosInterfaseARP+=",";
									//cuenta(10)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][8].ToString(),10,"0",true);
									outputDatosInterfaseARP+=",";
									//vigencia(06)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
									outputDatosInterfaseARP+=",";
									//comprobante(2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									outputDatosInterfaseARP+=",";
									//numero comprobante(6)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseARP+=",";
									//numerodocumento(6)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseARP+=",";
									//fecha(8)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
									outputDatosInterfaseARP+=",";
									//detalle(30)
									//outputDatosInterfaseARP+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
									outputDatosInterfaseARP+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][10].ToString()+txtVigencia.Text,30," ",true);
									outputDatosInterfaseARP+=",";
									//vrDebito(11.2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseARP+=",";
									//vrcredito(11.2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
									outputDatosInterfaseARP+=",";
									//CCosto(3)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(porcentajes.Tables[0].Rows[y][1].ToString(),3," ",true);
									outputDatosInterfaseARP+=",";
									//Tipo(1)
									outputDatosInterfaseARP+=1;
									outputDatosInterfaseARP+=",";
									//VrBase(11.2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseARP+=",";
									//Fec_Proceso(8)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
									outputDatosInterfaseARP+=",";
									//CodUser(4)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
									outputDatosInterfaseARP+=",";
									//prefijodocumento(2)
									outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
									sw2.WriteLine(outputDatosInterfaseARP);


								}

							}
							else
							{
								//credito***********************
								outputDatosInterfaseARP="";
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
						
								//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									
								//nit
								nit=numcuentas.Tables[0].Rows[x][6].ToString();
								outputDatosInterfaseARP+=comparacion.Completar_Campos(nit,12,"0",false);
								outputDatosInterfaseARP+=",";
								//cuenta(10)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][8].ToString(),10,"0",true);
								outputDatosInterfaseARP+=",";
								//vigencia(06)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
								outputDatosInterfaseARP+=",";
								//comprobante(2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								outputDatosInterfaseARP+=",";
								//numero comprobante(6)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseARP+=",";
								//numerodocumento(6)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseARP+=",";
								//fecha(8)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
								outputDatosInterfaseARP+=",";
								//detalle(30)
								//outputDatosInterfaseARP+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
								outputDatosInterfaseARP+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][10].ToString()+txtVigencia.Text,30," ",true);
								outputDatosInterfaseARP+=",";
								//vrDebito(11.2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseARP+=",";
								//vrcredito(11.2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
								outputDatosInterfaseARP+=",";
								//CCosto(3)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(centCosto,3," ",true);
								outputDatosInterfaseARP+=",";
								//Tipo(1)
								outputDatosInterfaseARP+=1;
								outputDatosInterfaseARP+=",";
								//VrBase(11.2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseARP+=",";
								//Fec_Proceso(8)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
								outputDatosInterfaseARP+=",";
								//CodUser(4)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
								outputDatosInterfaseARP+=",";
								//prefijodocumento(2)
								outputDatosInterfaseARP+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
								sw2.WriteLine(outputDatosInterfaseARP);	


							}
						
							
							
							
						}//CUENTAS

					}
						
						

				}
			}
			sw2.Close();
		}

		protected void EscribirArchivoProvisiones()
		{
			int i=0;
			int registros=0;
			double sumapagado=0,sumapagadoemp=0,sumadescontado=0;
			double sumatotalcredito=0;
			double sumatotaldebito=0;
			string codempleado="";
			string nit;
			int x,y;
			double valorcentrocosto=0;
			
			
			string numQuincena=DBFunctions.SingleData("select  mqui_codiquin from dbxschema.mquincenas where mqui_mesquin="+DDLMES.SelectedValue+" and mqui_tpernomi="+DDLQUIN.SelectedValue+" and mqui_anoquin="+DDLANO.SelectedValue+"");			
			DataSet empleados=new DataSet();
	//		DBFunctions.Request(empleados,IncludeSchema.NO,"select memp_codiempl from dbxschema.mempleado where test_estado='1' ");
	//		DBFunctions.Request(empleados,IncludeSchema.NO,"select memp_codiempl from dbxschema.mempleado where test_estado='1' ");
			DBFunctions.Request(empleados,IncludeSchema.NO,"select memp_codiempl from dbxschema.mempleado");
	
			if (empleados.Tables[0].Rows.Count!=0)
			{
				for (i=0;i<empleados.Tables[0].Rows.Count;i++)
				{
					DataSet numcuentas=new DataSet();
					//sacar las cuentas asociadas a los conceptos
					//Response.Write("<script language:javascript>alert(' empleado  "+empleados.Tables[0].Rows[i][0].ToString()+" ');</script>");
					//lb.Text="select MPROV.mqui_codiquin,MPROV.memp_codiempl,PCONCE.pdep_codidpto,MPROV.pcon_concepto,MPROV.mpro_valor,ppro_nombprov,mcue_codipucdebiprov,mcue_codipuccredprov from dbxschema.mprovisiones MPROV,dbxschema.pprovisionnomina PPRO,dbxschema.ppucprovisionconcepto PCONCE,dbxschema.mempleado MEMP where mqui_codiquin="+numQuincena+" and MPROV.memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"' and MPROV.pcon_concepto=PPRO.ppro_codiprov and PCONCE.ppro_codiprov=PPRO.ppro_codiprov and MEMP.memp_codiempl=MPROV.memp_codiempl and PCONCE.pdep_codidpto=(select pdep_codidpto from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"')";
					DBFunctions.Request(numcuentas,IncludeSchema.NO,"select MPROV.mqui_codiquin,MPROV.memp_codiempl,PCONCE.pdep_codidpto,MPROV.pcon_concepto,MPROV.mpro_valor,ppro_nombprov,mcue_codipucdebiprov,mcue_codipuccredprov from dbxschema.mprovisiones MPROV,dbxschema.pprovisionnomina PPRO,dbxschema.ppucprovisionconcepto PCONCE,dbxschema.mempleado MEMP where mqui_codiquin="+numQuincena+" and MPROV.memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"' and MPROV.pcon_concepto=PPRO.ppro_codiprov and PCONCE.ppro_codiprov=PPRO.ppro_codiprov and MEMP.memp_codiempl=MPROV.memp_codiempl and PCONCE.pdep_codidpto=(select pdep_codidpto from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"') ");
					//Averiguar el centro de costo con mayor porcentaje
					string centCosto=DBFunctions.SingleData("Select PCEN_CODIGO from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE mecc_PORCENTAJE=(Select MAX(MECC_PORCENTAJE) from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"') AND MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"'");
					


					codempleado=empleados.Tables[0].Rows[i][0].ToString();
					if (numcuentas.Tables[0].Rows.Count!=0)
					{
					
						for (x=0;x<numcuentas.Tables[0].Rows.Count;x++)
						{
							//proceso debito
							string clasecuenta=DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][6].ToString()+"' ");
							if (clasecuenta=="N")
							{
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, "ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][1].ToString() + " no se le han definido porcentajes Interfase Provisiones.");
								}
								//for de porcentajes
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
								
									//debito***************
									outputDatosInterfaseProvisiones="";
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
						
									//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									
									//nit
									nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(nit,12,"0",false);
									outputDatosInterfaseProvisiones+=",";
									//cuenta(10)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][6].ToString(),10,"0",true);
									outputDatosInterfaseProvisiones+=",";
									//vigencia(06)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
									outputDatosInterfaseProvisiones+=",";
									//comprobante(2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									outputDatosInterfaseProvisiones+=",";
									//numero comprobante(6)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseProvisiones+=",";
									//numerodocumento(6)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseProvisiones+=",";
									//fecha(8)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
									outputDatosInterfaseProvisiones+=",";
									//detalle(30)
									//outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][5].ToString()+txtVigencia.Text,30," ",true);
									outputDatosInterfaseProvisiones+=",";
									//vrDebito(11.2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
									outputDatosInterfaseProvisiones+=",";
									//vrcredito(11.2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseProvisiones+=",";
									//CCosto(3)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(porcentajes.Tables[0].Rows[y][1].ToString(),3," ",true);
									outputDatosInterfaseProvisiones+=",";
									//Tipo(1)
									outputDatosInterfaseProvisiones+=1;
									outputDatosInterfaseProvisiones+=",";
									//VrBase(11.2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseProvisiones+=",";
									//Fec_Proceso(8)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
									outputDatosInterfaseProvisiones+=",";
									//CodUser(4)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
									outputDatosInterfaseProvisiones+=",";
									//prefijodocumento(2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
									sw3.WriteLine(outputDatosInterfaseProvisiones);
									
									//sumapagado=sumapagado+valorcentrocosto;
									//Response.Write("<script language:javascript>alert(' SUMA pagado EMPLEADO "+numcuentas.Tables[0].Rows[x][0].ToString()+"VA EN : "+sumapagado+" ');</script>");
									//sumatotaldebito+=valorcentrocosto;
								
									

								}

	
							}
							else
							{
								
								//proceso sin porcentaje
								//debito***************
								outputDatosInterfaseProvisiones="";
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString());
						
								//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									
								//nit
								nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(nit,12,"0",false);
								outputDatosInterfaseProvisiones+=",";
								//cuenta(10)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][6].ToString(),10,"0",true);
								outputDatosInterfaseProvisiones+=",";
								//vigencia(06)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
								outputDatosInterfaseProvisiones+=",";
								//comprobante(2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								outputDatosInterfaseProvisiones+=",";
								//numero comprobante(6)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseProvisiones+=",";
								//numerodocumento(6)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseProvisiones+=",";
								//fecha(8)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
								outputDatosInterfaseProvisiones+=",";
								//detalle(30)
								//outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][5].ToString()+txtVigencia.Text,30," ",true);
								outputDatosInterfaseProvisiones+=",";
								//vrDebito(11.2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
								outputDatosInterfaseProvisiones+=",";
								//vrcredito(11.2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseProvisiones+=",";
								//CCosto(3)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(centCosto,3," ",true);
								outputDatosInterfaseProvisiones+=",";
								//Tipo(1)
								outputDatosInterfaseProvisiones+=1;
								outputDatosInterfaseProvisiones+=",";
								//VrBase(11.2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseProvisiones+=",";
								//Fec_Proceso(8)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
								outputDatosInterfaseProvisiones+=",";
								//CodUser(4)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
								outputDatosInterfaseProvisiones+=",";
								//prefijodocumento(2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
								sw3.WriteLine(outputDatosInterfaseProvisiones);
							}
							//PROCESO CREDITO
							clasecuenta=DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][7].ToString()+"' ");
							if (clasecuenta=="N")
							{
								DataSet porcentajes=new DataSet();
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									//credito***********************
									outputDatosInterfaseProvisiones="";
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
						
									//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
									//nit
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(nit,12,"0",false);
									outputDatosInterfaseProvisiones+=",";
									//cuenta(10)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][7].ToString(),10,"0",true);
									outputDatosInterfaseProvisiones+=",";
									//vigencia(06)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
									outputDatosInterfaseProvisiones+=",";
									//comprobante(2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									outputDatosInterfaseProvisiones+=",";
									//numero comprobante(6)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseProvisiones+=",";
									//numerodocumento(6)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfaseProvisiones+=",";
									//fecha(8)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
									outputDatosInterfaseProvisiones+=",";
									//detalle(30)
									//outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][5].ToString()+txtVigencia.Text,30," ",true);
									outputDatosInterfaseProvisiones+=",";
									//vrDebito(11.2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseProvisiones+=",";
									//vrcredito(11.2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
									outputDatosInterfaseProvisiones+=",";
									//CCosto(3)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(porcentajes.Tables[0].Rows[y][1].ToString(),3," ",true);
									outputDatosInterfaseProvisiones+=",";
									//Tipo(1)
									outputDatosInterfaseProvisiones+=1;
									outputDatosInterfaseProvisiones+=",";
									//VrBase(11.2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfaseProvisiones+=",";
									//Fec_Proceso(8)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
									outputDatosInterfaseProvisiones+=",";
									//CodUser(4)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
									outputDatosInterfaseProvisiones+=",";
									//prefijodocumento(2)
									outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
									sw3.WriteLine(outputDatosInterfaseProvisiones);

								}
								


							}
							else
							{
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString());
								//credito***********************
								outputDatosInterfaseProvisiones="";
								//valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][2].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
						
								//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
								nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");	
								//nit
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(nit,12,"0",false);
								outputDatosInterfaseProvisiones+=",";
								//cuenta(10)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][7].ToString(),10,"0",true);
								outputDatosInterfaseProvisiones+=",";
								//vigencia(06)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
								outputDatosInterfaseProvisiones+=",";
								//comprobante(2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								outputDatosInterfaseProvisiones+=",";
								//numero comprobante(6)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseProvisiones+=",";
								//numerodocumento(6)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
								outputDatosInterfaseProvisiones+=",";
								//fecha(8)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
								outputDatosInterfaseProvisiones+=",";
								//detalle(30)
								//outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][5].ToString()+txtVigencia.Text,30," ",true);
								outputDatosInterfaseProvisiones+=",";
								//vrDebito(11.2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseProvisiones+=",";
								//vrcredito(11.2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
								outputDatosInterfaseProvisiones+=",";
								//CCosto(3)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(centCosto,3," ",true);
								outputDatosInterfaseProvisiones+=",";
								//Tipo(1)
								outputDatosInterfaseProvisiones+=1;
								outputDatosInterfaseProvisiones+=",";
								//VrBase(11.2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos("",14,"0",false);
								outputDatosInterfaseProvisiones+=",";
								//Fec_Proceso(8)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
								outputDatosInterfaseProvisiones+=",";
								//CodUser(4)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
								outputDatosInterfaseProvisiones+=",";
								//prefijodocumento(2)
								outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
								
								sw3.WriteLine(outputDatosInterfaseProvisiones);

							}
							
							//outputDatosInterfaseProvisiones+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][0].ToString(),10," ",true);
							
							
							//sw3.WriteLine(outputDatosInterfaseProvisiones);
						}//cuentas

					}
				
																																																																																																																		  
				}//for empleados
			}
			sw3.Close();

		}


		protected void EscribirArchivo()
		{
			
			int i=0;
			int registros=0;
			double sumapagado=0,sumapagadoemp=0,sumadescontado=0;
			double sumatotalcredito=0;
			double sumatotaldebito=0;
			string codempleado="";
			string nit;
			string nombconcepto;
			string nombreEntidad="";
				
			int x,y;
			//averiguar el numero de quincena escogido
			string numQuincena=DBFunctions.SingleData("select  mqui_codiquin from dbxschema.mquincenas where mqui_mesquin="+DDLMES.SelectedValue+" and mqui_tpernomi="+DDLQUIN.SelectedValue+" and mqui_anoquin="+DDLANO.SelectedValue+"");
			//Response.Write("<script language:javascript>alert(' entro a numquincena  "+numQuincena+" ');</script>");
			
			DataSet conceptos=new DataSet();
			DBFunctions.Request(conceptos,IncludeSchema.NO,"select cnom_concrftecodi,cnom_concepscodi,cnom_concfondcodi,cnom_concfondsolipens,CNOM_CONCFONDPENSVOLU from dbxschema.cnomina ");
			string cuentaPUC=DBFunctions.SingleData("SELECT CNOM_CUENTAPUC FROM DBXSCHEMA.CNOMINA");		
			double valorcentrocosto=0;
			
			
			DataSet empleados=new DataSet();
			//DBFunctions.Request(empleados,IncludeSchema.NO,"select memp_codiempl from dbxschema.mempleado where test_estado='1'");
//			DBFunctions.Request(empleados,IncludeSchema.NO,"select  memp.memp_codiempl,memp.peps_codieps,peps.peps_nombeps,peps.mnit_nit,MEMP.pfon_codipens,PFP.pfon_nombpens,PFP.mnit_nit,memp.pfon_codipensvolu,PFP2.pfon_nombpens,PFP2.mnit_nit from dbxschema.mempleado memp,dbxschema.peps peps,dbxschema.pfondopension PFP ,dbxschema.pfondopension PFP2 where memp.peps_codieps=peps.peps_codieps and PFP.pfon_codipens=MEMP.pfon_codipens and PFP2.pfon_codipens=memp.pfon_codipensvolu and test_estado='1'");
			DBFunctions.Request(empleados,IncludeSchema.NO,"select  memp.memp_codiempl,memp.peps_codieps,peps.peps_nombeps,peps.mnit_nit,MEMP.pfon_codipens,PFP.pfon_nombpens,PFP.mnit_nit,memp.pfon_codipensvolu,PFP2.pfon_nombpens,PFP2.mnit_nit from dbxschema.mempleado memp,dbxschema.peps peps,dbxschema.pfondopension PFP ,dbxschema.pfondopension PFP2 where memp.peps_codieps=peps.peps_codieps and PFP.pfon_codipens=MEMP.pfon_codipens and PFP2.pfon_codipens=memp.pfon_codipensvolu");
			if (empleados.Tables[0].Rows.Count!=0)

			if (empleados.Tables[0].Rows.Count!=0)
			{
				for (i=0;i<empleados.Tables[0].Rows.Count;i++)
				{
									
					sumapagado=0;
					sumadescontado=0;
					//Averiguar el centro de costo con mayor porcentaje
					string centCosto=DBFunctions.SingleData("Select PCEN_CODIGO from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE mecc_PORCENTAJE=(Select MAX(MECC_PORCENTAJE) from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"') AND MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"'");
					DataSet numcuentas=new DataSet();
					//sacar las cuentas asociadas a los conceptos
					//Response.Write("<script language:javascript>alert(' empleado  "+empleados.Tables[0].Rows[i][0].ToString()+" ');</script>");
					DBFunctions.Request(numcuentas,IncludeSchema.NO,"select DQUI.memp_codiempl,DQUI.mqui_codiquin,DQUI.pcon_concepto,DQUI.dqui_apagar,DQUI.dqui_adescontar,DQUI.dqui_saldo,DQUI.dqui_docrefe,PPUC.MCUE_CODIPUC,PPUC.PDEP_CODIDPTO,MCUE.TCLA_CODIGO from dbxschema.dquincena DQUI,dbxschema.ppucconceptonomina PPUC,dbxschema.mcuenta MCUE where DQUI.pcon_concepto=PPUC.pcon_concepto and MCUE.MCUE_CODIPUC=PPUC.MCUE_CODIPUC and DQUI.mqui_codiquin="+numQuincena+" and memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"' and pdep_codidpto=(select pdep_codidpto from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"') ");
					
	
					codempleado=empleados.Tables[0].Rows[i][0].ToString();
					//Response.Write("<script language:javascript>alert(' registros de cuentas de la quincena "+numcuentas.Tables[0].Rows.Count+" ');</script>");
					string nombreFondoPension=DBFunctions.SingleData("select pfon_nombpens from dbxschema.pfondopension pfon,dbxschema.mempleado memp where pfon.PFON_CODIPENS=memp.PFON_CODIPENS and memp_codiempl='"+codempleado+"'");
					nombreFondoPension="("+nombreFondoPension+")";
					string nombreEps=DBFunctions.SingleData("select PEPS_NOMBEPS from dbxschema.peps peps,dbxschema.mempleado memp where peps.peps_CODIeps=memp.peps_CODIeps and memp_codiempl='"+codempleado+"'");
					nombreEps="("+nombreEps+")";
					if(codempleado=="19226532")
					{
					string numQuincena2=DBFunctions.SingleData("select  mqui_codiquin from dbxschema.mquincenas where mqui_mesquin="+DDLMES.SelectedValue+" and mqui_tpernomi="+DDLQUIN.SelectedValue+" and mqui_anoquin="+DDLANO.SelectedValue+"");
					}
					if (numcuentas.Tables[0].Rows.Count!=0)
					{
					
						

						for (x=0;x<numcuentas.Tables[0].Rows.Count;x++)
						{
							
							//PROCESO DE PORCENTAJES.
							if (numcuentas.Tables[0].Rows[x][9].ToString()=="N")
							{
								//Response.Write("<script language:javascript>alert(' ATENCION: Al empleado  "+numcuentas.Tables[0].Rows[x][0].ToString()+" no se le han definido porcentajes.Interfase Nomina');</script>");
								
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][0].ToString()+"'");
								//Response.Write("<script language:javascript>alert(' tantas filas de porcentajes "+porcentajes.Tables[0].Rows.Count+" empleado "+numcuentas.Tables[0].Rows[x][0].ToString()+"');</script>");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, "ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][0].ToString() + " no se le han definido porcentajes.Interfase Nomina");
								}
							
								//debito
								if (double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())>0)
								{
									//Response.Write("<script language:javascript>alert(' entro a numcuenta debito ');</script>");
					
									for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
									{
										nombreEntidad="";
										outputDatosInterfase="";
										valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
															
										//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
										//validar el nit para eps,fpen,fpensvol
										//1.EPs
										if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][1].ToString())
										{
											nit=empleados.Tables[0].Rows[i][3].ToString();
											nombreEntidad=nombreEps;
										}//2.Fpens
										else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][2].ToString())
										{
											nit=empleados.Tables[0].Rows[i][6].ToString();
											nombreEntidad=nombreFondoPension;
										}//3.FpensVolu
										else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][4].ToString())
										{
											nit=empleados.Tables[0].Rows[i][9].ToString();
										}//4. Fsolipens
										else if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][3].ToString())
										{
											nit=empleados.Tables[0].Rows[i][6].ToString();
										}
										else
										{
											//nit
											nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][0].ToString()+"'");
										
										}
																				
										outputDatosInterfase+=comparacion.Completar_Campos(nit,12,"0",false);
										outputDatosInterfase+=",";
										//cuenta(10)
										outputDatosInterfase+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][7].ToString(),10,"0",true);
										outputDatosInterfase+=",";
										//vigencia(06)
										outputDatosInterfase+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
										outputDatosInterfase+=",";
										//comprobante(2)
										outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
										outputDatosInterfase+=",";
										//numero comprobante(6)
										outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",false);
										outputDatosInterfase+=",";
										//numerodocumento(6)
										outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",false);
										outputDatosInterfase+=",";
										//fecha(8)
										outputDatosInterfase+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
										outputDatosInterfase+=",";
										//detalle(30)
										//outputDatosInterfase+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
										nombconcepto=DBFunctions.SingleData("Select pcon_nombconc from DBXSCHEMA.PCONCEPTONOMINA where pcon_concepto='"+numcuentas.Tables[0].Rows[x][2].ToString()+"'");
										outputDatosInterfase+=comparacion.Completar_Campos( nombconcepto+nombreEntidad+txtVigencia.Text,30," ",true);										
										
										outputDatosInterfase+=",";
										//vrDebito(11.2)
										outputDatosInterfase+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
										outputDatosInterfase+=",";
										//vrcredito(11.2)
										outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
										outputDatosInterfase+=",";
										//CCosto(3)
										outputDatosInterfase+=comparacion.Completar_Campos(porcentajes.Tables[0].Rows[y][1].ToString().Trim(),3," ",true);
										outputDatosInterfase+=",";
										//Tipo(1)
										outputDatosInterfase+=1;
										outputDatosInterfase+=",";
										//VrBase(11.2)
										//validar RETFTE
										if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][0].ToString() )
										{
											string VrBase=DBFunctions.SingleData("select dpaga_afrtfe from dbxschema.DPAGAAFECEPS where mqui_codiquin="+numQuincena+" and memp_codiempl='"+numcuentas.Tables[0].Rows[x][0].ToString()+"'");
											outputDatosInterfase+=comparacion.Completar_Campos(VrBase,14,"0",false);
								
										}
										else
										{
											outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
										}
										outputDatosInterfase+=",";
										//Fec_Proceso(8)
										outputDatosInterfase+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
										outputDatosInterfase+=",";
										//CodUser(4)
										outputDatosInterfase+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
										outputDatosInterfase+=",";
										//prefijodocumento(2)
										outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									
									
									
										 sw.WriteLine(outputDatosInterfase);
									
										sumapagado=sumapagado+valorcentrocosto;
										//Response.Write("<script language:javascript>alert(' SUMA pagado EMPLEADO "+numcuentas.Tables[0].Rows[x][0].ToString()+"VA EN : "+sumapagado+" ');</script>");
									
										sumatotaldebito+=valorcentrocosto;
									

									}
					

								}
									//es credito
								else
								{
									//Response.Write("<script language:javascript>alert(' entro a numcuenta credito  "+porcentajes.Tables[0].Rows.Count+" ');</script>");
					
									for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
									{
										nombreEntidad="";
										outputDatosInterfase="";
										//Response.Write("<script language:javascript>alert(' entro a forrr credito ');</script>");
										valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
										//validar el nit para eps,fpen,fpensvol
										//1.EPs
										if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][1].ToString())
										{
											nit=empleados.Tables[0].Rows[i][3].ToString();
											nombreEntidad=nombreEps;
										}//2.Fpens
										else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][2].ToString())
										{
											nit=empleados.Tables[0].Rows[i][6].ToString();
											nombreEntidad=nombreFondoPension;
										}//3.FpensVolu
										else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][4].ToString())
										{
											nit=empleados.Tables[0].Rows[i][9].ToString();
										}//4. Fsolipens
										else if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][3].ToString())
										{
											nit=empleados.Tables[0].Rows[i][6].ToString();
										}
										else
										{
											//nit
											nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][0].ToString()+"'");
											nombreEntidad="";
										}
																
										outputDatosInterfase+=comparacion.Completar_Campos(nit,12,"0",false);
										outputDatosInterfase+=",";
										//cuenta(10)
										outputDatosInterfase+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][7].ToString(),10,"0",true);
										outputDatosInterfase+=",";
										//vigencia(06)
										outputDatosInterfase+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
										outputDatosInterfase+=",";
										//comprobante(2)
										outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
										outputDatosInterfase+=",";
										//numero comprobante(6)
										outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
										outputDatosInterfase+=",";
										//numerodocumento(6)
										outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
										outputDatosInterfase+=",";
										//fecha(8)
										outputDatosInterfase+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
										outputDatosInterfase+=",";
										//detalle(30)
										//outputDatosInterfase+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
										nombconcepto=DBFunctions.SingleData("Select pcon_nombconc from DBXSCHEMA.PCONCEPTONOMINA where pcon_concepto='"+numcuentas.Tables[0].Rows[x][2].ToString()+"'");
										outputDatosInterfase+=comparacion.Completar_Campos( nombconcepto+nombreEntidad+txtVigencia.Text,30," ",true);										
										outputDatosInterfase+=",";
										//vrDebito(11.2)
										outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
										outputDatosInterfase+=",";
										//vrcredito(11.2)
										outputDatosInterfase+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
										outputDatosInterfase+=",";
										//CCosto(3)
										outputDatosInterfase+=comparacion.Completar_Campos(porcentajes.Tables[0].Rows[y][1].ToString(),3," ",true);
										outputDatosInterfase+=",";
										//Tipo(1)
										outputDatosInterfase+=1;
										outputDatosInterfase+=",";
										//VrBase(11.2)
										//validar RETFTE
										if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][0].ToString() )
										{
											string VrBase=DBFunctions.SingleData("select dpaga_afrtfe from dbxschema.DPAGAAFECEPS where mqui_codiquin="+numQuincena+" and memp_codiempl='"+numcuentas.Tables[0].Rows[x][0].ToString()+"'");
											outputDatosInterfase+=comparacion.Completar_Campos(VrBase,14,"0",false);
								
										}
										else
										{
											outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
										}
										outputDatosInterfase+=",";
										//Fec_Proceso(8)
										outputDatosInterfase+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
										outputDatosInterfase+=",";
										//CodUser(4)
										outputDatosInterfase+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
										outputDatosInterfase+=",";
										//prefijodocumento(2)
										outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									
									
										sw.WriteLine(outputDatosInterfase);
										sumadescontado=sumadescontado-valorcentrocosto;
										//Response.Write("<script language:javascript>alert(' SUMA descontado EMPLEADO "+numcuentas.Tables[0].Rows[x][0].ToString()+"VA EN : "+sumadescontado+" ');</script>");
									
										sumatotalcredito=sumatotalcredito+(valorcentrocosto*-1);
									}
					

								}

							}
							else
							{
								//PROCESO SIN PORCENTAJES
								//debito
								if (double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())>0)
								{
									//Response.Write("<script language:javascript>alert(' entro a numcuenta debito ');</script>");
									
									nombreEntidad="";								
									outputDatosInterfase="";
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
						
									//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									//validar el nit para eps,fpen,fpensvol
									//1.EPs
									if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][1].ToString())
									{
										nit=empleados.Tables[0].Rows[i][3].ToString();
										nombreEntidad=nombreEps;
									}//2.Fpens
									else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][2].ToString())
									{
										nit=empleados.Tables[0].Rows[i][6].ToString();
										nombreEntidad=nombreFondoPension;
									}//3.FpensVolu
									else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][4].ToString())
									{
										nit=empleados.Tables[0].Rows[i][9].ToString();
									}//4. Fsolipens
									else if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][3].ToString())
									{
										nit=empleados.Tables[0].Rows[i][6].ToString();
									}
									else
									{
										
										nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][0].ToString()+"'");
										nombreEntidad="";
									}
									
									outputDatosInterfase+=comparacion.Completar_Campos(nit,12,"0",false);
									outputDatosInterfase+=",";
									//cuenta(10)
									outputDatosInterfase+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][7].ToString(),10,"0",true);
									outputDatosInterfase+=",";
									//vigencia(06)
									outputDatosInterfase+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
									outputDatosInterfase+=",";
									//comprobante(2)
									outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									outputDatosInterfase+=",";
									//numero comprobante(6)
									outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",false);
									outputDatosInterfase+=",";
									//numerodocumento(6)
									outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",false);
									outputDatosInterfase+=",";
									//fecha(8)
									outputDatosInterfase+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
									outputDatosInterfase+=",";
									//detalle(30)
									//outputDatosInterfase+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
									nombconcepto=DBFunctions.SingleData("Select pcon_nombconc from DBXSCHEMA.PCONCEPTONOMINA where pcon_concepto='"+numcuentas.Tables[0].Rows[x][2].ToString()+"'");
									outputDatosInterfase+=comparacion.Completar_Campos( nombconcepto+nombreEntidad+txtVigencia.Text,30," ",true);										
									outputDatosInterfase+=",";
									//vrDebito(11.2)
									outputDatosInterfase+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
									outputDatosInterfase+=",";
									//vrcredito(11.2)
									outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfase+=",";
									//CCosto(3)
									outputDatosInterfase+=comparacion.Completar_Campos(centCosto,3," ",true);
									outputDatosInterfase+=",";
									//Tipo(1)
									outputDatosInterfase+=1;
									outputDatosInterfase+=",";
									//VrBase(11.2)
									//validar RETFTE
									if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][0].ToString() )
									{
										string VrBase=DBFunctions.SingleData("select dpaga_afrtfe from dbxschema.DPAGAAFECEPS where mqui_codiquin="+numQuincena+" and memp_codiempl='"+numcuentas.Tables[0].Rows[x][0].ToString()+"'");
										outputDatosInterfase+=comparacion.Completar_Campos(VrBase,14,"0",false);
								
									}
									else
									{
										outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
									}
									outputDatosInterfase+=",";
									//Fec_Proceso(8)
									outputDatosInterfase+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
									outputDatosInterfase+=",";
									//CodUser(4)
									outputDatosInterfase+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
									outputDatosInterfase+=",";
									//prefijodocumento(2)
									outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									
									
									
									sw.WriteLine(outputDatosInterfase);
									
									sumapagado=sumapagado+valorcentrocosto;
									//Response.Write("<script language:javascript>alert(' SUMA pagado EMPLEADO "+numcuentas.Tables[0].Rows[x][0].ToString()+"VA EN : "+sumapagado+" ');</script>");
									
									sumatotaldebito+=valorcentrocosto;
									

									
					

								}
									//es credito
								else
								{
									//Response.Write("<script language:javascript>alert(' entro a numcuenta credito  "+porcentajes.Tables[0].Rows.Count+" ');</script>");
					
									nombreEntidad="";						
									outputDatosInterfase="";
									//Response.Write("<script language:javascript>alert(' entro a forrr credito ');</script>");
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString());
									//validar el nit para eps,fpen,fpensvol
									//1.EPs
									if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][1].ToString())
									{
										nit=empleados.Tables[0].Rows[i][3].ToString();
										nombreEntidad=nombreEps;
									}//2.Fpens
									else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][2].ToString())
									{
										nit=empleados.Tables[0].Rows[i][6].ToString();
										nombreEntidad=nombreFondoPension;
									}//3.FpensVolu
									else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][4].ToString())
									{
										nit=empleados.Tables[0].Rows[i][9].ToString();
									}//4. Fsolipens
									else if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][3].ToString())
									{
										nit=empleados.Tables[0].Rows[i][6].ToString();
									}
									else
									{
										nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][0].ToString()+"'");
										nombreEntidad="";
									}
									
									outputDatosInterfase+=comparacion.Completar_Campos(nit,12,"0",false);
									outputDatosInterfase+=",";
									//cuenta(10)
									outputDatosInterfase+=comparacion.Completar_Campos(numcuentas.Tables[0].Rows[x][7].ToString(),10,"0",true);
									outputDatosInterfase+=",";
									//vigencia(06)
									outputDatosInterfase+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
									outputDatosInterfase+=",";
									//comprobante(2)
									outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									outputDatosInterfase+=",";
									//numero comprobante(6)
									outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfase+=",";
									//numerodocumento(6)
									outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
									outputDatosInterfase+=",";
									//fecha(8)
									outputDatosInterfase+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
									outputDatosInterfase+=",";
									//detalle(30)
									//outputDatosInterfase+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
									nombconcepto=DBFunctions.SingleData("Select pcon_nombconc from DBXSCHEMA.PCONCEPTONOMINA where pcon_concepto='"+numcuentas.Tables[0].Rows[x][2].ToString()+"'");
									outputDatosInterfase+=comparacion.Completar_Campos( nombconcepto+nombreEntidad+txtVigencia.Text,30," ",true);										
									outputDatosInterfase+=",";
									//vrDebito(11.2)
									outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
									outputDatosInterfase+=",";
									//vrcredito(11.2)
									outputDatosInterfase+=comparacion.Completar_Campos(valorcentrocosto.ToString("F",info),14,"0",false);
									outputDatosInterfase+=",";
									//CCosto(3)
									outputDatosInterfase+=comparacion.Completar_Campos(centCosto,3," ",true);
									outputDatosInterfase+=",";
									//Tipo(1)
									outputDatosInterfase+=1;
									outputDatosInterfase+=",";
									//VrBase(11.2)
									//validar RETFTE
									if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][0].ToString())
									{
										string VrBase=DBFunctions.SingleData("select dpaga_afrtfe from dbxschema.DPAGAAFECEPS where mqui_codiquin="+numQuincena+" and memp_codiempl='"+numcuentas.Tables[0].Rows[x][0].ToString()+"'");
										outputDatosInterfase+=comparacion.Completar_Campos(VrBase,14,"0",false);
								
									}
									else
									{
										outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
									}
									outputDatosInterfase+=",";
									//Fec_Proceso(8)
									outputDatosInterfase+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
									outputDatosInterfase+=",";
									//CodUser(4)
									outputDatosInterfase+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
									outputDatosInterfase+=",";
									//prefijodocumento(2)
									outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
																		
									sw.WriteLine(outputDatosInterfase);
									sumadescontado=sumadescontado-valorcentrocosto;
									//Response.Write("<script language:javascript>alert(' SUMA descontado EMPLEADO "+numcuentas.Tables[0].Rows[x][0].ToString()+"VA EN : "+sumadescontado+" ');</script>");
									sumatotalcredito=sumatotalcredito+(valorcentrocosto*-1);
									
					
								}
						
						
					
							}
			
							
			
						}//cuentas
						//Response.Write("<script language:javascript>alert(' voy a ingresar una contrapartida ');</script>");
						//insertar la contrapartida de nomina.
						outputDatosInterfase="";
						//Response.Write("<script language:javascript>alert(' SUMA PAGADO VA EN  "+sumapagado+" ');</script>");
						//Response.Write("<script language:javascript>alert(' SUMA DESCONTADO VA EN  "+sumadescontado+" ');</script>");
						double ajuste=sumapagado+sumadescontado;
						//Response.Write("<script language:javascript>alert(' no le creooo ');</script>");
						//Response.Write("<script language:javascript>alert(' EL AJUSTE SERA DE "+ajuste+" ');</script>");
						//nit
						nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+codempleado+"'");
						outputDatosInterfase+=comparacion.Completar_Campos(nit,12,"0",false);
						outputDatosInterfase+=",";
						//cuenta(10)
						outputDatosInterfase+=comparacion.Completar_Campos(cuentaPUC,10,"0",true);
						outputDatosInterfase+=",";
						//vigencia(06)
						outputDatosInterfase+=comparacion.Completar_Campos(txtVigencia.Text,6," ",true);
						outputDatosInterfase+=",";
						//comprobante(2)
						outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
						outputDatosInterfase+=",";
						//numero comprobante(6)
						outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
						outputDatosInterfase+=",";
						//numerodocumento(6)
						outputDatosInterfase+=comparacion.Completar_Campos(txtNumeroDocumento.Text,6,"0",true);
						outputDatosInterfase+=",";
						//fecha(8)
						outputDatosInterfase+=comparacion.Completar_Campos(txtFechaComprobante.Text,8," ",true);
						outputDatosInterfase+=",";
						//detalle(30)
						outputDatosInterfase+=comparacion.Completar_Campos("Liquidacion Nomina "+txtVigencia.Text+" ",30," ",true);
						outputDatosInterfase+=",";
						if (ajuste>0)
						{
							outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
							outputDatosInterfase+=",";
							outputDatosInterfase+=comparacion.Completar_Campos(ajuste.ToString("F",info),14,"0",false);
							sumatotalcredito=sumatotalcredito-ajuste;
						}
						else
						{
							outputDatosInterfase+=comparacion.Completar_Campos(ajuste.ToString("F",info),14,"0",false);
							outputDatosInterfase+=",";
							outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
							sumatotaldebito=sumatotaldebito+ajuste;
						}
						outputDatosInterfase+=",";
						//CCosto(3)
						outputDatosInterfase+=comparacion.Completar_Campos(centCosto,3," ",true);
						outputDatosInterfase+=",";
						//Tipo(1)
						outputDatosInterfase+=1;
						outputDatosInterfase+=",";
						//VrBase(11.2)
						outputDatosInterfase+=comparacion.Completar_Campos("",14,"0",false);
						outputDatosInterfase+=",";
						//Fec_Proceso(8)
						outputDatosInterfase+=comparacion.Completar_Campos(DateTime.Now.ToString("yyyyMMdd"),8," ",true);
						outputDatosInterfase+=",";
						//CodUser(4)
						outputDatosInterfase+=comparacion.Completar_Campos(txtCodUsuario.Text,4," ",true);
						outputDatosInterfase+=",";
						//prefijodocumento(2)
						outputDatosInterfase+=comparacion.Completar_Campos(txtCompDiario.Text,2," ",true);
									
						sw.WriteLine(outputDatosInterfase);
					
					}
					


				}//empl
			
				if((Math.Abs(sumatotalcredito))==sumatotaldebito)
				{
                    Utils.MostrarAlerta(Response, "El comprobante fue generado correctamente,Sumas iguales " + sumatotaldebito + "");

				}
				else
				{
                    Utils.MostrarAlerta(Response, "El comprobante es incorrecto no tiene sumas iguales Debito:" + sumatotaldebito + " Credito " + sumatotalcredito + "");
				}
				
				
					
			
				sw.Close();


			}//if

		}



		protected void GenerarPlanillaBanco(object sender, EventArgs e)
		{
			this.iniciarproceso();			
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DataSet defecto= new DataSet();
				DBFunctions.Request(defecto,IncludeSchema.NO,"select MQUI.mqui_anoquin,TMES.tmes_nombre,MQUI.mqui_mesquin,TPER.TPER_DESCRIPCION,MQUI.mqui_tpernomi,MQUI.mqui_codiquin from dbxschema.mquincenas MQUI,dbxschema.tmes TMES ,dbxschema.tperiodonomina TPER where mqui_codiquin=(select max(DQUI.mqui_codiquin) from dbxschema.dquincena DQUI) and TMES.tmes_mes=MQUI.mqui_mesquin AND TPER.tper_periodo=MQUI.mqui_tpernomi");
			
				string quincena=defecto.Tables[0].Rows[0][3].ToString();
				string mes=defecto.Tables[0].Rows[0][1].ToString();
				string ano=defecto.Tables[0].Rows[0][0].ToString();
				
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(DDLQUIN,"SELECT TPER_PERIODO, TPER_DESCRIPCION FROM TPERIODONOMINA");
				DatasToControls.EstablecerDefectoDropDownList(DDLQUIN,quincena);
				bind.PutDatasIntoDropDownList(DDLMES,"Select TMES_MES, TMES_NOMBRE from TMES");
				DatasToControls.EstablecerDefectoDropDownList(DDLMES,mes);
				bind.PutDatasIntoDropDownList(DDLANO,"SELECT PANO_ANO, PANO_DETALLE FROM PANO");
				DatasToControls.EstablecerDefectoDropDownList(DDLANO,ano);
				//bind.PutDatasIntoDropDownList(DDLNOMINA,"Select * from DBXSCHEMA.PDOCUMENTO WHERE TDOC_TIPODOCU='NM' ");
				//bind.PutDatasIntoDropDownList(DDLARP,"Select * from DBXSCHEMA.PDOCUMENTO WHERE TDOC_TIPODOCU='NM' ");
				//bind.PutDatasIntoDropDownList(DDLEPS,"Select * from DBXSCHEMA.PDOCUMENTO WHERE TDOC_TIPODOCU='NM' ");
			}
			info=new CultureInfo("en-US",false).NumberFormat;
			info.NumberDecimalDigits=2;
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
			this.Generar.Click += new System.EventHandler(this.GenerarPlanillaBanco);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		
	}
}
