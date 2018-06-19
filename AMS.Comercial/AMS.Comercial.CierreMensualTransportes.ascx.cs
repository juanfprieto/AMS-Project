namespace AMS.Comercial
{
	using System;
	using System.Configuration;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Data.Odbc;
	using System.Drawing;
	using System.IO;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Globalization;
	using CrystalDecisions.CrystalReports.Engine;
	using CrystalDecisions.Shared;
	using AMS.CriptoServiceProvider;
    using Ajax;
	using AMS.DB;
	using AMS.Tools;

	/// <summary>
	///		Descripción breve de Comercial_CierreMensualTransportes.
	/// </summary>
	public class AMS_Comercial_CierreMensualTransportes : System.Web.UI.UserControl
	{
	    #region Controles
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlperiodo;
        protected System.Web.UI.WebControls.DataGrid dgrConceptos;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label8;
        protected System.Web.UI.WebControls.Label txtFechaI;
		protected System.Web.UI.WebControls.Label txtFechaF;
		protected System.Web.UI.WebControls.Panel pnlConsulta;
		        
		protected System.Web.UI.WebControls.Button btnVerCierre;
		protected System.Web.UI.WebControls.Button btnPrecierrePeriodo;
		protected System.Web.UI.WebControls.Button btnCerrarPeriodo;
		protected System.Web.UI.WebControls.Label lblError;
		#endregion Controles

		int Periodo;
		string TipoCierre;
        string estado;
		double TotalIngresos=0,TotalEgresos=0;
		protected System.Web.UI.WebControls.Button btnAbrirPeriodo;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblPeriodo;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblFechaInicial;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblFechaFinal;
		protected System.Web.UI.WebControls.Panel pnlAbrirPeriodo;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Button btnCrear;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
                Agencias.TraerPeriodo(ddlperiodo);
				btnPrecierrePeriodo.Visible=false;
				btnCerrarPeriodo.Visible=false;
			}

            
            
            if(IsPostBack)
			{
				
				if(estado=="C")
				{
					btnPrecierrePeriodo.Visible=false;
					btnCerrarPeriodo.Visible=false;
				}
				else
				{
					btnPrecierrePeriodo.Visible=true;
					btnCerrarPeriodo.Visible=true;
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
			this.btnAbrirPeriodo.Click += new System.EventHandler(this.btnAbrirPeriodo_Click);
			this.btnVerCierre.Click += new System.EventHandler(this.btnVerCierre_Click);
			this.btnPrecierrePeriodo.Click += new System.EventHandler(this.btnPrecierrePeriodo_Click);
			this.btnCerrarPeriodo.Click += new System.EventHandler(this.btnCerrarPeriodo_Click);
			this.dgrConceptos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrConceptos_ItemDataBound);
			this.btnCrear.Click += new System.EventHandler(this.btnCrearPeriodo_Click);
			this.ddlperiodo.SelectedIndexChanged+=new System.EventHandler(this.ddlperiodo_SelectedIndexChanged);
            this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

        private void cargarInformacion() {

            if (ddlperiodo.SelectedValue.Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe seleccionar el periodo.");
                return;
            }

            Periodo = Convert.ToInt32(ddlperiodo.SelectedValue);
            lblError.Text = " ";
            estado = DBFunctions.SingleData("select estado_cierre from DBXSCHEMA.periodos_cierre_transporte where numero_periodo=" + Periodo + ";");
            if (estado.Length == 0)
            {
                Utils.MostrarAlerta(Response, "El periodo NO Existe.");
                return;
            }
        
        }
        private void ddlperiodo_SelectedIndexChanged(object sender, System.EventArgs e) {
            
            string aux = ddlperiodo.SelectedValue.ToString();
        
        
        }


        private void btnAbrirPeriodo_Click(object sender, System.EventArgs e)
		{
			string NumeroPeriodo = DBFunctions.SingleData("SELECT MAX(NUMERO_PERIODO) FROM DBXSCHEMA.PERIODOS_CIERRE_TRANSPORTE;");

            cargarInformacion();

            if (NumeroPeriodo.Length  ==0)
			{
				lblError.Text= "Periodo No existe ";
				return;

			}
			Periodo = Convert.ToInt32(NumeroPeriodo.ToString());
			int AnoPeriodo  = Periodo /100;
			int MesPeriodo = Periodo%100;
			if  (MesPeriodo   == 12)
			{
			    MesPeriodo=1;
				AnoPeriodo++;
			}
			else 
				 MesPeriodo++;
			Periodo = AnoPeriodo * 100 +  MesPeriodo;
			string fecha = AnoPeriodo+"-"+MesPeriodo+"-"+"01";
			DateTime fechaInicial=Convert.ToDateTime(fecha.ToString());
			DateTime fechaFinal = fechaInicial.AddMonths(1).AddDays(-1) ;
			lblPeriodo.Text = Convert.ToString(Periodo);
			lblFechaInicial.Text=fechaInicial.ToString("yyyy-MM-dd"); 
			lblFechaFinal.Text=fechaFinal.ToString("yyyy-MM-dd");
			pnlAbrirPeriodo.Visible=true;
			btnAbrirPeriodo.Visible=false;
			pnlConsulta.Visible=false;
		}
		private void btnCrearPeriodo_Click(object sender, System.EventArgs e)
        {
            cargarInformacion();
			pnlConsulta.Visible=false;
			DBFunctions.NonQuery("insert into DBXSCHEMA.PERIODOS_CIERRE_TRANSPORTE "+
					"values("+lblPeriodo.Text+",'"+lblFechaInicial.Text+"','"+lblFechaFinal.Text+"',CURRENT TIMESTAMP,CURRENT TIMESTAMP,'A','"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
			btnCrear.Visible=false;
			lblError.Text= "Periodo Abierto ";
		}
		private void btnVerCierre_Click(object sender, System.EventArgs e)
		{
            cargarInformacion();
            pnlAbrirPeriodo.Visible=false;
			if(estado=="A")
				GenerarConsulta(Periodo,"A");
			else
			{
				if(estado=="C")
					lblError.Text= "Periodo Cerrado ";
				else
					lblError.Text= "Periodo en Precierre ";
				VerConsulta();
			}
		}
		private void btnCerrarPeriodo_Click(object sender, System.EventArgs e)
        {
            cargarInformacion();
			GenerarConsulta(Periodo,"C");
			btnPrecierrePeriodo.Visible=false;
			btnCerrarPeriodo.Visible=false;
		}
		private void btnPrecierrePeriodo_Click(object sender, System.EventArgs e)
        {   
            cargarInformacion();
			GenerarConsulta(Periodo,"P");
		}

		private void GenerarConsulta(int Periodo, string TipoCierre)
		{   
			pnlConsulta.Visible=false;
			pnlAbrirPeriodo.Visible=false;
            cargarInformacion();
            
            DataSet dsCierre=new DataSet();
			DBFunctions.Request(dsCierre,IncludeSchema.NO,"CALL DBXSCHEMA.ACUMULADOS_TRANSPORTE("+Periodo+",'"+TipoCierre+"');");
			if(dsCierre.Tables[0].Rows.Count>0) 
			  lblError.Text=dsCierre.Tables[0].Rows[0]["MENSAJE"].ToString() + dsCierre.Tables[0].Rows[0]["CODIGO_SQL"].ToString();
			else
			{
				lblError.Text += "Error: " + DBFunctions.exceptions;
				return;
			}
			
			VerConsulta();
										
		}

		private void VerConsulta()
		{	
			DateTime fecha;
		
			DataSet dsPeriodo=new DataSet();
			DBFunctions.Request(dsPeriodo,IncludeSchema.NO,"SELECT FECHA_INICIO,FECHA_FINAL FROM DBXSCHEMA.PERIODOS_CIERRE_TRANSPORTE WHERE NUMERO_PERIODO = "+Periodo+";");
			fecha  = Convert.ToDateTime(dsPeriodo.Tables[0].Rows[0]["FECHA_INICIO"].ToString());
			txtFechaI.Text = fecha.ToString("yyyy-MM-dd");
			fecha  = Convert.ToDateTime(dsPeriodo.Tables[0].Rows[0]["FECHA_FINAL"].ToString());
			txtFechaF.Text = fecha.ToString("yyyy-MM-dd");
			
			DataSet dsConceptos=new DataSet();
			//DBFunctions.Request(dsConceptos,IncludeSchema.NO,"CALL DBXSCHEMA.CONSULTA_TOTAL_CONCEPTO("+Periodo+");");
            DBFunctions.Request(dsConceptos, IncludeSchema.NO, "CALL DBXSCHEMA.CONSULTA_VENTA_BUSES_MES(" + Periodo + ");");

			dgrConceptos.DataSource=dsConceptos.Tables[0].DefaultView;
			dgrConceptos.DataBind();
			pnlConsulta.Visible=true;
			pnlAbrirPeriodo.Visible=false;
	
		}

		private void dgrConceptos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				TotalIngresos+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[3].ToString());
				TotalEgresos+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[4].ToString());
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text=TotalIngresos.ToString("#,##0");
                e.Item.Cells[3].Text=TotalEgresos.ToString("#,##0");
			}
		}
	}
}
