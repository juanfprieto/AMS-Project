namespace AMS.Documentos
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
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Documentos_Retenciones.
	/// </summary>
	public partial class Retenciones : System.Web.UI.UserControl
	{
		public DataTable tablaRtns;
		public string nit="";
		public string tipoFactura="";

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
            if (!Page.IsPostBack && ViewState["TABLERETS"]==null)
			{
				Cargar_Tabla_Rtns();
				ViewState["TABLERETS"]=tablaRtns;
				ViewState["TABLERETSNIT"]=nit;
				ViewState["TABLERETSTIPOFAC"]=tipoFactura;
			}
			else
            {
				if(ViewState["TABLERETS"]!=null)
					tablaRtns=(DataTable)ViewState["TABLERETS"];
				if(ViewState["TABLERETSNIT"]!=null)
					nit=ViewState["TABLERETSNIT"].ToString();
				if(ViewState["TABLERETSTIPOFAC"]!=null)
					tipoFactura=ViewState["TABLERETSTIPOFAC"].ToString();
                if (ConfigurationManager.AppSettings["ModificaRetenciones"] != null && !Convert.ToBoolean(ConfigurationManager.AppSettings["ModificaRetenciones"]))
                    gridRtns.Columns[gridRtns.Columns.Count - 1].Visible = false;
			}
		}

		protected void Cargar_Tabla_Rtns()
		{
			tablaRtns=new DataTable();
			tablaRtns.Columns.Add(new DataColumn("CODRET", typeof(string)));
			tablaRtns.Columns.Add(new DataColumn("PORCRET", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALOR", typeof(double)));
            tablaRtns.Columns.Add(new DataColumn("NOMBRERET", typeof(string)));
		}

		public void PrecargarRetenciones(string nt, string tipoF, ArrayList arV, ArrayList arT, ArrayList arB)
		{
			string campoPorcentaje="";
			nit=nt;
			tipoFactura=tipoF;
			Cargar_Tabla_Rtns();
			Retencion rtns=new Retencion(nt,false);
			if(rtns.TipoSociedad=="N" || rtns.TipoSociedad=="U")
				campoPorcentaje="PRET_PORCENNODECL";
			else 
				campoPorcentaje="PRET_PORCENDECL";

			for(int n=0;n<arV.Count;n++)
			{
				DataRow drR=tablaRtns.NewRow();
				drR["CODRET"]    = arT[n].ToString();
				drR["PORCRET"]   = Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE("+campoPorcentaje+",0) FROM PRETENCION WHERE PRET_CODIGO='"+arT[n].ToString()+"';"));
				drR["VALOR"]     = Convert.ToDouble(arV[n]);
                drR["VALORBASE"] = Convert.ToDouble(arB[n]); //(Convert.ToDouble(drR["VALOR"]) * 100) / Convert.ToDouble(drR["PORCRET"]);
                drR["NOMBRERET"] = DBFunctions.SingleData("SELECT PRET_NOMBRE FROM PRETENCION WHERE PRET_CODIGO='" + arT[n].ToString() + "';");
                tablaRtns.Rows.Add(drR);
			}
			ViewState["TABLERETS"]=tablaRtns;
			ViewState["TABLERETSNIT"]=nit;
			ViewState["TABLERETSTIPOFAC"]=tipoFactura;
            Bind();
		}

		protected void gridRtns_Item(object Sender,DataGridCommandEventArgs e)
		{
			DataRow fila;
            tablaRtns = (DataTable)ViewState["TABLERETS"];
			if(((Button)e.CommandSource).CommandName=="AgregarRetencion")
			{
				if((((TextBox)e.Item.Cells[0].Controls[1]).Text==""))
                    Utils.MostrarAlerta(Response, "Debe escoger un tipo de retención");
				else if(this.Buscar_Retencion(((TextBox)e.Item.Cells[0].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Solo puede haber una retención de ese tipo");
				else if(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
				else
				{
					fila=tablaRtns.NewRow();
					fila["CODRET"]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila["PORCRET"]=Convert.ToDouble(((TextBox)e.Item.Cells[1].Controls[1]).Text);
					fila["VALORBASE"]=Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
					fila["VALOR"]=Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
					tablaRtns.Rows.Add(fila);
					Bind();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverRetencion")
			{
				tablaRtns.Rows[e.Item.DataSetIndex].Delete();
				Bind();
			}
            ViewState["TABLERETS"] = tablaRtns;
		}

		public void Bind()
		{
			gridRtns.DataSource=tablaRtns;
            if (ConfigurationManager.AppSettings["ModificaRetenciones"] != null && !Convert.ToBoolean(ConfigurationManager.AppSettings["ModificaRetenciones"]))
                gridRtns.Columns[gridRtns.Columns.Count - 1].Visible = false;
			gridRtns.DataBind();
            if (ConfigurationManager.AppSettings["ModificaRetenciones"] != null && !Convert.ToBoolean(ConfigurationManager.AppSettings["ModificaRetenciones"]))
                gridRtns.Columns[gridRtns.Columns.Count - 1].Visible = false;
            Response.Write(((DataTable)ViewState["TABLERETS"]).Rows.Count);
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

				if(tipoFactura=="FC")
					((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pret_codigo,pret_nombre,pret_porcennodecl FROM pretencion ORDER BY tret_codigo',new Array());"+scrTotal);
				else if(tipoFactura=="FP")
				{
					Retencion rtns=new Retencion(nit,false);
					if(rtns.TipoSociedad=="N" || rtns.TipoSociedad=="U")
						((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcennodecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr where pr.ttip_codigo IN (\\'N\\',\\'T\\') ORDER BY tipo;',new Array());"+scrTotal);
					else 
						((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcendecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr where pr.ttip_codigo IN (\\'J\\',\\'T\\') ORDER BY tipo;',new Array());"+scrTotal);
				}
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
	}
}
