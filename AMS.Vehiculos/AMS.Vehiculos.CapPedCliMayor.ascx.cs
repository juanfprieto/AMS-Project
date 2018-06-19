using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.CriptoServiceProvider;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using AMS.Utilidades;
using Ajax;
using AMS.Tools;
namespace AMS.Vehiculos
{
	/// <summary>
	///		Descripción breve de AMS_Vehiculos_CapPedCliMayor.
	/// </summary>
	public partial class AMS_Vehiculos_CapPedCliMayor : System.Web.UI.UserControl
	{
		#region controles, variables
		protected DataTable tablaPedidosM;
		protected string path=ConfigurationManager.AppSettings["PathToPreviews"];
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		#region Eventos Control
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Vehiculos_CapPedCliMayor));	
			Page.SmartNavigation=true;
			if(!IsPostBack)
			{
				tipoDocu.Text = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"'");
				if(Request.QueryString["tip"]=="new")
				{
					fechaPedido.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
					if(Request.QueryString["cons"]=="A")
					{
						idPedido.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"'");
						idPedido.Enabled = false;
					}
					this.Preparar_Tabla_Pedido();
					Session["tablaPedidosM"] = tablaPedidosM;
					this.Bind_dgInserts();
					btnGuardar.Visible = true;
				}
				else if(Request.QueryString["tip"]=="old")
				{
					idPedido.Text = Request.QueryString["num"];
					idPedido.Enabled = false;
					observacion.Text = DBFunctions.SingleData("SELECT mped_observacion FROM mpedidovehiculoclientemayor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+Request.QueryString["num"]+"");
					txtNoPedido.Text = DBFunctions.SingleData("SELECT mped_numepedi_original FROM mpedidovehiculoclientemayor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+Request.QueryString["num"]+"");
					fechaPedido.Text = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mped_pedido FROM mpedidovehiculoclientemayor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+Request.QueryString["num"]+"")).ToString("yyyy-MM-dd");
					nitCliente.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculoclientemayor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+Request.QueryString["num"]+"");
					nitClientea.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_apellidos FROM mnit WHERE mnit_nit='"+nitCliente.Text+"'");
					txtCodVendedor.Text = DBFunctions.SingleData("SELECT pven_codigo FROM mpedidovehiculoclientemayor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+Request.QueryString["num"]+"");
					txtCodVendedora.Text = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+txtCodVendedor.Text+"';");
					DataSet dsCliente=TraerClienteDs(nitCliente.Text);
					if(dsCliente.Tables[0].Rows.Count>0)
					{
						nitDireccion.Text=dsCliente.Tables[0].Rows[0]["DIRECCION"].ToString();
						nitCiudad.Text=dsCliente.Tables[0].Rows[0]["CIUDAD"].ToString();
						nitTelefono.Text=dsCliente.Tables[0].Rows[0]["TELEFONO"].ToString();
						txtSaldoCartera.Text=dsCliente.Tables[0].Rows[0]["SALDO"].ToString();
						txtCupo.Text=dsCliente.Tables[0].Rows[0]["CUPO"].ToString();
					}
					this.Cargar_Datos_Pedido_Existente(Request.QueryString["tipoDocu"],Request.QueryString["num"]);
					btnEditar.Visible = true;
				}
			}
			if(Session["tablaPedidosM"] == null)
				this.Preparar_Tabla_Pedido();
			else
				tablaPedidosM= (DataTable)Session["tablaPedidosM"];
		}
		
		#endregion
		
		#region AJAX
		[Ajax.AjaxMethod]
		public DataSet TraerCliente(string nitCliente)
		{
			return(TraerClienteDs(nitCliente));
		}
		public DataSet TraerClienteDs(string nitCliente)
		{
			DataSet Vins= new DataSet();
			double saldoCartera =Clientes.ConsultarSaldo(nitCliente);
			double saldoCarteraM=Clientes.ConsultarSaldoMora(nitCliente);
			DBFunctions.Request(Vins,IncludeSchema.NO,"select mn.mnit_direccion AS DIRECCION,pc.pciu_nombre AS CIUDAD,mn.mnit_telefono AS TELEFONO,'"+saldoCartera.ToString("#,##0")+"' AS SALDO,'"+saldoCarteraM.ToString("#,##0")+"' AS SALDOMORA, MCLI_CUPOCRED AS CUPO from mnit mn, mcliente mc, pciudad pc where mn.pciu_codigo=pc.pciu_codigo and mc.mnit_nit=mn.mnit_nit and mn.mnit_nit='"+nitCliente+"';");
			return Vins;
		}
		#endregion

		#region Metodos Especiales
		protected void Preparar_Tabla_Pedido()
		{
			tablaPedidosM = new DataTable();
			tablaPedidosM.Columns.Add(new DataColumn("CATALOGO",System.Type.GetType("System.String")));//0
			tablaPedidosM.Columns.Add(new DataColumn("COLOR",System.Type.GetType("System.String")));//1
			tablaPedidosM.Columns.Add(new DataColumn("COLORALTER",System.Type.GetType("System.String")));//2
			tablaPedidosM.Columns.Add(new DataColumn("CANTPED",System.Type.GetType("System.Int32")));//3
			tablaPedidosM.Columns.Add(new DataColumn("VALUNIT",System.Type.GetType("System.Double")));//4
			tablaPedidosM.Columns.Add(new DataColumn("FECHALLEGADA",System.Type.GetType("System.String")));//4
		}
		#endregion
		
		#region Grilla Pedido
		protected void Bind_dgInserts()
		{
			Session["tablaPedidosM"] = tablaPedidosM;
			dgInserts.DataSource = tablaPedidosM;
			dgInserts.DataBind();
			for(int i=0;i<dgInserts.Items.Count;i++)
			{
				dgInserts.Items[i].Cells[3].HorizontalAlign = HorizontalAlign.Right;
				dgInserts.Items[i].Cells[4].HorizontalAlign = HorizontalAlign.Right;
			}
			Total();
			if(tablaPedidosM.Rows.Count != 0)
			{
				btnGuardar.Enabled = true;
			}
			else
			{
				btnGuardar.Enabled = false;
			}
		}
		
		protected void DgInserts_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Footer)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[1].Controls[1]),"SELECT pcol_codigo,pcol_descripcion FROM pcolor order by pcol_descripcion ");
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]),"SELECT pcol_codigo,pcol_descripcion FROM pcolor order by pcol_descripcion ");
			}
		}
 		
		protected void DgInserts_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValues(e))
			{
				DataRow fila = tablaPedidosM.NewRow();
				fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
				fila[1] = ((DropDownList)e.Item.Cells[1].Controls[1]).SelectedValue;
				fila[2] = ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue;
				fila[3] = System.Convert.ToInt32(((TextBox)e.Item.Cells[3].Controls[1]).Text);
				fila[4] = System.Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
				fila[5] = ((TextBox)e.Item.Cells[5].Controls[1]).Text;
				tablaPedidosM.Rows.Add(fila);
			}
			if(((Button)e.CommandSource).CommandName == "DelDatasRow")
			{
				try
				{
					tablaPedidosM.Rows.Remove(tablaPedidosM.Rows[e.Item.ItemIndex]);
				}
				catch
				{
					tablaPedidosM.Rows.Clear();
				}
			}
			Bind_dgInserts();
		}
		
		public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
		{
			dgInserts.EditItemIndex = (int)e.Item.ItemIndex;
			Bind_dgInserts();
		}
    	
		public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
		{
			tablaPedidosM.Rows[dgInserts.EditItemIndex][0] = ((TextBox)e.Item.FindControl("catalogoEdit")).Text;
			tablaPedidosM.Rows[dgInserts.EditItemIndex][1] = ((TextBox)e.Item.FindControl("colorEdit")).Text;
			tablaPedidosM.Rows[dgInserts.EditItemIndex][2] = ((TextBox)e.Item.FindControl("colorAlterEdit")).Text;
			tablaPedidosM.Rows[dgInserts.EditItemIndex][3] = ((TextBox)e.Item.FindControl("cantidadPedidaEdit")).Text;
			tablaPedidosM.Rows[dgInserts.EditItemIndex][4] = ((TextBox)e.Item.FindControl("valorUnitarioEdit")).Text;
			tablaPedidosM.Rows[dgInserts.EditItemIndex][5] = ((TextBox)e.Item.FindControl("txtFechaEdit")).Text;
			dgInserts.EditItemIndex = -1;
			Bind_dgInserts();
		}
    	
		public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgInserts.EditItemIndex = -1;
			Bind_dgInserts();
		}
		
		protected bool Validar_Valores_Grilla()
		{
			bool validador = true;
			for(int i=0;i<tablaPedidosM.Rows.Count;i++)
			{
				for(int j=i+1;j<tablaPedidosM.Rows.Count;j++)
				{
					if((tablaPedidosM.Rows[i][0].ToString()==tablaPedidosM.Rows[j][0].ToString())&&(tablaPedidosM.Rows[i][1].ToString()==tablaPedidosM.Rows[j][1].ToString()))
						validador = false;
				}
			}
			return validador;
		}
				
		protected bool CheckValues(DataGridCommandEventArgs e)
		{
			bool check = true;
			if(((TextBox)e.Item.Cells[0].Controls[1]).Text=="" || ((DropDownList)e.Item.Cells[1].Controls[1]).Items.Count==0 || ((DropDownList)e.Item.Cells[2].Controls[1]).Items.Count==0)
				check = false;
			if(((TextBox)e.Item.Cells[3].Controls[1]).Text=="" || ((TextBox)e.Item.Cells[4].Controls[1]).Text=="")
				check = false;
			return check;
		}
		
		protected void Total()
		{
			double total = 0;
			for(int i=0;i<tablaPedidosM.Rows.Count;i++)
				total += Convert.ToDouble(tablaPedidosM.Rows[i][4])*System.Convert.ToInt32(tablaPedidosM.Rows[i][3]);
			totalPedido.Text = total.ToString("C");
		}
		
		protected void Cargar_Datos_Pedido_Existente(string prefijoPedido, string numeroPedido)
		{
			Preparar_Tabla_Pedido();
			//Debemos traer los valores que se encuentran en dpedidovehiculoclientemayor
			DataSet elementosPedido = new DataSet();
			DBFunctions.Request(elementosPedido,IncludeSchema.NO,"SELECT pcat_codigo, pcol_codigo, pcol_codigoalte, dped_cantpedi, dped_valounit,dped_fechallegada FROM dpedidovehiculoclientemayor WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			for(int i=0;i<elementosPedido.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaPedidosM.NewRow();
				fila[0] = elementosPedido.Tables[0].Rows[i][0].ToString();
				fila[1] = elementosPedido.Tables[0].Rows[i][1].ToString();
				fila[2] = elementosPedido.Tables[0].Rows[i][2].ToString();
				fila[3] = Convert.ToInt32(elementosPedido.Tables[0].Rows[i][3]);
				fila[4] = Convert.ToDouble(elementosPedido.Tables[0].Rows[i][4]);
				try {fila[5] = Convert.ToDateTime(elementosPedido.Tables[0].Rows[i][5]).ToString("yyyy-MM-dd");} 
				catch {fila[5]="";}
				tablaPedidosM.Rows.Add(fila);
			}
			Bind_dgInserts();
		}

		#endregion

		#region Eventos Botones
		protected void Guardar_Pedido(Object  Sender, EventArgs e)
		{
			//Ahora debemos comprobar que el se halla escogido un nit
			if(nitCliente.Text == ""){
                Utils.MostrarAlerta(Response, "Debe seleccionar el Cliente");
				return;
			}
			if(txtCodVendedor.Text == ""){
                Utils.MostrarAlerta(Response, "Debe seleccionar el Vendedor");
				return;
			}
			if(txtNoPedido.Text == ""){
                Utils.MostrarAlerta(Response, "Debe ingresar el no. de Pedido");
				return;
			}
			if(!Validar_Valores_Grilla()){
                Utils.MostrarAlerta(Response, "Existen catálogos con el mismo color repetidos");
				return;
			}
			int ano=Convert.ToInt16(DBFunctions.SingleData("select PANO_ANO from CVEHICULOS;"));
			int mes=Convert.ToInt16(DBFunctions.SingleData("select PMES_MES from CVEHICULOS;"));
			DateTime fchAct=Convert.ToDateTime(fechaPedido.Text);
			if(ano!=fchAct.Year || mes!=fchAct.Month)
			{
                Utils.MostrarAlerta(Response, "Fecha no vigente!");
				return;
			}
			//Ahora comprobamos que este pedido no exista 
			if(DBFunctions.RecordExist("SELECT * FROM mpedidovehiculoclientemayor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+idPedido.Text+""))
                Utils.MostrarAlerta(Response, "Este pedido ya existe");
			else
			{
				PedidoClienteMayor miPedido = new PedidoClienteMayor(tablaPedidosM);
				miPedido.PrefijoPedido = Request.QueryString["tipoDocu"];
				miPedido.NumeroPedido = idPedido.Text;
				miPedido.FechaPedido = fechaPedido.Text;
				miPedido.NitCliente = nitCliente.Text;
				miPedido.Observacion = observacion.Text;
				miPedido.CodigoVendedor = txtCodVendedor.Text;
				miPedido.NumeroPedidoOriginal = txtNoPedido.Text.Trim();
				if(miPedido.Grabar_Pedido(Request.QueryString["cons"])){
					DBFunctions.NonQuery("drop view dbxschema.vvehiculos_solicitudpedidosvehiculosmayor_r");
					DBFunctions.NonQuery("CREATE VIEW DBXSCHEMA.VVEHICULOS_SOLICITUDPEDIDOSVEHICULOSMAYOR_R AS select * from dbxschema.VVEHICULOS_SOLICITUDPEDIDOSVEHICULOSMAYOR where pref_ped='"+miPedido.PrefijoPedido+"' and num_ped="+miPedido.NumeroPedido+" ");
					dgInserts.Enabled=false;
					btnGuardar.Visible=false;
                    Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClienteMayor&pref=" + miPedido.PrefijoPedido + "&num=" + miPedido.NumeroPedido);
				}
				else
					lb.Text = miPedido.ProcessMsg;
			}
		}
		
		protected void Editar_Pedido(Object  Sender, EventArgs e)
		{
			//Ahora debemos comprobar que el se halla escogido un nit
			if(nitCliente.Text == ""){
                Utils.MostrarAlerta(Response, "Debe seleccionar el Cliente");
				return;
			}
			if(txtCodVendedor.Text == ""){
                Utils.MostrarAlerta(Response, "Debe seleccionar el Vendedor");
				return;
			}
			if(txtNoPedido.Text == ""){
                Utils.MostrarAlerta(Response, "Debe ingresar el no. de Pedido");
				return;
			}
			if(!Validar_Valores_Grilla()){
                Utils.MostrarAlerta(Response, "Existen catálogos con el mismo color repetidos");
				return;
			}
			PedidoClienteMayor miPedido = new PedidoClienteMayor(tablaPedidosM);
			miPedido.PrefijoPedido = Request.QueryString["tipoDocu"];
			miPedido.NumeroPedido = idPedido.Text;
			miPedido.FechaPedido = fechaPedido.Text;
			miPedido.NitCliente = nitCliente.Text;
			miPedido.Observacion = observacion.Text;
			miPedido.CodigoVendedor = txtCodVendedor.Text;
			miPedido.NumeroPedidoOriginal = txtNoPedido.Text.Trim();
			if(miPedido.Editar_Pedido())
				Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClienteMayor&path="+Request.QueryString["path"]);
			else
				lb.Text = miPedido.ProcessMsg;
		}

		protected void generarInforme()
		{
			Label lbvacio= new Label();
			string[] Formulas = new string[8];
			string[] ValFormulas = new string[8];
			string header = "AMS_HEADER.rpt";
			string footer = "AMS_FOOTER.rpt";
			DataSet tempDS = new DataSet();
			Formulas[0] = "CLIENTE";
			Formulas[1] = "NIT";
			Formulas[2] = "TITULO";
			Formulas[3] = "TITULO1";
			Formulas[4] = "SELECCION1";
			Formulas[5] = "SELECCION2";
			Formulas[6] = "VERSION";
			Formulas[7] = "REPORTE";

			string empresa= DBFunctions.SingleData("select  cemp_nombre from dbxschema.cempresa");
			ValFormulas[0] = ""+empresa+""; //nombre empresa
			string nit= DBFunctions.SingleData("select  mnit_nit from dbxschema.cempresa");
			
			DataSet datosReporte= new DataSet();
			ValFormulas[1] = ""+nit+"" ;
			ValFormulas[2] = "SOLICITUD DE PEDIDO VEHICULOS CLIENTES MAYOR "; //titulo rpt
			ValFormulas[3] = "SISTEMA DE VEHICULOS"; //subtitulo Sistema de Nomina
			ValFormulas[4] = " "; //año mes quince
			ValFormulas[5] = ""; //
			ValFormulas[6] = "ECAS - AMS VER 3.0.1";

			Imprimir funcion=new Imprimir();
			string servidor=ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
			string database=ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
			string usuario=ConfigurationManager.AppSettings["UID"];
			string password=ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];
			AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
			miCripto.IV=ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
			miCripto.Key=ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
			string newPwd=miCripto.DescifrarCadena(password);
            string nomA = "Solicitud Vehiculos Mayor";
			funcion.PreviewReport2("AMS.Vehiculos.SolicitudDePedidosVehiculosMayor",header,footer,1,1,1,"","",nomA,"pdf",usuario,newPwd,Formulas,ValFormulas,lbvacio);
            Response.Write("<script language:javascript>w=window.open('" + funcion.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
            //Utils.MostrarAlerta(Response, "Pedido creado correctamente!");
            //funcion.ReportUnload();
            hl1.NavigateUrl=funcion.Documento;
            hl1.Visible=true;
		}
		
		#endregion

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
	}
}
