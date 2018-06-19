namespace AMS.Produccion
{
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

	/// <summary>
	///		Descripción breve de AMS_Produccion_EntradasEnsambles.
	/// </summary>
	public partial class AMS_Produccion_EntradasEnsambles : System.Web.UI.UserControl
	{
		#region Controles
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private DataTable dtColores, dtEnsambles;
		protected System.Web.UI.WebControls.TextBox txtPlazo;
		protected System.Web.UI.WebControls.Label Label5;

		#endregion Controles
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Produccion_EntradasEnsambles));
			if(!Page.IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				DateTime dttFecha = DateTime.Now;
				DateTime dttFecha1 = dttFecha.AddMonths(-1);
				DateTime dttFecha2 = dttFecha.AddMonths(-2);
				DateTime dttFecha3 = dttFecha.AddMonths(1);
				DateTime dttFecha4 = dttFecha.AddMonths(2);

				IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
				//bind.PutDatasIntoDropDownList(ddlPrefOrden,"SELECT pdoc_codigo from pdocumento where tdoc_tipodocu='OP';");
                Utils.llenarPrefijos(Response, ref ddlPrefOrden , "%", "%", "OP");
				bind.PutDatasIntoDropDownList(ddlNumOrden,"SELECT mord_numeorde from mordenproduccion where pdoc_codigo='"+ddlPrefOrden.SelectedValue+"' and test_estado='A' and mord_tipo='E';");
				bind.PutDatasIntoDropDownList(ddlLote,
					"SELECT mprog_numero from MPROGRAMAPRODUCCION "+
					"where (mprog_ano="+dttFecha.Year+" and mprog_mes="+dttFecha.Month+") or "+
					"(mprog_ano="+dttFecha1.Year+" and mprog_mes="+dttFecha1.Month+") or "+
					"(mprog_ano="+dttFecha2.Year+" and mprog_mes="+dttFecha2.Month+") or "+
					"(mprog_ano="+dttFecha3.Year+" and mprog_mes="+dttFecha3.Month+") or "+
					"(mprog_ano="+dttFecha4.Year+" and mprog_mes="+dttFecha4.Month+");");
				bind.PutDatasIntoDropDownList(ddlAlmacen,"SELECT PA.palm_almacen, PA.palm_descripcion FROM PALMACEN PA, MPLANTAS MP WHERE PA.palm_almacen=MP.mpla_codigo;");
				bind.PutDatasIntoDropDownList(ddlVendedor,string.Format(Almacen.VENDEDORESPORALMACEN,ddlAlmacen.SelectedValue));
                bind.PutDatasIntoDropDownList(ddlPrefE, string.Format(Documento.DOCUMENTOSTIPOHECHO, "FP", "IP", ddlAlmacen.SelectedValue));
                if (ddlPrefE.Items.Count > 0)
                    txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO, ddlPrefE.SelectedValue));
                else txtNumFacE.Text = "0";
          
                tbDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

				int mes=DateTime.Now.Month;
				if(mes<6)
					txtAno.Text=DBFunctions.SingleData("SELECT PANO_ANO FROM CVEHICULOS;");
				else
					txtAno.Text=(DateTime.Now.Year+1).ToString();
				
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
			}

			if(ViewState["COLORES"]==null)
			{
				dtColores=new DataTable();
				dtColores.Columns.Add("PCOL_CODIGO",typeof(string));
				dtColores.Columns.Add("PCOL_DESCRIPCION",typeof(string));
				dtColores.Columns.Add("PCOL_CANTIDAD",typeof(int));
				BindColores();
				ViewState["COLORES"]=dtColores;
			}
			else
				dtColores=(DataTable)ViewState["COLORES"];

			if(ViewState["ENSAMBLES"]!=null)
				dtEnsambles=(DataTable)ViewState["ENSAMBLES"];
				
		}


		#region Ajax
		[Ajax.AjaxMethod]
		public string CargarNumero(string prefijo)
		{
			string valor="";
			valor=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijo+"'");
			return valor;
		}
		#endregion

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

		#region Grilla Colores
		protected void DgColoresDelete(Object sender, DataGridCommandEventArgs e)
		{
			dtColores.Rows.Remove(dtColores.Rows[e.Item.ItemIndex]);
			dgrColores.EditItemIndex=-1;
			BindColores();
		}

		protected void DgColoresAddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(e.CommandName == "AddDatasRow")
			{
				DropDownList ddlCol=((DropDownList)e.Item.FindControl("ddlColor"));
				TextBox txtCant=((TextBox)e.Item.FindControl("txtCantidad"));
				int cantidad;
				try{
					cantidad=Convert.ToInt32(txtCant.Text);
					if(cantidad<=0)throw(new Exception());
				}
				catch{
                    Utils.MostrarAlerta(Response, "Cantidad no valida.");
					return;}
				if(dtColores.Rows.Count==0 || dtColores.Select("PCOL_CODIGO='"+ddlCol.SelectedValue+"'").Length==0){
					DataRow drCol=dtColores.NewRow();
					drCol["PCOL_CODIGO"]=ddlCol.SelectedValue;
					drCol["PCOL_DESCRIPCION"]=ddlCol.SelectedItem.Text;
					drCol["PCOL_CANTIDAD"]=cantidad;
					dtColores.Rows.Add(drCol);
					ViewState["COLORES"]=dtColores;
					BindColores();
				}
			}
		}

		protected void DgColoresDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Footer)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList((DropDownList)e.Item.FindControl("ddlColor"),"SELECT pcol_codigo,pcol_descripcion from pcolor where PCOL_ACTIVO = 'SI';");
			}
		}
		
		private void BindColores()
		{
			dgrColores.DataSource=dtColores;
			dgrColores.DataBind();
			btnSeleccionar.Enabled=(dtColores.Rows.Count>0);
		}
		#endregion
		
		#region Grilla Ensambles
		protected void DgEnsambleDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				DropDownList ddlCol=((DropDownList)e.Item.FindControl("ddlColorE"));
				ddlCol.DataValueField="PCOL_CODIGO";
				ddlCol.DataTextField="PCOL_DESCRIPCION";
				ddlCol.DataSource=dtColores;
				ddlCol.DataBind();
				ddlCol.SelectedIndex=ddlCol.Items.IndexOf(ddlCol.Items.FindByValue(dtEnsambles.Rows[e.Item.ItemIndex]["PCOL_CODIGO"].ToString()));
			}
		}
		protected void DgEnsambleEdit(object sender, DataGridCommandEventArgs e){
			dgEnsambles.EditItemIndex=e.Item.ItemIndex;
			BindEnsambles();
		}
		protected void DgEnsambleUpdate(object sender, DataGridCommandEventArgs e){
			dtEnsambles.Rows[e.Item.ItemIndex]["PCOL_CODIGO"]=((DropDownList)e.Item.FindControl("ddlColorE")).SelectedValue;
			dtEnsambles.Rows[e.Item.ItemIndex]["PCOL_DESCRIPCION"]=((DropDownList)e.Item.FindControl("ddlColorE")).SelectedItem.Text;
			ViewState["ENSAMBLES"]=dtEnsambles;
			dgEnsambles.EditItemIndex=-1;
			BindEnsambles();
		}
		protected void DgEnsambleCancel(object sender, DataGridCommandEventArgs e){
			dgEnsambles.EditItemIndex=-1;
			BindEnsambles();
		}
		private void CrearTablaEnsambles()
		{
			dtEnsambles=new DataTable();
			dtEnsambles.Columns.Add("PCAT_CODIGO",typeof(string));
			dtEnsambles.Columns.Add("PCOL_CODIGO",typeof(string));
			dtEnsambles.Columns.Add("PCOL_DESCRIPCION",typeof(string));
			dtEnsambles.Columns.Add("MCAT_VIN",typeof(string));
		}

		private void BindEnsambles()
		{
			dgEnsambles.DataSource=dtEnsambles;
			dgEnsambles.DataBind();
		}

		#endregion

		#region Botones
		public void Ejecutar(object sender, System.EventArgs e)
		{

			uint ndrefE=0;//Numero Factura - Numero Factura Entrada
			DateTime fechaProceso;
			string vend,codigoAlmacen,ccos,carg,prefE,ano_cinv,tipoE;
			UInt64 numPre;
			ArrayList sqlStrings=new ArrayList();
			string nit,ciudad;
			int anoV;
			UInt32 cVIN;

			#region Validaciones
			try{
				anoV=Convert.ToInt16(txtAno.Text);
				if(Math.Abs(DateTime.Now.Year-anoV)>5)
					throw(new Exception());}
			catch{
                Utils.MostrarAlerta(Response, "Año no válido.");
				return;}
			if(dtEnsambles.Rows.Count==0){
                Utils.MostrarAlerta(Response, "No hay ensambles seleccionados.");
				return;
			}
			try{ndrefE=Convert.ToUInt32(txtNumFacE.Text);}
            catch { Utils.MostrarAlerta(Response, "El número de entrada de almacén no es valido!"); return; }
            Utils.MostrarAlerta(Response, "El número de entrada de almacén no es valido!");
			try{
				fechaProceso=Convert.ToDateTime(tbDate.Text);}
			catch{
                Utils.MostrarAlerta(Response, "Fecha de proceso no es valido!");
				return;}
			#endregion Validaciones

			//Aqui Iniciamos el proceso como tal, habiendo superado con exito el proceso de validacion
			nit=DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA;");
			ciudad=DBFunctions.SingleData("SELECT CEMP_CIUDAD FROM CEMPRESA;");
			vend = ddlVendedor.SelectedValue;//Codigo del Vendedor
			codigoAlmacen = ddlAlmacen.SelectedValue;
			ccos = DBFunctions.SingleData("SELECT pcen_centinv FROM palmacen WHERE palm_almacen='"+codigoAlmacen+"'");
			carg = DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vend+"'");//Cargo???????
			prefE = "";//Prefijo Documento Interno (PDOCUMENTO)
			numPre = 0;//Numero de Documento Interno (PDOCUMENTO)
			ano_cinv = ConfiguracionInventario.Ano;
			tipoE=DBFunctions.SingleData("SELECT MORD_TIPO FROM MORDENPRODUCCION WHERE PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND MORD_NUMEORDE="+ddlNumOrden.SelectedValue+";");
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
				ddlPrefOrden.SelectedValue,nit,ddlAlmacen.SelectedValue,"F",numeroFacturaProveedor,
				Convert.ToUInt64(ddlNumOrden.SelectedValue),"V",fechaProceso,
				fechaProceso.AddDays(0),
				Convert.ToDateTime(null),fechaProceso,0,
				0,0,0,0,txtObs.Text,
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
			dtRet.Rows[0][5]=0;
			dtRet.Rows[0][6]=0;
			dtRet.Rows[0][7]=0;
			dtRet.Rows[0][8]="";
			dtRet.Rows[0][9]=0;
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

			#region Vehiculos, catalogos, detalles ordenes, pcatalogovehiculo
			bool vehiculos=false;
			for(int n=0;n<dtEnsambles.Rows.Count;n++)
			{
				if(((CheckBox)dgEnsambles.Items[n].FindControl("chkUsarE")).Checked)
				{
					cVIN=Convert.ToUInt32(dtEnsambles.Rows[n]["MCAT_VIN"].ToString().Substring(12));
					
					//Insertar mcatalogovehiculo
					sqlStrings.Add("INSERT INTO MCATALOGOVEHICULO VALUES ("+
						"'"+dtEnsambles.Rows[n]["PCAT_CODIGO"].ToString()+"',"+//Catalogo
						"'"+dtEnsambles.Rows[n]["MCAT_VIN"].ToString()+"',"+//VIN
						"'"+dtEnsambles.Rows[n]["MCAT_VIN"].ToString().Substring(12)+"',"+//placa
						"'"+dtEnsambles.Rows[n]["MCAT_VIN"].ToString()+"',"+//Motor
						"'"+nit+"',"+//NIT
						"NULL,"+
						"'"+dtEnsambles.Rows[n]["MCAT_VIN"].ToString()+"',"+//Chasis
						"'"+dtEnsambles.Rows[n]["PCOL_CODIGO"].ToString()+"',"+//Color
                        anoV + ",'A',NULL,NULL,'" + tbDate.Text + "',0,NULL,0,0,NULL,NULL,'" + tbDate.Text + "',null)");

					//Insertar mvehiculo
					sqlStrings.Add("INSERT INTO MVEHICULO VALUES ("+
						"DEFAULT,"+//Inventario
						"NULL,"+//Catalogo
						"'"+dtEnsambles.Rows[n]["MCAT_VIN"].ToString()+"',"+//VIN
						"1,"+//Estado
						"NULL,NULL,"+//Pedido
						"'"+nit+"',"+//NIT
						"0,"+//No recepcion
						"'"+tbDate.Text+"',"+//Fecha Recep
						"'"+tbDate.Text+"',"+//Fecha Disponb.
						"0,"+//Kilometraje
						"'N',"+//Nuevo
						"'"+dtEnsambles.Rows[n]["MCAT_VIN"].ToString()+"',"+//Manifiesto
						"'"+tbDate.Text+"',"+//Fecha Manifiesto
						"'"+ciudad+"',"+//Aduana
						"NULL,"+//Version
						"'"+dtEnsambles.Rows[n]["MCAT_VIN"].ToString()+"',"+//No Levante
						"NULL,NULL,"+//Vr Gastos, Inflacion
						"'P',"+//Tipo de compra
						"NULL,NULL,"+//Orden de pago
						"0,"+//Valor compra a proveedor
						"NULL,"+//Fecha entrega
						"'"+nit+"',"+//NIT Proveedor
						"NULL,NULL"+//Prenda,IVA
						");");

					//Actualizar pcatalogovehiculo
					sqlStrings.Add("update pcatalogovehiculo "+
						"set pcat_consecutivo="+cVIN+" "+
						"WHERE PCAT_CODIGO='"+dtEnsambles.Rows[n]["PCAT_CODIGO"].ToString()+"' AND pcat_consecutivo<"+cVIN+";");

					//Actualizar dordenproduccion
					sqlStrings.Add("UPDATE DORDENPRODUCCION SET "+
						"DORD_CANTENTR=DORD_CANTENTR+1 "+
						"WHERE PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND "+
						"MORD_NUMEORDE="+ddlNumOrden.SelectedValue+" AND "+
						"PCAT_CODIGO='"+dtEnsambles.Rows[n]["PCAT_CODIGO"]+"';");
					vehiculos=true;
				}
			}
			if(!vehiculos)
			{
                Utils.MostrarAlerta(Response, "No seleccionó ningún elemento.");
				return;
			}
			#endregion

			#region Orden Produccion
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

			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect(""+indexPage+"?process=Produccion.EntradasEnsambles&path="+Request.QueryString["path"]+"&pref="+prefijoFacturaProveedor+"&num="+numeroFacturaProveedor);
			else
				lbInfo.Text += "<br>Error : Detalles <br>"+DBFunctions.exceptions;
		}

		public void Seleccionar(object sender, System.EventArgs e)
		{
			#region Validaciones
			if(dtColores.Rows.Count==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar los colores.");
				return;
			}
			//Validaciones
			try{Convert.ToDateTime(tbDate.Text);}
            catch { Utils.MostrarAlerta(Response, "Fecha no válida!"); return; }
            Utils.MostrarAlerta(Response, "Fecha no válida!");
			try{Convert.ToUInt32(txtNumFacE.Text);}
            catch { Utils.MostrarAlerta(Response, "El número de entrada de almacén no es valido!"); return; }
          
			
			txtAno.Enabled=ddlLote.Enabled=tbDate.Enabled=ddlNumOrden.Enabled=ddlPrefOrden.Enabled=txtNumFacE.Enabled=ddlPrefE.Enabled=ddlVendedor.Enabled=ddlAlmacen.Enabled=false;
			btnSeleccionar.Visible=dgrColores.Visible=false;
			#endregion Validaciones

			#region Cargar Ensambles
			DataSet dsEnsamble = new DataSet();
			int numI;
			UInt32 numV;
			string vinBasico;

			DBFunctions.Request(dsEnsamble,IncludeSchema.NO,
				"SELECT * from DORDENPRODUCCION "+
				"WHERE PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND "+
				"MORD_NUMEORDE="+ddlNumOrden.SelectedValue+";");
			CrearTablaEnsambles();
			DataRow drItem;
			//int contColor=0;
			int indColor=0;
			int cantColor=1;

			for(int n=0;n<dsEnsamble.Tables[0].Rows.Count;n++)
			{
				numI=Convert.ToInt32(dsEnsamble.Tables[0].Rows[n]["DORD_CANTXPROD"])-Convert.ToInt32(dsEnsamble.Tables[0].Rows[n]["DORD_CANTENTR"]);
				vinBasico=DBFunctions.SingleData("SELECT PCAT_VINBASICO FROM PCATALOGOVEHICULO WHERE PCAT_CODIGO='"+dsEnsamble.Tables[0].Rows[n]["PCAT_CODIGO"]+"';");
				numV=Convert.ToUInt32(DBFunctions.SingleData("SELECT PCAT_CONSECUTIVO FROM PCATALOGOVEHICULO WHERE PCAT_CODIGO='"+dsEnsamble.Tables[0].Rows[n]["PCAT_CODIGO"]+"';"));
				if(vinBasico.Length!=12)
				{
                    Utils.MostrarAlerta(Response, "El VIN básico del catálogo " + dsEnsamble.Tables[0].Rows[n]["PCAT_CODIGO"] + " está mal formado, debe tener 12 caracteres.");
					return;
				}
				for(int i=0;i<numI;i++)
				{
					numV++;
					drItem=dtEnsambles.NewRow();
					drItem["PCAT_CODIGO"]=dsEnsamble.Tables[0].Rows[n]["PCAT_CODIGO"];
					drItem["PCOL_CODIGO"]=dtColores.Rows[indColor]["PCOL_CODIGO"].ToString();
					drItem["PCOL_DESCRIPCION"]=dtColores.Rows[indColor]["PCOL_DESCRIPCION"].ToString();
					drItem["MCAT_VIN"]=Tools.VINs.GenerarVIN(vinBasico+numV.ToString("00000"));
					dtEnsambles.Rows.Add(drItem);
					//Siguiente color?
					if(cantColor>=Convert.ToInt32(dtColores.Rows[indColor]["PCOL_CANTIDAD"])){
						if(indColor<dtColores.Rows.Count-1)
						{
							indColor++;
							cantColor=1;
						}
						else indColor=dtColores.Rows.Count-1;
					}
					else cantColor++;
					/*contColor++;
					if(contColor>=dtColores.Rows.Count)
						contColor=0;*/
				}
			}
			DataView dvEnsambles=dtEnsambles.DefaultView;
			dvEnsambles.Sort="PCOL_CODIGO,MCAT_VIN";
			dtEnsambles=CreateTable(dvEnsambles); 
			ViewState["ENSAMBLES"]=dtEnsambles;
			plcVehiculos.Visible=true;
			BindEnsambles();
			#endregion Cargar Ensambles
		}

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

		#endregion
		
		#region Dropdownlists
		protected void ddlPrefE_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO,ddlPrefE.SelectedValue));
		}
		protected void ddlPrefOrden_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlNumOrden,"SELECT mord_numeorde from mordenproduccion where pdoc_codigo='"+ddlPrefOrden.SelectedValue+"' and test_estado='A' and mord_tipo='E';");
		}
		protected void ddlAlmacen_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlVendedor,string.Format(Almacen.VENDEDORESPORALMACEN,ddlAlmacen.SelectedValue));
            bind.PutDatasIntoDropDownList(ddlPrefE, string.Format(Documento.DOCUMENTOSTIPOHECHO, "FP", "IP", ddlAlmacen.SelectedValue));
            if (ddlPrefE.Items.Count > 0)
                txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO, ddlPrefE.SelectedValue));
            else txtNumFacE.Text = "0";
          
		}
		#endregion
	}
}
