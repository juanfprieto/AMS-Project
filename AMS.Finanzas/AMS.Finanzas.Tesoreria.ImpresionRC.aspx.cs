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

namespace AMS.Finanzas.Tesoreria
{
	public partial  class ImpresionRC : System.Web.UI.Page
	{
		protected DataTable dtDocs,dtNoCaus,dtAbonos,dtPagos,dtRtns;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			this.Llenar_Formato();
		}
		
		protected void Preparar_dtDocs()
		{
			dtDocs=new DataTable();
			dtDocs.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
			dtDocs.Columns.Add(new DataColumn("NUMERODOCUMENTO", typeof(string)));
			dtDocs.Columns.Add(new DataColumn("VALORABONAR", typeof(double)));
		}
		
		protected void Preparar_dtNoCaus()
		{
			dtNoCaus=new DataTable();
			dtNoCaus.Columns.Add(new DataColumn("DESCRIPCION", typeof(string)));
			dtNoCaus.Columns.Add(new DataColumn("CUENTA", typeof(string)));
			dtNoCaus.Columns.Add(new DataColumn("SEDE", typeof(string)));
			dtNoCaus.Columns.Add(new DataColumn("CENTROCOSTO", typeof(string)));
			dtNoCaus.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
			dtNoCaus.Columns.Add(new DataColumn("NUMERO", typeof(string)));
			dtNoCaus.Columns.Add(new DataColumn("NIT", typeof(string)));
			dtNoCaus.Columns.Add(new DataColumn("VALORDEBITO", typeof(double)));
			dtNoCaus.Columns.Add(new DataColumn("VALORCREDITO", typeof(double)));
			dtNoCaus.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
		}
		
		protected void Preparar_dtAbonos()
		{
			dtAbonos=new DataTable();
			dtAbonos.Columns.Add("PREFIJO",typeof(string));
			dtAbonos.Columns.Add("NUMERO",typeof(string));
			dtAbonos.Columns.Add("VALORPEDIDO",typeof(double));
			dtAbonos.Columns.Add("ABONO",typeof(double));
		}
		
		protected void Preparar_dtPagos()
		{
			dtPagos=new DataTable();
			dtPagos.Columns.Add(new DataColumn("TIPO", typeof(string)));
			dtPagos.Columns.Add(new DataColumn("CODIGOBANCO", typeof(string)));
			dtPagos.Columns.Add(new DataColumn("NUMERODOC", typeof(string)));
			dtPagos.Columns.Add(new DataColumn("TIPOMONEDA", typeof(string)));
			dtPagos.Columns.Add(new DataColumn("VALOR", typeof(double)));
			dtPagos.Columns.Add(new DataColumn("VALORTC", typeof(double)));
			dtPagos.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
			dtPagos.Columns.Add(new DataColumn("FECHA", typeof(string)));
		}
		
		protected void Preparar_dtRtns()
		{
			dtRtns=new DataTable();
			dtRtns.Columns.Add(new DataColumn("CODRET", typeof(string)));
			dtRtns.Columns.Add(new DataColumn("VALOR", typeof(double)));
		}
		
		protected void Llenar_dgDocs()
		{
			DataSet ds=new DataSet();
			DataRow fila;
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,ddet_valodocu FROM dbxschema.ddetallefacturacliente WHERE pdoc_coddocref='"+Request.QueryString["pref"]+"' AND ddet_numedocu="+Request.QueryString["num"]+"");
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codiordepago,mfac_numeordepago,dcaj_valorecicaja FROM dbxschema.dcajaproveedor WHERE pdoc_codigo='"+Request.QueryString["pref"]+"' AND mcaj_numero="+Request.QueryString["num"]+"");
			if(ds.Tables[0].Rows.Count!=0 || ds.Tables[0].Rows.Count!=0)
			{
				this.lbDocs.Text="Documentos Pagados o Abonados";
				if(dtDocs==null)
					this.Preparar_dtDocs();
				if(ds.Tables[0].Rows.Count!=0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						fila=dtDocs.NewRow();
						fila[0]=ds.Tables[0].Rows[i][0].ToString();
						fila[1]=ds.Tables[0].Rows[i][1].ToString();
						fila[2]=Convert.ToDouble(ds.Tables[0].Rows[i][2]);
						dtDocs.Rows.Add(fila);
						dgDocs.DataSource=dtDocs;
						dgDocs.DataBind();
					}	
				}
				if(ds.Tables[1].Rows.Count!=0)
				{
					for(int i=0;i<ds.Tables[1].Rows.Count;i++)
					{
						fila=dtDocs.NewRow();
						fila[0]=ds.Tables[1].Rows[i][0].ToString();
						fila[1]=ds.Tables[1].Rows[i][1].ToString();
						fila[2]=Convert.ToDouble(ds.Tables[1].Rows[i][2]);
						dtDocs.Rows.Add(fila);
						dgDocs.DataSource=dtDocs;
						dgDocs.DataBind();
					}
				}
			}
		}
		
		protected void Llenar_dgNoCaus()
		{
			DataSet ds=new DataSet();
			DataRow fila;
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT DCAJ.dcaj_concepto,DCAJ.mcue_codipuc,PALM.palm_descripcion,PCEN.pcen_nombre,DCAJ.dcaj_prefdocu,DCAJ.dcaj_numedocu,DCAJ.mnit_nit,DCAJ.dcaj_valor,DCAJ.dcaj_naturaleza,DCAJ.dcaj_valobase FROM dbxschema.dcajavarios DCAJ,dbxschema.palmacen PALM ,dbxschema.pcentrocosto PCEN WHERE tvig_vigencia='V' and DCAJ.palm_almacen=PALM.palm_almacen AND DCAJ.pcen_codigo=PCEN.pcen_codigo AND DCAJ.pdoc_codigo='" + Request.QueryString["pref"] + "' AND DCAJ.mcaj_numero=" + Request.QueryString["num"] + "");
			if(ds.Tables[0].Rows.Count!=0)
			{
				this.lbNoCaus.Text="Conceptos Ingresos - Egresos";
				if(dtNoCaus==null)
					this.Preparar_dtNoCaus();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=dtNoCaus.NewRow();
					fila[0]=ds.Tables[0].Rows[i][0].ToString();
					fila[1]=ds.Tables[0].Rows[i][1].ToString();
					fila[2]=ds.Tables[0].Rows[i][2].ToString();
					fila[3]=ds.Tables[0].Rows[i][3].ToString();
					fila[4]=ds.Tables[0].Rows[i][4].ToString();
					fila[5]=ds.Tables[0].Rows[i][5].ToString();
					fila[6]=ds.Tables[0].Rows[i][6].ToString();
					if(ds.Tables[0].Rows[i][8].ToString()=="D")
					{
						fila[7]=Convert.ToDouble(ds.Tables[0].Rows[i][7]);
						fila[8]=0;
					}
					else if(ds.Tables[0].Rows[i][8].ToString()=="C")
					{
						fila[7]=0;
						fila[8]=Convert.ToDouble(ds.Tables[0].Rows[i][7]);
					}
					fila[9]=Convert.ToDouble(ds.Tables[0].Rows[i][9]);
					dtNoCaus.Rows.Add(fila);
					dgNoCaus.DataSource=dtNoCaus;
					dgNoCaus.DataBind();
				}
			}
		}
		
		protected void Llenar_dgAbonos()
		{
			DataSet ds=new DataSet();
			DataRow fila;
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MANT.mped_codigo,MANT.mped_numepedi, MPED.mped_valounit - mped_valodesc, MANT.mant_valorecicaja FROM dbxschema.manticipovehiculo MANT,dbxschema.mpedidovehiculo MPED WHERE MPED.pdoc_codigo=MANT.mped_codigo AND MPED.mped_numepedi=MANT.mped_numepedi AND MANT.pdoc_codigo='"+Request.QueryString["pref"]+"' AND MANT.mcaj_numero="+Request.QueryString["num"]+"");
			if(ds.Tables[0].Rows.Count!=0)
			{
				this.lbVarios.Text="Abonos a Vehículos";
				if(dtAbonos==null)
					this.Preparar_dtAbonos();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=dtAbonos.NewRow();
					fila[0]=ds.Tables[0].Rows[i][0].ToString();
					fila[1]=ds.Tables[0].Rows[i][1].ToString();
					fila[2]=Convert.ToDouble(ds.Tables[0].Rows[i][2]);
					fila[3]=Convert.ToDouble(ds.Tables[0].Rows[i][3]);
					dtAbonos.Rows.Add(fila);
					dgAbonos.DataSource=dtAbonos;
					dgAbonos.DataBind();
				}
			}
		}
		
		protected void Llenar_dgPagos()
		{
			DataSet ds=new DataSet();
			DataRow fila;
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT TTIP.ttip_nombre,MCAJ.pban_codigo,MCAJ.mcpag_numerodoc,MCAJ.mcpag_tipomoneda,MCAJ.mcpag_valor,MCAJ.mcpag_valortasacambio,MCAJ.mcpag_valorbase,MCAJ.mcpag_fecha FROM dbxschema.mcajapago MCAJ,dbxschema.ttipopago TTIP WHERE MCAJ.ttip_codigo=TTIP.ttip_codigo AND MCAJ.pdoc_codigo='"+Request.QueryString["pref"]+"' AND MCAJ.mcaj_numero="+Request.QueryString["num"]+"");
			if(ds.Tables[0].Rows.Count!=0)
			{
				this.lbPagos.Text="Relación de Pagos";
				if(dtPagos==null)
					this.Preparar_dtPagos();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=dtPagos.NewRow();
					fila[0]=ds.Tables[0].Rows[i][0].ToString();
					fila[1]=DBFunctions.SingleData("SELECT pban_nombre FROM dbxschema.pbanco WHERE pban_codigo='"+ds.Tables[0].Rows[i][1].ToString()+"'");
					fila[2]=ds.Tables[0].Rows[i][2].ToString();
					fila[3]=ds.Tables[0].Rows[i][3].ToString();
					fila[4]=Convert.ToDouble(ds.Tables[0].Rows[i][4]);
					fila[5]=Convert.ToDouble(ds.Tables[0].Rows[i][5]);
					fila[6]=Convert.ToDouble(ds.Tables[0].Rows[i][6]);
					fila[7]=Convert.ToDateTime(ds.Tables[0].Rows[i][7]).ToString("yyyy-MM-dd");
					dtPagos.Rows.Add(fila);
					dgPagos.DataSource=dtPagos;
					dgPagos.DataBind();
				}
			}
		}
		
		protected void Llenar_dgRtns()
		{
			DataSet ds=new DataSet();
			DataRow fila;
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT PRET.pret_nombre,MCAJ.mcaj_valor FROM dbxschema.pretencion PRET,dbxschema.mcajaretencion MCAJ WHERE MCAJ.pret_codigo=PRET.pret_codigo AND MCAJ.pdoc_codigo='"+Request.QueryString["pref"]+"' AND MCAJ.mcaj_numero="+Request.QueryString["num"]+"");
			if(ds.Tables[0].Rows.Count!=0)
			{
				this.lbRets.Text="Retenciones";
				if(dtRtns==null)
					this.Preparar_dtRtns();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=dtRtns.NewRow();
					fila[0]=ds.Tables[0].Rows[i][0].ToString();
					fila[1]=Convert.ToDouble(ds.Tables[0].Rows[i][1]);
					dtRtns.Rows.Add(fila);
					dgRtns.DataSource=dtRtns;
					dgRtns.DataBind();
				}
			}
		}
		
		protected void Llenar_Formato()
		{
			this.lbBeneficiario.Text="Beneficiario : "+DBFunctions.SingleData("SELECT MNIT.mnit_apellidos CONCAT ' ' CONCAT MNIT.mnit_nombres FROM dbxschema.mnit MNIT,dbxschema.mcaja MCAJ WHERE MCAJ.mnit_nitben=MNIT.mnit_nit AND MCAJ.pdoc_codigo='"+Request.QueryString["pref"]+"' AND MCAJ.mcaj_numero="+Request.QueryString["num"]+"");
			this.lbConcepto.Text="Concepto : "+DBFunctions.SingleData("SELECT mcaj_razon FROM dbxschema.mcaja WHERE pdoc_codigo='"+Request.QueryString["pref"]+"' AND mcaj_numero="+Request.QueryString["num"]+"");
            this.lbDireccion.Text = "Direccion : " + DBFunctions.SingleData("SELECT PALM.palm_direccion FROM dbxschema.palmacen PALM,dbxschema.mcaja MCAJ WHERE tvig_vigencia='V' and PALM.palm_almacen=MCAJ.palm_almacen AND MCAJ.pdoc_codigo='" + Request.QueryString["pref"] + "' AND MCAJ.mcaj_numero=" + Request.QueryString["num"] + "");
			this.lbEmpresa.Text=DBFunctions.SingleData("SELECT cemp_nombre FROM dbxschema.cempresa");
			this.lbFecha.Text="Fecha : "+Convert.ToDateTime(DBFunctions.SingleData("SELECT mcaj_fecha FROM dbxschema.mcaja WHERE pdoc_codigo='"+Request.QueryString["pref"]+"' AND mcaj_numero="+Request.QueryString["num"]+"")).ToString("yyyy-MM-dd");
			this.lbNit.Text="Nit : "+DBFunctions.SingleData("SELECT mnit_nit FROM dbxschema.cempresa");
			this.lbNitBen.Text="Nit o CC : "+DBFunctions.SingleData("SELECT mnit_nitben FROM dbxschema.mcaja WHERE pdoc_codigo='"+Request.QueryString["pref"]+"' AND mcaj_numero="+Request.QueryString["num"]+"");
			this.lbNumero.Text="# "+DBFunctions.SingleData("SELECT mcaj_numero FROM dbxschema.mcaja WHERE pdoc_codigo='"+Request.QueryString["pref"]+"' AND mcaj_numero="+Request.QueryString["num"]+"");
			this.lbRecibo.Text=DBFunctions.SingleData("SELECT PDOC.pdoc_descripcion CONCAT ' - ' CONCAT PDOC.pdoc_codigo FROM dbxschema.pdocumento PDOC,dbxschema.mcaja MCAJ WHERE MCAJ.pdoc_codigo=PDOC.pdoc_codigo AND MCAJ.pdoc_codigo='"+Request.QueryString["pref"]+"' AND MCAJ.mcaj_numero="+Request.QueryString["num"]+"");
            this.lbSede.Text = "Sede : " + DBFunctions.SingleData("SELECT PALM.palm_descripcion FROM dbxschema.palmacen PALM, dbxschema.mcaja MCAJ WHERE tvig_vigencia='V' and MCAJ.palm_almacen=PALM.palm_almacen AND MCAJ.pdoc_codigo='" + Request.QueryString["pref"] + "' AND MCAJ.mcaj_numero=" + Request.QueryString["num"] + "");
			this.lbTipo.Text="Tipo de Recibo : "+DBFunctions.SingleData("SELECT tcla_nombre FROM dbxschema.tclaserecaja WHERE tcla_claserecaja='"+Request.QueryString["tipo"]+"'");
			this.Llenar_dgDocs();
			this.Llenar_dgNoCaus();
			this.Llenar_dgAbonos();
			this.Llenar_dgPagos();
			this.Llenar_dgRtns();
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
