namespace AMS.Comercial
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Tools;
	using AMS.DBManager;
	using AMS.Forms;
	using Ajax;

	/// <summary>
	///		Descripción breve de AMS_Comercial_AgregarBusAfiliado.
	/// </summary>
	public class AMS_Comercial_AgregarBusAfiliado : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.TextBox FechaIngreso;
		protected System.Web.UI.WebControls.TextBox NumBus;
		protected System.Web.UI.WebControls.DropDownList ddlestado;
		protected System.Web.UI.WebControls.DropDownList ddlplaca;
		protected System.Web.UI.WebControls.TextBox txtVin;
		protected System.Web.UI.WebControls.TextBox txtCatalogo;
		protected System.Web.UI.WebControls.Button btnGuardar;

		string txtVinS,txtCatalogoS;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox ValVehiculo;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList categoria;
		protected System.Web.UI.WebControls.TextBox ddlpropietario;
		protected System.Web.UI.WebControls.TextBox ddlasociado;
		protected System.Web.UI.WebControls.TextBox ddlconductor;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.TextBox SegundoConductor;
		protected System.Web.UI.WebControls.TextBox txtReposicion;
		protected System.Web.UI.WebControls.TextBox txtObservaciones;
		protected System.Web.UI.WebControls.TextBox txtPotencia;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlConfiguracion;
		protected System.Web.UI.WebControls.TextBox txtNombrePropietario;
		protected System.Web.UI.WebControls.TextBox ddlpropietarioa;
		protected System.Web.UI.WebControls.TextBox ddlasociadoa;
		protected System.Web.UI.WebControls.TextBox ddlconductora;
		protected System.Web.UI.WebControls.TextBox SegundoConductora;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox txtCapacidadC;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox txtCapacidad;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_AgregarBusAfiliado));	
			txtVinS=Request.Form[txtVin.UniqueID];
			txtCatalogoS=Request.Form[txtCatalogo.UniqueID];
			
            
			if (!IsPostBack)
			{

				btnGuardar.Enabled=true;
                DatasToControls bind=  new DatasToControls();
				DataSet placas=new DataSet();
				bind.PutDatasIntoDropDownList(ddlplaca,"select mc.mcat_placa,mc.mcat_placa from dbxschema.mcatalogovehiculo mc where mc.mcat_placa not in(select distinct mb.mcat_placa from dbxschema.mbusafiliado mb);");
				ListItem itm=new ListItem("---Placa---","");
				ddlplaca.Items.Insert(0,itm);
				ddlplaca.SelectedIndex=0;

				FechaIngreso.Text= DateTime.Now.ToString("yyyy-MM-dd");//OJO ESTA FECHA REALMENTE ES?, SE DEBERÍA PDOER INGRESAR OTRA FECHA
				bind.PutDatasIntoDropDownList(ddlestado,"select testa_codigo,testa_descripcion from DBXSCHEMA.testadobusafiliado where testa_codigo>=0");
				bind.PutDatasIntoDropDownList(categoria,"select MBUS_CATEGORIA from DBXSCHEMA.MCATEGORIA_FONDO_REPOSICION_BUS ORDER BY MBUS_CATEGORIA");
				bind.PutDatasIntoDropDownList(ddlConfiguracion,"select MCON_COD,NOMBRE from DBXSCHEMA.MCONFIGURACIONBUS ORDER BY NOMBRE");

				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "Vinculación exitosa.");
			}
			btnGuardar.Enabled=true;
			// Introducir aquí el código de usuario para inicializar la página
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
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		[Ajax.AjaxMethod]
		public DataSet CargarPlaca(string mplaca)
		{
			DataSet Vins= new DataSet();
			string sqlPlaca="select mc.mcat_vin as VIN, mc.pcat_codigo as CATALOGO,mc.MNIT_NIT AS NIT, np.MNIT_NOMBRES concat ' ' concat np.MNIT_APELLIDOS as NOMNIT from dbxschema.mcatalogovehiculo mc LEFT JOIN dbxschema.mnit np ON np.MNIT_NIT=mc.MNIT_NIT where mc.mcat_placa='"+mplaca+"'";
			DBFunctions.Request(Vins,IncludeSchema.NO,sqlPlaca);
			return Vins;
			
		}
		[Ajax.AjaxMethod]
		public DataSet FindMatches(string SearchCriteria)
		{
			DataSet Vins= new DataSet();
			
			DBFunctions.Request(Vins,IncludeSchema.NO,"select mnit_nombres concat ' ' concat coalesce(mnit_nombre2,'') concat ' ' concat mnit_apellidos concat ' ' concat coalesce(mnit_apellido2,'') as Nombre from dbxschema.mnit where mnit_nombres concat coalesce(mnit_nombre2,'') concat ' ' concat mnit_apellidos concat coalesce(mnit_apellido2,'') like '"+SearchCriteria.ToUpper()+"%'");
			return Vins;
			
		}

		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			int valor=0, NumBusS=0, pasajeros=0;
			double galonaje;
			//int ini=Convert.ToInt32(DBFunctions.SingleData("select TCAT_RANGOI from DBXSCHEMA.TCAT_BUS WHERE TCAT_CODIGO='"+categoria.SelectedValue.ToString()+"'"));
			//int fin=Convert.ToInt32(DBFunctions.SingleData("select TCAT_RANGOF from DBXSCHEMA.TCAT_BUS WHERE TCAT_CODIGO='"+categoria.SelectedValue.ToString()+"'"));
			string asociado,chofer1,chofer2,reposicion;
			
			//Validaciones
			if(ddlpropietario.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe dar el nit del propietario.");
				return;
			}
			//NITS
			asociado=((ddlasociado.Text.Length>0)?("'"+ddlasociado.Text+"'"):"NULL");
			chofer1=((ddlconductor.Text.Length>0)?("'"+ddlconductor.Text+"'"):"NULL");
			chofer2=((SegundoConductor.Text.Length>0)?("'"+SegundoConductor.Text+"'"):"NULL");
			reposicion=((txtReposicion.Text.Length>0)?("'"+txtReposicion.Text+"'"):"NULL");
			//Valor
			try
			{
				valor=Convert.ToInt32(ValVehiculo.Text.Replace(",",""));}
			catch
			{
                Utils.MostrarAlerta(Response, "Valor de vehículo no válido.");
				return;
			}
			//Galonaje
			try
			{
				galonaje=Convert.ToDouble(txtCapacidadC.Text.Replace(",",""));}
			catch
			{
                Utils.MostrarAlerta(Response, "Capacidad de combustible no válida.");
				return;
			}
			//Pasajeros
			try{
				pasajeros=Math.Abs(Convert.ToInt16(txtCapacidad.Text.Replace(",","")));}
			catch
			{
                Utils.MostrarAlerta(Response, "Capacidad de pasajeros no válida.");
				return;
			}
			//Numero de bus
			try
			{
				NumBusS=Convert.ToInt32(NumBus.Text.Replace(",",""));
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Número de bus no válido.");
				return;
			}
			//Revisar que no se ha afiliado el bus
			if(DBFunctions.RecordExist("SELECT MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO where MCAT_PLACA='"+ddlplaca.SelectedValue+"'"))
			{
                Utils.MostrarAlerta(Response, "El bus de placa " + ddlplaca.SelectedValue + " ya está vinculado.");
				return;
			}

			//Revisar que no se ha utilizado el numero
			if(DBFunctions.RecordExist("SELECT MBUS_NUMERO from DBXSCHEMA.MBUS_NUMERO_INTERNO where MBUS_NUMERO="+NumBusS+" ") || DBFunctions.RecordExist("SELECT MBUS_NUMERO from DBXSCHEMA.MBUSAFILIADO where MBUS_NUMERO="+NumBusS+" "))
			{
                Utils.MostrarAlerta(Response, "El número del bus ya ha sido utilizado.");
				return;
			}
			
			//Revisar categoria
			//if(!(valor >= ini && valor <= fin))
			//{
			//	Response.Write("<script language='javascript'>alert('Categoría incorrecta o valor incorrecto.');</script>");
			//	return;
			//}

			//Afiliar bus
			int ValorBus=Convert.ToInt32(ValVehiculo.Text.Replace(",",""));
			
			ArrayList sqlB=new ArrayList();
			sqlB.Add("insert into dbxschema.MBUSAFILIADO values ('"+ddlpropietario.Text.ToString()+"',"+asociado+","+chofer1+","+chofer2+",'"+FechaIngreso.Text.ToString()+"',"+NumBusS+",'"+txtVinS+"','"+txtCatalogoS+"',"+ddlestado.SelectedValue+",'"+ddlplaca.SelectedValue.ToString()+"',"+ValorBus.ToString("0")+",'"+categoria.SelectedValue.ToString()+"',"+reposicion+",'"+txtObservaciones.Text+"','"+txtPotencia.Text+"',"+ddlConfiguracion.SelectedValue+","+galonaje.ToString("0")+","+pasajeros.ToString("0")+");");
			//Almacenar numero en historial numero
			sqlB.Add("insert into dbxschema.MBUS_NUMERO_INTERNO values ("+NumBusS+",'"+ddlplaca.SelectedValue+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
			sqlB.Add("insert into DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS values('"+ddlplaca.SelectedValue.ToString()+"','"+ddlpropietario.Text.ToString()+"',100,'S','"+FechaIngreso.Text+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL)");
			sqlB.Add("insert into DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS_HST values(DEFAULT,'"+ddlplaca.SelectedValue.ToString()+"','"+ddlpropietario.Text.ToString()+"',100,'S','N','"+FechaIngreso.Text+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',NULL)");

			if(DBFunctions.Transaction(sqlB))
				Response.Redirect(indexPage+"?process=Comercial.AgregarBusAfiliado&act=1&path="+Request.QueryString["path"]);
			else
				lblError.Text=DBFunctions.exceptions;
		}

	}
}
