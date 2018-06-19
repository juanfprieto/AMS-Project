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
	///		Descripción breve de AMS_Comercial_ConsultarPapeleria.
	/// </summary>
	public class AMS_Comercial_ConsultarPapeleria : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlTipoDocumento;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtInicioDocumento;
		protected System.Web.UI.WebControls.TextBox txtFinDocumento;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlClaseDocumento;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlDespachados;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlAsignados;
		protected System.Web.UI.WebControls.TextBox txtResponsable;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.DropDownList ddlDevueltos;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.DropDownList ddlAnulados;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.DropDownList ddlPlanillados;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.DropDownList ddlUsados;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.DataGrid dgrPapeleria;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.TextBox txtPlanilla;
		protected System.Web.UI.WebControls.Panel pnlPapeleria;
		public string strActScript;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				
				//Agencias
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				ddlAgencia.Items.Insert(0,new ListItem("Todas","0"));
				//Tipos
				DataRow drC;
				DataSet dsTipos=new DataSet();
				DBFunctions.Request(dsTipos,IncludeSchema.NO,
					"select tdoc_codigo as valor,tdoc_nombre as texto from DBXSCHEMA.TDOCU_TRANS ORDER BY tdoc_nombre;");
				drC=dsTipos.Tables[0].NewRow();
				drC[0]="";
				drC[1]="Todos";
				dsTipos.Tables[0].Rows.InsertAt(drC,0);
				ddlTipoDocumento.DataSource=dsTipos.Tables[0];
				ddlTipoDocumento.DataTextField="texto";
				ddlTipoDocumento.DataValueField="valor";
				ddlTipoDocumento.DataBind();
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
			this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
			this.dgrPapeleria.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgrPapeleria_PageIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnConsultar_Click(object sender, System.EventArgs e)
		{
			//Validar
			int desde=0,hasta=0,planilla=0,agencia=Int16.Parse(ddlAgencia.SelectedValue);
			string responsable=txtResponsable.Text.Trim();
			if(txtInicioDocumento.Text.Trim().Length>0)
				try{desde=Convert.ToInt32(txtInicioDocumento.Text);}
                catch { Utils.MostrarAlerta(Response, "Número inicial de documento no válido."); return; }
			if(txtFinDocumento.Text.Trim().Length>0)
				try{hasta=Convert.ToInt32(txtFinDocumento.Text);}
				catch{
                    Utils.MostrarAlerta(Response, "Número final de documento no válido.");
                    return;}
			if(desde>hasta){
                Utils.MostrarAlerta(Response, "Número inicial mayor a número final.");
                return;}
			if(txtPlanilla.Text.Trim().Length>0)
				try{planilla=Convert.ToInt32(txtPlanilla.Text);}
				catch{
                    Utils.MostrarAlerta(Response, "Número de planilla no válido.");
                    return;}
			long maxNoPrefijo=((long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))-1;
			//Consulta
			string sqlC=" FROM TABLE(SELECT CP.TDOC_CODIGO, cast(right(rtrim(char(CP.NUM_DOCUMENTO)),7) as integer) AS NUM_PARTE, "+
				" rtrim(char(cast(right(rtrim(char(CP.NUM_DOCUMENTO)),"+ (Comercial.Tiquetes.lenTiquete+1) +") as integer))) concat case when (CP.NUM_DOCUMENTO>"+maxNoPrefijo+" AND CP.TIPO_DOCUMENTO='V') then '*' else '' end AS NUM_DOCUMENTO,"+
				" CP.TIPO_DOCUMENTO, CP.FECHA_RECEPCION, CP.FECHA_DESPACHO, MA.MAG_CODIGO AS MAG_CODIGO, MA.MAGE_NOMBRE AS AGENCIA, CP.FECHA_ASIGNACION, CP.MNIT_RESPONSABLE, CP.FECHA_DEVOLUCION, CP.FECHA_ANULACION, CP.MPLA_CODIGO,CP.FECHA_USO, CP.MRUT_CODIGO "+
				"FROM DBXSCHEMA.MCONTROL_PAPELERIA CP LEFT JOIN DBXSCHEMA.MAGENCIA MA ON CP.MAG_CODIGO=MA.MAG_CODIGO) AS TBLP ";
			string filtro="";
			if(ddlTipoDocumento.SelectedValue.Length>0)filtro+=" TBLP.TDOC_CODIGO='"+ddlTipoDocumento.SelectedValue+"' AND";
			if(desde>0)filtro+=" TBLP.NUM_PARTE>="+desde+" AND";
            if (hasta > 0) filtro += " TBLP.NUM_DOCUMENTO<=" + hasta + " AND";  //se cambia NUM_PARTE por NUM_DOCUMENTO pra afinar la condicion.
			if(ddlClaseDocumento.SelectedValue.Length>0)
			{
				if(ddlClaseDocumento.SelectedValue.Equals("M"))
					filtro+=" TBLP.TIPO_DOCUMENTO='"+ddlClaseDocumento.SelectedValue+"' AND";
				else filtro+=" TBLP.TIPO_DOCUMENTO not in 'M' AND";
			}
			if(ddlDespachados.SelectedValue.Length>0){
				if(ddlDespachados.SelectedValue.Equals("S")) filtro+=" TBLP.FECHA_DESPACHO IS NOT NULL AND";
				else filtro+=" TBLP.FECHA_DESPACHO IS NULL AND";}
			if(!ddlAgencia.SelectedValue.Equals("0"))
				 filtro+=" (TBLP.MAG_CODIGO="+ddlAgencia.SelectedValue+" OR TBLP.MAG_CODIGO IS NULL) AND";
			if(ddlAsignados.SelectedValue.Length>0){
				if(ddlAsignados.SelectedValue.Equals("S")){
					filtro+=" TBLP.FECHA_ASIGNACION IS NOT NULL AND";
					if(responsable.Length>0)filtro+=" TBLP.MNIT_RESPONSABLE='"+responsable+"' AND";}
				else filtro+=" TBLP.FECHA_ASIGNACION IS NULL AND";}
			if(ddlDevueltos.SelectedValue.Length>0){
				if(ddlDevueltos.SelectedValue.Equals("S")) filtro+=" TBLP.FECHA_DEVOLUCION IS NOT NULL AND";
				else filtro+=" TBLP.FECHA_DEVOLUCION IS NULL AND";}
			if(ddlAnulados.SelectedValue.Length>0){
				if(ddlAnulados.SelectedValue.Equals("S")) filtro+=" TBLP.FECHA_ANULACION IS NOT NULL AND";
				else filtro+=" TBLP.FECHA_ANULACION IS NULL AND";}
			if(ddlPlanillados.SelectedValue.Length>0){
				if(ddlPlanillados.SelectedValue.Equals("S")){
					filtro+=" TBLP.MPLA_CODIGO IS NOT NULL AND";
					if(planilla>0) filtro+=" TBLP.MPLA_CODIGO="+planilla+" AND";}
				else filtro+=" TBLP.MPLA_CODIGO IS NULL AND";}
			if(ddlUsados.SelectedValue.Length>0){
				if(ddlUsados.SelectedValue.Equals("S")) filtro+=" TBLP.FECHA_USO IS NOT NULL AND";
				else filtro+=" TBLP.FECHA_USO IS NULL AND";}
			if(filtro.EndsWith("AND"))filtro=filtro.Substring(0,filtro.Length-4);
			if(filtro.Length>0)filtro=" WHERE "+filtro;
			sqlC+=filtro;

			string totRegs=DBFunctions.SingleData("SELECT COUNT(*) "+sqlC);
			if(Convert.ToInt64(totRegs)>1000)
			{
                Utils.MostrarAlerta(Response, "Se han encontrado " + totRegs + " registros, el límite de consulta es de 1000.\\r\\nUtilice los filtros para reducir el número de registros consultados.");
				return;
			}
			sqlC="SELECT * "+sqlC;
			sqlC+=" ORDER BY TBLP.NUM_PARTE;";
			ViewState["QUERY"]=sqlC;

			//Consulta
			dgrPapeleria.CurrentPageIndex=0;
			DataSet dsPapeleria=new DataSet();
			DBFunctions.Request(dsPapeleria, IncludeSchema.NO,sqlC);
			dgrPapeleria.DataSource=dsPapeleria.Tables[0];
			dgrPapeleria.DataBind();
			pnlPapeleria.Visible=true;
		}

		private void dgrPapeleria_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			dgrPapeleria.CurrentPageIndex=e.NewPageIndex;
			DataSet dsPapeleria=new DataSet();
			DBFunctions.Request(dsPapeleria, IncludeSchema.NO,ViewState["QUERY"].ToString());
			dgrPapeleria.DataSource=dsPapeleria.Tables[0];
			dgrPapeleria.DataBind();
		}
	}
}
