// created on 22/12/2004 at 13:02

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Mail;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{

    public partial class LiquidaManoObra : System.Web.UI.UserControl
    {
        protected DatasToControls bind;
        protected DataTable tablaDiferencial;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            bind = new DatasToControls();
            if (!IsPostBack)
            {
                Session.Clear();
                string MORDENCOMISION = "";
                MORDENCOMISION = DBFunctions.SingleData("SELECT COUNT FROM MORDENCOMISION");  // valida que la tabla MORDENCOMISION este definida
                if (MORDENCOMISION == "")
                {
                    Utils.MostrarAlerta(Response, "NO ha definido la tabla de registro de liquidaciones de mano de obra. Llamar a ecas caso MORDENCOMISION !!");
                    Page.Visible = false;
                }

                bind.PutDatasIntoDropDownList(taller, "SELECT palm_descripcion FROM palmacen pa where (pa.pcen_centtal is not null  or pcen_centcoli is not null) and pa.TVIG_VIGENCIA = 'V' order by pa.PALM_DESCRIPCION;");
                bind.PutDatasIntoDropDownList(empleado, "SELECT pven_nombre FROM pvendedor WHERE tvend_codigo='" + tipoEmpleado.SelectedValue.ToString() + "' order by pven_nombre");
                this.Inicializar_Grilla();
            }
            if (Session["tablaDiferencial"] == null)
                this.Preparar_Tabla_Diferencial();
            else if (Session["tablaDiferencial"] != null)
                tablaDiferencial = (DataTable)Session["tablaDiferencial"];
        }

        protected void dgSeleccion_TablaDiferencial(object sender, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName == "AddDatasRow")
            {
                //Primero verificamos si podemos crear una nueva fila es decir si los valores
                //dentro de los TextBox son validos
                if ((!(DatasToControls.ValidarDouble((((TextBox)e.Item.Cells[0].Controls[1]).Text)))) && (!(DatasToControls.ValidarDouble((((TextBox)e.Item.Cells[1].Controls[1]).Text)))))
                    Utils.MostrarAlerta(Response, "Algun Dato no es valido, Revise y vuelva a intentarlo");
                else
                {
                    //Debemos verificar que sean datos validos para la tabla es decir que vayan en orden ascendente
                    bool noOrdenAscendente = false;
                    if (tablaDiferencial.Rows.Count > 0)
                    {
                        if ((System.Convert.ToDouble(tablaDiferencial.Rows[tablaDiferencial.Rows.Count - 1][0].ToString())) >= (System.Convert.ToDouble(((TextBox)e.Item.Cells[0].Controls[1]).Text)))
                            noOrdenAscendente = true;
                    }
                    //Creamos una fila para el DataTable
                    if (!noOrdenAscendente)
                    {
                        DataRow fila = tablaDiferencial.NewRow();
                        fila["HASTA"] = System.Convert.ToDouble((((TextBox)e.Item.Cells[0].Controls[1]).Text));
                        fila["PAGAR"] = System.Convert.ToDouble((((TextBox)e.Item.Cells[1].Controls[1]).Text));
                        tablaDiferencial.Rows.Add(fila);
                        grillaTablaDiferencial.DataSource = tablaDiferencial;
                        grillaTablaDiferencial.DataBind();
                        Session["tablaDiferencial"] = tablaDiferencial;
                    }
                    else
                        Utils.MostrarAlerta(Response, "El valor de tiempo es menor al anterior");
                }
            }
            else if (((Button)e.CommandSource).CommandName == "Remover")
            {
                tablaDiferencial.Rows[System.Convert.ToInt32(e.Item.DataSetIndex.ToString())].Delete();
                grillaTablaDiferencial.DataSource = tablaDiferencial;
                grillaTablaDiferencial.DataBind();
                Session["tablaDiferencial"] = tablaDiferencial;
            }
        }

        protected void Ejecutar_Accion(Object sender, EventArgs e)
        {
            bool inconveniente = false;
            string empresa = (DBFunctions.SingleData("SELECT mnit_nit FROM CEMPRESA;"));
            string selectOperaciones = @"Select DISTINCT DOR.pdoc_codigo,DOR.mord_numeorde,DOR.ptem_operacion,DOR.tcar_cargo,DOR.pven_codigo,DOR.dord_fechcump,DOR.test_estado,coalesce(DOR.pgar_codigo1,'0'),  
                 coalesce(DOR.pgar_codigo2,'0'),coalesce(DOR.pgar_codigo3,'0'),coalesce(DOR.pgar_codigo4,'0'),DOR.dord_tiemliqu,DOR.dord_valooper,DOR.dord_fechliqu,coalesce(DOR.dord_tiempoper,0),
                  PTEM.TTIP_CODILIQU, PTEM.PTEM_VALOCOMI
              FROM dordenoperacion DOR 
               INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTEM ON DOR.ptem_operacion = PTEM.ptem_operacion 
               INNER JOIN pvendedor PVEN ON DOR.pven_codigo = PVEN.pven_codigo INNER JOIN palmacen PAL ON MOR.palm_almacen = PAL.palm_almacen 
               INNER JOIN mfacturaclientetaller MFCT ON MOR.pdoc_codigo = MFCT.pdoc_prefordetrab AND MOR.mord_numeorde = MFCT.mord_numeorde AND DOR.TCAR_CARGO = MFCT.TCAR_CARGO  
               INNER JOIN mfacturacliente MFC ON MFCT.pdoc_codigo = MFC.pdoc_codigo AND MFCT.mfac_numedocu = MFC.mfac_numedocu 
              WHERE DOR.test_estado='C' AND (MOR.test_estado='E' OR MOR.test_estado='F') ";
            //   "WHERE DOR.test_estado='C' ";

            if (tipoProceso.SelectedValue != "R") // Las reliquidaciones Ignorar la fecha de Liquidacion de la operación
                selectOperaciones += "and  DOR.pdoc_codigo||DOR.mord_numeorde NOT IN (SELECT pdoc_codigo||mord_numeorde FROM MORDENCOMISION WHERE MORD_TIPOCOMI = 'M')";

            string selectRecepcionista = "SELECT DISTINCT MOR.*, mfc.mfac_valofact, pven_porccomi, mcat_placa, pmar_nombre, " +
                    " MFC.PDOC_CODIGO CONCAT '-' CONCAT CAST(MFC.MFAC_NUMEDOCU AS CHAR(10)), tcar_nombre AS CARGO" +
                    " FROM morden MOR, palmacen PAL, pvendedor PVEN, mfacturaclientetaller MFCT, mfacturacliente MFC, mcatalogovehiculo mc, Pcatalogovehiculo Pc, pmarca pm, tcargoorden TC " +
                    " WHERE (MOR.test_estado='E' OR MOR.test_estado='F') " +
                    " AND MOR.pven_codigo=pven.pven_codigo AND PVEN.tvend_codigo='RT' " +
                    " AND MOR.PALM_ALMACEN = PAL.PALM_ALMACEN " +
                    " and mor.mcat_vin = MC.MCAT_VIN " +
                    " AND pc.pcat_codigo = mc.pcat_codigo AND pc.pmar_codigo = pm.pmar_codigo " +
                    " AND MOR.tcar_cargo=TC.tcar_cargo " +
                    " AND MOR.pdoc_codigo=MFCT.pdoc_prefordetrab AND MOR.mord_numeorde=MFCT.mord_numeorde " +
                    " AND MFCT.pdoc_codigo=MFC.pdoc_codigo AND MFCT.mfac_numedocu=MFC.mfac_numedocu ";
            if (tipoProceso.SelectedValue != "R") // Las reliquidaciones Ignorar la fecha de Liquidacion de la operación
                selectRecepcionista += "and  mOR.pdoc_codigo||mOR.mord_numeorde NOT IN (SELECT pdoc_codigo||mord_numeorde FROM MORDENCOMISION WHERE MORD_TIPOCOMI = 'R')";


            // nueva sentencia para sacar costos de m.o e items para recepcionistras
            /*
              SELECT MOR.PDOC_CODIGO, MOR.MORD_NUMEORDE, mfc.mfac_valofact,  
--  sum( COALESCE(dp.dord_valooper,0) * COALESCE(PM.PVEN_PORCCOMI,0) *0.01 ) + SUM(COALESCE(PM.PVEN_VALOUNIDVENT,0)*COALESCE(DP.DORD_TIEMLIQU,0)) as costo_manoobra, 
 --di.mite_codigo,DI.DITE_CANTIDAD,DI.DITE_VALOUNIT,di.dite_costprom,
 SUM((DI.DITE_CANTIDAD - COALESCE(DID.DITE_CANTIDAD,0) )*DI.DITE_VALOUNIT*(1-(DI.DITE_PORCDESC*0.01))) as totalitem,  sum(di.dite_cantidad * di.dite_costprom) AS COSTOITEMS,  
 PVEN.pven_porccomi, mcat_placa, pmar_nombre, MFC.PDOC_CODIGO CONCAT '-' CONCAT CAST(MFC.MFAC_NUMEDOCU AS CHAR(10)) CONCAT tcar_nombre  
 FROM morden MOR, palmacen PAL, pvendedor PVEN, mfacturacliente MFC, mcatalogovehiculo mc, Pcatalogovehiculo Pc, pmarca pm, tcargoorden TC, mfacturaclientetaller MFCT
 -- inner JOIN DORDENOPERACION DP ON DP.PDOC_CODIGO = MFCT.PDOC_PREFORDETRAB AND DP.MORD_NUMEORDE = MFCT.MORD_numeorde and dp.tcar_cargo = mfct.tcar_cargo
--  LEFT JOIN PVENDEDOR PM ON DP.PVEN_CODIGO = PM.PVEN_CODIGO

 inner JOIN MORDENTRANSFERENCIA MOT ON MOT.PDOC_CODIGO = MFCT.PDOC_PREFORDETRAB AND MOT.MORD_NUMEORDE = MFCT.MORD_NUMEORDE AND MOT.TCAR_CARGO = MFCT.TCAR_CARGO
 LEFT JOIN DITEMS DI  ON MOt.PDOC_factura = di.PDOC_CODIGO AND MOt.Mfac_numero = di.dite_NUMEdocu AND DI.TMOV_TIPOMOVI = 80
 LEFT JOIN DITEMS DID ON di.PDOC_CODIGO = diD.DITE_PREFDOCUREFE AND di.dite_NUMEdocu = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81

WHERE (MOR.test_estado='E' OR MOR.test_estado='F')  
 AND MOR.pven_codigo=pven.pven_codigo AND PVEN.tvend_codigo='RT'  
 and mor.mcat_vin = MC.MCAT_VIN
 AND MOR.PALM_ALMACEN = PAL.PALM_ALMACEN  
 AND pc.pcat_codigo = mc.pcat_codigo AND pc.pmar_codigo = pm.pmar_codigo  
 AND MOR.tcar_cargo=TC.tcar_cargo  
 AND MOR.pdoc_codigo=MFCT.pdoc_prefordetrab AND MOR.mord_numeorde=MFCT.mord_numeorde  
 AND MFCT.pdoc_codigo=MFC.pdoc_codigo AND MFCT.mfac_numedocu=MFC.mfac_numedocu 
 AND MORD_FECHLIQU = '2012-11-26'
           AND MOR.PDOC_CODIGO = 'OTSK' AND MOR.MORD_NUMEORDE = 726
  group by MOR.PDOC_CODIGO, MOR.MORD_NUMEORDE, mfc.mfac_valofact, PVEN.pven_porccomi, mcat_placa, pmar_nombre, MFC.PDOC_CODIGO, MFC.MFAC_NUMEDOCU, tcar_nombre 
 -- ,di.mite_codigo,DI.DITE_CANTIDAD,DI.DITE_VALOUNIT,di.dite_costprom
 order by 1,2
 ;

             */
            //   "SELECT DISTINCT MOR.*,mfc.mfac_valofact FROM morden MOR, palmacen PAL, pvendedor PVEN, mfacturaclientetaller MFCT, mfacturacliente MFC WHERE (MOR.test_estado='E' OR MOR.test_estado='F') ";
            string selectEmpleados = "SELECT * FROM pvendedor WHERE tvend_codigo='" + tipoEmpleado.SelectedValue.ToString() + "' ";
            //vamos a adicionar el pedazo de los talleres 
            if ((tipoTaller.SelectedValue.ToString()) == "E")
            {
                selectOperaciones += "AND MOR.palm_almacen = '" + DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE TVIG_VIGENCIA = 'V' and palm_descripcion='" + taller.SelectedItem.ToString() + "'") + "' ";
                selectRecepcionista += "AND MOR.palm_almacen = '" + DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE TVIG_VIGENCIA = 'V' and palm_descripcion='" + taller.SelectedItem.ToString() + "'") + "' ";
                selectEmpleados += "AND MOR.palm_almacen='" + DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE TVIG_VIGENCIA = 'V' and palm_descripcion='" + taller.SelectedItem.ToString() + "'") + "'";
            }
            //Ahora vamos a revisar que tipo de empleado vamos a liquidar
            selectOperaciones += "AND PVEN.tvend_codigo='" + tipoEmpleado.SelectedValue.ToString() + "'";
            selectRecepcionista += "AND PVEN.tvend_codigo='" + tipoEmpleado.SelectedValue.ToString() + "'";
            //Ahora vamos a revisar si es un empleado o son todos
            if ((grupoEmpleado.SelectedValue.ToString()) == "E")
            {
                selectOperaciones += "AND DOR.pven_codigo='" + DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + empleado.SelectedItem.ToString() + "'") + "' ";
                selectRecepcionista += "AND MOR.pven_codigo='" + DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + empleado.SelectedItem.ToString() + "'") + "' ";
                selectEmpleados += "AND pven_codigo='" + DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + empleado.SelectedItem.ToString() + "'") + "' ";
            }
            selectEmpleados += " order by pven_nombre;";
            //Ahora revisamos si es por fecha o  a nivel general que se va a realizar la liquidacion
            if ((tipoFecha.SelectedValue.ToString()) == "I")
            {
                if (calFechaInicial.SelectedDate.Date > calFechaFinal.SelectedDate.Date)
                {
                    inconveniente = true;
                    Utils.MostrarAlerta(Response, "Fechas Invalidas, vuelva a seleccionar");
                }
                else if (calFechaInicial.SelectedDate.Date == calFechaFinal.SelectedDate.Date)
                {
                        selectOperaciones += "AND MFC.mfac_factura = '" + calFechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "'";
                        selectRecepcionista += "AND MOR.pdoc_codigo=MFCT.pdoc_prefordetrab AND MOR.mord_numeorde=MFCT.mord_numeorde AND MFCT.pdoc_codigo=MFC.pdoc_codigo AND MFCT.mfac_numedocu=MFC.mfac_numedocu AND mfac_factura='" + calFechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "'";
                    }
                else if (calFechaInicial.SelectedDate.Date < calFechaFinal.SelectedDate.Date)
                {
                        selectOperaciones += "AND MFC.mfac_factura >= '" + calFechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND MFC.mfac_factura <='" + calFechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ";
                        selectRecepcionista += "AND MOR.pdoc_codigo=MFCT.pdoc_prefordetrab AND MOR.mord_numeorde=MFCT.mord_numeorde AND MFCT.pdoc_codigo=MFC.pdoc_codigo AND MFCT.mfac_numedocu=MFC.mfac_numedocu AND MFC.mfac_factura>='" + calFechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND MFC.mfac_factura<='" + calFechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ";
                }
            }
            if (empresa == "800020006") //AutoInvercol paga TODO aun sin liquidar
            {
                selectOperaciones = "Select DOR.pdoc_codigo,DOR.mord_numeorde,DOR.ptem_operacion,DOR.tcar_cargo,DOR.pven_codigo,DOR.dord_fechcump,DOR.test_estado,coalesce(DOR.pgar_codigo1,'0'),coalesce(DOR.pgar_codigo2,'0'),coalesce(DOR.pgar_codigo3,'0'),coalesce(DOR.pgar_codigo4,'0'),DOR.dord_tiemliqu,DOR.dord_valooper,DOR.dord_fechliqu,coalesce(DOR.dord_tiempoper,0) " +
               "FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTEM ON DOR.ptem_operacion = PTEM.ptem_operacion INNER JOIN pvendedor PVEN ON DOR.pven_codigo = PVEN.pven_codigo INNER JOIN palmacen PAL ON MOR.palm_almacen = PAL.palm_almacen " +
               "WHERE DOR.test_estado='C' and dor.dord_fechcump >= '" + calFechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND dor.dord_fechcump <='" + calFechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ";
            }

            if ((tipoOperacion.SelectedValue.ToString()) == "B")
                selectOperaciones += "AND DOR.ptem_operacion=PTEM.ptem_operacion AND PTEM.ttip_codiliqu='B'";
            else if ((tipoOperacion.SelectedValue.ToString()) == "T")
                selectOperaciones += "AND DOR.ptem_operacion=PTEM.ptem_operacion AND PTEM.ttip_codiliqu='T'";
            else if ((tipoOperacion.SelectedValue.ToString()) == "F")
                selectOperaciones += "AND DOR.ptem_operacion=PTEM.ptem_operacion AND PTEM.ttip_codiliqu='F'";
            selectOperaciones += " order by DOR.pdoc_codigo, DOR.mord_numeorde, DOR.ptem_operacion;";
            selectRecepcionista += " order by MOR.pdoc_codigo, MOR.mord_numeorde;";

            if (!inconveniente)
            {
                if ((tipoEmpleado.SelectedValue.ToString()) == "MG")
                    this.Liquidar_Mecanicos(selectOperaciones, selectEmpleados);
                else if ((tipoEmpleado.SelectedValue.ToString()) == "RT")
                    this.Liquidar_Recepcionistas(selectRecepcionista, selectEmpleados);
                Session["Rep"] = this.Construccion_Impresion();
            }
        }
        //Funcion Para Hacer los cambios respectivos cuando se de click en el RadioButtonList tipoTaller
        protected void Cambio_Tipo_Taller(Object sender, EventArgs e)
        {
            if ((tipoTaller.SelectedValue.ToString()) == "T")
            {
                //Si vamos a realizar la consulta por todos los talleres
                labelTaller.Enabled = false;
                taller.Enabled = false;
                bind.PutDatasIntoDropDownList(taller, "SELECT palm_descripcion FROM palmacen pa where (pa.pcen_centtal is not null  or pcen_centcoli is not null) and pa.TVIG_VIGENCIA = 'V' order by pa.PALM_DESCRIPCION;");
                bind.PutDatasIntoDropDownList(empleado, "SELECT pven_nombre FROM pvendedor WHERE tvend_codigo='" + tipoEmpleado.SelectedValue.ToString() + "' order by pven_nombre");
            }
            else if ((tipoTaller.SelectedValue.ToString()) == "E")
            {
                labelTaller.Enabled = true;
                taller.Enabled = true;
                bind.PutDatasIntoDropDownList(empleado, "SELECT pven_nombre FROM pvendedor WHERE palm_almacen='" + DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE TVIG_VIGENCIA='V' and palm_descripcion='" + taller.SelectedItem.ToString() + "'") + "' AND tvend_codigo='" + tipoEmpleado.SelectedValue.ToString() + "'");
            }
        }

        //Funcion Para Hacer los cambios respectivos cuando se de click en el DropDownList
        protected void Cambio_Taller(Object sender, EventArgs e)
        {
            bind.PutDatasIntoDropDownList(empleado, "SELECT pven_nombre FROM pvendedor WHERE palm_almacen='" + DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE TVIG_VIGENCIA='V' and palm_descripcion='" + taller.SelectedItem.ToString() + "'") + "' AND tvend_codigo='" + tipoEmpleado.SelectedValue.ToString() + "'");
        }

        //Funcion Para Hacer los cambios respectivos cuando se de click en el RadioButtonList tipoEmpleado
        protected void Cambio_Tipo_Empleado(Object sender, EventArgs e)
        {
            if ((tipoTaller.SelectedValue.ToString()) == "T")
                bind.PutDatasIntoDropDownList(empleado, "SELECT pven_nombre FROM pvendedor WHERE tvend_codigo='" + tipoEmpleado.SelectedValue.ToString() + "' AND PVEN_VIGENCIA = 'V'");
            else if ((tipoTaller.SelectedValue.ToString()) == "E")
                bind.PutDatasIntoDropDownList(empleado, "SELECT pven_nombre FROM pvendedor WHERE palm_almacen='" + DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE tvig_vigencia='V' and palm_descripcion='" + taller.SelectedItem.ToString() + "'") + "' AND tvend_codigo='" + tipoEmpleado.SelectedValue.ToString() + "' AND PVEN_VIGENCIA = 'V'");
        }

        //Funcion Para Hacer los cambios respectivos cuando se de click en el RadioButtonList grupoEmpleado
        protected void Cambio_Tipo_Conjunto_Empleado(Object sender, EventArgs e)
        {
            if ((grupoEmpleado.SelectedValue.ToString()) == "T")
            {
                labelEmpleado.Enabled = false;
                empleado.Enabled = false;
            }
            else if ((grupoEmpleado.SelectedValue.ToString()) == "E")
            {
                labelEmpleado.Enabled = true;
                empleado.Enabled = true;
            }
        }
        //Funcion Para Cambiar el estado de la grilla cuando se de click en el RadioButtonList tipoParametro
        protected void Cambio_Tipo_Parametro(Object sender, EventArgs e)
        {
            if ((tipoParametro.SelectedValue.ToString()) == "P")
                grillaTablaDiferencial.Enabled = false;
            else if ((tipoParametro.SelectedValue.ToString()) == "D")
                    grillaTablaDiferencial.Enabled = true;
        }
        //Funcion Para Cambiar el estado de los calendarios cuando se de click en el RadioButtonList tipoFecha
        protected void Cambio_Tipo_Fecha(Object sender, EventArgs e)
        {
            if ((tipoFecha.SelectedValue.ToString()) == "G")
            {
                labelFecha1.Enabled = false;
                labelFecha2.Enabled = false;
                calFechaInicial.Enabled = false;
                calFechaFinal.Enabled = false;
            }
            else if ((tipoFecha.SelectedValue.ToString()) == "I")
            {
                labelFecha1.Enabled = true;
                labelFecha2.Enabled = true;
                calFechaInicial.Enabled = true;
                calFechaFinal.Enabled = true;
            }
        }

        protected void Llenar_Label_Datos_Empleado(Label info, string codigoMecanico)
        {
            info.Text = "Codigo : " + codigoMecanico + " Nombre : " + DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='" + codigoMecanico + "'");
            info.Text += "<br>Sede : " + DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE TVIG_VIGENCIA='V' and palm_almacen=(SELECT palm_almacen FROM pvendedor WHERE pven_codigo='" + codigoMecanico + "')");
        }

        protected void Preparar_Tabla_Diferencial()
        {
            tablaDiferencial = new DataTable();
            tablaDiferencial.Columns.Add(new DataColumn("HASTA", System.Type.GetType("System.Double")));
            tablaDiferencial.Columns.Add(new DataColumn("PAGAR", System.Type.GetType("System.Double")));
        }

        protected void Inicializar_Grilla()
        {
            this.Preparar_Tabla_Diferencial();
            grillaTablaDiferencial.DataSource = tablaDiferencial;
            grillaTablaDiferencial.DataBind();
        }


        ////////////////BLOQUE PARA LOS MECANICOS/////////////////

        //Funcion para llenar las liquidaciones de los mecanicos
        protected void Liquidar_Mecanicos(string selectOperacionesMecanicos, string selectEmpleados)
        {
            //Aqui vamos a crear un ArrayList que nos permita realizar la atcualizacion dentro de la base de datos
            ArrayList sqlStrings = new ArrayList();
            ////////////////////////////////////////////////////////////
            controlesResultado.Controls.Clear();
            DataSet empleados = new DataSet();
            double valorVenta = 0;
            DBFunctions.Request(empleados, IncludeSchema.NO, selectEmpleados);
            DataSet operacionesMecanico = new DataSet();
            DBFunctions.Request(operacionesMecanico, IncludeSchema.NO, selectOperacionesMecanicos);
            for (int i = 0; i < empleados.Tables[0].Rows.Count; i++)
            {
                int apariciones = 0;
                try
                {
                    apariciones = this.Apariciones_Operaciones_Liquidar(operacionesMecanico, empleados.Tables[0].Rows[i][0].ToString());
                }
                catch
                {
                }

                ArrayList filasMecanico = new ArrayList();
                if (apariciones > 0)
                {
                    double cantidadHoras = this.Cantidad_Horas_Empleado(operacionesMecanico, empleados.Tables[0].Rows[i][0].ToString());
                    double valorHoraTablaDiferencial = this.Valor_Hora_Tabla_Diferencial(cantidadHoras);
                    //Ahora construyamos el datatable asociado de la grilla del mecanico
                    DataTable tablaAsociadaLiquidacion = new DataTable();
                    tablaAsociadaLiquidacion = this.Preparar_Tabla_Liquidacion_Mecanico(tablaAsociadaLiquidacion);
                    //Ahora creamos y asociamos los controles secundarios al PlaceHolder controlesResultado
                    Label informacionMecanico = new Label();
                    this.Llenar_Label_Datos_Empleado(informacionMecanico, empleados.Tables[0].Rows[i][0].ToString());
                    DataGrid grillaLiquidacion = new DataGrid();
                    DatasToControls.Aplicar_Formato_Grilla(grillaLiquidacion);
                    Label resultadosLiquidacion = new Label();
                    controlesResultado.Controls.Add(new LiteralControl("<br><br>"));
                    controlesResultado.Controls.Add(informacionMecanico);
                    controlesResultado.Controls.Add(grillaLiquidacion);
                    controlesResultado.Controls.Add(new LiteralControl("<br>"));
                    controlesResultado.Controls.Add(resultadosLiquidacion);
                    double valorFacturado = 0, valorAPagar = 0;
                    double cantidadHorasLiquidadas = 0;
                    double ivaO = Convert.ToDouble(DBFunctions.SingleData("SELECT CEMP_PORCIVA FROM CEMPRESA;")) / 100;
                    string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                    string conceptoNomina = DBFunctions.SingleData("select CNOM_CONCECOMICODI from dbxschema.cnomina");

                    //Ahora vamos a construir las filas del DataTable Asociado
                    for (int j = 0; j < operacionesMecanico.Tables[0].Rows.Count; j++)
                    {
                        //Verificamos que sea nuestro empleado y que la operacion no haya sido liquidada
                        if ((((empleados.Tables[0].Rows[i][0].ToString()) == (operacionesMecanico.Tables[0].Rows[j][4].ToString())) && ((operacionesMecanico.Tables[0].Rows[j][13].ToString()) == "") && ((tipoProceso.SelectedValue.ToString() == "P") || (tipoProceso.SelectedValue.ToString() == "L"))) || (((empleados.Tables[0].Rows[i][0].ToString()) == (operacionesMecanico.Tables[0].Rows[j][4].ToString())) && ((operacionesMecanico.Tables[0].Rows[j][13].ToString()) != "") && (tipoProceso.SelectedValue.ToString() == "R")))
                        {
                            string placa = DBFunctions.SingleData(
                                 "SELECT mcat_placa " +
                                 "FROM mcatalogovehiculo " +
                                 "WHERE mcat_vin = (SELECT mcat_vin " +
                                 "                  FROM morden " +
                                 "                  WHERE pdoc_codigo = '" + operacionesMecanico.Tables[0].Rows[j][0].ToString() + "' " +
                                 "                  AND   mord_numeorde = " + operacionesMecanico.Tables[0].Rows[j][1].ToString() + ")");
                            string marca = DBFunctions.SingleData(
                                "SELECT pmar_nombre " +
                                "FROM mcatalogovehiculo mc, " +
                                "     pmarca pm, " +
                                "     pcatalogovehiculo pc " +
                                "WHERE mc.mcat_vin = (SELECT mcat_vin " +
                                "                     FROM morden " +
                                "                     WHERE pdoc_codigo = '" + operacionesMecanico.Tables[0].Rows[j][0].ToString() + "' " +
                                "                     AND   mord_numeorde = " + operacionesMecanico.Tables[0].Rows[j][1].ToString() + ") " +
                                "AND   pc.pcat_codigo = mc.pcat_codigo " +
                                "AND   pc.pmar_codigo = pm.pmar_codigo");
                            if (operacionesMecanico.Tables[0].Rows[j][11].ToString() == "")
                                operacionesMecanico.Tables[0].Rows[j][11] = "0";
                            cantidadHorasLiquidadas += System.Convert.ToDouble(operacionesMecanico.Tables[0].Rows[j][11].ToString());
                            double porcentajeComision = System.Convert.ToDouble(DBFunctions.SingleData("SELECT pven_porccomi FROM pvendedor WHERE pven_codigo='" + empleados.Tables[0].Rows[i][0].ToString() + "'")) / 100;
                            String operacionTempario = operacionesMecanico.Tables[0].Rows[j][2].ToString();
                            double valorComision = operacionesMecanico.Tables[0].Rows[j][15].ToString().Equals("F") ?
                                System.Convert.ToDouble(operacionesMecanico.Tables[0].Rows[j][16].ToString()) // por valor fijo
                                : System.Convert.ToDouble(operacionesMecanico.Tables[0].Rows[j][12].ToString()) * porcentajeComision / 1.16; // otros (DESCUENTA EL IVA)
                            double valorUnidad = tipoParametro.SelectedValue.ToString() == "P" ? valorUnidad = System.Convert.ToDouble(DBFunctions.SingleData("SELECT pven_valounidvent FROM pvendedor WHERE pven_codigo='" + empleados.Tables[0].Rows[i][0].ToString() + "'"))
                                                                                               : valorUnidad = valorHoraTablaDiferencial; //tipoParametro = "D"
                            double valorLiquidado = operacionesMecanico.Tables[0].Rows[j][15].ToString().Equals("F") ? 0 : valorUnidad * System.Convert.ToDouble(operacionesMecanico.Tables[0].Rows[j][11].ToString());
                            double valorSubTotal = valorLiquidado + valorComision;

                            DataRow fila = tablaAsociadaLiquidacion.NewRow();
                            fila["PREFIJO OT"] = operacionesMecanico.Tables[0].Rows[j][0].ToString();
                            fila["NUMERO OT"] = operacionesMecanico.Tables[0].Rows[j][1].ToString();
                            fila["PLACA"] = placa;
                            fila["MARCA"] = marca;
                            fila["DESCRIPCION DE OPERACION"] = DBFunctions.SingleData("SELECT ptem_descripcion FROM ptempario WHERE ptem_operacion='" + operacionesMecanico.Tables[0].Rows[j][2].ToString() + "'");
                            fila["CODIGO DE OPERACION"] = operacionesMecanico.Tables[0].Rows[j][2].ToString();
                            try
                            {
                                fila["FECHA FINALIZACION"] = System.Convert.ToDateTime(operacionesMecanico.Tables[0].Rows[j][5].ToString()).ToString("yyyy-MM-dd");
                            }
                            catch
                            {
                                fila["FECHA FINALIZACION"] = "";
                            }
                            try
                            {
                                fila["TIEMPO ESTANDAR"] = System.Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_tempario='" + operacionesMecanico.Tables[0].Rows[j][2].ToString() + "' AND ptie_grupcata=(SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='" + placa + "'))"));
                            }
                            catch
                            {
                                if (Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM dbxschema.ptempario WHERE ptem_operacion='" + operacionesMecanico.Tables[0].Rows[j][2].ToString() + "'")) == 0)
                                    fila["TIEMPO ESTANDAR"] = 1;
                                else
                                    fila["TIEMPO ESTANDAR"] = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM dbxschema.ptempario WHERE ptem_operacion='" + operacionesMecanico.Tables[0].Rows[j][2].ToString() + "'"));
                            }
                            fila["TIEMPO LIQUIDADO"] = System.Convert.ToDouble(operacionesMecanico.Tables[0].Rows[j][11].ToString());
                            fila["CARGO"] = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + operacionesMecanico.Tables[0].Rows[j][3].ToString() + "'");
                            fila["VALOR UNIDAD"] = Math.Round(valorUnidad, 0).ToString("C");
                            if(operacionesMecanico.Tables[0].Rows[j][3].ToString() == "S" || operacionesMecanico.Tables[0].Rows[j][3].ToString() == "C") // solo cargo S y C tiene el iva incluido
                                valorVenta  = Convert.ToDouble(Math.Round((System.Convert.ToDouble(operacionesMecanico.Tables[0].Rows[j][12]) / (1 + ivaO)), 0));
                            else
                                valorVenta = Convert.ToDouble(Math.Round((System.Convert.ToDouble(operacionesMecanico.Tables[0].Rows[j][12]) / (1 )), 0));
                            fila["VALOR FACTURADO"] = Math.Round(valorVenta, 0).ToString("C");
                            fila["VALOR LIQUIDADO"] = Math.Round(valorLiquidado, 0).ToString("C");
                            fila["COMISION"] = Math.Round(valorComision, 0).ToString("C");
                            fila["SUB-TOTAL"] = Math.Round(valorSubTotal, 0).ToString("C");

                            //Fila duplicada?
                            string act = fila["PREFIJO OT"].ToString() + "@" + fila["NUMERO OT"] + "@" + fila["CODIGO DE OPERACION"];
                            //	if(!filasMecanico.Contains(act))
                            {
                                filasMecanico.Add(act);
                                tablaAsociadaLiquidacion.Rows.Add(fila);
                            }

                            valorFacturado += valorVenta;
                            valorAPagar += valorSubTotal;
                            //Aqui se adiciona A LA TABLA de registro de liquidación de comisiones tipo MECANICO para no volver a pagar una orden ya pagada
                            if (j == 0 || j > 0 && operacionesMecanico.Tables[0].Rows[j][0] + operacionesMecanico.Tables[0].Rows[j][1].ToString() != operacionesMecanico.Tables[0].Rows[j - 1][0] + operacionesMecanico.Tables[0].Rows[j - 1][1].ToString())
                            {
                                if (tipoProceso.SelectedValue == "L" &&
                                    !DBFunctions.RecordExist("SELECT PDOC_CODIGO FROM MORDENCOMISION WHERE PDOC_CODIGO = '" + fila["PREFIJO OT"].ToString() + "' AND MORD_NUMEORDE = " + fila["NUMERO OT"] + " AND mord_tipocomi = 'M' "))
                                    sqlStrings.Add("INSERT INTO MORDENCOMISION VALUES ('" + fila["PREFIJO OT"].ToString() + "'," + fila["NUMERO OT"] + ",'" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','M') ");
                            }
                        }
                    }

                    //Agregar novedades de las comisiones de los tecnicos
                    string codigoEmpleado = empleados.Tables[0].Rows[i][2].ToString();
                    RegistroNovedadesNomina(codigoEmpleado, conceptoNomina, valorAPagar, fechaHoy, ref sqlStrings);

                    grillaLiquidacion.DataSource = tablaAsociadaLiquidacion;
                    grillaLiquidacion.DataBind();
                    //Aqui construimos el otro label del resultado
                    this.Llenar_Label_Resultado_Mecanico(resultadosLiquidacion, cantidadHorasLiquidadas, valorFacturado, valorAPagar, empleados.Tables[0].Rows[i][0].ToString());
                }
            }
            //Aqui realizamos la actualizacion
            if (tipoProceso.SelectedValue.ToString() != "P")
            {
                if (DBFunctions.Transaction(sqlStrings))
                    Utils.MostrarAlerta(Response, "Actualización Exitosa");
                else
                    lb.Text += "Error: " + DBFunctions.exceptions + "<br><br>";
            }
        }
        protected DataTable Preparar_Tabla_Liquidacion_Recepcionista(DataTable tablaLiquidacionRecepcionista)
        {
            tablaLiquidacionRecepcionista.Columns.Add(new DataColumn("PREFIJO OT", System.Type.GetType("System.String")));
            tablaLiquidacionRecepcionista.Columns.Add(new DataColumn("NUMERO OT", System.Type.GetType("System.String")));
            tablaLiquidacionRecepcionista.Columns.Add(new DataColumn("PLACA", System.Type.GetType("System.String")));
            tablaLiquidacionRecepcionista.Columns.Add(new DataColumn("MARCA", System.Type.GetType("System.String")));
            tablaLiquidacionRecepcionista.Columns.Add(new DataColumn("CARGO", System.Type.GetType("System.String")));
            tablaLiquidacionRecepcionista.Columns.Add(new DataColumn("VALOR TOTAL ORDEN", System.Type.GetType("System.String")));
            tablaLiquidacionRecepcionista.Columns.Add(new DataColumn("PORCENTAJE DE COMISION", System.Type.GetType("System.String")));
            tablaLiquidacionRecepcionista.Columns.Add(new DataColumn("VALOR DE COMISION", System.Type.GetType("System.String")));
            return tablaLiquidacionRecepcionista;
        }
        protected DataTable Preparar_Tabla_Liquidacion_Mecanico(DataTable tablaLiquidacionMecanico)
        {
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("PREFIJO OT", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("NUMERO OT", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("PLACA", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("MARCA", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("DESCRIPCION DE OPERACION", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("CODIGO DE OPERACION", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("FECHA FINALIZACION", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("TIEMPO ESTANDAR", System.Type.GetType("System.Double")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("TIEMPO LIQUIDADO", System.Type.GetType("System.Double")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("CARGO", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("VALOR UNIDAD", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("VALOR FACTURADO", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("VALOR LIQUIDADO", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("COMISION", System.Type.GetType("System.String")));
            tablaLiquidacionMecanico.Columns.Add(new DataColumn("SUB-TOTAL", System.Type.GetType("System.String")));
            return tablaLiquidacionMecanico;
        }

        protected int Apariciones_Operaciones_Liquidar(DataSet operacionesLiquidar, string codigoMecanico)
        {
            int aparicion = 0;
            for (int i = 0; i < operacionesLiquidar.Tables[0].Rows.Count; i++)
            {
                if (codigoMecanico == (operacionesLiquidar.Tables[0].Rows[i][4].ToString()))
                    aparicion += 1;
            }
            return aparicion;
        }

        protected double Cantidad_Horas_Empleado(DataSet operacionesLiquidar, string codigoMecanico)
        {
            double cantidadHoras = 0;
            for (int i = 0; i < operacionesLiquidar.Tables[0].Rows.Count; i++)
            {
                if (codigoMecanico == (operacionesLiquidar.Tables[0].Rows[i][4].ToString()))
                {
                    string tem = operacionesLiquidar.Tables[0].Rows[i][11].ToString();
                    if (tem == "" || tem == null)
                        tem = "0";
                    cantidadHoras += System.Convert.ToDouble(tem);
                }
            }
            return cantidadHoras;
        }

        protected double Valor_Hora_Tabla_Diferencial(double cantidadHoras)
        {
            double valorHora = 0;
            for (int i = 0; i < tablaDiferencial.Rows.Count; i++)
            {
                if (i == 0)
                {
                    //en caso que sea menor que el primero
                    if (cantidadHoras <= System.Convert.ToDouble(tablaDiferencial.Rows[i][0].ToString()))
                    {
                        valorHora = System.Convert.ToDouble(tablaDiferencial.Rows[i][1].ToString());
                    }
                }
                else if (i == (tablaDiferencial.Rows.Count - 1))
                {
                    //en caso que sea mayor que el ultimo
                    if (cantidadHoras >= System.Convert.ToDouble(tablaDiferencial.Rows[i][0].ToString()))
                        valorHora = System.Convert.ToDouble(tablaDiferencial.Rows[i][1].ToString());
                }
                else
                {
                    if ((cantidadHoras <= System.Convert.ToDouble(tablaDiferencial.Rows[i][0].ToString())) && (cantidadHoras > System.Convert.ToDouble(tablaDiferencial.Rows[i - 1][0].ToString())))
                    {
                        valorHora = System.Convert.ToDouble(tablaDiferencial.Rows[i][1].ToString());
                    }
                }
            }
            return valorHora;
        }

        protected void Llenar_Label_Resultado_Mecanico(Label info, double cantidadHoras, double facturado,  double comision, string codigoMecanico)
        {
            info.Text = "Cantidad de Horas Liquidadas : " + cantidadHoras.ToString();
            info.Text += "<br>Valor Total Facturado : " + facturado.ToString("C");
            info.Text += "<br>Valor Total de Comisión : " + comision.ToString("C");
            info.Text += "<br>Salario Basico : " + (System.Convert.ToDouble(DBFunctions.SingleData("SELECT pven_salabasi FROM pvendedor WHERE pven_codigo='" + codigoMecanico + "'"))).ToString("C");
            //  AQUI SE INSERTAN LA SENTENCIAS PARA INCLUIRLO EN LAS NOVEDADES DE NOMINA
        }
        ////////////////FIN BLOQUE PARA LOS MECANICOS/////////////////

        ////////////////BLOQUE PARA LOS RECEPCIONISTAS/////////////////

        protected void Liquidar_Recepcionistas(string selectOrdenesRecepcionista, string selectEmpleados)
        {
            //Aqui vamos a crear un ArrayList que nos permita realizar la acualizacion dentro de la base de datos
            ArrayList sqlStrings = new ArrayList();
            ////////////////////////////////////////////////////////////
            controlesResultado.Controls.Clear();
            DataSet empleados = new DataSet();
            DBFunctions.Request(empleados, IncludeSchema.NO, selectEmpleados);
            DataSet ordenesRecepcionista = new DataSet();
            DBFunctions.Request(ordenesRecepcionista, IncludeSchema.NO, selectOrdenesRecepcionista);
            for (int i = 0; i < empleados.Tables[0].Rows.Count; i++)
            {
                int aparicion = 0;
                try
                {
                    aparicion = this.Apariciones_Ordenes_Liquidar(ordenesRecepcionista, empleados.Tables[0].Rows[i][0].ToString());
                }
                catch
                {
                }
                if (aparicion > 0)
                {
                    double cantidadAPagar = 0;
                    //Ahora construyamos el datatable asociado de la grilla del mecanico
                    DataTable tablaAsociadaLiquidacion = new DataTable();
                    tablaAsociadaLiquidacion = this.Preparar_Tabla_Liquidacion_Recepcionista(tablaAsociadaLiquidacion);
                    //Ahora creamos y asociamos los controles secundarios al PlaceHolder controlesResultado
                    Label informacionRecepcionista = new Label();
                    this.Llenar_Label_Datos_Empleado(informacionRecepcionista, empleados.Tables[0].Rows[i][0].ToString());
                    DataGrid grillaLiquidacion = new DataGrid();
                    DatasToControls.Aplicar_Formato_Grilla(grillaLiquidacion);
                    Label resultadosLiquidacion = new Label();
                    controlesResultado.Controls.Add(new LiteralControl("<br><br>"));
                    controlesResultado.Controls.Add(informacionRecepcionista);
                    controlesResultado.Controls.Add(grillaLiquidacion);
                    controlesResultado.Controls.Add(new LiteralControl("<br>"));
                    controlesResultado.Controls.Add(resultadosLiquidacion);

                    if (ordenesRecepcionista.Tables.Count > 0)
                    {
                        for (int j = 0; j < ordenesRecepcionista.Tables[0].Rows.Count; j++)
                        {
                            //Si el codigo del empleado de ordenesRecerpcionista es igual al de empleados							  y						proceso es PreLiquidación o proceso es Liquidación						o				codigo vendedor de ordenesRecepcionista es igual al codigo del vendedor de empleados    y		fecha de liquidacion es diferente vacia (nula)				  y				proceso es reliquidacion
                            if ((((ordenesRecepcionista.Tables[0].Rows[j][17].ToString()) == (empleados.Tables[0].Rows[i][0].ToString())) && ((tipoProceso.SelectedValue.ToString() == "P") || (tipoProceso.SelectedValue.ToString() == "L"))) || (((ordenesRecepcionista.Tables[0].Rows[j][17].ToString()) == (empleados.Tables[0].Rows[i][0].ToString())) && (ordenesRecepcionista.Tables[0].Rows[j][30].ToString() != "") && (tipoProceso.SelectedValue.ToString() == "R")))
                            {
                                double valorLiquidado = Convert.ToDouble(ordenesRecepcionista.Tables[0].Rows[j]["MFAC_VALOFACT"].ToString());
                                double porcentajeComision = System.Convert.ToDouble(ordenesRecepcionista.Tables[0].Rows[j]["PVEN_PORCCOMI"].ToString()) * 0.01;
                                //           DBFunctions.SingleData("SELECT pven_porccomi FROM pvendedor WHERE pven_codigo = '"+empleados.Tables[0].Rows[i][0].ToString()+"'").ToString())/100;
                                string placa = ordenesRecepcionista.Tables[0].Rows[j]["MCAT_PLACA"].ToString();
                                /*          DBFunctions.SingleData(
                                            "SELECT mcat_placa " +
                                            "FROM mcatalogovehiculo " +
                                            "WHERE mcat_vin = (SELECT mcat_vin " +
                                            "                  FROM morden " +
                                            "                  WHERE pdoc_codigo = '" + ordenesRecepcionista.Tables[0].Rows[j][0].ToString() + "' " +
                                            "                  AND   mord_numeorde = " + ordenesRecepcionista.Tables[0].Rows[j][1].ToString() + ") ");
                                */
                                string marca = ordenesRecepcionista.Tables[0].Rows[j]["PMAR_NOMBRE"].ToString();
                                /*          DBFunctions.SingleData(
                                            "SELECT pmar_nombre " +
                                            "FROM mcatalogovehiculo mc, " +
                                            "     pmarca pm, " +
                                            "     pcatalogovehiculo pc " +
                                            "WHERE mc.mcat_vin = (SELECT mcat_vin " +
                                            "                     FROM morden " +
                                            "                     WHERE pdoc_codigo = '" + ordenesRecepcionista.Tables[0].Rows[j][0].ToString() + "' " +
                                            "                     AND   mord_numeorde = " + ordenesRecepcionista.Tables[0].Rows[j][1].ToString() + ") " +
                                            "AND   pc.pcat_codigo = mc.pcat_codigo " +
                                            "AND   pc.pmar_codigo = pm.pmar_codigo");
                                */
                                //		double valorLiquidado = this.Valor_Orden_Trabajo(ordenesRecepcionista.Tables[0].Rows[j][0].ToString(),ordenesRecepcionista.Tables[0].Rows[j][1].ToString());
                                string cargo = ordenesRecepcionista.Tables[0].Rows[j]["CARGO"].ToString();
                                /*           DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + ordenesRecepcionista.Tables[0].Rows[j][7].ToString() + "'");
                                        */
                                DataRow fila = tablaAsociadaLiquidacion.NewRow();
                                fila["PREFIJO OT"] = ordenesRecepcionista.Tables[0].Rows[j][0].ToString();
                                fila["NUMERO OT"] = ordenesRecepcionista.Tables[0].Rows[j][1].ToString();
                                fila["PLACA"] = placa;
                                fila["MARCA"] = marca;
                                fila["CARGO"] = cargo;
                                fila["VALOR TOTAL ORDEN"] = Math.Round(valorLiquidado, 0).ToString("C");
                                fila["PORCENTAJE DE COMISION"] = (porcentajeComision * 100).ToString() + "%";
                                valorLiquidado = valorLiquidado * porcentajeComision;
                                fila["VALOR DE COMISION"] = Math.Round(valorLiquidado, 0).ToString("C");
                                tablaAsociadaLiquidacion.Rows.Add(fila);

                                cantidadAPagar += valorLiquidado;

                                /*  se debe crear una tabla de control de liquidaciones de comisiones  
                                //Aqui Adicionamos la linea de actualizacion a la base de datos
                                sqlStrings.Add(
                                    "UPDATE morden " +
                                    "   SET mord_fechliqu = '"+DateTime.Now.Date.ToString("yyyy-MM-dd")+"' " +
                                    "WHERE pdoc_codigo = '"+ordenesRecepcionista.Tables[0].Rows[j][0].ToString()+"' " +
                                    "AND   mord_numeorde = "+ordenesRecepcionista.Tables[0].Rows[j][1].ToString()+"");
                                */
                            }
                        }
                        controlesResultado.Controls.Add(resultadosLiquidacion);
                        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                        string conceptoNomina = DBFunctions.SingleData("select CNOM_CONCECOMICODI from dbxschema.cnomina");

                        for (int j = 0; j < ordenesRecepcionista.Tables[0].Rows.Count; j++)
                        {
                            //Si el codigo del empleado de ordenesRecerpcionista es igual al de empleados							  y						proceso es PreLiquidación o proceso es Liquidación						o				codigo vendedor de ordenesRecepcionista es igual al codigo del vendedor de empleados    y		fecha de liquidacion es diferente vacia (nula)				  y				proceso es reliquidacion
                            if ((((ordenesRecepcionista.Tables[0].Rows[j][17].ToString()) == (empleados.Tables[0].Rows[i][0].ToString())) && ((tipoProceso.SelectedValue.ToString() == "P") || (tipoProceso.SelectedValue.ToString() == "L"))) || (((ordenesRecepcionista.Tables[0].Rows[j][17].ToString()) == (empleados.Tables[0].Rows[i][0].ToString())) && (ordenesRecepcionista.Tables[0].Rows[j][30].ToString() != "") && (tipoProceso.SelectedValue.ToString() == "R")))
                            {
                                double valorLiquidado = 0;
                                double porcentajeComision = 0;
                                valorLiquidado = Convert.ToDouble(ordenesRecepcionista.Tables[0].Rows[j]["MFAC_VALOFACT"].ToString());
                                porcentajeComision = System.Convert.ToDouble(ordenesRecepcionista.Tables[0].Rows[j]["PVEN_PORCCOMI"].ToString()) * 0.01;

                                //String a = ordenesRecepcionista.Tables[0].Rows[j][37].ToString();
                                //string b = ordenesRecepcionista.Tables[0].Rows[j]["MFAC_VALOFACT"].ToString();
                                //String c = ordenesRecepcionista.Tables[0].Rows[j][38].ToString();
                                //string d = ordenesRecepcionista.Tables[0].Rows[j]["PVEN_PORCCOMI"].ToString();
                                //String e = ordenesRecepcionista.Tables[0].Rows[j][39].ToString();
                                //string f = ordenesRecepcionista.Tables[0].Rows[j]["MCAT_PLACA"].ToString();
                                //String g = ordenesRecepcionista.Tables[0].Rows[j][40].ToString();
                                //string h = ordenesRecepcionista.Tables[0].Rows[j]["PMAR_NOMBRE"].ToString();
                                //String z = ordenesRecepcionista.Tables[0].Rows[j][41].ToString();
                                //string u = ordenesRecepcionista.Tables[0].Rows[j]["42"].ToString();


                                //           DBFunctions.SingleData("SELECT pven_porccomi FROM pvendedor WHERE pven_codigo = '"+empleados.Tables[0].Rows[i][0].ToString()+"'").ToString())/100;
                                string placa = ordenesRecepcionista.Tables[0].Rows[j]["MCAT_PLACA"].ToString();
                                /*          DBFunctions.SingleData(
                                            "SELECT mcat_placa " +
                                            "FROM mcatalogovehiculo " +
                                            "WHERE mcat_vin = (SELECT mcat_vin " +
                                            "                  FROM morden " +
                                            "                  WHERE pdoc_codigo = '" + ordenesRecepcionista.Tables[0].Rows[j][0].ToString() + "' " +
                                            "                  AND   mord_numeorde = " + ordenesRecepcionista.Tables[0].Rows[j][1].ToString() + ") ");
                                */
                                string marca = ordenesRecepcionista.Tables[0].Rows[j]["PMAR_NOMBRE"].ToString();
                                /*          DBFunctions.SingleData(
                                            "SELECT pmar_nombre " +
                                            "FROM mcatalogovehiculo mc, " +
                                            "     pmarca pm, " +
                                            "     pcatalogovehiculo pc " +
                                            "WHERE mc.mcat_vin = (SELECT mcat_vin " +
                                            "                     FROM morden " +
                                            "                     WHERE pdoc_codigo = '" + ordenesRecepcionista.Tables[0].Rows[j][0].ToString() + "' " +
                                            "                     AND   mord_numeorde = " + ordenesRecepcionista.Tables[0].Rows[j][1].ToString() + ") " +
                                            "AND   pc.pcat_codigo = mc.pcat_codigo " +
                                            "AND   pc.pmar_codigo = pm.pmar_codigo");
                                */
                                //		double valorLiquidado = this.Valor_Orden_Trabajo(ordenesRecepcionista.Tables[0].Rows[j][0].ToString(),ordenesRecepcionista.Tables[0].Rows[j][1].ToString());
                                string cargo = ordenesRecepcionista.Tables[0].Rows[j]["CARGO"].ToString();
                                /*           DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + ordenesRecepcionista.Tables[0].Rows[j][7].ToString() + "'");
                                */
                                DataRow fila = tablaAsociadaLiquidacion.NewRow();
                                fila["PREFIJO OT"] = ordenesRecepcionista.Tables[0].Rows[j][0].ToString();
                                fila["NUMERO OT"] = ordenesRecepcionista.Tables[0].Rows[j][1].ToString();
                                fila["PLACA"] = placa;
                                fila["MARCA"] = marca;
                                fila["CARGO"] = cargo;
                                fila["VALOR TOTAL ORDEN"] = Math.Round(valorLiquidado, 0).ToString("C");
                                fila["PORCENTAJE DE COMISION"] = (porcentajeComision * 100).ToString() + "%";
                                valorLiquidado = valorLiquidado * porcentajeComision;
                                fila["VALOR DE COMISION"] = Math.Round(valorLiquidado, 0).ToString("C");
                                tablaAsociadaLiquidacion.Rows.Add(fila);
                            }
                            /*  se debe crear una tabla de control de liquidaciones de comisiones  
                            //Aqui Adicionamos la linea de actualizacion a la base de datos
                            sqlStrings.Add(
                                "UPDATE morden " +
                                "   SET mord_fechliqu = '"+DateTime.Now.Date.ToString("yyyy-MM-dd")+"' " +
                                "WHERE pdoc_codigo = '"+ordenesRecepcionista.Tables[0].Rows[j][0].ToString()+"' " +
                                "AND   mord_numeorde = "+ordenesRecepcionista.Tables[0].Rows[j][1].ToString()+"");
                            */
                        }
                    }
                    grillaLiquidacion.DataSource = tablaAsociadaLiquidacion;
                    grillaLiquidacion.DataBind();
                    //Aqui construimos el otro label del resultado
                    this.Llenar_Label_Resultado_Recepcionista(resultadosLiquidacion, cantidadAPagar, empleados.Tables[0].Rows[i][0].ToString());
                }
            }
            //Aqui realizamos la actualizacion
            if (tipoProceso.SelectedValue != "P")
            {
                if (DBFunctions.Transaction(sqlStrings))
                    Utils.MostrarAlerta(Response, "Actualización Exitosa");
                else
                    lb.Text += "Error: " + DBFunctions.exceptions + "<br><br>";
            }
        }

        protected void RegistroNovedadesNomina(string codEmpleado, string conceptoNomina, double valorAPagar, string fechaHoy, ref ArrayList sqlStrings)
        {
            //Agregar novedades de las comisiones de los Mecanicos y Recepcionistas
            string sqlEmpleado = "select memp_codiempl from mempleado where memp_codiempl = '" + codEmpleado + "'";
            if (DBFunctions.RecordExist(sqlEmpleado))
            {
                sqlStrings.Add("INSERT INTO MNOVEDADESNOMINA( MEMP_CODIEMPL, PCON_CONCEPTO, MNOV_NOVEDAD, MNOV_VALRTOTL, MNOV_CANTIDAD, MNOV_FECHA) " +
                               "VALUES( '" + codEmpleado + "','" + conceptoNomina + "','1'," + valorAPagar + ",0,'" + fechaHoy + "');");
            }
        }

        protected int Apariciones_Ordenes_Liquidar(DataSet ordenesLiquidar, string codigoRecepcionista)
        {
            int aparicion = 0;
            for (int i = 0; i < ordenesLiquidar.Tables[0].Rows.Count; i++)
            {
                if (codigoRecepcionista == (ordenesLiquidar.Tables[0].Rows[i][17].ToString()))
                    aparicion += 1;
            }
            return aparicion;
        }

        protected double Valor_Orden_Trabajo(string prefijoOT, string numeroOT)
        {
            double valor = 0;
            double porcentajeIva = System.Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
            DataSet operaciones = new DataSet();
            DBFunctions.Request(operaciones, IncludeSchema.NO, "SELECT * FROM dordenoperacion WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numeroOT + "");
            for (int i = 0; i < operaciones.Tables[0].Rows.Count; i++)
            {
                //Miramos si las operaciones son excentas de iva o no
                string excencion = DBFunctions.SingleData("SELECT ptem_exceiva FROM ptempario WHERE ptem_operacion='" + operaciones.Tables[0].Rows[i][2] + "'");
                if (excencion == "S")
                    valor += System.Convert.ToDouble(operaciones.Tables[0].Rows[i][12].ToString());
                else if (excencion == "N")
                        valor += (System.Convert.ToDouble(operaciones.Tables[0].Rows[i][12])) - (System.Convert.ToDouble(operaciones.Tables[0].Rows[i][12]) * (porcentajeIva / 100));
            }
            return valor;
        }

        protected void Llenar_Label_Resultado_Recepcionista(Label info, double comision, string codigoMecanico)
        {
            info.Text += "Valor Total de Comisión : " + comision.ToString("C");
            info.Text += "<br>Salario Basico : " + (System.Convert.ToDouble(DBFunctions.SingleData("SELECT pven_salabasi FROM pvendedor WHERE pven_codigo='" + codigoMecanico + "'"))).ToString("C");
            //  AQUI SE INSERTAN LA SENTENCIAS PARA INCLUIRLO EN LAS NOVEDADES DE NOMINA
        }
        ////////////////FINAL BLOQUE RECEPCIONISTA///
        /////////////////////////////////////////////

        protected string Construccion_Impresion()
        {
            StringBuilder SB = new StringBuilder();
            StringWriter SW = new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            controlesResultado.RenderControl(htmlTW);
            return SB.ToString();
        }

        public void SendMail(Object Sender, ImageClickEventArgs E)
        {
            MailMessage MyMail = new MailMessage();
            MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
            MyMail.To = tbEmail.Text;
            MyMail.Subject = "LIQUIDACION MANO DE OBRA";
            MyMail.Body = ((string)Session["Rep"]);
            MyMail.BodyFormat = MailFormat.Html;
            try
            {
                SmtpMail.Send(MyMail);
            }
            catch (Exception e)
            {
                lb.Text = e.ToString();
            }
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


