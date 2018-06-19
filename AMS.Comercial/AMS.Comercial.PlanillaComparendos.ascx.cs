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

	/// <summary>
	///		Descripción breve de AMS_Comercial_PlanillaComparendos.
	/// </summary>
	public class AMS_Comercial_PlanillaComparendos : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.DropDownList añoI;
		protected System.Web.UI.WebControls.DropDownList mesI;
		protected System.Web.UI.WebControls.TextBox diaI;
		protected System.Web.UI.WebControls.DropDownList añoF;
		protected System.Web.UI.WebControls.DropDownList mesF;
		protected System.Web.UI.WebControls.TextBox diaF;
		protected System.Web.UI.WebControls.DropDownList conductor;
		protected System.Web.UI.WebControls.CheckBox pago;
		protected System.Web.UI.WebControls.CheckBox proceso;
		protected System.Web.UI.WebControls.CheckBox nopago;
		protected System.Web.UI.WebControls.CheckBox todos;
		protected System.Web.UI.WebControls.TextBox tbEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.ImageButton ibMail;
		protected System.Web.UI.WebControls.PlaceHolder toolsHolder;
		protected System.Web.UI.WebControls.Table tabPreHeader;
		protected System.Web.UI.WebControls.DataGrid Grid;
		protected System.Web.UI.WebControls.Table tabFirmas;
		protected System.Web.UI.WebControls.Label lb;
		protected System.Web.UI.WebControls.Button Generar;
		protected System.Web.UI.WebControls.Label RemeasLabel;
		protected string reportTitle="Relacion De Comparendos";
		protected DataSet lineas;
		protected DataTable resultado;
		//int codigoestado=0;
		string conductoresS=null;
		string queryF=null;
		string queryEstado=null;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				
				bind.PutDatasIntoDropDownList(añoI, "SELECT pano_ano FROM DBXSCHEMA.pano");
				bind.PutDatasIntoDropDownList(mesI, "SELECT  pmes_nombre FROM DBXSCHEMA.pmes");		
				bind.PutDatasIntoDropDownList(añoF, "SELECT pano_ano FROM DBXSCHEMA.pano");
				bind.PutDatasIntoDropDownList(mesF, "SELECT  pmes_nombre FROM DBXSCHEMA.pmes");		
				if(conductor.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(conductor,"select MNIT.MNIT_NOMBRES CONCAT ' ' CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' 'CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') AS NOMBRE FROM DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MBUSAFILIADO MBUS WHERE MNIT.MNIT_NIT=MBUS.MNIT_NITCHOFER ORDER BY NOMBRE");
					ListItem it=new ListItem("--TODOS--","0");
					conductor.Items.Insert(0,it);
				}

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
//////////////FECHA////////////
			int Año1=Convert.ToInt32(añoI.SelectedValue.ToString());
			int Año2=Convert.ToInt32(añoF.SelectedValue.ToString());
			int Mes1=Convert.ToInt32(DBFunctions.SingleData("select PMES_MES from DBXSCHEMA.PMES WHERE PMES_NOMBRE='"+mesI.SelectedValue.ToString()+"'"));
			int Mes2=Convert.ToInt32(DBFunctions.SingleData("select PMES_MES from DBXSCHEMA.PMES WHERE PMES_NOMBRE='"+mesF.SelectedValue.ToString()+"'"));
			int dia1=1;
			int dia2=1;
			string dia1S=diaI.Text;
			string dia2S=diaF.Text;
			if(dia1S.ToString().Equals(null) || dia1S.ToString().Equals("0"))
			{
				dia1=1;
			}
			else
			{
				dia1=Convert.ToInt32(diaI.Text.ToString());
			}
			if(dia2S.ToString().Equals(null) || dia2S.ToString().Equals("0"))
			{
				dia2=Convert.ToInt32(DateTime.Now.Day);
			}
			else
			{
				dia2=Convert.ToInt32(diaF.Text.ToString());
			}

			string fecha = Año1 + "-" + Mes1 + "-" + dia1; 
			string fecha2= Año2 + "-" + Mes2 + "-" + dia2; 

///////////////////////////////
/////opciones de estado comparendo
			if(pago.Checked.Equals(true))
			{
			queryEstado="AND MCOM_ESTADO =1"; 
			}
			if(nopago.Checked.Equals(true))
			{
			queryEstado="AND MCOM_ESTADO =3";
			}
			if(proceso.Checked.Equals(true))
			{
			queryEstado="AND MCOM_ESTADO =2";
			}
			if(todos.Checked.Equals(true))
			{
			queryEstado=" ";
			}
//////////////////////////////////////////////////
////seleccion conductor

			conductoresS=conductor.SelectedValue.ToString();

			if(conductoresS.Equals("0"))
			{
				queryF="select MCOM_FECHACOM,MCOM_INFRACCION,MCOM_CONDUCTOR,MCOM_ESTADO,MCOM_NUMCOM FROM DBXSCHEMA.MCOMPARENDOS WHERE MCOM_FECHACOM BETWEEN '"+fecha+"' AND '"+fecha2+"'  "+queryEstado+" ";
			}
			else
			{
			string nit=DBFunctions.SingleData("select MNIT.MNIT_NIT FROM DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NOMBRES CONCAT ' ' CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' 'CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') ='"+conductoresS+"' ");
			queryF="select MCOM_FECHACOM,MCOM_INFRACCION,MCOM_CONDUCTOR,MCOM_ESTADO,MCOM_NUMCOM FROM DBXSCHEMA.MCOMPARENDOS WHERE MCOM_FECHACOM BETWEEN '"+fecha+"' AND '"+fecha2+"'  "+queryEstado+" AND MCOM_CONDUCTOR='"+nit+"' ";
			}
///////////////////////////////////////////////////////			
			
			
			this.PrepararTabla();
			lineas = new DataSet();
			
			DBFunctions.Request(lineas,IncludeSchema.NO," "+queryF+" ");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				string infraccion=null;
				int codigoinfraccion=0;
				double valorcomparendo=0;
				codigoinfraccion=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[1].ToString());
				infraccion=DBFunctions.SingleData("Select DESC_COMPARENDO from DBXSCHEMA.TCOMPARENDO WHERE COD_COMPARENDO="+codigoinfraccion+" ");
				valorcomparendo=Convert.ToDouble(DBFunctions.SingleData("Select VALO_COMPARENDO from DBXSCHEMA.TCOMPARENDO WHERE COD_COMPARENDO="+codigoinfraccion+" "));
				string estadoIn=null;
				int estadoInfraccion=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[3].ToString());
				estadoIn=DBFunctions.SingleData("Select DESC_ESTACOMPA from DBXSCHEMA.TESTADO_COMPARENDO WHERE COD_ESTACOMPA="+estadoInfraccion+" ");
				string nombreco=DBFunctions.SingleData("select MNIT.MNIT_NOMBRES CONCAT ' ' CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' 'CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') AS NOMBRE FROM DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT='"+lineas.Tables[0].Rows[i].ItemArray[2].ToString()+"' "); 
				
				
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				
				fila= resultado.NewRow();
				
				string ValorFormato=null;
				ValorFormato=String.Format("{0:C}",valorcomparendo);
				

				
				fila["FECHA"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["NUMCOMPARENDO"]	= lineas.Tables[0].Rows[i].ItemArray[4].ToString();
				fila["INFRACCION"]	= infraccion.ToString();
				fila["VALOR"]	= ValorFormato.ToString();
				fila["CONDUCTOR"]	= nombreco.ToString();
				fila["ESTADO"]	= estadoIn.ToString();
				
				
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
			DataColumn numcomparendo = new DataColumn();
			numcomparendo.DataType = System.Type.GetType("System.String");
			numcomparendo.ColumnName = "NUMCOMPARENDO";
			numcomparendo.ReadOnly=true;
			resultado.Columns.Add(numcomparendo);
			
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn infraccion = new DataColumn();
			infraccion.DataType = System.Type.GetType("System.String");
			infraccion.ColumnName = "INFRACCION";
			infraccion.ReadOnly=true;
			resultado.Columns.Add(infraccion);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valor = new DataColumn();
			valor.DataType = System.Type.GetType("System.String");
			valor.ColumnName = "VALOR";
			valor.ReadOnly=true;
			resultado.Columns.Add(valor);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn conductor = new DataColumn();
			conductor.DataType = System.Type.GetType("System.String");
			conductor.ColumnName = "CONDUCTOR";
			conductor.ReadOnly=true;
			resultado.Columns.Add(conductor);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn estado = new DataColumn();
			estado.DataType = System.Type.GetType("System.String");
			estado.ColumnName = "ESTADO";
			estado.ReadOnly=true;
			resultado.Columns.Add(estado);
			
			



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

