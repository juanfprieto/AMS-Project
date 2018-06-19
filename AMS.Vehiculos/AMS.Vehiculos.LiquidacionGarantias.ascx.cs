namespace AMS.Vehiculos
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Tools;


	/// <summary>
	///		Descripción breve de AMS_Vehiculos_LiquidacionGarantias.
	/// </summary>
	public partial class AMS_Vehiculos_LiquidacionGarantias : System.Web.UI.UserControl
	{
		#region COntroles
		private double totalAlistamientos,totalRevisiones,totalGarantias,totalIVA,subtotal,total;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		public string concesionario;
		#endregion COntroles

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlAno,"SELECT PANO_ANO FROM DBXSCHEMA.PANO;");
				bind.PutDatasIntoDropDownList(ddlMes,"SELECT PMES_MES FROM DBXSCHEMA.PMES;");
                bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM DBXSCHEMA.PVENDEDOR where tVENd_CODIGO = 'AG';");
				ddlAno.Items.Insert(0,new ListItem("--seleccione--",""));
				ddlMes.Items.Insert(0,new ListItem("--seleccione--",""));
				ddlVendedor.Items.Insert(0,new ListItem("--seleccione--",""));
				txtFechaProceso.Text=DateTime.Now.ToString("yyyy-MM-dd");
				ViewState["PIVA"]=Convert.ToDouble(DBFunctions.SingleData("SELECT CEMP_PORCIVA FROM CEMPRESA;"));
				concesionario="";
				if(Request.QueryString["cnc"]!=null)
					concesionario=Request.QueryString["cnc"];
				if(concesionario!="C")
                    txtConcesionario.Attributes.Add("onclick", "ModalDialog(this,'SELECT MN.MNIT_NIT NIT, MN.MNIT_APELLIDOS concat \\' \\' concat MN.MNIT_nombreS concat \\' \\' concat COALESCE(MN.MNIT_REPRESENTANTE,\\'\\') NOMBRE, MN.MNIT_DIRECCION DIRECCION, MN.MNIT_TELEFONO TELEFONO FROM MNIT MN, MCONCESIONARIO MC WHERE MC.MNIT_NIT=MN.MNIT_NIT;',new Array(),1)");
				else{
					string nitDistribuidor=DBFunctions.SingleData(
						"SELECT MC.MNIT_NIT FROM MCONCESIONARIOUSUARIO MC,SUSUARIO SU "+
						"WHERE SU.SUSU_LOGIN='"+HttpContext.Current.User.Identity.Name.ToLower()+"' AND SU.SUSU_CODIGO=MC.SUSU_CODIGO;");
					if(nitDistribuidor.Length==0)
					{
                        Utils.MostrarAlerta(Response, "El usuario no tiene un concesionario asociado para el proceso.");
						btnSeleccionar.Enabled=false;
						return;
					}
					txtConcesionario.Text=nitDistribuidor;
                    txtConcesionarioa.Text = DBFunctions.SingleData("SELECT MN.MNIT_APELLIDOS concat \\' \\' concat MN.MNIT_nombreS concat \\' \\' concat COALESCE(MN.MNIT_REPRESENTANTE,\\'\\') NOMBRE FROM MNIT MN WHERE MN.MNIT_NIT='" + nitDistribuidor + "';");
				}
				if(concesionario.Length>0)
					pnlAutorizar.Visible=btnAceptar.Visible=false;
			}

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

		}
		#endregion

		//Seleccionar vehiculo
		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			if(ddlAno.SelectedValue.Length==0 || ddlMes.SelectedValue.Length==0 || ddlVendedor.SelectedValue.Length==0 || txtConcesionario.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe ingresar el año, mes, vendedor y concesionario para continuar con el proceso");
				return;
			}
			lblConcesionario.Text=txtConcesionarioa.Text;
			lblCategoria.Text=DBFunctions.SingleData("SELECT PCAT_CATECONC FROM MCONCESIONARIO MC WHERE MC.MNIT_NIT='"+txtConcesionario.Text+"';");
			lblEjecutivo.Text=ddlVendedor.SelectedItem.Text;
			lblAno.Text=ddlAno.SelectedValue;
			lblMes.Text=ddlMes.SelectedValue;
			
			string categoria=DBFunctions.SingleData("SELECT PC.PCAT_VALOHORA FROM MCONCESIONARIO MC,PCATEGORIACONCESIONARIO PC WHERE MC.MNIT_NIT='"+txtConcesionario.Text+"' AND PC.PCAT_CATECONC=MC.PCAT_CATECONC;");
			if(categoria.Length==0)
			{
                Utils.MostrarAlerta(Response, "No se ha especificado la categoría del concesionario.");
				return;
			}
			ViewState["VALOR_CATEGORIA"]=Convert.ToDouble(categoria);
			
			DataSet dsOrdenes=null;
			TraerOrdenes(ref dsOrdenes);
			ViewState["NUMERO_ORDENES"]=dsOrdenes.Tables[0].Rows.Count;
			
			if(dsOrdenes.Tables[0].Rows.Count==0)
			{
                Utils.MostrarAlerta(Response, "No hay órdenes de trabajo con los parámetros ingresados.");
				return;
			}
			
			//Colorear
			string proceso,estado,autoriza,color="",strEstado;
			for(int n=0;n<dsOrdenes.Tables[0].Rows.Count;n++)
			{
				proceso=dsOrdenes.Tables[0].Rows[n]["tproc_codigo"].ToString();
				estado=dsOrdenes.Tables[0].Rows[n]["test_estado"].ToString();
				autoriza=dsOrdenes.Tables[0].Rows[n]["tres_sino"].ToString();
                strEstado = "";

				if(proceso=="G")
				{
					if(autoriza=="S")
                    {
                        if (estado == "F")
                        {
                            color = ColorTranslator.ToHtml(Color.LightGreen);
                            strEstado = "LIQUIDADA";
                        }
                        else
                        {
                            color = ColorTranslator.ToHtml(Color.Yellow);
                            strEstado = "APROBADA";
                        }
                    }
                    else 
                        if (autoriza == "N")
                        {
						color=ColorTranslator.ToHtml(Color.Maroon);
                            strEstado = "NEGADA";
                        }
					else
                        {
                            color = ColorTranslator.ToHtml(Color.Tomato);
                            strEstado = "RADICADA";
				}
                }
                else
                {
                    if (estado == "F")
                    {
                        color = ColorTranslator.ToHtml(Color.LightGreen);
                        strEstado = "LIQUIDADA";
                    }
                    else
                    {
                        color = ColorTranslator.ToHtml(Color.Tomato);
                        strEstado = "RADICADA";
                    }
                }
				dsOrdenes.Tables[0].Rows[n]["color"]=color;
                dsOrdenes.Tables[0].Rows[n]["strestado"] = strEstado;
			}

			rptFactura.DataSource=dsOrdenes.Tables[0];
			rptFactura.DataBind();
			plcFactura.Visible=true;
			plcSeleccion.Visible=false;

			CalcularTotales();
			
			lblTotalAlistamientos.Text=totalAlistamientos.ToString("#,##0");
			lblTotalRevisiones.Text=totalRevisiones.ToString("#,##0");
			lblTotalGarantias.Text=totalGarantias.ToString("#,##0");
			lblSubtotal.Text=subtotal.ToString("#,##0");
			lblIVA.Text=totalIVA.ToString("#,##0");
			lblTotal.Text=total.ToString("#,##0");

		}
		
		//Calcular totales
		private void  CalcularTotales()
		{
			//Totales
            string strTotalAlistamientos = "", strTotalRevisiones = "", strTotalGarantias = "", strTotalGarantiasI = "";
			double categoriaV=Convert.ToDouble(ViewState["VALOR_CATEGORIA"]);
			double iva=Convert.ToDouble(ViewState["PIVA"]);
			totalAlistamientos=totalGarantias=totalRevisiones=totalIVA=subtotal=total=0;
			strTotalAlistamientos=
				DBFunctions.SingleData(
				"select coalesce(sum(tbA.Precio),0) "+
				"from("+
				" select "+
				" CASE WHEN pt.PTEM_INDIGENERIC='S' THEN pt.PTEM_TIEMPOESTANDAR "+
				" ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG "+
				" WHERE PTG.PTIE_GRUPCATA=pc.PGRU_GRUPO AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
				" END * "+categoriaV+" Precio "+
				"from morden mo, mordenpostventa mp, dordenoperacion dr, ptempario pt, pcatalogovehiculo pc, mcatalogovehiculo mcv "+
                "where mcv.pcat_codigo=pc.pcat_codigo and mo.mcat_vin = mcv.mcat_vin AND  " +
				"mo.PDOC_CODIGO=mp.PDOC_CODIGO and mo.MORD_NUMEORDE=mp.MORD_NUMEORDE AND mp.tproc_codigo='A' AND "+
				"dr.PDOC_CODIGO=mp.PDOC_CODIGO and dr.MORD_NUMEORDE=mp.MORD_NUMEORDE AND "+
				"dr.ptem_operacion=pt.ptem_operacion AND mo.test_estado='A' and "+
				"month(mo.mord_entrada)="+ddlMes.SelectedValue+" and "+
				"year(mo.mord_entrada)="+ddlAno.SelectedValue+" and "+
				"mp.mnit_nitconc='"+txtConcesionario.Text+"')as tbA;");
			strTotalRevisiones=
				DBFunctions.SingleData(
				"select coalesce(sum(tbA.Precio),0) "+
				"from("+
				" select "+
				" CASE WHEN pt.PTEM_INDIGENERIC='S' THEN pt.PTEM_TIEMPOESTANDAR "+
				" ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG "+
				" WHERE PTG.PTIE_GRUPCATA=pc.PGRU_GRUPO AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
				" END * "+categoriaV+" Precio "+
				"from morden mo, mordenpostventa mp, dordenoperacion dr, ptempario pt, pcatalogovehiculo pc, mcatalogovehiculo mcv "+
                "where mcv.pcat_codigo=pc.pcat_codigo and mo.mcat_vin = mcv.mcat_vin AND  " +
				"mo.PDOC_CODIGO=mp.PDOC_CODIGO and mo.MORD_NUMEORDE=mp.MORD_NUMEORDE AND mp.tproc_codigo='R' AND "+
				"dr.PDOC_CODIGO=mp.PDOC_CODIGO and dr.MORD_NUMEORDE=mp.MORD_NUMEORDE AND "+
				"dr.ptem_operacion=pt.ptem_operacion AND mo.test_estado='A' and "+
				"month(mo.mord_entrada)="+ddlMes.SelectedValue+" and "+
				"year(mo.mord_entrada)="+ddlAno.SelectedValue+" and "+
				"mp.mnit_nitconc='"+txtConcesionario.Text+"')as tbA;");
			strTotalGarantias=
				DBFunctions.SingleData(
				"select coalesce(sum(tbA.Precio),0) "+
				"from("+
				" select "+
				" CASE WHEN pt.PTEM_INDIGENERIC='S' THEN pt.PTEM_TIEMPOESTANDAR "+
				" ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG "+
				" WHERE PTG.PTIE_GRUPCATA=pc.PGRU_GRUPO AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
				" END * "+categoriaV+" Precio "+
				"from morden mo, mordenpostventa mp, dordenoperacion dr, ptempario pt, pcatalogovehiculo pc, MGARANTIAAUTORIZACION mg, mcatalogovehiculo mcv "+
                "where mcv.pcat_codigo=pc.pcat_codigo and mo.mcat_vin = mcv.mcat_vin AND  " +
				"mo.PDOC_CODIGO=mp.PDOC_CODIGO and mo.MORD_NUMEORDE=mp.MORD_NUMEORDE AND mp.tproc_codigo='G' AND "+
				"dr.PDOC_CODIGO=mp.PDOC_CODIGO and dr.MORD_NUMEORDE=mp.MORD_NUMEORDE AND "+
				"mg.PDOC_CODIGO=mo.PDOC_CODIGO and mg.MORD_NUMEORDEN=mo.MORD_NUMEORDE AND mg.tres_sino='S' AND "+
                "dr.ptem_operacion=pt.ptem_operacion AND mo.test_estado='A' and  dr.test_estado='C' and " +
				"month(mo.mord_entrada)="+ddlMes.SelectedValue+" and "+
				"year(mo.mord_entrada)="+ddlAno.SelectedValue+" and "+
				"mp.mnit_nitconc='"+txtConcesionario.Text+"')as tbA;");
            strTotalGarantiasI =
                DBFunctions.SingleData(
                "select coalesce(sum(mite_valaprob),0) " +
                "from morden mo, mordenpostventa mp, dordenitemspostventa dr, pcatalogovehiculo pc, MGARANTIAAUTORIZACION mg, mcatalogovehiculo mcv " +
                "where mcv.pcat_codigo=pc.pcat_codigo and mo.mcat_vin = mcv.mcat_vin AND  " +
                "mo.PDOC_CODIGO=mp.PDOC_CODIGO and mo.MORD_NUMEORDE=mp.MORD_NUMEORDE AND mp.tproc_codigo='G' AND " +
                "dr.PDOC_CODIGO=mp.PDOC_CODIGO and dr.MORD_NUMEORDE=mp.MORD_NUMEORDE AND " +
                "mg.PDOC_CODIGO=mo.PDOC_CODIGO and mg.MORD_NUMEORDEN=mo.MORD_NUMEORDE AND mg.tres_sino='S' AND " +
                "mo.test_estado='A' and  dr.test_estado='P' and " +
                "month(mo.mord_entrada)=" + ddlMes.SelectedValue + " and " +
                "year(mo.mord_entrada)=" + ddlAno.SelectedValue + " and " +
                "mp.mnit_nitconc='" + txtConcesionario.Text + "';");

			totalAlistamientos=(strTotalAlistamientos.Length>0)?Convert.ToDouble(strTotalAlistamientos):0;
			totalRevisiones=(strTotalRevisiones.Length>0)?Convert.ToDouble(strTotalRevisiones):0;
			totalGarantias=(strTotalGarantias.Length>0)?Convert.ToDouble(strTotalGarantias):0;
            totalGarantias += (strTotalGarantiasI.Length > 0) ? Convert.ToDouble(strTotalGarantiasI) : 0;
			subtotal=totalAlistamientos+totalGarantias+totalRevisiones;
			totalIVA=Math.Round((subtotal*iva)/100);
			total=subtotal+totalIVA;

			ViewState["SUBTOTAL"]=subtotal;
			ViewState["TOTAL_IVA"]=totalIVA;
			ViewState["TOTAL"]=total;
		}
		//Traer Ordenes de Trabajo
		private void TraerOrdenes(ref DataSet dsOrdenes)
		{
			dsOrdenes=new DataSet();
            string sqlOrdenes =
                "select mop.pdoc_codigo, mop.mord_numeorde, mop.mord_entrada, mop.tproc_codigo, mop.color, mop.strestado," +
				"mop.tipo_proceso,mg.tres_sino,mop.test_estado,mop.mnit_nitconc,mop.mcat_vin,mop.mcat_placa,"+
				"mop.mcat_motor,mop.pgru_grupo, pm.pman_pagafabrica "+
				"from "+
                "(SELECT mo.pdoc_codigo,mo.mord_numeorde,mo.mord_entrada,'' as color,mp.mnit_nitconc, '' as strestado, " +
				" mv.mcat_vin,mv.mcat_placa,mv.mcat_motor, pc.pgru_grupo, "+
                " case when mp.tproc_codigo='A' THEN 'ALISTAMIENTO' " +
                " WHEN mp.tproc_codigo='R' THEN 'REVISION' " +
                " WHEN mp.tproc_codigo='G' THEN 'GARANTIA' " +
				" ELSE '?' END tipo_proceso,mo.test_estado, mp.tproc_codigo "+
				" from morden mo, mordenpostventa mp, mcatalogovehiculo mv, pcatalogovehiculo pc "+
				" where mo.PDOC_CODIGO=mp.PDOC_CODIGO and mo.MORD_NUMEORDE=mp.MORD_NUMEORDE and "+
				" mo.mcat_vin=mv.mcat_vin and mv.pcat_codigo=pc.pcat_codigo and "+
				" mp.mnit_nitconc='"+txtConcesionario.Text+"' and mo.mcat_vin=mv.mcat_vin and "+
				" month(mo.mord_entrada)="+ddlMes.SelectedValue+" and year(mo.mord_entrada)="+ddlAno.SelectedValue+
				") as mop "+
				"left join mgarantiaautorizacion mg "+
				"on mop.PDOC_CODIGO=mg.PDOC_CODIGO and mop.MORD_NUMEORDE=mg.MORD_NUMEORDEN "+
				"left join DORDENKIT dr "+
				"on mop.PDOC_CODIGO=dr.PDOC_CODIGO and mop.MORD_NUMEORDE=dr.MORD_NUMEORDE "+
				"left join PMANTENIMIENTOPROGRAMADO pm "+
				"on pm.pgru_codigo=mop.pgru_grupo and pm.pkit_codigo=dr.pkit_codigo "+
                "order by mop.PDOC_CODIGO,mop.MORD_NUMEORDE;";
			DBFunctions.Request(dsOrdenes,IncludeSchema.NO, sqlOrdenes);
	
		}
		//Databind OTs
		public void rptFactura_Bound(object sender, RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item||e.Item.ItemType==ListItemType.AlternatingItem){
				UInt32 numOrden=Convert.ToUInt32(((DataRowView)e.Item.DataItem).Row.ItemArray[1]);
				string prefOrden=(((DataRowView)e.Item.DataItem).Row.ItemArray[0]).ToString();
				DataGrid dgrOpers=(DataGrid)e.Item.FindControl("dgrOperaciones");
                DataGrid dgrObs = (DataGrid)e.Item.FindControl("dgrObserv");
				DataGrid dgrReps=(DataGrid)e.Item.FindControl("dgrRepuestos");
				DataSet dsElementos=new DataSet();
				double categoria=Convert.ToDouble(ViewState["VALOR_CATEGORIA"]);
				DBFunctions.Request(dsElementos,IncludeSchema.NO,
                    "Select mo.mord_obseclie OBSERCLIEN, mo.mord_obserece OBSERTALLER, pt.ptem_operacion codigo, pt.ptem_descripcion nombre, mo.mord_creacion fecha, " +
					"mo.mord_kilometraje kilometraje, "+
					"CASE WHEN pt.PTEM_INDIGENERIC='S' THEN pt.PTEM_TIEMPOESTANDAR "+
					"ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG "+
					"WHERE PTG.PTIE_GRUPCATA=pc.PGRU_GRUPO AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
					"END TIEMPO, "+
					"CASE WHEN pt.PTEM_INDIGENERIC='S' THEN pt.PTEM_TIEMPOESTANDAR "+
					"ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG "+
					"WHERE PTG.PTIE_GRUPCATA=pc.PGRU_GRUPO AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
					"END * "+categoria+" PRECIO, "+
                    "dor.test_estado as estado, '' as color "+
					"from dordenoperacion dor, morden mo, ptempario pt, pcatalogovehiculo pc, mcatalogovehiculo mcv "+
					"where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and "+
                    "dor.pdoc_codigo='" + prefOrden + "' and dor.mord_numeorde=" + numOrden + " and pc.pcat_codigo=mcv.pcat_codigo and mcv.mcat_vin = mo.mcat_vin and " +
					"pt.ptem_operacion=dor.ptem_operacion order by mord_creacion desc;"+
					"Select mcat_vin,mi.mite_codigo codigo, mi.mite_NOMBRE nombre, mo.mord_creacion fecha, "+
					"mo.mord_kilometraje kilometraje, dor.mite_precio precio, dor.mite_cantidad cantidad, "+
                    "dor.test_estado as estado, '' as color, dor.mite_valaprob  " +
					"from dbxschema.dordenitemspostventa dor, dbxschema.morden mo, dbxschema.mitems mi "+
					"where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and "+
					"dor.pdoc_codigo='"+prefOrden+"' and dor.mord_numeorde="+numOrden+" and "+
					"mi.mite_codigo=dor.mite_codigo order by mord_creacion desc;");
                foreach (DataRow drC in dsElementos.Tables[0].Rows)
                {
                    if (drC["estado"].ToString().Equals("X"))
                        drC["color"] = ColorTranslator.ToHtml(Color.Maroon);
                    else{
                        if (drC["estado"].ToString().Equals("C"))
                            drC["color"] = ColorTranslator.ToHtml(Color.LimeGreen);
                        else
                            drC["color"] = ColorTranslator.ToHtml(Color.Yellow);
                    }
                }

                foreach (DataRow drC in dsElementos.Tables[1].Rows)
                {
                    if (drC["estado"].ToString().Equals("X"))
                        drC["color"] = ColorTranslator.ToHtml(Color.Maroon);
                    else
                    {
                        if (drC["estado"].ToString().Equals("C"))
                            drC["color"] = ColorTranslator.ToHtml(Color.LimeGreen);
                        else
                        {
                            if (drC["estado"].ToString().Equals("P"))
                            {
                                drC["color"] = ColorTranslator.ToHtml(Color.Blue);
                                drC["cantidad"] = 0;
                                drC["precio"] = drC["mite_valaprob"];
                            }
                            else
                                drC["color"] = ColorTranslator.ToHtml(Color.Yellow);
                        }
                    }
                }
                
                //Obtencion de observaciones de garantias
                DataSet dsObservaciones = new DataSet();
                DBFunctions.Request(dsObservaciones, IncludeSchema.NO,
                    "select mgar_observac OBSG, mgar_observrep OBSR from MGARANTIAAUTORIZACION where mord_numeorden =" + numOrden + ";");
                
                //Obtebncion operaciones
                 DataSet dsOperaciones = new DataSet();
                 DBFunctions.Request(dsOperaciones, IncludeSchema.NO,
                             "select mo.mcat_vin, pt.ptem_operacion as CODIGOOP, pt.ptem_descripcion as NOMBREOP, " +
                             "dop.test_estado as ESTADO, '' as COLOR " +
                             "from dbxschema.morden mo, dbxschema.dordenoperacion dop, dbxschema.ptempario pt " +
                            "where dop.pdoc_codigo=mo.pdoc_codigo and dop.mord_numeorde=mo.mord_numeorde and  " +
                            "dop.pdoc_codigo='" + prefOrden + "' and dop.mord_numeorde=" + numOrden + " and  " +
                            "pt.ptem_operacion = dop.ptem_operacion order by CODIGOOP asc;");

                 foreach (DataRow drC in dsOperaciones.Tables[0].Rows)
                 {
                     if (drC["estado"].ToString().Equals("X"))
                         drC["color"] = ColorTranslator.ToHtml(Color.Maroon);
                     else
                     {
                         if (drC["estado"].ToString().Equals("C"))
                             drC["color"] = ColorTranslator.ToHtml(Color.LimeGreen);
                         else
                             drC["color"] = ColorTranslator.ToHtml(Color.Yellow);
                     }
                 }

                dgrOpers.DataSource = dsOperaciones.Tables[0];
				dgrOpers.DataBind();
                dgrOpers.Visible = dsOperaciones.Tables[0].Rows.Count > 0;
                
                dgrObs.DataSource = dsObservaciones.Tables[0];
                dgrObs.DataBind();
                dgrObs.Visible = dsObservaciones.Tables[0].Rows.Count > 0;
                
                dgrReps.DataSource=dsElementos.Tables[1];
				dgrReps.DataBind();
				dgrReps.Visible=dsElementos.Tables[1].Rows.Count>0;
			}
		}

		
		//Realizar proceso
		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			DateTime fechaProceso;
			string prefAprobacion;
			UInt32 numAprobacion;
			try{
				fechaProceso=Convert.ToDateTime(txtFechaProceso.Text);}
			catch{
                Utils.MostrarAlerta(Response, "Debe ingresar la fecha de proceso en el formato correcto.");
				return;}
			try{
				prefAprobacion=txtPrefAprobacion.Text.Trim();
				numAprobacion=Convert.ToUInt32(txtNumAprobacion.Text);}
			catch{
                Utils.MostrarAlerta(Response, "Debe ingresar el número de aprobación correctamente.");
				return;}
			
			//Verificar no hay cambios
			DataSet dsOrdenes=null;
			TraerOrdenes(ref dsOrdenes);
			if(dsOrdenes.Tables[0].Rows.Count!=Convert.ToInt16(ViewState["NUMERO_ORDENES"]))
			{
                Utils.MostrarAlerta(Response, "Han cambiado las ordenes de trabajo desde la última consulta, debe volver a revisar el listado.");
				plcSeleccion.Visible=true;
				plcFactura.Visible=false;
				return;
			}
			
			LiquidacionPostVenta liquida=new LiquidacionPostVenta(prefAprobacion,numAprobacion,txtConcesionario.Text,ddlVendedor.SelectedValue,Convert.ToInt16(ddlAno.SelectedValue),Convert.ToInt16(ddlMes.SelectedValue),fechaProceso,Convert.ToDouble(ViewState["SUBTOTAL"]),Convert.ToDouble(ViewState["TOTAL_IVA"]),Convert.ToDouble(ViewState["TOTAL"]),dsOrdenes.Tables[0],Convert.ToDouble(ViewState["VALOR_CATEGORIA"]),Convert.ToDouble(ViewState["PIVA"]));
			if(liquida.Liquidar())
			{
				plcSeleccion.Visible=true;
				plcFactura.Visible=false;
                Utils.MostrarAlerta(Response, "Se ha realizado la liquidación del concesionario " + txtConcesionarioa.Text + " para el mes " + ddlMes.SelectedValue + " y año " + ddlAno.SelectedValue + ".");
			}
			else
			{
                Utils.MostrarAlerta(Response, "" + liquida.error + "");
				lblError.Text=liquida.sqlError;
				if(liquida.cambianDatos)
				{
					plcFactura.Visible=false;
					plcSeleccion.Visible=true;
				}
			}
		}
	}
}
