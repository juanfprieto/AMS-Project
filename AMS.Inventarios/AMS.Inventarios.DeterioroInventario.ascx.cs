using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AMS.DB;
using AMS.Tools;
using System.Collections;

namespace AMS.Inventarios
{
    public partial class AMS_Inventarios_DeterioroInventario : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Mismo caso para el prefijo: 9000 - DETERIORO DE INVENTARIO...
            //Pendiente definir obtener: Almacen -> DDETERIOROINVENTARIO... Almacen, Centro de costo -> DCUENTA...
            if (!IsPostBack)
            {
                Session.Clear();

                txtPrefijoComprobante.Text = DBFunctions.SingleData("SELECT p.pdoc_codigo concat ' - ' concat p.pdoc_nombre from pdocumento p, ccontabilidad c where c.CCON_PREFNIIF = p.pdoc_codigo;");
                ViewState["prefijoNiif"] = txtPrefijoComprobante.Text.Substring(0, txtPrefijoComprobante.Text.IndexOf("-"));

                txtLiquidaYear.Text = DBFunctions.SingleData("select pano_ano from ccontabilidad;");
                txtLiquidaMonth.Text = DBFunctions.SingleData("select pmes_mes from ccontabilidad;");

                DataSet dsDiasMora = new DataSet();
                DBFunctions.Request(dsDiasMora, IncludeSchema.NO, "SELECT pinv_diaini as DIAINI, pinv_diafin as DIAFIN, pinv_tasad as TASADIARIA from PINVENTARIORANGOSNIIF order by DIAINI;");
                dgDiasMora.DataSource = dsDiasMora;
                dgDiasMora.DataBind();
            }

        }

        protected void CalcularDeterioro_Click(object Sender, EventArgs e)
        {
            //if (DBFunctions.RecordExist("select pano_ano from ddeteriorocartera where pano_ano=" + txtLiquidaYear.Text + " and pmes_mes=" + txtLiquidaMonth.Text))
            //{
            //    Utils.MostrarAlerta(Response, "Este proceso ya se realizó.");
            //    return;
            //}

            plcDeterioro.Visible = true;
            DataSet dtDeterioro = new DataSet();

            String[] fechaCorte = txtFechaCorte.Text.Split('-');
            if (fechaCorte[1] == "01")
            {
                fechaCorte[0] = (Convert.ToInt16(fechaCorte[0]) - 1).ToString();
                fechaCorte[1] = "12";
            }
            else
            {
                fechaCorte[1] = (Convert.ToInt16(fechaCorte[1]) - 1).ToString();
            }

            DBFunctions.Request(dtDeterioro, IncludeSchema.NO,
                 @" select ms.mite_codigo as CODIGO, mi.mite_nombre as NOMBRE, ms.msal_cantactual as CANT_ACTUAL, ms.msal_costprom as COST_PROMEDIO,
                    ms.msal_ultivent as ULTIMA_VENTA, ms.msal_ultiingr as ULTIMO_INGRESO, 
                    COALESCE( DAYS('" + txtFechaCorte.Text + @"') - DAYS(ms.msal_ultivent), COALESCE( DAYS('" + txtFechaCorte.Text + @"') - DAYS(ms.msal_ultiingr),999999)) as DIAS_MORAVENT, 
                    COALESCE( DAYS('" + txtFechaCorte.Text + @"') - DAYS(ms.msal_ultiingr),999999) as DIAS_MORAINGR,
                    (select pinv_tasad from  PINVENTARIORANGOSNIIF 
                    where  pinv_diaini <= ( COALESCE(DAYS('" + txtFechaCorte.Text + @"') - DAYS(ms.msal_ultivent),999999) ) and  ( COALESCE(DAYS('" + txtFechaCorte.Text + @"') - DAYS(ms.msal_ultivent),999999) ) <= pinv_diafin) as PORCENTAJE,
                    (ms.msal_costprom * ms.msal_cantactual * 
                    (select pinv_tasad from  PINVENTARIORANGOSNIIF 
                    where  pinv_diaini <= ( COALESCE(DAYS('" + txtFechaCorte.Text + @"') - DAYS(ms.msal_ultivent),999999) ) and  ( COALESCE(DAYS('" + txtFechaCorte.Text + @"') - DAYS(ms.msal_ultivent),999999) ) <= pinv_diafin)
                    ) as DETERIORO,
                    COALESCE((select ddet_precdete from DDETERIOROINVENTARIO where mite_codigo=ms.mite_codigo order by pano_ano, ddet_consecut desc fetch first row only),0) as VALORANTERIOR
                    from MSALDOITEM ms, mitems mi where ms.mite_codigo=mi.mite_codigo and ms.pano_ano= (select m.pano_ano from MSALDOITEM m where m.mite_codigo=ms.mite_codigo order by m.pano_ano desc fetch first row only)
                    and ms.msal_cantactual <> 0 and ms.msal_ultivent <= '" + txtFechaCorte.Text + @"'
                    order by CODIGO;"); // fetch first 1000 row only;");  //restriccion para pruebas de 1000 datos.

            Session["dtDeterioro"] = dtDeterioro;
            dgDeterioro.DataSource = dtDeterioro;
            dgDeterioro.DataBind();
        }

        public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            dgDeterioro.EditItemIndex = -1;
            dgDeterioro.EnableViewState = true;
            dgDeterioro.DataSource = (DataSet)Session["dtDeterioro"];
            dgDeterioro.DataBind();
        }

        public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
        {
            dgDeterioro.EditItemIndex = (int)e.Item.ItemIndex;
            dgDeterioro.EnableViewState = true;
            dgDeterioro.DataSource = (DataSet)Session["dtDeterioro"];
            dgDeterioro.DataBind();
        }

        public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
        {
            DataSet dsDeterioro = new DataSet();
            dsDeterioro = (DataSet)Session["dtDeterioro"];
            double tasaNueva = Convert.ToDouble(((TextBox)e.Item.Cells[0].FindControl("txtEditTasa")).Text.ToString());
            double cantidadActual = Convert.ToDouble(dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][2]);
            double costoPromedio = Convert.ToDouble(dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][3]);
            double nuevoDeterioro = cantidadActual * costoPromedio * tasaNueva;

            dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][8] = tasaNueva;
            dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][9] = nuevoDeterioro;

            Session["dtDeterioro"] = dsDeterioro;
            dgDeterioro.EditItemIndex = -1;
            dgDeterioro.EnableViewState = true;
            dgDeterioro.DataSource = dsDeterioro;
            dgDeterioro.DataBind();

        }

        protected void GenerarComprobante_Click(object Sender, EventArgs e)
        {
            //DataSet dsListaPrecio = new DataSet();
            ArrayList sqlStrings = new ArrayList();
            string insertSQL = "";
            double sumaDeterioro = 0;
            string cuentaDeteInvDebi = DBFunctions.SingleData("select ccon_deteinvedebi from CCONTABILIDAD");
            string cuentaDeteInvCre = DBFunctions.SingleData("select ccon_deteinvecred from CCONTABILIDAD");
            int contSecuencia = 1;
            string nitEmpresa = DBFunctions.SingleData("select mnit_nit from cempresa");
            string numeroComprobante = DBFunctions.SingleData("SELECT PDOC_ULTIDOCU + 1 FROM pdocumento WHERE PDOC_CODIGO='" + ViewState["prefijoNiif"] + "'");  
            string numeroDocumento = DBFunctions.SingleData("SELECT PDOC_ULTIDOCU + 1 FROM pdocumento WHERE PDOC_CODIGO='9000'");   // ?
            string almacen = DBFunctions.SingleData("select palm_almacen from ccontabilidad;");
            string centroCosto = DBFunctions.SingleData("select p.pcen_codigo from ccontabilidad c, pcentrocosto p where p.pcen_codigo = c.pcen_codigo and p.timp_codigo != 'N' and p.timp_codigo != '';");
            
            DataSet dsDeterioro = new DataSet();
            dsDeterioro = (DataSet)Session["dtDeterioro"];
            

            //Codigo 99 para lista de precio de Deterior de Inventarios.
            //DBFunctions.Request(dsListaPrecio, IncludeSchema.NO, "select mite_codigo as CODIGO, mpre_precio as PRECIO from mprecioitem where ppre_codigo='3';"); //'99';");
            //DataTable dtItemsSeleccionados = (DataTable)Session["dtItemsSeleccionado"];

            for (int h = 0; h < dsDeterioro.Tables[0].Rows.Count; h++)
            {
                //ddeterioroinventario
                double valorDeterioro = Convert.ToDouble(dsDeterioro.Tables[0].Rows[h]["DETERIORO"].ToString());
                double costoPromedioItem = Convert.ToDouble(dsDeterioro.Tables[0].Rows[h]["COST_PROMEDIO"].ToString());
                double cantidadItem = Convert.ToDouble(dsDeterioro.Tables[0].Rows[h]["CANT_ACTUAL"].ToString());
                double valorUltimoDeterioro = Convert.ToDouble(dsDeterioro.Tables[0].Rows[h]["VALORANTERIOR"].ToString());
                double diferenciaDeterioro = valorDeterioro - valorUltimoDeterioro;
                double difDetReversion = 0;

                if (valorDeterioro > 0 && diferenciaDeterioro != 0)
                {
                    //Validar si hay reversion de saldo
                    if (diferenciaDeterioro < 0)
                    {
                        difDetReversion = diferenciaDeterioro * -1;
                        diferenciaDeterioro = 0;
                    }

                    insertSQL = "INSERT INTO DDETERIOROINVENTARIO VALUES (DEFAULT,";
                    insertSQL += txtLiquidaYear.Text + ",";
                    insertSQL += "'" + almacen + "',";
                    insertSQL += "'" + dsDeterioro.Tables[0].Rows[h]["CODIGO"].ToString() + "',";
                    insertSQL += "'" + DBFunctions.SingleData("SELECT plin_codigo FROM MITEMS where mite_codigo='" + dsDeterioro.Tables[0].Rows[h]["CODIGO"].ToString() + "';") + "',";
                    insertSQL += Math.Round(costoPromedioItem) + ",";
                    insertSQL += Math.Round(valorDeterioro) + ",";
                    insertSQL += cantidadItem + ",";
                    insertSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "');";

                    sqlStrings.Add(insertSQL);

                    //DETALLES.......
                    //dcuenta debito
                    insertSQL = "INSERT INTO DCUENTA VALUES(";
                    insertSQL += "'" + ViewState["prefijoNiif"] + "',";
                    insertSQL += numeroComprobante + ",";
                    insertSQL += "'" + cuentaDeteInvDebi + "',"; ;
                    insertSQL += "'9000',";
                    insertSQL += numeroDocumento + ",";
                    insertSQL += contSecuencia + ",";
                    insertSQL += "'" + nitEmpresa + "',";
                    insertSQL += "'" + almacen + "',";
                    insertSQL += "'" + centroCosto + "',";
                    insertSQL += "'DETERIORO INVENTARIO NIIF',";
                    insertSQL += "0,0,0,";
                    insertSQL += Math.Round(diferenciaDeterioro) + ",";
                    insertSQL += difDetReversion + ");";

                    contSecuencia++;
                    sqlStrings.Add(insertSQL);

                    //dcuenta credito
                    insertSQL = "INSERT INTO DCUENTA VALUES(";
                    insertSQL += "'" + ViewState["prefijoNiif"] + "',";
                    insertSQL += numeroComprobante + ",";
                    insertSQL += "'" + cuentaDeteInvCre + "',"; ;
                    insertSQL += "'9000',";
                    insertSQL += numeroDocumento + ",";
                    insertSQL += contSecuencia + ",";
                    insertSQL += "'" + nitEmpresa + "',";
                    insertSQL += "'" + almacen + "',";
                    insertSQL += "'" + centroCosto + "',";
                    insertSQL += "'DETERIORO INVENTARIO NIIF',";
                    insertSQL += "0,0,0,";
                    insertSQL += difDetReversion + ",";
                    insertSQL += Math.Round(diferenciaDeterioro) + ");";

                    contSecuencia++;
                    sqlStrings.Add(insertSQL);

                    sumaDeterioro += diferenciaDeterioro + difDetReversion;
                }
            }

            //Creacion del comprobante de deterioro
            insertSQL = "INSERT INTO MCOMPROBANTE VALUES('" + ViewState["prefijoNiif"] + "',";
            insertSQL += numeroComprobante + ",";
            insertSQL += DateTime.Now.Year + ",";
            insertSQL += DateTime.Now.Month + ",";
            insertSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            insertSQL += "'COMPROBANTE NIIF DETERIORO DE INVENTARIOS',";
            insertSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            insertSQL += "'" + HttpContext.Current.User.Identity.Name.ToLower() + "',";
            insertSQL += sumaDeterioro + ");";

            sqlStrings.Insert(0, insertSQL);

            insertSQL = "UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU + 1 WHERE PDOC_CODIGO='" + ViewState["prefijoNiif"] + "'";
            sqlStrings.Add(insertSQL);

            insertSQL = "UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU + 1 WHERE PDOC_CODIGO='9000'";
            sqlStrings.Add(insertSQL);

            if (DBFunctions.Transaction(sqlStrings))
                Utils.MostrarAlerta(Response, "Proceso finalizado con exito! Se ha creado el compronante: " + ViewState["prefijoNiif"]  + " - " + numeroComprobante + ". Y el registro 9000-" + numeroDocumento);
            else
            {
                Utils.MostrarAlerta(Response, "Error en el proceso!");
                lb.Text += DBFunctions.exceptions;
            }
        }

    }
}