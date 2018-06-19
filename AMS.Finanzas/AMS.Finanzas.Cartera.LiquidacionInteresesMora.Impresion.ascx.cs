using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial  class ImpresionMora : System.Web.UI.UserControl
	{
		protected ReportDocument reporte;
		protected DataSet ds;
		protected DataTable tbLetras;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,Armar_Select()+";"+
			                   "SELECT MNIT.mnit_nit,CEMP.cemp_nombre,CEMP.cemp_direccion,MNIT.mnit_telefono,TREG.treg_nombre CONCAT''CONCAT 'Nº 'CONCAT' 'CONCAT CEMP.cemp_numeregiiva CONCAT' 'CONCAT 'de 'CONCAT''CONCAT CAST(CEMP.cemp_regiiva AS char(10)),MNIT.mnit_web,PCIU.pciu_nombre FROM dbxschema.mnit MNIT,dbxschema.cempresa CEMP,dbxschema.tregimeniva TREG,dbxschema.pciudad PCIU WHERE MNIT.mnit_nit=CEMP.mnit_nit AND CEMP.cemp_indiregiiva=TREG.treg_regiiva AND PCIU.pciu_codigo=CEMP.cemp_ciudad;"+
			                   "SELECT 'Numeración autorizada por la DIAN Resolución Nº ' CONCAT pdoc_numeresofact CONCAT ' Fecha ' CONCAT CAST(pdoc_fechresofact AS char(10)) CONCAT ' Rango con prefijo ' CONCAT pdoc_codigo CONCAT ' Nº ' CONCAT CAST(pdoc_numeinic AS char(10)) CONCAT ' al Nº ' CONCAT CAST(pdoc_numefina AS char(10)) FROM dbxschema.pdocumento WHERE pdoc_codigo='"+(((ArrayList)Session["final"])[0].ToString().Split('-'))[0].ToString()+"'");
				this.Sacar_Letras();
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Finanzas.Cartera.rpte_ImpFacMora.xsd"));
				reporte=new ReportDocument();
				reporte.Load(Path.Combine(Request.PhysicalApplicationPath,"rpt/Finanzas.Cartera.rpte_ImpFacMora.rpt"));
				reporte.SetDataSource(ds);
				visor.ReportSource=reporte;
				visor.DataBind();
				reporte.ExportToDisk(ExportFormatType.WordForWindows,Path.Combine(Request.PhysicalApplicationPath,"rptgen/Finanzas.Cartera.rpte_ImpFacMora.doc"));
			}
		}
		
		protected string Armar_Select()
		{
			string sel="";
			for(int i=0;i<((ArrayList)Session["final"]).Count;i++)
			{
				string select="SELECT 'Intereses por Mora de la Factura' CONCAT' 'CONCAT DFAC.pdoc_codirefe CONCAT'-'CONCAT CAST(DFAC.mfac_numerela AS char(4)),MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,MNIT.mnit_nit,MNIT.mnit_direccion,MNIT.mnit_telefono,PCIU.pciu_nombre,MFAC.pdoc_codigo,MFAC.mfac_numedocu,DFAC.dfac_valor,DFAC.dfac_dias,MFAC.mfac_valofact,MFAC.mfac_valoiva FROM dbxschema.mnit MNIT,dbxschema.pciudad PCIU,dbxschema.mfacturacliente MFAC,dbxschema.dfacturaclientemora DFAC WHERE MNIT.pciu_codigo=PCIU.pciu_codigo AND MFAC.mnit_nit=MNIT.mnit_nit AND MFAC.pdoc_codigo=DFAC.pdoc_codigo AND MFAC.mfac_numedocu=DFAC.mfac_numedocu";
				string[] partes=((ArrayList)Session["final"])[i].ToString().Split('-');
				if(i<((ArrayList)Session["final"]).Count-1)
					select+=" AND MFAC.pdoc_codigo='"+partes[0]+"' AND MFAC.mfac_numedocu="+partes[1]+" UNION ALL ";
				else if(i==((ArrayList)Session["final"]).Count-1)
					select+=" AND MFAC.pdoc_codigo='"+partes[0]+"' AND MFAC.mfac_numedocu="+partes[1]+"";
				sel+=select;
			}
			return sel;
		}
		
		protected void Sacar_Letras()
		{
			Interprete inter=new Interprete();
			DataRow fila;
			for(int i=0;i<((ArrayList)Session["final"]).Count;i++)
			{
				double valor=Math.Round(Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoiva FROM dbxschema.mfacturacliente WHERE pdoc_codigo='"+(((ArrayList)Session["final"])[i].ToString().Split('-'))[0]+"' AND mfac_numedocu="+(((ArrayList)Session["final"])[i].ToString().Split('-'))[1]+"")),2);
				string letra=inter.Letras(valor.ToString());

				if(tbLetras==null)
					this.Preparar_tbLetras();
				fila=tbLetras.NewRow();
				fila[0]=(((ArrayList)Session["final"])[i].ToString().Split('-'))[0];
				fila[1]=Convert.ToInt32((((ArrayList)Session["final"])[i].ToString().Split('-'))[1]);
				fila[2]=valor;
				fila[3]=letra;
				tbLetras.Rows.Add(fila);
			}
			ds.Tables.Add(tbLetras);
			ds.AcceptChanges();
		}
		
		protected void Preparar_tbLetras()
		{
			tbLetras=new DataTable();
			tbLetras.Columns.Add("PREFIJO",typeof(string));
			tbLetras.Columns.Add("NUMERO",typeof(int));
			tbLetras.Columns.Add("VALOR",typeof(double));
			tbLetras.Columns.Add("LETRAS",typeof(string));
		}
		
		protected void btnImprimir_Click(object Sender,EventArgs e)
		{
			Response.ClearContent();
			Response.ClearHeaders();
			Response.ContentType = "application/msword";
			Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Finanzas.Cartera.rpte_ImpFacMora.doc"));
			Response.Flush();
			Response.Close();
		}
		
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
