namespace AMS.Comercial
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Microsoft.Web.UI.WebControls;
	using AMS.DB;
	using AMS.Forms;
	using Ajax;
	using AMS.DBManager;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_ConsultaViajes.
	/// </summary>
	public class AMS_Comercial_ConsultaViajes : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtRuta;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.DataGrid dgrPapeleria;
		protected System.Web.UI.WebControls.Panel pnlPapeleria;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.Panel pnlRuta;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaS;
		protected System.Web.UI.WebControls.Label lblError;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlAgenciaS,"select mag_codigo,mage_nombre from DBXSCHEMA.magencia;");
				ddlAgenciaS.Items.Add(new ListItem("--todas--",""));
				if(ddlAgencia.Items.Count>0)
					ddlAgencia_SelectedIndexChanged(sender,e);
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
			this.ddlAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlAgencia_SelectedIndexChanged);
			this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			pnlRuta.Visible=false;
			pnlPapeleria.Visible=false;
			if(agencia.Length==0)return;
			pnlRuta.Visible=true;
			CargarRutasPrincipales(agencia);
		}
		
		private void CargarRutasPrincipales(string agencia)
		{
			DataSet dsRutasP=new DataSet();
			string ciudad=DBFunctions.SingleData("SELECT pciu_codigo from dbxschema.magencia where mag_codigo="+agencia+";");
			DBFunctions.Request(dsRutasP,IncludeSchema.NO,
				"select distinct mr.mrut_codigo as valor, "+
				"'[' concat mr.mrut_codigo concat '] ' concat pco.pciu_nombre concat ' - ' concat pcd.pciu_nombre as texto "+
				"from DBXSCHEMA.mrutas mr, DBXSCHEMA.pciudad pco, DBXSCHEMA.pciudad pcd "+
				"where mr.pciu_coddes=pcd.pciu_codigo and mr.pciu_cod=pco.pciu_codigo and mr.mrut_clase=2 and "+
				"  (mr.pciu_cod='"+ciudad+"' "+
				"  or mr.mrut_codigo in( "+
				"   select mrap.mrut_codigo from DBXSCHEMA.MRUTAS mrap, DBXSCHEMA.MRUTAS mras, DBXSCHEMA.MRUTA_INTERMEDIA mri, DBXSCHEMA.PCIUDAD pci "+
				"   WHERE mras.pciu_cod='"+ciudad+"' and mri.mruta_secundaria=mras.mrut_codigo and mri.mruta_principal=mrap.mrut_codigo and pci.pciu_codigo=mrap.pciu_cod "+
				" )"+
				") order by valor");
			DataRow drS=dsRutasP.Tables[0].NewRow();
			drS[0]="";
			drS[1]="---seleccione---";
			dsRutasP.Tables[0].Rows.InsertAt(drS,0);
			ddlRuta.DataSource=dsRutasP.Tables[0];
			ddlRuta.DataTextField="texto";
			ddlRuta.DataValueField="valor";
			ddlRuta.DataBind();
			txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
		}

		private void btnConsultar_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			string agencia=ddlAgencia.SelectedValue, ruta=ddlRuta.SelectedValue;
			DateTime fecha;
			if(ruta.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar la ruta principal.");
				return;}
			try{
				fecha=Convert.ToDateTime(txtFecha.Text);
			}
			catch{
                Utils.MostrarAlerta(Response, "Fecha no valida.");
				return;}
			
			DataSet dsViajes=new DataSet();
			DBFunctions.Request(dsViajes,IncludeSchema.NO,
				"SELECT V.MAG_CODIGO as CODIGO,MA.MAGE_NOMBRE AS AGENCIA,P.MPLA_CODIGO AS PLANILLA,P.MVIAJE_NUMERO AS VIAJE,"+
				"B.MBUS_NUMERO AS BUS,V.MCAT_PLACA AS PLACA,V.MRUT_CODIGO AS RUTA_PRINCIPAL,"+
	            "CASE WHEN P.FECHA_LIQUIDACION IS NULL THEN 'No' ELSE 'Si' END AS DESPACHADO,fecha_salida,"+
				"rtrim(char(floor(v.hora_salida/60))) concat ':'   concat case when (mod(v.hora_salida,60)<10) then '0' else '' end concat "+  
				"rtrim(char(mod(v.hora_salida,60))) as HORA_PROGRAMADA, "+
				"coalesce(rtrim(char(floor(v.hora_despacho/60))) concat ':'   concat case when (mod(v.hora_despacho,60)<10) then '0' else '' end concat "+   
				"rtrim(char(mod(v.hora_despacho,60))),' ') as HORA_DESPACHO "+    			 
				"from DBXSCHEMA.MplanillaVIAJE p,DBXSCHEMA.MVIAJE V,DBXSCHEMA.MBUSAFILIADO B,DBXSCHEMA.MAGENCIA MA "+
				"WHERE p.FECHA_planilla = '"+fecha.ToString("yyyy-MM-dd")+"' AND "+ 
				"P.MRUT_CODIGO   = '"+ruta+"' AND "+
				"P.MRUT_CODIGO   = V.MRUT_CODIGO  AND "+
			    "P.MVIAJE_NUMERO = V.MVIAJE_NUMERO AND "+
				"V.mcat_placa    = B.MCAT_PLACA    AND "+
			    "v.MAG_CODIGO    = MA.MAG_CODIGO;");  
		
			dgrPapeleria.DataSource=dsViajes.Tables[0];
			dgrPapeleria.DataBind();
			pnlPapeleria.Visible=true;
		}
	}
}
