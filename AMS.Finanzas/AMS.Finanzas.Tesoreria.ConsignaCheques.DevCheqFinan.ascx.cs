using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial class DevolucionChequesFinanciera : System.Web.UI.UserControl
	{
		protected DataTable tablaDatos;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
                //bind.PutDatasIntoDropDownList(ddlPrefijo, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='FP' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref ddlPrefijo , "%", "%", "FP");
			}
			else
			{
				if(Session["tablaDatos"]!=null)
					tablaDatos=(DataTable)Session["tablaDatos"];
			}
		}
		
		protected void Cargar_Tabla()
		{
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("CODIGOREM",typeof(string));//0
			tablaDatos.Columns.Add("NUMEROREM",typeof(string));//1
			tablaDatos.Columns.Add("CODIGOFAC",typeof(string));//2
			tablaDatos.Columns.Add("NUMEROFAC",typeof(string));//3
			tablaDatos.Columns.Add("NUMERO",typeof(string));//4
			tablaDatos.Columns.Add("VALOR",typeof(double));//5
			tablaDatos.Columns.Add("FECHA",typeof(string));//6
			tablaDatos.Columns.Add("ESTADO",typeof(bool));//7
			tablaDatos.Columns.Add("PREFIJONOTA",typeof(string));//8
		}
		
		protected void aceptar_Click(object Sender,EventArgs e)
		{
			Session.Clear();
			DataSet ds=new DataSet();
			DataRow fila;
			int i;
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pdoc_codigo,M.mtes_numero,D.mfac_codigo,D.mfac_numero,D.dtes_numdoc,D.dtes_valor,M.mtes_fecha FROM dbxschema.mtesoreria M,dbxschema.dtesoreriaremision D,dbxschema.mcajapago G WHERE M.pdoc_codigo=D.mtes_codigo AND M.mtes_numero=D.mtes_numero AND G.pdoc_codigo=D.mcaj_codigo AND G.mcaj_numero=D.mcaj_numero AND G.mcaj_consecpag=D.mcaj_cons AND G.test_estado='G' AND D.mnit_nitfinanciera='"+this.tbNitFin.Text+"'");
			if(ds.Tables[0].Rows.Count!=0)
			{
				if(Session["tablaDatos"]==null)
					this.Cargar_Tabla();
				for(i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=tablaDatos.NewRow();
					fila[0]=ds.Tables[0].Rows[i][0].ToString();//Prefijo Remision
					fila[1]=ds.Tables[0].Rows[i][1].ToString();//Número Remision
					fila[2]=ds.Tables[0].Rows[i][2].ToString();//Codigo Factura
					fila[3]=ds.Tables[0].Rows[i][3].ToString();//Número Factura
					fila[4]=ds.Tables[0].Rows[i][4].ToString();//Número Cheque
					fila[5]=Convert.ToDouble(ds.Tables[0].Rows[i][5].ToString());//Valor
					fila[6]=Convert.ToDateTime(ds.Tables[0].Rows[i][6].ToString()).ToString("yyyy-MM-dd");//Fecha Remision
					fila[7]=false;
					tablaDatos.Rows.Add(fila);
				}
				Session["tablaDatos"]=tablaDatos;					
				gridDatos.DataSource=tablaDatos;
				gridDatos.DataBind();
			}
			else
            Utils.MostrarAlerta(Response, "No hay cheques de esa financiera");
		}
		
		protected bool Validar_CheckBox(ref double valor)
		{
			bool exito=false;
			int cont=0;
			for(int i=0;i<gridDatos.Items.Count;i++)
			{
				if(!((CheckBox)gridDatos.Items[i].Cells[8].Controls[1]).Checked)
					cont++;
				else
				{
					valor=valor+Convert.ToDouble(tablaDatos.Rows[i][5]);
					((CheckBox)gridDatos.Items[i].Cells[8].Controls[1]).Checked=false;
					gridDatos.Items[i].Visible=false;
					tablaDatos.Rows[i][7]=true;
					tablaDatos.Rows[i][8]=((DropDownList)gridDatos.Items[i].FindControl("ddlPrefNot")).SelectedValue;
					Session["tablaDatos"]=tablaDatos;
				}
			}
			if(cont==gridDatos.Items.Count)
				exito=true;
			return exito;
		}
		
		protected void gridDatos_Item(object Sender,DataGridCommandEventArgs e)
		{
			double valor=0;
			Control padre=(this.Parent).Parent;
			if(((Button)e.CommandSource).CommandName=="devolver")
			{
				if(!Validar_CheckBox(ref valor))
				{
					((Label)padre.FindControl("lbDetalle")).Text="Detalle de la Devolución :";
					((Label)padre.FindControl("lbDetalle")).Visible=true;
					((Label)padre.FindControl("lbValor")).Text="Valor Devuelto :";
					((Label)padre.FindControl("lbValor")).Visible=true;
					((Label)padre.FindControl("lbTotalEf")).Visible=false;
					((Panel)padre.FindControl("panelValores")).Visible=true;
					((TextBox)padre.FindControl("totalEfectivo")).Visible=false;
					((TextBox)padre.FindControl("detalleTransaccion")).Visible=true;
					((TextBox)padre.FindControl("valorConsignado")).Visible=true;
					((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
					((TextBox)padre.FindControl("valorConsignado")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)))+valor).ToString("C");
					((Button)padre.FindControl("guardar")).Enabled=true;
					((Button)e.Item.Cells[8].Controls[1]).Text="Cancelar";
					((Button)e.Item.Cells[8].Controls[1]).CommandName="cancelar";
				}
				else
                Utils.MostrarAlerta(Response, "Debe seleccionar por lo menos un cheque");
			}
			else if(((Button)e.CommandSource).CommandName=="cancelar")
			{
				double valor2=0;
				((Button)e.Item.Cells[7].Controls[1]).CommandName="devolver";
				((Button)e.Item.Cells[7].Controls[1]).Text="Devolver";
				for(int i=0;i<gridDatos.Items.Count;i++)
				{
					if(gridDatos.Items[i].Visible==false)
					{
						((CheckBox)gridDatos.Items[i].Cells[7].Controls[1]).Checked=false;
						gridDatos.Items[i].Visible=true;
						valor2=valor2+(Convert.ToDouble(tablaDatos.Rows[i][5]));
						tablaDatos.Rows[i][7]=false;
					}
				}
				Session["tablaDatos"]=tablaDatos;
				((TextBox)padre.FindControl("valorConsignado")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)))-valor2).ToString("C");
				if(((TextBox)padre.FindControl("valorConsignado")).Text=="$0.00")
					((Button)padre.FindControl("guardar")).Enabled=false;
			}
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
			this.gridDatos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridDatos_ItemDataBound);

		}
		#endregion

		private void gridDatos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			DatasToControls bind=new DatasToControls();
            if(e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[7].FindControl("ddlPrefNot")), "SELECT pdoc_codigo,pdoc_codigo ||'-'|| pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='NC' and tvig_vigencia = 'V' ");
		}
	}
}
