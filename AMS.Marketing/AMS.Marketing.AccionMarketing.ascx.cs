namespace AMS.Marketing
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.Mail;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text;
	using System.IO;
    using AMS.Automotriz;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Marketing_AccionMarketing.
	/// </summary>
	public partial class AMS_Marketing_AccionMarketing : System.Web.UI.UserControl
	{
		public string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        public string strCookie;
        private string Tipo;
        private string actividad;
        private string sqlCliente = "INSERT INTO MCLIENTE VALUES(" +
                        "'{0}',NULL,NULL,0,0,NULL,NULL," +
                        "NULL,NULL,NULL,NULL,NULL,NULL,'N',0,NULL,'{1}'," +
                        "NULL,NULL,NULL,'1','1',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);";
        
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
            plcCita.Controls.Add(LoadControl("" + pathToControls + "AMS.Automotriz.CitaTaller.ascx"));
            
            plcEncuesta.Controls.Add(LoadControl("" + pathToControls + "AMS.Marketing.AplicacionEncuestas.ascx"));
            Session["encuesta"] = 1;
            
            if(!IsPostBack)
			{
                ((Automotriz.CitaTaller)plcCita.Controls[0]).CargarDatosIniciales();
                ViewState["tipo"] = Request.QueryString["tipo"];
                Tipo = (string)ViewState["tipo"];
                ViewState["tiempoIni"] = DateTime.Now;
                actividad = Request.QueryString["Activ"];

                if (actividad == "C2S") //llamadas de 2do dia taller
                    lblCliente.Text = Request.QueryString["CliContact"];
                else
                    lblCliente.Text = DBFunctions.SingleData("select mnit_nombres CONCAT ' ' CONCAT COALESCE(mnit_nombre2,'') CONCAT ' ' concat mnit_apellidos CONCAT ' ' concat COALESCE(mnit_apellido2,'') concat ' (' concat mnit_nit concat ')' AS Nombre FROM dbxschema.mnit where mnit_nit='" + Request.QueryString["idCli"] + "';");
                    
                lbTituloCliente.Text = lblCliente.Text.ToUpper();

				DatasToControls bind=new DatasToControls();
				lblVendedor.Text=DBFunctions.SingleData("select pven_nombre concat ' (' concat pven_codigo concat ')' FROM dbxschema.pvendedor where pven_codigo='"+Request.QueryString["idVen"]+"';");
				lblFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");

                if(actividad != null)
                    bind.PutDatasIntoDropDownList(ddlActividad, "SELECT PACT_CODIMARK, PACT_NOMBMARK  FROM DBXSCHEMA.PACTIVIDADMARKETING WHERE PACT_VIGENTE='S' AND TACT_TIPOACTI='" + actividad + "' ORDER BY PACT_NOMBMARK;");
				else
                    bind.PutDatasIntoDropDownList(ddlActividad, "SELECT PACT_CODIMARK, PACT_NOMBMARK  FROM DBXSCHEMA.PACTIVIDADMARKETING WHERE PACT_VIGENTE='S' ORDER BY PACT_NOMBMARK;");

                bind.PutDatasIntoDropDownList(ddlResultado,  "SELECT PRES_CODIGO, PRES_DESCRIPCION FROM DBXSCHEMA.PRESULTADOACTIVIDAD WHERE PRES_VIGENTE='S' ORDER BY PRES_DESCRIPCION;");

                if(Tipo == "C")
                    bind.PutDatasIntoDropDownList(ddlMercadeista, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM DBXSCHEMA.PVENDEDOR           WHERE TVEND_CODIGO='MC' AND PVEN_VIGENCIA='V' ORDER BY PVEN_NOMBRE;");
                else
                    bind.PutDatasIntoDropDownList(ddlMercadeista, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM DBXSCHEMA.PVENDEDOR           WHERE TVEND_CODIGO<>'MG' AND PVEN_VIGENCIA='V' ORDER BY PVEN_NOMBRE;");

                strCookie = "var d=new Date();document.cookie='mytab1tabber=0;expires='+d.toGMTString()+';' + ';';";
                if (ddlActividad.Items.Count == 0 || ddlResultado.Items.Count == 0 || ddlMercadeista.Items.Count == 0)
                {
                    Utils.MostrarAlerta(Response, "No ha Parametrizado las Actividades o Resultados o Mercaderistas, Proceso Cancelado ...!");
                    return;
                }
                CargarBasicos();
                CargarFicha();
                CargarVehiculos();
                CargarHistorial();
                CargarOrdenes();
                Session["HREFRETURN"] = "process=Marketing.AccionMarketing&idCli=" + Request.QueryString["idCli"] + "&idVen=" + Request.QueryString["idVen"] + "&path=" + Request.QueryString["path"] + "&vin=" + Request.QueryString["vin"];
                if (Request.QueryString["vin"] != null)
                {
                    string placa = DBFunctions.SingleData("SELECT MCAT_PLACA FROM MCATALOGOVEHICULO WHERE MCAT_VIN='" + Request.QueryString["vin"].ToString() + "';");
                    ((TextBox)plcCita.Controls[0].FindControl("placa")).Text = placa;
                    if (placa.Length > 0)
                    {
                        ((Automotriz.CitaTaller)plcCita.Controls[0]).cargarPanelCliente();
                        ((Automotriz.CitaTaller)plcCita.Controls[0]).Consultar_Vehiculo(sender, e);
                    }
                }
                if (Session["MARKFLT_FECHADESDE"] != null)
                    ViewState["MARKFLT_FECHADESDE"] = Session["MARKFLT_FECHADESDE"];
                if (Session["MARKFLT_FECHAHASTA"] != null)
                    ViewState["MARKFLT_FECHAHASTA"] = Session["MARKFLT_FECHAHASTA"];
                if (Session["MARKFLT_VENDEDOR"] != null)
                    ViewState["MARKFLT_VENDEDOR"] = Session["MARKFLT_VENDEDOR"];
                if (Session["MARKFLT_CLAVE"]!=null) 
                    ViewState["MARKFLT_CLAVE"]=Session["MARKFLT_CLAVE"];
			}

			lblNumero.Text=DBFunctions.SingleData("SELECT MAX(dmar_numeacti)+1 from dBxschema.dmarketing;");
            Tipo = (string)ViewState["tipo"];
            
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
         
        protected void ddlActividad_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlActividad, "SELECT PACT_CODIMARK, PACT_NOMBMARK  FROM DBXSCHEMA.PACTIVIDADMARKETING WHERE PACT_VIGENTE='S' ORDER BY PACT_NOMBMARK;");
        }
        
	
        private void CargarVehiculos() {
            DataSet dsVehiculos = new DataSet();
            DBFunctions.Request(dsVehiculos, IncludeSchema.NO,
                "SELECT * from MCATALOGOVEHICULO WHERE MNIT_NIT='" + Request.QueryString["idCli"] + "';");
            if (dsVehiculos.Tables[0].Rows.Count == 0)
            {
                lblClienteV.Text = "No hay vehículos registrados.";
                return;
            }
            else
                lblClienteV.Text = "Vehículos registrados:";
            dgrVehiculos.DataSource = dsVehiculos.Tables[0];
            dgrVehiculos.DataBind();
            if (ViewState["FICHAVEHI"] != null)
            {
                lblVehiAct.Text = "Ficha del vehículo seleccionado:";
                CargarVehiculo();
            }
        }
        private void CargarHistorial()
        {
            DataSet dsHistorial = new DataSet();
            DBFunctions.Request(dsHistorial, IncludeSchema.NO,
                "SELECT * from dmarketing dm, pactividadmarketing pa, pvendedor pv, presultadoactividad pr "+
                "WHERE pa.pact_codimark=dm.pact_codimark and "+
                "dm.MNIT_NIT='" + Request.QueryString["idCli"] + "' and "+
                "pv.pven_codigo=dm.pven_codigo and dm.pven_codigo=pv.pven_codigo and " +
                "pr.pres_codigo=dm.pres_codigo " +
                "ORDER BY DMAR_FECHACTI DESC;");
            if (dsHistorial.Tables[0].Rows.Count == 0)
            {
                lblClienteH.Text = "No hay entradas registradas.";
                return;
            }
            else
                lblClienteH.Text = "Entradas registradas:";
            dgrHistorial.DataSource = dsHistorial.Tables[0];
            dgrHistorial.DataBind();
        }
        private void CargarOrdenes()
        {
            DataSet dsOrdenes = new DataSet();
            DBFunctions.Request(dsOrdenes, IncludeSchema.NO,
                "select MO.PDOC_CODIGO concat '-' concat RTRIM(CHAR(MO.MORD_NUMEORDE)) AS ORDEN, MC.MCAT_PLACA AS PLACA,MORD_ENTRADA AS FECHA, "+
                "TE.TEST_NOMBRE ESTADO, TT.TTRA_NOMBRE AS TRABAJO, TC.TCAR_NOMBRE AS CARGO, MO.MORD_KILOMETRAJE AS KILOMETRAJE, "+
                "MO.MORD_OBSERECE AS OBSREC, MO.MORD_OBSECLIE AS OBSCLI, MO.MORD_FECHLIQU AS FECHAS "+
                "from MORDEN MO,MCATALOGOVEHICULO MC, TTRABAJOORDEN TT, TCARGOORDEN TC, TESTADOORDEN TE "+
                "where MO.MCAT_VIN=MC.MCAT_VIN AND TT.TTRA_CODIGO=MO.TTRA_CODIGO AND MO.TCAR_CARGO=TC.TCAR_CARGO AND MO.MNIT_NIT='" + Request.QueryString["idCli"] + "' AND TE.TEST_ESTADO=MO.TEST_ESTADO " +
                "ORDER BY MC.MCAT_PLACA, MORD_ENTRADA DESC;");
            if (dsOrdenes.Tables[0].Rows.Count == 0)
            {
                lblClienteH.Text = "No hay entradas registradas.";
                return;
            }
            else
                lblClienteH.Text = "Entradas registradas:";
            dgrOrdenes.DataSource = dsOrdenes.Tables[0];
            dgrOrdenes.DataBind();
        }
        private void CargarVehiculo()
        {
            DataSet dsFicha = new DataSet();
            DataTable dtFicha = new DataTable(), dtCliente;
            DataRow drFicha;
            dtFicha.Columns.Add(new DataColumn("CAMPO", System.Type.GetType("System.String")));
            dtFicha.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.String")));
            DBFunctions.Request(dsFicha, IncludeSchema.NO,
                "select remarks from SYSIBM.SYSCOLUMNS WHERE TBNAME='MCATALOGOVEHICULO' ORDER BY COLNO;" +
                "SELECT * from MCATALOGOVEHICULO WHERE MCAT_VIN='" + Request.QueryString["vin"] + "';");
            dtCliente = dsFicha.Tables[1];
            for (int n = 0; n < dsFicha.Tables[1].Columns.Count; n++)
            {
                drFicha = dtFicha.NewRow();
                drFicha[0] = dsFicha.Tables[0].Rows[n][0].ToString();
                drFicha[1] = dsFicha.Tables[1].Rows[0][n].ToString();
                dtFicha.Rows.Add(drFicha);
            }
            dgrVehiculo.DataSource = dtFicha;
            dgrVehiculo.DataBind();
        }
        private void CargarFicha()
        {
            DataSet dsFicha = new DataSet();
            DataTable dtFicha = new DataTable(), dtCliente;
            DataRow drFicha;
            dtFicha.Columns.Add(new DataColumn("CAMPO", System.Type.GetType("System.String")));
            dtFicha.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.String")));
            DBFunctions.Request(dsFicha, IncludeSchema.NO,
                "select remarks from SYSIBM.SYSCOLUMNS WHERE TBNAME='MCLIENTE' ORDER BY COLNO;" +
                "SELECT * from mcliente WHERE MNIT_NIT='" + Request.QueryString["idCli"] + "';");
            if (dsFicha.Tables[1].Rows.Count == 0)
            {
                lblClienteE.Text = "Cliente no registrado.";
                ViewState["CREAR"] = true;
                return;
            }
            else
                lblClienteE.Text = "Datos del Cliente:";
            dtCliente=dsFicha.Tables[1];
            for (int n = 0; n < dsFicha.Tables[1].Columns.Count; n++)
            {
                drFicha = dtFicha.NewRow();
                drFicha[0] = dsFicha.Tables[0].Rows[n][0].ToString();
                if(dtCliente.Columns[n].ColumnName.Equals("PCON_CONTACTO"))
                    drFicha[1] = DBFunctions.SingleData("SELECT PCON_NOMBRE FROM PCONTACTOCLIENTE WHERE PCON_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString()+"';");
                else if(dtCliente.Columns[n].ColumnName.Equals("PCIU_CODIGO"))
                    drFicha[1] = DBFunctions.SingleData("SELECT PCIU_NOMBRE FROM PCIUDAD WHERE PCIU_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString()+"';");
                else if (dtCliente.Columns[n].ColumnName.Equals("PCLA_CODIGO"))
                    drFicha[1] = DBFunctions.SingleData("SELECT PCLA_DESCRIPCION FROM PCLASECLIENTE WHERE PCLA_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("POCU_CODIGO"))
                    drFicha[1] = DBFunctions.SingleData("SELECT POCU_NOMBRE FROM POCUPACION WHERE POCU_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("PHOB_CODIGO"))
                    drFicha[1] = DBFunctions.SingleData("SELECT PHOB_NOMBRE FROM PHOBBIE WHERE PHOB_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("PPRO_CODIGO"))
                    drFicha[1] = DBFunctions.SingleData("SELECT PPRO_NOMBRE FROM PPROFESION WHERE PPRO_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("PVEN_CODIGO"))
                    drFicha[1] = DBFunctions.SingleData("SELECT PVEN_NOMBRE FROM PVENDEDOR WHERE PVEN_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("TSEX_CODIGO"))
                    drFicha[1] = DBFunctions.SingleData("SELECT TSEX_NOMBRE FROM TSEXO WHERE TSEX_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("TEST_ESTACIVIL"))
                    drFicha[1] = DBFunctions.SingleData("SELECT TEST_NOMBRE FROM TESTADOCIVIL WHERE TEST_ESTACIVIL='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else drFicha[1] = dsFicha.Tables[1].Rows[0][n].ToString();
                dtFicha.Rows.Add(drFicha);
            }
            dgrCliente.DataSource = dtFicha;
            dgrCliente.DataBind();
        }
        private void CargarBasicos()
        {
            DataSet dsFicha = new DataSet();
            DataTable dtFicha = new DataTable(), dtCliente;
            DataRow drFicha;
            dtFicha.Columns.Add(new DataColumn("CAMPO", System.Type.GetType("System.String")));
            dtFicha.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.String")));
            DBFunctions.Request(dsFicha, IncludeSchema.NO,
                "select remarks from SYSIBM.SYSCOLUMNS WHERE TBNAME='MNIT' ORDER BY COLNO;" +
                "SELECT * from mnit WHERE MNIT_NIT='" + Request.QueryString["idCli"] + "';");
            if (dsFicha.Tables[1].Rows.Count == 0)
            {
                lblClienteE.Text = "NIT no registrado.";
                return;
            }
            else
                lblClienteE.Text = "Datos del NIT:";
            dtCliente = dsFicha.Tables[1];
            for (int n = 0; n < dsFicha.Tables[1].Columns.Count; n++)
            {
                drFicha = dtFicha.NewRow();
                drFicha[0] = dsFicha.Tables[0].Rows[n][0].ToString();
                if (dtCliente.Columns[n].ColumnName.Equals("PCIU_CODIGOEXPNIT"))
                    drFicha[1] = DBFunctions.SingleData("SELECT PCIU_NOMBRE FROM PCIUDAD WHERE PCIU_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("TNAC_TIPONACI"))
                    drFicha[1] = DBFunctions.SingleData("SELECT TNAC_NOMBRE FROM TNACIONALIDAD WHERE TNAC_TIPONACI='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("PCIU_CODIGO"))
                    drFicha[1] = DBFunctions.SingleData("SELECT PCIU_NOMBRE FROM PCIUDAD WHERE PCIU_CODIGO='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("TVIG_VIGENCIA"))
                    drFicha[1] = DBFunctions.SingleData("SELECT TVIG_NOMBVIGE FROM TVIGENCIA WHERE TVIG_VIGENCIA='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("TSOC_SOCIEDAD"))
                    drFicha[1] = DBFunctions.SingleData("SELECT TSOC_NOMBRE FROM TSOCIEDAD WHERE TSOC_SOCIEDAD='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("TREG_REGIIVA"))
                    drFicha[1] = DBFunctions.SingleData("SELECT TREG_NOMBRE FROM TREGIMENIVA WHERE TREG_REGIIVA='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else if (dtCliente.Columns[n].ColumnName.Equals("PSEC_ACTIVIDAD"))
                    drFicha[1] = DBFunctions.SingleData("SELECT PSEC_ACTIVIDAD FROM TREGIMENIVA WHERE PSEC_ACTIVIDAD='" + dsFicha.Tables[1].Rows[0][n].ToString() + "';");
                else drFicha[1] = dsFicha.Tables[1].Rows[0][n].ToString();
                dtFicha.Rows.Add(drFicha);
            }
            dgrNIT.DataSource = dtFicha;
            dgrNIT.DataBind();
        }
        protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
            if (ViewState["MARKFLT_FECHADESDE"] != null)
                Session["MARKFLT_FECHADESDE"] = ViewState["MARKFLT_FECHADESDE"];
            if (ViewState["MARKFLT_FECHAHASTA"] != null)
                Session["MARKFLT_FECHAHASTA"] = ViewState["MARKFLT_FECHAHASTA"];
            if (ViewState["MARKFLT_VENDEDOR"] != null)
                Session["MARKFLT_VENDEDOR"] = ViewState["MARKFLT_VENDEDOR"];
            if (ViewState["MARKFLT_CLAVE"] != null)
                Session["MARKFLT_CLAVE"] = ViewState["MARKFLT_CLAVE"];

            Response.Redirect(indexPage + "?process=Marketing.ReporteLlamadasClientes&ret=1&nit=" + lbTituloCliente.Text + "&tipo=" + Tipo);
		}
        protected void btnEditar_Click(object sender, System.EventArgs e)
        {
            //Insertar MCLIENTE si no existe
            Session["HREFRETURN"] = "process=Marketing.AccionMarketing&idCli=" + Request.QueryString["idCli"] + "&idVen=" + Request.QueryString["idVen"] + "&path=" + Request.QueryString["path"] + "&vin=" + Request.QueryString["vin"];
            if(ViewState["CREAR"]!=null)
                if (!DBFunctions.RecordExist("SELECT * FROM MCLIENTE WHERE MNIT_NIT='" + Request.QueryString["idCli"] + "';"))
                    DBFunctions.SingleData(String.Format(sqlCliente,Request.QueryString["idCli"],DateTime.Now.ToString("yyyy-MM-dd")));
            Response.Redirect(indexPage + "?process=DBManager.Inserts&table=MCLIENTE&action=update&pks=MNIT_NIT='" + Request.QueryString["idCli"] + "'&cod=&name=&processTitle=&path=Cat%C3%A1logo%20Clientes");
        }
        protected void btnEditarN_Click(object sender, System.EventArgs e)
        {
            //Insertar MCLIENTE si no existe
            Session["HREFRETURN"] = "process=Marketing.AccionMarketing&idCli=" + Request.QueryString["idCli"] + "&idVen=" + Request.QueryString["idVen"] + "&path=" + Request.QueryString["path"] + "&vin=" + Request.QueryString["vin"];
            Response.Redirect(indexPage + "?process=DBManager.Inserts&table=MNIT&action=update&pks=MNIT_NIT='" + Request.QueryString["idCli"] + "'&cod=&name=&processTitle=&path=Cat%C3%A1logo%20Clientes");
        }
		protected void btnGrabar_Click(object sender, System.EventArgs e)
		{
			ArrayList sqlUpd=new ArrayList();
			string numero=DBFunctions.SingleData("SELECT MAX(dmar_numeacti)+1 from dBxschema.dmarketing;");
			string fechaProx=(txtFechaProx.PostedDate == "" ? "NULL" : "'" + txtFechaProx.SelectedDate.ToString("yyyy-MM-dd") + "'");
            DateTime fchProx = DateTime.Now; 

            TimeSpan tRes = DateTime.Now - (DateTime)ViewState["tiempoIni"];
            int minD = (int) Math.Round(tRes.TotalMinutes);
            minD = minD == 0 ? 1 : minD;

			try{
				if(txtFechaProx.PostedDate.Length>0){
                    fchProx = txtFechaProx.SelectedDate;
					if(fchProx<DateTime.Now.AddDays(-1))
						throw(new Exception());
				}
			}
			catch{
                Utils.MostrarAlerta(Response, "Fecha próxima no válida, debe ser menor o igual a la fecha actual.");
				return;
			}


            //Insertar MCLIENTE si no existe
            if (!DBFunctions.RecordExist("SELECT * FROM MCLIENTE WHERE MNIT_NIT='" + Request.QueryString["idCli"] + "';"))
                sqlUpd.Add(String.Format(sqlCliente,Request.QueryString["idCli"],DateTime.Now.ToString("yyyy-MM-dd")));

            string idVen = Request.QueryString["idVen"];
            if (idVen == null || idVen == "") idVen = ddlMercadeista.SelectedValue;

            sqlUpd.Add("INSERT INTO DBXSCHEMA.DMARKETING VALUES(DEFAULT,NULL,'" + Request.QueryString["idCli"] + "','" + ddlActividad.SelectedValue + "','"
            + txtDetalle.Text.Replace("'", "''").Trim() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + idVen + "','" + ddlResultado.SelectedValue + "'," + 
            fechaProx + ",'" + ddlMercadeista.SelectedValue + "'," + minD + ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',null,null);");

			if(DBFunctions.Transaction(sqlUpd))
			{
                Response.Redirect(indexPage + "?process=Marketing.ReporteLlamadasClientes&act=1&ret=1&nit=" + lbTituloCliente.Text + "&tipo=" + Tipo);
			}   
			else
				lb.Text+="Error "+DBFunctions.exceptions;
		}
        protected void dgrVehiculos_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (Request.QueryString["vin"]!=null && DataBinder.Eval(e.Item.DataItem, "MCAT_VIN").ToString().Trim().Equals(Request.QueryString["vin"].ToString().Trim()))
                {
                    for (int n = 0; n < e.Item.Cells.Count;n++) 
                        e.Item.Cells[n].BackColor = System.Drawing.Color.Tomato;
                    ViewState["FICHAVEHI"] = true;
                }
            }
        }
        protected void dgrVehiculos_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Session["HREFRETURN"] = "process=Marketing.AccionMarketing&idCli=" + Request.QueryString["idCli"] + "&idVen=" + Request.QueryString["idVen"] + "&path=" + Request.QueryString["path"] + "&vin=" + Request.QueryString["vin"];
            if (e.CommandName == "Editar")
                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=MCATALOGOVEHICULO&action=update&pks=PCAT_CODIGO='" + e.Item.Cells[0].Text + "' AND MCAT_VIN='" + e.Item.Cells[1].Text + "'&cod=&name=&processTitle=&path=Cat%C3%A1logos%20Vehiculos");
            else if (e.CommandName == "HV")
                Response.Redirect(indexPage + "?process=Automotriz.HojaVida&vinE=" + e.Item.Cells[1].Text + "&vin=" + Request.QueryString["vin"] + "&cod=&name=&path=Hojas de Vida Vehículos");
        }
	}
}
