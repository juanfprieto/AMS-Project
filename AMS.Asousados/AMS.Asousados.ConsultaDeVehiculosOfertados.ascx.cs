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
using AMS.Tools;

namespace AMS.Asousados
{
	public partial class ConsultaDeVehiculosOfertados : System.Web.UI.UserControl
	{
        public string indexPage = ConfigurationManager.AppSettings["MainAjaxPage"];


		protected void Page_Load(object sender, EventArgs e)
		{

            if (Request.QueryString["codAso"] == "1")
                Utils.MostrarAlerta(Response, "Su nit no Tiene Permisos para Ingresar Vehiculos");

            prepararTablaVehiculos();
		}

        #region Eventos

        protected void dgListaVehiculos_Command(Object sender, DataGridCommandEventArgs e)
        {
            string codVehiculo = ((DataBoundLiteralControl)e.Item.Cells[0].Controls[0]).Text.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            string redirect = null;
                   redirect = String.Format("{0}?process=Asousados.IngresoVehiculo&id={1}&Consultar=S&oferente=S",
                        indexPage, codVehiculo);
                   Response.Redirect(redirect);
        }
        #endregion
        #region Metodos

        private void prepararTablaVehiculos()
        {
             string sql = String.Format(
             "SELECT MVU.MVEH_SECUENCIA ID,  \n" +
             "mof.MOFE_NOMBRE ASOCIADO, \n"  +       
             "       substr(MVU.MCAT_PLACA,6,1) PLACA,  \n" +
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
             "WHERE MVU.TEST_TIPOESTA = 20  \n" +
             "and MVU.mnit_nit is null  \n" + 
             "ORDER BY ASOCIADO");

            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);

            dgListaVehiculos.DataSource = ds.Tables[0];
            dgListaVehiculos.DataBind();
        }
        #endregion
    }
}