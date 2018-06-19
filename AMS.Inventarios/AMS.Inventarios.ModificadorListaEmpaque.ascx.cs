using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.Documentos;
using AMS.DB;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial  class ModificadorListaEmpaque : System.Web.UI.UserControl
	{
		protected DataTable dtListaEmpaque,dtInserts;
        protected string nacionalidad;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				//Vamos a Llenar la informacion relacionada con la lista de empaque
				string numLisEmpaque = Request.QueryString["numLista"];
				lbNumLista.Text =  numLisEmpaque;
                lbAlmacen.Text = DBFunctions.SingleData("SELECT palm_almacen CONCAT '-' CONCAT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM mlistaempaque WHERE mlis_numero=" + numLisEmpaque + ")");
				lbNitCliente.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mlistaempaque WHERE mlis_numero="+numLisEmpaque+"");
				lbNombreCliente.Text = DBFunctions.SingleData("SELECT CASE WHEN TNIT_TIPONIT = 'N' THEN mnit_apellidos ELSE mnit_apellidos CONCAT ' ' CONCAT COALESCE(mnit_apellido2,'') CONCAT ' ' CONCAT mnit_nombres CONCAT ' ' CONCAT COALESCE(mnit_nombre2,'') END AS NOMBRE FROM mnit WHERE mnit_nit='" + lbNitCliente.Text+"'");
				lbDate.Text = Convert.ToDateTime(DBFunctions.SingleData("SELECT mlis_fechproc FROM mlistaempaque WHERE mlis_numero="+numLisEmpaque+"")).ToString("yyyy-MM-dd");
				Session.Clear();
				LlenarTablaDetalle();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlLineas,"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				txtItem.Attributes.Add("onclick","CargaItem("+txtItem.ClientID+","+ddlLineas.ClientID+",'"+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+"');");
			}
			if(Session["dtListaEmpaque"] == null)
				PrepararDtListaEmpaque();
			else
				dtListaEmpaque = (DataTable)Session["dtListaEmpaque"];
			if(Session["dtInsertsCPM"]==null)
				LoadDataTable();
			else
				dtInserts = (DataTable)Session["dtInsertsCPM"];
		}
		protected void LoadDataTable()
		{
			int i;
			ArrayList types = new ArrayList();
			ArrayList lbFields = new ArrayList();
			lbFields.Add("mite_codigo");//0
			types.Add(typeof(string));
			lbFields.Add("mite_nombre");//1
			types.Add(typeof(string));
			lbFields.Add("mite_cantidad");//2
			types.Add(typeof(double));
			lbFields.Add("mite_cantasig");//3
			types.Add(typeof(double));
			lbFields.Add("mite_precio");//4
			types.Add(typeof(double));
			lbFields.Add("mite_iva");//5
			types.Add(typeof(double));
			lbFields.Add("mite_desc");//6
			types.Add(typeof(double));
			lbFields.Add("mite_tot");//7
			types.Add(typeof(double));
			lbFields.Add("mite_disp");//8
			types.Add(typeof(double));
			lbFields.Add("mite_totA");//9
			types.Add(typeof(double));			
			lbFields.Add("mite_precioinicial");//10
			types.Add(typeof(double));
			lbFields.Add("plin_codigo");//11
			types.Add(typeof(string));
			lbFields.Add("mite_color");//12
			types.Add(typeof(string));
			dtInserts = new DataTable();
			for(i=0; i<lbFields.Count; i++)
				dtInserts.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
			Session["dtInsertsCPM"] = dtInserts;
		}
		protected void PrepararDtListaEmpaque()
		{
			dtListaEmpaque = new DataTable();
			dtListaEmpaque.Columns.Add(new DataColumn("CODIGOORIGINAL",System.Type.GetType("System.String")));//0
			dtListaEmpaque.Columns.Add(new DataColumn("LINEA",System.Type.GetType("System.String")));//1
			dtListaEmpaque.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//2
			dtListaEmpaque.Columns.Add(new DataColumn("PEDIDO",System.Type.GetType("System.String")));//3
			dtListaEmpaque.Columns.Add(new DataColumn("PREFIJOPEDIDO",System.Type.GetType("System.String")));//4
			dtListaEmpaque.Columns.Add(new DataColumn("NUMEROPEDIDO",System.Type.GetType("System.String")));//5
			dtListaEmpaque.Columns.Add(new DataColumn("CANTIDADPENDIENTE",System.Type.GetType("System.Int32")));//6
			dtListaEmpaque.Columns.Add(new DataColumn("CANTIDADDISPONIBLE",System.Type.GetType("System.Int32")));//7
			dtListaEmpaque.Columns.Add(new DataColumn("CANTIDADASIGNADA",System.Type.GetType("System.Int32")));//8
			dtListaEmpaque.Columns.Add(new DataColumn("CANTIDADASIGNADAORIGINAL",System.Type.GetType("System.Int32")));//9
		}
		
		protected void LlenarTablaDetalle()
		{
			this.PrepararDtListaEmpaque();
			string numLisEmpaque = Request.QueryString["numLista"];
			string almacen = DBFunctions.SingleData("SELECT palm_almacen FROM mlistaempaque WHERE mlis_numero="+numLisEmpaque+"");
			string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DLE.mite_codigo, MIT.plin_codigo, DBXSCHEMA.EDITARREFERENCIAS(DLE.mite_codigo,PLIN.plin_tipo), DLE.pped_codigo CONCAT '-' CONCAT CAST(DLE.mped_numepedi AS character(10)),DLE.pped_codigo,DLE.mped_numepedi, DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact, MSIA.msal_cantactual - MSIA.msal_cantasig, DLE.dped_cantasig FROM dlistaempaque DLE, mitems MIT, msaldoitemalmacen MSIA, dpedidoitem DPI, plineaitem PLIN WHERE DLE.mite_codigo = MIT.mite_codigo AND MSIA.mite_codigo = DLE.mite_codigo AND MSIA.palm_almacen = '"+almacen+"' AND MSIA.pano_ano = "+ano_cinv+" AND DPI.pped_codigo = DLE.pped_codigo AND DPI.mped_numepedi = DLE.mped_numepedi AND DPI.mite_codigo = DLE.mite_codigo AND DLE.mlis_numero = "+numLisEmpaque+" AND MIT.plin_codigo=PLIN.plin_codigo ORDER BY DLE.mite_codigo");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow fila = dtListaEmpaque.NewRow();
				fila[0] = ds.Tables[0].Rows[i][0].ToString();
				fila[1] = ds.Tables[0].Rows[i][1].ToString();
				fila[2] = ds.Tables[0].Rows[i][2].ToString();
				fila[3] = ds.Tables[0].Rows[i][3].ToString();
				fila[4] = ds.Tables[0].Rows[i][4].ToString();
				fila[5] = ds.Tables[0].Rows[i][5].ToString();
				fila[6] = Convert.ToInt32(ds.Tables[0].Rows[i][6]);
				fila[7] = Convert.ToInt32(ds.Tables[0].Rows[i][7]);
				fila[8] = Convert.ToInt32(ds.Tables[0].Rows[i][8]);
				fila[9] = Convert.ToInt32(ds.Tables[0].Rows[i][8]);
				dtListaEmpaque.Rows.Add(fila);
			}
			this.BindDgListaEmpaque();
		}
		
		protected void BindDgListaEmpaque()
		{
			Session["dtListaEmpaque"] = dtListaEmpaque;
			dgListaEmpaque.DataSource = dtListaEmpaque;
			dgListaEmpaque.DataBind();
		}
		
		#region Grilla Empaque
		protected void DgListaEmpaqueDelete(Object sender, DataGridCommandEventArgs e)
		{
			try
            {
        		dtListaEmpaque.Rows.Remove(dtListaEmpaque.Rows[e.Item.ItemIndex]);
            	dgListaEmpaque.EditItemIndex=-1;
        	}
            catch{};
            BindDgListaEmpaque();
		}
		
		public void DgListaEmpaqueEdit(Object sender, DataGridCommandEventArgs e)
		{
     		if(dtListaEmpaque.Rows.Count>0)
     			dgListaEmpaque.EditItemIndex=(int)e.Item.ItemIndex;
    		BindDgListaEmpaque();
	    }
		
		public void DgListaEmpaqueUpdate(Object sender, DataGridCommandEventArgs e)
		{
			if(DatasToControls.ValidarInt(((TextBox)e.Item.Cells[3].Controls[0]).Text))
			{
				int cantidad = Convert.ToInt32(((TextBox)e.Item.Cells[3].Controls[0]).Text);
				int cantidadInicial = Convert.ToInt32(dtListaEmpaque.Rows[e.Item.ItemIndex][9]);
				int cantidadPendiente = Convert.ToInt32(dtListaEmpaque.Rows[e.Item.ItemIndex][6]);
				int cantidadDisponible = Convert.ToInt32(dtListaEmpaque.Rows[e.Item.ItemIndex][7]);
				if(cantidad>0)
				{
					DataRow[] selection = dtListaEmpaque.Select("CODIGOORIGINAL ='"+dtListaEmpaque.Rows[e.Item.ItemIndex][0]+"'");
					for(int i=0;i<selection.Length;i++)
					{
						if(selection[i][3].ToString().Trim()!=dtListaEmpaque.Rows[e.Item.ItemIndex][3].ToString().Trim())
						{
							int varianteDisponibilidad = Convert.ToInt32(selection[i][8]) - Convert.ToInt32(selection[i][9]);
							cantidadDisponible  += (varianteDisponibilidad*(-1));
						}
					}
					if((cantidad-cantidadInicial)>cantidadDisponible)
                        Utils.MostrarAlerta(Response, "No hay disponibilidad suficiente para este ajuste.\\nCantidad Disponible : "+cantidadDisponible+"");
					else{
						if((cantidad-cantidadInicial)>cantidadPendiente)
                            Utils.MostrarAlerta(Response, "Esta empacando mas de la cantidad pedida!");
						if(cantidad<0)
							cantidad = 0;
						dtListaEmpaque.Rows[e.Item.ItemIndex][8] = cantidad;
						dtListaEmpaque.Rows[e.Item.ItemIndex][6] = Convert.ToInt32(dtListaEmpaque.Rows[e.Item.ItemIndex][6]) + ((cantidad - cantidadInicial)*(-1));
					}
				}
				else
                    Utils.MostrarAlerta(Response, "No se puede asignar un valor menor o igual a 0!");
			}
			else
                Utils.MostrarAlerta(Response, "La cantidad ingresada no es valida!");
			dgListaEmpaque.EditItemIndex=-1;
    		BindDgListaEmpaque();
		}
		
		public void DgListaEmpaqueCancel(Object sender, DataGridCommandEventArgs e)
		{
			dgListaEmpaque.EditItemIndex=-1;
    		BindDgListaEmpaque();
    	}
		
		#endregion Grilla Empaque
		public void CancelarModificacion(object sender, System.EventArgs e)
		{
			Response.Redirect(""+indexPage+"?process=Inventarios.ListasEmpaque&actor=C&subprocess=Mod");
		}
		
		public void AceptarModificacion(object sender, System.EventArgs e)
		{
			ListaEmpaque listaModificar = new ListaEmpaque(Convert.ToUInt32(Request.QueryString["numLista"]));
            if (listaModificar.ModificarLista(dtListaEmpaque, dtInserts))
				//lb.Text += "<br>Bien Modificao : "+listaModificar.ProcessMsg;
				Response.Redirect(""+indexPage+"?process=Inventarios.ListasEmpaque&actor=C&subprocess=Mod");
			else
				lb.Text += "<br>Error : "+listaModificar.ProcessMsg;
		}

		public void AgregarItem(object sender, System.EventArgs e)
		{
			if(CheckValues())
			{
				int ivm=1;
				double cant = 0;
				double prec=0;
				double desc=0;
				double cantidadIngresada = Convert.ToDouble(txtCantidad.Text);
				string codI = "";	
				Referencias.Guardar(txtItem.Text.Trim(),ref codI,(ddlLineas.SelectedValue.Split('-'))[1]);
				cant = cantidadIngresada;
				prec=Convert.ToDouble(txtPrecioF.Text);
				desc=Convert.ToDouble(txtDescuento.Text);
				InsertaItem(txtItem.Text.Trim(),ddlLineas.SelectedValue.Split('-')[0],cant,prec,desc,ivm);//Se le pasa el codigo, cantidad solicitada,precio,descuento y un indicativo si es una solicitud de taller
				BindDatasInsertar();
			}
			else
                Utils.MostrarAlerta(Response, "Algun valor no es valido para la inserción o el precio es cero!");
		}
		
		protected void BindDatasInsertar()
		{
			int i,j;
			dgItemsNuevos.EnableViewState=true;
			DataView dvInserts = new DataView(dtInserts);
			dgItemsNuevos.DataSource = dtInserts;
			Session["dtInsertsCPM"] = dtInserts;
			dgItemsNuevos.DataBind();
			for(i=0;i<dgItemsNuevos.Columns.Count;i++)
				if(i>=3 && i<=9)
					for(j=0;j<dgItemsNuevos.Items.Count;j++)
						dgItemsNuevos.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
			for(i=0;i<dtInserts.Rows.Count;i++)
			{
				if(dtInserts.Rows[i][12].ToString() == "0")
					dgItemsNuevos.Items[i].Cells[4].BackColor = Color.Red;
				else if(dtInserts.Rows[i][12].ToString() == "1")
					dgItemsNuevos.Items[i].Cells[4].BackColor = Color.Yellow;
				else if(dtInserts.Rows[i][12].ToString() == "2")
					dgItemsNuevos.Items[i].Cells[4].BackColor = Color.Green;
				else
					lb.Text += "<br>"+dtInserts.Rows[i][12].ToString();
			}
		}

		protected void InsertaItem(string CodNIT,string linea, double cant, double prec, double descuento, int ivm)
		{
			DataSet ds = new DataSet();
			if(CodNIT.Length > 0)
			{
				string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
				string mes_cinv = DBFunctions.SingleData("SELECT pmes_mes from cinventario");
				string codI = "";
				string almacen=DBFunctions.SingleData("SELECT PALM_ALMACEN FROM MLISTAEMPAQUE WHERE MLIS_NUMERO="+lbNumLista.Text+";");
				double prIni=0,pr=0;
				if(!Referencias.Guardar(CodNIT,ref codI,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+linea+"'")))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " no es valido para la linea de bodega " + DBFunctions.SingleData("SELECT plin_nombre FROM plineaitem WHERE plin_codigo='" + linea + "'") + ".\\nRevise Por Favor!");
					return;
				}
				if(!Referencias.RevisionSustitucion(ref codI,linea))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " NO se encuentra registrado. " + codI + "\\nRevise Por Favor!");
					return;
				}
				string CodNIT2 = "";
				Referencias.Editar(codI,ref CodNIT2,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+linea+"'"));
				if(CodNIT2 != CodNIT)
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " se ha sustituido.\\nEl codigo actual es " + CodNIT2 + "!");
				//Revisar que no este ya el item en lista
				if(((dtInserts.Select("mite_codigo='"+CodNIT+"'")).Length > 0) || ((dtListaEmpaque.Select("CODIGOORIGINAL='"+CodNIT+"'")).Length > 0))
				{
                    Utils.MostrarAlerta(Response, "El item ya se utilizó!\\nCodigo Item :" + CodNIT + "  \\nDescripción:" + DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mitems.mite_codigo='" + codI + "'") + "");
					return;
				}
				//Que tipo de pedido es, si el pedido es tipo Garantia(G) o Interno(I) el precio del item no se toma de la lista de precios sino es el costo promedio + el factor de garantia o interno
				//Cantidad: Si la cantidad es menor o igual que cero le asigna 1
				if(cant <= 0)
					cant = 1;
				//Datos del item cod, nom, iva, porcentaje de ganancia
				DBFunctions.Request(ds, IncludeSchema.NO,"select mitems.mite_codigo, mitems.mite_nombre, mitems.piva_porciva, mitems.plin_codigo, pdescuentoitem.pdes_porcdesc from mitems, pdescuentoitem where mitems.mite_codigo='" +codI+"' and pdescuentoitem.pdes_codigo=mitems.pdes_codigo");
				//															0					1				2					3                         4
				//Precio del item segun lista de precios
				prIni=pr=prec;
				//Descuento del cliente
				double desc = 0;
				desc=descuento;
				double cantA = 0;						//Cantidad Asignada
				double cantD = Referencias.ConsultarDisponibilidad(codI,almacen,ano_cinv,0); //Cantidad Disponible
				if(cant>cantD)
					cantA=cantD;
				else 
					cantA=cant;
                nacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI from MNIT WHERE MNIT_NIT = '" + lbNitCliente.Text.ToString() + "' ");
          
                if (nacionalidad == "E") ivm = 0;
				double iva=Convert.ToDouble(ds.Tables[0].Rows[0][2])*ivm;//Si se esta realizando una transferencia de taller no se liquida el iva todavia
				//Total
				double tot = cant*pr;
				tot=tot+Math.Round(tot*(iva/100),0);
				tot=tot-Math.Round((desc/100)*tot,0);
				double totA=cantA*pr;
				totA=totA+Math.Round(totA*(iva/100),0);
				totA=totA-Math.Round((desc/100)*totA,0);
				//Llenar nueva fila
				if(ds.Tables[0].Rows.Count>0)
				{
					DataRow dr = dtInserts.NewRow();
					dr[0] = CodNIT;//Codigo
					dr[1] = ds.Tables[0].Rows[0][1];//Nombre
					dr[2] = cant;					//Cantidad
					dr[3] = cantA;					//CantidadAsig
					//dr[4] = pr;						//Precio
					dr[4] = prIni;						//Precio
					dr[5] = iva;					//IVA
					dr[6] = desc;					//Descuento
					//dr[7] = tot;					//Total
					dr[7] = totA;					//Total
					dr[8] = cantD;					//Disponible
					dr[9] = totA;					//Total Asignado
					dr[10] = prIni;					//Precio Inicial
					dr[11] = ds.Tables[0].Rows[0][3];//Linea
					//Vamos a determinar cual es el color del semaforo
					dr[12] = Referencias.ConsultaSemaforoDisponibilidad(codI,almacen,Convert.ToUInt32(mes_cinv),Convert.ToInt32(ano_cinv)).ToString();
					dtInserts.Rows.Add(dr);
				}
			}
		}

		protected void DgInserts_Delete(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				dtInserts.Rows.Remove(dtInserts.Rows[e.Item.ItemIndex]);
				dgItemsNuevos.EditItemIndex=-1;
			}
			catch{};
			BindDatasInsertar();
		}

		
		protected bool CheckValues()
		{
			bool check=true;
			if(txtItem.Text == "" || !DatasToControls.ValidarDouble(txtCantidad.Text))
				check=false;
			else if(!DatasToControls.ValidarDouble(txtPrecioF.Text) || Convert.ToDouble(txtPrecioF.Text)<=0)
				check=false;
			else if(!DatasToControls.ValidarDouble(txtDescuento.Text) || Convert.ToDouble(txtDescuento.Text)>100)
				check=false;
			return check;
		}

		public void ReiniciarGrilla(object sender, System.EventArgs e)
		{
			this.LlenarTablaDetalle();
		}
		
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
