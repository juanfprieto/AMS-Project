// created on 09/11/2004 at 13:59
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
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
    public partial class TorreControl : System.Web.UI.UserControl
    {
        #region Atributos
        public static string elementosBorrados;
        public DataTable miTabla;
        protected string Multitaller = "";
        protected int numDecimales = 0;
        protected string numeroOrdenSeleccionada;
        protected string tipoDocumentoSeleccionado;
        protected string codigoPeritaje;
        protected string tipoProceso;
        protected string liqGarantiaFabrica;

        protected DataTable tablaOperaciones;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected DatasToControls bind = new DatasToControls();

        #endregion

        #region Eventos
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Automotriz.TorreControl));
            tipoProceso = Request.QueryString["actor"]; //Debe definir el tipo de proceso: Torre de Control / Liquidacion OT
            try { liqGarantiaFabrica = DBFunctions.SingleData("SELECT COALESCE(CTAL_IVALIQUGARA,'S') FROM ctaller"); }
            catch { liqGarantiaFabrica = "S"; };

            if (!IsPostBack)
            {
                elementosBorrados = "";
                Session.Clear();
                //bind.PutDatasIntoDropDownList(tipoDocumento, "SELECT DISTINCT PD.pdoc_codigo, PD.pdoc_codigo || ' - ' || pdoc_nombre FROM pdocumento PD, MORDEN MO WHERE tdoc_tipodocu='OT' and MO.PDOC_CODIGO = PD.PDOC_CODIGO and mo.test_estado='A' ");
                Utils.llenarPrefijos(Response, ref tipoDocumento, "%", "%", "OT");
                if (tipoDocumento.Items.Count > 1)
                    tipoDocumento.Items.Insert(0, "Seleccione:..");
                else
                    bind.PutDatasIntoDropDownList(ordenes, "SELECT mord_numeorde FROM morden WHERE test_estado='A' AND pdoc_codigo='" + tipoDocumento.SelectedValue + "' ORDER BY mord_numeorde ASC");

                string centavos = DBFunctions.SingleData("SELECT COALESCE(CEMP_LIQUDECI,'N') FROM CEMPRESA"); //ConfigurationManager.AppSettings["ManejoCentavos"]; // SE debe leer de cempresa
                if (centavos == "S")
                    numDecimales = 2;
                else
                    numDecimales = 0;
                ViewState["numDecimales"] = numDecimales;


            }
            if (Session["tblTerceros"] == null)
                llenarMiTabla();
            else
                miTabla = (DataTable)Session["tblTerceros"];

            if (Session["tablaOperaciones"] == null)
                PrepararTablaOperaciones();
            else if (Session["tablaOperaciones"] != null)
                    tablaOperaciones = (DataTable)Session["tablaOperaciones"];

            if (Session["numeroOrdenSeleccionada"] == null) {

                Session["numeroOrdenSeleccionada"] = ordenes.SelectedValue;
                numeroOrdenSeleccionada = ordenes.SelectedValue;
            }
            else if (Session["numeroOrdenSeleccionada"] != null)
                    numeroOrdenSeleccionada = Session["numeroOrdenSeleccionada"].ToString();

            if (Session["tipoDocumentoSeleccionado"] == null) {

                Session["tipoDocumentoSeleccionado"] = tipoDocumento.SelectedValue;
                tipoDocumentoSeleccionado = tipoDocumento.SelectedValue;
            }
            else if (Session["tipoDocumentoSeleccionado"] != null)
                    tipoDocumentoSeleccionado = Session["tipoDocumentoSeleccionado"].ToString();

            codigoPeritaje = DBFunctions.SingleData("SELECT ptem_operacion FROM ctaller").ToString();

        }

        protected void llenarMiTabla()
        {
            miTabla = new DataTable();
            miTabla.Columns.Add(new DataColumn("PREFIJODOC", System.Type.GetType("System.String")));
            miTabla.Columns.Add(new DataColumn("NUMORDEN", System.Type.GetType("System.String")));
            miTabla.Columns.Add(new DataColumn("CODIGOOP", System.Type.GetType("System.String")));
            miTabla.Columns.Add(new DataColumn("NOMBREOP", System.Type.GetType("System.String")));
            Session["tblTerceros"] = miTabla;
        }

        protected void Cambio_Documento(Object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ordenes, "SELECT mord_numeorde FROM morden WHERE test_estado='A' AND pdoc_codigo='" + tipoDocumento.SelectedValue + "' ORDER BY mord_numeorde ASC");
        }

        protected void Cargar_Orden(Object Sender, EventArgs e)
        {
            if (ordenes.Items.Count > 0)
            {
                Session.Clear();
                CargarGrilla();
                numeroOrdenSeleccionada = ordenes.SelectedItem.ToString();
                tipoDocumentoSeleccionado = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + tipoDocumento.SelectedItem.ToString() + "'");
                Session["numeroOrdenSeleccionada"] = numeroOrdenSeleccionada;
                Session["tipoDocumentoSeleccionado"] = tipoDocumentoSeleccionado;
                aceptar.Enabled = true;
            }
            else
                Utils.MostrarAlerta(Response, "No Se Ha Seleccionado Ninguna Orden");
        }

        protected void Guardar_Cambios(Object Sender, EventArgs e)
        {
            string tTercero = DBFunctions.SingleData("SELECT CTAL_TRABTERCTALL FROM CTALLER");
            ArrayList sqlStrings = new ArrayList();
            int i;
            bool incoveniente = false;
            string strIncoveniente = "";
            string update, insert, observSql;
            double cantidadHorasModificadas = 0;
            double valorOperacion = 0;
            string excencion = "";
            string tipoLiquidacion = "";
            string fechaCumplimiento = "";
            string fechaActual = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            double tiempoCliente = 0;
            numDecimales = Convert.ToUInt16(ViewState["numDecimales"].ToString());

            for (i = 0; i < operaciones.Items.Count; i++)
            {
                double iva = Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
                excencion = DBFunctions.SingleData("SELECT ptem_exceiva FROM ptempario WHERE ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'");
                tipoLiquidacion = DBFunctions.SingleData("SELECT ttip_codiliqu FROM ptempario WHERE ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'");

                if (((Button)operaciones.Items[i].Cells[9].Controls[1]).Text == "Remover")
                {
                    //Primero debemos comprobar si la operacion existe dentro de la orden sino, debemos crearla dentro de dordenoperacion
                    if (DBFunctions.RecordExist("SELECT * FROM dordenoperacion WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "' AND ptem_operacion= '" + tablaOperaciones.Rows[i][0].ToString() + "'"))
                    {

                        //Aqui entra para empezar a hacer los cambios
                        update = "UPDATE dordenoperacion SET ";
                        insert = "INSERT INTO destadisticaoperacion VALUES(default,";

                        update += "tcar_cargo ='" + ((DropDownList)operaciones.Items[i].Cells[7].Controls[1]).SelectedValue + "', ";
                        //Se actualiza el codigo del vendedor
                        if (((DropDownList)operaciones.Items[i].Cells[2].Controls[1]).Items.Count == 0)
                            update += " pven_codigo=null ,";
                        else
                            update += " pven_codigo='" + ((DropDownList)operaciones.Items[i].Cells[2].Controls[1]).SelectedValue + "' ,";
                        //Actualizamos si es necesario el tiempo de liquidación
                        if (((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text != "")
                            update += " dord_tiemliqu=" + Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text).ToString() + " ,";
                        //Actualizamos el estado de la operacion
                        update += " test_estado='" + ((DropDownList)operaciones.Items[i].Cells[4].Controls[1]).SelectedValue + "' ,";
                        //Si el estado es cumplido actualizamos con la fecha de cumplimiento sino colocamos null
                        //if(((DropDownList)operaciones.Items[i].Cells[4].Controls[1]).SelectedValue == "C")

                        //DateTime fechaEventoTextBoxs = Convert.ToDateTime(((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text+" "+((TextBox)operaciones.Items[i].Cells[6].Controls[7]).Text);
                        update += " dord_fechcump='" + fechaActual + "' ,";
                        //else
                        //    update +=" dord_fechcump=NULL ,";
                        //Miramos si la operacion se liquida por bono y luego miramos si ese precio ha variado

                        tablaOperaciones.Rows[i][4] = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo ='" + ((DropDownList)operaciones.Items[i].Cells[7].Controls[1]).SelectedValue + "'");
                 
                        if (tipoLiquidacion == "B")
                        {
                            valorOperacion = Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[5].Controls[1]).Text);
                            tiempoCliente = Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                            valorOperacion = AproxOperacionEspecifica(valorOperacion);
                            update += " dord_valooper=" + valorOperacion.ToString() + " ,";
                        }
                        //Si la operacion tiene un valor por tiempo, se recalcula
                        else if (tipoLiquidacion == "T")
                        {
                            double valorHora = 0;

                            string cargoOperacion = tablaOperaciones.Rows[i][4].ToString().Trim();
                            valorHora = ObtenerValorHoraOperacion(cargoOperacion);
                            tiempoCliente = Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                            valorOperacion = Math.Round(valorHora * tiempoCliente,numDecimales);
                            

                            //if(tablaOperaciones.Rows[i][4].ToString().Trim()=="Gtía Fabrica")
                            //{
                            //    valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoragtia FROM ppreciotaller WHERE ppreta_codigo='" + DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'") + "'"));
                            //    tiempoCliente=Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                            //    valorOperacion=valorHora*tiempoCliente;
                            //}
                            //else if(tablaOperaciones.Rows[i][4].ToString().Trim()=="Cliente" || tablaOperaciones.Rows[i][4].ToString().Trim()=="Seguros")
                            //{
                            //    valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='" + DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'") + "'"));
                            //    tiempoCliente=Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                            //    valorOperacion=valorHora*tiempoCliente;
                            //}
                            //else if(tablaOperaciones.Rows[i][4].ToString().Trim()=="Alistamiento" || tablaOperaciones.Rows[i][4].ToString().Trim()=="Interno" || tablaOperaciones.Rows[i][4].ToString().Trim()=="Gtía Taller")
                            //{
                            //    valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohorainte FROM ppreciotaller WHERE ppreta_codigo='" + DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'") + "'"));
                            //    tiempoCliente=Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                            //    valorOperacion=valorHora*tiempoCliente;
                            //}
                            //else if(tablaOperaciones.Rows[i][4].ToString()=="Cortesía")
                            //{
                            //    valorOperacion=0;
                            //}

                            if (excencion == "S" || (DBFunctions.SingleData("SELECT CTAL_IVAOPERSINTER FROM ctaller") == "N" && (tablaOperaciones.Rows[i][4].ToString().Trim() == "Alistamiento" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Interno" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Taller"))
                                || (liqGarantiaFabrica == "N" && tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Fabrica"))
                            {
                                valorOperacion = AproxOperacionEspecifica(valorOperacion);
                                update += "dord_valooper=" + valorOperacion.ToString() + " ,";
                                iva = 0;
                            }
                            else
                            {
                                valorOperacion = AproxOperacionEspecifica(valorOperacion);
                                update += "dord_valooper=" + valorOperacion.ToString() + " ,";
                            }
                        }

                        //Si la operacion tiene un valor fijo, se recalcula si se ha modificado el tiempo
                        else if (tipoLiquidacion == "F")
                        {
                      //      if (tablaOperaciones.Rows[i][4].ToString().Trim() == "Cliente" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Fabrica")
                            {
                                double valorFijo = 0;
                                tiempoCliente = Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                                valorFijo = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'"));
                                valorOperacion = Math.Round(valorFijo * tiempoCliente,numDecimales);
                            }

                            if (excencion == "S" || (DBFunctions.SingleData("SELECT CTAL_IVAOPERSINTER FROM ctaller") == "N" && (tablaOperaciones.Rows[i][4].ToString().Trim() == "Alistamiento" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Interno"
                                || tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Taller")) || (liqGarantiaFabrica == "N" && tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Fabrica"))
                            {
                                iva = 0;
                //                valorOperacion = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'"));
                            }
                //            else
                            {
                                 valorOperacion = Math.Round((valorOperacion * 1), numDecimales); ;

                            }
                            valorOperacion = AproxOperacionEspecifica(valorOperacion);
                            update += "dord_valooper=" + valorOperacion.ToString() + " ,";
                        }

                        update += "piva_porciva =" + iva.ToString() + " ,";

                        double costoOper = 0.00;
                        try
                        {
                            costoOper = Convert.ToDouble(DBFunctions.SingleData("select round((coalesce(me.memp_sUELaCtu,COALESCE(PVEN_SALABASI,0))/240 + coalesce(pv.pven_valounidvent * "+ tiempoCliente.ToString() +",0) + (coalesce(pv.pven_porccomi,0)*0.01*"+ valorOperacion + ")) * 1.5,0) FROM dordenoperacion dp, PVENDEDOR PV left join  MEMPLEADO ME on PV.MEMP_CODIEMP = ME.MEMP_CODIEMPL WHERE pv.pven_codigo = dp.pven_codigo AND DP.PDOC_CODIGO = '" + tipoDocumento.SelectedValue + "' AND DP.MORD_NUMEORDE = " + ordenes.SelectedValue + " AND DP.PTEM_OPERACION = '" + tablaOperaciones.Rows[i][0].ToString() + "'; "));
                        }
                        catch
                        {
                            costoOper = 0.00;
                        }

                        if (costoOper.ToString() != "")
                            update += "dord_costoper =" + costoOper.ToString() + " ,";


                        if (tablaOperaciones.Rows[i][12].ToString().Trim() == "G")
                        {
                            update += "pgar_codigo1='" + ((DropDownList)operaciones.Items[i].Cells[8].FindControl("ddlCodigoIncidente")).SelectedValue + "' ,";
                            update += "pgar_codigo2='" + ((DropDownList)operaciones.Items[i].Cells[8].FindControl("ddlCausalGarantia")).SelectedValue + "' ,";
                            update += "pgar_codigo3='" + ((DropDownList)operaciones.Items[i].Cells[8].FindControl("ddlCodigoRemedio")).SelectedValue + "' ,";
                            update += "pgar_codigo4='" + ((DropDownList)operaciones.Items[i].Cells[8].FindControl("ddlCodigoDefecto")).SelectedValue + "' ,";
                        }
                        update = update.Substring(0, update.Length - 1) + " WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "' AND ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'";
                        sqlStrings.Add(update);
                        //Ahora miramos como realizar la insercion de datos dentro de destadisticaoperacion
                        string estadoOperacion = ((DropDownList)operaciones.Items[i].Cells[4].Controls[1]).SelectedValue;
                        if (!DBFunctions.RecordExist("SELECT * FROM destadisticaoperacion WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "' AND ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "' AND test_estado='" + estadoOperacion + "' AND destoper_hora='" + ((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text + " " + ((TextBox)operaciones.Items[i].Cells[6].Controls[7]).Text + "'"))
                        {
                            if (!DatasToControls.ValidarDateTime(((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text + " " + ((TextBox)operaciones.Items[i].Cells[6].Controls[7]).Text))
                            {
                                incoveniente = true;
                                strIncoveniente += "Existe problema en la fecha y hora del evento en la operación " + tablaOperaciones.Rows[i][1].ToString() + ", valor no valido.\\n";
                            }
                            else if (!ValidacionFechaRango(Convert.ToDateTime(((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text)))
                            {
                                    incoveniente = true;
                                    strIncoveniente += "La fecha del evento en la operación " + tablaOperaciones.Rows[i][1].ToString() + ", es un dia no valido.\\n";
                            }
                            else if (!ValidacionHoraRangoTaller(Convert.ToDateTime(((TextBox)operaciones.Items[i].Cells[6].FindControl("horaOpcion")).Text).TimeOfDay))
                            {
                                    incoveniente = true;
                                    strIncoveniente += "La hora del evento en la operación " + tablaOperaciones.Rows[i][1].ToString() + ", esta fuera del rango de atención del taller.\\n";
                            }
                            else
                            {
                                //Vamos a verificar que la fechas no sea menor que la que se va ingresar por medio de los campos de texto que se encuentran dentro de la Grilla operaciones
                                bool problemaLogicaFechas = false;
                                DataSet operacionCoinciden = new DataSet();
                                DBFunctions.Request(operacionCoinciden, IncludeSchema.NO, "SELECT destoper_hora FROM destadisticaoperacion WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "' AND ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "' ORDER BY destoper_indice ASC");
                                DateTime fechaHoraEventoTextBoxs = Convert.ToDateTime(((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text + " " + ((TextBox)operaciones.Items[i].Cells[6].Controls[7]).Text);
                                if (operacionCoinciden.Tables[0].Rows.Count > 0)
                                {
                                    int ultimoRegistro = operacionCoinciden.Tables[0].Rows.Count - 1;
                                    DateTime fechaHoraEventoAlmacenado = Convert.ToDateTime(operacionCoinciden.Tables[0].Rows[ultimoRegistro][0].ToString());
                                    int cmp = DateTime.Compare(fechaHoraEventoTextBoxs, fechaHoraEventoAlmacenado);
                                    if (cmp == -1)
                                        problemaLogicaFechas = true;
                                }
                                if (problemaLogicaFechas)
                                {
                                    incoveniente = true;
                                    strIncoveniente += "La fecha y hora del evento para la operación " + tablaOperaciones.Rows[i][1].ToString() + ", es menor al ultimo valor almacenado.\\n";
                                }
                                else
                                {
                                    if (tablaOperaciones.Rows[i][14].ToString() == "")
                                        tablaOperaciones.Rows[i][14] = DateTime.Now.ToString();
                                    DateTime fechaAnt = Convert.ToDateTime(tablaOperaciones.Rows[i][14].ToString());
                                    insert += "'" + tipoDocumento.SelectedValue + "'," + ordenes.SelectedValue + ",'" + tablaOperaciones.Rows[i][0].ToString() + "','" + ((DropDownList)operaciones.Items[i].Cells[2].Controls[1]).SelectedValue + "','" + ((DropDownList)operaciones.Items[i].Cells[4].Controls[1]).SelectedValue + "','" + fechaActual + "','" + tablaOperaciones.Rows[i][15].ToString() + "','" + fechaAnt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    sqlStrings.Add(insert);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Debemos sumar la cantidad de horas de esta operacion a la orden
                        cantidadHorasModificadas += Convert.ToDouble(tablaOperaciones.Rows[i][2].ToString());
                        //Fin suma de horas
                        //Si la operación es por bono
                 
                        if (tipoLiquidacion == "B")
                        {
                            try
                            {
                                if (excencion == "S" || (liqGarantiaFabrica == "N" && tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Fabrica"))
                                    iva = 0;
                                valorOperacion = Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[5].Controls[1]).Text);
                            }
                            catch
                            {
                                Utils.MostrarAlerta(Response, "Defina un valor para la operación");
                                return;
                            }
                        }
                        //Si la operacion tiene un valor por tiempo, se recalcula
                        else if (tipoLiquidacion == "T")
                        {
                            double valorHora = 0;

                            string cargoOperacion = tablaOperaciones.Rows[i][4].ToString().Trim();
                            valorHora = ObtenerValorHoraOperacion(cargoOperacion);
                            if (cargoOperacion == "Cliente" || cargoOperacion == "Seguros") //Condicion especial.
                            {
                                valorHora = (AproxOperacionEspecifica(valorHora * 1));
                            }
                            tiempoCliente = Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                            valorOperacion = Math.Round(valorHora * tiempoCliente,numDecimales);

                            //if(tablaOperaciones.Rows[i][4].ToString().Trim()=="Gtía Fabrica")
                            //{
                            //    valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoragtia FROM ppreciotaller WHERE ppreta_codigo='" + DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'") + "'"));
                            //    tiempoCliente=Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                            //    valorOperacion=valorHora*tiempoCliente;
                            //}
                            //else if(tablaOperaciones.Rows[i][4].ToString().Trim()=="Cliente" || tablaOperaciones.Rows[i][4].ToString().Trim()=="Seguros")
                            //{
                            //    valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='" + DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'") + "'"));
                            //    //valorHora llega con un valor sin iva, calculado a 4 decimales. A continuacion se realiza conversion para no perder decimales.
                            //    valorHora = (AproxOperacionEspecifica(valorHora*1.16)/1.16);
                            //    tiempoCliente=Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                            //    valorOperacion=valorHora*tiempoCliente;
                            //}
                            //else if(tablaOperaciones.Rows[i][4].ToString().Trim()=="Alistamiento" || tablaOperaciones.Rows[i][4].ToString().Trim()=="Interno" || tablaOperaciones.Rows[i][4].ToString().Trim()=="Gtía Taller")
                            //{
                            //    valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohorainte FROM ppreciotaller WHERE ppreta_codigo='" + DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'") + "'"));
                            //    tiempoCliente=Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                            //    valorOperacion=valorHora*tiempoCliente;
                            //}
                            //else if(tablaOperaciones.Rows[i][4].ToString()=="Cortesía")
                            //    valorOperacion=0;

                            if (excencion == "S" || (tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Fabrica" && liqGarantiaFabrica == "N") || (DBFunctions.SingleData("SELECT CTAL_IVAOPERSINTER FROM ctaller") == "N" && (tablaOperaciones.Rows[i][4].ToString().Trim() == "Alistamiento" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Interno" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Taller")))
                            {
                                valorOperacion = valorOperacion * 1;
                                iva = 0;
                            }
                        }
                        // si la operacion tiene un valor fijo...
                        else if (tipoLiquidacion == "F")
                        {

                            if (tablaOperaciones.Rows[i][4].ToString().Trim() != "x Cortesia")
                            {
                                double valorFijo = 0;
                                tiempoCliente = Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text);
                                valorFijo = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'"));
                                valorOperacion = Math.Round(valorFijo * tiempoCliente,numDecimales);
                            }
                            else
                                valorOperacion = 0; // Las cortesias no se cobran

                            if (excencion == "S" || (tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Fabrica" && liqGarantiaFabrica == "N") || (DBFunctions.SingleData("SELECT CTAL_IVAOPERSINTER FROM ctaller") == "N" && (tablaOperaciones.Rows[i][4].ToString().Trim() == "Alistamiento" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Interno" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Taller")))
                            {
                                iva = 0;
                               // valorOperacion = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'"));
                            }
                        }

                        //Aqui es donde debemos crear la operacion nueva que no se encuentra dentro de dordenoperacion
                        string vendedor = "";
                        if (((DropDownList)operaciones.Items[i].Cells[2].Controls[1]).Items.Count == 0)
                            vendedor = "null";
                        else
                            vendedor = "'" + ((DropDownList)operaciones.Items[i].Cells[2].Controls[1]).SelectedValue + "'";
                        //if(((DropDownList)operaciones.Items[i].Cells[4].Controls[1]).SelectedValue == "C")
                        //DateTime fechaAct = Convert.ToDateTime(((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text);
                        //fechaCumplimiento = "'" + fechaAct.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        //else
                        //fechaCumplimiento="NULL";
                        string insertNuevo = "";
                        //valorOperacion = Math.Round(valorOperacion*1, 0);
                        valorOperacion = Math.Round(valorOperacion, numDecimales);
                        valorOperacion = AproxOperacionEspecifica(valorOperacion);

                        double costoOper = 0.00;
                        try
                        {
                            costoOper = Convert.ToDouble(DBFunctions.SingleData("select round((coalesce(me.memp_sUELaCtu,COALESCE(PVEN_SALABASI,0))/240 + coalesce(pv.pven_valounidvent * " + tiempoCliente.ToString() + ",0) + (coalesce(pv.pven_porccomi,0)*0.01*" + valorOperacion + ")) * 1.5,0) FROM dordenoperacion dp, PVENDEDOR PV left join  MEMPLEADO ME on PV.MEMP_CODIEMP = ME.MEMP_CODIEMPL WHERE pv.pven_codigo = dp.pven_codigo AND DP.PDOC_CODIGO = '" + tipoDocumento.SelectedValue + "' AND DP.MORD_NUMEORDE = " + ordenes.SelectedValue + " AND DP.PTEM_OPERACION = '" + tablaOperaciones.Rows[i][0].ToString() + "'; "));
                        }
                        catch
                        {
                            costoOper = 0.00;
                        }

                        if (tablaOperaciones.Rows[i][12].ToString().Trim() == "G")
                            insertNuevo = "INSERT INTO dordenoperacion VALUES('" + tipoDocumento.SelectedValue + "'," + ordenes.SelectedValue + ",'" + tablaOperaciones.Rows[i][0].ToString() + "','" + DBFunctions.SingleData("SELECT tcar_cargo FROM tcargoorden WHERE tcar_nombre='" + tablaOperaciones.Rows[i][4].ToString() + "'") + "'," + vendedor + ",'" + fechaActual + "','" + ((DropDownList)operaciones.Items[i].Cells[4].Controls[1]).SelectedValue + "','" + ((DropDownList)operaciones.Items[i].Cells[8].FindControl("ddlCodigoIncidente")).SelectedValue + "','" + ((DropDownList)operaciones.Items[i].Cells[8].FindControl("ddlCausalGarantia")).SelectedValue + "','" + ((DropDownList)operaciones.Items[i].Cells[8].FindControl("ddlCodigoRemedio")).SelectedValue + "','" + ((DropDownList)operaciones.Items[i].Cells[8].FindControl("ddlCodigoDefecto")).SelectedValue + "'," + ((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text + "," + valorOperacion.ToString() + ",null,null,"+ costoOper +",'" + iva + "')";
                        //				pdoc_codigo						            mord_numeorde					ptem_operacion															tcar_cargo																				                 pven_codigo		dord_fechcump								test_estado														pgar_codigo1																								2																					3																						4														dord_tiemliqu										dord_valooper			  dord_fechliqu,dord_tiempooper
                        else
                            insertNuevo = "INSERT INTO dordenoperacion VALUES('" + tipoDocumento.SelectedValue + "'," + ordenes.SelectedValue + ",'" + tablaOperaciones.Rows[i][0].ToString() + "','" + DBFunctions.SingleData("SELECT tcar_cargo FROM tcargoorden WHERE tcar_nombre='" + tablaOperaciones.Rows[i][4].ToString() + "'") + "'," + vendedor + ",'" + fechaActual + "','" + ((DropDownList)operaciones.Items[i].Cells[4].Controls[1]).SelectedValue + "',null,null,null,null," + ((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text + "," + valorOperacion.ToString() + ",null,null, " + costoOper + ",'" + iva + "')";
                        //				pdoc_codigo						mord_numeorde					        ptem_operacion															tcar_cargo																					                 pven_codigo		dord_fechcump								test_estado										            pgar_codigo1 2 3 4					dord_tiemliqu										dord_valooper			  dord_fechliqu,dord_tiempooper
                        sqlStrings.Add(insertNuevo);

                        //comentado por que hasta ahora se crea la operacion y no hay un valor de estado ni fecha anteriores.
                        //DateTime fechaAnt = Convert.ToDateTime(tablaOperaciones.Rows[i][14].ToString());
                        //string estadoAnt = tablaOperaciones.Rows[i][15].ToString();
                        //sqlStrings.Add("INSERT INTO destadisticaoperacion VALUES(default,'" + tipoDocumento.SelectedValue + "'," + ordenes.SelectedValue + ",'" + tablaOperaciones.Rows[i][0] + "'," + vendedor + ",'" + ((DropDownList)operaciones.Items[i].Cells[4].Controls[1]).SelectedValue + "','" + fechaActual + "','" + estadoAnt + "','" + fechaAnt.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                        if (!DatasToControls.ValidarDateTime(((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text + " " + ((TextBox)operaciones.Items[i].Cells[6].Controls[7]).Text))
                        {
                            incoveniente = true;
                            strIncoveniente += "Existe problema en la fecha y hora del evento en la operación " + tablaOperaciones.Rows[i][1].ToString() + ", valor no valido.\\n";
                        }
                        else if (!ValidacionFechaRango(Convert.ToDateTime(((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text)))
                        {
                            incoveniente = true;
                            strIncoveniente += "La fecha del evento en la operación " + tablaOperaciones.Rows[i][1].ToString() + ", es un dia no valido.\\n";
                        }
                        else if (!ValidacionHoraRangoTaller(Convert.ToDateTime(((TextBox)operaciones.Items[i].Cells[6].FindControl("horaOpcion")).Text).TimeOfDay))
                        {
                            incoveniente = true;
                            strIncoveniente += "La hora del evento en la operación " + tablaOperaciones.Rows[i][1].ToString() + ", esta fuera del rango de atención del taller.\\n";
                        }
                    }

                    //Almacenamiento de observaciones de operaciones
                    string observaciones = ((TextBox)operaciones.Items[i].Cells[1].FindControl("txtObservaciones")).Text;
                    if (observaciones != "")
                    {
                        if (DBFunctions.RecordExist("select * from dbxschema.dobservacionesot where pdoc_codigo='" + tipoDocumento.SelectedValue + "' and mord_numeorde=" + ordenes.SelectedValue + " and ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "';"))
                        {
                            observSql = String.Format("UPDATE dobservacionesot SET DOBSOT_OBSERVACIONES='" + observaciones + "', DOBSOT_PROCESADO='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE pdoc_codigo='" + tipoDocumento.SelectedValue + "' and mord_numeorde=" + ordenes.SelectedValue + " and ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "';");
                        }
                        else
                        {
                            observSql = "INSERT INTO dobservacionesot VALUES(default,'" + tipoDocumento.SelectedValue + "'," + ordenes.SelectedValue + ",'" + tablaOperaciones.Rows[i][0].ToString() + "','" + observaciones + "','" + ((DropDownList)operaciones.Items[i].Cells[2].Controls[1]).SelectedValue + "','" + ((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
                        }
                        sqlStrings.Add(observSql);
                    }
                    if (excencion == "S" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Alistamiento" || tablaOperaciones.Rows[i][4].ToString().Trim() == "Interno"
                       || tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Taller" || (liqGarantiaFabrica == "N" && tablaOperaciones.Rows[i][4].ToString().Trim() == "Gtía Fabrica"))
                        iva = 0;  // Las liquidaciones de Garantia de Fabrica, internos, alistamientos y garantias Taller NO lelvan IVA porque la mano de Obra se paga por la nomina
                }
                else if (((Button)operaciones.Items[i].Cells[9].Controls[1]).Text == "Restaurar")
                {
                    elementosBorrados += tablaOperaciones.Rows[i][0].ToString() + "-";
                    //Primero Debemos Comprobar que esta operacion exista dentro de dordenoperacion para poder borrarlo
                    if (DBFunctions.RecordExist("SELECT * FROM dordenoperacion WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "' AND ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'"))
                    {
                        string delete = "DELETE FROM dordenoperacion WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "' AND ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'";
                        sqlStrings.Add(delete);
                        delete = "DELETE FROM dobservacionesot WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "' AND ptem_operacion='" + tablaOperaciones.Rows[i][0].ToString() + "'";
                        sqlStrings.Add(delete);
                        cantidadHorasModificadas -= Convert.ToDouble(tablaOperaciones.Rows[i][2].ToString());
                    }
                    //También debemos comprobar de que exista dentro de DTERCEROSTALLER para borrarla
                    if(DBFunctions.RecordExist("SELECT DTER_SECUENCIA FROM DTERCEROSTALLER WHERE PDOC_CODIGO = '" + tipoDocumento.SelectedValue + "' AND MORD_NUMEORDE = " + ordenes.SelectedValue + " AND PTEM_OPERACION = '" + tablaOperaciones.Rows[i][0].ToString() + "' "))
                    {
                        string borrarTercero = "DELETE FROM DTERCEROSTALLER WHERE PDOC_CODIGO = '" + tipoDocumento.SelectedValue + "' AND MORD_NUMEORDE = " + ordenes.SelectedValue + " AND PTEM_OPERACION = '" + tablaOperaciones.Rows[i][0].ToString() + "'";
                        sqlStrings.Add(borrarTercero);
                    }
                }
            }

            //Aqui vamos a realizar la actualizacion en la base de datos	
            if (!incoveniente)
            {
                DateTime horaFechaEntrega = Convert.ToDateTime(DBFunctions.SingleData("SELECT mord_entregar FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'"));
                horaFechaEntrega = Convert.ToDateTime(horaFechaEntrega.Date.ToString("yyyy-MM-dd") + " " + DBFunctions.SingleData("SELECT mord_horaentg FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'"));
                DateTime nuevaFechaEntrega;
                if (cantidadHorasModificadas > 0)
                {
                    nuevaFechaEntrega = Adicionar_Horas(cantidadHorasModificadas, horaFechaEntrega);
                    sqlStrings.Add("UPDATE morden SET mord_entregar='" + nuevaFechaEntrega.Date.ToString("yyyy-MM-dd") + "', mord_horaentg='" + nuevaFechaEntrega.TimeOfDay.ToString() + "' WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'");
                }
                else if (cantidadHorasModificadas < 0)
                {
                    nuevaFechaEntrega = Remover_Horas(cantidadHorasModificadas, horaFechaEntrega);
                    sqlStrings.Add("UPDATE morden SET mord_entregar='" + nuevaFechaEntrega.Date.ToString("yyyy-MM-dd") + "', mord_horaentg='" + nuevaFechaEntrega.TimeOfDay.ToString() + "' WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'");
                }
                //Cambio hecho el 2005-05-20
                //Realizamos la actualizacion del estado de la liquidacion de la orden a estado A(mord_estaliqu)
                sqlStrings.Add("UPDATE morden SET mord_estaliqu='A' WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'");
                if (DBFunctions.Transaction(sqlStrings))
                {
                    if(tTercero == "S")
                    {
                        divOperaciones.Visible = false;
                        //Utils.MostrarAlerta(Response, "Se han guardado los cambios");
                    }
                    else if (tipoProceso == "T")
                        Response.Redirect("" + indexPage + "?process=Automotriz.TorreControl&actor=T");
                    else
                        Response.Redirect("" + indexPage + "?process=Automotriz.PreliquidacionOT&pref=" + tipoDocumento.SelectedValue + "&num=" + ordenes.SelectedValue + "");
                }
                else
                    lb.Text = "Error: " + DBFunctions.exceptions + "<br><br>";
            }
            else
            {
                Utils.MostrarAlerta(Response, "" + strIncoveniente + "");
                return;
            }

            
            //falta preguntar si ctaller esta en si o no
            if (tTercero == "S")
            {
                for (int z = 0; z < tablaOperaciones.Rows.Count; z++)
                {
                    if(!elementosBorrados.Contains(tablaOperaciones.Rows[z][0].ToString()))
                    {
                        string especialidad = DBFunctions.SingleData("SELECT tope_codigo FROM ptempario WHERE ptem_operacion='" + tablaOperaciones.Rows[z][0].ToString() + "'");
                        bool existencia = DBFunctions.RecordExist("SELECT DTER_SECUENCIA FROM DTERCEROSTALLER WHERE PDOC_CODIGO = '" + tipoDocumento.SelectedValue + "' AND MORD_NUMEORDE = " + ordenes.SelectedValue + " AND PTEM_OPERACION = '" + tablaOperaciones.Rows[z][0].ToString() + "' ");
                        if (especialidad.Trim() == "TER" && !existencia)
                        {
                            DataRow drow = miTabla.NewRow();
                            drow[0] = tipoDocumento.SelectedValue.ToString();
                            drow[1] = numeroOrdenSeleccionada;
                            drow[2] = tablaOperaciones.Rows[z][0].ToString();
                            drow[3] = tablaOperaciones.Rows[z][1].ToString();
                            miTabla.Rows.Add(drow);
                        }
                    }
                }
                cargar.Enabled = false;
                if(miTabla.Rows.Count != 0)
                {
                    gridTerceros.DataSource = miTabla;
                    gridTerceros.DataBind();
                    for (int z = 0; z < gridTerceros.Items.Count; z++)
                    {
                        ((TextBox)gridTerceros.Items[z].Cells[4].Controls[1]).Attributes.Add("onClick", "ModalDialog(this,'select distinct mp.mnit_nit, vm.nombre from mproveedor mp, vmnit vm where mp.mnit_nit = vm.mnit_nit;', new Array(),1)");

                    }
                    Session["tblTerceros"] = miTabla;
                    divCosto.Visible = true;
                }
                else
                {
                    cancelarTerceros(Sender, e);
                }
                
            }
        }
    

        protected double ObtenerValorHoraOperacion(string cargoOperacion)
        {
            double valorHora = 0;

            switch (cargoOperacion)
            {
                case "Gtía Fabrica":
                    valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoragtia FROM ppreciotaller WHERE ppreta_codigo='" + DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'") + "'"));
                    break;
                case "Cliente":
                case "Seguros":
                    valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='" + DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'") + "'"));
                    break;
                case "Alistamiento":
                case "Interno":
                case "Gtía Taller":
                    valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohorainte FROM ppreciotaller WHERE ppreta_codigo='" + DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE mord_numeorde=" + ordenes.SelectedValue + " AND pdoc_codigo='" + tipoDocumento.SelectedValue + "'") + "'"));
                    break;
                case "Cortesía":
                    valorHora = 0;
                    break;
            }
            return valorHora;
        }

        protected double AproxOperacionEspecifica(double valorOperacion)
        {
            string aproximacionTotal = valorOperacion.ToString();
            if (aproximacionTotal.Contains(".999"))
                valorOperacion = Math.Round(valorOperacion, 0);

            return valorOperacion;
        }

		protected void Cancelar_Torre(Object  Sender, EventArgs e)
        {

            if (tipoProceso == "T")
                Response.Redirect("" + indexPage + "?process=Automotriz.TorreControl&actor=T"); 
            else
                Response.Redirect("" + indexPage + "?process=Automotriz.TorreControl");
		}
      
        protected void dgOperacionesBound(object sender, DataGridItemEventArgs e)
        {
            

            if (e.Item.ItemType == ListItemType.Footer || e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[7].Controls[1]), "SELECT '',' Seleccione:..' FROM tcargoorden UNION SELECT tcar_cargo,tcar_nombre FROM tcargoorden ");
            }

            if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    ((System.Web.UI.WebControls.Image)e.Item.Cells[2].FindControl("imglupa1")).Attributes.Add("onClick", "ModalDialog(" + ((DropDownList)e.Item.Cells[2].FindControl("mecanicoE")).ClientID + ",'select pven_codigo, pven_nombre from pvendedor where tvend_codigo = \\'MG\\' AND PVEN_VIGENCIA = \\'V\\'', new Array());");
                }
                catch { }
            }
            
                    
        }

		protected void dgOpcion_Operaciones(Object sender, DataGridCommandEventArgs e) 
		{
			if(((Button)e.CommandSource).Text == "Remover")
				((Button)e.CommandSource).Text = "Restaurar";
			else if(((Button)e.CommandSource).Text == "Restaurar")
				    ((Button)e.CommandSource).Text = "Remover";
			if(((Button)e.CommandSource).CommandName == "AddDatasRow") 
			{
                string cargoPrinOT = DBFunctions.SingleData("SELECT tcar_cargo FROM morden WHERE pdoc_codigo='" + tipoDocumento.SelectedValue + "' AND mord_numeorde=" + ordenes.SelectedValue + "");
				if(!RevisionCargo(cargoPrinOT,((DropDownList)e.Item.Cells[7].Controls[1]).SelectedValue))
				{
                    Utils.MostrarAlerta(Response, "El cargo " + ((DropDownList)e.Item.Cells[7].Controls[1]).SelectedItem.Text.Trim() + " no es valido con el cargo de la orden de trabajo " + tipoDocumento.SelectedValue + "-" + ordenes.SelectedValue + " : " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + cargoPrinOT + "'").Trim() + ".\\ Revise Por Favor!");
					return;
				}
                string cargoFacturado = DBFunctions.SingleData("SELECT tcar_cargo FROM mfacturaclientetaller WHERE pdoc_prefordetrab='" + tipoDocumento.SelectedValue + "' AND mord_numeorde=" + ordenes.SelectedValue + " and tcar_cargo = '" + (((DropDownList)e.Item.Cells[7].Controls[1]).SelectedValue) + "' ");
                if(!RevisionCargoFacturado(cargoFacturado,((DropDownList)e.Item.Cells[7].Controls[1]).SelectedValue))
				{
                    Utils.MostrarAlerta(Response, "El cargo " + ((DropDownList)e.Item.Cells[7].Controls[1]).SelectedItem.Text.Trim() + " no es valido para la orden de trabajo " + tipoDocumento.SelectedValue + "-" + ordenes.SelectedValue + " porque ya fue FACTURADO en esta ORDEN. Revise Por Favor!");
					return;
				}
                string operacion = ((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim();
		        string operacionFacturada = DBFunctions.SingleData("SELECT ptem_operacion FROM dordenoperacion d, mfacturaclientetaller mf WHERE d.pdoc_codigo ='" + tipoDocumento.SelectedValue + "' AND d.mord_numeorde=" + ordenes.SelectedValue + " and d.pdoc_codigo = mf.pdoc_prefordetrab and d.mord_numeorde = mf.mord_Numeorde and d.ptem_operacion = '" + operacion +"' ");
                if (operacionFacturada.Length > 0)
                {
                    Utils.MostrarAlerta(Response, "El código de operación " + operacion + " no es válido porque ya fue facturado en esta Orden. Elija otra operación.  Revise Por Favor !");
                    return;
                }
                if ((tablaOperaciones.Select("CODIGOOPERACION='" + ((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim() + "'")).Length == 0)
				{
					string errors = "";
					Actualizar_Grilla();
					DataRow fila = tablaOperaciones.NewRow();
					fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim();
					fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim();
					try{fila[2] = Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo.Text.Trim()+"' AND ptie_tempario='"+((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim()+"'"));}
					catch{fila[2] = Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(ptem_tiempoestandar,0) FROM ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim()+"'"));}
					//vamos a calcular el valor de esta operacion
					string tipoLiquidacion = DBFunctions.SingleData("SELECT ttip_codiliqu FROM ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim()+"'");
					string excencion = DBFunctions.SingleData("SELECT ptem_exceiva FROM ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"'");
					double valorOperacion = 0;
					if(tipoLiquidacion == "F")
					{
						if(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"'") == String.Empty)
							errors = "- Esta operación no tiene configurado un valor en el tempario\\n";
						else
					//revisar para que valide el precio  de internos y no lo cargue!!!!!!		
                            valorOperacion = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"'"));	
				    }
					else if(tipoLiquidacion == "T")
					{
						double valorHora=0,tiempoCliente=0;
						string grupo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_vin='"+DBFunctions.SingleData("SELECT mcat_vin FROM morden WHERE mord_numeorde="+ordenes.SelectedValue+" AND pdoc_codigo='"+tipoDocumento.SelectedValue+"'")+"')");

                        string cargoOperacion = ((DropDownList)e.Item.Cells[7].Controls[1]).SelectedItem.Text.Trim();
                        valorHora = ObtenerValorHoraOperacion(cargoOperacion);
                        //valorHora=Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='"+DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE pdoc_codigo='"+tipoDocumento.SelectedValue+"' AND mord_numeorde="+ordenes.SelectedValue+"")+"'"));
						
                        try{tiempoCliente=Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupo+"' AND ptie_tempario='"+((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim()+"'"));}
						catch
						{
							if(Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM dbxschema.ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim()+"'"))==0)
								tiempoCliente=1;
							else
								tiempoCliente=Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM dbxschema.ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim()+"'"));
						}
						valorOperacion = Math.Round(valorHora *tiempoCliente,numDecimales);
                    }
					 
					fila[3] = Math.Round(valorOperacion, numDecimales);
					//////////////////////////////////////////////////////////////////////////////////////////
					fila[4] = ((DropDownList)e.Item.Cells[7].Controls[1]).SelectedItem.Text;
					fila[6] = DBFunctions.SingleData("SELECT test_nombre FROM testadooperacion WHERE test_estaoper='A'");
                    fila[12] = ((DropDownList)e.Item.Cells[7].Controls[1]).SelectedValue;
                    fila[13] = ((TextBox)e.Item.Cells[1].Controls[3]).Text.Trim();
                    if(errors != String.Empty)
                        Utils.MostrarAlerta(Response, ""+errors+"-Operación No Agregada");
					else
						tablaOperaciones.Rows.Add(fila);
					
                    operaciones.DataSource = tablaOperaciones;
                    operaciones.DataBind();
                    Session["tablaOperaciones"] = tablaOperaciones;
                    ValidarGrillaOperaciones(ordenes.SelectedValue, tipoDocumento.SelectedValue);
                    //operaciones.DataBind();
                }
				else
                    Utils.MostrarAlerta(Response, "La Operación " + ((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim() + " ya se encuentra registrada.\\nRevise Por Favor!");
			}
		}

		private void operaciones_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if(tablaOperaciones.Rows[e.Item.DataSetIndex][12].ToString() != "G")
					((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoIncidente")).Visible = ((DropDownList)e.Item.Cells[8].FindControl("ddlCausalGarantia")).Visible = ((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoRemedio")).Visible = ((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoDefecto")).Visible = false;
				else
                {
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoIncidente")), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'I' ORDER BY pgar_codigo");
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[8].FindControl("ddlCausalGarantia")), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'C' ORDER BY pgar_codigo");
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoRemedio")), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'R' ORDER BY pgar_codigo");
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoDefecto")), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'D' ORDER BY pgar_codigo");
                    ((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoIncidente")).Visible = ((DropDownList)e.Item.Cells[8].FindControl("ddlCausalGarantia")).Visible = ((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoRemedio")).Visible = ((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoDefecto")).Visible = true;
                }
                if (tablaOperaciones.Rows[e.Item.DataSetIndex][8].ToString() != "0" && tablaOperaciones.Rows[e.Item.DataSetIndex][8].ToString() != string.Empty)((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoIncidente")).SelectedValue = tablaOperaciones.Rows[e.Item.DataSetIndex][8].ToString().Trim();
				if(tablaOperaciones.Rows[e.Item.DataSetIndex][9].ToString() != "0" && tablaOperaciones.Rows[e.Item.DataSetIndex][9].ToString() != string.Empty)((DropDownList)e.Item.Cells[8].FindControl("ddlCausalGarantia")).SelectedValue = tablaOperaciones.Rows[e.Item.DataSetIndex][9].ToString().Trim();
				if(tablaOperaciones.Rows[e.Item.DataSetIndex][10].ToString() != "0" && tablaOperaciones.Rows[e.Item.DataSetIndex][10].ToString() != string.Empty)((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoRemedio")).SelectedValue = tablaOperaciones.Rows[e.Item.DataSetIndex][10].ToString().Trim();
				if(tablaOperaciones.Rows[e.Item.DataSetIndex][11].ToString() != "0" && tablaOperaciones.Rows[e.Item.DataSetIndex][11].ToString() != string.Empty)((DropDownList)e.Item.Cells[8].FindControl("ddlCodigoDefecto")).SelectedValue = tablaOperaciones.Rows[e.Item.DataSetIndex][11].ToString().Trim();
			}
		}

		#endregion
		
		#region Metodos
		protected void PrepararTablaOperaciones()
		{
			tablaOperaciones = new DataTable();
			tablaOperaciones.Columns.Add(new DataColumn("CODIGOOPERACION",System.Type.GetType("System.String")));//0
			tablaOperaciones.Columns.Add(new DataColumn("DESCRIPCIONOPERACION",System.Type.GetType("System.String")));//1
			tablaOperaciones.Columns.Add(new DataColumn("TIEMPO",System.Type.GetType("System.Double")));//2
			tablaOperaciones.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.Double")));//3
			tablaOperaciones.Columns.Add(new DataColumn("CARGO",System.Type.GetType("System.String")));//4
			tablaOperaciones.Columns.Add(new DataColumn("TIEMPOLIQUIDACION",System.Type.GetType("System.String")));//5
			tablaOperaciones.Columns.Add(new DataColumn("ESTADOOPERACION",System.Type.GetType("System.String")));//6
			tablaOperaciones.Columns.Add(new DataColumn("CODIGOVENDEDOR",System.Type.GetType("System.String")));//7
			tablaOperaciones.Columns.Add(new DataColumn("CODIGOINCIDENTE",System.Type.GetType("System.String")));//8
			tablaOperaciones.Columns.Add(new DataColumn("CAUSALGARANTIA",System.Type.GetType("System.String")));//9
			tablaOperaciones.Columns.Add(new DataColumn("CODIGOREMEDIO",System.Type.GetType("System.String")));//10
			tablaOperaciones.Columns.Add(new DataColumn("CODIGODEFECTO",System.Type.GetType("System.String")));//11
			tablaOperaciones.Columns.Add(new DataColumn("CODIGOCARGO",System.Type.GetType("System.String")));//12
            tablaOperaciones.Columns.Add(new DataColumn("OBSERVACIONOP", System.Type.GetType("System.String")));//13
            tablaOperaciones.Columns.Add(new DataColumn("FECHAOPERACION", System.Type.GetType("System.String")));    //14
            tablaOperaciones.Columns.Add(new DataColumn("CODIGOESTADO", System.Type.GetType("System.String"))); //15
		}
		
		protected void CargarGrilla()
		{
			PrepararTablaOperaciones();
			DatasToControls bind = new DatasToControls();
			DataSet operacionesOrdenSeleccionada = new DataSet();
			Hashtable infoOrdenSeleccionada = new Hashtable();

            string sqlInfoOrden = String.Format(
             @"SELECT pc.pgru_grupo || ' Placa: ' || COALESCE(MCAT_PLACA, 'Error') || '  '||PCAT_DESCRIPCION AS GRUPO,  
              		  mo.ttra_codigo AS TIPOTRABAJO  
              FROM morden mo   
                LEFT JOIN mcatalogovehiculo mc   
                	LEFT JOIN pcatalogovehiculo pc ON pc.pcat_codigo = mc.pcat_codigo   
                ON mc.mcat_vin = mo.mcat_vin  
              WHERE mord_numeorde = {0}  
              AND   pdoc_codigo = '{1}'"
             , ordenes.SelectedValue
             , tipoDocumento.SelectedValue);
            infoOrdenSeleccionada = (Hashtable) DBFunctions.RequestAsCollection(sqlInfoOrden)[0];

            string grupo = infoOrdenSeleccionada["GRUPO"].ToString();
            string tipoTrabajo = infoOrdenSeleccionada["TIPOTRABAJO"].ToString();
            grupoCatalogo.Text = grupo;
            plhGrpCata.Visible = true;
            double iva=Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));

            string sqlopers = String.Format(
             @"SELECT d.ptem_operacion, 
                    d.pven_codigo, 
                    d.test_estado, 
                    d.dord_valooper, 
                    d.tcar_cargo,
                    d.dord_tiemliqu, 
                    d.test_estado, 
                    d.pgar_codigo1, 
                    d.pgar_codigo2, 
                    d.pgar_codigo3, 
                    d.pgar_codigo4, 
                    pt.ptem_exceiva, 
                    pt.ptem_descripcion, 
                    pt.ptem_tiempoestandar, 
                    tc.tcar_nombre, 
                    te.test_nombre, 
                    dob.DOBSOT_OBSERVACIONES, 
                    d.DORD_FECHCUMP 
             FROM dordenoperacion d  
               LEFT JOIN tcargoorden tc ON tc.tcar_cargo = d.tcar_cargo  
               LEFT JOIN testadooperacion te ON te.test_estaoper = d.test_estado 
               INNER JOIN ptempario pt ON d.ptem_operacion = pt.ptem_operacion 
               LEFT JOIN dobservacionesot dob ON (dob.pdoc_codigo = d.pdoc_codigo AND dob.mord_numeorde = d.mord_numeorde AND dob.ptem_operacion = d.ptem_operacion)  
             WHERE d.mord_numeorde = {0} 
             AND   d.pdoc_codigo = '{1}' 
             AND   d.tcar_cargo NOT IN (SELECT tcar_cargo 
                                      FROM mfacturaclientetaller 
                                      WHERE mord_numeorde = {0} 
                                      AND   pdoc_prefordetrab = '{1}')"
             , ordenes.SelectedValue
             , tipoDocumento.SelectedValue);
            DBFunctions.Request(operacionesOrdenSeleccionada,IncludeSchema.NO,sqlopers);

            foreach (DataRow operacion in operacionesOrdenSeleccionada.Tables[0].Rows)
            {
                DataRow nuevaFila = tablaOperaciones.NewRow();
                nuevaFila[0] = operacion[0];  //codigo de la operacion
                nuevaFila[1] = operacion[12]; // descripcion de la operacion

                if (tipoTrabajo == "P")
                    nuevaFila[2] = 0;
                else
                {
                    try 
                    { 
                        nuevaFila[2] = Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata= '" + grupo + "' AND ptie_tempario= '" + operacion[0].ToString() + "'")); 
                    }
                    catch 
                    { 
                        nuevaFila[2] = operacion[13]; // tiempo estándar
                    }
                }

                if (operacion[11].ToString() == "S" ||  //excento de iva
                    operacion[4].ToString() == "A" ||   // cargo Alistamiento
                    operacion[4].ToString() == "I" ||   // cargo Interno
                    operacion[4].ToString() == "T" ||   // cargo Garantía Taller
                    (liqGarantiaFabrica == "N" && operacion[4].ToString() == "G")) // cuando las garantias fca se liquidan sin iva                                                                Interno                                                         Gtia Taller               

                    nuevaFila[3] = Convert.ToDouble(operacion[3]); //valor de la operación
                else
                    nuevaFila[3] = Convert.ToDouble(operacion[3]); //valor de la operación

                nuevaFila[4] = operacion[14].ToString(); // nombre del cargo
                nuevaFila[5] = operacion[5].ToString(); // tiempo de liquidación
                nuevaFila[6] = operacion[15].ToString(); // nombre del estado
                nuevaFila[7] = operacion[1].ToString(); // codigo del vendedor
                nuevaFila[8] = operacion[7].ToString(); // garantia 1
                nuevaFila[9] = operacion[8].ToString(); // garantia 2
                nuevaFila[10] = operacion[9].ToString(); // garantia 3
                nuevaFila[11] = operacion[10].ToString(); // garantia 4
                nuevaFila[12] = operacion[4].ToString(); // codigo del cargo
                nuevaFila[13] = operacion[16].ToString(); //Observacion operacion
                nuevaFila[14] = operacion[17].ToString(); //Fecha operacion
                nuevaFila[15] = operacion[2].ToString(); //Codigo del estado

                tablaOperaciones.Rows.Add(nuevaFila);
            }

			operaciones.DataSource = tablaOperaciones;
			operaciones.DataBind();
			ValidarGrillaOperaciones(ordenes.SelectedValue,tipoDocumento.SelectedValue);
			Session["tablaOperaciones"] = tablaOperaciones;
		}
		
		protected void ValidarGrillaOperaciones(string numeroOrden, string prefijoDocumento)
		{
			int i;
			string codAlmacen = DBFunctions.SingleData("SELECT palm_almacen FROM morden WHERE pdoc_codigo='"+prefijoDocumento+"' AND mord_numeorde="+numeroOrden+"");
			DatasToControls bind = new DatasToControls();
			double porcIva=Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
            
            for (i=0;i<tablaOperaciones.Rows.Count;i++)
			{
				string especialidad = DBFunctions.SingleData("SELECT tope_codigo FROM ptempario WHERE ptem_operacion='"+tablaOperaciones.Rows[i][0].ToString()+"'");
               
                Multitaller = DBFunctions.SingleData("SELECT CTAL_TECNMULTI FROM CTALLER");
                string sqlMecanicos = "";
                if (Multitaller == "S")
                {
                    sqlMecanicos = @"SELECT PV.pven_codigo,
                                    PV.pven_nombre,
                                    COUNT(VMA.mord_numeorde)
                                    FROM pespecialidadmecanico PE,pvendedor PV 
                                      LEFT JOIN vtaller_mecanicoasignadoot VMA
                                        ON PV.pven_codigo = VMA.pven_codigo 
                                    WHERE PV.tvend_codigo in ('MG','TT')
                                     AND  PV.pven_vigencia = 'V'
                                     AND  PE.tope_codigo = '" + especialidad + @"' 
                                     AND  PE.pven_codigo = PV.pven_codigo  
                                    GROUP BY PV.pven_codigo,
                                    PV.pven_nombre
		                            HAVING COUNT(VMA.mord_numeorde) <(SELECT COALESCE(ctal_maxopermc,5)
                                    FROM ctaller)
                                    ORDER BY PVEN_NOMBRE";
                }

                else 
                    //if (Multitaller == "N")  // POR DEFECTo DEBE TOMAR N PORQUE SI NO PARMETRIZA SE REVIENTA....
                {
                    sqlMecanicos = @"SELECT PV.pven_codigo,  
                                     PV.pven_nombre,  
                                     COUNT(VMA.mord_numeorde)  
                              FROM pvendedor PV   
                                INNER JOIN pespecialidadmecanico PE ON PE.pven_codigo = PV.pven_codigo  
                                INNER JOIN pvendedoralmacen PVA ON PVA.pven_codigo = PV.pven_codigo  
                                LEFT JOIN vtaller_mecanicoasignadoot VMA ON PV.pven_codigo = VMA.pven_codigo  
                              WHERE PV.tvend_codigo in ('MG','TT')  
                               AND  PVA.palm_almacen = '" + codAlmacen + @"' 
                               AND  PE.tope_codigo = '" + especialidad + @"'  
                               AND  PV.pven_vigencia = 'V'   
                              GROUP BY PV.pven_codigo,  
                                       PV.pven_nombre  
                              HAVING COUNT(VMA.mord_numeorde) < (SELECT COALESCE(ctal_maxopermc,4) FROM ctaller)  ORDER BY PVEN_NOMBRE";
                }                                 
                
                if(tablaOperaciones.Rows[i][7].ToString() != "")
				{
					bind.PutDatasIntoDropDownList(((DropDownList)operaciones.Items[i].Cells[2].Controls[1]), sqlMecanicos);

					if(!DBFunctions.RecordExist("SELECT PV.pven_codigo, PV.pven_nombre, COUNT(VMA.mord_numeorde) FROM pvendedor PV INNER JOIN pespecialidadmecanico PE ON PE.pven_codigo = PV.pven_codigo INNER JOIN vtaller_mecanicoasignadoot VMA ON PV.pven_codigo = VMA.pven_codigo WHERE PV.tvend_codigo='MG' AND PV.palm_almacen='"+codAlmacen+"' AND PE.tope_codigo='"+especialidad+"' AND PV.pven_vigencia = 'V' AND PV.pven_codigo='"+tablaOperaciones.Rows[i][7]+"' GROUP BY PV.pven_codigo,PV.pven_nombre HAVING COUNT(VMA.mord_numeorde) < (SELECT COALESCE(ctal_maxopermc,4) FROM ctaller) "))
                        ((DropDownList)operaciones.Items[i].Cells[2].Controls[1]).Items.Add(new ListItem(DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo = '" + tablaOperaciones.Rows[i][7] + "' order by pven_nombre"), tablaOperaciones.Rows[i][7].ToString()));
					try{((DropDownList)operaciones.Items[i].Cells[2].Controls[1]).SelectedValue = tablaOperaciones.Rows[i][7].ToString();}catch{}
				}
				else
				{
					bind.PutDatasIntoDropDownList(((DropDownList)operaciones.Items[i].Cells[2].Controls[1]), sqlMecanicos);

					if(((DropDownList)operaciones.Items[i].Cells[2].Controls[1]).Items.Count == 0)
					{
                        Utils.MostrarAlerta(Response, "La operación " + DBFunctions.SingleData("SELECT ptem_descripcion FROM ptempario WHERE ptem_operacion='" + tablaOperaciones.Rows[i][0] + "'") + " no se le ha asignado tecnico, ya que ninguno cumple con la especialidad o todos los tecnicos especializados ya estan a su maxima capacidad.");
						bind.PutDatasIntoDropDownList(((DropDownList)operaciones.Items[i].Cells[4].Controls[1]),"SELECT test_estaoper, test_nombre FROM testadooperacion");
						((DropDownList)operaciones.Items[i].Cells[4].FindControl("estadoE")).SelectedValue = "S";
						((DropDownList)operaciones.Items[i].Cells[4].FindControl("estadoE")).Enabled = false;
					}
				}
            
                ((DropDownList)operaciones.Items[i].Cells[7].FindControl("ddlCargoOp")).SelectedValue =
                    DBFunctions.SingleData("SELECT tcar_cargo FROM tcargoorden WHERE tcar_nombre ='" + tablaOperaciones.Rows[i][4].ToString() + "'"); 

				//Cambio hecho para cargar el tiempo que duro la operacion
				try{((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text = Convert.ToDouble(tablaOperaciones.Rows[i][5]).ToString("N");}
				catch{((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text = ((DataBoundLiteralControl)operaciones.Items[i].Cells[3].Controls[2]).Text.Trim();}
				bind.PutDatasIntoDropDownList(((DropDownList)operaciones.Items[i].Cells[4].Controls[1]),"SELECT test_estaoper, test_nombre FROM testadooperacion");
				DatasToControls.EstablecerDefectoDropDownList(((DropDownList)operaciones.Items[i].Cells[4].Controls[1]),tablaOperaciones.Rows[i][6].ToString());
				//DatasToControls.EstablecerValueDropDownList(((DropDownList)operaciones.Items[i].Cells[4].Controls[1]),tablaOperaciones.Rows[i][6].ToString())
				if(DBFunctions.RecordExist("SELECT * FROM destadisticaoperacion WHERE mord_numeorde="+numeroOrden+" AND pdoc_codigo='"+prefijoDocumento+"' AND ptem_operacion='"+tablaOperaciones.Rows[i][0].ToString()+"'"))
				{
					DataSet operacionCoinciden = new DataSet();
					DBFunctions.Request(operacionCoinciden,IncludeSchema.NO, "SELECT destoper_hora FROM destadisticaoperacion WHERE mord_numeorde="+numeroOrden+" AND pdoc_codigo='"+prefijoDocumento+"' AND ptem_operacion='"+tablaOperaciones.Rows[i][0].ToString()+"' ORDER BY destoper_indice ASC");
					int ultimoRegistro = operacionCoinciden.Tables[0].Rows.Count - 1;
					DateTime fechaHoraEvento = Convert.ToDateTime(operacionCoinciden.Tables[0].Rows[ultimoRegistro][0].ToString());
					((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text = fechaHoraEvento.Date.ToString("yyyy-MM-dd");
					((TextBox)operaciones.Items[i].Cells[6].Controls[7]).Text = fechaHoraEvento.ToString("HH:mm:ss");
				}
				else
				{
					((TextBox)operaciones.Items[i].Cells[6].Controls[3]).Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
					((TextBox)operaciones.Items[i].Cells[6].Controls[7]).Text = DateTime.Now.ToString("HH:mm:ss");
				}
				/////////////////////////////////////////////////////////////////////////////////////////////////////
				string tipoLiquidacion = DBFunctions.SingleData("SELECT ttip_codiliqu FROM ptempario WHERE ptem_operacion='"+tablaOperaciones.Rows[i][0].ToString()+"'");
                if ((tipoLiquidacion == "B" || tablaOperaciones.Rows[i][7].ToString() == "Seguros") && (((TextBox)operaciones.Items[i].Cells[5].Controls[1]).Visible == false))
				{
					((TextBox)operaciones.Items[i].Cells[5].Controls[1]).Visible = true;
					((TextBox)operaciones.Items[i].Cells[5].Controls[1]).Text = (Convert.ToDouble(tablaOperaciones.Rows[i][3])/*/(1+(porcIva/100))*/).ToString();
				}
                else
                    ((TextBox)operaciones.Items[i].Cells[5].Controls[1]).Text = (Convert.ToDouble(tablaOperaciones.Rows[i][3])/*/(1+(porcIva/100))*/).ToString();

                ((TextBox)operaciones.Items[i].Cells[1].FindControl("txtObservaciones")).Text = tablaOperaciones.Rows[i][13].ToString();
			}
            //divCosto.Visible = true;
            //gridTerceros.DataSource = miTabla;
            //gridTerceros.DataBind();
            //Session["tblTerceros"] = miTabla;
        }

        protected void guadarTerceros(Object sender, EventArgs e)
        {
            
            ArrayList sqlTerceros = new ArrayList();
            
            for(int i = 0; i < gridTerceros.Items.Count; i++)
            {
                string codigoOp = miTabla.Rows[i][2].ToString();
                string nitProveedor = ((TextBox)gridTerceros.Items[i].Cells[4].Controls[1]).Text;
                string costoOp = ((TextBox)gridTerceros.Items[i].Cells[5].Controls[1]).Text;
                if(nitProveedor.Trim() == "" || costoOp.Trim() == "")
                {
                    Utils.MostrarAlerta(Response, "Debe llenar todos los campos!");
                    return;
                }
                sqlTerceros.Add(@"INSERT INTO DBXSCHEMA.DTERCEROSTALLER(PDOC_CODIGO, MORD_NUMEORDE, PTEM_OPERACION, MNIT_NIT, DORD_VALOOPER ) VALUES ('" + tipoDocumento.SelectedValue + "', " + numeroOrdenSeleccionada + ", " + "'" + codigoOp + "', " + "'" + nitProveedor + "', " + costoOp + ");");
            }
            if(DBFunctions.Transaction(sqlTerceros))
            {
                if (tipoProceso == "T")
                    Response.Redirect("" + indexPage + "?process=Automotriz.TorreControl&actor=T");
                else
                    Response.Redirect("" + indexPage + "?process=Automotriz.PreliquidacionOT&pref=" + tipoDocumento.SelectedValue + "&num=" + ordenes.SelectedValue + "");
            }
            else
            {
                //Utils.MostrarAlerta(Response, DBFunctions.exceptions.ToString());
                lbError.Text = DBFunctions.exceptions.ToString();
            }
        }

        protected void cancelarTerceros(Object sender, EventArgs e)
        {
            if (tipoProceso == "T")
                Response.Redirect("" + indexPage + "?process=Automotriz.TorreControl&actor=T");
            else
                Response.Redirect("" + indexPage + "?process=Automotriz.PreliquidacionOT&pref=" + tipoDocumento.SelectedValue + "&num=" + ordenes.SelectedValue + "");
        }


        protected void Actualizar_Grilla()
		{
			for(int i=0;i<tablaOperaciones.Rows.Count;i++)
			{
				try{tablaOperaciones.Rows[i][3]=Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[5].FindControl("valorOperacion")).Text);}
				catch{}

				tablaOperaciones.Rows[i][5]=Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[3].FindControl("tiempoOperacion")).Text).ToString();
                tablaOperaciones.Rows[i][6]=((DropDownList)operaciones.Items[i].Cells[4].FindControl("estadoE")).SelectedItem.Text;
                //((DropDownList)operaciones.Items[i].Cells[4].FindControl("estadoE")).SelectedValue = "A";
                //tablaOperaciones.Rows[i][6] = ((DropDownList)operaciones.Items[i].Cells[4].FindControl("estadoE")).SelectedItem.Text;

                tablaOperaciones.Rows[i][7]=((DropDownList)operaciones.Items[i].Cells[2].FindControl("mecanicoE")).SelectedValue;
			}
		}
		
		protected bool Revision_Tabla_Operaciones()
		{
			int i,j;
			bool operacionRepetida = false;
			for(i=0;i<tablaOperaciones.Rows.Count;i++)
			{
				for(j=i+1;j<tablaOperaciones.Rows.Count;j++)
				{
					if((((Button)operaciones.Items[i].Cells[8].Controls[1]).Text == "Remover")&&(((Button)operaciones.Items[j].Cells[8].Controls[1]).Text == "Remover"))
						if((tablaOperaciones.Rows[i][0].ToString())==(tablaOperaciones.Rows[j][0].ToString()))
							operacionRepetida = true;
				}
			}
			return operacionRepetida;
		}
		
		protected DateTime Adicionar_Horas(double horas, DateTime fechaBase)
		{
			DateTime horaFinalTaller = Convert.ToDateTime(fechaBase.Date.ToString("yyyy-MM-dd")+" "+DBFunctions.SingleData("SELECT ctal_hfmec FROM ctaller"));
			DateTime horaInicioTaller = Convert.ToDateTime(fechaBase.Date.ToString("yyyy-MM-dd")+" "+DBFunctions.SingleData("SELECT ctal_himec FROM ctaller"));
			DateTime resultadoAdicion = fechaBase.AddHours(horas);
			//Miramos si el resultado esta por encima de la hora de atencion en el taller
			if((DateTime.Compare(resultadoAdicion,horaFinalTaller))==1)
			{
				TimeSpan diferenciaArriba = resultadoAdicion - horaFinalTaller;
				TimeSpan diferenciaAbajo = horaFinalTaller - fechaBase;
				TimeSpan diferenciaTaller = horaFinalTaller - horaInicioTaller;
				if((TimeSpan.Compare(diferenciaArriba,diferenciaTaller))==1)
				{
					TimeSpan controlBucle = new TimeSpan(0,0,0);
					while(controlBucle < diferenciaArriba)
					{
						//Si se requiere mas tiempo que un dia de trabajo
						if((TimeSpan.Compare((diferenciaArriba-controlBucle),diferenciaTaller))==1)
						{
							resultadoAdicion = resultadoAdicion.AddDays(1);
							resultadoAdicion = Convert.ToDateTime(resultadoAdicion.ToString("yyyy-MM-dd")+" "+horaInicioTaller.TimeOfDay.ToString());
							controlBucle += diferenciaTaller;
						}
						else
						{
							resultadoAdicion = resultadoAdicion.Add(diferenciaArriba-controlBucle);
							controlBucle += (diferenciaArriba-controlBucle);
						}
					}
				}
				else
				{
					resultadoAdicion = resultadoAdicion.AddDays(1);
					resultadoAdicion = Convert.ToDateTime(resultadoAdicion.ToString("yyyy-MM-dd")+" "+(horaInicioTaller.Add(diferenciaArriba)).TimeOfDay.ToString());
				}
			}
			return resultadoAdicion;
		}
		
		protected DateTime Remover_Horas(double horas, DateTime fechaBase)
		{
			DateTime horaFinalTaller = Convert.ToDateTime(fechaBase.Date.ToString("yyyy-MM-dd")+" "+DBFunctions.SingleData("SELECT ctal_hfmec FROM ctaller"));
			DateTime horaInicioTaller = Convert.ToDateTime(fechaBase.Date.ToString("yyyy-MM-dd")+" "+DBFunctions.SingleData("SELECT ctal_himec FROM ctaller"));
			DateTime resultadoSustraccion = fechaBase.AddHours(horas);
			//Miramos si el resultado esta por debajo de la hora de atencion en el taller
			if((DateTime.Compare(resultadoSustraccion,horaInicioTaller))==-1)
			{
				TimeSpan diferenciaArriba = fechaBase - horaInicioTaller;
				TimeSpan diferenciaAbajo = horaInicioTaller - resultadoSustraccion;
				TimeSpan diferenciaTaller = horaFinalTaller - horaInicioTaller;
				if((TimeSpan.Compare(diferenciaAbajo,diferenciaTaller))==1)
				{
					TimeSpan controlBucle = new TimeSpan(0,0,0);
					while(controlBucle < diferenciaAbajo)
					{
						if((TimeSpan.Compare((diferenciaAbajo-controlBucle),diferenciaTaller))==1)
						{
							resultadoSustraccion = resultadoSustraccion.AddDays(-1);
							resultadoSustraccion = Convert.ToDateTime(resultadoSustraccion.ToString("yyyy-MM-dd")+" "+horaFinalTaller.TimeOfDay.ToString());
							controlBucle += diferenciaTaller;
						}
						else
						{
							resultadoSustraccion = resultadoSustraccion.Add((diferenciaAbajo-controlBucle).Negate());
							controlBucle += (diferenciaAbajo-controlBucle);
						}
					}
				}
				else
				{
					resultadoSustraccion = resultadoSustraccion.AddDays(-1);
					resultadoSustraccion = Convert.ToDateTime(resultadoSustraccion.ToString("yyyy-MM-dd")+" "+(horaFinalTaller.Add(diferenciaAbajo.Negate())).TimeOfDay.ToString());
				}
			}
			return resultadoSustraccion;
		}
		
		protected bool RevisionCargo(string cargoPrincipalOT, string cargoEscogido)
		{
			bool cargoValido = true;
			if(cargoPrincipalOT == "S")
				cargoValido = true;
			else if(cargoPrincipalOT == "C")
			{
				if(cargoEscogido == "S")
					cargoValido = false;
			}
			else if(cargoPrincipalOT == "G")
			{
				if(cargoEscogido == "S" || cargoEscogido == "C")
					cargoValido = false;
			}
			else
			{
				if(cargoEscogido == "S" || cargoEscogido == "C" || cargoEscogido == "G")
					cargoValido = false;
			}
			return cargoValido;
		}

		protected bool RevisionCargoFacturado(string cargoFacturado, string cargoEscogido)
		{
			bool cargoValido = true; // valida que no inserten un cargo a una operacion con ese cargo ya facturado en la orden
			if(cargoFacturado == cargoEscogido)
				cargoValido = false;
			return cargoValido;
		}
		protected bool ValidacionFechaRango(DateTime fechaIngresada)
		{
			bool salida = true;
			if(UtilitarioPlanning.GetWeekDayNumber(fechaIngresada.DayOfWeek) == 7 || DBFunctions.RecordExist("SELECT pdf_secuencia FROM pdiafestivo WHERE pdf_fech='"+fechaIngresada.ToString("yyyy-MM-dd")+"'"))
				salida = false;
			return salida;
		}

		protected bool ValidacionHoraRangoTaller(TimeSpan horaIngresada)
		{
			bool salida = true;
			TimeSpan horaInicio = Convert.ToDateTime(DBFunctions.SingleData("SELECT ctal_himec FROM ctaller")).TimeOfDay;
			TimeSpan horaFin = Convert.ToDateTime(DBFunctions.SingleData("SELECT ctal_hfmec FROM ctaller")).TimeOfDay;
			if(horaIngresada < horaInicio || horaIngresada > horaFin)
				salida = false;
			return salida;
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
			this.operaciones.ItemDataBound+= new System.Web.UI.WebControls.DataGridItemEventHandler(this.operaciones_ItemDataBound);

		}
        
        public void cargarNuevoPrecio(object sender, EventArgs e)
        {
            
            this.operaciones.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.operaciones_ItemDataBound);
            string operacion = ((TextBox)operaciones.Items[0].Cells[3].Controls[1]).Text;
            string tiempo = ((TextBox)operaciones.Items[0].Cells[3].Controls[1]).Text;
            string precio = DBFunctions.SingleData(@"select case when mo.tcar_cargo in ('C','S') THEN PPRETA_VALOHORACLIE * " + tiempo + @" else case when mo.tcar_cargo in ('G') THEN PPRETA_VALOHORAGTIA * " + tiempo + @" ELSE PPRETA_VALOHORAINTE * " + tiempo + @" END END
                    FROM PTEMPARIO TP, MORDEN MO, ppreciotalleR PT
                    WHERE MO.PDOC_CODIGO = '" + tipoDocumento.SelectedValue +@"' AND MO.MORD_NUMEORDE = "+ ordenes.SelectedValue+@" AND MO.PPRETA_CODIGO = PT.PPRETA_CODIGO AND TP.PTEM_OPERACION = '"+ operacion +"' AND TTIP_CODILIQU = 'T'; ");
            if(precio!="")
                ((TextBox)operaciones.Items[0].Cells[5].Controls[1]).Text = precio.ToString();
            
        }
        #endregion
    }
}
