using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using AMS.DB;
using System.Data;
using System.Collections;
using AMS.Tools;

namespace AMS.Contabilidad
{
	public partial class ValorizaActivosNIIF : System.Web.UI.UserControl
	{
        protected ArrayList sqlStrings = new ArrayList();
        protected int contSecuencia;

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlAno, "SELECT pano_ano FROM pano where pano_ano >= (select pano_ano from ccontabilidad) order by 1");
                bind.PutDatasIntoDropDownList(ddlMes, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 13");
                bind.PutDatasIntoDropDownList(tComprobante, "SELECT PDOC_CODIGO, PDOC_CODIGO ||' - ' ||pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='CC' ORDER BY PDOC_NOMBRE");
            }
		}

        protected void Efectuar_Valorizacion(Object Sender, EventArgs e)
        {
            sqlStrings = new ArrayList();
            string insertSQL = "";
            double sumaDebitos = 0;
            DataSet dsDeterioro = new DataSet();
            dsDeterioro = (DataSet)Session["dtDeterioro"];
            string nitEmpresa = DBFunctions.SingleData("select mnit_nit from cempresa;");
            string cuentaValorizaNIIF = DBFunctions.SingleData("select CCON_VALORIZANIIF from CCONTABILIDAD");
            string cuentaDesvalorNIIF = DBFunctions.SingleData("select CCON_DESVALORNIIF from CCONTABILIDAD");
            string cuentaPatrimonio   = DBFunctions.SingleData("select CCON_PATRVALORIZA from CCONTABILIDAD");
            string numeroComprobante  = DBFunctions.SingleData("SELECT PDOC_ULTIDOCU + 1 FROM pdocumento WHERE PDOC_CODIGO='"+tComprobante.SelectedValue+"'");
            contSecuencia = 1;
            string almacenAux = DBFunctions.SingleData("select palm_almacen from palmacen where tvig_vigencia = 'V' fetch first row only;");

            DataSet dsActivos = new DataSet();
            DBFunctions.Request(dsActivos, IncludeSchema.NO, 
                @"select mafj_codiacti as codigo, mafj_valohist as VALORHISTORICO, mafj_valomejora as VALORMEJORA, mafj_valoactniif as VALORACTIVONIIF,
                  mafj_valomejoraniif as VALORMEJORANIIF, pcco_centcost as CENTROCOSTO from mactivofijo;");

            //Creacion del comprobante de deterioro
            insertSQL = "INSERT INTO MCOMPROBANTE VALUES('" + tComprobante.SelectedValue + "',";
            insertSQL += numeroComprobante + ",";
            insertSQL += ddlAno.SelectedValue + ",";
            insertSQL += ddlMes.SelectedValue + ",";
            int diaFin = 31;
            if (ddlMes.SelectedValue == "2")
                diaFin = 28;
            else if (ddlMes.SelectedValue == "4" || ddlMes.SelectedValue == "6" || ddlMes.SelectedValue == "9" || ddlMes.SelectedValue == "11")
                    diaFin = 30;
            insertSQL += "'" +  ddlAno.SelectedValue  + "-" +  ddlMes.SelectedValue  + "-" +  diaFin  + "'" + "," ;
            insertSQL += "'VALORIZACION ACTIVOS FIJOS NIIF',";
            insertSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            insertSQL += "'" + HttpContext.Current.User.Identity.Name.ToLower() + "',";
            insertSQL += sumaDebitos + ");";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 

            sqlStrings.Add(insertSQL);

            for (int g = 0; g < dsActivos.Tables[0].Rows.Count; g++)
            {
                String centroCosto   = dsActivos.Tables[0].Rows[g]["CENTROCOSTO"].ToString();
                double valActivoNIIF = Convert.ToDouble(dsActivos.Tables[0].Rows[g]["VALORACTIVONIIF"]);
                double valMejoraNIIF = Convert.ToDouble(dsActivos.Tables[0].Rows[g]["VALORMEJORANIIF"]);
                double valHistorico  = Convert.ToDouble(dsActivos.Tables[0].Rows[g]["VALORHISTORICO"]);
                double valMejora     = Convert.ToDouble(dsActivos.Tables[0].Rows[g]["VALORMEJORA"]);
                double diferenciaValores = valActivoNIIF + valMejoraNIIF - valHistorico - valMejora;

                if (diferenciaValores > 0)
                {
                    ObtenerInsertDetalleCuenta(1, numeroComprobante, cuentaValorizaNIIF, nitEmpresa, almacenAux, centroCosto, diferenciaValores, cuentaPatrimonio);
                    sumaDebitos += diferenciaValores;
                }
                else if (diferenciaValores < 0)
                {
                        ObtenerInsertDetalleCuenta(0, numeroComprobante, cuentaDesvalorNIIF, nitEmpresa, almacenAux, centroCosto, diferenciaValores, cuentaPatrimonio);
                }
            }

            insertSQL = "UPDATE Mcomprobante set mcom_valor=" + sumaDebitos + " where pdoc_codigo = '" + tComprobante.SelectedValue + "' and mcom_numedocu=" + numeroComprobante;
            sqlStrings.Add(insertSQL);

            insertSQL = "UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU + 1 WHERE PDOC_CODIGO='" + tComprobante.SelectedValue + "' ";
            sqlStrings.Add(insertSQL);

            if (DBFunctions.Transaction(sqlStrings))
                Utils.MostrarAlerta(Response, "Proceso finalizado con exito! Se ha creado el compronante: '" + tComprobante.SelectedValue + "'-" + numeroComprobante);
            else
            {
                Utils.MostrarAlerta(Response, "Error en el proceso!");
                lb.Text += DBFunctions.exceptions;
            }
        }

        protected void ObtenerInsertDetalleCuenta(double diferencia, string numeroComprobante, string cuenta, string nitEmpresa, string almacenAux, 
                                    string centroCostoAux, double valoDiferencia, string cuentaPatrimonio)
        {
            string insertSQL = "";
            string cuentaDebito = "";
            string cuentaCredito = "";

            //creacion de detalles.
            if (diferencia == 1) //Diferencia mayor.
            {
                cuentaDebito = cuenta;
                cuentaCredito = cuentaPatrimonio;
            }
            else //Direfencia menor.
            {
                cuentaDebito = cuentaPatrimonio;
                cuentaCredito = cuenta;
            }

            //dcuenta debito
            insertSQL = "INSERT INTO DCUENTA VALUES(";
            insertSQL += "'" + tComprobante.SelectedValue + "',";
            insertSQL += numeroComprobante + ",";
            insertSQL += "'" + cuentaDebito + "',"; ;
            insertSQL += "'" + tComprobante.SelectedValue + "',";
            insertSQL += numeroComprobante + ",";
            insertSQL += contSecuencia + ",";
            insertSQL += "'" + nitEmpresa + "',";
            insertSQL += "'" + almacenAux + "',";
            insertSQL += "'" + centroCostoAux + "',";
            insertSQL += "'VALORIZACION ACTIVOS FIJOS NIIF',";
            insertSQL += "0,0,0,";
            insertSQL += valoDiferencia + ",";
            insertSQL += "0);";

            contSecuencia++;
            sqlStrings.Add(insertSQL);

            //dcuenta credito
            insertSQL = "INSERT INTO DCUENTA VALUES(";
            insertSQL += "'" + tComprobante.SelectedValue + "',";
            insertSQL += numeroComprobante + ",";
            insertSQL += "'" + cuentaCredito + "',"; ;
            insertSQL += "'" + tComprobante.SelectedValue + "',";
            insertSQL += numeroComprobante + ",";
            insertSQL += contSecuencia + ",";
            insertSQL += "'" + nitEmpresa + "',";
            insertSQL += "'" + almacenAux + "',";
            insertSQL += "'" + centroCostoAux + "',";
            insertSQL += "'VALORIZACION ACTIVOS FIJOS NIIF',";
            insertSQL += "0,0,0,";
            insertSQL += "0,";
            insertSQL += valoDiferencia + ");";

            contSecuencia++;
            sqlStrings.Add(insertSQL);

        }
	}
}