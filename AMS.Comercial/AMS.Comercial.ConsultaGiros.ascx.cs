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
	///		Descripción breve de AMS_Comercial_ConsultaGiros.
	/// </summary>
	public class AMS_Comercial_ConsultaGiros : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlTipo;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtInicioDocumento;
		protected System.Web.UI.WebControls.TextBox txtFinDocumento;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaO;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaD;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlEntregados;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.DataGrid dgrPapeleria;
		protected System.Web.UI.WebControls.Panel pnlPapeleria;
		protected System.Web.UI.WebControls.Label lblError;
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DataRow drC;
				//Agencias
				DataSet dsAgencias=new DataSet();
				DBFunctions.Request(dsAgencias,IncludeSchema.NO,
					"select rtrim(char(mag_codigo)) as valor,mage_nombre as texto from DBXSCHEMA.MAGENCIA ORDER BY mage_nombre;");
				drC=dsAgencias.Tables[0].NewRow();
				drC[0]="";
				drC[1]="Todas";
				dsAgencias.Tables[0].Rows.InsertAt(drC,0);
				ddlAgenciaD.DataSource=dsAgencias.Tables[0];
				ddlAgenciaD.DataTextField="texto";
				ddlAgenciaD.DataValueField="valor";
				ddlAgenciaD.DataBind();
				ddlAgenciaO.DataSource=dsAgencias.Tables[0];
				ddlAgenciaO.DataTextField="texto";
				ddlAgenciaO.DataValueField="valor";
				ddlAgenciaO.DataBind();
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

		private void dgrPapeleria_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			dgrPapeleria.CurrentPageIndex=e.NewPageIndex;
			DataSet dsPapeleria=new DataSet();
			DBFunctions.Request(dsPapeleria, IncludeSchema.NO,ViewState["QUERY"].ToString());
			dgrPapeleria.DataSource=dsPapeleria.Tables[0];
			dgrPapeleria.DataBind();
		}

		private void btnConsultar_Click(object sender, System.EventArgs e)
		{
			//Validar
			int desde=0,hasta=0;
			if(txtInicioDocumento.Text.Trim().Length>0)
				try{desde=Convert.ToInt32(txtInicioDocumento.Text);}
                catch { Utils.MostrarAlerta(Response, "Número inicial de documento no válido."); return; }
			if(txtFinDocumento.Text.Trim().Length>0)
				try{hasta=Convert.ToInt32(txtFinDocumento.Text);}
                catch { Utils.MostrarAlerta(Response, "Número final de documento no válido."); return; }
			if(desde>hasta){
                Utils.MostrarAlerta(Response, "Número inicial mayor a número final.");
                return;}
			long maxNoPrefijo=((long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))-1;
			//Consulta
            string sqlC = " FROM TABLE(SELECT CP.TIPO_DOCUMENTO, cast(right(rtrim(char(CP.NUM_DOCUMENTO)),6) as integer) AS NUM_PARTE, " +
                " cast(rtrim(char(cast(right(rtrim(char(CP.NUM_DOCUMENTO))," + Comercial.Tiquetes.lenTiquete + ") as integer)))AS INTEGER) AS NUM_DOCUMENTO, CP.MPLA_CODIGO, " +
                " MAO.MAG_CODIGO AS MAG_AGENCIA_ORIGEN, MAO.MAGE_NOMBRE AS AGENCIA_O, " +
                " MAD.MAG_CODIGO AS MAG_AGENCIA_DESTINO, MAD.MAGE_NOMBRE AS AGENCIA_D, " +
                " CP.MRUT_CODIGO," +
                " CP.MNIT_EMISOR,(coalesce(NE.mnit_APELLIDOS,'') concat coalesce( NE.mnit_APELLIDO2,'') concat ' ' " +
                " concat NE.mnit_NOMBRES concat ' ' concat coalesce(NE.mnit_NOMBRE2 ,'')) AS NOMBRE_EMISOR," +
                " CP.MNIT_DESTINATARIO,(coalesce(ND.mnit_APELLIDOS,'') concat coalesce( ND.mnit_APELLIDO2,'') concat ' ' " +
                " concat ND.mnit_NOMBRES concat ' ' concat coalesce(ND.mnit_NOMBRE2 ,'')) AS NOMBRE_DESTINATARIO, " +
                " CP.VALOR_IVA, CP.COSTO_GIRO, CP.VALOR_GIRO, CP.FECHA_RECIBE, CP.FECHA_ENTREGA " +
                " FROM DBXSCHEMA.MGIROS CP LEFT JOIN DBXSCHEMA.MAGENCIA MAO ON CP.MAG_AGENCIA_ORIGEN=MAO.MAG_CODIGO" +
                " LEFT JOIN DBXSCHEMA.MAGENCIA MAD ON CP.MAG_AGENCIA_DESTINO=MAD.MAG_CODIGO " +
                " LEFT JOIN DBXSCHEMA.MNIT NE ON  cp.MNIT_EMISOR = Ne.MNIT_NIT " +
                " LEFT JOIN DBXSCHEMA.MNIT ND ON  cp.MNIT_DESTINATARIO = Nd.MNIT_NIT ) AS TBLP ";
            string filtro = "";
			if(ddlTipo.SelectedValue.Length>0)filtro+=" TBLP.TIPO_DOCUMENTO='"+ddlTipo.SelectedValue+"' AND";
			if(desde>0)filtro+=" TBLP.NUM_PARTE>="+desde+" AND";
			if(hasta>0)filtro+=" TBLP.NUM_PARTE<="+hasta+" AND";
			if(ddlAgenciaO.SelectedValue.Length>0)filtro+=" TBLP.MAG_AGENCIA_ORIGEN="+ddlAgenciaO.SelectedValue+" AND";
			if(ddlAgenciaD.SelectedValue.Length>0)filtro+=" TBLP.MAG_AGENCIA_DESTINO="+ddlAgenciaD.SelectedValue+" AND";
			if(ddlEntregados.SelectedValue.Length>0)
			{
				if(ddlEntregados.SelectedValue.Equals("S")) filtro+=" TBLP.FECHA_ENTREGA IS NOT NULL AND";
				else filtro+=" TBLP.FECHA_ENTREGA IS NULL AND";}
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
			sqlC+=" ORDER BY TBLP.NUM_PARTE ASC, TBLP.FECHA_RECIBE DESC;";
			ViewState["QUERY"]=sqlC;

			//Consulta
			dgrPapeleria.CurrentPageIndex=0;
			DataSet dsPapeleria=new DataSet();
			DBFunctions.Request(dsPapeleria, IncludeSchema.NO,sqlC);
			dgrPapeleria.DataSource=dsPapeleria.Tables[0];
			dgrPapeleria.DataBind();
			pnlPapeleria.Visible=true;
		}
	}
}
