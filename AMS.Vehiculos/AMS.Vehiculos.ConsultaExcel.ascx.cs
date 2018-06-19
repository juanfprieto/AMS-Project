using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Collections;
using System.Linq;
using System.Web.Security;
using AMS.Tools;

namespace AMS.Vehiculos
{

	/// <summary>
    ///		Descripción breve de AMS_Vehiculos_ConsultaExcel.
	/// </summary>
    public partial class AMS_Vehiculos_ConsultaExcel : System.Web.UI.UserControl
	{
        

		#region Controles
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		public string concesionario;
        string ubicacion = "C:\\Inetpub\\wwwroot\\vascolombia\\imp\\excel\\coneccion.xls";
        
        #endregion

        #region metodos

        //respuesta excel del lado del cliente
        private void enviar_excel()
        {
            Response.ContentType = "application/vnd.xls";
            Response.AppendHeader("Content-Disposition", "attachment; filename=test.xls");
            Response.TransmitFile(Server.MapPath("~/coneccion.xls"));
            Response.End();

        }

        //abrir el aplicativo excel con determinados parametros
        private void consultas_excel()
        {

            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            try
            {
                //Iniciar Excel y obtener el objeto de la aplicación.                
                oXL = new Excel.Application();
                oXL.Visible = true;
                //Obtener un nuevo libro.
                oWB = (Excel._Workbook)(oXL.Workbooks._OpenXML(ubicacion, 1));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                
                //Agregar encabezados de la tabla va celda por celda
                oSheet.Cells[4, 2] = txtConcesionario.Text;
                oSheet.Cells[3, 2] = ddlAno.Text;
                oSheet.Cells[6, 2] = ddlMes.Text;
               
                oXL.Visible = true;
                oXL.UserControl = true;

            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);
                        
            }
            
        }
        #endregion
        private void InitializeComponent()
        {

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
        
        #endregion

        #region Eventos
        protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlAno,"SELECT PANO_ANO FROM DBXSCHEMA.PANO;");
                bind.PutDatasIntoDropDownList(ddlMes, "SELECT PMES_NOMBRE FROM DBXSCHEMA.PMES;");
                bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM DBXSCHEMA.PVENDEDOR where tVENd_CODIGO = 'AG';");
				ddlAno.Items.Insert(0,new ListItem("--seleccione--",""));
				ddlMes.Items.Insert(0,new ListItem("--seleccione--",""));
				ddlVendedor.Items.Insert(0,new ListItem("--seleccione--",""));
				txtFechaProceso.Text=DateTime.Now.ToString("yyyy-MM-dd");
				ViewState["PIVA"]=Convert.ToDouble(DBFunctions.SingleData("SELECT CEMP_PORCIVA FROM CEMPRESA;"));
				concesionario="";
				if(Request.QueryString["cnc"]!=null)
					concesionario=Request.QueryString["cnc"];
				if(concesionario!="C")
                    //txtConcesionario.Attributes.Add("onclick", "ModalDialog(this'select \"PCON_CODIGO\", \"PCON_NOMBRE FROM\"\"DBXSCHEMA\".\"PCONCESIONARIO\" ;',new Array(),1)");
                    //txtConcesionario.Attributes.Add("onclick", "ModalDialog(this,'SELECT MN.MNIT_NIT NIT, MN.MNIT_APELLIDOS concat \\' \\' concat MN.MNIT_nombreS concat \\' \\' concat COALESCE(MN.MNIT_establecimiento, \\'\\') NOMBRE, MN.MNIT_DIRECCION DIRECCION, MN.MNIT_TELEFONO TELEFONO FROM MNIT MN, MCONCESIONARIO MC WHERE MC.MNIT_NIT=MN.MNIT_NIT;',new Array(),1)");
                    txtConcesionario.Attributes.Add("onclick", "ModalDialog(this,'SELECT PCON_CODIGO, PCON_NOMBRE, PCON_DIRECCION, PCON_TELEFONO, PCIU_NOMBRE FROM PCONCESIONARIO PC, PCIUDAD PX WHERE PC.PCIU_CODIGO = PX.PCIU_CODIGO;',new Array(),1)");

                else{
					string nitDistribuidor=DBFunctions.SingleData(
						"SELECT MC.MNIT_NIT FROM MCONCESIONARIOUSUARIO MC,SUSUARIO SU "+
						"WHERE SU.SUSU_LOGIN='"+HttpContext.Current.User.Identity.Name.ToLower()+"' AND SU.SUSU_CODIGO=MC.SUSU_CODIGO;");
					if(nitDistribuidor.Length==0)
					{
                        Utils.MostrarAlerta(Response, "El usuario no tiene un concesionario asociado para el proceso.");
						btnSeleccionar.Enabled=false;
						return;
                        
					}
					txtConcesionario.Text=nitDistribuidor;
                    txtConcesionarioa.Text = DBFunctions.SingleData("SELECT MN.MNIT_APELLIDOS concat \\' \\' concat MN.MNIT_nombreS concat \\' \\' concat COALESCE(MN.MNIT_REPRESENTANTE, \\'\\') NOMBRE FROM MNIT MN WHERE MN.MNIT_NIT='" + nitDistribuidor + "';");
				}
				if(concesionario.Length>0)
					pnlAutorizar.Visible=btnAceptar.Visible=false;
			}

        }
                
        //Seleccionar vehiculo
		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
            //consultas_excel();
            enviar_excel();
            
		}
        
        //Databind OTs
		public void rptFactura_Bound(object sender, RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item||e.Item.ItemType==ListItemType.AlternatingItem){
				UInt32 numOrden=Convert.ToUInt32(((DataRowView)e.Item.DataItem).Row.ItemArray[1]);
				string prefOrden=(((DataRowView)e.Item.DataItem).Row.ItemArray[0]).ToString();
				DataGrid dgrOpers=(DataGrid)e.Item.FindControl("dgrOperaciones");
				DataGrid dgrReps=(DataGrid)e.Item.FindControl("dgrRepuestos");
				DataSet dsElementos=new DataSet();
				double categoria=Convert.ToDouble(ViewState["VALOR_CATEGORIA"]);
				DBFunctions.Request(dsElementos,IncludeSchema.NO,
                    "Select pt.ptem_operacion codigo, pt.ptem_descripcion nombre, mo.mord_creacion fecha, " +
                    "mo.mord_kilometraje kilometraje, " +
                    "CASE WHEN pt.PTEM_INDIGENERIC='S' THEN pt.PTEM_TIEMPOESTANDAR " +
                    "ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG " +
                    "WHERE PTG.PTIE_GRUPCATA=pc.PGRU_GRUPO AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) " +
                    "END TIEMPO, " +
                    "CASE WHEN pt.PTEM_INDIGENERIC='S' THEN pt.PTEM_TIEMPOESTANDAR " +
                    "ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG " +
                    "WHERE PTG.PTIE_GRUPCATA=pc.PGRU_GRUPO AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) " +
                    "END * " + categoria + " PRECIO, " +
                    "dor.test_estado as estado, '' as color " +
                    "from dordenoperacion dor, morden mo, ptempario pt, pcatalogovehiculo pc, mcatalogovehiculo mcv " +
                    "where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and " +
                    "dor.pdoc_codigo='" + prefOrden + "' and dor.mord_numeorde=" + numOrden + " and pc.pcat_codigo=mcv.pcat_codigo and mo.pcat_codigo=mcv.pcat_codigo" +
                    "pt.ptem_operacion=dor.ptem_operacion order by mord_creacion desc;" +
                    "Select mcat_vin,mi.mite_codigo codigo, mi.mite_NOMBRE nombre, mo.mord_creacion fecha, " +
                    "mo.mord_kilometraje kilometraje, dor.mite_precio precio, dor.mite_cantidad cantidad, " +
                    "dor.test_estado as estado, '' as color, dor.mite_valaprob  " +
                    "from dbxschema.dordenitemspostventa dor, dbxschema.morden mo, dbxschema.mitems mi " +
                    "where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and " +
                    "dor.pdoc_codigo='" + prefOrden + "' and dor.mord_numeorde=" + numOrden + " and " +
                    "mi.mite_codigo=dor.mite_codigo order by mord_creacion desc;");
                   
                foreach (DataRow drC in dsElementos.Tables[0].Rows)
                {
                    if (drC["estado"].ToString().Equals("X"))
                        drC["color"] = ColorTranslator.ToHtml(Color.Maroon);
                    else{
                        if (drC["estado"].ToString().Equals("C"))
                            drC["color"] = ColorTranslator.ToHtml(Color.LimeGreen);
                        else
                            drC["color"] = ColorTranslator.ToHtml(Color.Yellow);
                    }
                }
                foreach (DataRow drC in dsElementos.Tables[1].Rows)
                {
                    if (drC["estado"].ToString().Equals("X"))
                        drC["color"] = ColorTranslator.ToHtml(Color.Maroon);
                    else
                    {
                        if (drC["estado"].ToString().Equals("C"))
                            drC["color"] = ColorTranslator.ToHtml(Color.LimeGreen);
                        else
                        {
                            if (drC["estado"].ToString().Equals("P"))
                            {
                                drC["color"] = ColorTranslator.ToHtml(Color.Blue);
                                drC["cantidad"] = 0;
                                drC["precio"] = drC["mite_valaprob"];
                            }
                            else
                                drC["color"] = ColorTranslator.ToHtml(Color.Yellow);
                        }
                    }
                }
				dgrOpers.DataSource=dsElementos.Tables[0];
				dgrOpers.DataBind();
				dgrOpers.Visible=dsElementos.Tables[0].Rows.Count>0;
				dgrReps.DataSource=dsElementos.Tables[1];
				dgrReps.DataBind();
				dgrReps.Visible=dsElementos.Tables[1].Rows.Count>0;
			}
		}

		//Realizar proceso
		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			DateTime fechaProceso;
			string prefAprobacion;
			UInt32 numAprobacion;
			try{
				fechaProceso=Convert.ToDateTime(txtFechaProceso.Text);}
			catch{
                Utils.MostrarAlerta(Response, "Debe ingresar la fecha de proceso en el formato correcto.");
				return;}
			try{
				prefAprobacion=txtPrefAprobacion.Text.Trim();
				numAprobacion=Convert.ToUInt32(txtNumAprobacion.Text);}
			catch{
                Utils.MostrarAlerta(Response, "Debe ingresar el número de aprobación correctamente.");
				return;}
			
			//Verificar no hay cambios
			DataSet dsOrdenes=null;
			//TraerOrdenes(ref dsOrdenes);
			if(dsOrdenes.Tables[0].Rows.Count!=Convert.ToInt16(ViewState["NUMERO_ORDENES"]))
			{
                Utils.MostrarAlerta(Response, "Han cambiado las ordenes de trabajo desde la última consulta, debe volver a revisar el listado.");
				plcSeleccion.Visible=true;
				plcFactura.Visible=false;
				return;
			}
			
			LiquidacionPostVenta liquida=new LiquidacionPostVenta(prefAprobacion,numAprobacion,txtConcesionario.Text,ddlVendedor.SelectedValue,Convert.ToInt16(ddlAno.SelectedValue),Convert.ToInt16(ddlMes.SelectedValue),fechaProceso,Convert.ToDouble(ViewState["SUBTOTAL"]),Convert.ToDouble(ViewState["TOTAL_IVA"]),Convert.ToDouble(ViewState["TOTAL"]),dsOrdenes.Tables[0],Convert.ToDouble(ViewState["VALOR_CATEGORIA"]),Convert.ToDouble(ViewState["PIVA"]));
			if(liquida.Liquidar())
			{
				plcSeleccion.Visible=true;
				plcFactura.Visible=false;
                Utils.MostrarAlerta(Response, "Se ha realizado la liquidación del concesionario " + txtConcesionarioa.Text + " para el mes " + ddlMes.SelectedValue + " y año " + ddlAno.SelectedValue + ".");
			}
			else
			{
                Utils.MostrarAlerta(Response, "" + liquida.error + "");
				lblError.Text=liquida.sqlError;
				if(liquida.cambianDatos)
				{
					plcFactura.Visible=false;
					plcSeleccion.Visible=true;
				}
			}
        }
        #endregion Eventos

    }
}
