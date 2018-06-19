using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
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
using AMS.Documentos;
using AMS.DB;
using AMS.Contabilidad;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class Facturacion : System.Web.UI.UserControl
	{

        protected int numDecimales = 0;
		protected Label lblTipoOrden,lblNumOrden,lblTipoPedido,lblPlaca;
		protected TextBox txtNumPed;
		protected DataTable dtInserts;
		protected DataSet ds;
		protected DataView dvInserts;
		protected ArrayList types = new ArrayList();
		protected ArrayList lbFields = new ArrayList();
		protected string Tipo, kit, AnoA, almacen;
        ProceHecEco contaOnline = new ProceHecEco();

        private string tipoPedido;
			
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Tipo = Request.QueryString["actor"];//carga el tipo de de registro a realizar
			//this.ClearChildViewState();
			LoadDataColumns();
			string nLis1 = Request.QueryString["nped"]; //Numero de lista de empaque
            string[] nLis = nLis1.ToString().Split('-');
            kit = Request.QueryString["kitPed"]; //Numero de lista de empaque
            AnoA = Request.QueryString["AnoA"];
            almacen = Request.QueryString["almacen"];
            if (nLis1.Length==0)//Si no es valido el numero de lista de empaque que no haga nada
				return;

            //string centavos = ConfigurationManager.AppSettings["ManejoCentavos"];
            //if (Utils.EsEntero(centavos))
            //    numDecimales = Convert.ToInt32(centavos);

            try { numDecimales = Convert.ToInt16(DBFunctions.SingleData("select cinv_numedeci from cinventario")); }
            catch (Exception er) { numDecimales = 2; }

            if (!IsPostBack)
			{
                //Impimir trasnferencia:
                if (Request.QueryString["prefPed"] != null)
                {
                    FormatosDocumentos formatoFactura = new FormatosDocumentos();
                    try
                    {
                        formatoFactura.Prefijo = Request.QueryString["prefPed"];
                        formatoFactura.Numero = Convert.ToInt32(Request.QueryString["numPed"]);
                        formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefPed"] + "'");
                        if (formatoFactura.Codigo != string.Empty)
                        {
                            if (formatoFactura.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                        }
                    }
                    catch
                    {
                        lbInfo.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                    }
                }

				Session.Clear();
				dgItems.EditItemIndex = -1;
				DatasToControls bind = new DatasToControls();
				hdCargoTrans.Value = DBFunctions.SingleData("SELECT DISTINCT MPD.tcar_cargo FROM   dbxschema.mpedidotransferencia MPD INNER JOIN dbxschema.mpedidoitem MPI ON MPD.pped_codigo=MPI.pped_codigo AND MPD.mped_numero=MPI.mped_numepedi INNER JOIN dbxschema.dlistaempaque DLIS ON MPI.pped_codigo=DLIS.pped_codigo AND MPI.mped_numepedi=DLIS.mped_numepedi WHERE DLIS.mlis_numero="+nLis[0]+"");
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
				bind.PutDatasIntoDropDownList(ddlPIVA,"SELECT piva_porciva, piva_decreto FROM piva order by 2");//Porcentaje de IVA
				string tp = DBFunctions.SingleData("SELECT t1.tped_codigo from PPEDIDO as t1, DLISTAEMPAQUE t2 WHERE t1.pped_codigo=t2.pped_codigo and t2.mlis_numero="+nLis[0]);//Tipo de pedido
                ViewState["tipoPedido"] = tipoPedido = tp;
                PublicarInformacionVendedor(nLis);

                if(tp == "T")
				{
					//Si es transferencia de taller se trae un documento tipo transferencia de taller y adicionalmente se bloquean los campos de texto de fletes, ni iva de fletes
                    
                    Utils.llenarPrefijos(Response, ref ddlCodDoc, "%", ddlAlmacen.SelectedValue, "TT");
					txtFlet.Enabled = ddlPIVA.Enabled = false;
				}
                else if (tp == "E")
                {
                    //Si es CONSUMO INTERNO  se trae un documento tipo CONSUMO INTERNO E  y adicionalmente se bloquean los campos de texto de fletes, ni iva de fletes
                    Utils.llenarPrefijos(Response, ref ddlCodDoc, "%", ddlAlmacen.SelectedValue, "CI");
                    txtFlet.Enabled = ddlPIVA.Enabled = false;
                }
                else
                {
                    //Si no es una transferencia se traen documentos de tipo factura cliente
                    //   falta filtrar por almacen
                    Utils.llenarPrefijos(Response, ref ddlCodDoc, "IC", ddlAlmacen.SelectedValue, "FC");
                    txtFlet.Enabled = ddlPIVA.Enabled = true;
                }


				IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
				tbDate.Text = DateTime.Now.GetDateTimeFormats()[6];
				//tbDate.Text = DateTime.Now;
				ds = new DataSet();
                for (int i = 0; i < nLis.Length-1; i++) {  
                DBFunctions.Request(ds,IncludeSchema.NO,"SELECT t1.mnit_nit,t1.mnit_nombres concat ' ' concat t1.mnit_apellidos, t2.palm_almacen from MNIT as t1,MLISTAEMPAQUE as t2 WHERE t1.MNIT_NIT=t2.MNIT_NIT AND t2.MLIS_NUMERO="+nLis[i]+"");
				//Observacion (PRIMERA ENCONTRADA EN LOS PEDIDOS DE LA LISTA DE EMPAQUE)
				txtObs.Text +=DBFunctions.SingleData(
					@"SELECT MP.MPED_OBSERVACION  
		  			    FROM MPEDIDOITEM MP, DLISTAEMPAQUE DL  
					   WHERE DL.MLIS_NUMERO="+nLis[i]+@" AND DL.PPED_CODIGO=MP.PPED_CODIGO AND DL.MPED_NUMEPEDI=MP.MPED_NUMEPEDI  
					FETCH FIRST 1 ROWS ONLY;"
				);
            }
            try
				{
					txtNIT.Text=ds.Tables[0].Rows[0][0].ToString();
					txtAlm.Text=ds.Tables[0].Rows[0][2].ToString();
					txtNITa.Text=ds.Tables[0].Rows[0][1].ToString();
				}
				catch{}
				//Ahora cargamos los dias de plazo que tiene este cliente
				if(DBFunctions.SingleData("SELECT coalesce(mcli_diasplaz,0) FROM mcliente WHERE mnit_nit='"+txtNIT.Text+"'").Length > 0)
					txtDiasP.Text = DBFunctions.SingleData("SELECT coalesce(mcli_diasplaz,0) FROM mcliente WHERE mnit_nit='"+txtNIT.Text+"'");
				else
					txtDiasP.Text = "0";

                //txtFlet.Attributes.Add("onkeyup", "NumericMask(" + txtFlet.ClientID + ");");
				txtFlet.Attributes.Add("onkeyup","CalculoIva("+txtFlet.ClientID+","+ddlPIVA.ClientID+","+tbIvaFlts.ClientID+","+txtTotIF.ClientID+",'"+txtTotal.ClientID+"','"+txtGTot.ClientID+"');");
				ddlPIVA.Attributes.Add("onchange","CambioIva("+txtFlet.ClientID+","+ddlPIVA.ClientID+","+tbIvaFlts.ClientID+","+txtTotIF.ClientID+",'"+txtTotal.ClientID+"','"+txtGTot.ClientID+"');");
			}
			txtNumDoc.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlCodDoc.SelectedValue+"'");
            tipoPedido = ViewState["tipoPedido"].ToString();
			if(Session["dtInsertsF"] == null)
				LlenarTabla();
			else
			{
				dtInserts=(DataTable)Session["dtInsertsF"];
				//BindDatas();
			}
			if(dtInserts.Rows.Count==0)
				LlenarTabla();
		}
				
		public void LlenarTabla() //Función que llena los items a facturar, los que se encuentran almacenados en la tabla de dlistaempaque
		{
			int i;
            String sql = "";
            dtInserts = new DataTable();
			for(i=0;i<lbFields.Count; i++)
				dtInserts.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
			string mLis1 = Request.QueryString["nped"];
            string[] mLis = mLis1.ToString().Split('-');
            for (int j = 0; j < mLis.Length - 1; j++)
            {
                 sql += @"SELECT DBXSCHEMA.EDITARREFERENCIAS (t1.mite_codigo,t3.plin_tipo),  
                     t2.mite_nombre,  
                     t1.dped_cantasig,  
                     t1.dlis_porcdesc,  
                     t1.piva_porciva,  
                     t1.dlis_valopubl,  
                     t1.dlis_valounit,  
                     t2.plin_codigo,  
                     t1.mite_codigo,
                     t1.PPED_CODIGO,
                     MLIS_NUMERO CONCAT '-' CONCAT PPED_CODIGO CONCAT '-' CONCAT MPED_NUMEPEDI, 
                     MPED_NUMEPEDI 
              FROM dlistaempaque AS t1, 
                   mitems AS t2,  
                   plineaitem t3  
              WHERE mlis_numero = " + mLis[j] + @"  
                AND t1.mite_codigo = t2.mite_codigo  
                AND t3.plin_codigo = t2.plin_codigo  
              ORDER BY 1;";
            }
            
			ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO, sql);
			
            int n;
            for (int k = 0; k < mLis.Length - 1; k++)
            {
                for (n = 0; n < ds.Tables[k].Rows.Count; n++)
                {
                    DataRow dr = dtInserts.NewRow();
                    dr[0] = ds.Tables[k].Rows[n][0];//Codigo
                    dr[1] = ds.Tables[k].Rows[n][1];//Nombre
                    double cant = Convert.ToDouble(ds.Tables[k].Rows[n][2]);
                    dr[2] = cant;//CantidadFacturada
                    double pr = Convert.ToDouble(ds.Tables[k].Rows[n][5]);
                    dr[3] = pr;//Precio
                    double iva = Convert.ToDouble(ds.Tables[k].Rows[n][4]);
                    dr[4] = iva;//IVA
                    double desc = Convert.ToDouble(ds.Tables[k].Rows[n][3]);
                    dr[5] = desc;//Descuento
                    double tot = cant * pr;
                    double stot = tot - Math.Round((desc / 100) * tot, numDecimales);//Sub total
                    double totiva = Math.Round(stot * (iva / 100), numDecimales);//total iva
                    tot = tot + totiva; // estaba asi (tot*(iva/100));
                    double totdesc = Math.Round((desc / 100) * tot, numDecimales);
                    tot = tot - totdesc; //estaba asi ((desc/100)*tot);
                    dr[6] = tot;                    //Total
                    dr[7] = totdesc;                //Disponible
                    dr[8] = stot;
                    dr[9] = cant;   //Cantidad inicial
                    dr[10] = totiva;
                    dr[11] = ds.Tables[k].Rows[n][7];
                    dr[12] = ds.Tables[k].Rows[n]["PPED_CODIGO"];
                    dr[13] = ds.Tables[k].Rows[n][10];
                    dr[14] = ds.Tables[k].Rows[n]["MPED_NUMEPEDI"];
                    dtInserts.Rows.Add(dr);
                }
            }
			Session["dtInsertsF"] = dtInserts;		
			BindDatas();
		}
		
		protected void BindDatas()
		{
			//dgItems.EnableViewState = true;
			txtNumItem.Text = dtInserts.Rows.Count.ToString();
			double total = 0, subtotal = 0, iva = 0, totalDescuento = 0, numeroUnidades = 0; 
			double totalf=0,subtotalf=0,ivaf=0,totalDescuentof=0;
			double valorFletes = Convert.ToDouble(txtTotIF.Text.Substring(1));
			double ivaFletes = Convert.ToDouble(tbIvaFlts.Text.Substring(1));
			int n;
			if(dtInserts.Rows.Count>0)
			{
				for(n=0;n<dtInserts.Rows.Count;n++)
				{
					subtotal += (Convert.ToDouble(dtInserts.Rows[n][2]) * Convert.ToDouble(dtInserts.Rows[n][3]));
					subtotalf=(Convert.ToDouble(dtInserts.Rows[n][2]) * Convert.ToDouble(dtInserts.Rows[n][3]));
                    totalDescuento += Math.Round(((Convert.ToDouble(dtInserts.Rows[n][2]) * Convert.ToDouble(dtInserts.Rows[n][3])) * (Convert.ToDouble(dtInserts.Rows[n][5]) / 100)), numDecimales);
					totalDescuentof=((Convert.ToDouble(dtInserts.Rows[n][2]) * Convert.ToDouble(dtInserts.Rows[n][3])) * (Convert.ToDouble(dtInserts.Rows[n][5])/100));
                    iva += Math.Round((((Convert.ToDouble(dtInserts.Rows[n][2]) * Convert.ToDouble(dtInserts.Rows[n][3])) - ((Convert.ToDouble(dtInserts.Rows[n][2]) * Convert.ToDouble(dtInserts.Rows[n][3])) * (Convert.ToDouble(dtInserts.Rows[n][5]) / 100))) * (Convert.ToDouble(dtInserts.Rows[n][4]) / 100)), numDecimales);
					ivaf=(((Convert.ToDouble(dtInserts.Rows[n][2]) * Convert.ToDouble(dtInserts.Rows[n][3])) - ((Convert.ToDouble(dtInserts.Rows[n][2]) * Convert.ToDouble(dtInserts.Rows[n][3])) * (Convert.ToDouble(dtInserts.Rows[n][5])/100))) * (Convert.ToDouble(dtInserts.Rows[n][4])/100));
                    totalDescuentof = Math.Round(totalDescuentof * 1, numDecimales);
                    ivaf = Math.Round(ivaf * 1, numDecimales);
					totalf=subtotalf-totalDescuentof+ivaf;
					dtInserts.Rows[n][6]=totalf;
					numeroUnidades += Convert.ToDouble(dtInserts.Rows[n][2]);
				}
			}
            totalDescuento = Math.Round(totalDescuento * 1, numDecimales);
            iva = Math.Round(iva * 1, numDecimales);
            total = subtotal - totalDescuento + iva;//Convert.ToDouble(dtInserts.Rows[n][6]);
			txtSubTotal.Text = subtotal.ToString("C");
			txtDesc.Text = totalDescuento.ToString("C");
			txtIVA.Text = iva.ToString("C");
			txtTotal.Text = total.ToString("C");
			txtNumItem.Text = dtInserts.Rows.Count.ToString("N");
			txtNumUnid.Text = numeroUnidades.ToString("N");
			txtGTot.Text = (total+valorFletes+ivaFletes).ToString("C");
			
			dvInserts = new DataView(dtInserts);
			dgItems.DataSource = dtInserts;
			dgItems.DataBind();
			DatasToControls.JustificacionGrilla(dgItems,dtInserts);
		}

		protected void LoadDataColumns()
		{
			lbFields.Add("mite_codigo");//0
			types.Add(typeof(string));
			lbFields.Add("mite_nombre");//1
			types.Add(typeof(string));
			lbFields.Add("mite_cantfac");//2
			types.Add(typeof(double));
			lbFields.Add("mite_precio");//3 Precio Final
			types.Add(typeof(double));
			lbFields.Add("mite_iva");//4
			types.Add(typeof(double));
			lbFields.Add("mite_desc");//5
			types.Add(typeof(double));
			lbFields.Add("mite_tot");//6
			types.Add(typeof(double));
			lbFields.Add("mite_totdesc");//7 Total descuento
			types.Add(typeof(double));
			lbFields.Add("mite_subtot");//8 Subtotal, total sin IVA
			types.Add(typeof(double));
			lbFields.Add("mite_cantini");//9 Auxiliar par guardar cantidad especificada en el pedido
			types.Add(typeof(double));
			lbFields.Add("mite_totiva");//10 IVA
			types.Add(typeof(double));
			lbFields.Add("plin_codigo");//11 LINEA
			types.Add(typeof(string));
            lbFields.Add("PPED_CODIGO");//12
            types.Add(typeof(string));
            lbFields.Add("MLIS_NUMELIST");//13
            types.Add(typeof(string));
            lbFields.Add("MPED_NUMEPEDI");//14
            types.Add(typeof(string));
        }

		protected void dgItems_ItemCommand(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "ResetRows")
			{
				dgItems.EditItemIndex=-1;
				LlenarTabla();
			}
			else if(e.CommandName=="Actualizar")
			{
				double cant=Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
				double desc=Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
				if(cant>=0&&cant<=Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][9]))
					dtInserts.Rows[dgItems.EditItemIndex][2]=cant;
				else
                    Utils.MostrarAlerta(Response, "No puede dar valores negativos o mayores a los registrados en el pedido !");
				if(desc>=0 && desc<=100)
					dtInserts.Rows[dgItems.EditItemIndex][5]=desc;
				else
                    Utils.MostrarAlerta(Response, "Porcentaje de descuento erroneo !");
				dgItems.EditItemIndex=-1;
				BindDatas();
			}
		}
    	
		public void DgItems_Update(Object sender, DataGridCommandEventArgs e)
		{//Este selecciona la fila
			double cant=Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[0]).Text);
			double porc=Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[0]).Text);
			if(cant>=0&&cant<=Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][9]))
				dtInserts.Rows[dgItems.EditItemIndex][3]=cant;
			else
                Utils.MostrarAlerta(Response, "No puede dar valores negativos o mayores a los registrados en el pedido !");
			dgItems.EditItemIndex=-1;
			BindDatas();
		} 
	    
		protected void NewAjust(Object Sender, EventArgs E)
		{
			if(!VerificarValoresGrillaFacturacion())
			{
				BindDatas();
				return;
			}
			//QUEDA
			int diasP = -1;
			try{diasP = Convert.ToInt16(txtDiasP.Text);}
			catch{};
			if(diasP<0||diasP>180)
			{
				BindDatas();
                Utils.MostrarAlerta(Response, "El numero de días de plazo dado NO es válido, debe estar entre 0 y 180!");
				return;
			}
            if(ddlCodDoc.SelectedIndex == 0 && ddlCodDoc.Items.Count > 1)
            {
                BindDatas();
                Utils.MostrarAlerta(Response, "Por favor seleccione el Documento para realizar el proceso !!!");
                return;
            }

            string nLis1 = Request.QueryString["nped"];
            string[]nLis = nLis1.ToString().Split('-');//Numero de Lista de empaque
			string ano_cont = DBFunctions.SingleData("SELECT pano_ano from cinventario");
			int ano=Convert.ToInt16(ano_cont);
			int mes=Convert.ToInt16(DBFunctions.SingleData("select PMES_MES from CINVENTARIO;"));
			if(ano!= Convert.ToDateTime(tbDate.Text).Year || mes!= Convert.ToDateTime(tbDate.Text).Month)
			{
                Utils.MostrarAlerta(Response, "Fecha NO vigente !!");
                if (HttpContext.Current.User.Identity.Name.ToLower().ToString() == "abarrios")  // EUROTECK temporalmente permite facturar con otras fechas
                { }
                else
				return;
			}
            if (ddlCodDoc.SelectedItem.Value=="Seleccione..")
            {
                Utils.MostrarAlerta(Response, "Usted NO ha configurado un documento del tipo FC en esta sede para este proceso..!");
                return;
            }
            btnAjus.Enabled = false; // apaga el boton de facturar

			PedidoFactura pedfac = new PedidoFactura(txtNIT.Text, Convert.ToDateTime(tbDate.Text), txtObs.Text,
				//											1				2					3
				diasP, nLis1, ddlVendedor.SelectedItem.Value,ddlCodDoc.SelectedItem.Value,Convert.ToUInt32(txtNumDoc.Text),
				//4    5               6                           7                           8
				Convert.ToDouble(txtTotIF.Text.Substring(1)),Convert.ToDouble(tbIvaFlts.Text.Substring(1)),
				//9                                                                              10
				Convert.ToDouble(txtSubTotal.Text.Substring(1)) - Convert.ToDouble(txtDesc.Text.Substring(1)),
				//11
				Convert.ToDouble(txtTotal.Text.Substring(1)), // 12
                kit,  // 13
                AnoA, //14
                almacen // 15
                );
			
			if(hdCargoTrans.Value != "")
				pedfac.CargoOrden = hdCargoTrans.Value;
			int n;
			for(n=0;n<dtInserts.Rows.Count;n++)
			{
				string codI = "";
				Referencias.Guardar(dtInserts.Rows[n][0].ToString(),ref codI,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtInserts.Rows[n][11].ToString()+"'"));
				pedfac.InsertaFila(codI,Convert.ToDouble(dtInserts.Rows[n][2]),
					//1
					Convert.ToDouble(dtInserts.Rows[n][3]),Convert.ToDouble(dtInserts.Rows[n][4]),
					//2											3
					Convert.ToDouble(dtInserts.Rows[n][5]),Convert.ToDouble(dtInserts.Rows[n][9]),dtInserts.Rows[n]["PPED_CODIGO"].ToString(),dtInserts.Rows[n]["MPED_NUMEPEDI"].ToString());
            //4											5                                                   6                                       7
        }

            // causacion de retenciones en la venta
             Retencion retencion = new Retencion(
                                            txtNIT.Text,

                                            ddlCodDoc.SelectedItem.Value ,    // PrefijoFactura,
                                            Convert.ToInt32(txtNumDoc.Text),  // NumeroFactura,
                                            Convert.ToDouble(txtSubTotal.Text.Substring(1)) - Convert.ToDouble(txtDesc.Text.Substring(1)), //ValorFactura,
                                            Convert.ToDouble(txtIVA.Text.Substring(1)),                      //ValorIva,
                                            "R", true);

			if(pedfac.RealizarFac(true))
			{
                if (retencion.Guardar_Retenciones(true))
				{
				    string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                    string indexAjaxPage = ConfigurationManager.AppSettings["MainAjaxPage"];
				    Session.Clear();

                    contaOnline.contabilizarOnline(ddlCodDoc.SelectedItem.Value, Convert.ToInt32(txtNumDoc.Text), DateTime.Now, ddlAlmacen.SelectedValue);
                
                    if(Request.QueryString["orig"]=="Inventarios.ListasEmpaque")
					    Response.Redirect(indexPage+"?process=Inventarios.ListasEmpaque&actor=C&subprocess=Fact&prefF="+pedfac.Coddocumentof+"&numF="+pedfac.Numdocumentof+"&factGen=1");
					    //Response.Redirect("" + indexPage + "?process=Inventarios.VistaImpresion&prefFact="+pedfac.Coddocumentof+"&numFact="+pedfac.Numdocumentof+"&cliente="+Tipo+"&orig="+Request.QueryString["orig"]+"");
				    else if(Request.QueryString["orig"]=="Inventarios.CrearPedido")
                            Response.Redirect(indexAjaxPage + "?process=Inventarios.CrearPedido&actor=C&subprocess=Fact&prefF=" + pedfac.Coddocumentof + "&numF=" + pedfac.Numdocumentof + "");
				    //Response.Redirect(indexPage+"?process=Inventarios.ListasEmpaque&actor=C&subprocess=Fact");
				    //lbInfo.Text +="<br>BIEN :" +pedfac.ProcessMsg;
                }
                else
                    lbInfo.Text += retencion.Mensajes;
			}
			else
				lbInfo.Text +="<br>ERROR :" +pedfac.ProcessMsg;
		}
		
		public void CargaArchivo()
		{
			
		}
		
		protected void ChangeDate(Object Sender, EventArgs E)
		{
			tbDate.Text = Convert.ToDateTime(tbDate.Text).GetDateTimeFormats()[6];
		}
		
		protected void PublicarInformacionVendedor(string [] numLista)
		{
			ArrayList arrCodVend = new ArrayList();
			//Debemos traer los pedidos relacionados con esta lista de empaque
			DataSet da = new DataSet(); 
            DBFunctions.Request(da,IncludeSchema.NO,"SELECT DISTINCT pped_codigo, mped_numepedi FROM dlistaempaque WHERE mlis_numero="+numLista[0]);
			//Traemos el codigo del almacen relacionado con la lista de empaque
			if(da.Tables[0].Rows.Count>0)
			{
				string codAlmacen = DBFunctions.SingleData("SELECT palm_almacen FROM mpedidoitem WHERE pped_codigo='"+da.Tables[0].Rows[0][0].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[0][1].ToString()+"");
				if(codAlmacen != "")
				{
                    DatasToControls.EstablecerDefectoDropDownList(ddlAlmacen, DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codAlmacen + "'"));

                    if (tipoPedido == "T")
                        Utils.llenarPrefijos(Response, ref ddlCodDoc, "%", ddlAlmacen.SelectedValue, "TT");
                    else if (tipoPedido == "E")
                        Utils.llenarPrefijos(Response, ref ddlCodDoc, "%", ddlAlmacen.SelectedValue, "CI");
                    else
                        Utils.llenarPrefijos(Response, ref ddlCodDoc, "IC", ddlAlmacen.SelectedValue, "FC");
                    
                    //  hECTOR VALIDAR QUE EL ALMACEN SEA IGUAL AL SELECCIONADO DE LA LISTA INICIAL EN CASO CONTRARIO RECHAZAR
					for(int i=0;i<da.Tables[0].Rows.Count;i++)
					{
						//Traemos el codigo del vendedor relacionado con el pedido
						string codVendedor = DBFunctions.SingleData("SELECT pven_codigo FROM mpedidoitem WHERE pped_codigo='"+da.Tables[0].Rows[0][0].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[0][1].ToString()+"");
						if(arrCodVend.BinarySearch(codVendedor) < 0)
						{
							arrCodVend.Add(codVendedor);
							ddlVendedor.Items.Add(new ListItem(DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+codVendedor+"'"),codVendedor));
						}
					}
					if(arrCodVend.Count == 1)
						ddlVendedor.Enabled = false;
				}
				else
				{
					DatasToControls bind = new DatasToControls();
		// Ayco 	bind.PutDatasIntoDropDownList(ddlVendedor,"SELECT pven_codigo, pven_nombre FROM pvendedor WHERE palm_almacen='"+ddlAlmacen.SelectedValue+"'");
                    bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT pven_codigo, pven_nombre FROM pvendedor WHERE TVEND_CODIGO IN ('TT','VM') AND tvig_vigencia = 'V' ORDER BY pven_nombre");
				}
			}
		}
		
		
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

		protected void btnAct_Click(object sender, System.EventArgs e)
		{
			/*double cant=Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
			double desc=Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
			if(cant>=0&&cant<=Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][9]))
				dtInserts.Rows[dgItems.EditItemIndex][2]=cant;
			else
				Response.Write("<script language='javascript'>alert('No puede dar valores negativos o mayores a los registrados en el pedido!');</script>");
			if(desc>=0 && desc<=100)
				dtInserts.Rows[dgItems.EditItemIndex][5]=desc;
			else
				Response.Write("<script language='javascript'>alert('Porcentaje de descuento erroneo!');</script>");
			dgItems.EditItemIndex=-1;
			BindDatas();*/
			VerificarValoresGrillaFacturacion();
			BindDatas();
		}

		private bool VerificarValoresGrillaFacturacion()
		{
			bool exito = true;
			double cant=-1,desc=-1;
			string ano_cont = DBFunctions.SingleData("SELECT pano_ano from cinventario");
			for(int i=0;i<dtInserts.Rows.Count;i++)
			{
				try{cant=Convert.ToDouble(((TextBox)dgItems.Items[i].Cells[3].FindControl("tbcantfac")).Text);}
                catch
                {Utils.MostrarAlerta(Response, "Digite un número en la cantidad");return false;}
				try{desc=Convert.ToDouble(((TextBox)dgItems.Items[i].Cells[3].FindControl("tbdesc")).Text);}
                catch
                {Utils.MostrarAlerta(Response, "Digite un número en el porcentaje de descuento");return false;}
				if(cant>=0&&cant<=Convert.ToDouble(dtInserts.Rows[i][9]))
					dtInserts.Rows[i][2]=cant;
				else
				{
                    Utils.MostrarAlerta(Response, "Referencia: " + dtInserts.Rows[i][0].ToString() + " No puede dar valores de cantidad negativos o mayores a los registrados en el pedido! Se toma la que viene por defecto");
					exito = false;
					break;
				}
				if(desc>=0 && desc<=100)
					dtInserts.Rows[i][5]=desc;
				else
				{
                    Utils.MostrarAlerta(Response, "Referencia: " + dtInserts.Rows[i][0].ToString() + " Porcentaje de descuento erroneo. Se toma el que viene por defecto!");
					exito = false;
					break;
				}
                /* 
				double disAlmacen=0,disTotal=0,disponible=0;
				try
				{
					disAlmacen=Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_CANTACTUAL FROM MSALDOITEMALMACEN WHERE PANO_ANO="+ano_cont+" AND MITE_CODIGO='"+ds.Tables[0].Rows[i][8].ToString()+"' AND PALM_ALMACEN='"+ddlAlmacen.SelectedValue+"';"));
					disTotal  =Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_CANTACTUAL FROM MSALDOITEM        WHERE PANO_ANO="+ano_cont+" AND MITE_CODIGO='"+ds.Tables[0].Rows[i][8].ToString()+"';"));
					if(disAlmacen<cant||disTotal<cant)
					{
						disponible=(disAlmacen<disTotal)?disAlmacen:disTotal;
						throw(new Exception());
					}
				}
				catch
				{
					dtInserts.Rows[i][2]=disponible;
					Response.Write("<script language='javascript'>alert('Referencia: "+ds.Tables[0].Rows[i][8].ToString()+" La cantidad disponible es menor a la facturada, se asignó la cantidad disponible actual, Almacen: "+disAlmacen+" Total: "+disTotal+"');</script>");
					exito = false;
					break;
				}
				*/ 
			}
			return exito;
		}
		
		protected void btnRec_Click(object sender, System.EventArgs e)
		{
			LlenarTabla();
		}

        protected void ddlAlmacen_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (tipoPedido == "T")
                Utils.llenarPrefijos(Response, ref ddlCodDoc, "%", ddlAlmacen.SelectedValue, "TT");
            else if (tipoPedido == "E")
                Utils.llenarPrefijos(Response, ref ddlCodDoc, "%", ddlAlmacen.SelectedValue, "CI");
            else
                Utils.llenarPrefijos(Response, ref ddlCodDoc, "IC", ddlAlmacen.SelectedValue, "FC");
        }

        protected void ddlCodDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNumDoc.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + ddlCodDoc.SelectedValue + "'");
        }
	}
}
