namespace AMS.Comercial
{
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
	using AMS.DB;

	/// <summary>
	///		Descripción breve de AMS_ComercialConsolidarPlanillasBuses.
	/// </summary>
	public class AMS_ComercialConsolidarPlanillasBuses : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label label;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label label20;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.DropDownList añoi;
		protected System.Web.UI.WebControls.DropDownList añof;
		protected System.Web.UI.WebControls.DropDownList mesi;
		protected System.Web.UI.WebControls.TextBox diai;
		protected System.Web.UI.WebControls.DropDownList mesf;
		protected System.Web.UI.WebControls.TextBox diaf;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.DropDownList bus;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label conductor;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label numbus;
		protected System.Web.UI.WebControls.Button generar;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.Label Label26;
		protected System.Web.UI.WebControls.Label numta;
		protected System.Web.UI.WebControls.Label ttiqa;
		protected System.Web.UI.WebControls.Label numremm;
		protected System.Web.UI.WebControls.Label totremm;
		protected System.Web.UI.WebControls.Label Label27;
		protected System.Web.UI.WebControls.Label Label29;
		protected System.Web.UI.WebControls.Label Label31;
		protected System.Web.UI.WebControls.TextBox totaltiquetes;
		protected System.Web.UI.WebControls.TextBox totalremesas;
		protected System.Web.UI.WebControls.TextBox totalventas;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label30;
		protected System.Web.UI.WebControls.Label Label32;
		protected System.Web.UI.WebControls.Label numanti;
		protected System.Web.UI.WebControls.Label totanti;
		protected System.Web.UI.WebControls.Label Label28;
		protected System.Web.UI.WebControls.Label Label33;
		protected System.Web.UI.WebControls.Label Label34;
		protected System.Web.UI.WebControls.TextBox TOTALNETO;
		protected System.Web.UI.WebControls.Button guardar;
		protected System.Web.UI.WebControls.TextBox totalanticipos;
		protected System.Web.UI.WebControls.Label Label35;
		protected System.Web.UI.WebControls.Label Label36;
		protected System.Web.UI.WebControls.Label Label37;
		protected System.Web.UI.WebControls.Label numae;
		protected System.Web.UI.WebControls.Label totane;
		protected System.Web.UI.WebControls.Label Label1;
		public double TAValor=0;
		public double TPValor=0;
		public double TMValor=0;
		public double RAvalor=0;
		public double RMvalor=0;
		public double TOTALTiquetes=0;
		public double TOTALRemesas=0;
		public double Totalventas=0;
		public double ANvalor=0;
		public double AEtotal=0;
		public double TotalAnticipos=0;
		public double TotalNeto=0;
		string fechaI=null;
		protected System.Web.UI.WebControls.Label Label38;
		protected System.Web.UI.WebControls.Label Label39;
		protected System.Web.UI.WebControls.Label Label40;
		protected System.Web.UI.WebControls.Label Label41;
		string fechaF=null;
		protected System.Web.UI.WebControls.Label numrema;
		protected System.Web.UI.WebControls.Label totrema;
		protected System.Web.UI.WebControls.Label numtiqm;
		protected System.Web.UI.WebControls.Label ttiqm;
		protected System.Web.UI.WebControls.Label numtipre;
		protected System.Web.UI.WebControls.Label tottiqpre;
		protected System.Web.UI.WebControls.Label FechaLabel;
		double PREPAGO=0;

		public void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				FechaLabel.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				int contadorBus=0;
				if(contadorBus==0)
				{
					bind.PutDatasIntoDropDownList(bus,"Select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
					ListItem it=new ListItem("--PLACA--","0");
					bus.Items.Insert(0,it);
				}
				
				int contadorAÑOi=0;
				if(contadorAÑOi==0)
				{
					bind.PutDatasIntoDropDownList(añoi, "SELECT pano_ano FROM DBXSCHEMA.pano");
					ListItem it=new ListItem("-Año-");
					añoi.Items.Insert(0,it);
				}
				int contadorMESi=0;
				if(contadorMESi==0)
				{
					bind.PutDatasIntoDropDownList(mesi, "SELECT  pmes_mes,pmes_nombre FROM DBXSCHEMA.pmes");		
					ListItem it=new ListItem("--Mes--");
					mesi.Items.Insert(0,it);

				}
				int contadorAÑOf=0;
				if(contadorAÑOf==0)
				{
					bind.PutDatasIntoDropDownList(añof, "SELECT pano_ano FROM DBXSCHEMA.pano");		
					ListItem it=new ListItem("-Año-");
					añof.Items.Insert(0,it);
				}
				int contadorMESf=0;
				if(contadorMESf==0)
				{
					bind.PutDatasIntoDropDownList(mesf, "SELECT  pmes_mes,pmes_nombre FROM DBXSCHEMA.pmes");		
					ListItem it=new ListItem("--Mes--");
					mesf.Items.Insert(0,it);
				}
				
				
				
				diai.Text=DateTime.Now.Day.ToString();
				diaf.Text=DateTime.Now.Day.ToString();
			}
			else
			{
				if (Session["PREPAGO"]!=null)
					PREPAGO=Convert.ToDouble(Session["PREPAGO"]);
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
			this.bus.SelectedIndexChanged += new System.EventHandler(this.bus_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public void bus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		DatasToControls bind = new DatasToControls();
		string nit=DBFunctions.SingleData("select 	MNIT_NITCHOFER FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+bus.SelectedValue.ToString()+"' ");
		string nombres=DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MEMPLEADO MEMP WHERE MNIT.MNIT_NIT='"+nit.ToString()+"' ");
		conductor.Text=nombres.ToString();
		string NumBus=DBFunctions.SingleData("select MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+bus.SelectedValue.ToString()+"'");
		numbus.Text=NumBus.ToString();
		}
		public  void  generar_OnClick(Object  Sender, EventArgs e)
		{
			Panel1.Visible=true;
			DatasToControls bind = new DatasToControls();
			fechaI=añoi.SelectedValue.ToString()+"-"+mesi.SelectedValue.ToString()+"-"+diai.Text.ToString();
			fechaI=Convert.ToDateTime(fechaI).ToString("yyyy-MM-dd");
			fechaF=añof.SelectedValue.ToString()+"-"+mesf.SelectedValue.ToString()+"-"+diaf.Text.ToString();
			fechaF=Convert.ToDateTime(fechaF).ToString("yyyy-MM-dd");
			//tiquetes automaticos
			string TACantidad=DBFunctions.SingleData("select count(*) from DBXSCHEMA.DTIQUETE where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND  DTIQ_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			string TAValorS=DBFunctions.SingleData("select sum(VAL_TIQUETE) from DBXSCHEMA.DTIQUETE WHERE MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND DTIQ_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			TAValor=0;
			if(TAValorS.Equals("") || TAValorS.Equals(null))
			{
				TAValor=0;
			}
			else
			{
				TAValor=Convert.ToDouble(DBFunctions.SingleData("select sum(VAL_TIQUETE) from DBXSCHEMA.DTIQUETE WHERE MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND DTIQ_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			string TAvalorFormato=String.Format("{0:C}",TAValor);
			numta.Text=TACantidad.ToString();
			ttiqa.Text=TAvalorFormato.ToString();
			//////////////////////TIQUETES PREPAGO
			string TPcantidad=DBFunctions.SingleData("select count(*) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+bus.SelectedValue.ToString()+"' AND  FECHA_PLANILLA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"' AND TTIPO_TIQUETE=2 ");
			string TPvalorS=DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+bus.SelectedValue.ToString()+"' AND  FECHA_PLANILLA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"' AND TTIPO_TIQUETE=2 ");
			TMValor=0;
			if(TPvalorS.Equals("") || TPvalorS.Equals(""))
			{
				TPValor=0;
			}
			else
			{
				TPValor=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+bus.SelectedValue.ToString()+"' AND  FECHA_PLANILLA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"' AND TTIPO_TIQUETE=2"));
			}
			string TPvalorFormato=String.Format("{0:C}",TPValor);
			numtipre.Text=TPcantidad.ToString();
			tottiqpre.Text=TPvalorFormato.ToString();
			
			
			////////////////////////
			//////tiquetes manuales
			string TMcantidad=DBFunctions.SingleData("select count(*) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+bus.SelectedValue.ToString()+"' AND  FECHA_PLANILLA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"' AND TTIPO_TIQUETE=1 ");
			string TMvalorS=DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+bus.SelectedValue.ToString()+"' AND  FECHA_PLANILLA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"' AND TTIPO_TIQUETE=1 ");
			TMValor=0;
			if(TMvalorS.Equals("") || TMvalorS.Equals(""))
			{
				TMValor=0;
			}
			else
			{
				TMValor=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+bus.SelectedValue.ToString()+"' AND  FECHA_PLANILLA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"' AND TTIPO_TIQUETE=1"));
			}
			string TMvalorFormato=String.Format("{0:C}",TMValor);
			numtiqm.Text=TMcantidad.ToString();
			ttiqm.Text=TMvalorFormato.ToString();
			//////////////Remesas Automaticas
			string RAcantidad=DBFunctions.SingleData("select count(*) from DBXSCHEMA.MREMESA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MREM_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			string RAValorS=DBFunctions.SingleData("select sum(VALO_FLET) from DBXSCHEMA.MREMESA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MREM_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			RAvalor=0;
			if(RAValorS.Equals(null) || RAValorS.Equals(""))
			{
				RAvalor=0;
			}
			else
			{
				RAvalor=Convert.ToDouble(DBFunctions.SingleData("select sum(VALO_FLET) from DBXSCHEMA.MREMESA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MREM_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			string RAvalorFormato=String.Format("{0:C}",RAvalor);
			numrema.Text=RAcantidad.ToString();
			totrema.Text=RAvalorFormato.ToString();
			///////////remesas manuales
			string RMcantidad=DBFunctions.SingleData("select count(*) from DBXSCHEMA.MREMESA_MANUAL where MCAT_PLACA ='"+bus.SelectedValue.ToString()+"' AND FECHA_REMESA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			string RMvalorS=DBFunctions.SingleData("select sum(VALOR_REMESA) from DBXSCHEMA.MREMESA_MANUAL where MCAT_PLACA ='"+bus.SelectedValue.ToString()+"' AND FECHA_REMESA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			RMvalor=0;
			if(RMvalorS.Equals("") || RMvalorS.Equals(null))
			{
				RMvalor=0;
			}
			else
			{
				RMvalor=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_REMESA) from DBXSCHEMA.MREMESA_MANUAL where MCAT_PLACA ='"+bus.SelectedValue.ToString()+"' AND FECHA_REMESA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			string RMvalorFormato=String.Format("{0:C}",RMvalor);
			numremm.Text=RMcantidad.ToString();
			totremm.Text=RMvalorFormato.ToString();
			//////////////totales tiquetes
			TOTALTiquetes=TAValor+TMValor+TPValor;
			string TotalTiqFormato=String.Format("{0:C}",TOTALTiquetes);
			totaltiquetes.Text=TotalTiqFormato.ToString();
			//////////////totales remesas
			TOTALRemesas=RMvalor+RAvalor;
			string TotalRemFormato=String.Format("{0:C}",TOTALRemesas);
			totalremesas.Text=TotalRemFormato.ToString();
			
			//////////////total ventas
			Totalventas=TOTALTiquetes + TOTALRemesas;
			string TotalVentasFormato=String.Format("{0:C}",Totalventas);
			totalventas.Text=TotalVentasFormato.ToString();
			
			//////////////ANTICIPOS
			string ANcantidad=DBFunctions.SingleData("select count(*)  from DBXSCHEMA.MANTICIPO where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			string ANvalorS=DBFunctions.SingleData("select sum(MAN_VALOTOTAL)  from DBXSCHEMA.MANTICIPO where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			ANvalor=0;
			if(ANvalorS.Equals("") || ANvalorS.Equals(null))
			{
				ANvalor=0;
			}
			else
			{
				ANvalor=Convert.ToDouble(DBFunctions.SingleData("select sum(MAN_VALOTOTAL)  from DBXSCHEMA.MANTICIPO where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			string ANvalorFormato=String.Format("{0:C}",ANvalor);
			numanti.Text=ANcantidad.ToString();
			totanti.Text=ANvalorFormato.ToString();
			
			////// anticipos extras
			string AEcantidad=DBFunctions.SingleData("select count(*) from DBXSCHEMA.MANTICIPO_EXTRA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			string AEtotalS=DBFunctions.SingleData("select sum(MAN_VALOTOTAL)from DBXSCHEMA.MANTICIPO_EXTRA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			AEtotal=0;
			if(AEtotalS.Equals("") || AEtotalS.Equals(null))
			{
				AEtotal=0;
			}
			else
			{
			AEtotal=Convert.ToDouble(DBFunctions.SingleData("select sum(MAN_VALOTOTAL)from DBXSCHEMA.MANTICIPO_EXTRA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			
			}
			numae.Text=AEcantidad.ToString();
			string AEtotalFormato=String.Format("{0:C}",AEtotal);
			totane.Text=AEtotalFormato.ToString();
			//////TOTAL ANTICIPOS
			TotalAnticipos=AEtotal+ANvalor;
			string TotalAnticiposFomato=String.Format("{0:C}",TotalAnticipos);
			totalanticipos.Text=TotalAnticiposFomato.ToString();
			//TOTAL NETO (ventas-anticipos)
			TotalNeto=Totalventas-TotalAnticipos;
			string TotalNetoFormato=String.Format("{0:C}",TotalNeto);
			TOTALNETO.Text=TotalNetoFormato.ToString();
			Session["PREPAGO"]=TPValor;


		}
		public  void  guardar_OnClick(Object  Sender, EventArgs e)
		{
			fechaI=añoi.SelectedValue.ToString()+"-"+mesi.SelectedValue.ToString()+"-"+diai.Text.ToString();
			fechaF=añof.SelectedValue.ToString()+"-"+mesf.SelectedValue.ToString()+"-"+diaf.Text.ToString();
			double ValorPREPAGO=Math.Round((double)Session["PREPAGO"],2);
			string TAValorS=DBFunctions.SingleData("select sum(VAL_TIQUETE) from DBXSCHEMA.DTIQUETE WHERE MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND DTIQ_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			TAValor=0;
			if(TAValorS.Equals("") || TAValorS.Equals(null))
			{
				TAValor=0;
			}
			else
			{
				TAValor=Convert.ToDouble(DBFunctions.SingleData("select sum(VAL_TIQUETE) from DBXSCHEMA.DTIQUETE WHERE MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND DTIQ_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			string TMvalorS=DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+bus.SelectedValue.ToString()+"' AND  FECHA_PLANILLA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			TMValor=0;
			if(TMvalorS.Equals("") || TMvalorS.Equals(""))
			{
				TMValor=0;
			}
			else
			{
				TMValor=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_VENTA) from DBXSCHEMA.MPLANILLA_VIAJE where BUS='"+bus.SelectedValue.ToString()+"' AND  FECHA_PLANILLA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			string RAValorS=DBFunctions.SingleData("select sum(VALO_FLET) from DBXSCHEMA.MREMESA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MREM_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			RAvalor=0;
			if(RAValorS.Equals(null) || RAValorS.Equals(""))
			{
				RAvalor=0;
			}
			else
			{
				RAvalor=Convert.ToDouble(DBFunctions.SingleData("select sum(VALO_FLET) from DBXSCHEMA.MREMESA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MREM_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			string RMvalorS=DBFunctions.SingleData("select sum(VALOR_REMESA) from DBXSCHEMA.MREMESA_MANUAL where MCAT_PLACA ='"+bus.SelectedValue.ToString()+"' AND FECHA_REMESA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			RMvalor=0;
			if(RMvalorS.Equals("") || RMvalorS.Equals(null))
			{
				RMvalor=0;
			}
			else
			{
				RMvalor=Convert.ToDouble(DBFunctions.SingleData("select sum(VALOR_REMESA) from DBXSCHEMA.MREMESA_MANUAL where MCAT_PLACA ='"+bus.SelectedValue.ToString()+"' AND FECHA_REMESA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			string ANvalorS=DBFunctions.SingleData("select sum(MAN_VALOTOTAL)  from DBXSCHEMA.MANTICIPO where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			ANvalor=0;
			if(ANvalorS.Equals("") || ANvalorS.Equals(null))
			{
				ANvalor=0;
			}
			else
			{
				ANvalor=Convert.ToDouble(DBFunctions.SingleData("select sum(MAN_VALOTOTAL)  from DBXSCHEMA.MANTICIPO where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			string AEtotalS=DBFunctions.SingleData("select sum(MAN_VALOTOTAL)from DBXSCHEMA.MANTICIPO_EXTRA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			AEtotal=0;
			if(AEtotalS.Equals("") || AEtotalS.Equals(null))
			{
				AEtotal=0;
			}
			else
			{
				AEtotal=Convert.ToDouble(DBFunctions.SingleData("select sum(MAN_VALOTOTAL)from DBXSCHEMA.MANTICIPO_EXTRA where MCAT_PLACA='"+bus.SelectedValue.ToString()+"' AND MAN_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			
			}
			TOTALTiquetes=TAValor+TMValor+ValorPREPAGO;
			TOTALRemesas=RMvalor+RAvalor;
			TotalAnticipos=AEtotal+ANvalor;
			Totalventas=TOTALTiquetes + TOTALRemesas;
			TotalAnticipos=AEtotal+ANvalor;
			TotalNeto=Totalventas-TotalAnticipos;
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MPLANILLA_PRELIMINAR VALUES(DEFAULT,DEFAULT,'"+DateTime.Now.Date.ToString("yyyy-MM-dd")+"','"+bus.SelectedValue.ToString()+"',"+numbus.Text.ToString()+","+TOTALTiquetes+","+TotalAnticipos+","+TOTALRemesas+","+TotalNeto+")");
			Label38.Text=DBFunctions.exceptions;
		}
	}
}
