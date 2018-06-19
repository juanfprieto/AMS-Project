
	using System;
	using System.Text;
	using System.IO;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.Mail;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Documentos;
    using AMS.Tools;

namespace AMS.Automotriz
    {

	/// <summary>
	///		Descripción breve de AMS_Automotriz_ConsultaOT.
	/// </summary>
	public partial class AMS_Automotriz_ConsultaOT : System.Web.UI.UserControl
	{
		double TotalCosDev=0;
		int TotalDev=0;
		double TotalCosTra=0;
		int TotalTra=0;
		
	//
		protected DataSet lineas;
        protected DataSet lineas1;
        protected DataSet lineas2;
		protected DataSet lineas3;
		protected DataSet lineas4;
		protected DataTable resultado;
		protected DataTable resultado2;
        protected FormatosDocumentos formatoRecibo = new FormatosDocumentos();
        protected string codigoPrefijo;
        protected string numeroOrden;
        protected string cargoEspecial;
	//
		string vin=null;
		string placa=null;
		string Vehiculo=null;
		string estadoOT=null;
        string estadoOrden = null;
        string Nit = null;
		int cantidadTRepuestos=0;
		int cantidadTRepuestosDev=0;
		string OrdenTrabajo=null;
		string Nombre=null;
        
		protected void Page_Load(object sender, System.EventArgs e)
        
            {
			DatasToControls bind = new DatasToControls();
			
			if(OrdenOT.Items.Count==0)
			{
				//bind.PutDatasIntoDropDownList(OrdenOT,"Select PDOC_CODIGO,PDOC_CODIGO CONCAT ' ' CONCAT PDOC_DESCRIPCION from DBXSCHEMA.PDOCUMENTO WHERE TDOC_TIPODOCU='OT'");
                Utils.llenarPrefijos(Response, ref OrdenOT , "%", "%", "OT");
				ListItem it=new ListItem("--Prefijo --","0");
				OrdenOT.Items.Insert(0,it);
			}
		}
        
        //private void RevisionImpresionPDF()
        
        //{
        //    if (Request["ImpFac"] != null && Request["ImpFac"] != string.Empty)
        //    {
        //        try
        //        {
        //            formatoRecibo.Prefijo = Request["prefOT"];
        //            formatoRecibo.Numero = Convert.ToInt32(Request["numOT"]);
        //            formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request["prefOT"] + "'");
        //            if (formatoRecibo.Codigo != string.Empty)
        //            {
        //                if (formatoRecibo.Cargar_Formato())
        //                    Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
        //            }
        //        }
        //        catch
        //        {
        //            Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
        //        }
        //    }
        //    else if (Request["ImpOrd"] != null && Request["ImpOrd"] != string.Empty)
        //    {
        //        try
        //        {
        //            if (ConfigurationManager.AppSettings["ImprimirPreliquidacionOT"] == "true")
        //            {
        //                Hashtable parametros = new Hashtable();
        //                parametros.Add("PREFIJO", codigoPrefijo);
        //                parametros.Add("NUMERO", numeroOrden);
        //                parametros.Add("CARGO", cargoEspecial);

        //                string documento = Utils.Imprimir("ams.taller.preliquidacion", parametros);
        //                Response.Write("<script language:javascript>w=window.open('" + documento + "','','HEIGHT=600,WIDTH=600');</script>");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + ex.InnerException + "");
        //        }
        //    }
        //}
		protected  void  consultar(Object  Sender, EventArgs e)
		{
			if(!DBFunctions.RecordExist("SELECT pdoc_codigo FROM morden WHERE pdoc_codigo='"+OrdenOT.SelectedValue+"' AND mord_numeorde="+NumeOt.Text))
			{
                Utils.MostrarAlerta(Response, "La orden de trabajo seleccionada no se encuentra registrada!");
				
				return;
			}

			Panel1.Visible=true;
			Label16.Visible=true;
			lineas  = new DataSet();
            lineas1 = new DataSet();
			lineas2 = new DataSet();
			lineas3 = new DataSet();
			OrdenTrabajo=OrdenOT.SelectedValue.ToString() +NumeOt.Text.ToString();
			
			DatasToControls bind = new DatasToControls();
			DBFunctions.Request(lineas,IncludeSchema.NO,"select * from DBXSCHEMA.MORDEN where PDOC_CODIGO concat '' concat CAST(MORD_NUMEORDE AS character(10)) ='"+OrdenTrabajo.ToString()+"'");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				vin             = lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				placa           = DBFunctions.SingleData("select MCAT_PLACA  from DBXSCHEMA.MCATALOGOVEHICULO WHERE MCAT_VIN='"+vin+"' ");
                estadoOT        = lineas.Tables[0].Rows[i].ItemArray[06].ToString();
                estadoOrden     = DBFunctions.SingleData("select test_nombre  from DBXSCHEMA.testadoorden WHERE test_estado='" + estadoOT + "' ");
                PlacaLabel.Text = placa.ToString()+ "   Estado OT " + estadoOrden;
				VinLabel.Text   = vin.ToString();
			    Vehiculo        = DBFunctions.SingleData("select PCAT_DESCRIPCION from DBXSCHEMA.PCATALOGOVEHICULO PC, DBXSCHEMA.MCATALOGOVEHICULO MC WHERE PC.PCAT_CODIGO = MC.PCAT_CODIGO AND MC.MCAT_VIN='" + vin + "' ");
				VehiLabel.Text  = Vehiculo.ToString();
				Nit             = lineas.Tables[0].Rows[i].ItemArray[4].ToString();
				Nombre          = DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT='"+Nit+"' ");
				PropLabel.Text  = Nombre.ToString();
				IDLabel.Text    = Nit.ToString();
				KilLabel.Text   = lineas.Tables[0].Rows[i].ItemArray[16].ToString();
				ObseRLabel.Text = lineas.Tables[0].Rows[i].ItemArray[22].ToString();
				ObseCliLabel.Text=lineas.Tables[0].Rows[i].ItemArray[23].ToString();
				string fechaEntr= lineas.Tables[0].Rows[i].ItemArray[9].ToString();
				FechaELabel.Text= Convert.ToDateTime(fechaEntr).ToString("yyyy-MM-dd");
				HoraEnLabel.Text= lineas.Tables[0].Rows[i].ItemArray[10].ToString();
				string fechaSal = lineas.Tables[0].Rows[i].ItemArray[12].ToString();
				FechaEGLabel.Text= Convert.ToDateTime(fechaSal).ToString("yyyy-MM-dd");
				HoraEGLabel.Text= lineas.Tables[0].Rows[i].ItemArray[13].ToString();
                PrefOt.Text     = lineas.Tables[0].Rows[i].ItemArray[0].ToString();
                NumOt.Text      = lineas.Tables[0].Rows[i].ItemArray[1].ToString();
			}
            
            //// GRILLA 1 /// REPUESTOS PEDIDOS A LA ORDEN TRABJO
            this.PrepararTabla();
            
            string sqlPedido = String.Format(@"SELECT DISTINCT DBXSCHEMA.EDITARREFERENCIAS(D.MITE_CODIGO,PLIN.PLIN_TIPO),  
                   D.DPED_CANTFACT,  
                   D.DPED_VALOUNIT,  
                   D.MITE_CODIGO,  
                   D.PPED_CODIGO,  
                   D.MPED_NUMEPEDI,   
                   D.DPED_PORCDESC * 0.01 AS DESC,  
                   D.DPED_VALOunit * (1 - D.DPED_PORCDESC * 0.01) * D.DPED_CANTPEDI,
				   TC.TCAR_NOMBRE,
				   D.DPED_CANTPEDI,
				   MITEM.MITE_NOMBRE,     
                   D.PIVA_PORCIVA * 0.01 AS iva
              FROM DBXSCHEMA.MORDEN MORDEN,  
                   DBXSCHEMA.MPEDIDOTRANSFERENCIA MP,  
                   DBXSCHEMA.PLINEAITEM PLIN,  
                   DBXSCHEMA.MITEMS MITEM,  
                   DBXSCHEMA.TCARGOORDEN TC,
                   DBXSCHEMA.dpedidoitem D
             WHERE MORDEN.PDOC_CODIGO CONCAT '' CONCAT CAST(MORDEN.MORD_NUMEORDE AS CHARACTER (10)) = '{0}'  
             AND   MP.PDOC_CODIGO = MORDEN.PDOC_CODIGO AND MP.MORD_NUMEORDE = MORDEN.MORD_NUMEORDE  
             AND   MP.PPED_CODIGO = D.PPED_CODIGO  
             AND   MP.MPED_NUMERO = D.MPED_NUMEPEDI  
             AND   D.MITE_CODIGO = MITEM.MITE_CODIGO  
             AND   MITEM.PLIN_CODIGO = PLIN.PLIN_CODIGO 
             AND   TC.TCAR_CARGO = MP.TCAR_CARGO             
             order by 1,5,6;", OrdenTrabajo);
            string tipoDoc = "Pedido ";

            string mostrarRep = DBFunctions.SingleData("SELECT CTAL_MOSTREPUORDETRAB FROM CTALLER;");
            if(mostrarRep == "S")
            {
                DBFunctions.Request(lineas1, IncludeSchema.NO, sqlPedido);
                tipoDoc = "Pedido ";
                if (lineas1.Tables.Count > 0)
                {
                    for (int a = 0; a < lineas1.Tables[0].Rows.Count; a++)
                    {
                        DataRow fila;
                        fila = resultado.NewRow();
                        double valor1 = Convert.ToDouble(lineas1.Tables[0].Rows[a].ItemArray[2].ToString());
                        string ValorFormato1 = String.Format("{0:C}", valor1);
                        int cantidad1 = Convert.ToInt32(lineas1.Tables[0].Rows[a].ItemArray[1]);

                        fila["ORDENT"] = tipoDoc + lineas1.Tables[0].Rows[a].ItemArray[4].ToString() + '-' + lineas1.Tables[0].Rows[a].ItemArray[5].ToString() + ' ' + lineas1.Tables[0].Rows[a].ItemArray[8].ToString();
                        fila["REPUESTO"] = lineas1.Tables[0].Rows[a].ItemArray[10].ToString();
                        fila["CODREPUESTO"] = lineas1.Tables[0].Rows[a].ItemArray[0].ToString();
                        fila["VALORU"] = ValorFormato1.ToString();
                        fila["CANTIDAD"] = lineas1.Tables[0].Rows[a].ItemArray[9].ToString() + '|' + lineas1.Tables[0].Rows[a].ItemArray[1].ToString();
                        fila["DESCUENTO"] = String.Format("{0:P}", Convert.ToDouble(lineas1.Tables[0].Rows[a].ItemArray[6]));
                        fila["IVA"] = String.Format("{0:P}", Convert.ToDouble(lineas1.Tables[0].Rows[a].ItemArray[11]));
                        resultado.Rows.Add(fila);

                    }
                }
            }
            
            //// GRILLA 1 /// REPUESTOS TRANSFERIDOS A LA ORDEN TRABJO
            //this.PrepararTabla();

            string sql = String.Format(@"SELECT DISTINCT DBXSCHEMA.EDITARREFERENCIAS(DITEM.MITE_CODIGO,PLIN.PLIN_TIPO),  
                   DITEM.DITE_CANTIDAD,  
                   DITEM.DITE_VALOPUBL,  
                   DITEM.MITE_CODIGO,  
                   MORTRA.PDOC_FACTURA,  
                   MORTRA.MFAC_NUMERO,   
                   DITEM.DITE_PORCDESC*0.01 AS DESC,  
                   DITEM.DITE_VALOunit * (1 - DITEM.DITE_PORCDESC * 0.01) * DITEM.DITE_CANTIDAD,
				   TC.TCAR_NOMBRE,
				   D.DPED_CANTPEDI,
                   MITEM.MITE_NOMBRE,
                   DITEM.PIVA_PORCIVA * 0.01 AS IVA   
              FROM DBXSCHEMA.MORDEN MORDEN,  
                   DBXSCHEMA.MORDENTRANSFERENCIA MORTRA,  
                   DBXSCHEMA.PLINEAITEM PLIN,  
                   DBXSCHEMA.MITEMS MITEM,  
                   DBXSCHEMA.TCARGOORDEN TC,
                   DBXSCHEMA.DITEMS DITEM 
                   LEFT JOIN DBXSCHEMA.dpedidoitem D ON mped_clasregi = 'C' AND D.PPED_CODIGO = DITEM.DITE_PREFDOCUREFE AND D.MPED_NUMEPEDI = DITEM.DITE_NUMEDOCUREFE
				         AND DITEM.MITE_CODIGO = D.MITE_CODIGO    
             WHERE MORDEN.PDOC_CODIGO CONCAT '' CONCAT CAST(MORDEN.MORD_NUMEORDE AS CHARACTER (10)) = '{0}'  
             AND   MORTRA.PDOC_CODIGO concat '' concat CAST(MORTRA.MORD_NUMEORDE AS CHARACTER (10)) = '{0}'  
             AND   MORTRA.PDOC_FACTURA = DITEM.PDOC_CODIGO  
             AND   MORTRA.MFAC_NUMERO = DITEM.DITE_NUMEDOCU  
             AND   DITEM.MITE_CODIGO = MITEM.MITE_CODIGO  
             AND   MITEM.PLIN_CODIGO = PLIN.PLIN_CODIGO 
             AND   TC.TCAR_CARGO = MORTRA.TCAR_CARGO             
             order by 1,5,6", OrdenTrabajo);

            DBFunctions.Request(lineas2, IncludeSchema.NO, sql);
            double sumaTotales = 0;
            double sumaTotalesInterno = 0;
            double sumaTotalesSeguro = 0;
            double sumaTotalesCliente = 0;
            tipoDoc = "Transfernc ";
            if (lineas2.Tables.Count > 0)
            {
                for (int a = 0; a < lineas2.Tables[0].Rows.Count; a++)
                {
                    //  string repuesto = DBFunctions.SingleData("Select MITE_NOMBRE from DBXSCHEMA.MITEMS WHERE MITE_CODIGO='" + lineas2.Tables[0].Rows[a].ItemArray[3].ToString() + "'");
                    DataRow fila;
                    fila = resultado.NewRow();
                    double valor1 = Convert.ToDouble(lineas2.Tables[0].Rows[a].ItemArray[2].ToString());
                    string ValorFormato1 = String.Format("{0:C}", valor1);
                    int cantidad1 = Convert.ToInt32(lineas2.Tables[0].Rows[a].ItemArray[1]);

                    fila["ORDENT"] = tipoDoc + lineas2.Tables[0].Rows[a].ItemArray[4].ToString() + '-' + lineas2.Tables[0].Rows[a].ItemArray[5].ToString() + ' ' + lineas2.Tables[0].Rows[a].ItemArray[8].ToString();
                    fila["REPUESTO"] = lineas2.Tables[0].Rows[a].ItemArray[10].ToString();
                    fila["CODREPUESTO"] = lineas2.Tables[0].Rows[a].ItemArray[0].ToString();
                    fila["VALORU"] = ValorFormato1.ToString();
                    fila["CANTIDAD"] = lineas2.Tables[0].Rows[a].ItemArray[9].ToString() + '|' + lineas2.Tables[0].Rows[a].ItemArray[1].ToString();
                    fila["DESCUENTO"] = String.Format("{0:P}", Convert.ToDouble(lineas2.Tables[0].Rows[a].ItemArray[6]));
                    fila["IVA"] = String.Format("{0:P}", Convert.ToDouble(lineas2.Tables[0].Rows[a].ItemArray[11]));
                    resultado.Rows.Add(fila);

                    if (lineas2.Tables[0].Rows[a].ItemArray[8].ToString().Trim().Equals("Interno") == true)
                    {
                        string auxInterno = lineas2.Tables[0].Rows[a].ItemArray[7].ToString();
                        sumaTotalesInterno += Convert.ToDouble(auxInterno);
                    }

                    if (lineas2.Tables[0].Rows[a].ItemArray[8].ToString().Trim().Equals("Seguros") == true)
                    {
                        string auxSeguro = lineas2.Tables[0].Rows[a].ItemArray[7].ToString();
                        sumaTotalesSeguro += Convert.ToDouble(auxSeguro);
                    }
                    if (lineas2.Tables[0].Rows[a].ItemArray[8].ToString().Trim().Equals("Cliente") == true)
                    {
                        string auxCliente = lineas2.Tables[0].Rows[a].ItemArray[7].ToString();
                        sumaTotalesCliente += Convert.ToDouble(auxCliente);
                    }

                    string aux = lineas2.Tables[0].Rows[a].ItemArray[7].ToString();
                    sumaTotales += Convert.ToDouble(aux);
                    TotalCosTra = TotalCosTra + (valor1 * cantidad1);
                    cantidadTRepuestos = cantidadTRepuestos + cantidad1;

                    TotalTra = TotalTra + 1;
                }
            }
            
            lbValorCargoInterno.Text = sumaTotalesInterno.ToString("C");
            lbValorCargoSeguro.Text  = sumaTotalesSeguro.ToString("C");
            lbValorCargoCliente.Text = sumaTotalesCliente.ToString("C");
            
            DataRow filaFinal     = resultado.NewRow();
            filaFinal["ORDENT"]   = "Total:";
            filaFinal["REPUESTO"] = String.Format("{0:C}", Math.Round(sumaTotales));
            resultado.Rows.Add(filaFinal);

            Grid.DataSource = resultado;
            Grid.DataBind();

            //// FIN GRILLA ORDENES DE TRABAJO
			this.PrepararTabla2();
			//// GRILLA 2 DEVOLUCIONES
			DBFunctions.Request(lineas3,IncludeSchema.NO,"select PDOC_FACTURA,MFAC_NUMERO FROM DBXSCHEMA.MORDENTRANSFERENCIA WHERE PDOC_CODIGO CONCAT '' CONCAT CAST(MORD_NUMEORDE AS character(10))='"+OrdenTrabajo.ToString()+"'");
			//                                                        0            1      
			
			for(int c=0;c<lineas3.Tables[0].Rows.Count;c++)
			{
				lineas4 = new DataSet();
				DBFunctions.Request(lineas4,IncludeSchema.NO,"SELECT  DISTINCT DBXSCHEMA.EDITARREFERENCIAS(DITEM.MITE_CODIGO,PLIN.PLIN_TIPO),DITEM.DITE_CANTIDAD,DITEM.DITE_VALOPUBL,DITEM.MITE_CODIGO,DITEM.PDOC_CODIGO,DITEM.DITE_NUMEDOCU,DITEM.DITE_PREFDOCUREFE,DITEM.DITE_NUMEDOCUREFE from DBXSCHEMA.DITEMS DITEM,DBXSCHEMA.PLINEAITEM PLIN,DBXSCHEMA.MITEMS MITEM,DBXSCHEMA.MCATALOGOVEHICULO MCATV WHERE DITEM.DITE_PREFDOCUREFE='"+lineas3.Tables[0].Rows[c].ItemArray[0].ToString()+"' AND DITEM.DITE_NUMEDOCUREFE="+lineas3.Tables[0].Rows[c].ItemArray[1]+" AND DITEM.MITE_CODIGO=MITEM.MITE_CODIGO AND MITEM.PLIN_CODIGO = PLIN.PLIN_CODIGO AND  DITEM.TMOV_TIPOMOVI=81");
				//                                                                                  0                                                   1                      2                3             4                   5                          6                  7
				for(int b=0;b<lineas4.Tables[0].Rows.Count;b++)
				{	
					string repuesto1=DBFunctions.SingleData("Select MITE_NOMBRE from DBXSCHEMA.MITEMS WHERE MITE_CODIGO='"+lineas4.Tables[0].Rows[b].ItemArray[3].ToString()+"'");
					
				
					DataRow fila3;
					fila3               = resultado2.NewRow();
					double valor        = Convert.ToDouble(lineas4.Tables[0].Rows[b].ItemArray[2].ToString());
					string ValorFormato = String.Format("{0:C}",valor);
					int cantidad        = Convert.ToInt32(lineas4.Tables[0].Rows[b].ItemArray[1]);
                    fila3["ORDENDEV"]   = lineas4.Tables[0].Rows[b].ItemArray[4].ToString() + '-' + lineas4.Tables[0].Rows[b].ItemArray[5].ToString();
                    fila3["TRANSFE"]    = lineas4.Tables[0].Rows[b].ItemArray[6].ToString() + '-' + lineas4.Tables[0].Rows[b].ItemArray[7].ToString();
					fila3["REPUESTO1"]  = repuesto1.ToString();
					fila3["CODREPUESTO1"]=lineas4.Tables[0].Rows[b].ItemArray[0].ToString();
					fila3["VALORU1"]    = ValorFormato.ToString();
					fila3["CANTIDAD1"]  = lineas4.Tables[0].Rows[b].ItemArray[1].ToString();
					resultado2.Rows.Add(fila3);
					
					TotalCosDev         = TotalCosDev+(valor * cantidad );
					cantidadTRepuestosDev=cantidadTRepuestosDev+cantidad;					
					TotalDev            = TotalDev+1;

				}
			
			}

			//MANEJO GRILLA OPERACIONES
			DataSet dsOperaciones = new DataSet();
            DBFunctions.Request(dsOperaciones, IncludeSchema.NO,
               @"SELECT DORP.ptem_operacion as CODIGOOPERACION, PTE.ptem_descripcion concat ' - ' concat DORP.pven_codigo as DESCRIPCION, 
                    CASE WHEN DORP.TCAR_CARGO IN ('C','S') THEN ROUND(DORP.dord_valooper,0) ELSE DORP.dord_valooper END as PRECIO, 
                    DORP.PIVA_PORCIVA*0.01 AS IVA,
                    TEO.test_nombre concat ' ' concat TC.TCAR_NOMBRE as ESTADOPERACION, DORP.dord_fechcump as FECHATERM  
				  FROM dordenoperacion DORP 
                    INNER JOIN morden MOR ON MOR.pdoc_codigo=DORP.pdoc_codigo AND MOR.mord_numeorde=DORP.mord_numeorde 
                    INNER JOIN ptempario PTE ON DORP.ptem_operacion = PTE.ptem_operacion 
                    INNER JOIN testadooperacion TEO ON DORP.test_estado = TEO.test_estaoper
                    INNER JOIN TCARGOORDEN TC ON DORP.tCAR_CARGO = TC.tCAR_CARGO 
				  WHERE DORP.pdoc_codigo='" + OrdenOT.SelectedValue+@"' AND DORP.mord_numeorde = "+NumeOt.Text+@"  
				  ORDER BY DORP.pdoc_codigo,DORP.mord_numeorde ASC; 
				 SELECT MFCT.pdoc_codigo as PREFIJOFACTURA, MFCT.mfac_numedocu as NUMEROFACTURA, TCA.tcar_nombre as CARGOFACTURA, MFC.mfac_valofact as VALORFACTURA, MFC.mfac_valoiva as VALORIVAFACTURA, MFC.mfac_valorete as VALORRETENCIONES  
				  FROM mfacturaclientetaller MFCT INNER JOIN mfacturacliente MFC ON MFCT.pdoc_codigo=MFC.pdoc_codigo AND MFCT.mfac_numedocu=MFC.mfac_numedocu INNER JOIN tcargoorden TCA ON MFCT.tcar_cargo = TCA.tcar_cargo  
				WHERE MFCT.pdoc_prefordetrab='"+OrdenOT.SelectedValue+"' AND MFCT.mord_numeorde="+NumeOt.Text);
			dgOperaciones.DataSource = dsOperaciones.Tables[0];
			dgOperaciones.DataBind();
			Label21.Visible     = Label23.Visible = lbCantOperaciones.Visible = lbValorOperaciones.Visible = true;
			lbCantOperaciones.Text = dsOperaciones.Tables[0].Rows.Count.ToString();
			try{lbValorOperaciones.Text = Convert.ToDouble(dsOperaciones.Tables[0].Compute("SUM(PRECIO)","")).ToString("C");}catch{lbValorOperaciones.Text = (0).ToString("C");}
			//FIN MANEJO GRILLA OPERACIONES

			//MANEJO GRILLA FACTURAS
			dgFacturas.DataSource = dsOperaciones.Tables[1];
			dgFacturas.DataBind();
			//FIN MANEJO GRILLA FACTURA
			Grid1.DataSource    = resultado2;
			Grid1.DataBind();
			TotDevLabel.Text    =TotalDev.ToString();
			string ValorFormatoDev=String.Format("{0:C}",TotalCosDev);
			TotCosLabel.Text    =ValorFormatoDev.ToString();
			/////////////
			TotReLabel.Text     =TotalTra.ToString();
			string ValorFormatoTra=String.Format("{0:C}",TotalCosTra);
			TotReCoLabel.Text   =ValorFormatoTra.ToString();
			//////////////
			double TotalNeto    =(TotalCosTra - TotalCosDev);
			string ValorFormatoNet=String.Format("{0:C}",TotalNeto);
			totalRelabel.Text   =cantidadTRepuestos.ToString();
			totalReDeLabel.Text =cantidadTRepuestosDev.ToString();
			TotNetoLabel.Text   =ValorFormatoNet.ToString();
			//// FIN GRILLA 2
			///


			}
		public void PrepararTabla()
		{
			resultado = new DataTable();
			//Adicionamos una columna que almacene el total de la linea
			DataColumn ordent = new DataColumn();
			ordent.DataType = System.Type.GetType("System.String");
			ordent.ColumnName = "ORDENT";
			ordent.ReadOnly=true;
			resultado.Columns.Add(ordent);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn repuesto = new DataColumn();
			repuesto.DataType = System.Type.GetType("System.String");
			repuesto.ColumnName = "REPUESTO";
			repuesto.ReadOnly=true;
			resultado.Columns.Add(repuesto);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valoru = new DataColumn();
			valoru.DataType = System.Type.GetType("System.String");
			valoru.ColumnName = "VALORU";
			valoru.ReadOnly=true;
			resultado.Columns.Add(valoru);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn cantidad = new DataColumn();
			cantidad.DataType = System.Type.GetType("System.String");
			cantidad.ColumnName = "CANTIDAD";
			cantidad.ReadOnly=true;
			resultado.Columns.Add(cantidad);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn codrepuesto = new DataColumn();
			codrepuesto.DataType = System.Type.GetType("System.String");
			codrepuesto.ColumnName = "CODREPUESTO";
			codrepuesto.ReadOnly=true;
			resultado.Columns.Add(codrepuesto);
            //Adicionamos una columna que almacene el total de la linea
            DataColumn descuento = new DataColumn();
            descuento.DataType = System.Type.GetType("System.String");
            descuento.ColumnName = "DESCUENTO";
            descuento.ReadOnly = true;
            resultado.Columns.Add(descuento);
            //Adicionamos una columna que almacene el IVA
            DataColumn iva = new DataColumn();
            iva.DataType = System.Type.GetType("System.String");
            iva.ColumnName = "IVA";
            iva.ReadOnly = true;
            resultado.Columns.Add(iva);

        }
		public void PrepararTabla2()
		{
			resultado2 = new DataTable();
			//Adicionamos una columna que almacene el total de la linea
			DataColumn repuesto1 = new DataColumn();
			repuesto1.DataType = System.Type.GetType("System.String");
			repuesto1.ColumnName = "REPUESTO1";
			repuesto1.ReadOnly=true;
			resultado2.Columns.Add(repuesto1);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn ordendev = new DataColumn();
			ordendev.DataType = System.Type.GetType("System.String");
			ordendev.ColumnName = "ORDENDEV";
			ordendev.ReadOnly=true;
			resultado2.Columns.Add(ordendev);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valoru1 = new DataColumn();
			valoru1.DataType = System.Type.GetType("System.String");
			valoru1.ColumnName = "VALORU1";
			valoru1.ReadOnly=true;
			resultado2.Columns.Add(valoru1);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn cantidad1 = new DataColumn();
			cantidad1.DataType = System.Type.GetType("System.String");
			cantidad1.ColumnName = "CANTIDAD1";
			cantidad1.ReadOnly=true;
			resultado2.Columns.Add(cantidad1);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn codrepuesto1 = new DataColumn();
			codrepuesto1.DataType = System.Type.GetType("System.String");
			codrepuesto1.ColumnName = "CODREPUESTO1";
			codrepuesto1.ReadOnly=true;
			resultado2.Columns.Add(codrepuesto1);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn trasnfe = new DataColumn();
			trasnfe.DataType = System.Type.GetType("System.String");
			trasnfe.ColumnName = "TRANSFE";
			trasnfe.ReadOnly=true;
			resultado2.Columns.Add(trasnfe);
			
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
	}
}
