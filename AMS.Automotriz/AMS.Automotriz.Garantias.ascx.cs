
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using AMS.DBManager;
	using Ajax;
	using System.Configuration;

namespace AMS.Automotriz
{
	/// <summary>
	///		Descripción breve de AMS_Automotriz_Garantias.
	/// </summary>
	public partial class AMS_Automotriz_Garantias : System.Web.UI.UserControl
	{/*
		private string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		string ddlVinS;
		protected System.Web.UI.WebControls.RegularExpressionValidator REV;
		protected System.Web.UI.WebControls.RequiredFieldValidator RFV;
		protected System.Web.UI.WebControls.Button Factibilidad;
		protected System.Web.UI.WebControls.TextBox TBKilo;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// se carga la funcion ajax para los vins, esta funciòn està definida màs adelante y, tb esta definida en el codigo de HTML
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Automotriz.AMS_Automotriz_Garantias));
			ddlVinS=Request.Form[ddlVin.UniqueID];
			// Introducir aquí el código de usuario para inicializar la página
			if(!Page.IsPostBack)
			{				 
				//se cargan los datos para los drpdownlist que traen informacion de la base de datos
				DatasToControls bind = new DatasToControls();
				FechaActual.Text=DateTime.Now.ToString("yyyy-MM-dd");
				if(ddlCatalogo.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(ddlCatalogo,"Select pcat_codigo, PCAT_descripcion from DBXSCHEMA.PCATALOGOVEHICULO ORDER BY PCAT_descripcion");
					ListItem first=new ListItem("--Seleccione Un Catálogo Por Favor--","0");
					ddlCatalogo.Items.Insert(0,first);
				}
				
				if(ddlVin.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(ddlVin,"select mcat_vin as VIN from dbxschema.mvehiculo where pcat_codigo='"+ddlCatalogo.SelectedValue+"'");
					ListItem first=new ListItem("--Seleccione Un VIN Por Favor--","0");
					ddlVin.Items.Insert(0,first);
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

		}
		#endregion
		//Para Retornar la funcion a HTML y cargar los VINS evitando consulta en la BD
		[Ajax.AjaxMethod]
		//Metodo para cargar los Vins, se refleja en una funciòn que lleva el mismo nombre en HTML
		public DataSet CargarVin(string pcatalogo)
		{
			DataSet Vins= new DataSet();
			DBFunctions.Request(Vins,IncludeSchema.NO,"select mcat_vin as VIN from dbxschema.mvehiculo where pcat_codigo='"+pcatalogo+"'");
			return Vins;
			
		}
		//Se activa mediante el botòn consultar el segundo panel
		protected void Consultar_G_Click(object sender, System.EventArgs e)
		{
			ddlVinS=Request.Form[ddlVin.UniqueID];
			string variable=DBFunctions.SingleData("select mveh_fechentr from DBXSCHEMA.MVEHICULO where mcat_vin='"+ddlVinS+"'");
			string NIT, fechactual, fechentrega, catalogo, color;
			if(variable==null)
			{
                Response.Write("<script language:javascript>alert('El Vehículo NO tiene Fecha de Entrega');</script>"); 
				return;
			}
			//Se traen los valores de las fechas
			DateTime FecActual= new DateTime();
			DateTime FecEntrega= new DateTime();
			DataSet Fechas= new DataSet();
			DataSet CatalogoVeh= new DataSet();
			DataSet MCatalogoVeh= new DataSet();
			TimeSpan Tiempo = new TimeSpan();
			// Se carga el Nit asociado al VIN del Vehìculo
			NIT=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.MVEHICULO where mcat_vin='"+ddlVinS+"'");
			catalogo=DBFunctions.SingleData("select pcat_codigo from DBXSCHEMA.MVEHICULO where mcat_vin='"+ddlVinS+"'");
			// Se cargan los valores de las fechas
			DBFunctions.Request(Fechas,IncludeSchema.NO,"select current timestamp, mveh_fechentr from DBXSCHEMA.MVEHICULO where mcat_vin='"+ddlVinS+"'");
			fechentrega=Fechas.Tables[0].Rows[0][1].ToString();
			fechactual=Fechas.Tables[0].Rows[0][0].ToString();
			FecEntrega=Convert.ToDateTime(fechentrega);
			FecActual=Convert.ToDateTime(fechactual);
			
			//Se cargan los datos en pantalla
			TxtCliente.Text=DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos from dbxschema.mnit where mnit_nit='"+NIT+"'");
			TxtDoc.Text=NIT.ToString();
			
			DBFunctions.Request(CatalogoVeh,IncludeSchema.NO,"select pcat_descripcion, pmar_codigo from DBXSCHEMA.pcatalogovehiculo where pcat_codigo='"+catalogo+"'");
			Catalogo.Text=CatalogoVeh.Tables[0].Rows[0][0].ToString();
			Marca.Text=CatalogoVeh.Tables[0].Rows[0][1].ToString();
			
			DBFunctions.Request(MCatalogoVeh,IncludeSchema.NO,"select mcat_placa, mcat_motor, pcol_codigo, mcat_anomode, mcat_concvend from DBXSCHEMA.mcatalogovehiculo where mcat_vin='"+ddlVinS+"'");
			PlacaV.Text=MCatalogoVeh.Tables[0].Rows[0][0].ToString();
			MotorV.Text=MCatalogoVeh.Tables[0].Rows[0][1].ToString();
			color=MCatalogoVeh.Tables[0].Rows[0][2].ToString();
			ColorV.Text=DBFunctions.SingleData("Select pcol_descripcion from DBXSCHEMA.PCOLOR where pcol_codigo='"+color+"'");
			AModeloV.Text=MCatalogoVeh.Tables[0].Rows[0][3].ToString();
			Concesionario.Text=MCatalogoVeh.Tables[0].Rows[0][4].ToString();
			TxtVIN.Text=ddlVinS;
			FechaCompra.Text=fechentrega;
			ListItem first=new ListItem("-Seleccione KM Por Favor-","0");
			KilomSelect.Items.Insert(0,first);
			//Se coloca visible el panel
			Panel4.Visible=true;
		}

		protected void KilomSelect_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ddlVinS=TxtVIN.Text.ToString();
			string fechactual, fechentrega, catalogo, valido, invalido, kilom;
			int kilomint;
			valido="VALIDO";
			invalido="NO VALIDO";
			DateTime FecActual= new DateTime();
			DateTime FecEntrega= new DateTime();
			DataSet Fechas= new DataSet();
			DataSet CatalogoVeh= new DataSet();
			TimeSpan Tiempo = new TimeSpan();
			catalogo=DBFunctions.SingleData("select pcat_codigo from DBXSCHEMA.MVEHICULO where mcat_vin='"+ddlVinS+"'");
			
			DBFunctions.Request(Fechas,IncludeSchema.NO,"select current timestamp, mveh_fechentr from DBXSCHEMA.MVEHICULO where mcat_vin='"+ddlVinS+"'");
			fechentrega=Fechas.Tables[0].Rows[0][1].ToString();
			fechactual=Fechas.Tables[0].Rows[0][0].ToString();
			FecEntrega=Convert.ToDateTime(fechentrega);
			FecActual=Convert.ToDateTime(fechactual);
			
			DBFunctions.Request(CatalogoVeh,IncludeSchema.NO,"select pcat_gartiempo, pcat_garkm from DBXSCHEMA.pcatalogovehiculo where pcat_codigo='"+catalogo+"'");
			GarKm.Text=CatalogoVeh.Tables[0].Rows[0][1].ToString();
			string tiempogar=CatalogoVeh.Tables[0].Rows[0][0].ToString();
			int tiempodias=Convert.ToInt32(tiempogar);
			string kilometrogar=CatalogoVeh.Tables[0].Rows[0][1].ToString();
			int kilometrosgarantia=Convert.ToInt32(kilometrogar);
			Tiempo=FecActual-FecEntrega;
			int meses = Tiempo.Days/30;
			Meses.Text=meses.ToString();
			int dias = Tiempo.Days%30;
			Dias.Text=dias.ToString();
			string dias_dias=Tiempo.Days.ToString();
			int compdias=Convert.ToInt32(dias_dias);
			

			
			kilom=KilomSelect.SelectedValue.ToString();
			kilomint=Convert.ToInt32(kilom);
			//Revisamos si la Garantía es Válida por Tiempo y por Kilometraje
			if((tiempodias>=compdias)&&(kilomint<kilometrosgarantia))
			{
				GarTiempo.Text=valido;
				GarantiaKm.Text=valido;
			}
			else
			{
				if(tiempodias>=compdias)
				{
					GarTiempo.Text=valido;
					GarantiaKm.Text=invalido;
				}
				else
				{
					if(kilomint<kilometrosgarantia)
					{
						GarTiempo.Text=invalido;
						GarantiaKm.Text=valido;
					}
					else
						GarTiempo.Text=invalido;
					GarantiaKm.Text=invalido;
				}
			}
			//Hacemos Visible el Panel de Garantía
			Panel3.Visible=true;
		}

		protected void ValidarGar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Automotriz.Garantias_2");
			//enviar VIN
		}*/ 

	}
}
