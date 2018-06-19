// created on 25/11/2004 at 17:12
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Mail;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Marketing
{
	public partial class ReporteSeguimientoOrdenes : System.Web.UI.UserControl
	{
		protected DataGrid []dtgs;
		protected DataSet tablasAsociadas;
		protected ArrayList codigoResultado, cantidadOrdenes, codigosResultadoTotales, cantidadOrdenesTotales;
		protected double cantidadCitas;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(tipoAcciones,"SELECT pmar_codimark , pmar_nombmark FROM pmarketing");
				fechaInicial.SelectedDate = DateTime.Now;
				fechaFinal.SelectedDate = DateTime.Now;
			}
		}		
		
		protected void Generar_Reporte(Object  Sender, EventArgs e)
		{
			resultadoReporte.Controls.Clear();
			//Primero debemos comprobar que las fechas cumplan la condicion
			if(fechaFinal.SelectedDate.Date>fechaInicial.SelectedDate.Date)
			{
				//Construimos los ArrayList que van a contener los totales de los resultados
				codigosResultadoTotales = new ArrayList();
				cantidadOrdenesTotales = new ArrayList();
				this.Construir_Contadores_Resultados(codigosResultadoTotales,cantidadOrdenesTotales);
				//Fin de construccion
				int i,controlBucle=0,cantidadGrillas=0;
				//Luego determinamos cuales son las actividades
				DataSet actividadesMarketing = new DataSet();
				DBFunctions.Request(actividadesMarketing,IncludeSchema.NO,"SELECT pact_codimark, pact_nombmark FROM pactividadmarketing WHERE pmar_codimark='"+DBFunctions.SingleData("SELECT pmar_codimark FROM pmarketing WHERE pmar_nombmark='"+tipoAcciones.SelectedItem.ToString()+"'")+"'");
				//Ahora traemos todas las ordenes de trabajo que se encuentran dentro de las fechas seleccionadas
				DataSet ordenesSeleccionadas = new DataSet();
				DBFunctions.Request(ordenesSeleccionadas,IncludeSchema.NO,"SELECT pdoc_codigo, mord_numeorde , mnit_nit FROM morden WHERE mord_entrada>='"+fechaInicial.SelectedDate.ToString("yyyy-MM-dd")+"' AND  mord_entrada<='"+fechaFinal.SelectedDate.ToString("yyyy-MM-dd")+"'");
				cantidadCitas = ordenesSeleccionadas.Tables[0].Rows.Count;
				//Ahora por cada actividad vamos a generar las consultas correspondientes
				string opcion = tipoOrden.SelectedValue.ToString();
				if(opcion=="orden")
					controlBucle = ordenesSeleccionadas.Tables[0].Rows.Count;
				else if(opcion=="actividad")
					controlBucle = actividadesMarketing.Tables[0].Rows.Count;
				dtgs = new DataGrid[controlBucle];
				tablasAsociadas = new DataSet();
				//Iniciamos la creacion de los DataGrids dentro del Bucle de acuerdo a la opcion seleccionada por el usuario
				for(i=0;i<controlBucle;i++)
				{
					Label subtitulo = new Label();
					DataTable subtablaAsociada = new DataTable();
					DataGrid grillaInformativa = new DataGrid();
					Label resultado = new Label();
					bool mostrar = true;
					if(opcion=="orden")
					{
						this.Contruir_Label_Opcion1(subtitulo,ordenesSeleccionadas.Tables[0].Rows[i][0].ToString(),ordenesSeleccionadas.Tables[0].Rows[i][1].ToString());
						mostrar = this.Llenar_Tabla_Opcion1(grillaInformativa,subtablaAsociada,ordenesSeleccionadas.Tables[0].Rows[i][0].ToString(), ordenesSeleccionadas.Tables[0].Rows[i][1].ToString());						
					}						
					else if(opcion=="actividad")
					{
						this.Contruir_Label_Opcion2(subtitulo,actividadesMarketing.Tables[0].Rows[i][0].ToString());
						mostrar = this.Llenar_Tabla_Opcion2(grillaInformativa,subtablaAsociada,actividadesMarketing.Tables[0].Rows[i][0].ToString());
						this.Construir_Label_Resultados(resultado,codigoResultado,cantidadOrdenes);
					}
					if(mostrar)	
					{
                        resultadoReporte.Controls.Add(new LiteralControl("<br><br>"));
                        resultadoReporte.Controls.Add(subtitulo);
                        resultadoReporte.Controls.Add(grillaInformativa);
                        resultadoReporte.Controls.Add(resultado);

                        dtgs[cantidadGrillas] = grillaInformativa;
                        cantidadGrillas += 1;
                        DatasToControls.Aplicar_Formato_Grilla(grillaInformativa);
					}
				}
				//Aqui vamos a crear un label que nos muestre los totales 
				Label totales = new Label();
				resultadoReporte.Controls.Add(totales);
				Construir_Label_Total(totales,codigosResultadoTotales,cantidadOrdenesTotales);
				toolsHolder.Visible = true;
				Session["Rep"] = this.Html_Writer(resultadoReporte);
			}
			else
            Utils.MostrarAlerta(Response, "Fechas Invalidas. Por Favor Verifique los calendarios");
		}
		
		protected void SendMail(Object Sender, ImageClickEventArgs E)
		{
			MailMessage MyMail = new MailMessage();
   		  	MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
     		MyMail.To = tbEmail.Text;
			MyMail.Subject = "ORDEN DE TRABAJO";
			MyMail.Body = ((string)Session["Rep"]);
      		MyMail.BodyFormat = MailFormat.Html;
			try{
    	   		SmtpMail.Send(MyMail);}
    		catch(Exception e){
    	 	  lb.Text = e.ToString();
    		}    		
		}
		
		protected string Html_Writer(PlaceHolder reporte)
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			reporte.RenderControl(htmlTW);			
			string reporteString = SB.ToString();
			return reporteString;
		}
		
		protected DataTable Preparacion_Tabla_Opcion1(DataTable tablaNueva)
		{
			tablaNueva = new DataTable();
			tablaNueva.Columns.Add(new DataColumn("CODIGO ACCION",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("DESCRIPCION ACCION",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("RESULTADO ACCION",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("DETALLE RESULTADO",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("FECHA ACCION",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("EJECUTOR ACCION",System.Type.GetType("System.String")));
			return tablaNueva;
		}	
		
		protected bool Llenar_Tabla_Opcion1(DataGrid grilla ,DataTable tablaNueva, string prefijoOrden, string numeroOrden)
		{
			bool cantidad = true;
			tablaNueva = this.Preparacion_Tabla_Opcion1(tablaNueva);
			DataSet dordenactividad = new DataSet();
			DBFunctions.Request(dordenactividad,IncludeSchema.NO,"SELECT pact_codimark,dord_detalle,dord_fecha,dord_ejecutor, pres_codigo FROM dordenactividad WHERE pdoc_codigo='"+prefijoOrden+"' AND mord_numeorde="+numeroOrden+" ORDER BY pres_codigo");
			if(dordenactividad.Tables[0].Rows.Count==0)
				cantidad = false;
			for(int i=0;i<dordenactividad.Tables[0].Rows.Count;i++)
			{
				if(DBFunctions.RecordExist("SELECT * FROM pactividadmarketing WHERE pact_codimark='"+dordenactividad.Tables[0].Rows[0][0].ToString()+"' AND pmar_codimark='"+DBFunctions.SingleData("SELECT pmar_codimark FROM pmarketing WHERE pmar_nombmark='"+tipoAcciones.SelectedItem.ToString()+"'")+"'"))
				{
					DataRow fila = tablaNueva.NewRow();
					fila["CODIGO ACCION"] = dordenactividad.Tables[0].Rows[i][0].ToString();
					fila["DESCRIPCION ACCION"] = DBFunctions.SingleData("SELECT pact_nombmark FROM pactividadmarketing WHERE pact_codimark='"+dordenactividad.Tables[0].Rows[i][0].ToString()+"'");
					fila["RESULTADO ACCION"] = DBFunctions.SingleData("SELECT pres_descripcion FROM presultadoactividad WHERE pres_codigo='"+dordenactividad.Tables[0].Rows[i][4].ToString()+"'");
					this.Adicionar_Orden_Contador(codigosResultadoTotales,cantidadOrdenesTotales,dordenactividad.Tables[0].Rows[i][4].ToString());
					fila["DETALLE RESULTADO"] = dordenactividad.Tables[0].Rows[i][1].ToString();
					fila["FECHA ACCION"] = (System.Convert.ToDateTime(dordenactividad.Tables[0].Rows[i][2].ToString())).ToString("yyyy-MM-dd");
					fila["EJECUTOR ACCION"] = dordenactividad.Tables[0].Rows[i][3].ToString();
					tablaNueva.Rows.Add(fila);
				}								
			}
			grilla.DataSource = tablaNueva;
			grilla.DataBind();
			return cantidad;
		}
		
		protected DataTable Preparacion_Tabla_Opcion2(DataTable tablaNueva)
		{
			tablaNueva = new DataTable();
			tablaNueva.Columns.Add(new DataColumn("PREFIJO ORDEN",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("NUMERO ORDEN",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("PROPIETARIO",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("RESULTADO ACCION",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("DESCRIPCION RESULTADO",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("FECHA ACCION",System.Type.GetType("System.String")));
			tablaNueva.Columns.Add(new DataColumn("EJECUTOR ACCION",System.Type.GetType("System.String")));
			return tablaNueva;
		}
		
		protected bool Llenar_Tabla_Opcion2(DataGrid grilla ,DataTable tablaNueva, string codigoActividad)
		{
			codigoResultado = new ArrayList();
			cantidadOrdenes = new ArrayList();
			this.Construir_Contadores_Resultados(codigoResultado,cantidadOrdenes);
			bool cantidad = true;
			tablaNueva = this.Preparacion_Tabla_Opcion2(tablaNueva);
			DataSet dordenactividad = new DataSet();
			DBFunctions.Request(dordenactividad,IncludeSchema.NO,"SELECT pdoc_codigo, mord_numeorde, dord_detalle, dord_fecha, dord_ejecutor, pres_codigo FROM dordenactividad WHERE pact_codimark='"+codigoActividad+"' ORDER BY pres_codigo");
			if(dordenactividad.Tables[0].Rows.Count==0)
				cantidad = false;
			for(int i=0;i<dordenactividad.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaNueva.NewRow();
				fila["PREFIJO ORDEN"] = dordenactividad.Tables[0].Rows[i][0].ToString();
				fila["NUMERO ORDEN"] = dordenactividad.Tables[0].Rows[i][1].ToString();
				fila["PROPIETARIO"] = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit=(SELECT mnit_nit FROM morden WHERE pdoc_codigo='"+dordenactividad.Tables[0].Rows[0][0].ToString()+"' AND mord_numeorde="+dordenactividad.Tables[0].Rows[i][1].ToString()+")");
				fila["RESULTADO ACCION"] = DBFunctions.SingleData("SELECT pres_descripcion FROM presultadoactividad WHERE pres_codigo='"+dordenactividad.Tables[0].Rows[i][5].ToString()+"'");
				this.Adicionar_Orden_Contador(codigoResultado,cantidadOrdenes,dordenactividad.Tables[0].Rows[i][5].ToString());
				this.Adicionar_Orden_Contador(codigosResultadoTotales,cantidadOrdenesTotales,dordenactividad.Tables[0].Rows[i][5].ToString());
				fila["DESCRIPCION RESULTADO"] = dordenactividad.Tables[0].Rows[i][2].ToString();
				fila["FECHA ACCION"] = (System.Convert.ToDateTime(dordenactividad.Tables[0].Rows[i][3].ToString())).ToString("yyyy-MM-dd");
				fila["EJECUTOR ACCION"] = dordenactividad.Tables[0].Rows[i][4].ToString();
				tablaNueva.Rows.Add(fila);
			}
			grilla.DataSource = tablaNueva;
			grilla.DataBind();
			return cantidad;
		}
		
		protected void Contruir_Label_Opcion1(Label labelOpcion,string prefijoOrden, string numeroOrden)
		{
			labelOpcion.Text = 
                "Tipo de Documento : "+DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='"+prefijoOrden+"'")+" Numero Orden :"+numeroOrden+" Propietario "+DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit=(SELECT mnit_nit FROM morden WHERE pdoc_codigo='"+prefijoOrden+"' AND mord_numeorde="+numeroOrden+")");
		}
		
		protected void Contruir_Label_Opcion2(Label labelOpcion, string codigoActividad)
		{
			labelOpcion.Text = "Codigo de la Actividad : "+codigoActividad+" Nombre de la Actividad : "+DBFunctions.SingleData("SELECT pact_nombmark FROM pactividadmarketing WHERE pact_codimark='"+codigoActividad+"'");
		}	
		
		protected void Construir_Contadores_Resultados(ArrayList codigoResultado, ArrayList cantidadOrdenes)
		{
			DataSet resultados = new DataSet();
			DBFunctions.Request(resultados,IncludeSchema.NO,"SELECT pres_codigo FROM presultadoactividad");
			for(int i=0;i<resultados.Tables[0].Rows.Count;i++)
			{
				codigoResultado.Add(resultados.Tables[0].Rows[i][0].ToString());
				cantidadOrdenes.Add(0);
			}
		}
		
		protected void Adicionar_Orden_Contador(ArrayList codigoResultado, ArrayList cantidadOrdenes, string codigoResultadoAdicion)
		{
			//Primero debemos localizar donde se encuentra este codigo dentro de primer ArrayList
			int indice = this.Retornar_Ubicacion_Codigo(codigoResultado,codigoResultadoAdicion);
			if(indice>codigoResultado.Count)
				lb.Text += "<br>error no found";
			else
			{
				int cantidadAnterior = System.Convert.ToInt32(cantidadOrdenes[indice].ToString());
				cantidadAnterior += 1;
				cantidadOrdenes[indice] = cantidadAnterior;
			}
		}
		
		protected int Retornar_Ubicacion_Codigo(ArrayList codigoResultado , string codigoResultadoAdicion)
		{
			bool encontrado = false;
			int indice  = 0;
			for(int i=0;i<codigoResultado.Count;i++)
			{
				if(codigoResultado[i].ToString()==codigoResultadoAdicion)
				{
					encontrado = true;
					indice = i;
				}
			}
			if(!encontrado)
				indice = codigoResultado.Count + 1;
			return indice;
		}
		
		protected void Construir_Label_Resultados(Label resultados, ArrayList codigoResultado, ArrayList cantidadOrdenes)
		{
			resultados.Text = "Ha continuacion se relacionan la cantidad de ordenes de trabajo con esta actividad";
			for(int i=0;i<codigoResultado.Count;i++)
			{
				if(System.Convert.ToInt32(cantidadOrdenes[i].ToString())>0)
				{
					resultados.Text += "<br>El resultado "+DBFunctions.SingleData("SELECT pres_descripcion FROM presultadoactividad WHERE pres_codigo='"+codigoResultado[i].ToString()+"'")+" Se ha presentado en "+cantidadOrdenes[i].ToString()+" ordenes de trabajo";
					resultados.Text += " ,que corresponde al "+((double)(System.Convert.ToInt32(cantidadOrdenes[i].ToString())/cantidadCitas)*100).ToString()+"% de las ordenes que se encuentran entre las fechas seleccionadas";
				}
			}
		}
		
		protected void Construir_Label_Total(Label resultados, ArrayList codigoResultado, ArrayList cantidadOrdenes)
		{
			resultados.Text = "<br>TOTALES : ";
			for(int i=0;i<codigoResultado.Count;i++)
			{
				if(System.Convert.ToInt32(cantidadOrdenes[i].ToString())>0)
				{
					resultados.Text +="<br>Resultado de Actividad "+DBFunctions.SingleData("SELECT pres_descripcion FROM presultadoactividad WHERE pres_codigo='"+codigoResultado[i].ToString()+"'")+"";
					resultados.Text +="<br>Total Ordenes en que Aparece :"+cantidadOrdenes[i].ToString()+" que corresponde al "+((double)(System.Convert.ToInt32(cantidadOrdenes[i].ToString())/cantidadCitas)*100).ToString()+"% de las ordenes que se encuentran entre las fechas seleccionadas<br>";
				}
			}
		}
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    

		}
		#endregion

	}
}
