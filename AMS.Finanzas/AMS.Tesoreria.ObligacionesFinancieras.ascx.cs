namespace AMS.Finanzas
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
    using AMS.Documentos;
	using System.Configuration;
    using AMS.Tools;
    using AMS.Contabilidad;

	/// <summary>
	///		Descripción breve de AMS_Tesoreria_ObligacionesFinancieras.
	/// </summary>
	public partial class AMS_Tesoreria_ObligacionesFinancieras : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.DropDownList ddlAlmacenDesem;
		private DataTable dtDesembolsos, dtPagos;
        ProceHecEco contaOnline = new ProceHecEco();
		#endregion Controles

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			Page.SmartNavigation=true;
			if(!Page.IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlCuentaEditar,"SELECT PCUE_CODIGO, PCUE_NOMBRE CONCAT ' (' CONCAT PCUE_CODIGO CONCAT ' - ' CONCAT PCUE_NUMERO CONCAT ')' FROM PCUENTACORRIENTE ORDER BY PCUE_NOMBRE;");
				bind.PutDatasIntoDropDownList(ddlCreditoEditar,"SELECT MOBL_NUMERO FROM MOBLIGACIONFINANCIERA WHERE PCUE_CODIGO='"+ddlCuentaEditar.SelectedValue+"' ORDER BY MOBL_NUMERO;");
				
				bind.PutDatasIntoDropDownList(ddlCuenta,"SELECT PCUE_CODIGO, PCUE_NOMBRE CONCAT ' (' CONCAT PCUE_CODIGO CONCAT ' - ' CONCAT PCUE_NUMERO CONCAT ')' FROM PCUENTACORRIENTE ORDER BY PCUE_NOMBRE;");
				//bind.PutDatasIntoDropDownList(ddlDocumento,"SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU='ND' ORDER BY PDOC_NOMBRE;");
                Utils.llenarPrefijos(Response, ref ddlDocumento , "%", "%", "ND");
				lblNumDocumento.Text=DBFunctions.SingleData("SELECT PDOC_ULTIDOCU+1 FROM PDOCUMENTO WHERE PDOC_CODIGO='"+ddlDocumento.SelectedValue+"';");

                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT PALM_ALMACEN, PALM_DESCRIPCION FROM PALMACEN where (pcen_centcart is not null  or pcen_centteso is not null) and tvig_vigencia='V' order by palm_descripcion;");
				bind.PutDatasIntoDropDownList(ddlTipoCredito,"SELECT PCRE_CODIGO, PCRE_NOMBRE FROM PCREDITOBANCARIO ORDER BY PCRE_NOMBRE;");
				bind.PutDatasIntoDropDownList(ddlTipoTasa,"SELECT PTAS_CODIGO, PTAS_NOMBRE concat ' (' concat rtrim(char(ptas_monto)) concat ')' FROM PTASACREDITO ORDER BY PTAS_NOMBRE;");
				bind.PutDatasIntoDropDownList(ddlCondicion,"SELECT TCON_CODIGO, TCON_NOMBRE FROM TCONDICIONCREDITOBANCARIO ORDER BY TCON_NOMBRE;");
				CrearTablas();
				Bind();
				if(Request.QueryString["upd"]!=null)
				{
					Response.Write("<script language:javascript>alert('Las obligaciones financieras se han actualizado');</script>");

					if(Request["prf"]!=null && Request["num"]!=null)
					{
						FormatosDocumentos formatoFactura=new FormatosDocumentos();
						try
						{
							formatoFactura.Prefijo=Request.QueryString["prf"];
							formatoFactura.Numero=Convert.ToInt32(Request.QueryString["num"]);
							formatoFactura.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prf"]+"'");
							if(formatoFactura.Codigo!=string.Empty)
							{
								if(formatoFactura.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
							}

                            formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prf"] + "'");
                            if (formatoFactura.Codigo != string.Empty)
                            {
                                if (formatoFactura.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                            }
						}
						catch
						{
							lblInfo.Text="Error al generar el formato. Detalles : <br>"+formatoFactura.Mensajes;
						}

					}
				}
			}
			else
			{
				dtDesembolsos=(DataTable)ViewState["DT_DESEMBOLSOS"];
				dtPagos=(DataTable)ViewState["DT_PAGOS"];
			}
		}

		
		private void Bind()
		{
			ViewState["DT_DESEMBOLSOS"]=dtDesembolsos;
			ViewState["DT_PAGOS"]=dtPagos;
			dgrDesembolsos.DataSource=dtDesembolsos;
			dgrPagos.DataSource=dtPagos;
			dgrDesembolsos.DataBind();
			dgrPagos.DataBind();
		}

		//Crear Tablas
		private void CrearTablas()
		{
			dtPagos=new DataTable();
			dtPagos.Columns.Add("DOBL_NUMEPAGO",typeof(int));
			dtPagos.Columns.Add("MOBL_FECHA",typeof(DateTime));
			dtPagos.Columns.Add("MOBL_MONTPESOS",typeof(double));
			dtPagos.Columns.Add("MOBL_MONTINTERES",typeof(double));
			dtPagos.Columns.Add("MOBL_MONTCAPI",typeof(double));
			
			dtDesembolsos=new DataTable();
			dtDesembolsos.Columns.Add("DOBL_RAZON",typeof(string));
			dtDesembolsos.Columns.Add("MCUE_CODIPUC",typeof(string));
			dtDesembolsos.Columns.Add("PDOC_CODIDOCUREFE",typeof(string));
			dtDesembolsos.Columns.Add("DOBL_NUMEDOCUREFE",typeof(int));
			dtDesembolsos.Columns.Add("MNIT_NIT",typeof(string));
			dtDesembolsos.Columns.Add("PALM_ALMACEN",typeof(string));
			dtDesembolsos.Columns.Add("PCEN_CODIGO",typeof(string));
			dtDesembolsos.Columns.Add("DOBL_VALODEBI",typeof(double));
			dtDesembolsos.Columns.Add("DOBL_VALOCRED",typeof(double));
			dtDesembolsos.Columns.Add("DOBL_VALOBASE",typeof(double));

			ViewState["DT_DESEMBOLSOS"]=dtDesembolsos;
			ViewState["DT_PAGOS"]=dtPagos;
		}


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
			this.dgrPagos.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgrPagos_ItemCommand);
			this.dgrDesembolsos.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgrDesembolsos_ItemCommand);
			this.dgrDesembolsos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrDesembolsos_ItemDataBound);

		}
		#endregion

		#region Grillas
		private void dgrDesembolsos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
			}
		}

		private void dgrPagos_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName=="AgregarPago")
			{
				//Validaciones 
				double capitalPago=0, interesPago=0;
				DateTime fechaPago=new DateTime();
				try
				{
					capitalPago=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtCapitalPago"))).Text);
					interesPago=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtInteresPago"))).Text);
					fechaPago=Convert.ToDateTime(((TextBox)(e.Item.FindControl("txtFechaPago"))).Text);
				}
				catch
				{
					Response.Write("<script language:javascript>alert('No se puede agregar el pago, revise los valores ingresados');</script>");
					return;
				}
				DataRow drPago=dtPagos.NewRow();
				drPago["DOBL_NUMEPAGO"]=dtPagos.Rows.Count+1;
				drPago["MOBL_FECHA"]=fechaPago.ToString("yyyy-MM-dd");
				drPago["MOBL_MONTPESOS"]=capitalPago;
				drPago["MOBL_MONTINTERES"]=interesPago;
				dtPagos.Rows.Add(drPago);
				Bind();
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverPago")
			{
				dtPagos.Rows.RemoveAt(e.Item.DataSetIndex);
				for(int n=0;n<dtPagos.Rows.Count;n++)
					dtPagos.Rows[n]["DOBL_NUMEPAGO"]=n+1;
				Bind();
			}
		}

		private void dgrDesembolsos_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName=="AgregarDesem")
			{
				//Validaciones 
				string detalleD,cuentaD,prefDocD,numDocD,nitBeneD,almacenD,cenCostD;
				double debitoD=0, creditoD=0, baseD=0;
				DateTime fechaPago=new DateTime();
				try
				{
					detalleD=((TextBox)(e.Item.FindControl("txtDetalleDesem"))).Text;
                    cuentaD=((TextBox)(e.Item.FindControl("txtCuentaDesem"))).Text;
					prefDocD=((TextBox)(e.Item.FindControl("txtPrefDocDesem"))).Text;
					numDocD=((TextBox)(e.Item.FindControl("txtNumDocDesem"))).Text;
					nitBeneD=((TextBox)(e.Item.FindControl("txtNitDesem"))).Text;
					almacenD=((TextBox)(e.Item.FindControl("txtAlmacenDesem"))).Text;
					cenCostD=((TextBox)(e.Item.FindControl("txtCentroDesem"))).Text;
					debitoD=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtDebitoDesem"))).Text);
					creditoD=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtCreditoDesem"))).Text);
					baseD=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtBaseDesem"))).Text);
					if(detalleD.Trim().Length==0||cuentaD.Trim().Length==0||prefDocD.Trim().Length==0||numDocD.Trim().Length==0||nitBeneD.Length==0||almacenD.Length==0||cenCostD.Length==0)
						throw(new Exception());
				}
				catch
				{
					Response.Write("<script language:javascript>alert('No se puede agregar el desembolso, revise los valores ingresados');</script>");
					return;
				}
				if((debitoD==0&&creditoD==0) || (debitoD!=0&&creditoD!=0))
				{
					Response.Write("<script language:javascript>alert('Débito o crédito incorrectos');</script>");
					return;
				}
				DataRow drDesem=dtDesembolsos.NewRow();
				drDesem["DOBL_RAZON"]=detalleD;
				drDesem["MCUE_CODIPUC"]=cuentaD;
				drDesem["PDOC_CODIDOCUREFE"]=prefDocD;
				drDesem["DOBL_NUMEDOCUREFE"]=numDocD;
				drDesem["MNIT_NIT"]=nitBeneD;
				drDesem["PALM_ALMACEN"]=almacenD;
				drDesem["PCEN_CODIGO"]=cenCostD;
				drDesem["DOBL_VALODEBI"]=debitoD;
				drDesem["DOBL_VALOCRED"]=creditoD;
				drDesem["DOBL_VALOBASE"]=baseD;
				dtDesembolsos.Rows.Add(drDesem);
				Bind();
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverDesem")
			{
				dtDesembolsos.Rows.RemoveAt(e.Item.DataSetIndex);
				Bind();
			}
		}

		#endregion
		
		//Nuevo
		protected void btnNuevo_Click(object sender, System.EventArgs e)
		{
			pnlCredito.Visible=true;
			pnlSeleccion.Visible=false;
			btnNuevo.Enabled=false;
		}

		//Editar
		protected void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(ddlCreditoEditar.Items.Count==0 || ddlCuentaEditar.Items.Count==0){
				Response.Write("<script language:javascript>alert('Debe seleccionar el número y la cuenta');</script>");
				return;}
			pnlCredito.Visible=true;
			pnlSeleccion.Visible=false;
			btnEdit.Enabled=false;
			ddlCuenta.SelectedIndex=ddlCuenta.Items.IndexOf(ddlCuenta.Items.FindByValue(ddlCuentaEditar.SelectedValue));
			ddlCuenta.Enabled=false;
			txtNumero.Text=ddlCreditoEditar.SelectedValue;
			txtNumero.Enabled=false;

			ObligacionFinanciera obligacion=new ObligacionFinanciera(ddlCuentaEditar.SelectedValue,ddlCreditoEditar.SelectedValue);
			obligacion.ConsultarObligacion();
			txtFechaDesemb.Text=obligacion.fechaDesembolso.ToString("yyyy-MM-dd");
			ddlDocumento.SelectedIndex=ddlDocumento.Items.IndexOf(ddlDocumento.Items.FindByValue(obligacion.documento));
			ddlDocumento.Enabled=false;
			lblNumDocumento.Text=obligacion.numDocumento;
			ddlAlmacen.SelectedIndex=ddlAlmacen.Items.IndexOf(ddlAlmacen.Items.FindByValue(obligacion.almacen));
			txtMontoPesos.Text=obligacion.montoPesos.ToString("#,##0.##");
			txtMontoDolares.Text=obligacion.montoDolares.ToString("#,##0.##");
			txtNumCuotas.Text=obligacion.numCuotas.ToString("#,##0.##");
			txtMontoPagado.Text=obligacion.montoPagado.ToString("#,##0.##");
			txtInteresCausado.Text=obligacion.interesCausado.ToString("#,##0.##");
			txtInteresPagado.Text=obligacion.interesPagado.ToString("#,##0.##");
			ddlTipoCredito.SelectedIndex=ddlTipoCredito.Items.IndexOf(ddlTipoCredito.Items.FindByValue(obligacion.tipoCredito));
			ddlTipoTasa.SelectedIndex=ddlTipoTasa.Items.IndexOf(ddlTipoTasa.Items.FindByValue(obligacion.tasaCredito));
			txtInteresCredito.Text=obligacion.tasaInteres.ToString("#,##0.##");
			ddlCondicion.SelectedIndex=ddlCondicion.Items.IndexOf(ddlCondicion.Items.FindByValue(obligacion.condicion));
			txtDetalle.Text=obligacion.detalle;
			txtAutoriza.Text=obligacion.autoriza;
			txtCuentaDesemP.Text=obligacion.montoACuenta.ToString("#,##0.##");

			dtPagos=obligacion.dtPagos;
			dtDesembolsos=obligacion.dtDesembolsos;
			Bind();
		}

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			string numero=txtNumero.Text.Trim();
			string observacion=txtDetalle.Text.Trim();
			string autoriza=txtAutoriza.Text.Trim();
			double montoPeso=0,montoDolar=0,montoPagado=0,intereses=0,aCuenta=0;
			int numCuotas=0;
			DateTime dtFecha;
			if(numero.Length==0){
				Response.Write("<script language:javascript>alert('Debe ingresar el número del pagaré');</script>");
				return;}
			if(autoriza.Length==0){
				Response.Write("<script language:javascript>alert('Debe ingresar el nombre de la persona que autoriza');</script>");
				return;}
			try{
				dtFecha=Convert.ToDateTime(txtFechaDesemb.Text);}
			catch{
				Response.Write("<script language:javascript>alert('Fecha no válida');</script>");
				return;}
			try{montoPeso=Convert.ToDouble(txtMontoPesos.Text);}
			catch{Response.Write("<script language:javascript>alert('El monto en pesos no es válido');</script>");return;}
			try{montoDolar=Convert.ToDouble(txtMontoDolares.Text);}
			catch{Response.Write("<script language:javascript>alert('El monto en dólares no es válido');</script>");return;}
			try{numCuotas=Convert.ToInt16(txtNumCuotas.Text);}
			catch{Response.Write("<script language:javascript>alert('El número de cuotas no es válido');</script>");return;}
			try{intereses=Convert.ToDouble(txtInteresCredito.Text);}
			catch{Response.Write("<script language:javascript>alert('El interés no es válido');</script>");return;}
			try{aCuenta=Convert.ToDouble(txtCuentaDesemP.Text);}
			catch{Response.Write("<script language:javascript>alert('El valor a la cuenta en los desembolsos no es válido');</script>");return;}
			if(intereses>100){
				Response.Write("<script language:javascript>alert('El interés no es válido');</script>");
				return;}
			if(dtPagos.Rows.Count!=numCuotas){
				Response.Write("<script language:javascript>alert('El número de pagos registrados no es igual al número de cuotas');</script>");
				return;}
			double totalCapital=0;
			for(int n=0;n<dtPagos.Rows.Count;n++)
				totalCapital+=Convert.ToDouble(dtPagos.Rows[n]["MOBL_MONTPESOS"]);
			if(Math.Abs(totalCapital-montoPeso)>0.01){
				Response.Write("<script language:javascript>alert('La sumatoria de los capitales de los pagos no es igual al monto en pesos');</script>");
				return;}
			double totalDebitos=0,totalCreditos=0;
			for(int n=0;n<dtDesembolsos.Rows.Count;n++){
				totalDebitos+=Convert.ToDouble(dtDesembolsos.Rows[n]["DOBL_VALODEBI"]);
				totalCreditos+=Convert.ToDouble(dtDesembolsos.Rows[n]["DOBL_VALOCRED"]);
			}
			if( Math.Abs((totalDebitos-totalCreditos+aCuenta)-montoPeso)>0.01){
				Response.Write("<script language:javascript>alert('Los débitos menos los créditos mas el saldo a la cuenta en los desembolsos deben ser iguales al monto en pesos');</script>");
				return;}
			ObligacionFinanciera obligacion = new ObligacionFinanciera(
					ddlCuenta.SelectedValue,numero,dtFecha,ddlDocumento.SelectedValue,lblNumDocumento.Text, ddlAlmacen.SelectedValue,
					montoPeso,montoDolar,numCuotas,montoPagado,0,0,ddlTipoCredito.SelectedValue,ddlTipoTasa.SelectedValue,
					intereses,ddlCondicion.SelectedValue,observacion,autoriza,aCuenta,dtPagos,dtDesembolsos
			);

			if(!btnNuevo.Enabled)
			{
				if(DBFunctions.RecordExist("SELECT * FROM MOBLIGACIONFINANCIERA "+
					"WHERE MOBL_NUMERO='"+numero+"';")){
					Response.Write("<script language:javascript>alert('Ya fue registrado el número de crédito');</script>");
					return;}
				obligacion.GuardarObligacion();
			}
			else{
				if(DBFunctions.RecordExist("SELECT * FROM DOBLIGACIONFINANCIERACAUSACION "+
					"WHERE MOBL_NUMERO='"+numero+"';") || 
					DBFunctions.RecordExist("SELECT * FROM DOBLIGACIONFINANCIERAPAGOOBLI "+
					"WHERE MOBL_NUMERO='"+numero+"';"))
				{
					Response.Write("<script language:javascript>alert('Ya se han realizado causaciones de la obligación o comprobantes no puede editar la obligación');</script>");
					return;}
				obligacion.ActualizarObligacion();
			}

            if (obligacion.error.Length > 0)
                lblInfo.Text = obligacion.error;
            else
            {
                // contabilización ON LINE
                contaOnline.contabilizarOnline(obligacion.documento.ToString(), Convert.ToInt32(obligacion.numDocumento.ToString()), Convert.ToDateTime(obligacion.fechaDesembolso), "");
                Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Tesoreria.ObligacionesFinancieras&path=" + Request.QueryString["path"] + "&upd=1&prf=" + obligacion.documento + "&num=" + obligacion.numDocumento);
            }
				
		}

		protected void ddlCuentaEditar_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlCreditoEditar,"SELECT MOBL_NUMERO FROM MOBLIGACIONFINANCIERA WHERE PCUE_CODIGO='"+ddlCuentaEditar.SelectedValue+"' ORDER BY MOBL_NUMERO;");
		}

		protected void btnPagos_Click(object sender, System.EventArgs e)
		{
			int numMeses=0, numCuotas=0;
			double montoP=0,interes=0,tasa=0;
			DateTime fechaO;
			bool anticipado=false;
			try
			{
				numMeses=Convert.ToInt16(
					DBFunctions.SingleData(
					"SELECT TCON_NUMEMESES FROM TCONDICIONCREDITOBANCARIO WHERE TCON_CODIGO='"+ddlCondicion.SelectedValue+"';")
					);
				montoP=Convert.ToDouble(txtMontoPesos.Text);
				numCuotas=Convert.ToInt16(txtNumCuotas.Text);
				interes=Convert.ToDouble(txtInteresCredito.Text);
				tasa=Convert.ToDouble(DBFunctions.SingleData(
					"SELECT PTAS_MONTO FROM PTASACREDITO WHERE PTAS_CODIGO='"+ddlTipoTasa.SelectedValue+"';"));
				anticipado=!DBFunctions.SingleData(
					"SELECT TCON_VENCIDO FROM TCONDICIONCREDITOBANCARIO WHERE TCON_CODIGO='"+ddlCondicion.SelectedValue+"';")
					.Equals("S");
				fechaO=Convert.ToDateTime(txtFechaDesemb.Text);
				//fechaO=fechaO.AddDays(-(fechaO.Day-1));
			}
			catch
			{
				Response.Write("<script language:javascript>alert('Debe ingresar el monto, el número de cuotas, la condición, el interés y la fecha');</script>");
				return;
			}
			dtPagos.Rows.Clear();
			DataRow drPago;
			double totalP=0,pagoA=0,montoI=montoP;
			
			for(int n=1;n<=numCuotas;n++)
			{
				drPago=dtPagos.NewRow();
				drPago["DOBL_NUMEPAGO"]=n;
				drPago["MOBL_FECHA"]=fechaO.AddMonths(n*numMeses);
				pagoA=Math.Round(montoP/numCuotas,0);
				totalP+=pagoA;
				if(n==numCuotas)
					pagoA+=montoP-totalP;
				drPago["MOBL_MONTPESOS"]=pagoA;
				if(anticipado)montoI-=pagoA;
				drPago["MOBL_MONTINTERES"]=Math.Round((montoI) * ((tasa+interes)/100) * ((double)((30*(numMeses)))/360),0);
				if(!anticipado)montoI-=pagoA;
				dtPagos.Rows.Add(drPago);
			}
            if (dtPagos.Rows.Count > 0)
            {
                double capM = 0, mntC = Convert.ToDouble(txtMontoPesos.Text);
                for (int n = 0; n < dtPagos.Rows.Count; n++)
                {
                    dtPagos.Rows[n]["MOBL_MONTCAPI"] = mntC - capM;
                    capM += Convert.ToDouble(dtPagos.Rows[n]["MOBL_MONTPESOS"]);
                }
            }
			Bind();
		}
		public void dgrPagos_Edit(Object sender, DataGridCommandEventArgs e)
		{
			if(dtPagos.Rows.Count>0)
				dgrPagos.EditItemIndex=(int)e.Item.ItemIndex;
			Bind();		
		}
		public void dgrPagos_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgrPagos.EditItemIndex=-1;
			Bind();
		}
		public void dgrPagos_Update(Object sender, DataGridCommandEventArgs e)
		{
			//Validaciones 
			if(dtPagos.Rows.Count == 0)
				return;
			double capitalPago=0, interesPago=0;
			DateTime fechaPago=new DateTime();
			try
			{
				capitalPago=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtEdCapitalPago"))).Text);
				interesPago=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtEdInteresPago"))).Text);
				fechaPago=Convert.ToDateTime(((TextBox)(e.Item.FindControl("txtEdFechaPago"))).Text);
			}
			catch
			{
				Response.Write("<script language:javascript>alert('No se puede modificar el pago, revise los valores ingresados');</script>");
				return;
			}
			dtPagos.Rows[dgrPagos.EditItemIndex]["MOBL_FECHA"]=fechaPago.ToString("yyyy-MM-dd");
			dtPagos.Rows[dgrPagos.EditItemIndex]["MOBL_MONTPESOS"]=capitalPago;
			dtPagos.Rows[dgrPagos.EditItemIndex]["MOBL_MONTINTERES"]=interesPago;

			dgrPagos.EditItemIndex=-1;
			Bind();
		}

		protected void ddlDocumento_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			lblNumDocumento.Text=DBFunctions.SingleData("SELECT PDOC_ULTIDOCU+1 FROM PDOCUMENTO WHERE PDOC_CODIGO='"+ddlDocumento.SelectedValue+"';");
		}
	}
}