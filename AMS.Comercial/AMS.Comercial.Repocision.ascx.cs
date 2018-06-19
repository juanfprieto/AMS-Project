namespace AMS.Comercial
{
	using System;
	using System.Configuration;
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

	/// <summary>
	///		Descripción breve de AMS_Comercial_Repocision.
	/// </summary>
	public class AMS_Comercial_Repocision : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList placa;
		protected System.Web.UI.WebControls.Button Generar;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.TextBox tiquetesauto;
		protected System.Web.UI.WebControls.TextBox tiquetesmanu;
		protected System.Web.UI.WebControls.TextBox tiquetespre;
		protected System.Web.UI.WebControls.TextBox totaltiquetes;
		protected System.Web.UI.WebControls.TextBox remesasauto;
		protected System.Web.UI.WebControls.TextBox remesasmanu;
		protected System.Web.UI.WebControls.TextBox totalremesas;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Button detalles;
		protected System.Web.UI.WebControls.Panel Panel2;
		protected System.Web.UI.WebControls.TextBox totalventas;
		protected System.Web.UI.WebControls.Button LIQUIDAR;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox valorcuota;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox ultimaliq;
		protected System.Web.UI.WebControls.Label fechaultim;
		protected System.Web.UI.WebControls.TextBox acumliq;
		protected System.Web.UI.WebControls.Label fecha;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.Label añov;
		protected System.Web.UI.WebControls.Label mesv;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label1;
		double Cuota=0;
		double [] Cuota2;
		protected System.Web.UI.WebControls.Button liquitotal;
		protected System.Web.UI.WebControls.Panel Panel3;
		protected System.Web.UI.WebControls.DataGrid Grid;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.Label numero;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.Label recaudo;
		protected DataSet lineas;
		protected DataSet lineas2;
		protected DataTable resultado;
		
		protected System.Web.UI.WebControls.Button Liquidar2;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];


		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				añov.Text=DBFunctions.SingleData("select CTRANS_ANO from DBXSCHEMA.CTRANSPORTES").ToString();
				mesv.Text=DBFunctions.SingleData("select CTRANS_MES from DBXSCHEMA.CTRANSPORTES").ToString();
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				DatasToControls bind = new DatasToControls();
				int contadorBus=0;
				if(contadorBus==0)
				{
					bind.PutDatasIntoDropDownList(placa,"Select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
					ListItem it=new ListItem("--PLACA--","0");
					placa.Items.Insert(0,it);
				}
				string añoVigencia=DBFunctions.SingleData("select CTRANS_ANO from DBXSCHEMA.CTRANSPORTES");
				string mesVigencia=DBFunctions.SingleData("select CTRANS_MES from DBXSCHEMA.CTRANSPORTES");
			
			}
			else
			{
				if (Session["Cuota"]!=null)
				{
					Cuota=Convert.ToDouble(Session["Cuota"]);
				}
				if(Session["Cuota2"] != null)
				{
					Cuota2=(double[])Session["Cuota2"]; 
				}
			}
		}
		public  void  generar_OnClick(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			Panel1.Visible=true;
			Panel3.Visible=false;
			Panel3.Width=0;
			Panel3.Height=0;
			liquitotal.Enabled=false;
			
			int AñoA=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_ANO from DBXSCHEMA.CTRANSPORTES"));
			int MesA=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_MES from DBXSCHEMA.CTRANSPORTES"));
			int DiaA=Convert.ToInt32(DateTime.Now.Day.ToString());
				
				
			string TiquetesAutomaticosS=DBFunctions.SingleData("select sum(VAL_TIQUETE) from DBXSCHEMA.DTIQUETE WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND YEAR(DTIQ_FECHA)="+AñoA+" AND MONTH(DTIQ_FECHA)="+MesA+" AND DAY(DTIQ_FECHA)="+DiaA+" ");
			double TiquetesAutomaticos=0;
			if(TiquetesAutomaticosS.Equals(null) || TiquetesAutomaticosS.Equals(""))
			{
				TiquetesAutomaticos=0;
			}
			else
			{
				TiquetesAutomaticos=Convert.ToDouble(DBFunctions.SingleData("select sum(VAL_TIQUETE) from DBXSCHEMA.DTIQUETE WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND YEAR(DTIQ_FECHA)="+AñoA+" AND MONTH(DTIQ_FECHA)="+MesA+" AND DAY(DTIQ_FECHA)="+DiaA+" "));
			}
				
			////////////////////////////
			string TiquetesManualesS=DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+placa.SelectedValue.ToString()+"' AND year(FECHA_PLANILLA)="+AñoA+" AND MONTH(FECHA_PLANILLA)="+MesA+" AND DAY(FECHA_PLANILLA)="+DiaA+" and TTIPO_TIQUETE=1");
			double TiquetesManuales=0;
			if(TiquetesManualesS.Equals(null) || TiquetesManualesS.Equals(""))
			{
				TiquetesManuales=0;
			}
			else
			{
				TiquetesManuales=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+placa.SelectedValue.ToString()+"' AND year(FECHA_PLANILLA)="+AñoA+" AND MONTH(FECHA_PLANILLA)="+MesA+" AND DAY(FECHA_PLANILLA)="+DiaA+" and TTIPO_TIQUETE=1"));
			}
				
			////////////////////////////
			string TiquetesPrepagoS=DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+placa.SelectedValue.ToString()+"' AND year(FECHA_PLANILLA)="+AñoA+" AND MONTH(FECHA_PLANILLA)="+MesA+" AND DAY(FECHA_PLANILLA)="+DiaA+"and TTIPO_TIQUETE=2");
			double TiquetesPrepago=0;
			if(TiquetesPrepagoS.Equals(null) || TiquetesPrepagoS.Equals(""))
			{
				TiquetesPrepago=0;
			}
			else
			{
				TiquetesPrepago=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+placa.SelectedValue.ToString()+"' AND year(FECHA_PLANILLA)="+AñoA+" AND MONTH(FECHA_PLANILLA)="+MesA+" AND DAY(FECHA_PLANILLA)="+DiaA+" and TTIPO_TIQUETE=2"));
			}
				
			/////////////////////////////
			string RemesaAutomaticaS=DBFunctions.SingleData("SELECT SUM(VALO_FLET) FROM DBXSCHEMA.MREMESA WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND YEAR(MREM_FECHA)="+AñoA+" AND MONTH(MREM_FECHA)="+MesA+" AND DAY(MREM_FECHA)="+DiaA+" ");
			double RemesaAutomatica=0;
			if(RemesaAutomaticaS.Equals(null) || RemesaAutomaticaS.Equals(""))
			{
				RemesaAutomatica=0;			
			}
			else
			{
				RemesaAutomatica=Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(VALO_FLET) FROM DBXSCHEMA.MREMESA WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND YEAR(MREM_FECHA)="+AñoA+" AND MONTH(MREM_FECHA)="+MesA+" AND DAY(MREM_FECHA)="+DiaA+" "));
			}
				
			/////////////////////////////
			string RemesasManualesS=DBFunctions.SingleData("select sum(VALOR_REMESA) from DBXSCHEMA.MREMESA_MANUAL where MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND YEAR(FECHA_REMESA)="+AñoA+" AND MONTH(FECHA_REMESA)="+MesA+" AND DAY(FECHA_REMESA)="+DiaA+" ");
			double RemesasManuales=0;
			if(RemesasManualesS.Equals(null) || RemesasManualesS.Equals(""))
			{
				RemesasManuales=0;
			}
			else
			{
				RemesasManuales=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_REMESA) from DBXSCHEMA.MREMESA_MANUAL where MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND YEAR(FECHA_REMESA)="+AñoA+" AND MONTH(FECHA_REMESA)="+MesA+" AND DAY(FECHA_REMESA)="+DiaA+" "));
			}
				
			/////////////////////////////
			double TotalVentasTiquetes=TiquetesAutomaticos+TiquetesManuales+TiquetesPrepago;
			double TotalVentasRemesas=RemesasManuales+RemesaAutomatica;
			double TotalVentas=TotalVentasTiquetes+TotalVentasRemesas;

			string TotalVentasTiquetesF=String.Format("{0:C}",TotalVentasTiquetes);
			string TotalVentasRemesasF=String.Format("{0:C}",TotalVentasRemesas);
			string TotalVentasF=String.Format("{0:C}",TotalVentas);
			string RMFormato=String.Format("{0:C}",RemesasManuales);
			string RAFormato=String.Format("{0:C}",RemesaAutomatica);
			string TPFormato=String.Format("{0:C}",TiquetesPrepago); 
			string TMFormato=String.Format("{0:C}",TiquetesManuales);
			string TAFormato=String.Format("{0:C}",TiquetesAutomaticos);
				
			tiquetesauto.Text=TAFormato.ToString();
			tiquetesmanu.Text=TMFormato.ToString();
				
			tiquetespre.Text=TPFormato.ToString();
			remesasauto.Text=RAFormato.ToString();
			remesasmanu.Text=RMFormato.ToString();
			totaltiquetes.Text=TotalVentasTiquetesF.ToString();
			totalremesas.Text=TotalVentasRemesasF.ToString();
			totalventas.Text=TotalVentasF.ToString();
				
			double porcentaje=Math.Round(Convert.ToDouble(DBFunctions.SingleData("Select TREP_PORCENTAJE from DBXSCHEMA.TREPOSICIONVEHI")),2);
			double CalculoPorcentaje=TotalVentas* (porcentaje/100);
			string PorcentajeFormato=String.Format("{0:C}",CalculoPorcentaje);
			valorcuota.Text=PorcentajeFormato.ToString();
			Session["Cuota"]=CalculoPorcentaje;
				

			////////////////////////////
		}
		
		
		public  void  Detalles_OnClick(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			Panel2.Visible=true;
			int AñoA=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_ANO from DBXSCHEMA.CTRANSPORTES"));
			int MesA=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_MES from DBXSCHEMA.CTRANSPORTES"));
			int DiaA=Convert.ToInt32(DateTime.Now.Day.ToString());
			
			string UltimaLiq=DBFunctions.SingleData("SELECT MREPO_FECHAULTIMA from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'");
			
			if(UltimaLiq.Equals("") || UltimaLiq.Equals(null))
			{
				UltimaLiq="No Posee";
			}
			else
			{
				UltimaLiq=Convert.ToDateTime(DBFunctions.SingleData("SELECT MREPO_FECHAULTIMA from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'")).ToString("yyyy-MM-dd");		
			}
			fechaultim.Text=UltimaLiq.ToString();
			
			string UltiValorS=DBFunctions.SingleData("SELECT MREPO_ULTIVALLIQ from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' ");
			string AcumuladoS=DBFunctions.SingleData("SELECT MREPO_VALORACUM from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' ");
			double UltiValor=0;
			double Acumulado=0;
			if(UltiValorS.Equals(null) || UltiValorS.Equals(""))
			{
				UltiValor=0;
			}
			else
			{
				UltiValor=Convert.ToDouble(DBFunctions.SingleData("SELECT MREPO_ULTIVALLIQ from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' "));
                		
			}
			if(AcumuladoS.Equals("") || AcumuladoS.Equals(null))
			{
			
				Acumulado=0;	
			}
			else
			{
				Acumulado=Convert.ToDouble(DBFunctions.SingleData("SELECT MREPO_VALORACUM from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' "));
			}
			string UltimVFormato=String.Format("{0:C}",UltiValor);
			string AcumuladoFormato=String.Format("{0:C}",Acumulado);
			
			ultimaliq.Text=UltimVFormato;
			acumliq.Text=AcumuladoFormato.ToString();

		}


		public  void  Liquidar_OnClick(Object  Sender, EventArgs e)
		{
			

			string FechaUltimoCalculo=Convert.ToDateTime(DBFunctions.SingleData("SELECT MREPO_FECHAULTIMA from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'")).ToString("yyyy-MM-dd");
			string FechaActual=Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd");
			
			if(FechaUltimoCalculo.Equals("") || FechaUltimoCalculo.Equals(null))
			{
				FechaUltimoCalculo=DateTime.Now.Date.ToString("yyyy-MM-dd");
			
			}
			else
			{
				FechaUltimoCalculo=Convert.ToDateTime(DBFunctions.SingleData("SELECT MREPO_FECHAULTIMA from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'")).ToString("yyyy-MM-dd");
			}
			string[] fechaCalculo=FechaUltimoCalculo.Split('-');
			int AñoCalculo=Convert.ToInt32(fechaCalculo[0]);
			int MesCalculo=Convert.ToInt32(fechaCalculo[1]);
			int AñoVigente=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_ANO from DBXSCHEMA.CTRANSPORTES"));
			int MesVigente=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_MES from DBXSCHEMA.CTRANSPORTES"));
			
			string Primeravez=DBFunctions.SingleData("Select * from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'");
			double ValorCuota=Math.Round((double)Session["Cuota"],2);
			if(Primeravez.Equals(null) || Primeravez.Equals(""))
			{
				
				DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MREPOSICIONVEHICULAR VALUES(DEFAULT,'"+fecha.Text.ToString()+"',"+ValorCuota+","+ValorCuota+",'"+placa.SelectedValue.ToString()+"') ");
			}
			else
			{
				if(AñoVigente != AñoCalculo || MesVigente != MesCalculo)
				{
					int codigo=Convert.ToInt32(DBFunctions.SingleData("Select MREPO_CODIGO from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'"));
						
					DBFunctions.NonQuery("UPDATE DBXSCHEMA.MREPOSICIONVEHICULAR SET MREPO_FECHAULTIMA='"+fecha.Text.ToString()+"',MREPO_ULTIVALLIQ="+ValorCuota+",MREPO_VALORACUM="+ValorCuota+" WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MREPO_CODIGO="+codigo+" ");
					DBFunctions.NonQuery("UPDATE DBXSCHEMA.CTRANSPORTES SET CTRANS_ANO="+Convert.ToInt32(DateTime.Now.Year.ToString())+",CTRANS_MES="+Convert.ToInt32(DateTime.Now.Month.ToString())+" where CTRANS_CODIGO=1");
				}
					///////////////////
				else
				{
					if(FechaUltimoCalculo.Equals(""+FechaActual+""))
					{
						
						int codigo=Convert.ToInt32(DBFunctions.SingleData("Select MREPO_CODIGO from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'"));
						double Acum_anterior=Convert.ToDouble(DBFunctions.SingleData("select MREPO_VALORACUM from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MREPO_CODIGO="+codigo+" "));
						double Cuota_Anterior=Convert.ToDouble(DBFunctions.SingleData("select MREPO_ULTIVALLIQ from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MREPO_CODIGO="+codigo+" "));
						Acum_anterior=Acum_anterior - Cuota_Anterior;
						Acum_anterior=Acum_anterior + ValorCuota;
						string queryP="UPDATE DBXSCHEMA.MREPOSICIONVEHICULAR SET MREPO_FECHAULTIMA='"+fecha.Text.ToString()+"',MREPO_ULTIVALLIQ="+ValorCuota+",MREPO_VALORACUM="+Acum_anterior+" WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MREPO_CODIGO="+codigo+" ";
						DBFunctions.NonQuery("UPDATE DBXSCHEMA.MREPOSICIONVEHICULAR SET MREPO_FECHAULTIMA='"+fecha.Text.ToString()+"',MREPO_ULTIVALLIQ="+ValorCuota+",MREPO_VALORACUM="+Acum_anterior+" WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MREPO_CODIGO="+codigo+" ");
					}
					else
					{
						int codigo=Convert.ToInt32(DBFunctions.SingleData("Select MREPO_CODIGO from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'"));
						double AcumuladoFinal=Convert.ToDouble(DBFunctions.SingleData("select MREPO_VALORACUM from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MREPO_CODIGO="+codigo+" "))+ ValorCuota;
						DBFunctions.NonQuery("UPDATE DBXSCHEMA.MREPOSICIONVEHICULAR SET MREPO_FECHAULTIMA='"+fecha.Text.ToString()+"',MREPO_ULTIVALLIQ=0,MREPO_VALORACUM=0 WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MREPO_CODIGO="+codigo+" ");
					}
					DBFunctions.NonQuery("UPDATE DBXSCHEMA.CTRANSPORTES SET CTRANS_ANO="+Convert.ToInt32(DateTime.Now.Year.ToString())+",CTRANS_MES="+Convert.ToInt32(DateTime.Now.Month.ToString())+" where CTRANS_CODIGO=1");
				}
			}
			Label17.Text=DBFunctions.exceptions;
			Response.Write("<script language='javascript'>alert('Liquidacion Almacenada');</script>");
			Response.Redirect(indexPage+"?process=Comercial.Repocision");
		}

		public  void  LiquidarTotal_OnClick(Object  Sender, EventArgs e)
		{
			Panel1.Visible=false;
			Panel3.Visible=true;
			Generar.Enabled=false;
			double totalConteo=0;
			DatasToControls bind = new DatasToControls();
			int AñoA=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_ANO from DBXSCHEMA.CTRANSPORTES"));
			int MesA=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_MES from DBXSCHEMA.CTRANSPORTES"));
			int DiaA=Convert.ToInt32(DateTime.Now.Day.ToString());
			//////
			this.PrepararTabla();
			lineas = new DataSet();
			int conteTotal=0;
			
			DBFunctions.Request(lineas,IncludeSchema.NO,"select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
			double[] CuotaFinal=new double[lineas.Tables[0].Rows.Count];
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				
				//////////PROCESO CONTEO DE VENTAS	
							
				string TiquetesAutomaticosS=DBFunctions.SingleData("select sum(VAL_TIQUETE) from DBXSCHEMA.DTIQUETE WHERE MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND YEAR(DTIQ_FECHA)="+AñoA+" AND MONTH(DTIQ_FECHA)="+MesA+" AND DAY(DTIQ_FECHA)="+DiaA+" ");
				double TiquetesAutomaticos=0;
				if(TiquetesAutomaticosS.Equals(null) || TiquetesAutomaticosS.Equals(""))
				{
					TiquetesAutomaticos=0;
				}
				else
				{
					TiquetesAutomaticos=Convert.ToDouble(DBFunctions.SingleData("select sum(VAL_TIQUETE) from DBXSCHEMA.DTIQUETE WHERE MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND YEAR(DTIQ_FECHA)="+AñoA+" AND MONTH(DTIQ_FECHA)="+MesA+" AND DAY(DTIQ_FECHA)="+DiaA+" "));
				}
				
				////////////////////////////
				string TiquetesManualesS=DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND year(FECHA_PLANILLA)="+AñoA+" AND MONTH(FECHA_PLANILLA)="+MesA+" AND DAY(FECHA_PLANILLA)="+DiaA+" and TTIPO_TIQUETE=1");
				double TiquetesManuales=0;
				if(TiquetesManualesS.Equals(null) || TiquetesManualesS.Equals(""))
				{
					TiquetesManuales=0;
				}
				else
				{
					TiquetesManuales=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND year(FECHA_PLANILLA)="+AñoA+" AND MONTH(FECHA_PLANILLA)="+MesA+" AND DAY(FECHA_PLANILLA)="+DiaA+" and TTIPO_TIQUETE=1"));
				}
				
				////////////////////////////
				string TiquetesPrepagoS=DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND year(FECHA_PLANILLA)="+AñoA+" AND MONTH(FECHA_PLANILLA)="+MesA+" AND DAY(FECHA_PLANILLA)="+DiaA+"and TTIPO_TIQUETE=2");
				double TiquetesPrepago=0;
				if(TiquetesPrepagoS.Equals(null) || TiquetesPrepagoS.Equals(""))
				{
					TiquetesPrepago=0;
				}
				else
				{
					TiquetesPrepago=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND year(FECHA_PLANILLA)="+AñoA+" AND MONTH(FECHA_PLANILLA)="+MesA+" AND DAY(FECHA_PLANILLA)="+DiaA+" and TTIPO_TIQUETE=2"));
				}
				
				/////////////////////////////
				string RemesaAutomaticaS=DBFunctions.SingleData("SELECT SUM(VALO_FLET) FROM DBXSCHEMA.MREMESA WHERE MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND YEAR(MREM_FECHA)="+AñoA+" AND MONTH(MREM_FECHA)="+MesA+" AND DAY(MREM_FECHA)="+DiaA+" ");
				double RemesaAutomatica=0;
				if(RemesaAutomaticaS.Equals(null) || RemesaAutomaticaS.Equals(""))
				{
					RemesaAutomatica=0;			
				}
				else
				{
					RemesaAutomatica=Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(VALO_FLET) FROM DBXSCHEMA.MREMESA WHERE MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND YEAR(MREM_FECHA)="+AñoA+" AND MONTH(MREM_FECHA)="+MesA+" AND DAY(MREM_FECHA)="+DiaA+" "));
				}
				
				/////////////////////////////
				string RemesasManualesS=DBFunctions.SingleData("select sum(VALOR_REMESA) from DBXSCHEMA.MREMESA_MANUAL where MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND YEAR(FECHA_REMESA)="+AñoA+" AND MONTH(FECHA_REMESA)="+MesA+" AND DAY(FECHA_REMESA)="+DiaA+" ");
				double RemesasManuales=0;
				if(RemesasManualesS.Equals(null) || RemesasManualesS.Equals(""))
				{
					RemesasManuales=0;
				}
				else
				{
					RemesasManuales=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_REMESA) from DBXSCHEMA.MREMESA_MANUAL where MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND YEAR(FECHA_REMESA)="+AñoA+" AND MONTH(FECHA_REMESA)="+MesA+" AND DAY(FECHA_REMESA)="+DiaA+" "));
				}
				
				/////////////////////////////
				double TotalVentasTiquetes=TiquetesAutomaticos+TiquetesManuales+TiquetesPrepago;
				double TotalVentasRemesas=RemesasManuales+RemesaAutomatica;
				double TotalVentas=TotalVentasTiquetes+TotalVentasRemesas;
				double porcentaje=Math.Round(Convert.ToDouble(DBFunctions.SingleData("Select TREP_PORCENTAJE from DBXSCHEMA.TREPOSICIONVEHI")),2);
				double CalculoPorcentaje=TotalVentas* (porcentaje/100);
				string TotalVentasF=String.Format("{0:C}",TotalVentas);
				string PorcentajeFormato=String.Format("{0:C}",CalculoPorcentaje);
			
				CuotaFinal[i]=CalculoPorcentaje;
				
				//////////////////////////////////
				conteTotal=conteTotal+1;
				totalConteo=totalConteo+CalculoPorcentaje;
				DataRow fila;
				fila= resultado.NewRow();
				fila["PLACA"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["TOTALVENTAS"]	= TotalVentasF.ToString();
				fila["VALORCUOTA"]	=PorcentajeFormato.ToString();
				fila["FECHA"]	= DateTime.Now.Date.ToString("yyyy-MM-dd");
				
				
				
				resultado.Rows.Add(fila);
						

			}
			Session["Cuota2"]=CuotaFinal;
			
			//fin sentencia FOR	
			Grid.DataSource = resultado;
			Grid.DataBind();
			numero.Text=conteTotal.ToString();
			string FormatoTotalConteo=String.Format("{0:C}",totalConteo);
			recaudo.Text=FormatoTotalConteo.ToString();
			
		}
		public void PrepararTabla()
		{
			resultado = new DataTable();
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn placa = new DataColumn();
			placa.DataType = System.Type.GetType("System.String");
			placa.ColumnName = "PLACA";
			placa.ReadOnly=true;
			resultado.Columns.Add(placa);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn totalventas = new DataColumn();
			totalventas.DataType = System.Type.GetType("System.String");
			totalventas.ColumnName = "TOTALVENTAS";
			totalventas.ReadOnly=true;
			resultado.Columns.Add(totalventas);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valorcuota = new DataColumn();
			valorcuota.DataType = System.Type.GetType("System.String");
			valorcuota.ColumnName = "VALORCUOTA";
			valorcuota.ReadOnly=true;
			resultado.Columns.Add(valorcuota);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn fecha = new DataColumn();
			fecha.DataType = System.Type.GetType("System.String");
			fecha.ColumnName = "FECHA";
			fecha.ReadOnly=true;
			resultado.Columns.Add(fecha);
						
		}
		public  void  LiquidarFinal_OnClick(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			Liquidar2.Enabled=false;
			int AñoA=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_ANO from DBXSCHEMA.CTRANSPORTES"));
			int MesA=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_MES from DBXSCHEMA.CTRANSPORTES"));
			int DiaA=Convert.ToInt32(DateTime.Now.Day.ToString());
			lineas2 = new DataSet();
			DBFunctions.Request(lineas2,IncludeSchema.NO,"select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
			for(int i=0;i<lineas2.Tables[0].Rows.Count;i++)
			{
				//////////////
				string FechaUltimoCalculo=DBFunctions.SingleData("SELECT MREPO_FECHAULTIMA from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'");
				//Convert.ToDateTime(DBFunctions.SingleData("SELECT MREPO_FECHAULTIMA from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'")).ToString("yyyy-MM-dd");
				string FechaActual=Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd");
			
				if(FechaUltimoCalculo.Equals("") || FechaUltimoCalculo.Equals(null))
				{
					FechaUltimoCalculo=DateTime.Now.Date.ToString("yyyy-MM-dd");
			
				}
				else
				{
					FechaUltimoCalculo=Convert.ToDateTime(DBFunctions.SingleData("SELECT MREPO_FECHAULTIMA from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'")).ToString("yyyy-MM-dd");
				}
				string[] fechaCalculo=FechaUltimoCalculo.Split('-');
				int AñoCalculo=Convert.ToInt32(fechaCalculo[0]);
				int MesCalculo=Convert.ToInt32(fechaCalculo[1]);
				int AñoVigente=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_ANO from DBXSCHEMA.CTRANSPORTES"));
				int MesVigente=Convert.ToInt32(DBFunctions.SingleData("select CTRANS_MES from DBXSCHEMA.CTRANSPORTES"));
			
				string Primeravez=DBFunctions.SingleData("Select * from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'");
				//recuperar variable de session
				double [] ValorCuota2=(double[])Session["Cuota2"];
				/////////////////////////////
				if(Primeravez.Equals(null) || Primeravez.Equals(""))
				{
				
					DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MREPOSICIONVEHICULAR VALUES(DEFAULT,'"+fecha.Text.ToString()+"',"+ValorCuota2[i]+","+ValorCuota2[i]+",'"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"') ");
				}
				else
				{
					if(AñoVigente != AñoCalculo || MesVigente != MesCalculo)
					{
						int codigo=Convert.ToInt32(DBFunctions.SingleData("Select MREPO_CODIGO from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'"));
						
						DBFunctions.NonQuery("UPDATE DBXSCHEMA.MREPOSICIONVEHICULAR SET MREPO_FECHAULTIMA='"+fecha.Text.ToString()+"',MREPO_ULTIVALLIQ="+ValorCuota2[i]+",MREPO_VALORACUM="+ValorCuota2[i]+" WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND MREPO_CODIGO="+codigo+" ");
						DBFunctions.NonQuery("UPDATE DBXSCHEMA.CTRANSPORTES SET CTRANS_ANO="+Convert.ToInt32(DateTime.Now.Year.ToString())+",CTRANS_MES="+Convert.ToInt32(DateTime.Now.Month.ToString())+" where CTRANS_CODIGO=1");
					}
						///////////////////
					else
					{
						if(FechaUltimoCalculo.Equals(""+FechaActual+""))
						{
						
							int codigo=Convert.ToInt32(DBFunctions.SingleData("Select MREPO_CODIGO from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'"));
							double Acum_anterior=Convert.ToDouble(DBFunctions.SingleData("select MREPO_VALORACUM from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND MREPO_CODIGO="+codigo+" "));
							double Cuota_Anterior=Convert.ToDouble(DBFunctions.SingleData("select MREPO_ULTIVALLIQ from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND MREPO_CODIGO="+codigo+" "));
							Acum_anterior=Acum_anterior - Cuota_Anterior;
							Acum_anterior=Acum_anterior + ValorCuota2[i];
							string queryP="UPDATE DBXSCHEMA.MREPOSICIONVEHICULAR SET MREPO_FECHAULTIMA='"+fecha.Text.ToString()+"',MREPO_ULTIVALLIQ="+ValorCuota2[i]+",MREPO_VALORACUM="+Acum_anterior+" WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND MREPO_CODIGO="+codigo+" ";
							DBFunctions.NonQuery("UPDATE DBXSCHEMA.MREPOSICIONVEHICULAR SET MREPO_FECHAULTIMA='"+fecha.Text.ToString()+"',MREPO_ULTIVALLIQ="+ValorCuota2[i]+",MREPO_VALORACUM="+Acum_anterior+" WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND MREPO_CODIGO="+codigo+" ");
						}
						else
						{
							int codigo=Convert.ToInt32(DBFunctions.SingleData("Select MREPO_CODIGO from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'"));
							double AcumuladoFinal=Convert.ToDouble(DBFunctions.SingleData("select MREPO_VALORACUM from DBXSCHEMA.MREPOSICIONVEHICULAR WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND MREPO_CODIGO="+codigo+" "))+ ValorCuota2[i];
							DBFunctions.NonQuery("UPDATE DBXSCHEMA.MREPOSICIONVEHICULAR SET MREPO_FECHAULTIMA='"+fecha.Text.ToString()+"',MREPO_ULTIVALLIQ=0,MREPO_VALORACUM=0 WHERE MCAT_PLACA='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND MREPO_CODIGO="+codigo+" ");
						}
						DBFunctions.NonQuery("UPDATE DBXSCHEMA.CTRANSPORTES SET CTRANS_ANO="+Convert.ToInt32(DateTime.Now.Year.ToString())+",CTRANS_MES="+Convert.ToInt32(DateTime.Now.Month.ToString())+" where CTRANS_CODIGO=1");
					}
				}
				//////////////
				Response.Write("<script language='javascript'>alert('Liquidacion Total Realizada Con Exito !!');</script>");
				Response.Redirect(indexPage+"?process=Comercial.Repocision");
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}

