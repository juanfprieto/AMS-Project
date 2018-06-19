namespace AMS.Finanzas
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using System.Configuration;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Tesoreria_CausacionObligacionesFinancieras.
	/// </summary>
	public partial class AMS_Tesoreria_CausacionObligacionesFinancieras : System.Web.UI.UserControl
	{
		#region Controles
		private DataTable dtObligaciones;
		#endregion Controles

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
                //bind.PutDatasIntoDropDownList(ddlTipoDocumento, "SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU='ND' and tvig_vigencia = 'V' ORDER BY PDOC_NOMBRE;");
                Utils.llenarPrefijos(Response, ref ddlTipoDocumento , "%", "%", "ND");
				lblNumDocumento.Text=DBFunctions.SingleData("SELECT PDOC_ULTIDOCU+1 FROM PDOCUMENTO WHERE PDOC_CODIGO='"+ddlTipoDocumento.SelectedValue+"';");
				bind.PutDatasIntoDropDownList(ddlAno,"SELECT PANO_ANO FROM PANO ORDER BY PANO_ANO;");
				bind.PutDatasIntoDropDownList(ddlMes,"SELECT PMES_MES, PMES_NOMBRE FROM PMES ORDER BY PMES_MES;");
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT PALM_ALMACEN, PALM_DESCRIPCION FROM PALMACEN where (pcen_centcart is not null  or pcen_centteso is not null) and tvig_vigencia='V' order by palm_descripcion;");
				ddlAno.SelectedIndex=ddlAno.Items.IndexOf(ddlAno.Items.FindByValue(DBFunctions.SingleData("SELECT CTES_ANOVIGE FROM CTESORERIA;")));
				ddlMes.SelectedIndex=ddlMes.Items.IndexOf(ddlMes.Items.FindByValue(DBFunctions.SingleData("SELECT CTES_MESVIGE FROM CTESORERIA;")));
				CrearTabla();
				if(Request.QueryString["upd"]!=null)
                Utils.MostrarAlerta(Response, "Las obligaciones financieras han sido causadas");
			}
			else
			{
				dtObligaciones=(DataTable)ViewState["DT_OBLIG"];
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

		private void CrearTabla()
		{
			dtObligaciones=new DataTable();
			dtObligaciones.Columns.Add("PBAN_NOMBRE",typeof(string));
			dtObligaciones.Columns.Add("PCUE_CODIGO",typeof(string));
			dtObligaciones.Columns.Add("MOBL_NUMERO",typeof(string));
			dtObligaciones.Columns.Add("MOBL_MONTPESOS",typeof(double));
			dtObligaciones.Columns.Add("MOBL_SALDO",typeof(double));
			dtObligaciones.Columns.Add("MOBL_INTERESCAUSADO",typeof(double));
			dtObligaciones.Columns.Add("MOBL_INTERESCALCULADO",typeof(double));
			dtObligaciones.Columns.Add("MOBL_NUMECUOTAS",typeof(int));
			dtObligaciones.Columns.Add("MOBL_FECHA",typeof(DateTime));
			dtObligaciones.Columns.Add("MOBL_TASAINTERES",typeof(double));
			dtObligaciones.Columns.Add("PTAS_MONTO",typeof(double));
			ViewState["DT_OBLIG"]=dtObligaciones;
		}

		protected void ddlCuentaEditar_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
            //bind.PutDatasIntoDropDownList(ddlTipoDocumento, "SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU='ND' and tvig_vigencia = 'V' ORDER BY PDOC_NOMBRE;");
            Utils.llenarPrefijos(Response, ref ddlTipoDocumento , "%", "%", "ND");
			lblNumDocumento.Text=DBFunctions.SingleData("SELECT PDOC_ULTIDOCU+1 FROM PDOCUMENTO WHERE PDOC_CODIGO='"+ddlTipoDocumento.SelectedValue+"';");
		}

		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			lblError.Text="";
			if(ddlTipoDocumento.Items.Count==0 || ddlMes.Items.Count==0 || ddlAno.Items.Count==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar el tipo de documento y la fecha");
				return;
			}
			ddlTipoDocumento.Enabled=ddlAno.Enabled=ddlMes.Enabled=ddlAlmacen.Enabled=false;
			btnSeleccionar.Visible=false;
			btnAtras.Visible=true;
			DataSet dsObligaciones=new DataSet();
			string strSql="SELECT PB.PBAN_NOMBRE, MO.PCUE_CODIGO, MO.MOBL_NUMERO, MO.MOBL_MONTPESOS, MO.MOBL_NUMECUOTAS, TC.TCON_NUMEMESES, TC.TCON_VENCIDO, MO.MOBL_TASAINTERES, "+
				"(MO.MOBL_MONTPESOS-MO.MOBL_MONTPAGADO) AS MOBL_SALDO, MOBL_INTERESCAUSADO, CAST(0 AS DECIMAL) MOBL_INTERESCALCULADO, MO.MOBL_FECHA, PT.PTAS_MONTO "+
				"FROM MOBLIGACIONFINANCIERA MO, PBANCO PB, PCUENTACORRIENTE PC, TCONDICIONCREDITOBANCARIO TC, PTASACREDITO PT "+
				"WHERE PC.PCUE_CODIGO=MO.PCUE_CODIGO AND PC.PBAN_BANCO=PB.PBAN_CODIGO AND TC.TCON_CODIGO=MO.TCON_CODIGO AND "+
				"PT.PTAS_CODIGO=MO.PTAS_CODIGO AND "+
				"( YEAR(MOBL_FECHA)<"+ddlAno.SelectedValue+
				" OR "+
				"(YEAR(MOBL_FECHA)="+ddlAno.SelectedValue+" AND MONTH(MOBL_FECHA)<="+ddlMes.SelectedValue+") ) AND "+
				"MO.MOBL_NUMERO NOT IN( "+
				" SELECT MOBL_NUMERO FROM DOBLIGACIONFINANCIERACAUSACION "+
				" WHERE PANO_ANO="+ddlAno.SelectedValue+" AND PMES_MES="+ddlMes.SelectedValue+
				") "+
				"ORDER BY MO.MOBL_NUMERO;";
			DBFunctions.Request(dsObligaciones,IncludeSchema.NO,
				strSql);
			dtObligaciones=dsObligaciones.Tables[0];
			CalcularIntereses();
			dgrObligaciones.EditItemIndex=-1;
			Bind();
		}

		private void CalcularIntereses()
		{
			string numCred="";
			int numMeses=0, numCuotas=0, totalMeses=0;
			double difDias, saldo, tasa, interes;
			bool vencido=false;
			DateTime fechaO, fechaF;
			fechaF=new DateTime(Convert.ToInt16(ddlAno.SelectedValue),Convert.ToInt16(ddlMes.SelectedValue),1);
			fechaF=fechaF.AddMonths(1);
			//fechaF=fechaF.AddDays(-1);
			for(int n=0;n<dtObligaciones.Rows.Count;n++)
			{
				numCred=dtObligaciones.Rows[n]["MOBL_NUMERO"].ToString();
				dtObligaciones.Rows[n]["MOBL_INTERESCALCULADO"]=0;
				numCuotas=Convert.ToInt32(dtObligaciones.Rows[n]["MOBL_NUMECUOTAS"]);
				numMeses=Convert.ToInt32(dtObligaciones.Rows[n]["TCON_NUMEMESES"]);
				saldo=Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_SALDO"]);
				tasa=Convert.ToDouble(dtObligaciones.Rows[n]["PTAS_MONTO"]);
				interes=Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_TASAINTERES"]);
				vencido=dtObligaciones.Rows[n]["TCON_VENCIDO"].ToString().Equals("S");
				totalMeses=numMeses*numCuotas;
				//Fecha Obligacion
				fechaO=Convert.ToDateTime(dtObligaciones.Rows[n]["MOBL_FECHA"]);
				//Trabajar con dia 1 si no es el mes y año actual
				if((fechaF.Day==fechaO.Day && fechaF.Month==fechaO.Month))
					difDias=(fechaF-fechaO).Days;
				else
					difDias=30;
				dtObligaciones.Rows[n]["MOBL_INTERESCALCULADO"]=Math.Round(((difDias/360)*((tasa+interes)/100)*saldo),0);
			}
		}


		private void Bind()
		{
			if(dtObligaciones.Rows.Count == 0){
				pnlGrilla.Visible=false;
				btnSeleccionar.Visible=true;
				btnAtras.Visible=false;
				ddlTipoDocumento.Enabled=ddlAno.Enabled=ddlMes.Enabled=ddlAlmacen.Enabled=true;
                Utils.MostrarAlerta(Response, "No hay creditos activos sin causar para la fecha ingresada");
				return;
			}
			ViewState["DT_OBLIG"]=dtObligaciones;
			pnlGrilla.Visible=true;
			btnSeleccionar.Visible=false;
			btnAtras.Visible=true;
			ddlTipoDocumento.Enabled=ddlAno.Enabled=ddlMes.Enabled=ddlAlmacen.Enabled=false;
			dgrObligaciones.DataSource=dtObligaciones;
			dgrObligaciones.DataBind();
		}

		public void dgrObligaciones_Edit(Object sender, DataGridCommandEventArgs e)
		{
			if(dtObligaciones.Rows.Count>0)
				dgrObligaciones.EditItemIndex=(int)e.Item.ItemIndex;
			Bind();
		}
		public void dgrObligaciones_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgrObligaciones.EditItemIndex=-1;
			Bind();
		}
		public void dgrObligaciones_Update(Object sender, DataGridCommandEventArgs e)
		{
			//Validaciones 
			if(dtObligaciones.Rows.Count == 0)
				return;
			double interesCalc=0;
			try
			{
				interesCalc=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtEdInteresCalc"))).Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "No se puede modificar el interés, revise el valor ingresado");
				return;
			}
			dtObligaciones.Rows[dgrObligaciones.EditItemIndex]["MOBL_INTERESCALCULADO"]=interesCalc;
			dgrObligaciones.EditItemIndex=-1;
			Bind();
		}

		protected void btnAtras_Click(object sender, System.EventArgs e)
		{
			pnlGrilla.Visible=false;
			btnSeleccionar.Visible=true;
			btnAtras.Visible=false;
			ddlTipoDocumento.Enabled=ddlAno.Enabled=ddlMes.Enabled=ddlAlmacen.Enabled=true;
		}

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			int ano=0,mes=0;
			//Validaciones
			try{
				ano=Convert.ToInt32(ddlAno.SelectedValue);
				mes=Convert.ToInt32(ddlMes.SelectedValue);}
			catch{
                Utils.MostrarAlerta(Response, "Año o mes no válido");
				return;}
			for(int n=0;n<dtObligaciones.Rows.Count;n++){
				if(DBFunctions.RecordExist(
					"SELECT MOBL_NUMERO FROM DOBLIGACIONFINANCIERACAUSACION "+
					"WHERE MOBL_NUMERO='"+dtObligaciones.Rows[n]["MOBL_NUMERO"]+"' AND "+
					"PANO_ANO="+ddlAno.SelectedValue+" AND PMES_MES="+ddlMes.SelectedValue+";"
				)){
                    Utils.MostrarAlerta(Response, "Ya ha sido causada la obligación número " + dtObligaciones.Rows[n]["MOBL_NUMERO"] + " para el periodo especificado");
					return;
				}
			}

			ObligacionFinanciera obligacion=new ObligacionFinanciera();
			obligacion.CausarInteres(ano,mes,ddlTipoDocumento.SelectedValue,txtObservacion.Text,ddlAlmacen.SelectedValue,dtObligaciones);
			if(obligacion.error.Length>0){
                Utils.MostrarAlerta(Response, "No se pudo realizar el proceso");
				lblError.Text=obligacion.error;
				return;
			}
			else
				Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Tesoreria.CausacionObligacionesFinancieras&path="+Request.QueryString["path"]+"&upd=1");
		}
	}
}