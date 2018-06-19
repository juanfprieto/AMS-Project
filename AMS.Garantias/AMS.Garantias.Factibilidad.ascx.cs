namespace AMS.Garantias
{	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using AMS.DBManager;
	using System.Configuration;
	using Ajax;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Automotriz_Garantias.
	/// </summary>
	/// 
public partial class AMS_Garantias_Factibilidad : System.Web.UI.UserControl
	{
	#region ATRIBUTOS
		private string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.RegularExpressionValidator REV;
		protected System.Web.UI.WebControls.RequiredFieldValidator RFV;
		protected System.Web.UI.WebControls.Button Factibilidad;
		protected System.Web.UI.WebControls.TextBox TBKilo;
		protected System.Web.UI.WebControls.TextBox tbChasis;
	#endregion
		
	protected void Page_Load(object sender, System.EventArgs e)

	{
		Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Garantias.AMS_Garantias_Factibilidad));
		if(!Page.IsPostBack)
		{
				if (DBFunctions.RecordExist("Select cgar_kilotoler from DBXSCHEMA.CGARANTIA"))
			{	Panel4.Visible=false;	 
				//se cargan los datos para el  drpdownlist que trae informacion de los catalogos 
				DatasToControls bind = new DatasToControls();
				FechaActual.Text=DateTime.Now.ToString("yyyy-MM-dd");
				if(ddlCatalogo.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(ddlCatalogo,"Select pcat_codigo,PCAT_descripcion from PCATALOGOVEHICULO order by PCAT_descripcion");
					ListItem first=new ListItem("--Seleccione Un Catálogo Por Favor--","0");
					ddlCatalogo.Items.Insert(0,first);
					//bind.PutDatasIntoDropDownList(ddlPrefijo,"Select PDOC_CODIGO, pdoc_codigo||'-'|| pdoc_descripcion from pdocumento where tdoc_tipodocu = 'SG'");
                    Utils.llenarPrefijos(Response, ref ddlPrefijo  , "%", "%", "SG");
			        int n = Convert.ToInt32(DBFunctions.SingleData("select pdoc_ultidocu from pdocumento where pdoc_codigo = '"+ddlPrefijo.SelectedValue+"' "))+1;
					tbPdoc_codigo.Text = Convert.ToString(n);
				}
			}
			 //else //Utils.MostrarAlerta(Response, "Deben configurarse los parametros de garantias");

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

	#region EVENTOS
	//BUSQUEDA DEL VEHICULO
	protected void Consultar_G_Click(object sender, System.EventArgs e)
	{
		Panel4.Visible=false;
		Panel3.Visible=false;
		int estado = 0;
		string estado2  ="no";
		//validar si el vehiciulo está 
		if ((estado2 = Convert.ToString(DBFunctions.SingleData("Select test_tipoesta from MVEHICULO B, MCATALOGOVEHICULO A   where A.MCAT_VIN = B.MCAT_VIN ANd A.pcat_codigo = '"+ddlCatalogo.SelectedValue+"' and A."+ddlOpcion.SelectedValue+"= '"+tbVin.Text.ToString()+"'")))!="" )	
		{   
			estado= Convert.ToInt32(estado2);
			//si es estado ENTREGADO AL CLIENTE
			if (estado == 60)
			{
				if (tbId.Text.Trim() !="")
				{
					string checknit = DBFunctions.SingleData("Select mnit_nit from  MVEHICULO  where  mcat_vin = '"+tbVin.Text.ToString()+"'");
					// DEBE validar nit
					if (checknit == this.tbId.Text.ToString() )
						CargarDatos(estado);
                    Utils.MostrarAlerta(Response, "Nit NO valido");
				}
				else Utils.MostrarAlerta(Response, "El vehículo ya fue entregado, debe ingresar la identificación del cliente");
			}
				//si estado es entregado al dealer 
			else if (estado == 61){CargarDatos(estado);}
            else { Utils.MostrarAlerta(Response, "El Vehículo NO ha sido entregado");}
            
		}	
		else Utils.MostrarAlerta(Response, "Vehículo NO Encontrado");
		ViewState["estado"] = estado;
	}
	//Manejo de RESTRINCCIONES de TIEMPO y KILOMETRAJE para otorgar garantia
	protected void btaplicar_Click(object sender, System.EventArgs e)

	{
			string fechactual, fechentrega;
		DateTime FecActual= new DateTime();
		DateTime FecEntrega= new DateTime();
		DataSet Fechas= new DataSet();
		TimeSpan Tiempo = new TimeSpan();
		DataSet CatalogoVeh= new DataSet();

		//carga la fecha actual y la fecha de entrega del vehiculo
		DBFunctions.Request(Fechas,IncludeSchema.NO,"select current timestamp, mveh_fechentr from MVEHICULO where mcat_vin='"+this.lb2VIN.Text +"'");
		fechentrega=Fechas.Tables[0].Rows[0][1].ToString();
		int estado = (int)ViewState["estado"];//recupera el estado en que se encuentra el vehiculo
		fechactual=Fechas.Tables[0].Rows[0][0].ToString();
		FecActual=Convert.ToDateTime(fechactual);
		if (estado != 61)// si el estado es entregadoa dealer no tiene fecha de entrega; tons  no entra
		{
				FecEntrega=Convert.ToDateTime(fechentrega);
			//hallar  el tiempo transcurrido a partir de la fecha de entrega del vehiculo
			Tiempo=FecActual-FecEntrega;
			//sacamos los meseS
			int meses = Tiempo.Days/30;
			lbMeses.Text=meses.ToString();
			//sacamos los dias
			int dias = Tiempo.Days%30;
			lbDias.Text=dias.ToString();     
		}
		else {lbDias.Text="0";  lbMeses.Text="0";}
		//carga el tiempo de Garantia que se le otrga al Vehiculo
		DBFunctions.Request(CatalogoVeh,IncludeSchema.NO,"select pcat_gartiempo, pcat_garkm, pgru_grupo from  pcatalogovehiculo where pcat_codigo='"+lbPcatcodigo.Text+"'");
		this.lbGarTiempo.Text =CatalogoVeh.Tables[0].Rows[0][0].ToString();
		this.lbgarantiaKm.Text=CatalogoVeh.Tables[0].Rows[0][1].ToString();
		string grupo = CatalogoVeh.Tables[0].Rows[0][2].ToString();
		ViewState["grupo"] = grupo;

		//*Validar que el Kilometraje digitado sea el ultimo kilometraje rejistrado*
		if (Convert.ToDouble (tbKilometros.Text) > Convert.ToDouble (lbUltiKilo.Text))		
		{
				int ban1 = 0, ban2 = 0 ,ban3 = 0; //** banderas para validar condiciones  de entrega de la garantia
			if(estado==61) //si el estado es entregado dealer no va a tener fecha de entrega
				ban1=1;
			else 		if (Convert.ToInt32 (lbGarTiempo.Text) >Convert.ToInt32(lbMeses.Text ) ) 	ban1=1;
			else if (Convert.ToInt32 (lbGarTiempo.Text) == Convert.ToInt32(lbMeses.Text )&& Convert.ToInt32(lbDias.Text )==0)	ban1=1;
																	
			//**si cumple con kilometraje
			if( Convert.ToDouble (lbgarantiaKm.Text ) >= Convert.ToDouble(tbKilometros.Text ) ) ban2 = 2;
			ban3= ban1+ban2; 
				
			if (ban3==0) 
			{
					lbMensaje.Text = "El Vehiculo NO cumple con el tiempo ni con el Kilometraje para efectuar la grarantia"; 
				Panel3.Visible=true	;
			}
			else if(ban3==1)  
			{
					lbMensaje.Text="El Vehiculo NO cumple con la condicion de Kilometraje para efectuar la garantia";
				Panel3.Visible=true;		
			}
			else if(ban3==2)
			{
					lbMensaje.Text = "El Vehiculo NO cumple con la condicion de  tiempo para efectuar la garantia";
				Panel3.Visible=true;
			}
			else  if (!validarMantenimientos())	/*si el tiempo y el kilometraje son validos verificar kits*/
			{
					lbMensaje.Text = "El Vehiculo NO cumple con los mantenimientos programados para efectuar la garantia";
				Panel3.Visible=true;
			}
			else
			{	string indexpage = ConfigurationManager.AppSettings["MainIndexPage"];
				Response.Redirect(""+indexpage +"?process=Garantias.Otorgamiento&exito=1&prefijo= "+ddlPrefijo.SelectedValue+"&numero="+tbPdoc_codigo+"&catalogo="+lbPcatcodigo.Text+"&vin="+lb2VIN.Text+"&nitclient="+tbDoc.Text+"&kilometraje="+tbKilometros.Text+"");
			}
			dgKits.Visible=true;
		  
		}  
			//*El kilometrage Ingresado no corresponde al ultimo kilometraje registrado *
		else Utils.MostrarAlerta(Response, "El kilometrage Ingresado no corresponde al ultimo kilometraje registrado");
	}

	/* en caso de no ser otorgada la garantia envia solicitud */
	protected void ValidarGar_Click(object sender, System.EventArgs e)
	{
		Response.Redirect(indexPage+"?process=Aotra_paftina_2");
		//enviar  enviar la solicitud de una garantia no aprobada
	}

	# endregion

	#region  METODOS 
//Carga datos del vehiculo en pantalla
private void CargarDatos(int estado)
{

		string NIT, fechactual, fechentrega, catalogo, color;

				//Se traen los valores de las fechas
				DateTime FecActual= new DateTime();
				DateTime FecEntrega= new DateTime();
				DataSet Fechas= new DataSet();
				DataSet CatalogoVeh= new DataSet();
				DataSet MCatalogoVeh= new DataSet();
				TimeSpan Tiempo = new TimeSpan();
				// Se cargan datos de McatalogoVehìculo
		DBFunctions.Request(MCatalogoVeh ,IncludeSchema.NO,"select mnit_nit, pcat_codigo, mcat_vin ,mcat_placa, mcat_motor, pcol_codigo, mcat_anomode, mcat_concvend,mcat_numeultikilo from MCATALOGOVEHICULO  where "+ddlOpcion.SelectedValue+"='"+this.tbVin.Text.ToString()+"'");
		NIT = MCatalogoVeh.Tables[0].Rows[0][0].ToString();

		catalogo =MCatalogoVeh.Tables[0].Rows[0][1].ToString();
		DBFunctions.Request(CatalogoVeh,IncludeSchema.NO,"select pcat_descripcion, pmar_codigo from pcatalogovehiculo where pcat_codigo='"+catalogo+"'");
		tbCatalogo.Text=CatalogoVeh.Tables[0].Rows[0][0].ToString();
		lbMarca.Text=CatalogoVeh.Tables[0].Rows[0][1].ToString();
		lb2VIN.Text =MCatalogoVeh.Tables[0].Rows[0][2].ToString();
		lbPlacaV.Text=MCatalogoVeh.Tables[0].Rows[0][3].ToString();
		lbMotorV.Text=MCatalogoVeh.Tables[0].Rows[0][4].ToString();

		color=MCatalogoVeh.Tables[0].Rows[0][5].ToString();
		lbColorV.Text=DBFunctions.SingleData("Select pcol_descripcion from PCOLOR where pcol_codigo='"+color+"'");

		lbAModeloV.Text=MCatalogoVeh.Tables[0].Rows[0][6].ToString();
		lbConcesionario.Text=MCatalogoVeh.Tables[0].Rows[0][7].ToString();
		lbUltiKilo.Text = Convert.ToDouble(MCatalogoVeh.Tables[0].Rows[0][8]).ToString("N");


				// Se cargan los valores de las fechas
				DBFunctions.Request(Fechas,IncludeSchema.NO,"select current timestamp, mveh_fechentr from MVEHICULO where mcat_vin='"+lb2VIN.Text+"'");
				fechentrega=Fechas.Tables[0].Rows[0][1].ToString();
				fechactual=Fechas.Tables[0].Rows[0][0].ToString();
				FecActual=Convert.ToDateTime(fechactual);
				//Se cargan los datos en pantalla
				tbCliente.Text=DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos from dbxschema.mnit where mnit_nit='"+NIT+"'");
				tbDoc.Text=NIT.ToString();
			
				lbFechaCompra.Text=Convert.ToDateTime(fechentrega).ToString("yyyy-MM-dd");
				lbPcatcodigo.Text= ddlCatalogo.SelectedValue;
				//Se hace  visible el panel
				Panel4.Visible=true;
		}

//Validar KITS necesarios para otorgar o rechazar la garantia
private bool validarMantenimientos()
	{
		double resul=1, tolerancia; 
		int i,j ,ok=0;
		DataSet ds = new DataSet();
		string  grupo = (string)ViewState["grupo"];
		
		//buscar todos los matenimientos programados para el grupo al que pertenece el vehiculo
		DBFunctions.Request(ds, IncludeSchema.NO, "Select  pman_kilometraje from  PMANTENIMIENTOPROGRAMADO where PGRU_CODIGO = '"+grupo+"' order by pman_kilometraje asc");	
		tolerancia = Convert.ToDouble ( DBFunctions.SingleData("Select cgar_kilotoler from CGARANTIA"));//porsentaje de tolerancia de kilometraje para otorgar la garantia
		tolerancia = (tolerancia/100)+1;
		//hallar los matenimientos programados que debe tener con el kilometraje 	que llega
		for (i=0; resul>=0 && i < ds.Tables[0].Rows.Count; i++ )
			resul =  Convert.ToDouble (tbKilometros.Text) - Convert.ToDouble(ds.Tables[0].Rows[i][0])*tolerancia;
		DataSet ds2 = new DataSet();
		//buscar si el vehiculo tiene esos mantenimientos
		string query;
		query = "select mo.pdoc_codigo as PREFIJO_ORDEN ,mo.mord_numeorde as NUM_ORDEN ,mo.mord_entrada as FECHA_ENTRADA,mo.mord_kilometraje as MORD_KILO,dk.pkit_codigo as CODIGO_KIT,pk.pkit_nombre AS NOMBRE_KIT , PM.PMAN_KILOMETRAJE  AS KILOMETRAJE from dbxschema.mordenDealer mo inner join dbxschema.mcatalogovehiculo mc on mo.pcat_codigo = mc.pcat_codigo and mo.mcat_vin = mc.mcat_vin inner join  dbxschema.mvehiculo mv on mv.mcat_vin = mc.mcat_vin inner join dbxschema.dordenkitDealer dk on mo.pdoc_codigo = dk.pdoc_codigo and mo.mord_numeorde = dk.mord_numeorde inner join dbxschema.pkit pk  on dk.pkit_codigo = pk.pkit_codigo inner join dbxschema.pmantenimientoprogramado pm on pm.pkit_codigo =  pk.pkit_codigo where mo.pcat_codigo = '"+lbPcatcodigo.Text+"' and mo.mcat_vin = '"+lb2VIN.Text+"' ";
		query= query + " UNION select mo.pdoc_codigo as  PREFIJO_ORDEN ,mo.mord_numeorde as NUM_ORDEN,mo.mord_entrada as FECHA_ENTRADA,mo.mord_kilometraje as MORD_KILO,dk.pkit_codigo as CODIGO_KIT ,pk.pkit_nombre AS NOMBRE_KIT , PM.PMAN_KILOMETRAJE  AS KILOMETRAJE from dbxschema.morden mo inner join dbxschema.mcatalogovehiculo mc on mo.pcat_codigo = mc.pcat_codigo and mo.mcat_vin = mc.mcat_vin inner join  dbxschema.mvehiculo mv on mv.mcat_vin = mc.mcat_vin inner join dbxschema.dordenkit dk on mo.pdoc_codigo = dk.pdoc_codigo and mo.mord_numeorde = dk.mord_numeorde inner join dbxschema.pkit pk  on dk.pkit_codigo = pk.pkit_codigo inner join dbxschema.pmantenimientoprogramado pm on pm.pkit_codigo =  pk.pkit_codigo where mo.pcat_codigo = '"+lbPcatcodigo.Text+"' and mo.mcat_vin = '"+lb2VIN.Text+"' ";
		DBFunctions.Request(ds2, IncludeSchema.NO, query);	
		dgKits.DataSource=ds2.Tables[0];
		DatasToControls.Aplicar_Formato_Grilla(dgKits);
		dgKits.DataBind();
	
		//si el kilometraje en que se aplicó el kit es <= al kilometraje en que deberia aplicarse el kit  tons....
		for( j=0;  j < ds2.Tables[0].Rows.Count; j++ )
		{
			if (Convert.ToDouble( ds2.Tables [0].Rows[j][3])<= Convert.ToDouble( ds2.Tables [0].Rows[j][6])*tolerancia)
			{
				ok++;
				((System.Web.UI.WebControls.Image)dgKits.Items[j].Cells[7].FindControl("imVale")).ImageUrl="../img/OK.gif";
			}
			else 	((System.Web.UI.WebControls.Image)dgKits.Items[j].Cells[7].FindControl("imVale")).ImageUrl="../img/EX.gif";
		}	
i--;
		if (ok>= i ) return true;
		else 
		{
			lbFaltaKit.Text="Debe tener  "+i+" Mantenimientos programados Válidos ";
			return false;}


	}

//caRga prefijo del documento
	[Ajax.AjaxMethod]
	public String cargar_consecutivo(string prefijo)
	{
		string numero = DBFunctions.SingleData("select pdoc_ultidocu from pdocumento where pdoc_codigo = '"+ddlPrefijo.SelectedValue+"' ");
		return numero;
	}

	# endregion


 

	}
}
