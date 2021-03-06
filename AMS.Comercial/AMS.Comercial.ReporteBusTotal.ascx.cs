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
	using AMS.DBManager;
	using AMS.Reportes;
	using Ajax;

	/// <summary>
	///		Descripci�n breve de AMS_Comercial_ReporteBusTotal.
	/// </summary>
	public class AMS_Comercial_ReporteBusTotal : System.Web.UI.UserControl
	{
		
		protected DataSet lineas;
		protected DataTable resultado;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.TextBox tbEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.ImageButton ibMail;
		protected System.Web.UI.WebControls.PlaceHolder toolsHolder;
		protected System.Web.UI.WebControls.Table tabPreHeader;
		protected System.Web.UI.WebControls.DataGrid Grid;
		protected System.Web.UI.WebControls.Table tabFirmas;
		protected System.Web.UI.WebControls.Label lb;
		protected System.Web.UI.WebControls.DropDownList ddlagencia;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.TextBox FechaInicio;
		protected System.Web.UI.WebControls.TextBox FechaFinal;
		protected string reportTitle="Planilla De Reporte Total Bus";
		private void Page_Load(object sender, System.EventArgs e)
		{
			
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
			if(!Page.IsPostBack)
			{
				FechaInicio.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				FechaFinal.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlagencia,"select MOFI_CODIGO,MOFI_DESCRIPCION from DBXSCHEMA.MOFICINA");
				
			}
		}
		protected  void  generar(Object  Sender, EventArgs e)
		{
			string []pr=new string[2];
			pr[0]=pr[1]="";
			Press frontEnd = new Press(new DataSet(),reportTitle);
			frontEnd.PreHeader(tabPreHeader, Grid.Width, pr);
			frontEnd.Firmas(tabFirmas,Grid.Width);
			lb.Text="";
			

			///////////////////////////////////////////////////////
			this.PrepararTabla();
			lineas = new DataSet();
			string fecha=Convert.ToDateTime(FechaInicio.Text).ToString("yyyy-MM-dd");
			string fecha1=Convert.ToDateTime(FechaFinal.Text).ToString("yyyy-MM-dd");
			
			try
			{
				DBFunctions.Request(lineas,IncludeSchema.NO,"select distinct mbus.mbus_numero as NUMEROBUS,drut.mcat_placa as PLACA,tciu.tciu_nombre as CIUDADORIGEN,tciu1.tciu_nombre as CIUDADDESTINO,mrut.mrut_valor as VALOR,drut.drut_horasal as HORASALIDA,drut.drut_fecha as FECHA,drut.drut_codigo as CODIGORUTA,drut.drut_planilla as NUMEROPLANILLA,drut.drut_codrelevante as RELEVANTE from dbxschema.druta drut,dbxschema.druta drut1,dbxschema.dtiquete dtiq,dbxschema.mruta mrut, dbxschema.tciudad tciu,dbxschema.tciudad tciu1,dbxschema.tciudad tciu2,dbxschema.mbusafiliado mbus where drut.drut_fecha between '"+fecha+"' and '"+fecha1+"' and  drut.drut_codigo=drut.drut_codigo and drut.mrut_codigo=mrut.mrut_codigo and mrut.tciu_cod=tciu.tciu_codigo and mrut.tciu_coddes=tciu1.tciu_codigo and mbus.mcat_placa=drut.mcat_placa and tciu.tciu_nombre=(select tciu.TCIU_nombre from dbxschema.MOFICINA mofi,dbxschema.tciudad tciu where mofi_codigo='"+ddlagencia.SelectedValue+"'and tciu.tciu_codigo=mofi.tciu_codigo)order by drut.drut_fecha,tciu1.tciu_nombre");
			}
			catch(Exception t)
			{
				Response.Write ("<script language:javascript>alert('No se Encuentra Ningun Reporte De esta Bus'); </script>" + t.Message );
						
			}
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)

			{
							
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				string lineas1=DBFunctions.SingleData("Select count (*) from DBXSCHEMA.DTIQUETE where  drut_codigo="+lineas.Tables[0].Rows[i][7].ToString()+"");
				string lineas2=DBFunctions.SingleData("Select count(*)from dbxschema.dtiquete where drut_codigo="+lineas.Tables[0].Rows[i].ItemArray[7].ToString()+" and test_codigo='RS'");
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
				string capacidad = DBFunctions.SingleData("SELECT pcat.pcat_capacidad FROM dbxschema.pcatalogovehiculo pcat,dbxschema.druta druta,dbxschema.mruta mruta,dbxschema.mbusafiliado mbus,dbxschema.mcatalogovehiculo mcat WHERE druta.drut_codigo="+lineas.Tables[0].Rows[i][7].ToString()+" and druta.mrut_codigo=mruta.mrut_codigo and druta.mcat_placa=mbus.mcat_placa and mbus.mcat_placa=mcat.mcat_placa and pcat.pcat_codigo=mcat.pcat_codigo");
				string disponibilidad = ((System.Convert.ToInt32(capacidad))-(System.Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM dpuestoruta WHERE drut_codigo="+lineas.Tables[0].Rows[i][7].ToString()+" AND (test_codigo<>'LI')").Trim()))).ToString();
						
					
				DataRow fila;
				fila= resultado.NewRow();
				fila["NUMERO BUS"] = lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["PLACA"]	= lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				fila["ORIGEN"] =  lineas.Tables[0].Rows[i].ItemArray[2].ToString();
				fila["DESTINO"]	= lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				fila["VALOR"] = lineas.Tables[0].Rows[i].ItemArray[4].ToString();
				fila["HORA SALIDA"] = lineas.Tables[0].Rows[i].ItemArray[5].ToString();
				fila["FECHA"] = lineas.Tables[0].Rows[i].ItemArray[6].ToString();
				fila["CODIGO RUTA"] = lineas.Tables[0].Rows[i].ItemArray[7].ToString();
				fila["NUMERO PLANILLA"] = lineas.Tables[0].Rows[i].ItemArray[8].ToString();
				fila["RELEVANTE"] = lineas.Tables[0].Rows[i].ItemArray[9].ToString();
				fila["NUMERO DE PASAJEROS"] = puestos-puestosre;
				fila["DISPONIBLES"] = disponibilidad;
				fila["TOTAL"] = puestos*Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[4]);
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
			DataColumn numero = new DataColumn();
			numero.DataType = System.Type.GetType("System.String");
			numero.ColumnName = "NUMERO BUS";
			numero.ReadOnly=true;
			resultado.Columns.Add(numero);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn placa = new DataColumn();
			placa.DataType = System.Type.GetType("System.String");
			placa.ColumnName = "PLACA";
			placa.ReadOnly=true;
			resultado.Columns.Add(placa);
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
			DataColumn valor = new DataColumn();
			valor.DataType = System.Type.GetType("System.String");
			valor.ColumnName = "VALOR";
			valor.ReadOnly=true;
			resultado.Columns.Add(valor);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn hora = new DataColumn();
			hora.DataType = System.Type.GetType("System.String");
			hora.ColumnName = "HORA SALIDA";
			hora.ReadOnly=true;
			resultado.Columns.Add(hora);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn fecha = new DataColumn();
			fecha.DataType = System.Type.GetType("System.String");
			fecha.ColumnName = "FECHA";
			fecha.ReadOnly=true;
			resultado.Columns.Add(fecha);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn ruta = new DataColumn();
			ruta.DataType = System.Type.GetType("System.String");
			ruta.ColumnName = "CODIGO RUTA";
			ruta.ReadOnly=true;
			resultado.Columns.Add(ruta);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn planilla = new DataColumn();
			planilla.DataType = System.Type.GetType("System.String");
			planilla.ColumnName = "NUMERO PLANILLA";
			planilla.ReadOnly=true;
			resultado.Columns.Add(planilla);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn relevante = new DataColumn();
			relevante.DataType = System.Type.GetType("System.String");
			relevante.ColumnName = "RELEVANTE";
			relevante.ReadOnly=true;
			resultado.Columns.Add(relevante);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn numeropasajeros = new DataColumn();
			numeropasajeros.DataType = System.Type.GetType("System.String");
			numeropasajeros.ColumnName = "NUMERO DE PASAJEROS";
			numeropasajeros.ReadOnly=true;
			resultado.Columns.Add(numeropasajeros);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn disponibles = new DataColumn();
			disponibles.DataType = System.Type.GetType("System.String");
			disponibles.ColumnName = "DISPONIBLES";
			disponibles.ReadOnly=true;
			resultado.Columns.Add(disponibles);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn total = new DataColumn();
			total.DataType = System.Type.GetType("System.String");
			total.ColumnName = "TOTAL";
			total.ReadOnly=true;
			resultado.Columns.Add(total);
			
			
		}

		#region C�digo generado por el Dise�ador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Dise�ador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		M�todo necesario para admitir el Dise�ador. No se puede modificar
		///		el contenido del m�todo con el editor de c�digo.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
