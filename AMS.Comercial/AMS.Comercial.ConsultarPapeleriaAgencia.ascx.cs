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
	///	Descripción breve de AMS_Comercial_ConsultarPapeleria.
	/// </summary>
	public class AMS_Comercial_ConsultarPapeleriaAgencia : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlTipoDocumento;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtInicioDocumento;
		protected System.Web.UI.WebControls.TextBox txtFinDocumento;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlClaseDocumento;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlAsignados;
		protected System.Web.UI.WebControls.TextBox txtResponsable;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.DropDownList ddlPlanillados;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.DataGrid dgrPapeleria;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.TextBox txtPlanilla;
		protected System.Web.UI.WebControls.Panel pnlPapeleria;
		protected System.Web.UI.WebControls.TextBox TextFechaAsigI;
		protected System.Web.UI.WebControls.TextBox TextFechaAsigF;
		protected System.Web.UI.WebControls.TextBox TextFechaUsoI;
		protected System.Web.UI.WebControls.TextBox TextFechaUsoF;
		protected System.Web.UI.WebControls.TextBox TextFechaAnulI;
		protected System.Web.UI.WebControls.TextBox TextFechaAnulF;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		public string strActScript;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				
				//Agencias
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				//Agencias.TraerAgenciasPrefijoUsuario(ddlAgencia);
				
				//Tipos
				DataRow drC;
				DataSet dsTipos=new DataSet();
				DBFunctions.Request(dsTipos,IncludeSchema.NO,
					"select tdoc_codigo as valor,tdoc_nombre as texto from DBXSCHEMA.TDOCU_TRANS where PAPELERIA = 'S' ORDER BY tdoc_nombre;");
				drC=dsTipos.Tables[0].NewRow();
				
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
			DateTime fechaI=DateTime.Now;
			DateTime fechaF=DateTime.Now;
			//Validar

			long planilla=0;
			int agencia=Int16.Parse(ddlAgencia.SelectedValue.Trim().Replace("|",""));
			long desde=0,hasta=0;
			string responsable=txtResponsable.Text.Trim();
			if(txtInicioDocumento.Text.Trim().Length>0)
				try{desde=Convert.ToInt32(txtInicioDocumento.Text);}
                catch { Utils.MostrarAlerta(Response, "Número inicial de documento no válido."); return; }
			if(txtFinDocumento.Text.Trim().Length>0)
				try{hasta=Convert.ToInt32(txtFinDocumento.Text);}
				catch{
                    Utils.MostrarAlerta(Response, "Número final de documento no válido.");
                    return;}
			if(desde>hasta){Response.Write("<script language='javascript'>alert('Número inicial mayor a número final.');</script>");return;}
			if(txtPlanilla.Text.Trim().Length>0)
				try{planilla=Convert.ToInt64(txtPlanilla.Text);}
				catch{
                    Utils.MostrarAlerta(Response, "Número de planilla no válido.");
                    return;}
			long maxNoPrefijo=((long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))-1;
			// Consulta
			string sqlC= "SELECT TDOC_CODIGO, NUM_PARTE, NUM_DOCUMENTO,TIPO_DOCUMENTO,FECHA_RECEPCION,"+
				         "FECHA_DESPACHO, MAG_CODIGO,AGENCIA,FECHA_ASIGNACION, MNIT_RESPONSABLE, "+  
						 "coalesce(n.mnit_APELLIDOS,'') concat ' ' concat coalesce( n.mnit_APELLIDO2,'') concat ' '"+  
						 "concat n.mnit_NOMBRES concat ' ' concat coalesce(n.mnit_NOMBRE2 ,'') AS NOMBRE_RESPONSABLE,"+         
						 "FECHA_DEVOLUCION,FECHA_ANULACION,MPLA_CODIGO,FECHA_USO,MRUT_CODIGO,VALOR_DOCUMENTO "+
                         "FROM (SELECT CP.TDOC_CODIGO, cast(right(rtrim(char(CP.NUM_DOCUMENTO)),6) as integer) AS NUM_PARTE, "+
                         " case when (CP.NUM_DOCUMENTO>"+maxNoPrefijo+" AND CP.TIPO_DOCUMENTO='M') then cast(right(rtrim(char(CP.NUM_DOCUMENTO)),6) as integer) else CP.NUM_DOCUMENTO end AS NUM_DOCUMENTO,"+
				         " CP.TIPO_DOCUMENTO, CP.FECHA_RECEPCION, CP.FECHA_DESPACHO, MA.MAG_CODIGO AS MAG_CODIGO, MA.MAGE_NOMBRE AS AGENCIA,"+
				         "CP.FECHA_ASIGNACION, CP.MNIT_RESPONSABLE, CP.FECHA_DEVOLUCION, CP.FECHA_ANULACION, CP.MPLA_CODIGO,CP.FECHA_USO ";
			string sqltablas = "FROM DBXSCHEMA.MAGENCIA MA,DBXSCHEMA.MCONTROL_PAPELERIA CP  ";
			if(ddlTipoDocumento.SelectedValue == "ANT")
			{
				sqlC= sqlC + ", CP.MRUT_CODIGO, VALOR_TOTAL_AUTORIZADO as VALOR_DOCUMENTO " ;
				sqltablas = sqltablas + " LEFT JOIN DBXSCHEMA.MGASTOS_TRANSPORTES GT on CP.TDOC_CODIGO =   GT.TDOC_CODIGO AND CP.NUM_DOCUMENTO = GT.NUM_DOCUMENTO ";
			}
			else
			if(ddlTipoDocumento.SelectedValue == "TIQ")
			{
				sqlC= sqlC + ", T.MRUT_CODIGO, VALOR_TOTAL as VALOR_DOCUMENTO " ;
				sqltablas = sqltablas + " LEFT JOIN DBXSCHEMA.MTIQUETE_VIAJE T on CP.TDOC_CODIGO =   T.TDOC_CODIGO AND CP.NUM_DOCUMENTO = T.NUM_DOCUMENTO ";
				
			}
			else
				if(ddlTipoDocumento.SelectedValue == "PLA")
			{
				sqlC= sqlC + ", CP.MRUT_CODIGO,VALOR_INGRESOS - VALOR_EGRESOS as VALOR_DOCUMENTO " ;
				sqltablas = sqltablas + " LEFT JOIN DBXSCHEMA.MPLANILLAVIAJE P on CP.TDOC_CODIGO =   'PLA'  AND CP.NUM_DOCUMENTO = P.MPLA_CODIGO ";
			}
			else
				if(ddlTipoDocumento.SelectedValue == "GIR")
			{
				sqlC= sqlC + ", CP.MRUT_CODIGO, VALOR_IVA + COSTO_GIRO   as VALOR_DOCUMENTO " ;
				sqltablas = sqltablas + " LEFT JOIN DBXSCHEMA.MGIROS G on CP.TDOC_CODIGO = G.TDOC_CODIGO  AND CP.NUM_DOCUMENTO = G.NUM_DOCUMENTO ";
			}
			else
				if(ddlTipoDocumento.SelectedValue == "REM")
			{
				sqlC= sqlC + ", CP.MRUT_CODIGO,  VALOR_TOTAL as VALOR_DOCUMENTO " ;
				sqltablas = sqltablas + " LEFT JOIN DBXSCHEMA.MENCOMIENDAS E on CP.TDOC_CODIGO = E.TDOC_CODIGO  AND CP.NUM_DOCUMENTO = E.NUM_DOCUMENTO ";
			}
			string filtro="WHERE CP.MAG_CODIGO=MA.MAG_CODIGO  AND";
			if(ddlTipoDocumento.SelectedValue == "TIQ")
			{
				//Prefijo tiquetes
				string IndicativoPrefijo = DBFunctions.SingleData("Select PREFIJO from DBXSCHEMA.MAGENCIA where MAG_CODIGO = "+agencia+";");
				if(IndicativoPrefijo=="S")
				{
					 if(desde>0)
					 {
							//No. tiquete
							try
							{
								desde=(agencia*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+desde;}
                            catch { Utils.MostrarAlerta(Response, "Número Inicial de documento no válido.");return; }
					}
					if(hasta>0)
					{
							//No. tiquete
						try
						{
							hasta=(agencia*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+hasta;}
						catch{
                            Utils.MostrarAlerta(Response, "Número final de documento no válido.");
                            return;}
					}
				}
			}
			if(ddlTipoDocumento.SelectedValue.Length>0)filtro+="  CP.TDOC_CODIGO='"+ddlTipoDocumento.SelectedValue+"' AND";
			if(desde>0)filtro+="  CP.NUM_DOCUMENTO>="+desde+" AND";
			if(hasta>0)filtro+="  CP.NUM_DOCUMENTO<="+hasta+" AND";

			if(ddlClaseDocumento.SelectedValue.Length>0)filtro+="  CP.TIPO_DOCUMENTO='"+ddlClaseDocumento.SelectedValue+"' AND";
			
			if(!ddlAgencia.SelectedValue.Equals("0"))
				 filtro+="  CP.MAG_CODIGO="+agencia+"  AND";
			if(ddlAsignados.SelectedValue.Length>0){
				if(ddlAsignados.SelectedValue.Equals("S")){
					filtro+="  CP.FECHA_ASIGNACION IS NOT NULL AND";
					if(responsable.Length>0)filtro+="  CP.MNIT_RESPONSABLE='"+responsable+"' AND";}
				else filtro+="  CP.FECHA_ASIGNACION IS NULL AND";}
			
			if(TextFechaAsigI.Text.Trim().Length>0)
				try
				{
					fechaI=Convert.ToDateTime(TextFechaAsigI.Text);}
				catch                         
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Asignacion Inicial válida.");
					return;}
			if(TextFechaAsigF.Text.Trim().Length>0)
				try
				{
					fechaF=Convert.ToDateTime(TextFechaAsigF.Text);}
				catch                         
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Asignacion Inicial válida.");
					return;}
			if ((TextFechaAsigI.Text.Trim().Length>0) && (TextFechaAsigF.Text.Trim().Length==0)) filtro+="  CP.FECHA_ASIGNACION= '"+fechaI.ToString("yyyy-MM-dd")+"' AND"; 
			if ((TextFechaAsigI.Text.Trim().Length==0)&& (TextFechaAsigF.Text.Trim().Length>0)) filtro+="  CP.FECHA_ASIGNACION= '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
			if ((TextFechaAsigI.Text.Trim().Length>0)&& (TextFechaAsigF.Text.Trim().Length>0))
				if (fechaF >= fechaI) filtro+="  CP.FECHA_ASIGNACION >= '"+fechaI.ToString("yyyy-MM-dd")+"' AND  CP.FECHA_ASIGNACION <= '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
				else 
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Final < Fecha Inicial válida.");
					return;
				}
			/*
			if(ddlDevueltos.SelectedValue.Length>0){
				if(ddlDevueltos.SelectedValue.Equals("S")) filtro+=" TBLP.FECHA_DEVOLUCION IS NOT NULL AND";
				else filtro+="  CP..FECHA_DEVOLUCION IS NULL AND";}
			
			if(ddlAnulados.SelectedValue.Length>0){
				if(ddlAnulados.SelectedValue.Equals("S")) filtro+=" TBLP.FECHA_ANULACION IS NOT NULL AND";
				else filtro+=" TBLP.FECHA_ANULACION IS NULL AND";}
			*/
			if(ddlPlanillados.SelectedValue.Length>0){
				if(ddlPlanillados.SelectedValue.Equals("S")){
					filtro+=" cp.MPLA_CODIGO IS NOT NULL AND";
					if(planilla>0) filtro+=" cp.MPLA_CODIGO="+planilla+" AND";}
				else filtro+=" cp.MPLA_CODIGO IS NULL AND";}
			if(TextFechaUsoI.Text.Trim().Length>0)
				try
				{
					fechaI=Convert.ToDateTime(TextFechaUsoI.Text);}
				catch
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Uso Inicial válida.");
					return;}
			if(TextFechaUsoF.Text.Trim().Length>0)
				try
				{
					fechaF=Convert.ToDateTime(TextFechaUsoF.Text);}
				catch
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Asignacion Final válida.");
					return;}
			if ((TextFechaUsoI.Text.Trim().Length>0) && (TextFechaUsoF.Text.Trim().Length==0)) filtro+="  date(CP.FECHA_USO) = '"+fechaI.ToString("yyyy-MM-dd")+"' AND"; 
			if ((TextFechaUsoI.Text.Trim().Length==0)&& (TextFechaUsoF.Text.Trim().Length>0)) filtro+="  date(CP.FECHA_USO) = '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
			if ((TextFechaUsoI.Text.Trim().Length>0)&& (TextFechaUsoF.Text.Trim().Length>0))
				if (fechaF >= fechaI) filtro+="  date(CP.FECHA_USO) >= '"+fechaI.ToString("yyyy-MM-dd")+"' AND  date(CP.FECHA_USO) <= '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
				else 
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Final < Fecha Uso Inicial válida.");
					return;
				}
			if(TextFechaAnulI.Text.Trim().Length>0)
				try
				{
					fechaI=Convert.ToDateTime(TextFechaAnulI.Text);}
				catch
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Anulacion Inicial válida.");
					return;}
			if(TextFechaAnulF.Text.Trim().Length>0)
				try
				{
					fechaF=Convert.ToDateTime(TextFechaAnulF.Text);}
				catch
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Anulacion Final válida.");
					return;}
			if ((TextFechaAnulI.Text.Trim().Length>0) && (TextFechaAnulF.Text.Trim().Length==0)) filtro+="  date(CP.FECHA_ANULACION) = '"+fechaI.ToString("yyyy-MM-dd")+"' AND"; 
			if ((TextFechaAnulI.Text.Trim().Length==0)&& (TextFechaAnulF.Text.Trim().Length>0)) filtro+="  date(CP.FECHA_ANULACION) = '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
			if ((TextFechaAnulI.Text.Trim().Length>0) && (TextFechaAnulF.Text.Trim().Length>0))
				if (fechaF >= fechaI) filtro+="  date(CP.FECHA_ANULACION) >= '"+fechaI.ToString("yyyy-MM-dd")+"' AND  date(CP.FECHA_ANULACION) <= '"+fechaF.ToString("yyyy-MM-dd")+"' AND";
				else 
				{
                    Utils.MostrarAlerta(Response, "Debe dar una fecha Final < Fecha Anulacion Inicial válida.");
					return;
				}
			if(filtro.EndsWith("AND"))filtro=filtro.Substring(0,filtro.Length-4);
			
			sqlC+=sqltablas+filtro;

			string sqlContar="SELECT COUNT(*) FROM DBXSCHEMA.MAGENCIA MA,DBXSCHEMA.MCONTROL_PAPELERIA CP "+filtro;
			
			string totRegs=DBFunctions.SingleData(sqlContar);
			if (totRegs.Length ==0)
			{
                Utils.MostrarAlerta(Response, "NO se encontraron registros.");
				return;
			}
			if (Convert.ToInt64(totRegs)>3000)
			{
                Utils.MostrarAlerta(Response, "Se han encontrado " + totRegs + " registros, el límite de consulta es de 3000.\\r\\nUtilice los filtros para reducir el número de registros consultados.");
				return;
			}
						
			sqlC+=") DATOS_DOCUMENTOS LEFT JOIN DBXSCHEMA.MNIT N ON DATOS_DOCUMENTOS.MNIT_RESPONSABLE = N.MNIT_NIT ORDER BY NUM_DOCUMENTO;";
			ViewState["QUERY"]=sqlC;
			ViewState["PAGINA"] = 0;
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
