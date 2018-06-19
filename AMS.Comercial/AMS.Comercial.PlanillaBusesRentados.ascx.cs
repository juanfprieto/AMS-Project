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
	///		Descripción breve de AMS_Comercial_PlanillaBusesRentados.
	/// </summary>
	public class AMS_Comercial_PlanillaBusesRentados : System.Web.UI.UserControl
	{
		protected Label lb;
		protected DataSet lineas;
		protected DataTable resultado;
		protected DataGrid Grid;
		protected Table tabPreHeader,tabFirmas;
		protected PlaceHolder toolsHolder;
		protected TextBox tbEmail;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.ImageButton ibMail;
		protected System.Web.UI.HtmlControls.HtmlGenericControl P1;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList año;
		protected System.Web.UI.WebControls.DropDownList mes;	
		protected string reportTitle="Planilla Servicios Especiales";
		int Mes=0;
		int Año=0;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(año, "SELECT pano_ano FROM DBXSCHEMA.pano");
				bind.PutDatasIntoDropDownList(mes, "SELECT  pmes_nombre FROM DBXSCHEMA.pmes");		
				

			}	
		}
	
	
		protected  void  generar(Object  Sender, EventArgs e)
		{
			string []pr=new string[2];
			pr[0]=pr[1]="";
			Press frontEnd = new Press(new DataSet(), reportTitle);
			frontEnd.PreHeader(tabPreHeader, Grid.Width, pr);
			frontEnd.Firmas(tabFirmas,Grid.Width);
			lb.Text="";
			Año=Convert.ToInt32(año.SelectedValue.ToString());
			Mes=Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes FROM DBXSCHEMA.pmes WHERE pmes_nombre='"+mes.SelectedValue.ToString()+"' "));
			

			///////////////////////////////////////////////////////
			this.PrepararTabla();
			lineas = new DataSet();
			DBFunctions.Request(lineas,IncludeSchema.NO,"select MBR.MBUSR_FECHA,MBR.MCAT_PLACA,MBR.ORIGEN,MBR.DESTINO,MBR.NOMBRE_RESERVA,MBR.VALOR_RESERVA FROM DBXSCHEMA.MBUS_RENTADO MBR,DBXSCHEMA.MBUSAFILIADO MBUS WHERE MBR.MCAT_PLACA=MBUS.MCAT_PLACA AND MBUS.TESTA_CODIGO=6 AND year(MBR.MBUSR_FECHA)="+Año+" and month(MBR.MBUSR_FECHA)="+Mes+" ");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				
				
				
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				
				
				//SaldoFinal1=String.Format("{0:C}",saldoLinea);
				fila= resultado.NewRow();
				double valor=0;
				string ValorFormato=null;
				valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[5].ToString());
				ValorFormato=String.Format("{0:C}",valor);
				fila["FECHA"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["PLACA"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				fila["ORIGEN"]	= lineas.Tables[0].Rows[i].ItemArray[2].ToString();
				fila["DESTINO"]	= lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				fila["CONTRATANTE"]=lineas.Tables[0].Rows[i].ItemArray[4].ToString();
				fila["VALCONTRATO"]=ValorFormato.ToString();
				
			
				resultado.Rows.Add(fila);
						

			}
			
			//fin sentencia FOR	
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
			
		}
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			generar(Sender, E);
            Utils.MostrarAlerta(Response, "" + Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, Grid) + "");
			Grid.EnableViewState=false;
		}
	
		
		

		public void PrepararTabla()
		{
			resultado = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn fecha = new DataColumn();
			fecha.DataType = System.Type.GetType("System.String");
			fecha.ColumnName = "FECHA";
			fecha.ReadOnly=true;
			resultado.Columns.Add(fecha);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn placa = new DataColumn();
			placa.DataType = System.Type.GetType("System.String");
			placa.ColumnName = "PLACA";
			placa.ReadOnly=true;
			resultado.Columns.Add(placa);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn origen = new DataColumn();
			origen.DataType = System.Type.GetType("System.String");
			origen.ColumnName = "ORIGEN";
			origen.ReadOnly=true;
			resultado.Columns.Add(origen);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn destino = new DataColumn();
			destino.DataType = System.Type.GetType("System.String");
			destino.ColumnName = "DESTINO";
			destino.ReadOnly=true;
			resultado.Columns.Add(destino);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn contratante = new DataColumn();
			contratante.DataType = System.Type.GetType("System.String");
			contratante.ColumnName = "CONTRATANTE";
			contratante.ReadOnly=true;
			resultado.Columns.Add(contratante);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valcontrato = new DataColumn();
			valcontrato.DataType = System.Type.GetType("System.String");
			valcontrato.ColumnName = "VALCONTRATO";
			valcontrato.ReadOnly=true;
			resultado.Columns.Add(valcontrato);
			
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