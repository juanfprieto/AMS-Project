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
	public partial class Campos : System.Web.UI.UserControl 
	{
		#region Atributos
		protected DataTable dtCmpsRpt;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				Session.Clear();
				PrepararDdlTablas(Request.QueryString["tablas"]);
				BindCamposTablas(ddlCampos,ddlTablas.SelectedValue);
				configurar.Visible = false;
			}
			if(Session["dtCmpsRpt"]==null)
				PrepararDtCmpsRpt();
			else
				dtCmpsRpt = (DataTable)Session["dtCmpsRpt"];
		}
		
		protected void Cambio_Tabla(Object  Sender, EventArgs e)
		{
			BindCamposTablas(ddlCampos,ddlTablas.SelectedValue);
		}
		
		protected void ConfigurarCampo(Object  Sender, EventArgs e)
		{
			//Primero desactivamos los controles de seleccion
			AccionControlesSeleccion(false);
			//Ahora determinamos el tipo de dato que es campo seleccionado
			configurar.Visible = true;
			string tipoDato = "",  nombreTabla = "";
			if(ddlTablas.SelectedValue.IndexOf("-") == -1)
				nombreTabla = ddlTablas.SelectedValue.Trim();
			else
				nombreTabla = (ddlTablas.SelectedValue.Split('-'))[0].Trim();
			tipoDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+ddlCampos.SelectedValue.Trim()+"' AND tbname='"+nombreTabla+"'").Trim();
			//Debemos revisar si este campo hace parte de una llave foranea dentro de su respectiva tabla
			if(DBFunctions.RecordExist("SELECT * FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+ddlCampos.SelectedValue.Trim()+"')"))
				chkFk.Visible = true;
			/////////////////////////////
			if(tipoDato == "VARCHAR" || tipoDato == "CHARACTER" || tipoDato == "LONG VARCHAR" || tipoDato == "CHAR")
			{
				tipoDatos.Text = "Cadena de Caracteres";
				Preparar_DdlOperaciones(0);
				Preparar_DdlMascaras(0);
			}
			else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
			{
				tipoDatos.Text = "Campo Tipo Numérico";
				Preparar_DdlOperaciones(1);
				Preparar_DdlMascaras(1);
			}
			else if(tipoDato == "DATE")
			{
				tipoDatos.Text = "Fecha";
				Preparar_DdlOperaciones(0);
				Preparar_DdlMascaras(0);
			}
			else if(tipoDato == "TIME")
			{
				tipoDatos.Text = "Hora";
				Preparar_DdlOperaciones(0);
				Preparar_DdlMascaras(0);
			}
			Limpiar_Configuracion();
			ordPst.Text = (dgCmpsRpt.Items.Count+1).ToString();
		}
		
		protected void Cancelar_Adicion(Object  Sender, EventArgs e)
		{
			AccionControlesSeleccion(true);
			configurar.Visible = chkFk.Visible = chkFk.Checked = false;
		}
		
		protected void Aceptar_Adicion(Object  Sender, EventArgs e)
		{
			DataRow[] selection = dtCmpsRpt.Select("TABLA='"+ddlTablas.SelectedValue+"' AND NOMBRECAMPO='"+ddlCampos.SelectedValue+"'");
			DataRow[] selection2 = dtCmpsRpt.Select("ORDEN='"+ordPst.Text+"'");
			if(selection.Length>0)
			{
                Utils.MostrarAlerta(Response, "Este Campo ya fue Agregado");
				this.AccionControlesSeleccion(true);
				configurar.Visible = chkFk.Visible = chkFk.Checked = false;
			}
			else
			{
				if(selection2.Length>0)
				{
                    Utils.MostrarAlerta(Response, "Ya existe un Registro en este lugar, Por favor revisar que el orden de presentación no se encuentre repetido");
					this.AccionControlesSeleccion(true);
					configurar.Visible = chkFk.Visible = chkFk.Checked = false;
				}
				else
				{
					DataRow fila = dtCmpsRpt.NewRow();
					string nombreTabla = "", campoTabla= "";
					if(ddlTablas.SelectedValue.IndexOf("-")==-1)
					{
						nombreTabla = ddlTablas.SelectedValue;
						campoTabla = ddlCampos.SelectedValue;
					}
					else
					{
						string[] sep = ddlTablas.SelectedValue.Split('-');
						nombreTabla = sep[0];
						campoTabla = ddlCampos.SelectedValue+"-"+sep[1];
					}
					fila[0] = nombreTabla;
					fila[1] = campoTabla;
					fila[2] = etCmp.Text;
					fila[3] = ordPst.Text;
					fila[4] = oprCmp.SelectedValue;
					if(mscrVlr.SelectedValue != "")
						fila[5] = ordPst.Text+"-"+mscrVlr.SelectedValue;
					if(chkFk.Checked)
						fila[6] = "SI";
					//La relacion con la tabla principal es de :
					//0 : Es la misma tabla principal
					//1 : Es referencia de la tabla principal
					//2 : Hace referencia a la tabla principal
					if(nombreTabla == Request.QueryString["tablas"])
						fila[7] = "0";
					else if(DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels WHERE tbname='"+Request.QueryString["tablas"]+"' AND reftbname='"+nombreTabla+"'"))
						fila[7] = "1";
					else if(DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels WHERE tbname='"+nombreTabla+"' AND reftbname='"+Request.QueryString["tablas"]+"'"))
						fila[7] = "2";
					dtCmpsRpt.Rows.Add(fila);
					this.Bind_dgCmpsRpt();
					this.AccionControlesSeleccion(true);
					configurar.Visible = chkFk.Visible = chkFk.Checked = false;
					if(!btnAcpt2.Visible)
						btnAcpt2.Visible = true;
				}
			}
		}
		
		protected void AceptarCampos(Object  Sender, EventArgs e)
		{
			string select = "SELECT", from = " FROM " , where = " WHERE ", groupBy = " GROUP BY";
			string mainTable = Request.QueryString["tablas"];
			bool agrupamiento = false; 
			//Ahora vamos a meter dentro del select los campos que se han seleccionado y sus funciones si asi las van a necesitar
			//Debemos recorrer la tabla que contiene los campos seleccionados y adicionarlos a la sentencia select
			DataRow[] selection = dtCmpsRpt.Select("","ORDEN ASC");
			for(int i=0;i<selection.Length;i++)
			{
				//Revisamos si se necesita el valor interpretado de la llave foranea o no
				if(selection[i][6].ToString() == "")
				{
					//En este caso solo se necesita el valor literal del campo
					//1. Agregamos la columna al string SELECT 
					if(selection[i][4].ToString()=="")
					{
						if(selection[i][1].ToString().IndexOf("-")==-1)
						{
							select += " " + selection[i][0].ToString() + "." + selection[i][1].ToString() +" AS \""+selection[i][2].ToString()+"\" ,";
							groupBy += " " + selection[i][0].ToString() + "." + selection[i][1].ToString()+",";
						}
						else
						{
							string[] sep = selection[i][1].ToString().Split('-');
							select += " " + selection[i][0].ToString()+i.ToString() + "." + sep[0] +" AS \""+selection[i][2].ToString()+"\" ,";
							groupBy += " " + selection[i][0].ToString()+i.ToString() + "." + sep[0]+",";
							from += " "+mainTable+"."+sep[1]+"="+selection[i][0].ToString()+i.ToString() + "." + sep[0]+" AND";
						}
					}
					else
					{
						agrupamiento = true;
						select += " "+ selection[i][4].ToString()+"("+selection[i][0].ToString() + "." + selection[i][1].ToString()+") AS \""+selection[i][2].ToString()+"\" ,";
					}
					//2. Agregamos la tabla, si ya no esta agregada al FROM y la forma de acceso al campo dentro del WHERE
					if(from.IndexOf(selection[i][0].ToString()) == -1)
					{
						from += " "+selection[i][0].ToString()+",";
						if(selection[i][0].ToString() != mainTable)
						{
							string tabla1 = "", tabla2 = "";
							//Agregamos el acceso a la tabla, pero revisamos que relacion tiene con la tabla
							//si la relacion de la tabla es tipo 1 entonces traemos las referencias
							if(selection[i][7].ToString() == "1")
							{
								tabla1 = mainTable;
								tabla2 = selection[i][0].ToString();
							}
							else if(selection[i][7].ToString() == "2")
							{
								tabla1 = selection[i][0].ToString();
								tabla2 = mainTable;
							}
							where += RetornarCaminoAccesso(tabla1,tabla2);
						}
					}
				}
				else //Si se necesita el valor interpretado del campo de llave foranea
				{
					//Si la tabla es diferente de la principal debemos crear la relacion entre las dos, si no existe ya
					if(from.IndexOf(selection[i][0].ToString()) == -1)
					{
						from += " "+selection[i][0].ToString()+",";
						if(selection[i][0].ToString() != mainTable)
						{
							if(from.IndexOf(mainTable) == -1)
								from += " "+mainTable+",";
							string tabla1 = "", tabla2 = "";
							//Agregamos el acceso a la tabla, pero revisamos que relacion tiene con la tabla
							//si la relacion de la tabla es tipo 1 entonces traemos las referencias
							if(selection[i][7].ToString() == "1")
							{
								tabla1 = mainTable;
								tabla2 = selection[i][0].ToString();
							}
							else if(selection[i][7].ToString() == "2")
							{
								tabla1 = selection[i][0].ToString();
								tabla2 = mainTable;
							}
							where += RetornarCaminoAccesso(tabla1,tabla2);
						}
					}
					string nombreTablaAnterior = "";
					string nombreTabla = selection[i][0].ToString();
					string nombreCampo = selection[i][1].ToString();
					while(DBFunctions.SingleData("SELECT * FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')").Trim() != "")
					{
						//traemos la tabla a que se hace referencia 
						string tablaReferencia = DBFunctions.SingleData("SELECT reftbname FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')");
						DataSet fkcolname = new DataSet();
						DBFunctions.Request(fkcolname,IncludeSchema.NO,"SELECT colname FROM sysibm.syskeycoluse WHERE constname IN (SELECT relname FROM sysibm.sysrels WHERE tbname='"+nombreTabla+"' AND reftbname='"+tablaReferencia+"') ORDER BY colseq");
						string rlcolname = DBFunctions.SingleData("SELECT colnames FROM sysibm.sysindexes WHERE name IN (SELECT refkeyname FROM sysibm.sysrels WHERE tbname='"+nombreTabla+"' AND reftbname='"+tablaReferencia+"')");
						string[] rlcolnames = rlcolname.Split('+');
						if(fkcolname.Tables[0].Rows.Count == rlcolnames.Length-1)
						{
							for(int j=0;j<fkcolname.Tables[0].Rows.Count;j++)
							{
								if(from.IndexOf(nombreTabla) == -1)
									from += " "+nombreTabla+",";
								where += " "+nombreTabla+"."+fkcolname.Tables[0].Rows[j][0].ToString().Trim()+"="+tablaReferencia+"."+rlcolnames[j+1].Trim()+" AND";
								if(nombreCampo == fkcolname.Tables[0].Rows[j][0].ToString().Trim())
									nombreCampo = rlcolnames[j+1].Trim();
							}
						}
						nombreTablaAnterior = nombreTabla;
						nombreTabla = tablaReferencia;
					}
					if(from.IndexOf(nombreTabla) == -1)
						from += " "+nombreTabla+",";
					where += RetornarCaminoAccesso(nombreTablaAnterior,nombreTabla);
					//Ahora agregamos el valor
					if(selection[i][4].ToString() == "")
						select += " " + nombreTabla + "." + DBFunctions.SingleData("SELECT * FROM SYSIBM.SYSCOLUMNS WHERE tbname='"+nombreTabla+"' AND colno=1") +" AS \""+selection[i][2].ToString()+"\" ,";
					else
					{
						agrupamiento = true;
						select += " "+ selection[i][4].ToString()+"("+nombreTabla + "." + DBFunctions.SingleData("SELECT * FROM SYSIBM.SYSCOLUMNS WHERE tbname='"+nombreTabla+"' AND colno=1") +") AS \""+selection[i][2].ToString()+"\" ,";
					}
				}
			}
			string sqlQuery = "";
			if(select.Length > 6)
				sqlQuery += select.Substring(0,select.Length-1);
			if(from.Length > 6)
				sqlQuery += from.Substring(0,from.Length-1);
			if(where.Length > 7)
				sqlQuery += where.Substring(0,where.Length-3);
			if(groupBy.Length > 9 && agrupamiento)
				sqlQuery += groupBy.Substring(0,groupBy.Length-1);
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,sqlQuery);
			//Ahora preparamos el string que nos representa las mascaras
			string mscrs = "";
			for(int i=0;i<dtCmpsRpt.Rows.Count;i++)
			{
				if(dtCmpsRpt.Rows[i][5].ToString()!="")
					mscrs += " "+dtCmpsRpt.Rows[i][5].ToString()+" ,";
			}
			if(mscrs!="")
				mscrs = mscrs.Substring(0,mscrs.Length-1);
			if(ds.Tables.Count>0)
			{
				if(agrupamiento)
					Response.Redirect("" + indexPage + "?process=Reportes.FiltrosReporte&tabla="+mainTable+"&sql="+sqlQuery+"&group=S&mask="+mscrs+"");
				else
					Response.Redirect("" + indexPage + "?process=Reportes.FiltrosReporte&tabla="+mainTable+"&sql="+sqlQuery+"&group=N&mask="+mscrs+"");
				ds.Clear();
			}
			else
			{
                Utils.MostrarAlerta(Response, "Consulta No Valida");
				lb.Text = sqlQuery+DBFunctions.exceptions;
			}
		}
		
		public void EliminarCampo(Object sender, DataGridCommandEventArgs e)
		{
			try
            {
        		dtCmpsRpt.Rows.Remove(dtCmpsRpt.Rows[e.Item.ItemIndex]);
	            dgCmpsRpt.EditItemIndex = -1;
			}catch(Exception ex){lb.Text += "<br>"+ex.ToString();};
			for(int i=0;i<dtCmpsRpt.Rows.Count;i++)
				dtCmpsRpt.Rows[i][3] = (i+1).ToString();
			Bind_dgCmpsRpt();
		}
		
		#endregion
		
		#region Otros
		
		protected void PrepararDdlTablas(string tablas)
		{
			string[] tablasFilas = tablas.Split('-');
			for(int i=0;i<tablasFilas.Length;i++)
			{
				int j;
				ddlTablas.Items.Add(tablasFilas[i]);
				//Ahora traemos las tablas relacionadas con la tabla principal del reporte
				DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT reftbname, relname FROM sysibm.sysrels WHERE tbname = '"+tablasFilas[i]+"';"+
				                   						"SELECT tbname, relname FROM sysibm.sysrels WHERE reftbname = '"+tablasFilas[i]+"';");
				for(j=0;j<ds.Tables[0].Rows.Count;j++)
				{
					//Se cuenta la cantidad de relaciones que tienen la tabla principal con cada tabla secundaria
					if(Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM sysibm.sysrels WHERE tbname = '"+tablasFilas[i]+"' AND reftbname='"+ds.Tables[0].Rows[j][0].ToString()+"'").Trim()) > 1)
					{
						DataSet fkcolname = new DataSet();
						DBFunctions.Request(fkcolname,IncludeSchema.NO,"SELECT colname FROM sysibm.syskeycoluse WHERE constname ='"+ds.Tables[0].Rows[j][1].ToString()+"' ORDER BY colseq");
						string fks = "";
						for(int k=0;k<fkcolname.Tables[0].Rows.Count;k++)
							fks += fkcolname.Tables[0].Rows[k][0].ToString()+"*";
						ddlTablas.Items.Add(new ListItem(ds.Tables[0].Rows[j][0].ToString()+"-"+DBFunctions.SingleData("SELECT remarks FROM sysibm.syscolumns WHERE name=(SELECT colname FROM sysibm.syskeycoluse WHERE colseq=1 AND constname='"+ds.Tables[0].Rows[j][1].ToString()+"')"),ds.Tables[0].Rows[j][0].ToString()+"-"+fks.Substring(0,fks.Length-1)));
					}
					else
						ddlTablas.Items.Add(new ListItem(ds.Tables[0].Rows[j][0].ToString(),ds.Tables[0].Rows[j][0].ToString()));
				}
				//*******************************************************************************
				//Ahora Agregamos las tablas que hacen referencia a la tabla principal del reporte
				for(j=0;j<ds.Tables[1].Rows.Count;j++)
				{
					if(Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM sysibm.sysrels WHERE tbname = '"+ds.Tables[1].Rows[j][0].ToString()+"' AND reftbname='"+tablasFilas[i]+"'").Trim())>1)
					{
						DataSet fkcolname = new DataSet();
						DBFunctions.Request(fkcolname,IncludeSchema.NO,"SELECT colname FROM sysibm.syskeycoluse WHERE constname ='"+ds.Tables[1].Rows[j][1].ToString()+"' ORDER BY colseq");
						string fks = "";
						for(int k=0;k<fkcolname.Tables[0].Rows.Count;k++)
							fks += fkcolname.Tables[0].Rows[k][0].ToString()+"*";
						ddlTablas.Items.Add(new ListItem(ds.Tables[1].Rows[j][0].ToString()+"-"+DBFunctions.SingleData("SELECT remarks FROM sysibm.syscolumns WHERE name=(SELECT colname FROM sysibm.syskeycoluse WHERE colseq=1 AND constname='"+ds.Tables[1].Rows[j][1].ToString()+"')"),ds.Tables[1].Rows[j][0].ToString()+"-"+fks.Substring(0,fks.Length-1)));
					}
					else
						ddlTablas.Items.Add(new ListItem(ds.Tables[1].Rows[j][0].ToString(),ds.Tables[1].Rows[j][0].ToString()));
				}
			}
		}
		
		protected void AccionControlesSeleccion(bool valor)
		{
			ddlTablas.Enabled = valor;
			ddlCampos.Enabled = valor;
			btCnfg.Enabled = valor;
		}
		
		protected void Preparar_DdlOperaciones(int opcion)
		{
			oprCmp.Items.Clear();
			if(opcion == 0)
				oprCmp.Items.Add(new ListItem("Ninguna",""));
			else if(opcion == 1)
			{
				oprCmp.Items.Add(new ListItem("Ninguna",""));
				oprCmp.Items.Add(new ListItem("Sumatoria de todos los valores","SUM"));
				oprCmp.Items.Add(new ListItem("Promedio de todos los valores","AVG"));
				oprCmp.Items.Add(new ListItem("Cuenta la cantidad de registros","COUNT"));
				oprCmp.Items.Add(new ListItem("Mostrar el valor maximo del conjunto","MAX"));
				oprCmp.Items.Add(new ListItem("Mostrar el valor minimo del conjunto","MIN"));
				oprCmp.Items.Add(new ListItem("Valor absoluto","ABS"));
			}
		}
		
		protected void Preparar_DdlMascaras(int opcion)
		{
			mscrVlr.Items.Clear();
			if(opcion == 0)
				mscrVlr.Items.Add(new ListItem("Ninguna",""));
			else
			{
				mscrVlr.Items.Add(new ListItem("Ninguna",""));
				mscrVlr.Items.Add(new ListItem("Formato de Moneda","FM"));
			}
		}
		
		protected void Limpiar_Configuracion()
		{
			etCmp.Text = "";
			ordPst.Text = "";
		}
		
		protected void PrepararDtCmpsRpt()
		{
			dtCmpsRpt = new DataTable();
			dtCmpsRpt.Columns.Add(new DataColumn("TABLA",System.Type.GetType("System.String")));//0
			dtCmpsRpt.Columns.Add(new DataColumn("NOMBRECAMPO",System.Type.GetType("System.String")));//1
			dtCmpsRpt.Columns.Add(new DataColumn("ETIQUETA",System.Type.GetType("System.String")));//2
			dtCmpsRpt.Columns.Add(new DataColumn("ORDEN",System.Type.GetType("System.String")));//3
			dtCmpsRpt.Columns.Add(new DataColumn("OPERACION",System.Type.GetType("System.String")));//4
			dtCmpsRpt.Columns.Add(new DataColumn("MASCARA",System.Type.GetType("System.String")));//5
			dtCmpsRpt.Columns.Add(new DataColumn("VALORINTERPRETADO",System.Type.GetType("System.String")));//6
			dtCmpsRpt.Columns.Add(new DataColumn("RELACIONTABLAPRINCIPAL",System.Type.GetType("System.String")));//7
			//La relacion con la tabla principal es de :
			//0 : Es la misma tabla principal
			//1 : Es referencia de la tabla principal
			//2 : Hace referencia a la tabla principal
		}
		
		protected void Bind_dgCmpsRpt()
		{
			Session["dtCmpsRpt"] = dtCmpsRpt;
			dgCmpsRpt.DataSource = dtCmpsRpt;
			dgCmpsRpt.DataBind();
		}
		
		protected bool RevisarSiExiste(string sep, string rev)
		{
			bool exist = false;
			string[] seps = sep.Split('-');
			for(int i=0;i<seps.Length;i++)
			{
				if(seps[i].Trim() == rev)
					exist = true;
			}
			return exist;
		}
		
		protected void BindCamposTablas(DropDownList ddl, string tabla)
		{
			DatasToControls bind = new DatasToControls();
			if(tabla.IndexOf("-")==-1)
				bind.PutDatasIntoDropDownList(ddl,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+tabla+"'");
			else
			{
				string[] sep = tabla.Split('-');
				bind.PutDatasIntoDropDownList(ddl,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+sep[0]+"'");
			}
		}
		
		protected string RetornarCaminoAccesso(string tabla1, string tabla2)
		{
			string camino = "";
			//////
			DataSet fkcolname = new DataSet();
			DBFunctions.Request(fkcolname,IncludeSchema.NO,"SELECT colname FROM sysibm.syskeycoluse WHERE constname IN (SELECT relname FROM sysibm.sysrels WHERE tbname='"+tabla1+"' AND reftbname='"+tabla2+"') ORDER BY colseq");
			string rlcolname = DBFunctions.SingleData("SELECT colnames FROM sysibm.sysindexes WHERE name IN (SELECT refkeyname FROM sysibm.sysrels WHERE tbname='"+tabla1+"' AND reftbname='"+tabla2+"')");
			string[] rlcolnames = rlcolname.Split('+');
			if(fkcolname.Tables[0].Rows.Count == rlcolnames.Length-1)
			{
				for(int j=0;j<fkcolname.Tables[0].Rows.Count;j++)
					camino += " "+tabla1+"."+fkcolname.Tables[0].Rows[j][0].ToString().Trim()+"="+tabla2+"."+rlcolnames[j+1].Trim()+" AND";
			}
			//////
			return camino;
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
