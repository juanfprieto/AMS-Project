// created on 24/01/2005 at 11:21

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
using AMS.Tools;

namespace AMS.Automotriz
{

	public partial class ConsultarHoja : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataSet lineas;
		protected DataSet lineas2;
		protected DataTable resultado;
		protected System.Web.UI.WebControls.Label repuestos;
		string vinFinal=null;
        string Habeasdata = "";
        DataSet operacionesRealizadas = new DataSet();
        #endregion

        #region Eventos
        protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Session["HREFRETURN"] != null)
                ViewState["HREFRETURN"]=Session["HREFRETURN"];
            Session.Remove("Rep");
			string placaVehiculo = Request.QueryString["placa"];
            string vinVehiculo = Request.QueryString["vin"];
            Habeasdata = Request.QueryString["habeas"];
			this.Distribuir_Datos_Cliente(placaVehiculo, vinVehiculo);
			this.Distribuir_Datos_Vehiculo(placaVehiculo);
			this.Distribuir_Grilla_Operaciones(placaVehiculo);
			this.generar(sender,e);
			Session["Rep"] = this.Cuerpo_Correo();
		}
		
		protected void Volver(Object  Sender, EventArgs e)
		{
            string indexPage = ConfigurationManager.AppSettings["MainAjaxPage"];
            if (ViewState["HREFRETURN"] != null)
            {
                string retRef = ViewState["HREFRETURN"].ToString();
                Response.Redirect(indexPage + "?" + retRef);
            }
            else
			    Response.Redirect("" + indexPage + "?process=Automotriz.HojaVida");
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			MailMessage MyMail = new MailMessage();
   		  	MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
     		MyMail.To = tbEmail.Text;
			MyMail.Subject = "Hoja de Vida de Automovil";
			MyMail.Body = ((string)Session["Rep"]);
      		MyMail.BodyFormat = MailFormat.Html;
			try{
    	   		SmtpMail.Send(MyMail);}
    		catch(Exception e){
    	 	  lb.Text = e.ToString();
    		}
		}

        
        protected void generar(Object  Sender, EventArgs e)
		{			
			string prefijoNumeroOrden = String.Empty;
			double valorSumatoria = 0;
			PrepararTabla();
			lineas = new DataSet();
	        /*
            //	lineas = DBFunctions.Request(lineas,IncludeSchema.NO,"SELECT codigo_orden,numero_orden,codigo_factura,numero_factura,codigo_editado,nombre_item,cantidad_original-COALESCE(cantidad_devuelta,0),valor_unitario FROM vtaller_consrephv WHERE vin_relacionado = '"+vinFinal+"' AND (cantidad_original-COALESCE(cantidad_devuelta,0)) > 0 ORDER BY codigo_orden,numero_orden ASC");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				if(prefijoNumeroOrden != String.Empty && prefijoNumeroOrden != lineas.Tables[0].Rows[i][0]+"-"+lineas.Tables[0].Rows[i][1])
				{
					DataRow drTotal = resultado.NewRow();
					drTotal["REPUESTO"] = "------";
					drTotal["CODREPUESTO"] = "------";
					drTotal["ORDENOP"] = "------";
					drTotal["CANTIDAD"] = "------";
					drTotal["OTRELAC"] = "<span style='color:#FF0000'>TOTAL "+prefijoNumeroOrden+"</span>";
					drTotal["VALORU"] = valorSumatoria.ToString("C");
					resultado.Rows.Add(drTotal);
					valorSumatoria = 0;
				}
				DataRow fila = resultado.NewRow();
				fila["REPUESTO"] = lineas.Tables[0].Rows[i][5].ToString();
				fila["CODREPUESTO"] = lineas.Tables[0].Rows[i][4].ToString();
				fila["ORDENOP"] = lineas.Tables[0].Rows[i][2]+"-"+lineas.Tables[0].Rows[i][3];
				fila["VALORU"] = Convert.ToDouble(lineas.Tables[0].Rows[i][7]).ToString("C");
				fila["CANTIDAD"] = lineas.Tables[0].Rows[i][6].ToString();
				fila["OTRELAC"] = prefijoNumeroOrden = lineas.Tables[0].Rows[i][0]+"-"+lineas.Tables[0].Rows[i][1];
				valorSumatoria += Convert.ToDouble(lineas.Tables[0].Rows[i][6])*Convert.ToDouble(lineas.Tables[0].Rows[i][7]);
				resultado.Rows.Add(fila);
			}
            */
			if(prefijoNumeroOrden != String.Empty)
			{
				DataRow drTotal = resultado.NewRow();
				drTotal["REPUESTO"] = "------";
				drTotal["CODREPUESTO"] = "------";
				drTotal["ORDENOP"] = "------";
				drTotal["CANTIDAD"] = "------";
				drTotal["OTRELAC"] = "<span style='color:#FF0000'>TOTAL "+prefijoNumeroOrden+"</span>";
				drTotal["VALORU"] = valorSumatoria.ToString("C");
				resultado.Rows.Add(drTotal);
				valorSumatoria = 0;
			}
			Grid.DataSource = resultado;
			Grid.DataBind();
		}
		#endregion
		
		#region Metodos
		protected void Distribuir_Datos_Cliente(string placaVehiculo, string vin)
		{
            if (Habeasdata != "1")
            { 
                ((Panel)FindControl("fsCliente")).Visible = true;
          
                string nitCliente = DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_placa='" + placaVehiculo + "' AND MCAT_VIN = '" + vin + "'");
                nombres.Text = DBFunctions.SingleData("SELECT mnit_nombres concat ' ' concat coalesce(mnit_nombre2,'') FROM mnit WHERE mnit_nit='" + nitCliente + "'");
                apellidos.Text = DBFunctions.SingleData("SELECT mnit_apellidos concat ' ' concat coalesce(mnit_apellido2,'') FROM mnit WHERE mnit_nit='" + nitCliente + "'");
                nit.Text = nitCliente;
                ciudadExpedicion.Text = DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigoexpnit FROM mnit WHERE mnit_nit='" + nitCliente + "')");
                tipoNacionalidad.Text = DBFunctions.SingleData("SELECT tnac_nombre FROM tnacionalidad WHERE tnac_tiponaci=(SELECT tnac_tiponaci FROM mnit WHERE mnit_nit='" + nitCliente + "')");
                direccion.Text = DBFunctions.SingleData("SELECT mnit_direccion FROM mnit WHERE mnit_nit='" + nitCliente + "'");
                ciudad.Text = DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM mnit WHERE mnit_nit='" + nitCliente + "')");
                telefono.Text = DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit='" + nitCliente + "'");
                celular.Text = DBFunctions.SingleData("SELECT mnit_celular FROM mnit WHERE mnit_nit='" + nitCliente + "'");
                email.Text = DBFunctions.SingleData("SELECT mnit_email FROM mnit WHERE mnit_nit='" + nitCliente + "'");
                website.Text = DBFunctions.SingleData("SELECT mnit_web FROM mnit WHERE mnit_nit='" + nitCliente + "'");
                try { hbData.Text = DBFunctions.SingleData("SELECT T.TRES_NOMBRE FROM MBASEDATOS M, TRESPUESTASINO T WHERE  MNIT_NIT = '" + nitCliente + "' AND T.TRES_SINO = M.TRES_SINO AND TBAS_CODIGO = 'OT';"); }
                catch { };
            }
            else
            {
                ((Panel)FindControl("fsCliente")).Visible = false;
            }
		}
		
		protected void Distribuir_Datos_Vehiculo(string placaVehiculo)
		{
			catalogo.Text   = DBFunctions.SingleData("SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			vin.Text        = DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			vinFinal        = DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			placa.Text      = placaVehiculo;
            marca.Text      = DBFunctions.SingleData("SELECT pmar_nombre FROM mcatalogovehiculo MCAT,pcatalogovehiculo PCAT,pmarca PMAR WHERE MCAT.mcat_placa='" + placaVehiculo + "' AND MCAT.pcat_codigo=PCAT.pcat_codigo and PCAT.pmar_codigo=PMAR.pmar_codigo");
            detalle.Text    = DBFunctions.SingleData("SELECT pcat_descripcion FROM mcatalogovehiculo MCAT,pcatalogovehiculo PCAT WHERE MCAT.mcat_placa='" + placaVehiculo + "' AND MCAT.pcat_codigo=PCAT.pcat_codigo");
			numeroMotor.Text = DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			serie.Text      = DBFunctions.SingleData("SELECT mcat_serie FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			chasis.Text     = DBFunctions.SingleData("SELECT mcat_chasis FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			color.Text      = DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo=(SELECT pcol_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"')");
			anoModelo.Text  = DBFunctions.SingleData("SELECT mcat_anomode FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			tipoServicio.Text = DBFunctions.SingleData("SELECT tser_nombserv FROM tserviciovehiculo WHERE tser_tiposerv=(SELECT tser_tiposerv FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"')");
			conscVendedor.Text = DBFunctions.SingleData("SELECT mcat_concvend FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			fechaVenta.Text = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mcat_venta FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'")).ToString("yyyy-MM-dd");
			ultimoKilometraje.Text = DBFunctions.SingleData("SELECT mcat_numeultikilo FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			kilometrajePromedio.Text = DBFunctions.SingleData("SELECT mcat_numekiloprom FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
		}
		
		protected DataTable Preparar_Tabla_Operaciones(DataTable tablaAsociada)
		{
			tablaAsociada.Columns.Add(new DataColumn("PREFNUMORD",System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("CODIGOOPERACION", System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("CANTIDAD", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("TIEMPO", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("PRECIO", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("KILOMETRAJE", System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("REPUESTOS",System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("FECHATERM",System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("OPERACION", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("FECHA", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("MECANICO", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("ESTADO", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("OBSERECE", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("OBSECLIE", System.Type.GetType("System.String")));
			return tablaAsociada;
		}
		
		protected void Distribuir_Grilla_Operaciones(string placaVehiculo)
		{
			string prefijoNumeroOrden = String.Empty;
			double valorSumatoria = 0;
            int aux = 0;
			string vin = DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			DataTable tablaOperacionesRealizadas = new DataTable();
			tablaOperacionesRealizadas = Preparar_Tabla_Operaciones(tablaOperacionesRealizadas);
            string sqlHojaVida = @"SELECT MO.PDOC_CODIGO concat ' - ' concat cast(mo.mord_numeorde as char (10)), ' Orden:', 'Ident ' concat VN.NOMBRE CONCAT ' - ' CONCAT coalesce(MNS.MNIT_APELLIDOS,''), 0, MO.MORD_KILOMETRAJE, MO.MORD_ENTRADA, MO.MORD_FECHLIQU, 
                   PV.PVEN_NOMBRE, TEST_NOMBRE concat ' - ' concat TCAR_NOMBRE, MORD_OBSECLIE, MORD_OBSERECE
             FROM MCATALOGOVEHICULO MC, VMNIT VN, PVENDEDOR PV, TESTADOORDEN TE, TCARGOORDEN TC, MORDEN MO
             LEFT JOIN DORDENSEGUROS DS ON MO.PDOC_CODIGO = DS.PDOC_CODIGO AND MO.MORD_NUMEORDE = DS.MORD_NUMEORDE AND MO.TCAR_CARGO = 'S'
             LEFT JOIN MNIT MNS ON DS.MNIT_NITSEGUROS = MNS.MNIT_NIT
            WHERE MCAT_PLACA = '" + placaVehiculo + @"'
            AND MC.MCAT_VIN = MO.MCAT_VIN
            AND MO.MNIT_NIT = VN.MNIT_NIT
            AND MO.PVEN_CODIGO = PV.PVEN_CODIGO
            AND MO.TEST_ESTADO = TE.TEST_ESTADO
            AND MO.TCAR_CARGO = TC.TCAR_CARGO
            AND MO.TEST_ESTADO <> 'X'
               UNION
            SELECT MO.PDOC_CODIGO concat ' - ' concat cast(mo.mord_numeorde as char (10)), 'Factura:', mf.pdoc_codigo concat ' - ' concat cast(mf.mfac_numedocu as char (10)) concat VN.NOMBRE, mf.mfac_valofact,Mf.Mfac_valoiva, MO.MORD_ENTRADA, Mf.Mfac_factura, 
                   PV.PVEN_NOMBRE, TCAR_NOMBRE, mf.Mfac_OBSErvacion concat ' ' concat mft.mfac_observacion, 'Descto M.O. ' concat cast(mfac_descoperaciones as varchar(20)) concat ' - ' concat ' Descto Reptos ' concat cast(mfac_descrepuestos as varchar (20)) 
             FROM MCATALOGOVEHICULO MC, mfacturaclientetaller mft, mfacturacliente mf, MORDEN MO, VMNIT VN, PVENDEDOR PV, TCARGOORDEN TC
            WHERE MCAT_PLACA = '" + placaVehiculo + @"'
            AND MC.MCAT_VIN = MO.MCAT_VIN
            AND Mf.MNIT_NIT = VN.MNIT_NIT
            AND MO.PVEN_CODIGO = PV.PVEN_CODIGO
            AND mft.TCAR_CARGO = TC.TCAR_CARGO
            AND MO.PDOC_CODIGO = mft.PDOC_prefordetrab AND MO.MORD_NUMEORDE = mft.MORD_NUMEORDE
            AND MO.TEST_ESTADO <> 'X'
            and mft.pdoc_codigo = mf.pdoc_codigo and mft.mfac_numedocu = mf.mfac_numedocu
              union
            SELECT MO.PDOC_CODIGO concat ' - ' concat cast(mo.mord_numeorde as char (10)), 'Llamada:', pa.Pact_nombmark, 
                   1,MO.MORD_KILOMETRAJE,da.dORD_fecha, Da.dORD_FECHa,da.dord_ejecutor, 'Crm', pr.pres_descripcion, DA.DORD_DETALLE
             FROM MCATALOGOVEHICULO MC, MORDEN MO,DORDENACTIVIDAD DA, pactividadmarketing pa, presultadoactividad pr
            WHERE MCAT_PLACA = '" + placaVehiculo + @"'
            AND MC.MCAT_VIN = MO.MCAT_VIN
            and MO.PDOC_CODIGO = DA.PDOC_CODIGO AND MO.MORD_NUMEORDE = DA.MORD_NUMEORDE
            and da.pact_codimark = pa.pact_codimark
            and da.pres_codigo = pr.pres_codigo
            AND MO.TEST_ESTADO <> 'X'
               union
            SELECT MO.PDOC_CODIGO concat ' - ' concat cast(mo.mord_numeorde AS char(10)),  
                    'Operc:',  
                    PT.PTEM_OPERACION concat ' - ' concat PTEm_DESCRIPCION,
                    DP.DORD_TIEMlIQU,  
                    DP.DORD_VALOOPER,  
                    MO.MORD_ENTRADA,  
                    DP.DORD_FECHCUMP,  
                    PV.PVEN_NOMBRE,  
                    TE.TEST_NOMBRE concat ' - ' concat TC.TCAR_NOMBRE,  
                    DS.DOBSOT_OBSERVACIONES,  
                    ''
             FROM MCATALOGOVEHICULO MC,  
                  PTEMPARIO PT,
                  PVENDEDOR PV,  
                  TESTADOOPERACION TE,
                  TCARGOORDEN TC,  
                  MORDEN MO,
                  DORDENOPERACION DP
               LEFT JOIN DOBSERVACIONESOT DS ON DP.PTEM_OPERACION = DS.PTEM_OPERACION AND DP.PDOC_CODIGO = DS.PDOC_CODIGO AND DP.MORD_NUMEORDE = DS.MORD_NUMEORDE
             WHERE MCAT_PLACA = '" + placaVehiculo + @"'
             AND MC.MCAT_VIN = MO.MCAT_VIN
             AND MO.PDOC_CODIGO = DP.PDOC_CODIGO
             AND MO.MORD_NUMEORDE = DP.MORD_NUMEORDE
             AND DP.PTEM_OPERACION = PT.PTEM_OPERACION
             AND DP.PVEN_CODIGO = PV.PVEN_CODIGO
             AND DP.TEST_ESTADO = TE.TEST_ESTAOPER
             AND DP.TCAR_CARGO = TC.TCAR_CARGO
             AND MO.TEST_ESTADO <> 'X'
               union
            select * from (
           SELECT orden, 'Repto:', item, sum(cantidad) as cantidad, dite_valounit, mord_entrada, mfac_factura, pven_nombre, tcar_nombre, mfac_observacion, TRANSFER

            FROM(SELECT MO.PDOC_CODIGO concat ' - ' concat cast(mo.mord_numeorde as char (10)) as orden, DBXSCHEMA.EDITARREFERENCIAS(mi.mite_codigo, pl.plin_tipo) concat ' - ' concat mite_nombre as item,
                   di.Dite_cantidad AS CANTIDAD, di.dite_valounit, MO.MORD_ENTRADA, mt.mfac_factura, PV.PVEN_NOMBRE, TC.TCAR_NOMBRE, mt.mfac_observacion, MT.PDOC_CODIGO || MT.MFAC_NUMEDOCU AS TRANSFER
              FROM MCATALOGOVEHICULO MC, mordentransferencia mot, mitems mi, PLINEAITEM PL, PVENDEDOR PV, mfacturacliente mt, TCARGOORDEN TC, MORDEN MO, Ditems Di
            WHERE MCAT_PLACA = '" + placaVehiculo + @"'
            AND MC.MCAT_VIN = MO.MCAT_VIN
            AND MO.PDOC_CODIGO = mot.PDOC_CODIGO AND MO.MORD_NUMEORDE = mot.MORD_NUMEORDE
            AND MOt.PDOC_factura = di.PDOC_CODIGO  AND MOt.Mfac_NUMEro = di.dite_NUMEdocu
            AND MOt.PDOC_factura = mt.PDOC_CODIGO  AND MOt.Mfac_NUMEro = mt.mfac_NUMEdocu
            AND Di.mite_codigo = mi.mite_codigo  AND MI.PLIN_CODIGO = PL.PLIN_codigo and di.tmov_tipomovi = 80
            AND mt.PVEN_CODIGO = PV.PVEN_CODIGO
            AND MO.TEST_ESTADO <> 'X'
            AND mot.TCAR_CARGO = TC.TCAR_CARGO

              union

            SELECT MO.PDOC_CODIGO concat ' - ' concat cast(mo.mord_numeorde as char (10)) as orden, DBXSCHEMA.EDITARREFERENCIAS(mi.mite_codigo, pl.plin_tipo) concat ' - ' concat mite_nombre as item,
                   did.Dite_cantidad*-1 AS CANTIDAD, di.dite_valounit, MO.MORD_ENTRADA, mt.mfac_factura, PV.PVEN_NOMBRE, TC.TCAR_NOMBRE, mt.mfac_observacion, MT.PDOC_CODIGO || MT.MFAC_NUMEDOCU AS TRANSFER
              FROM MCATALOGOVEHICULO MC, mordentransferencia mot, mitems mi, PLINEAITEM PL, PVENDEDOR PV, mfacturacliente mt, TCARGOORDEN TC, MORDEN MO, Ditems Di, Ditems Did
            WHERE MCAT_PLACA = '" + placaVehiculo + @"'
            AND MC.MCAT_VIN = MO.MCAT_VIN
            AND MO.PDOC_CODIGO = mot.PDOC_CODIGO AND MO.MORD_NUMEORDE = mot.MORD_NUMEORDE
            AND MOt.PDOC_factura = di.PDOC_CODIGO  AND MOt.Mfac_NUMEro = di.dite_NUMEdocu
            AND MOt.PDOC_factura = mt.PDOC_CODIGO  AND MOt.Mfac_NUMEro = mt.mfac_NUMEdocu
            AND Di.mite_codigo = mi.mite_codigo  AND MI.PLIN_CODIGO = PL.PLIN_codigo and di.tmov_tipomovi = 80
            AND mt.PVEN_CODIGO = PV.PVEN_CODIGO
            AND MO.TEST_ESTADO <> 'X'
            AND mot.TCAR_CARGO = TC.TCAR_CARGO
            and di.pdoc_codigo = did.dite_prefdocurefe and di.dite_numedocu = DID.dite_numedocurefe AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVi = 81
            ) AS A group by orden, item, dite_valounit, mord_entrada, mfac_factura, pven_nombre, tcar_nombre, mfac_observacion, TRANSFER
			) WHERE CANTIDAD <> 0
             ORDER BY 6 DESC,1 ASC,2 ASC, 3 ASC;";

            ViewState["CONSULTAExcel"] = sqlHojaVida;
            DBFunctions.Request(operacionesRealizadas, IncludeSchema.NO, sqlHojaVida);
            for (int i = 0; i < operacionesRealizadas.Tables[0].Rows.Count; i++)
			{
				if(prefijoNumeroOrden != String.Empty && prefijoNumeroOrden != operacionesRealizadas.Tables[0].Rows[i][0].ToString())
				{
					DataRow drTotal = tablaOperacionesRealizadas.NewRow();
					drTotal["PREFNUMORD"] = "<span style='color:#FF0000'>TOTAL "+prefijoNumeroOrden+"</span>";
                    drTotal["CODIGOOPERACION"] = drTotal["DESCRIPCION"] = drTotal["OPERACION"] = drTotal["KILOMETRAJE"] = drTotal["FECHA"] = drTotal["MECANICO"] = drTotal["ESTADO"] = drTotal["OBSECLIE"]  = drTotal["OBSERECE"] = "------";
					drTotal["PRECIO"] = valorSumatoria;
                    tablaOperacionesRealizadas.Rows.Add(drTotal);
					valorSumatoria = 0;
                    aux = i;
				}
				DataRow fila = tablaOperacionesRealizadas.NewRow();
                string operacion = operacionesRealizadas.Tables[0].Rows[i][1].ToString().TrimEnd();
				fila["PREFNUMORD"]  = prefijoNumeroOrden = operacionesRealizadas.Tables[0].Rows[i][0].ToString();
                fila["OPERACION"] = "<span style='color:#006400'>" + operacion + "</span>" + operacionesRealizadas.Tables[0].Rows[i][2].ToString();
		 
                switch (operacion)
                {
                    case " Orden:":
                        try { fila["KILOMETRAJE"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][4]).ToString("#,##0"); }
                        catch { }
                        break;

                    case "Factura:":
                        try { fila["PRECIO"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][3]).ToString("#,##0.00"); }
                        catch { }
                        try { fila["IVA"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][4]).ToString("#,##0"); }
                        catch { }
                        break;

                    case "Llamada:":
                        try { fila["PRECIO"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][3]).ToString("#,##0.00"); }
                        catch { }
                        try { fila["KILOMETRAJE"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][4]).ToString("#,##0"); }
                        catch { }
                        break;

                    case "Operc:":
                        try { fila["TIEMPO"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][3]).ToString("#,##0.00"); }
                        catch { }
                        try { fila["PRECIO"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][4]).ToString("#,##0"); }
                        catch { }
                        break;

                    case "Repto:":
                        try { fila["CANTIDAD"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][3]).ToString("#,##0.00"); }
                        catch { }
                        try { fila["PRECIO"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][4]).ToString("#,##0"); }
                        catch { }
                        break;
                }

                try { fila["FECHA"] = Convert.ToDateTime(operacionesRealizadas.Tables[0].Rows[i][5]).ToString("yyyy-MM-dd"); }
                 catch { }
                try { fila["FECHATERM"] = Convert.ToDateTime(operacionesRealizadas.Tables[0].Rows[i][6]).ToString("yyyy-MM-dd"); }
                 catch { }

                fila["MECANICO"] = operacionesRealizadas.Tables[0].Rows[i][7].ToString();
                fila["ESTADO"] = operacionesRealizadas.Tables[0].Rows[i][8].ToString();
                fila["OBSERECE"] = "<span style='color:#FF0000'> " + operacionesRealizadas.Tables[0].Rows[i][9].ToString() + "</span>";
                fila["OBSECLIE"] = "<span style='color:#FF0000'> " + operacionesRealizadas.Tables[0].Rows[i][10].ToString() + "</span>"; 
			
		
				tablaOperacionesRealizadas.Rows.Add(fila);
                ViewState["consulta"] = operacionesRealizadas;
            }
			if(prefijoNumeroOrden != String.Empty)
			{
				DataRow drTotal = tablaOperacionesRealizadas.NewRow();
				drTotal["PREFNUMORD"] = "<span style='color:#FF0000'>TOTAL "+prefijoNumeroOrden+"</span>";
				drTotal["CODIGOOPERACION"] = "------";
				drTotal["DESCRIPCION"] = "------";
                drTotal["OPERACION"] = "------";
				drTotal["PRECIO"] = valorSumatoria;
                drTotal["OBSERECE"] = "-----";
                drTotal["OBSECLIE"] = "-----"; 
				tablaOperacionesRealizadas.Rows.Add(drTotal);
				valorSumatoria = 0;
			}
			operacionesRealizadasGrilla.DataSource = tablaOperacionesRealizadas;
			operacionesRealizadasGrilla.DataBind();
		}

        protected void ImprimirExcelGrid(Object Sender, EventArgs e)
        {
            try
            {
                operacionesRealizadas = DBFunctions.Request(operacionesRealizadas, IncludeSchema.YES, ViewState["CONSULTAExcel"].ToString());
                Utils.ImprimeExcel(operacionesRealizadas, "ConsultaTabla");
            }
            catch (Exception ex)
            {
                lblResult.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
                return;
            }
        }


        protected string Cuerpo_Correo()
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			controlesConsulta.RenderControl(htmlTW);
			return SB.ToString();
		}

		public void PrepararTabla()
		{
			resultado = new DataTable();
			resultado.Columns.Add(new DataColumn("ORDENOP",typeof(string)));
			resultado.Columns.Add(new DataColumn("REPUESTO",typeof(string)));
			resultado.Columns.Add(new DataColumn("VALORU",typeof(string)));
			resultado.Columns.Add(new DataColumn("CANTIDAD",typeof(string)));
			resultado.Columns.Add(new DataColumn("CODREPUESTO",typeof(string)));
			resultado.Columns.Add(new DataColumn("OTRELAC",typeof(string)));
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
