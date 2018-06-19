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
	///		Descripción breve de AMS_Comercial_PlanillaAutorizacionServicio.
	/// </summary>
	public class AMS_Comercial_PlanillaAutorizacionServicio : System.Web.UI.UserControl
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
		protected string reportTitle="Planilla De Autorizacion de Servicios";
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
			DBFunctions.Request(lineas,IncludeSchema.NO,"select COD_AUTORIZACION,MAU_FECHA,MAU_CANTIDAD,MAU_VALOTOT,MAU_CODIGO FROM DBXSCHEMA.MAUTORIZACION WHERE MCAT_PLACA='"+Placa.SelectedValue.ToString()+"'  AND year(MAU_FECHA)="+Año+" and month(MAU_FECHA)="+Mes+"  ");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				string descripcion=null;
				int codigoservicio=0;
				codigoservicio=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[0].ToString());
				descripcion=DBFunctions.SingleData("select TAUT_DESCRIPCION FROM DBXSCHEMA.TAUTORIZACION WHERE COD_AUTORIZACION="+codigoservicio+" ");
				int valorservicio=0;
				valorservicio=Convert.ToInt32(DBFunctions.SingleData("select TAUT_VALO FROM DBXSCHEMA.TAUTORIZACION WHERE COD_AUTORIZACION="+codigoservicio+" "));

				
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				
				fila= resultado.NewRow();
				double valorserv=0;
				double valor=0;
				string ValorFormato=null;
				string ValorAutoFormato=null;
				valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[3].ToString());
				valorserv=Convert.ToDouble(valorservicio.ToString());
				ValorFormato=String.Format("{0:C}",valor);
				ValorAutoFormato=String.Format("{0:C}",valorserv);

				fila["FECHA"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				fila["CODIGO"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["DESCRIPCION"]	= descripcion.ToString();
				fila["VALORSERV"]	= ValorAutoFormato.ToString();
				fila["CANTIDAD"]	= lineas.Tables[0].Rows[i].ItemArray[2].ToString();
				fila["VALORTOTAL"]	= ValorFormato.ToString();
				fila["NUMAUTO"]= lineas.Tables[0].Rows[i].ItemArray[4].ToString();
			
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
            Utils.MostrarAlerta(Response, "  " + Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, Grid) + "");
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
			DataColumn codigo = new DataColumn();
			codigo.DataType = System.Type.GetType("System.String");
			codigo.ColumnName = "CODIGO";
			codigo.ReadOnly=true;
			resultado.Columns.Add(codigo);
			
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn descripcion = new DataColumn();
			descripcion.DataType = System.Type.GetType("System.String");
			descripcion.ColumnName = "DESCRIPCION";
			descripcion.ReadOnly=true;
			resultado.Columns.Add(descripcion);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valorservicio = new DataColumn();
			valorservicio.DataType = System.Type.GetType("System.String");
			valorservicio.ColumnName = "VALORSERV";
			valorservicio.ReadOnly=true;
			resultado.Columns.Add(valorservicio);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn cantidad = new DataColumn();
			cantidad.DataType = System.Type.GetType("System.String");
			cantidad.ColumnName = "CANTIDAD";
			cantidad.ReadOnly=true;
			resultado.Columns.Add(cantidad);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn valortotal = new DataColumn();
			valortotal.DataType = System.Type.GetType("System.String");
			valortotal.ColumnName = "VALORTOTAL";
			valortotal.ReadOnly=true;
			resultado.Columns.Add(valortotal);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn numauto = new DataColumn();
			numauto.DataType = System.Type.GetType("System.String");
			numauto.ColumnName = "NUMAUTO";
			numauto.ReadOnly=true;
			resultado.Columns.Add(numauto);

			
			
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
