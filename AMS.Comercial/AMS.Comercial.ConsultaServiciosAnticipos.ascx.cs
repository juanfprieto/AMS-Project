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
	///		Descripción breve de AMS_Comercial_ConsultaServiciosAnticipos.
	/// </summary>
	public class AMS_Comercial_ConsultaServiciosAnticipos : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlTipo;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtInicioDocumento;
		protected System.Web.UI.WebControls.TextBox txtFinDocumento;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox TextFechaI;
		protected System.Web.UI.WebControls.Label Labe44;
		protected System.Web.UI.WebControls.TextBox TextFechaF;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox TxtPlaca;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox TextPlanilla;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.DataGrid dgrAnticipos;
		protected System.Web.UI.WebControls.DropDownList ddlConcepto;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Panel pnlAnticipos;
		protected System.Web.UI.WebControls.Label lblError;
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
			  /*	
				DataRow drC;
				//Agencias
				DataSet dsAgencias=new DataSet();
				DBFunctions.Request(dsAgencias,IncludeSchema.NO,
					"select rtrim(char(mag_codigo)) as valor,mage_nombre as texto from DBXSCHEMA.MAGENCIA ORDER BY mage_nombre;");
				drC=dsAgencias.Tables[0].NewRow();
				drC[0]="";
				drC[1]="Todas";
				dsAgencias.Tables[0].Rows.InsertAt(drC,0);
				ddlAgencia.DataSource=dsAgencias.Tables[0];
				ddlAgencia.DataTextField="texto";
				ddlAgencia.DataValueField="valor";
				ddlAgencia.DataBind();
			 */
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				ddlAgencia.Items.Insert(0,new ListItem("Todas","0"));
 
				//Conceptos
				DataRow drConcepto;
				DataSet dsConceptos=new DataSet();
				DBFunctions.Request(dsConceptos,IncludeSchema.NO,
					"Select TCON_CODIGO AS VALOR,NOMBRE concat ' [' concat  rtrim(char(TCON_CODIGO)) concat ']'  AS TEXTO from DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE TDOC_CODIGO = 'ANT' ORDER BY NOMBRE;");
				drConcepto=dsConceptos.Tables[0].NewRow();
				drConcepto[0]=0;
				drConcepto[1]="Todos";
				dsConceptos.Tables[0].Rows.InsertAt(drConcepto,0);
				ddlConcepto.DataSource=dsConceptos.Tables[0];
				ddlConcepto.DataTextField="texto";
				ddlConcepto.DataValueField="valor";
				ddlConcepto.DataBind();
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

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
                catch { Utils.MostrarAlerta(Response, "Número inicial de documento no válido."); return; }
			if(txtFinDocumento.Text.Trim().Length>0)
				try{hasta=Convert.ToInt32(txtFinDocumento.Text);}
                catch { Utils.MostrarAlerta(Response, "Número final de documento no válido."); return; }
			if(desde>hasta){Response.Write("<script language='javascript'>alert('Número inicial mayor a número final.');</script>");return;}

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
						
			if(ddlTipo.SelectedValue.Length>0)filtro+=" CP.TIPO_DOCUMENTO ='"+ddlTipo.SelectedValue+"' AND";
			if(desde>0)filtro+=" CP.NUM_DOCUMENTO>="+desde+" AND";
			if(hasta>0)filtro+=" CP.NUM_DOCUMENTO<="+hasta+" AND";
			if(ddlAgencia.SelectedValue.Length>0)filtro+=" CP.MAG_CODIGO="+ddlAgencia.SelectedValue+" AND";
			if(!ddlConcepto.SelectedValue.Equals("0"))filtro+=" CP.TCON_CODIGO ="+ddlConcepto.SelectedValue+" AND";
			if(placa.Length>0)filtro+=" CP.MCAT_PLACA='"+placa+"' AND";
			if ((TextFechaI.Text.Trim().Length>0)&& (TextFechaF.Text.Trim().Length==0)) filtro+=" CP.FECHA_DOCUMENTO= '"+fechaI.ToString("yyyy-MM-dd")+"' AND"; 
			if ((TextFechaI.Text.Trim().Length==0)&& (TextFechaF.Text.Trim().Length>0)) filtro+=" CP.FECHA_DOCUMENTO= '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
			if ((TextFechaI.Text.Trim().Length>0)&& (TextFechaF.Text.Trim().Length>0))
				if (fechaF >= fechaI) filtro+=" CP.FECHA_DOCUMENTO >= '"+fechaI.ToString("yyyy-MM-dd")+"' AND CP.FECHA_DOCUMENTO <= '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
				else 
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Final < Fecha Inicial válida.");
					return;
				}

			if(TextPlanilla.Text.Trim().Length>0)filtro+=" CP.MPLA_CODIGO="+planilla+" AND";
				
			if(filtro.EndsWith("AND"))filtro=filtro.Substring(0,filtro.Length-4);
			 
			sqlContar="SELECT COUNT(*) FROM DBXSCHEMA.MGASTOS_TRANSPORTES CP WHERE "+filtro;
			string totRegs=DBFunctions.SingleData(sqlContar);
				
			if(Convert.ToInt64(totRegs)>2000)
			{
                Utils.MostrarAlerta(Response, "Se han encontrado " + totRegs + " registros, el límite de consulta es de 2000.\\r\\nUtilice los filtros para reducir el número de registros consultados.");
				return;
			}

			sqlC="SELECT NUM_DOCUMENTO AS NUM_DOCUMENTO,TIPO_DOCUMENTO,FECHA_USO as FECHA_DOCUMENTO,"+
				" CODIGO_AGENCIA,MA.MAGE_NOMBRE AS NOMBRE_AGENCIA,MPLA_CODIGO,TBLP.MCAT_PLACA,MBUS_NUMERO AS BUS,NIT_RESPONSABLE,"+ 
				" (coalesce(NR.mnit_APELLIDOS,'') concat coalesce( NR.mnit_APELLIDO2,'') concat ' ' "+ 
				" concat NR.mnit_NOMBRES concat ' ' concat coalesce(NR.mnit_NOMBRE2 ,'')) AS NOMBRE_RESPONSABLE,"+
				" MNIT_RESPONSABLE_RECIBE,TBLP.TCON_CODIGO AS CODIGO_CONCEPTO,GS.NOMBRE AS NOMBRE_CONCEPTO,VALOR_GASTO"+
				" FROM (SELECT  CP.NUM_DOCUMENTO AS NUM_DOCUMENTO,CP.TIPO_DOCUMENTO,CP.TCON_CODIGO,CP.MAG_CODIGO AS CODIGO_AGENCIA,"+
				" CP.MPLA_CODIGO,CP.MCAT_PLACA,MNIT_RESPONSABLE_ENTREGA AS NIT_RESPONSABLE,cp.MNIT_RESPONSABLE_RECIBE, "+
				" FECHA_USO,CP.VALOR_TOTAL_AUTORIZADO AS VALOR_GASTO "+
				" FROM DBXSCHEMA.MGASTOS_TRANSPORTES CP,DBXSCHEMA.MCONTROL_PAPELERIA Mcp "+
				"WHERE   CP.TDOC_CODIGO = Mcp.TDOC_CODIGO AND CP.NUM_DOCUMENTO = Mcp.NUM_DOCUMENTO AND  "+filtro;
			sqlC+= ") AS TBLP LEFT JOIN DBXSCHEMA.MNIT NR ON  TBLP.NIT_RESPONSABLE = NR.MNIT_NIT "+
				" LEFT JOIN DBXSCHEMA.MAGENCIA MA ON TBLP.CODIGO_AGENCIA=MA.MAG_CODIGO LEFT JOIN DBXSCHEMA.TCONCEPTOS_TRANSPORTES GS ON GS.TCON_CODIGO=TBLP.TCON_CODIGO"+	
				" LEFT JOIN DBXSCHEMA.MBUSAFILIADO B ON TBLP.MCAT_PLACA=B.MCAT_PLACA ORDER BY TBLP.NUM_DOCUMENTO ASC, TBLP.FECHA_USO DESC;";
			
			ViewState["QUERY"]=sqlC;
						
			//Consulta
			dgrAnticipos.CurrentPageIndex=0;
			DataSet dsAnticipos=new DataSet();
			DBFunctions.Request(dsAnticipos, IncludeSchema.NO,sqlC);
			dgrAnticipos.DataSource=dsAnticipos.Tables[0];
			dgrAnticipos.DataBind();
			pnlAnticipos.Visible=true;
		}

		private void dgrPapeleria_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			dgrAnticipos.CurrentPageIndex=e.NewPageIndex;
			DataSet dsAnticipos=new DataSet();
			DBFunctions.Request(dsAnticipos, IncludeSchema.NO,ViewState["QUERY"].ToString());
			dgrAnticipos.DataSource=dsAnticipos.Tables[0];
			dgrAnticipos.DataBind();
		}
	}
}
