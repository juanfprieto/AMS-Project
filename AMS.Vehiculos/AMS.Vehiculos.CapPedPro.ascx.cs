// created on 16/03/2005 at 16:17
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
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class CadPedPro : System.Web.UI.UserControl
	{
		protected string path=ConfigurationManager.AppSettings["PathToPreviews"];
		protected DataTable tablaPedidos;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
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
					Session["tablaPedidos"] = tablaPedidos;
					this.Bind_dgInserts();
					btnGuardar.Visible = true;
				}
				else if(Request.QueryString["tip"]=="old")
				{
					idPedido.Text = Request.QueryString["num"];
					idPedido.Enabled = false;
					observacion.Text = DBFunctions.SingleData("SELECT mped_observacion FROM mpedidovehiculoproveedor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+Request.QueryString["num"]+"");
					fechaPedido.Text = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mped_pedido FROM mpedidovehiculoproveedor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+Request.QueryString["num"]+"")).ToString("yyyy-MM-dd");
					nitProveedor.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculoproveedor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+Request.QueryString["num"]+"");
					nitProveedora.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_apellidos FROM mnit WHERE mnit_nit='"+nitProveedor.Text+"'");
					this.Cargar_Datos_Pedido_Existente(Request.QueryString["tipoDocu"],Request.QueryString["num"]);
					btnEditar.Visible = true;
				}
			}
			if(Session["tablaPedidos"] == null)
				this.Preparar_Tabla_Pedido();
			else
				tablaPedidos= (DataTable)Session["tablaPedidos"];
		}
		
		protected void Preparar_Tabla_Pedido()
		{
			tablaPedidos = new DataTable();
			tablaPedidos.Columns.Add(new DataColumn("CATALOGO",System.Type.GetType("System.String")));//0
			tablaPedidos.Columns.Add(new DataColumn("COLOR",System.Type.GetType("System.String")));//1
			tablaPedidos.Columns.Add(new DataColumn("COLORALTER",System.Type.GetType("System.String")));//2
			tablaPedidos.Columns.Add(new DataColumn("CANTPED",System.Type.GetType("System.Int32")));//3
			tablaPedidos.Columns.Add(new DataColumn("VALUNIT",System.Type.GetType("System.Double")));//4
			tablaPedidos.Columns.Add(new DataColumn("FECHALLEGADA",System.Type.GetType("System.String")));//4
		}
		
		protected void Bind_dgInserts()
		{
			Session["tablaPedidos"] = tablaPedidos;
			dgInserts.DataSource = tablaPedidos;
			dgInserts.DataBind();
			for(int i=0;i<dgInserts.Items.Count;i++)
			{
				dgInserts.Items[i].Cells[3].HorizontalAlign = HorizontalAlign.Right;
				dgInserts.Items[i].Cells[4].HorizontalAlign = HorizontalAlign.Right;
			}
			Total();
			if(tablaPedidos.Rows.Count != 0)
			{
				idPedido.Enabled = nitProveedor.Enabled = nitProveedora.Enabled = false;
				btnGuardar.Enabled = true;
			}
			else
			{
				idPedido.Enabled = nitProveedor.Enabled = nitProveedora.Enabled = true;
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
        		//Revisamos si el nit de proveedor existe o no
        		if(DBFunctions.RecordExist("SELECT * FROM mproveedor WHERE mnit_nit='"+nitProveedor.Text+"'"))
        		{
        			DataRow fila = tablaPedidos.NewRow();
	        		fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
        			fila[1] = ((DropDownList)e.Item.Cells[1].Controls[1]).SelectedValue;
        			fila[2] = ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue;
        			fila[3] = System.Convert.ToInt32(((TextBox)e.Item.Cells[3].Controls[1]).Text);
    	    		fila[4] = System.Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
					fila[5] = ((TextBox)e.Item.Cells[5].Controls[1]).Text;
	        		tablaPedidos.Rows.Add(fila);
        		}
        		else
        		{
                    Utils.MostrarAlerta(Response, "El nit de proveedor ingresado no es valido");
        			return;
        		}
        	}
        	if(((Button)e.CommandSource).CommandName == "DelDatasRow")
        	{
        		try
        		{
        			tablaPedidos.Rows.Remove(tablaPedidos.Rows[e.Item.ItemIndex]);
        		}
        		catch
        		{
        			tablaPedidos.Rows.Clear();
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
    		tablaPedidos.Rows[dgInserts.EditItemIndex][0] = ((TextBox)e.Item.FindControl("catalogoEdit")).Text;
    		tablaPedidos.Rows[dgInserts.EditItemIndex][1] = ((TextBox)e.Item.FindControl("colorEdit")).Text;
    		tablaPedidos.Rows[dgInserts.EditItemIndex][2] = ((TextBox)e.Item.FindControl("colorAlterEdit")).Text;
    		tablaPedidos.Rows[dgInserts.EditItemIndex][3] = ((TextBox)e.Item.FindControl("cantidadPedidaEdit")).Text;
    		tablaPedidos.Rows[dgInserts.EditItemIndex][4] = ((TextBox)e.Item.FindControl("valorUnitarioEdit")).Text;
			tablaPedidos.Rows[dgInserts.EditItemIndex][5] = ((TextBox)e.Item.FindControl("txtFechaEdit")).Text;
    		dgInserts.EditItemIndex = -1;
    		Bind_dgInserts();
    	}
    	
    	public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
    	{
    		dgInserts.EditItemIndex = -1;
    		Bind_dgInserts();
    	}
		
		
		protected void Guardar_Pedido(Object  Sender, EventArgs e)
		{
			//Ahora debemos comprobar que el se halla escogido un nit
			if(nitProveedor.Text == "")
                Utils.MostrarAlerta(Response, "Debe Seleccionar el Nit del Proveedor");
			else
			{
				//Debemos comprobar que no se encuentren registros repetidos dentro de la grilla
				//if(!Validar_Valores_Grilla())
				//	Response.Write("<script language:javascript>alert('Items Redundantes en Pedido');</script>");
				//Cambio para que se repitan valores en la grilla. 
				
			
				
					//Ahora comprobamos que este pedido no exista 
					if(DBFunctions.RecordExist("SELECT * FROM mpedidovehiculoproveedor WHERE pdoc_codigo='"+Request.QueryString["tipoDocu"]+"' AND mped_numepedi="+idPedido.Text+""))
                        Utils.MostrarAlerta(Response, "Este pedido ya existe");
					else
					{
						PedidoProveedor miPedido = new PedidoProveedor(tablaPedidos);
						miPedido.PrefijoPedido = Request.QueryString["tipoDocu"];
						miPedido.NumeroPedido = idPedido.Text;
						miPedido.FechaPedido = fechaPedido.Text;
						miPedido.NitProveedor = nitProveedor.Text;
						miPedido.Observacion = observacion.Text;
						if(miPedido.Grabar_Pedido(Request.QueryString["cons"]))
							//Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoProveedor");
						{
							DBFunctions.NonQuery("drop view dbxschema.vvehiculos_solicitudpedidosvehiculos_r");
							DBFunctions.NonQuery("CREATE VIEW DBXSCHEMA.VVEHICULOS_SOLICITUDPEDIDOSVEHICULOS_R AS select * from dbxschema.VVEHICULOS_SOLICITUDPEDIDOSVEHICULOS where pref_ped='"+miPedido.PrefijoPedido+"' and num_ped="+miPedido.NumeroPedido+" ");
							this.generarInforme();
						}
						else
							lb.Text = miPedido.ProcessMsg;
					}
				
			}
		}
		
		protected void generarInforme()
		{
			Label lbvacio= new Label();
			string[] Formulas = new string[8];
			string[] ValFormulas = new string[8];
			string header = "AMS_HEADER.rpt";
			string footer = "AMS_FOOTER.rpt";
			DataSet tempDS = new DataSet();
			//JFSC 11022008 Poner en comentario por no ser usado
			//string where = "";filtro="";
			//string[] filtros;
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
			ValFormulas[2] = "SOLICITUD DE PEDIDO VEHICULOS A PROVEEDOR "; //titulo rpt
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
			//JFSC 11022008 Cambiar nombre de método y cantidad de parámetros
            string nomA = "Solicitud Vehiculos";
			funcion.PreviewReport2("AMS.Vehiculos.SolicitudDePedidosVehiculos",header,footer,1,1,1,"","",nomA,"pdf",usuario,newPwd,Formulas,ValFormulas,lbvacio);
            hl1.NavigateUrl = funcion.Documento;
			hl1.Visible=true;
		}
		
		protected void Editar_Pedido(Object  Sender, EventArgs e)
		{
			PedidoProveedor miPedido = new PedidoProveedor(tablaPedidos);
			miPedido.PrefijoPedido = Request.QueryString["tipoDocu"];
			miPedido.NumeroPedido = idPedido.Text;
			miPedido.FechaPedido = fechaPedido.Text;
			miPedido.NitProveedor = nitProveedor.Text;
			miPedido.Observacion = observacion.Text;
			if(miPedido.Editar_Pedido())
				Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoProveedor");
			else
				lb.Text = miPedido.ProcessMsg;
		}
		
		protected bool Validar_Valores_Grilla()
		{
			bool validador = true;
			for(int i=0;i<tablaPedidos.Rows.Count;i++)
			{
				for(int j=i+1;j<tablaPedidos.Rows.Count;j++)
				{
					if((tablaPedidos.Rows[i][0].ToString()==tablaPedidos.Rows[j][0].ToString())&&(tablaPedidos.Rows[i][1].ToString()==tablaPedidos.Rows[j][1].ToString()))
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
			for(int i=0;i<tablaPedidos.Rows.Count;i++)
				total += Convert.ToDouble(tablaPedidos.Rows[i][4])*System.Convert.ToInt32(tablaPedidos.Rows[i][3]);
			totalPedido.Text = total.ToString("C");
		}
		
		protected void Cargar_Datos_Pedido_Existente(string prefijoPedido, string numeroPedido)
		{
			Preparar_Tabla_Pedido();
			//Debemos traer los valores que se encuentran en dpedidovehiculoproveedor
			DataSet elementosPedido = new DataSet();
			DBFunctions.Request(elementosPedido,IncludeSchema.NO,"SELECT pcat_codigo, pcol_codigo, pcol_codigoalte, dped_cantpedi, dped_valounit,dped_fechallegada FROM dpedidovehiculoproveedor WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			for(int i=0;i<elementosPedido.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaPedidos.NewRow();
				fila[0] = elementosPedido.Tables[0].Rows[i][0].ToString();
				fila[1] = elementosPedido.Tables[0].Rows[i][1].ToString();
				fila[2] = elementosPedido.Tables[0].Rows[i][2].ToString();
				fila[3] = Convert.ToInt32(elementosPedido.Tables[0].Rows[i][3]);
				fila[4] = Convert.ToDouble(elementosPedido.Tables[0].Rows[i][4]);
				try {fila[5] = Convert.ToDateTime(elementosPedido.Tables[0].Rows[i][5]).ToString("yyyy-MM-dd");} 
				catch {fila[5]="";}
				tablaPedidos.Rows.Add(fila);
			}
			Bind_dgInserts();
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
