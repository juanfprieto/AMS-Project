namespace AMS.Gerencial
{
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Services;
	using System.Web.SessionState;
	using System.Web.Services.Protocols;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using System.Text;
	using AMS.Tools;
	using System;

	/// <summary>
	///		Descripción breve de AMS_Gerencial_ConsultaTaller.
	/// </summary>
	public partial class AMS_Gerencial_ConsultaTaller : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataGrid Grid;
		protected DataSet lineas;
		protected DataSet lineas2;
		protected DataSet lineas3;
		protected DataSet Operaciones;
		protected DataSet Operaciones2;
		protected DataTable resultado;

		public double totalValorProceso=0,totalValorLiquidadas=0,totalProceso=0,totalLiquidadas=0;
		public double totalValorProcesoM=0,totalValorLiquidadasM=0,totalProcesoM=0,totalLiquidadasM=0;
		public double totalValorProcesoV=0,totalValorLiquidadasV=0,totalProcesoV=0,totalProcesoV10=0,totalProcesoV20=0,totalProcesoVM=0,totalLiquidadasV=0;
		public double totalValorProcesoR=0,totalValorLiquidadasR=0;
		

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ano1,"SELECT pano_ano FROM DBXSCHEMA.pano");
				bind.PutDatasIntoDropDownList(ano2,"SELECT pano_ano FROM DBXSCHEMA.pano");
				bind.PutDatasIntoDropDownList(mes1,"SELECT pmes_mes, pmes_nombre FROM DBXSCHEMA.pmes");
				bind.PutDatasIntoDropDownList(mes2,"SELECT pmes_mes, pmes_nombre FROM DBXSCHEMA.pmes");
				ano1.SelectedIndex=ano1.Items.IndexOf(ano1.Items.FindByValue(DateTime.Now.Year.ToString()));
				ano2.SelectedIndex=ano2.Items.IndexOf(ano2.Items.FindByValue(DateTime.Now.Year.ToString()));
				mes1.SelectedIndex=mes1.Items.IndexOf(mes1.Items.FindByValue(DateTime.Now.Month.ToString()));
				mes2.SelectedIndex=mes2.Items.IndexOf(mes2.Items.FindByValue(DateTime.Now.Month.ToString()));
				DiaFin.Text=DateTime.Now.Day.ToString();
			}
		}

		protected  void  Generar_Click(Object  Sender, EventArgs e)
		{
			DataSet dsTotalO=new DataSet();
			DataSet dsTotalM=new DataSet();
			DataSet dsTotalR=new DataSet();
			DataSet dsTotalV=new DataSet();
			DateTime fechaDesde,fechaHasta;
			totalValorProceso=totalValorLiquidadas=totalProceso=totalLiquidadas=0;
			totalValorProcesoM=totalValorLiquidadasM=totalProcesoM=totalLiquidadasM=0;
			totalValorProcesoV=totalValorLiquidadasV=totalProcesoV=totalProcesoV10=totalProcesoV20=totalProcesoM=totalLiquidadasV=0;
			//Validaciones
			try
			{
				fechaDesde=new DateTime(Convert.ToInt16(ano1.SelectedValue),Convert.ToInt16(mes1.SelectedValue),Convert.ToInt16(DiaInicio.Text));
				fechaHasta=new DateTime(Convert.ToInt16(ano2.SelectedValue),Convert.ToInt16(mes2.SelectedValue),Convert.ToInt16(DiaFin.Text));
				if(fechaHasta<fechaDesde)
					throw(new Exception());
			}
			catch
			{
				Response.Write("<script language:javascript>alert('Fechas no válidas.');</script>");
				return;
			}
			//Consultar total operaciones
			DBFunctions.Request(dsTotalO,IncludeSchema.NO,
                @"select tope_codigo, tope_nombre, cargo, SUM(LIQUIDADAS) LIQUIDADAS, SUM(VALOLIQUIDADAS) VALOLIQUIDADAS,SUM(PROCESO) PROCESO, SUM(VALOPROCESO) VALOPROCESO  
				FROM  
				(Select tto.tope_codigo, tto.tope_nombre, TCAR_NOMBRE AS cargo, mor.test_estado,  
				case when mor.test_estado<>'A' then sum(1) else sum (0) end LIQUIDADAS, 
				case when mor.test_estado<>'A' then 1
				                               CASE WHEN MFT.TCAR_CARGO IN ('C','S') THEN sum(coalesce(dor.dord_valooper,1)*COALESCE(MFAC_FACTORDEDUCIBLE,1)*1)
				                                                                     ELSE sum(coalesce(dor.dord_valooper,1)*COALESCE(MFAC_FACTORDEDUCIBLE,1))
				                               END
				                               else sum (0) end  VALOLIQUIDADAS,  
				case when mor.test_estado='A' then sum(1) else sum (0) end PROCESO,  
				case when mor.test_estado='A' then 
				                              CASE WHEN DOR.TCAR_CARGO IN ('C','S') THEN sum(coalesce(dor.dord_valooper,1)*1)
				                                                                    ELSE sum(coalesce(dor.dord_valooper,1)) 
				                              END 
				                              else sum (0) end VALOPROCESO  
				from ttipooperaciontaller tto, morden mor, ptempario ptm, TCARGOORDEN tC,  dordenoperacion dor
				 left join mfacturaclientetaller mft on dor.pdoc_codigo = mft.pdoc_prefordetrab and dor.mord_numeorde = mft.mord_numeorde 
				  and mft.tcar_cargo = dor.tcar_cargo  
				where tto.tope_codigo=ptm.tope_codigo and dor.ptem_operacion=ptm.ptem_operacion and dor.pdoc_codigo=mor.pdoc_codigo and   
				dor.mord_numeorde=mor.mord_numeorde and DOR.TCAR_CARGO = TC.TCAR_CARGO AND   
				((mor.test_estado<>'A' and mord_fechliqu between '" + fechaDesde.ToString("yyyy-MM-dd") + @"' and '" + fechaHasta.ToString("yyyy-MM-dd") + @"') or (mor.test_estado='A'))  
				group by tto.tope_codigo, tto.tope_nombre, mor.test_estado, TOPE_NOMBRE, TCAR_NOMBRE, MFT.TCAR_CARGO, DOR.TCAR_CARGO) ordenes  
				group by tope_codigo,tope_nombre, cargo
				order by 1,2,3;");
			dgrTotalO.DataSource=dsTotalO.Tables[0];
			for(int i=0;i<dsTotalO.Tables[0].Rows.Count;i++)
			{
				totalValorProceso+=Convert.ToDouble(dsTotalO.Tables[0].Rows[i]["VALOPROCESO"]);
				totalValorLiquidadas+=Convert.ToDouble(dsTotalO.Tables[0].Rows[i]["VALOLIQUIDADAS"]);
				totalProceso+=Convert.ToDouble(dsTotalO.Tables[0].Rows[i]["PROCESO"]);
				totalLiquidadas+=Convert.ToDouble(dsTotalO.Tables[0].Rows[i]["LIQUIDADAS"]);
			}
			dgrTotalO.DataBind();

            string sqlOperacionesdetallado = @"select coalesce(factura,'Proceso'), tope_codigo, tope_nombre, SUM(LIQUIDADAS) LIQUIDADAS, SUM(VALOLIQUIDADAS) VALOLIQUIDADAS,SUM(PROCESO) PROCESO, SUM(VALOPROCESO) VALOPROCESO  
				FROM  
				(Select mft.pdoc_codigo ||'-'|| mft.mfac_numedocu as factura, tto.tope_codigo,tto.tope_nombre || ' - ' || TCAR_NOMBRE AS TOPE_NOMBRE, mor.test_estado,  
				case when mor.test_estado<>'A' then sum(1) else sum (0) end LIQUIDADAS, 
				case when mor.test_estado<>'A' then sum(coalesce(dor.dord_valooper,1)) else sum (0) end VALOLIQUIDADAS,  
				case when mor.test_estado='A' then sum(1) else sum (0) end PROCESO,  
				case when mor.test_estado='A' then sum(coalesce(dor.dord_valooper,1)) else sum (0) end VALOPROCESO  
				from ttipooperaciontaller tto, morden mor, ptempario ptm, TCARGOORDEN tC, dordenoperacion dor
				 left join mfacturaclientetaller mft on dor.pdoc_codigo = mft.pdoc_prefordetrab and dor.mord_numeorde = mft.mord_numeorde 
				  and mft.tcar_cargo = dor.tcar_cargo  
				where tto.tope_codigo=ptm.tope_codigo and dor.ptem_operacion=ptm.ptem_operacion and dor.pdoc_codigo=mor.pdoc_codigo and   
				dor.mord_numeorde=mor.mord_numeorde and DOR.TCAR_CARGO = TC.TCAR_CARGO AND   
				((mor.test_estado<>'A' and mord_fechliqu between '2014-09-01' and '2014-09-23') or (mor.test_estado='A'))  
				group by mft.pdoc_codigo, mft.mfac_numedocu, tto.tope_codigo, tto.tope_nombre, mor.test_estado, TOPE_NOMBRE, TCAR_NOMBRE) ordenes  
				group by factura, tope_codigo,tope_nombre
				order by 1,2,3; ";

			//Recepcionistas
			DBFunctions.Request(dsTotalV,IncludeSchema.NO,
				"Select CODIGO, NOMBRE, "+
				"(select count(*) from morden mo1 "+
				" where mo1.pven_codigo=vnd.codigo and "+
				" (mo1.test_estado<>'A' and mo1.mord_fechliqu between '"+fechaDesde.ToString("yyyy-MM-dd")+"' and '"+fechaHasta.ToString("yyyy-MM-dd")+"')"+
				") ORDSLIQU,"+
				"(select count(*) from morden mol "+
				" where mol.pven_codigo=vnd.codigo and mol.test_estado='A' "+
				") ORDSPROC,"+
				"(select count(*) from morden mo2 "+
				" where mo2.pven_codigo=vnd.codigo and mo2.test_estado='A' and (days(current date)-days(mo2.mord_entrada))<=10 "+
				") ORDSPROC10,"+
				"(select count(*) from morden mo3 "+
				" where mo3.pven_codigo=vnd.codigo and mo3.test_estado='A' and (days(current date)-days(mo3.mord_entrada))>10 and (days(current date)-days(mo3.mord_entrada))<=20 "+
				") ORDSPROC20,"+
				"(select count(*) from morden mo4 "+
				" where mo4.pven_codigo=vnd.codigo and mo4.test_estado='A' and (days(current date)-days(mo4.mord_entrada))>20 "+
				") ORDSPROCM,"+
				"SUM(VENDLIQU) VENDLIQU, SUM(VENDPROC) VENDPROC "+
				"from "+
				"(Select pvn.pven_codigo CODIGO, pvn.pven_nombre NOMBRE, "+
				"case when mor.test_estado<>'A' then sum(coalesce(DORD_VALOOPER,1)) else sum (0) end VENDLIQU, "+
				"case when mor.test_estado='A' then sum(coalesce(DORD_VALOOPER,1)) else sum (0) end VENDPROC "+
				"from pvendedor pvn, morden mor, dordenoperacion dor "+
				"where mor.pven_codigo=pvn.pven_codigo and dor.pdoc_codigo=mor.pdoc_codigo and dor.mord_numeorde=mor.mord_numeorde and "+
				"((mor.test_estado<>'A' and mor.mord_fechliqu between '"+fechaDesde.ToString("yyyy-MM-dd")+"' and '"+fechaHasta.ToString("yyyy-MM-dd")+"') or (mor.test_estado='A')) "+
				"group by pvn.pven_codigo, pvn.pven_nombre, mor.test_estado) vnd "+
				"group by vnd.CODIGO, vnd.NOMBRE "+
				"order by vnd.NOMBRE;");
			dgrRecepcionistas.DataSource=dsTotalV.Tables[0];
			for(int i=0;i<dsTotalV.Tables[0].Rows.Count;i++)
			{
				totalValorProcesoV+=Convert.ToDouble(dsTotalV.Tables[0].Rows[i]["VENDPROC"]);
				totalValorLiquidadasV+=Convert.ToDouble(dsTotalV.Tables[0].Rows[i]["VENDLIQU"]);
				totalProcesoV+=Convert.ToDouble(dsTotalV.Tables[0].Rows[i]["ORDSPROC"]);
				totalProcesoV10+=Convert.ToDouble(dsTotalV.Tables[0].Rows[i]["ORDSPROC10"]);
				totalProcesoV20+=Convert.ToDouble(dsTotalV.Tables[0].Rows[i]["ORDSPROC20"]);
				totalProcesoVM+=Convert.ToDouble(dsTotalV.Tables[0].Rows[i]["ORDSPROCM"]);
				totalLiquidadasV+=Convert.ToDouble(dsTotalV.Tables[0].Rows[i]["ORDSLIQU"]);
			}
			dgrRecepcionistas.DataBind();

			//Mecanicos
			DBFunctions.Request(dsTotalM,IncludeSchema.NO,
				"Select CODIGO, NOMBRE, SUM(HORASLIQU) HORASLIQU, SUM(HORASPROC) HORASPROC, SUM(VENDLIQU) VENDLIQU, SUM(VENDPROC) VENDPROC "+
				"from "+
				"(Select pvn.pven_codigo CODIGO, pvn.pven_nombre NOMBRE, "+
				"case when mor.test_estado<>'A' then sum(coalesce(DORD_TIEMLIQU,1)) else sum (0) end HORASLIQU, "+
				"case when mor.test_estado='A' then sum(coalesce(DORD_TIEMLIQU,1)) else sum (0) end HORASPROC, "+
				"case when mor.test_estado<>'A' then sum(coalesce(DORD_VALOOPER,1)) else sum (0) end VENDLIQU, "+
				"case when mor.test_estado='A' then sum(coalesce(DORD_VALOOPER,1)) else sum (0) end VENDPROC "+
				"from pvendedor pvn, morden mor, dordenoperacion dor "+
				"where dor.pven_codigo=pvn.pven_codigo and dor.pdoc_codigo=mor.pdoc_codigo and dor.mord_numeorde=mor.mord_numeorde and "+
				"((mor.test_estado<>'A' and mor.mord_fechliqu between '"+fechaDesde.ToString("yyyy-MM-dd")+"' and '"+fechaHasta.ToString("yyyy-MM-dd")+"') or (mor.test_estado='A')) "+
				"group by pvn.pven_codigo, pvn.pven_nombre, mor.test_estado) vnd "+
				"group by vnd.CODIGO, vnd.NOMBRE "+
				"order by vnd.NOMBRE; ");
			dgrMecanicos.DataSource=dsTotalM.Tables[0];
			for(int i=0;i<dsTotalM.Tables[0].Rows.Count;i++)
			{
				totalValorProcesoM+=Convert.ToDouble(dsTotalM.Tables[0].Rows[i]["VENDPROC"]);
				totalValorLiquidadasM+=Convert.ToDouble(dsTotalM.Tables[0].Rows[i]["VENDLIQU"]);
				totalProcesoM+=Convert.ToDouble(dsTotalM.Tables[0].Rows[i]["HORASPROC"]);
				totalLiquidadasM+=Convert.ToDouble(dsTotalM.Tables[0].Rows[i]["HORASLIQU"]);
			}
			dgrMecanicos.DataBind();

			//Repuestos
			DBFunctions.Request(dsTotalR,IncludeSchema.NO,
				@"select  
				tcar_cargo, tcar_nombre,  
				(select   
				 coalesce(sum(mfac_valofact),0)  
				 from   
				 morden mor, mordentransferencia mot, mfacturacliente mfc,  tcargoorden tco   
				 where   
				 mor.test_estado='A' and   
				 mor.pdoc_codigo=mot.pdoc_codigo and mor.mord_numeorde=mot.mord_numeorde and   
				 mfc.pdoc_codigo=mot.pdoc_factura and mfc.mfac_numedocu=mot.mfac_numero and   
				 tco.tcar_cargo=mot.tcar_cargo and tco.tcar_cargo=tc.tcar_cargo and mfc.mfac_tipodocu<>'N'  
				)-  
				(select   
				 coalesce(sum(mfac_valofact),0)  
				 from   
				 morden mor, mordentransferencia mot, mfacturacliente mfc,  tcargoorden tco   
				 where   
				 mor.test_estado='A' and   
				 mor.pdoc_codigo=mot.pdoc_codigo and mor.mord_numeorde=mot.mord_numeorde and   
				 mfc.pdoc_codigo=mot.pdoc_factura and mfc.mfac_numedocu=mot.mfac_numero and   
				 tco.tcar_cargo=mot.tcar_cargo and tco.tcar_cargo=tc.tcar_cargo and mfc.mfac_tipodocu='N'  
				)  
				 VALORPROC,  
				(select   
				  coalesce(sum(mfac_valofact),0)  
				 from   
				 morden mor, mordentransferencia mot, mfacturacliente mfc,  tcargoorden tco   
				 where   
				 (mor.test_estado<>'A' and mord_fechliqu between '"+fechaDesde.ToString("yyyy-MM-dd")+@"' and '"+fechaHasta.ToString("yyyy-MM-dd")+@"') and   
 				 mor.pdoc_codigo=mot.pdoc_codigo and mor.mord_numeorde=mot.mord_numeorde and   
				 mfc.pdoc_codigo=mot.pdoc_factura and mfc.mfac_numedocu=mot.mfac_numero and   
				 tco.tcar_cargo=mot.tcar_cargo and tco.tcar_cargo=tc.tcar_cargo and mfc.mfac_tipodocu<>'N'  
				)-  
				(select   
				  coalesce(sum(mfac_valofact),0)  
				 from   
				 morden mor, mordentransferencia mot, mfacturacliente mfc,  tcargoorden tco   
				 where   
				 (mor.test_estado<>'A' and mord_fechliqu between '"+fechaDesde.ToString("yyyy-MM-dd")+@"' and '"+fechaHasta.ToString("yyyy-MM-dd")+@"') and  
				 mor.pdoc_codigo=mot.pdoc_codigo and mor.mord_numeorde=mot.mord_numeorde and   
				 mfc.pdoc_codigo=mot.pdoc_factura and mfc.mfac_numedocu=mot.mfac_numero and   
				 tco.tcar_cargo=mot.tcar_cargo and tco.tcar_cargo=tc.tcar_cargo and mfc.mfac_tipodocu='N'  
				) 
 				VALORLIQU  
				from   
				tcargoorden tc;");
			dgrRepuestos.DataSource=dsTotalR.Tables[0];
			for(int i=0;i<dsTotalR.Tables[0].Rows.Count;i++)
			{
				totalValorProcesoR+=Convert.ToDouble(dsTotalR.Tables[0].Rows[i]["VALORPROC"]);
				totalValorLiquidadasR+=Convert.ToDouble(dsTotalR.Tables[0].Rows[i]["VALORLIQU"]);
			}
			dgrRepuestos.DataBind();

			pnlResultados.Visible=true;
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
			this.dgrTotalO.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrTotalO_ItemDataBound);
			this.dgrRepuestos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrRepuestos_ItemDataBound);
			this.dgrRecepcionistas.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrRecepcionistas_ItemDataBound);
			this.dgrMecanicos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrMecanicos_ItemDataBound);

		}
		#endregion

		private void dgrTotalO_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text=totalLiquidadas.ToString();
				e.Item.Cells[3].Text=totalValorLiquidadas.ToString("C");
				e.Item.Cells[4].Text=totalProceso.ToString();
				e.Item.Cells[5].Text=totalValorProceso.ToString("C");
			}
		}

		private void dgrMecanicos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text=totalLiquidadasM.ToString();
				e.Item.Cells[3].Text=totalValorLiquidadasM.ToString("C");
				e.Item.Cells[4].Text=totalProcesoM.ToString();
				e.Item.Cells[5].Text=totalValorProcesoM.ToString("C");
			}
		}

		private void dgrRepuestos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[1].Text=totalValorLiquidadasR.ToString("C");
				e.Item.Cells[2].Text=totalValorProcesoR.ToString("C");
			}
		}

		private void dgrRecepcionistas_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text=totalLiquidadasV.ToString();
				e.Item.Cells[3].Text=totalValorLiquidadasV.ToString("C");
				e.Item.Cells[4].Text=totalProcesoV.ToString();
				e.Item.Cells[5].Text=totalProcesoV10.ToString();
				e.Item.Cells[6].Text=totalProcesoV20.ToString();
				e.Item.Cells[7].Text=totalProcesoVM.ToString();
				e.Item.Cells[8].Text=totalValorProcesoV.ToString("C");
			}
		}

	}
}
