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
using AMS.DBManager;
using AMS.Documentos;
using AMS.Forms;
using System.Configuration;
using AMS.Contabilidad;
using AMS.Tools;
using System.Globalization;

namespace AMS.Finanzas.Cartera
{
	public partial class CausacionFacturaAdministrativa : System.Web.UI.UserControl
	{
		#region Variables

		protected DataTable tablaActivosFijos,tablaDiferidos,tablaDetalles,tablaRtns,tablaNC,dtIva, dtCajaMenor;
		protected Panel pnlValoresP;
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
		protected ArrayList inserts=new ArrayList();
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoRecibo=new FormatosDocumentos();
        ProceHecEco contaOnline = new ProceHecEco();
	
		#endregion
		
		#region Metodos

		protected void Cargar_Tabla_Activos()
		{
			tablaActivosFijos=new DataTable();
			tablaActivosFijos.Columns.Add(new DataColumn("CODIGOGASTO",typeof(string)));
			tablaActivosFijos.Columns.Add(new DataColumn("CODIGOACTIVO",typeof(string)));
			tablaActivosFijos.Columns.Add(new DataColumn("NOMBREACTIVO",typeof(string)));
			tablaActivosFijos.Columns.Add(new DataColumn("VALORACTIVO",typeof(string)));
		}
		
		protected void Cargar_Tabla_Diferidos()
		{
			tablaDiferidos=new DataTable();
			tablaDiferidos.Columns.Add(new DataColumn("CODIGOGASTO",typeof(string)));
			tablaDiferidos.Columns.Add(new DataColumn("CODIGODIF",typeof(string)));
			tablaDiferidos.Columns.Add(new DataColumn("NOMBREDIF",typeof(string)));
			tablaDiferidos.Columns.Add(new DataColumn("VALORDIF",typeof(string)));
		}
		
		protected void Cargar_Tabla_Detalles()
		{
			tablaDetalles=new DataTable();
			tablaDetalles.Columns.Add(new DataColumn("CODIGOGASTO",typeof(string)));
			tablaDetalles.Columns.Add(new DataColumn("DETALLE",typeof(string)));
			tablaDetalles.Columns.Add(new DataColumn("DOCUREFE",typeof(string)));
			tablaDetalles.Columns.Add(new DataColumn("VALORDETALLE",typeof(string)));
		}
		
		protected void Cargar_Tabla_Rtns()
		{
			tablaRtns=new DataTable();
			tablaRtns.Columns.Add(new DataColumn("CODRET", typeof(string)));
			tablaRtns.Columns.Add(new DataColumn("PORCRET", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALOR", typeof(double)));
            tablaRtns.Columns.Add(new DataColumn("TIPORETE", typeof(string)));
		}
		
		public void Cargar_tablaNC()
		{
			tablaNC=new DataTable();
			tablaNC.Columns.Add(new DataColumn("DESCRIPCION", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("CUENTA", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("SEDE", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("CENTROCOSTO", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("NUMERO", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("NIT", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("VALORDEBITO", typeof(double)));
			tablaNC.Columns.Add(new DataColumn("VALORCREDITO", typeof(double)));
            tablaNC.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
            tablaNC.Columns.Add(new DataColumn("SECUENCIA", typeof(string)));
		}
		
		protected void Cargar_dtIva()
		{
			dtIva=new DataTable();
			dtIva.Columns.Add("PORCENTAJE",typeof(double));
			dtIva.Columns.Add("VALORBASE",typeof(double));
			dtIva.Columns.Add("VALOR",typeof(double));
			dtIva.Columns.Add("CUENTA",typeof(string));
			dtIva.Columns.Add("NIT",typeof(string));
			dtIva.Columns.Add("TIPO",typeof(string));
		}
		
		protected void Mostrar_gridRtns()
		{
			gridRtns.DataSource=tablaRtns;
			gridRtns.DataBind();
			Session["tablaRtns"]=tablaRtns;
		}
		
		protected void Mostrar_Grid_Activos()
		{
			gridActivosFijos.DataSource=tablaActivosFijos;
			gridActivosFijos.DataBind();
			Session["tablaActivosFijos"]=tablaActivosFijos;
		}
		
		protected void Mostrar_Grid_Diferidos()
		{
			gridDiferidos.DataSource=tablaDiferidos;
			gridDiferidos.DataBind();
			Session["tablaDiferidos"]=tablaDiferidos;
		}
		
		protected void Mostrar_Grid_Detalles()
		{
			gridDetalles.DataSource=tablaDetalles;
			gridDetalles.DataBind();
			Session["tablaDetalles"]=tablaDetalles;
		}
		
		public void Mostrar_gridNC()
		{
			Session["tablaNC"]=tablaNC;
			gridNC.DataSource=tablaNC;
			gridNC.DataBind();
		}
		
		protected void Mostrar_dgIva()
		{
			Session["dtIva"]=dtIva;
			dgIva.DataSource=dtIva;
			dgIva.DataBind();
		}
		
		protected bool DiferenciaCreditosDebitos()
		{
			bool diferencia=false;
			double totalDebito=0,totalCredito=0;
			for(int i=0;i<tablaNC.Rows.Count;i++)
			{
                if (tablaNC.Rows[i][7].ToString() == "")
                    tablaNC.Rows[i][7] = "0";
                if (tablaNC.Rows[i][8].ToString() == "")
                    tablaNC.Rows[i][8] = "0";
                if (tablaNC.Rows[i][9].ToString() == "")
                    tablaNC.Rows[i][9] = "0";
                totalDebito =totalDebito+(Convert.ToDouble(tablaNC.Rows[i][7]));
				totalCredito=totalCredito+(Convert.ToDouble(tablaNC.Rows[i][8]));
			}
			if(tipoFactura.SelectedValue=="FC")
			{
				//Créditos-Debitos
				if(totalCredito>totalDebito)
					diferencia=true;
			}
			else if(tipoFactura.SelectedValue=="FP")
			{
				//Debitos-Creditos
				if(totalDebito>totalCredito)
					diferencia=true;
                // EUROTECK si grabar facturas de proveedor en ceros
                if (totalDebito >= totalCredito && DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA") == "901087944")
                    diferencia = true;
            }
			return diferencia;
		}
		
		protected bool Buscar_Retencion(string rtn)
		{
			bool encontrado=false;
			if(tablaRtns!=null && tablaRtns.Rows.Count!=0)
			{
				for(int i=0;i<tablaRtns.Rows.Count;i++)
				{
					if(tablaRtns.Rows[i][0].ToString()==rtn)
						encontrado=true;
				}
			}
			return encontrado;
		}
		
		protected void SumarRetenciones()
		{
			double totalRet=0;
			for(int i=0;i<tablaRtns.Rows.Count;i++)
				totalRet=totalRet+Convert.ToDouble(tablaRtns.Rows[i][3]);
			tbretenciones.Text=totalRet.ToString("C");
		}
		
		protected bool ValidarBasesRetencion(double baseN)
		{
            double baseT = baseN;
            bool validaBase = true;
			double totalF=Convert.ToDouble(valorTotal.Text.Substring(1));
		//	for(int n=0;n<tablaRtns.Rows.Count;n++)
            {
		//		baseT = Convert.ToDouble(tablaRtns.Rows[n]["VALORBASE"]);
                if (baseT > totalF)
                    validaBase = false;
            }
            return (validaBase);
		}
		
		protected double Calcular_Retenciones()
		{
			double valor=0;
			if(tablaRtns.Rows.Count!=0)
			{
				for(int i=0;i<tablaRtns.Rows.Count;i++)
					valor=valor+Convert.ToDouble(tablaRtns.Rows[i][3]);
			}
			return valor;
		}
		
		protected void CalcularTotalFactura()
		{
			double valorItems=0,fletes=0,iva=0,ivaFletes=0,valorRet=0,valorDeb=0,valorCre=0;
			Control miControl=encabezadoCliente.Controls[0];
			Control tuControl=encabezadoProveedor.Controls[0];
			if(tipoFactura.SelectedValue=="FC")
			{
				if((((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="1" || (((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue == "5")))
				{
					for(int i=0;i<tablaActivosFijos.Rows.Count;i++)
						valorItems=valorItems+Convert.ToDouble(tablaActivosFijos.Rows[i][3].ToString().Substring(1));
				}
				else if((((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="2"))
				{
					for(int i=0;i<tablaDiferidos.Rows.Count;i++)
						valorItems=valorItems+Convert.ToDouble(tablaDiferidos.Rows[i][3].ToString().Substring(1));
				}
				else if((((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="3"))
				{
					for(int i=0;i<tablaDetalles.Rows.Count;i++)
						valorItems=valorItems+Convert.ToDouble(tablaDetalles.Rows[i][3].ToString().Substring(1));
				}
				else if((((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="4"))
				{
					for(int i=0;i<tablaNC.Rows.Count;i++)
					{
						valorDeb=valorDeb+Convert.ToDouble(tablaNC.Rows[i][7]);
						valorCre=valorCre+Convert.ToDouble(tablaNC.Rows[i][8]);
					}
					valorItems=valorItems+(valorCre-valorDeb);
				}
			}
			else if(tipoFactura.SelectedValue=="FP")
			{
				if((((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="1") || (((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue == "5"))
				{
					for(int i=0;i<tablaActivosFijos.Rows.Count;i++)
						valorItems=valorItems+Convert.ToDouble(tablaActivosFijos.Rows[i][3].ToString().Substring(1));
				}
				else if((((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="2"))
				{
					for(int i=0;i<tablaDiferidos.Rows.Count;i++)
						valorItems=valorItems+Convert.ToDouble(tablaDiferidos.Rows[i][3].ToString().Substring(1));
				}
				else if((((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="3"))
				{
					for(int i=0;i<tablaDetalles.Rows.Count;i++)
						valorItems=valorItems+Convert.ToDouble(tablaDetalles.Rows[i][3].ToString().Substring(1));
				}
				else if((((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="4"))
				{
					for(int i=0;i<tablaNC.Rows.Count;i++)
					{
						valorDeb=valorDeb+Convert.ToDouble(tablaNC.Rows[i][7]);
						valorCre=valorCre+Convert.ToDouble(tablaNC.Rows[i][8]);
					}
					valorItems=valorItems+(valorDeb-valorCre);
				}
			}
			pnlValores.Visible=true;
			try{fletes=Convert.ToDouble(valorFletes.Text);}
			catch{fletes=0;}
			try{valorRet=Convert.ToDouble(tbretenciones.Text.Substring(1));}
			catch{valorRet=0;}
			CalcularIva();
			iva=Convert.ToDouble(valorIva.Text.Substring(1));
			ivaFletes=Convert.ToDouble(valorIvaFletes.Text.Substring(1));
			valorTotal.Text=valorItems.ToString("C");
			tbTotalFac.Text=(valorItems+fletes+iva+ivaFletes-valorRet).ToString("C");
		}
		
		protected DataTable CopiarTablaRetenciones(DataTable dtR)
		{
			DataTable dtN=new DataTable();
			dtN.Columns.Add(new DataColumn("CODRET", typeof(string)));
			dtN.Columns.Add(new DataColumn("PORCRET", typeof(double)));
            dtN.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
			dtN.Columns.Add(new DataColumn("VALOR", typeof(double)));
			DataRow drN;
			for(int n=0;n<dtR.Rows.Count;n++)
			{
				drN=dtN.NewRow();
				drN["CODRET"]=dtR.Rows[n]["CODRET"];
				drN["PORCRET"]=dtR.Rows[n]["PORCRET"];
                drN["VALORBASE"] = dtR.Rows[n]["VALORBASE"];
				drN["VALOR"]=dtR.Rows[n]["VALOR"];
				dtN.Rows.Add(drN);
			}
			return(dtN);
		}

		protected DataTable CopiarTablaIva(DataTable dtR)
		{
			DataTable dtN=new DataTable();
			dtN.Columns.Add("PORCENTAJE",typeof(double));
			dtN.Columns.Add("VALOR",typeof(double));
			dtN.Columns.Add("CUENTA",typeof(string));
			dtN.Columns.Add("NIT",typeof(string));
			dtN.Columns.Add("TIPO",typeof(string));
			DataRow drN;
			for(int n=0;n<dtR.Rows.Count;n++)
			{
				drN=dtN.NewRow();
				drN["PORCENTAJE"]=dtR.Rows[n]["PORCENTAJE"];
				drN["VALOR"]=dtR.Rows[n]["VALOR"];
				drN["CUENTA"]=dtR.Rows[n]["CUENTA"];
				drN["NIT"]=dtR.Rows[n]["NIT"];
				drN["TIPO"]=dtR.Rows[n]["TIPO"];
				dtN.Rows.Add(drN);
			}
			return(dtN);
		}

		private void CalcularIva()
		{
			double valIva=0,valIvaFle=0;
			for(int i=0;i<dtIva.Rows.Count;i++)
			{
				if(dtIva.Rows[i][5].ToString()=="IVA")
					valIva=valIva+Convert.ToDouble(dtIva.Rows[i][2]);
				else if(dtIva.Rows[i][5].ToString()=="IVA Fletes")
					valIvaFle=valIvaFle+Convert.ToDouble(dtIva.Rows[i][2]);
			}
			valorIva.Text=valIva.ToString("C");
			valorIvaFletes.Text=valIvaFle.ToString("C");
		}
		

		#endregion

		#region Eventos

		protected void Page_Load(object sender, EventArgs e)
		{
			encabezadoCliente.Controls.Add(LoadControl(pathToControls + "AMS.Cartera.CausacionFacturasAdministrativas.Cliente.ascx"));
			encabezadoProveedor.Controls.Add(LoadControl(pathToControls + "AMS.Cartera.CausacionFacturasAdministrativas.Proveedor.ascx"));

            //pruebas
            plcExcelImporter.Controls.Add(LoadControl(pathToControls + "AMS.Tools.ExcelImportData.ascx"));
            
			if(!IsPostBack)
			{
				Session.Clear();
				if(Request.QueryString["pre"]!=null && Request.QueryString["num"]!=null && Request.QueryString["t"]!=null)
				{
					if(Request.QueryString["t"]=="C")
                        Utils.MostrarAlerta(Response, "Se ha creado la factura de cliente con prefijo "+Request.QueryString["pre"]+" y número "+Request.QueryString["num"]+"");
					else if(Request.QueryString["t"]=="P")
                            Utils.MostrarAlerta(Response, "Se ha creado la factura de proveedor con prefijo " + Request.QueryString["pre"] + " y número " + Request.QueryString["num"] + "");
					try
					{
                        Imprimir.ImprimirRPT(Response, Request.QueryString["pre"], Convert.ToInt32(Request.QueryString["num"]), true);
					}
					catch
					{
						lbInfo.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
					}
				}
				hdniva.Value=DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa");
				encabezadoCliente.Visible=false;
				encabezadoProveedor.Visible=false;
				this.Cargar_Tabla_Activos();
				this.Mostrar_Grid_Activos();
				this.Cargar_Tabla_Diferidos();
				this.Mostrar_Grid_Diferidos();
				this.Cargar_Tabla_Detalles();
				this.Mostrar_Grid_Detalles();
				this.Cargar_tablaNC();
				this.Mostrar_gridNC();
				this.Cargar_Tabla_Rtns();
				this.Mostrar_gridRtns();
				this.Cargar_dtIva();
				this.Mostrar_dgIva();
			}
			else
			{
				if(Session["tablaActivosFijos"]!=null)
					tablaActivosFijos=(DataTable)Session["tablaActivosFijos"];
				if(Session["tablaDiferidos"]!=null)
					tablaDiferidos=(DataTable)Session["tablaDiferidos"];
				if(Session["tablaDetalles"]!=null)
					tablaDetalles=(DataTable)Session["tablaDetalles"];
				if(Session["tablaRtns"]!=null)
					tablaRtns=(DataTable)Session["tablaRtns"];
				if(Session["tablaNC"]!=null)
					tablaNC=(DataTable)Session["tablaNC"];
				if(Session["dtIva"]!=null)
					dtIva=(DataTable)Session["dtIva"];
				if(Session["inserts"]!=null)
					inserts=(ArrayList)Session["inserts"];
                //if (Session["tbCajaMenor"] != null)
                //    dtCajaMenor = (DataTable)Session["tbCajaMenor"];

            }
		}

        protected void VincularExcel(object Sender, EventArgs e)
		{
            if (Session["dtExcel"] != null)
            {
                tablaNC = (DataTable)Session["dtExcel"];
                Session["tablaNC"] = tablaNC;
                gridNC.DataSource = tablaNC;
                gridNC.DataBind();
                Session.Remove("dtExcel");

            }
            fldRtns.Visible = lbRet.Visible = fldIva.Visible = lbIva.Visible = true;
            //gridRtns.Visible = lbRet.Visible = true;
            //dgIva.Visible = lbIva.Visible = true;

            // aqui llamar la rutina de validacion pero registro a registro

            Mostrar_dgIva();
            if (DiferenciaCreditosDebitos())
                guardar.Enabled = true;
            else
                guardar.Enabled = false;
            CalcularTotalFactura();
        }

        [Ajax.AjaxMethod]

        protected void Escoger_Factura(object Sender,EventArgs e)
		{
            fldGenerales.Visible = true;
			//pnlDatos.Visible=true;
			if(tipoFactura.SelectedValue=="FC")
			{
				encabezadoCliente.Visible=true;
				lbcuenta.Text="Código de la Cuenta por Cobrar";
                try {tbcuenta.Text = DBFunctions.SingleData("Select MCUE_CODIPUCCXC from CCARTERA;"); }
                catch { tbcuenta.Text = ""; }
			}
			else if(tipoFactura.SelectedValue=="FP")
			{
				encabezadoProveedor.Visible=true;
				lbcuenta.Text="Código de la Cuenta por Pagar";
                try { tbcuenta.Text = DBFunctions.SingleData("Select MCUE_CODIPUCCXP from CCARTERA;"); }
                catch { tbcuenta.Text = ""; }
		
			}
			tipoFactura.Enabled=false;
			guardar.Enabled=false;
		}
		
		protected bool Buscar_Documento(string prefijo, string numero)
		{
			DataRow[] docs=tablaNC.Select("PREFIJO='"+prefijo+"' AND NUMERO='"+numero+"'");
			if(docs.Length!=0)
				return true;
			else
				return false;
		}
		
		protected bool Validar_Datos(DataGridCommandEventArgs e)
		{
			bool error=false;
			//Si hay algun campo en blanco o no son validos los valores
			if((((TextBox)e.Item.Cells[0].Controls[1]).Text=="")||
                (((TextBox)e.Item.Cells[1].Controls[1]).Text=="")||
                (((DropDownList)e.Item.Cells[2].FindControl("ddlalmacen")).Items.Count==0)||
                (((DropDownList)e.Item.Cells[3].FindControl("ddlcentrocosto")).Items.Count==0)||
                (((TextBox)e.Item.Cells[4].Controls[1]).Text=="")||
                (!DatasToControls.ValidarLong (((TextBox)e.Item.Cells[5].Controls[1]).Text))||
                (((TextBox)e.Item.Cells[6].Controls[1]).Text=="")||
                (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text))||
                (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text))||
                (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text)))
			{
                    Utils.MostrarAlerta(Response, "Falta un Campo por Llenar o las entradas son Invalidos. Revisa tus Datos");
				    error=true;
			}
				//Si en los dos campos valor Debito y valor Credito hay un valor distinto de cero
			else if(((((TextBox)e.Item.Cells[7].Controls[1]).Text!="0")&&(((TextBox)e.Item.Cells[8].Controls[1]).Text!="0")))
			{
                    Utils.MostrarAlerta(Response, "Uno de los campos Valor Debito o Valor Credito debe tener un valor de 0. Revisa tus Datos");
				    error=true;
			}
				//Si en ninguno de los dos campos hay valor
			else if((((((TextBox)e.Item.Cells[7].Controls[1]).Text=="0")&&(((TextBox)e.Item.Cells[8].Controls[1]).Text=="0"))))
			{
                    Utils.MostrarAlerta(Response, "Uno de los campos Valor Debito o Valor Credito debe tener valor. Revisa tus Datos");
				    error=true;
			}
				//Si el nit digitado no existe
			else if(!DBFunctions.RecordExist("SELECT mnit_nit FROM mnit WHERE mnit_nit='"+((TextBox)e.Item.Cells[6].FindControl("numnittxt")).Text+"'"))
			{
                    Utils.MostrarAlerta(Response, "El nit especificado no existe");
				    error=true;
			}
				//Si la cuenta digitada no existe
			else if(!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='"+((TextBox)e.Item.Cells[1].FindControl("cuentatxt")).Text+"' and timp_codigo in ('A','P') and tcue_codigo <> 'N' "))
			{
			        Utils.MostrarAlerta(Response, "La cuenta especificada NO Existe o es NO Imputable o es solo NIIF");
				    error=true;
			}
			return error;
		}
		
		protected void RestarRetenciones(DataGridCommandEventArgs e)
		{
			double totalRet=0;
			totalRet=Convert.ToDouble(tbretenciones.Text.Substring(1));
			totalRet=totalRet-Convert.ToDouble(tablaRtns.Rows[e.Item.DataSetIndex][3]);
			tbretenciones.Text=totalRet.ToString("C");
		}
		
		protected void gridRtns_Item(object Sender,DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="AgregarRetencion")
			{
                DropDownList ddlTiporetencion = (DropDownList)e.Item.FindControl("ddlTiporet");
                TextBox txtCodRete = (TextBox)e.Item.FindControl("codret");
                TextBox txtPorcentaje = (TextBox)e.Item.FindControl("codretb");
                TextBox txtBase = (TextBox)e.Item.FindControl("base");
                TextBox txtValor = (TextBox)e.Item.FindControl("valor");

                if ((((TextBox)txtCodRete).Text == ""))
                    Utils.MostrarAlerta(Response, "Debe escoger un tipo de retención");
                else if (this.Buscar_Retencion(((TextBox)txtCodRete).Text))
                    Utils.MostrarAlerta(Response, "Solo puede haber una retención de ese tipo");
                else if (!DatasToControls.ValidarDouble(((TextBox)txtBase).Text))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
				else
				{
					fila=tablaRtns.NewRow();
                    fila["CODRET"] = ((TextBox)txtCodRete).Text;
                    fila["PORCRET"] = Convert.ToDouble(((TextBox)txtPorcentaje).Text);
                    fila["VALORBASE"] = Convert.ToDouble(((TextBox)txtBase).Text);
                    fila["VALOR"] = Convert.ToDouble(((TextBox)txtValor).Text);
                    fila["TIPORETE"] = ((DropDownList)ddlTiporetencion).SelectedItem;
                    if (!ValidarBasesRetencion(Convert.ToDouble(((TextBox)txtValor).Text)))
					{
                        Utils.MostrarAlerta(Response, "Los valores base de las retenciones superan el valor de la factura");
						return;
					}
					tablaRtns.Rows.Add(fila);
					this.Mostrar_gridRtns();
					SumarRetenciones();
					CalcularTotalFactura();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverRetencion")
			{
				RestarRetenciones(e);
				tablaRtns.Rows[e.Item.DataSetIndex].Delete();
				this.Mostrar_gridRtns();
				CalcularTotalFactura();
			}
		}

		protected void gridRtns_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
            Control proveedor=null,cliente=null;
			if(encabezadoProveedor.Controls.Count!=0)
				proveedor=encabezadoProveedor.Controls[0];
			if(encabezadoCliente.Controls.Count!=0)
				cliente=encabezadoCliente.Controls[0];

			if(e.Item.ItemType==ListItemType.Footer)
			{
				TextBox txtBase=(TextBox)e.Item.FindControl("base");
                TextBox txtCodRete = (TextBox)e.Item.FindControl("codret");
				TextBox txtPorcentaje=(TextBox)e.Item.FindControl("codretb");
				TextBox txtValor=(TextBox)e.Item.FindControl("valor");
                DropDownList ddlTiporetencion = (DropDownList)e.Item.FindControl("ddlTiporet");
                
				string scrTotal="PorcentajeVal('"+txtPorcentaje.ClientID+"','"+txtBase.ClientID+"','"+txtValor.ClientID+"');";
				txtBase.Attributes.Add("onkeyup","NumericMaskE(this,event);"+scrTotal);

                DatasToControls bind = new DatasToControls();
                Retencion rtns = new Retencion(((TextBox)proveedor.FindControl("nit")).Text, false);
                ddlTiporetencion.Attributes.Add("onChange", "Cambio_Retencion(this," + ((TextBox)txtCodRete).ClientID + ",'"+ rtns.TipoSociedad +"','','" + txtPorcentaje.ClientID + "','" + txtBase.ClientID + "','" + txtValor.ClientID + "')");
                bind.PutDatasIntoDropDownList(ddlTiporetencion, "select * from tretencion order by 2;");
                ddlTiporetencion.Items.Insert(0, new ListItem("Seleccione...", "0"));

				if(tipoFactura.SelectedValue=="FC")
                    ((TextBox)txtCodRete).Attributes.Add("onClick", "ModalDialog(this,'SELECT pr.pret_codigo codigo, REPLACE(pr.pret_nombre,',','.') nombre,pr.pret_porcennodecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov CUENTA_PROVEEDOR, pr.MCUE_CODIPUCCLIE CUENTA_CLIENTE FROM pretencion pr where pr.ttip_codigo IN (\\'N\\',\\'T\\') ORDER BY tipo',new Array());" + scrTotal);
				else if(tipoFactura.SelectedValue=="FP")
				{
                    //Retencion rtns = new Retencion(((TextBox)proveedor.FindControl("nit")).Text, false);
                  if (rtns.TipoSociedad == "N" || rtns.TipoSociedad == "U")
                        ((TextBox)txtCodRete).Attributes.Add("onClick", "ModalDialog(this,'SELECT pr.pret_codigo codigo, REPLACE(pr.pret_nombre,',','.') nombre,pr.pret_porcennodecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov CUENTA_PROVEEDOR, pr.MCUE_CODIPUCCLIE CUENTA_CLIENTE FROM pretencion pr where pr.ttip_codigo IN (\\'N\\',\\'T\\') ORDER BY tipo;',new Array());" + scrTotal);
					else
                        ((TextBox)txtCodRete).Attributes.Add("onClick", "ModalDialog(this,'SELECT pr.pret_codigo codigo, REPLACE(pr.pret_nombre,',','.') nombre,pr.pret_porcendecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov CUENTA_PROVEEDOR, pr.MCUE_CODIPUCCLIE CUENTA_CLIENTE FROM pretencion pr where pr.ttip_codigo IN (\\'J\\',\\'T\\') ORDER BY tipo;',new Array());" + scrTotal);
				}
			}
		}
		
		protected void Guardar_Factura(object Sender,EventArgs e)
		{
			FacturaAdministrativa miFactura=new FacturaAdministrativa();
			DataTable tablaRtnsA=CopiarTablaRetenciones(tablaRtns);
			DataTable dtIvaA=CopiarTablaIva(dtIva);
			//Miro que tipo de factura es
			if(tipoFactura.SelectedValue=="FC")
			{
				Control miControl=encabezadoCliente.Controls[0];
				if(!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='"+tbcuenta.Text+"' and timp_codigo in ('A','P') AND tcue_codigo <> 'N' "))
                    Utils.MostrarAlerta(Response, "La cuenta especificada NO Existe o es NO Imputable o es solo NIIF");
					//Verifico que hayan escogido un gasto
				else if(((DropDownList)miControl.FindControl("tipoGasto")).SelectedItem.Text=="--Escoja--")
                    Utils.MostrarAlerta(Response, "Escoja un gasto");
					//Verifico que haya metido algo en las grillas
				else if((((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="1")&&((tablaActivosFijos==null)||(tablaActivosFijos.Rows.Count==0)))
                    Utils.MostrarAlerta(Response, "Debe ingresar activos fijos");
				else if((((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="2")&&((tablaDiferidos==null)||(tablaDiferidos.Rows.Count==0)))
                    Utils.MostrarAlerta(Response, "Debe ingresar diferidos");
				else if((((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="3")&&((tablaDetalles==null)||(tablaDetalles.Rows.Count==0)))
                    Utils.MostrarAlerta(Response, "Debe ingresar gastos operativos");
                else if ((((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue == "5") && ((tablaActivosFijos == null) || (tablaActivosFijos.Rows.Count == 0)))
                    Utils.MostrarAlerta(Response, "Debe ingresar Mejoras de los activos");
                else if(!DatasToControls.ValidarDouble(valorTotal.Text.Substring(1)) || !DatasToControls.ValidarDouble(valorIva.Text.Substring(1)) || !DatasToControls.ValidarDouble(valorFletes.Text) || !DatasToControls.ValidarDouble(valorIvaFletes.Text.Substring(1)) || !DatasToControls.ValidarDouble(costoFac.Text))
                    Utils.MostrarAlerta(Response, "Alguno de los valores es invalido");
					//Si esta perfecto
				else
				{
					//Miro que tipo de gasto se escogio y envio dicha tabla al constructor de la factura
					if(((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="1" || ((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue == "5")
						miFactura=new FacturaAdministrativa(this.tablaActivosFijos,tablaRtnsA,dtIvaA,"C");
					else if(((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="2")
						miFactura=new FacturaAdministrativa(this.tablaDiferidos,tablaRtnsA,dtIvaA,"C");
					else if(((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="3")
						miFactura=new FacturaAdministrativa(this.tablaDetalles,tablaRtnsA,dtIvaA,"C");
					else if(((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue=="4")
						miFactura=new FacturaAdministrativa(this.tablaNC,tablaRtnsA,dtIvaA,"C");
					//Mando los datos necesarios
					miFactura.PrefijoFactura=((DropDownList)miControl.FindControl("prefijoFactura")).SelectedValue;
					miFactura.NumeroFactura=System.Convert.ToInt32(((TextBox)miControl.FindControl("numeroFactura")).Text);
					miFactura.Fecha=((TextBox)miControl.FindControl("fecha")).Text;
					miFactura.DiasPlazo=Convert.ToInt32(((TextBox)miControl.FindControl("tbDiasPlazo")).Text);
					miFactura.FechaVencimiento=(Convert.ToDateTime(miFactura.Fecha).AddDays(miFactura.DiasPlazo)).ToString();
					miFactura.Nit=((TextBox)miControl.FindControl("nit")).Text;
					miFactura.Almacen=((DropDownList)miControl.FindControl("almacen")).SelectedValue;
                    miFactura.CentroCosto = DBFunctions.SingleData("SELECT pcen_centcart FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + miFactura.Almacen + "'");
					miFactura.Vendedor=((DropDownList)miControl.FindControl("vendedor")).SelectedValue;
					miFactura.TipoGasto=((DropDownList)miControl.FindControl("tipoGasto")).SelectedValue;
					miFactura.Observacion=((TextBox)miControl.FindControl("observacion")).Text;
					miFactura.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
					miFactura.ValorFactura=System.Convert.ToDouble(this.valorTotal.Text.Substring(1));
					miFactura.ValorFletes=Convert.ToDouble(valorFletes.Text);
					miFactura.ValorIva=Convert.ToDouble(valorIva.Text.Substring(1));
					miFactura.ValorIvaFletes=Convert.ToDouble(valorIvaFletes.Text.Substring(1));
					miFactura.ValorRete=this.Calcular_Retenciones();
					miFactura.CostoFactura=Convert.ToDouble(costoFac.Text);
					miFactura.Cuenta=tbcuenta.Text;
					if(miFactura.Guardar_Factura())
					{
                        // contabiizacion ON LINE
                        Session.Clear();
                        contaOnline.contabilizarOnline(miFactura.PrefijoFactura.ToString(), Convert.ToInt32(miFactura.NumeroFactura.ToString()), Convert.ToDateTime(miFactura.Fecha), "");
                        lbInfo.Text = miFactura.Mensajes;
						
						Response.Redirect(indexPage+"?process=Cartera.CausacionFacturasAdministrativas&pre="+miFactura.PrefijoFactura+"&num="+miFactura.NumeroFactura+"&t=C");
					}
					else
						lbInfo.Text=miFactura.Mensajes;
				}
			}
			else if(tipoFactura.SelectedValue=="FP")
			{
				Control tuControl=encabezadoProveedor.Controls[0];
				if(!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='"+tbcuenta.Text+"' and timp_codigo in ('A','P') AND tcue_codigo <> 'N' "))
                    Utils.MostrarAlerta(Response, "La cuenta especificada no existe o e NO Imputable o es solo NIIF");
					//Verifico que hayan escogido un gasto
				else if(((DropDownList)tuControl.FindControl("tipoGasto")).SelectedItem.Text=="--Escoja--")
                    Utils.MostrarAlerta(Response, "Escoja un gasto");
					//Verifico que hayan metido algo en las grillas
				else if((((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="1")&&((tablaActivosFijos==null)||(tablaActivosFijos.Rows.Count==0)))
                    Utils.MostrarAlerta(Response, "Debe ingresar activos fijos");
				else if((((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="2")&&((tablaDiferidos==null)||(tablaDiferidos.Rows.Count==0)))
                    Utils.MostrarAlerta(Response, "Debe ingresar diferidos");
				else if((((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="3")&&((tablaDetalles==null)||(tablaDetalles.Rows.Count==0)))
                    Utils.MostrarAlerta(Response, "Debe ingresar gastos operativos");
				else if(!DatasToControls.ValidarDouble(valorTotal.Text.Substring(1)) || !DatasToControls.ValidarDouble(valorIva.Text.Substring(1)) || !DatasToControls.ValidarDouble(valorFletes.Text) || !DatasToControls.ValidarDouble(valorIvaFletes.Text.Substring(1)) || !DatasToControls.ValidarDouble(costoFac.Text))
                    Utils.MostrarAlerta(Response, "Alguno de los valores es invalido");
					//Si esta perfecto
				else
				{
					//Miro que tipo de gasto se escogió y envio dicha tabla al constructor de la factura
					if(((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="1" || ((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue == "5")
						miFactura=new FacturaAdministrativa(this.tablaActivosFijos,tablaRtnsA,dtIvaA,"P");
					else if(((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="2")
						miFactura=new FacturaAdministrativa(this.tablaDiferidos,tablaRtnsA,dtIvaA,"P");
					else if(((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="3")
						miFactura=new FacturaAdministrativa(this.tablaDetalles,tablaRtnsA,dtIvaA,"P");
					else if(((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue=="4")
						miFactura=new FacturaAdministrativa(this.tablaNC,tablaRtnsA,dtIvaA,"P");
					//Mando los datos necesarios
					miFactura.PrefijoFactura=((DropDownList)tuControl.FindControl("prefijoFactura")).SelectedValue;
					miFactura.NumeroFactura=System.Convert.ToInt32(((TextBox)tuControl.FindControl("numeroFactura")).Text);
					miFactura.Fecha=((TextBox)tuControl.FindControl("fecha")).Text;
					miFactura.FechaVencimiento=((TextBox)tuControl.FindControl("tbFecVen")).Text;
					miFactura.Nit=((TextBox)tuControl.FindControl("nit")).Text;
					miFactura.Almacen=((DropDownList)tuControl.FindControl("almacen")).SelectedValue;
					miFactura.TipoGasto=((DropDownList)tuControl.FindControl("tipoGasto")).SelectedValue;
					miFactura.PrefijoProveedor=((TextBox)tuControl.FindControl("prefijoProveedor")).Text;
					miFactura.NumeroProveedor=System.Convert.ToInt64(((TextBox)tuControl.FindControl("numeroProveedor")).Text);
					miFactura.Observacion=((TextBox)tuControl.FindControl("observacion")).Text;
					miFactura.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
					miFactura.ValorFactura=System.Convert.ToDouble(this.valorTotal.Text.Substring(1));
					miFactura.ValorFletes=Convert.ToDouble(valorFletes.Text);
					miFactura.ValorIva=Convert.ToDouble(valorIva.Text.Substring(1));
					miFactura.ValorIvaFletes=Convert.ToDouble(valorIvaFletes.Text.Substring(1));
					miFactura.ValorRete=this.Calcular_Retenciones();
					miFactura.Cuenta=tbcuenta.Text;
					miFactura.Inserts=inserts;
                    if (miFactura.Guardar_Factura())
                    {
                        // contabiizacion ON LINE
                        contaOnline.contabilizarOnline(miFactura.PrefijoFactura.ToString(), Convert.ToInt32(miFactura.NumeroFactura.ToString()), Convert.ToDateTime(miFactura.Fecha), "");
                        lbInfo.Text = miFactura.Mensajes;

                        //VALES DE CAJA MENOR
                        if (Session["excNCVales"] != null && tablaNC.Rows.Count > 0)
                        { 
                            string query = "DELETE FROM DCAJAMENOR WHERE DCAJ_SECUENCIA IN( ";
                            //borrar registros de tabla DCAJAMENOR
                            for (int i = 0; i < tablaNC.Rows.Count; i++)
                            {
                                if (tablaNC.Rows[i][10].ToString().Length > 0)
                                {
                                    query += tablaNC.Rows[i][10].ToString() + ",";
                                }
                                else// si ya no encuentra dato en la [10] es porque ya no se necesita realizar más este proceso
                                    break;
                            }
                            query = query.Substring(0, query.Length - 1);
                            //DBFunctions.NonQuery(query);
                            query += ");";
                            int rta = DBFunctions.NonQuery(query);

                            //if (rta > 0)
                            //{

                            //}
                            //else
                            //{
                            //    lbInfo.Text += "Se generó un error al eliminar los registros de Vales de Caja menor de la tabla principal" + "<br />";
                            //    lbInfo.Text += "Se generó la factura : " + miFactura.PrefijoFactura + " - " + miFactura.NumeroFactura +  "<br />";
                            //}
                        }
                        Session.Clear();
                        //FIN PROCESO
                        Response.Redirect(indexPage+"?process=Cartera.CausacionFacturasAdministrativas&pre="+miFactura.PrefijoFactura+"&num="+miFactura.NumeroFactura+"&t=P");
					}
					else
						lbInfo.Text=miFactura.Mensajes;
				}
			}
		}
		
		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Cartera.CausacionFacturasAdministrativas");
		}

        protected void cargarValesCMenor(object sender, EventArgs e)
        {
            //Cargar grilla con la tabla DCAJAMENOR
            string almacen = lbAlmacen.Text.Split('-')[0].Trim();
            DataSet dsCajaMenor = new DataSet();
            DBFunctions.Request(dsCajaMenor, IncludeSchema.NO, @"SELECT DCAJ_DETALLE AS DESCRIPCION, MCUE_CODIPUC AS CUENTA, PALM_ALMACEN AS SEDE, PCEN_CODIGO AS CENTROCOSTO,
                                                                PDOC_CODIGO AS PREFIJO, DITE_NUMEDOCU AS NUMERO, MNIT_NIT AS NIT, DCAJ_VALODEBI AS VALORDEBITO, 
                                                                DCAJ_VALOCRED AS VALORCREDITO, DCAJ_VALOBASE AS VALORBASE, DCAJ_SECUENCIA AS SECUENCIA FROM DCAJAMENOR WHERE PALM_ALMACEN = '" + almacen + "';");
            if(dsCajaMenor.Tables.Count == 0 || dsCajaMenor.Tables[0].Rows.Count == 0)
            {
                Utils.MostrarAlerta(Response, "Aún no ha parametrizado la tabla de caja menor o no existe ningún registro en la misma, favor de contactar al administrador del sistema o subir la información de forma manual.");
                return;
            }
            //gridNC.DataSource = dsCajaMenor.Tables[0];
            //gridNC.DataBind();
            //es necesario crear una nueva sesión específica para las filas de DCAJAMENOR
            //Session["tbCajaMenor"] = dsCajaMenor.Tables[1];
            Session["tablaNC"] = dsCajaMenor.Tables[0];
            tablaNC = dsCajaMenor.Tables[0];
            //if (Session["tablaNC"] == null)
                //this.Cargar_tablaNC();
            //for (int i = 0; i < gridNC.Items.Count; i++)
            //{
            //    fila = tablaNC.NewRow();
            //    fila[0] = ((TextBox)gridNC.Items[i].Cells[0].Controls[1]).Text;
            //    fila[1] = ((TextBox)gridNC.Items[i].Cells[1].Controls[1]).Text;
            //    fila[2] = ((DropDownList)gridNC.Items[i].Cells[2].FindControl("ddlalmacen")).SelectedValue;
            //    fila[3] = ((DropDownList)gridNC.Items[i].Cells[3].FindControl("ddlcentrocosto")).SelectedValue;
            //    fila[4] = ((TextBox)gridNC.Items[i].Cells[4].Controls[1]).Text;
            //    fila[5] = ((TextBox)gridNC.Items[i].Cells[5].Controls[1]).Text;
            //    fila[6] = ((TextBox)gridNC.Items[i].Cells[6].Controls[1]).Text;


            //    //Si el valor introducido es debito
            //    if ((((TextBox)gridNC.Items[i].Cells[7].Controls[1]).Text != "0") && (((TextBox)gridNC.Items[i].Cells[8].Controls[1]).Text == "0"))
            //    {
            //        fila[7] = Convert.ToDouble(((TextBox)gridNC.Items[i].Cells[7].Controls[1]).Text);
            //        fila[8] = Convert.ToDouble("0");
            //    }
            //    //Si el valor introducido es credito
            //    else if ((((TextBox)gridNC.Items[i].Cells[7].Controls[1]).Text == "0") && (((TextBox)gridNC.Items[i].Cells[8].Controls[1]).Text != "0"))
            //    {
            //        fila[7] = System.Convert.ToDouble("0");
            //        fila[8] = System.Convert.ToDouble(((TextBox)gridNC.Items[i].Cells[8].Controls[1]).Text);

            //    }
            //    //Ahora verificamos los valores del valor base
            //    verBase = DBFunctions.SingleData("SELECT tbas_codigo FROM mcuenta WHERE mcue_codipuc='" + (((TextBox)gridNC.Items[i].Cells[1].Controls[1]).Text).ToString() + "'");
            //    //Si la cuenta no soporta valor base y hay algun valor distinto de cero en ese campo
            //    if (verBase == "N" && (((TextBox)gridNC.Items[i].Cells[9].Controls[1]).Text != "0"))
            //    {
            //        Utils.MostrarAlerta(Response, "La cuenta afectada no soporta Valor Base por tanto se guardara un valor de 0");
            //        fila[9] = System.Convert.ToDouble("0");
            //    }
            //    //Si la cuenta no soporta valor base y hay un valor de cero en ese campo
            //    else if (verBase == "N" && (((TextBox)gridNC.Items[i].Cells[9].Controls[1]).Text == "0"))
            //        fila[9] = System.Convert.ToDouble(((TextBox)gridNC.Items[i].Cells[9].Controls[1]).Text);
            //    //Si la cuenta afectada soporta valor base
            //    else if (verBase == "B")
            //    {
            //        //Convierto a double el valor base
            //        vb1 = System.Convert.ToDouble(((TextBox)gridNC.Items[i].Cells[9].Controls[1]).Text);
            //        //Miro en la base de datos cual es el porcentaje de valor base
            //        //lo convierto a double y lo divido por 100 para saber el verdadero valor
            //        vporcBase = System.Convert.ToDouble(DBFunctions.SingleData("SELECT mcue_basegrav FROM mcuenta WHERE mcue_codipuc='" + (((TextBox)gridNC.Items[i].Cells[1].Controls[1]).Text).ToString() + "'"));
            //        //Si el valor introducido es debito entonces se calcula el valor base con base en este
            //        if (((TextBox)gridNC.Items[i].Cells[7].Controls[1]).Text != "0")
            //        {
            //            res = Math.Round(Convert.ToDouble(((TextBox)gridNC.Items[i].Cells[7].Controls[1]).Text) * 100 / vporcBase, 0);
            //            if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
            //                fila[9] = vb1;
            //            else
            //            {
            //                Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
            //                fila[9] = res;
            //            }
            //        }
            //        //Si el valor introducido es credito entonces se calcula el valor base con base en este
            //        else if (((TextBox)gridNC.Items[i].Cells[8].Controls[1]).Text != "0")
            //        {
            //            res = Math.Round(Convert.ToDouble(((TextBox)gridNC.Items[i].Cells[8].Controls[1]).Text) * 100 / vporcBase, 0);
            //            if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
            //                fila[9] = vb1;
            //            else
            //            {
            //                Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
            //                fila[9] = res;
            //            }
            //        }
            //    }
            //    tablaNC.Rows.Add(fila);
            //}
            
            gridNC.DataSource = tablaNC;
            gridNC.DataBind();
            Session["tablaNC"] = tablaNC;
            Session["excNCVales"] = "1";
            fldRtns.Visible = lbRet.Visible = fldIva.Visible = lbIva.Visible = true;
            //gridRtns.Visible=lbRet.Visible=true;
            //dgIva.Visible=lbIva.Visible=true;
            Mostrar_dgIva();
            if (DiferenciaCreditosDebitos())
                guardar.Enabled = true;
            else
                guardar.Enabled = false;
            CalcularTotalFactura();
        }

        private void gridNC_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			string verBase;
			double vb1=0,vporcBase,res;
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="AgregarFilas")
			{
				if(!Validar_Datos(e))
				{
					if(Session["tablaNC"]==null)
						this.Cargar_tablaNC();
					fila=tablaNC.NewRow();
					fila[0]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila[1]=((TextBox)e.Item.Cells[1].Controls[1]).Text;
					fila[2]=((DropDownList)e.Item.Cells[2].FindControl("ddlalmacen")).SelectedValue;
					fila[3]=((DropDownList)e.Item.Cells[3].FindControl("ddlcentrocosto")).SelectedValue;
					fila[4]=((TextBox)e.Item.Cells[4].Controls[1]).Text;
					fila[5]=((TextBox)e.Item.Cells[5].Controls[1]).Text;
					fila[6]=((TextBox)e.Item.Cells[6].Controls[1]).Text;
					//Si el valor introducido es debito
					if((((TextBox)e.Item.Cells[7].Controls[1]).Text!="0")&&(((TextBox)e.Item.Cells[8].Controls[1]).Text=="0"))
					{
						fila[7]=Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
						fila[8]=Convert.ToDouble("0");
					}
						//Si el valor introducido es credito
					else if((((TextBox)e.Item.Cells[7].Controls[1]).Text=="0")&&(((TextBox)e.Item.Cells[8].Controls[1]).Text!="0"))
					{
						fila[7]=System.Convert.ToDouble("0");
						fila[8]=System.Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);
						
					}
					//Ahora verificamos los valores del valor base
					verBase=DBFunctions.SingleData("SELECT tbas_codigo FROM mcuenta WHERE mcue_codipuc='"+(((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString()+"'");
					//Si la cuenta no soporta valor base y hay algun valor distinto de cero en ese campo
					if(verBase=="N" && (((TextBox)e.Item.Cells[9].Controls[1]).Text!="0"))
					{
                        Utils.MostrarAlerta(Response, "La cuenta afectada no soporta Valor Base por tanto se guardara un valor de 0");
						fila[9]=System.Convert.ToDouble("0");
					}
						//Si la cuenta no soporta valor base y hay un valor de cero en ese campo
					else if(verBase=="N" && (((TextBox)e.Item.Cells[9].Controls[1]).Text=="0"))
						fila[9]=System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
						//Si la cuenta afectada soporta valor base
					else if(verBase=="B")
					{
						//Convierto a double el valor base
						vb1=System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
						//Miro en la base de datos cual es el porcentaje de valor base
						//lo convierto a double y lo divido por 100 para saber el verdadero valor
						vporcBase=System.Convert.ToDouble(DBFunctions.SingleData("SELECT mcue_basegrav FROM mcuenta WHERE mcue_codipuc='"+(((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString()+"'"));
						//Si el valor introducido es debito entonces se calcula el valor base con base en este
						if(((TextBox)e.Item.Cells[7].Controls[1]).Text!="0")
						{
							res=Math.Round(Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text)*100/vporcBase,0);
                            if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
								fila[9]=vb1;
							else
							{
                                Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                                fila[9]=res;
							}
						}
							//Si el valor introducido es credito entonces se calcula el valor base con base en este
						else if(((TextBox)e.Item.Cells[8].Controls[1]).Text!="0")
						{
							res=Math.Round(Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text)*100/vporcBase,0);
                            if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
								fila[9]=vb1;
							else
							{
                                Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
								fila[9]=res;
							}
						}
					}
                    //fila[10] = "0";
					tablaNC.Rows.Add(fila);
					gridNC.DataSource=tablaNC;
					gridNC.DataBind();
					Session["tablaNC"]=tablaNC;

                    fldRtns.Visible = lbRet.Visible = fldIva.Visible = lbIva.Visible = true;
					//gridRtns.Visible=lbRet.Visible=true;
					//dgIva.Visible=lbIva.Visible=true;
					Mostrar_dgIva();
					if(DiferenciaCreditosDebitos())
						guardar.Enabled=true;
					else
						guardar.Enabled=false;
					CalcularTotalFactura();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverFilas")
			{
				//Lo unico que hacemos es restar el valor al total y borramos la fila
                //if(dtCajaMenor != null)
                //{
                //    if(dtCajaMenor.Rows.Count > 0)
                //    {
                //        //DataRow[] encontroFila = dtCajaMenor.Select("POSICION = '" + e.Item.DataSetIndex + "'");
                //        if (e.Item.DataSetIndex < dtCajaMenor.Rows.Count)
                //        {
                //            //dtCajaMenor.c
                //            dtCajaMenor.Rows[e.Item.DataSetIndex].Delete();
                //            dtCajaMenor.AcceptChanges();
                //        }
                //    }
                //}
				tablaNC.Rows[e.Item.DataSetIndex].Delete();
                tablaNC.AcceptChanges();
				gridNC.DataSource=this.tablaNC;
				gridNC.DataBind();
				Session["tablaNC"]=this.tablaNC;
				if(DiferenciaCreditosDebitos())
					guardar.Enabled=true;
				else
					guardar.Enabled=false;
				CalcularTotalFactura();
			}
            else if (((Button)e.CommandSource).CommandName == "CopiarFilas")
            {
                Session["indexCopy"] = e.Item.DataSetIndex;
                gridNC.DataSource = this.tablaNC;
                gridNC.DataBind();
            }
		}
		
		protected void gridNC_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
			Control clientes=encabezadoCliente.Controls[0];
			Control proveedores=encabezadoProveedor.Controls[0];
			DatasToControls bind=new DatasToControls();
			if(e.Item.ItemType==ListItemType.Footer)
			{
				if(tipoFactura.SelectedValue=="FC")
					((TextBox)e.Item.Cells[6].FindControl("numnittxt")).Text=((TextBox)clientes.FindControl("nit")).Text;
				else if(tipoFactura.SelectedValue=="FP")
				{
					((TextBox)e.Item.Cells[4].FindControl("prefijotxt")).Text=((TextBox)proveedores.FindControl("prefijoProveedor")).Text;
					((TextBox)e.Item.Cells[5].FindControl("numdocutxt")).Text=((TextBox)proveedores.FindControl("numeroProveedor")).Text;
					((TextBox)e.Item.Cells[6].FindControl("numnittxt")).Text=((TextBox)proveedores.FindControl("nit")).Text;
				}
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].FindControl("ddlalmacen")), "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[3].FindControl("ddlcentrocosto")), "SELECT pcen_codigo,pcen_codigo || '-' || pcen_nombre FROM pcentrocosto where timp_codigo <> 'N' ORDER BY pcen_codigo");

                //Copia de fila sobre el footer.
                if (Session["indexCopy"] != null && Session["indexCopy"] != "")
                {
                    int indiceCop = Convert.ToInt16(Session["indexCopy"]);

                    ((TextBox)e.Item.Cells[0].FindControl("descripciontxt")).Text = tablaNC.Rows[indiceCop][0].ToString();
                    ((TextBox)e.Item.Cells[1].FindControl("cuentatxt")).Text = tablaNC.Rows[indiceCop][1].ToString();
                    ((DropDownList)e.Item.Cells[2].FindControl("ddlalmacen")).SelectedValue = tablaNC.Rows[indiceCop][2].ToString();
                    ((DropDownList)e.Item.Cells[3].FindControl("ddlcentrocosto")).SelectedValue = tablaNC.Rows[indiceCop][3].ToString();
                    ((TextBox)e.Item.Cells[4].FindControl("prefijotxt")).Text = tablaNC.Rows[indiceCop][4].ToString();
                    ((TextBox)e.Item.Cells[5].FindControl("numdocutxt")).Text = tablaNC.Rows[indiceCop][5].ToString();
                    ((TextBox)e.Item.Cells[6].FindControl("numnittxt")).Text = tablaNC.Rows[indiceCop][6].ToString();
                    ((TextBox)e.Item.Cells[7].FindControl("valordebitotxt")).Text = tablaNC.Rows[indiceCop][7].ToString();
                    ((TextBox)e.Item.Cells[8].FindControl("valorcreditotxt")).Text = tablaNC.Rows[indiceCop][8].ToString();
                    ((TextBox)e.Item.Cells[9].FindControl("valorbasetxt")).Text = tablaNC.Rows[indiceCop][9].ToString();

                    Session.Remove("indexCopy");
                }
			}

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList almacen = ((DropDownList)e.Item.Cells[2].FindControl("ddlalmacen_edit"));
                DropDownList centro = ((DropDownList)e.Item.Cells[3].FindControl("ddlcentrocosto_edit"));
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                bind.PutDatasIntoDropDownList(centro, "SELECT pcen_codigo,pcen_codigo || '-' || pcen_nombre FROM pcentrocosto where timp_codigo <> 'N' ORDER BY pcen_codigo");
                almacen.SelectedValue = tablaNC.Rows[gridNC.EditItemIndex][2].ToString();
                centro.SelectedValue = tablaNC.Rows[gridNC.EditItemIndex][3].ToString();
            }
		}
		
		private void gridActivosFijos_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="Agregar_Gasto")
			{
				if((((TextBox)e.Item.Cells[0].Controls[1]).Text=="")||(((TextBox)e.Item.Cells[1].Controls[1]).Text==""))
                    Utils.MostrarAlerta(Response, "Debe introducir un activo fijo");
				else if(((TextBox)e.Item.Cells[3].Controls[1]).Text=="undefined")
                    Utils.MostrarAlerta(Response, "Debe introducir un activo fijo nuevo");
				else
				{
					if(Session["tablaActivosFijos"]==null)
						this.Cargar_Tabla_Activos();
					fila=tablaActivosFijos.NewRow();
					fila["CODIGOGASTO"]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila["CODIGOACTIVO"]=((TextBox)e.Item.Cells[1].Controls[1]).Text;
					fila["NOMBREACTIVO"]=((TextBox)e.Item.Cells[2].Controls[1]).Text;
					fila["VALORACTIVO"]=(System.Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text)).ToString("C");
					tablaActivosFijos.Rows.Add(fila);
					this.Mostrar_Grid_Activos();
					valorTotal.Text=(Convert.ToDouble(valorTotal.Text.Substring(1))+((System.Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text)))).ToString("C");
					pnlValores.Visible=true;
					if(tipoFactura.SelectedValue=="FC")
					{
						lbCosto.Visible=true;
						costoFac.Visible=true;
					}
					else
					{
						lbCosto.Visible=false;
						costoFac.Visible=false;
					}
					guardar.Enabled=true;

                    fldRtns.Visible = lbRet.Visible = dgIva.Visible = lbIva.Visible = true;
                    //gridRtns.Visible=lbRet.Visible=true;
                    //dgIva.Visible=lbIva.Visible=true;
					Mostrar_dgIva();
					if(Session["instFac"]!=null)
					{
						inserts.Add((string)Session["instFac"]);
						Session["inserts"]=inserts;
					}
					CalcularTotalFactura();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="Remover_Gasto")
			{
				valorTotal.Text=((Convert.ToDouble(valorTotal.Text.Substring(1)))-((Convert.ToDouble(tablaActivosFijos.Rows[e.Item.DataSetIndex][3].ToString().Substring(1))))).ToString("C");
                if (valorTotal.Text == "$0.00")
                {
                    guardar.Enabled = false;
                    tablaRtns.Clear();
                    this.Mostrar_gridRtns();
                    fldRtns.Visible = lbRet.Visible = false;
					//gridRtns.Visible=lbRet.Visible=false;
				}
				if(this.tipoFactura.SelectedValue=="FP")
				{
                    if(inserts.Count > 0)
                    {
                        try
                        {
                            inserts.RemoveAt(e.Item.DataSetIndex);
                            Session["inserts"] = inserts;
                        }
                        catch (Exception z)
                        {
                            //inserts = new ArrayList();
                        }
                    }
				}
				tablaActivosFijos.Rows[e.Item.DataSetIndex].Delete();
				this.Mostrar_Grid_Activos();
				CalcularTotalFactura();
			}
		}
		
		protected void gridActivosFijos_DataBound(object Sender,DataGridItemEventArgs e)
		{
			Control cliente=null,proveedor=null;
			if(encabezadoCliente.Controls.Count!=0)
				cliente=encabezadoCliente.Controls[0];
			if(encabezadoProveedor.Controls.Count!=0)
				proveedor=encabezadoProveedor.Controls[0];
			//Si es una factura de cliente cargo el modaldialog normal
			if(e.Item.ItemType==ListItemType.Footer)
			{
				if(tipoFactura.SelectedValue=="FC" && ((DropDownList)cliente.FindControl("tipoGasto")).SelectedValue=="1" || ((DropDownList)cliente.FindControl("tipoGasto")).SelectedValue == "5")
					((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT mafj_codiacti AS Codigo,mafj_descripcion AS Descripcion,mafj_valohist AS Valor FROM mactivofijo',new Array(),1)");
                else if (tipoFactura.SelectedValue=="FP" && ((DropDownList)proveedor.FindControl("tipoGasto")).SelectedValue=="1" || ((DropDownList)proveedor.FindControl("tipoGasto")).SelectedValue == "5")
                    //((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","ModalDialogFacAdmin(this,'MACTIVOFIJO','SELECT mafj_codiacti AS Codigo,mafj_descripcion AS Descripcion,mafj_valohist AS Valor FROM mactivofijo')");
                    ((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick", "ModalDialog(this,'SELECT mafj_codiacti AS Codigo, mafj_descripcion AS Descripcion, mafj_valohist AS Valor FROM mactivofijo', new Array(),1)");
            }
		}
		
		private void gridDiferidos_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="Agregar_Gasto")
			{
				if((((TextBox)e.Item.Cells[0].Controls[1]).Text=="")||(((TextBox)e.Item.Cells[1].Controls[1]).Text==""))
                    Utils.MostrarAlerta(Response, "Debe especificar un tipo de gasto");
				else if(((TextBox)e.Item.Cells[3].Controls[1]).Text=="undefined")
                    Utils.MostrarAlerta(Response, "Debe introducir un diferido nuevo");
				else
				{
					if(Session["tablaDiferidos"]==null)
						this.Cargar_Tabla_Diferidos();
					fila=tablaDiferidos.NewRow();
					fila["CODIGOGASTO"]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila["CODIGODIF"]=((TextBox)e.Item.Cells[1].Controls[1]).Text;
					fila["NOMBREDIF"]=((TextBox)e.Item.Cells[2].Controls[1]).Text;
					fila["VALORDIF"]=(System.Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text)).ToString("C");
					tablaDiferidos.Rows.Add(fila);
					this.Mostrar_Grid_Diferidos();
					valorTotal.Text=(System.Convert.ToDouble(valorTotal.Text.Substring(1))+((System.Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text)))).ToString("C");
					pnlValores.Visible=true;
					if(tipoFactura.SelectedValue=="FC")
					{
						lbCosto.Visible=true;
						costoFac.Visible=true;
					}
					else
					{
						lbCosto.Visible=false;
						costoFac.Visible=false;
					}
					guardar.Enabled=true;

                    fldRtns.Visible = lbRet.Visible =fldIva.Visible = lbIva.Visible = true;
					//gridRtns.Visible=lbRet.Visible=true;
					//dgIva.Visible=lbIva.Visible=true;
					Mostrar_dgIva();
					if(Session["instFac"]!=null)
					{
						inserts.Add((string)Session["instFac"]);
						Session["inserts"]=inserts;
					}
					CalcularTotalFactura();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="Remover_Gasto")
			{
				valorTotal.Text=((System.Convert.ToDouble(valorTotal.Text.Substring(1)))-((System.Convert.ToDouble(tablaDiferidos.Rows[e.Item.DataSetIndex][3].ToString().Substring(1))))).ToString("C");
				if(valorTotal.Text=="$0.00")
				{
					guardar.Enabled=false;
					tablaRtns.Clear();
					this.Mostrar_gridRtns();

                    fldRtns.Visible = lbRet.Visible = false;
					gridRtns.Visible=lbRet.Visible=false;
				}
				if(this.tipoFactura.SelectedValue=="FP")
				{
					inserts.RemoveAt(e.Item.DataSetIndex);
					Session["inserts"]=inserts;
				}
				tablaDiferidos.Rows[e.Item.DataSetIndex].Delete();
				this.Mostrar_Grid_Diferidos();
				CalcularTotalFactura();
			}
		}
		
		protected void gridDiferidos_DataBound(object Sender,DataGridItemEventArgs e)
		{
			Control cliente=null,proveedor=null;
			if(encabezadoCliente.Controls.Count!=0)
				cliente=encabezadoCliente.Controls[0];
			if(encabezadoProveedor.Controls.Count!=0)
				proveedor=encabezadoProveedor.Controls[0];
			//Si es una factura de cliente cargo el modaldialog normal
			if(e.Item.ItemType==ListItemType.Footer)
			{
				if(tipoFactura.SelectedValue=="FC" && ((DropDownList)cliente.FindControl("tipoGasto")).SelectedValue=="2")
					((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","ModalDialog2(this,'SELECT mdif_codidife AS Codigo,mdif_nombdife AS Nombre,mdif_valohist AS Valor FROM mdiferido',new Array())");
				else if(tipoFactura.SelectedValue=="FP" && ((DropDownList)proveedor.FindControl("tipoGasto")).SelectedValue=="2")
                    //((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","ModalDialogFacAdmin(this,'MDIFERIDO','SELECT mdif_codidife AS Codigo,mdif_nombdife AS Nombre,mdif_valohist AS Valor FROM mdiferido')");
                    ((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick", "ModalDialog(this,'SELECT mdif_codidife AS Codigo,mdif_nombdife AS Nombre,mdif_valohist AS Valor FROM mdiferido',new Array(),1)");
            }
        }
		
		private void gridDetalles_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="Agregar_Gasto")
			{
				if(((TextBox)e.Item.Cells[0].Controls[1]).Text=="")
                    Utils.MostrarAlerta(Response, "Debe especificar un tipo de gasto");
				else if(((TextBox)e.Item.Cells[1].Controls[1]).Text=="")
                    Utils.MostrarAlerta(Response, "Por favor digite el detalle");
				else if((!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text))||(((TextBox)e.Item.Cells[3].Controls[1]).Text=="0"))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
				else
				{
					if(Session["tablaDetalles"]==null)
						this.Cargar_Tabla_Detalles();
					fila=tablaDetalles.NewRow();
					fila["CODIGOGASTO"]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila["DETALLE"]=((TextBox)e.Item.Cells[1].Controls[1]).Text;
					fila["DOCUREFE"]=((TextBox)e.Item.Cells[2].Controls[1]).Text;
					fila["VALORDETALLE"]=(System.Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text)).ToString("C");
					tablaDetalles.Rows.Add(fila);
					this.Mostrar_Grid_Detalles();
					valorTotal.Text=(System.Convert.ToDouble(valorTotal.Text.Substring(1))+((System.Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text)))).ToString("C");
					pnlValores.Visible=true;
					if(tipoFactura.SelectedValue=="FC")
					{
						lbCosto.Visible=true;
						costoFac.Visible=true;
					}
					else
					{
						lbCosto.Visible=false;
						costoFac.Visible=false;
					}
					guardar.Enabled=true;

                    fldRtns.Visible = lbRet.Visible = fldIva.Visible = lbIva.Visible = true;
					//gridRtns.Visible=lbRet.Visible=true;
					//dgIva.Visible=lbIva.Visible=true;
					Mostrar_dgIva();
					CalcularTotalFactura();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="Remover_Gasto")
			{
				valorTotal.Text=((System.Convert.ToDouble(valorTotal.Text.Substring(1)))-((System.Convert.ToDouble(tablaDetalles.Rows[e.Item.DataSetIndex][3].ToString().Substring(1))))).ToString("C");
				if(valorTotal.Text=="$0.00")
				{
					guardar.Enabled=false;
					tablaRtns.Clear();
					this.Mostrar_gridRtns();

                    fldRtns.Visible = lbRet.Visible = false;
					//gridRtns.Visible=lbRet.Visible=false;
				}
				tablaDetalles.Rows[e.Item.DataSetIndex].Delete();
				this.Mostrar_Grid_Detalles();
				CalcularTotalFactura();
			}
		}
		
		private void dgIva_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(e.CommandName=="AgregarIva")
			{
				if(!DatasToControls.ValidarDouble(((DropDownList)e.Item.Cells[0].FindControl("ddlPorcIva")).SelectedValue) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[0].FindControl("tbValIva")).Text))
                    Utils.MostrarAlerta(Response, "Algún dato es erroneo");
                else if (!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='" + ((TextBox)e.Item.Cells[2].FindControl("tbCuenta")).Text + "' and timp_codigo in ('A','P') AND tcue_codigo <> 'N'"))
                    Utils.MostrarAlerta(Response, "La cuenta especificada es inexistente o es NO Imputable o es solo NIIF");
				else
				{
					if(!ExisteIva(((DropDownList)e.Item.Cells[0].FindControl("ddlPorcIva")).SelectedValue,((DropDownList)e.Item.Cells[3].FindControl("ddlnit")).SelectedValue))
					{
						fila=dtIva.NewRow();
						fila[0]=Convert.ToDouble(((DropDownList)e.Item.Cells[0].FindControl("ddlPorcIva")).SelectedValue);
						fila[1]=Convert.ToDouble(((TextBox)e.Item.Cells[1].FindControl("tbValIvaBase")).Text);
						fila[2]=Convert.ToDouble(((TextBox)e.Item.Cells[2].FindControl("tbValIva")).Text);
						fila[3]=((TextBox)e.Item.Cells[3].FindControl("tbCuenta")).Text;
						fila[4]=((DropDownList)e.Item.Cells[4].FindControl("ddlnit")).SelectedValue;
						if(((RadioButton)e.Item.Cells[5].FindControl("rbiva1")).Checked)
							fila[5]=((RadioButton)e.Item.Cells[5].FindControl("rbiva1")).Text;
						else if(((RadioButton)e.Item.Cells[5].FindControl("rbiva2")).Checked)
							fila[5]=((RadioButton)e.Item.Cells[5].FindControl("rbiva2")).Text;
						if(!SuperaIvaaBase(Convert.ToDouble(fila[2]),fila[5].ToString()))
						{
							dtIva.Rows.Add(fila);
							Mostrar_dgIva();
							CalcularIva();
							CalcularTotalFactura();
						}
						else
						{
                            Utils.MostrarAlerta(Response, "El valor base del " + fila[5].ToString() + " supera el valor base. Revise por favor");
							return;
						}
					}
					else
                    Utils.MostrarAlerta(Response, "La relación nit - iva ya existe.");
				}
			}
			else if(e.CommandName=="RemoverIva")
			{
				dtIva.Rows[e.Item.ItemIndex].Delete();
				Mostrar_dgIva();
				CalcularIva();
				CalcularTotalFactura();
			}
		}
		
		private void LlenarDropDownListNits(DropDownList ddl)
		{
			ListItem item=null;
			Control ctl=null;
			if(tipoFactura.SelectedValue=="FC")
			{
				ctl=encabezadoCliente.Controls[0];
				if(!ExisteNit(((TextBox)ctl.FindControl("nit")).Text,ddl))
				{
					item=new ListItem(DBFunctions.SingleData("SELECT mnit_nit || ' - ' || mnit_apellidos ||' '|| COALESCE(mnit_apellido2,'') ||' '|| mnit_nombres ||' '|| COALESCE(mnit_nombre2,'') FROM dbxschema.mnit WHERE mnit_nit='"+((TextBox)ctl.FindControl("nit")).Text+"'"),((TextBox)ctl.FindControl("nit")).Text);
					ddl.Items.Add(item);
				}

			}
			else if(tipoFactura.SelectedValue=="FP")
			{
				ctl=encabezadoProveedor.Controls[0];
				if(!ExisteNit(((TextBox)ctl.FindControl("nit")).Text,ddl))
				{
					item=new ListItem(DBFunctions.SingleData("SELECT mnit_nit || ' - ' || mnit_apellidos ||' '|| COALESCE(mnit_apellido2,'') ||' '|| mnit_nombres ||' '|| COALESCE(mnit_nombre2,'') FROM dbxschema.mnit WHERE mnit_nit='"+((TextBox)ctl.FindControl("nit")).Text+"'"),((TextBox)ctl.FindControl("nit")).Text);
					ddl.Items.Add(item);
				}
			}
			for(int i=0;i<tablaNC.Rows.Count;i++)
			{
				if(!ExisteNit(tablaNC.Rows[i][6].ToString(),ddl))
				{
					item=new ListItem(DBFunctions.SingleData("SELECT mnit_nit || ' - ' || mnit_apellidos ||' '|| COALESCE(mnit_apellido2,'') ||' '|| mnit_nombres ||' '|| COALESCE(mnit_nombre2,'') FROM dbxschema.mnit WHERE mnit_nit='"+tablaNC.Rows[i][6].ToString()+"'"),tablaNC.Rows[i][6].ToString());
					ddl.Items.Add(item);
				}
			}
		}
		
		private bool ExisteNit(string nit,DropDownList ddl)
		{
			bool existe=false;
			for(int i=0;i<ddl.Items.Count;i++)
			{
				if(nit==ddl.Items[i].Value)
				{
					existe=true;
					break;
				}
			}
			return existe;
		}
		
		private void dgIva_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(e.Item.ItemType==ListItemType.Footer)
			{
				LlenarDropDownListNits(((DropDownList)e.Item.FindControl("ddlnit")));
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].FindControl("ddlPorcIva")),"SELECT piva_porciva FROM piva ORDER BY piva_porciva");

				TextBox txtBase=(TextBox)e.Item.FindControl("tbValIvaBase");
				DropDownList ddlPorcIva=(DropDownList)e.Item.FindControl("ddlPorcIva");
				TextBox txtValor=(TextBox)e.Item.FindControl("tbValIva");
				string scrTotal="PorcentajeVal('"+ddlPorcIva.ClientID+"','"+txtBase.ClientID+"','"+txtValor.ClientID+"');";
				txtBase.Attributes.Add("onkeyup","NumericMaskE(this,event);"+scrTotal);
				ddlPorcIva.Attributes.Add("onchange",scrTotal);
			}
		}

		private bool ExisteIva(string iva,string nit)
		{
			DataRow[] ivas=dtIva.Select("PORCENTAJE="+iva+" AND NIT='"+nit+"'");
			if(ivas.Length!=0)
				return true;
			else
				return false;
		}

		private bool FleteMenoraIva(double valrIvaFle)
		{
			bool menor=false;
			double valFlet=Convert.ToDouble(valorFletes.Text);
			if(valFlet<valrIvaFle)
				menor=true;
			return menor;
		}

		private bool SuperaIvaaBase(double valor,string tipo)
		{
			bool supera=false;
			double vi=0;
			for(int i=0;i<dtIva.Rows.Count;i++)
			{
				if(dtIva.Rows[i][5].ToString()==tipo)
					vi=vi+Convert.ToDouble(dtIva.Rows[i][2]);
			}
			vi=vi+valor;
			if(tipo=="IVA Fletes")
			{
				if(Convert.ToDouble(valorFletes.Text)<vi)
					supera=true;
			}
			else if(tipo=="IVA")
			{
                /*if(valorTotal.Text.Contains("($"))
                    {
                        if (Convert.ToDouble("-" + valorTotal.Text.Split('$')[1].Substring(0, valorTotal.Text.Length - 3)) < vi)
                            supera = true;
                    }
                    //String le = (Convert.ToDouble(valorTotal.Text)).ToString("C");
                */
                try
                {
                    if (Convert.ToDouble(valorTotal.Text.Substring(1)) < vi)
                        supera = true;
                }catch
                {
                    supera = true;
                }
                
			}
			return supera;
		}
		
		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{
			this.gridActivosFijos.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridActivosFijos_ItemCommand);
			this.gridActivosFijos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridActivosFijos_DataBound);
			this.gridDiferidos.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridDiferidos_ItemCommand);
			this.gridDiferidos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridDiferidos_DataBound);
			this.gridDetalles.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridDetalles_ItemCommand);
			this.gridNC.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridNC_ItemCommand);
			this.gridNC.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridNC_ItemDataBound);
			this.dgIva.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgIva_ItemCommand);
			this.dgIva.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgIva_ItemDataBound);

		}

        public void gridNC_Edit(Object sender, DataGridCommandEventArgs e)
        {
            if (tablaNC.Rows.Count > 0)
                gridNC.EditItemIndex = (int)e.Item.ItemIndex;

            gridNC.DataSource = tablaNC;
            gridNC.DataBind();
        }

        public void gridNC_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            gridNC.EditItemIndex = -1;

            gridNC.DataSource = tablaNC;
            gridNC.DataBind();
        }

        public void gridNC_Update(Object sender, DataGridCommandEventArgs e)
        {
            if (tablaNC.Rows.Count == 0)//Como no hay nada, no se pone a actualizar nada
                return;

            tablaNC.Rows[gridNC.EditItemIndex][0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][2] = ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue;
            tablaNC.Rows[gridNC.EditItemIndex][3] = ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue;
            tablaNC.Rows[gridNC.EditItemIndex][4] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][5] = ((TextBox)e.Item.Cells[5].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][6] = ((TextBox)e.Item.Cells[6].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][7] = double.Parse( ((TextBox)e.Item.Cells[7].Controls[1]).Text, NumberStyles.Currency);
            tablaNC.Rows[gridNC.EditItemIndex][8] = double.Parse( ((TextBox)e.Item.Cells[8].Controls[1]).Text, NumberStyles.Currency);
            tablaNC.Rows[gridNC.EditItemIndex][9] = double.Parse( ((TextBox)e.Item.Cells[9].Controls[1]).Text, NumberStyles.Currency);

            gridNC.EditItemIndex = -1;
            gridNC.DataSource = tablaNC;
            gridNC.DataBind();
            CalcularTotalFactura();
        }
        
		#endregion
	}
}
