namespace AMS.Garantias
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using AMS.DBManager;
	using System.Configuration;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Garantias_RegistrodeVentas.
	/// </summary>
	public partial class AMS_Garantias_RegistrodeVentas : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox tbNombre1;

		

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack )
			{
					string lbnit, ciudad;
				//*******carga los datos del dealer en el form**********
				string usuario=HttpContext.Current.User.Identity.Name;
				//nombre del usuario
				lblUsuario.Text= DBFunctions.SingleData("Select susu_nombre from DBXSCHEMA.SUSUARIO where susu_login='"+usuario+"'");
				//nit del usuario
				lbnit  =Convert.ToString( DBFunctions.SingleData("Select mnit_nit  from DBXSCHEMA.SUSUARIO where susu_login='"+usuario+"'"));
				lblNitDealer.Text=lbnit;
				if (lbnit != null && lbnit  != "")
				{
					lblCiudad.Text=  DBFunctions.SingleData("Select pciu_nombre from DBXSCHEMA.MCLIENTE, dbxschema.pciudad where pciu_codigo = pcui_codigo and mnit_nit ='"+lbnit+"'");
				//ciudad del usuario
					ciudad = DBFunctions.SingleData("Select pcui_codigo from DBXSCHEMA.MCLIENTE where  mnit_nit ='"+lbnit+"'");

				}
				else
				{  //El dealer debe tener registrado el nit en la tabla SUSUARIO para poder ingresar 
                    Utils.MostrarAlerta(Response, "ERROR Debe asignar un Nit al usuario");
					Panel1.Visible=false;
				
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

		protected void btBuscar_Click(object sender, System.EventArgs e)
		{//******busca y carga los datos del vehiculo que se va a ingresar
			string codigocat, codicolor;
			codigocat= DBFunctions.SingleData("select pcat_codigo from dbxschema.mcatalogovehiculo where mcat_chasis='"+tbChasis.Text+"'");
			if (codigocat != "")
			{ 
				
				tbCatalogo.Text=DBFunctions.SingleData("select distinct pcat_descripcion  from dbxschema.pcatalogovehiculo  where pcat_codigo = '"+codigocat+"'");
				tbMotor.Text = DBFunctions.SingleData("select mcat_motor from dbxschema.mcatalogovehiculo where mcat_chasis='"+tbChasis.Text+"'");
				tbModelo.Text= Convert.ToString(DBFunctions.SingleData("select mcat_anomode from dbxschema.mcatalogovehiculo where mcat_chasis='"+tbChasis.Text+"'"));
				
				codicolor= DBFunctions.SingleData("select pcol_codigo from dbxschema.mcatalogovehiculo where mcat_chasis='"+tbChasis.Text+"'");
				tbColor.Text= DBFunctions.SingleData("select distinct pcol_descripcion  from dbxschema.pcolor  where pcol_codigo = '"+codicolor+"'");
				//deshabilita los campos una vez encontrados los datos del vehiculo
				tbCatalogo.ReadOnly=true;
				tbMotor.ReadOnly=true;
				tbModelo.ReadOnly=true;
				tbColor.ReadOnly=true;
			}
			else Utils.MostrarAlerta(Response, "Vehículo NO Encontrado");
		}

		protected void btIngresar_Click(object sender, System.EventArgs e)
		{//******************se ingresa el registro de venta del dealer en MVENTASDEALER*********************
			//validar si los datos de venta entan completos
			if (tbFechaFact.Text!="" && tbFactura.Text!= ""  && tbValofact.Text != "" && tbFechaEntrega.Text != "" )
			{
					DateTime date1= new DateTime();
				DateTime date2= new DateTime();
				
				date1=Convert.ToDateTime(tbFechaEntrega.Text ) ;
				date2= Convert.ToDateTime(tbFechaFact.Text);
				if (date1>= date2)      //validar que la fecha de entrega del vehiculo sea >= a la fecha de la factura
				{
					string mcat_vin, pcat_codigo,pciu_codigo;
					//retoma los datos del vehiculo
					mcat_vin = DBFunctions.SingleData("select mcat_vin from dbxschema.mcatalogovehiculo where mcat_chasis='"+tbChasis.Text+"'");
					pcat_codigo = DBFunctions.SingleData("select pcat_codigo from dbxschema.mcatalogovehiculo where mcat_chasis='"+tbChasis.Text+"'");
					pciu_codigo = DBFunctions.SingleData("Select pcui_codigo from DBXSCHEMA.MCLIENTE where  mnit_nit ='"+lblNitDealer.Text+"'");
					//se insertan en la DB
					DBFunctions.NonQuery("INSERT INTO MVENTASDEALER VALUES ('"+this.lblNitDealer.Text+"', "+Convert.ToInt32(tbFactura.Text )+","+Convert.ToInt32 (tbCarnet.Text)+",default,'"+tbNitCliente.Text+"','"+ pcat_codigo+"','"+Convert.ToDateTime(tbFechaEntrega.Text ).ToString("yyyy-MM-dd")+"','"+mcat_vin+"','"+Convert.ToDateTime(tbFechaFact.Text).ToString("yyyy-MM-dd")+"', '"+pciu_codigo+"',"+Convert.ToDecimal(tbValofact.Text)+") ");
					//se cambia el estado del vehiculo  a 60 "ENTREGADO"
					DBFunctions.NonQuery("UPDATE  MVEHICULO SET  TEST_TIPOESTA = 60 WHERE MCAT_VIN = '"+mcat_vin+"'");

					string indexpage = ConfigurationManager.AppSettings["MainIndexPage"];
					//REGRESA A INICIO
					Response.Redirect(""+indexpage +"?process=Garantias.RegistrodeVentas");
				}
				else Utils.MostrarAlerta(Response, "La fecha de entrega debe ser mayor o igual a la fecha de la factura");
			}
			else Utils.MostrarAlerta(Response, "Debe diligenciar los datos de la venta");
		}
		
	}
}
