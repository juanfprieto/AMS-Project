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
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_ConsolidarAutorizaciones.
	/// </summary>
	public class AMS_Comercial_ConsolidarAutorizaciones : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.DropDownList añoI;
		protected System.Web.UI.WebControls.DropDownList mesI;
		protected System.Web.UI.WebControls.DropDownList añoF;
		protected System.Web.UI.WebControls.DropDownList mesF;
		protected System.Web.UI.WebControls.TextBox diaF;
		protected System.Web.UI.WebControls.TextBox diaI;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.DropDownList placa;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label busLabel;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label PropLabel;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label conductor;
		protected System.Web.UI.WebControls.Button Generar;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label totauto;
		protected System.Web.UI.WebControls.Label numauto;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Button Detalles;
		protected System.Web.UI.WebControls.Panel Panel2;
		
		protected DataSet lineas;
		protected DataTable resultado;
		protected DataGrid Grid;

		protected System.Web.UI.WebControls.Label Label1;
		string fechaI=null;
		protected System.Web.UI.WebControls.Button Guardar;
		string fechaF=null;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				int contadorBus=0;
				if(contadorBus==0)
				{
					bind.PutDatasIntoDropDownList(placa,"Select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
					ListItem it=new ListItem("--PLACA--","0");
					placa.Items.Insert(0,it);
				}
				
				int contadorAÑOi=0;
				if(contadorAÑOi==0)
				{
					bind.PutDatasIntoDropDownList(añoI, "SELECT pano_ano FROM DBXSCHEMA.pano");
					ListItem it=new ListItem("-Año-");
					añoI.Items.Insert(0,it);
				}
				int contadorMESi=0;
				if(contadorMESi==0)
				{
					bind.PutDatasIntoDropDownList(mesI, "SELECT  pmes_mes,pmes_nombre FROM DBXSCHEMA.pmes");		
					ListItem it=new ListItem("--Mes--");
					mesI.Items.Insert(0,it);

				}
				int contadorAÑOf=0;
				if(contadorAÑOf==0)
				{
					bind.PutDatasIntoDropDownList(añoF, "SELECT pano_ano FROM DBXSCHEMA.pano");		
					ListItem it=new ListItem("-Año-");
					añoF.Items.Insert(0,it);
				}
				int contadorMESf=0;
				if(contadorMESf==0)
				{
					bind.PutDatasIntoDropDownList(mesF, "SELECT  pmes_mes,pmes_nombre FROM DBXSCHEMA.pmes");		
					ListItem it=new ListItem("--Mes--");
					mesF.Items.Insert(0,it);
				}
				
				
				
				diaI.Text=DateTime.Now.Day.ToString();
				diaF.Text=DateTime.Now.Day.ToString();
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
			this.placa.SelectedIndexChanged += new System.EventHandler(this.placa_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void placa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			string nitProp=DBFunctions.SingleData("select 	MNIT_NITPROPIETARIO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' ");
			string nombresProp=DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MEMPLEADO MEMP WHERE MNIT.MNIT_NIT='"+nitProp.ToString()+"' ");
			PropLabel.Text=nombresProp.ToString();
			string nitCondu=DBFunctions.SingleData("select 	MNIT_NITCHOFER FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' ");
			string nombreCondu=DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MEMPLEADO MEMP WHERE MNIT.MNIT_NIT='"+nitCondu.ToString()+"' ");
			conductor.Text=nombreCondu.ToString();
			string NumBus=DBFunctions.SingleData("select MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'");
			busLabel.Text=NumBus.ToString();
		}
		public  void  generar_OnClick(Object  Sender, EventArgs e)
		{
		
		DatasToControls bind = new DatasToControls();
		fechaI=añoI.SelectedValue.ToString()+"-"+mesI.SelectedValue.ToString()+"-"+diaI.Text.ToString();
		fechaF=añoF.SelectedValue.ToString()+"-"+mesF.SelectedValue.ToString()+"-"+diaF.Text.ToString();
		string NumAutori=DBFunctions.SingleData("select count(*)  from DBXSCHEMA.MAUTORIZACION where MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MAU_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"' ");
		string TotAutoS=DBFunctions.SingleData("select sum(MAU_VALOTOT)  from DBXSCHEMA.MAUTORIZACION where MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MAU_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
		double TotAuto=0;
			if(TotAutoS.Equals("") || TotAutoS.Equals(null))
			{
				TotAuto=0;
			}
			else
			{
			TotAuto=Convert.ToDouble(DBFunctions.SingleData("select sum(MAU_VALOTOT)  from DBXSCHEMA.MAUTORIZACION where MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MAU_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			
		numauto.Text=NumAutori.ToString();
		string TotAutoFormato=String.Format("{0:C}",TotAuto);
		totauto.Text=TotAutoFormato.ToString();
		
		TextBox1.Text=TotAutoFormato.ToString();
		
		}
		public  void  Detalles_OnClick(Object  Sender, EventArgs e)
		{
			fechaI=añoI.SelectedValue.ToString()+"-"+mesI.SelectedValue.ToString()+"-"+diaI.Text.ToString();
			fechaF=añoF.SelectedValue.ToString()+"-"+mesF.SelectedValue.ToString()+"-"+diaF.Text.ToString();
			this.PrepararTabla();
			lineas = new DataSet();
			DBFunctions.Request(lineas,IncludeSchema.NO,"SELECT PREF_DOCU concat' 'concat cast(MAU_CODIGO AS character (10) ),MAU_FECHA,MAU_CANTIDAD,COD_AUTORIZACION,MAU_VALOTOT FROM DBXSCHEMA.MAUTORIZACION WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MAU_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			//                                                              0                                                       1           2           3              4        
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				string descripcion=null;
				int codigoservicio=0;
				codigoservicio=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[3].ToString());
				descripcion=DBFunctions.SingleData("select TAUT_DESCRIPCION FROM DBXSCHEMA.TAUTORIZACION WHERE COD_AUTORIZACION="+codigoservicio+" ");
							
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				
				fila= resultado.NewRow();
				int valorserv=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[4]);
				string ValorAutoFormato=null;
				ValorAutoFormato=String.Format("{0:C}",valorserv);

				fila["NUMERO"]= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["FECHA"]	= Convert.ToDateTime(lineas.Tables[0].Rows[i].ItemArray[1]).ToString("yyyy-MM-dd");
				fila["CANTIDAD"]	= lineas.Tables[0].Rows[i].ItemArray[2].ToString();
				fila["DESCRIPCION"]	= descripcion.ToString();
				fila["VALOR"]	= ValorAutoFormato.ToString();
				resultado.Rows.Add(fila);
			
			}
			
			//fin sentencia FOR	
			Grid.DataSource = resultado;
			Grid.DataBind();	
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
			DataColumn numero = new DataColumn();
			numero.DataType = System.Type.GetType("System.String");
			numero.ColumnName = "NUMERO";
			numero.ReadOnly=true;
			resultado.Columns.Add(numero);
			
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn cantidad = new DataColumn();
			cantidad.DataType = System.Type.GetType("System.String");
			cantidad.ColumnName = "CANTIDAD";
			cantidad.ReadOnly=true;
			resultado.Columns.Add(cantidad);
			
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

	
		public void Guardar_Onclick(Object  Sender, EventArgs e)
		{
		
			fechaI=añoI.SelectedValue.ToString()+"-"+mesI.SelectedValue.ToString()+"-"+diaI.Text.ToString();
			fechaF=añoF.SelectedValue.ToString()+"-"+mesF.SelectedValue.ToString()+"-"+diaF.Text.ToString();
			string NumAutori=DBFunctions.SingleData("select count(*)  from DBXSCHEMA.MAUTORIZACION where MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MAU_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"' ");
			string TotAutoS=DBFunctions.SingleData("select sum(MAU_VALOTOT)  from DBXSCHEMA.MAUTORIZACION where MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MAU_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'");
			double TotAuto=0;
			if(TotAutoS.Equals("") || TotAutoS.Equals(null))
			{
				TotAuto=0;
			}
			else
			{
				TotAuto=Convert.ToDouble(DBFunctions.SingleData("select sum(MAU_VALOTOT)  from DBXSCHEMA.MAUTORIZACION where MCAT_PLACA='"+placa.SelectedValue.ToString()+"' AND MAU_FECHA BETWEEN '"+fechaI.ToString()+"' AND '"+fechaF.ToString()+"'"));
			}
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MCONSOLIDAR_AUTOSERVI VALUES(DEFAULT,CURRENT DATE,'"+placa.SelectedValue.ToString()+"',"+NumAutori+","+TotAuto+")" );
			//Label14.Text=DBFunctions.exceptions; 
            Utils.MostrarAlerta(Response, "Consolidacion Realizada Satisfactorimente");
		}
	}
}
