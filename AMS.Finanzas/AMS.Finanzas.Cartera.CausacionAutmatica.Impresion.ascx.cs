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
	public partial class ImpresionCausacionAutomatica : System.Web.UI.UserControl
	{
        protected FormatosDocumentos formatoRecibo = new FormatosDocumentos();
        protected ReportDocument reporte;
		protected DataSet ds;
		protected DataTable tbLetras;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			ds=new DataSet();
			reporte=new ReportDocument();
			if(Session["facs"]!=null)
			{
				if(Request.QueryString["tipo"]=="C")
				{
				        Utils.MostrarAlerta(Response, "Se ha creado la factura de cliente con prefijo " + Request.QueryString["pre"] + " y número " + Request.QueryString["num"] + "");
                        try
                        {
                            Imprimir.ImprimirRPT(Response, Request.QueryString["pre"], Convert.ToInt32(Request.QueryString["num"]), true);
                        }
                        catch
                        {
                            lb.Text += "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>";
                        }
                    
                }
				else if(Request.QueryString["tipo"]=="P")
				{
					DBFunctions.Request(ds,IncludeSchema.NO,Armar_Select_Pro()+";"+
					                    "SELECT MNIT.mnit_nit,CEMP.cemp_nombre,CEMP.cemp_direccion,MNIT.mnit_telefono,TREG.treg_nombre CONCAT''CONCAT 'Nº 'CONCAT' 'CONCAT CEMP.cemp_numeregiiva CONCAT' 'CONCAT 'de 'CONCAT''CONCAT CAST(CEMP.cemp_regiiva AS char(10)),MNIT.mnit_web,PCIU.pciu_nombre FROM dbxschema.mnit MNIT,dbxschema.cempresa CEMP,dbxschema.tregimeniva TREG,dbxschema.pciudad PCIU WHERE MNIT.mnit_nit=CEMP.mnit_nit AND CEMP.cemp_indiregiiva=TREG.treg_regiiva AND PCIU.pciu_codigo=CEMP.cemp_ciudad;"+
		                   				"SELECT 'Numeración autorizada por la DIAN Resolución Nº ' CONCAT pdoc_numeresofact CONCAT ' Fecha ' CONCAT CAST(pdoc_fechresofact AS char(10)) CONCAT ' Rango con prefijo ' CONCAT pdoc_codigo CONCAT ' Nº ' CONCAT CAST(pdoc_numeinic AS char(10)) CONCAT ' al Nº ' CONCAT CAST(pdoc_numefina AS char(10)) FROM dbxschema.pdocumento WHERE pdoc_codigo='"+(((ArrayList)Session["facs"])[0].ToString().Split('-'))[0].ToString()+"'");
					Sacar_Letras();
					ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Finanzas.Cartera.Finanzas.Cartera.rpte_ImpCauAutoPro.xsd"));
					reporte.Load(Path.Combine(Request.PhysicalApplicationPath,"rpt/Finanzas.Cartera.rpte_ImpCauAutoPro.rpt"));
					reporte.SetDataSource(ds);
					visor.ReportSource=reporte;
					visor.DataBind();
					reporte.ExportToDisk(ExportFormatType.WordForWindows,Path.Combine(Request.PhysicalApplicationPath,"rptgen/Finanzas.Cartera.rpte_ImpCauAutoPro.doc"));
				}
			}
		}
		
		protected string Armar_Select_Cli()
		{
			string sel="";
			for(int i=0;i<((ArrayList)Session["facs"]).Count;i++)
			{
				string select="SELECT MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,MNIT.mnit_nit,MNIT.mnit_direccion,MNIT.mnit_telefono,PCIU.pciu_nombre,MFAC.pdoc_codigo,MFAC.mfac_numedocu,DFAC.dfac_detalle,DFAC.dfac_valor,DFAC.dfac_valoriva,MFAC.mfac_factura,MFAC.mfac_valofact,MFAC.mfac_valoiva FROM dbxschema.mnit MNIT,dbxschema.pciudad PCIU,dbxschema.mfacturacliente MFAC,dbxschema.dfacturacliente DFAC WHERE MNIT.pciu_codigo=PCIU.pciu_codigo AND MFAC.mnit_nit=MNIT.mnit_nit AND MFAC.pdoc_codigo=DFAC.mfac_codigo AND MFAC.mfac_numedocu=DFAC.mfac_numero";
				string[] partes=((ArrayList)Session["facs"])[i].ToString().Split('-');
				if(i<((ArrayList)Session["facs"]).Count-1)
					select+=" AND MFAC.pdoc_codigo='"+partes[0]+"' AND MFAC.mfac_numedocu="+partes[1]+" UNION ALL ";
				else if(i==((ArrayList)Session["facs"]).Count-1)
					select+=" AND MFAC.pdoc_codigo='"+partes[0]+"' AND MFAC.mfac_numedocu="+partes[1]+"";
				sel+=select;
			}
			return sel;
		}
		
		protected string Armar_Select_Pro()
		{
			string sel="";
			for(int i=0;i<((ArrayList)Session["facs"]).Count;i++)
			{
				string select="SELECT MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,MNIT.mnit_nit,MNIT.mnit_direccion,MNIT.mnit_telefono,PCIU.pciu_nombre,MFAC.pdoc_codiordepago,MFAC.mfac_numeordepago,DFAC.dfac_detalle,DFAC.dfac_valor,DFAC.dfac_valoriva,MFAC.mfac_factura,MFAC.mfac_valofact,MFAC.mfac_valoiva FROM dbxschema.mnit MNIT,dbxschema.pciudad PCIU,dbxschema.mfacturaproveedor MFAC,dbxschema.dfacturaproveedor DFAC WHERE MNIT.pciu_codigo=PCIU.pciu_codigo AND MFAC.mnit_nit=MNIT.mnit_nit AND MFAC.pdoc_codiordepago=DFAC.mfac_codigo AND MFAC.mfac_numeordepago=DFAC.mfac_numero";
				string[] partes=((ArrayList)Session["facs"])[i].ToString().Split('-');
				if(i<((ArrayList)Session["facs"]).Count-1)
					select+=" AND MFAC.pdoc_codiordepago='"+partes[0]+"' AND MFAC.mfac_numeordepago="+partes[1]+" UNION ALL ";
				else if(i==((ArrayList)Session["facs"]).Count-1)
					select+=" AND MFAC.pdoc_codiordepago='"+partes[0]+"' AND MFAC.mfac_numeordepago="+partes[1]+"";
				sel+=select;
			}
			return sel;
		}
		
		protected void btnImprimir_Click(object Sender,EventArgs e)
		{
			Response.ClearContent();
			Response.ClearHeaders();
			Response.ContentType = "application/msword";
			if(Request.QueryString["tipo"]=="C")
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Finanzas.Cartera.rpte_ImpCauAutoCli.doc"));
			else if(Request.QueryString["tipo"]=="P")
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Finanzas.Cartera.rpte_ImpCauAutoPro.doc"));
			Response.Flush();
			Response.Close();
		}
		
		protected void Sacar_Letras()
		{
			Interprete inter=new Interprete();
			DataRow fila;
			if(Request.QueryString["tipo"]=="C")
			{
				for(int i=0;i<((ArrayList)Session["facs"]).Count;i++)
				{
					double valor=Math.Round(Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoiva FROM dbxschema.mfacturacliente WHERE pdoc_codigo='"+(((ArrayList)Session["facs"])[i].ToString().Split('-'))[0]+"' AND mfac_numedocu="+(((ArrayList)Session["facs"])[i].ToString().Split('-'))[1]+"")),2);
					string letra=inter.Letras(valor.ToString());
					if(tbLetras==null)
						this.Preparar_tbLetras();
					fila=tbLetras.NewRow();
					fila[0]=(((ArrayList)Session["facs"])[i].ToString().Split('-'))[0];
					fila[1]=Convert.ToInt32((((ArrayList)Session["facs"])[i].ToString().Split('-'))[1]);
					fila[2]=valor;
					fila[3]=letra;
					tbLetras.Rows.Add(fila);
				}
				ds.Tables.Add(tbLetras);
				ds.AcceptChanges();
			}
			else if(Request.QueryString["tipo"]=="P")
			{
				for(int i=0;i<((ArrayList)Session["facs"]).Count;i++)
				{
					double valor=Math.Round(Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoiva FROM dbxschema.mfacturaproveedor WHERE pdoc_codiordepago='"+(((ArrayList)Session["facs"])[i].ToString().Split('-'))[0]+"' AND mfac_numeordepago="+(((ArrayList)Session["facs"])[i].ToString().Split('-'))[1]+"")),2);
					string letra=inter.Letras(valor.ToString());
					if(tbLetras==null)
						this.Preparar_tbLetras();
					fila=tbLetras.NewRow();
					fila[0]=(((ArrayList)Session["facs"])[i].ToString().Split('-'))[0];
					fila[1]=Convert.ToInt32((((ArrayList)Session["facs"])[i].ToString().Split('-'))[1]);
					fila[2]=valor;
					fila[3]=letra;
					tbLetras.Rows.Add(fila);
				}
				ds.Tables.Add(tbLetras);
				ds.AcceptChanges();
			}
		}
		
		protected void Preparar_tbLetras()
		{
			tbLetras=new DataTable();
			tbLetras.Columns.Add("PREFIJO",typeof(string));
			tbLetras.Columns.Add("NUMERO",typeof(int));
			tbLetras.Columns.Add("VALOR",typeof(double));
			tbLetras.Columns.Add("LETRAS",typeof(string));
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
