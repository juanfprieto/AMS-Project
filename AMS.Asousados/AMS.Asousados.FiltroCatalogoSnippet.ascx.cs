using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Reportes;
using AMS.Tools;
using System.Collections;
using AMS.DB;

namespace AMS.Asousados
{
	public partial class FiltroCatalogoSnippet : System.Web.UI.UserControl, IFiltroSnippet
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                LlenarDatosIniciales();
            }
		}

        private void LlenarDatosIniciales()
        {
            string sql = "SELECT DISTINCT PCC.PCLA_CODIGO, \n" +
             "       PCC.PCLA_NOMBRE \n" +
             "FROM PCLASEVEHICULO PCC, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE PCC.PCLA_CODIGO = PCV.PCLA_CODIGO";

            Utils.FillDll(ddlClaseVeh, sql, true);
        }

        protected void ddlClaseVeh_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT PMV.PMAR_CODIGO, \n" +
             "       PMV.PMAR_NOMBRE \n" +
             "FROM PMARCA PMV, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE PMV.PMAR_CODIGO = PCV.PMAR_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}'"
             , ddlClaseVeh.SelectedValue);

            Utils.FillDll(ddlMarca, sql, true);
            ddlMarca_OnSelectedIndexChanged(sender, e); //reiniciará los ddl a partir de la selección
        }

        protected void ddlMarca_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT PGC.PGRU_GRUPO, \n" +
             "       PGC.PGRU_NOMBRE \n" +
             "FROM PGRUPOCATALOGO PGC, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE PGC.PGRU_GRUPO = PCV.PGRU_GRUPO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue);

            Utils.FillDll(ddlRefPrincipal, sql, true);

            ddlRefPrincipal_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlRefPrincipal_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT PSGC.PSGRU_GRUPO, " +
             "       PSGC.PSGRU_NOMBRE " +
             "FROM PCATALOGOVEHICULO PCV  " +
             "  LEFT JOIN PSUBGRUPOVEHICULO PSGC ON PSGC.PSGRU_GRUPO = PCV.PSGRU_CODIGO " +
             "WHERE PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}' \n" +
             "AND   PCV.PGRU_GRUPO = '{2}'"
              , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue);

            Utils.FillDll(ddlRefComplementaria, sql, true);

            ddlRefComplementaria_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlRefComplementaria_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT PC.PCAR_CODIGO, \n" +
             "       PC.PCAR_NOMBRE \n" +
             "FROM PCARROCERIA PC, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE PC.PCAR_CODIGO = PCV.PCAR_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}' \n" +
             "AND   PCV.PGRU_GRUPO = '{2}' \n" +
             "AND   PCV.PSGRU_CODIGO = '{3}' "
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             );

            Utils.FillDll(ddlCarroceria, sql, true);
            ddlCarroceria_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlCarroceria_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT RC.RCIL_CODIGO, \n" +
             "       RC.RCIL_NOMBRE \n" +
             "FROM RCILINDRAJEMOTOR RC, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE RC.RCIL_CODIGO = PCV.RCIL_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}' \n" +
             "AND   PCV.PGRU_GRUPO = '{2}' \n" +
             "AND   PCV.PSGRU_CODIGO = '{3}' \n" +
             "AND   PCV.PCAR_CODIGO = '{4}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue);

            Utils.FillDll(ddlCilindraje, sql, true);
            ddlCilindraje_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlCilindraje_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT pcm.PCOM_CODIGO, \n" +
             "       pcm.PCOM_NOMBRE \n" +
             "FROM PcombustibleMOTOR pcm, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE pcm.PCOM_CODIGO = PCV.PCOM_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}' \n" +
             "AND   PCV.PGRU_GRUPO = '{2}' \n" +
             "AND   PCV.PSGRU_CODIGO = '{3}' \n" +
             "AND   PCV.PCAR_CODIGO = '{4}' \n" +
             "AND   PCV.RCIL_CODIGO = '{5}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue
             , ddlCilindraje.SelectedValue);

            Utils.FillDll(ddlTipoCombustible, sql, true);
            ddlTipoCombustible_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlTipoCombustible_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT Ra.Rali_CODIGO, \n" +
             "       Ra.Rali_NOMBRE \n" +
             "FROM RalimentacionMOTOR Ra, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE Ra.Rali_CODIGO = PCV.Rali_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}' \n" +
             "AND   PCV.PGRU_GRUPO = '{2}' \n" +
             "AND   PCV.PSGRU_CODIGO = '{3}' \n" +
             "AND   PCV.PCAR_CODIGO = '{4}' \n" +
             "AND   PCV.RCIL_CODIGO = '{5}' \n" +
             "AND   PCV.PCOM_CODIGO = '{6}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue
             , ddlCilindraje.SelectedValue
             , ddlTipoCombustible.SelectedValue);

            Utils.FillDll(ddlAspiracion, sql, true);
            ddlAspiracion_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlAspiracion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT Rt.Rtra_CODIGO, \n" +
             "       Rt.Rtra_DESCRIPC \n" +
             "FROM RtraccionVEHICULO Rt, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE Rt.Rtra_CODIGO = PCV.Rtra_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}' \n" +
             "AND   PCV.PGRU_GRUPO = '{2}' \n" +
             "AND   PCV.PSGRU_CODIGO = '{3}' \n" +
             "AND   PCV.PCAR_CODIGO = '{4}' \n" +
             "AND   PCV.RCIL_CODIGO = '{5}' \n" +
             "AND   PCV.PCOM_CODIGO = '{6}' \n" +
             "AND   PCV.RALI_CODIGO = '{7}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue
             , ddlCilindraje.SelectedValue
             , ddlTipoCombustible.SelectedValue
             , ddlAspiracion.SelectedValue);

            Utils.FillDll(ddlTraccion, sql, true);
            ddlTraccion_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlTraccion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT RCC.RCAJ_CODIGO, \n" +
             "       RCC.RCAJ_NOMBRE \n" +
             "FROM RcajaCAMBIOS RCC, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE RCC.RCAJ_CODIGO = PCV.RCAJ_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}' \n" +
             "AND   PCV.PGRU_GRUPO = '{2}' \n" +
             "AND   PCV.PSGRU_CODIGO = '{3}' \n" +
             "AND   PCV.PCAR_CODIGO = '{4}' \n" +
             "AND   PCV.RCIL_CODIGO = '{5}' \n" +
             "AND   PCV.PCOM_CODIGO = '{6}' \n" +
             "AND   PCV.RALI_CODIGO = '{7}' \n" +
             "AND   PCV.RTRA_CODIGO = '{8}';"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue
             , ddlCilindraje.SelectedValue
             , ddlTipoCombustible.SelectedValue
             , ddlAspiracion.SelectedValue
             , ddlTraccion.SelectedValue);

            Utils.FillDll(ddlCaja, sql, true);
            DatosCatalogo_OnSelectedIndexChanged(sender, e);
        }

        protected void DatosCatalogo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CambiarVisibilidadPHCatalogo();
            if (IsCatalogoFilled()) LlenarCatalogo();
        }

        private bool IsCatalogoFilled()
        {
            bool resp = true;

            resp &= ddlClaseVeh.Items.Count > 0 ? ddlClaseVeh.SelectedValue != "0" : true;
            resp &= ddlMarca.Items.Count > 0 ? ddlMarca.SelectedValue != "0" : true;
            resp &= ddlRefPrincipal.Items.Count > 0 ? ddlRefPrincipal.SelectedValue != "0" : true;
            resp &= ddlRefComplementaria.Items.Count > 0 ? ddlRefComplementaria.SelectedValue != "0" : true;
            resp &= ddlCarroceria.Items.Count > 0 ? ddlCarroceria.SelectedValue != "0" : true;
            resp &= ddlCilindraje.Items.Count > 0 ? ddlCilindraje.SelectedValue != "0" : true;
            resp &= ddlTipoCombustible.Items.Count > 0 ? ddlTipoCombustible.SelectedValue != "0" : true;
            resp &= ddlAspiracion.Items.Count > 0 ? ddlAspiracion.SelectedValue != "0" : true;
            resp &= ddlCaja.Items.Count > 0 ? ddlCaja.SelectedValue != "0" : true;

            return resp;
        }

        private void LlenarCatalogo()
        {

            ArrayList catalogos = ObtenerCatalogos();
            string catalogosStr = "(";

            foreach (Hashtable hash in catalogos)
                catalogosStr += String.Format("'{0}',", hash["PCAT_CODIGO"].ToString());

            if (catalogosStr.Length > 1)
                catalogosStr =
                    catalogosStr.Substring(0, catalogosStr.Length - 1) + ")";

            string sql = String.Format(
             "SELECT PV.PCAT_CODIGO,  \n" +
             "       pcv.PCLA_NOMBRE CONCAT ' ' CONCAT   \n" +
             "       pm.PMAR_NOMBRE CONCAT ' ' CONCAT   \n" +
             "       pg.PGRU_NOMBRE CONCAT ' ' CONCAT   \n" +
             ((ddlRefComplementaria.Items.Count > 1 || ddlRefComplementaria.SelectedValue != "NT") ?
             "       COALESCE(psg.PSGRU_NOMBRE,'') CONCAT ' ' CONCAT   \n" : "") +
             "       pc.PCAR_NOMBRE CONCAT '\n' CONCAT   \n" +
             "       COALESCE(rc.RCIL_NOMBRE,'') CONCAT ' ' CONCAT   \n" +
             "       pcm.PCOM_NOMBRE CONCAT ' ' CONCAT   \n" +
             "       COALESCE(ram.RALI_NOMBRE,'') CONCAT   \n" +
             "       ' Caja ' CONCAT COALESCE(rcc.RCAJ_NOMBRE,'') CONCAT ' ' CONCAT   \n" +
             "       COALESCE(rtv.RTRA_DESCRIPC,'')  \n" +
             "FROM PCLASEVEHICULO PCV,  \n" +
             "     PMARCA PM,  \n" +
             "     PGRUPOCATALOGO PG,  \n" +
             "     PCARROCERIA PC,  \n" +
             "     PCOMBUSTIBLEMOTOR PCM,  \n" +
             "     PCATALOGOVEHICULO PV   \n" +
             "  LEFT JOIN PSUBGRUPOVEHICULO PSG ON PV.PSGRU_CODIGO = PSG.PSGRU_GRUPO   \n" +
             "  LEFT JOIN RALIMENTACIONMOTOR RAM ON PV.RALI_CODIGO = RAM.RALI_CODIGO   \n" +
             "  LEFT JOIN RCILINDRAJEMOTOR RC ON PV.RCIL_CODIGO = RC.RCIL_CODIGO   \n" +
             "  LEFT JOIN RCAJACAMBIOS RCC ON PV.RCAJ_CODIGO = RCC.RCAJ_CODIGO   \n" +
             "  LEFT JOIN RTRACCIONVEHICULO RTV ON PV.RTRA_CODIGO = RTV.RTRA_CODIGO  \n" +
             "WHERE PV.PCLA_CODIGO = PCV.PCLA_CODIGO  \n" +
             "AND   PV.PMAR_CODIGO = PM.PMAR_CODIGO  \n" +
             "AND   PV.PGRU_GRUPO = PG.PGRU_GRUPO  \n" +
             "AND   PV.PCAR_CODIGO = PC.PCAR_CODIGO  \n" +
             "AND   PV.PCOM_CODIGO = PCM.PCOM_CODIGO  \n" +
             "AND   PV.PCAT_CODIGO in {0} \n" +
             "ORDER BY 2"
             , catalogosStr);

            ddlCatalogo.Items.Clear();
            Utils.FillDll(ddlCatalogo, sql, true);
        }

        private void CambiarVisibilidadPHCatalogo()
        {
            phMarca.Visible = false;
            phRefPrincipal.Visible = false;
            phCarroceria.Visible = false;
            phRefComplementaria.Visible = false;
            phDatosCatalogo.Visible = false;
            phCatalogo.Visible = false;

            if (ddlClaseVeh.Items.Count > 0 && ddlClaseVeh.SelectedValue != "0")
                phMarca.Visible = true;

            if (ddlMarca.Items.Count > 0 && ddlMarca.SelectedValue != "0")
                phRefPrincipal.Visible = true;


            if (ddlRefPrincipal.Items.Count > 0 && ddlRefPrincipal.SelectedValue != "0")
            {
                if (ddlRefComplementaria.Items.Count > 1 ||
                    ddlRefComplementaria.SelectedValue != "NT") // Referencia Complementaria diferente de "NO TIENE"
                    phRefComplementaria.Visible = true;

                phCarroceria.Visible = true;
            }

            if (ddlCarroceria.Items.Count > 0 && ddlCarroceria.SelectedValue != "0")
                phDatosCatalogo.Visible = true;

            if (IsCatalogoFilled())
                phCatalogo.Visible = true;
        }

        private ArrayList ObtenerCatalogos()
        {
            string sql =
             "SELECT PCV.PCAT_CODIGO " +
             "FROM PCATALOGOVEHICULO PCV " +
             String.Format("WHERE PCV.PCLA_CODIGO = '{0}' ", ddlClaseVeh.SelectedValue);

            if (ddlMarca.Items.Count > 0)
                sql += String.Format("AND   PCV.PMAR_CODIGO = '{0}' ", ddlMarca.SelectedValue);
            if (ddlRefPrincipal.Items.Count > 0)
                sql += String.Format("AND   PCV.PGRU_GRUPO = '{0}' ", ddlRefPrincipal.SelectedValue);
            if (ddlRefComplementaria.Items.Count > 0 && ddlRefComplementaria.SelectedValue != "")
                sql += String.Format("AND	PCV.PSGRU_CODIGO = '{0}' ", ddlRefComplementaria.SelectedValue);
            else if (ddlRefComplementaria.SelectedValue == "")
                sql += "AND	PCV.PSGRU_CODIGO is null ";
            if (ddlCarroceria.Items.Count > 0)
                sql += String.Format("AND   PCV.PCAR_CODIGO = '{0}' ", ddlCarroceria.SelectedValue);
            if (ddlCilindraje.Items.Count > 0)
                sql += String.Format("AND   PCV.RCIL_CODIGO = '{0}' ", ddlCilindraje.SelectedValue);
            if (ddlTipoCombustible.Items.Count > 0)
                sql += String.Format("AND   PCV.PCOM_CODIGO = '{0}' ", ddlTipoCombustible.SelectedValue);
            if (ddlAspiracion.Items.Count > 0)
                sql += String.Format("AND   PCV.Rali_CODIGO = '{0}' ", ddlAspiracion.SelectedValue);
            if (ddlTraccion.Items.Count > 0)
                sql += String.Format("AND   PCV.Rtra_CODIGO = '{0}' ", ddlTraccion.SelectedValue);
            if (ddlCaja.Items.Count > 0)
                sql += String.Format("AND   PCV.RCAJ_CODIGO = '{0}' ", ddlCaja.SelectedValue);

            return DBFunctions.RequestAsCollection(sql);
        }

        public string ObtenerValorFiltro()
        {
            return ddlCatalogo.SelectedValue;
        }
    }
}