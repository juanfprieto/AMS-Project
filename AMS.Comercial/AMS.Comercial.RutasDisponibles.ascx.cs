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

	/// <summary>
	///		Descripción breve de AMS_Comercial_RutasDisponibles.
	/// </summary>
	public class AMS_Comercial_RutasDisponibles : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Button Buscar;
		protected DataSet lineas;
		protected DataTable resultado;
		protected System.Web.UI.WebControls.TextBox fecha;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.DataGrid Grid;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Image Image3;
		protected System.Web.UI.WebControls.Image Image2;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.Panel Panel2;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label FechaProceso;
		protected System.Web.UI.WebControls.Label horalabel;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label hora1;
	
		protected System.Web.UI.WebControls.Label RemeasLabel;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");	
				Panel1.Visible=false;
			}
			
		}
		protected  void  generar(Object  Sender, EventArgs e)
		{
			
			this.PrepararTabla();
			lineas = new DataSet();
			string NomImagen=null;
			//int CM=0;
			string NomImagen2=null;
			string url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"];
			string url2=System.Configuration.ConfigurationManager.AppSettings["PathToImages"];
			DBFunctions.Request(lineas,IncludeSchema.NO,"select DRUT_DESC,DRUT_HORASAL,MCAT_PLACA,DRUT_DURACION,DRUT_TIPORUTA from DBXSCHEMA.DRUTA WHERE DRUT_FECHA='"+fecha.Text.ToString()+"' ORDER BY DRUT_HORASAL");
			//                                                       0         1           2            3              4          
			if(lineas.Tables[0].Rows.Count==0)
			{
				Panel1.Visible=false;
				Response.Write("<script language='javascript'>alert('No hay Buses Programados');</script>");
				
				Grid.Visible=false;
				Grid.Height=0;
				Grid.Width=0;
				Panel2.Visible=true;
				horalabel.Text=(DateTime.Now.Hour.ToString()+":"+DateTime.Now.Minute.ToString()+":"+DateTime.Now.Second.ToString());
				FechaProceso.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
			}
			else
			{
				//fecha1.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				hora1.Text=(DateTime.Now.Hour.ToString()+":"+DateTime.Now.Minute.ToString()+":"+DateTime.Now.Second.ToString());
				Panel1.Visible=true;
				Grid.Visible=true;
				Panel2.Visible=false;
				
				for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
				{
				
					int NumeroBus=Convert.ToInt32(DBFunctions.SingleData("select MBUS_NUMERO from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[2].ToString()+"'"));
					string hora=lineas.Tables[0].Rows[i].ItemArray[1].ToString();
					string []HoraSalida=hora.Split(':');
					int Hora=Convert.ToInt32(HoraSalida[0]);
					int Minutos=Convert.ToInt32(HoraSalida[1]);
				
					
					System.DateTime HORA1=new System.DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);
					System.DateTime HORA2= new System.DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,Convert.ToInt32(HoraSalida[0].ToString()),Convert.ToInt32(HoraSalida[1].ToString()),Convert.ToInt32(HoraSalida[2]));
					System.TimeSpan diff1 = HORA2.Subtract(HORA1);
					
					int HorasD=Convert.ToInt32(diff1.Hours.ToString());
					int MinutosD=Convert.ToInt32(diff1.Minutes.ToString());
					
					if(HorasD < 1 )
					{
					NomImagen="busEstado1.jpg";
					}
					if(HorasD >= 1)
					{
					NomImagen="busEstado3.jpg";
					}
					if(HorasD==0 && (MinutosD >= 0 && MinutosD <= 30 ) )
					{
						NomImagen="busEstado2.jpg";
					}
					if(HorasD==0 && (MinutosD >= 30 && MinutosD <= 59 ) )
					{
						NomImagen="busEstado3.jpg";
					}					
					url=url+NomImagen.ToString();
					/////////////////
					string lineas1=DBFunctions.SingleData("SELECT COUNT(*) from DBXSCHEMA.DTIQUETE WHERE DTIQ_FECHA='"+fecha.Text.ToString()+"' AND  MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[2].ToString()+"' AND DTIQ_HORA='"+lineas.Tables[0].Rows[i].ItemArray[1].ToString()+"'");
					string lineas2=DBFunctions.SingleData("Select count(*)from dbxschema.dtiquete where DTIQ_FECHA='"+fecha.Text.ToString()+"' AND  MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[2].ToString()+"' AND DTIQ_HORA='"+lineas.Tables[0].Rows[i].ItemArray[1].ToString()+"' and TEST_CODIGO='RS'");
					int puestosre=0;
					int puestos=0;
					if(lineas2.Length==0)
					{
						puestosre=0;
						puestos=Convert.ToInt32(lineas1);
					}
					else
					{
						puestos=Convert.ToInt32(lineas1);
						puestosre=Convert.ToInt32(lineas2);				
					}
					if(lineas.Tables[0].Rows[i].ItemArray[4].Equals("C"))
					{
						NomImagen2="corriente.jpg";
					}
					
					else
					{
						NomImagen2="directo.jpg";
					}
					url2=url2+NomImagen2.ToString();
					puestos=puestos-puestosre;
					int Capacidad=Convert.ToInt32(DBFunctions.SingleData("select count(*) from DBXSCHEMA.MELEMENTOBUS where MCAT_PLACA='"+lineas.Tables[0].Rows[i].ItemArray[2].ToString()+"' and tele_codigo='SC'"));
					puestos=Capacidad-puestos;
					//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
					DataRow fila;
				
					fila= resultado.NewRow();
					fila["FECHA"]	=fecha.Text.ToString();
					fila["PLACA"]	= lineas.Tables[0].Rows[i].ItemArray[2].ToString();
					fila["NUMERO"]	= NumeroBus.ToString();
					fila["HORASAL"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
					fila["RUTA"]	= lineas.Tables[0].Rows[i].ItemArray[0].ToString();
					fila["IMAGEN"] = url.ToString();
					fila["PUESTOS"]	= puestos.ToString();
					fila["TIPO"]	= url2.ToString();
					NomImagen=null;
					NomImagen2=null;
					url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"];
					url2=System.Configuration.ConfigurationManager.AppSettings["PathToImages"];
					resultado.Rows.Add(fila);
						

				}
			
				//fin sentencia FOR	
				Grid.DataSource = resultado;
				Grid.DataBind();	
			
			}
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
			DataColumn numero = new DataColumn();
			numero.DataType = System.Type.GetType("System.String");
			numero.ColumnName = "NUMERO";
			numero.ReadOnly=true;
			resultado.Columns.Add(numero);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn horasal = new DataColumn();
			horasal.DataType = System.Type.GetType("System.String");
			horasal.ColumnName = "HORASAL";
			horasal.ReadOnly=true;
			resultado.Columns.Add(horasal);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn ruta = new DataColumn();
			ruta.DataType = System.Type.GetType("System.String");
			ruta.ColumnName = "RUTA";
			ruta.ReadOnly=true;
			resultado.Columns.Add(ruta);
			/////////////////////////////////////
			DataColumn imagen = new DataColumn();
			imagen.DataType = System.Type.GetType("System.String");
			imagen.ColumnName = "IMAGEN";
			imagen.ReadOnly=true;
			resultado.Columns.Add(imagen);
			/////////////////////////////////////
			DataColumn puestos = new DataColumn();
			puestos.DataType = System.Type.GetType("System.String");
			puestos.ColumnName = "PUESTOS";
			puestos.ReadOnly=true;
			resultado.Columns.Add(puestos);
			/////////////////////////////////////
			DataColumn tipo = new DataColumn();
			tipo.DataType = System.Type.GetType("System.String");
			tipo.ColumnName = "TIPO";
			tipo.ReadOnly=true;
			resultado.Columns.Add(tipo);

			
			
			
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
			this.Grid.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.Grid_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Grid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.AlternatingItem  || e.Item.ItemType==ListItemType.Item)
			{
				((System.Web.UI.WebControls.Image)e.Item.Cells[3].FindControl("IMAGEN")).ImageUrl=resultado.Rows[e.Item.DataSetIndex].ItemArray[5].ToString();
				((System.Web.UI.WebControls.Image)e.Item.Cells[3].FindControl("IMAGEN")).Height=30;
				((System.Web.UI.WebControls.Image)e.Item.Cells[3].FindControl("IMAGEN")).Width=90;
				///////////////////////////////////////////////
				((System.Web.UI.WebControls.Image)e.Item.Cells[3].FindControl("TIPO")).ImageUrl=resultado.Rows[e.Item.DataSetIndex].ItemArray[7].ToString();
				((System.Web.UI.WebControls.Image)e.Item.Cells[3].FindControl("TIPO")).Height=30;
				((System.Web.UI.WebControls.Image)e.Item.Cells[3].FindControl("TIPO")).Width=90;
				
			
			}
		}

		
	}
}
