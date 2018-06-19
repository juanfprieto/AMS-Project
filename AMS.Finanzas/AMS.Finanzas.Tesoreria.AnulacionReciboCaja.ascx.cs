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
using AMS.Forms;
using AMS.Contabilidad;
using System.Configuration;
using AMS.Tools;
using AMS.Documentos;

namespace AMS.Finanzas.Tesoreria
{
	public partial class AnulacionReciboCaja : System.Web.UI.UserControl
	{
        protected DataTable tablaFacturas, tablaVarios, tablaPagos, tablaRetenciones, tablaObligaciones;
		protected DataSet facturas,varios,pagos,retenciones, obligaciones;
        protected DataGrid gridObligaciones_financieras;
        ProceHecEco contaOnline = new ProceHecEco();
        protected FormatosDocumentos formatoRecibo=new FormatosDocumentos();
		
		protected void Page_Load(object sender, EventArgs e)
		{
            if(!IsPostBack)//Principio
			{
				Session.Clear();

                if (Request.QueryString["preAn"] != null && Request.QueryString["consAn"] != null)
                {
                    try
                    {
                        Imprimir.ImprimirRPT(Response, Request.QueryString["preAn"], Convert.ToInt32(Request.QueryString["consAn"]), true);
                    }
                    catch
                    {
                        lb.Text += "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>";
                    }
                }

                anular.Enabled = false;
                if (Request.QueryString["errDocAnu"] != null)
                {
                    Utils.MostrarAlerta(Response, "Falta seleccionar el Prefijo del Documento de Anulación!");
                }
				if(Request.QueryString["ex"]!=null)
                    Utils.MostrarAlerta(Response, "Anulación exitosa del documento con prefijo "+Request.QueryString["prefC"]+" y número "+Request.QueryString["numC"]+". Anulacion con prefijo: "+Request.QueryString["preAn"]+" y número "+Request.QueryString["consAn"]+ "");
				DatasToControls bind=new DatasToControls();
                bind.PutDatasIntoDropDownList(prefijoRecibo, "SELECT pdoc_codigo,pdoc_descripcion || ' - ' || pdoc_codigo FROM pdocumento where tdoc_tipodocu IN('RP','RC','CE') and tvig_vigencia = 'V' ORDER BY pdoc_descripcion");
                if (!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='AN' and tvig_vigencia = 'V' "))
                {
                    Utils.MostrarAlerta(Response, "No hay documentos del tipo AN definidos para anulación");
                }
                else
                {
                    //bind.PutDatasIntoDropDownList(ddlDocAnu, "SELECT pdoc_codigo,pdoc_codigo ||'-'||pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='AN' and tvig_vigencia = 'V' ORDER BY pdoc_descripcion");
                    Utils.llenarPrefijos(Response, ref ddlDocAnu  , "%", "%", "AN");
                }
                    //Si es un egreso, cargamos solo aquellos cuyos pagos nos esten entregados
				if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"'").Trim()=="CE")
					bind.PutDatasIntoDropDownList(numeroRecibo,"SELECT DISTINCT MC.mcaj_numero FROM dbxschema.mcaja MC,dbxschema.mcajapago MCP WHERE MC.pdoc_codigo=MCP.pdoc_codigo AND MC.mcaj_numero=MCP.mcaj_numero AND MCP.test_estado NOT IN ('N') AND MC.pdoc_codigo='"+prefijoRecibo.SelectedValue+"' AND MC.test_estadodoc<>'N' ORDER BY MC.mcaj_numero");
				//Si es un recibo de caja, cargamos solo los del dia
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"'").Trim()=="RC")
                     {
                        DateTime fecha = DateTime.Today.AddMonths(-2);
                        //bind.PutDatasIntoDropDownList(numeroRecibo,"SELECT mcaj_numero FROM mcaja WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"' AND test_estadodoc<>'N' AND mcaj_fecha='"+DateTime.Now.ToString("yyyy-MM-dd")+"'");
					    bind.PutDatasIntoDropDownList(numeroRecibo,"SELECT DISTINCT MC.mcaj_numero FROM dbxschema.mcaja MC,dbxschema.mcajapago MCP WHERE MC.pdoc_codigo=MCP.pdoc_codigo AND MC.mcaj_numero=MCP.mcaj_numero AND MCP.test_estado NOT IN('G','D','E') AND MC.pdoc_codigo='"+prefijoRecibo.SelectedValue+"' AND MC.test_estadodoc<>'N' AND MC.mcaj_fecha >= '"+fecha.ToString("yyyy-MM-dd")+"' ORDER BY MC.mcaj_numero");
                     }
				    //Si es un provisional, cargamos todos los q hayan
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"'").Trim()=="RP")
					    bind.PutDatasIntoDropDownList(numeroRecibo,"SELECT mcaj_numero FROM mcaja WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"' AND test_estadodoc<>'N'");
					    //bind.PutDatasIntoDropDownList(numeroRecibo,"SELECT mcaj_numero FROM mcaja WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"' AND test_estadodoc<>'N'");
			}
			else
			{
				if(Session["tablaFacturas"]!=null)
					tablaFacturas=(DataTable)Session["tablaFacturas"];
				if(Session["tablaPagos"]!=null)
					tablaPagos=(DataTable)Session["tablaPagos"];
				if(Session["tablaVarios"]!=null)
					tablaVarios=(DataTable)Session["tablaVarios"];
				if(Session["tablaRetenciones"]!=null)
					tablaRetenciones=(DataTable)Session["tablaRetenciones"];
                if(Session["tablaObligaciones"]!= null)
                    tablaObligaciones = (DataTable)Session["tablaObligaciones"];
                
            }
                
        }

		protected void Llenar_Grid_Facturas()
		{
			DataRow fila;
			int i;
			if(facturas.Tables[0].Rows.Count!=0)
			{
				if(Session["tablaFacturas"]==null)
					this.Preparar_TablaFacturas();
				for(i=0;i<facturas.Tables[0].Rows.Count;i++)
				{
					fila=tablaFacturas.NewRow();
					fila["PREFIJO"]=facturas.Tables[0].Rows[i][0].ToString();
					fila["NUMERODOCUMENTO"]=facturas.Tables[0].Rows[i][1].ToString();
					fila["VALOR"]=System.Convert.ToDouble(facturas.Tables[0].Rows[i][2].ToString()).ToString("C");
					tablaFacturas.Rows.Add(fila);
				}
				gridPagos_Abonos.DataSource=tablaFacturas;
				gridPagos_Abonos.DataBind();
				Session["tablaFacturas"]=tablaFacturas;
			}
		}
		
		protected void Llenar_Grid_Varios()
		{
			DataRow filaVarios;
			int i;
			if(varios.Tables[0].Rows.Count!=0)
			{
				if(Session["tablaVarios"]==null)
					this.Preparar_TablaVarios();
				for(i=0;i<varios.Tables[0].Rows.Count;i++)
				{
					filaVarios=tablaVarios.NewRow();
					filaVarios["DESCRIPCION"]=varios.Tables[0].Rows[i][0].ToString();
					filaVarios["CUENTA"]=varios.Tables[0].Rows[i][1].ToString();
                    filaVarios["SEDE"] = DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + varios.Tables[0].Rows[i][2].ToString() + "'");
					filaVarios["CENTROCOSTO"]=DBFunctions.SingleData("SELECT pcen_nombre FROM pcentrocosto WHERE pcen_codigo='"+varios.Tables[0].Rows[i][3].ToString()+"'");
					filaVarios["PREFIJO"]=varios.Tables[0].Rows[i][4].ToString();
					filaVarios["NUMERO"]=varios.Tables[0].Rows[i][5].ToString();
					filaVarios["NIT"]=varios.Tables[0].Rows[i][6].ToString();
					filaVarios["VALOR"]=System.Convert.ToDouble(varios.Tables[0].Rows[i][7].ToString()).ToString("C");
					if(varios.Tables[0].Rows[i][8].ToString()=="D")
						filaVarios["NATURALEZA"]="Débito";
					else
						filaVarios["NATURALEZA"]="Crédito";
					filaVarios["VALORBASE"]=System.Convert.ToDouble(varios.Tables[0].Rows[i][9].ToString()).ToString("C");
					tablaVarios.Rows.Add(filaVarios);
				}
				gridVarios.DataSource=tablaVarios;
				gridVarios.DataBind();
				Session["tablaVarios"]=tablaVarios;
			}
		}
		
		protected void Llenar_Grid_Pagos()
		{
			DataRow filaPagos;
			int i;
			if(pagos.Tables[0].Rows.Count!=0)
			{
				if(Session["tablaPagos"]==null)
					this.Preparar_TablaPagos();
				for(i=0;i<pagos.Tables[0].Rows.Count;i++)
				{
					filaPagos=tablaPagos.NewRow();
					filaPagos["TIPO"]=DBFunctions.SingleData("SELECT ttip_nombre FROM ttipopago WHERE ttip_codigo='"+pagos.Tables[0].Rows[i][0].ToString()+"'");
					filaPagos["CODIGOBANCO"]=pagos.Tables[0].Rows[i][1].ToString();
					filaPagos["NOMBREBANCO"]=DBFunctions.SingleData("SELECT pban_nombre FROM pbanco WHERE pban_codigo=(SELECT pban_codigo FROM mcajapago WHERE pban_codigo='"+pagos.Tables[0].Rows[i][1].ToString()+"' AND pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+")");
					filaPagos["NUMERODOC"]=pagos.Tables[0].Rows[i][2].ToString();
					filaPagos["TIPOMONEDA"]=pagos.Tables[0].Rows[i][3].ToString();
					filaPagos["VALOR"]=System.Convert.ToDouble(pagos.Tables[0].Rows[i][4].ToString()).ToString("C");
					filaPagos["VALORTC"]=System.Convert.ToDouble(pagos.Tables[0].Rows[i][5].ToString()).ToString("C");
					filaPagos["FECHA"]=pagos.Tables[0].Rows[i][6].ToString();
                    filaPagos["ESTADO"] = pagos.Tables[0].Rows[i][7].ToString();
                    tablaPagos.Rows.Add(filaPagos);
				}
				gridPagos.DataSource=tablaPagos;
				gridPagos.DataBind();
				Session["tablaPagos"]=tablaPagos;
			}
		}
		
		protected void Llenar_Grid_Retenciones()
		{
			DataRow filaRet;
			int i;
			if(retenciones.Tables[0].Rows.Count!=0)
			{
				if(Session["tablaRetenciones"]==null)
					this.Preparar_TablaRetenciones();
				for(i=0;i<retenciones.Tables[0].Rows.Count;i++)
				{
					filaRet=tablaRetenciones.NewRow();
					filaRet["NOMRET"]=retenciones.Tables[0].Rows[i][0].ToString();
					filaRet["VALOR"]=retenciones.Tables[0].Rows[i][1].ToString();
					tablaRetenciones.Rows.Add(filaRet);
				}
				gridRetenciones.DataSource=tablaRetenciones;
				gridRetenciones.DataBind();
				Session["tablaRetenciones"]=tablaRetenciones;
			}
		}

        protected void Llenar_Grid_Obligaciones()
        {
            DataRow fila;
            int i;
            if (obligaciones.Tables[0].Rows.Count != 0)
            {
                if (Session["tablaObligaciones"] == null)
                    this.Preparar_TablaObligciones();
                for (i = 0; i < obligaciones.Tables[0].Rows.Count; i++)
                {
                    fila = tablaObligaciones.NewRow();
                    fila["NUMEROOBLIGACION"] = obligaciones.Tables[0].Rows[i][0].ToString();
                    fila["SECUENCIA"] = obligaciones.Tables[0].Rows[i][1].ToString();
                    fila["MONTOPAGO"] = System.Convert.ToDouble(obligaciones.Tables[0].Rows[i][2].ToString()).ToString("C");
                    fila["VALORINTPAGO"] = System.Convert.ToDouble(obligaciones.Tables[0].Rows[i][3].ToString()).ToString("C");
                    tablaObligaciones.Rows.Add(fila);
                }
                gridObligaciones_financieras.DataSource = tablaObligaciones;
                gridObligaciones_financieras.DataBind();
                Session["tablaObligaciones"] = tablaObligaciones;
            }
        }

		protected void Preparar_TablaFacturas()
		{
			tablaFacturas=new DataTable();
			tablaFacturas.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
			tablaFacturas.Columns.Add(new DataColumn("NUMERODOCUMENTO", typeof(string)));
			tablaFacturas.Columns.Add(new DataColumn("VALOR", typeof(string)));
		}
		
		protected void Preparar_TablaPagos()
		{
			tablaPagos=new DataTable();
			tablaPagos.Columns.Add(new DataColumn("TIPO", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("CODIGOBANCO", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("NOMBREBANCO", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("NUMERODOC", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("TIPOMONEDA", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("VALOR", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("VALORTC", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("FECHA", typeof(string)));
            tablaPagos.Columns.Add(new DataColumn("ESTADO", typeof(string)));
		}
		
		protected void Preparar_TablaVarios()
		{
			tablaVarios=new DataTable();
			tablaVarios.Columns.Add(new DataColumn("DESCRIPCION", typeof(string)));
			tablaVarios.Columns.Add(new DataColumn("CUENTA", typeof(string)));
			tablaVarios.Columns.Add(new DataColumn("SEDE", typeof(string)));
			tablaVarios.Columns.Add(new DataColumn("CENTROCOSTO", typeof(string)));
			tablaVarios.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
			tablaVarios.Columns.Add(new DataColumn("NUMERO", typeof(int)));
			tablaVarios.Columns.Add(new DataColumn("NIT", typeof(string)));
			tablaVarios.Columns.Add(new DataColumn("VALOR", typeof(string)));
			tablaVarios.Columns.Add(new DataColumn("NATURALEZA", typeof(string)));
			tablaVarios.Columns.Add(new DataColumn("VALORBASE", typeof(string)));
		}
		
		protected void Preparar_TablaRetenciones()
		{
			tablaRetenciones=new DataTable();
			tablaRetenciones.Columns.Add(new DataColumn("NOMRET",typeof(string)));
			tablaRetenciones.Columns.Add(new DataColumn("VALOR",typeof(string)));
		}

        protected void Preparar_TablaObligciones()
		{
			tablaObligaciones=new DataTable();
            tablaObligaciones.Columns.Add(new DataColumn("NUMEROOBLIGACION", typeof(string)));
            tablaObligaciones.Columns.Add(new DataColumn("SECUENCIA", typeof(string)));
            tablaObligaciones.Columns.Add(new DataColumn("MONTOPAGO", typeof(string)));
            tablaObligaciones.Columns.Add(new DataColumn("VALORINTPAGO", typeof(string)));
		}

		protected void Escoger_Prefijo(object sender, EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"'").Trim()=="CE")
				bind.PutDatasIntoDropDownList(numeroRecibo,"SELECT DISTINCT MC.mcaj_numero FROM dbxschema.mcaja MC,dbxschema.mcajapago MCP WHERE MC.pdoc_codigo=MCP.pdoc_codigo AND MC.mcaj_numero=MCP.mcaj_numero AND MCP.test_estado NOT IN ('N') AND MC.pdoc_codigo='"+prefijoRecibo.SelectedValue+"' AND MC.test_estadodoc<>'N' ORDER BY MC.mcaj_numero");
				//Si es un recibo de caja, cargamos solo los del dia anterior y del dia actual
			else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"'").Trim()=="RC")
                     //bind.PutDatasIntoDropDownList(numeroRecibo, "SELECT mcaj_numero FROM mcaja WHERE pdoc_codigo='" + prefijoRecibo.SelectedValue + "' AND test_estadodoc<>'N' AND mcaj_fecha >='" + DateTime.Now.AddDays(-260).ToString("yyyy-MM-dd") + "' ORDER BY mcaj_numero");
                     bind.PutDatasIntoDropDownList(numeroRecibo, @"SELECT MCAJ_NUMERO FROM( SELECT DISTINCT MC.mcaj_numero, COALESCE(MAV.TEST_ESTADO, 0)  FROM mcaja MC
                     LEFT JOIN  MANTICIPOVEHICULO MAV  ON MC.pdoc_codigo = MAV.pdoc_codigo  AND MC.MCAJ_NUMERO = MAV.MCAJ_NUMERO AND MAV.TEST_ESTADO < 40 AND MAV.MANT_VALORECICAJA > 0
                     WHERE MC.pdoc_codigo = '" + prefijoRecibo.SelectedValue + "' AND   MC.test_estadodoc <> 'N' AND   MC.mcaj_fecha >='" + DateTime.Now.AddDays(-260).ToString("yyyy-MM-dd") + "' ORDER BY MC.mcaj_numero) AS A;");
				//Si es un provisional, cargamos todos los q hayan
			    else if (DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"'").Trim()=="RP")
				        bind.PutDatasIntoDropDownList(numeroRecibo,"SELECT mcaj_numero FROM mcaja WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+ "' AND test_estadodoc<>'N' ORDER BY mcaj_numero");
			if(numeroRecibo.Items.Count==0)
                cargarRecibo.Enabled=false;
			else 
				cargarRecibo.Enabled=true;
		}
		
		protected void Cargar_Recibo(object Sender, EventArgs e)
		{
            if (ddlDocAnu.SelectedValue.ToString() == "0")
            {
                string eliminar = "";
                if (Request.QueryString["eliminar"] != null)
                {
                    eliminar = "&elminar=1";
                }
                Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Tesoreria.AnulacionReciboCaja&errDocAnu=1" + eliminar);
            }

         	fecha.Text      = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mcaj_fecha FROM mcaja WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"")).ToString("yyyy-MM-dd");
			claseRecibo.Text= DBFunctions.SingleData("SELECT tcla_nombre FROM tclaserecaja WHERE tcla_claserecaja=(SELECT tcla_claserecaja FROM mcaja WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+")");
			nitCliente.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mcaja WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"");
			nitBeneficiario.Text=DBFunctions.SingleData("SELECT mnit_nitben FROM mcaja WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"");
			concepto.Text   = DBFunctions.SingleData("SELECT mcaj_razon FROM mcaja WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"");
            sede.Text       = DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM mcaja WHERE pdoc_codigo='" + this.prefijoRecibo.SelectedValue.ToString() + "' AND mcaj_numero=" + numeroRecibo.SelectedValue.ToString() + ")");
			if(claseRecibo.Text=="Anticipos (Repuestos/Taller)")
			{
				facturas=new DataSet();
				pagos=new DataSet();
				DBFunctions.Request(facturas,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,ddet_valodocu FROM ddetallefacturacliente WHERE pdoc_coddocref='"+this.prefijoRecibo.SelectedValue+"' AND ddet_numedocu="+this.numeroRecibo.SelectedValue+"");
				DBFunctions.Request(pagos,IncludeSchema.NO,"SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_fecha,test_estado FROM mcajapago WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"");
				tipoPago.Text="Notas a Favor del Cliente";
				this.Llenar_Grid_Facturas();
				this.Llenar_Grid_Pagos();
			}
			else if(claseRecibo.Text=="Act. Prorrogas Cheques")
			{
				pagos=new DataSet();
                DBFunctions.Request(pagos, IncludeSchema.NO, "SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_fecha,test_estado FROM mcajapago WHERE pdoc_codigo='" + this.prefijoRecibo.SelectedValue.ToString() + "' AND mcaj_numero=" + numeroRecibo.SelectedValue.ToString() + "");
				this.Llenar_Grid_Pagos();
			}
			else if((claseRecibo.Text=="Ingreso Definitivo")||(claseRecibo.Text=="Legalización de un Provisional"))
			{
				facturas = new DataSet();
				varios   = new DataSet();
				pagos    = new DataSet();
				retenciones=new DataSet();
				DBFunctions.Request(facturas,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,ddet_valodocu FROM ddetallefacturacliente WHERE pdoc_coddocref='"+this.prefijoRecibo.SelectedValue+"' AND ddet_numedocu="+this.numeroRecibo.SelectedValue+"");
				DBFunctions.Request(varios,IncludeSchema.NO,"SELECT dcaj_concepto,mcue_codipuc,palm_almacen,pcen_codigo,dcaj_prefdocu,dcaj_numedocu,mnit_nit,dcaj_valor,dcaj_naturaleza,dcaj_valobase FROM dcajavarios WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"");
                DBFunctions.Request(pagos, IncludeSchema.NO, "SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_fecha,test_estado FROM mcajapago WHERE pdoc_codigo='" + this.prefijoRecibo.SelectedValue.ToString() + "' AND mcaj_numero=" + numeroRecibo.SelectedValue.ToString() + "");
				DBFunctions.Request(retenciones,IncludeSchema.NO,"SELECT PRET.pret_codigo,MRET.mcaj_valor FROM mcajaretencion MRET,pretencion PRET WHERE (MRET.pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND MRET.mcaj_numero="+numeroRecibo.SelectedValue.ToString()+") AND MRET.pret_codigo=PRET.pret_codigo");
				tipoPago.Text="Facturas Abonadas";
				this.Llenar_Grid_Facturas();
				this.Llenar_Grid_Varios();
				this.Llenar_Grid_Pagos();
				this.Llenar_Grid_Retenciones();
			}
			else if(claseRecibo.Text=="Anticipo a Pedido de Vehiculos")
			{
				facturas=new DataSet();
				pagos=new DataSet();
				DBFunctions.Request(facturas,IncludeSchema.NO,"SELECT mped_codigo,mped_numepedi,mant_valorecicaja FROM manticipovehiculo MAV WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"");
                DBFunctions.Request(pagos, IncludeSchema.NO, "SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_fecha,test_estado FROM mcajapago WHERE pdoc_codigo='" + this.prefijoRecibo.SelectedValue.ToString() + "' AND mcaj_numero=" + numeroRecibo.SelectedValue.ToString() + "");
				tipoPago.Text="Anticipos a Vehículos";
				this.Llenar_Grid_Facturas();
				this.Llenar_Grid_Pagos();
			}
			else if(claseRecibo.Text=="Reconsignación Cheques Dev.")
			{
				facturas=new DataSet();
				pagos=new DataSet();
				DBFunctions.Request(facturas,IncludeSchema.NO,"SELECT dcaj_prefdocu,dcaj_numedocu,dcaj_valorecicaja FROM dcajacliente WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue+"' AND mcaj_numero="+this.numeroRecibo.SelectedValue+"");
                DBFunctions.Request(pagos, IncludeSchema.NO, "SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_fecha,test_estado FROM mcajapago WHERE pdoc_codigo='" + this.prefijoRecibo.SelectedValue.ToString() + "' AND mcaj_numero=" + numeroRecibo.SelectedValue.ToString() + "");
				this.Llenar_Grid_Facturas();
				this.Llenar_Grid_Pagos();
			}
			else if(claseRecibo.Text=="Egreso Definitivo")
			{
				facturas=new DataSet();
				varios=new DataSet();
				pagos=new DataSet();
				retenciones=new DataSet();
				DBFunctions.Request(facturas,IncludeSchema.NO,"SELECT pdoc_codiordepago,mfac_numeordepago,dcaj_valorecicaja FROM dcajaproveedor WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"");
				DBFunctions.Request(varios,IncludeSchema.NO,"SELECT dcaj_concepto,mcue_codipuc,palm_almacen,pcen_codigo,dcaj_prefdocu,dcaj_numedocu,mnit_nit,dcaj_valor,dcaj_naturaleza,dcaj_valobase FROM dcajavarios WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"");
                DBFunctions.Request(pagos, IncludeSchema.NO, "SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_fecha,test_estado FROM mcajapago WHERE pdoc_codigo='" + this.prefijoRecibo.SelectedValue.ToString() + "' AND mcaj_numero=" + numeroRecibo.SelectedValue.ToString() + "");
				DBFunctions.Request(retenciones,IncludeSchema.NO,"SELECT PRET.pret_codigo,MRET.mcaj_valor FROM mcajaretencion MRET,pretencion PRET WHERE (MRET.pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND MRET.mcaj_numero="+numeroRecibo.SelectedValue.ToString()+") AND MRET.pret_codigo=PRET.pret_codigo");
				tipoPago.Text="Facturas Abonadas";
				this.Llenar_Grid_Facturas();
				this.Llenar_Grid_Varios();
				this.Llenar_Grid_Pagos();
				this.Llenar_Grid_Retenciones();
			}
			else if(claseRecibo.Text=="Devolución Pedido Cliente")
			{
				facturas=new DataSet();
				pagos=new DataSet();
				DBFunctions.Request(facturas,IncludeSchema.NO,"SELECT mped_codigo,mped_numepedi,mant_valorecicaja FROM manticipovehiculo WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+" AND test_estado=40");
                DBFunctions.Request(pagos, IncludeSchema.NO, "SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_fecha,test_estado FROM mcajapago WHERE pdoc_codigo='" + this.prefijoRecibo.SelectedValue.ToString() + "' AND mcaj_numero=" + numeroRecibo.SelectedValue.ToString() + "");
				tipoPago.Text="Anticipos Devueltos";
				this.Llenar_Grid_Facturas();
				this.Llenar_Grid_Pagos();
			}
			else if(claseRecibo.Text=="Anticipo General")
			{
				facturas=new DataSet();
				pagos=new DataSet();
				DBFunctions.Request(facturas,IncludeSchema.NO,"SELECT pdoc_codiordepago,mfac_numeordepago,dcaj_valorecicaja FROM dcajaproveedor WHERE pdoc_codigo='"+this.prefijoRecibo.SelectedValue.ToString()+"' AND mcaj_numero="+numeroRecibo.SelectedValue.ToString()+"");
                DBFunctions.Request(pagos, IncludeSchema.NO, "SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_fecha,test_estado FROM mcajapago WHERE pdoc_codigo='" + this.prefijoRecibo.SelectedValue.ToString() + "' AND mcaj_numero=" + numeroRecibo.SelectedValue.ToString() + "");
				tipoPago.Text="Notas a Favor del Proveedor";
				this.Llenar_Grid_Facturas();
				this.Llenar_Grid_Pagos();
			}
            else if (claseRecibo.Text == "Obligaciones Financieras")
            {
                obligaciones = new DataSet();
                DBFunctions.Request(obligaciones, IncludeSchema.NO, "select mobl_numero, dobl_secuencia, dobl_montoblipago, dobl_valointepago  from DOBLIGACIONFINANCIERAPAGOOBLI where pdoc_codigo='" + this.prefijoRecibo.SelectedValue.ToString() + "' and dobl_numedocu=" + numeroRecibo.SelectedValue.ToString() + ";");
                tipoPago.Text = "Obligaciones Financieras";
                this.Llenar_Grid_Obligaciones();
            }

            tablaPagos = (DataTable)Session["tablaPagos"];
			Session.Clear();
			this.prefijoRecibo.Enabled=false;
			this.numeroRecibo.Enabled=false;
			this.gridPagos_Abonos.DataBind();
			this.gridVarios.DataBind();
			this.gridPagos.DataBind();
			this.gridRetenciones.DataBind();
			this.anular.Enabled=true;
            Session["tablaPagos"] = tablaPagos;
            cargarRecibo.Enabled = false;
            if (DBFunctions.SingleData("SELECT CCAR_ANULDOCUANT FROM CCARTERA") == "S")
                anular.Attributes.Add("onclick", "return confirm('Está anulando un documento de otra vigencia, ¿Desea continuar?');");
        }
		
		protected void Anular_Recibo(object sender, EventArgs e)
		{
            string permiteAnular = DBFunctions.SingleData("SELECT CCAR_ANULDOCUANT FROM CCARTERA");

            if(permiteAnular != "S")
                if (!Tools.General.validarCierreFinanzas(fecha.Text, "C"))
                {
                    Utils.MostrarAlerta(Response, "La fecha del documento no corresponde a la vigencia del sistema de cartera. Por favor revise.");
                    return;
                }
            Recibo anulado=new Recibo();
            DataSet miDS = new DataSet();
			anulado.PrefijoRecibo=prefijoRecibo.SelectedValue;
			anulado.NumeroRecibo=System.Convert.ToInt32(numeroRecibo.SelectedValue);
			anulado.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
			anulado.PrefijoAnulacion=ddlDocAnu.SelectedValue;
            tablaPagos = (DataTable)Session["tablaPagos"];

            bool procesoErrado = false;

            if (tablaPagos != null)
            {
                string codTipoPago = "";
                int i;
                for (i = 0; i < tablaPagos.Rows.Count; i++)
		        {
                    codTipoPago = DBFunctions.SingleData("SELECT TTIP_CODIGO FROM DBXSCHEMA.TTIPOPAGO WHERE TTIP_NOMBRE= '" + tablaPagos.Rows[i][0].ToString().Trim() + "'").ToUpper();
                    if (codTipoPago != "B" && codTipoPago != "DL" && codTipoPago != "DC" && codTipoPago != "C")
                    {
                        if (tablaPagos.Rows[i][8].ToString() != "C")
                        {
                            procesoErrado = true;
                        }
                    } 
                    /*
                        if (tablaPagos.Rows[i][0].ToString().Trim() != "Desctos Varios" && tablaPagos.Rows[i][0].ToString().Trim() != "Tf Bancaria" && tablaPagos.Rows[i][0].ToString().Trim() != "Desc. Ley" && tablaPagos.Rows[i][0].ToString().Trim() != "Desc. Comercial" && tablaPagos.Rows[i][0].ToString().Trim() != "Concepto Contable" && tablaPagos.Rows[i][0].ToString().Trim() != "Cheq Recibo de Clte")
                        {
                            if (tablaPagos.Rows[i][8].ToString() != "C" )
                            {
                                procesoErrado = true;
                            }
                        }
                    */
                }
            }

            if (procesoErrado)
            {
                anulado.Mensajes += "El documento de Pago NO esta en CAJA, Proceso No realizado";
                Utils.MostrarAlerta(Response, anulado.Mensajes);
                return;
            }

            int consAnu = Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + ddlDocAnu.SelectedValue + "'"));
            if (Request.QueryString["elminar"] != null)
            {
                anulado.OperacionEliminar = true;
            }
			if(anulado.Anular_Recibo(claseRecibo.Text))
			{
                string tipoRecibo = DBFunctions.SingleData("select tdoc_tipodocu from pdocumento where pdoc_codigo='" + prefijoRecibo.SelectedValue + "';");
                if (tipoRecibo == "RC" || tipoRecibo == "CE")
                {
                    //Se obtiene el usuario que creó la factura y la fecha de creacion para hacer el ajuste de los RC y CE en el saldo de caja.
                    string usuarioFecha = DBFunctions.SingleData("select mcaj_usuario CONCAT '@' CONCAT mcaj_fecha   from mcaja where pdoc_codigo='" + prefijoRecibo.SelectedValue + "' and mcaj_numero=" + numeroRecibo.SelectedValue);
                    Consignacion consignacion = new Consignacion(tablaPagos);
                    consignacion.RegistrarSaldoCaja(usuarioFecha, tipoRecibo, -2);
                }

				lb.Text=anulado.Mensajes;
                contaOnline.contabilizarOnline(prefijoRecibo.SelectedValue, Convert.ToInt32(numeroRecibo.SelectedValue), Convert.ToDateTime(fecha.Text), "");
                Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Tesoreria.AnulacionReciboCaja&ex=1&prefC=" + prefijoRecibo.SelectedValue + "&numC=" + numeroRecibo.SelectedValue + "&preAn=" + ddlDocAnu.SelectedValue + "&consAn=" + consAnu + "");
				Session.Clear();
			}
			else
				lb.Text=anulado.Mensajes;
		}

		////////////////////////////////////////////////
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

		protected void cancelar_Click(object sender, System.EventArgs e)
		{
			Server.Transfer(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Tesoreria.AnulacionReciboCaja");
		}
	}
}
