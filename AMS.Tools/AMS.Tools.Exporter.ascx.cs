using System.IO;
using System.Data.Common;
using System.Configuration;
using System;
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
	/// <summary>
	///		Descripción breve de AMS_Tools_Exporter.
	/// </summary>
	public partial class Exporter : System.Web.UI.UserControl
	{
		protected int limite=10000;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(tablaAct, "Select NAME from SYSIBM.SYSTABLES WHERE ((NAME LIKE 'M%') OR (NAME LIKE 'P%')  OR (NAME LIKE 'D%') OR (NAME LIKE 'S%') OR (NAME LIKE 'T%')) AND CREATOR='DBXSCHEMA' ORDER BY NAME");
				bind.PutDatasIntoDropDownList(ddlAno, "SELECT pano_ano ,pano_detalle FROM pano;");
				bind.PutDatasIntoDropDownList(ddlMes, "SELECT pmes_mes, pmes_nombre FROM pmes;");
				CalcularTamano();
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

		}
		#endregion

		
		private void CalcularTamano()
		{
			string tabla=tablaAct.SelectedValue;
			int tamano=0;
			if(tabla.Equals("MCOMPROBANTE")||tabla.Equals("DCUENTA"))
			{
				pnlFecha.Visible=true;
				if(tabla.Equals("MCOMPROBANTE"))
					tamano=Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM "+tabla+" TBL "+
						"WHERE TBL.PANO_ANO="+ddlAno.SelectedValue+" AND TBL.PMES_MES="+ddlMes.SelectedValue+";"));
				else if(tabla.Equals("DCUENTA"))
					tamano=Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM "+tabla+" TBL, MCOMPROBANTE MC "+
						" WHERE MC.PDOC_CODIGO=TBL.PDOC_CODIGO AND MC.MCOM_NUMEDOCU=TBL.MCOM_NUMEDOCU AND "+
						" MC.PANO_ANO="+ddlAno.SelectedValue+" AND MC.PMES_MES="+ddlMes.SelectedValue+";"));
			}
			else
			{
				pnlFecha.Visible=false;
				tamano=Convert.ToInt32(DBFunctions.SingleData("SELECT Count(*) FROM "+tabla+";"));
			}
			lblTotal.Text=tamano.ToString();
			ddlRegistros.Items.Clear();
			for(int n=1;n<=tamano;n+=limite)
			{
				int ultP=n+limite-1;
				if(ultP>tamano)ultP=tamano;
				ListItem lstPagina=new ListItem(n+"-"+ultP,n+"-"+ultP);
				ddlRegistros.Items.Add(lstPagina);
			}
			actual.Enabled=ddlRegistros.Items.Count>0;
		}

		public void actual_Click(object sender, System.EventArgs e){
			DataSet dsTabla=new DataSet();
			DataTable dtTabla;
			string tabla=tablaAct.SelectedValue.Trim();
			string sqlC="";
			int desde=0,hasta=0;
			if(ddlRegistros.Items.Count==0)return;
			string []limites=ddlRegistros.SelectedValue.Split('-');
			desde=Convert.ToInt32(limites[0]);
			hasta=Convert.ToInt32(limites[1]);
			sqlC="SELECT * FROM("+
				" SELECT rownumber() over() EXPORTER_PAGNUM, TBL.* "+
				" FROM "+tabla+" TBL) AS TBE "+
				"WHERE EXPORTER_PAGNUM BETWEEN "+desde+" AND "+hasta+";";

			if(tabla.Equals("MCOMPROBANTE")||tabla.Equals("DCUENTA"))
			{
				pnlFecha.Visible=true;
				if(tabla.Equals("MCOMPROBANTE"))
					sqlC="SELECT * FROM("+
						" SELECT rownumber() over() EXPORTER_PAGNUM, TBL.* "+
						" FROM "+tabla+" TBL WHERE TBL.PANO_ANO="+ddlAno.SelectedValue+" AND TBL.PMES_MES="+ddlMes.SelectedValue+" ) AS TBE "+
						"WHERE EXPORTER_PAGNUM BETWEEN "+desde+" AND "+hasta+";";
				else if(tabla.Equals("DCUENTA"))
					sqlC="SELECT * FROM("+
						" SELECT rownumber() over() EXPORTER_PAGNUM, TBL.* "+
						" FROM "+tabla+" TBL, MCOMPROBANTE MC "+
						" WHERE MC.PDOC_CODIGO=TBL.PDOC_CODIGO AND MC.MCOM_NUMEDOCU=TBL.MCOM_NUMEDOCU AND "+
						" MC.PANO_ANO="+ddlAno.SelectedValue+" AND MC.PMES_MES="+ddlMes.SelectedValue+
						") AS TBE "+
						"WHERE EXPORTER_PAGNUM BETWEEN "+desde+" AND "+hasta+";";
			}
			else pnlFecha.Visible=false;

			DBFunctions.Request(dsTabla,IncludeSchema.YES,sqlC);
			dtTabla=dsTabla.Tables[0];
			//Excel
            //ddlFormato.SelectedValue.Equals(("e",bool,true));
			if(ddlFormato.SelectedValue.Equals("E"))
			{
				//Generar DataGrid
				for(int n=1;n<dtTabla.Columns.Count;n++)
				{
					BoundColumn dgrColumna=new BoundColumn();
					string nombreCol=dtTabla.Columns[n].ColumnName;
					string formato="";
					string tipoCol=DBFunctions.SingleData(
							"SELECT COLTYPE FROM SYSIBM.SYSCOLUMNS "+
							"WHERE NAME='"+nombreCol+"' AND TBNAME='"+tabla+"' AND "+
							"TBCREATOR='DBXSCHEMA';"
						).Trim();
					dgrColumna.DataField=nombreCol;
					dgrColumna.HeaderText=nombreCol;
					//Revisar tipo
					dgrColumna.ItemStyle.HorizontalAlign=HorizontalAlign.Left;
					if(tipoCol.Equals("DATE"))formato="{0:yyyy-MM-dd}";
					else if(tipoCol.Equals("DECIMAL")){formato="{0:N}";dgrColumna.ItemStyle.HorizontalAlign=HorizontalAlign.Right;}
					else if(tipoCol.Equals("DOUBLE")){formato="{0:N}";dgrColumna.ItemStyle.HorizontalAlign=HorizontalAlign.Right;}
					else if(tipoCol.Equals("TIME"))formato="{0:HH:mm:ss}";
					else if(tipoCol.Equals("TIMESTMP"))formato="{0:yyyy-MM-dd HH:mm:ss}";
					if(formato.Length>0)
						dgrColumna.DataFormatString=formato;
					dgTabla.Columns.Add(dgrColumna);
				}

				dgTabla.DataSource=dsTabla.Tables[0];
				dgTabla.DataBind();//
				Response.Clear();
				Response.AddHeader("content-disposition", "attachment;filename=AMS.Exporter."+tablaAct.SelectedValue+"("+ddlRegistros.SelectedIndex+").xls");
				Response.Charset = "Unicode";
				Response.Cache.SetCacheability(HttpCacheability.NoCache);
				Response.ContentType = "application/vnd.xls";
				System.IO.StringWriter stringWrite = new System.IO.StringWriter();
				System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
				dgTabla.RenderControl(htmlWrite);
				Response.Write(stringWrite.ToString());
				Response.End();
			}
		}

		protected void tablaAct_SelectedIndexChanged(object sender, System.EventArgs e){
			CalcularTamano();
		}

		protected void ddlMes_SelectedIndexChanged(object sender, System.EventArgs e){
			CalcularTamano();
		}

		protected void ddlAno_SelectedIndexChanged(object sender, System.EventArgs e){
			CalcularTamano();
		}

        protected void ddlFormato_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
	}
}