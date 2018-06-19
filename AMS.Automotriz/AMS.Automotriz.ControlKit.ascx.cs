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
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{


	public partial class ControlKit : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataTable dtItems, dtOperaciones;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected Combo comboEdicion;
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlGrupos, "SELECT pgru_grupo, pgru_nombre CONCAT '  _  ' CONCAT pgru_grupo FROM pgrupocatalogo ORDER BY pgru_nombre, pgru_grupo");
                bind.PutDatasIntoDropDownList(ddlListasPrecios, "SELECT ppre_codigo, ppre_nombre FROM pprecioitem order by ppre_nombre");
				Preparar_DtItems();
				Preparar_DtOperaciones();
				Bind_DgItems();
				Bind_DgOperaciones();
				if(Request.QueryString["tipo"]=="Nuevo")
					Distribuir_Datos_Iniciales(false,0);
				else if(Request.QueryString["tipo"]=="Editar")
					    Distribuir_Datos_Iniciales(true,1);
			}
			if(Session["dtItems"] == null)
				this.Preparar_DtItems();
			else
				this.dtItems = (DataTable)Session["dtItems"];
			if(Session["dtOperaciones"] == null)
				this.Preparar_DtOperaciones();
			else
				this.dtOperaciones = (DataTable)Session["dtOperaciones"];
			if(Session["comboEdicion"] == null)
				this.comboEdicion = new Combo();
			else
				this.comboEdicion = (Combo)Session["comboEdicion"];
		}
		
		protected void DgItems_DataBound(object sender, DataGridItemEventArgs e)
 		{
 			if(e.Item.ItemType == ListItemType.Footer)
 				((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onclick", "ModalDialog(this,'SELECT DISTINCT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, PLIN.plin_tipo as LINEA "+  
                                                                                                   " FROM plineaitem PLIN, mitems MIT LEFT JOIN MPRECIOitem MPI ON MIT.MITE_cODIGO = MPI.MITE_CODIGO "+
                                                                                                   "  WHERE MIT.plin_codigo = PLIN.plin_codigo ORDER BY 1; ; ',new Array(),null,1);");
 		}
		
		protected void DgOperaciones_DataBound(object sender, DataGridItemEventArgs e)
 		{
            string slqOper = @"SELECT * from (SELECT PTEM.ptem_operacion AS CODIGO, PTEM.ptem_descripcion AS DESCRIPCION, PTIE.ptie_tiemclie AS TIEMPO 
                                    FROM dbxschema.ptempario PTEM, dbxschema.ptiempotaller PTIE WHERE (PTIE.ptie_grupcata = \'" + this.ddlGrupos.SelectedValue + @"\' AND PTEM.ptem_operacion = PTIE.ptie_tempario) 
                              UNION  
                               SELECT DISTINCT PTEM.ptem_operacion AS CODIGO, PTEM.ptem_descripcion AS DESCRIPCION, coAlesce(PTEM.pteM_tiemPOESTANDAR,1) AS TIEMPO  
                                 FROM dbxschema.ptempario PTEM WHERE PTEM_INDIGENERIC = \'S\' ) as a ORDER BY 2; ";
            if (e.Item.ItemType == ListItemType.Footer) // MobtnAceptardalDialogAT
                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onclick", "ModalDialog(this,'SELECT * from (SELECT PTEM.ptem_operacion AS CODIGO, PTEM.ptem_descripcion AS DESCRIPCION, PTIE.ptie_tiemclie AS TIEMPO FROM dbxschema.ptempario PTEM, dbxschema.ptiempotaller PTIE WHERE (PTIE.ptie_grupcata = \\'" + this.ddlGrupos.SelectedValue + "\\' AND PTEM.ptem_operacion = PTIE.ptie_tempario) UNION SELECT DISTINCT PTEM.ptem_operacion AS CODIGO, PTEM.ptem_descripcion AS DESCRIPCION, coAlesce(PTEM.pteM_tiemPOESTANDAR,1) AS TIEMPO  FROM dbxschema.ptempario PTEM WHERE PTEM_INDIGENERIC = \\'S\\' ) as a ORDER BY 2; ', new Array(), 1);");
                //((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onclick", "ModalDialog(this,'" + slqOper + "', new Array(), 1);");
 		}
		
		protected void DgItems_Event(Object sender, DataGridCommandEventArgs e)
		{
			if(e.CommandName == "AddDatasRow")
			{
				//Revisamos que no se haya agregado ya este elemento a la tabla
				if(((TextBox)e.Item.Cells[0].Controls[1]).Text != "")
				{
					DataRow[] selection = this.dtItems.Select("ITEM='"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"'");
					if(selection.Length>0)
                        Utils.MostrarAlerta(Response, "Ya se ha agregado este Item");
					else
					{
						DataRow fila = this.dtItems.NewRow();
						fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
						fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
						double cantidad=0;
					 	try{cantidad=Convert.ToDouble(DBFunctions.SingleData("SELECT CASE WHEN MIG.mig_cantidaduso IS NOT NULL THEN MIG.mig_cantidaduso ELSE MIT.mite_usoxvehi END"+
					 														 " FROM mitems MIT LEFT JOIN mitemsgrupo MIG ON MIT.mite_codigo = MIG.mite_codigo AND MIG.pgru_grupo = '"+ddlGrupos.SelectedValue+"'"+
					 													 " WHERE MIT.mite_codigo = '"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"'"));}
                        catch { cantidad = 0.00; }
						fila[2] = cantidad;
                        fila[3] = DBFunctions.SingleData("SELECT CASE WHEN mite_indigeneric = 'S' THEN 'Si' ELSE 'No' END FROM MITEMS WHERE MITE_CODIGO = '"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"' ");
						dtItems.Rows.Add(fila);
						Bind_DgItems();
                        Utils.MostrarAlerta(Response, "Se han agregado " + this.Agregar_Operaciones(((TextBox)e.Item.Cells[0].Controls[1]).Text) + " operaciones");
					}
				}
				else
                    Utils.MostrarAlerta(Response, "Debe Seleccionar un item para agregar");
			}
		}
		
		protected void DgOperaciones_Event(Object sender, DataGridCommandEventArgs e)
		{
			if(e.CommandName == "AddDatasRow")
			{
				//Revisamos que no se haya agregado ya este elemento a la tabla
				if(((TextBox)e.Item.Cells[0].Controls[1]).Text != "")
				{
					DataRow[] selection = this.dtOperaciones.Select("CODIGO='"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"'");
					if(selection.Length>0)
                        Utils.MostrarAlerta(Response, "Ya se ha agregado esta operación");
					else
					{
						DataRow fila = this.dtOperaciones.NewRow();
						fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
						fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
						fila[2] = Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
						this.dtOperaciones.Rows.Add(fila);
						this.Bind_DgOperaciones();
                        Utils.MostrarAlerta(Response, "Se han agregado " + this.Agregar_Items(((TextBox)e.Item.Cells[0].Controls[1]).Text) + " Items");
					}
				}
				else
                    Utils.MostrarAlerta(Response, "Debe Seleccionar una operación para agregar");
			}
		}
		
		protected void DgItems_Delete(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				this.Eliminar_Operaciones(dtItems.Rows[e.Item.DataSetIndex][0].ToString());
				this.dtItems.Rows[e.Item.DataSetIndex].Delete();
				this.Bind_DgItems();
			}
            catch (Exception ex)
            {
                Utils.MostrarAlerta(Response, "No se pudo eliminar la fila");lb.Text = ex.ToString();
            }
		}
		
		protected void DgOperaciones_Delete(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				this.Eliminar_Items(dtOperaciones.Rows[e.Item.DataSetIndex][0].ToString());
				this.dtOperaciones.Rows[e.Item.DataSetIndex].Delete();
				this.Bind_DgOperaciones();
			}
            catch (Exception ex)
            {
                Utils.MostrarAlerta(Response, "No se pudo eliminar la fila");lb.Text = ex.ToString();
            }
		}
		 
		protected void Cancelar(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Automotriz.AdminKitscombos");
		}
		
		protected void Aceptar(Object  Sender, EventArgs e)
		{
			if(Request.QueryString["tipo"]=="Nuevo")
			{
				Combo miCombo         = new Combo(this.dtItems,this.dtOperaciones);
				miCombo.Codigo        = this.tbCodigoKit.Text;
				miCombo.Descripcion   = this.tbNombreKit.Text;
				miCombo.GrupoCatalogo = this.ddlGrupos.SelectedValue;
				miCombo.ListaPrecios  = this.ddlListasPrecios.SelectedValue;
                miCombo.Kilometraje   = this.TextBoxKms.Text;
                miCombo.Meses         = this.TextBoxMeses.Text;
                miCombo.vigencia      = this.chkVigencia.Checked ? "V" : "B";
               
        		if(miCombo.CommitValues())
					Response.Redirect("" + indexPage + "?process=Automotriz.AdminKitscombos");
				else
					lb.Text += "<br>"+miCombo.ProcessMsg;
			}
			else if(Request.QueryString["tipo"]=="Editar")
			{
				//Agregamos al combo el nuevo nombre y posible lista de precios
				this.comboEdicion.Descripcion   = this.tbNombreKit.Text;
				this.comboEdicion.GrupoCatalogo = this.ddlGrupos.SelectedValue;
                this.comboEdicion.ListaPrecios  = this.ddlListasPrecios.SelectedValue;
                this.comboEdicion.Kilometraje   = this.TextBoxKms.Text;
                this.comboEdicion.Meses         = this.TextBoxMeses.Text;
				//Ahora cambiamos las operaciones y los items;
				this.comboEdicion.Items         = this.dtItems;
				this.comboEdicion.Operaciones   = this.dtOperaciones;
				if(this.comboEdicion.UpdateValues())
					Response.Redirect("" + indexPage + "?process=Automotriz.AdminKitscombos");
				else
					lb.Text += "<br>"+this.comboEdicion.ProcessMsg;
			}
		}
		
		#endregion
		
		#region Otros
		private void Distribuir_Datos_Iniciales(bool estadoControl, int indicativoOrigen)
		{
			if(indicativoOrigen == 0)
			{
				tbCodigoKit.Text = Request.QueryString["codigo"];
				tbNombreKit.Text = Request.QueryString["nombre"];
				ddlGrupos.SelectedValue = Request["grupo"].Trim();
				ddlListasPrecios.SelectedValue = Request["lista"].Trim();
                TextBoxKms.Text  = Request.QueryString["kilometraje"];
                TextBoxMeses.Text = Request.QueryString["meses"];
                chkVigencia.Checked = true;
                chkVigencia.Enabled = false;
			}
			else if(indicativoOrigen == 1)
			{
				comboEdicion = new Combo(Request.QueryString["codigo"]);
				this.tbCodigoKit.Text  = comboEdicion.Codigo;
				this.tbNombreKit.Text  = comboEdicion.Descripcion;
				ddlGrupos.SelectedValue = comboEdicion.GrupoCatalogo;
				ddlListasPrecios.SelectedValue = comboEdicion.ListaPrecios;
                this.TextBoxKms.Text   = comboEdicion.Kilometraje;
                this.TextBoxMeses.Text = comboEdicion.Meses;
                this.dtItems           = comboEdicion.Items;
				this.dtOperaciones     = comboEdicion.Operaciones;
				this.Bind_DgItems();
				this.Bind_DgOperaciones();
				Session["comboEdicion"] = comboEdicion;
                chkVigencia.Checked    = comboEdicion.vigencia == "V";
			}
			this.tbNombreKit.Enabled = estadoControl;
			this.ddlListasPrecios.Enabled = estadoControl;
		}
		
			private void Preparar_DtItems()
			{
				dtItems = new DataTable();
				dtItems.Columns.Add(new DataColumn("ITEM",System.Type.GetType("System.String")));//0
				dtItems.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));//1
				dtItems.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.Double")));//2
                dtItems.Columns.Add(new DataColumn("ITEM GENERICO", System.Type.GetType("System.String")));//3
			}
		
		private void Preparar_DtOperaciones()
		{
			dtOperaciones = new DataTable();
			dtOperaciones.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//0
			dtOperaciones.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));//1
			dtOperaciones.Columns.Add(new DataColumn("TIEMPO",System.Type.GetType("System.Double")));//2
		}
		
		private void Bind_DgItems()
		{
			this.dgItems.DataSource = this.dtItems;
			this.dgItems.DataBind();
			Session["dtItems"] = this.dtItems;
		}
		
		private void Bind_DgOperaciones()
		{
			this.dgOperaciones.DataSource = dtOperaciones;
			this.dgOperaciones.DataBind();
			Session["dtOperaciones"] = dtOperaciones;
		}
		
		private int Agregar_Operaciones(string codigoItem)
		{
			DataSet ds = new DataSet();
			int agregados = 0;
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MIO.ptem_operacion, PTEM.ptem_descripcion FROM mitemoperacion MIO, ptempario PTEM WHERE PTEM.ptem_operacion = MIO.ptem_operacion AND MIO.mite_codigo='"+codigoItem+"'");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				//Revisamo si ya se ha agregado la operacion a la grilla
				DataRow[] selection = this.dtOperaciones.Select("CODIGO = '"+ds.Tables[0].Rows[i][0].ToString()+"'");
				if(selection.Length == 0)
				{
					agregados += 1;
					DataRow fila = this.dtOperaciones.NewRow();
					fila[0] = ds.Tables[0].Rows[i][0].ToString();
					fila[1] = ds.Tables[0].Rows[i][1].ToString();
					//Revisamos si esta operacion tiene tiempo para este grupo de catalogo, si no realizamos la advertencia
					if(DBFunctions.RecordExist("SELECT * FROM ptiempotaller WHERE ptie_grupcata='"+this.ddlGrupos.SelectedValue+"' AND ptie_tempario='"+ds.Tables[0].Rows[i][0].ToString()+"'"))
						fila[2] = Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+this.ddlGrupos.SelectedValue+"' AND ptie_tempario='"+ds.Tables[0].Rows[i][0].ToString()+"'"));
					else
                        Utils.MostrarAlerta(Response, "La operacion : " + ds.Tables[0].Rows[i][1].ToString() + " para el grupo de catalogo : " + this.ddlGrupos.SelectedValue + " no tiene un tiempo especificado. Por Favor Agregar, para evitar inconvenientes con la creación y edición de ordenes de trabajo"); 
					this.dtOperaciones.Rows.Add(fila);
				}
			}
			this.Bind_DgOperaciones();
			return agregados;
		}
		
		private int Agregar_Items(string codigoOperacion)
		{
			DataSet ds = new DataSet();
			int agregados = 0;
        //  string items = "SELECT DISTINCT MITE.mite_codigo , MITE.mite_nombre , CASE WHEN MITE.mite_codigo NOT IN (SELECT mite_codigo FROM mitemsgrupo WHERE pgru_grupo = '"+this.ddlGrupos.SelectedValue+"' AND mig_cantidaduso IS NOT NULL) THEN MITE.mite_usoxvehi WHEN MITE.mite_codigo IN (SELECT mite_codigo FROM mitemsgrupo WHERE pgru_grupo = '"+this.ddlGrupos.SelectedValue+"' AND mig_cantidaduso IS NOT NULL) THEN MIG.mig_cantidaduso END AS CANTIDAD FROM mitems MITE, mitemsgrupo MIG, mitemoperacion MIO WHERE (((MITE.mite_codigo=MIG.mite_codigo AND MIG.pgru_grupo='"+this.ddlGrupos.SelectedValue+"') OR (MITE.mite_indigeneric = 'S' AND MITE.mite_codigo NOT IN (SELECT mite_codigo FROM mitemsgrupo WHERE pgru_grupo = '"+this.ddlGrupos.SelectedValue+"' AND mig_cantidaduso IS NOT NULL)))) AND  (MIO.mite_codigo = MITE.mite_codigo AND MIO.ptem_operacion='"+codigoOperacion+"') ORDER BY MITE.mite_codigo";
            //string items = @"SELECT MKIT.mkit_coditem, MI.mite_nombre, coalesce(mig_cantidaduso, mi.mite_usoxvehi) as cantidad, 
            //                    case when MI.mite_indigeneric = 'S' then 'Si' else 'No' end as uso, MPI.MPRE_PRECIO
            //               FROM dbxschema.mkititem MKIT, dbxschema.Pkit PK, dbxschema.MPRECIOitem MPI, dbxschema.mitems MI 
            //                left join mitemsgrupo mig on mig.pgru_grupo = '" + this.ddlGrupos.SelectedValue + @"' and mig.mite_codigo = mi.mite_codigo and MI.mite_indigeneric = 'S'
            //               WHERE MKIT.mkit_coditem=MI.mite_codigo AND MKIT.mkit_codikit='" + this.tbCodigoKit.Text + @"' 
            //                AND MKIt.MKIT_CODIKIT = PK.PKIT_CODIGO and MPI.ppre_codigo=pk.ppre_codigo and mi.mite_codigo = MPI.mite_codigo; ";
            string items = @"SELECT MKIT.mkit_coditem, MI.mite_nombre, coalesce(mig_cantidaduso, mi.mite_usoxvehi) as cantidad, 
                                case when MI.mite_indigeneric = 'S' then 'Si' else 'No' end as uso, COALESCE(MPI.MPRE_PRECIO,-1) AS PRECIO
                           FROM dbxschema.Pkit PK
								left join dbxschema.mkititem MKIT ON MKIt.MKIT_CODIKIT = PK.PKIT_CODIGO
						   		left join dbxschema.mitems MI ON MKIT.mkit_coditem=MI.mite_codigo 
                        		LEFT JOIN dbxschema.MPRECIOitem MPI ON MPI.ppre_codigo=pk.ppre_codigo and mi.mite_codigo = MPI.mite_codigo
						    	left join dbxschema.mitemsgrupo mig on mig.pgru_grupo = '" + this.ddlGrupos.SelectedValue + @"' and mig.mite_codigo = mi.mite_codigo and MI.mite_indigeneric = 'S'
                           WHERE MKIT.mkit_codikit='" + this.tbCodigoKit.Text + @"' ";

            DBFunctions.Request(ds,IncludeSchema.NO,items);
         	for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow[] selection = this.dtItems.Select("ITEM='"+ds.Tables[0].Rows[i][0].ToString()+"'");
				if(selection.Length == 0)
				{
					agregados += 1;
					DataRow fila = this.dtItems.NewRow();
					fila[0] = ds.Tables[0].Rows[i][0].ToString();
					fila[1] = ds.Tables[0].Rows[i][1].ToString();
					fila[2] = Convert.ToDouble(ds.Tables[0].Rows[i][2].ToString());
                    fila[3] = ds.Tables[0].Rows[i][3].ToString();
					//Revisamos si este Item tiene precio dentro de la lista de precios escogida
                    if (Convert.ToString(ds.Tables[0].Rows[i][4].ToString()) == "0.00" || Convert.ToString(ds.Tables[0].Rows[i][4].ToString()) == "")
                        Utils.MostrarAlerta(Response, "El Item : " + ds.Tables[0].Rows[i][1].ToString() + " para la lista de precios : " + this.ddlListasPrecios.SelectedItem + " no tiene un precio especificado. Por Favor Agregar, para evitar inconvenientes con la creación y edición de ordenes de trabajo");
					this.dtItems.Rows.Add(fila);
					this.Bind_DgItems();
				}
			}
			return agregados;
		}
		
		private void Eliminar_Operaciones(string codigoItem)
		{
			int i,j;
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT ptem_operacion FROM mitemoperacion WHERE mite_codigo='"+codigoItem+"'");
			//Ya tenemos los codigos de las operaciones relacionadas, solo se podran eliminar las que no tengan otros items relacionados
			for(i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow[] selection = this.dtOperaciones.Select("CODIGO = '"+ds.Tables[0].Rows[i][0].ToString()+"'");
				if(selection.Length == 1)
				{
					//Se puede eliminar porque aun existe la operacion
					bool eliminar = true;
					DataSet ds1 = new DataSet();
					DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT mite_codigo FROM mitemoperacion WHERE ptem_operacion='"+ds.Tables[0].Rows[i][0].ToString()+"' AND mite_codigo <> '"+codigoItem+"'");
					for(j=0;j<ds1.Tables[0].Rows.Count;j++)
					{
						DataRow[] selection1 = this.dtItems.Select("ITEM = '"+ds1.Tables[0].Rows[j][0].ToString()+"'");
						if(selection1.Length == 1)
								eliminar = false;
					}
					if(eliminar)
					{
						for(j=0;j<this.dtOperaciones.Rows.Count;j++)
						{
							if(this.dtOperaciones.Rows[j] == selection[0])
								this.dtOperaciones.Rows[j].Delete();
						}
					}
				}
			}
			this.Bind_DgOperaciones();
		}
		
		private void Eliminar_Items(string codigoOperacion)
		{
			int i,j;
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mite_codigo FROM mitemoperacion WHERE ptem_operacion='"+codigoOperacion+"'");
			for(i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow[] selection = this.dtItems.Select("ITEM = '"+ds.Tables[0].Rows[i][0].ToString()+"'");
				if(selection.Length == 1)
				{
					bool eliminar = true;
					DataSet ds1 = new DataSet();
					DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT ptem_operacion FROM mitemoperacion WHERE mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"' AND ptem_operacion <> '"+codigoOperacion+"'");
					for(j=0;j<ds1.Tables[0].Rows.Count;j++)
					{
						DataRow[] selection1 = this.dtOperaciones.Select("CODIGO = '"+ds1.Tables[0].Rows[j][0].ToString()+"'");
						if(selection1.Length == 1)
								eliminar = false;
					}
					if(eliminar)
					{
						for(j=0;j<this.dtItems.Rows.Count;j++)
						{
							if(this.dtItems.Rows[j] == selection[0])
								this.dtItems.Rows[j].Delete();
						}
					}
				}
			}
			this.Bind_DgItems();
		}
	
		#endregion
		
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
