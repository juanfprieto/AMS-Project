using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using AMS.DB;
using System.Collections;
using AMS.Tools;
using System.IO;

namespace AMS.Cartera
{
	public partial class DeterioroCartera : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                Session.Clear();

                txtPrefijoComprobante.Text = DBFunctions.SingleData("SELECT p.pdoc_codigo concat ' - ' concat p.pdoc_nombre from pdocumento p, ccontabilidad c where c.CCON_PREFNIIF = p.pdoc_codigo;");
                ViewState["prefijoNiif"] = txtPrefijoComprobante.Text.Substring(0, txtPrefijoComprobante.Text.IndexOf("-"));

                txtLiquidaYear.Text = DBFunctions.SingleData("select pano_ano from ccontabilidad;");
                txtLiquidaMonth.Text = DBFunctions.SingleData("select pmes_mes from ccontabilidad;");
               
                DataSet dsDiasMoraNiif = new DataSet();
                DBFunctions.Request(dsDiasMoraNiif, IncludeSchema.NO, "SELECT pran_diaini as DIAINI, pran_diafin as DIAFIN, pran_tasad as TASADIARIA from PRANGOSMORANIIF order by DIAINI;");
                if (dsDiasMoraNiif.Tables.Count == 0)
                {
                    Utils.MostrarAlerta(Response, "No ha definido la parametrización para los dias de rangos de mora NIIF");
                }
                else
                { 
                    dgDiasMoraNIIF.DataSource = dsDiasMoraNiif;
                    dgDiasMoraNIIF.DataBind();
                }
                DataSet dsDiasMoraLegal = new DataSet();
                DBFunctions.Request(dsDiasMoraLegal, IncludeSchema.NO, "SELECT pral_diaini as DIAINI, pral_diafin as DIAFIN, pral_tasad as TASADIARIA from PRANGOSMORALEGAL order by DIAINI;");
                if(dsDiasMoraLegal.Tables.Count == 0)
                {
                    Utils.MostrarAlerta(Response,"No ha definido la parametrización para los dias de rangos de mora LEGAL");
                }
                else
                { 
                    dgDiasMoraLEGAL.DataSource = dsDiasMoraLegal;
                    dgDiasMoraLEGAL.DataBind();
                }
            }
		}

        protected void CalcularDeterioro_Click(object Sender, EventArgs e)
        {
            String[] fechaCorte = txtFechaCorte.Text.Split('-');

            ArrayList arrHistorialDeterioro = DBFunctions.RequestAsCollection("select coalesce(sum(ddet_valor),0) as SUMLEGAL, coalesce(sum(ddet_valorniif),0) as SUMNIIF from ddeteriorocartera where pano_ano=" + fechaCorte[0] + " and pmes_mes=" + fechaCorte[1]);
            btnGenerarComprobanteLegal.Enabled = true;
            btnGenerarComprobanteNiif.Enabled = true;
            if (((Hashtable)arrHistorialDeterioro[0])["SUMLEGAL"].ToString() != "")
            {
                double sumLegal = Convert.ToDouble(((Hashtable)arrHistorialDeterioro[0])["SUMLEGAL"].ToString());
                double sumNiif = Convert.ToDouble(((Hashtable)arrHistorialDeterioro[0])["SUMNIIF"].ToString());

                if (sumLegal > 0)
                {
                    Utils.MostrarAlerta(Response, "Ya se ha realizado el comprobante de Provision para el periodo seleccionado!");
                    btnGenerarComprobanteLegal.Enabled = false;
                }
                if (sumNiif > 0)
                {
                    Utils.MostrarAlerta(Response, "Ya se ha realizado el comprobante de Deterioro para el periodo seleccionado!");
                    btnGenerarComprobanteNiif.Enabled = false;
                }
                if (sumNiif > 0 && sumLegal > 0)
                    return;
            }

            plcDeterioro.Visible = true;
            DataSet dtDeterioro = new DataSet();

            if(fechaCorte[1] == "01")
            {
                fechaCorte[0] = (Convert.ToInt16(fechaCorte[0]) - 1).ToString();
                fechaCorte[1] = "12";
            }
            else
            {
                fechaCorte[1] = (Convert.ToInt16(fechaCorte[1]) - 1).ToString();
            }

            DBFunctions.Request(dtDeterioro, IncludeSchema.NO,
                @"SELECT det.NIT as NIT, det.NOMBRE as NOMBRE, det.PREFIJO as PREFIJO, det.NUMERO as NUMERO, det.MONTO as MONTO, det.DIASMORA as DIASMORA, det.TASA as TASANIIF,
                    (det.MONTO * det.TASA) as VALORNIIF, det.FECHAVENC as FECHAVENC,
                    COALESCE((select ddet_valorniif from ddeteriorocartera where pdoc_codigo =det.PREFIJO and mfac_numedocu=det.NUMERO order by pano_ano, pmes_mes desc fetch first row only),0) as VALORANTERIORNIIF,
                    det.TASALEGAL as TASALEGAL, (det.MONTO * det.TASALEGAL) as VALORLEGAL,
                    COALESCE((select ddet_valor from ddeteriorocartera where pdoc_codigo =det.PREFIJO and mfac_numedocu=det.NUMERO order by pano_ano, pmes_mes desc fetch first row only),0) as VALORANTERIORLEGAL,
                    det.FECHAFACT as FECHAFACT
                    FROM
                    (
                    SELECT 
                    mf.mnit_nit as NIT, 
                    TRIM(vm.nombre) as NOMBRE, 
                    mf.pdoc_codigo as PREFIJO, 
                    mf.mfac_numedocu as NUMERO,
                    (mf.mfac_valofact + mf.mfac_valoiva + mf.mfac_valoflet + mf.mfac_valoivaflet - mf.mfac_valorete - mf.mfac_valoabon) as MONTO,
                    (DAYS('" + txtFechaCorte.Text + @"') - DAYS(mf.mfac_vence))  as DIASMORA,
                    CASE
                    WHEN 
                    COALESCE( (select pran_tasad from PRANGOSMORANIIF where 
                    (DAYS('" + txtFechaCorte.Text + @"') - DAYS(mf.mfac_vence)) >= pran_diaini  and
                    (DAYS('" + txtFechaCorte.Text + @"') - DAYS(mf.mfac_vence)) <= pran_diafin), 0) >= 
                    COALESCE( (select ddet_tasaefecNiif from ddeteriorocartera where pdoc_codigo =mf.pdoc_codigo and mfac_numedocu=mf.mfac_numedocu order by pano_ano, pmes_mes desc fetch first row only), 0)
                    THEN 
                    COALESCE( (select pran_tasad from PRANGOSMORANIIF where 
                    (DAYS('" + txtFechaCorte.Text + @"') - DAYS(mf.mfac_vence)) >= pran_diaini  and
                    (DAYS('" + txtFechaCorte.Text + @"') - DAYS(mf.mfac_vence)) <= pran_diafin), 0)
                    ELSE
                    COALESCE( (select ddet_tasaefecNiif from ddeteriorocartera where pdoc_codigo =mf.pdoc_codigo and mfac_numedocu=mf.mfac_numedocu order by pano_ano, pmes_mes desc fetch first row only), 0)
                    END
                    as TASA, 
                    CASE
                    WHEN 
                    COALESCE( (select pral_tasad from PRANGOSMORALEGAL where 
                    (DAYS('" + txtFechaCorte.Text + @"') - DAYS(mf.mfac_vence)) >= pral_diaini  and
                    (DAYS('" + txtFechaCorte.Text + @"') - DAYS(mf.mfac_vence)) <= pral_diafin), 0) >= 
                    COALESCE( (select ddet_tasaefec from ddeteriorocartera where pdoc_codigo =mf.pdoc_codigo and mfac_numedocu=mf.mfac_numedocu order by pano_ano, pmes_mes desc fetch first row only), 0)
                    THEN 
                    COALESCE( (select pral_tasad from PRANGOSMORALEGAL where 
                    (DAYS('" + txtFechaCorte.Text + @"') - DAYS(mf.mfac_vence)) >= pral_diaini  and
                    (DAYS('" + txtFechaCorte.Text + @"') - DAYS(mf.mfac_vence)) <= pral_diafin), 0)
                    ELSE
                    COALESCE( (select ddet_tasaefec from ddeteriorocartera where pdoc_codigo =mf.pdoc_codigo and mfac_numedocu=mf.mfac_numedocu order by pano_ano, pmes_mes desc fetch first row only), 0)
                    END
                    as TASALEGAL, 
                    mf.mfac_vence AS FECHAVENC,
                    mf.mfac_factura AS FECHAFACT
                    from mfacturacliente mf, pdocumento pd, vmnit vm where mf.tvig_vigencia IN ('V','A') 
                    and (mf.mfac_valofact + mf.mfac_valoiva + mf.mfac_valoflet + mf.mfac_valoivaflet - mf.mfac_valorete - mf.mfac_valoabon) > 0 
                    and vm.mnit_nit = mf.mnit_nit
                    and pd.pdoc_codigo = mf.pdoc_codigo and pd.tdoc_tipodocu='FC' and UPPER(pd.Pdoc_gerencial) <> 'N' 
                    and TIMESTAMPDIFF(16,CHAR( TIMESTAMP('" + txtFechaCorte.Text + @"') - TIMESTAMP(mf.mfac_vence) ) ) > 0
                    ) as det;");

            // TOCA Completar ESTA SENTENCIA COMO ESTA A CONTINUACION porque se debn incluir los documentos que tienen pago posteror a la fecha del proceso

            /*
            SELECT MF.PDOC_CODIGO, MF.MNIT_NIT,FECHAVENC, FECHAFACT, SALDO + PAGO_POSTERIOR + CRUCE_POSTERIOR AS MONTO 
FROM (
SELECT MF.PDOC_CODIGO, MF.MNIT_NIT, mf.mfac_vence AS FECHAVENC, mf.mfac_factura AS FECHAFACT, 
       SUM(mf.mfac_valofact + mf.mfac_valoiva + mf.mfac_valoflet + mf.mfac_valoivaflet - mf.mfac_valorete - mf.mfac_valoabon) AS SALDO,
       COALESCE(SUM(DCP.DDET_VALODOCU),0) AS PAGO_POSTERIOR, COALESCE(SUM(DCD.MCRU_VALOR),0) AS CRUCE_POSTERIOR
                    from PDOCUMENTO PD, mfacturacliente mf 
                         left join ddetallefacturacliente dcp 
                            LEFT JOIN MCAJA MCJ ON DCP.PDOC_CODDOCREF = MCJ.PDOC_CODIGO AND DCP.DDET_NUMEDOCU = MCJ.MCAJ_NUMERO AND MCJ.MCAJ_FECHA > '" + txtFechaCorte.Text + @"'
                         on mf.pdoc_codigo = dcp.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = DCP.MFAC_NUMEDOCU
                         left join dCRUCEDOCUMENTO dcD 
                            LEFT JOIN MCRUCEDOCUMENTO MCD ON DCD.PDOC_CODIGO = MCD.PDOC_CODIGO AND DCD.MCRU_NUMERO = MCD.MCRU_NUMERO AND MCD.MCRU_FECHA > '" + txtFechaCorte.Text + @"'
                         on mf.pdoc_codigo = dcD.MFAC_CODIGO AND MF.MFAC_NUMEDOCU = DCD.MFAC_NUMERO
                    where MF.PDOC_CODIGO = PD.PDOC_CODIGO AND PD.PDOC_GERENCIAL <> 'N' AND mf.mfac_vence <= '" + txtFechaCorte.Text + @"'
					GROUP BY  MF.PDOC_CODIGO, MF.MNIT_NIT, mf.mfac_vence, mf.mfac_factura) AS mf
 WHERE SALDO + PAGO_POSTERIOR + CRUCE_POSTERIOR > 0
            */

            Session["dtDeterioro"] = dtDeterioro;
            dgDeterioro.DataSource = dtDeterioro;
            dgDeterioro.DataBind();

            GetValorTotalDeterioro(dtDeterioro);
        }

        protected void GetValorTotalDeterioro(DataSet dsDeterioro)
        {
            double valorDeterioroNiif = 0;
            double valorDeterioroLegal = 0;

            for (int k = 0; k < dsDeterioro.Tables[0].Rows.Count; k++)
            {
                valorDeterioroNiif += Convert.ToDouble(dsDeterioro.Tables[0].Rows[k][7].ToString());
                valorDeterioroLegal += Convert.ToDouble(dsDeterioro.Tables[0].Rows[k][11].ToString());
            }

            lbValorDeterioroNiif.Text = valorDeterioroNiif.ToString("C");
            lbValorDeterioroLegal.Text = valorDeterioroLegal.ToString("C");
        }

        protected void GenerarComprobante_Click(object Sender, EventArgs e)
        {
            Button btnSender = (Button)Sender;
            String[] fechaCorte = txtFechaCorte.Text.Split('-');
            bool deterioro = btnSender.Text.ToString().Contains("NIIF");
            
            //Registro del deterioro
            ArrayList sqlStrings = new ArrayList();
            string insertSQL = "";
            double sumaDeterioroLegal = 0;
            double sumaDeterioroNiif = 0;
            DataSet dsDeterioro = new DataSet();
            dsDeterioro = (DataSet)Session["dtDeterioro"];
            //string cuentaDeteCarDebi = DBFunctions.SingleData("select ccon_detecartdebi from CCONTABILIDAD");
            //string cuentaDeteCarCre = DBFunctions.SingleData("select ccon_detecartcred from CCONTABILIDAD");
            
            ArrayList arrContabilidad = DBFunctions.RequestAsCollection("select CCON_CARTREALDETE, CCON_CARTREALPROV, CCON_CARTREALNOMI  from ccontabilidad;");

            string cuentaDeteCarCre = "";
            try
            {
                cuentaDeteCarCre = ((Hashtable)arrContabilidad[0])["CCON_CARTREALDETE"].ToString();
            }
            catch
            {
                cuentaDeteCarCre = "CCON_CARTREALDETE";
            }

            string cuentaProvCarCre = "";
            try
            {
                cuentaProvCarCre = ((Hashtable)arrContabilidad[0])["CCON_CARTREALPROV"].ToString();
            }
            catch
            {
                cuentaProvCarCre = "CCON_CARTREALPROV";
            }

            string cuentaNominalDebi = "";
            try
            {
                cuentaNominalDebi = ((Hashtable)arrContabilidad[0])["CCON_CARTREALNOMI"].ToString();
            }
            catch
            {
                cuentaNominalDebi = "CCON_CARTREALNOMI";
            }

            string almacenAuxiliar = DBFunctions.SingleData("select palm_almacen from ccontabilidad;");
            string centroCostroAux = DBFunctions.SingleData("select p.pcen_codigo from ccontabilidad c, pcentrocosto p where p.pcen_codigo = c.pcen_codigo and p.timp_codigo != 'N' and p.timp_codigo != '';");
            string numeroComprobante = DBFunctions.SingleData("SELECT PDOC_ULTIDOCU + 1 FROM pdocumento WHERE PDOC_CODIGO='" + ViewState["prefijoNiif"] + "'");
            int contSecuencia = 1;

            if (centroCostroAux == "")
            {
                Utils.MostrarAlerta(Response, "Por favor parametrizar adecuadamente el centro de costo en configuración de contabilidad!");
                return;
            }

            DataSet dsRegistrosActuales = new DataSet();
            DBFunctions.Request(dsRegistrosActuales, IncludeSchema.NO,
                "select pdoc_codigo, mfac_numedocu, mnit_nit from ddeteriorocartera where pano_ano=" + fechaCorte[0] + " and pmes_mes= " + fechaCorte[1]);

            //Detalles del deterioro.
            for (int k = 0; k < dsDeterioro.Tables[0].Rows.Count; k++)
            {
                double valorDeterioroNiif = Convert.ToDouble(dsDeterioro.Tables[0].Rows[k]["VALORNIIF"].ToString());
                double valorUltimoDeterioroNiif = Convert.ToDouble(dsDeterioro.Tables[0].Rows[k]["VALORANTERIORNIIF"].ToString());
                double valorDeterioroLegal = Convert.ToDouble(dsDeterioro.Tables[0].Rows[k]["VALORLEGAL"].ToString());
                double valorUltimoDeterioroLegal = Convert.ToDouble(dsDeterioro.Tables[0].Rows[k]["VALORANTERIORLEGAL"].ToString());
                double tasaLegal = Convert.ToDouble(dsDeterioro.Tables[0].Rows[k]["TASALEGAL"].ToString());
                double tasaNiif = Convert.ToDouble(dsDeterioro.Tables[0].Rows[k]["TASANIIF"].ToString());

                double diferenciaDeterioroNiif = valorDeterioroNiif - valorUltimoDeterioroNiif;
                double diferenciaDeterioroLegal = valorDeterioroLegal - valorUltimoDeterioroLegal;

                double difDetReversionLegal = 0;
                double difDetReversionNiif = 0;
                int contSecuenciaAux = contSecuencia;
                bool crearDetalle = false;

                if (deterioro == false && valorDeterioroLegal > 0 && diferenciaDeterioroLegal != 0)
                {
                    //Validar si hay reversion de saldo
                    if (diferenciaDeterioroLegal < 0)
                    {
                        difDetReversionLegal = diferenciaDeterioroLegal * -1;
                        diferenciaDeterioroLegal = 0;
                    }
                    //Detales de comprobante de Provision
                    CrearDetallesComprobante(k, dsDeterioro, cuentaNominalDebi, cuentaProvCarCre, diferenciaDeterioroLegal, difDetReversionLegal, 0, 0, numeroComprobante, almacenAuxiliar, centroCostroAux, ref contSecuencia, ref sqlStrings);
                    tasaNiif = 0;
                    valorDeterioroNiif = 0;
                    crearDetalle = true;
                }

                if (deterioro == true && valorDeterioroNiif > 0 && diferenciaDeterioroNiif != 0)
                {
                    //Validar si hay reversion de saldo
                    if (diferenciaDeterioroNiif < 0)
                    {
                        difDetReversionNiif = diferenciaDeterioroNiif * -1;
                        diferenciaDeterioroNiif = 0;
                    }
                    //Detalles de comprobante de Deterioro
                    CrearDetallesComprobante(k, dsDeterioro, cuentaNominalDebi, cuentaDeteCarCre, 0, 0, diferenciaDeterioroNiif, difDetReversionNiif, numeroComprobante, almacenAuxiliar, centroCostroAux, ref contSecuencia, ref sqlStrings);
                    tasaLegal = 0;
                    valorDeterioroLegal = 0;
                    crearDetalle = true;
                }


                DataRow[] drRegistro = dsRegistrosActuales.Tables[0].Select(
                    "pdoc_codigo='" + dsDeterioro.Tables[0].Rows[k]["PREFIJO"].ToString() + "' AND mfac_numedocu=" + dsDeterioro.Tables[0].Rows[k]["NUMERO"].ToString() + " AND mnit_nit='" + dsDeterioro.Tables[0].Rows[k]["NIT"].ToString() + "'");

                if (drRegistro.Length == 0 && crearDetalle)
                {
                    //ddeteriorocartera
                    insertSQL = "INSERT INTO ddeteriorocartera VALUES(";
                    insertSQL += fechaCorte[0] + ",";
                    insertSQL += fechaCorte[1] + ",";
                    insertSQL += "'" + dsDeterioro.Tables[0].Rows[k]["PREFIJO"].ToString() + "',";
                    insertSQL += dsDeterioro.Tables[0].Rows[k]["NUMERO"].ToString() + ",";
                    insertSQL += "'" + dsDeterioro.Tables[0].Rows[k]["NIT"].ToString() + "',";
                    insertSQL += dsDeterioro.Tables[0].Rows[k]["MONTO"].ToString() + ",";
                    insertSQL += dsDeterioro.Tables[0].Rows[k]["DIASMORA"].ToString() + ",";
                    insertSQL += tasaLegal + ",";
                    insertSQL += valorDeterioroLegal + ",";
                    insertSQL += "'" + Convert.ToDateTime(dsDeterioro.Tables[0].Rows[k]["FECHAVENC"].ToString()).ToString("yyyy-MM-dd") + "',";
                    insertSQL += tasaNiif+ ",";
                    insertSQL += valorDeterioroNiif + ");";
                }
                else if (crearDetalle)
                {
                    //ddeteriorocartera
                    insertSQL = "UPDATE DBXSCHEMA.DDETERIOROCARTERA SET ";

                    if (deterioro == false)
                    {
                        insertSQL += "DDET_TASAEFEC = " + tasaLegal + ", ";
                        insertSQL += "DDET_VALOR = " + valorDeterioroLegal;
                    }
                    else
                    {
                        insertSQL += "DDET_TASAEFECNIIF = " + tasaNiif + ", ";
                        insertSQL += "DDET_VALORNIIF = " + valorDeterioroNiif;
                    }

                    insertSQL += " WHERE PANO_ANO = " + fechaCorte[0];
                    insertSQL += " AND   PMES_MES = " + fechaCorte[1];
                    insertSQL += " AND   PDOC_CODIGO = '" + dsDeterioro.Tables[0].Rows[k]["PREFIJO"].ToString() + "'";
                    insertSQL += " AND   MFAC_NUMEDOCU = " + dsDeterioro.Tables[0].Rows[k]["NUMERO"].ToString();
                    insertSQL += " AND   MNIT_NIT = '" + dsDeterioro.Tables[0].Rows[k]["NIT"].ToString() + "'";
                }

                if (crearDetalle)
                    sqlStrings.Add(insertSQL);

                sumaDeterioroLegal += diferenciaDeterioroLegal + difDetReversionLegal;
                sumaDeterioroNiif += diferenciaDeterioroNiif + difDetReversionNiif;
                crearDetalle = false;
            }

            //Creacion del comprobante
            insertSQL = "INSERT INTO MCOMPROBANTE VALUES('" + ViewState["prefijoNiif"] + "',";
            insertSQL += numeroComprobante + ",";
            insertSQL += fechaCorte[0] + ",";
            insertSQL += fechaCorte[1] + ",";
            insertSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            if (deterioro == false)
                insertSQL += "'COMPROBANTE NIIF PROVISION DE CARTERA',";
            else
                insertSQL += "'COMPROBANTE NIIF DETERIORO DE CARTERA',";
            insertSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            insertSQL += "'" + HttpContext.Current.User.Identity.Name.ToLower() + "',";
            if (deterioro == false)
                insertSQL += sumaDeterioroLegal + ");";
            else
                insertSQL += sumaDeterioroNiif + ");";

            sqlStrings.Insert(0, insertSQL);

            insertSQL = "UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU + 1 WHERE PDOC_CODIGO='" + ViewState["prefijoNiif"] + "'";
            sqlStrings.Add(insertSQL);

            string info = "";
            if (deterioro == false)
                info = "Provision";
            else
                info = "Deterioro";

            //sqlStrings.Add("INSERT WHERE U;");
            
            if (DBFunctions.Transaction(sqlStrings))
                Utils.MostrarAlerta(Response, "Proceso finalizado con exito! Se ha creado el compronante de " + info + ": " + ViewState["prefijoNiif"]  + " - " + numeroComprobante );
            else
            {
                Utils.MostrarAlerta(Response, "Error en el proceso!");
                lb.Text += DBFunctions.exceptions;
            }
        }

        public void CrearDetallesComprobante(int k, DataSet dsDeterioro, string cuentaDebito, string cuentaCredito, double diferenciaDeterioroLegal, double difDetReversionLegal, double diferenciaDeterioroNiif, double difDetReversionNiif, string numeroComprobante, string almacenAuxiliar, string centroCostroAux, ref int contSecuencia, ref ArrayList sqlStrings)
        {
            string insertSQL = "";
            string observacion = "";

            if(diferenciaDeterioroLegal == 0 && difDetReversionLegal == 0)
                observacion = "DETERIORO CARTERA NIIF";
            else
                observacion = "DETERIORO CARTERA LEGAL";
            
                //DETALLES.......
            //dcuenta debito
            insertSQL = "INSERT INTO DCUENTA VALUES(";
            insertSQL += "'" + ViewState["prefijoNiif"] + "',";
            insertSQL += numeroComprobante + ",";
            insertSQL += "'" + cuentaDebito + "',"; ;
            insertSQL += "'" + dsDeterioro.Tables[0].Rows[k]["PREFIJO"].ToString() + "',";
            insertSQL += dsDeterioro.Tables[0].Rows[k]["NUMERO"].ToString() + ",";
            insertSQL += contSecuencia + ",";
            insertSQL += "'" + dsDeterioro.Tables[0].Rows[k]["NIT"].ToString() + "',";
            insertSQL += "'" + almacenAuxiliar + "',";
            insertSQL += "'" + centroCostroAux + "',";
            insertSQL += "'" + observacion + "',";
            insertSQL += diferenciaDeterioroLegal + ",";
            insertSQL += difDetReversionLegal + ",";
            insertSQL += "0,";
            insertSQL += diferenciaDeterioroNiif + ",";
            insertSQL += difDetReversionNiif + ");";

            contSecuencia++;
            sqlStrings.Add(insertSQL);

            //dcuenta credito
            insertSQL = "INSERT INTO DCUENTA VALUES(";
            insertSQL += "'" + ViewState["prefijoNiif"] + "',";
            insertSQL += numeroComprobante + ",";
            insertSQL += "'" + cuentaCredito + "',"; ;
            insertSQL += "'" + dsDeterioro.Tables[0].Rows[k]["PREFIJO"].ToString() + "',";
            insertSQL += dsDeterioro.Tables[0].Rows[k]["NUMERO"].ToString() + ",";
            insertSQL += contSecuencia + ",";
            insertSQL += "'" + dsDeterioro.Tables[0].Rows[k]["NIT"].ToString() + "',";
            insertSQL += "'" + almacenAuxiliar + "',";
            insertSQL += "'" + centroCostroAux + "',";
            insertSQL += "'" + observacion + "',";
            insertSQL += difDetReversionLegal + ",";
            insertSQL += diferenciaDeterioroLegal + ",";
            insertSQL += "0,";
            insertSQL += difDetReversionNiif + ",";
            insertSQL += diferenciaDeterioroNiif + ");";

            contSecuencia++;
            sqlStrings.Add(insertSQL);
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
            double tasaNuevaNiif = Convert.ToDouble(((TextBox)e.Item.Cells[0].FindControl("txtEditTasaNiif")).Text.ToString());
            double tasaNuevaLegal = Convert.ToDouble(((TextBox)e.Item.Cells[0].FindControl("txtEditTasaLegal")).Text.ToString());
            //double tasaAnteriorNiif = Convert.ToDouble(dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][6].ToString());
            //double tasaAnteriorLegal = Convert.ToDouble(dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][10].ToString());
            double monto = Convert.ToDouble(dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][4].ToString());

            double valorNiif = monto * tasaNuevaNiif;
            double valorLegal = monto * tasaNuevaLegal;

            dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][6] = tasaNuevaNiif;
            dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][7] = valorNiif;
            dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][10] = tasaNuevaLegal;
            dsDeterioro.Tables[0].Rows[dgDeterioro.EditItemIndex][11] = valorLegal;

            Session["dtDeterioro"] = dsDeterioro;
            dgDeterioro.EditItemIndex = -1;
            dgDeterioro.EnableViewState = true;
            dgDeterioro.DataSource = dsDeterioro;
            dgDeterioro.DataBind();

            GetValorTotalDeterioro(dsDeterioro);
        }


        protected void ImprimirExcelGrid(Object Sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = (DataSet)Session["dtDeterioro"];
                Utils.ImprimeExcel(ds, "DeterioroCartera");
            }
            catch (Exception ex)
            {
                return;
            }
        }

	}
}