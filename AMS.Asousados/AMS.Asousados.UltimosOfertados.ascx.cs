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

namespace AMS.Asousados
{
	public partial class UltimosOfertados : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            cargar_Vehiculos(); 
		} 

        
        protected void cargar_Vehiculos()
        {
            string Dia = DBFunctions.SingleData("SELECT DAYNAME (CURRENT DATE) FROM SYSIBM.SYSDUMMY");
            int DiaSemana = 0;
            if (Dia == "Monday")
            {
                 DiaSemana = 2;
            }
            else
            {
                 DiaSemana = 1;
            }
            string sql = String.Format("SELECT PC.PCLA_NOMBRE concat ' ' concat \n" +
             "       PM.PMAR_NOMBRE concat ' ' concat \n" +
             "       PGC.PGRU_NOMBRE concat ' ' concat \n" +
             "       CASE \n" +
             "         WHEN PSG.PSGRU_NOMBRE = 'NO TIENE' THEN '' \n" +
             "         ELSE PSG.PSGRU_NOMBRE \n" +
             "       END concat ' ' concat \n" +
             "       PCA.PCAR_NOMBRE concat ' ' concat \n" +
             "       'Placa ' concat substr(MCAT_PLACA,6,1) concat ' Año ' concat pano_anomode \n" +
             "FROM MVEHICULOUSADO MVU, \n" +
             "     PCATALOGOVEHICULO PCV, \n" +
             "     PCLASEVEHICULO PC, \n" +
             "     PSUBGRUPOVEHICULO PSG, \n" +
             "     PMARCA PM, \n" +
             "     PCARROCERIA PCA,\n" +
             "     PGRUPOCATALOGO PGC \n" +
             "WHERE MVU.PCAT_CODIGO = PCV.PCAT_CODIGO \n" +
             "AND   PC.PCLA_CODIGO = PCV.PCLA_CODIGO \n" +
             "AND   PSG.PSGRU_GRUPO = PCV.PSGRU_CODIGO \n" +
             "AND   PM.PMAR_CODIGO = PCV.PMAR_CODIGO \n" +
             "AND   PCA.PCAR_CODIGO = PCV.PCAR_CODIGO \n" +
             "AND   PCV.PGRU_GRUPO = PGC.PGRU_GRUPO \n" +
             "AND   MVEH_FECHINGR between   ((CURRENT DATE) - "+DiaSemana+" day) and ((CURRENT DATE))   \n" +
             "AND   TEST_TIPOESTA = '20' \n" +
             "AND   MNIT_NIT IS NULL");
            
            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
            string codigoHTML = "";

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                codigoHTML += "<div class='tarjeta'>";
			    codigoHTML += ds.Tables[0].Rows[i][0].ToString();
                codigoHTML += "</div>";
            }

            VehiculosOfertados.InnerHtml = codigoHTML;
         }
	}
}