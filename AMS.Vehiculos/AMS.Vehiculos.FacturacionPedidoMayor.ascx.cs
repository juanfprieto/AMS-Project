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
	///		Descripción breve de AMS_Vehiculos_FacturacionPedidoMayor.
	/// </summary>
	public partial class AMS_Vehiculos_FacturacionPedidoMayor : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.Label Label27;
		protected System.Web.UI.WebControls.Label Label29;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.TextBox txtIVAFletes;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		#region Eventos Principales Control
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Vehiculos_FacturacionPedidoMayor));	
			this.Page.SmartNavigation=true;
			if(!IsPostBack){
				DatasToControls bind=new DatasToControls();
				txtFechaFac.Text=DateTime.Now.ToString("yyyy-MM-dd");
				MostrarPanel(plcInfoFact);
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen, palm_descripcion FROM dbxschema.palmacen where (pcen_centvvn is not null or pcen_centvvu is not null)  and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
				bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT pd.pdoc_codigo, pd.pdoc_codigo concat ' - ' concat pd.pdoc_nombre FROM dbxschema.pdocumento pd, pdocumentohecho pdh WHERE pd.tdoc_tipodocu='FC' AND pd.pdoc_codigo=pdh.pdoc_codigo AND pdh.palm_almacen='"+ddlAlmacen.SelectedValue+"' AND pdh.tpro_proceso='VN';");
				bind.PutDatasIntoDropDownList(ddlIVAFletes,"SELECT PIVA_PORCIVA,PIVA_DECRETO FROM PIVA ORDER BY PIVA_PORCIVA;");
				CambioTipoFacturacion(sender,e);
				//Impresion
				if(Request.QueryString["prefFC"]!=null)
				{
                    Utils.MostrarAlerta(Response, "Se ha creado la factura de cliente por venta mayor con prefijo " + Request.QueryString["prefFC"] + " y el número " + Request.QueryString["numFC"] + ".");
					FormatosDocumentos formatoRecibo=new FormatosDocumentos();
					try
					{
						formatoRecibo.Prefijo=Request.QueryString["prefFC"];
						formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["numFC"]);
						formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefFC"]+"'");
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
		#endregion

		#region AJAX
		[Ajax.AjaxMethod]
		public DataSet TraerCliente(string nitCliente)
		{
			return(TraerClienteDs(nitCliente));
		}
		public DataSet TraerClienteDs(string nitCliente)
		{
			DataSet Vins= new DataSet();
			double saldoCartera=Clientes.ConsultarSaldo(nitCliente);
			double saldoCarteraM=Clientes.ConsultarSaldoMora(nitCliente);
			DBFunctions.Request(Vins,IncludeSchema.NO,"select mn.mnit_direccion AS DIRECCION,pc.pciu_nombre AS CIUDAD,mn.mnit_telefono AS TELEFONO,'"+saldoCartera.ToString("#,##0")+"' AS SALDO,'"+saldoCarteraM.ToString("#,##0")+"' AS SALDOMORA, MCLI_CUPOCRED AS CUPO from mnit mn, mcliente mc, pciudad pc where mn.pciu_codigo=pc.pciu_codigo and mc.mnit_nit=mn.mnit_nit and mn.mnit_nit='"+nitCliente+"';");
			return Vins;
		}
		#endregion AJAX

		#region Manejo Manejo Placeholders
		private void MostrarPanel(PlaceHolder plAct)
		{
			plcInfoCliente.Visible=plcInfoFact.Visible=plInfoPedidos.Visible=plcVehiculos.Visible=false;
			plAct.Visible=true;
		}
		protected void MostrarInfoCliente(Object  Sender, EventArgs e)
		{
			if(plcInfoFact.Visible==true){
				if(DBFunctions.RecordExist("select * from mfacturacliente where pdoc_codigo='"+prefijoFactura.SelectedValue+"' and mfac_numedocu="+numeroFactura.Text)){
                    Utils.MostrarAlerta(Response, "Ya se utilizó el número de factura");
					return;
				}
				DateTime fchAct;
				try{
					if(prefijoFactura.SelectedValue.Length==0 || numeroFactura.Text.Length==0 || txtCodVendedor.Text.Length==0)
						throw(new Exception());
					int numFactura=Convert.ToInt16(numeroFactura.Text);
					ViewState["NUM_FACTURA"]=numFactura;
					ViewState["PREF_FACTURA"]=prefijoFactura.SelectedValue;
					ViewState["COD_VENDEDOR"]=txtCodVendedor.Text;
					ViewState["NOM_VENDEDOR"]=txtCodVendedora.Text;
					fchAct=Convert.ToDateTime(txtFechaFac.Text);
				}
				catch{
                    Utils.MostrarAlerta(Response, "Debe dar datos correctos de la factura y el vendedor");
					return;
				}
				int ano=Convert.ToInt16(DBFunctions.SingleData("select PANO_ANO from CVEHICULOS;"));
				int mes=Convert.ToInt16(DBFunctions.SingleData("select PMES_MES from CVEHICULOS;"));
				if(ano!=fchAct.Year || mes!=fchAct.Month)
				{
                    Utils.MostrarAlerta(Response, "Fecha no vigente!");
					return;
				}
			}
			MostrarPanel(plcInfoCliente);
		}
		protected void MostrarInfoFactura(Object  Sender, EventArgs e)
		{
			MostrarPanel(plcInfoFact);
		}
		protected void MostrarInfoPedidos(Object  Sender, EventArgs e)
		{
			if(plcInfoCliente.Visible==true)
			{
				double saldoC=0,cupoC=0;
				if(nitCliente.Text.Length==0){
                    Utils.MostrarAlerta(Response, "Debe seleccionar el cliente.");
					return;
				}
				if(DBFunctions.RecordExist("select * from mnit where mnit_nit='"+nitCliente.Text+"' and (tvig_vigencia='B' or tvig_vigencia='C')")){
                    Utils.MostrarAlerta(Response, "El cliente no tiene un estado válido (debe estar bloqueado o cancelado).");
					return;
				}
				try{
					saldoC=Convert.ToDouble(txtSaldoCartera.Text.Replace(",",""));
					cupoC=Convert.ToDouble(txtCupo.Text.Replace(",",""));
					if(saldoC>cupoC && cupoC>0)throw (new Exception());
				}
				catch{
                    Utils.MostrarAlerta(Response, "Saldo no válido. El saldo del cliente debe ser menor a su cupo.");
					return;
				}
				if(!CargarPedidos(nitCliente.Text)){
                    Utils.MostrarAlerta(Response, "No se encontraron pedidos pendientes para el cliente.");
					return;
				}

				DatasToControls bind=new DatasToControls();
				DataSet dsIva=new DataSet();
				string nacionalidad=DBFunctions.SingleData("SELECT TNAC_TIPONACI FROM MNIT WHERE MNIT_NIT='"+nitCliente.Text+"';");
				ViewState["NIT_CLIENTE"]=nitCliente.Text;
				ViewState["NOM_CLIENTE"]=nitClientea.Text;
				ViewState["NAC_CLIENTE"]=nacionalidad;
				if(nacionalidad=="E")
					DBFunctions.Request(dsIva,IncludeSchema.NO,"SELECT PIVA_PORCIVA, PIVA_DECRETO FROM PIVA WHERE PIVA_PORCIVA=0 ORDER BY PIVA_PORCIVA;");
				else
					DBFunctions.Request(dsIva,IncludeSchema.NO,"SELECT PIVA_PORCIVA, PIVA_DECRETO FROM PIVA ORDER BY PIVA_PORCIVA;");
				ViewState["PIVA"]=dsIva.Tables[0];
			}
			MostrarPanel(plInfoPedidos);
		}
		protected void MostrarInfoVehiculos(Object  Sender, EventArgs e)
		{
			if(!CargarVehiculos())
			{
                Utils.MostrarAlerta(Response, "No seleccionó ningún pedido o no todos los pedidos seleccionados han sido autorizados.");
				return;
			}
			lblAdvertencia.Text="";
			MostrarPanel(plcVehiculos);
		}
		#endregion Placeholders
		
		#region Metodos especiales
		protected DataTable Preparar_Tabla_Pedido()
		{
			DataTable tablaPedidos = new DataTable();
			tablaPedidos.Columns.Add(new DataColumn("PDOC_CODIGO",System.Type.GetType("System.String")));
			tablaPedidos.Columns.Add(new DataColumn("MPED_NUMEPEDI",System.Type.GetType("System.Int32")));
			tablaPedidos.Columns.Add(new DataColumn("PCAT_CODIGO",System.Type.GetType("System.String")));
			tablaPedidos.Columns.Add(new DataColumn("PCOL_CODIGO",System.Type.GetType("System.String")));
			tablaPedidos.Columns.Add(new DataColumn("PCOL_CODRGB",System.Type.GetType("System.String")));
			tablaPedidos.Columns.Add(new DataColumn("PCOL_NOMBRE",System.Type.GetType("System.String")));
			tablaPedidos.Columns.Add(new DataColumn("PCOL_CODIGOALTE",System.Type.GetType("System.String")));
			tablaPedidos.Columns.Add(new DataColumn("PCOL_CODRGBALTE",System.Type.GetType("System.String")));
			tablaPedidos.Columns.Add(new DataColumn("PCOL_NOMBREALTE",System.Type.GetType("System.String")));
			tablaPedidos.Columns.Add(new DataColumn("DPED_CANTPEDI",System.Type.GetType("System.Int32")));
			tablaPedidos.Columns.Add(new DataColumn("DPED_VALOUNIT",System.Type.GetType("System.Double")));
			tablaPedidos.Columns.Add(new DataColumn("DPED_FECHALLEGADA",System.Type.GetType("System.String")));
			tablaPedidos.Columns.Add(new DataColumn("DPED_CANTFACT",System.Type.GetType("System.Int32")));
			tablaPedidos.Columns.Add(new DataColumn("DPED_VALOFACT",System.Type.GetType("System.Double")));
			tablaPedidos.Columns.Add(new DataColumn("DPED_CANTFALTA",System.Type.GetType("System.Int32")));
			return(tablaPedidos);
		}
		
		//Copiar vista de tabla de vehiculos
		public static DataTable CreateTable(DataView obDataView)
		{
			DataTable obNewDt = obDataView.Table.Clone();
			int idx = 0;
			string[] strColNames = new string[obNewDt.Columns.Count];
			foreach (DataColumn col in obNewDt.Columns)
			{
				strColNames[idx++] = col.ColumnName;
			}
			IEnumerator viewEnumerator = obDataView.GetEnumerator();
			while (viewEnumerator.MoveNext())
			{
				DataRowView drv = (DataRowView)viewEnumerator.Current;
				DataRow dr = obNewDt.NewRow();
				foreach (string strName in strColNames)
				{
					dr[strName] = drv[strName];
				}
				obNewDt.Rows.Add(dr);
			}
			return obNewDt;
		}
		#endregion

		#region Consultas Vehiculos, Pedidos
		//Traer Vehiculos disponibles para los pedidos
		protected bool CargarVehiculos()
		{
			//Tabla de detalles de pedido
			DataTable dtPedidosD=Preparar_Tabla_Pedido();
			//Tabla de pedidos activos del cliente
			DataTable dtPedidosM=(DataTable)Session["TABLA_MPEDIDOS"];
			//Tabla de vehiculos disponbles
			DataTable dtVehiculos;
			DataSet dsPedidosD,dsVehiculos=new DataSet();
			bool encontrado=false;
			int nPed=0;
			string sqlP,filtroCatalogos="";
			string noPed1="",prfPed1="";
			//Pedidos seleccionados
			ArrayList arlPedidos=new ArrayList();
			//Consultar detalles de los pedidos seleccionados
			foreach(DataGridItem dgiPed in dgPedidos.Items)
			{
				if(dgiPed.ItemType==ListItemType.Item || dgiPed.ItemType==ListItemType.AlternatingItem)
				{
					if(((CheckBox)dgiPed.FindControl("chkFactPed")).Checked)
					{
						//Verificar autorizacion
						if(!DBFunctions.SingleData(
							"SELECT MPED_AUTORIZA FROM MPEDIDOCLIENTEAUTORIZACION "+
							"WHERE PDOC_CODIGO='"+dtPedidosM.Rows[nPed]["PDOC_CODIGO"].ToString()+"' AND "+
							"MPED_NUMEPEDI="+dtPedidosM.Rows[nPed]["MPED_NUMEPEDI"].ToString()+";"
						).Equals("S")){
							return(false);
						}
						
						dtPedidosM.Rows[nPed]["USADO"]=1;
						//Traer Detalles Pedidos seleccionados
						sqlP="SELECT PDOC_CODIGO,MPED_NUMEPEDI,PCAT_CODIGO,"+
							"PCP.PCOL_DESCRIPCION AS PCOL_NOMBRE,PCA.PCOL_DESCRIPCION AS PCOL_NOMBREALTE,"+
							"PCP.PCOL_CODRGB AS PCOL_CODRGB,PCA.PCOL_CODRGB AS PCOL_CODRGBALTE,"+
							"PCP.PCOL_CODIGO AS PCOL_CODIGO, PCA.PCOL_CODIGO AS PCOL_CODIGOALTE,"+
							"DPED_CANTPEDI, DPED_CANTINGR, DPED_VALOUNIT, DPED_FECHALLEGADA, DPED_CANTFACT, "+
							"DPED_VALOFACT, DPED_CANTPEDI-DPED_CANTFACT AS DPED_CANTFALTA "+
							"FROM DPEDIDOVEHICULOCLIENTEMAYOR DP, PCOLOR PCP, PCOLOR PCA "+
							"WHERE PCP.PCOL_CODIGO=DP.PCOL_CODIGO AND PCA.PCOL_CODIGO=DP.PCOL_CODIGOALTE AND "+
							"PDOC_CODIGO='"+dtPedidosM.Rows[nPed]["PDOC_CODIGO"].ToString()+"' AND "+
							"MPED_NUMEPEDI="+dtPedidosM.Rows[nPed]["MPED_NUMEPEDI"].ToString();
						dsPedidosD=new DataSet();
						DBFunctions.Request(dsPedidosD,IncludeSchema.NO,sqlP);
						for(int nPedD=0;nPedD<dsPedidosD.Tables[0].Rows.Count;nPedD++){
							DataRow drPed=dtPedidosD.NewRow();
							filtroCatalogos+=" MC.PCAT_CODIGO='"+dsPedidosD.Tables[0].Rows[nPedD]["PCAT_CODIGO"]+"' OR";
							drPed["PDOC_CODIGO"]=dsPedidosD.Tables[0].Rows[nPedD]["PDOC_CODIGO"];
							drPed["MPED_NUMEPEDI"]=dsPedidosD.Tables[0].Rows[nPedD]["MPED_NUMEPEDI"];
							drPed["PCAT_CODIGO"]=dsPedidosD.Tables[0].Rows[nPedD]["PCAT_CODIGO"];
							drPed["PCOL_CODIGO"]=dsPedidosD.Tables[0].Rows[nPedD]["PCOL_CODIGO"];
							drPed["PCOL_NOMBRE"]=dsPedidosD.Tables[0].Rows[nPedD]["PCOL_NOMBRE"];
							drPed["PCOL_CODIGOALTE"]=dsPedidosD.Tables[0].Rows[nPedD]["PCOL_CODIGOALTE"];
							drPed["PCOL_NOMBREALTE"]=dsPedidosD.Tables[0].Rows[nPedD]["PCOL_NOMBREALTE"];
							drPed["PCOL_CODRGB"]=dsPedidosD.Tables[0].Rows[nPedD]["PCOL_CODRGB"];
							drPed["PCOL_CODRGBALTE"]=dsPedidosD.Tables[0].Rows[nPedD]["PCOL_CODRGBALTE"];
							drPed["DPED_CANTPEDI"]=dsPedidosD.Tables[0].Rows[nPedD]["DPED_CANTPEDI"];
							drPed["DPED_VALOUNIT"]=dsPedidosD.Tables[0].Rows[nPedD]["DPED_VALOUNIT"];
							drPed["DPED_FECHALLEGADA"]=dsPedidosD.Tables[0].Rows[nPedD]["DPED_FECHALLEGADA"];
							drPed["DPED_CANTFACT"]=dsPedidosD.Tables[0].Rows[nPedD]["DPED_CANTFACT"];
							drPed["DPED_VALOFACT"]=dsPedidosD.Tables[0].Rows[nPedD]["DPED_VALOFACT"];
							drPed["DPED_CANTFALTA"]=dsPedidosD.Tables[0].Rows[nPedD]["DPED_CANTFALTA"];
							dtPedidosD.Rows.Add(drPed);
							encontrado=true;
						}
						if(!arlPedidos.Contains(dtPedidosM.Rows[nPed]["PEDIDO"].ToString())){
							if(arlPedidos.Count==0){
								prfPed1=dtPedidosM.Rows[nPed]["PDOC_CODIGO"].ToString();
								noPed1=dtPedidosM.Rows[nPed]["MPED_NUMEPEDI"].ToString();
							}
							arlPedidos.Add(dtPedidosM.Rows[nPed]["PEDIDO"].ToString());
						}
					}
					else dtPedidosM.Rows[nPed]["USADO"]=0;
					nPed++;
				}
			}
			if(encontrado)
			{
				if(filtroCatalogos.EndsWith("OR"))filtroCatalogos=filtroCatalogos.Substring(0,filtroCatalogos.Length-3);
				//TREAR VEHICULOS DISPONIBLES 
				//datos pedido(0,1,2,3),datos vehiculo(...)
				string piva=(ViewState["NAC_CLIENTE"].ToString()=="E"?"0":"PV.PIVA_PORCIVA");
				double desc=Convert.ToDouble(DBFunctions.SingleData("SELECT mcli_porcdesc FROM mcliente WHERE mnit_nit='"+nitCliente.Text+"';"));
				
                sqlP="SELECT '"+prfPed1+"' AS PDOC_CODIGO, "+noPed1+" AS MPED_NUMEPEDI, '"+prfPed1+"|"+noPed1+"' AS MVEH_PEDIDO, "+
					"'' AS PCOL_CODRGBPEDI, '' AS PCOL_CODRGBPEDIALT, "+
					"MC.PCAT_CODIGO AS PCAT_CODIGO, MV.MCAT_VIN AS MCAT_VIN, MC.MCAT_MOTOR AS MCAT_MOTOR, PC.PCOL_DESCRIPCION PCOL_NOMBRE,"+
					"PC.PCOL_CODIGO AS PCOL_CODIGO, PC.PCOL_CODRGB AS PCOL_CODRGBVEH, MC.MCAT_ANOMODE AS MCAT_ANOMODE, "+
					"days(CURRENT DATE)-days(MVEH_FECHRECE) AS MVEHI_DIAS, "+
					"0 AS USADO, PP.PPRE_PRECIO AS PRECIO, "+piva+" AS IVA, "+
					"ROUND((PP.PPRE_PRECIO-ROUND(PP.PPRE_PRECIO*mcli_porcdesc/100,0))*"+piva+"/100,0) AS PRECIO_IVA, "+
					"ROUND(PP.PPRE_PRECIO*mcli_porcdesc/100,0) AS DESCUENTO, "+
					"ROUND((PP.PPRE_PRECIO-ROUND(PP.PPRE_PRECIO*mcli_porcdesc/100,0))+((PP.PPRE_PRECIO-ROUND(PP.PPRE_PRECIO*mcli_porcdesc/100,0))*"+piva+"/100),0) AS TOTAL "+
					"FROM MCATALOGOVEHICULO MC, MVEHICULO MV, PCOLOR PC, PPRECIOVEHICULO PP, PCATALOGOVEHICULO PV, MCLIENTE ML "+
					"WHERE MC.MCAT_VIN=MV.MCAT_VIN AND PV.PCAT_CODIGO=MC.PCAT_CODIGO AND "+
					"(MV.TEST_TIPOESTA=20 OR MV.TEST_TIPOESTA=10) AND ML.MNIT_NIT='"+nitCliente.Text+"' AND "+
					"PP.PCAT_CODIGO=MC.PCAT_CODIGO AND "+
					"("+filtroCatalogos+") AND PC.PCOL_CODIGO=MC.PCOL_CODIGO ORDER BY MC.PCAT_CODIGO, MC.MCAT_MOTOR, MVEHI_DIAS DESC;";

                DBFunctions.Request(dsVehiculos,IncludeSchema.NO,sqlP);
				//Seleccionar los de los pedidos y establecer info de los pedidos a los que corresponden
				//COLORES PRINCAIPAL; ALTERNATIVO
				string[] coloresT={"PCOL_CODIGO","PCOL_CODIGOALTE"};
				DataRow[] drVehPed;
				int cP;
				for(int cCol=0;cCol<2;cCol++){
					cP=0;
					for(int nP=0;nP<dtPedidosD.Rows.Count;nP++){
						//No. vehiculos necesitados
						cP=Convert.ToInt16(dtPedidosD.Rows[nP]["DPED_CANTFALTA"]);
						if(cP>0){
							//Buscar vehiculos para el pedido
							drVehPed=dsVehiculos.Tables[0].Select(
								"PCAT_CODIGO='"+dtPedidosD.Rows[nP]["PCAT_CODIGO"]+"' AND "+
								"PCOL_CODIGO='"+dtPedidosD.Rows[nP][coloresT[cCol]]+"' AND "+
								"USADO=0");
							if(drVehPed.Length>0)
							{
								//No. Vehiculos disponibles
								int cV=drVehPed.Length;
								for(int nPV=0;nPV<cP && nPV<cV;nPV++)
								{
									//usar vehiculo
									drVehPed[nPV]["USADO"]=1;
									drVehPed[nPV]["PDOC_CODIGO"]=dtPedidosD.Rows[nP]["PDOC_CODIGO"];
									drVehPed[nPV]["MPED_NUMEPEDI"]=dtPedidosD.Rows[nP]["MPED_NUMEPEDI"];
									drVehPed[nPV]["MVEH_PEDIDO"]=dtPedidosD.Rows[nP]["PDOC_CODIGO"].ToString()+"|"+dtPedidosD.Rows[nP]["MPED_NUMEPEDI"].ToString();
									drVehPed[nPV]["PCOL_CODRGBPEDI"]="<td width='50%' bgcolor='"+dtPedidosD.Rows[nP]["PCOL_CODRGB"].ToString()+"' onclick=\"alert('"+dtPedidosD.Rows[nP]["PCOL_NOMBRE"].ToString()+" ("+dtPedidosD.Rows[nP]["PCOL_CODIGO"].ToString()+")');\">&nbsp;</td>";
									drVehPed[nPV]["PCOL_CODRGBPEDIALT"]="<td width='50%' bgcolor='"+dtPedidosD.Rows[nP]["PCOL_CODRGBALTE"].ToString()+"' onclick=\"alert('"+dtPedidosD.Rows[nP]["PCOL_NOMBREALTE"].ToString()+" ("+dtPedidosD.Rows[nP]["PCOL_CODIGOALTE"].ToString()+")');\">&nbsp;</td>";
									dtPedidosD.Rows[nP]["DPED_CANTFALTA"]=Convert.ToInt16(dtPedidosD.Rows[nP]["DPED_CANTFALTA"])-1;
								}
							}
						}
					}
				}
				//Asignar pedidos similares(contienen catalogo) a los vehiculos no usados aún
				for(int nP=0;nP<dsVehiculos.Tables[0].Rows.Count;nP++){
					DataRow[] drsPedidos;
					if(Convert.ToInt16(dsVehiculos.Tables[0].Rows[nP]["USADO"])==0)
					{
						drsPedidos=dtPedidosD.Select("PCAT_CODIGO='"+dsVehiculos.Tables[0].Rows[nP]["PCAT_CODIGO"]+"'");
						if(drsPedidos.Length>0)
						{
							dsVehiculos.Tables[0].Rows[nP]["PDOC_CODIGO"]=drsPedidos[0]["PDOC_CODIGO"].ToString();
							dsVehiculos.Tables[0].Rows[nP]["MPED_NUMEPEDI"]=Convert.ToInt32(drsPedidos[0]["MPED_NUMEPEDI"]);
							dsVehiculos.Tables[0].Rows[nP]["MVEH_PEDIDO"]=drsPedidos[0]["PDOC_CODIGO"].ToString()+"|"+Convert.ToInt32(drsPedidos[0]["MPED_NUMEPEDI"]);
						}
					}
				}

				//Asignar precio a los vehiculos segun su pedido asociado
				/*DataRow[] drsValor;
				DataRow drVehiculoA;
				double precioA,ivaA;
				for(int nV=0;nV<dsVehiculos.Tables[0].Rows.Count;nV++){
					//Consultar primer precio del catalogo encontrado en los detalles del pedido correspondiente
					drVehiculoA=dsVehiculos.Tables[0].Rows[nV];
					drsValor=dtPedidosD.Select(
						"PDOC_CODIGO='"+drVehiculoA["PDOC_CODIGO"].ToString()+"' AND "+
						"MPED_NUMEPEDI="+drVehiculoA["MPED_NUMEPEDI"].ToString()+" AND "+
						"PCAT_CODIGO='"+drVehiculoA["PCAT_CODIGO"].ToString()+"'");
					if(drsValor.Length>0){
						ivaA=Convert.ToDouble(drVehiculoA["IVA"]);
						precioA=Convert.ToDouble(drsValor[0]["DPED_VALOUNIT"]);
						drVehiculoA["PRECIO"]=precioA;
						drVehiculoA["DESCUENTO"]=Math.Round(precioA*desc/100,0);
						drVehiculoA["PRECIO_IVA"]=Math.Round((precioA-Convert.ToDouble(drVehiculoA["DESCUENTO"]))*ivaA/100,0);
						drVehiculoA["TOTAL"]=Math.Round(precioA-Convert.ToDouble(drVehiculoA["DESCUENTO"])+Convert.ToDouble(drVehiculoA["PRECIO_IVA"]),0);
					}
				}*/

				dsVehiculos.Tables[0].DefaultView.Sort="MVEH_PEDIDO,PCAT_CODIGO, MVEHI_DIAS DESC, MCAT_VIN";
				dtVehiculos=CreateTable(dsVehiculos.Tables[0].DefaultView);
				Session["TABLA_MVEHICULOS"]=dtVehiculos;
				Session["TABLA_MPEDIDOS"]=dtPedidosM;
				Session["PEDIDOS"]=arlPedidos;
				ViewState.Remove("OTROS_CATALOGOS");
				dgrVehiculos.EditItemIndex = -1;
				Bind_dgInserts();
			}
			return(encontrado);
		}
		//Traer pedidos del cliente
		protected bool CargarPedidos(string nitC)
		{
			DataSet dsPedidos=new DataSet();
			DBFunctions.Request(dsPedidos,IncludeSchema.NO,
			"SELECT * FROM ("+
			"SELECT "+
			"MP.PDOC_CODIGO PDOC_CODIGO,MP.MPED_NUMEPEDI MPED_NUMEPEDI, MP.MPED_PEDIDO MPED_PEDIDO, "+
			"SUM(DP.DPED_VALOUNIT*DPED_CANTPEDI) MPED_TOTAL, SUM(DPED_VALOFACT) MPED_VALOFACT, SUM(DPED_CANTPEDI-DPED_CANTFACT) MPED_CANTPEND, "+
			"INT(0) AS USADO, MP.PDOC_CODIGO CONCAT '|' CONCAT RTRIM(CHAR(MP.MPED_NUMEPEDI)) PEDIDO "+
			"FROM DBXSCHEMA.MPEDIDOVEHICULOCLIENTEMAYOR MP, DBXSCHEMA.DPEDIDOVEHICULOCLIENTEMAYOR DP "+
			"WHERE MP.PDOC_CODIGO=DP.PDOC_CODIGO AND DP.MPED_NUMEPEDI=mP.MPED_NUMEPEDI AND MP.MNIT_NIT='"+nitCliente.Text+"' "+
			"AND (MP.TEST_TIPOESTA=10 OR MP.TEST_TIPOESTA=20) "+
			"GROUP BY MP.PDOC_CODIGO,MP.MPED_NUMEPEDI,MP.MPED_PEDIDO "+
			")AS TBLPEDS "+
			"WHERE TBLPEDS.MPED_CANTPEND>0");
			if(dsPedidos.Tables[0].Rows.Count==0)
				return(false);
			dgPedidos.DataSource=dsPedidos.Tables[0];
			dgPedidos.DataBind();
			//Pedidos activos del cliente
			Session["TABLA_MPEDIDOS"]=dsPedidos.Tables[0];
			return(true);
		}
		#endregion
		
		#region Facturacion
		//Facturar
		protected void Facturar(Object  Sender, EventArgs e)
		{
			//Almacena sentencias de faturacion
			ArrayList sqlRefs=null;
			string vinsVehiculos="";
			string codVendedor=txtCodVendedor.Text.Trim();
			string codAlmacen = ddlAlmacen.SelectedValue;
			string codCentroCosto;
			string prefijoFac = prefijoFactura.SelectedValue;
			uint numeroFac = Convert.ToUInt16(numeroFactura.Text);
			string cliente=nitCliente.Text;
			DateTime fechaFactura;
			double totalVehiculos=0,totalDescuentos=0,totalIVA=0,valRetenciones=0,ivaFletes=0,valIVAFletes=0,valFletes=0,totalFactura=0,saldoCartera,cupo;
			DataTable dtVehiculos=(DataTable)Session["TABLA_MVEHICULOS"];
			DataRow[] drVehiculo=dtVehiculos.Select("USADO=1");
			int idUsuario=Convert.ToInt16(DBFunctions.SingleData("select susu_codigo from susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'"));

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
			catch{
                Utils.MostrarAlerta(Response, "Fecha no válida");
				return;}

			//Valor Fletes y retenciones
			try{
				if(txtRetenciones.Text.Length>0) valRetenciones=Convert.ToDouble(txtRetenciones.Text);
				if(txtFletes.Text.Length>0) valFletes=Convert.ToDouble(txtFletes.Text);
				ivaFletes=Convert.ToDouble(ddlIVAFletes.SelectedValue);
				if(ivaFletes<0||valFletes<0||valRetenciones<0)
					throw(new Exception());
				valIVAFletes=Math.Round(((valFletes*ivaFletes)/100),0);
			}
			catch{
                Utils.MostrarAlerta(Response, "Revise el valor de retenciones y fletes");
				return;
			}

			//Saldo, cupo
			try{
				saldoCartera=Convert.ToDouble(txtSaldoCartera.Text.Replace(",",""));
				cupo=Convert.ToDouble(txtCupo.Text.Replace(",",""));}
			catch{
                Utils.MostrarAlerta(Response, "Revise el saldo y cupo del cliente");
				return;}

			//Centro de costo
            codCentroCosto = DBFunctions.SingleData("SELECT pcen_centvvn FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codAlmacen + "'");
			
			//Traer los dias de plazo fecha de vencimiento factura
			string mfac_vence=DBFunctions.SingleData("select mcli_diasplaz from dbxschema.mcliente where mnit_nit='"+cliente+"'");
			if (mfac_vence!=String.Empty)
				mfac_vence=fechaFactura.AddDays(Convert.ToDouble(mfac_vence)).ToString("yyyy-MM-dd");
			else
			 	mfac_vence=fechaFactura.ToString("yyyy-MM-dd");

			//Generar sentencias relacionadas al proceso
			Pedidos.PedidoVehiculosMayorCliente(drVehiculo,ref sqlRefs,1,cliente,ref totalVehiculos, ref totalDescuentos, ref totalIVA, ref totalFactura,txtPrefGuia.Text,txtNumGuia.Text, ref vinsVehiculos, fechaFactura, idUsuario);

			if(vinsVehiculos.EndsWith(","))vinsVehiculos=vinsVehiculos.Substring(0,vinsVehiculos.Length-1);
			
			totalFactura+=valRetenciones+valFletes+valIVAFletes;

			//Si valor a FACTURAR mas el SALDO EN CARTERA es mayor al cupo y el cupo>0 mostrar advertencia
			if(cupo>0 && cupo-(totalFactura+saldoCartera)<0 && lblAdvertencia.Text.Length==0){
				lblAdvertencia.Text="Advertencia: el cupo del cliente es menor al saldo en cartera mas el valor de la factura. Para continuar de todas formas vuelva a dar click en facturar.";
			}

			//VERIFICAR numero de factura esta disponible, cambiarlo si no es manual
			if(DBFunctions.RecordExist("select * from mfacturacliente where pdoc_codigo='"+prefijoFac+"' and mfac_numedocu="+numeroFac)){
				if(tipoFacturacion.SelectedValue.ToString()=="A")
					numeroFac = Convert.ToUInt16(DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFac+"'"));
				else{
                    Utils.MostrarAlerta(Response, "Ya se utilizó el número de factura");
					return;
				}
			}

			//Revisamos si es necesario crear la factura de financiera, se crea como una factura de cliente al nit de la financiera y se agrega esta factura como un pago a la factura de nuestro cliente
			FacturaCliente facturaVentaVehiculo = new FacturaCliente("FVV",prefijoFac, cliente, codAlmacen , "F" ,numeroFac,
				0, fechaFactura, Convert.ToDateTime(mfac_vence), DateTime.Now, totalVehiculos,totalIVA, 
				valFletes, valIVAFletes, valRetenciones, 0, codCentroCosto, observaciones.Text, codVendedor, HttpContext.Current.User.Identity.Name,null);

			facturaVentaVehiculo.SqlRels = sqlRefs;

			//VERIFICAR  DISPONIBILIDAD VEHICULOS ANTES DE FACTURAR
			if(DBFunctions.RecordExist("SELECT * FROM MVEHICULO WHERE (TEST_TIPOESTA<>20 AND TEST_TIPOESTA<>10) AND MCAT_VIN IN("+vinsVehiculos+");"))
			{
				MarcarVehiculosTomados();
                Utils.MostrarAlerta(Response, "Los vehículos marcados con rojo ya han sido facturados, por favor desmárquelos (puede seleccionar otro que esté disponible)");
				return;
			}
			//Como no es necesaria realizar la retoma entonces grabamos la factura directamente
			if(facturaVentaVehiculo.GrabarFacturaCliente(true))
			{
				Session.Remove("TABLA_MVEHICULOS");
				Session.Remove("TABLA_MPEDIDOS");
				Session.Remove("PEDIDOS");
				Response.Redirect("" + indexPage + "?process=Vehiculos.FacturacionPedidoMayor&prefFC="+facturaVentaVehiculo.PrefijoFactura+"&numFC="+facturaVentaVehiculo.NumeroFactura+"&path="+Request.QueryString["path"]);
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
		//Cambia el almacen
		protected void CambioAlmacen(Object  Sender, EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT pd.pdoc_codigo, pd.pdoc_codigo concat ' - ' concat pd.pdoc_nombre FROM dbxschema.pdocumento pd, pdocumentohecho pdh WHERE pd.tdoc_tipodocu='FC' AND pd.pdoc_codigo=pdh.pdoc_codigo AND pdh.palm_almacen='"+ddlAlmacen.SelectedValue+"' AND pdh.tpro_proceso='VN';");
			CambioTipoFacturacion(Sender,e);
		}
		#endregion

		#region Manejo Grilla de Vehiculos
		//Marcar vehiculos(busqueda)
		protected void Marcar_Vehiculos(Object  Sender, EventArgs e)
		{
			int dias=-1;
			string motor,vin;
			if(txtDiasM.Text.Trim().Length>0)
				try{
					dias=Convert.ToInt16(txtDiasM.Text.Trim());
					if(dias<0)throw(new Exception());}
				catch{
                    Utils.MostrarAlerta(Response, "Número de días no válido");
					return;}
			motor=txtMotorM.Text.Trim();
			vin=txtVINM.Text.Trim();
			DataTable dtVehiculos=(DataTable)Session["TABLA_MVEHICULOS"];
			DataRow drVehiculo;
			string filtro="";
			for(int nV=0;nV<dtVehiculos.Rows.Count;nV++){
				drVehiculo=dtVehiculos.Rows[nV];
				filtro="MCAT_VIN='"+drVehiculo["MCAT_VIN"].ToString()+"' ";
				if(dias>=0)
					filtro+=" AND MVEHI_DIAS="+dias;
				if(motor.Length>0)
					filtro+=" AND MCAT_MOTOR like '"+motor+"'";
				if(vin.Length>0)
					filtro+=" AND MCAT_VIN like '"+vin+"'";
				try
				{
					if((dias>=0||vin.Length>0||motor.Length>0) && dtVehiculos.Select(filtro).Length>0)
						dgrVehiculos.Items[nV].BackColor=Color.Yellow;
					else
						dgrVehiculos.Items[nV].BackColor=Color.Empty;
				}
				catch
				{
                    Utils.MostrarAlerta(Response, "Búsqueda no válida");
					return;
				}
			}
		}
		//Agregar un catalogo a los vehiculos disponibles
		protected void Agregar_Catalogo(Object  Sender, EventArgs e)
		{
			DataTable dtVehiculos=(DataTable)Session["TABLA_MVEHICULOS"];
			ArrayList arlCatalogos;
			DataSet dsVehiculos=new DataSet();
			string catalogo=txtCatalogo.Text, sqlP;
			string[] pedido=((ArrayList)Session["PEDIDOS"])[0].ToString().Split('|');
			if(catalogo.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar un catálogo");
				return;
			}
			if(dtVehiculos.Select("PCAT_CODIGO='"+catalogo+"'").Length>0)
			{
                Utils.MostrarAlerta(Response, "Ya se ha agregado el catálogo");
				return;
			}
			if(ViewState["OTROS_CATALOGOS"]==null)
				arlCatalogos=new ArrayList();
			else
				arlCatalogos=(ArrayList)ViewState["OTROS_CATALOGOS"];
			if(!arlCatalogos.Contains(catalogo))
				arlCatalogos.Add(catalogo);

			string piva=(ViewState["NAC_CLIENTE"].ToString()=="E"?"0":"PV.PIVA_PORCIVA");
			double desc=Convert.ToDouble(DBFunctions.SingleData("SELECT mcli_porcdesc FROM mcliente WHERE mnit_nit='"+nitCliente.Text+"';"));
			sqlP="SELECT '"+pedido[0]+"' AS PDOC_CODIGO, "+pedido[1]+" AS MPED_NUMEPEDI, '"+pedido[0]+"|"+pedido[1]+"' AS MVEH_PEDIDO, "+
				"'' AS PCOL_CODRGBPEDI, '' AS PCOL_CODRGBPEDIALT, "+
				"MC.PCAT_CODIGO AS PCAT_CODIGO, MV.MCAT_VIN AS MCAT_VIN, MC.MCAT_MOTOR AS MCAT_MOTOR, PC.PCOL_DESCRIPCION PCOL_NOMBRE,"+
				"PC.PCOL_CODIGO AS PCOL_CODIGO, PC.PCOL_CODRGB AS PCOL_CODRGBVEH, MC.MCAT_ANOMODE AS MCAT_ANOMODE, "+
				"days(CURRENT DATE)-days(MVEH_FECHRECE) AS MVEHI_DIAS, "+
				"0 AS USADO, PP.PPRE_PRECIO AS PRECIO , "+piva+" AS IVA, "+
				"ROUND((PP.PPRE_PRECIO-(PP.PPRE_PRECIO*"+desc+"/100))*"+piva+"/100,0) AS PRECIO_IVA, "+
				"ROUND((PP.PPRE_PRECIO*"+desc+"/100),0) AS DESCUENTO, "+
				"PPRE_PRECIO - ROUND((PP.PPRE_PRECIO*"+desc+"/100),0) + ROUND((PP.PPRE_PRECIO-(PP.PPRE_PRECIO*"+desc+"/100))*"+piva+"/100,0) AS TOTAL "+
				"FROM MCATALOGOVEHICULO MC, MVEHICULO MV, PCOLOR PC, PPRECIOVEHICULO PP, PCATALOGOVEHICULO PV "+
				"WHERE MC.MCAT_VIN=MV.MCAT_VIN AND "+
				"(MV.TEST_TIPOESTA=20 OR MV.TEST_TIPOESTA=10) AND "+
				"MC.PCAT_CODIGO='"+catalogo+"' AND PC.PCOL_CODIGO=MC.PCOL_CODIGO AND "+
				"MC.PCAT_CODIGO=PP.PCAT_CODIGO AND MC.PCAT_CODIGO=PV.PCAT_CODIGO "+
				"ORDER BY MVEHI_DIAS DESC;";
			
			DBFunctions.Request(dsVehiculos,IncludeSchema.NO,sqlP);
			for(int nV=0;nV<dsVehiculos.Tables[0].Rows.Count;nV++)
			{
				DataRow drV=dtVehiculos.NewRow();
				for(int nR=0;nR<dtVehiculos.Columns.Count;nR++)
					drV[nR]=dsVehiculos.Tables[0].Rows[nV][nR];
				dtVehiculos.Rows.Add(drV);
			}
			Session["TABLA_MVEHICULOS"]=dtVehiculos;
			dgrVehiculos.EditItemIndex = -1;
			Bind_dgInserts();
		}

		//Editar Vehiculo
		public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
		{
			dgrVehiculos.EditItemIndex = (int)e.Item.ItemIndex;
			Bind_dgInserts();
		}
		//Actualizar Edicion Vehiculo
		public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
		{
			double valor=0,descuento=0,iva=0,priva=0;
			string[]pedido;
			DataTable dtVehiculos=(DataTable)Session["TABLA_MVEHICULOS"];
			try
			{
				pedido=((DropDownList)e.Item.FindControl("ddlPedido")).SelectedValue.Split('|');
				valor=Convert.ToDouble(((TextBox)e.Item.FindControl("txtPrecio")).Text.Replace(",",""));
				descuento=Convert.ToDouble(((TextBox)e.Item.FindControl("txtDescuento")).Text.Replace(",",""));
				iva=Convert.ToDouble(((DropDownList)e.Item.FindControl("ddlIva")).SelectedValue);
				if(descuento>=valor)
				{
                    Utils.MostrarAlerta(Response, "El descuento no puede superar el valor");
					return;
				}
				if(valor<0||descuento<0||iva<0)
				{
                    Utils.MostrarAlerta(Response, "Los valores no pueden ser negativos");
					return;
				}
				dtVehiculos.Rows[e.Item.ItemIndex]["PDOC_CODIGO"]=pedido[0];
				dtVehiculos.Rows[e.Item.ItemIndex]["MPED_NUMEPEDI"]=Convert.ToInt32(pedido[1]);
				dtVehiculos.Rows[e.Item.ItemIndex]["MVEH_PEDIDO"]=pedido[0]+"|"+pedido[1];
				dtVehiculos.Rows[e.Item.ItemIndex]["PRECIO"]=valor;
				dtVehiculos.Rows[e.Item.ItemIndex]["DESCUENTO"]=descuento;
				dtVehiculos.Rows[e.Item.ItemIndex]["IVA"]=((DropDownList)e.Item.FindControl("ddlIva")).SelectedValue;
				priva=Math.Round(((valor-descuento)*iva)/100);
				dtVehiculos.Rows[e.Item.ItemIndex]["PRECIO_IVA"]=priva;
				dtVehiculos.Rows[e.Item.ItemIndex]["TOTAL"]=valor-descuento+priva;
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Debe dar datos correctos del vehiculo editado");
				return;
			}
			dgrVehiculos.EditItemIndex = -1;
			Session["TABLA_MVEHICULOS"]=dtVehiculos;
			Bind_dgInserts();
		}
		//Cancelar edicion vehiculo
		public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgrVehiculos.EditItemIndex = -1;
			Bind_dgInserts();
		}
		//Vehiculos DataBound
		protected void DgInserts_Bound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				DataTable dtVehiculos=(DataTable)Session["TABLA_MVEHICULOS"];
				DropDownList dlIva = (DropDownList)e.Item.FindControl("ddlIva");
				DropDownList dlPedido = (DropDownList)e.Item.FindControl("ddlPedido");
				dlIva.DataSource=(DataTable)ViewState["PIVA"];
				dlIva.DataTextField="PIVA_PORCIVA";
				dlIva.DataValueField="PIVA_PORCIVA";
				dlIva.DataBind();
				if(dlIva.Items.Count>1)
					dlIva.SelectedIndex=dlIva.Items.IndexOf(dlIva.Items.FindByValue(dtVehiculos.Rows[e.Item.ItemIndex]["IVA"].ToString()));
				dlPedido.DataSource=(ArrayList)Session["PEDIDOS"];
				dlPedido.DataBind();
				if(dlPedido.Items.Count>1)
					dlPedido.SelectedIndex=dlPedido.Items.IndexOf(dlPedido.Items.FindByValue(dtVehiculos.Rows[e.Item.ItemIndex]["MVEH_PEDIDO"].ToString()));
			}
		}
		//Chequean un vehiculo
		protected void DgInserts_Check(object sender, EventArgs e)
		{
			int nF;
			CheckBox chkSel=(CheckBox)sender;
			nF=((DataGridItem)chkSel.Parent.Parent).ItemIndex;
			DataTable dtVehiculos=(DataTable)Session["TABLA_MVEHICULOS"];
			if(chkSel.Checked)
				dtVehiculos.Rows[nF]["USADO"]=1;
			else
				dtVehiculos.Rows[nF]["USADO"]=0;
			Session["TABLA_MVEHICULOS"]=dtVehiculos;
			Bind_dgInserts();
		}
		//Bind vehiculos
		protected void Bind_dgInserts()
		{
			DataTable dtVehiculos=(DataTable)Session["TABLA_MVEHICULOS"];
			dgrVehiculos.DataSource = dtVehiculos;
			dgrVehiculos.DataBind();
			Totales();
		}
		private void MarcarVehiculosTomados()
		{
			DataTable dtVehiculos=(DataTable)Session["TABLA_MVEHICULOS"];
			int nv=0;
			for(int n=0;n<dgrVehiculos.Items.Count;n++)
			{
				if(dgrVehiculos.Items[n].ItemType==ListItemType.Item || dgrVehiculos.Items[n].ItemType==ListItemType.AlternatingItem)
				{
					if(!DBFunctions.RecordExist("SELECT MCAT_VIN FROM MVEHICULO WHERE (TEST_TIPOESTA=20 or TEST_TIPOESTA=10) AND MCAT_VIN='"+dtVehiculos.Rows[nv]["MCAT_VIN"].ToString()+"';"))
						dgrVehiculos.Items[n].BackColor=Color.Tomato;
					nv++;
				}
			}
		}
		//Calcular totales vehiculos
		private void Totales()
		{
			DataTable dtVehiculos=(DataTable)Session["TABLA_MVEHICULOS"];
			double totalV=0,totalIVA=0,subtotal=0,totalD=0,totalFletes=0,totalRetenciones=0;
			//Verificar seleccionados
			for(int nV=0;nV<dtVehiculos.Rows.Count;nV++)
			{
				if(Convert.ToInt16(dtVehiculos.Rows[nV]["USADO"])==1)
				{
					totalV+=Convert.ToDouble(dtVehiculos.Rows[nV]["PRECIO"]);
					totalIVA+=Convert.ToDouble(dtVehiculos.Rows[nV]["PRECIO_IVA"]);
					totalD+=Convert.ToDouble(dtVehiculos.Rows[nV]["DESCUENTO"]);
				}
			}
			try{totalRetenciones=Convert.ToDouble(txtRetenciones.Text.Replace(",",""));}
			catch{}
			try{
				totalFletes=Convert.ToDouble(txtFletes.Text.Replace(",",""));
				totalFletes+=Math.Round((Convert.ToDouble(ddlIVAFletes.SelectedValue)*totalFletes)/100);}
			catch{}
			subtotal=totalIVA+totalV-totalD;
			txtTotalIVA.Text=totalIVA.ToString("#,##0");
			txtTotalVehiculos.Text=totalV.ToString("#,##0");
			txtSubtotal.Text=subtotal.ToString("#,##0");
			txtTotalDescuentos.Text=totalD.ToString("#,##0");
			txtTotal.Text=(totalFletes+totalRetenciones+subtotal).ToString("#,##0");

		}
		#endregion Manejo de Vehiculos
		
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

	}
}
