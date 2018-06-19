namespace AMS.Vehiculos
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
    using AMS.Documentos;
	using AMS.Forms;
	using Ajax;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Vehiculos_AutorizacionGarantias.
	/// </summary>
	public partial class AMS_Vehiculos_AutorizacionGarantias : System.Web.UI.UserControl
	{
		#region Controles
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
		//		bind.PutDatasIntoDropDownList(ddlConcesionario,"SELECT MN.MNIT_NIT, MN.MNIT_APELLIDOS FROM MNIT MN, MCONCESIONARIO MC WHERE MC.MNIT_NIT=MN.MNIT_NIT ORDER BY MNIT_APELLIDOS;");
		//		bind.PutDatasIntoDropDownList(ddlConcesionario,"SELECT MN.MNIT_NIT, MN.nombre         FROM vMNIT MN, MCONCESIONARIO MC WHERE MC.MNIT_NIT=MN.MNIT_NIT ORDER BY mn.nombre;");
				bind.PutDatasIntoDropDownList(ddlConcesionario,"SELECT distinct MN.MNIT_NIT, MN.nombre FROM vMNIT MN, MCONCESIONARIO MC, mordenpostventa mo WHERE MC.MNIT_NIT=MN.MNIT_NIT and MC.MNIT_NIT=Mo.MNIT_NITconc and mo.mgar_numero is Null ORDER BY mn.nombre;");
				ddlConcesionario.Items.Insert(0,new ListItem("--seleccione--",""));
				bind.PutDatasIntoDropDownList(ddlPrefOrden,"SELECT PDOC_CODIPOST FROM CTALLER;");
                bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT pven_codigo,pven_nombre from pvendedor WHERE TVEND_CODIGO = 'AG';");
				//bind.PutDatasIntoDropDownList(ddlAutorizar,"SELECT tres_sino,tres_nombre from trespuestasino;");
				ddlVendedor.Items.Insert(0,new ListItem("--seleccione--",""));
				ddlNumeOrden.Items.Insert(0,new ListItem("--seleccione--",""));
				txtFechaProceso.Text=DateTime.Now.ToString("yyyy-MM-dd");
				if(Request.QueryString["idG"]!=null)
				{
                    Utils.MostrarAlerta(Response, "La autorización se ha realizado con el número " + Request.QueryString["idG"] + ".");
                    if (Request["prefOT"] != null && Request["prefOT"] != string.Empty && Request["numOT"] != null && Request["numOT"] != string.Empty)
                    {
                        FormatosDocumentos formatoRecibo = new FormatosDocumentos();
                        try
                        {
                            formatoRecibo.Prefijo = Request["prefOT"];
                            formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numOT"]);
                            formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefOT"] + "'");
                            if (formatoRecibo.Codigo != string.Empty)
                            {
                                if (formatoRecibo.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                            }
                        }
                        catch
                        {
                            Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
                        }

                        try
                        {
                            formatoRecibo.Prefijo = Request["tipoPED"];
                            formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numPED"]);
                            formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.PPEDIDO WHERE pped_codigo='" + Request.QueryString["tipoPED"] + "'");
                            if (formatoRecibo.Codigo != string.Empty)
                            {
                                if (formatoRecibo.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                            }
                        }
                        catch
                        {
                            Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
                        }

                        try
                        {
                            formatoRecibo.Prefijo = Request["prefTRA"];
                            formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numTRA"]);
                            formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefTRA"] + "'");
                            if (formatoRecibo.Codigo != string.Empty)
                            {
                                if (formatoRecibo.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                            }
                        }
                        catch
                        {
                            Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
                        }
                       
                    }
				}
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

		//Seleccionan orden
		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			if(ddlConcesionario.SelectedValue.Length==0 || ddlNumeOrden.SelectedValue.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar la orden y el concesionario");
				return;
			}
			plcInformacion.Visible=true;
			plcSeleccion.Visible=false;
			DataSet dsOperAnt=new DataSet();
			DBFunctions.Request(dsOperAnt,IncludeSchema.NO,
                "Select top.pdoc_codigo, top.mord_numeorde, top.ptem_operacion, top.codigo, top.nombre, top.fecha, top.kilometraje, top.precio, pg1.pgar_descripcion incidente, pg2.pgar_descripcion causal, 1 as usar " +
				"from( "+
				"Select "+
                " dor.pdoc_codigo, dor.mord_numeorde, dor.ptem_operacion, "+
				" pt.ptem_operacion codigo, pt.ptem_descripcion nombre, mo.mord_creacion fecha, "+
				" mo.mord_kilometraje kilometraje, pt.ptem_valooper precio, pgar_codigo1, pgar_codigo2 "+
				"from dbxschema.dordenoperacion dor, dbxschema.morden mo, dbxschema.ptempario pt "+
				"where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and "+
				"mo.mord_numeorde="+ddlNumeOrden.SelectedValue+" and mo.pdoc_codigo='"+ddlPrefOrden.SelectedValue+"' and "+
				"pt.ptem_operacion=dor.ptem_operacion order by mord_creacion desc) as top "+
				"left join pgarantia pg1 on pg1.pgar_codigo=top.pgar_codigo1 "+
				"left join pgarantia pg2 on pg2.pgar_codigo=top.pgar_codigo2;");
			dgrOperacionesAnt.DataSource=dsOperAnt.Tables[0];
			dgrOperacionesAnt.DataBind();
			DataSet dsRepAnt=new DataSet();
			DBFunctions.Request(dsRepAnt,IncludeSchema.NO,
                "Select dor.pdoc_codigo, dor.mord_numeorde, dor.mite_codigo, mcat_vin, mi.mite_codigo codigo, mi.mite_NOMBRE nombre, mo.mord_creacion fecha, mo.mord_kilometraje kilometraje, dor.mite_precio precio, dor.mite_cantidad cantidad, cast(0 as decimal(14,2)) as valaprueba " +
				"from dbxschema.dordenitemspostventa dor, dbxschema.morden mo, dbxschema.mitems mi "+
				"where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and "+
				"mo.mord_numeorde="+ddlNumeOrden.SelectedValue+" and mo.pdoc_codigo='"+ddlPrefOrden.SelectedValue+"' "+
				"and mi.mite_codigo=dor.mite_codigo order by mord_creacion desc;");
			dgrRepuestosAnt.DataSource=dsRepAnt.Tables[0];
			dgrRepuestosAnt.DataBind();
            ViewState["OPERACIONES"] = dsOperAnt.Tables[0];
            ViewState["REPUESTOS"] = dsRepAnt.Tables[0];

            DataSet dsInfo = new DataSet(), dsGarantia = new DataSet();
			DBFunctions.Request(dsInfo,IncludeSchema.NO,
				"SELECT mo.PDOC_FACTCONCE, mo.MFAC_NUMECONCE, mv.mcat_placa AS PLACA, mv.mcat_vin, mv.pcat_codigo as CATALOGO, "+
				"mp.mveh_fechvent FECHA, mp.mveh_preffactvent PREF, mp.mveh_numefactvent NUMERO, mp.mnit_nitdistr NIT_DIST, "+
                "COALESCE(MN.MNIT_NOMBRES,'') CONCAT ' ' CONCAT COALESCE(MN.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT COALESCE(MN.MNIT_APELLIDOS,'') CONCAT ' ' CONCAT COALESCE(MN.MNIT_APELLIDO2,'') NOMBRE_DIST, MN.MNIT_DIRECCION DIRECCION_DIST, " +
                "mv.mcat_motor as MOTOR,mv.mcat_anomode AS MODELO,mv.mcat_numeultikilo KILOMETRAJE,COALESCE(VARCHAR(mv.mcat_fechultikilo),'') AS FECHKILOMETRAJE "+
				"FROM mordenpostventa mo, mcatalogovehiculo mv, mvehiculopostventa mp, mnit mn, morden mr "+
				"WHERE mo.PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND mo.MORD_NUMEORDE="+ddlNumeOrden.SelectedValue+" AND "+
				"mo.PDOC_CODIGO=mr.PDOC_CODIGO and mo.MORD_NUMEORDE=mr.MORD_NUMEORDE and "+
				"mv.mcat_vin=mr.mcat_vin and mn.mnit_nit=mnit_nitdistr and mp.mcat_vin=mv.mcat_vin;");
			lblConcesionario.Text=ddlConcesionario.SelectedItem.Text;
            lblModelo.Text = dsInfo.Tables[0].Rows[0]["MODELO"].ToString();
            lblKilometraje.Text = dsInfo.Tables[0].Rows[0]["KILOMETRAJE"].ToString();
            lblFechKilometraje.Text = dsInfo.Tables[0].Rows[0]["FECHKILOMETRAJE"].ToString();
            txtPlaca.Text = dsInfo.Tables[0].Rows[0]["PLACA"].ToString();
            lblMotorVehiculo.Text = dsInfo.Tables[0].Rows[0]["MOTOR"].ToString();
			lblFactConc.Text=dsInfo.Tables[0].Rows[0]["PDOC_FACTCONCE"].ToString()+" "+dsInfo.Tables[0].Rows[0]["MFAC_NUMECONCE"].ToString();
			lblOrden.Text=ddlPrefOrden.SelectedValue+" - "+ddlNumeOrden.SelectedValue;
			lblVehiculoPlaca.Text=dsInfo.Tables[0].Rows[0]["PLACA"].ToString();
			lblVehiculoVIN.Text=dsInfo.Tables[0].Rows[0]["MCAT_VIN"].ToString();
            lblCatalogo.Text = dsInfo.Tables[0].Rows[0]["CATALOGO"].ToString();
			ViewState["VIN_VEHICULO"]=dsInfo.Tables[0].Rows[0]["MCAT_VIN"].ToString();
			lblFactFecha.Text=Convert.ToDateTime(dsInfo.Tables[0].Rows[0]["FECHA"]).ToString("yyyy-MM-dd");
			lblFactDitri.Text="<table><tr><td valign='top'>"+
				dsInfo.Tables[0].Rows[0]["NOMBRE_DIST"].ToString()+"<br>"+
				dsInfo.Tables[0].Rows[0]["NIT_DIST"].ToString()+"<br>"+
				dsInfo.Tables[0].Rows[0]["DIRECCION_DIST"].ToString()+"</td></tr></table>";
			lblFactIni.Text=dsInfo.Tables[0].Rows[0]["PREF"].ToString()+" "+dsInfo.Tables[0].Rows[0]["NUMERO"].ToString();
            DBFunctions.Request(dsGarantia, IncludeSchema.NO, "SELECT PCAT_KILOMETRAJEGARANTIA KILOM,PCAT_MESESGARANTIA MESES FROM PCATALOGOVEHICULO PC, MCATALOGOVEHICULO MC WHERE MC.MCAT_VIN='" + lblVehiculoVIN.Text + "' AND MC.PCAT_CODIGO=PC.PCAT_CODIGO;");
            if (dsGarantia.Tables[0].Rows.Count > 0)
            {
                int mesesG = Convert.ToInt16(dsGarantia.Tables[0].Rows[0]["MESES"]);
                int kilosG = Convert.ToInt16(dsGarantia.Tables[0].Rows[0]["KILOM"]);
                lblMesesGarantia.Text = mesesG.ToString();
                lblKiloGarantia.Text = kilosG.ToString();
            }
		}
		
		
		//Ejecutar autorizacion
		public void btnEjecutar_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			string vendedor=ddlVendedor.SelectedValue;
			string usuario=txtUsuario.Text.Trim();
			DateTime fechaProc;
            DataTable dtOperaciones=(DataTable)ViewState["OPERACIONES"];
            DataTable dtRepuestos = (DataTable)ViewState["REPUESTOS"];
			if(vendedor.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar el ejecutivo de garantías");
				return;
			}
			if(usuario.Length==0){
                Utils.MostrarAlerta(Response, "Debe ingresar el usuario que solicita");
				return;
			}
			try
			{
				fechaProc=Convert.ToDateTime(txtFechaProceso.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Debe ingresar el usuario que solicita");
				return;
			}
            string selItem = "";
            double valorA = 0, valorM = 0;
            for(int i=0;i<dgrOperacionesAnt.Items.Count;i++)
				dtOperaciones.Rows[i]["usar"]=((CheckBox)dgrOperacionesAnt.Items[i].Cells[6].FindControl("chkUsar")).Checked?1:0;
            for(int i=0;i<dgrRepuestosAnt.Items.Count;i++){
                selItem = ((DropDownList)dgrRepuestosAnt.Items[i].Cells[6].FindControl("ddlAprobar")).SelectedValue;
                valorM = Convert.ToDouble(dtRepuestos.Rows[i]["precio"]) * Convert.ToDouble(dtRepuestos.Rows[i]["cantidad"]);
                switch(selItem){
                    case "A":
                        valorA = valorM;
                    break;
                    case "N":
                        valorA = 0;
                    break;
                    case "M":
                        try
                        {
                            valorA = Double.Parse(((TextBox)dgrRepuestosAnt.Items[i].Cells[6].FindControl("txtMonto")).Text);
                            if (valorA > valorM || valorA <= 0)
                                throw (new Exception());
                        }
                        catch
                        {
                            Utils.MostrarAlerta(Response, "Valor aprobado inválido para el repuesto " + (i + 1) + ".");
                            return;
                        }
                    break;
                }
                dtRepuestos.Rows[i]["valaprueba"]=valorA;
            }
			AutorizacionGarantias autorizacion=new AutorizacionGarantias(ddlPrefOrden.SelectedValue,Convert.ToUInt32(ddlNumeOrden.SelectedValue),fechaProc,ddlConcesionario.SelectedValue,usuario,vendedor,"S",ViewState["VIN_VEHICULO"].ToString(),txtObsSolicitud.Text,txtObsReposicion.Text, dtOperaciones, dtRepuestos);
            string urlParams = "&prefOT=" + ddlPrefOrden.SelectedValue + "&numOT=" + ddlNumeOrden.SelectedValue;
			string error=autorizacion.Autorizar(ref urlParams);
			if(error.Length>0)
			{
                Utils.MostrarAlerta(Response, "Revise los errores reportados en la parte inferior.");
				lblError.Text="Errores: <br>"+error;
			}
			else
				Response.Redirect(indexPage + "?process=Vehiculos.AutorizacionGarantias&path="+Request.QueryString["path"]+"&idG="+autorizacion.idGarantia + urlParams);
		}

		//Cambia el concesionario
		protected void ddlConcesionario_Change(Object  Sender, EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			ddlNumeOrden.Items.Clear();
			if(ddlConcesionario.SelectedValue.Length>=0){
				bind.PutDatasIntoDropDownList(ddlNumeOrden,
				"SELECT	MORD_NUMEORDE FROM MORDENPOSTVENTA "+
				"WHERE TPROC_CODIGO='G' AND MGAR_NUMERO IS NULL AND PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND "+
				"MNIT_NITCONC='"+ddlConcesionario.SelectedValue+"';");
			}
			ddlNumeOrden.Items.Insert(0,new ListItem("--seleccione--",""));
		}
	}
}