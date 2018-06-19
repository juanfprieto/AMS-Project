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
	public partial class DevolucionGastoDirectoVehiculos : System.Web.UI.UserControl
	{
		#region Controles
		protected DataTable dtVehiculos, dtGastos, dtGuardar, tablaRtns;
		#endregion Controles
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string distGastosVehiculo = "";
		DatasToControls bind = new DatasToControls();
		protected void Page_Load(object sender, System.EventArgs e)
		{

			Ajax.Utility.RegisterTypeForAjax(typeof(DevolucionGastoDirectoVehiculos));
			DateTime time = DateTime.Now;              // Use current time
			fechaFact.Text = time.ToString("yyyy-MM-dd");
			if (!IsPostBack)
			{
				Session.Clear();
				distGastosVehiculo = DBFunctions.SingleData("select cveh_distgastvehi from dbxschema.cvehiculos;");
				Session["distGastosVehiculo"] = distGastosVehiculo;

				
				//bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='FP'");

				Utils.llenarPrefijos(Response, ref prefijoFactura, "%", "%", "NP");


				
				numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura.SelectedValue + "'");
				if (Request.QueryString["cons"] == "M")
					numeroFactura.ReadOnly = false;
				this.Preparar_Tabla_Vehiculos();
				this.Preparar_Tabla_Gastos();
				//this.Bind_DgVehiculos();
				this.Bind_DgGastos();
			}

			if (Session["dtVehiculos"] == null)
				this.Preparar_Tabla_Vehiculos();
			else
				dtVehiculos = (DataTable)Session["dtVehiculos"];

			if (Session["dtGastos"] == null)
				this.Preparar_Tabla_Gastos();
			else
				dtGastos = (DataTable)Session["dtGastos"];

			if (Session["tablaRtns"] == null)
			{
				this.Cargar_Tabla_Rtns();
				this.Mostrar_gridRtns();
			}
			else
				tablaRtns = (DataTable)Session["tablaRtns"];
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


		protected void Cambio_Documento(Object Sender, EventArgs e)
		{
			numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura.SelectedValue + "'");
		}

        protected void Cargue_PrefijoDocumento(Object Sender, EventArgs e)
        {
            if (tipoInclusion.SelectedValue == "V")
            {
                bind.PutDatasIntoDropDownList(prefFactProveedor, @"select DISTINCT MFP.pdoc_codiordepago, MFP.pdoc_codiordepago || ' - ' || pd.pdoc_nombre 
                                                                     FROM mfacturaproveedor MFP, DFACTURAGASTOVEHICULO DGV, pdocumento pd
																    WHERE MFP.pdoc_codiordepago  =  DGV.pdoc_codiordepago
																    AND MFP.mfac_numeordepago  =  DGV.mfac_numeordepago
																    AND PD.PDOC_CODIGO = MFP.pdoc_codiordepago
																    AND MNIT_NIT ='" + nitProveedor.Text + "';");

                prefFactProveedor.Items.Insert(0, new ListItem("Seleccione...", "0"));
            }
            if (tipoInclusion.SelectedValue == "E")
            {
               bind.PutDatasIntoDropDownList(prefFactProveedor, @"select DISTINCT MFP.pdoc_codiordepago, MFP.pdoc_codiordepago || ' - ' || pd.pdoc_nombre 
                                                                 FROM mfacturaproveedor MFP, DFACTURAGASTO DFG, pdocumento pd
																WHERE MFP.pdoc_codiordepago  =  DFG.pdoc_codiordepago
																AND MFP.mfac_numeordepago  =  DFG.mfac_numeordepago
																AND PD.PDOC_CODIGO = MFP.pdoc_codiordepago
																AND MNIT_NIT ='" + nitProveedor.Text + "';");
                prefFactProveedor.Items.Insert(0, new ListItem("Seleccione...", "0"));

            }
                
        }

        protected void Cargue_Documento(Object Sender, EventArgs e)
		{ 
            if (tipoInclusion.SelectedValue == "V") 
            { 
			    bind.PutDatasIntoDropDownList(numeFactProveedor, @"select DISTINCT MFP.mfac_numeordepago FROM mfacturaproveedor MFP, DFACTURAGASTOVEHICULO DGV
																    WHERE MFP.pdoc_codiordepago  =  DGV.pdoc_codiordepago
																    AND MFP.mfac_numeordepago  =  DGV.mfac_numeordepago
                                                                    AND MFP.pdoc_codiordepago = '" + prefFactProveedor.SelectedValue + @"'
                                                                    AND MFP.MNIT_NIT ='" + nitProveedor.Text + "';");

			    numeFactProveedor.Items.Insert(0, new ListItem("Seleccione...", "0"));
            }

            if (tipoInclusion.SelectedValue == "E") 
            {
                bind.PutDatasIntoDropDownList(numeFactProveedor, @"select DISTINCT MFP.mfac_numeordepago FROM mfacturaproveedor MFP, DFACTURAGASTO DFG
																WHERE MFP.pdoc_codiordepago  =  DFG.pdoc_codiordepago
																AND MFP.mfac_numeordepago  =  DFG.mfac_numeordepago
                                                                AND MFP.pdoc_codiordepago = '" + prefFactProveedor.SelectedValue + @"'
																AND MFP.MNIT_NIT ='" + nitProveedor.Text + "';");
																

                numeFactProveedor.Items.Insert(0, new ListItem("Seleccione...", "0"));
            }
		}
        protected void Cargar_Datos(Object Sender, EventArgs e)
		{
			fechaFact.Text = System.Convert.ToDateTime(DBFunctions.SingleData("select MFAC_FACTURA from mfacturaproveedor  where pdoc_codiordepago ='" + prefFactProveedor.SelectedValue + "' and mfac_numeordepago  = '" + numeFactProveedor.SelectedValue + "'")).ToString("yyyy-MM-dd");
			valorFactura.Text = Convert.ToDouble(DBFunctions.SingleData("select MFAC_VALOFACT from mfacturaproveedor  where pdoc_codiordepago ='" + prefFactProveedor.SelectedValue + "' and mfac_numeordepago  = '" + numeFactProveedor.SelectedValue + "'")).ToString("C");
			fechaVencimiento.Text = System.Convert.ToDateTime(DBFunctions.SingleData("select MFAC_VENCE from mfacturaproveedor  where pdoc_codiordepago ='" + prefFactProveedor.SelectedValue + "' and mfac_numeordepago  = '" + numeFactProveedor.SelectedValue + "'")).ToString("yyyy-MM-dd");
			observacion.Text = DBFunctions.SingleData("select MFAC_OBSERVACION from mfacturaproveedor  where pdoc_codiordepago ='" + prefFactProveedor.SelectedValue + "' and mfac_numeordepago  = '" + numeFactProveedor.SelectedValue + "'");
			txtIVA.Text = Convert.ToDouble(DBFunctions.SingleData("select MFAC_VALOIVA from mfacturaproveedor  where pdoc_codiordepago ='" + prefFactProveedor.SelectedValue + "' and mfac_numeordepago  = '" + numeFactProveedor.SelectedValue + "'")).ToString("C");
			txtRetenciones.Text =  Convert.ToDouble(DBFunctions.SingleData("select MFAC_VALORETE from mfacturaproveedor  where pdoc_codiordepago ='" + prefFactProveedor.SelectedValue + "' and mfac_numeordepago  = '" + numeFactProveedor.SelectedValue + "'")).ToString("C");
			//txtTotal.Text = Convert.ToDouble(DBFunctions.SingleData("select MFAC_VALOFACT from mfacturaproveedor  where pdoc_codiordepago ='" + prefFactProveedor.SelectedValue + "' and mfac_numeordepago  = '" + numeFactProveedor.SelectedValue + "'")).ToString("C");
            double Suma = double.Parse(valorFactura.Text, NumberStyles.Currency) + double.Parse(txtIVA.Text, NumberStyles.Currency);
            txtTotal.Text = Convert.ToDouble(Suma).ToString ("C");
            bind.PutDatasIntoDropDownList(almacen, "select MFP.PALM_ALMACEN, P.palm_descripcion  from mfacturaproveedor MFP, PALMACEN P  where pdoc_codiordepago = '" + prefFactProveedor.SelectedValue + "' and mfac_numeordepago  =  '" + numeFactProveedor.SelectedValue + "' AND MFP.PALM_ALMACEN = P.PALM_ALMACEN;");
		}


		protected void Preparar_Tabla_Vehiculos()
		{
			dtVehiculos = new DataTable();
			dtVehiculos.Columns.Add(new DataColumn("CATALOGO", System.Type.GetType("System.String")));
			dtVehiculos.Columns.Add(new DataColumn("VIN", System.Type.GetType("System.String")));
			dtVehiculos.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.Double")));
			dtVehiculos.Columns.Add(new DataColumn("PORCENTAJE", System.Type.GetType("System.Double")));
			dtVehiculos.Columns.Add(new DataColumn("VALORFACT", System.Type.GetType("System.Double")));
		}

		protected void Preparar_Tabla_Gastos()
		{
			dtGastos = new DataTable();
			dtGastos.Columns.Add(new DataColumn("CODIGO", System.Type.GetType("System.String")));
			dtGastos.Columns.Add(new DataColumn("NOMBRE", System.Type.GetType("System.String")));
			dtGastos.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.Double")));
			dtGastos.Columns.Add(new DataColumn("TASA", System.Type.GetType("System.Double")));
			dtGastos.Columns.Add(new DataColumn("ORIGINAL", System.Type.GetType("System.Double")));
		}

		protected void Preparar_Tabla_Guardar()
		{
			dtGuardar = new DataTable();
			dtGuardar.Columns.Add(new DataColumn("NUMEROINVENTARIO", System.Type.GetType("System.String")));
			dtGuardar.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.String")));
		}

		protected void LLenar_Tabla_Guardar()
		{
			this.Preparar_Tabla_Guardar();
			for (int i = 0; i < dtVehiculos.Rows.Count; i++)
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
			if (((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValuesVeh(e))
			{
				DataRow fila = dtVehiculos.NewRow();
				fila["CATALOGO"] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
				fila["VIN"] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
				fila["VALOR"] = double.Parse(((TextBox)e.Item.Cells[2].Controls[1]).Text, NumberStyles.Currency);
				//fila["VALOR"] = System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);


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
				//this.Bind_DgVehiculos();

				if (distGastosVehiculo == "S" || ((Button)e.CommandSource).CommandName == "DelDatasRow")
				{
					CacularValorFactura();
				}
			}
		}

		protected void DgGastos_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if (((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValuesGas(e))
			{
				double tasaC = 1;
				if (DBFunctions.SingleData("SELECT PGAS_MODENACI FROM PGASTODIRECTO WHERE PGAS_CODIGO='" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + "'") == "N")
				{
					try
					{
						tasaC = Convert.ToDouble(txtTasa.Text);
						if (tasaC <= 0) throw (new Exception());
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
				fila["ORIGINAL"] = Math.Round(System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text), 0);
				fila["VALOR"] = Math.Round(System.Convert.ToDouble(valorFactura.Text));

				//fila["VALOR"] = Math.Round(System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text)*tasaC,0);
				fila["TASA"] = tasaC;
				dtGastos.Rows.Add(fila);
			}
			if (((Button)e.CommandSource).CommandName == "DelDatasRow")
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


		protected void Bind_DgGastos()
		{
			dgGastos.DataSource = dtGastos;
			Session["dtGastos"] = dtGastos;
			dgGastos.DataBind();
		}

		protected bool CheckValuesVeh(DataGridCommandEventArgs e)
		{
			bool check = true;
			if (((TextBox)e.Item.Cells[1].Controls[1]).Text == "" || ((TextBox)e.Item.Cells[1].Controls[1]).Text == "")
				check = false;
			if (check)
			{
				for (int i = 0; i < dtVehiculos.Rows.Count; i++)
				{
					if ((dtVehiculos.Rows[i][0].ToString() == ((TextBox)e.Item.Cells[1].Controls[1]).Text) && (dtVehiculos.Rows[i][1].ToString() == ((TextBox)e.Item.Cells[1].Controls[1]).Text))
						check = false;
				}
			}
			return check;
		}

		protected bool CheckValuesGas(DataGridCommandEventArgs e)
		{
			bool check = true;
			if (((TextBox)e.Item.Cells[1].Controls[1]).Text == "" || ((TextBox)e.Item.Cells[1].Controls[1]).Text == "" || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text))
				check = false;
			if (check)
			{
				for (int i = 0; i < dtGastos.Rows.Count; i++)
				{
					if ((dtGastos.Rows[i][0].ToString() == ((TextBox)e.Item.Cells[1].Controls[1]).Text) && (dtGastos.Rows[i][1].ToString() == ((TextBox)e.Item.Cells[1].Controls[1]).Text))
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
			for (i = 0; i < dtVehiculos.Rows.Count; i++)
				totalVehiculos += System.Convert.ToDouble(dtVehiculos.Rows[i]["VALOR"]);
			//Al tener el total ahora calculmos el porcentaje de cada vehiculo
			double valorTotalFactura = double.Parse(valorFactura.Text, NumberStyles.Currency);
			for (i = 0; i < dtVehiculos.Rows.Count; i++)
			{
				double porcentaje = ((System.Convert.ToDouble(dtVehiculos.Rows[i]["VALOR"])) / (totalVehiculos));
				if (totalVehiculos == 0) porcentaje = 0;
				distGastosVehiculo = Session["distGastosVehiculo"].ToString();
				if (distGastosVehiculo == "N")
				{
					dtVehiculos.Rows[i]["PORCENTAJE"] = 100;
				}
				else
				{
					dtVehiculos.Rows[i]["PORCENTAJE"] = Math.Round(porcentaje * 100, 0);
					dtVehiculos.Rows[i]["VALORFACT"] = (valorTotalFactura * porcentaje);
				}
			}
			tlVehiculos.Text = totalVehiculos.ToString("C");
		}

		protected void Total_Gastos()
		{
			double totalGastos = 0;
			double totalvehiculos = 0;
			double valorTotalFactura = double.Parse(valorFactura.Text, NumberStyles.Currency);
			for (int i = 0; i < dtGastos.Rows.Count; i++)
				totalGastos += System.Convert.ToDouble(dtGastos.Rows[i]["VALOR"]);
			tlGastos.Text = totalGastos.ToString("C");
			tlVehiculos.Text = totalvehiculos.ToString("C");
			tlDiferencia.Text = (valorTotalFactura - totalGastos).ToString("C");
		}

		protected void Cancelar_Proceso(Object Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Vehiculos.DevolucionGastoDirectoVehiculos");
		}

		protected void Realizar_Proceso(Object Sender, EventArgs e)
		{

			 //Aqui es donde debemos llamar el objeto que graba la factura de proveedor y los datos sobre los vehiculos afectados
				//primero llenamos la tabla de los vehiculos
				this.LLenar_Tabla_Guardar();
				//Ahora  creamos el objeto de tipo factura gastos
				tablaRtns = (DataTable)Session["tablaRtns"];
				FacturaGastos miFacturaGastos = new FacturaGastos("V", dtGuardar, dtGastos, dtVehiculos, tablaRtns);
				miFacturaGastos.PrefijoFactura = prefijoFactura.SelectedValue;
				miFacturaGastos.NumeroFactura = numeroFactura.Text;
				miFacturaGastos.Nit = nitProveedor.Text;
				miFacturaGastos.PrefijoProveedor = prefFactProveedor.Text;
				miFacturaGastos.NumeroProveedor = numeFactProveedor.Text;
				miFacturaGastos.FechaFactura = fechaFact.Text;
				miFacturaGastos.Almacen = almacen.SelectedValue;
				miFacturaGastos.FechaVencimiento = fechaVencimiento.Text;
				miFacturaGastos.ValorFactura = double.Parse(valorFactura.Text, NumberStyles.Currency).ToString();
				miFacturaGastos.ValorIva = double.Parse(txtIVA.Text, NumberStyles.Currency).ToString();
				miFacturaGastos.Observacion = observacion.Text;
				miFacturaGastos.Usuario = HttpContext.Current.User.Identity.Name;
				if (miFacturaGastos.Devolucion_Factura(true))
					Response.Redirect("" + indexPage + "?process=Vehiculos.DevolucionGastoDirectoVehiculos&pref=" + prefijoFactura.SelectedValue + "&num=" + numeroFactura.Text);
				else
					lb.Text = miFacturaGastos.ProcessMsg;
		}
		#region Retenciones


		protected void Seleccionar(Object Sender, EventArgs e)
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
		   

			if (prefijoFactura.SelectedValue == "Seleccione..." || numeroFactura.Text == "" || prefFactProveedor.Text == "" || numeFactProveedor.Text == "" || fechaFact.Text == "" || fechaVencimiento.Text == "" || valorFactura.Text == "" || txtTotal.Text == "")
			{
				Utils.MostrarAlerta(Response, "El Documento de Entrada o a la factura de Proveedor estan erradas");
				return;
			}

			double valFac = double.Parse(valorFactura.Text, NumberStyles.Currency);
			double valIva = double.Parse(txtIVA.Text, NumberStyles.Currency);
			double valTotal = double.Parse(txtTotal.Text, NumberStyles.Currency);
			
			
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




				//tablaRtns = Retenciones();
				//gridRtns.DataSource = tablaRtns;
				//gridRtns.DataBind();

				tablaRtns = Retenciones();
				Session["tablaRtns"] = tablaRtns;
				gridRtns.DataSource = tablaRtns;
				gridRtns.DataBind();

				//dtGastos= Gastovehiculos();
				//dgGastos.DataSource = dtGastos;
				//dgGastos.DataBind();

				dtGastos = Gastovehiculos();
				Session["dtGastos"] = dtGastos;
				dgGastos.DataSource = dtGastos;
				dgGastos.DataBind();


				//dtVehiculos = Vehiculos();
				//dgVehiculos.DataSource = dtVehiculos;
				//dgVehiculos.DataBind();

				dtVehiculos = Vehiculos();
				Session["dtVehiculos"] = dtVehiculos;
				dgVehiculos.DataSource = dtVehiculos;
				dgVehiculos.DataBind();
				
				try
				{
					tlVehiculos.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mv.MVEH_VALOCOMP FROM mvehiculo mv, MCATALOGOVEHICULO mcv, DFACTURAGASTOVEHICULO dfgv WHERE dfgv.mveh_inventario = mv.mveh_inventario AND   mv.mcat_vin = mcv.mcat_vin AND dfgv.pdoc_codiordepago = '" + prefFactProveedor.SelectedValue + "' AND   dfgv.mfac_numeordepago = '" + numeFactProveedor.SelectedValue + "';")).ToString("C");
					tlGastos.Text = txtTotal.Text;
					txtGastosVeh.Text = txtTotal.Text;
				}
				catch { }


			}
			else
			{
				Utils.MostrarAlerta(Response, "La suma de Valor Factura y Valor Iva, no coincide con el Valor Total de la Factura!");
				return;
			}
					 


		}

		protected void Mostrar_gridRtns()
		{
			gridRtns.DataSource = tablaRtns;
			gridRtns.DataBind();
			if (tablaRtns.Rows.Count == 0)
				btnGrabar.Attributes.Add("onClick", "return;");
			else
				btnGrabar.Attributes.Remove("onClick");
			Session["tablaRtns"] = tablaRtns;
		}
		protected void Cargar_Tabla_Rtns()
		{
			tablaRtns = new DataTable();
			tablaRtns.Columns.Add(new DataColumn("CODRET", typeof(string)));
			tablaRtns.Columns.Add(new DataColumn("PORCRET", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALOR", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("TIPORETE", typeof(string)));
		}
		protected bool Buscar_Retencion(string rtn)
		{
			bool encontrado = false;
			if (tablaRtns != null && tablaRtns.Rows.Count != 0)
			{
				for (int i = 0; i < tablaRtns.Rows.Count; i++)
				{
					if (tablaRtns.Rows[i][0].ToString() == rtn)
						encontrado = true;
				}
			}
			return encontrado;
		}

		private void TotalRetenciones()
		{
			double totalR = 0;
			foreach (DataRow drRet in tablaRtns.Rows)
			{
				totalR += Convert.ToDouble(drRet["VALOR"]);
			}
			txtRetenciones.Text = Math.Round(totalR, 0).ToString("#,###");
		}
		protected void gridRtns_Item(object Sender, DataGridCommandEventArgs e)
		{
			DataRow fila;
			if (((Button)e.CommandSource).CommandName == "AgregarRetencion")
			{
				if ((((TextBox)e.Item.Cells[1].Controls[1]).Text == ""))
					Utils.MostrarAlerta(Response, "Debe escoger un tipo de retención");
				else if (this.Buscar_Retencion(((TextBox)e.Item.Cells[1].Controls[1]).Text))
					Utils.MostrarAlerta(Response, "Solo puede haber una retención de ese tipo");
				else if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text));
				//Utils.MostrarAlerta(Response, "Valor Invalido");
				else
				{
					fila = tablaRtns.NewRow();
					fila["CODRET"] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
					fila["PORCRET"] = Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
					fila["VALORBASE"] = double.Parse(((TextBox)e.Item.Cells[3].Controls[1]).Text, NumberStyles.Currency);
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
			else if (((Button)e.CommandSource).CommandName == "RemoverRetencion")
			{
				tablaRtns.Rows[e.Item.DataSetIndex].Delete();
				this.Mostrar_gridRtns();
				TotalRetenciones();
			}
		}


		[Ajax.AjaxMethod]
		public DataSet Cargar_Nombre(string Cedula)
		{
			DataSet Vins = new DataSet();
			DBFunctions.Request(Vins, IncludeSchema.NO, "select mnit_nit from dbxschema.mnit where mnit_nit like '" + Cedula + "%';select mnit_nombres concat ' ' CONCAT COALESCE(mnit_nombre2,'') concat ' 'concat mnit_apellidos concat ' 'concat COALESCE(mnit_apellido2,'') as NOMBRE from dbxschema.mnit where mnit_nit='" + Cedula + "'");
			return Vins;

		}
		public DataTable Gastovehiculos()
		{
			DataSet Gasvehi = new DataSet();
			DBFunctions.Request(Gasvehi, IncludeSchema.NO, @"SELECT DFGV.PGAS_CODIGO AS CODIGO,
																PGD.PGAS_NOMBRE AS NOMBRE,
																DFGV.DFAC_VALORGASTO AS VALOR
														FROM   DFACTURAGASTOVEHICULO DFGV,
																PGASTODIRECTO PGD
														WHERE  DFGV.PGAS_CODIGO = PGD.PGAS_CODIGO
																AND DFGV.PDOC_CODIORDEPAGO = '" + prefFactProveedor.SelectedValue + @"'
																AND DFGV.MFAC_NUMEORDEPAGO = '" + numeFactProveedor.SelectedValue + "'");
			return Gasvehi.Tables[0];
		}

		public DataTable Vehiculos()
		{
			DataSet vehi = new DataSet();
            DBFunctions.Request(vehi, IncludeSchema.NO, @"DECLARE GLOBAL TEMPORARY TABLE SESSION.PORCENTAJE
															(
															PORCENTAJE INTEGER
															)
															ON COMMIT PRESERVE ROWS NOT LOGGED WITH REPLACE;
															DECLARE GLOBAL TEMPORARY TABLE SESSION.DEVGASTO
															(
															CATALOGO VARCHAR (16),
															VIN VARCHAR (20),
															VALOR INTEGER, 
															VALORFACT INTEGER,
															GASTO  VARCHAR (10)
															)
															ON COMMIT PRESERVE ROWS NOT LOGGED WITH REPLACE;
															INSERT INTO SESSION.PORCENTAJE
															SELECT  100/MAX(DEV.PORCENTAJE) AS PORCENTAJE
															FROM(SELECT DEVOLUCION.CATALOGO, DEVOLUCION.VIN, DEVOLUCION.VALOR, (rownumber() over(order by DEVOLUCION.CATALOGO)) AS PORCENTAJE, DEVOLUCION.VALORFACT FROM 
															(SELECT DISTINCT MC.PCAT_CODIGO AS CATALOGO,
																	MVEH.MCAT_VIN AS VIN,
																	MVEH.MVEH_VALOCOMP AS VALOR,	   
																	DGV.DFAC_VALORGASTO AS VALORFACT,
																	DGV.PGAS_CODIGO AS GASTO
															FROM   MVEHICULO MVEH,
																	MCATALOGOVEHICULO MC,
																	MFACTURAPROVEEDOR MFAC,
																	DFACTURAGASTOVEHICULO DGV
															WHERE  DGV.MVEH_INVENTARIO = MVEH.MVEH_INVENTARIO
															AND DGV.PDOC_CODIORDEPAGO = '" + prefFactProveedor.SelectedValue + @"'
															AND DGV.MFAC_NUMEORDEPAGO = '" + numeFactProveedor.SelectedValue + @"' 
															AND MC.MCAT_VIN = MVEH.MCAT_VIN) AS DEVOLUCION) AS DEV;
															INSERT INTO SESSION.DEVGASTO
															SELECT DISTINCT MC.PCAT_CODIGO AS CATALOGO,
																	MVEH.MCAT_VIN AS VIN,
																	MVEH.MVEH_VALOCOMP AS VALOR,	   
																	DGV.DFAC_VALORGASTO AS VALORFACT,
																	DGV.PGAS_CODIGO AS GASTO
															FROM   MVEHICULO MVEH,
																	MCATALOGOVEHICULO MC,
																	MFACTURAPROVEEDOR MFAC,
																	DFACTURAGASTOVEHICULO DGV
															WHERE  DGV.MVEH_INVENTARIO = MVEH.MVEH_INVENTARIO
															AND DGV.PDOC_CODIORDEPAGO = '" + prefFactProveedor.SelectedValue + @"'
															AND DGV.MFAC_NUMEORDEPAGO = '" + numeFactProveedor.SelectedValue + @"' 
															AND MC.MCAT_VIN = MVEH.MCAT_VIN;
															SELECT SDG.CATALOGO, SDG.VIN, SDG.VALOR, SP.PORCENTAJE, SDG.VALORFACT, GASTO 
															FROM SESSION.PORCENTAJE SP, SESSION.DEVGASTO SDG;");
			return vehi.Tables[0];
		}

		public DataTable Retenciones()
		{
			DataSet Rete = new DataSet();
			DBFunctions.Request(Rete, IncludeSchema.NO, @"SELECT MFPR.PRET_CODIGO AS CODRET,
                                                           PR.TRET_CODIGO AS TIPORETE,
                                                           PR.PRET_PORCENNODECL AS PORCRET,
                                                           MFPR.MFAC_VALOBASE AS VALORBASE,
                                                           MFPR.MFAC_VALORETE AS VALOR
                                                        FROM MFACTURAPROVEEDORRETENCION MFPR,
                                                         PRETENCION PR
                                                        WHERE PR.PRET_CODIGO = MFPR.PRET_CODIGO
                                                        AND MFPR.PDOC_CODIORDEPAGO= '" + prefFactProveedor.SelectedValue + @"'
                                                        AND MFPR.MFAC_NUMEORDEPAGO = '" + numeFactProveedor.SelectedValue +"';");
			return Rete.Tables[0];
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
