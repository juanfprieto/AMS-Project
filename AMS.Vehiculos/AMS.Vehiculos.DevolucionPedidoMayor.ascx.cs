namespace AMS.Vehiculos
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Documentos;
	using AMS.Utilidades;
	using Ajax;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Vehiculos_DevolucionPedidoMayor.
	/// </summary>
	public partial class AMS_Vehiculos_DevolucionPedidoMayor : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.TextBox txtCatalogo;
		protected System.Web.UI.WebControls.Button btnAgregarCatalogo;
		protected System.Web.UI.WebControls.TextBox txtVINM;
		protected System.Web.UI.WebControls.TextBox txtMotorM;
		protected System.Web.UI.WebControls.TextBox txtDiasM;
		protected System.Web.UI.WebControls.Button btnMarcar;
		protected System.Web.UI.WebControls.TextBox txtPrefGuia;
		protected System.Web.UI.WebControls.TextBox txtNumGuia;
		#endregion Controles
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.Page.SmartNavigation=true;
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Vehiculos_DevolucionPedidoMayor));	
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				txtFechaFac.Text=DateTime.Now.ToString("yyyy-MM-dd");
				MostrarPanel(plcInfoFact);
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen, palm_descripcion FROM dbxschema.palmacen where (pcen_centvvn is not null  or pcen_centvvu is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
				bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT pd.pdoc_codigo, pd.pdoc_codigo concat ' - ' concat pd.pdoc_nombre FROM dbxschema.pdocumento pd, pdocumentohecho pdh WHERE pd.tdoc_tipodocu='NC' AND pd.pdoc_codigo=pdh.pdoc_codigo AND pdh.palm_almacen='"+ddlAlmacen.SelectedValue+"' AND pdh.tpro_proceso='VN';");
				//bind.PutDatasIntoDropDownList(ddlFacturaD,"SELECT pd.pdoc_codigo, pd.pdoc_codigo concat ' - ' concat pd.pdoc_nombre FROM dbxschema.pdocumento pd, pdocumentohecho pdh WHERE pd.tdoc_tipodocu='FC' AND pd.pdoc_codigo=pdh.pdoc_codigo AND pdh.tpro_proceso='VN';");
                Utils.llenarPrefijos(Response, ref ddlFacturaD, "VN", "%", "FC");
				bind.PutDatasIntoDropDownList(ddlIVAFletes,"SELECT PIVA_PORCIVA,PIVA_DECRETO FROM PIVA ORDER BY PIVA_PORCIVA;");
				CambioTipoFacturacion(sender,e);
				//Impresion
				if(Request.QueryString["prefFC"]!=null)
				{
                    Utils.MostrarAlerta(Response, "Se ha creado la devolucion de cliente por venta mayor con prefijo " + Request.QueryString["prefFC"] + " y el número " + Request.QueryString["numFC"] + ".");
                    FormatosDocumentos formatoRecibo=new FormatosDocumentos();
					try
					{
						formatoRecibo.Prefijo=Request.QueryString["prefFC"];
						formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["numFC"]);
						formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefFC"]+"'");
						if(formatoRecibo.Codigo!=string.Empty)
							if(formatoRecibo.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=600');</script>");
					}
					catch
					{
						lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
					}
				}
			}
		}

		#region AJAX
		[Ajax.AjaxMethod]
		public DataSet TraerCliente(string nitCliente)
		{
			return(TraerClienteDs(nitCliente));
		}
		public DataSet TraerClienteDs(string nitCliente)
		{
			DataSet Vins = new DataSet();
            double cupoCli = Clientes.ConsultarSaldo(nitCliente);
            txtCupo.Text = cupoCli.ToString();
			double saldoCartera  = Clientes.ConsultarSaldo(nitCliente);
            txtSaldoCartera.Text = saldoCartera.ToString();
			double saldoCarteraM = Clientes.ConsultarSaldoMora(nitCliente);
            txtSaldoCarteraMora.Text = saldoCarteraM.ToString();
			DBFunctions.Request(Vins,IncludeSchema.NO,"select mn.mnit_direccion AS DIRECCION,pc.pciu_nombre AS CIUDAD,mn.mnit_telefono AS TELEFONO,'"+saldoCartera.ToString("#,##0")+"' AS SALDO,'"+saldoCarteraM.ToString("#,##0")+"' AS SALDOMORA, MCLI_CUPOCRED AS CUPO from mnit mn, mcliente mc, pciudad pc where mn.pciu_codigo=pc.pciu_codigo and mc.mnit_nit=mn.mnit_nit and mn.mnit_nit='"+nitCliente+"';");
			return Vins;
		}
		#endregion AJAX

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
		#region Manejo Manejo Placeholders
		private void MostrarPanel(PlaceHolder plAct)
		{
			plcInfoCliente.Visible=plcInfoFact.Visible=plcVehiculos.Visible=plcFacturaDev.Visible=false;
			plAct.Visible=true;
		}
		protected void MostrarInfoCliente(Object  Sender, EventArgs e)
		{
			if(plcInfoFact.Visible==true)
			{
				if(DBFunctions.RecordExist("select * from mfacturacliente where pdoc_codigo='"+prefijoFactura.SelectedValue+"' and mfac_numedocu="+numeroFactura.Text))
				{
                    Utils.MostrarAlerta(Response, "Ya se utilizó el número de factura");
					return;
				}
				try
				{
					if(prefijoFactura.SelectedValue.Length==0 || numeroFactura.Text.Length==0 || txtCodVendedor.Text.Length==0)
						throw(new Exception());
					int numFactura=Convert.ToInt16(numeroFactura.Text);
					ViewState["NUM_FACTURA"] =numFactura;
					ViewState["PREF_FACTURA"]=prefijoFactura.SelectedValue;
					ViewState["COD_VENDEDOR"]=txtCodVendedor.Text;
					ViewState["NOM_VENDEDOR"]=txtCodVendedora.Text;
					Convert.ToDateTime(txtFechaFac.Text);
				}
				catch
				{
                    Utils.MostrarAlerta(Response, "Debe dar datos correctos de la factura y el vendedor");
					return;
				}
			}
			MostrarPanel(plcInfoCliente);
		}
		protected void MostrarInfoFactura(Object  Sender, EventArgs e)
		{
			MostrarPanel(plcInfoFact);
		}
		protected void MostrarInfoFacturaD(Object  Sender, EventArgs e)
		{
			ViewState["NIT_CLIENTE"]=nitCliente.Text;
			ViewState["NOM_CLIENTE"]=nitClientea.Text;
			CambioTipoFacturacionD(Sender,e);
			MostrarPanel(plcFacturaDev);
		}
		protected void MostrarInfoVehiculos(Object  Sender, EventArgs e)
		{
			if(ddlNumFacturaD.SelectedValue.Length==0)
			{
                Utils.MostrarAlerta(Response, "No seleccionó ningúna factura a devolver");
				return;
			}
			CargarVehiculos();
			MostrarPanel(plcVehiculos);
		}
		#endregion Placeholders
		#region Consultas Vehiculos, Pedidos
		//Traer Vehiculos disponibles para los pedidos
		protected bool CargarVehiculos()
		{
			DataSet dsDevoluciones=new DataSet();
			string sqlV="SELECT "+
			"mfp.pdoc_codigo pdoc_codigo,mfp.mfac_numedocu,mfp.pdoc_codigo concat '-' concat rtrim(char(mfp.mfac_numedocu)) as MVEH_FACTURA,"+
			"mfp.MPED_CODIGO concat '-' concat rtrim(char(mfp.mped_numepedi)) as MVEH_PEDIDO,MC.mcat_anomode,"+
			"mfp.MPED_CODIGO,mfp.mped_numepedi,"+
			"MV.MCAT_VIN, MC.MCAT_MOTOR, MC.PCAT_CODIGO, PC.PCOL_CODRGB PCOL_CODRGBVEH, PC.PCOL_DESCRIPCION PCOL_NOMBRE, PC.PCOL_CODIGO, "+
			"MC.mnit_nit,days(CURRENT DATE)-days(MVEH_FECHRECE) AS MVEHI_DIAS, 0 AS USADO, "+
			"mfp.mfac_valounitario as precio, mfp.mfac_valodesc as descuento, mfp.mfac_valoiva as iva, mfp.mfac_valounitario-mfp.mfac_valodesc+mfp.mfac_valoiva as total, "+
			"pdoc_guia,mfac_numeguia "+
			"from DBXSCHEMA.DFACTURAPEDIDOMAYORVEHICULO mfp,DBXSCHEMA.mvehiculo MV, DBXSCHEMA.MCATALOGOVEHICULO MC, DBXSCHEMA.PCOLOR PC "+
			"WHERE MFP.MCAT_VIN=MV.MCAT_VIN and MC.MCAT_VIN=MV.MCAT_VIN AND PC.PCOL_CODIGO=MC.PCOL_CODIGO and mfp.mfac_numedocudev is null and MC.mnit_nit='"+nitCliente.Text+"' AND "+
			"mfp.pdoc_codigo='"+ddlFacturaD.SelectedValue+"' and mfp.mfac_numedocu="+ddlNumFacturaD.SelectedValue+" "+
			"order by MC.PCAT_CODIGO,MV.MCAT_VIN, MVEHI_DIAS DESC;";
			DBFunctions.Request(dsDevoluciones,IncludeSchema.NO,sqlV);
			dgrVehiculos.DataSource=dsDevoluciones.Tables[0];
			dgrVehiculos.DataBind();
			Session["TABLA_MDEVOLUCIONES"]=dsDevoluciones.Tables[0];
			return true;
		}
		#endregion
		
		#region Facturacion
		//Facturar
		protected void Facturar(Object  Sender, EventArgs e)
		{
			//Almacena sentencias de faturacion
			ArrayList sqlRefs=null;
			//string vinsVehiculos="";
			string codVendedor=txtCodVendedor.Text.Trim();
			string codAlmacen = ddlAlmacen.SelectedValue;
			string codCentroCosto;
			string prefijoFac = prefijoFactura.SelectedValue;
			uint numeroFac = Convert.ToUInt16(numeroFactura.Text);
			string cliente=nitCliente.Text;
			DateTime fechaFactura;
			double totalVehiculos=0,totalDescuentos=0,totalIVA=0,valRetenciones=0,ivaFletes=0,valIVAFletes=0,valFletes=0,totalFactura=0,saldoCartera,cupo;
			DataTable dtVehiculos=(DataTable)Session["TABLA_MDEVOLUCIONES"];
			DataRow[] drVehiculo=dtVehiculos.Select("USADO=1");
			int idUsuario=Convert.ToInt16(DBFunctions.SingleData("select susu_codigo from susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'"));
			
			//Verificar mfactura no se ha devuelto
			if(!DBFunctions.RecordExist("SELECT MFAC_NUMEDOCU FROM DBXSCHEMA.DFACTURAPEDIDOMAYORVEHICULO WHERE PDOC_CODIGO='"+ddlFacturaD.SelectedValue+"' AND MFAC_NUMEDOCU="+ddlNumFacturaD.SelectedValue+" AND MFAC_NUMEDOCUDEV IS NULL;")){
                Utils.MostrarAlerta(Response, "La factura no esta disponible para devoluciones.");
				return;
			}
			//Verificar vehiculos seleccionados
			if(drVehiculo.Length==0)
			{
                Utils.MostrarAlerta(Response, "No seleccionó ningún vehículo");
				return;
			}

			//Validar fecha
			try
			{
				fechaFactura=Convert.ToDateTime(txtFechaFac.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "Fecha no válida");
				return;}

			//Valor Fletes y retenciones
			try
			{
				if(txtRetenciones.Text.Length>0) valRetenciones=Convert.ToDouble(txtRetenciones.Text);
				if(txtFletes.Text.Length>0) valFletes=Convert.ToDouble(txtFletes.Text);
				ivaFletes=Convert.ToDouble(ddlIVAFletes.SelectedValue);
				if(ivaFletes<0||valFletes<0||valRetenciones<0)
					throw(new Exception());
				valIVAFletes=((valFletes*ivaFletes)/100);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Revise el valor de retenciones y fletes");
				return;
			}

			//Saldo, cupo
			try
			{
				saldoCartera=Convert.ToDouble(txtSaldoCartera.Text.Replace(",",""));
				cupo=Convert.ToDouble(txtCupo.Text.Replace(",",""));}
			catch
			{
                saldoCartera = 0;
                cupo = 0;
                Utils.MostrarAlerta(Response, "Revise el saldo y cupo del cliente");
			//	return;
            }

			//Centro de costo
            codCentroCosto = DBFunctions.SingleData("SELECT pcen_centvvn FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codAlmacen + "'");
			
			//Traer los dias de plazo fecha de vencimiento factura
			string mfac_vence=DBFunctions.SingleData("select mcli_diasplaz from dbxschema.mcliente where mnit_nit='"+cliente+"'");
			if (mfac_vence!=String.Empty)
				mfac_vence=fechaFactura.AddDays(Convert.ToDouble(mfac_vence)).ToString("yyyy-MM-dd");
			else
				mfac_vence=fechaFactura.ToString("yyyy-MM-dd");

			//Generar sentencias relacionadas al proceso de devolucion
			Pedidos.DevolucionVehiculosMayorCliente(ref sqlRefs,ddlFacturaD.SelectedValue,ddlNumFacturaD.SelectedValue,prefijoFactura.SelectedValue,numeroFactura.Text,dtVehiculos,nitCliente.Text,ref totalVehiculos,ref totalDescuentos, ref totalIVA, ref totalFactura,fechaFactura,idUsuario);

			totalFactura+=valRetenciones+valFletes+valIVAFletes;

			//Si valor a FACTURAR mas el SALDO EN CARTERA es mayor al cupo y el cupo>0 mostrar advertencia
			if(cupo>0 && cupo-(totalFactura+saldoCartera)<0 && lblAdvertencia.Text.Length==0)
			{
				lblAdvertencia.Text="Advertencia: el cupo del cliente es menor al saldo en cartera mas el valor de la factura. Para continuar de todas formas vuelva a dar click en facturar.";
			}

			//VERIFICAR numero de factura esta disponible, cambiarlo si no es manual
			if(DBFunctions.RecordExist("select * from mfacturacliente where pdoc_codigo='"+prefijoFac+"' and mfac_numedocu="+numeroFac))
			{
				if(tipoFacturacion.SelectedValue.ToString()=="A")
					numeroFac = Convert.ToUInt16(DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFac+"'"));
				else
				{
                    Utils.MostrarAlerta(Response, "Ya se utilizó el número de factura");
					return;
				}
			}

			//Revisamos si es necesario crear la factura de financiera, se crea como una factura de cliente al nit de la financiera y se agrega esta factura como un pago a la factura de nuestro cliente
			FacturaCliente facturaVentaVehiculo = new FacturaCliente("FVV",prefijoFac, cliente, codAlmacen , "N" ,numeroFac,
				0, fechaFactura, Convert.ToDateTime(mfac_vence), DateTime.Now, totalVehiculos-totalDescuentos, totalIVA, 
				valFletes, valIVAFletes, valRetenciones, 0, codCentroCosto, observaciones.Text, codVendedor, HttpContext.Current.User.Identity.Name,null);

			facturaVentaVehiculo.SqlRels = sqlRefs;

			//Como no es necesaria realizar la retoma entonces grabamos la factura directamente
			if(facturaVentaVehiculo.GrabarFacturaCliente(true))
			{
				Session.Remove("TABLA_MDEVOLUCIONES");
				Response.Redirect("" + indexPage + "?process=Vehiculos.DevolucionPedidoMayor&prefFC="+facturaVentaVehiculo.PrefijoFactura+"&numFC="+facturaVentaVehiculo.NumeroFactura+"&path="+Request.QueryString["path"]);
			}
			else
				lb.Text += "Error: "+facturaVentaVehiculo.ProcessMsg;

		}
		//Cambia el tipo de facturacion
		protected void CambioTipoFacturacion(Object  Sender, EventArgs e)
		{
			if(tipoFacturacion.SelectedValue.ToString()=="A")
			{
				numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura.SelectedValue.ToString()+"'");
				numeroFactura.ReadOnly = true;
			}
			else if(tipoFacturacion.SelectedValue.ToString()=="M")
				numeroFactura.ReadOnly = false;
		}
		protected void CambioTipoFacturacionD(Object  Sender, EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			string sqlF="Select distinct mfp.mfac_numedocu "+
			"from DBXSCHEMA.DFACTURAPEDIDOMAYORVEHICULO mfp,DBXSCHEMA.mvehiculo MV, DBXSCHEMA.MCATALOGOVEHICULO MC "+
			"WHERE MFP.MCAT_VIN=MV.MCAT_VIN and MC.MCAT_VIN=MV.MCAT_VIN AND mfp.mfac_numedocudev is null "+
			"and MFP.PDOC_CODIGO='"+ddlFacturaD.SelectedValue+"' and MC.mnit_nit='"+nitCliente.Text+"' "+
			"order by mfac_numedocu";
			bind.PutDatasIntoDropDownList(ddlNumFacturaD,sqlF);
		}
		//Cambia el almacen
		protected void CambioAlmacen(Object  Sender, EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT pd.pdoc_codigo, pd.pdoc_codigo concat ' - ' concat pd.pdoc_nombre FROM dbxschema.pdocumento pd, pdocumentohecho pdh WHERE pd.tdoc_tipodocu='NC' AND pd.pdoc_codigo=pdh.pdoc_codigo AND pdh.palm_almacen='"+ddlAlmacen.SelectedValue+"' AND pdh.tpro_proceso='VN';");
			CambioTipoFacturacion(Sender,e);
		}
		#endregion

		#region Manejo Grilla de Vehiculos
		//Chequean un vehiculo
		protected void DgInserts_Check(object sender, EventArgs e)
		{
			int nF;
			CheckBox chkSel=(CheckBox)sender;
			nF=((DataGridItem)chkSel.Parent.Parent).ItemIndex;
			DataTable dtVehiculos=(DataTable)Session["TABLA_MDEVOLUCIONES"];
			if(chkSel.Checked)
				dtVehiculos.Rows[nF]["USADO"]=1;
			else
				dtVehiculos.Rows[nF]["USADO"]=0;
			Session["TABLA_MDEVOLUCIONES"]=dtVehiculos;
			Bind_dgInserts();
		}
		//Bind vehiculos
		protected void Bind_dgInserts()
		{
			DataTable dtVehiculos=(DataTable)Session["TABLA_MDEVOLUCIONES"];
			dgrVehiculos.DataSource = dtVehiculos;
			dgrVehiculos.DataBind();
			Totales();
		}
		//Calcular totales vehiculos
		private void Totales()
		{
			DataTable dtVehiculos=(DataTable)Session["TABLA_MDEVOLUCIONES"];
			double totalV=0,totalIVA=0,subtotal=0,totalD=0,totalFletes=0,totalRetenciones=0;
			//Verificar seleccionados
			for(int nV=0;nV<dtVehiculos.Rows.Count;nV++)
			{
				if(Convert.ToInt16(dtVehiculos.Rows[nV]["USADO"])==1)
				{
					totalV+=Convert.ToDouble(dtVehiculos.Rows[nV]["PRECIO"]);
					totalIVA+=Convert.ToDouble(dtVehiculos.Rows[nV]["IVA"]);
					totalD+=Convert.ToDouble(dtVehiculos.Rows[nV]["DESCUENTO"]);
				}
			}
			try{totalRetenciones=Convert.ToDouble(txtRetenciones.Text.Replace(",",""));}
			catch{}
			try
			{
				totalFletes=Convert.ToDouble(txtFletes.Text.Replace(",",""));
				totalFletes+=(Convert.ToDouble(ddlIVAFletes.SelectedValue)*totalFletes)/100;}
			catch{}
			subtotal=totalIVA+totalV-totalD;
			txtTotalIVA.Text=totalIVA.ToString("#,##0");
			txtTotalVehiculos.Text=totalV.ToString("#,##0");
			txtSubtotal.Text=subtotal.ToString("#,##0");
			txtTotalDescuentos.Text=totalD.ToString("#,##0");
			txtTotal.Text=(totalFletes+totalRetenciones+subtotal).ToString("#,##0");

		}
		#endregion Manejo de Vehiculos
		
	}
}
