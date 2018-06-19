
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
using AMS.Tools;
using AMS.Contabilidad;
using AMS.Inventarios;

namespace AMS.Automotriz
{

	public partial class AMS_Automotriz_DevolucionesOT : System.Web.UI.UserControl
	{
		private DatasToControls bind = new DatasToControls();
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
     
		//JFSC 13022008 Cadena para manejo de cargo.
		//public String letracargo;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			btnDevolucion.Attributes.Add("onClick","if(confirm('Esta seguro que desea Realizar la Devolucion?')){document.getElementById('"+btnDevolucion.ClientID+"').disabled = true;"+this.Page.GetPostBackEventReference(btnDevolucion)+";}else{return (false);}");
			if(!IsPostBack)
			{				
				btnDevolucion.Enabled=false;

                Utils.FillDll(ddPrefOt, string.Format(Documento.DOCUMENTOSTIPO, "OT"), true);
                CambioTipoPref(null, null);
			
				if(Request.QueryString["prefD"]!=null && Request.QueryString["numD"]!=null)
                    Utils.MostrarAlerta(Response, "Se ha generado la nota de devolución de taller " + Request.QueryString["prefD"] + "-" + Request.QueryString["numD"] + "");
			}
		}

		protected void btnDevolucion_Click(object sender, System.EventArgs e)
		{
			int i;
			if(ddlOTS.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado ninguna orden de trabajo.\\n Revise Por Favor.");
				return;
			}
			if(ddlFactRel.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "La orden de trabajo " + ddlOTS.SelectedItem.Text + " no tiene ninguna factura relacionada.\\nRevise Por Favor.");
				return;
			}
            string prefOtSel = ddPrefOt.SelectedValue;
			string numOtSel  = ddlOTS.SelectedValue;
			string[] factRel = ddlFactRel.SelectedValue.Split('-');
			double valorFactura = 0;
			double valorIva = 0;
			double valorRetenciones = 0;
            string observacion = DBFunctions.SingleData("SELECT MFAC_OBSERVACION FROM MFACTURACLIENTE WHERE PDOC_CODIGO = '" + factRel[2].Trim() + "' AND MFAC_NUMEDOCU = '" + factRel[3].Trim() + "'");
            //NotaDevolucionCliente prueba = new NotaDevolucionCliente() ;
            //prueba.ObservacionDevolucion = txtObserva.Text;
            if (txtObserva.Text == "" || txtObserva.Text == null)
            {
                txtObserva.Text = " Sin Observación. ";
            }
            NotaDevolucionCliente.observacionDevolucion = " || Razón Devolucion: " + txtObserva.Text + "";

			try{valorFactura = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM mfacturacliente WHERE pdoc_codigo='"+factRel[2].Trim()+"' AND mfac_numedocu="+factRel[3].Trim()+""));}
			catch{}
			try{valorIva = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoiva FROM mfacturacliente WHERE pdoc_codigo='"+factRel[2].Trim()+"' AND mfac_numedocu="+factRel[3].Trim()+""));}
			catch{}
			try{valorRetenciones = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturacliente WHERE pdoc_codigo='"+factRel[2].Trim()+"' AND mfac_numedocu="+factRel[3].Trim()+""));}
			catch{}
            DataSet dsRetenciones = new DataSet();
            DataTable dtRetenciones = null;
            if (valorRetenciones != 0)
            {
                dsRetenciones = DBFunctions.Request(dsRetenciones, IncludeSchema.NO, "SELECT * FROM mfacturaclienteRETENCION WHERE pdoc_codigo='" + factRel[2].Trim() + "' AND mfac_numedocu=" + factRel[3].Trim() + "");
                dtRetenciones = dsRetenciones.Tables[0];
            }
            
			NotaDevolucionCliente notaDevFT = new NotaDevolucionCliente(ddlPrefNDC.SelectedValue,factRel[2].Trim(),Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefNDC.SelectedValue+"'")),Convert.ToUInt32(factRel[3].Trim()),
                "N", "FA", valorFactura, valorIva, valorRetenciones,DateTime.Now, HttpContext.Current.User.Identity.Name, dtRetenciones);
            
			ArrayList sqlRefs = new ArrayList();
			//Eliminamos la relacion entre la orden de trabajo y la factura que se esta eliminando
            sqlRefs.Add("DELETE FROM mfacturaclientetaller WHERE pdoc_codigo='" + factRel[2].Trim() + "' AND mfac_numedocu=" + factRel[3].Trim() + " AND pdoc_prefordetrab='" + prefOtSel + "' AND mord_numeorde=" + numOtSel + "");
			//Debemos cambiar el estado de las transferencias que estan relacionadas con este cargo (mordentransferencia ,mpedidotransferencia y mfacturacliente)
            sqlRefs.Add("UPDATE mfacturacliente SET tvig_vigencia='C' WHERE (pdoc_codigo,mfac_numedocu) IN (SELECT pdoc_factura,mfac_numero FROM mordentransferencia WHERE pdoc_codigo='" + prefOtSel + "' AND mord_numeorde=" + numOtSel + " AND tcar_cargo='" + factRel[0].Trim() + "')");
            sqlRefs.Add("UPDATE mpedidotransferencia SET tvig_vigencia='C' WHERE pdoc_codigo='" + prefOtSel + "' AND mord_numeorde=" + numOtSel + " AND tcar_cargo='" + factRel[0].Trim() + "'");
			//Vuelvo a dejar la orden es estao abierto
            sqlRefs.Add("UPDATE morden SET test_estado='A',mord_estaliqu='P' WHERE pdoc_codigo='" + prefOtSel + "' AND mord_numeorde=" + numOtSel + "");
			//Se agrega el comentario del por qué la devolución.
            sqlRefs.Add("UPDATE MFACTURACLIENTE SET MFAC_OBSERVACION = '" + observacion + " " + NotaDevolucionCliente.observacionDevolucion + "' WHERE PDOC_CODIGO = '" + factRel[2].Trim() + "' AND MFAC_NUMEDOCU = " + factRel[3].Trim());
            //Ahora agregamos los sqlrels
			for(i=0;i<sqlRefs.Count;i++)
				notaDevFT.SqlRels.Add(sqlRefs[i].ToString());
			//Ahora agregamos los sql relacionados con el almacenamiento historico de la ot
            ArrayList sqlHistorico = Orden.AlmacenarHistorialDevolucionOrden(prefOtSel, numOtSel, factRel[2].Trim(), factRel[3].Trim(), DateTime.Now.ToString("yyyy-MM-dd"), HttpContext.Current.User.Identity.Name.ToLower());
			for(i=0;i<sqlHistorico.Count;i++)
				notaDevFT.SqlRels.Add(sqlHistorico[i].ToString());
            ///*notaDevFT.GrabarNotaDevolucionCliente(false);
            //for(i=0;i<notaDevFT.SqlStrings.Count;i++)
            //    lb.Text += "<br>"+notaDevFT.SqlStrings[i].ToString();*/
			if(notaDevFT.GrabarNotaDevolucionCliente(true))
			{
				Orden.AlmacenarCostosDevolucionOT(notaDevFT.PrefijoFactura,notaDevFT.NumeroFactura,notaDevFT.PrefijoNota,notaDevFT.NumeroNota);
                RevisionDevolucionesOT(prefOtSel, Convert.ToUInt32(numOtSel));
                ProceHecEco contaOnline = new ProceHecEco();
                contaOnline.contabilizarOnline(notaDevFT.PrefijoNota, Convert.ToInt32(notaDevFT.NumeroNota.ToString()), DateTime.Now, "");
                Response.Redirect(indexPage + "?process=Automotriz.DevolucionesOT&prefD=" + notaDevFT.PrefijoNota + "&numD=" + notaDevFT.NumeroNota + "");
			}
			else
				lb.Text += "<br>Mal "+notaDevFT.ProcessMsg;
		}
        protected void CambioTipoPref(Object Sender, EventArgs e)
        {
            // AQUI se DEBE PONER por PARAMETRO el numero de meses máximo permitido para devolver FACTURAS DE TALLER, porque cada usuario maneja eso diferente
            string sql = "SELECT DISTINCT MFCT.mord_numeorde FROM mfacturaclientetaller MFCT WHERE MFCT.pdoc_prefordetrab = '" + ddPrefOt.SelectedValue + "' AND MFAC_FECHCREA > (CURRENT DATE - 36 month)";
            Utils.FillDll(ddlOTS, sql, true);
            ddlOTS.Items.Insert(0, new ListItem("Seleccione...", "0"));

            CargarFacturas();	
        }

        protected void CambioOTLiqu(Object Sender, EventArgs e)
        {
            CargarFacturas();
            CambioFacLiqu(Sender, e);
        }
        protected void CambioFacLiqu(Object  Sender, EventArgs e)
		{
			String letracargo;
			string numeroFactura;
			string codigoFactura;
			lbCargo.Text = (ddlFactRel.SelectedValue.Split('-'))[1].Trim();
			letracargo   = (ddlFactRel.SelectedValue.Split('-'))[0].Trim();
			numeroFactura= (ddlFactRel.SelectedValue.Split('-'))[3].Trim();
			codigoFactura= (ddlFactRel.SelectedValue.Split('-'))[2].Trim();
			//JFSC 13022008 Modificación de consulta en pdocumento
			//Consulta anterior: "SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='NC'"
			String consulta= String.Empty;
			string actividad=string.Empty;
			switch (letracargo)
			{
				case "S":
					actividad = "TC";
					break;
				case "C":
					actividad = "TC";
					break;
				case "G":
					actividad = "TG";
					break;
				case "I":
					actividad = "TI";
					break;
				case "A":
					actividad = "TA";
					break;
				case "T":
					actividad = "TT";
					break;
				default:
					break;
			}
			//consulta="SELECT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'NC' and PH.tpro_proceso = '"+actividad+"' AND P.PDOC_CODIGO = PH.PDOC_CODIGO";
			//bind.PutDatasIntoDropDownList(ddlPrefNDC,consulta);
			//if(ddlPrefNDC.Items.Count == 0)
			//{
			//	Response.Write("<script language:javascript>alert('No ha parametrizado ningún documento del tipo NC para la actividad "+actividad+"');</script>");
			//	btnDevolucion.Enabled=false;
			//}
			//else
			//{
			//	btnDevolucion.Enabled=true;
			//}
			string almacen =DBFunctions.SingleData("select palm_almacen from dbxschema.mfacturacliente where mfac_numedocu="+numeroFactura+" and pdoc_codigo='"+codigoFactura+"'");
			General.cargarDocumentos(ddlPrefNDC,btnDevolucion,"NC",actividad,almacen);
		}

        protected void CargarFacturas ()//(DropDownList ddPrefOt, DropDownList ddlOT, DropDownList ddlFact)
		{
            // ddPrefOt,ddlOTS,ddlFactRel
            if (ddPrefOt.SelectedValue == null || ddPrefOt.SelectedValue == "0" ||
                ddlOTS.SelectedValue == null || ddlOTS.SelectedValue == "0" ||
                ddlFactRel.SelectedValue == null || ddlFactRel.SelectedValue == "0")
                return;

            ddlFactRel.Items.Clear();
			//string[] ot = ddlOT.SelectedValue.Trim().Split('-');
            //DatasToControls bind = new DatasToControls();
            //bind.PutDatasIntoDropDownList(ddlFact, "SELECT TCAR.tcar_cargo CONCAT '-' CONCAT TCAR.tcar_nombre CONCAT '-' CONCAT MFCT.pdoc_codigo CONCAT '-' CONCAT CAST(MFCT.mfac_numedocu AS CHARACTER(6)),PDO.pdoc_nombre CONCAT '-' CONCAT CAST(MFCT.mfac_numedocu AS CHARACTER(6)) FROM dbxschema.mfacturaclientetaller MFCT, pdocumento PDO, tcargoorden TCAR WHERE PDO.pdoc_codigo=MFCT.pdoc_codigo AND TCAR.tcar_cargo=MFCT.tcar_cargo AND MFCT.pdoc_prefordetrab='" + ot[0].Trim() + "' AND mord_numeorde=" + ot[1].Trim() + "");
            Utils.FillDll(ddlFactRel, "SELECT TCAR.tcar_cargo CONCAT '-' CONCAT TCAR.tcar_nombre CONCAT '-' CONCAT MFCT.pdoc_codigo CONCAT '-' CONCAT CAST(MFCT.mfac_numedocu AS CHARACTER(6)),PDO.pdoc_nombre CONCAT '-' CONCAT CAST(MFCT.mfac_numedocu AS CHARACTER(6)) FROM dbxschema.mfacturaclientetaller MFCT, pdocumento PDO, tcargoorden TCAR WHERE PDO.pdoc_codigo=MFCT.pdoc_codigo AND TCAR.tcar_cargo=MFCT.tcar_cargo AND MFCT.pdoc_prefordetrab='" + ddPrefOt.SelectedValue + "' AND mord_numeorde=" + ddlOTS.SelectedValue + "", false);

		}

		protected void RevisionDevolucionesOT(string prefOT, uint numOT)
		{
			//Revisamos si existe algun registro en la tabla mfacturaclientetaller 
			int cantidadRegistros = Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM mfacturaclientetaller WHERE pdoc_prefordetrab='"+prefOT+"' AND mord_numeorde="+numOT+""));
			if(cantidadRegistros == 0)
				if(DBFunctions.NonQuery("UPDATE morden SET test_estado='A' WHERE pdoc_codigo='"+prefOT+"' AND mord_numeorde="+numOT+"") < 1)
					lb.Text += "<br>Error en actualizacion de estado OT "+DBFunctions.exceptions;
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

