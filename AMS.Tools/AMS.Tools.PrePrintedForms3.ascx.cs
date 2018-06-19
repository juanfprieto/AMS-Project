using System;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{
	public partial class BuildFooter : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataTable dtCampos;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				//LLenamos el combo de las tablas
				dtCampos = (DataTable)Session["dtCampos"];
				Bind_dgCampos();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlTablas,"SELECT STB.staf_nombretabla , STB.staf_nombretabla CONCAT ' - ' CONCAT COALESCE(SYT.remarks,'Tabla No Comentada') FROM stablaasociadafactura STB, sysibm.systables SYT WHERE STB.staf_nombretabla = SYT.name AND STB.stfc_codigo='"+Request.QueryString["TipEsp"]+"' AND STB.tpar_codigo='F'");
				ddlTablas.Items.Add(new ListItem("Sumatorias",""));
				bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"'");
				Agregar_Columnas_Especiales(ddlCampos,ddlTablas.SelectedValue);
				Preparar_ddlAlineacion();
			}
			else
			{
				if(Session["dtCampos"]!=null)
					dtCampos = (DataTable)Session["dtCampos"];
				else
					Preparar_dtCampos();
			}
		}
		
		protected void Cambio_ddlTablas(Object  Sender, EventArgs e)
		{
			if(ddlTablas.SelectedValue!="")
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"'");
			}
			else
				this.Agregar_Sumatorias();
		}
		
		protected void Configurar_Campo(Object  Sender, EventArgs e)
		{
			Estado_ddls(false);
			//Debo Mostrar el placeholder 
			plcConf.Visible = true;
			string tipoDato = "";
			if(ddlTablas.SelectedValue!="")
				tipoDato = DBFunctions.SingleData("SELECT COLTYPE FROM sysibm.syscolumns WHERE name='"+ddlCampos.SelectedValue+"' AND tbname='"+ddlTablas.SelectedValue+"'").Trim();
			else
				tipoDato = "DOUBLE";
			if(tipoDato == "")
				tipoDato = DBFunctions.SingleData("SELECT scle_tipodato FROM scolumnaespecial WHERE scle_columna='"+ddlCampos.SelectedValue+"' AND staf_nombretabla='"+ddlTablas.SelectedValue+"' AND stfc_codigo='"+Request.QueryString["TipEsp"]+"'");
			if(tipoDato == "VARCHAR" || tipoDato == "CHARACTER" || tipoDato == "LONG VARCHAR" || tipoDato == "CHAR")
			{
				lbTpDat.Text = "Cadena de Caracteres";
				Preparar_ddlMascara(0);
			}
			else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
			{
				lbTpDat.Text = "Valor Númerico";
				Preparar_ddlMascara(1);
			}
			else if(tipoDato == "DATE")
			{
				lbTpDat.Text = "Fecha";
				Preparar_ddlMascara(2);
			}
			else if(tipoDato == "TIME")
			{
				lbTpDat.Text = "Hora";
				Preparar_ddlMascara(0);
			}
		}
		
		protected void Aceptar_Configuracion(Object  Sender, EventArgs e)
		{
			DataRow fila = dtCampos.NewRow();
			if(ddlTablas.SelectedValue!="")
			{
				fila[0] = ddlTablas.SelectedValue;				
				fila[2] = ddlCampos.SelectedValue;
			}
			else
			{
				string[] sep = ddlCampos.SelectedValue.Split('-');
				fila[0] = sep[0];
				fila[2] = sep[1];
			}
			fila[1] = Request.QueryString["TipEsp"];
			fila[3] = System.Convert.ToDouble(posX.Text);
			fila[4] = System.Convert.ToDouble(posY.Text);
			fila[5] = ddlMascara.SelectedValue;
			fila[8] = "SR";
			fila[9] = "F";
			fila[10] = etiqCamp.Text;
			fila[11] = ddlAlineacion.SelectedValue;
			dtCampos.Rows.Add(fila);
			Bind_dgCampos();
			Clear_Config();
			Estado_ddls(true);
		}
		
		protected void Cancelar_Configuracion(Object  Sender, EventArgs e)
		{
			Clear_Config();
			Estado_ddls(true);
		}
		
		protected void Guardar_Formato(Object  Sender, EventArgs e)
		{
			if(nomFormato.Text!="")
			{
				FormatoFactura miFormato = new FormatoFactura(dtCampos);
				miFormato.DescripcionFormato = nomFormato.Text;
				miFormato.TipoFactura = Request.QueryString["TipEsp"];
				miFormato.PosIDX = Request.QueryString["posIDX"];
				miFormato.PosIDY = Request.QueryString["posIDY"];
				miFormato.PosFDX = Request.QueryString["posFDX"];
				miFormato.PosFDY = Request.QueryString["posFDY"];
				if(miFormato.Grabar_Formato()!=-1)
					Response.Redirect("" + indexPage + "?process=Tools.PrePrintedForms");
				else
					lb.Text += miFormato.ProcessMsg;
			}
			else
            Utils.MostrarAlerta(Response, "Por favor digite un nombre para el formato creado");
		}
		
		public void EliminarCampo(Object sender, DataGridCommandEventArgs e)
		{
			try
            {
        		dtCampos.Rows.Remove(dtCampos.Rows[e.Item.ItemIndex]);
	            dgCampos.EditItemIndex = -1;
			}catch(Exception ex){lb.Text += "<br>"+ex.ToString();};
			Bind_dgCampos();
		}
		#endregion
		
		#region Otros
		
		protected void Agregar_Columnas_Especiales(DropDownList ddlCamposEsp, string nombreTabla)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT scle_columna, scle_etiqueta FROM scolumnaespecial WHERE staf_nombretabla='"+nombreTabla+"' AND tpar_codigo='F' AND stfc_codigo='"+Request.QueryString["TipEsp"]+"'");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				ddlCamposEsp.Items.Add(new ListItem(ds.Tables[0].Rows[i][1].ToString(),ds.Tables[0].Rows[i][0].ToString()));
		}
		
		protected void Agregar_Sumatorias()
		{
			ddlCampos.Items.Clear();
			DataRow[] selection = dtCampos.Select("SUMATORIA='S'");
			for(int i=0;i<selection.Length;i++)
				ddlCampos.Items.Add(new ListItem("Sumatoria de "+selection[i][2].ToString(),selection[i][0].ToString()+"-"+selection[i][2].ToString()));
		}
		
		protected void Preparar_dtCampos()
		{
			dtCampos = new DataTable();
			dtCampos.Columns.Add(new DataColumn("TABLA",System.Type.GetType("System.String")));//0
			dtCampos.Columns.Add(new DataColumn("DOCUMENTO",System.Type.GetType("System.String")));//1
			dtCampos.Columns.Add(new DataColumn("CAMPO",System.Type.GetType("System.String")));//2
			dtCampos.Columns.Add(new DataColumn("POSX",System.Type.GetType("System.Double")));//3
			dtCampos.Columns.Add(new DataColumn("POSY",System.Type.GetType("System.Double")));//4
			dtCampos.Columns.Add(new DataColumn("MASCARA",System.Type.GetType("System.String")));//5
			dtCampos.Columns.Add(new DataColumn("ORDENCOLUMNA",System.Type.GetType("System.Int32")));//6
			dtCampos.Columns.Add(new DataColumn("PORCANCHOCOLUMNA",System.Type.GetType("System.Double")));//7
			dtCampos.Columns.Add(new DataColumn("SUMATORIA",System.Type.GetType("System.String")));//8
			dtCampos.Columns.Add(new DataColumn("PARTEIMPRESION",System.Type.GetType("System.String")));//9
			dtCampos.Columns.Add(new DataColumn("ETIQUETA",System.Type.GetType("System.String")));//10
			dtCampos.Columns.Add(new DataColumn("JUSTIFY",System.Type.GetType("System.String")));//11
		}
		
		protected void Bind_dgCampos()
		{
			Session["dtCampos"] = dtCampos;
			dgCampos.DataSource = dtCampos;
			dgCampos.DataBind();
		}
		
		protected void Estado_ddls(bool estado)
		{
			ddlTablas.Enabled = estado;
			ddlCampos.Enabled = estado;
			btnCnf.Enabled = estado;
		}
		
		protected void Clear_Config()
		{
			posX.Text = "";
			posY.Text = "";
			etiqCamp.Text = "";
			ddlMascara.Items.Clear();
			ddlAlineacion.SelectedIndex = 0;
			plcConf.Visible = false;
		}
		
		protected void Preparar_ddlMascara(int opc)
		{
			ddlMascara.Items.Clear();
			ddlMascara.Items.Add(new ListItem("Ninguna",""));
			if(opc == 1)
				ddlMascara.Items.Add(new ListItem("Formato de Moneda","C"));
			else if(opc == 2)
			{
				ddlMascara.Items.Add(new ListItem("Ninguna",""));
				ddlMascara.Items.Add(new ListItem("aa-mm-dd","yy-MM-dd"));
				ddlMascara.Items.Add(new ListItem("dd-mm-aa","dd-MM-aa"));
				ddlMascara.Items.Add(new ListItem("mm-dd-aa","MM-dd-yy"));
				ddlMascara.Items.Add(new ListItem("aaaa-mm-dd","yyyy-MM-dd"));
				ddlMascara.Items.Add(new ListItem("dd-mm-aaaa","dd-MM-yyyy"));
				ddlMascara.Items.Add(new ListItem("mm-dd-aaaa","MM-dd-yyyy"));
			}
		}
		
		protected void Preparar_ddlAlineacion()
		{
			ddlAlineacion.Items.Clear();
			ddlAlineacion.Items.Add(new ListItem("Izquierda","I"));
			ddlAlineacion.Items.Add(new ListItem("Derecha","D"));
		}
		
		#endregion
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
