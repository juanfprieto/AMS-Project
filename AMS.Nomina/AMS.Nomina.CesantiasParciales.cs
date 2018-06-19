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
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Mail;
using AMS.Tools;


namespace AMS.Nomina
{
	public class CesantiasParciales : System.Web.UI.UserControl 
	{
		protected DropDownList DDLANO,DDLMESFINAL,DDLEMPLEADO;
		protected Label LBMESINICIAL,LBPRUEBAS,LBSUELDOPROMEDIO,LBBASELIQUIDACION,LBCESAAPAGAR,LBINTAPAGAR,LBDIASTRABAJADOS;
		protected DataSet valorsueldopromedio,resumencesantias,cesaanteriores,crucedepagos,crucedepagos2,licencias;
		protected DataGrid DATAGRIDCESANTIAS,DATAGRIDCESAANTERIORES;
		protected DataTable DataTable1,DataTable2;
		protected Button BTNLIQUIDARDEFINITIVAMENTE;
		string fechainicio, fechafinal,codempleado,fechainiciocesaanterior,fechafinalcesaanterior;
		string mainpage=ConfigurationManager.AppSettings["MainIndexPage"];
		double valorliquidacion,interesescesantia;
		int errores,diastrabajados;
		protected System.Web.UI.WebControls.PlaceHolder toolsHolder;
		protected System.Web.UI.WebControls.TextBox tbEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.ImageButton ibMail;
		protected System.Web.UI.WebControls.Button BTNLIQUIDAR;
		protected Label lbInfo;
		protected string table;
		protected System.Web.UI.WebControls.Label LBFECHARETIRO;
		protected System.Web.UI.WebControls.Label LBIDENTIFICACION;
		protected System.Web.UI.WebControls.Label LBNOMBREEMPLEADO;
		protected System.Web.UI.WebControls.Label LBCARGO;
		protected System.Web.UI.WebControls.Label LBDEPENDENCIA;
		protected System.Web.UI.WebControls.Label LBSUELDOCARGO;
		protected System.Web.UI.WebControls.Panel panelbaseliquidacion;
		protected System.Web.UI.WebControls.Label LBAÑOCORTE;
		protected System.Web.UI.WebControls.Label LBMESCORTE;
        protected System.Web.UI.WebControls.PlaceHolder phGrilla;
		protected Label LBCESANTIAFINAL,LBINTERESFINAL,LBDIASFINAL;
		protected System.Web.UI.WebControls.TextBox DIASADICIONALES;
		
		protected void LiquidacionCesantiasParciales( object sender, EventArgs e)
		{
			bool pasar      = false;
			string Sql      = "";
			valorsueldopromedio= new DataSet();
            string codempleado = DDLEMPLEADO.SelectedValue;
		    string quinomens = DBFunctions.SingleData("select MEMP_periPAGO from dbxschema.mempleado where memp_codiempl='" + codempleado + "'");
			resumencesantias = new DataSet();
			cesaanteriores  = new DataSet();
			crucedepagos    = new DataSet();
			crucedepagos2   = new DataSet();
			this.armarfechaliqcesantias();
			//averiguar que tipo de contrato tiene el empleado
			Session["codigoempleado"]=codempleado;
			string tiposalario=DBFunctions.SingleData("select tsal_salario from dbxschema.mempleado where memp_codiempl='"+codempleado+"'");
			double promedio =0,valorliquidacion=0;
			double suma     =0,interesescesantia=0,cesantiafinal=0,interesfinal=0;
			int i,diastrabajados=0,diasfinal=0;
            Utils.MostrarAlerta(Response, "Codigo del empleado: " + codempleado + " tipo de SALARIO " + tiposalario + "");
			this.prepargrilla_cesantias();
			this.preparargrilla_cesaanteriores();
			//revisar que dcesantias no exista un registro del empleado con las fechas ingresadas,esto indica que ya se le pago ese periodo.
			
			//si es variable=2, se saca promedio ultimo año o tiempo trabajado
			if (tiposalario=="1")
			{
                Utils.MostrarAlerta(Response, "Este empleado tiene sueldo Integral..");
				Response.Redirect(mainpage+"?process=Nomina.CesantiasParciales");
			}
			
			
			if (tiposalario=="2"  || tiposalario=="3")
			{
				
				Sql = "SELECT sum(DQUI.dqui_apagar - DQUI.DQUI_ADESCONTAR),MQUI.mqui_mesquin,T.TMES_NOMBRE FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI,dbxschema.tmes T WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin AND DQUI.pcon_concepto=PCON.pcon_concepto AND PCON.tres_afeccesantia='S'   AND MQUI.mqui_anoquin="+DDLANO.SelectedValue+"    AND MQUI.mqui_mesquin BETWEEN 1 AND "+DDLMESFINAL.SelectedValue+" AND T.tmes_mes=mqui.mqui_mesquin   AND DQUI.memp_codiempl='"+codempleado+"'   group by mqui.mqui_mesquin, TMES_NOMBRE";
				DBFunctions.Request(valorsueldopromedio,IncludeSchema.NO,Sql);
				
				Sql = "SELECT sum(DQUI.dqui_apagar - DQUI.DQUI_ADESCONTAR),MQUI.mqui_codiquin FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI  WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin AND DQUI.pcon_concepto=PCON.pcon_concepto AND PCON.tres_afeccesantia='S'  AND MQUI.mqui_anoquin=" + DDLANO.SelectedValue+"   AND MQUI.mqui_mesquin BETWEEN 1 AND "+DDLMESFINAL.SelectedValue+" AND DQUI.memp_codiempl='"+codempleado+"' group by MQUI.mqui_codiquin"; 
				DBFunctions.Request(resumencesantias,IncludeSchema.NO,Sql);

				Sql = "SELECT MCESA.MCES_FECHINIC , MCESA.MCES_FECHFINA, DCESA.DCES_VALOCESA,DCESA.DCES_INTECESA,DCESA.MEMP_CODIEMP,DCESA.DCES_DIASTRAB FROM DBXSCHEMA.DCESANTIAS DCESA , DBXSCHEMA.MCESANTIAS MCESA WHERE DCESA.MEMP_CODIEMP='"+codempleado+"'and mcesa.mces_secuencia=dcesa.mces_secuencia";
				DBFunctions.Request(cesaanteriores, IncludeSchema.NO,Sql ); 
				//miro si la fecha inicial de liquidacin esta entre pagos anteriores
				Sql = "SELECT MCESA.MCES_FECHINIC , MCESA.MCES_FECHFINA, DCESA.DCES_VALOCESA,DCESA.DCES_INTECESA,DCESA.MEMP_CODIEMP FROM DBXSCHEMA.DCESANTIAS DCESA , DBXSCHEMA.MCESANTIAS MCESA WHERE DCESA.MEMP_CODIEMP='"+codempleado+"' and '"+fechainicio+"' between mcesa.mces_fechinic and mcesa.mces_fechfina and mcesa.mces_secuencia=dcesa.mces_secuencia";
				DBFunctions.Request(crucedepagos ,IncludeSchema.NO,Sql);
				//miro si la segunda fecha de liquidacion cae en el periodo,caso excepcional. 
				Sql = "SELECT MCESA.MCES_FECHINIC , MCESA.MCES_FECHFINA, DCESA.DCES_VALOCESA,DCESA.DCES_INTECESA,DCESA.MEMP_CODIEMP FROM DBXSCHEMA.DCESANTIAS DCESA , DBXSCHEMA.MCESANTIAS MCESA WHERE DCESA.MEMP_CODIEMP='"+codempleado+"'  and '"+fechafinal+"' between mcesa.mces_fechinic and mcesa.mces_fechfina and mcesa.mces_secuencia=dcesa.mces_secuencia";
				DBFunctions.Request(crucedepagos2 ,IncludeSchema.NO,Sql);
				//valido los errores , muestro mensaje, sumo error.

				if (crucedepagos.Tables.Count > 0)
				{
					if (crucedepagos.Tables[0].Rows.Count > 0)
					{
						pasar = true;
					}
				}
				if (pasar == false)
				{
					if (crucedepagos2.Tables.Count > 0)
					{
						if (crucedepagos2.Tables[0].Rows.Count>0)
						{
							pasar = true;
						}
					}
				}

				if (pasar)
				{
					for(i=0;i<cesaanteriores.Tables[0].Rows.Count;i++)
					{
						cesantiafinal = cesantiafinal + double.Parse(cesaanteriores.Tables[0].Rows[i][2].ToString());
						interesfinal  = interesfinal + double.Parse(cesaanteriores.Tables[0].Rows[i][3].ToString());
						diasfinal     = diasfinal + Int32.Parse(cesaanteriores.Tables[0].Rows[i][5].ToString());
					}

                    Utils.MostrarAlerta(Response, "El periodo de liquidacion escogido ya ha sido liquidado, porfavor revise cuidadosamente las cesantias pagadas anteriormente");
					errores=0;
					Session["errores"]=errores;
					BTNLIQUIDARDEFINITIVAMENTE.Visible=true;
				}
				
				
				for (i=0;i<cesaanteriores.Tables[0].Rows.Count;i++)
				{
					DateTime fechainiciocesaanterior2 = new DateTime();
					DateTime fechafinalcesaanterior2 = new DateTime();
					fechainiciocesaanterior2= Convert.ToDateTime(cesaanteriores.Tables[0].Rows[i][0].ToString());
					fechafinalcesaanterior2 =Convert.ToDateTime(cesaanteriores.Tables[0].Rows[i][1].ToString());
					fechainiciocesaanterior =fechainiciocesaanterior2.Date.ToString("yyyy-MM-dd");
					fechafinalcesaanterior  =fechafinalcesaanterior2.Date.ToString("yyyy-MM-dd");
					this.ingresardatos_cesaanteriores(fechainiciocesaanterior,fechafinalcesaanterior,double.Parse(cesaanteriores.Tables[0].Rows[i][2].ToString()),double.Parse(cesaanteriores.Tables[0].Rows[i][3].ToString()),double.Parse(cesaanteriores.Tables[0].Rows[i][5].ToString()));
				}
				//mirar si se le esta pagando por quincenas=1 o mensualidades=2 ó 4
				if (quinomens=="1")
				{
					diastrabajados=int.Parse(resumencesantias.Tables[0].Rows.Count.ToString())*15;
				}
                if (quinomens == "2" || quinomens == "4")
				{
					diastrabajados=int.Parse(resumencesantias.Tables[0].Rows.Count.ToString())*30;
				}
				for (i=0;i<valorsueldopromedio.Tables[0].Rows.Count;i++)
				{
					this.ingresardatos_cesantias(double.Parse(valorsueldopromedio.Tables[0].Rows[i][0].ToString()),valorsueldopromedio.Tables[0].Rows[i][2].ToString(),int.Parse(valorsueldopromedio.Tables[0].Rows[i][1].ToString()));
					suma+= double.Parse(valorsueldopromedio.Tables[0].Rows[i][0].ToString());
				}
				string  valorbaseliquidacion=DBFunctions.SingleData("SELECT sum(DQUI.dqui_apagar) FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin AND DQUI.pcon_concepto=PCON.pcon_concepto AND PCON.tres_afeccesantia='S' AND MQUI.mqui_anoquin="+DDLANO.SelectedValue+" AND MQUI.mqui_mesquin BETWEEN 1 AND "+DDLMESFINAL.SelectedValue+" AND DQUI.memp_codiempl='"+codempleado+"'");
				if (valorbaseliquidacion=="")
				{
                    Utils.MostrarAlerta(Response, "Este empleado no tiene Registro de Pagos en el periodo de Tiempo Seleccionado. ");
					LBSUELDOPROMEDIO.Text="";
					
				}
				else
				{
					promedio              = (suma/ valorsueldopromedio.Tables[0].Rows.Count);
					LBSUELDOPROMEDIO.Text = promedio.ToString("C");
					LBBASELIQUIDACION.Text= (Math.Round(double.Parse(valorbaseliquidacion),0)).ToString("C");
					LBDIASTRABAJADOS.Text = diastrabajados.ToString();
				
					valorliquidacion      = (promedio*diastrabajados)/360;
				
					LBCESAAPAGAR.Text     = Math.Round(valorliquidacion,0).ToString("C");
					interesescesantia     = (valorliquidacion*diastrabajados*0.12)/360;
				
				
					LBINTAPAGAR.Text      = Math.Round(interesescesantia,0).ToString("C");
					//LBPRUEBAS.Text="SELECT AVG(DQUI.dqui_apagar) FROM dbxschema.dquincena DQUI,dbxschema.pconceptonomina PCON,dbxschema.mquincenas MQUI WHERE MQUI.mqui_codiquin=DQUI.mqui_codiquin AND DQUI.pcon_concepto=PCON.pcon_concepto AND PCON.tres_afeccesantia='S' AND MQUI.mqui_anoquin="+DDLANO.SelectedValue+" AND MQUI.mqui_mesquin BETWEEN 1 AND "+DDLMESFINAL.SelectedValue+" AND DQUI.memp_codiempl='"+codempleado+"'";

					Empleado o_Empleado   = new Empleado("='" + codempleado + "'");
	
					LBCARGO.Text          = o_Empleado.p_PCAR_NOMBCARG;
					LBDEPENDENCIA.Text    = o_Empleado.p_PDEP_NOMBDPTO;
					LBSUELDOCARGO.Text    = Convert.ToDouble(o_Empleado.p_MEMP_SUELACTU).ToString("C");
					LBNOMBREEMPLEADO.Text = o_Empleado.p_MNIT_APELLIDOS.Trim() + " " + o_Empleado.p_MNIT_APELLIDO2.Trim()+ " " + o_Empleado.p_MNIT_NOMBRES.Trim() + " " + o_Empleado.p_MNIT_NOMBRE2;
					LBIDENTIFICACION.Text = o_Empleado.p_MEMP_CODIEMPL;
					LBAÑOCORTE.Text       = DDLANO.SelectedValue;
					LBMESCORTE.Text       = DDLMESFINAL.SelectedValue;

					LBCESANTIAFINAL.Text  = Math.Round(valorliquidacion-cesantiafinal,0).ToString("C");
					LBINTERESFINAL.Text   = Math.Round(interesescesantia-interesfinal,0).ToString("C");
					int totaldias         = diastrabajados -diasfinal + Int32.Parse(DIASADICIONALES.Text);
					LBDIASFINAL.Text      = totaldias.ToString();

					Session["valorliquidacion"]=valorliquidacion-cesantiafinal;
					Session["interesescesantia"]=interesescesantia-interesfinal;
					Session["diastrabajados"]=diastrabajados-diasfinal + Int32.Parse(DIASADICIONALES.Text);
                    Session["sueldoPromedio"] = promedio;
                }
				
			}
			BTNLIQUIDARDEFINITIVAMENTE.Visible = true;
			BTNLIQUIDARDEFINITIVAMENTE.Enabled = true;
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
	

		protected void armarfechaliqcesantias()
		{
			fechainicio=DDLANO.SelectedValue+"-01"+"-01";
			if(Convert.ToInt32(DDLMESFINAL.SelectedValue)>=1 && Convert.ToInt32(DDLMESFINAL.SelectedValue)<=9)
				{
				if(Convert.ToInt32(DDLMESFINAL.SelectedValue)==2)
				{
				
				fechafinal=DDLANO.SelectedValue+"-0"+DDLMESFINAL.SelectedValue+"-"+"28";
				
				}
				else
				{
					
					fechafinal=DDLANO.SelectedValue+"-0"+DDLMESFINAL.SelectedValue+"-"+"30";
			
				}
			   }
				else
			{
				fechafinal=DDLANO.SelectedValue+"-"+DDLMESFINAL.SelectedValue+"-"+"30";
			
			}		
				//Response.Write("<script language:java>alert(' "+fechainicio+" "+fechafinal+" ');</script>");
				Session["fechainicio"]=fechainicio;
				Session["fechafinal"]=fechafinal;
				
					
		}
		
		
		protected void grabarcesantiasparciales(object sender ,EventArgs e)
		{
			if (errores>0)
			{
                Utils.MostrarAlerta(Response, "Tiene errores en la Liquidacion de Cesantias Parciales,porfavor corrijalos. ");
			}
			else
			{

                double valorliquidacion = 0, interesescesantia = 0, sueldopromedio = 0;
			    interesescesantia   = (double)Session["interesescesantia"];
			    valorliquidacion    = (double)Session["valorliquidacion"];
                sueldopromedio      = (double)Session["sueldoPromedio"];
			    string fechainicio  = (string)Session["fechainicio"];
			    string fechafinal   = (string)Session["fechafinal"];
			    string codigoempleado=(string)Session["codigoempleado"];
			    int diastrabajados  = (int)Session["diastrabajados"];
						                                  
			    try
                {
                    DBFunctions.NonQuery("insert into dbxschema.mcesantias values (default,'" + fechainicio + "','" + fechafinal + "')");
                    string secuencia = DBFunctions.SingleData("select max(mces_secuencia) from dbxschema.mcesantias");
                    DBFunctions.NonQuery("insert into dbxschema.dcesantias values (" + valorliquidacion + "," + interesescesantia + ",'" + codigoempleado + "',default," + secuencia + "," + diastrabajados + "," + sueldopromedio + ")");
                    Utils.MostrarAlerta(Response, "Proceso Liquidación Cesantias parciales " + valorliquidacion + " mas intereses " + interesescesantia + " Realizado Exitosamente !!!");
                }
                catch 
                {
                    Utils.MostrarAlerta(Response, "Fallé al grabar censatías...Liquidacion no realizada !!! ");
                } 
            }
			
		}
		
		protected void ingresardatos_cesantias(double valorpagado,string mes,int numero)
		{
			DataRow fila = DataTable1.NewRow();
			fila["VALOR PAGADO"]=valorpagado;
			fila["MES"]=mes;
			DataTable1.Rows.Add(fila);
			DATAGRIDCESANTIAS.DataSource=DataTable1;
			DATAGRIDCESANTIAS.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DATAGRIDCESANTIAS);
			DatasToControls.JustificacionGrilla(DATAGRIDCESANTIAS,DataTable1);
		}
		
		protected void ingresardatos_cesaanteriores(string fechaInicio,string fechaFinal,double Cesantias,double interesescesantia,double diastrabajados)
		{
			DataRow fila=DataTable2.NewRow();
			fila["FECHA INICIO"]=fechaInicio;
			fila["FECHA FINAL"]=fechaFinal;
			fila["CESANTIAS"]=Cesantias;
			fila["INTERESES DE CESANTIA"]=interesescesantia;
			fila["DIAS TRABAJADOS"]=diastrabajados;
			DataTable2.Rows.Add(fila);
			DATAGRIDCESAANTERIORES.DataSource=DataTable2;
			DATAGRIDCESAANTERIORES.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DATAGRIDCESAANTERIORES);
			DatasToControls.JustificacionGrilla(DATAGRIDCESAANTERIORES,DataTable2);
		}
		
		protected void prepargrilla_cesantias()
		{
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("VALOR PAGADO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("MES",System.Type.GetType("System.String")));
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
		
		


		protected void Page_Load(object sender , EventArgs e)
		{
			if (!IsPostBack)
			{
				Session.Clear();
				DatasToControls param = new DatasToControls();
				param.PutDatasIntoDropDownList(DDLANO,"SELECT PANO_ANO,PANO_DETALLE FROM PANO order by pano_ano");
				param.PutDatasIntoDropDownList(DDLMESFINAL,"Select pMES_MES, pMES_NOMBRE from pMES order by pmes_mes");
                param.PutDatasIntoDropDownList(DDLEMPLEADO, "SELECT M.MEMP_CODIEMPL, N.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(N.MNIT_APELLIDO2,'') CONCAT ' ' CONCAT N.MNIT_NOMBRES CONCAT ' ' CONCAT coalesce(N.MNIT_NOMBRE2,'') FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT and M.test_estado <> '4' order by N.MNIT_APELLIDOS");
			}
			else
			{
			if (Session["valorliquidacion"]!=null)
				valorliquidacion=(double)Session["valorliquidacion"];
			if (Session["fechainicio"]!=null)
				fechainicio=(string)Session["fechainicio"];
			if (Session["fechafinal"]!=null)
				fechafinal=(string)Session["fechafinal"];
			if(Session["codigoempleado"]!=null)
				codempleado=(string)Session["codigoempledo"];
			if(Session["interesescesantia"]!=null)
				interesescesantia=(double)Session["interesescesantia"];
			if (Session["errores"]!=null)
				errores=(int)Session["errores"];
			if(Session["diastrabajados"]!=null)
				diastrabajados=(int)Session["diastrabajados"];
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
	
