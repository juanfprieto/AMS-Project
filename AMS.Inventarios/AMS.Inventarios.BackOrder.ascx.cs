using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Utilidades;
using AMS.Documentos;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial  class AsignacionBackOrder : System.Web.UI.UserControl
	{
		protected DataTable dtBackOrder;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlTipoPedido, "SELECT tped_codigo, tped_nombre FROM tpedido ORDER BY tped_nombre");
                //bind.PutDatasIntoDropDownList(ddlPrefTrans, "SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='TT' and tvig_vigencia = 'V' ORDER BY pdoc_nombre");
                Utils.llenarPrefijos(Response, ref ddlPrefTrans  , "%", "%", "TT");
                bind.PutDatasIntoDropDownList(ddlLinea, "SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem ORDER BY plin_codigo");
                bind.PutDatasIntoDropDownList(ddlTPedido, "SELECT pped_codigo, pped_nombre FROM ppedido ORDER BY pped_nombre");
				tbCodigoItem.Attributes.Add("onkeyup","ItemMask("+tbCodigoItem.ClientID+","+ddlLinea.ClientID+");");
				ddlLinea.Attributes.Add("onchange","ChangeLine("+ddlLinea.ClientID+","+tbCodigoItem.ClientID+");");
				tbCodigoItem.Attributes.Add("ondblclick","MostrarRefs("+tbCodigoItem.ClientID+","+ddlLinea.ClientID+");");
				CargarNumerosPedido();
			}
			if(Session["dtBackOrder"]==null)
				this.PrepararDtBackOrder();
			else
				dtBackOrder = (DataTable)Session["dtBackOrder"];
		}

		private void CargarNumerosPedido()
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlNPedido,
				@"SELECT distinct mp.mped_numepedi   
				 FROM mpedidoitem mp   
				 WHERE mp.pped_codigo='"+ddlTPedido.SelectedValue+@"' AND mp.mped_numepedi in(  
				  select dp.mped_numepedi from dpedidoitem dp   
				  where dp.pped_codigo=mp.pped_codigo and dp.mped_numepedi=mp.mped_numepedi and   
				  dp.dped_cantpedi+dp.dped_cantasig>dp.dped_cantfact)   
				 order by mp.mped_numepedi;");
		}
		
		protected void CambioTipoConsulta(Object Sender, EventArgs E)
		{
            if (rblTipoConsulta.SelectedValue == "T")
            {
                plRow1.Visible = plRow2.Visible = PlaceHolderOP.Visible = plcPE.Visible = false;
            }
            else if (rblTipoConsulta.SelectedValue == "I")
            {
                plRow1.Visible = true; plRow2.Visible = PlaceHolderOP.Visible = plcPE.Visible = false;
            }
            else if (rblTipoConsulta.SelectedValue == "P")
            {
                plRow2.Visible = true; plRow1.Visible = PlaceHolderOP.Visible = plcPE.Visible = false;
            }
            else if (rblTipoConsulta.SelectedValue == "PE")
            {
                plcPE.Visible = true; plRow1.Visible = PlaceHolderOP.Visible = plRow2.Visible = false;
            }
            else if (rblTipoConsulta.SelectedValue == "OP" || rblTipoConsulta.SelectedValue == "OT")
            {
                DatasToControls bind = new DatasToControls();
                if (rblTipoConsulta.SelectedValue == "OP")
                    //bind.PutDatasIntoDropDownList(DropDownListPO, "SELECT pdoc_codigo, pdoc_nombre FROM pdocumento where TDOC_TIPODOCU = 'OP' and tvig_vigencia = 'V' ");
                     Utils.llenarPrefijos(Response, ref DropDownListPO , "%", "%", "OP");
                else
                    //bind.PutDatasIntoDropDownList(DropDownListPO, "SELECT pdoc_codigo, pdoc_nombre FROM pdocumento where TDOC_TIPODOCU = 'OT' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref DropDownListPO , "%", "%", "OT");
                SeleccionaPrefijoOrden(Sender, E);
                PlaceHolderOP.Visible = true; plRow1.Visible = plcPE.Visible = plRow2.Visible = false;
            }
	    }

        protected void SeleccionaPrefijoOrden(Object Sender, EventArgs E)
		{
            DatasToControls bind = new DatasToControls();
            if (rblTipoConsulta.SelectedValue == "OP")
                // FALTA ADICIONAR EL ESTADO DE LAS OPRDENES DE PRODUCCION 
                bind.PutDatasIntoDropDownList(DropDownListNO, "SELECT mord_numeorde FROM mordenproduccion mo where mo.pdoc_codigo = '" + DropDownListPO.SelectedValue + "';");
            else
            {
                string otsback = @"SELECT MORD_NUMEORDE FROM (SELECT MO.mord_numeorde, SUM(DP.DPED_CANTPEDI - DPED_CANTASIG - DPED_CANTFACT) AS BACKORDER 
                                 FROM  morden mo, MPEDIDOTRANSFERENCIA MP,  DPEDIDOITEM DP  
                                 where mo.pdoc_codigo = '" + DropDownListPO.SelectedValue + @"' and mo.test_estado in ('A','B')  
                                   AND MO.PDOC_CODIGO = MP.PDOC_CODIGO AND MO.MORD_NUMEORDE = MP.MORD_NUMEORDE  
                                   AND MP.PPED_CODIGO = DP.PPED_CODIGO AND MP.MPED_NUMERO = DP.MPED_NUMEPEDI  
                                 GROUP BY MO.mord_numeorde) AS A WHERE A.BACKORDER > 0;";
                bind.PutDatasIntoDropDownList(DropDownListNO, otsback);
            }
        }
		
		protected void RealizarAsignacion(Object Sender, EventArgs E)
		{
            if (rblTipoConsulta.SelectedValue == "T" || rblTipoConsulta.SelectedValue == "P" || rblTipoConsulta.SelectedValue == "PE" || rblTipoConsulta.SelectedValue == "OT" || rblTipoConsulta.SelectedValue == "OP")
				RealizarConsultaBackOrder();
			else
			{
				//Revisamos primero que la linea digitada exista
				string codI = "";
				if(!Referencias.Guardar(tbCodigoItem.Text.Trim(),ref codI,(ddlLinea.SelectedValue.Split('-'))[1]))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + tbCodigoItem.Text.Trim() + " no es valido para la linea de bodega " + ddlLinea.SelectedItem.Text + ".\\nRevisar Por Favor!");
					return;
				}
				if(!Referencias.RevisionSustitucion(ref codI, (ddlLinea.SelectedValue.Split('-'))[0]))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + tbCodigoItem.Text.Trim() + " esta registrado.\\nRevisar Por Favor!");
					return;
				}
				string icodTmp2 = "";
				Referencias.Editar(codI,ref icodTmp2,(ddlLinea.SelectedValue.Split('-'))[1]);
				if(tbCodigoItem.Text.Trim() != icodTmp2)
                    Utils.MostrarAlerta(Response, "El codigo " + tbCodigoItem.Text.Trim() + " ha sido sustituido.\\nEl codigo actual es " + icodTmp2 + "!");
				RealizarConsultaBackOrder();
			}
		}
		
		protected void ReiniciarControl(Object Sender, EventArgs E)
		{
			Response.Redirect("" + ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Inventarios.BackOrder");
		}
		
		protected void PrepararDtBackOrder()
		{
			dtBackOrder = new DataTable();
			dtBackOrder.Columns.Add(new DataColumn("CODIGOORIGINAL",System.Type.GetType("System.String")));//0
			dtBackOrder.Columns.Add(new DataColumn("LINEA",System.Type.GetType("System.String")));//1
			dtBackOrder.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//2
			dtBackOrder.Columns.Add(new DataColumn("PEDIDO",System.Type.GetType("System.String")));//3
			dtBackOrder.Columns.Add(new DataColumn("PREFIJOPEDIDO",System.Type.GetType("System.String")));//4
			dtBackOrder.Columns.Add(new DataColumn("NUMEROPEDIDO",System.Type.GetType("System.String")));//5
			dtBackOrder.Columns.Add(new DataColumn("NIT",System.Type.GetType("System.String")));//6
			dtBackOrder.Columns.Add(new DataColumn("CANTIDADPENDIENTE",System.Type.GetType("System.Int32")));//7
			dtBackOrder.Columns.Add(new DataColumn("CANTIDADDISPONIBLE",System.Type.GetType("System.Int32")));//8
			dtBackOrder.Columns.Add(new DataColumn("ALMACEN",System.Type.GetType("System.String")));//9
			dtBackOrder.Columns.Add(new DataColumn("CANTIDADASIGNADA",System.Type.GetType("System.Int32")));//10
			dtBackOrder.Columns.Add(new DataColumn("VALORUNIDAD",System.Type.GetType("System.Double")));//11
			dtBackOrder.Columns.Add(new DataColumn("VALORTOTAL",System.Type.GetType("System.Double")));//12
			dtBackOrder.Columns.Add(new DataColumn("LISTAEMPAQUE",System.Type.GetType("System.String")));//13
			dtBackOrder.Columns.Add(new DataColumn("OTRELACIONADA",System.Type.GetType("System.String")));//14
		}
		
		protected void RealizarConsultaBackOrder()
		{
			int i,j;
			dtBackOrder.Rows.Clear();
			//Aqui vamos a realizar la consulta y a mostrar la grilla de asignacion de backorder
			string sqlQuery = "";
			string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
			if(this.rblTipoConsulta.SelectedValue == "T")
				sqlQuery = @"SELECT DISTINCT DPI.mite_codigo, MIT.plin_codigo, DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo, PLIN.plin_tipo), 
                                MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi AS character(10)),MPI.pped_codigo,MPI.mped_numepedi, MPI.mnit_nit, 
                                DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact, MSI.msal_cantactual - MSI.msal_cantasig, MPI.palm_almacen, DPI.dped_valounit 
                           FROM mpedidoitem MPI, dpedidoitem DPI, mitems MIT, msaldoitemalmacen MSI, plineaitem PLIN 
                           WHERE MIT.mite_codigo = DPI.mite_codigo AND DPI.mnit_nit = MPI.mnit_nit AND DPI.pped_codigo = MPI.pped_codigo 
                            AND DPI.mped_numepedi = MPI.mped_numepedi AND MPI.mped_claseregi in ('C','M') 
                            AND (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)>0 AND DPI.mite_codigo = MSI.mite_codigo 
                            AND MPI.palm_almacen = MSI.palm_almacen AND MSI.pano_ano="+ano_cinv+" AND PLIN.plin_codigo = MIT.plin_codigo ORDER BY MIT.plin_codigo,DPI.mite_codigo,MPI.mnit_nit";
			else if(this.rblTipoConsulta.SelectedValue == "I")
			{
				string codI = "";
				if(!Referencias.Guardar(tbCodigoItem.Text.Trim(),ref codI,(ddlLinea.SelectedValue.Split('-'))[1]))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + tbCodigoItem.Text.Trim() + " no es valido para la linea de bodega " + ddlLinea.SelectedItem.Text + ".\\nRevisar Por Favor!");
					return;
				}
				if(!Referencias.RevisionSustitucion(ref codI, (ddlLinea.SelectedValue.Split('-'))[0]))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + tbCodigoItem.Text.Trim() + " El codigo " + tbCodigoItem.Text.Trim() + " esta registrado.\\nRevisar Por Favor!");
					return;
				}
				string icodTmp2 = "";
				Referencias.Editar(codI,ref icodTmp2,(ddlLinea.SelectedValue.Split('-'))[1]);
				if(tbCodigoItem.Text.Trim() != icodTmp2)
                    Utils.MostrarAlerta(Response, "El codigo " + tbCodigoItem.Text.Trim() + " ha sido sustituido.\\nEl codigo actual es " + icodTmp2 + "!");
				sqlQuery = @"SELECT DISTINCT DPI.mite_codigo, MIT.plin_codigo, DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo, PLIN.plin_tipo), 
                            MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi AS character(10)),MPI.pped_codigo,MPI.mped_numepedi, MPI.mnit_nit, 
                            DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact, MSI.msal_cantactual - MSI.msal_cantasig, MPI.palm_almacen, DPI.dped_valounit 
                            FROM mpedidoitem MPI, dpedidoitem DPI, mitems MIT, msaldoitemalmacen MSI, plineaitem PLIN WHERE MIT.mite_codigo = DPI.mite_codigo 
                            AND DPI.mnit_nit = MPI.mnit_nit AND DPI.pped_codigo = MPI.pped_codigo AND DPI.mped_numepedi = MPI.mped_numepedi 
                            AND MPI.mped_claseregi in ('C','M') AND (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)>0 
                            AND DPI.mite_codigo = MSI.mite_codigo AND MPI.palm_almacen = MSI.palm_almacen AND MSI.pano_ano="+ano_cinv+" AND DPI.mite_codigo='"+codI+"' AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,DPI.mite_codigo,MPI.mnit_nit";
			}
			else if(this.rblTipoConsulta.SelectedValue == "P")
				sqlQuery = @"SELECT DISTINCT DPI.mite_codigo, MIT.plin_codigo, DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo, PLIN.plin_tipo), 
                           MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi AS character(10)),MPI.pped_codigo,MPI.mped_numepedi, MPI.mnit_nit, 
                           DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact, MSI.msal_cantactual - MSI.msal_cantasig, MPI.palm_almacen, DPI.dped_valounit 
                           FROM mpedidoitem MPI, dpedidoitem DPI, mitems MIT, msaldoitemalmacen MSI, ppedido PPE, plineaitem PLIN 
                           WHERE MIT.mite_codigo = DPI.mite_codigo AND DPI.mnit_nit = MPI.mnit_nit AND DPI.pped_codigo = MPI.pped_codigo 
                            AND DPI.mped_numepedi = MPI.mped_numepedi AND MPI.mped_claseregi in ('C','M') AND (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)>0 
                            AND DPI.mite_codigo = MSI.mite_codigo AND MPI.palm_almacen = MSI.palm_almacen AND MSI.pano_ano="+ano_cinv+" AND DPI.pped_codigo = PPE.pped_codigo AND PPE.tped_codigo = '"+this.ddlTipoPedido.SelectedValue+"' AND PLIN.plin_codigo=MIT.plin_codigo";
            else if (this.rblTipoConsulta.SelectedValue == "OP")
                sqlQuery = @"SELECT DPI.mite_codigo,MIT.plin_codigo, DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo, PLIN.plin_tipo), 
                            MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi AS character(10)),MPI.pped_codigo,MPI.mped_numepedi, 
                            MPI.mnit_nit, DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact, MSI.msal_cantactual - MSI.msal_cantasig, MPI.palm_almacen,DPI.dped_valounit 
                           FROM mpedidoitem MPI, dpedidoitem DPI,mitems MIT, msaldoitemalmacen MSI, plineaitem PLIN, MPEDIDOPRODUCCIONTRANSFERENCIA mop 
                           WHERE MIT.mite_codigo = DPI.mite_codigo AND PLIN.plin_codigo=MIT.plin_codigo AND  
					 DPI.pped_codigo = MPI.pped_codigo AND DPI.mped_numepedi = MPI.mped_numepedi AND  
					 (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)>0 AND  
					 DPI.mite_codigo = MSI.mite_codigo AND MIT.mite_codigo = MSI.mite_codigo AND  
                     MPI.palm_almacen = MSI.palm_almacen AND MSI.pano_ano=" + ano_cinv + @" AND  
					 mpi.pped_codigo = mop.pped_codigo AND mpi.mped_numepedi = mop.mped_numero AND  
                     (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)<= MSI.MSAL_CANTACTUAL AND  
					 mop.pdoc_codigo = '"+this.DropDownListPO.SelectedValue+"' AND mop.mord_numeorde="+this.DropDownListNO.SelectedValue+";";
			else if (this.rblTipoConsulta.SelectedValue == "OT") // BUSCAR LA TABLA QUE ENLAZA LA ORDEN CON EL PEDIDO
            //     sqlQuery = "SELECT DISTINCT DPI.mite_codigo, MIT.plin_codigo, DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo, PLIN.plin_tipo), MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi AS character(10)),MPI.pped_codigo,MPI.mped_numepedi, MPI.mnit_nit, DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact, MSI.msal_cantactual - MSI.msal_cantasig, MPI.palm_almacen, DPI.dped_valounit FROM mpedidoitem MPI, dpedidoitem DPI, mitems MIT, msaldoitemalmacen MSI, ppedido PPE, plineaitem PLIN, mordenproduccion mop WHERE MIT.mite_codigo = DPI.mite_codigo AND DPI.mnit_nit = MPI.mnit_nit AND DPI.pped_codigo = MPI.pped_codigo AND DPI.mped_numepedi = MPI.mped_numepedi AND MPI.mped_claseregi in ('C','M') AND (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)>0 AND DPI.mite_codigo = MSI.mite_codigo AND MPI.palm_almacen = MSI.palm_almacen AND MSI.pano_ano=" + ano_cinv + " AND dpi.pped_codigo = mop.pped_codipedi AND dpi.mped_numepedi = mop.mped_numepedi AND mop.pdoc_codigo = '" + this.DropDownListPO.SelectedValue + "' AND " + this.DropDownListNO.SelectedValue + " = mop.mord_numeorde AND PLIN.plin_codigo=MIT.plin_codigo";
			       sqlQuery = @"SELECT DISTINCT DPI.mite_codigo, MIT.plin_codigo, DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo, PLIN.plin_tipo),  
                                 MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi AS character(10)),MPI.pped_codigo,MPI.mped_numepedi,  
                                 MPI.mnit_nit, DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact, MSI.msal_cantactual - MSI.msal_cantasig,  
                                 MPI.palm_almacen, DPI.dped_valounit  
                              FROM mpedidoitem MPI, dpedidoitem DPI, mitems MIT, msaldoitemalmacen MSI, ppedido PPE, plineaitem PLIN, MPEDIDOTRANSFERENCIA mop  
                             WHERE MIT.mite_codigo = DPI.mite_codigo AND DPI.mnit_nit = MPI.mnit_nit AND DPI.pped_codigo = MPI.pped_codigo  
                              AND DPI.mped_numepedi = MPI.mped_numepedi AND MPI.mped_claseregi in ('C','M')  
                              AND (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)>0 AND DPI.mite_codigo = MSI.mite_codigo  
                              AND MPI.palm_almacen = MSI.palm_almacen AND MSI.pano_ano=" + ano_cinv + @"  
                              AND dpi.pped_codigo = mop.pped_codiGO AND dpi.mped_numepedi = mop.mped_numeRO  
                              AND (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)<= MSI.MSAL_CANTACTUAL  
                              AND mop.pdoc_codigo = '" + this.DropDownListPO.SelectedValue + "' AND " + this.DropDownListNO.SelectedValue + " = mop.mord_numeorde AND PLIN.plin_codigo=MIT.plin_codigo; ";

            else if (this.rblTipoConsulta.SelectedValue == "PE")
                sqlQuery = @"SELECT DISTINCT DPI.mite_codigo, MIT.plin_codigo, DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo, PLIN.plin_tipo), 
                            MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi AS character(10)),MPI.pped_codigo,MPI.mped_numepedi, MPI.mnit_nit, 
                            DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact, MSI.msal_cantactual - MSI.msal_cantasig, MPI.palm_almacen, DPI.dped_valounit  
                           FROM mpedidoitem MPI, dpedidoitem DPI, mitems MIT, msaldoitemalmacen MSI, ppedido PPE, plineaitem PLIN 
                           WHERE MIT.mite_codigo = DPI.mite_codigo AND DPI.mnit_nit = MPI.mnit_nit AND DPI.pped_codigo = MPI.pped_codigo AND DPI.mped_numepedi = MPI.mped_numepedi 
                           AND MPI.mped_claseregi in ('C','M') AND (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)>0 AND DPI.mite_codigo = MSI.mite_codigo 
                           AND MPI.palm_almacen = MSI.palm_almacen AND MSI.pano_ano=" + ano_cinv + " AND DPI.pped_codigo = PPE.pped_codigo AND MPI.PPED_CODIGO='" + ddlTPedido.SelectedValue + "' AND MPI.MPED_NUMEPEDI=" + ddlNPedido.SelectedValue + " AND PLIN.plin_codigo=MIT.plin_codigo";

            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, sqlQuery);
            string tipoPedido="";
            bool siAsignar = false;
            double saldoMora, saldo, cupo;
			for(i=0;i<ds.Tables[0].Rows.Count;i++)
			{
                if (i == 0 || i > 0 && ds.Tables[0].Rows[i-1][4].ToString() != ds.Tables[0].Rows[i][4].ToString())
                   tipoPedido=DBFunctions.SingleData("SELECT tped_codigo FROM ppedido WHERE pped_codigo='"+ds.Tables[0].Rows[i][4].ToString()+"'");
				if(tipoPedido == "T")
                    siAsignar = true; 
                else
				{
					//Verificar que este autorizado el pedido si es mayor
                    string auth = DBFunctions.SingleData("SELECT coalesce(mped_autoriza,'N') FROM MPEDIDOCLIENTEAUTORIZACION WHERE pdoc_codigo='" +
                                        ds.Tables[0].Rows[i][4].ToString() + "' and mped_numepedi=" + ds.Tables[0].Rows[i][5].ToString());
                    if (auth == "S")   //Verificar reglas de cartera del pedido si existe el Cliente
                        siAsignar = true;  // el pedido fue autorizado inicialmente  pasa
                    else  // el pedido no ha sido autorizado, toca validar la cartera y el cupo
                    {
                        saldoMora = Utilidades.Clientes.ConsultarSaldoMora(ds.Tables[0].Rows[i][6].ToString());
                        saldo = Utilidades.Clientes.ConsultarSaldo(ds.Tables[0].Rows[i][6].ToString());
                        if (DBFunctions.RecordExist("SELECT mnit_nit FROM MCLIENTE WHERE mnit_nit='" + ds.Tables[0].Rows[i][6].ToString() + "';"))
                        {
                            cupo = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(MCLI_CUPOCRED,0) FROM mcliente WHERE mnit_nit='" + ds.Tables[0].Rows[i][6].ToString() + "'"));
                        }
                        else
                        {
                            cupo = 0;
                    //       Utils.MostrarAlerta(Response, "El cliente " + ds.Tables[0].Rows[i][6].ToString() + " NO esta PARAMETRIZADO en la tabla de CLIENTES..  asignelo, revice saldo en cartera.\\nRevisar Por Favor!");
                        }
                        if ((saldo <= cupo && saldoMora <= 0))
                            siAsignar = true;
                        else
                            Utils.MostrarAlerta(Response, "El cliente " + ds.Tables[0].Rows[i][6].ToString() + " NO le asigna back_order porque tiene CUPO en cartera por " + cupo.ToString("C") + " tiene saldo en cartera por " + saldo.ToString("C") + " saldo en cartera MORA por " + saldoMora.ToString("C") + ".\\nRevisar Por Favor!");
                    
                    }
				}

				DataRow fila = dtBackOrder.NewRow();
				fila[0] = ds.Tables[0].Rows[i][0].ToString();
				fila[1] = ds.Tables[0].Rows[i][1].ToString();
				fila[2] = ds.Tables[0].Rows[i][2].ToString();
				fila[3] = ds.Tables[0].Rows[i][3].ToString();
				fila[4] = ds.Tables[0].Rows[i][4].ToString();
				fila[5] = ds.Tables[0].Rows[i][5].ToString();
				fila[6] = ds.Tables[0].Rows[i][6].ToString();
				fila[7] = Convert.ToInt32(ds.Tables[0].Rows[i][7]);
				fila[8] = Convert.ToInt32(ds.Tables[0].Rows[i][8]);
				fila[9] = ds.Tables[0].Rows[i][9].ToString();
				//Ahora vamos a asignar la cantidad disponible a los pedidos que tienen pendientes
				int cantidadAsignadaBackOrder = 0;
				DataRow[] selection = dtBackOrder.Select("CODIGO='"+ds.Tables[0].Rows[i][2].ToString()+"' AND ALMACEN='"+ds.Tables[0].Rows[i][9].ToString()+"'");
				for(j=0;j<selection.Length;j++)
					cantidadAsignadaBackOrder += Convert.ToInt32(selection[j][10]);
				if((Convert.ToInt32(ds.Tables[0].Rows[i][8]) - cantidadAsignadaBackOrder)==0)
					fila[10] = 0;
				else if((Convert.ToInt32(ds.Tables[0].Rows[i][8]) - cantidadAsignadaBackOrder)>=Convert.ToInt32(ds.Tables[0].Rows[i][7]))
					fila[10] = Convert.ToInt32(ds.Tables[0].Rows[i][7]);
				else if((Convert.ToInt32(ds.Tables[0].Rows[i][8]) - cantidadAsignadaBackOrder)<Convert.ToInt32(ds.Tables[0].Rows[i][7]))
					fila[10] = (Convert.ToInt32(ds.Tables[0].Rows[i][8]) - cantidadAsignadaBackOrder);
				fila[11] = Convert.ToDouble(ds.Tables[0].Rows[i][10]);
				fila[12] = Convert.ToInt32(fila[10])*Convert.ToDouble(ds.Tables[0].Rows[i][10]);
				//Ahora para determinar cual es el numero de la lista de empaque, debemos determinar si es una transferencia de taller o un pedido de cliente
				if(tipoPedido=="T")
				{
					if (this.rblTipoConsulta.SelectedValue == "OP")
					{
						//Revisamos si la orden de trabajo asociada a este pedido esta en proceso o no
						if(DBFunctions.SingleData("SELECT MOR.test_estado FROM dbxschema.mordenproduccion MOR, dbxschema.MPEDIDOPRODUCCIONTRANSFERENCIA MPT WHERE MPT.pdoc_codigo = MOR.pdoc_codigo AND MPT.mord_numeorde = MOR.mord_numeorde AND MPT.pped_codigo = '"+ds.Tables[0].Rows[i][4].ToString()+"' AND MPT.mped_numero = "+ds.Tables[0].Rows[i][5].ToString()+"")=="A" && Convert.ToInt32(fila[10])>0)
						    fila[14] = DBFunctions.SingleData("SELECT pdoc_codigo CONCAT '-' CONCAT CAST(mord_numeorde AS character(10)) FROM MPEDIDOPRODUCCIONTRANSFERENCIA WHERE pped_codigo='"+ds.Tables[0].Rows[i][4].ToString()+"' AND mped_numero="+ds.Tables[0].Rows[i][5].ToString()+"").Trim();
					}
					else
					{
						//Revisamos si la orden de trabajo asociada a este pedido esta en proceso o no
						if(DBFunctions.SingleData("SELECT MOR.test_estado FROM dbxschema.morden MOR, dbxschema.mpedidotransferencia MPT WHERE MPT.pdoc_codigo = MOR.pdoc_codigo AND MPT.mord_numeorde = MOR.mord_numeorde AND MPT.pped_codigo = '"+ds.Tables[0].Rows[i][4].ToString()+"' AND MPT.mped_numero = "+ds.Tables[0].Rows[i][5].ToString()+"")=="A" && Convert.ToInt32(fila[10])>0)
							fila[14] = DBFunctions.SingleData("SELECT pdoc_codigo CONCAT '-' CONCAT CAST(mord_numeorde AS character(10)) FROM mpedidotransferencia WHERE pped_codigo='"+ds.Tables[0].Rows[i][4].ToString()+"' AND mped_numero="+ds.Tables[0].Rows[i][5].ToString()+"").Trim();
					}
				}
				if(siAsignar)
				{
					dtBackOrder.Rows.Add(fila);
				}
                if (tipoPedido=="T" && (dtBackOrder.Rows[0][14].ToString() == "" || dtBackOrder.Rows[0][14].ToString() == null))
                {
                    Utils.MostrarAlerta(Response, "No hay disponibilidad para realizar esta asignación.");
                    return;
                }
			}
			RealizarAsignacion();
		}
		
		protected void RealizarAsignacion()
		{
			int maxLista,maxL;
			string tip="";
			try{maxLista=Convert.ToInt32(DBFunctions.SingleData("SELECT CINV_TAMLISTEMPA FROM CINVENTARIO;"));}
			catch{maxLista=0;}

			//Ahora volvemos a recorrer la tabla y creamos las listas de empaque o facturas que se necesiten
			for(int i=0;i<dtBackOrder.Rows.Count;i++)
			{
				//Primero revisamos si el pedido es de cliente o es tipo trasnferencia
				if(DBFunctions.SingleData("SELECT tped_codigo FROM ppedido WHERE pped_codigo='"+dtBackOrder.Rows[i][4].ToString()+"'")=="T")
				{
					DataRow[] selection = dtBackOrder.Select("OTRELACIONADA='"+dtBackOrder.Rows[i][14].ToString()+"' AND NIT='"+dtBackOrder.Rows[i][6].ToString()+"' AND ALMACEN='"+dtBackOrder.Rows[i][9].ToString()+"'");
					string numTrans = this.RevisarListaEmpaque(selection);
					if(numTrans != "")
						dtBackOrder.Rows[i][13] = numTrans;
					else
					{
						//dtBackOrder.Rows[i][13] = "Pendiente";
						double totalTrans = 0;
						for(int j=0;j<selection.Length;j++)
							totalTrans += Convert.ToDouble(selection[j][12]);
						uint numeroTransferencia = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefTrans.SelectedValue+"'"));
						string codVend = "";
						if(rblTipoConsulta.SelectedValue == "OP")
						{
							codVend = DBFunctions.SingleData("SELECT pven_codigo FROM mordenproduccion WHERE pdoc_codigo='"+(dtBackOrder.Rows[i][14].ToString().Split('-'))[0]+"' AND mord_numeorde="+(dtBackOrder.Rows[i][14].ToString().Split('-'))[1]+"");
							tip="OP";
						}
						else
						{
							codVend = DBFunctions.SingleData("SELECT pven_codigo FROM morden WHERE pdoc_codigo='"+(dtBackOrder.Rows[i][14].ToString().Split('-'))[0]+"' AND mord_numeorde="+(dtBackOrder.Rows[i][14].ToString().Split('-'))[1]+"");
							tip="OT";
						}
						PedidoFactura transferencia = new PedidoFactura(dtBackOrder.Rows[i][6].ToString(),ddlPrefTrans.SelectedValue,numeroTransferencia,DateTime.Now,0,"",0,0,totalTrans,totalTrans,dtBackOrder.Rows[i][9].ToString(),codVend,tip);
						for(int j=0;j<selection.Length;j++)
						{
							double porcDesc = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_porcdesc FROM dpedidoitem WHERE pped_codigo='"+selection[j][4].ToString()+"' AND mped_numepedi="+selection[j][5].ToString()+" AND mite_codigo='"+selection[j][0].ToString()+"'"));
							double cantPedida = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi FROM dpedidoitem WHERE pped_codigo='"+selection[j][4].ToString()+"' AND mped_numepedi="+selection[j][5].ToString()+" AND mite_codigo='"+selection[j][0].ToString()+"'"));
							transferencia.InsertaFila(selection[j][0].ToString(),Convert.ToDouble(selection[j][10]),Convert.ToDouble(selection[j][11]),0,cantPedida,porcDesc,selection[j][3].ToString());
						}
						//Ahora realizamos grabamos la transferencia
						if(transferencia.RealizarFacDir())
						{
							dtBackOrder.Rows[i][13] = ddlPrefTrans.SelectedValue+"-"+numeroTransferencia.ToString();
							rblTipoConsulta.Enabled = lbInfo1.Enabled = tbCodigoItem.Enabled = ddlLinea.Enabled = btnRealizar.Enabled =lbInfo2.Enabled = ddlTipoPedido.Enabled = ddlPrefTrans.Enabled =false;
							btnReiniciar.Visible = true;
                            FormatosDocumentos formatoFactura = new FormatosDocumentos();
                            try
                            {
                                formatoFactura.Prefijo = ddlPrefTrans.SelectedValue;
                                formatoFactura.Numero = (int)numeroTransferencia;
                                formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlPrefTrans.SelectedValue + "'");
                                if (formatoFactura.Codigo != string.Empty)
                                {
                                    if (formatoFactura.Cargar_Formato())
                                        Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                                }
                            }
                            catch
                            {
                                lb.Text = "Error al generar el formato pedido 1. Detalles : <br>" + formatoFactura.Mensajes;
                            }
						}
						else
							lb.Text += "<br>Error "+transferencia.ProcessMsg;
					}
				}
				else
				{
					//Aqui debemos revisar si dentro de esta consulta ya existen listas de empaques para este nit en este almacen
					DataRow[] selection = dtBackOrder.Select("NIT='"+dtBackOrder.Rows[i][6].ToString()+"' AND ALMACEN='"+dtBackOrder.Rows[i][9].ToString()+"'");
					//Ahora revisamos si ya existe una lista de empaque para este cliente en este almacen
					string numLis = this.RevisarListaEmpaque(selection);
					if(numLis != "")
						dtBackOrder.Rows[i][13] = numLis;
					else
					{
						uint numeroLista = Convert.ToUInt32(DBFunctions.SingleData("SELECT coalesce(MAX(mlis_numero),0) FROM mlistaempaque"))+1;
						ListaEmpaque listaBackOrder = new ListaEmpaque(numeroLista,dtBackOrder.Rows[i][6].ToString(),DateTime.Now,dtBackOrder.Rows[i][9].ToString(),HttpContext.Current.User.Identity.Name.ToLower(),"C",null);
						maxL=maxLista;
						if(maxL==0) maxL=selection.Length;
						for(int j=0;j<selection.Length && j<maxL;j++)
							listaBackOrder.AgregarItem(selection[j][0].ToString(),selection[j][4].ToString(),selection[j][5].ToString(),Convert.ToInt32(selection[j][10]),Convert.ToDouble(selection[j][11]));
						//Vamos a almacenar la lista de empaque en la base de datos
						if(listaBackOrder.AlmacenarLista(true))
						{
							dtBackOrder.Rows[i][13] = numeroLista.ToString();
							ddlNPedido.Enabled = ddlTPedido.Enabled = rblTipoConsulta.Enabled = lbInfo1.Enabled = tbCodigoItem.Enabled = ddlLinea.Enabled = btnRealizar.Enabled =lbInfo2.Enabled = ddlTipoPedido.Enabled = ddlPrefTrans.Enabled = false;
							btnReiniciar.Visible = true;
						}
						else
							lb.Text+= "<br><br>Error : "+listaBackOrder.ProcessMsg;
					}
				}
			}
			BindDgBackOrder();
		}
		
		protected void DgBackOrderPage(Object sender, DataGridPageChangedEventArgs e)
		{
			this.dgBackOrder.CurrentPageIndex = e.NewPageIndex;
			this.BindDgBackOrder();
		}
		
		protected void BindDgBackOrder()
		{
			dgBackOrder.DataSource = dtBackOrder;
			Session["dtBackOrder"] = dtBackOrder;
			dgBackOrder.DataBind();
			DatasToControls.JustificacionGrilla(dgBackOrder,dtBackOrder);
		}
		
		protected string RevisarListaEmpaque(DataRow[] selection)
		{
			string numLis = "";
			for(int i=0;i<selection.Length;i++)
				if(selection[i][13].ToString()!="")
					numLis = selection[i][13].ToString();
			return numLis;
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
		protected void ddlTPedido_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CargarNumerosPedido();
		}
	}
}
