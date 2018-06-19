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
	///		Descripci�n breve de AMS_Comercial_Planilla.
	/// </summary>
	public class AMS_Comercial_Planilla : System.Web.UI.UserControl
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
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList a�o;
		protected System.Web.UI.WebControls.DropDownList mes;	
		protected string reportTitle="Planilla De Anticipos Por Bus";
		int Mes=0;
		int A�o=0;
		
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(Placa,"Select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
				bind.PutDatasIntoDropDownList(a�o, "SELECT pano_ano FROM DBXSCHEMA.pano");
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
			A�o=Convert.ToInt32(a�o.SelectedValue.ToString());
			Mes=Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes FROM DBXSCHEMA.pmes WHERE pmes_nombre='"+mes.SelectedValue.ToString()+"' "));
			

			///////////////////////////////////////////////////////
			this.PrepararTabla();
			lineas = new DataSet();
			DBFunctions.Request(lineas,IncludeSchema.NO,"select MAN_CODIGO,MAN_FECHA,COD_ANTICIPO,MCAT_PLACA,MAN_VALOTOTAL from DBXSCHEMA.MANTICIPO MAN  where MAN.MCAT_PLACA='"+Placa.SelectedValue.ToString()+"' AND year(MAN_FECHA)="+A�o+" and month(MAN_FECHA)="+Mes+" ");
			//                                                          0        1        2               3           4
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				
				
				string codigoE=DBFunctions.SingleData("select TTANTI_CODIGO FROM DBXSCHEMA.TANTICIPO WHERE COD_ANTICIPO="+lineas.Tables[0].Rows[i].ItemArray[2]+"");
				string descripcion=DBFunctions.SingleData("select TTIPA_DESCRIPCION from DBXSCHEMA.TTIOPOANTICIPO WHERE TTIPA_CODIGO="+codigoE+" ");
				//codigoanticipo=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[2].ToString());
				//descripcion=DBFunctions.SingleData("select TTIPA_DESCRIPCION from DBXSCHEMA.TTIOPOANTICIPO WHERE TTIPA_CODIGO="+codigoanticipo+" ");
				
				
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				
				
				//SaldoFinal1=String.Format("{0:C}",saldoLinea);
				fila= resultado.NewRow();
				double valor=0;
				string ValorFormato=null;
				valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[4].ToString());
				ValorFormato=String.Format("{0:C}",valor);
				
				fila["FECHA"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				fila["CODIGO"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["DESCRIPCION"]	= descripcion.ToString();
				fila["VALOR"]	= ValorFormato;
				
				
				
			
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
			DataColumn valor = new DataColumn();
			valor.DataType = System.Type.GetType("System.String");
			valor.ColumnName = "VALOR";
			valor.ReadOnly=true;
			resultado.Columns.Add(valor);
						
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