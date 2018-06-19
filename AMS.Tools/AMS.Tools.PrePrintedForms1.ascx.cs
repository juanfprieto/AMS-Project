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
	public partial class BuildHeader : System.Web.UI.UserControl
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
				Session.Clear();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlTablas,"SELECT STB.staf_nombretabla , STB.staf_nombretabla CONCAT ' - ' CONCAT COALESCE(SYT.remarks,'Tabla No Comentada') FROM stablaasociadafactura STB, sysibm.systables SYT WHERE STB.staf_nombretabla = SYT.name AND STB.stfc_codigo='"+Request.QueryString["TipEsp"]+"' AND STB.tpar_codigo='H'");
				bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"' ORDER BY colno");
				this.Agregar_Columnas_Especiales(ddlCampos,ddlTablas.SelectedValue);
				this.Preparar_dtCampos();
			}
			if(Session["dtCampos"]==null)
				this.Preparar_dtCampos();
			else
				dtCampos = (DataTable)Session["dtCampos"];
		}
		
		protected void Cambio_ddlTablas(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"' ORDER BY colno");
			this.Agregar_Columnas_Especiales(ddlCampos,ddlTablas.SelectedValue);
		}
		
		protected void Continuar(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Tools.PrePrintedForms2&TipGen="+Request.QueryString["TipGen"]+"&TipEsp="+Request.QueryString["TipEsp"]+"");
		}
			
		protected void Configurar_Campo(Object  Sender, EventArgs e)
		{
			this.Estado_ddls(false);
			//Debo Mostrar el placeholder 
			plcConf.Visible = true;
			string tipoDato = DBFunctions.SingleData("SELECT COLTYPE FROM sysibm.syscolumns WHERE name='"+ddlCampos.SelectedValue+"' AND tbname='"+ddlTablas.SelectedValue+"'").Trim();
			if(tipoDato == "")
				tipoDato = DBFunctions.SingleData("SELECT scle_tipodato FROM scolumnaespecial WHERE scle_columna='"+ddlCampos.SelectedValue+"' AND staf_nombretabla='"+ddlTablas.SelectedValue+"' AND stfc_codigo='"+Request.QueryString["TipEsp"]+"'");
			if(tipoDato == "VARCHAR" || tipoDato == "CHARACTER" || tipoDato == "LONG VARCHAR" || tipoDato == "CHAR")
			{
				lbTpDat.Text = "Cadena de Caracteres";
				this.Preparar_ddlMascara(0);
			}
			else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
			{
				lbTpDat.Text = "Valor Númerico";
				this.Preparar_ddlMascara(1);
			}
			else if(tipoDato == "DATE")
			{
				lbTpDat.Text = "Fecha";
				this.Preparar_ddlMascara(2);
			}
			else if(tipoDato == "TIME")
			{
				lbTpDat.Text = "Hora";
				this.Preparar_ddlMascara(0);
			}
		}
			
		protected void Aceptar_Configuracion(Object  Sender, EventArgs e)
		{
			DataRow fila = dtCampos.NewRow();
			fila[0] = ddlTablas.SelectedValue;
			fila[1] = Request.QueryString["TipEsp"];
			fila[2] = ddlCampos.SelectedValue;
			fila[3] = System.Convert.ToDouble(posX.Text);
			fila[4] = System.Convert.ToDouble(posY.Text);
			fila[5] = ddlMascara.SelectedValue;
			fila[9] = "H";
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
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT scle_columna, scle_etiqueta FROM scolumnaespecial WHERE staf_nombretabla='"+nombreTabla+"' AND tpar_codigo='H' AND stfc_codigo='"+Request.QueryString["TipEsp"]+"'");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				ddlCamposEsp.Items.Add(new ListItem(ds.Tables[0].Rows[i][1].ToString(),ds.Tables[0].Rows[i][0].ToString()));
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
			DatasToControls.JustificacionGrilla(dgCampos,dtCampos);
			if(dtCampos.Rows.Count > 0)
				btnCnt.Visible = true;
			else
				btnCnt.Visible = false;
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
			ddlMascara.Items.Clear();
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
				ddlMascara.Items.Add(new ListItem("dd-mm-aa","dd-MM-yy"));
				ddlMascara.Items.Add(new ListItem("mm-dd-aa","MM-dd-yy"));
				ddlMascara.Items.Add(new ListItem("aaaa-mm-dd","yyyy-MM-dd"));
				ddlMascara.Items.Add(new ListItem("dd-mm-aaaa","dd-MM-yyyy"));
				ddlMascara.Items.Add(new ListItem("mm-dd-aaaa","MM-dd-yyyy"));
			}
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
