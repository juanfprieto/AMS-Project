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
	using WebChart;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_MedidorEncuesta.
	/// </summary>
	public class AMS_Comercial_MedidorEncuesta : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList servicio;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label promcal;
		protected System.Web.UI.WebControls.Label mincal;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label maxcal;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label serv;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label toten;
		protected System.Web.UI.WebControls.Label totser;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Button detalles;
		protected System.Web.UI.WebControls.Panel Panel2;
		protected System.Web.UI.WebControls.DataGrid Grid;
		protected System.Web.UI.WebControls.Button Generar;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label promedio;
		protected System.Web.UI.WebControls.Label RemeasLabel;
		protected DataSet lineas;
		protected DataTable resultado;
		protected WebChart.ChartControl chart1;
		int codser=0;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(servicio, "select MSEREN_DESC FROM DBXSCHEMA.MSERENCUESTA ORDER BY MSEREN_DESC");
			}
		}
		protected  void  generar(Object  Sender, EventArgs e)
		{
			
			codser=Convert.ToInt32(DBFunctions.SingleData("select MSEREN_CODIGO FROM DBXSCHEMA.MSERENCUESTA WHERE MSEREN_DESC='"+servicio.SelectedValue.ToString()+"'"));	
			int numEncuestas=Convert.ToInt32(DBFunctions.SingleData("select count(*) from DBXSCHEMA.MENCUESTA_TRANSPORTES where MENCU_SERVICIO="+codser+" "));
			
			if(numEncuestas.Equals(0))
			{
                Utils.MostrarAlerta(Response, "  No Hay Registros De Este Servicio");
			}
				
			else
			{
			
				Panel1.Visible=true;
				
				double maxcalD=Convert.ToDouble(DBFunctions.SingleData("select max(MENCU_CALSERVICIO) from DBXSCHEMA.MENCUESTA_TRANSPORTES where MENCU_SERVICIO="+codser+" "));
				double mincalD=Convert.ToDouble(DBFunctions.SingleData("select min(MENCU_CALSERVICIO) from DBXSCHEMA.MENCUESTA_TRANSPORTES where MENCU_SERVICIO="+codser+" "));
				int cantidadEncSer=Convert.ToInt32(DBFunctions.SingleData("select count(*) from DBXSCHEMA.MENCUESTA_TRANSPORTES where MENCU_SERVICIO="+codser+" "));
				int cantidadEnc=Convert.ToInt32(DBFunctions.SingleData("select count(*) from DBXSCHEMA.MENCUESTA_TRANSPORTES"));
				double calprom=Convert.ToDouble(DBFunctions.SingleData("select AVG(MENCU_CALSERVICIO)from DBXSCHEMA.MENCUESTA_TRANSPORTES where MENCU_SERVICIO="+codser+" "));
				double porcentaje=calprom*10;
				///////////// LABELS ///////////
				serv.Text=servicio.SelectedValue.ToString();
				maxcal.Text=maxcalD.ToString();
				mincal.Text=mincalD.ToString();
				promcal.Text=calprom.ToString();
				promedio.Text=porcentaje.ToString();
				toten.Text=cantidadEnc.ToString();
				totser.Text=cantidadEncSer.ToString();
				///////////////////////////////
				chart1.Visible=true;
				ChartPointCollection data = new ChartPointCollection();
				data.Add( new ChartPoint("Satisfecho", Convert.ToInt32(porcentaje)));
				data.Add( new ChartPoint("No Satisfecho", Convert.ToInt32((100-porcentaje))));
				
				PieChart c = new PieChart(data, Color.Blue );
				c.Colors = new Color[]{ Color.Green,Color.Red };
				
				chart1.Charts.Add( c );
				chart1.RedrawChart();
				///////////////////////////////
			}

		}
		protected  void  detalles_Click(Object  Sender, EventArgs e)
		{
			Panel2.Visible=true;
			this.PrepararTabla();
			lineas = new DataSet();
			codser=Convert.ToInt32(DBFunctions.SingleData("select MSEREN_CODIGO FROM DBXSCHEMA.MSERENCUESTA WHERE MSEREN_DESC='"+servicio.SelectedValue.ToString()+"'"));
			DBFunctions.Request(lineas,IncludeSchema.NO," select MENCU_OPSERVICIO,MENCU_OPGENERALES from DBXSCHEMA.MENCUESTA_TRANSPORTES WHERE MENCU_SERVICIO="+codser+" ");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				DataRow fila;
				fila= resultado.NewRow();
				fila["OBSERVICIO"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["OBGENERALES"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				resultado.Rows.Add(fila);
						

			}
			
			//fin sentencia FOR	
			Grid.DataSource = resultado;
			Grid.DataBind();	
			StringBuilder SB=new StringBuilder();
			StringWriter SW=new StringWriter(SB);
			HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			
			
		}
			

		public void PrepararTabla()
		{
			resultado = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn observivios = new DataColumn();
			observivios.DataType = System.Type.GetType("System.String");
			observivios.ColumnName = "OBSERVICIO";
			observivios.ReadOnly=true;
			resultado.Columns.Add(observivios);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn obgenerales = new DataColumn();
			obgenerales.DataType = System.Type.GetType("System.String");
			obgenerales.ColumnName = "OBGENERALES";
			obgenerales.ReadOnly=true;
			resultado.Columns.Add(obgenerales);
			
			
			



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
