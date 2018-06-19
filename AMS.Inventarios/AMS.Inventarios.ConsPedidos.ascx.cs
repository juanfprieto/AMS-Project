using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class ConsultaPedido : System.Web.UI.UserControl
	{
		protected string actor;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			actor = Request.QueryString["actor"];
			if(!IsPostBack)
			{
				//Revisamos que tipo de consulta se va a realizar si es de tipo cliente o proveedor
				if(Request.QueryString["actor"]=="C")
				{
					tbNit.Attributes.Add("ondblclick","ModalDialog(this, 'SELECT DISTINCT MNI.mnit_nit AS NIT, MNI.mnit_apellidos CONCAT \\' \\' CONCAT MNI.mnit_nombres AS NOMBRE FROM mnit MNI, mpedidoitem MPI WHERE MNI.mnit_nit = MPI.mnit_nit  AND mped_claseregi=\\'C\\' ORDER BY MNI.mnit_nit', new Array());");
					lbTipCli.Text = "Cliente";
				}
				else
				{
					tbNit.Attributes.Add("ondblclick","ModalDialog(this, 'SELECT DISTINCT MNI.mnit_nit AS NIT, MNI.mnit_apellidos CONCAT \\' \\' CONCAT MNI.mnit_nombres AS NOMBRE FROM mnit MNI, mpedidoitem MPI WHERE MNI.mnit_nit = MPI.mnit_nit  AND mped_claseregi=\\'P\\' ORDER BY MNI.mnit_nit', new Array());");
					lbTipCli.Text = "Proveedor";
				}
			}
			if(actor=="P")
				dgPedido.Columns[3].HeaderText = "CANTIDAD PRERECEPCIONADA";
		}
		
		protected void CargarPedidos(Object Sender, EventArgs E)
		{
			//Revisamos primero si el nit existe dentro de nuestra base de datos
			if(DBFunctions.RecordExist("SELECT * FROM mnit WHERE mnit_nit='"+this.tbNit.Text.Trim()+"'"))
			{
				//Ahora revisamos si tiene pedidos realizados
				if(DBFunctions.RecordExist("SELECT * FROM mpedidoitem WHERE mnit_nit='"+this.tbNit.Text.Trim()+"' AND mped_claseregi='"+actor+"'"))
				{
					this.lbPedidos.Visible = this.ddlPedidos.Visible = this.btnListar.Visible = this.chkItemPendi.Visible = true;
					DatasToControls bind = new DatasToControls();
					bind.PutDatasIntoDropDownList(this.ddlPedidos,"SELECT mnit_nit CONCAT '-' CONCAT pped_codigo CONCAT '-' CONCAT CAST(mped_numepedi AS character(20)), pped_codigo CONCAT '-' CONCAT CAST(mped_numepedi AS character(20)) FROM mpedidoitem WHERE mnit_nit='"+this.tbNit.Text.Trim()+"' AND mped_claseregi='"+actor+"'");
				}
				else
                    Utils.MostrarAlerta(Response, "Este nit no tiene pedidos registrados!");
			}
			else
                Utils.MostrarAlerta(Response, "Este nit esta registrado!");
		}
		
		protected void MostrarPedido(Object Sender, EventArgs E)
        {
            Int16  ano   = Convert.ToInt16(DBFunctions.SingleData("SELECT pano_ano from cinventario"));
            string query = "SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) AS REFERENCIA, MIT.mite_nombre AS DESCRIPCION, DPI.dped_cantpedi AS \"CANTIDAD PEDIDA\", DPI.dped_cantasig AS \"CANTIDAD ASIGNADA\", DPI.dped_cantfact AS \"CANTIDAD FACTURADA\", DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact AS \"CANTIDAD PENDIENTE\", DPI.dped_valounit AS \"VALOR UNITARIO\", DPI.dped_PORCDESC AS DESCTO, DPI.PIVA_PORCIVA AS IVA, MS.MSAL_CANTACTUAL - MS.MSAL_CANTASIG AS \"SALDO BODEGAS\" FROM dpedidoitem DPI, plineaitem PLIN, mitems MIT LEFT JOIN MSALDOITEM MS ON MS.MITE_CODIGO = MIT.MITE_CODIGO AND MS.PANO_ANO = "+ ano +" ";
            DataSet ds   = new DataSet();
            if (ddlPedidos.SelectedValue.Split('-').Length == 4)
            {
                string[] infoPed = this.ddlPedidos.SelectedValue.Split('-');

                query += " WHERE DPI.pped_codigo='" + infoPed[1] + "-" + infoPed[2] + "' AND DPI.mped_numepedi=" + infoPed[3] + " AND DPI.mite_codigo=MIT.mite_codigo AND DPI.mped_clasregi='" + actor + "' AND PLIN.plin_codigo=MIT.plin_codigo";
                
                //Primero cargamos la informacion general del pedido
                this.plInfoPedido.Controls.Add(new LiteralControl("Nit : " + infoPed[0] + ""));//Nit
                this.plInfoPedido.Controls.Add(new LiteralControl("<br>Apellidos y Nombres : " + DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='" + infoPed[0] + "'") + ""));
                this.plInfoPedido.Controls.Add(new LiteralControl("<br>Fecha del Pedido : " + Convert.ToDateTime(DBFunctions.SingleData("SELECT mped_pedido FROM mpedidoitem WHERE pped_codigo='" + infoPed[1] +"-"+ infoPed[2] + "' AND mped_numepedi=" + infoPed[3] + " AND mnit_nit='" + infoPed[0] + "'")).ToShortDateString() + ""));
                this.plInfoPedido.Controls.Add(new LiteralControl("<br>Almacen : " + DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM mpedidoitem WHERE pped_codigo='" + infoPed[1] + "-" + infoPed[2] + "' AND mped_numepedi=" + infoPed[3] + " AND mnit_nit='" + infoPed[0] + "')") + ""));

            }
            else
            {
                string[] infoPed = this.ddlPedidos.SelectedValue.Split('-');

                query += " WHERE DPI.pped_codigo='" + infoPed[1] + "' AND DPI.mped_numepedi=" + infoPed[2] + " AND DPI.mite_codigo=MIT.mite_codigo AND DPI.mped_clasregi='" + actor + "' AND PLIN.plin_codigo=MIT.plin_codigo";
                
                //Primero cargamos la informacion general del pedido
                this.plInfoPedido.Controls.Add(new LiteralControl("Nit : " + infoPed[0] + ""));//Nit
                this.plInfoPedido.Controls.Add(new LiteralControl("<br>Apellidos y Nombres : " + DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='" + infoPed[0] + "'") + ""));
                this.plInfoPedido.Controls.Add(new LiteralControl("<br>Fecha del Pedido : " + Convert.ToDateTime(DBFunctions.SingleData("SELECT mped_pedido FROM mpedidoitem WHERE pped_codigo='" + infoPed[1] + "' AND mped_numepedi=" + infoPed[2] + " AND mnit_nit='" + infoPed[0] + "'")).ToShortDateString() + ""));
                this.plInfoPedido.Controls.Add(new LiteralControl("<br>Almacen : " + DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM mpedidoitem WHERE pped_codigo='" + infoPed[1] + "' AND mped_numepedi=" + infoPed[2] + " AND mnit_nit='" + infoPed[0] + "')") + ""));
            }
            
            
            
            
            
            
            if(this.chkItemPendi.Checked)
			{
				this.plInfoPedido.Controls.Add(new LiteralControl("<br>SOLO ITEMS PENDIENTES"));
				DBFunctions.Request(ds,IncludeSchema.NO,query+" AND (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)>0 AND DPI.dped_cantfact = 0");
			}
			else
			{
				this.plInfoPedido.Controls.Add(new LiteralControl("<br>TODOS LOS ITEMS"));
				DBFunctions.Request(ds,IncludeSchema.NO,query);
			}
			this.dgPedido.DataSource = ds.Tables[0];
			this.dgPedido.DataBind();
			DatasToControls.JustificacionGrilla(this.dgPedido,ds.Tables[0]);
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
