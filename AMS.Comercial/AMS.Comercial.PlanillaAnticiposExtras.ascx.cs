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
	/// <summary>
	///		Descripción breve de AMS_Comercial_PlanillaAnticiposExtras.
	/// </summary>
	public class AMS_Comercial_PlanillaAnticiposExtras : System.Web.UI.UserControl
	{
		protected Label lb;
		protected DataSet lineas;
		protected DataTable resultado;
		protected DataGrid Grid;
		protected Table tabPreHeader,tabFirmas;
		protected PlaceHolder toolsHolder;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList placa;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.RadioButtonList RadioButtonList1;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Button generar;
		protected System.Web.UI.WebControls.TextBox AñoBox;
		protected System.Web.UI.WebControls.TextBox MesBox;
		protected System.Web.UI.WebControls.TextBox DiaBox;
		
		protected string reportTitle="Planilla De Anticipos Extras";

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(placa,"Select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
				
				
						
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
			this.RadioButtonList1.SelectedIndexChanged += new System.EventHandler(this.RadioButtonList1_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void RadioButtonList1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(RadioButtonList1.SelectedItem.Value.Equals("1"))
			{
				Label3.Visible=true;
				AñoBox.Visible=true;
				AñoBox.Text=DateTime.Now.Year.ToString();
			
			}
			if(RadioButtonList1.SelectedItem.Value.Equals("2"))
			{
				Label3.Visible=true;
				AñoBox.Visible=true;
				Label4.Visible=true;
				MesBox.Visible=true;
				AñoBox.Text=DateTime.Now.Year.ToString();
				MesBox.Text=DateTime.Now.Month.ToString();

			
			}
			if(RadioButtonList1.SelectedItem.Value.Equals("3"))
			{
				Label3.Visible=true;
				AñoBox.Visible=true;
				Label4.Visible=true;
				MesBox.Visible=true;
				Label5.Visible=true;
				DiaBox.Visible=true;
				AñoBox.Text=DateTime.Now.Year.ToString();
				MesBox.Text=DateTime.Now.Month.ToString();
				DiaBox.Text=DateTime.Now.Day.ToString();
			}
		}
	
		protected  void  generar_OnClick(Object  Sender, EventArgs e)
		{
			string []pr=new string[2];
			pr[0]=pr[1]="";
			Press frontEnd = new Press(new DataSet(), reportTitle);
			frontEnd.PreHeader(tabPreHeader, Grid.Width, pr);
			frontEnd.Firmas(tabFirmas,Grid.Width);
			lb.Text="";
			
			string fecha=null;
			if(RadioButtonList1.SelectedItem.Value.Equals("1"))
			{
				fecha="AND YEAR(MAN_FECHA)="+AñoBox.Text.ToString()+" ";
			}
			if(RadioButtonList1.SelectedItem.Value.Equals("2"))
			{
				fecha="AND YEAR(MAN_FECHA)="+AñoBox.Text.ToString()+" AND MONTH(MAN_FECHA)="+MesBox.Text.ToString()+"  ";
			}
			if(RadioButtonList1.SelectedItem.Value.Equals("3"))
			{
				fecha="AND YEAR(MAN_FECHA)="+AñoBox.Text.ToString()+" AND MONTH(MAN_FECHA)="+MesBox.Text.ToString()+" AND DAY(MAN_FECHA)="+DiaBox.Text.ToString()+" ";
			}
			
			///////////////////////////////////////////////////////
			this.PrepararTabla();
			lineas = new DataSet();
			string query=null;
			query="select PREF_DOCU CONCAT' 'CONCAT CAST(MAN_CODIGO AS CHARACTER(10)),MAN_FECHA,COD_ANTICIPO,MCAT_PLACA,MNIT_EMPLEADO_CONDUCTOR,MAN_DESC,MAN_VALOTOTAL from DBXSCHEMA.MANTICIPO_EXTRA WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' "+fecha.ToString()+" ";
			//                                                           0                 1           2          3                   4                5          6       
			DBFunctions.Request(lineas,IncludeSchema.NO,""+query.ToString()+"");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				string descripcion=null;
				descripcion=DBFunctions.SingleData("SELECT TTIPA_DESCRIPCION from DBXSCHEMA.TTIOPOANTICIPO WHERE TTIPA_CODIGO="+lineas.Tables[0].Rows[i].ItemArray[2].ToString()+"");
				string nombre=null;
				nombre=DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MEMPLEADO MEMP WHERE MNIT.MNIT_NIT='"+lineas.Tables[0].Rows[i].ItemArray[4].ToString()+"'");
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				
				fila= resultado.NewRow();
				double valor=0;
				string ValorFormato=null;
				valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[6].ToString());
				ValorFormato=String.Format("{0:C}",valor);
				string Fecha=Convert.ToDateTime(lineas.Tables[0].Rows[i].ItemArray[1]).ToString("yyyy-MM-dd");
				

				fila["DOCUMENTO"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["FECHA"]	= Fecha.ToString();
				fila["TANTICIPO"]	= descripcion.ToString();
				fila["PLACA"]	= lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				fila["NOMBRE"]	= nombre.ToString();
				fila["DESC"]	= descripcion.ToString();
				fila["VALOR"]	= ValorFormato.ToString();
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
		
	
		
		

		public void PrepararTabla()
		{
			resultado = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn documento = new DataColumn();
			documento.DataType = System.Type.GetType("System.String");
			documento.ColumnName = "DOCUMENTO";
			documento.ReadOnly=true;
			resultado.Columns.Add(documento);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn fecha = new DataColumn();
			fecha.DataType = System.Type.GetType("System.String");
			fecha.ColumnName = "FECHA";
			fecha.ReadOnly=true;
			resultado.Columns.Add(fecha);
			
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn tanticipo = new DataColumn();
			tanticipo.DataType = System.Type.GetType("System.String");
			tanticipo.ColumnName = "TANTICIPO";
			tanticipo.ReadOnly=true;
			resultado.Columns.Add(tanticipo);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn placa = new DataColumn();
			placa.DataType = System.Type.GetType("System.String");
			placa.ColumnName = "PLACA";
			placa.ReadOnly=true;
			resultado.Columns.Add(placa);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn nombre = new DataColumn();
			nombre.DataType = System.Type.GetType("System.String");
			nombre.ColumnName = "NOMBRE";
			nombre.ReadOnly=true;
			resultado.Columns.Add(nombre);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn desc = new DataColumn();
			desc.DataType = System.Type.GetType("System.String");
			desc.ColumnName = "DESC";
			desc.ReadOnly=true;
			resultado.Columns.Add(desc);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valor = new DataColumn();
			valor.DataType = System.Type.GetType("System.String");
			valor.ColumnName = "VALOR";
			valor.ReadOnly=true;
			resultado.Columns.Add(valor);

			


		}
	}

}
