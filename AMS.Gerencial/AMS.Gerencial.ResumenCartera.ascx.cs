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
	///		Descripción breve de AMS_Gerencial_ResumenCartera.
	/// </summary>
	public partial class AMS_Gerencial_ResumenCartera : System.Web.UI.UserControl
	{
		private double totalC,vencerC,d30C,d60C,d90C,d120C,masC;
		private double totalP,vencerP,d30P,d60P,d90P,d120P,masP;
		private double totalCA1,totalCA2,totalCA3,totalCM1A1,totalCM2A1,totalCM1A2,totalCM2A2,totalCM1A3,totalCM2A3;
		private double totalMO,totalPO,interesP,interesC,totalMP,interesPP;
		protected DataTable dtClientes,dtProveedores,dtCompara;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
				lblFecha.Text=DateTime.Now.ToString("yyyy/MM/dd");
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
			this.dgrCarteraC.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrCarteraC_ItemDataBound);
			this.dgrComparativos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrCarteraComp_ItemDataBound);
			this.dgrCarteraP.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrCarteraP_ItemDataBound);
			this.dgrObligaciones.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrObligaciones_ItemDataBound);

		}
		#endregion

		public void Generar_Click(object sender, System.EventArgs e)
		{
			DataSet dsClientes=new DataSet();
			DataSet dsProveedores=new DataSet();
			DataSet dsObligaciones=new DataSet();
			DataSet dsCompara=new DataSet();
			#region Clientes
			//Clientes
			totalC=vencerC=d30C=d60C=d90C=d120C=masC=0;
			DBFunctions.Request(dsClientes,IncludeSchema.NO,
				"Select pdc.pdoc_codigo, pdc.pdoc_nombre, pdc.tdoc_tipodocu, "+
				"sum(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) VALOR, "+//3
				"cast(0 as decimal) VENCER, "+//4
				"cast(0 as decimal) V30, "+//5
				"cast(0 as decimal) V60, "+//6
				"cast(0 as decimal) V90, "+//7
				"cast(0 as decimal) V120, "+//8
				"cast(0 as decimal) VMAX  "+ //9
				"from DBXSCHEMA.mfacturacliente mfc, DBXSCHEMA.pdocumento pdc "+
				"where "+
				"pdc.pdoc_codigo=mfc.pdoc_codigo and (pdc.pdoc_gerencial <> 'N' or pdc.pdoc_gerencial is null) and "+
				"(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 and "+
				"mnit_nit not in(select PNITAL_NITTALLER from PNITTALLER) and "+
				"mnit_nit not in(select mnit_nit from CEMPRESA) and "+
				"mnit_nit not in(select mnit_nit from PCASAMATRIZ where pcas_consgere='N') and "+
				"tdoc_tipodocu in('FC','NC')  "+
				"group by pdc.pdoc_codigo, pdc.pdoc_nombre, pdc.tdoc_tipodocu "+
				"order by pdc.pdoc_nombre;");
			//Totales
			if(dsClientes.Tables.Count>0)
			{
				for(int i=0;i<dsClientes.Tables[0].Rows.Count;i++)
				{
					int c=0;
					int z=1;
					if(dsClientes.Tables[0].Rows[i]["tdoc_tipodocu"].ToString().Equals("NC"))
						z=-1;
					for(int j=0;j<=150;j+=30)
					{
						DataSet dsClientesA=new DataSet();
						DBFunctions.Request(dsClientesA,IncludeSchema.NO,GenerarConsultaCliente(j,dsClientes.Tables[0].Rows[i]["pdoc_codigo"].ToString()));
						if(dsClientesA.Tables[0].Rows.Count>0)
							dsClientes.Tables[0].Rows[i][4+c]=Convert.ToDouble(dsClientesA.Tables[0].Rows[0][0]);
						c++;
						switch(j)
						{
							case 0:vencerC+=z*Convert.ToDouble(dsClientesA.Tables[0].Rows[0][0]);break;
							case 30:d30C+=z*Convert.ToDouble(dsClientesA.Tables[0].Rows[0][0]);break;
							case 60:d60C+=z*Convert.ToDouble(dsClientesA.Tables[0].Rows[0][0]);break;
							case 90:d90C+=z*Convert.ToDouble(dsClientesA.Tables[0].Rows[0][0]);break;
							case 120:d120C+=z*Convert.ToDouble(dsClientesA.Tables[0].Rows[0][0]);break;
							default:masC+=z*Convert.ToDouble(dsClientesA.Tables[0].Rows[0][0]);break;
						}
						totalC+=z*Convert.ToDouble(dsClientesA.Tables[0].Rows[0][0]);
					}
				}
				dtClientes=dsClientes.Tables[0];
				dgrCarteraC.DataSource=dsClientes.Tables[0];
				dgrCarteraC.DataBind();
			}
			#endregion

			#region Proveedores
			//Proveedores
			totalP=vencerP=d30P=d60P=d90P=d120P=masP=0;
			DBFunctions.Request(dsProveedores,IncludeSchema.NO,
		    	@"Select pdc.pdoc_codigo, pdc.pdoc_nombre, pdc.tdoc_tipodocu,   
				 sum(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) VALOR,      
				 cast(0 as decimal) VENCER,      
				 cast(0 as decimal) V30,      
				 cast(0 as decimal) V60,      
				 cast(0 as decimal) V90,      
				 cast(0 as decimal) V120,      
				 cast(0 as decimal) VMAX        
				 from DBXSCHEMA.mfacturaproveedor mfc, DBXSCHEMA.pdocumento pdc   
				 where   
				 pdc.pdoc_codigo=mfc.pdoc_codiordepago and (pdc.pdoc_gerencial <> 'N' or pdc.pdoc_gerencial is null) and   
				 (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 and   
				 tdoc_tipodocu in('FP','NP')    
				 group by pdc.pdoc_codigo, pdc.pdoc_nombre, pdc.tdoc_tipodocu   
				 order by pdc.pdoc_nombre;");
			//Totales
			if(dsProveedores.Tables.Count>0)
			{
				for(int i=0;i<dsProveedores.Tables[0].Rows.Count;i++)
				{
					int c=0;
					int z=1;
					if(dsProveedores.Tables[0].Rows[i]["tdoc_tipodocu"].ToString().Equals("NP"))
						z=-1;
					for(int j=0;j<=150;j+=30)
					{
						DataSet dsProveedoresA=new DataSet();
						DBFunctions.Request(dsProveedoresA,IncludeSchema.NO,GenerarConsultaProveedor(j,dsProveedores.Tables[0].Rows[i]["pdoc_codigo"].ToString()));
						if(dsProveedoresA.Tables[0].Rows.Count>0)
							dsProveedores.Tables[0].Rows[i][4+c]=Convert.ToDouble(dsProveedoresA.Tables[0].Rows[0][0]);
						c++;
						switch(j)
						{
							case 0:vencerP+=z*Convert.ToDouble(dsProveedoresA.Tables[0].Rows[0][0]);break;
							case 30:d30P+=z*Convert.ToDouble(dsProveedoresA.Tables[0].Rows[0][0]);break;
							case 60:d60P+=z*Convert.ToDouble(dsProveedoresA.Tables[0].Rows[0][0]);break;
							case 90:d90P+=z*Convert.ToDouble(dsProveedoresA.Tables[0].Rows[0][0]);break;
							case 120:d120P+=z*Convert.ToDouble(dsProveedoresA.Tables[0].Rows[0][0]);break;
							default:masP+=z*Convert.ToDouble(dsProveedoresA.Tables[0].Rows[0][0]);break;
						}
						totalP+=z*Convert.ToDouble(dsProveedoresA.Tables[0].Rows[0][0]);
					}
				}
				dtProveedores=dsProveedores.Tables[0];
				dgrCarteraP.DataSource=dsProveedores.Tables[0];
				dgrCarteraP.DataBind();
			}
			#endregion

			#region Obligaciones
			//Obligaciones
			totalMO=totalPO=interesP=interesC=totalMP=interesPP=0;
			DBFunctions.Request(dsObligaciones,IncludeSchema.NO,
			   @"Select mob.PCUE_CODIGO concat ' - ' concat pcn.pcue_nombre cuenta, mob.MOBL_NUMERO, mob.PDOC_CODIGO, mob.MOBL_NUMEDOCU, mob.PALM_ALMACEN,   
				 mob.MOBL_MONTPESOS, mob.MOBL_MONDOLARES, mob.MOBL_NUMECUOTAS, mob.MOBL_MONTPAGADO, mob.MOBL_INTERESPAGADO,   
                 dob.MOBL_MONTPESOS - dob.dOBL_MONTPAGO as saldo, mob.PCRE_CODIGO, mob.PTAS_CODIGO concat ' (' concat rtrim(char(pts.ptas_monto)) concat')' dob_tasa,    
				 mob.MOBL_TASAINTERES, mob.TCON_CODIGO, mob.MOBL_DETALLE,   
				 mob.MOBL_AUTORIZA, pcr.pcre_nombre, pts.ptas_nombre, tcn.tcon_nombre, dob.mobl_fecha mobl_fechapago, dob.dobl_numepago,   
				 dob.mobl_montpesos dobl_montpesos, dob.mobl_montinteres dobl_montinteres   
				 from mobligacionfinanciera mob, pcreditobancario pcr, tcondicioncreditobancario tcn, ptasacredito pts,   
				 pcuentacorriente pcn, DOBLIGACIONFINANCIERAPLANPAGO dob   
				 where mob.pcre_codigo=pcr.pcre_codigo and mob.tcon_codigo=tcn.tcon_codigo and mob.ptas_codigo=pts.ptas_codigo and   
				 mob.MOBL_MONTPESOS>mob.MOBL_MONTPAGADO and pcn.PCUE_CODIGO=mob.PCUE_CODIGO and dob.mobl_numero=mob.mobl_numero and   
				 dob.dobl_montpago+dob.dobl_intepago<dob.mobl_montpesos+dob.mobl_montinteres   
				 order by dob.mobl_fecha;");
			//Totales
			if(dsObligaciones.Tables.Count>0)
			{
				for(int i=0;i<dsObligaciones.Tables[0].Rows.Count;i++)
				{
					totalMO  +=Convert.ToDouble(dsObligaciones.Tables[0].Rows[i]["MOBL_MONTPESOS"]);
					totalPO  +=Convert.ToDouble(dsObligaciones.Tables[0].Rows[i]["MOBL_MONTPAGADO"]);
					interesP +=Convert.ToDouble(dsObligaciones.Tables[0].Rows[i]["MOBL_INTERESPAGADO"]);
					interesC +=Convert.ToDouble(dsObligaciones.Tables[0].Rows[i]["SALDO"]);
					totalMP  +=Convert.ToDouble(dsObligaciones.Tables[0].Rows[i]["dobl_montpesos"]);
					interesPP+=Convert.ToDouble(dsObligaciones.Tables[0].Rows[i]["dobl_montinteres"]);
				}
				dgrObligaciones.DataSource=dsObligaciones.Tables[0];
				dgrObligaciones.DataBind();
			}
			#endregion

			#region Comparativo
			//Clientes
			totalCA1=totalCA2=totalCA3=totalCM1A1=totalCM2A1=totalCM1A2=totalCM2A2=totalCM1A3=totalCM2A3=0;
			//Hace 2 años
			DateTime fchA1=DateTime.Now.AddYears(-2);
			DateTime fchA10=new DateTime(fchA1.Year,1,1);
			//Hace 1 año
			DateTime fchA2=DateTime.Now.AddYears(-1);
			DateTime fchA20=new DateTime(fchA2.Year,1,1);
			//Este año
			DateTime fchA3=DateTime.Now;
			DateTime fchA30=new DateTime(fchA3.Year,1,1);
            //Mes pasado hace 2 años
			DateTime fchM1A1=DateTime.Now.AddMonths(-1).AddYears(-2);
			DateTime fchM1A10=new DateTime(fchM1A1.Year,fchM1A1.Month,1);
			//Este mes hace 2 años
			DateTime fchM2A1=DateTime.Now.AddYears(-2);
			DateTime fchM2A10=new DateTime(fchM2A1.Year,fchM2A1.Month,1);
			
			//Mes pasado hace 1 año
			DateTime fchM1A2=DateTime.Now.AddMonths(-1).AddYears(-1);
			DateTime fchM1A20=new DateTime(fchM1A2.Year,fchM1A2.Month,1);
			//Este mes hace 1 año
			DateTime fchM2A2=DateTime.Now.AddYears(-1);
			DateTime fchM2A20=new DateTime(fchM2A2.Year,fchM2A2.Month,1);

			//Mes pasado este año
			DateTime fchM1A3=DateTime.Now.AddMonths(-1);
			DateTime fchM1A30=new DateTime(fchM1A3.Year,fchM1A3.Month,1);
			//Este mes este año
			DateTime fchM2A3=DateTime.Now;
			DateTime fchM2A30=new DateTime(fchM2A3.Year,fchM2A3.Month,1);
			string sqlComp="";
			string filtro=@"(select coalesce(sum(mfac_valofact),0)   
				  from mfacturacliente mfc1, pdocumento pdc1   
				  where pdc1.pdoc_codigo=pdc.pdoc_codigo and pdc1.tdoc_tipodocu=pdc.tdoc_tipodocu and   
				  mnit_nit not in(select PNITAL_NITTALLER from PNITTALLER) and   
				  mnit_nit not in(select mnit_nit from CEMPRESA) and   
				  mnit_nit not in(select mnit_nit from PCASAMATRIZ where pcas_consgere='N') and   
				  pdc.pdoc_codigo=mfc1.pdoc_codigo and mfc1.mfac_factura between '@FECHA_DESDE@' and '@FECHA_HASTA@' ) ";
			sqlComp="Select pdc.pdoc_codigo, pdc.pdoc_nombre, pdc.tdoc_tipodocu, "+
				filtro.Replace("@FECHA_DESDE@",fchA10.ToString("yyyy-MM-dd")).Replace("@FECHA_HASTA@",fchA1.ToString("yyyy-MM-dd"))+" AS ANO1, "+
				filtro.Replace("@FECHA_DESDE@",fchA20.ToString("yyyy-MM-dd")).Replace("@FECHA_HASTA@",fchA2.ToString("yyyy-MM-dd"))+" AS ANO2, "+
				filtro.Replace("@FECHA_DESDE@",fchA30.ToString("yyyy-MM-dd")).Replace("@FECHA_HASTA@",fchA3.ToString("yyyy-MM-dd"))+" AS ANO3, "+
				filtro.Replace("@FECHA_DESDE@",fchM1A10.ToString("yyyy-MM-dd")).Replace("@FECHA_HASTA@",fchM1A1.ToString("yyyy-MM-dd"))+" AS MES1ANO1, "+
				filtro.Replace("@FECHA_DESDE@",fchM2A10.ToString("yyyy-MM-dd")).Replace("@FECHA_HASTA@",fchM2A1.ToString("yyyy-MM-dd"))+" AS MES2ANO1, "+
				filtro.Replace("@FECHA_DESDE@",fchM1A20.ToString("yyyy-MM-dd")).Replace("@FECHA_HASTA@",fchM1A2.ToString("yyyy-MM-dd"))+" AS MES1ANO2, "+
				filtro.Replace("@FECHA_DESDE@",fchM2A20.ToString("yyyy-MM-dd")).Replace("@FECHA_HASTA@",fchM2A2.ToString("yyyy-MM-dd"))+" AS MES2ANO2, "+
				filtro.Replace("@FECHA_DESDE@",fchM1A30.ToString("yyyy-MM-dd")).Replace("@FECHA_HASTA@",fchM1A3.ToString("yyyy-MM-dd"))+" AS MES1ANO3, "+
				filtro.Replace("@FECHA_DESDE@",fchM2A30.ToString("yyyy-MM-dd")).Replace("@FECHA_HASTA@",fchM2A3.ToString("yyyy-MM-dd"))+" AS MES2ANO3 "+
				"from pdocumento pdc "+
				"where "+
				"(pdc.pdoc_gerencial <> 'N' or pdc.pdoc_gerencial is null) and tdoc_tipodocu in('FC','NC') "+
				"order by pdc.pdoc_nombre;";
			DBFunctions.Request(dsCompara,IncludeSchema.NO,sqlComp);
			//Totales
			totalCA1=totalCA2=totalCA3=totalCM1A1=totalCM2A1=totalCM1A2=totalCM2A2=totalCM1A3=totalCM2A3=0;
			if(dsCompara.Tables[0].Rows.Count>0){
				dtCompara=dsCompara.Tables[0];
				for(int i=0;i<dsCompara.Tables[0].Rows.Count;i++)
				{
					int z=1;
					if(dsCompara.Tables[0].Rows[i]["tdoc_tipodocu"].ToString().Equals("NC"))
						z=-1;
					totalCA1  +=Convert.ToDouble(dtCompara.Rows[i]["ANO1"])*z;
					totalCA2  +=Convert.ToDouble(dtCompara.Rows[i]["ANO2"])*z;
					totalCA3  +=Convert.ToDouble(dtCompara.Rows[i]["ANO3"])*z;
					totalCM1A1+=Convert.ToDouble(dtCompara.Rows[i]["MES1ANO1"])*z;
					totalCM2A1+=Convert.ToDouble(dtCompara.Rows[i]["MES2ANO1"])*z;
					totalCM1A2+=Convert.ToDouble(dtCompara.Rows[i]["MES1ANO2"])*z;
					totalCM2A2+=Convert.ToDouble(dtCompara.Rows[i]["MES2ANO2"])*z;
					totalCM1A3+=Convert.ToDouble(dtCompara.Rows[i]["MES1ANO3"])*z;
					totalCM2A3+=Convert.ToDouble(dtCompara.Rows[i]["MES2ANO3"])*z;
				}
				
				dgrComparativos.DataSource=dsCompara.Tables[0];
				for(int n=2;n<=10;n++)
					dgrComparativos.Columns[n].HeaderStyle.HorizontalAlign=HorizontalAlign.Center;
				IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
				dgrComparativos.Columns[2].HeaderText=fchA10.Year.ToString();
				dgrComparativos.Columns[3].HeaderText=fchA20.Year.ToString();
				dgrComparativos.Columns[4].HeaderText=fchA30.Year.ToString();
				dgrComparativos.Columns[5].HeaderText=fchM1A10.ToString("MMM yyyy");
				dgrComparativos.Columns[6].HeaderText=fchM2A10.ToString("MMM yyyy");
				dgrComparativos.Columns[7].HeaderText=fchM1A20.ToString("MMM yyyy");
				dgrComparativos.Columns[8].HeaderText=fchM2A20.ToString("MMM yyyy");
				dgrComparativos.Columns[9].HeaderText=fchM1A30.ToString("MMM yyyy");
				dgrComparativos.Columns[10].HeaderText=fchM2A30.ToString("MMM yyyy");
				dgrComparativos.DataBind();
			}
			#endregion
			pnlResultados.Visible=true;
		}
		private string GenerarConsultaCliente(int dias, string codigo)
		{
			string d="";
			if(dias==0)
				d="days(current date)-days(mfac_vence)<=0 and ";
			else if(dias>120)
				d="days(current date)-days(mfac_vence)>"+120+" and ";
			else
				d="days(current date)-days(mfac_vence)<"+dias.ToString()+" and days(current date)-days(mfac_vence)>"+(dias-30)+" and ";
			return(
				"Select "+
				"coalesce(sum(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),0) VALOR "+
				"from DBXSCHEMA.mfacturacliente mfc, DBXSCHEMA.pdocumento pdc "+
				"where "+
				"pdc.pdoc_codigo=mfc.pdoc_codigo and pdc.pdoc_codigo='"+codigo+"' and "+
				"(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 and "+
				"mnit_nit not in(select PNITAL_NITTALLER from PNITTALLER) and "+
				"mnit_nit not in(select mnit_nit from PCASAMATRIZ where pcas_consgere='N') and "+
				"mnit_nit not in(select mnit_nit from CEMPRESA) and "+
				d+" tdoc_tipodocu in('FC','NC'); "
			);
		}

		private string GenerarConsultaProveedor(int dias, string codigo)
		{
			string d="";
			if(dias==0)
				d="days(current date)-days(mfac_vence)<=0 and ";
			else if(dias>120)
				d="days(current date)-days(mfac_vence)>"+120+" and ";
			else
				d="days(current date)-days(mfac_vence)<"+dias.ToString()+" and days(current date)-days(mfac_vence)>"+(dias-30)+" and ";
			return(
				"Select "+
				"coalesce(sum(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),0) VALOR "+
				"from DBXSCHEMA.mfacturaproveedor mfc, DBXSCHEMA.pdocumento pdc "+
				"where "+
				"pdc.pdoc_codigo=mfc.pdoc_codiordepago and pdc.pdoc_codigo='"+codigo+"' and "+
				"(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 and "+
				d+" tdoc_tipodocu in('FP','NP'); "
			);
		}

		private void dgrCarteraP_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item || e.Item.ItemType==ListItemType.AlternatingItem)
			{
				if(dtProveedores.Rows[e.Item.ItemIndex]["tdoc_tipodocu"].ToString().Equals("NP"))
					e.Item.Cells[0].BackColor=e.Item.Cells[1].BackColor=Color.BurlyWood;
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text="$"+Math.Round(totalP,0).ToString("#,##0.#");
				e.Item.Cells[3].Text="$"+Math.Round(vencerP,0).ToString("#,##0.#")+"<br>"+((vencerP/totalP)*100).ToString("##0.##")+"%";
				e.Item.Cells[4].Text="$"+Math.Round(d30P,0).ToString("#,##0.#")+"<br>"+((d30P/totalP)*100).ToString("##0.##")+"%";;
				e.Item.Cells[5].Text="$"+Math.Round(d60P,0).ToString("#,##0.#")+"<br>"+((d60P/totalP)*100).ToString("##0.##")+"%";;
				e.Item.Cells[6].Text="$"+Math.Round(d90P,0).ToString("#,##0.#")+"<br>"+((d90P/totalP)*100).ToString("##0.##")+"%";;
				e.Item.Cells[7].Text="$"+Math.Round(d120P,0).ToString("#,##0.0#")+"<br>"+((d120P/totalP)*100).ToString("##0.##")+"%";;
				e.Item.Cells[8].Text="$"+Math.Round(masP,0).ToString("#,##0.#")+"<br>"+((masP/totalP)*100).ToString("##0.##")+"%";;
			}
		}
		private void dgrCarteraC_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item || e.Item.ItemType==ListItemType.AlternatingItem)
			{
				if(dtClientes.Rows[e.Item.ItemIndex]["tdoc_tipodocu"].ToString().Equals("NC"))
					e.Item.Cells[0].BackColor=e.Item.Cells[1].BackColor=Color.BurlyWood;
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text="$"+Math.Round(totalC,0).ToString("#,##0.#");
				e.Item.Cells[3].Text="$"+Math.Round(vencerC,0).ToString("#,##0.#")+"<br>"+((vencerC/totalC)*100).ToString("##0.##")+"%";
				e.Item.Cells[4].Text="$"+Math.Round(d30C,0).ToString("#,##0.#")+"<br>"+((d30C/totalC)*100).ToString("##0.##")+"%";
				e.Item.Cells[5].Text="$"+Math.Round(d60C,0).ToString("#,##0.#")+"<br>"+((d60C/totalC)*100).ToString("##0.##")+"%";
				e.Item.Cells[6].Text="$"+Math.Round(d90C,0).ToString("#,##0.#")+"<br>"+((d90C/totalC)*100).ToString("##0.##")+"%";
				e.Item.Cells[7].Text="$"+Math.Round(d120C,0).ToString("#,##0.0#")+"<br>"+((d120C/totalC)*100).ToString("##0.##")+"%";
				e.Item.Cells[8].Text="$"+Math.Round(masC,0).ToString("#,##0.#")+"<br>"+((masC/totalC)*100).ToString("##0.##")+"%";
			}
		}

		private void dgrCarteraComp_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item || e.Item.ItemType==ListItemType.AlternatingItem)
			{
				if(dtCompara.Rows[e.Item.ItemIndex]["tdoc_tipodocu"].ToString().Equals("NC"))
					e.Item.Cells[0].BackColor=e.Item.Cells[1].BackColor=Color.BurlyWood;
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text="$"+Math.Round(totalCA1,0).ToString("#,##0.#");
				e.Item.Cells[3].Text="$"+Math.Round(totalCA2,0).ToString("#,##0.#");
				e.Item.Cells[4].Text="$"+Math.Round(totalCA3,0).ToString("#,##0.#");
				e.Item.Cells[5].Text="$"+Math.Round(totalCM1A1,0).ToString("#,##0.#");
				e.Item.Cells[6].Text="$"+Math.Round(totalCM2A1,0).ToString("#,##0.#");
				e.Item.Cells[7].Text="$"+Math.Round(totalCM1A2,0).ToString("#,##0.#");
				e.Item.Cells[8].Text="$"+Math.Round(totalCM2A2,0).ToString("#,##0.#");
				e.Item.Cells[9].Text="$"+Math.Round(totalCM1A3,0).ToString("#,##0.#");
				e.Item.Cells[10].Text="$"+Math.Round(totalCM2A3,0).ToString("#,##0.#");
			}
		}

		private void dgrObligaciones_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item || e.Item.ItemType==ListItemType.AlternatingItem)
			{
				if(Convert.ToDateTime(e.Item.Cells[2].Text)<DateTime.Now)
					e.Item.Cells[4].BackColor=e.Item.Cells[5].BackColor=e.Item.Cells[6].BackColor=e.Item.Cells[7].BackColor=e.Item.Cells[8].BackColor=Color.Tomato;
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[6].Text="$"+Math.Round(totalMO,0).ToString("#,##0.#");
				e.Item.Cells[7].Text="$"+Math.Round(totalPO,0).ToString("#,##0.#");
				e.Item.Cells[8].Text="$"+Math.Round(interesP,0).ToString("#,##0.#");
				e.Item.Cells[9].Text="$"+Math.Round(interesC,0).ToString("#,##0.#");
				e.Item.Cells[4].Text="$"+Math.Round(totalMP,0).ToString("#,##0.#");
				e.Item.Cells[5].Text="$"+Math.Round(interesPP,0).ToString("#,##0.#")+"<br>";
			}
		}
	}
}
