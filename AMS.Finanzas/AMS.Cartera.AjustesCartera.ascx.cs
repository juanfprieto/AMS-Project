namespace AMS.Finanzas
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Documentos;
    using AMS.Tools;

	/// <summary>
    ///		Descripción breve de AMS_Cartera_AjustesCartera.
	/// </summary>
    public partial class AMS_Cartera_AjustesCartera : System.Web.UI.UserControl
	{
		#region Controles
		private string tabla="",prefi="",nume="",nit="";
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		public DataTable tablaRtns;
		#endregion Controles

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!Page.IsPostBack)
			{
				Cargar_Tabla_Rtns();
				ViewState["TABLERETS"]=tablaRtns;
				if(Request.QueryString["act"]!=null)
				
                Utils.MostrarAlerta(Response, "Las retenciones han sido modificadas");
                if (Request.QueryString["pref"] != null && Request.QueryString["num"] != null)
                {
                    btnVolver.Visible = true;
                    rblTipo.SelectedIndex = rblTipo.Items.IndexOf(rblTipo.Items.FindByValue("P"));
                    rblTipo_SelectedIndexChanged(sender, e);
                    btnSeleccionar_Click(sender, e);
                }
                else
                {
                    btnVolver.Visible = false;
                }
                if (ConfigurationManager.AppSettings["ModificaRetenciones"] != null && !Convert.ToBoolean(ConfigurationManager.AppSettings["ModificaRetenciones"]))
                    gridRtns.Columns[gridRtns.Columns.Count - 2].Visible = gridRtns.Columns[gridRtns.Columns.Count - 1].Visible = false;
			}
			if(ViewState["TABLA"]!=null)
				tabla=ViewState["TABLA"].ToString();
			if(ViewState["PREFIJO"]!=null)
				prefi=ViewState["PREFIJO"].ToString();
			if(ViewState["NUMERO"]!=null)
				nume=ViewState["NUMERO"].ToString();
			if(ViewState["NIT"]!=null)
				nit=ViewState["NIT"].ToString();
			if(ViewState["TABLERETS"]!=null)
				tablaRtns=(DataTable)ViewState["TABLERETS"];
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

		protected void Cargar_Tabla_Rtns()
		{
			tablaRtns=new DataTable();
			tablaRtns.Columns.Add(new DataColumn("CODRET", typeof(string)));
			tablaRtns.Columns.Add(new DataColumn("NOMBRE", typeof(string)));
			tablaRtns.Columns.Add(new DataColumn("PORCRET", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALOR", typeof(double)));
		}

		protected void rblTipo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(rblTipo.SelectedValue=="C")
			{
				tabla="CLIENTE";
				prefi="PDOC_CODIGO";
				nume="MFAC_NUMEDOCU";
			}
			else 
			{
				tabla="PROVEEDOR";
				prefi="PDOC_CODIORDEPAGO";
				nume="MFAC_NUMEORDEPAGO";
			}
			ViewState["TABLA"]=tabla;
			ViewState["PREFIJO"]=prefi;
			ViewState["NUMERO"]=nume;
			DatasToControls bind = new DatasToControls();
            if (Request.QueryString["pref"] != null && Request.QueryString["num"] != null)
                bind.PutDatasIntoDropDownList(ddlPrefijo,
                "SELECT DISTINCT(MFCR." + prefi + ") " +
                "FROM MFACTURA" + tabla + "RETENCION MFCR, MFACTURA" + tabla + " MFC " +
                "WHERE MFCR." + prefi + "=MFC." + prefi + " AND MFCR." + nume + "=MFC." + nume + " AND " +
                "MFCR." + prefi + "='" + Request.QueryString["pref"] + "' AND " +
                "TVIG_VIGENCIA NOT IN ('P', 'C') order by MFCR." + prefi + ";");
            else
			    bind.PutDatasIntoDropDownList(ddlPrefijo,
				    "SELECT DISTINCT(MFCR."+prefi+") "+
				    "FROM MFACTURA"+tabla+"RETENCION MFCR, MFACTURA"+tabla+" MFC "+
				    "WHERE MFCR."+prefi+"=MFC."+prefi+" AND MFCR."+nume+"=MFC."+nume+" AND "+
				    "TVIG_VIGENCIA NOT IN ('P', 'C') order by MFCR."+prefi+";");
			CargarNumeros();
			rblTipo.Enabled=false;
			pnlDocumento.Visible=true;
		}

		private void CargarNumeros()
		{
			DatasToControls bind = new DatasToControls();
			/*bind.PutDatasIntoDropDownList(ddlNumero,
				"SELECT distinct(MFCR."+nume+") "+
				"FROM MFACTURA"+tabla+"RETENCION MFCR, MFACTURA"+tabla+" MFC "+
				"WHERE MFCR."+prefi+"=MFC."+prefi+" AND MFCR."+nume+"=MFC."+nume+" AND TVIG_VIGENCIA NOT IN ('P', 'C') AND "+
				"MFCR."+prefi+"='"+ddlPrefijo.SelectedValue+"' order by MFCR."+nume+";");*/
            if (Request.QueryString["pref"] != null && Request.QueryString["num"] != null)
                bind.PutDatasIntoDropDownList(ddlNumero,
                "SELECT MFC." + nume + " " +
                "FROM MFACTURA" + tabla + " MFC " +
                "WHERE MFC." + prefi + "='" + ddlPrefijo.SelectedValue + "' AND "+
                "MFC." + nume + " = " + Request.QueryString["num"] + " "+
                "order by MFC." + nume + ";");
            else
                bind.PutDatasIntoDropDownList(ddlNumero,
				    "SELECT MFC."+nume+" "+
				    "FROM MFACTURA"+tabla+" MFC "+
				    "WHERE MFC."+prefi+"='"+ddlPrefijo.SelectedValue+"' order by MFC."+nume+";");
		}

        protected void btnVolver_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("" + indexPage + "?process=Inventarios.RecepcionItems&path=Recepcin de Items&prR=" + Request.QueryString["prR"] + "&pref=" + Request.QueryString["pref"] + "&num=" + Request.QueryString["num"]);
        }

        protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			DataSet dsRetenciones=new DataSet();
			string campoPorcentaje="";
			btnSeleccionar.Visible=false;
			ddlPrefijo.Enabled    =false;
			ddlNumero.Enabled     =false;
			pnlRetenciones.Visible=true;
			nit=DBFunctions.SingleData("SELECT MNIT_NIT FROM MFACTURA"+tabla+" mf "+
				"WHERE mf."+prefi+"='"+ddlPrefijo.SelectedValue+"' AND mf."+nume+"="+ddlNumero.SelectedValue+";");
			ViewState["NIT"]=nit;
			Retencion rtns=new Retencion(nit,false);
			if(rtns.TipoSociedad=="N" || rtns.TipoSociedad=="U")
				campoPorcentaje="pret_porcennodecl";
			else 
				campoPorcentaje="pret_porcendecl";
			string strSql=
				"SELECT PR.PRET_CODIGO CODRET, PR.PRET_NOMBRE NOMBRE, PR."+campoPorcentaje+" PORCRET, "+
                "mfcr.MFAC_VALORETE VALOR, mfcr.MFAC_VALOBASE VALORBASE, " +
				"TRN.TRET_NOMBRE, TTP.TTIP_NOMBRE TTIP_PROCESO, TPP.TTIP_NOMBRE  "+
				"FROM MFACTURA"+tabla+"RETENCION MFCR, MFACTURA"+tabla+" MFC, PRETENCION PR, "+
				"TRETENCION TRN, TTIPOPERSONA TPP, TTIPOPROCESO TTP "+
				"WHERE MFCR."+prefi+"=MFC."+prefi+" AND MFCR."+nume+"=MFC."+nume+" AND "+
				"TRN.TRET_CODIGO=PR.TRET_CODIGO AND TPP.TTIP_CODIGO=PR.TTIP_CODIGO AND TTP.TTIP_CODIGO=PR.TTIP_PROCESO AND "+
				"MFCR."+prefi+"='"+ddlPrefijo.SelectedValue+"' AND MFCR."+nume+"="+ddlNumero.SelectedValue+" AND "+
				"PR.PRET_CODIGO=MFCR.PRET_CODIGO;";
			
			DBFunctions.Request(dsRetenciones,IncludeSchema.NO,strSql);
            if (dsRetenciones.Tables.Count > 0)
            {
                tablaRtns = dsRetenciones.Tables[0];
                ViewState["TABLERETS"] = tablaRtns;
                Bind();
            }
		}
		protected void gridRtns_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				TextBox txtBase=(TextBox)e.Item.FindControl("base");
				TextBox txtPorcentaje=(TextBox)e.Item.FindControl("codretb");
				TextBox txtValor=(TextBox)e.Item.FindControl("valor");
				string scrTotal="PorcentajeVal('"+txtPorcentaje.ClientID+"','"+txtBase.ClientID+"','"+txtValor.ClientID+"');";
				txtBase.Attributes.Add("onkeyup","NumericMaskE(this,event);"+scrTotal);

				if(rblTipo.SelectedValue=="C")
					((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pret_codigo,pret_nombre,pret_porcennodecl FROM pretencion ORDER BY tret_codigo',new Array());"+scrTotal);
				else if(rblTipo.SelectedValue=="P")
				{
					Retencion rtns=new Retencion(nit,false);
					if(rtns.TipoSociedad=="N" || rtns.TipoSociedad=="U")
						((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcennodecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr where pr.ttip_codigo IN (\\'N\\',\\'T\\') ORDER BY tipo;',new Array());"+scrTotal);
					else 
						((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcendecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr where pr.ttip_codigo IN (\\'J\\',\\'T\\') ORDER BY tipo;',new Array());"+scrTotal);
				}
			}
		}
		protected void gridRtns_Item(object Sender,DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="AgregarRetencion")
			{
				if((((TextBox)e.Item.Cells[0].Controls[1]).Text==""))
                    Utils.MostrarAlerta(Response, "Debe escoger un tipo de retención");
				else if(this.Buscar_Retencion(((TextBox)e.Item.Cells[0].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Solo puede haber una retención de ese tipo");
				else if(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
				else
				{
					fila=tablaRtns.NewRow();
					fila["CODRET"]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila["NOMBRE"]=DBFunctions.SingleData("SELECT PRET_NOMBRE FROM PRETENCION WHERE PRET_CODIGO='"+fila["CODRET"].ToString()+"';");
					fila["TRET_NOMBRE"]=DBFunctions.SingleData("SELECT TR.TRET_NOMBRE FROM PRETENCION PR, TRETENCION TR WHERE TR.TRET_CODIGO=PR.TRET_CODIGO AND PR.PRET_CODIGO='"+fila["CODRET"].ToString()+"';");
					fila["TTIP_PROCESO"]=DBFunctions.SingleData("SELECT TP.TTIP_NOMBRE FROM PRETENCION PR, TTIPOPROCESO TP WHERE PR.TTIP_PROCESO=TP.TTIP_CODIGO AND PR.PRET_CODIGO='"+fila["CODRET"].ToString()+"';");
					fila["TTIP_NOMBRE"]=DBFunctions.SingleData("SELECT TP.TTIP_NOMBRE FROM PRETENCION PR, TTIPOPERSONA TP WHERE PR.TTIP_CODIGO=TP.TTIP_CODIGO AND PR.PRET_CODIGO='"+fila["CODRET"].ToString()+"';");

					fila["PORCRET"]=Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);
					fila["VALORBASE"]=Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
					fila["VALOR"]=Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
					tablaRtns.Rows.Add(fila);
					Bind();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverRetencion")
			{
				tablaRtns.Rows.RemoveAt(e.Item.DataSetIndex);
				Bind();
			}
		}
		protected bool Buscar_Retencion(string rtn)
		{
			bool encontrado=false;
			if(tablaRtns!=null && tablaRtns.Rows.Count!=0)
			{
				for(int i=0;i<tablaRtns.Rows.Count;i++)
				{
					if(tablaRtns.Rows[i][0].ToString()==rtn)
						encontrado=true;
				}
			}
			return encontrado;
		}
		public void Bind()
		{
			ViewState["TABLERETS"]=tablaRtns;
			gridRtns.DataSource=tablaRtns;
            if (ConfigurationManager.AppSettings["ModificaRetenciones"] != null && !Convert.ToBoolean(ConfigurationManager.AppSettings["ModificaRetenciones"]))
                gridRtns.Columns[gridRtns.Columns.Count - 2].Visible = gridRtns.Columns[gridRtns.Columns.Count - 1].Visible = false;
			gridRtns.DataBind();
            if (ConfigurationManager.AppSettings["ModificaRetenciones"] != null && !Convert.ToBoolean(ConfigurationManager.AppSettings["ModificaRetenciones"]))
                gridRtns.Columns[gridRtns.Columns.Count - 2].Visible = gridRtns.Columns[gridRtns.Columns.Count - 1].Visible = false;
		}

		protected void ddlPrefijo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CargarNumeros();
		}

		public void dgrItems_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			gridRtns.EditItemIndex=-1;
			Bind();
		}

		public void dgrItems_Edit(Object sender, DataGridCommandEventArgs e)
		{
			if(tablaRtns.Rows.Count>0)
				gridRtns.EditItemIndex=(int)e.Item.ItemIndex;
			Bind();
		}

		public void dgrItems_Update(Object sender, DataGridCommandEventArgs e)
		{
			double valorBase;
			try
			{
				valorBase=Convert.ToDouble(((TextBox)e.Item.FindControl("txtEdV")).Text.Replace(",",""));
				if(valorBase<=0)throw(new Exception());
				tablaRtns.Rows[e.Item.ItemIndex]["VALORBASE"]=valorBase;
				tablaRtns.Rows[e.Item.ItemIndex]["VALOR"]=Math.Round(valorBase*Convert.ToDouble(tablaRtns.Rows[e.Item.ItemIndex]["PORCRET"])/100,0);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Valor base no válido.");
			}
			ViewState["TABLERETS"]=tablaRtns;
			gridRtns.EditItemIndex=-1;
			Bind();
		}

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			
			ArrayList arlSql=new ArrayList();
			double totalR=0;
			//Eliminar actuales
			arlSql.Add("DELETE FROM MFACTURA"+tabla+"RETENCION "+
			"WHERE "+prefi+"='"+ddlPrefijo.SelectedValue+"' AND "+nume+"="+ddlNumero.SelectedValue+";");
			
			for(int n=0;n<tablaRtns.Rows.Count;n++)
			{
				arlSql.Add("INSERT INTO MFACTURA"+tabla+"RETENCION "+
					"VALUES('"+ddlPrefijo.SelectedValue+"',"+ddlNumero.SelectedValue+","+
					"'"+tablaRtns.Rows[n]["CODRET"].ToString()+"',"+
                    tablaRtns.Rows[n]["VALOR"] + "," + tablaRtns.Rows[n]["VALORBASE"] + ");");
				totalR+=Convert.ToDouble(tablaRtns.Rows[n]["VALOR"]);
			}
			arlSql.Add("UPDATE MFACTURA"+tabla+" SET MFAC_VALORETE="+totalR+" "+
				"WHERE "+prefi+"='"+ddlPrefijo.SelectedValue+"' AND "+nume+"="+ddlNumero.SelectedValue+";");
			if(DBFunctions.Transaction(arlSql)){
                if (Request.QueryString["pref"] != null && Request.QueryString["num"] != null)
                    Response.Redirect("" + indexPage + "?process=Inventarios.RecepcionItems&path=Recepcin de Items&mret=1&prR=" + Request.QueryString["prR"] + "&pref=" + Request.QueryString["pref"] + "&num=" + Request.QueryString["num"]);
                else
                    Response.Redirect(indexPage + "?process=Cartera.ModificacionRetenciones&act=1");
			}
			else lblError.Text=DBFunctions.exceptions;
		}
	}
}
