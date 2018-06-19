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
	using AMS.Reportes;
    using AMS.Tools;
	//Realizado por:
	//Ing.German Lopera
	//2007-03-16

	/// <summary>
	///		Descripción breve de AMS_Comercial_PlanillaGeneral.
	/// </summary>
	public class AMS_Comercial_PlanillaGeneral : System.Web.UI.UserControl
	{
		
		protected DataSet lineas;
		protected DataSet lineas2;
		protected DataSet lineas3;
		protected DataSet lineas4;

		protected DataTable resultado;
		protected DataTable resultado2;
		protected DataTable resultado3;
		protected DataTable resultado4;

		protected PlaceHolder toolsHolder;
		protected TextBox tbEmail;
		protected System.Web.UI.WebControls.DropDownList año;
		protected System.Web.UI.WebControls.DropDownList mes;
		protected System.Web.UI.WebControls.Button Generar;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.ImageButton ibMail;
		protected System.Web.UI.WebControls.DataGrid Grid;
		protected System.Web.UI.WebControls.DataGrid Grid1;
		protected System.Web.UI.WebControls.DataGrid Grid2;
		protected System.Web.UI.WebControls.DataGrid Grid3;
		protected System.Web.UI.WebControls.Label lb;
		protected System.Web.UI.WebControls.TextBox Dia;
		protected System.Web.UI.WebControls.Table tabFirmas;
		protected string reportTitle="Control de Venta de Pasajes,Remesas y Entrega de Anticipos";
		protected System.Web.UI.WebControls.Label AñoLabel;
		protected System.Web.UI.WebControls.Label MesLabel;
		protected System.Web.UI.WebControls.Label DiaLabel;
		protected System.Web.UI.WebControls.Label PlanillaLabel;
		protected System.Web.UI.WebControls.Table tabPreHeader;
		int Mes=0;
		int Año=0;
		int dia=0;
		
		
		
		

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				
				bind.PutDatasIntoDropDownList(año, "SELECT pano_ano FROM DBXSCHEMA.pano");
				bind.PutDatasIntoDropDownList(mes, "SELECT  pmes_nombre FROM DBXSCHEMA.pmes");		
				

			}	
		}
		public  void  generar(Object  Sender, EventArgs e)
		{
			string []pr=new string[2];
			pr[0]=pr[1]="";
			Press frontEnd = new Press(new DataSet(), reportTitle);
			frontEnd.PreHeader(tabPreHeader, Grid.Width, pr);
			frontEnd.Firmas(tabFirmas,Grid.Width);
			lb.Text="";
			Año=Convert.ToInt32(año.SelectedValue.ToString());
			Mes=Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes FROM DBXSCHEMA.pmes WHERE pmes_nombre='"+mes.SelectedValue.ToString()+"' "));
			dia=Convert.ToInt32(Dia.Text.ToString());

			///////////////////////////////////////////////////////
			this.PrepararTabla();
			this.PrepararTabla1();
			this.PrepararTabla2();
			this.PrepararTabla4();
			
			lineas = new DataSet();
			lineas2 = new DataSet();
			lineas3 = new DataSet();
			lineas4 = new DataSet();

			DBFunctions.Request(lineas,IncludeSchema.NO,"Select MREM.MCAT_PLACA AS PLACA,MREM.NUM_REM AS NUMERO,MREM.VALO_FLET AS REMESA from DBXSCHEMA.MREMESA MREM where year(MREM_FECHA)="+Año+" AND MONTH(MREM_FECHA)="+Mes+" AND DAY(MREM_FECHA)="+dia+" ");//remesa
			DBFunctions.Request(lineas2,IncludeSchema.NO,"SELECT MAN.MCAT_PLACA AS PLACA,MAN.MAN_CODIGO AS NUMERO,MAN.MAN_VALOTOTAL AS VALOR from DBXSCHEMA.MANTICIPO MAN WHERE year(MAN_FECHA)="+Año+" AND MONTH(MAN_FECHA)="+Mes+" AND DAY(MAN_FECHA)="+dia+" ");//anticipo
			DBFunctions.Request(lineas3,IncludeSchema.NO,"select MAU.MCAT_PLACA AS PLACA,MAU.MAU_CODIGO AS NUMERO,MAU.MAU_VALOTOT AS VALOR from DBXSCHEMA.MAUTORIZACION MAU WHERE year(MAU_FECHA)="+Año+" AND MONTH(MAU_FECHA)="+Mes+" AND DAY(MAU_FECHA)="+dia+" ");//autorizacion de servicio
			DBFunctions.Request(lineas4,IncludeSchema.NO,"select DRUT.MCAT_PLACA,DRUT.DRUT_PLANILLA,DRUT.DRUT_CODIGO from DBXSCHEMA.DRUTA DRUT  WHERE year(DRUT.DRUT_FECHA)="+Año+" AND MONTH(DRUT.DRUT_FECHA)="+Mes+" AND DAY(DRUT.DRUT_FECHA)="+dia+" ");// tiquetes
			
			/////////////////////////TABLAS//////////////////////////
			
			/////////////////// TABLA 1 /////////////////////////////
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				fila= resultado.NewRow();
				double valor=0;
				string ValorFormato=null;
				valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[2].ToString());
				ValorFormato=String.Format("{0:C}",valor);
				fila["BUS"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["NUMERO"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				fila["VALOR"]	= ValorFormato.ToString();
				resultado.Rows.Add(fila);
						

			}
			///////////////////////TABLA 2///////////////////////////
			for(int a=0;a<lineas2.Tables[0].Rows.Count;a++)
			{
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila2;
				fila2= resultado2.NewRow();
				double valor2=0;
				string ValorFormato2=null;
				valor2=Convert.ToDouble(lineas2.Tables[0].Rows[a].ItemArray[2].ToString());
				ValorFormato2=String.Format("{0:C}",valor2);
				fila2["BUS1"]	= lineas2.Tables[0].Rows[a].ItemArray[0].ToString();
				fila2["NUMERO1"]	= lineas2.Tables[0].Rows[a].ItemArray[1].ToString();
				fila2["VALOR1"]	= ValorFormato2.ToString();
				resultado2.Rows.Add(fila2);
						

			}
			/////////////////// TABLA 3 /////////////////////
			for(int b=0;b<lineas3.Tables[0].Rows.Count;b++)
			{
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila3;
				fila3= resultado3.NewRow();
				double valor3=0;
				string ValorFormato3=null;
				valor3=Convert.ToDouble(lineas3.Tables[0].Rows[b].ItemArray[2].ToString());
				ValorFormato3=String.Format("{0:C}",valor3);
				fila3["BUS2"]	= lineas3.Tables[0].Rows[b].ItemArray[0].ToString();
				fila3["NUMERO2"]	= lineas3.Tables[0].Rows[b].ItemArray[1].ToString();
				fila3["VALOR2"]	= ValorFormato3.ToString();
				resultado3.Rows.Add(fila3);
						

			}
			/////////////////// TABLA 4 /////////////////////
			for(int c=0;c<lineas4.Tables[0].Rows.Count;c++)
			{
				
				int codigoruta=Convert.ToInt32(lineas4.Tables[0].Rows[c].ItemArray[2].ToString());
				int Puestos=Convert.ToInt32(DBFunctions.SingleData("SELECT count(*) AS NUMERO from DBXSCHEMA.DTIQUETE where DRUT_CODIGO="+codigoruta+" and TTIQ_CODIGO=1"));
				int codigoMruta=Convert.ToInt32(DBFunctions.SingleData("select MRUT_CODIGO FROM DBXSCHEMA.DRUTA WHERE DRUT_CODIGO="+codigoruta+" "));
				double precio=Convert.ToDouble(DBFunctions.SingleData("select MRUT_VALOR from DBXSCHEMA.MRUTA WHERE MRUT_CODIGO="+codigoMruta+""));
				double total=precio*Puestos;
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				
				
				
				DataRow fila4;
				fila4= resultado4.NewRow();
				
				string ValorFormato4=null;
				ValorFormato4=String.Format("{0:C}",total);
				fila4["BUS4"]	= lineas4.Tables[0].Rows[c].ItemArray[0].ToString();
				fila4["TOTAL"]	= Puestos;
				fila4["VALOR4"]	= ValorFormato4.ToString();
				resultado4.Rows.Add(fila4);
						

			}
			
			///////////// FIN TABLAS ////////////////////
			
			
			Grid.DataSource = resultado;
			Grid.DataBind();	
			StringBuilder SB=new StringBuilder();
			StringWriter SW=new StringWriter(SB);
			HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			tabPreHeader.RenderControl(htmlTW);
			Grid.RenderControl(htmlTW);
			tabFirmas.RenderControl(htmlTW);
			string strRep;
			strRep=SB.ToString();
			Session.Clear();
			Session["Rep"]=strRep;
			toolsHolder.Visible = true;
			//////////////////////////////
			Grid1.DataSource = resultado2;
			Grid1.DataBind();	
			Grid1.RenderControl(htmlTW);
			
			//////////////////////////////
			Grid2.DataSource = resultado3;
			Grid2.DataBind();	
			Grid2.RenderControl(htmlTW);
			//////////////////////////////
			Grid3.DataSource = resultado4;
			Grid3.DataBind();
			Grid3.RenderControl(htmlTW);
			////////////////////////////////

			
		}
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			generar(Sender, E);
            Utils.MostrarAlerta(Response, "" + Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, Grid) + "");
			Grid.EnableViewState=false;
		}
	
		
		
		
		/////////////////// PREPARAR TABLAS /////////////////////
		
		public void PrepararTabla()
		{
			resultado = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn bus = new DataColumn();
			bus.DataType = System.Type.GetType("System.String");
			bus.ColumnName = "BUS";
			bus.ReadOnly=true;
			resultado.Columns.Add(bus);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn numero = new DataColumn();
			numero.DataType = System.Type.GetType("System.String");
			numero.ColumnName = "NUMERO";
			numero.ReadOnly=true;
			resultado.Columns.Add(numero);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valor = new DataColumn();
			valor.DataType = System.Type.GetType("System.String");
			valor.ColumnName = "VALOR";
			valor.ReadOnly=true;
			resultado.Columns.Add(valor);
								
		}
		//////////////////////////////////////////////TABLA 2////////////////////
		public void PrepararTabla1()
		{
			resultado2 = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn bus1 = new DataColumn();
			bus1.DataType = System.Type.GetType("System.String");
			bus1.ColumnName = "BUS1";
			bus1.ReadOnly=true;
			resultado2.Columns.Add(bus1);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn numero1 = new DataColumn();
			numero1.DataType = System.Type.GetType("System.String");
			numero1.ColumnName = "NUMERO1";
			numero1.ReadOnly=true;
			resultado2.Columns.Add(numero1);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valor1 = new DataColumn();
			valor1.DataType = System.Type.GetType("System.String");
			valor1.ColumnName = "VALOR1";
			valor1.ReadOnly=true;
			resultado2.Columns.Add(valor1);
		}
		////////////////////////////// TABLA 3 ///////////////////////
		public void PrepararTabla2()
		{
			
			resultado3 = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn bus2 = new DataColumn();
			bus2.DataType = System.Type.GetType("System.String");
			bus2.ColumnName = "BUS2";
			bus2.ReadOnly=true;
			resultado3.Columns.Add(bus2);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn numero2 = new DataColumn();
			numero2.DataType = System.Type.GetType("System.String");
			numero2.ColumnName = "NUMERO2";
			numero2.ReadOnly=true;
			resultado3.Columns.Add(numero2);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valor2 = new DataColumn();
			valor2.DataType = System.Type.GetType("System.String");
			valor2.ColumnName = "VALOR2";
			valor2.ReadOnly=true;
			resultado3.Columns.Add(valor2);
			//////////////////////////////////////////////////////
		}
		////////////////////////////// TABLA 4 ///////////////////////
		public void PrepararTabla4()
		{
			
			resultado4 = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn bus4 = new DataColumn();
			bus4.DataType = System.Type.GetType("System.String");
			bus4.ColumnName = "BUS4";
			bus4.ReadOnly=true;
			resultado4.Columns.Add(bus4);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn total = new DataColumn();
			total.DataType = System.Type.GetType("System.String");
			total.ColumnName = "TOTAL";
			total.ReadOnly=true;
			resultado4.Columns.Add(total);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valor4 = new DataColumn();
			valor4.DataType = System.Type.GetType("System.String");
			valor4.ColumnName = "VALOR4";
			valor4.ReadOnly=true;
			resultado4.Columns.Add(valor4);
			//////////////////////////////////////////////////////
		}



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