using System;
using System.IO;
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
using Ajax;
using AMS.Forms;
using AMS.DB;
using AMS.DBManager;
using AMS.Documentos;
using AMS.Inventarios;
using AMS.Tools;

namespace AMS.Produccion
{

	/// <summary>
	///		Descripción breve de AMS_Produccion_EntradasProduccion1.
	/// </summary>
	public partial class AMS_Produccion_EntradasProduccion : System.Web.UI.UserControl
	{

		#region Atributos
		protected ArrayList types = new ArrayList();
		protected ArrayList lbFields = new ArrayList();
		protected Label  lblTipoOrden, lblNumOrden, lblTipoPedido, lblProc, lblCargo;
		protected DropDownList     ddlMedio, ddlPedSel,  ddlTipoPre, ddlNumP;
		protected TextBox    txtPreRe,   txtTotAsig,   txtNumFac,  txtPref      ;
		protected DataSet ds;
		protected DataGrid dgItemsRDir, dgItemsLeg, dgItemsPRec;
		protected PlaceHolder plhFile, plhOpLeg, plhFacE, plhFacF, plhNTPre;
		protected Button btnSelecNIT ;
		protected bool facRealizado = false;
		protected HtmlInputFile File1;
		protected System.Web.UI.WebControls.Button btnLoadFile;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqProceso;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqAlmacen;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqDdlTipoPre;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqTxtPreRe;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqResponsable;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqPrefijoEntrada;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqNumeroEntrada;
		protected System.Web.UI.WebControls.RegularExpressionValidator revTxtNumFacE;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqTxtPref;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqTxtNumFac;
		protected System.Web.UI.WebControls.RegularExpressionValidator revTxtNumFac;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqTxtPlazo;
		protected System.Web.UI.WebControls.RegularExpressionValidator revTxtPlazo;
		protected System.Web.UI.WebControls.RequiredFieldValidator rqTxtNIT;
		protected System.Web.UI.WebControls.ValidationSummary vstotal;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private DatasToControls bind = new DatasToControls();
        private DataTable dtItems;
		private FormatosDocumentos formatoFactura=new FormatosDocumentos();
		#endregion
		#region Ajax
		[Ajax.AjaxMethod]
		public string CargarNumero(string prefijo)
		{
			string valor="";
			valor=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijo+"'");
			return valor;
		}
		#endregion
		#region Eventos
		//LOAD--------------------------------------------------------
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Produccion_EntradasProduccion));
            dtItems = (DataTable)ViewState["dtItems"];
            if(!IsPostBack)
			{
				IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
               	bind.PutDatasIntoDropDownList(ddlAlmacen,"SELECT PA.palm_almacen, PA.palm_descripcion FROM PALMACEN PA, MPLANTAS MP WHERE PA.palm_almacen=MP.mpla_codigo;");
				bind.PutDatasIntoDropDownList(ddlVendedor,string.Format(Almacen.VENDEDORESPORALMACEN,ddlAlmacen.SelectedValue));
                //bind.PutDatasIntoDropDownList(ddlPrefE, string.Format(Documento.DOCUMENTOSTIPOHECHO, "FP", "IP", ddlAlmacen.SelectedValue));
                string sede = ddlAlmacen.SelectedValue.ToString();
                Utils.llenarPrefijos(Response, ref ddlPrefE, "IP", sede, "FP");
                if (ddlPrefE.Items.Count > 0)
                    txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO, ddlPrefE.SelectedValue));
                else
                {
                    txtNumFacE.Text = "0";
                    Utils.MostrarAlerta(Response, "Usted NO HO configurado un documento del tipo FACTURA PROVEEDOR para el proceso PROCESOS PRODUCCION en esta sede, configurelo ...! ");
                    return;
                }
                bind.PutDatasIntoDropDownList(ddlPIVA, "SELECT piva_porciva, piva_decreto FROM piva ORDER BY piva_porciva");
				//bind.PutDatasIntoDropDownList(ddlPrefOrden,"SELECT pdoc_codigo from pdocumento where tdoc_tipodocu='OP';");
                Utils.llenarPrefijos(Response, ref ddlPrefOrden, "%", "%", "OP");
                bind.PutDatasIntoDropDownList(ddlNumOrden, "SELECT mord_numeorde from mordenproduccion where pdoc_codigo='" + ddlPrefOrden.SelectedValue + "' and test_estado='A' and mord_tipo='P' order by mord_numeorde;");
				tbDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
				//txtFlet.Attributes.Add("onkeyup","CalculoIva("+txtFlet.ClientID+","+ddlPIVA.ClientID+","+txtTotIF.ClientID+",'"+txtTotal.ClientID+"','"+txtGTot.ClientID+"');");
				//ddlPIVA.Attributes.Add("onchange","CambioIva("+txtFlet.ClientID+","+ddlPIVA.ClientID+","+txtTotIF.ClientID+",'"+txtTotal.ClientID+"','"+txtGTot.ClientID+"');");
				txtFlet.Attributes.Add("onkeyup","NumericMaskE(this,event);Totales();");
				txtTotalExternos.Attributes.Add("onkeyup","NumericMaskE(this,event);Totales();");
				txtDesc.Attributes.Add("onkeyup","NumericMaskE(this,event);Totales();");
				txtIVA.Attributes.Add("onkeyup","NumericMaskE(this,event);Totales();");
				txtTotal.Attributes.Add("onkeyup","NumericMaskE(this,event);Totales();");
				ddlPIVA.Attributes.Add("onchange","Totales();");
				CambiaProceso(sender,e);
				plcTotales.Visible=false;

                //Cierre de orden de producción
                Utils.llenarPrefijos(Response, ref ddlPrefijoAjuste, "%", "%", "AJ");
                ddlPrefijoAjuste_OnSelectedIndexChanged(null, null);
                
				#region Reportes
				if(Request.QueryString["pref"]!=null && Request.QueryString["num"]!=null)
				{
					FormatosDocumentos formatoFactura=new FormatosDocumentos();
                    Utils.MostrarAlerta(Response, "Se ha generado la factura con prefijo " + Request.QueryString["pref"] + " y número " + Request.QueryString["num"] + "");
					try
					{
						formatoFactura.Prefijo=Request.QueryString["pref"];
						formatoFactura.Numero=Convert.ToInt32(Request.QueryString["num"]);
						formatoFactura.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["pref"]+"'");
						if(formatoFactura.Codigo!=string.Empty)
						{
							if(formatoFactura.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
						}
					}
					catch
					{
						lbInfo.Text="Error al generar el formato. Detalles : <br>"+formatoFactura.Mensajes;
					}
				}

                //Si se cerró la orden de producción
                if (Request.QueryString["act"] != null)
                {
                    Utils.MostrarAlerta(Response, "La orden ha sido cerrada");
                    Imprimir.ImprimirRPT(Response, Request.QueryString["prefA"], Convert.ToInt32(Request.QueryString["numA"]), true);
                }

				#endregion Reportes
			}
		}
		
		//Cambia el tipo de proceso
		protected void CambiaProceso(Object Sender, EventArgs E)
		{
			txtTotIF.Text=txtFlet.Text="0";
			if(ddlTProc.SelectedValue!="1")
			{
				txtTotal.ReadOnly=txtIVA.ReadOnly=txtDesc.ReadOnly=txtTotalExternos.ReadOnly=txtPlazo.ReadOnly=txtNIT.ReadOnly=txtNITa.ReadOnly=false;
			}
			else
			{
				txtNIT.Text=DBFunctions.SingleData("SELECT mnit_nit FROM CEMPRESA;");
				txtNITa.Text=DBFunctions.SingleData("SELECT CEMP_NOMBRE FROM CEMPRESA;");
				txtPlazo.Text="0";
				txtTotal.ReadOnly=txtIVA.ReadOnly=txtDesc.ReadOnly=txtTotalExternos.ReadOnly=txtPlazo.ReadOnly=txtNIT.ReadOnly=txtNITa.ReadOnly=true;
				txtSubTotal.Text=txtTotal.Text=txtIVA.Text=txtDesc.Text=txtTotalExternos.Text="0";
			}
		}
		
		//Seleccionar
		protected void Seleccionar(Object Sender, EventArgs E)
		{
			//Validaciones
			if(!DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='"+txtNIT.Text+"';"))
            {
                Utils.MostrarAlerta(Response, "El NIT no existe!");
				return;
            }
			try{Convert.ToDateTime(tbDate.Text);}
            catch { Utils.MostrarAlerta(Response, "Fecha no válida!"); return; }
            try{Convert.ToInt32(txtPlazo.Text);}
            catch { Utils.MostrarAlerta(Response, "Plazo no válido!"); return; }
            try{Convert.ToUInt32(txtNumFacE.Text);}
            catch { Utils.MostrarAlerta(Response, "El número de entrada de almacén " + txtNumFacE.Text + " NO es valido!"); return; }
            
			txtNITa.Enabled=txtNIT.Enabled=txtPlazo.Enabled=tbDate.Enabled=ddlNumOrden.Enabled=ddlPrefOrden.Enabled=txtNumFacE.Enabled=ddlPrefE.Enabled=ddlVendedor.Enabled=ddlAlmacen.Enabled=ddlTProc.Enabled=false;
			CargarItems();
			btnSeleccionar.Visible=false;
			plcTotales.Visible=true;

            if (chkCerrarO.Checked == true)
            {
                LlenarItems();
                pnlDetalle.Visible = true;
                pnlObservacion.Visible = true;
            }
		}
		//REALIZAR PROCESO-------------------------------------	
		protected void NewAjust(Object Sender, EventArgs E)
		{
            if (chkCerrarO.Checked == true)
            {
                Totales();
            }
            btnAjus.Enabled = false;
            int diasP = 0; //Dias plazo
			double vFlet, vIVAFlet, total, vDescuento, vIVA, vExternos;
			DateTime fechaProceso;
			string vend,codigoAlmacen,ccos,carg,prefE,ano_cinv,tipoE;
			UInt64 numPre;
			ArrayList sqlStrings;

			#region Validaciones
			try{numPre=Convert.ToUInt32(txtNumFacE.Text);}
            catch { Utils.MostrarAlerta(Response, "El número de entrada de almacén no es valido!");BindDatas(); return; }
            
			try{diasP=Convert.ToInt32(txtPlazo.Text);}
            catch { Utils.MostrarAlerta(Response, "Los días de plazo no son válidos!"); BindDatas(); return; }
          
			try
			{
				vDescuento=Convert.ToDouble(txtDesc.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor de los descuentos no es válido!");
				return;}

			try
			{
				vExternos=Convert.ToDouble(txtTotalExternos.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor de los totales externos no es valido!");
				return;}

			try
			{
				vFlet=Convert.ToDouble(txtFlet.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor de los fletes no es valido!");
				return;}
			try
			{
				vIVAFlet=Convert.ToDouble(txtTotIF.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor IVA de los fletes no es valido!");
				return;}
			try
			{
				vIVA=Convert.ToDouble(txtIVA.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor del IVA no es valido!");
				return;}
			try
			{
				fechaProceso=Convert.ToDateTime(tbDate.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "Fecha de proceso no es valido!");
				return;
			}
			try
			{
				total=Convert.ToDouble(txtTotal.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor del total no es valido!");
				return;
			}
			if(vExternos<vDescuento)
			{
                Utils.MostrarAlerta(Response, "Los descuentos no pueden superar los valores externos!");
				return;}
			//Validar total
			if(Math.Abs((vExternos-vDescuento+vIVA+vFlet+vIVAFlet)-(total))>0.009)
			{
                Utils.MostrarAlerta(Response, "El valor del total no coincide!");
				BindDatas();
				return;
			}
			#endregion Validaciones

			//Aqui Iniciamos el proceso como tal, habiendo superado con exito el proceso de validacion
			vend = ddlVendedor.SelectedValue;//Codigo del Vendedor
			codigoAlmacen = ddlAlmacen.SelectedValue;
			ccos = DBFunctions.SingleData("SELECT pcen_centinv FROM palmacen WHERE palm_almacen='"+codigoAlmacen+"'");
			carg = DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vend+"'");//Cargo???????
			prefE  = "";//Prefijo Documento Interno (PDOCUMENTO)
			numPre = 0;//Numero de Documento Interno (PDOCUMENTO)
			ano_cinv = ConfiguracionInventario.Ano;
			tipoE = DBFunctions.SingleData("SELECT MORD_TIPO FROM MORDENPRODUCCION WHERE PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND MORD_NUMEORDE="+ddlNumOrden.SelectedValue+";");
			sqlStrings = new ArrayList();
	
			#region Factura Proveedor
			ArrayList arrRecepciones = new ArrayList();
			FacturaProveedor facturaRepuestos = new FacturaProveedor();
			string prefijoFacturaProveedor = ddlPrefE.SelectedValue;
			UInt64 numeroFacturaProveedor = Convert.ToUInt64(txtNumFacE.Text.Trim());
			prefE = prefijoFacturaProveedor;
			numPre = numeroFacturaProveedor;
		
			if(DBFunctions.RecordExist("SELECT * FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFacturaProveedor+"' AND mfac_numeordepago="+numeroFacturaProveedor+""))
				numeroFacturaProveedor = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFacturaProveedor+"'"));
		
			FacturaProveedor facturaRepuestosProv = new FacturaProveedor("FPR",prefijoFacturaProveedor,
				ddlPrefOrden.SelectedValue,txtNIT.Text.Trim(),ddlAlmacen.SelectedValue,"F",numeroFacturaProveedor,
				Convert.ToUInt64(ddlNumOrden.SelectedValue),"P",fechaProceso,
				fechaProceso.AddDays(Convert.ToDouble(txtPlazo.Text.Trim())),
				Convert.ToDateTime(null),fechaProceso,vExternos-vDescuento,
				vIVA,vFlet,vIVAFlet,0,txtObs.Text,
				HttpContext.Current.User.Identity.Name.ToLower());
		
			facturaRepuestosProv.GrabarFacturaProveedor(false);
			numPre = facturaRepuestosProv.NumeroFactura;
		
			for(int i=0;i<facturaRepuestosProv.SqlStrings.Count;i++)
				sqlStrings.Add(facturaRepuestosProv.SqlStrings[i].ToString());
			#endregion Factura Proveedor
		
			#region Retenciones
			DataTable dtRet=TablaRetenciones();
			dtRet.Rows[0][0]="1";
			dtRet.Rows[0][1]=DBFunctions.SingleData("SELECT MITE_CODIGO FROM MITEMS WHERE TORI_CODIGO='X' FETCH FIRST 1 ROWS ONLY;");
			dtRet.Rows[0][2]="";
			dtRet.Rows[0][3]=1;
			dtRet.Rows[0][4]=1;
			dtRet.Rows[0][5]=vExternos;
			dtRet.Rows[0][6]=vDescuento;
			dtRet.Rows[0][7]=(vIVA*100)/vExternos;
			dtRet.Rows[0][8]="";
			dtRet.Rows[0][9]=total;
			dtRet.Rows[0][10]="";
			dtRet.Rows[0][11]="";
			try
			{
				Retencion RetencionItems=new Retencion(facturaRepuestosProv.NitProveedor,
					facturaRepuestosProv.PrefijoFactura,
					Convert.ToInt32(facturaRepuestosProv.NumeroFactura),
					dtRet,
					(facturaRepuestosProv.ValorFactura+facturaRepuestosProv.ValorFletes),
					(facturaRepuestosProv.ValorIva+facturaRepuestosProv.ValorIvaFletes),
					"R",false);

				RetencionItems.Guardar_Retenciones(false);

				for(int i=0;i<RetencionItems.Sqls.Count;i++)
					sqlStrings.Add(RetencionItems.Sqls[i].ToString());
			}
			catch(Exception ex)
			{
                Utils.MostrarAlerta(Response, "Error en Retenciones. Detalles : \\n" + ex.Message + "");
				return;
			}
			#endregion

			#region Movimiento de Kardex
			string ano_cont = ConfiguracionInventario.Ano;

			int tm = 20;//Entradas de proveedor
			//Creamos el Objeto manejador de movimientos de kardex utilizando el constructor #2
			Movimiento Mov = new Movimiento(prefE,numPre,tm,fechaProceso,txtNIT.Text,codigoAlmacen,vend,carg,ccos,"N");
		
			double cant,valU,costP,costPH,costPA,costPHA,invI,invIA,pIVA,pDesc,cantDev,valP;
		
			ArrayList Prrs = new ArrayList();//Prerecepciones de la lista
			DataTable dtItems=(DataTable)ViewState["ITEMS"];
			//DITEMS
			for(int n=0;n<dtItems.Rows.Count;n++)
			{
				string codI = dtItems.Rows[n]["CODIGO"].ToString();
				string prefijoDocumentoReferencia = "";
				UInt64 numeroDocumentoReferencia = 0;
			    prefijoDocumentoReferencia = ddlPrefOrden.SelectedValue; // prefijo Orden
                numeroDocumentoReferencia = Convert.ToUInt32(ddlNumOrden.SelectedValue); // Numero Orden
				cant = Convert.ToDouble(dtItems.Rows[n]["CANTIDAD"]);//cantidad ingresada
				valU = Convert.ToDouble(dtItems.Rows[n]["VALOR"]);//(Costeo)
				pIVA = 0; //iva
				pDesc = 0; //descuento
				costP = SaldoItem.ObtenerCostoPromedio(codI,ano_cont);
				costPH = SaldoItem.ObtenerCostoPromedioHistorico(codI,ano_cont);
				costPA = SaldoItem.ObtenerCostoPromedioAlmacen(codI,ano_cont,codigoAlmacen);
				costPHA = SaldoItem.ObtenerCostoPromedioHistoricoAlmacen(codI,ano_cont,codigoAlmacen);
				invI = SaldoItem.ObtenerCantidadActual(codI,ano_cont);//Inventario inicial
				invIA = SaldoItem.ObtenerCantidadActualAlmacen(codI,ano_cont,codigoAlmacen);//Inventario inicial Almacen
				cantDev = 0;//devolucion
				valP = costP;//Valor publico
				Mov.InsertaFila(codI,cant,valU,costP,costPA,pIVA,pDesc,cantDev,costPH,costPHA,valP,invI,invIA,prefijoDocumentoReferencia,numeroDocumentoReferencia);
				//				0	  1		2	3		4	 5		6		7	  8		9	   10	11	  12			13							14
			}

			Mov.RealizarMov(false);
			for(int i=0;i<Mov.SqlStrings.Count;i++)
				sqlStrings.Add(Mov.SqlStrings[i].ToString());
			#endregion

			#region Actualizar detalle y orden produccion
			//Actualizar cantidad entregada
			for(int n=0;n<dtItems.Rows.Count;n++)
				sqlStrings.Add(
					"UPDATE DORDENPRODUCCION SET "+
					"DORD_CANTENTR=DORD_CANTENTR+"+dtItems.Rows[n]["CANTIDAD"]+" "+
					"WHERE PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND "+
					"MORD_NUMEORDE="+ddlNumOrden.SelectedValue+" AND "+
					"PENS_CODIGO='"+dtItems.Rows[n]["ENSAMBLE"]+"' AND "+
					"MITE_CODIGO='"+dtItems.Rows[n]["CODIGO"]+"';");

			//cerrar mordenproduccion si se recibieron todos los items
			sqlStrings.Add(
				"UPDATE MORDENPRODUCCION MO SET MO.TEST_ESTADO='F' "+
				"WHERE MO.pdoc_codigo='"+ddlPrefOrden.SelectedValue+"' AND mord_numeorde="+ddlNumOrden.SelectedValue+" AND "+
				"NOT EXISTS("+
				" SELECT * FROM DORDENPRODUCCION DOR "+
				" WHERE "+
				" DOR.pdoc_codigo=MO.pdoc_codigo AND DOR.mord_numeorde=MO.mord_numeorde AND "+
				" DOR.DORD_CANTXPROD>DORD_CANTENTR "+
				");");
			#endregion

			#region Costeo
			//Costeo
			sqlStrings.Add("INSERT INTO dcostoproduccion values("+
				"'"+ddlPrefE.SelectedValue+"',"+numPre+","+
				Convert.ToDouble(ViewState["TOT_MATPRIMA"]).ToString()+","+
				Convert.ToDouble(ViewState["TOT_TERCEROS"]).ToString()+","+
				Convert.ToDouble(ViewState["TOT_MANOBRA"]).ToString()+","+
				Convert.ToDouble(ViewState["TOT_CIF"]).ToString()+","+
				Convert.ToDouble(ViewState["TOT_MAQUINARIA"]).ToString()+","+
				"'"+ViewState["MITE_CODIGO"].ToString()+"'"+
				");");
			#endregion Costeo
            
            //Cerrar orden de producción
            if (chkCerrarO.Checked == true)
            {
                string sql = CerrarOrdenProduccion();
                if (sql != "")
                    sqlStrings.Add(sql);
                else
                {
                    Utils.MostrarAlerta(Response, "Se presentó un error al intentar cerrar la orden de ooperación!");
                    return;
                }
            }

            if (chkCerrarO.Checked == false)
            {
                if (DBFunctions.Transaction(sqlStrings))
                    Response.Redirect("" + indexPage + "?process=Produccion.EntradasProduccion&path=" + Request.QueryString["path"] + "&pref=" + prefijoFacturaProveedor + "&num=" + numeroFacturaProveedor);
                else
                    lbInfo.Text += "<br>Error : Detalles <br>" + DBFunctions.exceptions;
            }
            else
            {
                if (DBFunctions.Transaction(sqlStrings) && RealizarAjusteInventario())
                        Response.Redirect("" + indexPage + "?process=Produccion.EntradasProduccion&path=" + Request.QueryString["path"] + "&pref=" + prefijoFacturaProveedor + "&num=" + numeroFacturaProveedor + "&act=1&prefA=" + ddlPrefijoAjuste.SelectedValue + "&numA=" + lblNumeroAjuste.Text);
                else
                    lbInfo.Text += "<br>Error : Detalles <br>" + DBFunctions.exceptions;
            }

            
		}

		#endregion Eventos
		#region Metodos
		//Crear tabla retenciones
		public DataTable TablaRetenciones()
		{
			DataTable dtRets;
			ArrayList lbFields=new ArrayList();
			ArrayList types=new ArrayList();
			lbFields.Add("num_ped");//0 Numero pedido o Prerecepcion
			types.Add(typeof(string));
			lbFields.Add("mite_codigo");//1 codigo Item
			types.Add(typeof(string));
			lbFields.Add("mite_nombre");//2 nombre Item
			types.Add(typeof(string));
			lbFields.Add("mite_cantped");//3 Cantidad ingresada
			types.Add(typeof(double));
			lbFields.Add("mite_cantfac");//4 Cantidad facturada
			types.Add(typeof(double));
			lbFields.Add("mite_precio");//5 Valor unidad
			types.Add(typeof(double));
			lbFields.Add("mite_desc");//6 Descuento
			types.Add(typeof(double));
			lbFields.Add("mite_iva");//7 Iva
			types.Add(typeof(double));
			lbFields.Add("mite_ubic");//8 Ubicacion
			types.Add(typeof(string));
			lbFields.Add("mite_tot");//9 Total
			types.Add(typeof(double));
			lbFields.Add("mite_unid");//10 Unidad medida
			types.Add(typeof(string));
			lbFields.Add("plin_codigo");//11 Linea Bodega
			types.Add(typeof(string));
			dtRets = new DataTable();
			for(int i=0; i<lbFields.Count; i++)
				dtRets.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
			DataRow drRet=dtRets.NewRow();
			dtRets.Rows.Add(drRet);
			return(dtRets);
		}
		//Cargar grilla items
		private void CargarItems()
		{
			string tipo=DBFunctions.SingleData(
				"SELECT MORD_TIPO FROM MORDENPRODUCCION "+
				"WHERE PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND MORD_NUMEORDE="+ddlNumOrden.SelectedValue+";");
			DataSet dsItems=new DataSet();
			if(ddlPrefOrden.Items.Count==0 || ddlNumOrden.Items.Count==0)
			{
				dgrItems.Visible=false;
				dsItems.Tables.Add(new DataTable());
			}
			else
			{
				dgrItems.Visible=true;
				if(tipo=="E")
					DBFunctions.Request(dsItems,IncludeSchema.NO,
                        "SELECT pc.PCAT_CODIGO CODIGO, pc.PCAT_DESCRIPCION NOMBRE, (dor.DORD_CANTXPROD - dor.DORD_CANTENTR) As  CANTIDAD, dor.PENS_CODIGO ENSAMBLE, 0.00 AS VALOR " +
						"FROM PCATALOGOVEHICULO pc, DORDENPRODUCCION dor "+
						"WHERE pc.PCAT_CODIGO=dor.PCAT_CODIGO AND "+
						"PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND "+
						"MORD_NUMEORDE="+ddlNumOrden.SelectedValue+";");
				else
					DBFunctions.Request(dsItems,IncludeSchema.NO,
                        "SELECT mi.MITE_CODIGO CODIGO, mi.MITE_NOMBRE NOMBRE, (dor.DORD_CANTXPROD - dor.DORD_CANTENTR) As  CANTIDAD, dor.PENS_CODIGO ENSAMBLE, 0.00 AS VALOR " +
						"FROM MITEMS mi, DORDENPRODUCCION dor "+
						"WHERE mi.MITE_CODIGO=dor.MITE_CODIGO AND "+
						"PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND "+
						"MORD_NUMEORDE="+ddlNumOrden.SelectedValue+";");
			}
			ViewState["ITEMS"]=dsItems.Tables[0];
			btnAjus.Enabled = dsItems.Tables[0].Rows.Count>0;
			BindDatas();
		}
		
		//Calcular Totales
		private void Totales()
		{
            DataTable dtItems=(DataTable)ViewState["ITEMS"];

            DataSet dsValorItems = new DataSet();
            DBFunctions.Request(dsValorItems, IncludeSchema.NO,
                        "SELECT mi.MITE_CODIGO CODIGO, dor.DORD_CANTXPROD CANTIDAD " +
                        "FROM MITEMS mi, DORDENPRODUCCION dor WHERE mi.MITE_CODIGO=dor.MITE_CODIGO AND " +
                        "PDOC_CODIGO='" + ddlPrefOrden.SelectedValue + "' AND " +
                        "MORD_NUMEORDE=" + ddlNumOrden.SelectedValue + ";");
            
            
            double subtotal=0,CIF,cargaPrestacional,costeoI=0;
			double cantidad=0,totalCantidad=0;
			int items=0;
			string horasMes;
			double costoHoraPlanta=Convert.ToDouble(DBFunctions.SingleData("SELECT MPLA_COSTOHORA FROM MPLANTAS WHERE MPLA_CODIGO='"+ddlAlmacen.SelectedValue+"';"));
			double capacidadPlanta=Convert.ToDouble(DBFunctions.SingleData("SELECT MPLA_CAPACIDAD FROM MPLANTAS WHERE MPLA_CODIGO='"+ddlAlmacen.SelectedValue+"';"));
			double totMateriaPrima=0,totTrabajoTerceros=0,totManoObra=0,totMaquinas=0,totCIF=0;
			DateTime fechaP;
			try{
				fechaP=Convert.ToDateTime(tbDate.Text).AddMonths(-1);}
			catch{
                Utils.MostrarAlerta(Response, "Fecha no válida, utilizando fecha actual.");
				tbDate.Text=DateTime.Now.ToString("yyyy-MM-dd");
				fechaP=DateTime.Now.AddMonths(-1);}
			//Consultar CIF
			CIF=CosteoProduccion.TraerCIF(ddlAlmacen.SelectedValue,fechaP.Year,fechaP.Month);
			//Horas laboradas al mes
			horasMes=DBFunctions.SingleData("SELECT cnom_horaslabxmes from CNOMINA");
			if(horasMes.Length==0)horasMes="0";
			//Carga Prestacional
			cargaPrestacional=Convert.ToDouble(DBFunctions.SingleData("SELECT CNOM_PORCCARGPRES from CNOMINA"));
			string debug="",mtcod="";
			for(int i=0;i<dtItems.Rows.Count;i++)
			{
				cantidad=Convert.ToDouble(dtItems.Rows[i]["CANTIDAD"]);
				items++;
				totalCantidad+=cantidad;
				//if(ddlTProc.SelectedValue!="1")
				{
					CosteoProduccion costeo=new CosteoProduccion(
						dtItems.Rows[i]["CODIGO"].ToString(), cantidad,
						dtItems.Rows[i]["ENSAMBLE"].ToString(), TipoCosteo.Produccion,
						CIF, horasMes, costoHoraPlanta, capacidadPlanta,cargaPrestacional);
					mtcod=dtItems.Rows[i]["CODIGO"].ToString();

                    //Se confirma si hace cierre para realizar los calculos correspondientes a las entregas de producción.
                    if (chkCerrarO.Checked == true)
                    {
                        costeo.hacerCierre = true;
                        costeo.cantidadOriginal = Convert.ToDouble(dsValorItems.Tables[0].Rows[i]["CANTIDAD"].ToString());
                        costeo.cantidadActual = Convert.ToDouble(dtItems.Rows[i]["CANTIDAD"].ToString() );
                        costeo.dgCantidadSubItems = dgItems;
                    }

					costeoI=costeo.Calcular();
                    if(costeo.error.Length>0)
					{
                        Utils.MostrarAlerta(Response, "" + costeo.error + "");
						this.btnAjus.Enabled=false;
						return;
					}
					totMateriaPrima+=costeo.totalMateriaPrima;
					totTrabajoTerceros+=costeo.totalTrabajoTerceros;
					totManoObra+=costeo.totalManoObra;
					totMaquinas+=costeo.totalMaquinas;
					totCIF+=costeo.totalCIF;
					subtotal+=costeoI;
					dtItems.Rows[i]["VALOR"]=Math.Round(costeoI/cantidad,2);
					debug+=costeo.debug+"<br><br>";
				}
			}
			ViewState["TOT_MATPRIMA"]=totMateriaPrima;
			ViewState["TOT_TERCEROS"]=totTrabajoTerceros;
			ViewState["TOT_MANOBRA"]=totManoObra;
			ViewState["TOT_CIF"]=totCIF;
			ViewState["TOT_MAQUINARIA"]=totMaquinas;
			ViewState["MITE_CODIGO"]=mtcod;
			txtNumUnid.Text=totalCantidad.ToString();
			txtNumItem.Text=items.ToString();
			txtSubTotal.Text=subtotal.ToString("#,##0");
		}
		#endregion Metodos
		#region Grilla Items
		private void BindDatas()
		{
			DataTable dtItems=(DataTable)ViewState["ITEMS"];
			dgrItems.DataSource=dtItems;
			dgrItems.DataBind();
			Totales();
		}
	
		public void dgrItems_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgrItems.EditItemIndex=-1;
			BindDatas();
		}
		public void dgrItems_Update(Object sender, DataGridCommandEventArgs e)
		{
			DataTable dtItems=(DataTable)ViewState["ITEMS"];
			int cantidad;
			try{
				cantidad=Convert.ToInt32(((TextBox)e.Item.FindControl("txtCantidad")).Text.Replace(",",""));
				if(cantidad<=0)throw(new Exception());
				dtItems.Rows[e.Item.ItemIndex]["CANTIDAD"]=cantidad;
			}
			catch{
                Utils.MostrarAlerta(Response, "Cantidad no válida.");
			}
			ViewState["ITEMS"]=dtItems;
			dgrItems.EditItemIndex=-1;
			BindDatas();
		}
		public void dgrItems_Delete(Object sender, DataGridCommandEventArgs e)
		{
		}
		public void dgrItems_Edit(Object sender, DataGridCommandEventArgs e)
		{
			DataTable dtItems=(DataTable)ViewState["ITEMS"];
			if(dtItems.Rows.Count>0)
				dgrItems.EditItemIndex=(int)e.Item.ItemIndex;
			BindDatas();
		}
	 	
		#endregion Grilla Items
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
		#region Dropdownlists
		protected void ddlAlmacen_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bind.PutDatasIntoDropDownList(ddlVendedor,string.Format(Almacen.VENDEDORESPORALMACEN,ddlAlmacen.SelectedValue));
            bind.PutDatasIntoDropDownList(ddlPrefE, string.Format(Documento.DOCUMENTOSTIPOHECHO, "FP", "IP", ddlAlmacen.SelectedValue));
            if (ddlPrefE.Items.Count > 0)
                txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO, ddlPrefE.SelectedValue));
            else txtNumFacE.Text = "0";
            
		}
		protected void ddlPrefE_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO,ddlPrefE.SelectedValue));
		}

		protected void ddlPrefOrden_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            bind.PutDatasIntoDropDownList(ddlNumOrden, "SELECT mord_numeorde from mordenproduccion where pdoc_codigo='" + ddlPrefOrden.SelectedValue + "' and test_estado='A' ORDER BY mord_numeorde;");
		}

        protected void ddlPrefijoAjuste_OnSelectedIndexChanged(Object Sender, EventArgs E)
        {
            lblNumeroAjuste.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo = '" + ddlPrefijoAjuste.SelectedValue + "'");
        }

        private void LlenarItems()
        {
            //string sql = String.Format(
            //    "SELECT mi.mite_codigo as \"mite_codigo\", \n" +
            //    "       mi.mite_nombre as \"mite_nombre\", \n" +
            //    "       mi.plin_codigo as \"mite_linea\", \n" +
            //    "       pu.puni_nombre as \"puni_nombre\", \n" +
            //    "       di.dite_cantidad as \"dite_cantidad\", \n" +
            //    "       di.dite_valounit as \"msal_costprom\" \n" +
            //    "FROM dordenproduccion do  \n" +
            //    "  LEFT JOIN mordenproducciontransferencia mot  \n" +
            //    "		  LEFT JOIN ditems di  \n" +
            //    "				  LEFT JOIN mitems mi  \n" +
            //    "				  		LEFT JOIN punidad pu ON mi.puni_codigo = pu.puni_codigo  \n" +
            //    "				  ON di.mite_codigo = mi.mite_codigo  \n" +
            //    "		  ON di.pdoc_codigo = mot.pdoc_factura AND dite_numedocu = mot.mfac_numero  \n" +
            //    "  ON do.pdoc_codigo = mot.pdoc_codigo AND do.mord_numeorde = mot.mord_numeorde \n" +
            //    "WHERE do.pdoc_codigo = '{0}' \n" +
            //    "AND   do.mord_numeorde = {1}"
            //    , ddlPrefOrden.SelectedValue
            //    , ddlNumOrden.SelectedValue);

            string sql = String.Format(
                "SELECT " +
                "mi.mite_codigo as \"mite_codigo\",  " +
                "mi.mite_nombre as \"mite_nombre\",  " +
                "mi.plin_codigo as \"mite_linea\",  " +
                "pu.puni_nombre as \"puni_nombre\",  " +
                "mp.MENS_CANTIDAD * do.dord_cantxprod as \"dite_cantidad\" , " +
                "ms.msal_costprom  as \"msal_costprom\" " +
                "FROM  PENSAMBLEPRODUCCION pe,  dordenproduccion do, MENSAMBLEPRODUCCIONITEMS mp " +
                "LEFT JOIN mitems mi   " +
                "LEFT JOIN punidad pu ON mi.puni_codigo = pu.puni_codigo   " +
                "left join msaldoitem ms on mi.mite_codigo = ms.mite_codigo and pano_ano= (select pano_ano from cinventario) " +
                "ON mi.mite_codigo = mp.mite_codigo   " +
                "WHERE pe.PENS_CODIGO=mp.PENS_CODIGO AND pe.PENS_CODIGO='GUART002' and pe.pens_vigente='S' and do.pdoc_codigo= '{0}' " +
                "AND  do.mord_numeorde = {1} and pe.PENS_CODIGO = do.pens_codigo;", ddlPrefOrden.SelectedValue
                , ddlNumOrden.SelectedValue);

            dtItems = DBFunctions.Request(new DataSet(), IncludeSchema.NO, sql).Tables[0];
            dtItems.Columns.Add("msal_cantasig");

            dgItems.DataSource = dtItems;
            dgItems.DataBind();
            ViewState["dtItems"] = dtItems;
        }

        protected string CerrarOrdenProduccion()
        {
            string arlSql = "";
            bool datosValidos = actualizarCantidadesItems();
            if (!datosValidos) return arlSql;

            if (txtObservacion.Text.Trim().Length == 0)
            {
                Utils.MostrarAlerta( Response, "Debe dar la observación de cierre de la orden de producción.");
                return arlSql;
            }

            arlSql = "update MORDENPRODUCCION set test_estado='F', MORD_OBSERVACION='" + txtObservacion.Text + "' " +
                "WHERE PDOC_CODIGO='" + ddlPrefOrden.SelectedValue + "' AND MORD_NUMEORDE=" + ddlNumOrden.SelectedValue + ";";

            return arlSql;
        }

        private bool actualizarCantidadesItems()
        {
            for (int i = 0; i < dtItems.Rows.Count; i++)
            {
                string cantidadDevolucion = ((TextBox)dgItems.Items[i].FindControl("txtCantidad")).Text;
                double cantidadPedida = Convert.ToDouble(dtItems.Rows[i]["dite_cantidad"]);

                if (!Utils.EsNumero(cantidadDevolucion))
                {
                    Utils.MostrarAlerta(Response, "Por favor ingrese datos numéricos en las catidades a devolver");
                    return false;
                }
                if (cantidadPedida < Convert.ToDouble(cantidadDevolucion))
                {
                    Utils.MostrarAlerta(Response, "No se puede devolver una mayor cantidad a la transferida a planta");
                    return false;
                }

                dtItems.Rows[i]["msal_cantasig"] = cantidadDevolucion;
            }

            return true;
        }

        private bool RealizarAjusteInventario()
        {
            string centroCosto = "";
            string msjError = "";

            if (Request.QueryString["ens"] == null)
                centroCosto = DBFunctions.SingleData("select pcen_centplan from cproduccion");
            else
                centroCosto = DBFunctions.SingleData("select pcen_centensa from cproduccion");

            string sql = String.Format(
                "SELECT mpi.palm_almacen, \n" +
             "       mop.pven_codigo \n" +
             "FROM mordenproduccion mop  \n" +
             "  LEFT JOIN MPEDIDOPRODUCCIONTRANSFERENCIA mpp  \n" +
             "  		LEFT JOIN mpedidoitem mpi ON mpi.pped_codigo = mpp.pped_codigo AND mpi.mped_numepedi = mpp.mped_numero  \n" +
             "  ON mop.pdoc_codigo = mpp.pdoc_codigo AND mop.mord_numeorde = mpp.mord_numeorde \n" +
             "WHERE mop.pdoc_codigo = '{0}' \n" +
             "AND   mop.mord_numeorde = {1}"
                , ddlPrefOrden.SelectedValue
                , ddlNumOrden.SelectedValue);

            Hashtable hashOrdenProd = (Hashtable)DBFunctions.RequestAsCollection(sql)[0]; // debe traer siempre una orden

            bool resultado = AjustesInv.RealizarAjusteInventario(
                ddlPrefijoAjuste.SelectedValue,
                Convert.ToInt32(lblNumeroAjuste.Text),
                ddlPrefOrden.SelectedValue,
                Convert.ToInt32(ddlNumOrden.SelectedValue),
                hashOrdenProd["PALM_ALMACEN"].ToString(),
                DateTime.Now,
                hashOrdenProd["PVEN_CODIGO"].ToString(),
                centroCosto,
                txtObservacion.Text,
                dtItems,
                ref msjError);

            if (!resultado)
                lbInfo.Text += msjError;

            return resultado;
        }


		#endregion
	}
}
