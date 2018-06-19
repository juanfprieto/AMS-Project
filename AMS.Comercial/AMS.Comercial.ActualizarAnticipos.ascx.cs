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
	///		Descripción breve de AMS_Comercial_ActualizarAnticipos.
	/// </summary>
	public class AMS_Comercial_ActualizarAnticipos : System.Web.UI.UserControl
	{
		
		protected System.Web.UI.WebControls.Label lbInfo;
		
		protected System.Web.UI.WebControls.Label lblPaginaActual;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.TextBox TextNumeroDocumento;
		protected System.Web.UI.WebControls.TextBox TextPlanilla;
		protected System.Web.UI.WebControls.Label lblResult;
		protected System.Web.UI.WebControls.TextBox TxtResponsable;
		protected System.Web.UI.WebControls.TextBox TxtPlaca;
		
		protected System.Web.UI.WebControls.Button btnBuscar;
		protected System.Web.UI.WebControls.Panel pnlAnticipos;
		protected System.Web.UI.WebControls.DataGrid dgrAnticipos;
		protected System.Web.UI.WebControls.DropDownList ddlConcepto;
		protected System.Web.UI.WebControls.TextBox TextFechaI;
		protected System.Web.UI.WebControls.TextBox TextFechaF;
		string sqlC;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{	
				if (Request.QueryString["Regresar"]==null)
				{
					//Agencias
					SeleccionarConsulta();
					
				}
				else
				
					if (Request.QueryString["Regresar"]== "1")
				{
					ViewState["PAGINA"]= Request.QueryString["pagina"];
					ViewState["QUERY"] = Request.QueryString["sql"];
					SeleccionarConsulta();
					MostarPagina();
					pnlAnticipos.Visible=true;
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
			this.btnBuscar.Click += new System.EventHandler(this.btnConsultar_Click);
			this.dgrAnticipos.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgrAnticipos_PageIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnConsultar_Click(object sender, System.EventArgs e)
		{
			
			//Validar
			DateTime fechaI=DateTime.Now;
			DateTime fechaF=DateTime.Now;
			int planilla=0,NumeroDocumento=0;
			int agencia=Int16.Parse(ddlAgencia.SelectedValue);
			string responsable=TxtResponsable.Text.Trim();
			string placa=TxtPlaca.Text.Trim();
			if(TextNumeroDocumento.Text.Trim().Length>0)
				try{NumeroDocumento=Convert.ToInt32(TextNumeroDocumento.Text);}
                catch { Utils.MostrarAlerta(Response, "Número de documento no válido."); return; }
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
			//Consulta
			sqlC="SELECT CP.NUM_DOCUMENTO  AS NUM_DOCUMENTO,CP.FECHA_DOCUMENTO AS FECHA_DOCUMENTO,  "+
				" MA.MAG_CODIGO AS CODIGO_AGENCIA, MA.MAGE_NOMBRE AS NOMBRE_AGENCIA," + 
				" CP.MCAT_PLACA,MBUS_NUMERO AS BUS,CP.MPLA_CODIGO,"+
				" CP.TCON_CODIGO AS CODIGO_CONCEPTO, GS.NOMBRE AS NOMBRE_CONCEPTO, "+
				" CP.MNIT_RESPONSABLE_RECIBE AS NIT_RESPONSABLE, CP.VALOR_TOTAL_AUTORIZADO as VALOR_GASTO "+
				"FROM DBXSCHEMA.MGASTOS_TRANSPORTES CP "+
				"LEFT JOIN DBXSCHEMA.MAGENCIA MA ON CP.MAG_CODIGO=MA.MAG_CODIGO "+
				"LEFT JOIN DBXSCHEMA.MBUSAFILIADO MB ON CP.MCAT_PLACA=MB.MCAT_PLACA "+
				"LEFT JOIN DBXSCHEMA.TCONCEPTOS_TRANSPORTES GS ON GS.TCON_CODIGO=CP.TCON_CODIGO ";
			filtro="WHERE CP.TDOC_codigo = 'ANT'";
			if(NumeroDocumento>0)filtro+=" AND CP.NUM_DOCUMENTO="+NumeroDocumento+" ";
			if(!ddlConcepto.SelectedValue.Equals("0"))filtro+=" AND CP.TCON_CODIGO ="+ddlConcepto.SelectedValue+" ";
			if(!ddlAgencia.SelectedValue.Equals("0"))filtro+=" AND CP.MAG_CODIGO="+ddlAgencia.SelectedValue+" ";
			else filtro+=" AND CP.MAG_CODIGO in (select sa.mag_codigo from DBXSCHEMA.susuario su, DBXSCHEMA.susuario_transporte_agencia sa "+ 
					 " where su.susu_login= '"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"' and su.susu_codigo=sa.susu_codigo) ";
			if(placa.Length>0)filtro+=" AND CP.MCAT_PLACA='"+placa+"' ";
			if ((TextFechaI.Text.Trim().Length>0)&& (TextFechaF.Text.Trim().Length==0)) filtro+=" AND CP.FECHA_DOCUMENTO= '"+fechaI.ToString("yyyy-MM-dd")+"' "; 
			if ((TextFechaI.Text.Trim().Length==0)&& (TextFechaF.Text.Trim().Length>0)) filtro+=" AND CP.FECHA_DOCUMENTO= '"+fechaF.ToString("yyyy-MM-dd")+"' ";
			if ((TextFechaI.Text.Trim().Length>0)&& (TextFechaF.Text.Trim().Length>0))
				if (fechaF >= fechaI) filtro+=" AND CP.FECHA_DOCUMENTO >= '"+fechaI.ToString("yyyy-MM-dd")+"' AND CP.FECHA_DOCUMENTO <= '"+fechaF.ToString("yyyy-MM-dd")+"' ";
				else 
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Final >= Fecha Inicial válida.");
					return;
				}
				
			if(TextPlanilla.Text.Trim().Length>0)filtro+=" AND CP.MPLA_CODIGO="+planilla+" ";
			if(responsable.Length>0)filtro+=" AND CP.MNIT_RESPONSABLE_RECIBE='"+responsable+"' ";
			//if(filtro.EndsWith("AND"))filtro=filtro.Substring(0,filtro.Length-4);
			
			sqlC+=filtro;
			/*
			string totRegs=DBFunctions.SingleData("SELECT COUNT(*) "+sqlC);
			if(Convert.ToInt64(totRegs)>1000)
			{
				Response.Write("<script language:javascript>alert('Se han encontrado "+totRegs+" registros, el límite de consulta es de 1000.\\r\\nUtilice los filtros para reducir el número de registros consultados.');</script>");
				return;
			}
			*/
			//sqlC="SELECT * "+sqlC;
			sqlC+=" ORDER BY cp.NUM_documento ASC, cp.FECHA_DOCUMENTO DESC;";
			ViewState["QUERY"]=sqlC;
			ViewState["PAGINA"] = 0;
			//Consulta
			dgrAnticipos.CurrentPageIndex=0;
			DataSet dsAnticipos=new DataSet();
			DBFunctions.Request(dsAnticipos, IncludeSchema.NO,sqlC);
			dgrAnticipos.DataSource=dsAnticipos.Tables[0];
			dgrAnticipos.DataBind();
			pnlAnticipos.Visible=true;
			
		}
		private void dgrAnticipos_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			ViewState["PAGINA"]= e.NewPageIndex;
			MostarPagina();
		}
		// public int PageCount() { int UltimaPagina = PageCount; }
		private void MostarPagina()
		{
			dgrAnticipos.CurrentPageIndex=  Convert.ToInt16(ViewState["PAGINA"]);
			DataSet dsAnticipos=new DataSet();
			DBFunctions.Request(dsAnticipos, IncludeSchema.NO,ViewState["QUERY"].ToString());
			dgrAnticipos.DataSource=dsAnticipos.Tables[0];
			dgrAnticipos.DataBind();
		}

		
		//Editar, copiar, eliminar, imprimir
		protected void dgActualizarAnticipo(object sender, DataGridCommandEventArgs e) 
		{
			//string campos=ViewState["CAMPOS"].ToString();
			int ind=e.Item.DataSetIndex;

			#region Editar
			
			if ((e.CommandName == "Actualizar") || (e.CommandName == "Borrar") ||(e.CommandName == "Imprimir") )
			{
				DataSet dsAnticipos=new DataSet();
				DBFunctions.Request(dsAnticipos, IncludeSchema.NO,ViewState["QUERY"].ToString());			  		
				int documento =Convert.ToInt32(dsAnticipos.Tables[0].Rows[ind]["NUM_DOCUMENTO"]);
				string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
				
				
				//if(Request.QueryString["processTitle"] != null)
				//	processTitle = Request.QueryString["processTitle"];
				
				Response.Redirect(indexPage+ "?process=Comercial.IngresosGastos" + "&path="+Request.QueryString["path"]+ "&comando=" +e.CommandName + "&Documento=" +documento + "&sql="+ViewState["QUERY"] + "&pagina="+ViewState["PAGINA"]);
				
			}
			#endregion Editar
		}
		
		private  void SeleccionarConsulta()
		{
			//Agencias
			
			Agencias.TraerAgenciasUsuario(ddlAgencia);
			ddlAgencia.Items.Insert(0,new ListItem("Agencias Asignadas","0"));
 
			//Conceptos
			DataRow drConcepto;
			DataSet dsConceptos=new DataSet();
			DBFunctions.Request(dsConceptos,IncludeSchema.NO,
				"Select TCON_CODIGO AS VALOR,NOMBRE AS TEXTO from DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE TDOC_CODIGO = 'ANT';");
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

}


