// created on 27/04/2005 at 13:44
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
using Ajax;
using System.Globalization;

namespace AMS.Vehiculos
{
	public partial class GastoDirectoVehiculos : System.Web.UI.UserControl
	{
		#region Controles
		protected DataTable dtVehiculos, dtGastos, dtGuardar, tablaRtns;
		#endregion Controles
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string distGastosVehiculo = "";

		protected void Page_Load(object sender, System.EventArgs e)
		{

            Ajax.Utility.RegisterTypeForAjax(typeof(GastoDirectoVehiculos));
            DateTime time = DateTime.Now;              // Use current time
            fechaFact.Text = time.ToString("yyyy-MM-dd");
            if(!IsPostBack)
			{
				Session.Clear();
                distGastosVehiculo = DBFunctions.SingleData("select cveh_distgastvehi from dbxschema.cvehiculos;");
                Session["distGastosVehiculo"] = distGastosVehiculo;

				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='FP'");
             
                Utils.llenarPrefijos(Response, ref prefijoFactura , "%", "%", "FP");
                
                
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen, palm_descripcion FROM palmacen where (pcen_centvvn is not null  or pcen_centvvu is not null) and tvig_vigencia='V' order by palm_descripcion;");
				numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura.SelectedValue+"'");
				if(Request.QueryString["cons"]=="M")
					numeroFactura.ReadOnly = false;
				this.Preparar_Tabla_Vehiculos();
				this.Preparar_Tabla_Gastos();
				this.Bind_DgVehiculos();
				this.Bind_DgGastos();
			}
			
			if(Session["dtVehiculos"]==null)
				this.Preparar_Tabla_Vehiculos();
			else
				dtVehiculos = (DataTable)Session["dtVehiculos"];
			
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

        [Ajax.AjaxMethod()]
        public string ConsultarFecha(string venFec)
        {
            DateTime fechaI = Convert.ToDateTime(venFec);
            string numero = "30";
            string fechaTotal = (fechaI.AddDays(Convert.ToDouble(numero))).ToString("yyyy-MM-dd");
            return fechaTotal;
        }

        public void gridNC_Update(Object sender, DataGridCommandEventArgs e)
        {
            dtVehiculos.Rows[dgVehiculos.EditItemIndex][2] = double.Parse(((TextBox)e.Item.Cells[2].Controls[1]).Text, NumberStyles.Currency);
            dtVehiculos.Rows[dgVehiculos.EditItemIndex][4] = double.Parse(((TextBox)e.Item.Cells[4].Controls[1]).Text, NumberStyles.Currency);
            dgVehiculos.EditItemIndex = -1;
            dgVehiculos.DataSource = this.dtVehiculos;
            dgVehiculos.DataBind();

            CacularValorFactura();
        }

        private void CacularValorFactura()
        {
            double valorFacturaVeh = 0;

            for (int k = 0; k < dtVehiculos.Rows.Count; k++)
            {
                try
                {
                    valorFacturaVeh += double.Parse(dtVehiculos.Rows[k][4].ToString(), NumberStyles.Currency);
                }
                catch (Exception Er)
                {
                    valorFacturaVeh += 0;
                }
                
            }
            txtGastosVeh.Text = valorFacturaVeh.ToString("C");

            double totalDiferenciaVeh = double.Parse(valorFactura.Text, NumberStyles.Currency);
            totalDiferenciaVeh = totalDiferenciaVeh - valorFacturaVeh;
            txtDiferenciaVeh.Text = totalDiferenciaVeh.ToString("C"); ;
        }

        public void gridNC_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            dgVehiculos.EditItemIndex = -1;
            dgVehiculos.DataSource = this.dtVehiculos;
            dgVehiculos.DataBind();
        }

        public void gridNC_Edit(Object sender, DataGridCommandEventArgs e)
        {
            if (dtVehiculos.Rows.Count > 0)
                dgVehiculos.EditItemIndex = (int)e.Item.ItemIndex;

            dgVehiculos.DataSource = this.dtVehiculos;
            dgVehiculos.DataBind();

        }

		
		protected void Cambio_Documento(Object  Sender, EventArgs e)
		{
			numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura.SelectedValue+"'");
		}
		
		protected void Preparar_Tabla_Vehiculos()
		{
			dtVehiculos = new DataTable();
			dtVehiculos.Columns.Add(new DataColumn("CATALOGO",System.Type.GetType("System.String")));
			dtVehiculos.Columns.Add(new DataColumn("VIN",System.Type.GetType("System.String")));
			dtVehiculos.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.Double")));
			dtVehiculos.Columns.Add(new DataColumn("PORCENTAJE",System.Type.GetType("System.Double")));
			dtVehiculos.Columns.Add(new DataColumn("VALORFACT",System.Type.GetType("System.Double")));
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
			dtGuardar.Columns.Add(new DataColumn("NUMEROINVENTARIO",System.Type.GetType("System.String")));
			dtGuardar.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));
		}
		
		protected void LLenar_Tabla_Guardar()
		{
			this.Preparar_Tabla_Guardar();
			for(int i=0;i<dtVehiculos.Rows.Count;i++)
			{
				DataRow fila = dtGuardar.NewRow();
			//	fila["NUMEROINVENTARIO"] = DBFunctions.SingleData("SELECT mveh_inventario FROM mvehiculo WHERE pcat_codigo='"+dtVehiculos.Rows[i]["CATALOGO"].ToString()+"' AND mcat_vin='"+dtVehiculos.Rows[i]["VIN"].ToString()+"'");
                fila["NUMEROINVENTARIO"] = DBFunctions.SingleData("SELECT max(mveh_inventario) FROM mvehiculo WHERE mcat_vin='" + dtVehiculos.Rows[i]["VIN"].ToString() + "'");
                fila["VALOR"] = dtVehiculos.Rows[i]["VALORFACT"].ToString();
				dtGuardar.Rows.Add(fila);
			}
		}
		
		protected void DgVehiculos_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValuesVeh(e))
			{
				DataRow fila = dtVehiculos.NewRow();
				fila["CATALOGO"] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
				fila["VIN"] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
				fila["VALOR"] = System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
                             

                distGastosVehiculo = Session["distGastosVehiculo"].ToString();
                dtVehiculos.Rows.Add(fila);

                if (distGastosVehiculo == "N")
                {
                    fila["VALORFACT"] = 0;
                }
                
                Session["CommandName"] = ((Button)e.CommandSource).CommandName;
			}
            else if (((Button)e.CommandSource).CommandName == "AddDatasRow")
                    Utils.MostrarAlerta(Response, "No se puede Adicionar este vehiculo");
            if (((Button)e.CommandSource).CommandName == "DelDatasRow")
            {
                try
                {
                    dtVehiculos.Rows.Remove(dtVehiculos.Rows[e.Item.ItemIndex]);
                }
                catch
                {
                    dtVehiculos.Rows.Clear();
                }
            }
            if (((Button)e.CommandSource).CommandName == "AddDatasRow" || ((Button)e.CommandSource).CommandName == "DelDatasRow")
            {
                this.Total_Vehiculos();
                this.Bind_DgVehiculos();

                if (distGastosVehiculo == "S" || ((Button)e.CommandSource).CommandName == "DelDatasRow")
                {
                    CacularValorFactura();
                }
            }
		}
		
		protected void DgGastos_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValuesGas(e))
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
				DataRow fila = dtGastos.NewRow();
				fila["CODIGO"] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
				fila["NOMBRE"] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
				fila["ORIGINAL"] = Math.Round(System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text),0);
                //fila["VALOR"] = Math.Round(System.Convert.ToDouble(valorFactura.Text));
				fila["VALOR"] = Math.Round(System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text)*tasaC,0);
				fila["TASA"] = tasaC;
				dtGastos.Rows.Add(fila);
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
            if (e.Item.ItemType == ListItemType.Footer){
                
                TextBox txtValorGasto = (TextBox)e.Item.FindControl("valorInserts");
                txtValorGasto.Text = valorFactura.Text;
            }

			if(e.Item.ItemType==ListItemType.AlternatingItem || e.Item.ItemType==ListItemType.Item)
			{
                if (Convert.ToDouble(dtGastos.Rows[e.Item.ItemIndex]["TASA"]) > 1)
                {
                    e.Item.BackColor = Color.SeaGreen;
                   
                }
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
    	
		protected void Bind_DgVehiculos()
		{
			dgVehiculos.DataSource = dtVehiculos;
			Session["dtVehiculos"] = dtVehiculos;
			dgVehiculos.DataBind();

            distGastosVehiculo = Session["distGastosVehiculo"].ToString();
            if (Session["CommandName"] == "AddDatasRow" && distGastosVehiculo == "N")
            {
                
                dgVehiculos.EditItemIndex = dtVehiculos.Rows.Count-1;
                
                dgVehiculos.DataBind();
                Session["CommandName"] = "";
            }
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
			if(((TextBox)e.Item.Cells[1].Controls[1]).Text=="" || ((TextBox)e.Item.Cells[1].Controls[1]).Text=="")
				check = false;
			if(check)
			{
				for(int i=0;i<dtVehiculos.Rows.Count;i++)
				{
					if((dtVehiculos.Rows[i][0].ToString()==((TextBox)e.Item.Cells[1].Controls[1]).Text)&&(dtVehiculos.Rows[i][1].ToString()==((TextBox)e.Item.Cells[1].Controls[1]).Text))
						check = false;
				}
			}
			return check;
		}
		
		protected bool CheckValuesGas(DataGridCommandEventArgs e)
		{
			bool check = true;
			if(((TextBox)e.Item.Cells[1].Controls[1]).Text=="" || ((TextBox)e.Item.Cells[1].Controls[1]).Text=="" || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text))
				check = false;
			if(check)
			{
				for(int i=0;i<dtGastos.Rows.Count;i++)
				{
					if((dtGastos.Rows[i][0].ToString()==((TextBox)e.Item.Cells[1].Controls[1]).Text)&&(dtGastos.Rows[i][1].ToString()==((TextBox)e.Item.Cells[1].Controls[1]).Text))
						check = false;
				}
			}
			return check;
		}
		
		protected void Total_Vehiculos()
		{
			//Primero calculamos el total del VALORFACT de los vehiculos
			int i;
			double totalVehiculos = 0;
			for(i=0; i<dtVehiculos.Rows.Count;i++)
				totalVehiculos += System.Convert.ToDouble(dtVehiculos.Rows[i]["VALOR"]);
			//Al tener el total ahora calculmos el porcentaje de cada vehiculo
			double valorTotalFactura = System.Convert.ToDouble(valorFactura.Text);
			for(i=0; i<dtVehiculos.Rows.Count;i++)
			{
				double porcentaje = ((System.Convert.ToDouble(dtVehiculos.Rows[i]["VALOR"]))/(totalVehiculos));
				if(totalVehiculos==0)porcentaje=0;
                distGastosVehiculo = Session["distGastosVehiculo"].ToString();
                if (distGastosVehiculo == "N")
                {
                    dtVehiculos.Rows[i]["PORCENTAJE"] = 100;
                }
                else 
                {
                    dtVehiculos.Rows[i]["PORCENTAJE"] = Math.Round(porcentaje*100,0);
                    dtVehiculos.Rows[i]["VALORFACT"] = (valorTotalFactura*porcentaje);
                }
			}
			tlVehiculos.Text = totalVehiculos.ToString("C");
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
			if(!ValidarBasesRetencion())
			{
                Utils.MostrarAlerta(Response, "El valor de las retenciones no puede superar el valor de la factura");
				return;
            }
            if (Convert.ToDecimal(dtVehiculos.Compute("Sum(VALORFACT)", "")) != Convert.ToDecimal(valorFactura.Text) )
            {
                Utils.MostrarAlerta(Response, "Sumas Desiguales en Vehiculos Relacionados");
                return;
            }
         
            
            //if (!ValidarBasesRetencion())
            //{
            //    Utils.MostrarAlerta(Response, "El valor de las retenciones no puede superar el valor de la factura");
            //    return;
            //}
			//Primero verificamos que la suma del total de los gastos y de la factura sea igual a cero
			if( (Convert.ToDouble(valorFactura.Text)-Convert.ToDouble(tlGastos.Text.Substring(1))==0) && 
				( (Convert.ToDouble(valorFactura.Text)+Convert.ToDouble(txtIVA.Text))-Convert.ToDouble(txtTotal.Text)==0) )
			{
				if(dtVehiculos.Rows.Count>0 && dtGastos.Rows.Count>0)
				{
					//Aqui es donde debemos llamar el objeto que graba la factura de proveedor y los datos sobre los vehiculos afectados
					//primero llenamos la tabla de los vehiculos
					this.LLenar_Tabla_Guardar();
					//Ahora  creamos el objeto de tipo factura gastos
                                  
                    FacturaGastos miFacturaGastos   = new FacturaGastos("V", dtGuardar,dtGastos,dtVehiculos,tablaRtns);
					miFacturaGastos.PrefijoFactura  = prefijoFactura.SelectedValue;
					miFacturaGastos.NumeroFactura   = numeroFactura.Text;
					miFacturaGastos.Nit             = nitProveedor.Text;
					miFacturaGastos.PrefijoProveedor= prefFactProveedor.Text;
					miFacturaGastos.NumeroProveedor = numeFactProveedor.Text;
					miFacturaGastos.FechaFactura    = fechaFact.Text;
					miFacturaGastos.Almacen         = almacen.SelectedValue;
					miFacturaGastos.FechaVencimiento= fechaVencimiento.Text;
					miFacturaGastos.ValorFactura    = Convert.ToDouble(valorFactura.Text).ToString();
					miFacturaGastos.ValorIva        = Convert.ToDouble(txtIVA.Text).ToString();
					miFacturaGastos.Observacion     = observacion.Text;
					miFacturaGastos.Usuario         = HttpContext.Current.User.Identity.Name;
					if(miFacturaGastos.Guardar_Factura(true))
                        Response.Redirect("" + indexPage + "?process=Vehiculos.GastoDirecto&pref=" + prefijoFactura.SelectedValue + "&num=" + numeroFactura.Text);
					else
						lb.Text = miFacturaGastos.ProcessMsg;
				}
				else
                    Utils.MostrarAlerta(Response, "Alguna Tabla es Vacia");
			}
			else
                Utils.MostrarAlerta(Response, "Sumas Desiguales");
		}

		#region Retenciones

      
		protected void Seleccionar(Object  Sender, EventArgs e)
        {
            string existeFactura = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturaproveedor WHERE mnit_nit='" + nitProveedor.Text + "' and mfac_prefdocu = '" + prefFactProveedor.Text + "' and mfac_numedocu = " + numeFactProveedor.Text + " ");
             
            if (existeFactura == "" || existeFactura == null)
                {

                }

                else 
            {
                Utils.MostrarAlerta(Response, "El Numero de la factura o Prefijo de proveedor ya existen");
                return;
            }

            string existeNit = DBFunctions.SingleData("SELECT nombre FROM VMnit WHERE mnit_nit='" + nitProveedor.Text + "'  ");
            if (existeNit == "" || existeNit == null)
            {
                Utils.MostrarAlerta(Response, "El Nit No Existe");
                return;
            }
            else
                Nompro.Text = existeNit.ToString();

            if (txtIVA.Text == "")
            {
                Utils.MostrarAlerta(Response, "El valor del iva esta vacio");
                return;
            }
            //if (numeroFactura.Text == "")
            //{
            //    Utils.MostrarAlerta(Response, "El numero de digitado factura ya existe");
            //    return;
            //}

            if (prefijoFactura.SelectedValue == "Seleccione..." || numeroFactura.Text == "" || prefFactProveedor.Text == "" || numeFactProveedor.Text == "" || fechaFact.Text == "" || fechaVencimiento.Text == "" || valorFactura.Text == "" || txtTotal.Text == "")
            {
                Utils.MostrarAlerta(Response, "El Documento de Entrada o a la factura de Proveedor estan erradas");
                return;
            }
            
            double valFac = Convert.ToDouble(valorFactura.Text);
            double valIva = Convert.ToDouble(txtIVA.Text);
            double valTotal = Convert.ToDouble(txtTotal.Text);

            if (valFac + valIva == valTotal)
            {
                if (nitProveedor.Text.Length == 0)
                {
                    Utils.MostrarAlerta(Response, "Debe seleccionar el proveedor");
                    return;
                }
                nitProveedor.Enabled = false;
                btnSeleccionar.Visible = false;
                pnlDetalles.Visible = true;
                gridRtns.DataSource = tablaRtns;
                gridRtns.DataBind();

                dgGastos.DataSource = dtGastos;
                dgGastos.DataBind();

               
            }

            else
            {
                Utils.MostrarAlerta(Response, "La suma de Valor Factura y Valor Iva, no coincide con el Valor Total de la Factura!");
                return;
            }
             
         

   		}
      
		protected void Mostrar_gridRtns()
		{
			gridRtns.DataSource=tablaRtns;
			gridRtns.DataBind();
			if(tablaRtns.Rows.Count==0)
				btnGrabar.Attributes.Add("onClick","return confirm('No ha agregado retenciones.');");
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
            tablaRtns.Columns.Add(new DataColumn("TIPORETE", typeof(string)));
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
				if((((TextBox)e.Item.Cells[1].Controls[1]).Text==""))
                    Utils.MostrarAlerta(Response, "Debe escoger un tipo de retención");
				else if(this.Buscar_Retencion(((TextBox)e.Item.Cells[1].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Solo puede haber una retención de ese tipo");
				else if(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
				else
				{
					fila=tablaRtns.NewRow();
					fila["CODRET"]=((TextBox)e.Item.Cells[1].Controls[1]).Text;
					fila["PORCRET"]=Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
                    fila["VALORBASE"]=Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
                    fila["TIPORETE"] = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
                    //if (((TextBox)e.Item.Cells[3].Controls[1]).Text == "")
                    //    fila["VALOR"] = Convert.ToDouble(0);
                    if (((TextBox)e.Item.Cells[4].Controls[1]).Text == "")
                        Utils.MostrarAlerta(Response, "Ingrese un valor");
                    else 
                    {
                        fila["VALOR"] = Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
                        tablaRtns.Rows.Add(fila);
                        this.Mostrar_gridRtns();
                        TotalRetenciones();
                    }
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
			if(e.Item.ItemType==ListItemType.Footer)
			{
                Retencion rtns = new Retencion(nitProveedor.Text, false);
                DropDownList ddlTiporetencion = (DropDownList)e.Item.FindControl("ddlTiporet");

                if (rtns.TipoSociedad == null)
                    rtns.TipoSociedad = "";

                TextBox txtBase=(TextBox)e.Item.FindControl("base");
				TextBox txtPorcentaje=(TextBox)e.Item.FindControl("codretb");
				TextBox txtValor=(TextBox)e.Item.FindControl("valor");

                txtBase.Text = valorFactura.Text;
				string scrTotal="PorcentajeVal('"+txtPorcentaje.ClientID+"','"+txtBase.ClientID+"','"+txtValor.ClientID+"');";
				txtBase.Attributes.Add("onkeyup","NumericMaskE(this,event);"+scrTotal);
                
                DatasToControls bind = new DatasToControls();

                ddlTiporetencion.Attributes.Add("onChange", "Cambio_Retencion(this," + ((TextBox)e.Item.Cells[1].Controls[1]).ClientID + ",'" + rtns.TipoSociedad + "','" + txtPorcentaje.ClientID + "','" + txtBase.ClientID + "','" + txtValor.ClientID + "')");
                
                bind.PutDatasIntoDropDownList(ddlTiporetencion, "select * from tretencion order by 2;");
                ddlTiporetencion.Items.Insert(0, new ListItem("Seleccione...", "0"));
			}
		}

        [Ajax.AjaxMethod]
        public DataSet Cargar_Nombre(string Cedula)
        {
            DataSet Vins = new DataSet();
            DBFunctions.Request(Vins, IncludeSchema.NO, "select mnit_nit from dbxschema.mnit where mnit_nit like '" + Cedula + "%';select mnit_nombres concat ' ' CONCAT COALESCE(mnit_nombre2,'') concat ' 'concat mnit_apellidos concat ' 'concat COALESCE(mnit_apellido2,'') as NOMBRE from dbxschema.mnit where mnit_nit='" + Cedula + "'");
            return Vins;

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

		}
		#endregion
	}
}
