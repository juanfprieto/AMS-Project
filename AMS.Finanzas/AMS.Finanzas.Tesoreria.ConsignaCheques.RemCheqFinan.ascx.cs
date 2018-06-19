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
	public partial class RemisionChequeFinanciera : System.Web.UI.UserControl
	{
		protected DataTable tablaDatos,tablaCheques;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
                //bind.PutDatasIntoDropDownList(prefijoCaja, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='RC' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref prefijoCaja , "%", "%", "RC");
				bind.PutDatasIntoDropDownList(numeroCaja,"SELECT mcaj_numero FROM mcaja WHERE pdoc_codigo='"+prefijoCaja.SelectedValue+"'");
				//bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'FC' and PH.tpro_proceso = 'RF' AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                Utils.llenarPrefijos(Response, ref prefijoFactura , "RF", "%", "FC");
			}
			else
			{
				if(Session["tablaDatos"]!=null)
					tablaDatos=(DataTable)Session["tablaDatos"];
				if(Session["tablaCheques"]!=null)
					tablaCheques=(DataTable)Session["tablaCheques"];
			}
		}
		
		protected void Cargar_TablaDatos()
		{
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("CODIGORECIBOCAJA",typeof(string));
			tablaDatos.Columns.Add("NUMERORECIBOCAJA",typeof(string));
			tablaDatos.Columns.Add("CONSPAGO",typeof(string));
			tablaDatos.Columns.Add("NUMERO",typeof(string));
			tablaDatos.Columns.Add("NOMBREBANCO",typeof(string));
			tablaDatos.Columns.Add("VALOR",typeof(double));
			tablaDatos.Columns.Add("NIT",typeof(string));
			tablaDatos.Columns.Add("FECHA",typeof(string));
		}
		
		protected void Cargar_TablaCheques()
		{
			tablaCheques=new DataTable();
			tablaCheques.Columns.Add("CODIGORECIBOCAJA",typeof(string));
			tablaCheques.Columns.Add("NUMERORECIBOCAJA",typeof(string));
			tablaCheques.Columns.Add("CONSPAGO",typeof(string));
			tablaCheques.Columns.Add("NUMERO",typeof(string));
			tablaCheques.Columns.Add("VALOR",typeof(double));
			tablaCheques.Columns.Add("COMISION",typeof(double));
			tablaCheques.Columns.Add("IVA",typeof(double));
			tablaCheques.Columns.Add("RETENCION",typeof(double));
		}
		
		protected void Cambiar_Fecha(object Sender,EventArgs e)
		{
			fecha.Text=calendarioFecha.SelectedDate.Date.ToString("yyyy-MM-dd");
		}
		
		protected void prefijoCaja_IndexChanged(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroCaja,"SELECT mcaj_numero FROM mcaja WHERE pdoc_codigo='"+prefijoCaja.SelectedValue+"'");
		}
		
		protected void aceptar_Click(object Sender,EventArgs e)
		{
			DataSet ds;
			DataRow fila;
			int i;
			Session.Remove("tablaDatos");
			gridDatos.DataBind();
			if(numeroCaja.Items.Count==0)
                Utils.MostrarAlerta(Response, "Escoja otro prefijo de recibo de caja");
			else
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pdoc_codigo,M.mcaj_numero,P.mcaj_consecpag,P.mcpag_numerodoc,B.pban_nombre,P.mcpag_valor,M.mnit_nitben,P.mcpag_fecha FROM mcaja M,pbanco B,mcajapago P,pdocumento D WHERE M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND D.pdoc_codigo=M.pdoc_codigo AND B.pban_codigo=P.pban_codigo AND D.tdoc_tipodocu='RC' AND P.test_estado='C' AND P.ttip_codigo='C' AND M.pdoc_codigo='"+prefijoCaja.SelectedValue+"' AND M.mcaj_numero="+numeroCaja.SelectedValue+"");
				if(ds.Tables[0].Rows.Count!=0)
				{
					if(Session["tablaDatos"]==null)
						this.Cargar_TablaDatos();
					for(i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						fila=tablaDatos.NewRow();
						fila[0]=ds.Tables[0].Rows[i][0].ToString();//Prefijo Caja
						fila[1]=ds.Tables[0].Rows[i][1].ToString();//Numero Caja
						fila[2]=ds.Tables[0].Rows[i][2].ToString();//Consecutivo Pago
						fila[3]=ds.Tables[0].Rows[i][3].ToString();//Numero Documento
						fila[4]=ds.Tables[0].Rows[i][4].ToString();//Banco
						fila[5]=Convert.ToDouble(ds.Tables[0].Rows[i][5].ToString());//Valor
						fila[6]=ds.Tables[0].Rows[i][6].ToString();//Nit
						fila[7]=Convert.ToDateTime(ds.Tables[0].Rows[i][7].ToString()).ToString("yyyy-MM-dd");//Fecha
						tablaDatos.Rows.Add(fila);
						Session["tablaDatos"]=tablaDatos;
						gridDatos.DataSource=tablaDatos;
						gridDatos.Visible=true;
						gridDatos.DataBind();
					}
					panelGrillas.Visible=true;
				}
				else
				{
					if(tablaCheques==null)
						panelGrillas.Visible=false;
                    Utils.MostrarAlerta(Response, "No hay cheques en ese recibo de caja");
				}
					
			}
		}
		
		protected bool Buscar_Objeto_Tabla_Cheques(string prefijo,string numero,string numdoc)
		{
			//Función que me retorna true si encuentra el objeto en tablaCheques
			//y false si no lo encuentra
			bool encontrado=false;
			if(tablaCheques!=null)
				for(int i=0;i<tablaCheques.Rows.Count;i++)
					if(tablaCheques.Rows[i][0].ToString()==prefijo && tablaCheques.Rows[i][1].ToString()==numero && tablaCheques.Rows[i][2].ToString()==numdoc)
						encontrado=true;
			return encontrado;
		}
		
		protected void gridDatos_Item(object Sender,DataGridCommandEventArgs e)
		{
			//int i;
			bool existe=false;
			DataRow fila;
			Control padre=(this.Parent).Parent;
			if(((Button)e.CommandSource).CommandName=="agregar")
			{
				if(Session["tablaCheques"]==null)
					this.Cargar_TablaCheques();
				if(tablaDatos.Rows.Count!=0)
				{
					foreach(DataGridItem item in gridDatos.Items)
					{
						if(((CheckBox)item.Cells[7].Controls[1]).Checked==true)
						{
							existe=Buscar_Objeto_Tabla_Cheques(tablaDatos.Rows[item.DataSetIndex][0].ToString(),tablaDatos.Rows[item.DataSetIndex][1].ToString(),tablaDatos.Rows[item.DataSetIndex][2].ToString());
							if(!existe)
							{
								fila=tablaCheques.NewRow();
								fila[0]=tablaDatos.Rows[item.DataSetIndex][0].ToString();
								fila[1]=tablaDatos.Rows[item.DataSetIndex][1].ToString();
								fila[2]=tablaDatos.Rows[item.DataSetIndex][2].ToString();
								fila[3]=tablaDatos.Rows[item.DataSetIndex][3].ToString();
								fila[4]=Convert.ToDouble(tablaDatos.Rows[item.DataSetIndex][5].ToString());
								fila[5]=Convert.ToDouble("0");
								fila[6]=Convert.ToDouble("0");
								fila[7]=Convert.ToDouble("0");
								tablaCheques.Rows.Add(fila);
								Session["tablaCheques"]=tablaCheques;
								aceptarCheques.Enabled=true;
								((Button)padre.FindControl("guardar")).Enabled=false;
							}
							else
                            Utils.MostrarAlerta(Response, "Ese cheque ya ha sido adicionado");
						}
					}
					gridCheques.DataSource=tablaCheques;
					gridCheques.DataBind();
					gridDatos.DataBind();
				}
			}
		}
		
		protected void gridCheques_Edit(object Sender,DataGridCommandEventArgs e)
		{
			gridCheques.EditItemIndex=e.Item.DataSetIndex;
			gridCheques.DataSource=tablaCheques;
			gridCheques.DataBind();
		}
		
		protected void gridCheques_Cancel(object Sender,DataGridCommandEventArgs e)
		{
			gridCheques.EditItemIndex=-1;
			gridCheques.DataSource=tablaCheques;
			gridCheques.DataBind();
		}
		
		protected void gridCheques_Update(object Sender,DataGridCommandEventArgs e)
		{
			Control padre=(this.Parent).Parent;
			string cmsn,iva,rtn;
			cmsn=((TextBox)e.Item.Cells[4].Controls[0]).Text;
			iva=((TextBox)e.Item.Cells[5].Controls[0]).Text;
			rtn=((TextBox)e.Item.Cells[6].Controls[0]).Text;
			if(cmsn[0]=='$')
			{
				if(DatasToControls.ValidarDouble(cmsn.Substring(1)))
					tablaCheques.Rows[e.Item.DataSetIndex][5]=Convert.ToDouble(cmsn.Substring(1));
			}
			else
			{
				if(DatasToControls.ValidarDouble(cmsn))
					tablaCheques.Rows[e.Item.DataSetIndex][5]=Convert.ToDouble(cmsn);
			}
			if(iva[0]=='$')
			{
				if(DatasToControls.ValidarDouble(iva.Substring(1)))
					tablaCheques.Rows[e.Item.DataSetIndex][6]=Convert.ToDouble(iva.Substring(1));
			}
			else
			{
				if(DatasToControls.ValidarDouble(iva))
					tablaCheques.Rows[e.Item.DataSetIndex][6]=Convert.ToDouble(iva);
			}
			if(rtn[0]=='$')
			{
				if(DatasToControls.ValidarDouble(rtn.Substring(1)))
					tablaCheques.Rows[e.Item.DataSetIndex][7]=Convert.ToDouble(rtn.Substring(1));
			}
			else
			{
				if(DatasToControls.ValidarDouble(rtn))
					tablaCheques.Rows[e.Item.DataSetIndex][7]=Convert.ToDouble(rtn);
			}
			aceptarCheques.Text="Actualizar";
			aceptarCheques.Enabled=true;
			((Button)padre.FindControl("guardar")).Enabled=false;
			gridCheques.EditItemIndex=-1;
			Session["tablaCheques"]=tablaCheques;
			gridCheques.DataSource=tablaCheques;
			gridCheques.DataBind();
		}
		
		protected void gridCheques_Item(object Sender,DataGridCommandEventArgs e)
		{
			Control padre=(this.Parent).Parent;
			if(((Button)e.CommandSource).CommandName=="Delete")
			{
				if(((Panel)padre.FindControl("panelValores")).Visible==true)
				{
					((TextBox)padre.FindControl("valorConsignado")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)))-(Convert.ToDouble(tablaCheques.Rows[e.Item.DataSetIndex][4].ToString()))).ToString("C");
					((TextBox)padre.FindControl("totalEfectivo")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("totalEfectivo")).Text.Substring(1)))-(Convert.ToDouble(tablaCheques.Rows[e.Item.DataSetIndex][5].ToString()))-(Convert.ToDouble(tablaCheques.Rows[e.Item.DataSetIndex][6].ToString()))-(Convert.ToDouble(tablaCheques.Rows[e.Item.DataSetIndex][7].ToString()))).ToString("C");
					if(((TextBox)padre.FindControl("valorConsignado")).Text=="$0.00")
						((Button)padre.FindControl("guardar")).Enabled=false;
				}
				tablaCheques.Rows[e.Item.DataSetIndex].Delete();
				Session["tablaCheques"]=tablaCheques;
				gridCheques.DataSource=tablaCheques;
				gridCheques.DataBind();
			}
		}
		
		protected void aceptarCheques_Click(object Sender,EventArgs e)
		{
			int i;
			Control padre=(this.Parent).Parent;
			((Label)padre.FindControl("lbDetalle")).Visible=true;
			((TextBox)padre.FindControl("detalleTransaccion")).Visible=true;
			((Label)padre.FindControl("lbValor")).Visible=true;
			((TextBox)padre.FindControl("valorConsignado")).Visible=true;
			((Label)padre.FindControl("lbTotalEf")).Visible=true;
			((TextBox)padre.FindControl("totalEfectivo")).Visible=true;
			((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
			((TextBox)padre.FindControl("totalEfectivo")).ReadOnly=true;
			((Label)padre.FindControl("lbDetalle")).Text="Detalle Remisión :";
			((Label)padre.FindControl("lbValor")).Text="Valor de la Remisión :";
			((Label)padre.FindControl("lbTotalEf")).Text="Valor Comisión, IVA y Retención :";
			((TextBox)padre.FindControl("valorConsignado")).Text="$0.00";
			((TextBox)padre.FindControl("totalEfectivo")).Text="$0.00";
			((Panel)padre.FindControl("panelValores")).Visible=true;
			if(aceptarCheques.Text=="Aceptar")
			{
				for(i=0;i<tablaCheques.Rows.Count;i++)
				{
					((TextBox)padre.FindControl("valorConsignado")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)))+(Convert.ToDouble(tablaCheques.Rows[i][4].ToString()))).ToString("C");
					((TextBox)padre.FindControl("totalEfectivo")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("totalEfectivo")).Text.Substring(1)))+(Convert.ToDouble(tablaCheques.Rows[i][5].ToString()))+(Convert.ToDouble(tablaCheques.Rows[i][6].ToString()))+(Convert.ToDouble(tablaCheques.Rows[i][7].ToString()))).ToString("C");
				}
				aceptarCheques.Enabled=false;
				((Button)padre.FindControl("guardar")).Enabled=true;
			}
			else if(aceptarCheques.Text=="Actualizar")
			{
				((TextBox)padre.FindControl("valorConsignado")).Text="$0.00";
				((TextBox)padre.FindControl("totalEfectivo")).Text="$0.00";
				for(i=0;i<tablaCheques.Rows.Count;i++)
				{
					((TextBox)padre.FindControl("valorConsignado")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)))+(Convert.ToDouble(tablaCheques.Rows[i][4].ToString()))).ToString("C");
					((TextBox)padre.FindControl("totalEfectivo")).Text=((Convert.ToDouble(((TextBox)padre.FindControl("totalEfectivo")).Text.Substring(1)))+(Convert.ToDouble(tablaCheques.Rows[i][5].ToString()))+(Convert.ToDouble(tablaCheques.Rows[i][6].ToString()))+(Convert.ToDouble(tablaCheques.Rows[i][7].ToString()))).ToString("C");
				}
				aceptarCheques.Text="Aceptar";
				aceptarCheques.Enabled=false;
				((Button)padre.FindControl("guardar")).Enabled=true;
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
