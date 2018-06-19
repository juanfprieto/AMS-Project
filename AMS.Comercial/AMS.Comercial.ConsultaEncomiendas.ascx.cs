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
	///	Descripción breve de AMS_Comercial_ConsultaEncomiendas.
	/// </summary>
	public class AMS_Comercial_ConsultaEncomiendas : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlTipo;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtInicioDocumento;
		protected System.Web.UI.WebControls.TextBox txtFinDocumento;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaO;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaD;
		protected System.Web.UI.WebControls.DropDownList ddlEntregados;
		protected System.Web.UI.WebControls.Label Labe13;
		protected System.Web.UI.WebControls.TextBox TextFechaI;
		protected System.Web.UI.WebControls.TextBox TextFechaF;
		protected System.Web.UI.WebControls.Label Labe44;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Labe21;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.DataGrid dgrPapeleria;
		protected System.Web.UI.WebControls.Panel pnlPapeleria;
		protected System.Web.UI.WebControls.TextBox TxtPlaca;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox TextPlanilla;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.DropDownList ddlRutas;
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
					"select rtrim(char(mag_codigo)) as valor,mage_nombre concat ' [' concat  rtrim(char(mag_codigo)) concat ']' as texto from DBXSCHEMA.MAGENCIA ORDER BY mage_nombre;");
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
               // Rutas ---- 
				
				DataSet dsRutasP=new DataSet();
				DBFunctions.Request(dsRutasP,IncludeSchema.NO,
					"select mr.mrut_codigo as valor, "+
					"mr.mrut_codigo concat ' -- ' concat pco.pciu_nombre concat ' - ' concat pcd.pciu_nombre as texto "+
					"from DBXSCHEMA.mrutas mr, DBXSCHEMA.pciudad pco, DBXSCHEMA.pciudad pcd "+
					"where mr.pciu_coddes=pcd.pciu_codigo and mr.pciu_cod=pco.pciu_codigo order by valor;");
					
				DataRow drR;
				drR=dsRutasP.Tables[0].NewRow();
				drR[0]="";
				drR[1]="Todas";
				dsRutasP.Tables[0].Rows.InsertAt(drR,0);
				ddlRutas.DataSource=dsRutasP.Tables[0];
				ddlRutas.DataTextField="texto";
				ddlRutas.DataValueField="valor";
				ddlRutas.DataBind();
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
			string placa=TxtPlaca.Text.Trim(),sqlC,sqlContar;
			DateTime fechaI=DateTime.Now;
			DateTime fechaF=DateTime.Now;
			int planilla=0;
			//Validar
			int desde=0,hasta=0;
			if(txtInicioDocumento.Text.Trim().Length>0)
				try{desde=Convert.ToInt32(txtInicioDocumento.Text);}
				catch{
                    Utils.MostrarAlerta(Response, "Número inicial de documento no válido.");
                    return;}
			if(txtFinDocumento.Text.Trim().Length>0)
				try{hasta=Convert.ToInt32(txtFinDocumento.Text);}
                catch { Utils.MostrarAlerta(Response, "Número final de documento no válido."); return; }
            if (desde > hasta) { Utils.MostrarAlerta(Response, "Número inicial mayor a número final."); return; }

			if(TextFechaI.Text.Trim().Length>0)
				try
				{
					fechaI=Convert.ToDateTime(TextFechaI.Text);}
				catch
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Inicial válida.");
					return;}
			if(TextFechaF.Text.Trim().Length>0)
				try
				{
					fechaF=Convert.ToDateTime(TextFechaF.Text);}
				catch
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Final válida.");
					return;}

			if(TextPlanilla.Text.Trim().Length>0)
				try{planilla=Convert.ToInt32(TextPlanilla.Text);}
                catch { Utils.MostrarAlerta(Response, "Número de planilla no válido."); return; }
			
			string filtro="";
						
			if(ddlTipo.SelectedValue.Length>0)filtro+=" CP.TIPO_REMESA='"+ddlTipo.SelectedValue+"' AND";
			if(desde>0)filtro+=" CP.NUM_DOCUMENTO>="+desde+" AND";
			if(hasta>0)filtro+=" CP.NUM_DOCUMENTO<="+hasta+" AND";
			if(ddlAgenciaO.SelectedValue.Length>0)filtro+=" CP.MAG_RECIBE="+ddlAgenciaO.SelectedValue+" AND";
			if(ddlAgenciaD.SelectedValue.Length>0)filtro+=" CP.MAG_ENTREGA="+ddlAgenciaD.SelectedValue+" AND";
							
			if(placa.Length>0)filtro+=" CP.MCAT_PLACA='"+placa+"' AND";
			if ((TextFechaI.Text.Trim().Length>0)&& (TextFechaF.Text.Trim().Length==0)) filtro+=" CP.FECHA_RECIBE= '"+fechaI.ToString("yyyy-MM-dd")+"' AND"; 
			if ((TextFechaI.Text.Trim().Length==0)&& (TextFechaF.Text.Trim().Length>0)) filtro+=" CP.FECHA_RECIBE= '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
			if ((TextFechaI.Text.Trim().Length>0)&& (TextFechaF.Text.Trim().Length>0))
				if (fechaF >= fechaI) filtro+=" CP.FECHA_RECIBE >= '"+fechaI.ToString("yyyy-MM-dd")+"' AND CP.FECHA_RECIBE <= '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
				else 
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Final < Fecha Inicial válida.");
					return;
				}
			if(ddlRutas.SelectedValue.Length>0)filtro+=" CP.MRUT_CODIGO='"+ddlRutas.SelectedValue+"' AND";
			if(TextPlanilla.Text.Trim().Length>0)filtro+=" CP.MPLA_CODIGO="+planilla+" AND";
	
			if(ddlEntregados.SelectedValue.Length>0)
			{
				if(ddlEntregados.SelectedValue.Equals("S")) filtro+=" CP.FECHA_ENTREGA IS NOT NULL AND";
				else filtro+=" CP.FECHA_ENTREGA IS NULL AND";}
			if(filtro.EndsWith("AND"))filtro=filtro.Substring(0,filtro.Length-4);
			//if(filtro.Length>0)filtro=" WHERE "+filtro;

			sqlContar="SELECT COUNT(*) FROM DBXSCHEMA.MENCOMIENDAS CP WHERE "+filtro;
			string totRegs=DBFunctions.SingleData(sqlContar);
				
			if(Convert.ToInt64(totRegs)>2000)
			{
                Utils.MostrarAlerta(Response, "Se han encontrado " + totRegs + " registros, el límite de consulta es de 2000.\\r\\nUtilice los filtros para reducir el número de registros consultados.");
				return;
			}
            sqlC = "SELECT NUM_DOCUMENTO AS NUM_DOCUMENTO,TIPO_REMESA as TIPO_REMESA,MRUT_CODIGO," +
                " MAO.MAGE_NOMBRE AS AGENCIA_O,  MAD.MAGE_NOMBRE AS AGENCIA_D,MPLA_CODIGO,MCAT_PLACA," +
                " MNIT_RESPONSABLE_RECIBE,(coalesce(NR.mnit_APELLIDOS,'') concat coalesce( NR.mnit_APELLIDO2,'') concat ' ' " +
                " concat NR.mnit_NOMBRES concat ' ' concat coalesce(NR.mnit_NOMBRE2 ,'')) AS NOMBRE_RESPONSABLE," +
                " MNIT_EMISOR, (coalesce(NE.mPAS_APELLIDOS,'') concat coalesce( NE.mPAS_APELLIDO2,'') concat ' ' " +
                " concat NE.mPAS_NOMBRES concat ' ' concat coalesce(NE.mPAS_NOMBRE2 ,'')) AS NOMBRE_EMISOR," +
                " Ne.MPAS_TELEFONO as TEL_EMISOR , MNIT_DESTINATARIO, (coalesce(ND.mPAS_APELLIDOS,'') concat coalesce( ND.mPAS_APELLIDO2,'') concat ' ' " +
                " concat ND.mPAS_NOMBRES concat ' ' concat coalesce(ND.mPAS_NOMBRE2 ,'')) AS NOMBRE_DESTINATARIO," +
                "  Nd.MPAS_TELEFONO AS TEL_DESTINATARIO, FECHA_USO as FECHA_RECIBE,FECHA_ENTREGA,VALOR_IVA,COSTO_ENCOMIENDA, VALOR_TOTAL" +
                " FROM (SELECT  CP.NUM_DOCUMENTO AS NUM_DOCUMENTO,CP.TIPO_REMESA as TIPO_REMESA,CP.MRUT_CODIGO," +
                " CP.MAG_RECIBE,  CP.MAG_ENTREGA,CP.MPLA_CODIGO,CP.MCAT_PLACA,cp.MNIT_RESPONSABLE_RECIBE, CP.MNIT_EMISOR," +
                " CP.MNIT_DESTINATARIO,FECHA_USO,CP.FECHA_ENTREGA,CP.VALOR_IVA, CP.COSTO_ENCOMIENDA, CP.VALOR_TOTAL, DESCRIPCION_CONTENIDO " +
                " FROM DBXSCHEMA.MENCOMIENDAS CP,DBXSCHEMA.MCONTROL_PAPELERIA Mcp " +
                "WHERE   CP.TDOC_CODIGO = Mcp.TDOC_CODIGO AND CP.NUM_DOCUMENTO = Mcp.NUM_DOCUMENTO AND  " + filtro;
            sqlC += ") AS TBLP LEFT JOIN DBXSCHEMA.MNIT NR ON  TBLP.MNIT_RESPONSABLE_RECIBE = NR.MNIT_NIT " +
                   " LEFT JOIN DBXSCHEMA.MPASAJERO NE ON  TBLP.MNIT_EMISOR = Ne.MPAS_NIT " +
                   " LEFT JOIN DBXSCHEMA.MPASAJERO ND ON  TBLP.MNIT_DESTINATARIO = Nd.MPAS_NIT " +
                   " LEFT JOIN DBXSCHEMA.MAGENCIA MAO ON TBLP.MAG_RECIBE=MAO.MAG_CODIGO " +
                   " LEFT JOIN DBXSCHEMA.MAGENCIA MAD ON TBLP.MAG_ENTREGA=MAD.MAG_CODIGO " +
                   " ORDER BY TBLP.NUM_DOCUMENTO ASC, TBLP.FECHA_USO DESC;";
            ViewState["QUERY"] = sqlC;

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
