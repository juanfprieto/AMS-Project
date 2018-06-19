namespace AMS.Contabilidad
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
	using AMS.Reportes;
	using AMS.DB;

	
	public partial class AMS_Contabilidad_PerforVehiculos : System.Web.UI.UserControl
	{
		protected DataTable resultado;
		protected string reportTitle="Anexos vehiculos";
		protected System.Web.UI.WebControls.PlaceHolder toolsHolder;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		int Valto=0;
		int Valun=0;
		string total1=null;
		string unitario1=null;
		string poiva1=null;
		double total=0;
		double unitario=0;
		double poiva=0;
		
		protected DataSet lineas;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				
			}
			
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{

		}
		#endregion

		
		protected  void  generar(Object  Sender, EventArgs e)
		{
			string []pr=new string[2];
					

			pr[0]=pr[1]="";
			Press frontEnd = new Press(new DataSet(), reportTitle);
			frontEnd.PreHeader(tabPreHeader, Grid.Width, pr);
			frontEnd.Firmas(tabFirmas,Grid.Width);

			lb.Text="";
						
			this.PrepararTabla();
			lineas = new DataSet();
            DBFunctions.Request(lineas, IncludeSchema.NO, "select distinct  pcv.pcat_descripcion as modelo,mpv.mped_valounit as valor_total, mpv.mped_valounit/(1+(pcv.piva_porciva*0.01)) as valorunitario, mpv.mped_valounit - (mpv.mped_valounit/(1+(pcv.piva_porciva*0.01))) as valor_iva from dbxschema.mpedidovehiculo mpv, dbxschema.pcatalogovehiculo pcv,dbxschema.mcatalogovehiculo mcv, dbxschema.mnit mn, dbxschema.mvehiculo mv, dbxschema.mfacturacliente mfc, dbxschema.pvendedor pv, dbxschema.masignacionvehiculo mav,dbxschema.pclasevehiculo pclv, dbxschema.mfacturapedidovehiculo mfpv where mfc.pven_codigo = pv.pven_codigo and mfpv.pdoc_codigo = mfc.pdoc_codigo and mfpv.mped_codigo = mpv.pdoc_codigo and mfpv.mped_numepedi = mpv.mped_numepedi and mpv.pdoc_codigo = mav.pdoc_codigo and mpv.mped_numepedi = mav.mped_numepedi and mav.mveh_inventario = mv.mveh_inventario and mv.mcat_vin = mcv.mcat_vin and mcv.pcat_codigo = pcv.pcat_codigo and pcv.pcla_codigo = pclv.pcla_codigo");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				Valto=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[1]);
				Valun=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[2]);
				total=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[1]);
				unitario=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[2]);
				poiva=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[3]);
				///////////////////////////////////
				total1=String.Format("{0:C}",total);
				unitario1=String.Format("{0:C}",unitario);
				poiva1=String.Format("{0:C}",poiva);
				////////////////////////////
						
				int cantidad=0;
				cantidad=(Valto/Valun);
				fila= resultado.NewRow();
				fila["MODELO"] = lineas.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["VALORUNITARIO"]	= unitario1;
				fila["VALORTOTAL"] = total1;
				fila["PORCENTAJE"] = poiva1;
				fila["CANTIDAD"] = cantidad;
				resultado.Rows.Add(fila);
			}//fin sentencia FOR	

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
			//toolsHolder.Visible = true;
		}
		
		public void PrepararTabla()
		{
			resultado = new DataTable();
			//Adicionamos una columna que almacene el nombre de la linea
			DataColumn modelo = new DataColumn();
			modelo.DataType = System.Type.GetType("System.String");
			modelo.ColumnName = "MODELO";
			modelo.ReadOnly=true;
			resultado.Columns.Add(modelo);
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn valorUnitario = new DataColumn();
			valorUnitario.DataType = System.Type.GetType("System.String");
			valorUnitario.ColumnName = "VALORUNITARIO";
			valorUnitario.ReadOnly=true;
			resultado.Columns.Add(valorUnitario);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valorTotal = new DataColumn();
			valorTotal.DataType = System.Type.GetType("System.String");
			valorTotal.ColumnName = "VALORTOTAL";
			valorTotal.ReadOnly=true;
			resultado.Columns.Add(valorTotal);
			//Adicionamos una columna que almacene el porcentaje del iva
			DataColumn porcentajeIva = new DataColumn();
			porcentajeIva.DataType = System.Type.GetType("System.String");
			porcentajeIva.ColumnName = "PORCENTAJE";
			porcentajeIva.ReadOnly=true;
			resultado.Columns.Add(porcentajeIva);
			//Adicionamos una columna que almacene el porcentaje del iva
			DataColumn cantidad = new DataColumn();
			cantidad.DataType = System.Type.GetType("System.String");
			cantidad.ColumnName = "CANTIDAD";
			cantidad.ReadOnly=true;
			resultado.Columns.Add(cantidad);
		}
		protected void Grid_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
		}
		
	}

		
	
}
