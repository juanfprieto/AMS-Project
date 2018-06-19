using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Documentos;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class RemisionAlmacenes : System.Web.UI.UserControl
	{
		#region Propiedades

		protected DataTable dtItems;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoRecibo = new FormatosDocumentos();

		#endregion

		#region Eventos
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(RemisionAlmacenes));

			if(!IsPostBack)
			{
				if(Request["prefRem"] != String.Empty && Request["numRem"] != String.Empty)
				{
					formatoRecibo.Prefijo = Request["prefRem"];
					formatoRecibo.Numero = Convert.ToInt32(Request["numRem"]);
					formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM pdocumento WHERE pdoc_codigo='"+ Request["prefRem"] +"'");
					if(formatoRecibo.Cargar_Formato())
						Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=600');</script>");
				}
				Session.Clear();
				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(ddlPrefRemi,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='RA' AND TVIG_VIGENCIA = 'V'");
                Utils.llenarPrefijos(Response, ref ddlPrefRemi, "%", "%", "RA");
                bind.PutDatasIntoDropDownList(ddlAlmaOrigen, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
                bind.PutDatasIntoDropDownList(ddlAlmaDestino, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
				bind.PutDatasIntoDropDownList(ddlVendedor,"SELECT pven_codigo, pven_nombre FROM pvendedor WHERE tvend_codigo='VM' and pven_vigencia = 'V'");
				tbNumRemi.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefRemi.SelectedValue+"'");
				PrepararDtItems();
				BindDgItems();
			}
			if(Session["dtItems"]==null)
				this.PrepararDtItems();
			else
				dtItems = (DataTable)Session["dtItems"];
		}
		
		protected void DgItemsDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Footer)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]),"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				
                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("ondblclick","MostrarRefs("+((TextBox)e.Item.Cells[0].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[2].Controls[1]).ClientID+","+ddlAlmaOrigen.ClientID+",'"+DBFunctions.SingleData("SELECT pano_ano from cinventario")+"');");
				((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onkeyup","ItemMask("+((TextBox)e.Item.Cells[0].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[2].Controls[1]).ClientID+");");
                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onblur", "MostrarRefsBlur(" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "," + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + "," + ddlAlmaOrigen.ClientID + ",'" + DBFunctions.SingleData("SELECT pano_ano from cinventario") + "'," + ((TextBox)e.Item.Cells[1].Controls[1]).ClientID + "," + ((TextBox)e.Item.Cells[3].Controls[1]).ClientID + ");" );

                ((DropDownList)e.Item.Cells[2].Controls[1]).Attributes.Add("onchange","ChangeLine("+((DropDownList)e.Item.Cells[2].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[0].Controls[1]).ClientID+");");
			}
		}
		
		protected void DgItemsAddAndDel(object sender, DataGridCommandEventArgs e)
		{
			//Revisamos si los almacenes son diferentes o no
			if(ddlAlmaOrigen.SelectedValue != ddlAlmaDestino.SelectedValue)
			{
				if(e.CommandName == "AddDatasRow")
				{
					if(((TextBox)e.Item.Cells[0].Controls[1]).Text == "" || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text))
                        Utils.MostrarAlerta(Response, "Datos invalidos para la inserción!");
					else
					{
						string codI = "";
						if(!Referencias.Guardar(((TextBox)e.Item.Cells[0].Controls[1]).Text,ref codI,(((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[1]))
						{
                            Utils.MostrarAlerta(Response, "El codigo " + ((TextBox)e.Item.Cells[0].Controls[1]).Text + " no es valido para la linea de bodega " + ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedItem.Text + ".\\Revise Por Favor!");
							return;
						}
						if(!Referencias.RevisionSustitucion(ref codI,(((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[0]))
						{
                            Utils.MostrarAlerta(Response, "El codigo " + ((TextBox)e.Item.Cells[0].Controls[1]).Text + " no se encuentra registrado.\\Revise Por Favor!");
							return;
						}
						string codTmp2 = "";
						Referencias.Editar(codI,ref codTmp2,(((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[1]);
						if(codTmp2 != ((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim())
                            Utils.MostrarAlerta(Response, "El codigo " + ((TextBox)e.Item.Cells[0].Controls[1]).Text + " ha sido sustituido.\\El codigo actual es " + codTmp2 + "!");
						//Revisamos si ya se ha agregado eset item en este translado
						DataRow[] selection = dtItems.Select("CODIGO='"+codTmp2+"'");
						if(selection.Length == 0)
						{
							double cantidadTranslado = Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
							double cantidadDisponible = 0;
							try{cantidadDisponible = Convert.ToDouble(DBFunctions.SingleData("SELECT CAST(msal_cantactual - msal_cantasig AS integer) FROM msaldoitemalmacen WHERE mite_codigo='"+codI+"' AND palm_almacen='"+ddlAlmaOrigen.SelectedValue+"' AND pano_ano="+DBFunctions.SingleData("SELECT pano_ano from cinventario")+""));}
							catch{}
							if(cantidadDisponible <= 0)
							{
                                Utils.MostrarAlerta(Response, "La cantidad disponible el menor o igual a 0");
								return;
							}
							DataRow fila = dtItems.NewRow();
							fila[0] = codI;
							fila[1] = codTmp2;
							fila[2] = (((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[1];
							if(((TextBox)e.Item.Cells[1].Controls[1]).Text != "")
								fila[3] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
							else
								fila[3] = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
							fila[4] = cantidadDisponible;
							try{fila[6]=Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitemalmacen WHERE mite_codigo='"+codI+"' AND palm_almacen='"+ddlAlmaOrigen.SelectedValue+"' AND pano_ano="+DBFunctions.SingleData("SELECT pano_ano from cinventario")+""));}
							catch{fila[6]=0;}
							try{fila[7]=Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitemalmacen WHERE mite_codigo='"+codI+"' AND palm_almacen='"+ddlAlmaDestino.SelectedValue+"' AND pano_ano="+DBFunctions.SingleData("SELECT pano_ano from cinventario")+""));}
							catch{fila[7]=0;}
							if(cantidadTranslado>cantidadDisponible)
							{
								fila[5] = cantidadDisponible;
                                Utils.MostrarAlerta(Response, "La cantidad solicitada supera la disponibilidad.\\nSe ha asignado la maxima disponibilidad a esta solicitud : " + cantidadDisponible.ToString() + "");
							}
							else
								fila[5] = cantidadTranslado;
							if(cantidadTranslado>0)
							{
								dtItems.Rows.Add(fila);
								BindDgItems();
								ddlAlmaOrigen.Enabled = ddlAlmaDestino.Enabled = false;
							}
							else
                                Utils.MostrarAlerta(Response, "La cantidad para el traslado debe ser mayor a 0!");
						}
						else
                            Utils.MostrarAlerta(Response, "Este Item ya se ha agregado. Por Favor intentar editando!");
					}
				}
			}
			else
                Utils.MostrarAlerta(Response, "El almacen de destino es igual al de origen. Por favor modificar estos valores!");
		}
		
		protected void DgItemsDelete(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				dtItems.Rows.Remove(dtItems.Rows[e.Item.ItemIndex]);
				dgItems.EditItemIndex=-1;
			}
			catch{};
			BindDgItems();
			if(dtItems.Rows.Count == 0)
				ddlAlmaOrigen.Enabled = ddlAlmaDestino.Enabled = true;
		}
		
		public void DgItemsEdit(Object sender, DataGridCommandEventArgs e)
		{
			if(dtItems.Rows.Count>0)
				dgItems.EditItemIndex=(int)e.Item.ItemIndex;
			BindDgItems();
		}
		
		public void DgItemsCancel(Object sender, DataGridCommandEventArgs e)
		{
			dgItems.EditItemIndex=-1;
			BindDgItems();
		}

		public void DgItemsUpdate(Object sender, DataGridCommandEventArgs e)
		{
			//Revisamos que el valor de edicion es valido o no
			if(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text))
                Utils.MostrarAlerta(Response, "Valor invalido para la cantidad de items!");
			else
			{
				double cantidadTranslado = Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
				double cantidadDisponible = Convert.ToDouble(dtItems.Rows[dgItems.EditItemIndex][4]);
				if(cantidadTranslado>0)
				{
					if(cantidadTranslado>=cantidadDisponible)
						dtItems.Rows[dgItems.EditItemIndex][5] = cantidadDisponible;
					else
						dtItems.Rows[dgItems.EditItemIndex][5] = cantidadTranslado;
				}
				else
                    Utils.MostrarAlerta(Response, "La cantidad no puede ser menor o igual a 0!");
			}
		}

		#endregion
		
		#region Metodos

		protected void PrepararDtItems()
		{
			dtItems = new DataTable();
			dtItems.Columns.Add(new DataColumn("CODIGOORIGINAL",System.Type.GetType("System.String")));//0
			dtItems.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//1
			dtItems.Columns.Add(new DataColumn("LINEA",System.Type.GetType("System.String")));//2
			dtItems.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));//3
			dtItems.Columns.Add(new DataColumn("CANTIDADDISPONIBLE",System.Type.GetType("System.Double")));//4
			dtItems.Columns.Add(new DataColumn("CANTIDADTRANSLADO",System.Type.GetType("System.Double")));//5
			dtItems.Columns.Add(new DataColumn("COSTOPROMEDIOORIGEN",System.Type.GetType("System.Double")));//6
			dtItems.Columns.Add(new DataColumn("COSTOPROMEDIODESTINO",System.Type.GetType("System.Double")));//7
		}
		
		protected void BindDgItems()
		{
			Session["dtItems"] = dtItems;
			dgItems.DataSource = dtItems;
			dgItems.DataBind();
			for(int i=0;i<dgItems.Columns.Count;i++)
			{
				if(i==3 || i==4)
				{
					for(int j=0;j<dgItems.Items.Count;j++)
						dgItems.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
				}
			}
		}
		
		public void CambioDocumento(object sender, System.EventArgs e)
		{
			tbNumRemi.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefRemi.SelectedValue+"'");
		}
		
		public void RealizarRemision(object sender, System.EventArgs e)
		{

            if (pswVendedor.Text != (DBFunctions.SingleData("SELECT pven_clave FROM pvendedor WHERE pven_codigo='" + ddlVendedor.SelectedValue + "'")))
            {
                Utils.MostrarAlerta(Response, "La clave del Vendedor no es correcta");
                return;
            }

			string processMsg = "";

			uint numeroRemision = Convert.ToUInt32(tbNumRemi.Text.Trim());

            if (Remision.CrearRemision(ddlPrefRemi.SelectedValue, ref numeroRemision, ddlAlmaOrigen.SelectedValue, ddlAlmaDestino.SelectedValue, dtItems, ddlVendedor.SelectedValue, ref processMsg, txObservaciones.Text))
				Response.Redirect("" + indexPage + "?process=Inventarios.RemisionAlmacenes&prefRem="+ddlPrefRemi.SelectedValue+"&numRem="+numeroRemision);
			else
				lb.Text += "<br>Error : "+processMsg;
		}
		
		public void CancelarRemision(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Inventarios.RemisionAlmacenes");
		}

        [Ajax.AjaxMethod]
        public DataSet MostrarRefsOnBlur(string codigo, string almacen, string ano, string linea)
        {
            DataSet dtRemision = new DataSet();
            DBFunctions.Request(dtRemision, IncludeSchema.NO,
                       @"select * from 
                            (SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, 
                            CAST(MSIA.msal_cantactual - MSIA.msal_cantasig AS integer) AS CANTIDAD_DISPONIBLE 
                            FROM mitems MIT, msaldoitemalmacen MSIA, plineaitem PLIN WHERE MIT.mite_codigo = MSIA.mite_codigo AND 
                            MSIA.palm_almacen = '" + almacen + "' AND MSIA.pano_ano = '" + ano + "' AND PLIN.plin_tipo='" + linea + @"' 
                            AND (MSIA.msal_cantactual -MSIA.msal_cantasig)>0 AND MIT.plin_codigo = PLIN.plin_codigo  ORDER 
                            BY MIT.plin_codigo,MIT.mite_codigo) 
                        as REFERENCIAS where codigo = '" + codigo + "';");
            return dtRemision;
        }

		#endregion 
		
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

		protected void dgItems_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}
