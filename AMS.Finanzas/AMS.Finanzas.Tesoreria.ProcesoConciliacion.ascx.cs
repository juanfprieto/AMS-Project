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
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial  class ProcesoConciliacion : System.Web.UI.UserControl
	{
		protected DataTable dtConciliacion,dtMovimiento;
		protected string mainPage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				this.Llenar_dgConciliacion();
				this.Llenar_dtMovimiento();
			}
			else
			{
				if(Session["dtConciliacion"]!=null)
					dtConciliacion=(DataTable)Session["dtConciliacion"];
				if(Session["dtMovimiento"]!=null)
					dtMovimiento=(DataTable)Session["dtMovimiento"];
				ReConstruir_dgConciliacion();
			}
		}
		
		protected void Construir_dgConciliacion3()
		{
			BoundColumn fecha=new BoundColumn();
			BoundColumn doc=new BoundColumn();
			BoundColumn val=new BoundColumn();
			fecha.DataField="FECHA";
			fecha.HeaderText="Fecha";
			doc.DataField="DOCUMENTO";
			doc.HeaderText="Documento";
			val.DataField="VALOR";
			val.HeaderText="Valor";
			val.DataFormatString="{0:C}";
			dgConciliacion.Columns.Add(fecha);
			dgConciliacion.Columns.Add(doc);
			dgConciliacion.Columns.Add(val);
		}
		
		protected void Construir_dgConciliacion4()
		{
			BoundColumn fecha=new BoundColumn();
			BoundColumn doc=new BoundColumn();
			BoundColumn valdeb=new BoundColumn();
			BoundColumn valcre=new BoundColumn();
			fecha.DataField="FECHA";
			fecha.HeaderText="Fecha";
			doc.DataField="DOCUMENTO";
			doc.HeaderText="Documento";
			valdeb.DataField="VALORDEBITO";
			valdeb.HeaderText="Valor Débito";
			valdeb.DataFormatString="{0:C}";
			valcre.DataField="VALORCREDITO";
			valcre.HeaderText="Valor Crédito";
			valcre.DataFormatString="{0:C}";
			dgConciliacion.Columns.Add(fecha);
			dgConciliacion.Columns.Add(doc);
			dgConciliacion.Columns.Add(valcre);  // suma  a la cuenta en mi banco
			dgConciliacion.Columns.Add(valdeb);  // resta a la cuenta en mi banco
		}
		
		protected void Cargar_dtConciliacion3()
		{
			dtConciliacion=new DataTable();
			dtConciliacion.Columns.Add("FECHA",typeof(string));
			dtConciliacion.Columns.Add("DOCUMENTO",typeof(string));
			dtConciliacion.Columns.Add("VALOR",typeof(double));
			dtConciliacion.Columns.Add("CRUZADO",typeof(bool));
			dtConciliacion.Columns.Add("CON",typeof(int));
		}
		
		protected void Cargar_dtConciliacion4()
		{
			dtConciliacion=new DataTable();
			dtConciliacion.Columns.Add("FECHA",typeof(string));
			dtConciliacion.Columns.Add("DOCUMENTO",typeof(string));
			dtConciliacion.Columns.Add("VALORDEBITO",typeof(double));
			dtConciliacion.Columns.Add("VALORCREDITO",typeof(double));
			dtConciliacion.Columns.Add("CRUZADO",typeof(bool));
			dtConciliacion.Columns.Add("CON",typeof(int));
		}
		
		protected void Cargar_dtMovimiento()
		{
			dtMovimiento=new DataTable();
			dtMovimiento.Columns.Add("FECHA",typeof(string));
			dtMovimiento.Columns.Add("CODIGO",typeof(string));
			dtMovimiento.Columns.Add("NUMERO",typeof(string));
			dtMovimiento.Columns.Add("VALOR",typeof(double));
			dtMovimiento.Columns.Add("CRUZADO",typeof(bool));
			dtMovimiento.Columns.Add("CON",typeof(int));
		}
		
		protected void Llenar_dtMovimiento()
		{
			DataSet ds=new DataSet();
			DataRow fila;
			int dias=DateTime.DaysInMonth(DateTime.Now.Year,Convert.ToInt32(Request.QueryString["mes"]));
			string selDoc,selEfe,selDev,selTras1,selTras2,selTrasCh1,selTrasCh2,selNot,selEgresos,selBcaElec,selEntChqs,selPen;
			string union=" UNION ALL ";
			selDoc    ="SELECT MTES.mtes_fecha,DTES.mtes_codigo,DTES.mtes_numero,DTES.dtes_valor    FROM dbxschema.dtesoreriadocumentos DTES,dbxschema.mtesoreria MTES,dbxschema.pcuentacorriente PCUE                                        WHERE DTES.mtes_codigo=MTES.pdoc_codigo AND DTES.mtes_numero=MTES.mtes_numero AND DTES.pcue_codigo=PCUE.pcue_codigo AND MTES.mtes_fecha BETWEEN '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selEfe    ="SELECT MTES.mtes_fecha,DTES.mtes_codigo,DTES.mtes_numero,DTES.dtes_valor    FROM dbxschema.dtesoreriaefectivo DTES,dbxschema.mtesoreria MTES,dbxschema.pcuentacorriente PCUE                                          WHERE DTES.mtes_codigo=MTES.pdoc_codigo AND DTES.mtes_numero=MTES.mtes_numero AND DTES.pcue_codigo=PCUE.pcue_codigo AND MTES.mtes_fecha BETWEEN'"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selDev    ="SELECT MTES.mtes_fecha,DTES.mtes_codigo,DTES.mtes_numero,MFAC.mfac_valofact FROM dbxschema.dtesoreriadevoluciones DTES,dbxschema.mtesoreria MTES,dbxschema.pcuentacorriente PCUE,dbxschema.mfacturacliente MFAC       WHERE DTES.mtes_codigo=MTES.pdoc_codigo AND DTES.mtes_numero=MTES.mtes_numero AND DTES.mfac_codigo=MFAC.pdoc_codigo AND DTES.mfac_numero=MFAC.mfac_numedocu AND MTES.pcue_codigo=PCUE.pcue_codigo AND MTES.mtes_fecha BETWEEN'"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selTras1  ="SELECT MTES.mtes_fecha,DTES.mtes_codigo,DTES.mtes_numero,MTEL.mtes_saldo    FROM dbxschema.dtesoreriatraslados DTES,dbxschema.mtesoreria MTES,dbxschema.pcuentacorriente PCUE,dbxschema.mtesoreriasaldos MTEL         WHERE DTES.mtes_codigo=MTES.pdoc_codigo AND DTES.mtes_numero=MTES.mtes_numero AND DTES.pcue_codigoori=PCUE.pcue_codigo AND MTES.pcue_codigo=PCUE.pcue_codigo AND MTEL.mtes_codigo=MTES.pdoc_codigo AND mTEl.pcue_codigo=PCUE.pcue_codigo AND MTEL.mtes_numero=MTES.mtes_numero AND MTES.mtes_fecha BETWEEN'"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selTras2  ="SELECT MTES.mtes_fecha,DTES.mtes_codigo,DTES.mtes_numero,MTEL.mtes_saldo    FROM dbxschema.dtesoreriatraslados DTES,dbxschema.mtesoreria MTES,dbxschema.pcuentacorriente PCUE,dbxschema.mtesoreriasaldos MTEL         WHERE DTES.mtes_codigo=MTES.pdoc_codigo AND DTES.mtes_numero=MTES.mtes_numero AND DTES.pcue_codigodes=PCUE.pcue_codigo AND MTEL.mtes_codigo=MTES.pdoc_codigo AND MTEL.mtes_numero=MTES.mtes_numero AND mTEl.pcue_codigo=PCUE.pcue_codigo AND MTES.mtes_fecha BETWEEN'"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selTrasCh1="SELECT MTES.mtes_fecha,DTES.mtes_codigo,DTES.mtes_numero,MTEL.mtes_saldo    FROM dbxschema.dtesoreriatrasladoscheque DTES,dbxschema.mtesoreria MTES,dbxschema.pcuentacorriente PCUE,dbxschema.mtesoreriasaldos MTEL   WHERE DTES.mtes_codigo=MTES.pdoc_codigo AND DTES.mtes_numero=MTES.mtes_numero AND DTES.pcue_codigoori=PCUE.pcue_codigo AND MTES.pcue_codigo=PCUE.pcue_codigo AND MTEL.mtes_codigo=MTES.pdoc_codigo AND MTEL.mtes_numero=MTES.mtes_numero AND MTES.mtes_fecha BETWEEN'"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selTrasCh2="SELECT MTES.mtes_fecha,DTES.mtes_codigo,DTES.mtes_numero,MTEL.mtes_saldo    FROM dbxschema.dtesoreriatrasladoscheque DTES,dbxschema.mtesoreria MTES,dbxschema.pcuentacorriente PCUE,dbxschema.mtesoreriasaldos MTEL   WHERE DTES.mtes_codigo=MTES.pdoc_codigo AND DTES.mtes_numero=MTES.mtes_numero AND DTES.pcue_codigodes=PCUE.pcue_codigo AND MTEL.mtes_codigo=MTES.pdoc_codigo AND MTEL.mtes_numero=MTES.mtes_numero AND MTES.mtes_fecha BETWEEN'"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selNot    ="SELECT MTES.mtes_fecha,DTES.mtes_codigo,DTES.mtes_numero,case when pd.tdoc_tipodocu = 'ND' then DTES.dtes_valor ELSE DTES.dtes_valor*(-1) END AS VALOR    FROM dbxschema.dtesorerianota DTES,dbxschema.mtesoreria MTES,dbxschema.pcuentacorriente PCUE, pdocumento pd           WHERE DTES.mtes_codigo=MTES.pdoc_codigo AND DTES.mtes_numero=MTES.mtes_numero AND mTES.pcue_codigo=PCUE.pcue_codigo and MTES.pdoc_codigo = pd.pdoc_codigo AND MTES.mtes_fecha BETWEEN'"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selBcaElec="SELECT MTES.mtes_fecha,DTES.mtes_codigo,DTES.mtes_numero,mcp.mcpag_valor    FROM dbxschema.dtesoreriabancaelectronica DTES,dbxschema.mtesoreria MTES,dbxschema.mcajapago MCP,dbxschema.pcuentacorriente PCUE          WHERE DTES.mtes_codigo=MTES.pdoc_codigo AND DTES.mtes_numero=MTES.mtes_numero and DTES.mtes_codigo=MCP.pdoc_codigo AND DTES.mtes_numero=MCP.mcaj_numero and MCP.TTIP_CODIGO = 'B' and dtes.pcue_codigo=PCUE.pcue_codigo AND MTES.mtes_fecha BETWEEN'"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selEgresos="SELECT Mcj.mcaj_fecha,mcpg.pdoc_codigo,mcpg.mcaj_numero,(-1*mcpg.mcpag_valor)  FROM dbxschema.mcaja mcj,dbxschema.mcajapago Mcpg,dbxschema.pchequera pch,dbxschema.pcuentacorriente PCUE, dbxschema.dtesoreriaentregas dte  WHERE mcj.pdoc_codigo = mcpg.pdoc_codigo and mcj.mcaj_numero = mcpg.mcaj_numero and dte.mcaj_codigo = mcpg.pdoc_codigo and dte.mcaj_numero = mcpg.mcaj_numero AND mcj.test_estadoDOC <> 'N' and mcpg.pche_id=pch.pche_id and pch.pcue_codigo=PCUE.pcue_codigo AND Mcj.mcaj_fecha BETWEEN'"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
	        selEntChqs="SELECT Mcpg.mcpag_fecha,dte.mtes_codigo,dte.mtes_numero, (-1*mcpg.mcpag_valor*pt.ptes_porcentaje*0.01) FROM dbxschema.dtesoreriaentregas dte,dbxschema.mcajapago Mcpg,dbxschema.pchequera pch,dbxschema.pcuentacorriente PCUE, dbxschema.ptesoreria pt WHERE dte.mcaj_codigo = mcpg.pdoc_codigo and dte.mcaj_numero = mcpg.mcaj_numero AND mcPG.test_estado IN ('E','N') and mcpg.pche_id=pch.pche_id and pch.pcue_codigo=PCUE.pcue_codigo AND Mcpg.mcpag_fecha BETWEEN '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND PCUE.pcue_codigo='"+Request.QueryString["cnt"]+"'";
			selPen    ="SELECT mpen_fecha,mtes_codigo,mtes_numero,mpen_valor                        FROM dbxschema.mpendientesconciliacion                                                                                                    WHERE mpen_fecha BETWEEN '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-01"+"' AND '"+DateTime.Now.Year+"-"+Request.QueryString["mes"]+"-"+dias+"' AND pcue_codigo='"+Request.QueryString["cnt"]+"'";
			


			DBFunctions.Request(ds,IncludeSchema.NO,selDoc+union+selEfe+union+selDev+union+selTras1+union+selTras2+union+selTrasCh1+union+selTrasCh2+union+selNot+union+selBcaElec+union+selEgresos+union+selEntChqs+union+selPen);
			//lb.Text=selDoc+union+selEfe+union+selDev+union+selTras1+union+selTras2+union+selTrasCh1+union+selTrasCh2+union+selNot+union+selPen;
			if(ds.Tables[0].Rows.Count!=0)
			{
				if(Session["dtMovimiento"]==null)
					this.Cargar_dtMovimiento();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=dtMovimiento.NewRow();
					fila[0]=Convert.ToDateTime(ds.Tables[0].Rows[i][0].ToString()).ToString("yyyy-MM-dd");
					fila[1]=ds.Tables[0].Rows[i][1].ToString();
					fila[2]=ds.Tables[0].Rows[i][2].ToString();
					fila[3]=Convert.ToDouble(ds.Tables[0].Rows[i][3]);
					fila[4]=false;
					fila[5]=-1;
					dtMovimiento.Rows.Add(fila);
					dgMovimiento.DataSource=dtMovimiento;
					dgMovimiento.DataBind();
					Session["dtMovimiento"]=dtMovimiento;
				}
			}
            else
            {
                Utils.MostrarAlerta(Response, "No hay Movimiento en Tesorería para Conciliar");
                btnConciliar.Enabled = false;
            }
		}
		
		protected void Llenar_dgConciliacion()
		{
			DataSet ds=new DataSet();
			if(Session["ds3"]!=null || Session["ds4"]!=null)
			{
				if(Request.QueryString["col"]=="3")
					ds=(DataSet)Session["ds3"];
				else if(Request.QueryString["col"]=="4")
					ds=(DataSet)Session["ds4"];
				this.Llenar_ds(ds);
			}
			else
            Utils.MostrarAlerta(Response, "Error Interno. Repita el proceso completo");
		}
		
		protected void Llenar_ds(DataSet ds)
		{
			DataRow fila;
			if(ds.Tables[0].Columns.Count==3)
			{
				if(Session["dtConciliacion"]==null)
					this.Cargar_dtConciliacion3();
				this.Construir_dgConciliacion3();
               
                try
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        fila = dtConciliacion.NewRow();

                        fila[0] = Convert.ToDateTime(ds.Tables[0].Rows[i][0]).ToString("yyyy-MM-dd");
                        fila[1] = ds.Tables[0].Rows[i][1].ToString();
                        fila[2] = Convert.ToDouble(ds.Tables[0].Rows[i][2]);

                        fila[3] = false;
                        fila[4] = -1;
                        dtConciliacion.Rows.Add(fila);
                        dgConciliacion.DataSource = dtConciliacion;
                        dgConciliacion.DataBind();
                        DatasToControls.JustificacionGrilla(dgConciliacion, dtConciliacion);
                        Session["dtConciliacion"] = dtConciliacion;
                        
                    }
                }
                catch {
                 
                }
			}else if(ds.Tables[0].Columns.Count==4)
			{
				if(Session["dtConciliacion"]==null)
					this.Cargar_dtConciliacion4();
				this.Construir_dgConciliacion4();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=dtConciliacion.NewRow();
					fila[0]=Convert.ToDateTime(ds.Tables[0].Rows[i][0]).ToString("yyyy-MM-dd");
					fila[1]=ds.Tables[0].Rows[i][1].ToString();
					if(ds.Tables[0].Rows[i][2].ToString()=="")
						fila[2]=0;
					else if(ds.Tables[0].Rows[i][2].ToString()!="")
						fila[2]=Convert.ToDouble(ds.Tables[0].Rows[i][2]);
					if(ds.Tables[0].Rows[i][3].ToString()=="")
						fila[3]=0;
					else if(ds.Tables[0].Rows[i][3].ToString()!="")
						fila[3]=Convert.ToDouble(ds.Tables[0].Rows[i][3]);
					fila[4]=false;
					fila[5]=-1;
					dtConciliacion.Rows.Add(fila);
					dgConciliacion.DataSource=dtConciliacion;
					dgConciliacion.DataBind();
					DatasToControls.JustificacionGrilla(dgConciliacion,dtConciliacion);
					Session["dtConciliacion"]=dtConciliacion;
					//lb.Text+=ds.Tables[0].Rows[i][3]+"<br><br>";
				}
			}
		}
		
		protected void ReConstruir_dgConciliacion()
		{
			if(Request.QueryString["col"]=="3")
				this.Construir_dgConciliacion3();
			else if(Request.QueryString["col"]=="4")
				this.Construir_dgConciliacion4();
			dgConciliacion.DataSource=dtConciliacion;
			dgConciliacion.DataBind();
			DatasToControls.JustificacionGrilla(dgConciliacion,dtConciliacion);
		}
		
		protected void dgConciliacion_PageChanged(object Sender,DataGridPageChangedEventArgs e)
		{
			dgConciliacion.CurrentPageIndex=e.NewPageIndex;
			dgConciliacion.DataBind();
			//ReConstruir_dgConciliacion();
		}
		
		protected void dgMovimiento_PageChanged(object Sender,DataGridPageChangedEventArgs e)
		{
			dgMovimiento.CurrentPageIndex=e.NewPageIndex;
			dgMovimiento.DataSource=dtMovimiento;
			dgMovimiento.DataBind();
		}
		
		protected void btnConciliar_Click(object Sender,EventArgs e)
		{
			if(Request.QueryString["col"]=="3")
			{
				this.Conciliar_3();
				Response.Redirect(mainPage+"?process=Tesoreria.ConciliacionManual&cnt="+Request.QueryString["cnt"]+"&col="+Request.QueryString["col"]+"&mes="+Request.QueryString["mes"]+"");
			}
			else if(Request.QueryString["col"]=="4")
			{
				this.Conciliar_4();
				Response.Redirect(mainPage+"?process=Tesoreria.ConciliacionManual&cnt="+Request.QueryString["cnt"]+"&col="+Request.QueryString["col"]+"&mes="+Request.QueryString["mes"]+"");
			}
			/*for(int i=0;i<dtConciliacion.Rows.Count;i++)
				lb.Text+=dtConciliacion.Rows[i][2].ToString()+" "+dtConciliacion.Rows[i][3].ToString()+"<br>";*/
		}
		
		protected void Conciliar_3()
		{
			for(int i=0;i<dtMovimiento.Rows.Count;i++)
			{
				if(!Convert.ToBoolean(dtMovimiento.Rows[i][4]))
				{
					for(int j=0;j<dtConciliacion.Rows.Count;j++)
					{
						if(!Convert.ToBoolean(dtConciliacion.Rows[j][3]))
						{
							if(Convert.ToDouble(dtMovimiento.Rows[i][3])==Convert.ToDouble(dtConciliacion.Rows[j][2]))
							{
								dtMovimiento.Rows[i][4]=true;
								dtConciliacion.Rows[j][3]=true;
								dtMovimiento.Rows[i][5]=i;
								dtConciliacion.Rows[j][4]=i;
								Session["dtMovimiento"]=dtMovimiento;
								Session["dtConciliacion"]=dtConciliacion;
								break;
							}
						}
					}
				}
			}
		}
		
		protected void Conciliar_4()
		{
			for(int i=0;i<dtMovimiento.Rows.Count;i++)
			{
				if(!Convert.ToBoolean(dtMovimiento.Rows[i][4]))
				{
					for(int j=0;j<dtConciliacion.Rows.Count;j++)
					{
						if(!Convert.ToBoolean(dtConciliacion.Rows[j][4]))
						{
							if(Convert.ToDouble(dtConciliacion.Rows[j][2])!=0)
							{
								if(Convert.ToDouble(dtMovimiento.Rows[i][3])==Convert.ToDouble(dtConciliacion.Rows[j][2]))
								{
									dtMovimiento.Rows[i][4]=true;
									dtConciliacion.Rows[j][4]=true;
									dtMovimiento.Rows[i][5]=i;
									dtConciliacion.Rows[j][5]=i;
									Session["dtMovimiento"]=dtMovimiento;
									Session["dtConciliacion"]=dtConciliacion;
									break;
								}
							}
							else if(Convert.ToDouble(dtConciliacion.Rows[j][3])!=0)
							{
								if(Convert.ToDouble(dtMovimiento.Rows[i][3])==(Convert.ToDouble(dtConciliacion.Rows[j][3])*-1))
								{
									dtMovimiento.Rows[i][4]=true;
									dtConciliacion.Rows[j][4]=true;
									dtMovimiento.Rows[i][5]=i;
									dtConciliacion.Rows[j][5]=i;
									Session["dtMovimiento"]=dtMovimiento;
									Session["dtConciliacion"]=dtConciliacion;
									break;
								}
							}
						}
					}
				}
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
