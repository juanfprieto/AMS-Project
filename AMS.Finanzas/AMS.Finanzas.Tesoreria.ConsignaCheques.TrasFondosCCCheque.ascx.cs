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
	public partial class TrasladoFondosCheque : System.Web.UI.UserControl
	{
		protected DataTable tablaDatos;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				DatasToControls bind=new DatasToControls();
                //bind.PutDatasIntoDropDownList(prefijoEgreso, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CE' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref prefijoEgreso , "%", "%", "CE");
				//string Sql = "SELECT M.mtes_numero FROM dbxschema.ttipopago T,dbxschema.dtesoreriadocumentos D,dbxschema.pbanco P,dbxschema.mtesoreria M,dbxschema.mcajapago C WHERE (D.ttip_codigo=T.ttip_codigo AND D.pban_banco=P.pban_codigo AND M.pdoc_codigo=D.mtes_codigo AND M.pcue_codigo=D.pcue_codigo AND M.mtes_numero=D.mtes_numero AND C.pdoc_codigo=D.mcaj_codigo AND C.mcaj_numero=D.mcaj_numero) AND D.mtes_codigo='"+this.prefijoEgreso.SelectedValue+"' AND C.test_estado='C'";
                     //      "SELECT M.mcaj_numero FROM mcaja M,pdocumento P,tdocumento T WHERE T.tdoc_tipodocu=P.tdoc_tipodocu AND M.pdoc_codigo=P.pdoc_codigo AND P.pdoc_codigo='"+prefijoEgreso.SelectedValue+"'"
				string Sql = "SELECT M.mcaj_numero FROM mcaja M,pdocumento P,tdocumento T,mcajapago MP WHERE T.tdoc_tipodocu=P.tdoc_tipodocu AND M.pdoc_codigo=P.pdoc_codigo AND P.pdoc_codigo='CE' and m.pdoc_codigo = MP.pdoc_codigo and m.mcaj_numero = MP.mcaj_numero and MP.test_estado = 'C' and MP.ttip_codigo = 'C'";
				//and MP.pban_codigo = '" + codigoCCOb.Text + "'";
				bind.PutDatasIntoDropDownList(numeroEgreso,Sql);
			}
			else
			{
				if(Session["tablaDatos"]!=null)
					tablaDatos=(DataTable)Session["tablaDatos"];
			}
		}
		
		protected void Cargar_Tabla_Datos()
		{
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("CODIGORECIBOCAJA",typeof(string));
			tablaDatos.Columns.Add("NUMERORECIBOCAJA",typeof(string));
			tablaDatos.Columns.Add("TIPOPAGO",typeof(string));
			tablaDatos.Columns.Add("NUMERO",typeof(string));
			tablaDatos.Columns.Add("NOMBREBANCO",typeof(string));
			tablaDatos.Columns.Add("VALOR",typeof(double));
			tablaDatos.Columns.Add("NIT",typeof(string));
			tablaDatos.Columns.Add("NITBENEFICIARIO",typeof(string));
			tablaDatos.Columns.Add("FECHA",typeof(string));
			tablaDatos.Columns.Add("ESTADO",typeof(bool));
		}
		
		protected void Cambiar_Fecha(object Sender,EventArgs e)
		{
			fecha.Text=calendarioFecha.SelectedDate.Date.ToString("yyyy-MM-dd");
		}
		
		protected void Escoger_Egreso(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroEgreso,"SELECT M.mcaj_numero FROM mcaja M,pdocumento P,tdocumento T WHERE T.tdoc_tipodocu=P.tdoc_tipodocu AND M.pdoc_codigo=P.pdoc_codigo AND P.pdoc_codigo='"+prefijoEgreso.SelectedValue+"'");
		}
		
		protected void Aceptar_Datos(object Sender,EventArgs e)
		{
			DataSet ds=new DataSet();
			DataRow fila;
			int i;
			if(numeroEgreso.Items.Count==0)
                Utils.MostrarAlerta(Response, "No hay recibos creados con ese prefijo");
			else
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pdoc_codigo,M.mcaj_numero,T.ttip_nombre,P.mcpag_numerodoc,B.pban_nombre,P.mcpag_valor,M.mnit_nit,M.mnit_nitben,M.mcaj_fecha FROM dbxschema.mcaja M,dbxschema.mcajapago P,dbxschema.ttipopago T,dbxschema.pbanco B WHERE (M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND P.ttip_codigo=T.ttip_codigo AND P.pban_codigo=B.pban_codigo) AND P.pdoc_codigo='"+prefijoEgreso.SelectedValue+"' AND P.mcaj_numero="+numeroEgreso.SelectedValue+" AND P.test_estado='C'");
				if(ds.Tables[0].Rows.Count!=0)
				{
					if(Session["tablaDatos"]==null)
						this.Cargar_Tabla_Datos();
					for(i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						fila=tablaDatos.NewRow();
						fila["CODIGORECIBOCAJA"]=ds.Tables[0].Rows[0][0].ToString();
						fila["NUMERORECIBOCAJA"]=ds.Tables[0].Rows[0][1].ToString();
						fila["TIPOPAGO"]=ds.Tables[0].Rows[0][2].ToString();
						fila["NUMERO"]=ds.Tables[0].Rows[0][3].ToString();
						fila["NOMBREBANCO"]=ds.Tables[0].Rows[0][4].ToString();
						fila["VALOR"]=Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString());
						fila["NIT"]=ds.Tables[0].Rows[0][6].ToString();
						fila["NITBENEFICIARIO"]=ds.Tables[0].Rows[0][7].ToString();
						fila["FECHA"]=System.Convert.ToDateTime(ds.Tables[0].Rows[0][8].ToString()).ToString("yyyy-MM-dd");
						fila["ESTADO"]=false;
						tablaDatos.Rows.Add(fila);
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
						Session["tablaDatos"]=tablaDatos;
					}
					aceptarDatos.Enabled=false;
				}
				else
                Utils.MostrarAlerta(Response, "No hay cheques disponibles en ese comprobante");
			}
		}
		
		protected void gridDatos_Item(object Sender,DataGridCommandEventArgs e)
		{
			Control padre=(this.Parent).Parent;
			if(((Button)e.CommandSource).CommandName=="Agregar")
			{
				//Si la cuenta no está exenta de impuesto (o sea que se cobra impuesto)
				if(DBFunctions.SingleData("SELECT tres_exenimpuesto FROM pcuentacorriente WHERE pcue_codigo='"+this.codigoCCO.Text+"'")=="N")
				{
					//Si existe algun porcentaje
					if(Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='"+this.codigoCCO.Text+"'"))!=0)
					{
						((TextBox)padre.FindControl("valorConsignado")).Visible=true;
						((TextBox)padre.FindControl("totalEfectivo")).Visible=true;
						((TextBox)padre.FindControl("valorConsignado")).Text=(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString())+Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1))).ToString("C");
						((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
						((TextBox)padre.FindControl("totalEfectivo")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("totalEfectivo")).Text.Substring(1)))+(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString())*Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='"+this.codigoCCO.Text+"'"))/100)).ToString("C");
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
				tablaDatos.Rows[e.Item.DataSetIndex][9]=true;
				((Button)e.Item.Cells[9].Controls[1]).Enabled=false;
				((Button)e.Item.Cells[10].Controls[1]).Enabled=true;
				((Panel)padre.FindControl("panelValores")).Visible=true;
				((Label)padre.FindControl("lbDetalle")).Text="Detalle de la Transferencia :";
				((Label)padre.FindControl("lbValor")).Text="Valor Transferido :";
				((Label)padre.FindControl("lbTotalEf")).Text="Total Impuestos :";
				((Button)padre.FindControl("guardar")).Enabled=true;
			}
			else if(((Button)e.CommandSource).CommandName=="Remover")
			{
				((TextBox)padre.FindControl("valorConsignado")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)))-(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				((TextBox)padre.FindControl("totalEfectivo")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("totalEfectivo")).Text.Substring(1)))-(Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString())*Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='"+this.codigoCCO.Text+"'"))/100)).ToString("C");
				tablaDatos.Rows[e.Item.DataSetIndex][9]=false;
				((Button)e.Item.Cells[9].Controls[1]).Enabled=true;
				((Button)e.Item.Cells[10].Controls[1]).Enabled=false;
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

		}
		#endregion
	}
}
