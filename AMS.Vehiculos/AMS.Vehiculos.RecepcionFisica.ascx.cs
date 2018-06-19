using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Configuration;
using AMS.Forms;
using AMS.Tools;
using AMS.Documentos;
using AMS.DB;
using Ajax;

namespace AMS.Vehiculos
{
	public partial class RecepcionFisica : System.Web.UI.UserControl
	{
        protected string indexPage = System.Configuration.ConfigurationManager.AppSettings["MainIndexPage"];
        FormatosDocumentos formatoRecibo = new FormatosDocumentos();

        protected void Page_Load(object sender, EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Vehiculos.RecepcionFisica));

            if (!IsPostBack)
            {
                if (Request.QueryString["ok"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ha realizado la recepción física correctamente! \\n Prefijo Recepción: " + Request.QueryString["pref"] + " - " + Request.QueryString["num"] + "\\n Prefijo orden: " + Request.QueryString["prefOT"] + " - " + Request.QueryString["numOT"]);
                    try
                    {
                        formatoRecibo.Prefijo = Request.QueryString["pref"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["num"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["pre"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                        }
                        formatoRecibo.Prefijo = Request.QueryString["prefOT"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numOT"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefOT"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=500');</script>");
                        }
                    }
                    catch
                    {
                        Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>" + "Los formatos pueden ser impresos por la opción Impresión<br>");
                    }
                }

                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE pcen_centvvn is not null and TVIG_VIGENCIA = 'V' or pcen_centvvu is not null ORDER BY palm_descripcion");
                if (ddlAlmacen.Items.Count > 1)
                    ddlAlmacen.Items.Insert(0, new ListItem("Seleccione...", "0"));

                bind.PutDatasIntoDropDownList(ddlCatalogo, "SELECT PP.PCAT_CODIGO, PP.PCAT_CODIGO || ' - ' || PC.PCAT_DESCRIPCION FROM PCATALOGOVEHICULO PC, PPRECIOVEHICULO PP WHERE PC.PCAT_CODIGO = PP.PCAT_CODIGO; ");
                if (ddlCatalogo.Items.Count > 1)
                    ddlCatalogo.Items.Insert(0, new ListItem("Seleccione...", "0"));

                bind.PutDatasIntoDropDownList(ddlColor, "SELECT pcol_codigo, pcol_descripcion concat ' - [' concat ptip_descripcion concat ']' DESCRIPCION " +
                                                        "FROM dbxschema.pcolor pcol inner join dbxschema.ptipopintura ptpi on pcol.ptip_codigo = ptpi.ptip_codigo ORDER BY pcol_descripcion;");
                if (ddlColor.Items.Count > 1)
                    ddlColor.Items.Insert(0, new ListItem("Seleccione...", "0"));

                bind.PutDatasIntoDropDownList(ddlServicio, "SELECT * from tserviciovehiculo ORDER BY tser_tiposerv");
                if (ddlServicio.Items.Count > 1)
                    ddlServicio.Items.Insert(0, new ListItem("Seleccione...", "0"));

                bind.PutDatasIntoDropDownList(ddlOpcion, "SELECT POPC_OPCIVEHI, POPC_OPCIVEHI || ' - ' || POPC_NOMBOPCI FROM DBXSCHEMA.POPCIONVEHICULO");
                if(ddlOpcion.Items.Count > 0)
                    ddlOpcion.Items.Insert(0, new ListItem("Seleccione...", "0"));

                DataSet dtAccesorios = new DataSet();
            //    DBFunctions.Request(dtAccesorios, IncludeSchema.NO, "select PREC_PRECID AS CODIGO, PREC_PRECNOMB AS NOMBRE from precepcionaccesorio;");
                DBFunctions.Request(dtAccesorios, IncludeSchema.NO, "select pacc_codigo as CODIGO, PACC_DESCRIPCION AS NOMBRE FROM paccesorio;");

                if (dtAccesorios != null && dtAccesorios.Tables[0].Rows.Count > 0)
                {
                    grdAccesorios.DataSource = dtAccesorios;
                    grdAccesorios.DataBind();
                }
                else
                {
                    Utils.MostrarAlerta(Response, "Por favor parametrizar los accesorios para registrar en la recepción física del vehículo!");
                }
            }else
            {
                
                string tales = Request["__EVENTARGUMENT"];
                string[] otroTal = Request["__EVENTTARGET"].ToString().Split(',');
                if (tales.Length > 0)
                {
                    ViewState["datos"] = tales;
                    ViewState["posAccesorios"] = otroTal;
                    BtnRegistrar_Click(sender, e);
                }
            }
                

        }
        [Ajax.AjaxMethod]
        public string llenarVinBasico(string catalogo)
        {
            return DBFunctions.SingleData("SELECT PCAT_VINBASICO FROM DBXSCHEMA.PCATALOGOVEHICULO WHERE PCAT_CODIGO = '" + catalogo + "';");
        }
        [Ajax.AjaxMethod]
        public string cargarNumOrden(string ot)
        {
            return DBFunctions.SingleData("SELECT PDOC_ULTIDOCU + 1 FROM DBXSCHEMA.PDOCUMENTO WHERE PDOC_CODIGO ='" + ot + "';");
        }
        [Ajax.AjaxMethod]
        public DataSet validar_VIN1(string VIN)
        {
            DataSet datosVin = new DataSet();
            string vin = VIN;
            DBFunctions.Request(datosVin, IncludeSchema.NO, @"select MC.MCAT_VIN, MC.PCAT_CODIGO, MC.MCAT_MOTOR, MC.MCAT_PLACA, MC.MCAT_SERIE, MC.MCAT_CHASIS, MC.PCOL_CODIGO, MC.MCAT_ANOMODE, MC.MCAT_NUMEKILOVENT, MV.MPRO_NIT, MC.TSER_TIPOSERV, MV.POPC_OPCIVEHI  
                                                             from MCATALOGOVEHICULO MC, MVEHICULO MV WHERE MC.MCAT_VIN = MV.MCAT_VIN AND MC.MCAT_VIN = '" + vin + "';");
            
            return datosVin;
        }
        protected void CargarPrefijos(object Sender, EventArgs e)
        {
            Utils.llenarPrefijos(Response, ref ddlPrefijo, "%", ddlAlmacen.SelectedValue , "RV");
            Utils.llenarPrefijos(Response, ref ddlPrefijoOT, "%", ddlAlmacen.SelectedValue, "OT");
            
            if (ddlPrefijo != null && ddlPrefijo.Items.Count > 0)
                txtNumeroPrefijo.Text = DBFunctions.SingleData("select pdoc_ultidocu + 1 from pdocumento where pdoc_codigo='" + ddlPrefijo.SelectedValue + "';");

            if (ddlPrefijoOT != null && ddlPrefijoOT.Items.Count > 0)
                txtNumeroOT.Text = DBFunctions.SingleData("select pdoc_ultidocu + 1 from pdocumento where pdoc_codigo='" + ddlPrefijoOT.SelectedValue + "';");

        }

        protected void CargaNumeroPrefijo(object Sender, EventArgs e)
        {
            if (ddlPrefijo != null && ddlPrefijo.Items.Count > 0)
                txtNumeroPrefijo.Text = DBFunctions.SingleData("select pdoc_ultidocu + 1 from pdocumento where pdoc_codigo='" + ddlPrefijo.SelectedValue + "';");

        }

        protected void CargaNumeroOT(object Sender, EventArgs e)
        {
            if (ddlPrefijoOT != null && ddlPrefijoOT.Items.Count > 0)
                txtNumeroOT.Text = DBFunctions.SingleData("select pdoc_ultidocu + 1 from pdocumento where pdoc_codigo='" + ddlPrefijoOT.SelectedValue + "';");

        }

        [Ajax.AjaxMethod]
        public string ValidarVIN(string vin, string motor, string placa)
        {
            DataSet dtVehiculo = new DataSet();
            string mensaje = "";

            if (vin != "")
            {
                DBFunctions.Request(dtVehiculo, IncludeSchema.NO, "select mcat_placa, mcat_motor from DBXSCHEMA.mcatalogovehiculo where mcat_vin = '" + vin + "';");
                if (dtVehiculo != null && dtVehiculo.Tables[0].Rows.Count > 0)
                {
                    if ( (motor != "" && motor != dtVehiculo.Tables[0].Rows[0][1].ToString()) || (placa != "" && placa != dtVehiculo.Tables[0].Rows[0][0].ToString()) )
                        mensaje = "El VIN: " + vin + " actualmente existe en el catálogo!  Serial del motor: " + dtVehiculo.Tables[0].Rows[0][1].ToString() + "  Placa del vehículo: " + dtVehiculo.Tables[0].Rows[0][0].ToString() + ". Por favor verifique los datos!";
                }
            }

            return mensaje;
        }

        protected void BtnRegistrar_Click(object Sender, EventArgs e)
        {
            bool resultado = false;
            lblInfoSistema.Text = "";
            string razon = "";
            string[] losDatos = Convert.ToString(ViewState["datos"]).ToString().Split('~');
            string[] checkeados = (string[])ViewState["posAccesorios"];
            if (losDatos.Length != 18 || checkeados == null)
            {
                Utils.MostrarAlerta(Response, "Algún dato se encentra mal escrito, recuerde que no se permite el uso de caracteres especiales");
                return;
            }
            else
            {
                DataSet dsVehiculo = new DataSet();//                                                                                                     vin
                DBFunctions.Request(dsVehiculo, IncludeSchema.NO, "select mcat_placa, mcat_motor from DBXSCHEMA.mcatalogovehiculo where mcat_vin = '" + losDatos[6] + "';");
                //existe catálogo
                if (dsVehiculo.Tables[0].Rows.Count > 0)
                {
                    //                                                                                                             vin
                    string estado = DBFunctions.SingleData("SELECT TEST_TIPOESTA FROM DBXSCHEMA.MVEHICULO WHERE MCAT_VIN = '" + losDatos[6] + "'").Trim();
                    if (estado == "" || Convert.ToInt32(estado) >= 60)
                    {
                        if (operarRecepcion(losDatos, checkeados, 1, ref razon))
                        {
                            resultado = true;
                        }
                    }
                    else//es menor a 60
                    {
                        if(operarRecepcion(losDatos, checkeados, 3, ref razon))
                        {
                            resultado = true;
                        }
                    }
                    
                }
                else //no existe catálogo
                {
                    if(operarRecepcion(losDatos, checkeados, 2, ref razon))
                    {
                        resultado = true;
                    }
                }
                if (resultado)
                    Response.Redirect(indexPage + "?process=Vehiculos.RecepcionFisica&ok=1&pref=" + losDatos[1] + "&num=" + losDatos[2] + "&prefOT=" + losDatos[3] + "&numOT=" + losDatos[4] + "");
                else
                    lblInfoSistema.Text = "proceso ejecutado con errores -> " + razon;

            }
        }

        protected bool operarRecepcion(string[] losDatos, string[] losCheck, int proceso, ref string razon)
        {
            bool rta = false;
            ArrayList sqlStrings = new ArrayList();
            //ArrayList sqlStringsTablas = new ArrayList();
            //string sqlSentence;
            string almacen = losDatos[0], prefijo = losDatos[1], numeroPref = losDatos[2], prefijoOt = losDatos[3], numeroOt = losDatos[4], catalogo = losDatos[5],
                vin = losDatos[6], motor = losDatos[7], placa = losDatos[8], proveedor = losDatos[9], color = losDatos[10], serie = losDatos[11],
                chasis = losDatos[12], anoModelo = losDatos[13], kilometraje = losDatos[14], servicio = losDatos[15], opcion = losDatos[16], observacion = losDatos[17];
            int numeInv = Convert.ToInt32(DBFunctions.SingleData("SELECT MAX(MVEH_INVENTARIO + 1) FROM DBXSCHEMA.MVEHICULO; "));
            txtNumeroOT.Text = numeroOt;
            //numeInv++;
            placa =  (placa == "" ? DBFunctions.SingleData("SELECT 'N-' || MAX(CAST (SUBSTR(mcat_placa,3) + 1 AS INTEGER)) FROM mcatalogovehiculo WHERE mcat_placa LIKE 'N-%'") : placa);
            switch (proceso)
            {
                case 1:
                    //mvehiculo
                    sqlStrings.Add("INSERT INTO DBXSCHEMA.MVEHICULO (MVEH_INVENTARIO,MCAT_VIN,TEST_TIPOESTA,MNIT_NIT,MVEH_NUMERECE,MVEH_KILOMETR,TCLA_CODIGO,MVEH_NUMEMANI,MVEH_ADUANA,MVEH_NUMELEVANTE,TCOM_CODIGO,MVEH_VALOCOMP,MPRO_NIT)"
                                    + "VALUES( " + numeInv + ",'" + vin + "',10,'" + proveedor + "'," + numeInv + ",'" + kilometraje + "','N','ACTUALIZAR','ACTUALIZAR','ACTUALIZAR','P',1,'" + proveedor + "')");

                    break;
                case 2:
                    sqlStrings.Add("INSERT INTO DBXSCHEMA.MCATALOGOVEHICULO (PCAT_CODIGO,MCAT_VIN,MCAT_PLACA,MCAT_MOTOR,MNIT_NIT,MCAT_SERIE,MCAT_CHASIS,PCOL_CODIGO,MCAT_ANOMODE,TSER_TIPOSERV,MCAT_VENTA,MCAT_NUMEKILOVENT, MCAT_NUMEULTIKILO, MCAT_NUMEKILOPROM, MCAT_MATRICULA) " 
                                    + "VALUES('" + catalogo + "','" + vin + "','" + placa + "','" + motor + "','" + proveedor + "','" 
                                    + serie + "','" + chasis + "','" + color + "'," + anoModelo + ",'" + servicio + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") 
                                    + "'," + kilometraje + "," + kilometraje + "," + kilometraje + ",NULL); ");
                    sqlStrings.Add("INSERT INTO DBXSCHEMA.MVEHICULO (MVEH_INVENTARIO,MCAT_VIN,TEST_TIPOESTA,MNIT_NIT,MVEH_NUMERECE,MVEH_KILOMETR,TCLA_CODIGO,MVEH_NUMEMANI,MVEH_ADUANA,MVEH_NUMELEVANTE,TCOM_CODIGO,MVEH_VALOCOMP,MPRO_NIT)"
                                    + "VALUES( " + numeInv + ",'" + vin + "',10,'" + proveedor + "'," + numeInv + ",'" + kilometraje + "','N','ACTUALIZAR','ACTUALIZAR','ACTUALIZAR','P',1,'" + proveedor + "')");
                    /*sqlStrings.Add("INSERT INTO DBXSCHEMA.MCATALOGOVEHICULO VALUES ('" + catalogo + "','" + vin + "'," +
                                "'" + placa + "', '" + motor + "', (select mnit_nit from cempresa), '" + serie + "', '" + chasis + "', '" + color+ "', " +
                                anoModelo + ", '" + servicio + "', NULL, '" + proveedor + "', '" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "', " +
                                kilometraje + ", NULL, " + kilometraje + ", CAST((select kiloprom_mensual from ctaller) AS INTEGER), NULL, " +
                                "'" + vin + "', '" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "');");*/
                    /*sqlStrings.Add("INSERT INTO DBXSCHEMA.MVEHICULO (MVEH_INVENTARIO,MCAT_VIN,TEST_TIPOESTA,MNIT_NIT,MVEH_NUMERECE,MVEH_KILOMETR,TCLA_CODIGO,MVEH_NUMEMANI,MVEH_ADUANA,MVEH_NUMELEVANTE,TCOM_CODIGO,MVEH_VALOCOMP,MPRO_NIT)"
                                    + "VALUES( DEFAULT,'VIN',10,'NIT','NUMERECE','KILOMETRAJE','N','NUMEMANI','ADUANA','NUMELEVANTE','P','VALOR','NITPRO')");*/
                    //mcatalogovehiculo,mvehiculo
                    break;
                default:
                    break;
            }
            string id = DBFunctions.SingleData("SELECT max(MREV_RECEID) + 1 FROM DBXSCHEMA.MVEHICULORECEPCION");
            id = id == "" ? "1" : id;
            //aquí insertar en las 3 tablas
            sqlStrings.Add("INSERT INTO DBXSCHEMA.MVEHICULORECEPCION (PDOC_CODIGO,MREV_RECENUM,MORD_CODIGO,MORD_NUMERO,MCAT_VIN,MREV_MOTOR,MREV_CHASIS,MREV_SERIE,MREV_PLACA,MREV_OBSERVACION,SUSU_LOGIN,MREV_FECHA)VALUES ('" 
                                + prefijo + "'," + numeroPref + ",'" + prefijoOt + "'," + numeroOt + ",'"
                                + vin + "','" + motor + "','" + chasis + "','" + serie + "','" + placa + "'," 
                                + "'" + observacion  + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "',DEFAULT)");
            sqlStrings.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = " + numeroPref + " WHERE PDOC_CODIGO = '" + prefijo + "'");
            


            /*if (sqlStrings.Count > 0)
            {
                if (!DBFunctions.Transaction(sqlStrings))
                {
                    razon += DBFunctions.exceptions + " ";
                    return false;
                }
            }*/
            //if (DBFunctions.NonQuery(sqlSentence) > 0)
            //{

            //string id = DBFunctions.SingleData("SELECT MREV_RECEID FROM DBXSCHEMA.MVEHICULORECEPCION WHERE MCAT_VIN = '" + vin + "' AND MREV_PLACA = '" + placa + "'");
            if (losCheck.Length >= 1 && losCheck[0].Length > 0)
                for (int k = 0; k < losCheck.Length; k++)
                {
                    sqlStrings.Add("INSERT INTO DBXSCHEMA.DRECEPCIONACCESORIO (PDOC_CODIGO,MREV_RECENUM,PACC_CODIGO,DREC_OBSERVACION) VALUES ('" + prefijo + "'," + numeroPref 
                                    + ",'" + losCheck[k].Split('~')[0] + "',"
                                    + "'" + losCheck[k].Split('~')[1] + "'); ");

                }
            DateTime fechaEntrega = DateTime.Now;
            //fechaEntrega.AddMonths(1);
            string vendedor = DBFunctions.SingleData("SELECT PV.PVEN_CODIGO FROM DBXSCHEMA.PVENDEDOR PV, DBXSCHEMA.PVENDEDORALMACEN PVA WHERE PV.PVEN_CODIGO = PVA.PVEN_CODIGO AND PVA.PALM_ALMACEN = '" + almacen + "' AND PVEN_VIGENCIA = 'V' AND PV.TVEND_CODIGO = 'RT'"),
            listaPrecio = DBFunctions.SingleData("SELECT PPRETA_CODIGO FROM DBXSCHEMA.PPRECIOTALLER FETCH FIRST ROW ONLY ;");
            vendedor = vendedor == "" ? DBFunctions.SingleData("SELECT PVEN_CODIGO FROM DBXSCHEMA.PVENDEDORALMACEN WHERE PALM_ALMACEN = '" + almacen + "'") : vendedor;


            sqlStrings.Add("INSERT INTO DBXSCHEMA.MORDEN (PDOC_CODIGO,MORD_NUMEORDE,MCAT_VIN,MNIT_NIT,TPRO_CODIGO,TEST_ESTADO,TCAR_CARGO,TTRA_CODIGO,MORD_ENTRADA,MORD_HORAENTR,MORD_CREACION,MORD_ENTREGAR,MORD_HORAENTG,MORD_NUMEENTR,MORD_KILOMETRAJE,PVEN_CODIGO,PALM_ALMACEN,MORD_OBSERECE,MORD_OBSECLIE,PPRETA_CODIGO,TTIP_CODIGO,TNIVCOMB_CODIGO,TESTCIT_ESTACITA) VALUES ("
                                     + "'" + prefijoOt + "'," + numeroOt + ",'" + vin + "','" + proveedor + "','P','A','A','S','" + fechaEntrega.ToString("yyyy-MM-dd") + "','" 
                                     + fechaEntrega.ToString("HH':'mm':'ss") + "','" + fechaEntrega.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fechaEntrega.AddMonths(1).ToString("yyyy-MM-dd") 
                                     + "','" + fechaEntrega.ToString("HH':'mm':'ss") + "','1','" + kilometraje + "','" + vendedor + "','" + almacen + "','" + observacion + "','" + observacion + "'," + "'" + listaPrecio + "','E','0','I');"
                           /*"'" + prefijoOt + "'," + numeroOt + ",NULL,'" + vin + "',(select mnit_nit from cempresa), " +
                           "'P','A','A','M','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "', " +
                           "'" + DateTime.Now.ToString("HH':'mm':'ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss") + "','"
                           + fechaEntrega.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH':'mm':'ss") + "',NULL, " +
                           "'   '," + kilometraje + ",(select pven_codigo from pvendedor fetch first row only),'" + almacen
                           + "',NULL, " + "NULL,NULL,'" + txtObservaciones.Text + "','ORDEN DE ALISTAMIENTO CREADA AUTOMATICAMENTE POR LA RECEPCION DEL VEHICULO','', " +
                           "'',(SELECT ppreta_codigo FROM ppreciotaller fetch first row only),'E','14','I', " +
                           "NULL,'N','N','N','' );"*/
            );
			string codOperacion = DBFunctions.SingleData("SELECT PTEM_OPERACION || ' ~ ' || PTEM_TIEMPOESTANDAR || ' ~ ' || PTEM_VALOOPER FROM DBXSCHEMA.PTEMPARIO WHERE PTEM_DESCRIPCION LIKE '%ALISTA%' OR PTEM_DESCRIPCION LIKE '%Alista%' FETCH FIRST ROW ONLY;");
			if(codOperacion == "")
			{
				codOperacion = DBFunctions.SingleData("SELECT PTEM_OPERACION || ' ~ ' || PTEM_TIEMPOESTANDAR || ' ~ ' || PTEM_VALOOPER FROM DBXSCHEMA.PTEMPARIO FETCH FIRST ROW ONLY");
			}
            sqlStrings.Add("INSERT INTO DBXSCHEMA.DORDENOPERACION (PDOC_CODIGO,MORD_NUMEORDE,PTEM_OPERACION,TCAR_CARGO,PVEN_CODIGO,DORD_FECHCUMP,TEST_ESTADO,DORD_TIEMLIQU,DORD_VALOOPER,DORD_TIEMPOPER,DORD_COSTOPER,PIVA_PORCIVA) VALUES "
                            + "('" + prefijoOt + "'," + numeroOt + ",'" + codOperacion.Split('~')[0] + "','A','" + vendedor + "','" + fechaEntrega.ToString("yyyy-MM-dd HH:mm:ss") + "','A'," + codOperacion.Split('~')[1] + "," + codOperacion.Split('~')[2] + "," + codOperacion.Split('~')[1] + "," + codOperacion.Split('~')[2] + ",0)");

            sqlStrings.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = " + numeroOt + " WHERE PDOC_CODIGO = '" + prefijoOt + "'");
            

            if (DBFunctions.Transaction(sqlStrings))
            {
                rta = true;
            }
            else
            {
                razon += DBFunctions.exceptions + " ";
                rta = false;
            }
            //}
            //else
            //{
                //razon += DBFunctions.exceptions + " "; ;
            //}

            return rta;

            
        }

        protected bool ValidarDatos()
        {
            bool aceptar = false;
            if (ddlAlmacen.Items.Count > 1
                || ddlPrefijo.Items.Count > 1
                || ddlPrefijoOT.Items.Count > 1
                || ddlCatalogo.Items.Count > 1)
            {
                lblAlm.Visible = lblPref.Visible = lblOT.Visible = lblCat.Visible = true;
                return false;
            }
            else
                lblAlm.Visible = lblPref.Visible = lblOT.Visible = lblCat.Visible = false;

            if (txtVIN.Text == ""
                || txtMotor.Text == ""
                || txtProveedor.Text == ""
                || txtSerie.Text == ""
                || txtChasis.Text == ""
                || txtModelo.Text == ""
                || txtKilometros.Text == ""
                || ddlColor.SelectedIndex == 0
                || ddlServicio.SelectedIndex == 0
                || ddlOpcion.SelectedIndex == 0)
            {
                lblVIN.Visible = true;
                aceptar = false;
            }
            else
                lblVIN.Visible = false;
            /*
            if (txtMotor.Text == "") { lblMotor.Visible = true; aceptar = false; }
            else lblMotor.Visible = false;

            if (txtProveedor.Text == "") { lblProv.Visible = true; aceptar = false; }
            else lblProv.Visible = false;

            if (ddlColor.SelectedIndex == 0 && ddlColor.Items.Count > 1) { lblCol.Visible = true; aceptar = false; }
            else lblCol.Visible = false;

            if (txtSerie.Text == "") { lblSeri.Visible = true; aceptar = false; }
            else lblSeri.Visible = false;

            if (txtChasis.Text == "") { lblChas.Visible = true; aceptar = false; }
            else lblChas.Visible = false;

            if (txtModelo.Text == "") { lblMod.Visible = true; aceptar = false; }
            else lblMod.Visible = false;

            if (txtKilometros.Text == "") { lblKilo.Visible = true; aceptar = false; }
            else lblKilo.Visible = false;

            if (ddlServicio.SelectedIndex == 0 && ddlServicio.Items.Count > 1) { lblSer.Visible = true; aceptar = false; }
            else lblSer.Visible = false;

            if(ddlOpcion.SelectedValue == "0" || ddlServicio.Items.Count == 0) { aceptar = false; }
            */
            return aceptar;
        }

	}
}