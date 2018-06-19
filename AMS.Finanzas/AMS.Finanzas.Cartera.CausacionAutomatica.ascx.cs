using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial  class CausacionAutomatica : System.Web.UI.UserControl
	{
		protected DataTable dtConceptos;
		protected ArrayList facs;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
				
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				//Session.Clear();
				DatasToControls bind=new DatasToControls();
                if (((ArrayList)Session["facturas"]) == null)
                {
                   
                }
                else if (((ArrayList)Session["facturas"]).Count != 0)
                {
                    for (int i = 0; i < ((ArrayList)Session["facturas"]).Count; i++)
                    {
                        string[] partes = ((ArrayList)Session["facs"])[i].ToString().Split('-');
                        Utils.MostrarAlerta(Response, "Se han creado las facturas de cliente con prefijo prefijo " + partes[0] + " y número " + partes[1] + " ");
                        try
                        {
                            Imprimir.ImprimirRPT(Response, partes[0], Convert.ToInt32(partes[1]), true);
                        }
                        catch
                        {
                            lb.Text += "Error al generar la impresión.";
                        }
                    }                    
                }
                if (Request.QueryString["tipo"] == "C")
                   Utils.llenarPrefijos(Response, ref ddlPrefijo, "%", "%", "FC");
                else if(Request.QueryString["tipo"] == "P")               
                Utils.llenarPrefijos(Response, ref ddlPrefijo , "%", "%", "FP");
				bind.PutDatasIntoDropDownList(ddlMes,"SELECT pmes_mes,pmes_nombre FROM dbxschema.pmes ORDER BY pmes_mes");
				DatasToControls.EstablecerDefectoDropDownList(ddlMes,DBFunctions.SingleData("SELECT pmes_nombre FROM pmes WHERE pmes_mes="+DateTime.Now.Month+""));
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen,palm_descripcion FROM dbxschema.palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
				this.Mostrar_Causacion(null, null);
			}
			else
			{
				if(Session["dtConceptos"]!=null)
					dtConceptos=(DataTable)Session["dtConceptos"];
				if(Session["facs"]!=null)
					facs=(ArrayList)Session["facs"];
			}
		}
		
		protected void Cargar_dtConceptos()
		{
			dtConceptos=new DataTable();
			dtConceptos.Columns.Add("NIT",typeof(string));
			dtConceptos.Columns.Add("VALOR",typeof(double));
			dtConceptos.Columns.Add("PORCIVA",typeof(double));
			dtConceptos.Columns.Add("DETALLE",typeof(string));
            dtConceptos.Columns.Add("FECHA", typeof(string));
            dtConceptos.Columns.Add("PORC", typeof(double));
            dtConceptos.Columns.Add("NOMBRE", typeof(string));
        }
		
		protected void Mostrar_Causacion(object sender, System.EventArgs e)
		{
			Session.Remove("dtConceptos");
			DataRow fila;
			DataSet ds=new DataSet();
			if(Request.QueryString["tipo"].Trim() =="C")
				DBFunctions.Request(ds,IncludeSchema.NO, "SELECT mcli_nit AS Nit,pcau_valocaus AS Valor,piva_porciva AS IVA,pcau_detalle AS Detalle, coalesce(PCAU_FECHINIC,current date) AS FECHA, CASE WHEN MONTH(PCAU_FECHINIC) = " + ddlMes.SelectedValue+ " THEN PCAU_PORCINCR ELSE 0 END AS PORC, NOMBRE FROM dbxschema.pcausacioncliente P, VMNIT VN WHERE P.MCLI_NIT = VN.MNIT_NIT ORDER BY mcli_nit,pcau_secuencia ;");
			else if(Request.QueryString["tipo"].Trim() == "P")
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mpro_nit AS Nit,pcau_valocaus AS Valor,piva_porciva AS IVA,pcau_detalle AS Detalle FROM dbxschema.pcausacionproveedor ORDER BY mpro_nit");
			if(ds.Tables[0].Rows.Count!=0)
			{
				if(Session["dtConceptos"]==null)
					this.Cargar_dtConceptos();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=dtConceptos.NewRow();
					fila[0] = ds.Tables[0].Rows[i][0].ToString();
					fila[1] = Convert.ToDouble(ds.Tables[0].Rows[i][1])*(1+(Convert.ToDouble(ds.Tables[0].Rows[i][5])*0.01));
					fila[2] = Convert.ToDouble(ds.Tables[0].Rows[i][2]);
					fila[3] = ds.Tables[0].Rows[i][3].ToString();
                    fila[4] = Convert.ToDateTime(ds.Tables[0].Rows[i][4]).ToString("yyyy-MM-dd");
                    fila[5] = ds.Tables[0].Rows[i][5].ToString();
                    fila[6] = ds.Tables[0].Rows[i][6].ToString();
                    dtConceptos.Rows.Add(fila);
				}
				dgConceptos.DataSource=dtConceptos;
				dgConceptos.DataBind();
				Session["dtConceptos"]=dtConceptos;
				this.Mostrar_Controles();
			}
			else
                Utils.MostrarAlerta(Response, "No hay conceptos que causar. Revise su parametrización");
		}
		
		protected void Mostrar_Controles()
		{
			pnlControles.Visible=true;
		}
		
		protected void btnGenerar_Click(object Sender,EventArgs e)
		{
            string mensaje = "";
            if (tbDetalle.Text == "")
                mensaje = "Debe diligenciar el campo Detalle \\n" ;
            if (ddlPrefijo.SelectedItem.ToString() == "Seleccione...")
                mensaje += "Debe Seleccionar un documento para emitir las facturas \\n";  
            if (ddlMes.SelectedValue != DBFunctions.SingleData("SELECT pmes_mesvigente FROM ccartera"))
                mensaje += "El mes Seleccionado NO corresponde con la vigencia en cartera. \\n ";
            int facturasEmitidas = 0;
            if (Request.QueryString["tipo"] == "C")
                facturasEmitidas = Convert.ToInt16(DBFunctions.SingleData(@"SELECT COUNT(*) FROM MFACTURACLIENTE MF, dfacturacliente DF, CCARTERA CC
                                                                             WHERE MF.PDOC_CODIGO = DF.MFAC_CODIGO AND MF.MFAC_NUMEDOCU = DF.MFAC_NUMERO AND YEAR(MFAC_FACTURA) = CC.PANO_ANOVIGENTE AND MONTH(MFAC_FACTURA) = CC.PMES_MESVIGENTE ;"));
            else facturasEmitidas = Convert.ToInt16(DBFunctions.SingleData(@"SELECT COALESCE(COUNT(*),0) FROM MFACTURAproveedor MF, dfacturaproveedor DF, CCARTERA CC
                                                                              WHERE MF.PDOC_CODIordepaGO = DF.MFAC_CODIGO AND MF.MFAC_NUMEordepago = DF.MFAC_NUMERO AND YEAR(MFAC_FACTURA) = CC.PANO_ANOVIGENTE AND MONTH(MFAC_FACTURA) = CC.PMES_MESVIGENTE ;"));

            if (facturasEmitidas > 0)
                mensaje += "Ya se han EMITIDO "+ facturasEmitidas +" FACTURAS para esta vigencia. No se puede repetir este proceso \\n";
            if (Request.QueryString["tipo"] == "C" && ddlPrefijo.SelectedItem.ToString() != "Seleccione..." && dtConceptos.Rows.Count > Convert.ToUInt16(DBFunctions.SingleData("select coalesce(pdoc_numefina - pdoc_ultidocu,0) FROM PDOCUMENTO WHERE PDOC_CODIGO = '"+ddlPrefijo.SelectedValue.ToString()+"' ")))
            {
                mensaje += "El rango disponible Autorizado de Facturación para emitir las facturas es insuficiente \\n";
            }
            if (mensaje.Length > 0)
            {
                mensaje += "Proceso NO realizado";
                Utils.MostrarAlerta(Response, mensaje);
            }
            else
            { 
                ArrayList sqlStrings=new ArrayList();
                ArrayList facturas = new ArrayList();

                {
                    if (Request.QueryString["tipo"] == "C")
                        this.Guardar_Cliente(dtConceptos, ref sqlStrings);
                    else if (Request.QueryString["tipo"] == "P")
                            this.Guardar_Proveedor(dtConceptos, ref sqlStrings);
                    if (DBFunctions.Transaction(sqlStrings))
                    {
                        lb.Text += DBFunctions.exceptions + "<br>";

                        for (int i = 0; i < ((ArrayList)Session["facs"]).Count; i++)
                        {
                            string[] partes = ((ArrayList)Session["facs"])[i].ToString().Split('/');
                            facturas.Add(partes);
                            Session["facturas"] = facturas;
                        }

                        Response.Redirect(indexPage + "?process=Cartera.CausacionAutomatica&tipo=" + Request.QueryString["tipo"].Trim() + " &Facts=" + facturas + "");
                        //Response.Redirect(indexPage + "?process=Cartera.CausacionAutomatica&tipo=" + Request.QueryString["tipo"] + "&prefF="+ partes[0]+"&numF="+ partes[1]+"");
                        //Response.Redirect(indexPage+"?process=Cartera.CausacionAutomatica.Impresion&tipo="+Request.QueryString["tipo"]+"&prefijo="+ddlPrefijo+" ");
                        //Response.Redirect(indexPage+"?process=Cartera.CausacionAutomatica&tipo="+Request.QueryString["tipo"]+"&prefF="++"&numF="++"");
                    }
                    else
                        lb.Text += "Error : " + DBFunctions.exceptions + "<br>";
                    /*for(int i=0;i<sqlStrings.Count;i++)
                        lb.Text+=sqlStrings[i]+"<br>";*/
                }
			}
		}
		
		protected ArrayList Calcular_Totales()
		{
			ArrayList facturas=new ArrayList();
			ArrayList nits=new ArrayList();
			nits=this.Sacar_Nits(dtConceptos);
			DataRow[] conceptos;
			for(int i=0;i<nits.Count;i++)
			{
				double total=0;
				double iva=0;
				conceptos=dtConceptos.Select("NIT='"+nits[i]+"'");
				for(int j=0;j<conceptos.Length;j++)
				{
					total=total+Convert.ToDouble(conceptos[j][1]);
					iva=iva+(Convert.ToDouble(conceptos[j][1])*(Convert.ToDouble(conceptos[j][2])/100));
				}
				//			   nit			valor total			total iva
				facturas.Add(nits[i]+"-"+total.ToString()+"-"+iva.ToString());
			}
			return facturas;
		}
		
		protected ArrayList Sacar_Nits(DataTable dtConceptos)
		{
			ArrayList nit=new ArrayList();
			for(int i=0;i<dtConceptos.Rows.Count;i++)
			{
				if(nit.BinarySearch(dtConceptos.Rows[i][0].ToString())<0)
					nit.Add(dtConceptos.Rows[i][0].ToString());
			}
			return nit;
		}
		
		protected void Sacar_Sqls(FacturaCliente fc,ref ArrayList sqlStrings)
		{
			for(int i=0;i<fc.SqlStrings.Count;i++)
				sqlStrings.Add(fc.SqlStrings[i]);
		}
		
		protected void Sacar_SqlsP(FacturaProveedor fp,ref ArrayList sqlStrings)
		{
			for(int i=0;i<fp.SqlStrings.Count;i++)
				sqlStrings.Add(fp.SqlStrings[i]);
		}
		
		protected void Guardar_Cliente(DataTable dtConceptos,ref ArrayList sqlStrings)
		{
			ArrayList totales=new ArrayList();
			facs = new ArrayList();
			totales = this.Calcular_Totales();
			FacturaCliente miFactura=new FacturaCliente();
			string completo;
			string[] partes;
			int numero = Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM dbxschema.pdocumento WHERE pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"'"));
            // Se actualiza el valor del servicio a los clientes que este mes se cumple un Año de contrato
            miFactura.SqlRels.Add("UPDATE pcausacioncliente SET PCAU_VALOCAUS = ROUND(PCAU_VALOCAUS*(1+(PCAU_PORCINCR*0.01)),0) WHERE MONTH(PCAU_FECHINIC) = (SELECT PMES_MESVIGENTE FROM CCARTERA);");

            for (int i=0;i<totales.Count;i++)
			{
				completo=totales[i].ToString();
				partes=completo.Split('-');
				miFactura=new FacturaCliente(ddlPrefijo.SelectedValue, ddlPrefijo.SelectedValue,partes[0],ddlAlmacen.SelectedValue,"F",Convert.ToUInt32(numero+i+1),Convert.ToUInt32(DBFunctions.SingleData("SELECT CASE WHEN mcli_diasplaz IS NULL THEN 0 ELSE mcli_diasplaz END CASE FROM dbxschema.mcliente WHERE mnit_nit='"+partes[0]+"'")),DateTime.Now.Date,DateTime.Now.AddDays(Convert.ToInt32(DBFunctions.SingleData("SELECT CASE WHEN mcli_diasplaz IS NULL THEN 0 ELSE mcli_diasplaz END CASE FROM dbxschema.mcliente WHERE mnit_nit='"+partes[0]+"'"))),Convert.ToDateTime(null),Convert.ToDouble(partes[1]),Convert.ToDouble(partes[2]),0,0,0,0,DBFunctions.SingleData("SELECT pcen_codigo FROM cempresa"),tbDetalle.Text,DBFunctions.SingleData("SELECT pven_codigo FROM ccartera"),HttpContext.Current.User.Identity.Name.ToLower(),null);
				facs.Add(ddlPrefijo.SelectedValue+"-"+(numero+i+1).ToString());
				Session["facs"]=facs;
				for(int j=0;j<dtConceptos.Rows.Count;j++)
				{
					if(dtConceptos.Rows[j][0].ToString()==partes[0])
						miFactura.SqlRels.Add("INSERT INTO dfacturacliente VALUES(default,'@1',@2,'"+dtConceptos.Rows[j][3].ToString() + " " + tbDetalle.Text + "', " + dtConceptos.Rows[j][1].ToString()+","+(Convert.ToDouble(dtConceptos.Rows[j][1])*(Convert.ToDouble(dtConceptos.Rows[j][2])/100)).ToString()+")");
				}
				if(miFactura.GrabarFacturaCliente(false))
					this.Sacar_Sqls(miFactura,ref sqlStrings);
				else
					sqlStrings.Add(miFactura.ProcessMsg);   
			}
		}
		
		protected void Guardar_Proveedor(DataTable dtConceptos,ref ArrayList sqlStrings)
		{
			ArrayList totales=new ArrayList();
			totales = this.Calcular_Totales();
			facs = new ArrayList();
			FacturaProveedor miFacturaP=new FacturaProveedor();
			string completo;
            // Se actualiza el valor del servicio a los proveedores que este mes se cumple un Año de contrato
            miFacturaP.SqlRels.Add("UPDATE pcausacionPROVEEDOR SET PCAU_VALOCAUS = ROUND(PCAU_VALOCAUS*(1+(PCAU_PORCINCR*0.01)),0) WHERE MONTH(PCAU_FECHINIC) = (SELECT PMES_MESVIGENTE FROM CCARTERA);");

            string[] partes;
			int numero = Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM dbxschema.pdocumento WHERE	pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"'"));
			for(int i=0;i<totales.Count;i++)
			{
				completo=totales[i].ToString();
				partes=completo.Split('-');
				miFacturaP=new FacturaProveedor(ddlPrefijo.SelectedValue,ddlPrefijo.SelectedValue,ddlPrefijo.SelectedValue,partes[0],ddlAlmacen.SelectedValue,"F",Convert.ToUInt64(numero+i+1),Convert.ToUInt64(numero+i+1),
				                                "V",DateTime.Now,DateTime.Now,Convert.ToDateTime(null),DateTime.Now,Convert.ToDouble(partes[1]),Convert.ToDouble(partes[2]),
				                                0,0,0,tbDetalle.Text,HttpContext.Current.User.Identity.Name.ToLower());
				facs.Add(ddlPrefijo.SelectedValue+"-"+(numero+i+1).ToString());
				Session["facs"]=facs;
				for(int j=0;j<dtConceptos.Rows.Count;j++)
				{
					if(dtConceptos.Rows[j][0].ToString()==partes[0])
						miFacturaP.SqlRels.Add("INSERT INTO dfacturaproveedor VALUES(default,'@1',@2,'"+dtConceptos.Rows[j][3].ToString()+" " + tbDetalle.Text + "'," + dtConceptos.Rows[j][1].ToString()+","+(Convert.ToDouble(dtConceptos.Rows[j][1])*(Convert.ToDouble(dtConceptos.Rows[j][2])/100)).ToString()+")");
				}
				if(miFacturaP.GrabarFacturaProveedor(false))
					this.Sacar_SqlsP(miFacturaP,ref sqlStrings);
				else
					sqlStrings.Add(miFacturaP.ProcessMsg);
			}
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
