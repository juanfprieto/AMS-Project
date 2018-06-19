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
	///		Descripción breve de AMS_Comercial_ConsolidarPlanillas.
	/// </summary>
	public class AMS_Comercial_ConsolidarPlanillas : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox fecha;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList planillas;
		protected System.Web.UI.WebControls.Label label;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label conductor;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label ruta;
		protected System.Web.UI.WebControls.Button Generar;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label tiquetesV;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label totTV;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label tiquetesM;
		protected System.Web.UI.WebControls.Label totTM;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label NumEnco;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label totEn;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label NumAn;
		protected System.Web.UI.WebControls.Label totAN;
		protected System.Web.UI.WebControls.Button Detalle;
		protected System.Web.UI.WebControls.Panel Panel2;
		protected System.Web.UI.WebControls.DataGrid Grid;
		protected System.Web.UI.WebControls.Label bus;
		protected System.Web.UI.WebControls.Button GenerarPlanilla;
		protected System.Web.UI.WebControls.Button Buscar;
		protected System.Web.UI.WebControls.Label Label1;
		public DataSet lineas;
		public DataSet lineas1;
		public DataSet lineas2;
		public DataSet lineas3;
		public DataSet lineas4;
		public DataSet lineas5;
		public DataSet lineas6;
		public DataTable resultado;
		double valor_remesas=0;
		double totales=0;
		double totANT=0;
		double Gtotal=0;
		string GTotalF=null;
		string ValTotAF=null;
		string ValTotF=null;
		//
		double total2=0;
		string placa=null;
		string NumBus=null;
		string busID=null;
		string nit=null;
		string nombre=null;
		string Ruta=null;
		int cantidad=0;
		int z=0;
		protected System.Web.UI.WebControls.Panel Panel3;
		double total=0;
		int contador=0;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox prod;
		protected System.Web.UI.WebControls.TextBox costos;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Panel TOTALPANEL;
		protected System.Web.UI.WebControls.Label Label17;
		double VALORTIQ=0;
		protected System.Web.UI.WebControls.TextBox ultimototal;
		int totalremesas=0;
		
		//

		public void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				
			}	
		}
		public  void  buscar(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			string planillaB=DBFunctions.SingleData("select DRUT_PLANILLA FROM DBXSCHEMA.DRUTA WHERE DRUT_FECHA='"+fecha.Text.ToString()+"' ");
			if(planillaB.Equals("") || planillaB.Equals(null) )
			{
                Utils.MostrarAlerta(Response, "NO Existen Planillas en Esta Fecha");

			}
			else
			{
				if(planillas.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(planillas,"select DRUT_PLANILLA FROM DBXSCHEMA.DRUTA WHERE DRUT_FECHA='"+fecha.Text.ToString()+"' ");
					ListItem it=new ListItem("-Planilla-","0");
					planillas.Items.Insert(0,it);
				}
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
			this.planillas.SelectedIndexChanged += new System.EventHandler(this.planillas_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public void planillas_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			placa=DBFunctions.SingleData("select MCAT_PLACA from DBXSCHEMA.DRUTA where DRUT_PLANILLA="+planillas.SelectedValue.ToString()+" AND DRUT_FECHA='"+fecha.Text.ToString()+"'");
			NumBus=DBFunctions.SingleData("select MBUS_NUMERO from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"'");
			busID= placa+"-"+NumBus;
			bus.Text=busID.ToString();
			nit=DBFunctions.SingleData("select MNIT_NIT from DBXSCHEMA.DRUTA  where DRUT_PLANILLA="+planillas.SelectedValue.ToString()+" AND DRUT_FECHA='"+fecha.Text.ToString()+"'");
			nombre=DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT='"+nit+"'");
			conductor.Text=nombre.ToString();
			Ruta=DBFunctions.SingleData("select DRUT_DESC FROM DBXSCHEMA.DRUTA where DRUT_PLANILLA="+planillas.SelectedValue.ToString()+" AND DRUT_FECHA='"+fecha.Text.ToString()+"'");
			ruta.Text=Ruta.ToString();

		}
		public  void generar(Object  Sender, EventArgs e)
		{
			//TIQUETES MANUALES
			lineas = new DataSet();
			lineas2 = new DataSet();
			lineas3 = new DataSet();
			lineas5 = new DataSet();
			lineas6 = new DataSet();
			Panel3.Visible=true;		
			DBFunctions.Request(lineas,IncludeSchema.NO,"select CANTIDAD,VALOR_VENTA from DBXSCHEMA.MPLANILLA_VIAJE WHERE NUM_PLANILLA="+planillas.SelectedValue.ToString()+" ");
			//                                                      0         1
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
			int cant=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[0]);
			double valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[1]);
			total=(cant*valor);
			cantidad=cant+cantidad;
			total2=total2+total;
			}
			
			tiquetesM.Text=cantidad.ToString();
			totTM.Text=total2.ToString();
			//REMESAS MANUALES
			DBFunctions.Request(lineas2,IncludeSchema.NO,"Select count(*) AS CONTEO,sum(VALOR_REMESA) AS VALOR from DBXSCHEMA.MREMESA_MANUAL  where  PLANILLA="+planillas.SelectedValue.ToString()+" ");
			//REMESAS AUTOMATICAS
			DBFunctions.Request(lineas6,IncludeSchema.NO,"select COUNT(*),SUM(VALO_FLET) from DBXSCHEMA.MREMESA WHERE MREM_PLANILLA="+planillas.SelectedValue.ToString()+" ");
			totalremesas=(Convert.ToInt32(lineas2.Tables[0].Rows[0].ItemArray[0])+Convert.ToInt32(lineas6.Tables[0].Rows[0].ItemArray[0]));
			valor_remesas=(Convert.ToDouble(lineas2.Tables[0].Rows[0].ItemArray[1])+Convert.ToDouble(lineas6.Tables[0].Rows[0].ItemArray[1]));
			//
			NumEnco.Text=totalremesas.ToString();
			totEn.Text=valor_remesas.ToString();
			
			
			//ANTICIPOS 
			DBFunctions.Request(lineas3,IncludeSchema.NO,"SELECT COUNT(*) AS CONTEO,SUM(MAN_VALOTOTAL) AS VALOR from DBXSCHEMA.MANTICIPO WHERE MAN_FECHA='"+fecha.Text.ToString()+"' AND DRUT_PLANILLA="+planillas.SelectedValue.ToString()+"");
			//                                                      0         1
			NumAn.Text=lineas3.Tables[0].Rows[z].ItemArray[0].ToString();
			totAN.Text=lineas3.Tables[0].Rows[z].ItemArray[1].ToString();
			//TIQUETES AUTOMATICOS
			DBFunctions.Request(lineas5,IncludeSchema.NO,"SELECT DRUT_CODIGO AS RUTA from DBXSCHEMA.DTIQUETE WHERE DRUT_PLANILLA="+planillas.SelectedValue.ToString()+" AND DTIQ_FECHA='"+fecha.Text.ToString()+"'");
			for(int y=0;y<lineas5.Tables[0].Rows.Count;y++)
			{
			
				contador++;
				string rutaM=DBFunctions.SingleData("select MRUT_CODIGO FROM DBXSCHEMA.DRUTA WHERE DRUT_CODIGO="+lineas5.Tables[0].Rows[y].ItemArray[0]+"");
				string valorTIQ=DBFunctions.SingleData("select MRUT_VALOR from DBXSCHEMA.MRUTA WHERE MRUT_CODIGO="+rutaM+"");
				
				VALORTIQ=Convert.ToDouble(valorTIQ)+VALORTIQ;

			}
			tiquetesV.Text=contador.ToString();
			totTV.Text=VALORTIQ.ToString();
			//totales
			TOTALPANEL.Visible=true;
			totales=(VALORTIQ+valor_remesas+total2);
			totANT=Convert.ToDouble(lineas3.Tables[0].Rows[z].ItemArray[1]);
			Gtotal=totales-totANT;
			GTotalF=String.Format("{0:C}",Gtotal);
			ValTotAF=String.Format("{0:C}",totANT);
			ValTotF=String.Format("{0:C}",totales);
			prod.Text=ValTotF.ToString();
			costos.Text=ValTotAF.ToString();
			ultimototal.Text=GTotalF.ToString();
			

			
			//

		}
		public  void Guardar_Click(Object  Sender, EventArgs e)
		{
		lineas6 = new DataSet();
		lineas2 = new DataSet();
		lineas = new DataSet();
		lineas5 = new DataSet();
		DatasToControls bind = new DatasToControls();
		//
			DBFunctions.Request(lineas,IncludeSchema.NO,"select CANTIDAD,VALOR_VENTA from DBXSCHEMA.MPLANILLA_VIAJE WHERE NUM_PLANILLA="+planillas.SelectedValue.ToString()+" ");
			//                                                      0         1
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				int cant=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[0]);
				double valor=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[1]);
				total=(cant*valor);
				cantidad=cant+cantidad;
				total2=total2+total;
			}
			//

			//
			DBFunctions.Request(lineas5,IncludeSchema.NO,"SELECT DRUT_CODIGO AS RUTA from DBXSCHEMA.DTIQUETE WHERE DRUT_PLANILLA="+planillas.SelectedValue.ToString()+" AND DTIQ_FECHA='"+fecha.Text.ToString()+"'");
			for(int y=0;y<lineas5.Tables[0].Rows.Count;y++)
			{
			
				contador++;
				string rutaM=DBFunctions.SingleData("select MRUT_CODIGO FROM DBXSCHEMA.DRUTA WHERE DRUT_CODIGO="+lineas5.Tables[0].Rows[y].ItemArray[0]+"");
				string valorTIQ=DBFunctions.SingleData("select MRUT_VALOR from DBXSCHEMA.MRUTA WHERE MRUT_CODIGO="+rutaM+"");
				
				VALORTIQ=Convert.ToDouble(valorTIQ)+VALORTIQ;

			}
			//
		DBFunctions.Request(lineas2,IncludeSchema.NO,"Select count(*) AS CONTEO,sum(VALOR_REMESA) AS VALOR from DBXSCHEMA.MREMESA_MANUAL  where  PLANILLA="+planillas.SelectedValue.ToString()+" ");
		DBFunctions.Request(lineas6,IncludeSchema.NO,"select COUNT(*),SUM(VALO_FLET) from DBXSCHEMA.MREMESA WHERE MREM_PLANILLA="+planillas.SelectedValue.ToString()+" ");
		totalremesas=(Convert.ToInt32(lineas2.Tables[0].Rows[0].ItemArray[0])+Convert.ToInt32(lineas6.Tables[0].Rows[0].ItemArray[0]));
		valor_remesas=(Convert.ToDouble(lineas2.Tables[0].Rows[0].ItemArray[1])+Convert.ToDouble(lineas6.Tables[0].Rows[0].ItemArray[1]));
		//
		totales=0;
		totales=VALORTIQ+total2;
		double tiquetesTotal=totales;
		double  GranTotal=totales+valor_remesas;

		int cod_ruta=Convert.ToInt32(DBFunctions.SingleData("Select MRUT_CODIGO from DBXSCHEMA.DRUTA WHERE DRUT_PLANILLA="+planillas.SelectedValue.ToString()+""));
		
		placa=DBFunctions.SingleData("select MCAT_PLACA from DBXSCHEMA.DRUTA where DRUT_PLANILLA="+planillas.SelectedValue.ToString()+" AND DRUT_FECHA='"+fecha.Text.ToString()+"'");
		NumBus=DBFunctions.SingleData("select MBUS_NUMERO from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"'");
		
		string fechaU=DateTime.Now.Date.ToString("yyyy-MM-dd");
		DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MPLANILLA_PRELIMINAR VALUES(DEFAULT,DEFAULT,"+planillas.SelectedValue.ToString()+","+cod_ruta+",'"+fechaU.ToString()+"','"+placa.ToString()+"',"+NumBus+","+tiquetesTotal+","+valor_remesas+","+GranTotal+")");
		Label17.Text=DBFunctions.exceptions;  
		}
		public  void Detalle_OnClick(Object  Sender, EventArgs e)
		{
			
			this.PrepararTabla();
			lineas4 = new DataSet();
			DBFunctions.Request(lineas4,IncludeSchema.NO,"select PREF_DOCU CONCAT''CONCAT CAST(MAN_CODIGO AS CHAR(10)) AS CODIGO,COD_ANTICIPO,MAN_VALOTOTAL FROM DBXSCHEMA.MANTICIPO WHERE MAN_FECHA='"+fecha.Text.ToString()+"' AND DRUT_PLANILLA="+planillas.SelectedValue.ToString()+"");
			//                                                                              0                                          1            2            
			for(int x=0;x<lineas4.Tables[0].Rows.Count;x++)
			{
				string descripcion=null;
				int codigoanticipo=Convert.ToInt32(lineas4.Tables[0].Rows[x].ItemArray[1].ToString());
				descripcion=DBFunctions.SingleData("select TTIPA_DESCRIPCION from DBXSCHEMA.TTIOPOANTICIPO WHERE TTIPA_CODIGO="+codigoanticipo+"");
				
				DataRow fila;
				fila= resultado.NewRow();
				double valor=0;
				string ValorFormato=null;
				valor=Convert.ToDouble(lineas4.Tables[0].Rows[x].ItemArray[2].ToString());
				ValorFormato=String.Format("{0:C}",valor);
				fila["CODIGO"]	= lineas4.Tables[0].Rows[x].ItemArray[0].ToString();
				fila["DESCRIPCION"]	= descripcion.ToString();
				fila["VALOR"]	= ValorFormato.ToString();
				resultado.Rows.Add(fila);
			}
			
			//fin sentencia FOR	
			Grid.DataSource = resultado;
			Grid.DataBind();	
			
			
		}
		public void PrepararTabla()
		{
			resultado = new DataTable();
			
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
	}

}
