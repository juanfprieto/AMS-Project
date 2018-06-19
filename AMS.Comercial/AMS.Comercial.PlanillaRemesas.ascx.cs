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
	///		Descripción breve de AMS_Comercial_PlanillaRemesas.
	/// </summary>
	public class AMS_Comercial_PlanillaRemesas : System.Web.UI.UserControl
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
		protected string reportTitle="Planilla De Remesas y Encomiendas";
		int Mes=0;
		int Año=0;
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(Placa,"Select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
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
			DBFunctions.Request(lineas,IncludeSchema.NO,"SELECT TREM_CODIGO,NUM_REM,MREM_FECHA,NOM_EMISOR,NOM_DESTINO,ORIGEN,DESTINO,VALOR_DEC,UNIDADES,PESO,VALO_FLET,MREM_ESTADO,MREM_CONTENIDO FROM DBXSCHEMA.MREMESA where MCAT_PLACA='"+Placa.SelectedValue.ToString()+"'  AND year(MREM_FECHA)="+Año+" and month(MREM_FECHA)="+Mes+"  ");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				string descripcion=null;
				int codigoremesa=0;
				codigoremesa=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[0].ToString());
				descripcion=DBFunctions.SingleData("Select TREM_DESCRIPCION from DBXSCHEMA.TREMESA WHERE TREM_CODIGO="+codigoremesa+" ");
				int codigoEstado=0;
				string descrpcionestado=null;
				
				codigoEstado=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[11].ToString());
				descrpcionestado=DBFunctions.SingleData("Select TREMEST_DESCRIPCION from DBXSCHEMA.TREMESAESTADO WHERE TREMEST_CODIGO="+codigoEstado+" ");
				

				
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				
				fila= resultado.NewRow();
				double valor=0;
				double valordec=0;
				string ValorDecFormato=null;
				string ValorFormato=null;
				valordec=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[7].ToString());
				valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[10].ToString());
				ValorFormato=String.Format("{0:C}",valor);
				ValorDecFormato=String.Format("{0:C}",valordec);

				fila["NUMEROREMESA"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				fila["FECHA"]	= lineas.Tables[0].Rows[i].ItemArray[2].ToString();
				fila["DESCRIPCION"]	= descripcion.ToString();
				fila["NOMBREEMISOR"]	= lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				fila["NOMBREDESTINO"]	= lineas.Tables[0].Rows[i].ItemArray[4].ToString();
				fila["ORIGEN"]	= lineas.Tables[0].Rows[i].ItemArray[5].ToString();
				fila["DESTINO"]	= lineas.Tables[0].Rows[i].ItemArray[6].ToString();
				fila["VALORDECLARADO"]	= ValorDecFormato.ToString();
				fila["UNIDADES"] = lineas.Tables[0].Rows[i].ItemArray[8].ToString();
				fila["PESO"]	= lineas.Tables[0].Rows[i].ItemArray[9].ToString();
				fila["VALORFLETE"]	= ValorFormato.ToString(); 
				fila["ESTADOREMESA"] = descrpcionestado.ToString(); 
				fila["CONTENIDO"] = lineas.Tables[0].Rows[i].ItemArray[12].ToString();
				
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
			DataColumn numeroremesa = new DataColumn();
			numeroremesa.DataType = System.Type.GetType("System.String");
			numeroremesa.ColumnName = "NUMEROREMESA";
			numeroremesa.ReadOnly=true;
			resultado.Columns.Add(numeroremesa);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn fecha = new DataColumn();
			fecha.DataType = System.Type.GetType("System.String");
			fecha.ColumnName = "FECHA";
			fecha.ReadOnly=true;
			resultado.Columns.Add(fecha);
			
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn descripcion = new DataColumn();
			descripcion.DataType = System.Type.GetType("System.String");
			descripcion.ColumnName = "DESCRIPCION";
			descripcion.ReadOnly=true;
			resultado.Columns.Add(descripcion);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn nombreemisor = new DataColumn();
			nombreemisor.DataType = System.Type.GetType("System.String");
			nombreemisor.ColumnName = "NOMBREEMISOR";
			nombreemisor.ReadOnly=true;
			resultado.Columns.Add(nombreemisor);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn nombredestino = new DataColumn();
			nombredestino.DataType = System.Type.GetType("System.String");
			nombredestino.ColumnName = "NOMBREDESTINO";
			nombredestino.ReadOnly=true;
			resultado.Columns.Add(nombredestino);

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
			DataColumn valordeclarado = new DataColumn();
			valordeclarado.DataType = System.Type.GetType("System.String");
			valordeclarado.ColumnName = "VALORDECLARADO";
			valordeclarado.ReadOnly=true;
			resultado.Columns.Add(valordeclarado);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn unidades = new DataColumn();
			unidades.DataType = System.Type.GetType("System.String");
			unidades.ColumnName = "UNIDADES";
			unidades.ReadOnly=true;
			resultado.Columns.Add(unidades);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn peso = new DataColumn();
			peso.DataType = System.Type.GetType("System.String");
			peso.ColumnName = "PESO";
			peso.ReadOnly=true;
			resultado.Columns.Add(peso);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn valorflete = new DataColumn();
			valorflete.DataType = System.Type.GetType("System.String");
			valorflete.ColumnName = "VALORFLETE";
			valorflete.ReadOnly=true;
			resultado.Columns.Add(valorflete);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn descripcionEstado = new DataColumn();
			descripcionEstado.DataType = System.Type.GetType("System.String");
			descripcionEstado.ColumnName = "ESTADOREMESA";
			descripcionEstado.ReadOnly=true;
			resultado.Columns.Add(descripcionEstado);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn contenido = new DataColumn();
			contenido.DataType = System.Type.GetType("System.String");
			contenido.ColumnName = "CONTENIDO";
			contenido.ReadOnly=true;
			resultado.Columns.Add(contenido);



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

