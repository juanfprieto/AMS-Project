using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Vehiculos
{

	/// <summary>
	///		Descripción breve de AMS_Embarques_GastoDirectoEmbarques.
	/// </summary>
	public partial class AMS_Vehiculos_GastoDirectoEmbarques : System.Web.UI.UserControl
	{
		#region Controles
		protected DataTable dtEmbarques, dtGastos, dtGuardar,tablaRtns;
		#endregion Controles

		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='FP'");
                Utils.llenarPrefijos(Response, ref prefijoFactura , "%", "%", "FP");
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen, palm_descripcion FROM palmacen where (pcen_centvvn is not null  or pcen_centvvu is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
				numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura.SelectedValue+"'");
				if(Request.QueryString["cons"]=="M")
					numeroFactura.ReadOnly = false;
				this.Preparar_Tabla_Embarques();
				this.Preparar_Tabla_Gastos();
				this.Bind_DgEmbarques();
				this.Bind_DgGastos();
				ViewState["TASA"]= 1;
			}
			
			if(Session["dtEmbarques"]==null)
				this.Preparar_Tabla_Embarques();
			else
				dtEmbarques = (DataTable)Session["dtEmbarques"];
			
			if(Session["dtGastos"]==null)
				this.Preparar_Tabla_Gastos();
			else
				dtGastos = (DataTable)Session["dtGastos"];
			
			if(Session["tablaRtns"]==null){
				this.Cargar_Tabla_Rtns();
				this.Mostrar_gridRtns();
			}
			else
				tablaRtns=(DataTable)Session["tablaRtns"];
		}
		
		protected void Cambio_Documento(Object  Sender, EventArgs e)
		{
			numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura.SelectedValue+"'");
		}
		
		protected void Preparar_Tabla_Embarques()
		{
			dtEmbarques = new DataTable();
			dtEmbarques.Columns.Add(new DataColumn("SECUENCIA",System.Type.GetType("System.String")));
			dtEmbarques.Columns.Add(new DataColumn("NUMERO",System.Type.GetType("System.String")));
			dtEmbarques.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.Double")));
			dtEmbarques.Columns.Add(new DataColumn("PORCENTAJE",System.Type.GetType("System.Double")));
			dtEmbarques.Columns.Add(new DataColumn("VALORFACT",System.Type.GetType("System.Double")));
		}
		
		protected void Preparar_Tabla_Gastos()
		{
			dtGastos = new DataTable();
			dtGastos.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			dtGastos.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			dtGastos.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.Double")));
			dtGastos.Columns.Add(new DataColumn("TASA",System.Type.GetType("System.Double")));
			dtGastos.Columns.Add(new DataColumn("ORIGINAL",System.Type.GetType("System.Double")));
		}
		
		protected void Preparar_Tabla_Guardar()
		{
			dtGuardar = new DataTable();
			dtGuardar.Columns.Add(new DataColumn("NUMEROINVENTARIO",System.Type.GetType("System.Double")));
			dtGuardar.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));
		}
		
		protected void LLenar_Tabla_Guardar()
		{
			this.Preparar_Tabla_Guardar();
			for(int i=0;i<dtEmbarques.Rows.Count;i++)
			{
				DataRow fila = dtGuardar.NewRow();
				fila["NUMEROINVENTARIO"] = dtEmbarques.Rows[i]["SECUENCIA"].ToString();
				fila["VALOR"] = dtEmbarques.Rows[i]["VALORFACT"].ToString();
				dtGuardar.Rows.Add(fila);
			}
		}
		
		protected void DgEmbarques_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValuesVeh(e))
			{
				DataRow fila = dtEmbarques.NewRow();
				fila["SECUENCIA"] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
				fila["NUMERO"] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
				fila["VALOR"] = System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
				dtEmbarques.Rows.Add(fila);
			}
			else if(!CheckValuesVeh(e))
            Utils.MostrarAlerta(Response, "No se puede Adicionar este embarque");
			if(((Button)e.CommandSource).CommandName == "DelDatasRow")
			{
				try
				{
					dtEmbarques.Rows.Remove(dtEmbarques.Rows[e.Item.ItemIndex]);
				}
				catch
				{
					dtEmbarques.Rows.Clear();
				}     		
			}
			this.Total_Embarques();
			this.Bind_DgEmbarques();
		}
		
		protected void DgGastos_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValuesGas(e))
			{
				
				double tasaC=1;
				if(DBFunctions.SingleData("SELECT PGAS_MODENACI FROM PGASTODIRECTO WHERE PGAS_CODIGO='"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"'")=="N")
				{
					try{
						tasaC=Convert.ToDouble(txtTasa.Text);
						if(tasaC<=0)throw(new Exception());
					}
					catch{
                        Utils.MostrarAlerta(Response, "Tasa de cambio no valida");
						return;
					}

				}
				DataRow fila = dtGastos.NewRow();
				fila["CODIGO"] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
				fila["NOMBRE"] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
				fila["ORIGINAL"] = Math.Round(System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text),0);
		//		fila["VALOR"]    = Math.Round(System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text)*tasaC,0);
				fila["VALOR"]    = Math.Round(System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text),4);
				fila["TASA"] = tasaC;
				dtGastos.Rows.Add(fila);
				ViewState["TASA"]= tasaC;
			}
			if(((Button)e.CommandSource).CommandName == "DelDatasRow")
			{
				try
				{
					dtGastos.Rows.Remove(dtGastos.Rows[e.Item.ItemIndex]);
				}
				catch
				{
					dtGastos.Rows.Clear();
				}     		
			}
			this.Total_Gastos();
			this.Bind_DgGastos();
		}
		
		public void DgGastos_Edit(Object sender, DataGridCommandEventArgs e)
		{
			dgGastos.EditItemIndex = (int)e.Item.ItemIndex;
			this.Bind_DgGastos();
		}
		protected void dgGastos_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.AlternatingItem || e.Item.ItemType==ListItemType.Item)
			{
				if(Convert.ToDouble(dtGastos.Rows[e.Item.ItemIndex]["TASA"])>1)
					e.Item.BackColor=Color.SeaGreen;
			}
		}
		public void DgGastos_Update(Object sender, DataGridCommandEventArgs e)
		{
			double tasaC=1;
			if(DBFunctions.SingleData("SELECT PGAS_MODENACI FROM PGASTODIRECTO WHERE PGAS_CODIGO='"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"'")=="N")
			{
				try
				{
					tasaC=Convert.ToDouble(txtTasa.Text);
					if(tasaC<=0)throw(new Exception());
				}
				catch
				{
                    Utils.MostrarAlerta(Response, "Tasa de cambio no valida");
					return;
				}

			}
			dtGastos.Rows[dgGastos.EditItemIndex][0] = ((TextBox)e.Item.FindControl("gastEdit")).Text;
			dtGastos.Rows[dgGastos.EditItemIndex][1] = ((TextBox)e.Item.FindControl("gastEdita")).Text;
			dtGastos.Rows[dgGastos.EditItemIndex][2] = System.Convert.ToDouble(((TextBox)e.Item.FindControl("valorEdit")).Text);
			dtGastos.Rows[dgGastos.EditItemIndex][3] = tasaC;
			dgGastos.EditItemIndex = -1;
			this.Bind_DgGastos();
		}
		
		public void DgGastos_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgGastos.EditItemIndex = -1;
			this.Bind_DgGastos();
		}
    	
		protected void Bind_DgEmbarques()
		{
			dgEmbarques.DataSource = dtEmbarques;
			Session["dtEmbarques"] = dtEmbarques;
			dgEmbarques.DataBind();
		}
		
		protected void Bind_DgGastos()
		{
			dgGastos.DataSource = dtGastos;
			Session["dtGastos"] = dtGastos;
			dgGastos.DataBind();
		}
		
		protected bool CheckValuesVeh(DataGridCommandEventArgs e)
		{
			bool check = true;
			if(((TextBox)e.Item.Cells[0].Controls[1]).Text=="" || ((TextBox)e.Item.Cells[1].Controls[1]).Text=="")
				check = false;
			if(check)
			{
				for(int i=0;i<dtEmbarques.Rows.Count;i++)
				{
					if((dtEmbarques.Rows[i][0].ToString()==((TextBox)e.Item.Cells[0].Controls[1]).Text)&&(dtEmbarques.Rows[i][1].ToString()==((TextBox)e.Item.Cells[1].Controls[1]).Text))
						check = false;
				}
			}
			return check;
		}
		
		protected bool CheckValuesGas(DataGridCommandEventArgs e)
		{
			bool check = true;
			if(((TextBox)e.Item.Cells[0].Controls[1]).Text=="" || ((TextBox)e.Item.Cells[1].Controls[1]).Text=="" || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text))
				check = false;
			if(check)
			{
				for(int i=0;i<dtGastos.Rows.Count;i++)
				{
					if((dtGastos.Rows[i][0].ToString()==((TextBox)e.Item.Cells[0].Controls[1]).Text)&&(dtGastos.Rows[i][1].ToString()==((TextBox)e.Item.Cells[1].Controls[1]).Text))
						check = false;
				}
			}
			return check;
		}
		
		protected void Total_Embarques()
		{
			//Primero calculamos el total del valor de los embarques
			int i;
			double totalEmbarques = 0;
			for(i=0; i<dtEmbarques.Rows.Count;i++)
				totalEmbarques += System.Convert.ToDouble(dtEmbarques.Rows[i]["VALOR"]);
			//Al tener el total ahora calculmos el porcentaje de cada embarque
			double valorTotalFactura = System.Convert.ToDouble(valorFactura.Text);
			for(i=0; i<dtEmbarques.Rows.Count;i++)
			{
				double porcentaje = ((System.Convert.ToDouble(dtEmbarques.Rows[i]["VALOR"]))/(totalEmbarques));
				if(totalEmbarques==0)porcentaje=0;
				dtEmbarques.Rows[i]["PORCENTAJE"] = Math.Round(porcentaje*100,0);
				dtEmbarques.Rows[i]["VALORFACT"] = (valorTotalFactura*porcentaje);
			}
			tlEmbarques.Text = totalEmbarques.ToString("C");
		}
		
		protected void Total_Gastos()
		{
			double totalGastos = 0;
			double valorTotalFactura = System.Convert.ToDouble(valorFactura.Text);
			for(int i=0;i<dtGastos.Rows.Count;i++)
				totalGastos += System.Convert.ToDouble(dtGastos.Rows[i]["VALOR"]);
			tlGastos.Text = totalGastos.ToString("C");
			tlDiferencia.Text = (valorTotalFactura-totalGastos).ToString("C");
		}
		
		protected void Cancelar_Proceso(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Vehiculos.GastoDirecto");
		}
		
		protected void Realizar_Proceso(Object  Sender, EventArgs e)
		{
  			double valGasT = System.Convert.ToDouble(tlGastos.Text.Substring(1));
			double valTotF = Convert.ToDouble(txtTotal.Text);
			double valIvaT = Convert.ToDouble(txtIVA.Text);
	
			if(!ValidarBasesRetencion())
			{
                Utils.MostrarAlerta(Response, "El valor de las retenciones no puede superar el valor de la factura");
				return;
			}
			//Primero verificamos que la suma del total de los gastos y de la factura sea igual a cero
			if (tlGastos.Text.Length<=1)
				tlGastos.Text="000";
//			if ((( Convert.ToDouble(valorFactura.Text)-Convert.ToDouble(tlGastos.Text.Substring(1)))==0) && 
//				(((Convert.ToDouble(valorFactura.Text)+Convert.ToDouble(txtIVA.Text))-Convert.ToDouble(txtTotal.Text))==0) )

			if  (( Convert.ToDouble(valorFactura.Text)-valGasT)!= 0)
                Utils.MostrarAlerta(Response, "Sumas Desiguales en Total Gastos");
		    else
				if	((Math.Round((valGasT+valIvaT),2)-valTotF)!=0)  
                    Utils.MostrarAlerta(Response, "Sumas Desiguales en Total Factura");
				else
				{
					if(dtEmbarques.Rows.Count>0 && dtGastos.Rows.Count>0)
					{
						//double porcentajeIva = System.Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
						//Aqui es donde debemos llamar el objeto que graba la factura de proveedor y los datos sobre los embarques afectados
						//primero llenamos la tabla de los embarques
						this.LLenar_Tabla_Guardar();
						//Ahora  creamos el objeto de tipo factura gastos
						FacturaGastos miFacturaGastos = new FacturaGastos("E",dtGuardar,dtGastos,dtEmbarques,tablaRtns);
						miFacturaGastos.PrefijoFactura = prefijoFactura.SelectedValue;
						miFacturaGastos.NumeroFactura = numeroFactura.Text;
						miFacturaGastos.Nit = nitProveedor.Text;
						miFacturaGastos.PrefijoProveedor = prefFactProveedor.Text;
						miFacturaGastos.NumeroProveedor = numeFactProveedor.Text;
						miFacturaGastos.FechaFactura = fechaFact.Text;
						miFacturaGastos.Almacen      = almacen.SelectedValue;
						miFacturaGastos.FechaVencimiento = fechaVencimiento.Text;
						miFacturaGastos.ValorFactura = Math.Round((Convert.ToDouble(valorFactura.Text)*Convert.ToDouble(ViewState["TASA"])),0).ToString();
						miFacturaGastos.ValorIva     = Math.Round((Convert.ToDouble(txtIVA.Text)*Convert.ToDouble(ViewState["TASA"])),0).ToString();
						if (Convert.ToDouble(ViewState["TASA"])== 1)
							miFacturaGastos.Observacion = observacion.Text;
		                else
							miFacturaGastos.Observacion = observacion.Text +" Tasa de Cambio "+ txtTasa.Text;
						miFacturaGastos.Usuario = HttpContext.Current.User.Identity.Name;
						if(miFacturaGastos.Guardar_Factura(true))
							Response.Redirect("" + indexPage + "?process=Vehiculos.GastoDirecto");
						else
							lb.Text = miFacturaGastos.ProcessMsg;
					}
					else
                    Utils.MostrarAlerta(Response, "Alguna Tabla es Vacia");
				}
		}

		#region Retenciones
		protected void Seleccionar(Object  Sender, EventArgs e)
		{
			if(nitProveedor.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar el proveedor");
				return;
			}
			nitProveedor.Enabled=false;
			btnSeleccionar.Visible=false;
			pnlDetalles.Visible=true;
		}
		protected void Mostrar_gridRtns()
		{
			gridRtns.DataSource=tablaRtns;
			gridRtns.DataBind();
			if(tablaRtns.Rows.Count==0)
				btnGrabar.Attributes.Add("onClick","return confirm('No ha agregado retenciones. Continua ?');");
			else
				btnGrabar.Attributes.Remove("onClick");
			Session["tablaRtns"]=tablaRtns;
		}
		protected void Cargar_Tabla_Rtns()
		{
			tablaRtns=new DataTable();
			tablaRtns.Columns.Add(new DataColumn("CODRET", typeof(string)));
			tablaRtns.Columns.Add(new DataColumn("PORCRET", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALOR", typeof(double)));
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
		
		protected bool ValidarBasesRetencion()
		{
			double baseT=0;
			double totalF=Convert.ToDouble(valorFactura.Text);
			for(int n=0;n<tablaRtns.Rows.Count;n++)
				baseT+=Convert.ToDouble(tablaRtns.Rows[n]["VALOR"]);
			return(baseT<=totalF);
		}

		private void TotalRetenciones()
		{
			double totalR=0;
			foreach(DataRow drRet in tablaRtns.Rows)
			{
				totalR+=Convert.ToDouble(drRet["VALOR"]);
			}
			txtRetenciones.Text=Math.Round(totalR,0).ToString("#,###");
		}
		protected void gridRtns_Item(object Sender,DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="AgregarRetencion")
			{
				if((((TextBox)e.Item.Cells[0].Controls[1]).Text==""))
                    Utils.MostrarAlerta(Response, "Debe escoger un tipo de retención");
				else if(this.Buscar_Retencion(((TextBox)e.Item.Cells[0].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Solo puede haber una retención de ese tipo");
				else if(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
				else
				{
					fila=tablaRtns.NewRow();
					fila["CODRET"]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila["PORCRET"]=Convert.ToDouble(((TextBox)e.Item.Cells[1].Controls[1]).Text);
					fila["VALORBASE"]=Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
					fila["VALOR"]=Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
					tablaRtns.Rows.Add(fila);
					this.Mostrar_gridRtns();
					TotalRetenciones();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverRetencion")
			{
				tablaRtns.Rows[e.Item.DataSetIndex].Delete();
				this.Mostrar_gridRtns();
				TotalRetenciones();
			}
		}

		protected void gridRtns_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
			/*Control proveedor=null,cliente=null;
			if(encabezadoProveedor.Controls.Count!=0)
				proveedor=encabezadoProveedor.Controls[0];
			if(encabezadoCliente.Controls.Count!=0)
				cliente=encabezadoCliente.Controls[0];*/

			if(e.Item.ItemType==ListItemType.Footer)
			{
				TextBox txtBase=(TextBox)e.Item.FindControl("base");
				TextBox txtPorcentaje=(TextBox)e.Item.FindControl("codretb");
				TextBox txtValor=(TextBox)e.Item.FindControl("valor");
				string scrTotal="PorcentajeVal('"+txtPorcentaje.ClientID+"','"+txtBase.ClientID+"','"+txtValor.ClientID+"');";
				txtBase.Attributes.Add("onkeyup","NumericMaskE(this,event);"+scrTotal);

				/*if(tipoFactura.SelectedValue=="FC")
					//((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pret_codigo,pret_nombre,pret_porcennodecl FROM pretencion ORDER BY tret_codigo',new Array());"+scrTotal);
				else 
				if(tipoFactura.SelectedValue=="FP")*/
				{
					Retencion rtns=new Retencion(nitProveedor.Text,false);
					if(rtns.TipoSociedad=="N" || rtns.TipoSociedad=="U")
						((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcennodecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr where pr.ttip_codigo IN (\\'N\\',\\'T\\') ORDER BY tipo;',new Array());"+scrTotal);
					else 
						((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcendecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr where pr.ttip_codigo IN (\\'J\\',\\'T\\') ORDER BY tipo;',new Array());"+scrTotal);
				}
			}
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

	}
}
