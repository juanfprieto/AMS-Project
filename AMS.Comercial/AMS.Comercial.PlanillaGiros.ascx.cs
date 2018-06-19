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
	//Realizado por:
	//Ing.German Lopera
	//2007-03-16
	/// <summary>
	///		Descripción breve de AMS_Comercial_PlanillaGiros.
	/// </summary>
	public class AMS_Comercial_PlanillaGiros : System.Web.UI.UserControl
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
		protected System.Web.UI.WebControls.Label PlacaLabel;
		protected System.Web.UI.WebControls.DropDownList Placa;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList año;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList mes;
		protected System.Web.UI.WebControls.DropDownList Agencia;
		
		string codigoagencia=null;
		protected string reportTitle="Planilla De Giros";
		int Mes=0;
		int Año=0;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				if(Agencia.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(Agencia,"select MOFI_DESCRIPCION from DBXSCHEMA.MOFICINA ORDER BY MOFI_DESCRIPCION");
					ListItem it=new ListItem("--AGENCIA --","0");
					Agencia.Items.Insert(0,it);
				}
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
			codigoagencia=DBFunctions.SingleData("select MOFI_CODIGO from DBXSCHEMA.MOFICINA WHERE MOFI_DESCRIPCION ='"+Agencia.SelectedValue.ToString()+"' ");
			Año=Convert.ToInt32(año.SelectedValue.ToString());
			Mes=Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes FROM DBXSCHEMA.pmes WHERE pmes_nombre='"+mes.SelectedValue.ToString()+"' "));
			
			
			///////////////////////////////////////////////////////
			this.PrepararTabla();
			lineas = new DataSet();
			DBFunctions.Request(lineas,IncludeSchema.NO,"select  MGIRO_CODGIRO,FECHA_GIRO,AGENCIA_ORIGEN,NOMBRE_EMISOR,CEDULA_EMISOR,NOMBRE_DESTINATARIO,CEDULA_DESTINATARIO,MGIRO_VALOR,MGIRO_COSTO,DGIRO_CODIGO from DBXSCHEMA.MGIRO WHERE AGENCIA_DESTINO='"+codigoagencia+"'  AND year(FECHA_GIRO)="+Año+" and month(FECHA_GIRO)="+Mes+"  ");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				
				
				
				string estado=DBFunctions.SingleData("Select DGIRO_DESCRIPCION from DBXSCHEMA.DGIRO WHERE  DGIRO_CODIGO="+lineas.Tables[0].Rows[i].ItemArray[9].ToString()+" ");
				string agencia=DBFunctions.SingleData("select MOFI_DESCRIPCION from DBXSCHEMA.MOFICINA WHERE MOFI_CODIGO ='"+lineas.Tables[0].Rows[i].ItemArray[2].ToString()+"' ");
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				
				fila= resultado.NewRow();
				double valor=0;
				double costo=0;
				string Valor=null;
				string Costo=null;
				valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[7].ToString());
				costo=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[8].ToString());
				Valor=String.Format("{0:C}",valor);
				Costo=String.Format("{0:C}",costo);
				fila["NUMEROGIRO"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["FECHA"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				fila["ORIGEN"]	= agencia.ToString();
				fila["NOMBREEMISOR"]	= lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				fila["CEDULAEMISOR"]	= lineas.Tables[0].Rows[i].ItemArray[4].ToString();
				fila["NOMBRERECEPTOR"]	= lineas.Tables[0].Rows[i].ItemArray[5].ToString();
				fila["CEDULARECEPTOR"]	= lineas.Tables[0].Rows[i].ItemArray[6].ToString();
				fila["VALORGIRO"]	= Valor.ToString();
				fila["COSTOGIRO"] = Costo.ToString();
				fila["ESTADOGIRO"] = estado.ToString();
				
			
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
			Response.Write("<script language='javascript'>alert('"+Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, Grid)+"');</script>");
			Grid.EnableViewState=false;
		}
	
		
		

		public void PrepararTabla()
		{
			resultado = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn numerogiro = new DataColumn();
			numerogiro.DataType = System.Type.GetType("System.String");
			numerogiro.ColumnName = "NUMEROGIRO";
			numerogiro.ReadOnly=true;
			resultado.Columns.Add(numerogiro);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn fecha = new DataColumn();
			fecha.DataType = System.Type.GetType("System.String");
			fecha.ColumnName = "FECHA";
			fecha.ReadOnly=true;
			resultado.Columns.Add(fecha);
			
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn origen = new DataColumn();
			origen.DataType = System.Type.GetType("System.String");
			origen.ColumnName = "ORIGEN";
			origen.ReadOnly=true;
			resultado.Columns.Add(origen);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn nombreemisor = new DataColumn();
			nombreemisor.DataType = System.Type.GetType("System.String");
			nombreemisor.ColumnName = "NOMBREEMISOR";
			nombreemisor.ReadOnly=true;
			resultado.Columns.Add(nombreemisor);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn cedulaemisor= new DataColumn();
			cedulaemisor.DataType = System.Type.GetType("System.String");
			cedulaemisor.ColumnName = "CEDULAEMISOR";
			cedulaemisor.ReadOnly=true;
			resultado.Columns.Add(cedulaemisor);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn nombrerecpetor = new DataColumn();
			nombrerecpetor.DataType = System.Type.GetType("System.String");
			nombrerecpetor.ColumnName = "NOMBRERECEPTOR";
			nombrerecpetor.ReadOnly=true;
			resultado.Columns.Add(nombrerecpetor);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn cedulareceptor = new DataColumn();
			cedulareceptor.DataType = System.Type.GetType("System.String");
			cedulareceptor.ColumnName = "CEDULARECEPTOR";
			cedulareceptor.ReadOnly=true;
			resultado.Columns.Add(cedulareceptor);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn valorgiro = new DataColumn();
			valorgiro.DataType = System.Type.GetType("System.String");
			valorgiro.ColumnName = "VALORGIRO";
			valorgiro.ReadOnly=true;
			resultado.Columns.Add(valorgiro);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn costogiro = new DataColumn();
			costogiro.DataType = System.Type.GetType("System.String");
			costogiro.ColumnName = "COSTOGIRO";
			costogiro.ReadOnly=true;
			resultado.Columns.Add(costogiro);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn estadogiro = new DataColumn();
			estadogiro.DataType = System.Type.GetType("System.String");
			estadogiro.ColumnName = "ESTADOGIRO";
			estadogiro.ReadOnly=true;
			resultado.Columns.Add(estadogiro);

		
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