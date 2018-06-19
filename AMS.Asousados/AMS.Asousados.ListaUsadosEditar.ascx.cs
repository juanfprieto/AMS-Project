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

namespace AMS.Asousados
{
	public partial class ListaUsadosEditar : System.Web.UI.UserControl
	{
        public string indexPage = ConfigurationManager.AppSettings["MainAjaxPage"];
        private bool esOferente, esConsultar;
        protected string mainPage = ConfigurationSettings.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            DataSet ds = new DataSet();
            string sql = String.Format("SELECT MNIT_NIT, MNIT_NIT2  FROM MCONCESIONARIO WHERE MNIT_NIT = (SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')");
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
            string valu = "";
            string valu2 = ""; 
            if (ds.Tables[0].Rows.Count != 0)
            {
                valu = ds.Tables[0].Rows[0][0].ToString();
                valu2 = ds.Tables[0].Rows[0][1].ToString();

                if (valu != valu2)
                    Response.Redirect(mainPage + "?process=Asousados.ConsultaDeVehiculosOfertados&codAso=1");
            }


            esOferente = Request.QueryString["oferente"] != null && Request.QueryString["oferente"] != "";
            esConsultar = Request.QueryString["Consultar"] == "S";
            prepararTablaVehiculos();
            if (!esOferente && valu != "") 
                dgListaVehiculos.Columns[5].Visible = false;
		}

        #region Eventos

        protected void dgListaVehiculos_Command(Object sender, DataGridCommandEventArgs e)
        {
            string codVehiculo = ((DataBoundLiteralControl)e.Item.Cells[0].Controls[0]).Text.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            string placaVeh = ((DataBoundLiteralControl)e.Item.Cells[2].Controls[0]).Text.Replace("\n", "").Replace("\r", "").Replace("\t", "");

            if (e.CommandName == "editar")
            {
                string redirect = null;

                if (esOferente)
                    redirect = String.Format("{0}?process=Asousados.IngresoVehiculo&id={1}&modifica=S&oferente=S",
                        indexPage, codVehiculo);
                else
                    redirect = String.Format("{0}?process=Asousados.IngresoVehiculo&id={1}&modifica=S",
                        indexPage, codVehiculo);

                Response.Redirect(redirect);
            }
            else if (e.CommandName == "remover")
            {
                Hashtable deletePk = new Hashtable();
                string tabla = "MVEHICULOUSADO";

                //deletePk.Add("MVEH_SECUENCIA", codVehiculo);
                deletePk.Add("MCAT_PLACA", placaVeh);
                DBFunctions.DeleteHashtable(tabla, deletePk);

                prepararTablaVehiculos();
            }
        }

        #endregion

        #region Metodos

        private void prepararTablaVehiculos()
        {
            string nit = DBFunctions.SingleData(String.Format("SELECT mnit_nit FROM susuario WHERE susu_login = '{0}'", HttpContext.Current.User.Identity.Name));

            string sql = String.Format(
             "SELECT MVU.MVEH_SECUENCIA ID,  \n" +
             (esOferente ? "mof.MOFE_NOMBRE ASOCIADO, \n" :
             "       mn.mnit_nombres || ' ' || mn.mnit_apellidos ASOCIADO,  \n") +
             (esOferente && esConsultar? "substr(MVU.MCAT_PLACA,6,1) PLACA, \n" :
             " MVU.MCAT_PLACA  PLACA,  \n") +
             "       pcv.PCLA_NOMBRE CONCAT ' ' CONCAT pm.PMAR_NOMBRE CONCAT ' ' CONCAT pg.PGRU_NOMBRE CONCAT ' ' CONCAT COALESCE(psg.PSGRU_NOMBRE,'') CONCAT ' ' CONCAT pc.PCAR_NOMBRE CONCAT ' ' CONCAT COALESCE(rc.RCIL_NOMBRE,'') CONCAT ' ' CONCAT pcm.PCOM_NOMBRE CONCAT ' ' CONCAT COALESCE(ram.RALI_NOMBRE,'') CONCAT ' Caja ' CONCAT COALESCE(rcc.RCAJ_NOMBRE,'') CONCAT ' ' CONCAT COALESCE(rtv.RTRA_DESCRIPC,'') CATALOGO  \n" +
             "FROM MVEHICULOUSADO MVU   \n" +
             "  LEFT JOIN MOFERENTE mof ON MVU.MVEH_OFERENTE = mof.MOFE_SECUENCIA  \n" +
             "  LEFT JOIN MCONCESIONARIO mc   \n" +
             "  	INNER JOIN mnit mn ON mc.mnit_nit = mn.mnit_nit  \n" +
             "  ON MVU.mnit_nit = mc.mnit_nit   \n" +
             "  LEFT JOIN PCATALOGOVEHICULO PV   \n" +
             "	  LEFT JOIN PSUBGRUPOVEHICULO PSG ON PV.PSGRU_CODIGO = PSG.PSGRU_GRUPO   \n" +
             "	  LEFT JOIN RALIMENTACIONMOTOR RAM ON PV.RALI_CODIGO = RAM.RALI_CODIGO   \n" +
             "	  LEFT JOIN RCILINDRAJEMOTOR RC ON PV.RCIL_CODIGO = RC.RCIL_CODIGO   \n" +
             "	  LEFT JOIN RCAJACAMBIOS RCC ON PV.RCAJ_CODIGO = RCC.RCAJ_CODIGO   \n" +
             "	  LEFT JOIN RTRACCIONVEHICULO RTV ON PV.RTRA_CODIGO = RTV.RTRA_CODIGO   \n" +
             "	  INNER JOIN PCLASEVEHICULO PCV ON PV.PCLA_CODIGO = PCV.PCLA_CODIGO   \n" +
             "	  INNER JOIN PCARROCERIA PC ON PV.PCAR_CODIGO = PC.PCAR_CODIGO   \n" +
             "	  INNER JOIN PMARCA PM ON PV.PMAR_CODIGO = PM.PMAR_CODIGO   \n" +
             "	  INNER JOIN PGRUPOCATALOGO PG ON PV.PGRU_GRUPO = PG.PGRU_GRUPO   \n" +
             "	  INNER JOIN PCOMBUSTIBLEMOTOR PCM ON PV.PCOM_CODIGO = PCM.PCOM_CODIGO  \n" +
             "  ON MVU.PCAT_CODIGO = PV.PCAT_CODIGO  \n" +
             "WHERE MVU.TEST_TIPOESTA = 20 {0} {1}  \n" +
             "ORDER BY ASOCIADO"
             , nit != "" && !esOferente ? String.Format("and MVU.mnit_nit = '{0}'", nit) : ""
             , esOferente ? "and MVU.MVEH_OFERENTE is not null" : "and MVU.MVEH_OFERENTE is null");

            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);

            dgListaVehiculos.DataSource = ds.Tables[0];
            dgListaVehiculos.DataBind();
        }
        #endregion
	}
}