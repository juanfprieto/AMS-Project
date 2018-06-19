// created on 09/11/2004 at 11:17
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
using AMS.DBManager;
using AMS.Forms;
using AMS.Documentos;
using Ajax;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class LiquidacionOrden : System.Web.UI.UserControl
	{
		private FormatosDocumentos formatoRecibo=new FormatosDocumentos();
		
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string tipoProceso;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(LiquidacionOrden));
            TorreControl.Visible = LiquidacionOT.Visible = false;
            tipoProceso = Request.QueryString["actor"]; //Debe definir el tipo de proceso: Torre de Control / Liquidacion OT
            if (tipoProceso == "T")
                TorreControl.Visible = true;
            else
            {
                if (tipoProceso == "F")
                    LiquidacionOT.Visible = true;
                else
                    TorreControl.Visible = LiquidacionOT.Visible = true;
            }
        //    Session["tipoProceso"] = tipoProceso.ToString();
			
			//botones q hace la confirmacion de los datos y cancela el boton para q no se produzca. doble facturacion o etc
			if(!IsPostBack)
			{
                double factorDeducible = 0;
                //try
                //{
                //    factorDeducible = Convert.ToDouble(DBFunctions.SingleData("SELECT MFAC_FACTORDEDUCIBLE FROM MFACTURACLIENTETALLER FETCH FIRST 1 ROWS ONLY;"));
                //}
                //catch
                //{
                //    Utils.MostrarAlerta(Response, "NO se ha creado el campo MFAC_FACTORDEDUCIBLE en MF...TALLER, llamar a eCAS antes de continuar !!! ");
                //    return;
                //}

				DatasToControls bind = new DatasToControls();
                //bind.PutDatasIntoDropDownList(tipoDocumento1, "SELECT DISTINCT PD.pdoc_codigo, pdoc_nombre FROM pdocumento PD, MORDEN MO WHERE tdoc_tipodocu='OT' and MO.PDOC_CODIGO = PD.PDOC_CODIGO and mo.test_estado='A' ");
                Utils.llenarPrefijos(Response, ref tipoDocumento1, "%", "%", "OT");
                if (tipoDocumento1.Items.Count > 1)
                    tipoDocumento1.Items.Insert(0, "Seleccione:..");
                //bind.PutDatasIntoDropDownList(tipoDocumento2, "SELECT DISTINCT PD.pdoc_codigo, pdoc_nombre FROM pdocumento PD, MORDEN MO WHERE tdoc_tipodocu='OT' and MO.PDOC_CODIGO = PD.PDOC_CODIGO and mo.test_estado='A' ");
                Utils.llenarPrefijos(Response, ref tipoDocumento2 , "%", "%", "OT");
                if (tipoDocumento2.Items.Count > 1)
                    tipoDocumento2.Items.Insert(0, "Seleccione:..");
                bind.PutDatasIntoDropDownList(ordenesPreliquidar, "SELECT mord_numeorde FROM morden WHERE pdoc_codigo = '" + tipoDocumento1.SelectedValue + "'  and mord_estaliqu in ('A','P') and test_estado='A' ORDER BY mord_numeorde");
                int ano = DateTime.Now.Year;
                int mes = DateTime.Now.Month;
                bind.PutDatasIntoDropDownList(ddlOTS, "SELECT DISTINCT MFCT.pdoc_prefordetrab CONCAT '-' CONCAT CAST(MFCT.mord_numeorde AS CHARACTER(6)), PDO.pdoc_nombre CONCAT '-' CONCAT CAST(MFCT.mord_numeorde AS CHARACTER(6)) FROM mfacturaclientetaller MFCT, pdocumento PDO WHERE PDO.pdoc_codigo=MFCT.pdoc_prefordetrab AND MFAC_FECHCREA > (CURRENT DATE - 12 MONTHS); ");
                if (ddlOTS.Items.Count > 1)
                    ddlOTS.Items.Insert(0, "Seleccione:.."); 
                else
                    CargarFacturas(ddlOTS, ddlFactRel); 
				ddlFactRel.Attributes.Add("onchange","CambioFact("+ddlFactRel.ClientID+","+lbCargo.ClientID+");");
				if(Request.QueryString["prefD"]!=null && Request.QueryString["numD"]!=null)
				{
                    Utils.MostrarAlerta(Response, "Se ha generado la nota de devolución de taller " + Request.QueryString["prefD"] + "-" + Request.QueryString["numD"] + "");
					try
					{
						formatoRecibo.Prefijo=Request.QueryString["prefD"];
						formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["numD"]);
						formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefD"]+"'");
						if(formatoRecibo.Codigo!=string.Empty)
						{
							if(formatoRecibo.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
						}
					}
					catch
					{
						lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
					}
				}
				if(Request.QueryString["factOT"] != null)
				{
                    string msg = "Se ha creado la factura con prefijo " + Request.QueryString["prefF1"] + " y numero " + Request.QueryString["numF1"] + " por el cargo " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + Request.QueryString["car1"] + "'").Trim() + ".\\nSe ha creado la factura con prefijo " + Request.QueryString["prefF2"] + " y numero " + Request.QueryString["numF2"] + " por el cargo " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + Request.QueryString["car2"] + "'").Trim() + ");";
                    if(Request.QueryString["factOT"] == "0")
                        Utils.MostrarAlerta(Response, "Se ha creado la factura con prefijo "+Request.QueryString["prefF"]+" y numero "+Request.QueryString["numF"]+" por el cargo "+DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='"+Request.QueryString["car"]+"'").Trim()+".");
					else if(Request.QueryString["factOT"] == "1")
                            Utils.MostrarAlerta(Response, msg );
				}
            }
			if(ddlFactRel.Items.Count > 0)
				lbCargo.Text = (ddlFactRel.SelectedValue.Split('-'))[1].Trim();
		}
		
		protected void Ingresar_Torre(Object  Sender, EventArgs e)
		{
            if (tipoProceso == "T")
			    Response.Redirect("" + indexPage + "?process=Automotriz.TorreControl&actor=T ");
            else
                Response.Redirect("" + indexPage + "?process=Automotriz.TorreControl ");
        }
		
		protected void Ingresar_Programacion_Taller(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Automotriz.ProgramacionTaller");
		}
		
		protected void Preliquidar_Orden(Object  Sender, EventArgs e)
		{
			if(ordenesPreliquidar.Items.Count>0)
				Response.Redirect("" + indexPage + "?process=Automotriz.PreliquidacionOT&pref="+tipoDocumento1.SelectedValue+"&num="+ordenesPreliquidar.SelectedValue+"");
			else
                Utils.MostrarAlerta(Response, "No Se Ha Seleccionado Ninguna Orden De Trabajo");
		}
		
		protected void Liquidar_Orden(Object  Sender, EventArgs e)
		{
			if(ordenesLiquidar.Items.Count>0)
				Response.Redirect("" + indexPage + "?process=Automotriz.ProcesosLiquidacion&pref="+tipoDocumento2.SelectedValue+"&num="+ordenesLiquidar.SelectedValue+"");
			else
                Utils.MostrarAlerta(Response, "No Se Ha Seleccionado Ninguna Orden De Trabajo");
		}

        [Ajax.AjaxMethod()]
        protected void Cambio_Documento1(Object  Sender, EventArgs e)
		{
			Llenar_Ordenes_Liquidar(ordenesPreliquidar,tipoDocumento1);
		}

        [Ajax.AjaxMethod()]
        protected void Cambio_Documento2(Object  Sender, EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
            bind.PutDatasIntoDropDownList(ordenesLiquidar, "SELECT mord_numeorde FROM dbxschema.morden WHERE pdoc_codigo='" + tipoDocumento2.SelectedValue + "' AND mord_estaliqu='P' and test_estado='A' ORDER BY mord_numeorde");
		}

        [Ajax.AjaxMethod()]
        protected void CambioOTLiqu(Object  Sender, EventArgs e)
		{
			CargarFacturas(ddlOTS,ddlFactRel);
			lbCargo.Text = (ddlFactRel.SelectedValue.Split('-'))[1].Trim();
		}
		
		/*protected void DevolverFactura(Object Sender, EventArgs e)
		{
			int i;
			if(ddlOTS.Items.Count == 0)
			{
				Response.Write("<script language:javascript>alert('No se ha seleccionado ninguna orden de trabajo.\\nRevise Por Favor.');</script>");
				return;
			}
			if(ddlFactRel.Items.Count == 0)
			{
				Response.Write("<script language:javascript>alert('La orden de trabajo "+ddlOTS.SelectedItem.Text+" no tiene ninguna factura relacionada.\\nRevise Por Favor.');</script>");
				return;
			}
			string[] otSel = ddlOTS.SelectedValue.Split('-');
			string[] factRel = ddlFactRel.SelectedValue.Split('-');
			NotaDevolucionCliente notaDevFT = new NotaDevolucionCliente();
			if(Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM mfacturacliente WHERE pdoc_codigo='"+factRel[2].Trim()+"' AND mfac_numedocu="+factRel[3].Trim()+"")) == 0)
			{
				notaDevFT = new NotaDevolucionCliente(ddlPrefNDC.SelectedValue,factRel[2].Trim(),Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefNDC.SelectedValue+"'")),Convert.ToUInt32(factRel[3].Trim()),
			                                          "N","FC",0,"C","C",DateTime.Now,HttpContext.Current.User.Identity.Name.ToLower(),false);
			}
			else
			{
				notaDevFT = new NotaDevolucionCliente(ddlPrefNDC.SelectedValue,factRel[2].Trim(),Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefNDC.SelectedValue+"'")),Convert.ToUInt32(factRel[3].Trim()),
			                                          "N","FC",0,"V","C",DateTime.Now,HttpContext.Current.User.Identity.Name.ToLower(),false);
			}
			ArrayList sqlRefs = new ArrayList();
			//Eliminamos la relacion entre la orden de trabajo y la factura que se esta eliminando
			sqlRefs.Add("DELETE FROM mfacturaclientetaller WHERE pdoc_codigo='"+factRel[2].Trim()+"' AND mfac_numedocu="+factRel[3].Trim()+" AND pdoc_prefordetrab='"+otSel[0].Trim()+"' AND mord_numeorde="+otSel[1].Trim()+"");
			//Debemos cambiar el estado de las transferencias que estan relacionadas con este cargo (mordentransferencia ,mpedidotransferencia y mfacturacliente)
			sqlRefs.Add("UPDATE mfacturacliente SET tvig_vigencia='C' WHERE (pdoc_codigo,mfac_numedocu) IN (SELECT pdoc_factura,mfac_numero FROM mordentransferencia WHERE pdoc_codigo='"+otSel[0].Trim()+"' AND mord_numeorde="+otSel[1].Trim()+" AND tcar_cargo='"+factRel[0].Trim()+"')");
			sqlRefs.Add("UPDATE mpedidotransferencia SET tvig_vigencia='C' WHERE pdoc_codigo='"+otSel[0].Trim()+"' AND mord_numeorde="+otSel[1].Trim()+" AND tcar_cargo='"+factRel[0].Trim()+"'");
			//Vuelvo a dejar la orden es estao abierto
			sqlRefs.Add("UPDATE morden SET test_estado='A',mord_estaliqu='P' WHERE pdoc_codigo='"+otSel[0].Trim()+"' AND mord_numeorde="+otSel[1].Trim()+"");
			//Ahora agregamos los sqlrels
			for(i=0;i<sqlRefs.Count;i++)
				notaDevFT.SqlRels.Add(sqlRefs[i].ToString());
			//Ahora agregamos los sql relacionados con el almacenamiento historico de la ot
			ArrayList sqlHistorico = Orden.AlmacenarHistorialDevolucionOrden(otSel[0].Trim(),otSel[1].Trim(),factRel[2].Trim(),factRel[3].Trim(),DateTime.Now.ToString("yyyy-MM-dd"),HttpContext.Current.User.Identity.Name.ToLower());
			for(i=0;i<sqlHistorico.Count;i++)
				notaDevFT.SqlRels.Add(sqlHistorico[i].ToString());
			/*notaDevFT.GrabarNotaDevolucionCliente(false);
			for(i=0;i<notaDevFT.SqlStrings.Count;i++)
				lb.Text += "<br>"+notaDevFT.SqlStrings[i].ToString();
			if(notaDevFT.GrabarNotaDevolucionCliente(true))
			{
				lb.Text += "<br>Bien "+notaDevFT.ProcessMsg;
				RevisionDevolucionesOT(otSel[0].Trim(),Convert.ToUInt32(otSel[1].Trim()));
				Response.Redirect(indexPage+"?process=Automotriz.LiquidaOrden&prefD="+notaDevFT.PrefijoNota+"&numD="+notaDevFT.NumeroNota+"");
			}
			else
				lb.Text += "<br>Mal "+notaDevFT.ProcessMsg;
		}*/
		
		protected void ReimpresionFactura(Object Sender, EventArgs e)
		{
			if(ddlOTS.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado ninguna orden de trabajo.\\nRevise Por Favor.");
				return;
			}
			if(ddlFactRel.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "La orden de trabajo " + ddlOTS.SelectedItem.Text + " no tiene ninguna factura relacionada.\\nRevise Por Favor.");
				return;
			}
			string[] factRel = ddlFactRel.SelectedValue.Split('-');
			Response.Redirect("" + indexPage + "?process=Automotriz.VistaImpresion&tipVis=liquidar&prefOT="+factRel[2]+"&numOT="+factRel[3]+"&cargo="+factRel[0]+"&dest=0");
		}

        [Ajax.AjaxMethod()]
		protected void CargarFacturas(DropDownList ddlOT, DropDownList ddlFact)
		{
			int i;
			if(ddlOT.Items.Count > 0)
			{
				ddlFact.Items.Clear();
				string[] ot = ddlOT.SelectedValue.Trim().Split('-');
				DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT TCAR.tcar_cargo CONCAT '-' CONCAT TCAR.tcar_nombre CONCAT '-' CONCAT MFCT.pdoc_codigo CONCAT '-' CONCAT CAST(MFCT.mfac_numedocu AS CHARACTER(6)),PDO.pdoc_nombre CONCAT '-' CONCAT CAST(MFCT.mfac_numedocu AS CHARACTER(6)) FROM dbxschema.mfacturaclientetaller MFCT, pdocumento PDO, tcargoorden TCAR WHERE PDO.pdoc_codigo=MFCT.pdoc_codigo AND TCAR.tcar_cargo=MFCT.tcar_cargo AND MFCT.pdoc_prefordetrab='"+ot[0].Trim()+"' AND mord_numeorde="+ot[1].Trim()+"");
				for(i=0;i<ds.Tables[0].Rows.Count;i++)
					ddlFact.Items.Add(new ListItem(ds.Tables[0].Rows[i][1].ToString(),ds.Tables[0].Rows[i][0].ToString()));
			}
		}

        [Ajax.AjaxMethod()]
        protected void Llenar_Ordenes_Liquidar(DropDownList objetivo, DropDownList fuente)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(objetivo,"SELECT mord_numeorde FROM dbxschema.morden WHERE pdoc_codigo='"+fuente.SelectedValue+"' AND mord_estaliqu IN ('A','P') and test_estado='A' ORDER BY mord_numeorde");
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
	}
}
