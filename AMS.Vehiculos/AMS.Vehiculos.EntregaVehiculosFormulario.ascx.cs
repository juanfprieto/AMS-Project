// created on 15/02/2005 at 16:00
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Timers;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Vehiculos
{
	public partial class EntregaVehiculosFormulario : System.Web.UI.UserControl
	{
		protected DataTable tablaFacturas, tablaRetoma, tablaAccesorios;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.Llenar_Tabla_Accesorios(Request.QueryString["tipPro"]);
			if(!IsPostBack)
			{
				this.Llenar_Datos_Generales(Request.QueryString["prefPedi"],Request.QueryString["numePedi"],Request.QueryString["numeDocu"]);
				this.Llenar_Tabla_Facturas(Request.QueryString["prefPedi"],Request.QueryString["numePedi"]);
				this.Llenar_Datos_Cliente(Request.QueryString["prefPedi"],Request.QueryString["numePedi"]);
				this.Llenar_Datos_Vehiculo(Request.QueryString["prefPedi"],Request.QueryString["numePedi"]);				
				if(Request.QueryString["tipPro"]=="G")
				{
					toolsHolder.Visible = true;
					btnGuardar.Visible = false;
					this.Generar_Formato_Impresion();
				}
			}
		}
		
		protected void Llenar_Datos_Generales(string prefijoPedido, string numeroPedido, string numeroDocumento)
		{
			numEntrega.Text = numeroDocumento;
			responsable.Text = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+")");
			fechaHoraentrega.Text = DateTime.Now.ToString();
		}
		
		protected void Preparar_Tabla_Facturas()
		{
			tablaFacturas = new DataTable();
			tablaFacturas.Columns.Add(new DataColumn("PREFIJOFACTURA",System.Type.GetType("System.String")));
			tablaFacturas.Columns.Add(new DataColumn("NUMEROFACTURA",System.Type.GetType("System.String")));
			tablaFacturas.Columns.Add(new DataColumn("VALORTOTAL",System.Type.GetType("System.Double")));
			tablaFacturas.Columns.Add(new DataColumn("VALORABONADO",System.Type.GetType("System.Double")));
			tablaFacturas.Columns.Add(new DataColumn("VALORSALDO",System.Type.GetType("System.Double")));
			tablaFacturas.Columns.Add(new DataColumn("NITCLIENTE",System.Type.GetType("System.String")));
			tablaFacturas.Columns.Add(new DataColumn("NOMBRECLIENTE",System.Type.GetType("System.String")));
		}		
		
		protected void Llenar_Tabla_Facturas(string prefijoPedido, string numeroPedido)
		{
			double totalSaldos = 0;
			double totalRetomas = 0;
			this.Preparar_Tabla_Facturas();
			this.Preparar_Tabla_Retomas();
			DataSet nitsPedido = new DataSet();
			DBFunctions.Request(nitsPedido,IncludeSchema.NO,"SELECT mnit_nit, mnit_nit2 FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			DataSet facturasAsociadas = new DataSet();			
			//DBFunctions.Request(facturasAsociadas,IncludeSchema.NO,"SELECT MFC.pdoc_codigo,MFC.mfac_numedocu,MFC.mfac_valofact,MFC.mfac_valoabon, MFC.mfac_valofact-MFC.mfac_valoabon FROM mfacturapedidovehiculo MFP, mfacturacliente MFC, pdocumento PDC WHERE MFP.pdoc_codigo=MFC.pdoc_codigo AND MFP.mfac_numedocu=MFC.mfac_numedocu AND MFC.tvig_vigencia='V' AND MFC.pdoc_codigo=PDC.pdoc_codigo AND PDC.tdoc_tipodocu='FC' AND MFP.mped_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			//DBFunctions.Request(facturasAsociadas,IncludeSchema.NO,"SELECT DISTINCT pdoc_codigo, mfac_numedocu, mfac_valofact + mfac_valoiva + mfac_valoflet + mfac_valoivaflet - mfac_valorete, mfac_valoabon FROM mfacturacliente WHERE (mnit_nit='"+nitsPedido.Tables[0].Rows[0][0].ToString()+"' OR mnit_nit='"+nitsPedido.Tables[0].Rows[0][1].ToString()+"') AND ((mfac_valofact + mfac_valoiva + mfac_valoflet + mfac_valoivaflet - mfac_valorete)-mfac_valoabon)>0");
			//																			0				1											2												3
			DBFunctions.Request(facturasAsociadas,IncludeSchema.NO,"SELECT MFAC.pdoc_codigo,MFAC.mfac_numedocu,MFAC.mfac_valofact + MFAC.mfac_valoiva + MFAC.mfac_valoflet + MFAC.mfac_valoivaflet - MFAC.mfac_valorete, MFAC.mfac_valoabon FROM dbxschema.mfacturacliente MFAC,dbxschema.mfacturapedidovehiculo MFP WHERE MFAC.pdoc_codigo=MFP.pdoc_codigo AND MFAC.mfac_numedocu=MFP.mfac_numedocu AND MFP.mped_codigo='"+Request.QueryString["prefPedi"]+"' AND MFP.mped_numepedi="+Request.QueryString["numePedi"]+" AND MFAC.tvig_vigencia NOT IN('C') AND (MFAC.mfac_valofact + MFAC.mfac_valoiva + MFAC.mfac_valoflet + MFAC.mfac_valoivaflet - MFAC.mfac_valorete <> MFAC.mfac_valoabon)");
			//																	0					1													2																		3				
			for(int i=0;i<facturasAsociadas.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaFacturas.NewRow();
				fila["PREFIJOFACTURA"] = facturasAsociadas.Tables[0].Rows[i][0].ToString();
				fila["NUMEROFACTURA"] = facturasAsociadas.Tables[0].Rows[i][1].ToString();
				fila["VALORTOTAL"] = Convert.ToDouble(facturasAsociadas.Tables[0].Rows[i][2]);
				fila["VALORABONADO"] = Convert.ToDouble(facturasAsociadas.Tables[0].Rows[i][3]);
				fila["VALORSALDO"] = Convert.ToDouble(facturasAsociadas.Tables[0].Rows[i][2]) - Convert.ToDouble(facturasAsociadas.Tables[0].Rows[i][3]);
				fila["NITCLIENTE"] = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadas.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadas.Tables[0].Rows[i][1].ToString()+"");
				fila["NOMBRECLIENTE"] = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' 'CONCAT mnit_nombres FROM mnit WHERE mnit_nit=(SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadas.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadas.Tables[0].Rows[i][1].ToString()+")");
				totalSaldos += Convert.ToDouble(facturasAsociadas.Tables[0].Rows[i][2]) - Convert.ToDouble(facturasAsociadas.Tables[0].Rows[i][3]);
				tablaFacturas.Rows.Add(fila);
				totalRetomas += Llenar_Tabla_Retomas(facturasAsociadas.Tables[0].Rows[i][0].ToString(),facturasAsociadas.Tables[0].Rows[i][1].ToString());
			}
			grillaFacturas.DataSource = tablaFacturas;
			grillaFacturas.DataBind();
			grillaRetoma.DataSource = tablaRetoma;
			grillaRetoma.DataBind();
			saldos.Text = totalSaldos.ToString("C");
			retomas.Text = totalRetomas.ToString("C");
			saldoFinal.Text = (totalSaldos-totalRetomas).ToString("C");
		}
		
		protected void Preparar_Tabla_Retomas()
		{
			tablaRetoma = new DataTable();
			tablaRetoma.Columns.Add(new DataColumn("NUMEROCONTRATORETOMA",System.Type.GetType("System.String")));
			tablaRetoma.Columns.Add(new DataColumn("TIPOVEHICULO",System.Type.GetType("System.String")));
			tablaRetoma.Columns.Add(new DataColumn("PLACAVEHICULO",System.Type.GetType("System.String")));
			tablaRetoma.Columns.Add(new DataColumn("ANOMODELO",System.Type.GetType("System.String")));
			tablaRetoma.Columns.Add(new DataColumn("CUENTAIMPUESTOS",System.Type.GetType("System.String")));
			tablaRetoma.Columns.Add(new DataColumn("VALORRETOMA",System.Type.GetType("System.Double")));
			tablaRetoma.Columns.Add(new DataColumn("ESTADORETOMA",System.Type.GetType("System.String")));
		}
		
		protected double Llenar_Tabla_Retomas(string prefijoFactura, string numeroFactura)
		{
			double valorRetomasF = 0;
			DataSet retomasAsociadas = new DataSet();
			DBFunctions.Request(retomasAsociadas,IncludeSchema.NO,"SELECT MRT.mret_numecont, DPV.pcat_codigo, MRT.mret_placa, MRT.pano_ano, MRT.mret_cuenimpu, DPV.dped_valounit, MRT.test_estado FROM mretomavehiculo MRT, dpedidovehiculoproveedor DPV WHERE MRT.mped_codigo=DPV.pdoc_codigo AND MRT.pcat_codigo=DPV.pcat_codigo AND MRT.mped_numepedi=DPV.mped_numepedi AND MRT.pdoc_codigo='"+prefijoFactura+"' AND MRT.mfac_numedocu="+numeroFactura+"");
			for(int i=0;i<retomasAsociadas.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaRetoma.NewRow();
				fila["NUMEROCONTRATORETOMA"] = retomasAsociadas.Tables[0].Rows[i][0].ToString();
				fila["TIPOVEHICULO"] = retomasAsociadas.Tables[0].Rows[i][1].ToString();
				fila["PLACAVEHICULO"] = retomasAsociadas.Tables[0].Rows[i][2].ToString();
				fila["ANOMODELO"] = retomasAsociadas.Tables[0].Rows[i][3].ToString();
				fila["CUENTAIMPUESTOS"] = retomasAsociadas.Tables[0].Rows[i][4].ToString();
				fila["VALORRETOMA"] = System.Convert.ToDouble(retomasAsociadas.Tables[0].Rows[i][5]);
				fila["ESTADORETOMA"] = DBFunctions.SingleData("SELECT test_descripcion FROM testadoretoma WHERE test_codigo='"+retomasAsociadas.Tables[0].Rows[i][6].ToString()+"'");
				if(retomasAsociadas.Tables[0].Rows[i][6].ToString()=="R")
					valorRetomasF += System.Convert.ToDouble(retomasAsociadas.Tables[0].Rows[i][5]);
				tablaRetoma.Rows.Add(fila);
			}
			return valorRetomasF;
		}
		
		protected void Preparar_Tabla_Accesorios()
		{
			tablaAccesorios = new DataTable();
			tablaAccesorios.Columns.Add(new DataColumn("ACCESORIO",System.Type.GetType("System.String")));
			tablaAccesorios.Columns.Add(new DataColumn("ESTADO",System.Type.GetType("System.String")));
		}
		
		protected void Llenar_Tabla_Accesorios(string tipoOperacion)
		{
			int i;
			this.Preparar_Tabla_Accesorios();
			DataSet accesorios = new DataSet();
			DBFunctions.Request(accesorios,IncludeSchema.NO,"SELECT pacc_descripcion FROM paccesorio");
			for(i=0;i<accesorios.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaAccesorios.NewRow();
				fila["ACCESORIO"] = accesorios.Tables[0].Rows[i][0].ToString();
				tablaAccesorios.Rows.Add(fila);
			}
			grillaAccesorios.DataSource = tablaAccesorios;
			grillaAccesorios.DataBind();
			for(i=0;i<grillaAccesorios.Items.Count;i++)
			{
				if(tipoOperacion=="I")
				{
					TextBox tx = new TextBox();
					tx.Width = new Unit(350);
					grillaAccesorios.Items[i].Cells[1].Controls.Add(tx);
				}
				else if(tipoOperacion=="G")
				{
					Label lbt = new Label();
					lbt.Width = new Unit(350);
					grillaAccesorios.Items[i].Cells[1].Controls.Add(lbt);
				}
			}
		}
		
		protected void Llenar_Datos_Cliente(string prefijoPedido, string numeroPedido)
		{
			string nitCliente = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			nombreCliente.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			idCliente.Text = nitCliente;
			direccionCliente.Text = DBFunctions.SingleData("SELECT mnit_direccion FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			ciudadCliente.Text = DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM mnit WHERE mnit_nit='"+nitCliente+"')");
			telefonoCliente.Text = DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			movilCliente.Text = DBFunctions.SingleData("SELECT mnit_celular FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			emailCliente.Text = DBFunctions.SingleData("SELECT mnit_email FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			websiteCliente.Text = DBFunctions.SingleData("SELECT mnit_web FROM mnit WHERE mnit_nit='"+nitCliente+"'");
		}
		
		protected void Llenar_Datos_Vehiculo(string prefijoPedido, string numeroPedido)
		{
			string numeroInventario = DBFunctions.SingleData("SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			numInvent.Text = numeroInventario;
			catalogoVeh.Text = DBFunctions.SingleData("SELECT mc.pcat_codigo FROM mvehiculo mv, mcatalogovehiculo mc WHERE mv.mcat_vin = mc.mcat_vin and mveh_inventario="+numeroInventario+"");
			vinVeh.Text = DBFunctions.SingleData("SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario="+numeroInventario+"");
			numMotor.Text = DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario="+numeroInventario+")");
			numSerie.Text = DBFunctions.SingleData("SELECT mcat_serie FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario="+numeroInventario+")");
			anoModelo.Text = DBFunctions.SingleData("SELECT mcat_anomode FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario="+numeroInventario+")");
			tipoServicio.Text = DBFunctions.SingleData("SELECT tser_nombserv FROM tserviciovehiculo WHERE tser_tiposerv=(SELECT tser_tiposerv FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario="+numeroInventario+"))");
			colorVeh.Text = DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo=(SELECT pcol_codigo FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario="+numeroInventario+"))");
			kilomVeh.Text = DBFunctions.SingleData("SELECT mcat_numeultikilo FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario="+numeroInventario+")");
		}
		
		protected void Generar_Formato_Impresion()
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			controlsFormulario.RenderControl(htmlTW);
			Session["Rep"] = SB.ToString();
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
