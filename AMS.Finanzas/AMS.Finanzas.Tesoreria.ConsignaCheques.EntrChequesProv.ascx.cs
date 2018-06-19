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
	public partial class EntegaChequesProveedores : System.Web.UI.UserControl
	{
		protected DataTable tablaDatos;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
                //bind.PutDatasIntoDropDownList(prefijoEgreso, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CE' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref prefijoEgreso , "%", "%", "CE");
				bind.PutDatasIntoDropDownList(numeroEgreso,"SELECT M.mcaj_numero FROM mcaja M,pdocumento P,tdocumento T WHERE T.tdoc_tipodocu=P.tdoc_tipodocu AND M.pdoc_codigo=P.pdoc_codigo AND P.pdoc_codigo='"+prefijoEgreso.SelectedValue+"'");
			}
			else
			{
				if(Session["tablaDatos"]!=null)
					this.tablaDatos=(DataTable)Session["tablaDatos"];
			}
		}
		
		protected void Cargar_Tabla()
		{
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("PREFIJOCAJA",typeof(string));
			tablaDatos.Columns.Add("NUMEROCAJA",typeof(string));
			tablaDatos.Columns.Add("TIPOPAGO",typeof(string));
			tablaDatos.Columns.Add("NUMERO",typeof(string));
			tablaDatos.Columns.Add("NOMBREBANCO",typeof(string));
			tablaDatos.Columns.Add("VALOR",typeof(double));
			tablaDatos.Columns.Add("NITBENEFICIARIO",typeof(string));
			tablaDatos.Columns.Add("FECHA",typeof(string));
			tablaDatos.Columns.Add("ESTADO",typeof(bool));
		}
		
		protected void Cambiar_Fecha(object Sender,EventArgs e)
		{
			fecha.Text=calendarioFecha.SelectedDate.Date.ToString("yyyy-MM-dd");
		}
		
		protected void IndexChanged_prefijoEgreso(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroEgreso,"SELECT M.mcaj_numero FROM mcaja M,pdocumento P,tdocumento T WHERE T.tdoc_tipodocu=P.tdoc_tipodocu AND M.pdoc_codigo=P.pdoc_codigo AND P.pdoc_codigo='"+prefijoEgreso.SelectedValue+"'");
		}
		
		protected void aceptarDatos_Click(object Sender,EventArgs e)
		{
			DataSet ds;
			DataRow fila;
			int i;
			//Session.Clear();
			//gridDatos.DataBind();
			if(this.numeroEgreso.Items.Count==0)
                Utils.MostrarAlerta(Response, "No existen documentos con ese prefijo. Escoja otro");
			else
			{
				ds=new DataSet();
				//"SELECT M.pdoc_codigo,M.mcaj_numero,T.ttip_nombre,P.mcpag_numerodoc,B.pban_nombre,P.mcpag_valor,M.mnit_nitben,M.mcaj_fecha FROM dbxschema.mcaja M,dbxschema.mcajapago P,dbxschema.ttipopago T,dbxschema.pbanco B WHERE (M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND P.ttip_codigo=T.ttip_codigo AND P.pban_codigo=B.pban_codigo) AND P.pdoc_codigo='"+this.prefijoEgreso.SelectedValue+"' AND P.mcaj_numero="+this.numeroEgreso.SelectedValue+" AND P.test_estado='C' AND P.ttip_codigo='C'"
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pdoc_codigo,M.mcaj_numero,T.ttip_nombre,P.mcpag_numerodoc,B.pban_nombre,P.mcpag_valor,M.mnit_nitben,M.mcaj_fecha FROM dbxschema.mcaja M,dbxschema.mcajapago P,dbxschema.ttipopago T,dbxschema.pbanco B,dbxschema.pcuentacorriente C,dbxschema.pchequera H WHERE M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND P.ttip_codigo=T.ttip_codigo AND P.pche_id=H.pche_id AND H.pcue_codigo=C.pcue_codigo AND C.pban_banco=B.pban_codigo AND P.test_estado='C' AND P.ttip_codigo='C' AND C.pcue_codigo='"+codigoCC.Text+"' AND P.pdoc_codigo='"+prefijoEgreso.SelectedValue+"' AND P.mcaj_numero="+numeroEgreso.SelectedValue+"");
				if(ds.Tables[0].Rows.Count!=0)
				{
					if(Session["tablaDatos"]==null)
						this.Cargar_Tabla();
					for(i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						fila=tablaDatos.NewRow();
						fila[0]=ds.Tables[0].Rows[i][0].ToString();//Prefijo Egreso
						fila[1]=ds.Tables[0].Rows[i][1].ToString();//Número Egreso
						fila[2]=ds.Tables[0].Rows[i][2].ToString();//Tipo de Pago
						fila[3]=ds.Tables[0].Rows[i][3].ToString();//Número
						fila[4]=ds.Tables[0].Rows[i][4].ToString();//Banco
						fila[5]=Convert.ToDouble(ds.Tables[0].Rows[i][5].ToString());//Valor
						fila[6]=ds.Tables[0].Rows[i][6].ToString();//Nit
						fila[7]=Convert.ToDateTime(ds.Tables[0].Rows[i][7].ToString()).ToString("yyyy-MM-dd");//Fecha
						fila[8]=false;//Estado
						tablaDatos.Rows.Add(fila);
						Session["tablaDatos"]=tablaDatos;
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
					}
				}
				else
                Utils.MostrarAlerta(Response, "No hay cheques girados en este comprobante de egreso de la cuenta corriente seleccionada");
			}
		}
		
		protected void gridDatos_Item(object Sender,DataGridCommandEventArgs e)
		{
			Control padre=(this.Parent).Parent;
			if(((Button)e.CommandSource).CommandName=="agregar")
			{
				//Si la cuenta no está exenta de impuesto (o sea que se cobra impuesto)
				if(DBFunctions.SingleData("SELECT tres_exenimpuesto FROM pcuentacorriente WHERE pcue_codigo='"+this.codigoCC.Text+"'")=="N")
				{
					//Si existe algun porcentaje
					if(Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='"+this.codigoCC.Text+"'"))!=0)
					{
						((TextBox)padre.FindControl("valorConsignado")).Visible=true;
						((TextBox)padre.FindControl("totalEfectivo")).Visible=true;
						((TextBox)padre.FindControl("valorConsignado")).Text=(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString())+Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1))).ToString("C");
						((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
						((TextBox)padre.FindControl("totalEfectivo")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("totalEfectivo")).Text.Substring(1)))+(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString())*Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='"+this.codigoCC.Text+"'"))/100)).ToString("C");
						((TextBox)padre.FindControl("totalEfectivo")).ReadOnly=true;
					}
					//Si no existe porcentaje
					else
					{
                        Utils.MostrarAlerta(Response, "La cuenta tiene impuesto pero el valor especificado es cero. Revise los parametros");
						((TextBox)padre.FindControl("valorConsignado")).Visible=true;
						((TextBox)padre.FindControl("totalEfectivo")).Visible=true;
						((TextBox)padre.FindControl("valorConsignado")).Text=(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString())+Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1))).ToString("C");
						((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
						((TextBox)padre.FindControl("totalEfectivo")).Text=Convert.ToDouble("0").ToString("C");
						((TextBox)padre.FindControl("totalEfectivo")).ReadOnly=true;
					}
				}
				//Si está exenta
				else
				{
					((TextBox)padre.FindControl("valorConsignado")).Visible=true;
					((TextBox)padre.FindControl("totalEfectivo")).Visible=true;
					((TextBox)padre.FindControl("valorConsignado")).Text=(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString())+Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1))).ToString("C");
					((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
					((TextBox)padre.FindControl("totalEfectivo")).Text=Convert.ToDouble("0").ToString("C");
					((TextBox)padre.FindControl("totalEfectivo")).ReadOnly=true;
				}
				tablaDatos.Rows[e.Item.DataSetIndex][8]=true;
				Session["tablaDatos"]=tablaDatos;
				((Panel)padre.FindControl("panelValores")).Visible=true;
				((Label)padre.FindControl("lbDetalle")).Text="Detalle de la Transferencia :";
				((Label)padre.FindControl("lbValor")).Text="Valor Transferido :";
				((Label)padre.FindControl("lbTotalEf")).Text="Total Impuestos :";
				((Button)padre.FindControl("guardar")).Enabled=true;
				((Button)e.Item.Cells[8].Controls[1]).CommandName="remover";
				((Button)e.Item.Cells[8].Controls[1]).Text="Remover";
				
			}
			else if(((Button)e.CommandSource).CommandName=="remover")
			{
				((TextBox)padre.FindControl("valorConsignado")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)))-(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				((TextBox)padre.FindControl("totalEfectivo")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("totalEfectivo")).Text.Substring(1)))-(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString())*Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='"+this.codigoCC.Text+"'")))).ToString("C");
				tablaDatos.Rows[e.Item.DataSetIndex][8]=false;
				Session["tablaDatos"]=tablaDatos;
				((Button)e.Item.Cells[8].Controls[1]).CommandName="agregar";
				((Button)e.Item.Cells[8].Controls[1]).Text="Agregar";
				if(((TextBox)padre.FindControl("valorConsignado")).Text=="$0.00")
					((Button)padre.FindControl("guardar")).Enabled=false;
			}
		}

		private void gridDatos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.AlternatingItem || e.Item.ItemType==ListItemType.Item)
			{
				if(Convert.ToBoolean(tablaDatos.Rows[e.Item.DataSetIndex][8]))
					((Button)e.Item.Cells[8].FindControl("agregar")).Text="Remover";
				else
					((Button)e.Item.Cells[8].FindControl("agregar")).Text="Agregar";
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
	}
}
