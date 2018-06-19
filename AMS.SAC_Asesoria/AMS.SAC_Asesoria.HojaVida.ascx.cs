using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AMS.DB;
using System.Configuration;
using System.Collections;
using System.Web.UI.HtmlControls;
using AMS.Tools;


namespace AMS.SAC_Asesoria
{
	public partial class HojaVida : System.Web.UI.UserControl
	{
        public string variableWhere = "";
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                llenarDdlPrincipal();
                ddlMenuPrin_OnSelectedIndexChanged(sender, e);
                tipoSolicitud_OnSelectedIndexChanged(sender, e);
            }
		}

        private void llenarDdlPrincipal()
        {
            Utils.FillDll(ddlMenuPrincipal, "SELECT SMEN_PRINCI, SMEN_NOMBRE_PRINCIPAL FROM SMENUPRINCIPAL ORDER BY 1", true);
            Utils.FillDll(tipoSolicitud, "SELECT TASE_CODIGO, TASE_NOMBRE FROM TASESORIA", true);
        }

        protected void ddlMenuPrin_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Utils.FillDll(ddlmenuCarpeta, "SELECT SMEN_CARPETA , SMEN_NOMBRE_CARPETA  \n" +
                                         "FROM SMENUCARPETA \n" +
                                         "WHERE SMEN_PRINCI = '" + ddlMenuPrincipal.SelectedValue + "' \n" +
                                         "ORDER BY 1", true);
            if (ddlMenuPrincipal.SelectedValue != "0")
            {
                variableWhere = "AND SP.SMEN_PRINCI = " + ddlMenuPrincipal.SelectedValue + " ";
                ViewState["variableWhere"] = variableWhere;
                if (ddlmenuCarpeta.Items.Count == 1)
                {
                    ddlmenuCarpeta_OnSelectedIndexChanged(sender, e);
                }
                else { cargar_HojaVida(variableWhere);  }
            }
        }

        protected void ddlmenuCarpeta_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Utils.FillDll(ddlmenuOpcion, "SELECT  SMEN_OPCION ,SMEN_NOMBRE_OPCION  \n" +
                                         "FROM SMENUOPCION \n" +
                                         "WHERE SMEN_CARPETA = '" + ddlmenuCarpeta.SelectedValue + "' \n" +
                                         "ORDER BY 1", true);

            if (ddlMenuPrincipal.SelectedValue != "0" && ddlmenuCarpeta.SelectedValue != "0")
            {
                variableWhere = "AND SP.SMEN_PRINCI = " + ddlMenuPrincipal.SelectedValue + " " +
                                "AND SC.SMEN_CARPETA =" + ddlmenuCarpeta.SelectedValue;
                ViewState["variableWhere"] = variableWhere;
                cargar_HojaVida(variableWhere);
            }
            
        }

        protected void ddlmenuOpcion_OnSelectedIndexChanged (object sender, EventArgs e)
        {
            if (ddlMenuPrincipal.SelectedValue != "0" && ddlmenuCarpeta.SelectedValue != "0" && ddlmenuOpcion.SelectedValue != "0")
            {
                variableWhere = "AND SP.SMEN_PRINCI =" + ddlMenuPrincipal.SelectedValue + " " +
                                "AND SC.SMEN_CARPETA =" + ddlmenuCarpeta.SelectedValue + " " +
                                "AND SO.SMEN_OPCION =" + ddlmenuOpcion.SelectedValue + " ";
                ViewState["variableWhere"] = variableWhere;
                cargar_HojaVida(variableWhere);
            }
        }

        protected void tipoSolicitud_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string varWhere = "";
            if (ViewState["variableWhere"] != null)
            {
                varWhere = ViewState["variableWhere"].ToString();
            }
            string whereTipoError = " AND DOS.TASE_CODIGO = '" + tipoSolicitud.SelectedValue + "'";
            cargar_HojaVida(varWhere + whereTipoError);
        }

        protected void cargar_HojaVida(string variable)
        {
            try
            {
                string sql = String.Format(@"SELECT DOBJ_SECUENCIA,
                                           SP.SMEN_NOMBRE_PRINCIPAL CONCAT '/' CONCAT SC.SMEN_NOMBRE_CARPETA CONCAT '/' CONCAT SO.SMEN_NOMBRE_OPCION AS RUTA,
                                           DOBJ_ACCION,
                                           DOBJ_FECHACTU,
                                           DOBJ_EJECUTOR,
                                           TASE_CODIGO
                                    FROM DOBJETOECAS DO,
                                         SMENUOPCION SO,
                                         SMENUCARPETA SC,
                                         SMENUPRINCIPAL SP,
                                         DORDENSERVICIO DOS
                                    WHERE SC.SMEN_PRINCI = SP.SMEN_PRINCI
                                    AND   SO.SMEN_CARPETA = SC.SMEN_CARPETA
                                    AND   DO.MOBJ_ID = SO.SMEN_OPCION
                                    AND   DOS.MORD_NUMERO = DO.MORD_NUMERO                             
                                    " + variable + @"
                                    AND   DOBJ_FECHACTU BETWEEN CURRENT DATE -30 DAY AND CURRENT DATE
                                    ORDER BY DOBJ_FECHACTU DESC");

            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
            string codigoHTML = "";

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                codigoHTML += "<div class='HojaVida'>";

                codigoHTML += "<div class='numero'>";
                codigoHTML += ds.Tables[0].Rows[i][0].ToString();
                codigoHTML += "</div>";

                codigoHTML += "<div class='ruta'>";
                codigoHTML += ds.Tables[0].Rows[i][1].ToString();
                codigoHTML += "</div>";

                codigoHTML += "<div class='fechaHoja'>";
                codigoHTML += ds.Tables[0].Rows[i][3].ToString();
                codigoHTML += "</div>";

                codigoHTML += "<fieldset class='testCambios'>";
                codigoHTML += "<table>";
                codigoHTML += "<tr>";
                codigoHTML += "<td>";
                codigoHTML += ds.Tables[0].Rows[i][2].ToString();
                codigoHTML += "<td>"; 
                codigoHTML += "<tr>";
                codigoHTML += "<legend>";
                codigoHTML += ds.Tables[0].Rows[i][4].ToString();
                codigoHTML += "</legend>";
                codigoHTML += "</table>";
                codigoHTML += "</fieldset>";

                codigoHTML += "</div>";
            }

            DivHojaVida.InnerHtml = codigoHTML;
           
            }
            catch {
                Utils.MostrarAlerta(Response, "No hay Ningun Cambio en el Modulo");
            }
        }

	}
}