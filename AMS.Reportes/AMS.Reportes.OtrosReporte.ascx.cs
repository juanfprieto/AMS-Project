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
	public partial class Otros : System.Web.UI.UserControl 
	{
		#region Atributos
		//Controles filas estaticas
		//Controles filas dinamicas
		//Otros
		protected DataTable dtFilas;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
		
		#region Eventos
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				this.Preparar_DdlTablas(ddlTablas,Request.QueryString["tabla"]);
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"'");
				btnConfi.Attributes.Add("onclick","CuadroConfiguracion('"+tbResultado.ClientID+"','"+ddlTablas.SelectedValue+"','"+ddlCampos.SelectedValue+"')");
			}
			if(Session["dtFilas"]==null)
				this.Preparar_dtFilas();
			else
				dtFilas = (DataTable)Session["dtFilas"];
		}
		
		protected void Cambio_Tabla(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"'");
			btnConfi.Attributes.Remove("onclick");
			btnConfi.Attributes.Add("onclick","CuadroConfiguracion('"+tbResultado.ClientID+"','"+ddlTablas.SelectedValue+"','"+ddlCampos.SelectedValue+"')");
		}
		
		protected void Cambio_Campo(Object  Sender, EventArgs e)
		{
			btnConfi.Attributes.Remove("onclick");
			btnConfi.Attributes.Add("onclick","CuadroConfiguracion('"+tbResultado.ClientID+"','"+ddlTablas.SelectedValue+"','"+ddlCampos.SelectedValue+"')");
		}
		
		protected void Agregar_Estatica(Object  Sender, EventArgs e)
		{
			//Primero revisamos si ya existe una fila en esta posicion
			DataRow[] selection = dtFilas.Select("POSICION='"+posRep1.SelectedValue+"' AND ORDEN='"+ordPrs1.Text+"'");
			if(selection.Length>0)
                Utils.MostrarAlerta(Response, "Ya Existe una fila en esta posición");
			else
			{
				if(DatasToControls.ValidarInt(ordPrs1.Text))
				{
					DataRow fila = dtFilas.NewRow();
					fila[0] = posRep1.SelectedValue;
					fila[1] = ordPrs1.Text;
					fila[2] = alin1.SelectedValue;
					fila[3] = text1.Text;
					fila[4] = rdOpc.SelectedValue;
					fila[5] = "";
					dtFilas.Rows.Add(fila);
					this.Bind_dgFilas();
					this.Reiniciar_Controles_Estaticas();
				}
				else
                Utils.MostrarAlerta(Response, "Orden de Presentación No Valida. Revisar Por Favor ");
			}
		}
		
		protected void Aceptar_Dinamica(Object  Sender, EventArgs e)
		{
			//Primero revisamos si existe algun valor en el textbox de resultados de la configuracion
			if(tbResultado.Text!="")
			{
				DataRow[] selection = dtFilas.Select("POSICION='"+posRep2.SelectedValue+"' AND ORDEN='"+ordPrs2.Text+"'");
				if(selection.Length>0)
                    Utils.MostrarAlerta(Response, "Ya Existe una fila en esta posición");
				else
				{
					if(DatasToControls.ValidarInt(ordPrs2.Text))
					{
						string []results = tbResultado.Text.Split('@');
						DataRow fila = dtFilas.NewRow();
						fila[0] = posRep2.SelectedValue;
						fila[1] = ordPrs2.Text;
						fila[2] = alin2.SelectedValue;
						if(text2.Text!="")
							fila[3] = text2.Text+"&"+this.Preparar_Select(Request.QueryString["sql"],results[0],results[1]);
							//lb.Text +="<br>" +this.Preparar_Select(Request.QueryString["sql"],results[0],results[1]);
						else
							fila[3] = this.Preparar_Select(Request.QueryString["sql"],results[0],results[1]);
						fila[4] = "";
						fila[5] = results[1];
						dtFilas.Rows.Add(fila);
						this.Bind_dgFilas();
						this.Reiniciar_Controles_Estaticas();
						tbResultado.Text = "";
					}
					else
                    Utils.MostrarAlerta(Response, "Orden de Presentación No Valida. Revisar Por Favor");
				}
			}
			else
            Utils.MostrarAlerta(Response, "No se ha configurado el valor de esta fila dinamica. Revisar Por Favor");
		}
		
		protected void Aceptar_Filas(Object  Sender, EventArgs e)
		{
			//Entonces lo unico que hacemos es direccionar hacia la ultima pantalla donde se mostrara lo que ya se ha configurado y donde se guardara el reporte
			Response.Redirect("" + indexPage + "?process=Reportes.FinalReporte&tabla="+Request.QueryString["tabla"]+"&sql="+Request.QueryString["sql"]+"&filters="+Request.QueryString["filters"]+"&mask="+Request.QueryString["mask"]+"");
		}
		
		#endregion
		
		#region Otros
		
		protected string Preparar_Select(string sqlOriginal, string newCore, string tables)
		{
			//Primero dividimos el select en sus partes basicas
			string sql1 = sqlOriginal.Substring(0,sqlOriginal.IndexOf("FROM"));//Del select hasta el from
			//string sql2 = sqlOriginal.Substring(sqlOriginal.IndexOf("FROM"),sqlOriginal.IndexOf("WHERE")-(sqlOriginal.IndexOf("FROM")+4));//del from al where
			string sql2 = sqlOriginal.Substring(sqlOriginal.IndexOf("FROM")+4,sqlOriginal.IndexOf("WHERE")-(sqlOriginal.IndexOf("FROM")+4));//del from al where
			string sql3 = "";//del where hasta el final o el group by
			string sql4 = "";//despues de group by
			string newSql = "";
			string mainTable = Request.QueryString["tabla"];
			if(sqlOriginal.IndexOf("GROUP BY")==-1)
				sql3 = sqlOriginal.Substring(sqlOriginal.IndexOf("WHERE"));
			else
			{
				sql3 = sqlOriginal.Substring(sqlOriginal.IndexOf("WHERE"),sqlOriginal.IndexOf("GROUP BY")-(sqlOriginal.IndexOf("WHERE")));
				sql4 = sqlOriginal.Substring(sqlOriginal.IndexOf("GROUP BY")+8);
			}
			newSql = "SELECT "+newCore+" FROM ";
			sql3 += " AND";
			//Empezamos a adicionar las tablas
			string[] tablesS = tables.Split('-');
			for(int i=0;i<tablesS.Length;i++)
			{
				if(!this.Revisar_Si_Existe(sql2,tablesS[i]))
				{
					sql2 += " "+tablesS[i]+" ,";
					//lb.Text += "<br>aqui hay problema "+sql2;
					DataSet fkcolname = new DataSet();
					DBFunctions.Request(fkcolname,IncludeSchema.NO,"SELECT colname FROM sysibm.syskeycoluse WHERE constname=(SELECT relname FROM sysibm.sysrels WHERE tbname='"+mainTable+"' AND reftbname='"+tablesS[i]+"') ORDER BY colseq");
					string rlcolname = DBFunctions.SingleData("SELECT colnames FROM sysibm.sysindexes WHERE name=(SELECT refkeyname FROM sysibm.sysrels WHERE tbname='"+mainTable+"' AND reftbname='"+tablesS[i]+"')");
					string[] rlcolnames = rlcolname.Split('+');
					for(int j=0;j<fkcolname.Tables[0].Rows.Count;j++)
						sql3 += " "+mainTable.Trim()+"."+fkcolname.Tables[0].Rows[j][0].ToString().Trim()+"="+tablesS[i].Trim()+"."+rlcolnames[j+1].Trim()+" AND";
				}
			}
			newSql += sql2.Substring(0,sql2.Length-1)+" "+sql3.Substring(0,sql3.Length-3);
			return newSql;
		}
		
		protected bool Revisar_Si_Existe(string sql2, string newTable)
		{
			bool existe = false;
			string[] mat = sql2.Split(',');
			for(int i=0;i<mat.Length;i++)
			{
				if(mat[i].Trim()==newTable.Trim())
					existe = true;
			}
			return existe;
		}
		
		
		protected void Preparar_dtFilas()
		{
			dtFilas = new DataTable();
			dtFilas.Columns.Add(new DataColumn("POSICION",System.Type.GetType("System.String")));//0
			dtFilas.Columns.Add(new DataColumn("ORDEN",System.Type.GetType("System.String")));//1
			dtFilas.Columns.Add(new DataColumn("ALINEACION",System.Type.GetType("System.String")));//2
			dtFilas.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));//3
			dtFilas.Columns.Add(new DataColumn("OPCION",System.Type.GetType("System.String")));//4
			dtFilas.Columns.Add(new DataColumn("TABLAS",System.Type.GetType("System.String")));//5
		}
		
		protected void Bind_dgFilas()
		{
			Session["dtFilas"] = dtFilas;
			dgFilas.DataSource = dtFilas;
			dgFilas.DataBind();
		}
		
		protected void Reiniciar_Controles_Estaticas()
		{
			posRep1.SelectedIndex = 0;
			ordPrs1.Text = "";
			text1.Text = "";
			alin1.SelectedIndex = 0;
			rdOpc.SelectedIndex = 0;
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
