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
	public partial class DevolucionChequesCC : System.Web.UI.UserControl
	{
        protected DataTable tablaDatos;
		
		protected void Page_Load(object Sender,EventArgs e)
		{

			if(!IsPostBack)
			{
                prefijoConsignacion.Visible = false;
                numeroConsignacion.Visible = false;

                if (Request.QueryString["faltaConf"] != null)
                    Utils.MostrarAlerta(Response, "NO ha definido el documento para registrar en la cartera las NOTAS por cheques devueltos en la configuracion de Tesorería");
                
				DatasToControls bind=new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlCuentaCorriente, "select pcue_codigo, pcue_codigo CONCAT ' - ' CONCAT pb.pban_nombre CONCAT ' - ' CONCAT pcue_numero CONCAT ' - ' CONCAT pcue_nombre from pcuentacorriente pc, pbanco pb  where pc.pban_banco = pb.pban_codigo order by 2; ");

                //bind.PutDatasIntoDropDownList(prefijoConsignacion, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CS' and tvig_vigencia = 'V' ORDER BY 2 ");
                Utils.llenarPrefijos(Response, ref prefijoConsignacion , "%", "%", "CS");
                if (prefijoConsignacion.Items.Count > 1)
                {
                    prefijoConsignacion.Items.Insert(0, "Seleccione:");
                }
                else
                {
                    string Sql = "SELECT M.mtes_numero FROM dbxschema.ttipopago T,dbxschema.dtesoreriadocumentos D,dbxschema.pbanco P,dbxschema.mtesoreria M,dbxschema.mcajapago C WHERE (D.ttip_codigo=T.ttip_codigo AND D.pban_banco=P.pban_codigo AND M.pdoc_codigo=D.mtes_codigo AND M.pcue_codigo=D.pcue_codigo AND M.mtes_numero=D.mtes_numero AND C.pdoc_codigo=D.mcaj_codigo AND C.mcaj_numero=D.mcaj_numero) AND D.mtes_codigo='" + this.prefijoConsignacion.SelectedValue + "' AND C.test_estado='G' AND C.TTIP_CODIGO IN ('C','T','D') ORDER BY 1";
				bind.PutDatasIntoDropDownList(numeroConsignacion,Sql);
                }
				fecha.Text=System.DateTime.Now.Date.ToString("yyyy-MM-dd");
			}
			else
			{
				if(Session["tablaDatosDev"]!=null)
					tablaDatos=(DataTable)Session["tablaDatosDev"];
			}
		}
		
		protected void Cargar_Tabla_Datos()
		{
            tablaDatos = new DataTable();
            tablaDatos.Columns.Add("PREFIJORECIBOCAJA", typeof(string));
            tablaDatos.Columns.Add("NUMERORECIBOCAJA", typeof(string));
            tablaDatos.Columns.Add("TIPOPAGO", typeof(string));
            tablaDatos.Columns.Add("NUMERO", typeof(string));
            tablaDatos.Columns.Add("NOMBREBANCO", typeof(string));
            tablaDatos.Columns.Add("VALOR", typeof(string));
            tablaDatos.Columns.Add("NITBENEFICIARIO", typeof(string));
            tablaDatos.Columns.Add("FECHA", typeof(string));
            tablaDatos.Columns.Add("BANDERA", typeof(bool));
            tablaDatos.Columns.Add("PREFIJONOTA", typeof(string));
            tablaDatos.Columns.Add("CONSECUTIVO", typeof(string));
            tablaDatos.Columns.Add("PREFIJOCONSIG", typeof(string));
            tablaDatos.Columns.Add("CONSECUTIVOCONSIG", typeof(string));
		}
		
		protected void Cambiar_Fecha(object Sender,EventArgs e)
		{
			fecha.Text=calendarioFecha.SelectedDate.Date.ToString("yyyy-MM-dd");
		}

        protected void Cambiar_PrefijoConsignacion (object Sender, EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
            string Sql = "SELECT DISTINCT M.mtes_numero FROM dbxschema.ttipopago T,dbxschema.dtesoreriadocumentos D,dbxschema.pbanco P,dbxschema.mtesoreria M,dbxschema.mcajapago C WHERE (D.ttip_codigo=T.ttip_codigo AND D.pban_banco=P.pban_codigo AND M.pdoc_codigo=D.mtes_codigo AND M.pcue_codigo=D.pcue_codigo AND M.mtes_numero=D.mtes_numero AND C.pdoc_codigo=D.mcaj_codigo AND C.mcaj_numero=D.mcaj_numero) AND D.mtes_codigo='" + this.prefijoConsignacion.SelectedValue + "' AND C.test_estado='G' AND C.TTIP_CODIGO IN ('C','T','D') ORDER BY 1";
            bind.PutDatasIntoDropDownList(numeroConsignacion, Sql);
			//bind.PutDatasIntoDropDownList(numeroConsignacion,"SELECT mtes_numero FROM mtesoreria M,pdocumento P WHERE M.pdoc_codigo=P.pdoc_codigo AND P.pdoc_codigo='"+this.prefijoConsignacion.SelectedValue+"'");
		}
        protected void Aceptar_Datos(object Sender,EventArgs e)
		{
			DataSet ds=new DataSet();
			DataRow fila;
			DatasToControls bind=new DatasToControls();
			int i;
			Control padre=(this.Parent).Parent;
			if(Session["tablaDatosDev"]==null)
				this.Cargar_Tabla_Datos();
            // DBFunctions.Request(ds, IncludeSchema.NO, "SELECT D.mcaj_codigo,D.mcaj_numero,T.ttip_nombre,D.dtes_numerodoc,P.pban_nombre,D.dtes_valor,D.mnit_nit,M.mtes_fecha,D.dtes_consecutivo FROM dbxschema.ttipopago T,dbxschema.dtesoreriadocumentos D,dbxschema.pbanco P,dbxschema.mtesoreria M,dbxschema.mcajapago C WHERE (D.ttip_codigo=T.ttip_codigo AND D.pban_banco=P.pban_codigo AND M.pdoc_codigo=D.mtes_codigo AND M.pcue_codigo=D.pcue_codigo AND M.mtes_numero=D.mtes_numero AND C.pdoc_codigo=D.mcaj_codigo AND C.mcaj_numero=D.mcaj_numero)  AND C.test_estado='G'AND Days('" + fecha .Text+ "')- Days(M.MTES_FECHA)<60 ORDER BY 1,2");
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT D.mcaj_codigo,D.mcaj_numero,T.ttip_nombre,D.dtes_numerodoc,P.pban_nombre,D.dtes_valor,D.mnit_nit,M.mtes_fecha,D.dtes_consecutivo, D.mtes_codigo as Codigo, D.mtes_numero as numero FROM dbxschema.ttipopago T,dbxschema.dtesoreriadocumentos D,dbxschema.pbanco P,dbxschema.mtesoreria M,dbxschema.mcajapago C WHERE (D.ttip_codigo=T.ttip_codigo AND D.pban_banco=P.pban_codigo AND M.pdoc_codigo=D.mtes_codigo AND M.pcue_codigo=D.pcue_codigo AND M.mtes_numero=D.mtes_numero AND C.pdoc_codigo=D.mcaj_codigo AND C.mcaj_numero=D.mcaj_numero and c.mcpag_numerodoc = d.dtes_numerodoc)  AND M.TEST_ESTADODOC = 'A' AND C.test_estado='G' AND Days('" + fecha.Text + "')- Days(M.MTES_FECHA) < 60 and D.pcue_codigo='" + ddlCuentaCorriente.SelectedValue + "' ORDER BY 1,2");

            if (ds.Tables[0].Rows.Count != 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    fila = tablaDatos.NewRow();
                    fila["PREFIJORECIBOCAJA"] = ds.Tables[0].Rows[i][0].ToString();
                    fila["NUMERORECIBOCAJA"] = ds.Tables[0].Rows[i][1].ToString();
                    fila["TIPOPAGO"] = ds.Tables[0].Rows[i][2].ToString();
                    fila["NUMERO"] = ds.Tables[0].Rows[i][3].ToString();
                    fila["NOMBREBANCO"] = ds.Tables[0].Rows[i][4].ToString();
                    fila["VALOR"] = System.Convert.ToDouble(ds.Tables[0].Rows[i][5].ToString()).ToString("C");
                    fila["NITBENEFICIARIO"] = ds.Tables[0].Rows[i][6].ToString();
                    fila["FECHA"] = System.Convert.ToDateTime(ds.Tables[0].Rows[i][7].ToString()).ToString("yyyy-MM-dd");
                    fila["BANDERA"] = false;
                    fila["CONSECUTIVO"] = ds.Tables[0].Rows[i][8].ToString();
                    fila["PREFIJOCONSIG"] = ds.Tables[0].Rows[i][9].ToString();
                    fila["CONSECUTIVOCONSIG"] = ds.Tables[0].Rows[i][10].ToString();

                    tablaDatos.Rows.Add(fila);
                    gridDatos.DataSource = tablaDatos;
                    gridDatos.DataBind();
                    Session["tablaDatosDev"] = tablaDatos;
                    aceptarDatos.Enabled = false;
                    ((Label)padre.FindControl("lbTotalEf")).Visible = false;
                    ((TextBox)padre.FindControl("totalEfectivo")).Visible = false;
                    ((Label)padre.FindControl("lbDetalle")).Text = "Detalle de la Devolución :";
                    ((TextBox)padre.FindControl("detalleTransaccion")).Visible = true;
                    ((Label)padre.FindControl("lbValor")).Text = "Valor Devuelto :";
                    ((TextBox)padre.FindControl("valorConsignado")).Visible = true;
                    ((TextBox)padre.FindControl("valorConsignado")).ReadOnly = true;
                    ((Panel)padre.FindControl("panelValores")).Visible = true;
                } 

                DropDownList DDL = new DropDownList();
                bind.PutDatasIntoDropDownList(DDL, "SELECT pdoc_codigo, PDOC_CODIGO CONCAT '-' CONCAT pdoc_nombre FROM pdocumento, ctesoreria WHERE tdoc_tipodocu='FC' and pdoc_codigo = PDOC_CODINOTACHEQDEVU");

                for (i = 0; i < gridDatos.Items.Count; i++)
                {
                   ((DropDownList)gridDatos.Items[i].Cells[8].Controls[1]).DataSource = DDL.Items; 
                    ((DropDownList)gridDatos.Items[i].Cells[8].Controls[1]).DataBind();

                    if (((DropDownList)gridDatos.Items[i].Cells[8].Controls[1]).Items.Count == 0)
                        Response.Redirect("AMS.Web.index.aspx?process=Tesoreria.ConsignaCheques&faltaConf=dummy");

                }
            }
			else
				Response.Write("<script language:javascript>alert('No hay documentos en esa consignación');</script>");
		}
		
		protected void Manejar_Documentos(object Sender,DataGridCommandEventArgs e)
		{
			Control padre=(this.Parent).Parent;
                if (((Button)e.CommandSource).CommandName == "Devolver_Documento")
                {
                    tablaDatos.Rows[e.Item.DataSetIndex][8] = true;
                    tablaDatos.Rows[e.Item.DataSetIndex][9] = ((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue;
                    ((TextBox)padre.FindControl("valorConsignado")).Text = (Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)) + Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString().Substring(1))).ToString("C");
                    ((Button)e.Item.Cells[9].Controls[1]).CommandName = "Cancelar_Documento";
                    ((Button)e.Item.Cells[9].Controls[1]).Text = "Cancelar";
                    ((Button)padre.FindControl("guardar")).Enabled = true;
                    Session["prefConsig"] = tablaDatos.Rows[e.Item.DataSetIndex][11];
                    Session["numConsig"] = tablaDatos.Rows[e.Item.DataSetIndex][12];
                }
                else if (((Button)e.CommandSource).CommandName == "Cancelar_Documento")
                {
                    ((Label)padre.FindControl("lb")).Text = "Bien";
                    tablaDatos.Rows[e.Item.DataSetIndex][8] = false;
                    ((TextBox)padre.FindControl("valorConsignado")).Text = (Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)) - Convert.ToDouble(tablaDatos.Rows[e.Item.DataSetIndex][5].ToString().Substring(1))).ToString("C");
                    ((Button)e.Item.Cells[9].Controls[1]).CommandName = "Devolver_Documento";
                    ((Button)e.Item.Cells[9].Controls[1]).Text = "Devolver";
                    if (((TextBox)padre.FindControl("valorConsignado")).Text == "$0.00")
                        ((Button)padre.FindControl("guardar")).Enabled = false;
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
