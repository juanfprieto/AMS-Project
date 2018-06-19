namespace AMS.Vehiculos
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using System.Collections;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Vehiculos_AnulacionPedidoClientes.
	/// </summary>
	public partial class AMS_Vehiculos_AnulacionPedidoClientes : System.Web.UI.UserControl
	{
		string user= HttpContext.Current.User.Identity.Name;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(prefijoDocumento,"SELECT pdoc_codigo ,pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu ='PC'");
                Utils.llenarPrefijos(Response, ref prefijoDocumento, "%", "%", "PC");
                if (prefijoDocumento.Items.Count > 1)
                    prefijoDocumento.Items.Insert(0, new ListItem("Seleccione el Documento...", ""));

                //solo creados	bind.PutDatasIntoDropDownList(numeroPedido,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue.ToString()+"' AND test_tipoesta not in (40,60) order by mped_numepedi");
				bind.PutDatasIntoDropDownList(numeroPedido,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue.ToString()+"' AND test_tipoesta = 10 order by mped_numepedi");
                //TextObserv.Text = "";
            }
		}

		protected void Cambio_Prefijo(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(numeroPedido, "SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue.ToString() + "' AND test_tipoesta = 10 order by mped_numepedi");
		}

		protected void CancelarPedido(Object sender, EventArgs e)
		{
			DataSet datosPed = new DataSet();
			//Verificar que el estado sea de 10
			if (DBFunctions.SingleData("Select test_tipoesta from DBXSCHEMA.MPEDIDOVEHICULO where PDOC_CODIGO='"+prefijoDocumento.SelectedValue+"' and MPED_NUMEPEDI="+numeroPedido.SelectedValue+"")=="10")
			{
				//Verificar que no tenga Anticipos de caja
				if (DBFunctions.RecordExist("SELECT * FROM manticipovehiculo WHERE mped_codigo='"+prefijoDocumento.SelectedValue+"' AND mped_numepedi="+numeroPedido.SelectedValue+" and test_estado=10"))
				{
                    Utils.MostrarAlerta(Response, "Este pedido tiene abono en caja creado, no puede ser cancelado");
				}
				else
				{
					DBFunctions.Request(datosPed,IncludeSchema.NO,"select PDOC_CODIGO,MPED_NUMEPEDI,PCAT_CODIGO,PVEN_CODIGO,MPED_VALOUNIT from dbxschema.MPEDIDOVEHICULO where pdoc_codigo='"+prefijoDocumento.SelectedValue+"' AND mped_numepedi="+numeroPedido.SelectedValue+"");
                    DBFunctions.NonQuery("DELETE FROM DBXSCHEMA.MCREDITOFINANCIERA where PDOC_CODIPEDI='" + prefijoDocumento.SelectedValue + "' and MPED_NUMEPEDI=" + numeroPedido.SelectedValue + "");
					DBFunctions.NonQuery("update DBXSCHEMA.MPEDIDOVEHICULO set test_tipoesta = 40 where PDOC_CODIGO='"+prefijoDocumento.SelectedValue+"' and MPED_NUMEPEDI="+numeroPedido.SelectedValue+"");
					DBFunctions.NonQuery("insert into dbxschema.MANULACIONPEDIDOVEHICULO values ('"+datosPed.Tables[0].Rows[0][0].ToString()+"',"+datosPed.Tables[0].Rows[0][1].ToString()+",'"+datosPed.Tables[0].Rows[0][2].ToString()+"','"+datosPed.Tables[0].Rows[0][3].ToString()+"',"+datosPed.Tables[0].Rows[0][4].ToString()+",'"+user+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+TextObserv.Text+"')");
                    Utils.MostrarAlerta(Response, "Pedido Cancelado Exitosamente");
					DatasToControls bind = new DatasToControls();
					bind.PutDatasIntoDropDownList(numeroPedido,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue.ToString()+"' AND test_tipoesta = 10 order by mped_numepedi");
                    if (prefijoDocumento.Items.Count > 1)
                        prefijoDocumento.Items.Insert(0, new ListItem("Seleccione el Documento...", ""));
                    TextObserv.Text = "";
                }
			}
			else
			{
                Utils.MostrarAlerta(Response, "El pedido no tiene el estado apropiado para ser cancelado.");
			}							
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
