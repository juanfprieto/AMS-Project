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
	using System.Collections;
    using AMS.Documentos;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Garantias_Mantenimientos.
	///		Se registran las ordenes de trabajo Que realiza el dealer al vehiculo
	///		estas ordenes de trabajo solo son informativas para llevar un control de las operaciones ,
	///		repuestos y Kits que se aplican a un vehiculo
	///		Este registro se hace en cualquier  Distribuidor autorizado (DEALER) y es escencial que lo realicen 
	///		para el posterior otorgamiento de La garantia; de no ser asi no quedara registro de los MANTENIMIENTOS PROGRAMADOS
	///		y en consecuencia no se podrá otorgar una garantia.
	///		Estas ordenes no generan facturación ni afectán el inventario de repuestos, tampoco 
	///		influye en la planificación de operaciones del taller
	/// </summary>
	public partial class AMS_Garantias_Mantenimientos : System.Web.UI.UserControl
	{ 
		#region ATRIBUTOS
		protected System.Web.UI.WebControls.DataGrid kitsCompletos;
		protected System.Web.UI.WebControls.Panel Panel2;
		protected DataTable dtKits ,dtRepuestos, dtOperaciones;
		protected System.Web.UI.WebControls.Label Label1;
		protected ArrayList kitsAplicados;
		//protected Hashtable valoresAguardar;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				if(Request.QueryString["exito"]!=null)
					Utils.MostrarAlerta(Response, "Transacción Exitosa");

				if (DBFunctions.RecordExist("Select cgar_prefot from CGARANTIA"))
				{
					Session.Clear();
					lbPrefOrden.Text= DBFunctions.SingleData("Select cgar_prefot from CGARANTIA");	
	
					string nitdealer, ciudad;
					//*******carga los datos del dealer en el form**********
					string usuario=HttpContext.Current.User.Identity.Name;
					//nombre del usuario
					lblUsuario.Text= DBFunctions.SingleData("Select susu_nombre from DBXSCHEMA.SUSUARIO where susu_login='"+usuario+"'");
					//nit del usuario
					nitdealer  =Convert.ToString( DBFunctions.SingleData("Select mnit_nit  from DBXSCHEMA.SUSUARIO where susu_login='"+usuario+"'"));
					lblNitDealer.Text=nitdealer;
					if (nitdealer != null && nitdealer  != "")
					{
						lblCiudad.Text=  DBFunctions.SingleData("Select pciu_nombre from DBXSCHEMA.MCLIENTE, dbxschema.pciudad where pciu_codigo = pcui_codigo and mnit_nit ='"+nitdealer+"'");
						//ciudad del usuario
						ciudad = DBFunctions.SingleData("Select pcui_codigo from DBXSCHEMA.MCLIENTE where  mnit_nit ='"+nitdealer+"'");
				
						Panel1.Enabled=true;
						Panel3.Visible =false;
						Panel4.Visible =false;
						btGuardar.Visible=false;
						dtRepuestos = new DataTable();
						dtOperaciones= new DataTable();
						ArrayList kitsAplicados = new ArrayList();

						dtOperaciones.Columns.Add("ptem_operacion");
						dtOperaciones.Columns.Add("ptem_descripcion");
						dtOperaciones.Columns.Add("Valor");

						dtRepuestos.Columns.Add("Codigo");
						dtRepuestos.Columns.Add("ITEM");
						dtRepuestos.Columns.Add("cantidad");
						dtRepuestos.Columns.Add("ValorIt");
						dtRepuestos.Columns.Add("linea");

						dgItems.DataSource= dtRepuestos;
						dgOpers.DataSource= dtOperaciones;
						dgItems.DataBind();
						dgOpers.DataBind();
						DatasToControls.Aplicar_Formato_Grilla(dgItems);
						DatasToControls.Aplicar_Formato_Grilla(dgOpers);
						dgItems.Visible=false;
						dgOpers.Visible=false;
						Session["dtRepuestos"]= dtRepuestos;
						Session["dtOperaciones"]= dtOperaciones;
						Session["kitsAplicados"]= kitsAplicados;
					}
					else
					{  //El dealer debe tener registrado el nit en la tabla SUSUARIO para poder ingresar 
						Utils.MostrarAlerta(Response, " ERROR Debe asignar un Nit al usuario");
						Panel1.Visible=false;			
					}
				}
				else  Utils.MostrarAlerta(Response, " Deben configurarse los parametros de garantias");
			}
			else 
			{
				if (Session["dtKits"]!= null)
					dtKits=(DataTable)Session["dtKits"];	
				if (Session["dtRepuestos"]!= null)
					dtRepuestos=(DataTable)Session["dtRepuestos"];	
				if (Session["dtOperaciones"]!= null)
					dtOperaciones=(DataTable)Session["dtOperaciones"];
				if (Session["kitsAplicados"]!= null)
				 kitsAplicados= (ArrayList)Session["kitsAplicados"];
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
			this.dgKits.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgKits_ItemCommand);
			this.dgKits.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgKits_ItemDataBound);
			this.dgItems.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgItems_ItemCommand);
			this.dgOpers.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgOpers_ItemCommand);

		}
		#endregion

		#region EVENTOS
		//***valida que los campos sean correctos; la existencia del vehiculo; desabilita controles  y permite continuar************/
		protected void btConfirmar_Click(object sender, System.EventArgs e)
		{
			string NIT, color,ultikilo,grupo;
			DataSet PCatalogoVeh= new DataSet();
			DataSet MCatalogoVeh= new DataSet();	
			bool ban1= true;
			if (DBFunctions.RecordExist("Select pcat_codigo from mcatalogovehiculo where mcat_placa ='"+tbPlaca.Text+"'")) //validar si el vehiculo existe
			{ 
				DBFunctions.Request(MCatalogoVeh ,IncludeSchema.NO,"select mnit_nit, pcat_codigo, mcat_motor, pcol_codigo, mcat_anomode,mcat_numeultikilo from MCATALOGOVEHICULO  where mcat_placa ='"+tbPlaca.Text+"'");
				ultikilo = MCatalogoVeh.Tables[0].Rows[0][5].ToString();
							
				ban1= validarTiempo();	

				if (Convert.ToDouble (tbkilometraje.Text) < Convert.ToDouble (ultikilo) )		
				{
					ban1= false;//validar ultimo kilometraje registrado
                    Utils.MostrarAlerta(Response, " El Kilometraje no es el ultimo registrado");
				}
		
				if (ban1)
				{
					// Se cargan datos de McatalogoVehìculo
					
					NIT = MCatalogoVeh.Tables[0].Rows[0][0].ToString();
					lbPcatcodigo.Text=MCatalogoVeh.Tables[0].Rows[0][1].ToString();

					DBFunctions.Request(PCatalogoVeh,IncludeSchema.NO,"select pcat_descripcion, pmar_codigo,pgru_grupo from pcatalogovehiculo where pcat_codigo='"+lbPcatcodigo.Text+"'");
					tbCatalogo.Text=PCatalogoVeh.Tables[0].Rows[0][0].ToString();
					lbMarca.Text=PCatalogoVeh.Tables[0].Rows[0][1].ToString();
					grupo= PCatalogoVeh.Tables[0].Rows[0][2].ToString();
					ViewState["grupo"] = grupo;
	
					lbMotorV.Text=MCatalogoVeh.Tables[0].Rows[0][2].ToString();
					color=MCatalogoVeh.Tables[0].Rows[0][3].ToString();
	
					lbColorV.Text=DBFunctions.SingleData("Select pcol_descripcion from PCOLOR where pcol_codigo='"+color+"'");
					lbAModeloV.Text=MCatalogoVeh.Tables[0].Rows[0][4].ToString();

					//Se cargan los datos en pantalla
					tbCliente.Text=DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos from mnit where mnit_nit='"+NIT+"'");
					tbDoc.Text=NIT.ToString();
					//Se hace  visible el panel
					tbFechaEntrada.Enabled=false;
					tbFechaEntrega.Enabled=false;
					tbHoraEntrada.Enabled=false;
					tbHoraEntrega.Enabled=false;
					tbkilometraje.Enabled=false;
					tbMinutoEntrada .Enabled=false;
					tbMinutoEntrega.Enabled=false;
					tbNumorden.Enabled=false;
					tbPlaca.Enabled=false;
					Panel4.Visible=true;
					//	Panel1.Enabled=false;
				}
			}  
			else Utils.MostrarAlerta(Response, "Vehiculo no encontrado");
		}

		//**aborta el procedimiento**********************
		protected void btCancelar_Click(object sender, System.EventArgs e)
		{
			string indexpage = ConfigurationManager.AppSettings["MainIndexPage"];
			Response.Redirect(""+indexpage +"?process=Garantias.Mantenimientos");
		}
	
		//**llama metodo  que carga los kits para el grupo
		protected void btAceptar_Click(object sender, System.EventArgs e)
		{
			btAceptar.Enabled=false;
			btCancelar.Enabled=false;
			cargarKits();
			Panel3.Visible =true;
			Panel1.Visible=false;
			Panel4.Visible=false;
			dgItems.Visible=true;
			dgOpers.Visible=true;
			btGuardar.Visible=true;
		}

		//**Activa el **MODAL DIALOG** para ver el detalle de los KITS
		private void dgKits_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
				// Activa el modal dialog del detalle de los Kits
				((Label)e.Item.Cells[3].FindControl("lbVerKit")).Attributes.Add("onClick","ModalDialogVerKit ('"+dtKits.Rows[e.Item.DataSetIndex][0].ToString()+"');");				
			}
		}

		//selecciona el **KIT** y carga operaciones y mano de obra
		private void dgKits_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{	
			DataSet ds = new DataSet();
			if(e.CommandName== "SelectKit")
			{
				DBFunctions.Request(ds, IncludeSchema.NO, "Select  ptem_operacion,ptem_descripcion, 0 as Valor  from MKITOPERACION mk inner join ptempario pt on pt.ptem_operacion = mk.mkit_operacion where mkit_codikitoper='"+dtKits.Rows[e.Item.DataSetIndex][0].ToString()+"' ; Select  DBXSCHEMA.EDITARREFERENCIAS(MI.MITE_CODIGO,PLI.PLIN_TIPO) as Codigo, mi.mite_nombre as Item ,  0 as cantidad,  0 as ValorIt, pli.plin_codigo as linea from MKITITEM mk inner join MITEMS mi ON mi.mite_codigo = mk.mkit_coditem inner join PLINEAITEM PLI on   MI.PLIN_CODIGO=PLI.PLIN_CODIGO WHERE  mk.mkit_codikit = '"+dtKits.Rows[e.Item.DataSetIndex][0].ToString()+"'");
				
				cargarOpers(ds.Tables[0]);
				cargarRepuestos(ds.Tables[1] );
				kitsAplicados.Add(dtKits.Rows[e.Item.DataSetIndex][0].ToString());
				cargarValores();
				Session["kitsAplicados"]= kitsAplicados;
			}
		}
	
		//Agrega  **ITEMS** a la grilla individualmente y remueve items no deseados
		private void dgItems_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Codigo");
			dt.Columns.Add("ITEM");
			dt.Columns.Add("cantidad");
			dt.Columns.Add("ValorIt");
			dt.Columns.Add("linea");
			DataRow fila;
			fila=dt.NewRow();
			if(e.CommandName== "AgregarItem")
			{
				if(((TextBox)e.Item.Cells[0].FindControl("tbCodItem")).Text != "")
				{
					fila["Codigo"]=((TextBox)e.Item.Cells[0].FindControl("tbCodItem")).Text;
					fila["ITEM"]=((TextBox)e.Item.Cells[1].FindControl("tbCodItema")).Text ;

					dt.Rows.Add(fila);
					cargarValores();
					cargarRepuestos(dt);			
				}
			}
			else  if(e.CommandName== "BorrarItem")
			{	cargarValores();
				dtRepuestos.Rows[e.Item.ItemIndex].Delete();
				dtRepuestos.AcceptChanges();
				dgItems.DataSource=dtRepuestos;	
				for (int i =0 ; i< dtRepuestos.Rows.Count ; i++)
				{
					((TextBox)dgItems.Items[i].Cells[3].FindControl("tbValorItem")).Text = dtRepuestos.Rows[i][3].ToString();
					((TextBox)dgItems.Items[i].Cells[2].FindControl("tbCantItem")).Text = dtRepuestos.Rows[i][2].ToString();
				}
				dgItems.DataBind();
				Session["dtRepuestos"]=dtRepuestos;
			}
		}

		//Agrega **OPERACIONES** a la grilla individualmente y remueve items no deseados
		private void dgOpers_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("ptem_operacion");
			dt.Columns.Add("ptem_descripcion");
			dt.Columns.Add("Valor");
			DataRow fila;
			fila=dt.NewRow();
			if(e.CommandName== "AgregarOper")
			{
				if(((TextBox)e.Item.Cells[0].FindControl("tbCodOper")).Text != "")
				{
					fila["ptem_operacion"]=((TextBox)e.Item.Cells[0].FindControl("tbCodOper")).Text ;
					fila["ptem_descripcion"]=((TextBox)e.Item.Cells[1].FindControl("tbCodOpera")).Text ;
					//      	fila["Valor"]=((TextBox)e.Item.Cells[1].FindControl("tbValorIn")).Text ;
					dt.Rows.Add(fila);
					cargarValores();
					cargarOpers(dt);
					
				}			
			}
			else  if(e.CommandName== "BorrarOper")
			{	cargarValores();
				dtOperaciones.Rows[e.Item.ItemIndex].Delete();
				dtOperaciones.AcceptChanges();
				dgOpers.DataSource=dtOperaciones;		
				for (int i =0 ; i< dtOperaciones.Rows.Count ; i++)
					((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Text = dtOperaciones.Rows[i][2].ToString();
				dgOpers.DataBind();
		
				Session["dtOperaciones"]=dtOperaciones;
			}
		}

		//guarda la orden de trabajo
		protected void btGuardar_Click(object sender, System.EventArgs e)
		{
			bool ban= true; 
			ArrayList query = new ArrayList();
			string str="";
			string vin = DBFunctions.SingleData("Select mcat_vin from MCATALOGOVEHICULO where MCAT_PLACA = '"+tbPlaca.Text+"'");
			string  horaentrada = (string)ViewState["horaentrada"];
			string  horaentrega = (string)ViewState["horaentrega"];


			query.Add("UPDATE  mcatalogovehiculo  SET  mcat_numeultikilo = "+Convert.ToInt32(tbkilometraje.Text)+"  WHERE MCAT_VIN = '"+vin+"'");
			query.Add("insert into MORDENDEALER  values ("+Convert.ToInt32(tbNumorden.Text )+", '"+lbPrefOrden.Text+"', '"+lblNitDealer.Text+"', '"+lbPcatcodigo.Text+"', '"+vin+"' , '"+tbDoc.Text+"','P', '"+tbFechaEntrada.Text+"','"+horaentrada+"' ,  '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+tbFechaEntrega.Text+"', '"+horaentrega+"', '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+Convert.ToInt32(tbkilometraje.Text)+", '"+obsrCliente.Text+"','"+obsrRecep.Text+"') ");		
			for (int i = 0 ; i< dtOperaciones.Rows.Count; i++)
			{	
				if (((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Text != "")
					query.Add("insert into DORDENOPERACIONDEALER  values ( '"+lbPrefOrden.Text+"',"+Convert.ToInt32(tbNumorden.Text )+", '"+lblNitDealer.Text+"','"+dtOperaciones.Rows[i][0]+"' , null, null,null, null, "+Convert.ToDouble(((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Text)+" )");
				else 
				{
                    Utils.MostrarAlerta(Response, "Debe ingresar valor de las operaciones");
					ban= false;
					break;
				}
			}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (ban)
			{
				for (int i = 0 ; i< dtRepuestos.Rows.Count; i++)
				{	
					if (((TextBox)dgItems.Items[i].Cells[2].FindControl("tbValorItem")).Text != "")
					{
						Referencias.Guardar(dtRepuestos.Rows[i][0].ToString(), ref str, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtRepuestos.Rows[i][4].ToString()+"'"));
						query.Add("insert into DORDENITEMSDEALER values (DEFAULT, '"+lbPrefOrden.Text+"',"+Convert.ToInt32(tbNumorden.Text )+", '"+lblNitDealer.Text+"','"+str+"' ,"+Convert.ToDouble(((TextBox)dgItems.Items[i].Cells[2].FindControl("tbCantItem")).Text)+", "+Convert.ToDouble(((TextBox)dgItems.Items[i].Cells[2].FindControl("tbValorItem")).Text)+",  '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', '"+DateTime.Now.ToString("yyyy-MM-dd")+"' )");
					}
					else 
					{
                        Utils.MostrarAlerta(Response, "Debe ingresar valor de los Repuestos");
						ban= false;
						break;
					}
				}
			}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////			
			if (ban)
			{
				for (int i = 0; i<  kitsAplicados.Count; i++)			
				 query.Add("insert into DORDENKITDEALER  values ('"+lbPrefOrden.Text+"', "+Convert.ToInt32(tbNumorden.Text )+", '"+kitsAplicados[i]+"', '"+lblNitDealer.Text+"') ");		

				if (DBFunctions.Transaction(query))
				{
					string indexpage = ConfigurationManager.AppSettings["MainIndexPage"];
					Response.Redirect(""+indexpage +"?process=Garantias.Mantenimientos&exito=1");
				}

				else lb.Text= DBFunctions.exceptions;
			}
		}

		#endregion

		#region METODOS
		//Valida el tiempo de entrada y salida del vehiculo al taller
		private bool validarTiempo()
		{	
			string horaentrada= tbHoraEntrada.Text +":"+tbMinutoEntrada.Text + ":"+"00";
			string horaentrega= tbHoraEntrega.Text +":"+tbMinutoEntrega.Text + ":"+"00";
			
	
			if (!(DatasToControls.ValidarDateTime(horaentrada) && DatasToControls.ValidarDateTime(horaentrega)))
				//	if(Convert.ToInt32( tbHoraEntrada.Text )>12  || Convert.ToInt32( tbHoraEntrada.Text )<1 ||  Convert.ToInt32(tbHoraEntrega.Text)>12 || Convert.ToInt32(tbHoraEntrega.Text)<1  || Convert.ToInt32(tbMinutoEntrada.Text)>=59 || Convert.ToInt32(tbMinutoEntrada.Text) < 0 || Convert.ToInt32(tbMinutoEntrega.Text)>=59 || Convert.ToInt32(tbMinutoEntrega.Text)<0)
			{	
                Utils.MostrarAlerta(Response, "Error en  formato de Horas");
				return false;// validar fecha de entrega
			}
			horaentrada= Convert.ToDateTime(horaentrada).ToString("HH:mm:ss");
			horaentrega= Convert.ToDateTime(horaentrega).ToString("HH:mm:ss");
			ViewState["horaentrada"] = horaentrada;
			ViewState["horaentrega"] = horaentrega;
		
			if (!(DatasToControls.ValidarDateTime(tbFechaEntrada.Text) &&  DatasToControls.ValidarDateTime(tbFechaEntrega.Text)))
			{
                Utils.MostrarAlerta(Response, "Error en formato de Fechas");
				return false;// validar fechas
			}

			if ( Convert.ToDateTime(tbFechaEntrada.Text) > Convert.ToDateTime(tbFechaEntrega.Text))
			{
                Utils.MostrarAlerta(Response, "La Fecha de Entrega debe ser Mayor o Igual a la de Fecha de Entrada");
				return false;// validar fecha de entrega
			}
			else if  ( Convert.ToDateTime(tbFechaEntrada.Text) == Convert.ToDateTime(tbFechaEntrega.Text))
			{
				if (Convert.ToInt32(tbHoraEntrada.Text) > Convert.ToInt32(tbHoraEntrega.Text))
				{
                    Utils.MostrarAlerta(Response, "La Hora de Entrega debe ser Mayor a la hora de Entrada");
					return false;
				}
				else if  (Convert.ToInt32(tbHoraEntrada.Text) ==  Convert.ToInt32(tbHoraEntrega.Text))
				{
					if (Convert.ToInt32(tbMinutoEntrada.Text)>=Convert.ToInt32(tbMinutoEntrega.Text))
					{
                        Utils.MostrarAlerta(Response, "La Hora de Entrega debe ser Mayor a la hora de Entrada");
						return false;
					}
					else return true;
				}
				else return true;
			}
			else return true;
		}

		//Carga Kits corespondientes a un grupo de catalogo y pinta grilla
		private void cargarKits()
		{
			string  grupo = (string)ViewState["grupo"];
			DataSet ds = new DataSet();
			DBFunctions.Request(ds, IncludeSchema.NO, "Select  pkit_codigo, pkit_nombre  from PKIT where pgru_grupo = '"+grupo+"'");
			dtKits= ds.Tables[0];
			dgKits.DataSource=dtKits;		
			DatasToControls.Aplicar_Formato_Grilla(dgKits);
			dgKits.DataBind();
			Session["dtKits"]=dtKits;//asigna sesion ' EJ: guarda datatable'
		}

		//Cargar  operaciones Correspondientes al Kit seleccionado sin reperir las ya incluidas
		private void cargarOpers(DataTable oper)
		{
			if (dtOperaciones == null)
				dtOperaciones= new DataTable();

			if (dtOperaciones.Rows.Count==0) //si la tabala de repuestos esta vacia 
			{
				dtOperaciones= oper;
				dgOpers.DataSource=dtOperaciones;		
				DatasToControls.Aplicar_Formato_Grilla(dgOpers);
				dgOpers.DataBind();
				Session["dtOperaciones"]=dtOperaciones;//asigna sesion ' EJ: guarda datatable'
			}
			else 
			{
				string c1= dtOperaciones.Columns[0].ColumnName.ToString();
				string c2= dtOperaciones.Columns[1].ColumnName.ToString();
				DataRow[] rep  = null;
				for (int i = 0 ; i< oper.Rows.Count; i++)
				{	
					rep= dtOperaciones.Select("ptem_operacion='"+oper.Rows[i][0].ToString()+"'");
					if (rep.Length == 0)
					{
						DataRow fila;
						fila=dtOperaciones.NewRow();
						fila["ptem_operacion"]=oper.Rows[i][0].ToString();
						fila["ptem_descripcion"]=oper.Rows[i][1].ToString();
						dtOperaciones.Rows.Add(fila);
					}
				}
				Session["dtOperaciones"] = dtOperaciones;
				dgOpers.DataSource = dtOperaciones;
				DatasToControls.Aplicar_Formato_Grilla(dgOpers);
				dgOpers.DataBind();		
			}
		}

		//lo mismo de cargarOpers pero con los repuestos
		private void cargarRepuestos (DataTable Repuestos)
		{ 
			if (dtRepuestos == null)
				dtRepuestos= new DataTable();

			if (dtRepuestos.Rows.Count==0) //si la tabala de repuestos esta vacia 
			{
				dtRepuestos= Repuestos;
				dgItems.DataSource=dtRepuestos;		
				DatasToControls.Aplicar_Formato_Grilla(dgItems);
				dgItems.DataBind();
				Session["dtRepuestos"]=dtRepuestos;//asigna sesion ' EJ: guarda datatable'
			}
			else 
			{ 
				DataRow[] rep  = null;
				for (int i = 0 ; i< Repuestos.Rows.Count; i++)
				{	
					rep= dtRepuestos.Select("Codigo='"+Repuestos.Rows[i][0].ToString()+"'");
					if (rep.Length == 0)
					{
						DataRow fila;
						fila=dtRepuestos.NewRow();
						fila["Codigo"]=Repuestos.Rows[i][0].ToString();
						fila["ITEM"]=Repuestos.Rows[i][1].ToString();
						fila["linea"]=Repuestos.Rows[i][4].ToString();
						dtRepuestos.Rows.Add(fila);
					}
				}
				Session["dtRepuestos"] = dtRepuestos;
				dgItems.DataSource = dtRepuestos;
				DatasToControls.Aplicar_Formato_Grilla(dgItems);
				dgItems.DataBind();
			
			}
		}

		//llena y guarda los Valor unitario de los repuestos y operaciones
		private void cargarValores()
		{
			for (int i =0 ; i< dgOpers.Items.Count ; i++)
			{
				if (((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Text == "")
					((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Text= "0";
				dtOperaciones.Rows[i][2]=((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Text; 
			}

			for (int i =0 ; i< dgItems.Items.Count ; i++)
			{
				if(((TextBox)dgItems.Items[i].Cells[3].FindControl("tbValorItem")).Text == "")
				{
					((TextBox)dgItems.Items[i].Cells[3].FindControl("tbValorItem")).Text="0";
					((TextBox)dgItems.Items[i].Cells[3].FindControl("tbCantItem")).Text="0";
				}
				dtRepuestos.Rows[i][3]=((TextBox)dgItems.Items[i].Cells[3].FindControl("tbValorItem")).Text; 
				dtRepuestos.Rows[i][2]=((TextBox)dgItems.Items[i].Cells[2].FindControl("tbCantItem")).Text; 
			}

		}

		#endregion

		

	

	}
}
