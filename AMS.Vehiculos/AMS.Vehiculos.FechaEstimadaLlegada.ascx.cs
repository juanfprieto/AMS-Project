namespace AMS.Vehiculos
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using System.Collections;
	using System.Configuration;
    using AMS.Tools;
	

	/// <summary>
	///		Descripción breve de AMS_Vehiculos_FechaEstimadaLlegada.
	/// </summary>
	public partial class AMS_Vehiculos_FechaEstimadaLlegada : System.Web.UI.UserControl
	{
		protected DataTable tablaItems;
		protected string indexPage= ConfigurationManager.AppSettings["MainIndexPage"];
		protected bool existe;
		protected DataSet datosPedido = new DataSet();
		protected System.Web.UI.WebControls.Image Image1;
		protected DataSet datosPedidoIng = new DataSet();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
//				bind.PutDatasIntoDropDownList(ddlNitProveedor,"Select mpro.mnit_nit,mpro.mnit_nit concat ' ' concat mnit.mnit_nombres concat ' ' concat mnit_apellidos from DBXSCHEMA.MPROVEEDOR mpro, dbxschema.mnit mnit where mpro.mnit_nit=mnit.mnit_nit order by mpro.mnit_nit");
				bind.PutDatasIntoDropDownList(ddlNitProveedor,"Select distinct Mpro.mnit_nit,mpro.mnit_nit concat ' ' concat mnit.mnit_nombres concat ' ' concat mnit_apellidos from DBXSCHEMA.MPEDIDOVEHICULOPROVEEDOR Mpro,dbxschema.mnit mnit where mpro.mnit_nit=mnit.mnit_nit order by mpro.mnit_nit");
				bind.PutDatasIntoDropDownList(ddlPrefijo,"Select pdoc_codigo,pdoc_codigo concat ' ' concat pdoc_descripcion  from DBXSCHEMA.PDOCUMENTO where tdoc_tipodocu='PV' ORDER BY PDOC_CODIGO ");
				bind.PutDatasIntoDropDownList(ddlNumPedido,"SELECT DISTINCT MPED.mped_numepedi FROM dbxschema.mpedidovehiculoproveedor MPED, dbxschema.dpedidovehiculoproveedor DPED WHERE MPED.pdoc_codigo = DPED.pdoc_codigo AND MPED.mped_numepedi = DPED.mped_numepedi AND DPED.dped_cantpedi <> DPED.dped_cantingr AND MPED.pdoc_codigo='"+ddlPrefijo.SelectedValue+"' and mped.mnit_nit='"+ddlNitProveedor.SelectedValue+"'");
//				imglupa.Attributes.Add("OnClick","ModalDialog("+ddlNitProveedor.ClientID+",'Select mpro.mnit_nit AS NIT,mnit.mnit_apellidos concat \\' \\' concat mnit.mnit_apellido2 concat \\' \\' concat mnit_nombres concat \\' \\' concat mnit_nombre2 AS NOMBRE from DBXSCHEMA.MPROVEEDOR mpro, dbxschema.mnit mnit where mpro.mnit_nit=mnit.mnit_nit ' ,new Array() )");
				if (ddlNitProveedor.Items.Count!=0)
				{
					ListItem item=new ListItem("--Seleccione un Nit--","Nit");
					ddlNitProveedor.Items.Insert(0,item);
				}
				if (ddlPrefijo.Items.Count!=0)
				{
					ListItem item=new ListItem("--Seleccione un Prefijo--","Pref");
					ddlPrefijo.Items.Insert(0,item);
				}

				//ddlPrefijo.Enabled=false;
				ddlNumPedido.Enabled=false;
				ddlNitProveedor.SelectedValue="Nit";
				ddlPrefijo.SelectedValue="Pref";
				btnIngresar.Visible=false;
				existe=false;
			}
			else
			{
				if (Session["existe"]!=null)
					existe=(bool)Session["existe"];
				if (Session["tablaItems"]!=null)
					tablaItems=(DataTable)Session["tablaItems"];
				
			}
		}
		
		protected void CambioNitProveedor(object sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			if (ddlNitProveedor.SelectedValue=="Nit")
                Utils.MostrarAlerta(Response, "Escoja un Nit por favor..");
			else
			{
				ddlPrefijo.Enabled=true;
				ddlNumPedido.Enabled=true;
				bind.PutDatasIntoDropDownList(ddlNumPedido,"SELECT DISTINCT MPED.mped_numepedi FROM dbxschema.mpedidovehiculoproveedor MPED, dbxschema.dpedidovehiculoproveedor DPED WHERE MPED.pdoc_codigo = DPED.pdoc_codigo AND MPED.mped_numepedi = DPED.mped_numepedi AND DPED.dped_cantpedi <> DPED.dped_cantingr AND MPED.pdoc_codigo='"+ddlPrefijo.SelectedValue+"' and mped.mnit_nit='"+ddlNitProveedor.SelectedValue+"'");
				dgPedido.DataSource = null;
				if (ddlNumPedido != null)
				{
					if (ddlNumPedido.Items.Count > 0)
					{
						btnCargarPedido.Enabled = true;
					}
					else
					{
						btnCargarPedido.Enabled = false;
					}
				}
				else
				{
					btnCargarPedido.Enabled = false;
				}
				dgPedido.DataBind();
			}
			
		}

		protected void CargarPedido(object sender, EventArgs e)
		{
			Preparar_Tabla_Vehiculos();
			DataSet datosPedido = new DataSet();
			DataSet datosPedidoIng = new DataSet();
			//Validacion si ya fue ingresada la información.
			if (DBFunctions.RecordExist("Select * from DBXSCHEMA.MVEHICULOLLEGADA where mped_numepedi="+ddlNumPedido.SelectedValue+" and pdoc_codigo='"+ddlPrefijo.SelectedValue+"'"))
			{
				existe=true;
				//DBFunctions.Request(datosPedidoIng,IncludeSchema.NO,"Select pcat_codigo, pcol_codigo,mvehllega_fechacolor,mvehllega_fechafabri,mvehllega_fechallegada,mvehllega_obs from DBXSCHEMA.MVEHICULOLLEGADA where mped_numepedi="+ddlNumPedido.SelectedValue+" and pdoc_codigo='"+ddlPrefijo.SelectedValue+"'");						
				DBFunctions.Request(datosPedidoIng,IncludeSchema.NO,"Select pcat.pcat_codigo,pcat.pcat_descripcion,pcol_codigo,mvehllega_fechacolor,mvehllega_fechafabri,mvehllega_fechallegada,mvehllega_obs,mveh.mvehllega_estado,mveh.mveh_secuencia from DBXSCHEMA.MVEHICULOLLEGADA mveh,dbxschema.pcatalogovehiculo pcat where mveh.pcat_codigo=pcat.pcat_codigo and mped_numepedi="+ddlNumPedido.SelectedValue+" and pdoc_codigo='"+ddlPrefijo.SelectedValue+"' order by pcat.pcat_codigo");						
				for (int i=0;i<datosPedidoIng.Tables[0].Rows.Count;i++)
				{
					this.ingresar_datos_llegada(datosPedidoIng.Tables[0].Rows[i][0].ToString(),datosPedidoIng.Tables[0].Rows[i][1].ToString(),datosPedidoIng.Tables[0].Rows[i][2].ToString(),datosPedidoIng.Tables[0].Rows[i][3].ToString(),datosPedidoIng.Tables[0].Rows[i][4].ToString(),datosPedidoIng.Tables[0].Rows[i][5].ToString(),datosPedidoIng.Tables[0].Rows[i][6].ToString(),datosPedidoIng.Tables[0].Rows[i][7].ToString());
				}
			}
			else
			{
				//DBFunctions.Request(datosPedido,IncludeSchema.NO,"Select pcat_codigo,pcol_codigo,dped_cantpedi from DBXSCHEMA.DPEDIDOVEHICULOPROVEEDOR where pdoc_codigo='"+ddlPrefijo.SelectedValue+"' and mped_numepedi="+ddlNumPedido.SelectedValue+" ");						
				DBFunctions.Request(datosPedido,IncludeSchema.NO,"Select pcat.pcat_codigo,pcat.pcat_descripcion,pcol_codigo,dped_cantpedi from DBXSCHEMA.DPEDIDOVEHICULOPROVEEDOR dped,dbxschema.pcatalogovehiculo pcat where dped.pcat_codigo=pcat.pcat_codigo and pdoc_codigo='"+ddlPrefijo.SelectedValue+"' and mped_numepedi="+ddlNumPedido.SelectedValue+" ");						
				if (datosPedido.Tables[0].Rows.Count>0)
				{
					for (int i=0;i<datosPedido.Tables[0].Rows.Count;i++)
					{
						int vehiculos = Convert.ToInt32(datosPedido.Tables[0].Rows[i][3]);
						for (int j=0;j<vehiculos;j++)
						{
							ingresar_datos_llegada(datosPedido.Tables[0].Rows[i][0].ToString(),datosPedido.Tables[0].Rows[i][1].ToString(),datosPedido.Tables[0].Rows[i][2].ToString(),"","","","","A");
						}
					}
				}
				else
                Utils.MostrarAlerta(Response, "No se han encontrado datos..");
			}
			this.btnIngresar.Visible=true;
			Session["existe"]=existe;
			Session["tablaItems"]=tablaItems;
		}

		protected void ingresar_Datos(object sender, EventArgs e)
		{
			existe=(bool)Session["existe"];
			ArrayList sql = new ArrayList();
			if (existe)
				sql.Add("delete from dbxschema.MVEHICULOLLEGADA where pdoc_codigo='"+ddlPrefijo.SelectedValue+"' and mped_numepedi="+ddlNumPedido.SelectedValue+"");
			for (int i=0;i<dgPedido.Items.Count;i++)
				sql.Add("insert into DBXSCHEMA.MVEHICULOLLEGADA values (default,'"+ddlNitProveedor.SelectedValue+"','"+ddlPrefijo.SelectedValue+"',"+ddlNumPedido.SelectedValue+",'"+dgPedido.Items[i].Cells[0].Text+"','"+((DropDownList)dgPedido.Items[i].Cells[2].FindControl("ddlColor")).SelectedValue+"',"+FechasNulas(((TextBox)dgPedido.Items[i].Cells[3].FindControl("txtFechaColorizacion")).Text)+","+FechasNulas(((TextBox)dgPedido.Items[i].Cells[4].FindControl("txtFechaFabricacion")).Text)+","+FechasNulas(((TextBox)dgPedido.Items[i].Cells[5].FindControl("txtFechaLlegada")).Text)+",'"+((TextBox)dgPedido.Items[i].Cells[6].FindControl("txtObs")).Text+"','"+dgPedido.Items[i].Cells[7].Text+"') ");
					
			if (sql.Count>0)
			{
					if (DBFunctions.Transaction(sql))
                        Response.Redirect("" + indexPage + "?process=Vehiculos.FechaEstimadaLlegada");
					else
						error.Text=DBFunctions.exceptions;
			}
		}

		protected void desabilitar()
		{
			for (int i=0;i<dgPedido.Items.Count;i++)
			{
				if (dgPedido.Items[i].Cells[7].Text=="I")
				{
					((DropDownList)dgPedido.Items[i].Cells[2].FindControl("ddlColor")).Enabled=false;
					((TextBox)dgPedido.Items[i].Cells[3].FindControl("txtFechaColorizacion")).Enabled=false;
					((TextBox)dgPedido.Items[i].Cells[4].FindControl("txtFechaFabricacion")).Enabled=false;
					((TextBox)dgPedido.Items[i].Cells[5].FindControl("txtFechaLlegada")).Enabled=false;
					((TextBox)dgPedido.Items[i].Cells[6].FindControl("txtObs")).Enabled=false;
				}
			}
		}

		protected string FechasNulas(string fecha)
		{
			if (fecha==String.Empty)
				fecha="null";
			else
				fecha="'"+fecha+"'";
			return fecha;
		}
		
		protected void Preparar_Tabla_Vehiculos()
		{
			tablaItems = new DataTable();
			tablaItems.Columns.Add(new DataColumn("pcat_codigo",System.Type.GetType("System.String")));//0
			tablaItems.Columns.Add(new DataColumn("pcat_descripcion",System.Type.GetType("System.String"))); //1
			tablaItems.Columns.Add(new DataColumn("pcol_codigo",System.Type.GetType("System.String"))); //2
			tablaItems.Columns.Add(new DataColumn("fechaColor",System.Type.GetType("System.String"))); //3
			tablaItems.Columns.Add(new DataColumn("fechaFabricacion",System.Type.GetType("System.String"))); //4
			tablaItems.Columns.Add(new DataColumn("fechaLlegada",System.Type.GetType("System.String"))); //5
			tablaItems.Columns.Add(new DataColumn("Observacion",System.Type.GetType("System.String")));//6
			tablaItems.Columns.Add(new DataColumn("estado",System.Type.GetType("System.String")));//7
		}
		
		protected void DataBound_Colores(object sender,DataGridItemEventArgs e)
		{
			if (e.Item.ItemType==ListItemType.Item || e.Item.ItemType==ListItemType.AlternatingItem)
			{
				DatasToControls param = new DatasToControls();
				param.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].FindControl("ddlColor")),"Select pcol_codigo,pcol_codigo concat ' ' concat '-' concat ' ' concat  pcol_descripcion from DBXSCHEMA.PCOLOR WHERE PCOL_ACTIVO IN ('S', 'SI') ORDER BY pcol_descripcion");
				DatasToControls.EstablecerValueDropDownList(((DropDownList)e.Item.Cells[2].FindControl("ddlColor")),tablaItems.Rows[e.Item.DataSetIndex][2].ToString());
			}
		}
		
		protected void gridVehis(object Sender,DataGridCommandEventArgs e)
		{
				if(((LinkButton)e.CommandSource).CommandName=="Eliminar")
				{
					if (((string)tablaItems.Rows[e.Item.DataSetIndex][7]).ToString()=="I")
					{
                        Utils.MostrarAlerta(Response, "Atencion:Si elige Ingresar Datos este dato sera marcado como Activo.");
						((DropDownList)e.Item.Cells[2].FindControl("ddlColor")).Enabled=true;
						((TextBox)e.Item.Cells[3].FindControl("txtFechaColorizacion")).Enabled=true;
						((TextBox)e.Item.Cells[4].FindControl("txtFechaFabricacion")).Enabled=true;
						((TextBox)e.Item.Cells[5].FindControl("txtFechaLlegada")).Enabled=true;
						((TextBox)e.Item.Cells[6].FindControl("txtObs")).Enabled=true;
						e.Item.Cells[7].Text="A";
						tablaItems.Rows[e.Item.DataSetIndex][7]="A";
					}
					else
					{
                        Utils.MostrarAlerta(Response, "Atencion:Si elige Ingresar Datos este dato sera marcado como Inactivo.");
						((DropDownList)e.Item.Cells[2].FindControl("ddlColor")).Enabled=false;
						((TextBox)e.Item.Cells[3].FindControl("txtFechaColorizacion")).Enabled=false;
						((TextBox)e.Item.Cells[4].FindControl("txtFechaFabricacion")).Enabled=false;
						((TextBox)e.Item.Cells[5].FindControl("txtFechaLlegada")).Enabled=false;
						((TextBox)e.Item.Cells[6].FindControl("txtObs")).Enabled=false;
						e.Item.Cells[7].Text="I";
						tablaItems.Rows[e.Item.DataSetIndex][7]="I";
					}
					dgPedido.DataSource=tablaItems;
					Session["tablaItems"]=tablaItems;
				}
		}

		protected void ingresar_datos_llegada(string pcat_codigo,string pcat_descripcion,string pcol_codigo,string fechaColor,string fechaFabricacion,string fechaLlegada,string obs,string estado)
		{
			DataRow fila = tablaItems.NewRow();
			fila["pcat_codigo"]=pcat_codigo;
			fila["pcat_descripcion"]=pcat_descripcion;
			fila["pcol_codigo"]=pcol_codigo;
			fila["fechaColor"]=fechasNulasDT(fechaColor);
			fila["fechaFabricacion"]=fechasNulasDT(fechaFabricacion);
			fila["fechaLlegada"]=fechasNulasDT(fechaLlegada);
			fila["Observacion"]=obs;
			fila["estado"]=estado;
			tablaItems.Rows.Add(fila);
			dgPedido.DataSource= tablaItems;
			dgPedido.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(dgPedido);
			DatasToControls.JustificacionGrilla(dgPedido,tablaItems);
			desabilitar();
		}

		protected string fechasNulasDT(string fecha)
		{
			if (fecha==String.Empty)
				fecha="";
			else
				fecha=Convert.ToDateTime(fecha).ToString("yyyy-MM-dd");
			return fecha;
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
	}
}
