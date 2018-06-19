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
using AMS.Tools;

namespace AMS.Reportes
{
	public partial class Filtros : System.Web.UI.UserControl 
	{
		#region Atributos
		//Condicionales
		protected DataTable dtFltrRpt;
		//Filtros
		protected DataTable dtFltrRpt2;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				confFiltro.Visible = confCondi.Visible = false;
				DatasToControls bind = new DatasToControls();
				PrepararDdlTablas(ddlTablas,Request.QueryString["tabla"]);
				PrepararDdlTablas(ddlTablas2,Request.QueryString["tabla"]);
				bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"'");
				bind.PutDatasIntoDropDownList(ddlCampos2,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas2.SelectedValue+"'");
			}
			if(Session["dtFltrRpt"]==null)
				this.Preparar_dtFltrRpt();
			else
				dtFltrRpt = (DataTable)Session["dtFltrRpt"];
			if(Session["dtFltrRpt2"]==null)
				this.Preparar_dtFltrRpt2();
			else
				dtFltrRpt2 = (DataTable)Session["dtFltrRpt2"];
		}
		
		protected void Cambio_Tabla(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"'");
		}
		
		protected void Cambio_Tabla2(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlCampos2,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas2.SelectedValue+"'");
		}
		
		protected void Configurar_Filtros(Object  Sender, EventArgs e)
		{
			//Primero desactivamos los controles de seleccion
			Accion_Controles_Seleccion1(false);
			//Ahora determinamos el tipo de dato que es campo seleccionado
			confCondi.Visible = true;
			string tipoDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+ddlCampos.SelectedValue+"' AND tbname='"+ddlTablas.SelectedValue+"'").Trim();
			if(tipoDato == "VARCHAR" || tipoDato == "CHARACTER" || tipoDato == "LONG VARCHAR" || tipoDato == "CHAR")
			{
				tipoDatos.Text = "Cadena de Caracteres";
				Preparar_DdlOperaciones(oprCmp,0);
			}
			else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
			{
				tipoDatos.Text = "Campo Tipo Numérico";
				Preparar_DdlOperaciones(oprCmp,1);
			}
			else if(tipoDato == "DATE")
			{
				tipoDatos.Text = "Fecha";
				Preparar_DdlOperaciones(oprCmp,1);
			}
			else if(tipoDato == "HORA")
			{
				tipoDatos.Text = "HORA";
				Preparar_DdlOperaciones(oprCmp,1);
			}
			vlCmp.Text = "";
		}
		
		protected void Configurar_Filtros2(Object  Sender, EventArgs e)
		{
			//Primero desactivamos los controles de seleccion
			Accion_Controles_Seleccion2(false);
			if(DBFunctions.RecordExist("SELECT constname FROM sysibm.syskeycoluse WHERE tbname='"+ddlTablas2.SelectedValue+"' AND colname='"+ddlCampos2.SelectedValue+"'"))
				chkValInt.Visible = true;
			//Ahora determinamos el tipo de dato que es campo seleccionado
			confFiltro.Visible = true;
			string tipoDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+ddlCampos2.SelectedValue+"' AND tbname='"+ddlTablas2.SelectedValue+"'").Trim();
			if(tipoDato == "VARCHAR" || tipoDato == "CHARACTER" || tipoDato == "LONG VARCHAR" || tipoDato == "CHAR")
			{
				tipoDatos2.Text = "Cadena de Caracteres";
				Preparar_DdlOperaciones(oprCmp2,0);
			}
			else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
			{
				tipoDatos2.Text = "Campo Tipo Numérico";
				Preparar_DdlOperaciones(oprCmp2,1);
			}
			else if(tipoDato == "DATE")
			{
				tipoDatos2.Text = "Fecha";
				Preparar_DdlOperaciones(oprCmp2,1);
			}
			else if(tipoDato == "TIME")
			{
				tipoDatos2.Text = "Hora";
				Preparar_DdlOperaciones(oprCmp2,1);
			}
			vlCmp2.Text = "";
		}
		
		protected void Cancelar_Filtro(Object  Sender, EventArgs e)
		{
			Accion_Controles_Seleccion1(true);
			confCondi.Visible = false;
			vlCmp.Text = "";
		}
		
		protected void Cancelar_Filtro2(Object  Sender, EventArgs e)
		{
			Accion_Controles_Seleccion2(true);
			confFiltro.Visible = false;
			vlCmp2.Text = "";
		}
		
		protected void Aceptar_Filtro(Object  Sender, EventArgs e)
		{
			//Debemos traer el tipo de dato que se ha seleccionad
			string tipoDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+ddlCampos.SelectedValue+"' AND tbname='"+ddlTablas.SelectedValue+"'").Trim();
			bool validacion = true;
			//Si el tipo de dato es bigint, integer o smallint debo comprobar que sea compatible
			if(tipoDato == "BIGINT" || tipoDato == "INTEGER" || tipoDato == "SMALLINT")
				validacion = DatasToControls.ValidarInt(vlCmp.Text);
			else if(tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "REAL")
				validacion = DatasToControls.ValidarDouble(vlCmp.Text);
			else if(tipoDato == "DATE" || tipoDato == "TIME")
				validacion = DatasToControls.ValidarDateTime(vlCmp.Text);
			if(validacion)
			{
				DataRow fila = dtFltrRpt.NewRow();
				fila[0] = ddlTablas.SelectedValue;
				fila[1] = ddlCampos.SelectedValue;
				fila[2] = oprCmp.SelectedValue;
				fila[3] = vlCmp.Text;
				fila[4] = tipoDato;
				dtFltrRpt.Rows.Add(fila);
				Bind_dgFltrRpt();
				Accion_Controles_Seleccion1(true);
				confCondi.Visible = false;
				vlCmp.Text = "";
				btnAptFn.Visible = true;
			}
			else
            Utils.MostrarAlerta(Response, "Valor no valido para este filtro");
		}
		
		protected void Aceptar_Filtro2(Object  Sender, EventArgs e)
		{
			DataRow fila = dtFltrRpt2.NewRow();
			fila[0] = ddlTablas2.SelectedValue;
			fila[1] = ddlCampos2.SelectedValue;
			fila[2] = vlCmp2.Text;
			fila[3] = oprCmp2.SelectedValue;
			fila[4] = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+ddlCampos2.SelectedValue+"' AND tbname='"+ddlTablas2.SelectedValue+"'").Trim();
			//Para determinar el control grafico asociado debemos determinar si este campo hace parte de una llave foranea
			DataSet nombreIndice = new DataSet();
			DBFunctions.Request(nombreIndice,IncludeSchema.NO,"SELECT constname FROM sysibm.syskeycoluse WHERE tbname='"+ddlTablas2.SelectedValue+"' AND colname='"+ddlCampos2.SelectedValue+"'");
			if(nombreIndice.Tables[0].Rows.Count==0)
				fila[5] = "TextBox";
			else
			{
				bool ddl = false;
				for(int i=0;i<nombreIndice.Tables[0].Rows.Count;i++)
				{
					if(DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels WHERE relname='"+nombreIndice.Tables[0].Rows[i][0].ToString()+"'"))
						ddl = true;
				}
				//Ahora debemos determinar si es una llave primaria o no
				if(ddl)
					fila[5] = "DropDownList";
				else
					fila[5] = "TextBox";
			}
			if(chkValInt.Visible && chkValInt.Checked)
				fila[6] = "S";
			else
				fila[6] = "N";
			chkValInt.Checked = true;
			chkValInt.Visible = false;
			dtFltrRpt2.Rows.Add(fila);
			Bind_dgFltrRpt2();
			Accion_Controles_Seleccion2(true);
			confFiltro.Visible = false;
			vlCmp2.Text = "";
			btnAptFn.Visible = true;
		}
		
		protected void Construir_Consulta(Object  Sender, EventArgs e)
		{
			string sqlOri = Request.QueryString["sql"];
			string groupby = "";
			string sqlCore = "";
			string mainTable = Request.QueryString["tabla"];
			string refs = "";
			string filtros = "";
			if(Request.QueryString["group"]=="S")
			{
				sqlCore = sqlOri.Substring(0,sqlOri.IndexOf("GROUP"));
				groupby = sqlOri.Substring(sqlOri.IndexOf("GROUP")+9);
			}
			else
				sqlCore = sqlOri;
			//Revisamos si el sqlCore trae WHERE
			if(sqlCore.IndexOf("WHERE")==-1)
				sqlCore += " WHERE ";
			else
				sqlCore += " AND ";
			//Ahora vamos a agregar las sentencias condicionales diseñadas por el usuario
			for(int i=0;i<dtFltrRpt.Rows.Count;i++)
			{
				if(dtFltrRpt.Rows[i][0].ToString()!=mainTable)
				{
					if(!this.Revisar_Si_Existe(refs,dtFltrRpt.Rows[i][0].ToString(),sqlCore))
						refs += dtFltrRpt.Rows[i][0].ToString()+"-";
				}
				if(dtFltrRpt.Rows[i][4].ToString() == "VARCHAR" || dtFltrRpt.Rows[i][4].ToString() == "CHARACTER" || dtFltrRpt.Rows[i][4].ToString() == "LONG VARCHAR" || dtFltrRpt.Rows[i][4].ToString() == "CHAR")
				{
					if(dtFltrRpt.Rows[i][2].ToString()=="LIKE")
						sqlCore += " "+dtFltrRpt.Rows[i][0].ToString()+"."+dtFltrRpt.Rows[i][1].ToString()+" "+dtFltrRpt.Rows[i][2].ToString()+" '"+dtFltrRpt.Rows[i][3].ToString()+"%' AND";
					else
						sqlCore += " "+dtFltrRpt.Rows[i][0].ToString()+"."+dtFltrRpt.Rows[i][1].ToString()+dtFltrRpt.Rows[i][2].ToString()+"'"+dtFltrRpt.Rows[i][3].ToString()+"' AND";
				}
				else if(dtFltrRpt.Rows[i][4].ToString() == "BIGINT" || dtFltrRpt.Rows[i][4].ToString() == "DECIMAL" || dtFltrRpt.Rows[i][4].ToString() == "DOUBLE" || dtFltrRpt.Rows[i][4].ToString() == "INTEGER" || dtFltrRpt.Rows[i][4].ToString() == "REAL" || dtFltrRpt.Rows[i][4].ToString() == "SMALLINT")
					sqlCore += " "+dtFltrRpt.Rows[i][0].ToString()+"."+dtFltrRpt.Rows[i][1].ToString()+dtFltrRpt.Rows[i][2].ToString()+dtFltrRpt.Rows[i][3].ToString()+" AND";
				else if(dtFltrRpt.Rows[i][4].ToString() == "DATE" || dtFltrRpt.Rows[i][4].ToString() == "TIME")
					sqlCore += " "+dtFltrRpt.Rows[i][0].ToString()+"."+dtFltrRpt.Rows[i][1].ToString()+dtFltrRpt.Rows[i][2].ToString()+"'"+dtFltrRpt.Rows[i][3].ToString()+"' AND";
			}
			//Ahora agregamos las partes del filtro externo que van separadas por reemplaza el valor por un simbolo @
			//Y ademas vamos seleccionando en el string filtros de la forma tipoControl-campo-tborigen-etiqueta
			for(int i=0;i<dtFltrRpt2.Rows.Count;i++)
			{
				if(dtFltrRpt2.Rows[i][0].ToString()!=mainTable)
				{
					if(!this.Revisar_Si_Existe(refs,dtFltrRpt2.Rows[i][0].ToString(),sqlCore))
						refs += dtFltrRpt2.Rows[i][0].ToString()+"-";
				}
				if(dtFltrRpt2.Rows[i][4].ToString() == "VARCHAR" || dtFltrRpt2.Rows[i][4].ToString() == "CHARACTER" || dtFltrRpt2.Rows[i][4].ToString() == "LONG VARCHAR" || dtFltrRpt2.Rows[i][4].ToString() == "CHAR")
				{
					if(dtFltrRpt2.Rows[i][3].ToString()=="LIKE")
						sqlCore += " "+dtFltrRpt2.Rows[i][0].ToString()+"."+dtFltrRpt2.Rows[i][1].ToString()+" "+dtFltrRpt2.Rows[i][3].ToString()+" '@' AND";
					else
						sqlCore += " "+dtFltrRpt2.Rows[i][0].ToString()+"."+dtFltrRpt2.Rows[i][1].ToString()+dtFltrRpt2.Rows[i][3].ToString()+"'@' AND";
				}
				else if(dtFltrRpt2.Rows[i][4].ToString() == "BIGINT" || dtFltrRpt2.Rows[i][4].ToString() == "DECIMAL" || dtFltrRpt2.Rows[i][4].ToString() == "DOUBLE" || dtFltrRpt2.Rows[i][4].ToString() == "INTEGER" || dtFltrRpt2.Rows[i][4].ToString() == "REAL" || dtFltrRpt2.Rows[i][4].ToString() == "SMALLINT")
					sqlCore += " "+dtFltrRpt2.Rows[i][0].ToString()+"."+dtFltrRpt2.Rows[i][1].ToString()+dtFltrRpt2.Rows[i][3].ToString()+"@ AND";
				else if(dtFltrRpt2.Rows[i][4].ToString() == "DATE" || dtFltrRpt2.Rows[i][4].ToString() == "TIME" )
					sqlCore += " "+dtFltrRpt2.Rows[i][0].ToString()+"."+dtFltrRpt2.Rows[i][1].ToString()+dtFltrRpt2.Rows[i][3].ToString()+"'@' AND";
				filtros += dtFltrRpt2.Rows[i][5].ToString()+"-"+dtFltrRpt2.Rows[i][1].ToString()+"-"+dtFltrRpt2.Rows[i][0].ToString()+"-"+dtFltrRpt2.Rows[i][2].ToString()+"-"+dtFltrRpt2.Rows[i][3].ToString()+"-"+dtFltrRpt2.Rows[i][6].ToString()+"*";
			}
			sqlCore = sqlCore.Substring(0,sqlCore.Length-3);
			//Ahora revisamos si es necesario que existan mas tablas de referencia dentro del from
			if(refs!="")
			{
				string sql1 = sqlCore.Substring(0,sqlCore.IndexOf("FROM"));
				string sql2 = sqlCore.Substring(sqlCore.IndexOf("FROM")+4,sqlCore.IndexOf("WHERE")-(sqlCore.IndexOf("FROM")+4));
				string sql3 = sqlCore.Substring(sqlCore.IndexOf("WHERE")+5);
				sql3 += " AND ";
				sql2 += " ,";
				refs = refs.Substring(0,refs.Length-1);
				string[] refsArray = refs.Split('-');
				for(int i=0;i<refsArray.Length;i++)
				{
					//Ahora creamos la relacion entre la tabla secundaria y la tabla principal
					sql2 += " "+refsArray[i]+" ,";
					DataSet fkcolname = new DataSet();
					DBFunctions.Request(fkcolname,IncludeSchema.NO,"SELECT colname FROM sysibm.syskeycoluse WHERE constname=(SELECT relname FROM sysibm.sysrels WHERE tbname='"+mainTable+"' AND reftbname='"+refsArray[i]+"') ORDER BY colseq");
					string rlcolname = DBFunctions.SingleData("SELECT colnames FROM sysibm.sysindexes WHERE name=(SELECT refkeyname FROM sysibm.sysrels WHERE tbname='"+mainTable+"' AND reftbname='"+refsArray[i]+"')");
					string[] rlcolnames = rlcolname.Split('+');
					for(int j=0;j<fkcolname.Tables[0].Rows.Count;j++)
						sql3 += " "+mainTable.Trim()+"."+fkcolname.Tables[0].Rows[j][0].ToString().Trim()+"="+refsArray[i].Trim()+"."+rlcolnames[j+1].Trim()+" AND";
				}
				sql2 = sql2.Substring(0,sql2.Length-1);
				sql3 = sql3.Substring(0,sql3.Length-3);
				sqlCore = sql1+" FROM "+sql2+" WHERE "+sql3;
			}
			if(Request.QueryString["group"] == "S")
				sqlCore += " GROUP BY "+groupby;
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,sqlCore);
			try{
				if(filtros!="")
					Response.Redirect("" + indexPage + "?process=Reportes.OtrosReporte&tabla="+mainTable+"&sql="+sqlCore+"&filters="+filtros.Substring(0,filtros.Length-1)+"&mask="+Request.QueryString["mask"]+"");
				else
					Response.Redirect("" + indexPage + "?process=Reportes.OtrosReporte&tabla="+mainTable+"&sql="+sqlCore+"&filters="+filtros+"&mask="+Request.QueryString["mask"]+"");
			}
			catch(Exception ex){lb.Text += ex.ToString()+"<br>"+sqlCore;}
		}
		
		public void EliminarCondicional(Object sender, DataGridCommandEventArgs e)
		{
			try
            {
        		dtFltrRpt.Rows.Remove(dtFltrRpt.Rows[e.Item.ItemIndex]);
	            dgFltrRpt.EditItemIndex = -1;
			}catch(Exception ex){lb.Text += "<br>"+ex.ToString();}			
			Bind_dgFltrRpt();
		}
		
		public void EliminarFiltro(Object sender, DataGridCommandEventArgs e)
		{
			try
            {
        		dtFltrRpt2.Rows.Remove(dtFltrRpt2.Rows[e.Item.ItemIndex]);
	            dgFltrRpt2.EditItemIndex = -1;
			}catch(Exception ex){lb.Text += "<br>"+ex.ToString();}			
			Bind_dgFltrRpt2();
		}
		#endregion
		
		#region Otros
		
		protected void PrepararDdlTablas(DropDownList ddl, string tablas)
		{
			string[] tablasFilas = tablas.Split('-');
			for(int i=0;i<tablasFilas.Length;i++)
			{
				int j;
				ddl.Items.Add(tablasFilas[i]);
				//Ahora traemos las tablas relacionadas con la tabla principal del reporte
				DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT reftbname FROM sysibm.sysrels WHERE tbname = '"+tablasFilas[i]+"';"+
				                   						"SELECT tbname FROM sysibm.sysrels WHERE reftbname = '"+tablasFilas[i]+"';");
				for(j=0;j<ds.Tables[0].Rows.Count;j++)
					ddl.Items.Add(ds.Tables[0].Rows[j][0].ToString());
				for(j=0;j<ds.Tables[1].Rows.Count;j++)
					ddl.Items.Add(ds.Tables[1].Rows[j][0].ToString());
			}
		}
		
		protected void Accion_Controles_Seleccion1(bool valor)
		{
			ddlTablas.Enabled = valor;
			ddlCampos.Enabled = valor;
			btCnfg1.Enabled = valor;
		}
		
		protected void Accion_Controles_Seleccion2(bool valor)
		{
			ddlTablas2.Enabled = valor;
			ddlCampos2.Enabled = valor;
			btCnfg2.Enabled = valor;
		}
		
		protected void Preparar_DdlOperaciones(DropDownList ddl, int opcion)
		{
			ddl.Items.Clear();
			if(opcion == 0)
			{
				ddl.Items.Add(new ListItem("Comparación exacta","="));
				ddl.Items.Add(new ListItem("Diferente","<>"));
				ddl.Items.Add(new ListItem("Similitud","LIKE"));
			}
			else if(opcion == 1)
			{
				ddl.Items.Add(new ListItem("Igual","="));
				ddl.Items.Add(new ListItem("Diferente","<>"));
				ddl.Items.Add(new ListItem("Mayor que",">"));
				ddl.Items.Add(new ListItem("Menor que","<"));
				ddl.Items.Add(new ListItem("Mayor igual que",">="));
				ddl.Items.Add(new ListItem("Menor igual que","<="));				
			}
		}
		
		protected void Preparar_dtFltrRpt()
		{
			dtFltrRpt = new DataTable();
			dtFltrRpt.Columns.Add(new DataColumn("TABLA",System.Type.GetType("System.String")));//0
			dtFltrRpt.Columns.Add(new DataColumn("CAMPO",System.Type.GetType("System.String")));//1
			dtFltrRpt.Columns.Add(new DataColumn("TIPOCOMPARACION",System.Type.GetType("System.String")));//2
			dtFltrRpt.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));//3
			dtFltrRpt.Columns.Add(new DataColumn("TIPODATO",System.Type.GetType("System.String")));//4
		}
		
		protected void Preparar_dtFltrRpt2()
		{
			dtFltrRpt2 = new DataTable();
			dtFltrRpt2.Columns.Add(new DataColumn("TABLA",System.Type.GetType("System.String")));//0
			dtFltrRpt2.Columns.Add(new DataColumn("CAMPO",System.Type.GetType("System.String")));//1
			dtFltrRpt2.Columns.Add(new DataColumn("ETIQUETA",System.Type.GetType("System.String")));//2
			dtFltrRpt2.Columns.Add(new DataColumn("TIPOCOMPARACION",System.Type.GetType("System.String")));//3
			dtFltrRpt2.Columns.Add(new DataColumn("TIPODATO",System.Type.GetType("System.String")));//4
			dtFltrRpt2.Columns.Add(new DataColumn("CONTROLASOCIADO",System.Type.GetType("System.String")));//5
			dtFltrRpt2.Columns.Add(new DataColumn("VALORINTERPRETADO",System.Type.GetType("System.String")));//6
		}
		
		protected void Bind_dgFltrRpt()
		{
			Session["dtFltrRpt"] = dtFltrRpt;
			dgFltrRpt.DataSource = dtFltrRpt;
			dgFltrRpt.DataBind();
		}
		
		protected void Bind_dgFltrRpt2()
		{
			Session["dtFltrRpt2"] = dtFltrRpt2;
			dgFltrRpt2.DataSource = dtFltrRpt2;
			dgFltrRpt2.DataBind();
		}
		
		protected bool Revisar_Si_Existe(string sep, string rev, string sqlCore)
		{
			bool exist = false;
			string[] seps = sep.Split('-');
			for(int i=0;i<seps.Length;i++)
			{
				if(seps[i].Trim() == rev)
					exist = true;
			}
			//Aqui ya se ha revisado si existe en las refs ahora reviosamos si existe en dentro de la sentencia
			string tables = "";
			if(sqlCore.IndexOf("WHERE")!=-1)
				tables = sqlCore.Substring(sqlCore.IndexOf("FROM")+4,sqlCore.IndexOf("WHERE")-(sqlCore.IndexOf("FROM")+4));
			else
				tables = sqlCore.Substring(sqlCore.IndexOf("FROM")+4);
			string[] seps2 = tables.Split(',');
			for(int i=0;i<seps2.Length;i++)
			{
				if(seps2[i].Trim() == rev)
					exist = true;
			}
			return exist;
		}
		
		#endregion
		
		////////////////////////////////////////////////
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
