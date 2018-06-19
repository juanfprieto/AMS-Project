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
	public partial class ConsignacionChequesCC : System.Web.UI.UserControl
	{
		protected TextBox banco;
		protected DataTable tablaDatos;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				fecha.Text=System.DateTime.Now.Date.ToString("yyyy-MM-dd");
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
			tablaDatos.Columns.Add("VALOR",typeof(string));
			tablaDatos.Columns.Add("NIT",typeof(string));
			tablaDatos.Columns.Add("NITBENEFICIARIO",typeof(string));
			tablaDatos.Columns.Add("FECHA",typeof(string));
		}
		
		protected void Cambiar_Fecha(object Sender,EventArgs e)
		{
			fecha.Text=calendarioFecha.SelectedDate.Date.ToString("yyyy-MM-dd");
		}
		
		protected void Aceptar_Datos(object Sender,EventArgs e)
		{
            //string empresa = GlobalData.EMPRESA;
            //string mesVige = DBFunctions.SingleData("SELECT CTES_MESVIGE FROM CTESORERIA;");

            //string anoVige = DBFunctions.SingleData("SELECT CTES_ANOVIGE FROM CTESORERIA");
            //if(empresa.Equals("autoorion"))
            //{
                //if(mesVige != fecha.Text.Split('-')[1] || anoVige != fecha.Text.Split('-')[0])
                //{
                //    Utils.MostrarAlerta(Response, "La fecha del documento no corresponde a la vigencia del sistema de tesorería. Por favor revise.");
                //    return;
                //}
            //}
            if(!Tools.General.validarCierreFinanzas(fecha.Text, "T"))
            {
                Utils.MostrarAlerta(Response, "La fecha del documento no corresponde a la vigencia del sistema de tesorería. Por favor revise.");
                return;
            }

			DataSet ds=new DataSet();
			DataRow fila;
			Control padre=(this.Parent).Parent;
			if(numeroDocumento.Text!="")
			{
                if(numeroDocumento.Text.Split('-').Length > 2)
                {
                    Utils.MostrarAlerta(Response, "El documento: " + numeroDocumento.Text + " está mal escrito, es necesario retirar uno de los guiones(-)");
                    return;
                }
				if(Session["tablaDatos"]==null)
					this.Cargar_Tabla_Datos();
				if(hdnChk.Value=="C")
				{
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pdoc_codigo,M.mcaj_numero,T.ttip_nombre,TRIM(P.mcpag_numerodoc),B.pban_nombre,P.mcpag_valor,M.mnit_nit,M.mnit_nitben,M.mcaj_fecha FROM mcaja M,mcajapago P,ttipopago T,pbanco B WHERE (M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND P.ttip_codigo=T.ttip_codigo AND P.pban_codigo=B.pban_codigo) AND P.mcpag_numerodoc='"+(numeroDocumento.Text.Split('-'))[1].Trim()+"' AND P.ttip_codigo='C' AND P.test_estado='C' AND P.pban_codigo='"+(numeroDocumento.Text.Split('-'))[0]+"'");
					if(!ExisteDocumentoGrilla(ds.Tables[0].Rows[0][4].ToString(),ds.Tables[0].Rows[0][3].ToString()))
					{
						fila=tablaDatos.NewRow();
						fila["CODIGORECIBOCAJA"]=ds.Tables[0].Rows[0][0].ToString();
						fila["NUMERORECIBOCAJA"]=ds.Tables[0].Rows[0][1].ToString();
						fila["TIPOPAGO"]=ds.Tables[0].Rows[0][2].ToString();
						fila["NUMERO"]=ds.Tables[0].Rows[0][3].ToString();
						fila["NOMBREBANCO"]=ds.Tables[0].Rows[0][4].ToString();
						fila["VALOR"]=System.Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString()).ToString("C");
						fila["NIT"]=ds.Tables[0].Rows[0][6].ToString();
						fila["NITBENEFICIARIO"]=ds.Tables[0].Rows[0][7].ToString();
						fila["FECHA"]=System.Convert.ToDateTime(ds.Tables[0].Rows[0][8].ToString()).ToString("yyyy-MM-dd");
						tablaDatos.Rows.Add(fila);
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
						Session["tablaDatos"]=tablaDatos;
					}
					else
                        Utils.MostrarAlerta(Response, "La combinación banco-número de documento ya se encuentra listada en la grilla");
				}
				else if(hdnChk.Value=="D")
				{
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pdoc_codigo,M.mcaj_numero,T.ttip_nombre,P.mcpag_numerodoc,B.pban_nombre,P.mcpag_valor,M.mnit_nit,M.mnit_nitben,M.mcaj_fecha FROM dbxschema.mcaja M,dbxschema.mcajapago P,dbxschema.ttipopago T,dbxschema.pbanco B WHERE (M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND P.ttip_codigo=T.ttip_codigo AND P.pban_codigo=B.pban_codigo) AND P.mcpag_numerodoc='"+(numeroDocumento.Text.Split('-'))[1]+"' AND P.ttip_codigo='D' AND P.test_estado='C' AND P.pban_codigo='"+(numeroDocumento.Text.Split('-'))[0]+"'");
					if(!ExisteDocumentoGrilla(ds.Tables[0].Rows[0][4].ToString(),ds.Tables[0].Rows[0][3].ToString()))
					{
						fila=tablaDatos.NewRow();
						fila["CODIGORECIBOCAJA"]=ds.Tables[0].Rows[0][0].ToString();
						fila["NUMERORECIBOCAJA"]=ds.Tables[0].Rows[0][1].ToString();
						fila["TIPOPAGO"]=ds.Tables[0].Rows[0][2].ToString();
						fila["NUMERO"]=ds.Tables[0].Rows[0][3].ToString();
						fila["NOMBREBANCO"]=ds.Tables[0].Rows[0][4].ToString();
						fila["VALOR"]=System.Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString()).ToString("C");
						fila["NIT"]=ds.Tables[0].Rows[0][6].ToString();
						fila["NITBENEFICIARIO"]=ds.Tables[0].Rows[0][7].ToString();
						fila["FECHA"]=System.Convert.ToDateTime(ds.Tables[0].Rows[0][8].ToString()).ToString("yyyy-MM-dd");
						tablaDatos.Rows.Add(fila);
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
						Session["tablaDatos"]=tablaDatos;
					}
					else
                        Utils.MostrarAlerta(Response, "La combinación banco-número de documento ya se encuentra listada en la grilla");
				}
				else if(hdnChk.Value=="T")
				{
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT M.pdoc_codigo,M.mcaj_numero,T.ttip_nombre,P.mcpag_numerodoc,B.pban_nombre,P.mcpag_valor,M.mnit_nit,M.mnit_nitben,M.mcaj_fecha FROM dbxschema.mcaja M,dbxschema.mcajapago P,dbxschema.ttipopago T,dbxschema.pbanco B WHERE (M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND P.ttip_codigo=T.ttip_codigo AND P.pban_codigo=B.pban_codigo) AND P.mcpag_numerodoc='" + (numeroDocumento.Text.Split('-'))[1].Trim() + "' AND P.ttip_codigo='T' AND P.test_estado='C' AND P.pban_codigo='" + (numeroDocumento.Text.Split('-'))[0].Trim() + "'");
                    if(ds.Tables[0].Rows.Count == 0)
                    {
                        Utils.MostrarAlerta(Response, "No se encontró información por algún error en el documento. Contacte al administrador");
                        return;
                    }
                    if (!ExisteDocumentoGrilla(ds.Tables[0].Rows[0][4].ToString(),ds.Tables[0].Rows[0][3].ToString()))
					{
						fila=tablaDatos.NewRow();
						fila["CODIGORECIBOCAJA"]=ds.Tables[0].Rows[0][0].ToString();
						fila["NUMERORECIBOCAJA"]=ds.Tables[0].Rows[0][1].ToString();
						fila["TIPOPAGO"]=ds.Tables[0].Rows[0][2].ToString();
						fila["NUMERO"]=ds.Tables[0].Rows[0][3].ToString();
						fila["NOMBREBANCO"]=ds.Tables[0].Rows[0][4].ToString();
						fila["VALOR"]=System.Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString()).ToString("C");
						fila["NIT"]=ds.Tables[0].Rows[0][6].ToString();
						fila["NITBENEFICIARIO"]=ds.Tables[0].Rows[0][7].ToString();
						fila["FECHA"]=System.Convert.ToDateTime(ds.Tables[0].Rows[0][8].ToString()).ToString("yyyy-MM-dd");
						tablaDatos.Rows.Add(fila);
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
						Session["tablaDatos"]=tablaDatos;
					}
					else
                        Utils.MostrarAlerta(Response, "La combinación banco-número de documento ya se encuentra listada en la grilla");
				}
				numeroDocumento.Text="";
				((Panel)padre.FindControl("panelValores")).Visible=true;
				((Label)padre.FindControl("lbDetalle")).Text="Detalle de la Consignación :";
				((Label)padre.FindControl("lbValor")).Text="Valor Consignado :";
				((Label)padre.FindControl("lbTotalEf")).Text="Total Efectivo :";
				((TextBox)padre.FindControl("valorConsignado")).Visible=true;
				((TextBox)padre.FindControl("valorConsignado")).Text=((System.Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)))+(System.Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString()))).ToString("C");
				((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
				((TextBox)padre.FindControl("totalEfectivo")).Visible=true;
				((Button)padre.FindControl("guardar")).Enabled=true;
			}
			else
			{
				((Panel)padre.FindControl("panelValores")).Visible=true;
				((Label)padre.FindControl("lbDetalle")).Text="Detalle de la Consignación :";
				((Label)padre.FindControl("lbTotalEf")).Text="Total Efectivo :";
				((TextBox)padre.FindControl("valorConsignado")).Visible=false;
				((Button)padre.FindControl("guardar")).Enabled=true;
			}
		}
		
		protected void Manejar_Documentos(object Sender,DataGridCommandEventArgs e)
		{
			Control padre=(this.Parent).Parent;
			if(((Button)e.CommandSource).CommandName=="Remover_Documento")
			{
				((TextBox)padre.FindControl("valorConsignado")).Text=((System.Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)))-(System.Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString().Substring(1)))).ToString("C");
				tablaDatos.Rows[e.Item.DataSetIndex].Delete();
				if(tablaDatos.Rows.Count==0)
				{
					((TextBox)padre.FindControl("valorConsignado")).Text=Convert.ToDouble("0").ToString("C");
					((Button)padre.FindControl("guardar")).Enabled=false;
				}
				Session["tablaDatos"]=tablaDatos;
				gridDatos.DataSource=tablaDatos;
				gridDatos.DataBind();
			}
		}

		private bool ExisteDocumentoGrilla(string banco,string documento)
		{
			bool existe=false;
			DataRow[] doc;
			doc=tablaDatos.Select("NOMBREBANCO='"+banco+"' AND NUMERO='"+documento+"'");
			if(doc.Length!=0)
				existe=true;
			return existe;
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
