namespace AMS.Comercial
{
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
	using System;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Tools;
	using Ajax;

	/// <summary>
	///		Descripción breve de AMS_Comercial_AuxilioMutuo.
	/// </summary>
	public class AMS_Comercial_AuxilioMutuo : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList bus;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox acta;
		protected System.Web.UI.WebControls.TextBox fecha;
		protected System.Web.UI.WebControls.Button calcular;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.TextBox valoPC;
		protected System.Web.UI.WebControls.TextBox sumC;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox sumB;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox valoPB;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox sumA;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.TextBox PorA;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.TextBox valoPA;
		
		protected System.Web.UI.WebControls.Label Label1;
		////
		int contDias=0;
		protected DataSet lineas;
		protected DataSet lineas1;
		protected DataSet lineas2;
		protected DataTable resultado;
		protected DataTable resultado1;
		protected DataTable resultado2;
		protected DataGrid Grid2;
		int canA=0;
		int canB=0;
		int canC=0;
		double CuotaA=0;
		double CuotaB=0;
		double CuotaC=0;
		double ValorCategoriaA=0;
		double ValorCategoriaB=0;
		double ValorCategoriaC=0;
		
		protected System.Web.UI.WebControls.Button Guardar;
		protected System.Web.UI.WebControls.TextBox PorB;
		protected System.Web.UI.WebControls.TextBox PorC;
		protected System.Web.UI.WebControls.DataGrid Grid1;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.TextBox totalA;
		protected System.Web.UI.WebControls.TextBox totalB;
		protected System.Web.UI.WebControls.TextBox totalC;
		protected DataGrid Grid;
		//////
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				int placaItem=0;
				if(placaItem==0)
				{
					bind.PutDatasIntoDropDownList(bus,"Select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
					ListItem it=new ListItem("--Placa--","0");
					bus.Items.Insert(0,it);
				}	
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
			}
			else
			{
				if (Session["CuotaA"]!=null)
					CuotaA=Convert.ToDouble(Session["CuotaA"]);
				if(Session["CuotaB"]!=null)
					CuotaB=Convert.ToDouble(Session["CuotaB"]);
				if(Session["CuotaC"]!=null)
					CuotaC=Convert.ToDouble(Session["CuotaC"]);
						

			}

		}
		public void Calcular_Click(object sender, System.EventArgs e)
		{
		DatasToControls bind = new DatasToControls();
		
		this.PrepararTabla();
		lineas = new DataSet();
		this.PrepararTabla1();
		lineas1 = new DataSet();
		this.PrepararTabla2();
		lineas2 = new DataSet();
		///////// Categoria A /////////
			DBFunctions.Request(lineas,IncludeSchema.NO,"SELECT MBUS_NUMERO,MCAT_PLACA,FEC_INGRESO,MNIT_NITPROPIETARIO,MBUS_VALOR from DBXSCHEMA.MBUSAFILIADO WHERE MBUS_CATEGORIA='A'");
			//                                                          0        1        2               3                    4    
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				
				
				string Nombre=DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MEMPLEADO MEMP WHERE MNIT.MNIT_NIT='"+lineas.Tables[0].Rows[i].ItemArray[3].ToString()+"'");
				string fechaF=Convert.ToDateTime(lineas.Tables[0].Rows[i].ItemArray[2]).ToString("yyyy-MM-dd");
				DataRow fila;
					
				
				fila= resultado.NewRow();
				double valor=0;
				string ValorFormato=null;
				valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[4].ToString());
				ValorFormato=String.Format("{0:C}",valor);
				
				fila["NUMERO"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["PLACA"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				fila["FECHA"]	= fechaF.ToString();
				fila["PROPIETARIO"]	= Nombre.ToString();
				fila["AVALUO"]	= ValorFormato.ToString();
				
				
				
			
				resultado.Rows.Add(fila);
						
			ValorCategoriaA=ValorCategoriaA+valor;
			canA=canA+1;
			}
			
			//fin sentencia FOR	
			Grid.DataSource = resultado;
			Grid.DataBind();	
			string valorAcumCategoriaA=String.Format("{0:C}",ValorCategoriaA);
			sumA.Text=valorAcumCategoriaA.ToString();
		///////////////////////////////
			//////////// Categoria B /////////
			DBFunctions.Request(lineas1,IncludeSchema.NO,"SELECT MBUS_NUMERO,MCAT_PLACA,FEC_INGRESO,MNIT_NITPROPIETARIO,MBUS_VALOR from DBXSCHEMA.MBUSAFILIADO WHERE MBUS_CATEGORIA='B'");
			//                                                          0        1        2               3                    4    
			for(int a=0;a<lineas1.Tables[0].Rows.Count;a++)
			{
				
				
				string NombreB=DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MEMPLEADO MEMP WHERE MNIT.MNIT_NIT='"+lineas1.Tables[0].Rows[a].ItemArray[3].ToString()+"'");
				string fechaFB=Convert.ToDateTime(lineas1.Tables[0].Rows[a].ItemArray[2]).ToString("yyyy-MM-dd");
				DataRow fila1;
					
				
				fila1= resultado1.NewRow();
				double valorB=0;
				string ValorFormatoB=null;
				valorB=Convert.ToDouble(lineas1.Tables[0].Rows[a].ItemArray[4].ToString());
				ValorFormatoB=String.Format("{0:C}",valorB);
				
				fila1["NUMEROB"]	= lineas1.Tables[0].Rows[a].ItemArray[0].ToString();
				fila1["PLACAB"]	= lineas1.Tables[0].Rows[a].ItemArray[1].ToString();
				fila1["FECHAB"]	= fechaFB.ToString();
				fila1["PROPIETARIOB"]	= NombreB.ToString();
				fila1["AVALUOB"]	= ValorFormatoB.ToString();
				
				
				
			
				resultado1.Rows.Add(fila1);
						
				ValorCategoriaB=ValorCategoriaB+valorB;
				canB=canB+1;
			}
			
			//fin sentencia FOR	
			Grid1.DataSource = resultado1;
			Grid1.DataBind();	
			string valorAcumCategoriaB=String.Format("{0:C}",ValorCategoriaB);
			sumB.Text=valorAcumCategoriaB.ToString();
		//////////////////////////////////////////////////
			/////////////// Categoria C /////////
			DBFunctions.Request(lineas2,IncludeSchema.NO,"SELECT MBUS_NUMERO,MCAT_PLACA,FEC_INGRESO,MNIT_NITPROPIETARIO,MBUS_VALOR from DBXSCHEMA.MBUSAFILIADO WHERE MBUS_CATEGORIA='C'");
			//                                                          0        1        2               3                    4    
			for(int b=0;b<lineas2.Tables[0].Rows.Count;b++)
			{
				
				
				string NombreC=DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MEMPLEADO MEMP WHERE MNIT.MNIT_NIT='"+lineas2.Tables[0].Rows[b].ItemArray[3].ToString()+"'");
				string fechaFC=Convert.ToDateTime(lineas2.Tables[0].Rows[b].ItemArray[2]).ToString("yyyy-MM-dd");
				DataRow fila2;
					
				
				fila2= resultado2.NewRow();
				double valorC=0;
				string ValorFormatoC=null;
				valorC=Convert.ToDouble(lineas2.Tables[0].Rows[b].ItemArray[4].ToString());
				ValorFormatoC=String.Format("{0:C}",valorC);
				
				fila2["NUMEROC"]	= lineas2.Tables[0].Rows[b].ItemArray[0].ToString();
				fila2["PLACAC"]	= lineas2.Tables[0].Rows[b].ItemArray[1].ToString();
				fila2["FECHAC"]	= fechaFC.ToString();
				fila2["PROPIETARIOC"]	= NombreC.ToString();
				fila2["AVALUOC"]	= ValorFormatoC.ToString();
				resultado2.Rows.Add(fila2);
						
				ValorCategoriaC=ValorCategoriaC+valorC;
				canC=canC+1;
			}
			
			//fin sentencia FOR	
			Grid2.DataSource = resultado2;
			Grid2.DataBind();	
			string valorAcumCategoriaC=String.Format("{0:C}",ValorCategoriaC);
			sumC.Text=valorAcumCategoriaC.ToString();
			////////////////////////////////////
			double ValorTotalBuses=Math.Round((ValorCategoriaA+ValorCategoriaB+ValorCategoriaC),2);
			double PorcentajeA=Math.Round((ValorCategoriaA/ValorTotalBuses)*100,2);
			double PorcentajeB=Math.Round((ValorCategoriaB/ValorTotalBuses)*100,2);
			double PorcentajeC=Math.Round((ValorCategoriaC/ValorTotalBuses)*100,2);
			PorA.Text=PorcentajeA.ToString();
			PorB.Text=PorcentajeB.ToString();
			PorC.Text=PorcentajeC.ToString();
			double ValorProrrateo=Convert.ToDouble(TextBox1.Text.ToString());
			//
			 CuotaA=((ValorProrrateo*PorcentajeA)/100)/canA;
			string CuotaAFormato=String.Format("{0:C}",CuotaA);
			valoPA.Text=CuotaAFormato.ToString();
			CuotaB=((ValorProrrateo*PorcentajeB)/100)/canB;
			string CuotaBFormato=String.Format("{0:C}",CuotaB);
			valoPB.Text=CuotaBFormato.ToString();
			CuotaC=((ValorProrrateo*PorcentajeC)/100)/canC;
			string CuotaCFormato=String.Format("{0:C}",CuotaC);
			valoPC.Text=CuotaCFormato.ToString();
			/*HtmlInputHidden hd1=new HtmlInputHidden();
			hd.ID="+CuotaA+";
			HtmlInputHidden hd2=new HtmlInputHidden();
			hd2.ID="+CuotaB+";
			HtmlInputHidden hd3=new HtmlInputHidden();
			hd3.ID="+CuotaC+";*/
			Session["CuotaC"]=CuotaC;
			Session["CuotaB"]=CuotaB;
			Session["CuotaA"]=CuotaA;
			double TotalCategoriaA=CuotaA * canA;
			double TotalCategoriaB=CuotaB * canB;
			double TotalCategoriaC=CuotaC * canC;
			string FormatoTotalCategoriaA=String.Format("{0:C}",TotalCategoriaA);
			string FormatoTotalCategoriaB=String.Format("{0:C}",TotalCategoriaB);
			string FormatoTotalCategoriaC=String.Format("{0:C}",TotalCategoriaC);
			totalA.Text=FormatoTotalCategoriaA.ToString();
			totalB.Text=FormatoTotalCategoriaB.ToString();
			totalC.Text=FormatoTotalCategoriaC.ToString();



		}
		
		public void PrepararTabla()
		{
			resultado = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn numero = new DataColumn();
			numero.DataType = System.Type.GetType("System.String");
			numero.ColumnName = "NUMERO";
			numero.ReadOnly=true;
			resultado.Columns.Add(numero);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn placa = new DataColumn();
			placa.DataType = System.Type.GetType("System.String");
			placa.ColumnName = "PLACA";
			placa.ReadOnly=true;
			resultado.Columns.Add(placa);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn fecha = new DataColumn();
			fecha.DataType = System.Type.GetType("System.String");
			fecha.ColumnName = "FECHA";
			fecha.ReadOnly=true;
			resultado.Columns.Add(fecha);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn propietario = new DataColumn();
			propietario.DataType = System.Type.GetType("System.String");
			propietario.ColumnName = "PROPIETARIO";
			propietario.ReadOnly=true;
			resultado.Columns.Add(propietario);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn avaluo = new DataColumn();
			avaluo.DataType = System.Type.GetType("System.String");
			avaluo.ColumnName = "AVALUO";
			avaluo.ReadOnly=true;
			resultado.Columns.Add(avaluo);
						
		}
		public void PrepararTabla1()
		{
			resultado1 = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn numerob = new DataColumn();
			numerob.DataType = System.Type.GetType("System.String");
			numerob.ColumnName = "NUMEROB";
			numerob.ReadOnly=true;
			resultado1.Columns.Add(numerob);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn placab = new DataColumn();
			placab.DataType = System.Type.GetType("System.String");
			placab.ColumnName = "PLACAB";
			placab.ReadOnly=true;
			resultado1.Columns.Add(placab);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn fechab = new DataColumn();
			fechab.DataType = System.Type.GetType("System.String");
			fechab.ColumnName = "FECHAB";
			fechab.ReadOnly=true;
			resultado1.Columns.Add(fechab);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn propietariob = new DataColumn();
			propietariob.DataType = System.Type.GetType("System.String");
			propietariob.ColumnName = "PROPIETARIOB";
			propietariob.ReadOnly=true;
			resultado1.Columns.Add(propietariob);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn avaluob = new DataColumn();
			avaluob.DataType = System.Type.GetType("System.String");
			avaluob.ColumnName = "AVALUOB";
			avaluob.ReadOnly=true;
			resultado1.Columns.Add(avaluob);
						
		}
		public void PrepararTabla2()
		{
			resultado2 = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn numeroc = new DataColumn();
			numeroc.DataType = System.Type.GetType("System.String");
			numeroc.ColumnName = "NUMEROC";
			numeroc.ReadOnly=true;
			resultado2.Columns.Add(numeroc);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn placac = new DataColumn();
			placac.DataType = System.Type.GetType("System.String");
			placac.ColumnName = "PLACAC";
			placac.ReadOnly=true;
			resultado2.Columns.Add(placac);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn fechac = new DataColumn();
			fechac.DataType = System.Type.GetType("System.String");
			fechac.ColumnName = "FECHAC";
			fechac.ReadOnly=true;
			resultado2.Columns.Add(fechac);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn propietarioc = new DataColumn();
			propietarioc.DataType = System.Type.GetType("System.String");
			propietarioc.ColumnName = "PROPIETARIOC";
			propietarioc.ReadOnly=true;
			resultado2.Columns.Add(propietarioc);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn avaluoc = new DataColumn();
			avaluoc.DataType = System.Type.GetType("System.String");
			avaluoc.ColumnName = "AVALUOC";
			avaluoc.ReadOnly=true;
			resultado2.Columns.Add(avaluoc);

		
						
		}
		
		private void bus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string FechaIngreso=Convert.ToDateTime(DBFunctions.SingleData("SELECT FEC_INGRESO from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+bus.SelectedValue.ToString()+"'")).ToString("yyyy-MM-dd");
			string FechaProrrateo=Convert.ToDateTime(fecha.Text).ToString("yyyy-MM-dd");
			string[] fechaMes=FechaIngreso.Split('-');
			string[] fechaMes2=FechaProrrateo.Split('-');
			int MesI=Convert.ToInt32(fechaMes[1]);
			int MesF=Convert.ToInt32(fechaMes2[1]);
			int DiaI=Convert.ToInt32(fechaMes[2]);
			int DiaF=Convert.ToInt32(fechaMes2[2]);
			int AñoI=Convert.ToInt32(fechaMes[0]);
			int AñoF=Convert.ToInt32(fechaMes2[0]);
			int conteo=0;
			int DiasMesI=Convert.ToInt32(DateTime.DaysInMonth(AñoI,MesI));
			int DiasMesF=Convert.ToInt32(DateTime.DaysInMonth(AñoF,MesF));
			
			if(AñoI==AñoF)
			{
				if((MesF-MesI)>2)
				{
				contDias=30;
				}
				if((MesF-MesI)< 2)
				{
					if((DiaF-DiaI)< 0 )
					{
					contDias=((DiaF-DiaI) * (-1));
					}
					if((DiaF-DiaI) > 0)
					{
						contDias=(DiaF-DiaI);
					}
				}
				
			}
			if(AñoF > AñoI)
			{
				if((MesF-MesI) < 0)
				{
					conteo=((MesF-MesI))*(-1);
					if(conteo > 2)
					{
						contDias=30;
					}
					else
					{
						if(((DiaF-1)+ (DiaI + (DiasMesI-DiaI)) ) > 30)
						{
							contDias=30;
						}
					}
					
				}
				else
				{
					contDias=30;
				}
			
			
			}
			if(contDias < 30)
			{
                Utils.MostrarAlerta(Response, "Este Vehiculo No tiene Derecho a Repocision");
			}
			else
				{
					Panel1.Visible=true;
				}
		}

	
		public void Guardar_click(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			
			int mes=Convert.ToInt32(DateTime.Now.Month.ToString());
			int año=Convert.ToInt32(DateTime.Now.Year.ToString());			
			
			double ValorA=Math.Round((double)Session["CuotaA"],2);
			double ValorB=Math.Round((double)Session["CuotaB"],2);
			double ValorC=Math.Round((double)Session["CuotaC"],2);
			


			mes=mes+1;
			if(mes > 12)
			{
			mes=1;
			año=año+1;
			}
			int dia=Convert.ToInt32(DateTime.DaysInMonth(año,mes));

			string fechaCobro=año+"-"+mes+"-"+dia;
			string fechaActual=DateTime.Now.Date.ToString("yyyy-MM-dd");
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MAUXILIOMUTUO VALUES(DEFAULT,DEFAULT,'"+fechaActual.ToString()+"',"+acta.Text.ToString()+","+ValorA+","+ValorB+","+ValorC+",'"+bus.SelectedValue.ToString()+"',"+TextBox1.Text.ToString()+",'"+fechaCobro.ToString()+"') ");
            Utils.MostrarAlerta(Response, "Insercion Satisfactoria");
		
			
			//Label7.Text=DBFunctions.exceptions;
			
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
			this.bus.SelectedIndexChanged += new System.EventHandler(this.bus_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		
	}
}
