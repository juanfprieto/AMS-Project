using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using AMS.DB;
using AMS.Forms;

namespace AMS.Web
{
	public partial class ConfigurarOperacion : System.Web.UI.Page
	{
		protected DataTable dtOpers;
		
		public ConfigurarOperacion()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				if(Session["dtOpers"]!=null)
					Session.Remove("dtOpers");
				nomTabla.Text = Request.QueryString["tabla"];
				nomCampo.Text = Request.QueryString["campo"];
				tipDato.Text = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+nomCampo.Text+"' AND tbname='"+Request.QueryString["tabla"]+"'").Trim();
				this.Preparar_ddlOperacionesOwn(ddlOper1,tipDato.Text);
				this.Preparar_ddlOperacionesExt(ddlOper2,tipDato.Text);
				this.Preparar_DdlTablas(ddlTablas,Request.QueryString["tabla"]);
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"' AND coltype='"+tipDato.Text+"'");
				this.Preparar_ddlOperacionesOwn(ddlOper3,tipDato.Text);
				btnCnclF.Attributes.Add("onclick","window.close();");
			}
			if(Session["dtOpers"]==null)
				this.Preparar_dtOpers();
			else
				dtOpers = (DataTable)Session["dtOpers"];
		}
		
		#region Eventos
		
		protected void Cambio_Tabla(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"' AND coltype='"+tipDato.Text+"'");
		}
			
		protected void Configurar_Opcion(Object  Sender, EventArgs e)
		{
			if(tipDato.Text == "DATE" || tipDato.Text == "TIME")
				Response.Write("<script language:javascript>alert('No existen operaciones de relación configuradas para este tipo de dato');</script>");
			else
			{
				if(opcionesRel.SelectedValue == "C")
				{
					confConst.Visible = true;
					this.Acciones_Controles_Configuracion(false);
					if(tipDato.Text == "VARCHAR" || tipDato.Text == "CHARACTER" || tipDato.Text == "LONG VARCHAR" || tipDato.Text == "CHAR")
					{
						chkSpace.Visible = true;
						chkSpace.Checked = false;
					}
					else
					{
						chkSpace.Visible = false;
						chkSpace.Checked = false;
					}		
				}
				else if(opcionesRel.SelectedValue == "V")
				{
					confVar.Visible = true;
					this.Acciones_Controles_Configuracion(false);
				}
			}
		}
		
		protected void Cancelar_Conf1(Object  Sender, EventArgs e)
		{
			confConst.Visible = false;
			this.Acciones_Controles_Configuracion(true);
		}
		
		protected void Cancelar_Conf2(Object  Sender, EventArgs e)
		{
			confVar.Visible = false;
			this.Acciones_Controles_Configuracion(true);
		}
		
		protected void Agregar_Operacion(Object  Sender, EventArgs e)
		{
			DataRow[] selection = dtOpers.Select("OPERACION=''");
			if(selection.Length==0)
			{
				if(tipDato.Text == "DATE" || tipDato.Text == "TIME")
					Response.Write("<script language:javascript>alert('No existen operaciones de relación configuradas para este tipo de dato');</script>");
				else
				{
					DataRow fila = dtOpers.NewRow();
					if(ddlOper1.SelectedValue=="")
						fila[0] = nomTabla.Text+"."+nomCampo.Text;
					else
						fila[0] = ddlOper1.SelectedValue+"("+nomTabla.Text+"."+nomCampo.Text+")";
					fila[1] = "";
					fila[2] = nomTabla.Text+"."+nomCampo.Text;
					fila[3] = nomTabla.Text;
					dtOpers.Rows.Add(fila);
					this.Bind_dgOpers();
				}
			}
			else
				Response.Write("<script language:javascript>alert('Ya se ha agregado una operacion sobre el mismo campo');</script>");
		}
		
		protected void Aceptar_Conf1(Object  Sender, EventArgs e)
		{
			bool error = false;
			if(tipDato.Text == "BIGINT" || tipDato.Text == "INTEGER" || tipDato.Text == "SMALLINT" || tipDato.Text == "DATE" || tipDato.Text == "TIME")
			{
				if(!DatasToControls.ValidarInt(vlrConst.Text))
					error = true;
			}
			else if(tipDato.Text == "DECIMAL" || tipDato.Text == "DOUBLE" || tipDato.Text == "REAL")
			{
				if(!DatasToControls.ValidarDouble(vlrConst.Text))
					error = true;
			}
			if(!error)
			{
				DataRow fila = dtOpers.NewRow();
				if(chkSpace.Checked)
					fila[0] = "nbsp";
				else
				{
					if(tipDato.Text == "VARCHAR" || tipDato.Text == "CHARACTER" || tipDato.Text == "LONG VARCHAR" || tipDato.Text == "CHAR")
						fila[0] = "'"+vlrConst.Text+"'";
					else
						fila[0] = vlrConst.Text;
				}
				fila[1] = ddlOper2.SelectedValue;
				fila[2] = "";
				fila[3] = "";
				dtOpers.Rows.Add(fila);
				this.Bind_dgOpers();
				confConst.Visible = false;
				this.Acciones_Controles_Configuracion(true);
				vlrConst.Text = "";
			}
			else
				Response.Write("<script language:javascript>alert('El valor de la constante no es compatible con el tipo de dato');</script>");
		}
		
		protected void Aceptar_Conf2(Object  Sender, EventArgs e)
		{
			DataRow fila = dtOpers.NewRow();
			if(ddlOper3.SelectedValue == "")
				fila[0] = ddlTablas.SelectedValue+"."+ddlCampos.SelectedValue;
			else
				fila[0] = ddlOper3.SelectedValue+"("+ddlTablas.SelectedValue+"."+ddlCampos.SelectedValue+")";
			fila[1] = ddlOper2.SelectedValue;
			fila[2] = ddlTablas.SelectedValue+"."+ddlCampos.SelectedValue;
			fila[3] = ddlTablas.SelectedValue;
			dtOpers.Rows.Add(fila);
			this.Bind_dgOpers();
			confVar.Visible = false;
			this.Acciones_Controles_Configuracion(true);
		}
		
		protected void Construir_Valor(Object  Sender, EventArgs e)
		{
			string result = "";
			string tables = "";
			//Debemos revisar si se ha agregado una operacion sobre el mismo campo, si no informamos que debe hacerlo
			if(tipDato.Text == "VARCHAR" || tipDato.Text == "CHARACTER" || tipDato.Text == "LONG VARCHAR" || tipDato.Text == "CHAR")
			{
				DataRow[] selection = dtOpers.Select("OPERACION=''");
				if(selection.Length>0)
				{
					result += selection[0][0].ToString();
					tables += selection[0][3].ToString()+"-";
				}
				else
				{
					result += nomTabla.Text +"."+nomCampo.Text;
					tables += nomTabla.Text+"-";
				}
				///////////////////////////////////////////////////////////////////////
				for(int i=0;i<dtOpers.Rows.Count;i++)
				{
					if(dtOpers.Rows[i][1].ToString()!="")
					{
						if(dtOpers.Rows[i][0].ToString()=="nbsp")
							result += " "+ dtOpers.Rows[i][1].ToString()+" \\'  \\' ";
						else
						{
							if(dtOpers.Rows[i][2].ToString()== "")
								result += " "+dtOpers.Rows[i][1].ToString()+" \\'"+dtOpers.Rows[i][0].ToString()+"\\' ";
							else
								result += " "+dtOpers.Rows[i][1].ToString()+" "+dtOpers.Rows[i][0].ToString()+" ";
						}
						if(dtOpers.Rows[i][3].ToString()!="")
							tables += dtOpers.Rows[i][3].ToString()+"-";
					}
				}
				btnConstruir.Visible = false;
				btnAcptF.Visible = true;
				if(tables.Length>0)
					tables = tables.Substring(0,tables.Length-1);
				btnAcptF.Attributes.Add("onclick","window.returnValue = '"+result+"@"+tables+"';window.close();");
				Session.Remove("dtOpers");
				
			}
			else
			{
				DataRow[] selection = dtOpers.Select("OPERACION=''");
				if(selection.Length>0)
				{
					//Primero debemos agregar la operacion sobre el mismo campo
					result += selection[0][0].ToString();
					//Ahora recorremos la tabla de operaciones y las vamos adicionado en el orden que las agregaron
					for(int i=0;i<dtOpers.Rows.Count;i++)
					{
						//Ahora revisamos el caso en que la operacion sea una concatenacion de cadena de caracteres
						if(dtOpers.Rows[i][1].ToString()=="+" || dtOpers.Rows[i][1].ToString()=="-")
							result += " "+dtOpers.Rows[i][1].ToString()+" "+dtOpers.Rows[i][0].ToString();
						else if(dtOpers.Rows[i][1].ToString()=="*" || dtOpers.Rows[i][1].ToString()=="/")
						{
							result = "("+result;
							result += " "+dtOpers.Rows[i][1].ToString()+" "+dtOpers.Rows[i][0].ToString()+")";
						}
						if(dtOpers.Rows[i][3].ToString()!="")
							tables += dtOpers.Rows[i][3].ToString()+"-";
					}
					btnConstruir.Visible = false;
					btnAcptF.Visible = true;
					if(tables.Length>0)
						tables = tables.Substring(0,tables.Length-1);
					btnAcptF.Attributes.Add("onclick","window.returnValue = '"+result+" @ "+tables+"';window.close();");
					Session.Remove("dtOpers");
				}
				else
					Response.Write("<script language:javascript>alert('Debe Agregarse una operación sobre el mismo campo');</script>");
			}
		}
		
		#endregion
		
		#region Otros
		protected void Preparar_ddlOperacionesOwn(DropDownList ddl, string tipoDato)
		{
			ddl.Items.Clear();
			if(tipoDato == "VARCHAR" || tipoDato == "CHARACTER" || tipoDato == "LONG VARCHAR" || tipoDato == "CHAR")
			{
				ddl.Items.Add(new ListItem("Ninguna",""));
				ddl.Items.Add(new ListItem("Convertir en Mayusculas","UPPER"));
				ddl.Items.Add(new ListItem("Convertir en Minusculas","LOWER"));
			}
			else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
			{
				ddl.Items.Add(new ListItem("Sumatoria de todos los valores","SUM"));
				ddl.Items.Add(new ListItem("Promedio de todos los valores","AVG"));
				ddl.Items.Add(new ListItem("Cuenta la cantidad de registros","COUNT"));
				ddl.Items.Add(new ListItem("Mostrar el valor maximo del conjunto","MAX"));
				ddl.Items.Add(new ListItem("Mostrar el valor minimo del conjunto","MIN"));
				ddl.Items.Add(new ListItem("Valor absoluto","ABS"));
			}
			else if(tipoDato == "DATE" || tipoDato == "TIME")
				ddl.Items.Add(new ListItem("Ninguna",""));
		}
		
		protected void Preparar_ddlOperacionesExt(DropDownList ddl, string tipoDato)
		{
			ddl.Items.Clear();
			if(tipoDato == "VARCHAR" || tipoDato == "CHARACTER" || tipoDato == "LONG VARCHAR" || tipoDato == "CHAR")
			{
				ddl.Items.Add(new ListItem("Concatenación","CONCAT"));
			}
			else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
			{
				ddl.Items.Add(new ListItem("Suma","+"));
				ddl.Items.Add(new ListItem("Resta","-"));
				ddl.Items.Add(new ListItem("Multiplicación","*"));
				ddl.Items.Add(new ListItem("División","/"));
			}
			else if(tipoDato == "DATE" || tipoDato == "TIME")
				ddl.Items.Add(new ListItem("Ninguna",""));
		}
		
		protected void Acciones_Controles_Configuracion(bool accion)
		{
			ddlOper2.Enabled = accion;
			opcionesRel.Enabled = accion;
			btnConf.Enabled = accion;
		}
		
		protected void Preparar_DdlTablas(DropDownList ddl, string tablas)
		{
			string[] tablasFilas = tablas.Split('-');
			for(int i=0;i<tablasFilas.Length;i++)
			{
				ddl.Items.Add(tablasFilas[i]);
				//Ahora traemos las tablas relacionadas con la tabla principal del reporte
				DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DISTINCT reftbname FROM sysibm.sysrels WHERE tbname = '"+tablasFilas[i]+"'");
				for(int j=0;j<ds.Tables[0].Rows.Count;j++)
					ddl.Items.Add(ds.Tables[0].Rows[j][0].ToString());
			}
		}
		
		protected void Preparar_dtOpers()
		{
			dtOpers = new DataTable();
			dtOpers.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));//0
			dtOpers.Columns.Add(new DataColumn("OPERACION",System.Type.GetType("System.String")));//1
			dtOpers.Columns.Add(new DataColumn("OBJETO",System.Type.GetType("System.String")));//2
			dtOpers.Columns.Add(new DataColumn("TABLA",System.Type.GetType("System.String")));//3
		}

		private void InitializeComponent()
		{
		
		}
			
		protected void Bind_dgOpers()
		{
			Session["dtOpers"] = dtOpers;
			dgOpers.DataSource = dtOpers;
			dgOpers.DataBind();
		}
		
		#endregion
	}
}
