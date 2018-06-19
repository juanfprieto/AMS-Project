using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using System.Text;
using AMS.Tools;
using System;
using Obout.Grid;

namespace AMS.Gerencial
{
    public partial class AMS_Gerencial_PruebaContenidos2 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                String nameSystem = ConfigurationManager.AppSettings["SystemName"] + ".";
                String pathToControls = ConfigurationManager.AppSettings["PathToControls"];

                gridHolder.Controls.Add(LoadControl("" + pathToControls + nameSystem + "Tools.Encabezado.ascx"));

                DataSet dsPrueba = new DataSet();

                string claseVehiculos = null;
                try
                {
                    claseVehiculos = DBFunctions.SingleData("SELECT TCLA_CODIGO FROM CVEHICULOS"); // aqui se define el tipo de vehiculos que comercializa  N=Solo Nuevos, U=Solo Usados, X = todos
                }
                catch { };
                if (claseVehiculos == null)
                {
                    Utils.MostrarAlerta(Response, "NO ha definido en CVEHICULOS.TCLA_CODIGO, la clase de vehiculos que comercializa. Favor llamar a eCAS !!! ");
                    return;
                }

                // aqui falta incluir la opcion del vehiculo en la comparacion del año y modelo contra la lista de precios
                DBFunctions.Request(dsPrueba, IncludeSchema.NO,
                    @"SELECT MARCA, FAMILIA, CATALOGO, VEHICULO, VIN, COLOR, D_O,MODELO, ESTADO, DIAS, PUBI_NOMBRE AS UBICACION,PRECIO_CON_IVA,ORIGEN,PAGO  
                    FROM ( 
                    SELECT CASE  
                                WHEN MV.TCLA_CODIGO = 'U' AND CV.TCLA_CODIGO <> 'U' THEN 'zUSADOS'  
                                ELSE PM.PMAR_NOMBRE   
                            END AS Marca,   
                            CASE   
                                WHEN MV.TCLA_CODIGO = 'U' AND CV.TCLA_CODIGO <> 'U' THEN 'VEHICULOS USADOS'   
                                ELSE pgc.pFAM_nombre   
                            END AS Familia,   
                            MCV.PCAT_CODIGO AS CATALOGO,   
                            PCAT_DESCRIPCION AS VEHICULO,   
                            CASE   
                                WHEN MV.TCLA_CODIGO = 'U' THEN MV.MCAT_VIN CONCAT ' - ' CONCAT PM.PMAR_NOMBRE CONCAT ' - ' CONCAT PCAT_DESCRIPCION  
                                ELSE PCAT_DESCRIPCION  CONCAT ' - ' CONCAT MCV.PCAT_CODIGO CONCAT ' - ' CONCAT MV.MCAT_VIN 
                            END AS vin,  
                            PCOL_DESCRIPCION AS COLOR,  
                            CASE  
                                WHEN MV.TCLA_CODIGO = 'U' THEN mv.Mveh_NUMED_O CONCAT ' - Kms: ' CONCAT CAST(mcv.mcat_numeultikilo AS INTEGER)  
                                ELSE mv.Mveh_NUMED_O  
                            END AS D_O,  
                            mcv. MCAT_ANOMODE concat ' - Recepcionado ' concat cast(MV.MVEH_FECHRECE as char(10)) AS MODELO,  
                            TEST_NOMBESTA AS ESTADO,  
                            days(CURRENT DATE) - days(MV.MVEH_FECHRECE) AS DIAS,  
                            MAX(MUBI_CODIGO) AS UBICACION,  
                            CASE  
                                WHEN MV.TCLA_CODIGO = 'U' THEN mv.mveh_valoinfl  
                                ELSE ROUND(COALESCE(pp.ppre_precio,0) + DOUBLE (COALESCE(pp.ppre_precio,0)*DOUBLE ((pca.piva_porciva+pca.pipo_porcipoc)*0.01)),0)  
                            END AS PRECIO_CON_IVA,  
                            CASE  
                                WHEN TC.TCOM_CODIGO = 'P' THEN 'Propio'  
                                ELSE TC.TCOM_TIPOCOMPRA  
                            END AS origen, 
                            mfp.pdoc_codiordepago CONCAT '-' CONCAT mfp.mfac_numeordepago CONCAT '  ' CONCAT mfp.mfac_pago AS pago 
                    FROM DBXSCHEMA.CVEHICULOS CV , DBXSCHEMA.MVEHICULO MV  
                        LEFT JOIN dbxschema.mfacturaproveedor mfp ON MV.mfac_numeordepago = mfp.mfac_numeordepago AND MV.pdoc_codiordepago = mfp.pdoc_codiordepago  
                        LEFT JOIN DBXSCHEMA.TESTADOVEHICULO te ON mv.TEST_TIPOESTA = te.TEST_TIPOESTA  
                        LEFT JOIN DBXSCHEMA.TCOMPRAVEHICULO TC ON MV.TCOM_CODIGO = TC.TCOM_CODIGO  
	                    LEFT JOIN DBXSCHEMA.MCATALOGOVEHICULO MCV  
		                        LEFT JOIN dbxschema.ppreciovehiculo pp ON MCV.pcat_codigo = pp.pcat_codigo AND MCV.MCAT_ANOMODE = PP.PANO_ANO
		                        LEFT JOIN DBXSCHEMA.MUBICACIONVEHICULO mU ON MCV.MCAT_VIN = mU.MCAT_VIN 
		                        LEFT JOIN DBXSCHEMA.PCOLOR PC ON MCV.PCOL_CODIGO = PC.PCOL_CODIGO  
		                        LEFT JOIN dbxschema.pcatalogovehiculo pca  
                                        LEFT JOIN dbxschema.pFAMILIAcatalogo pgc ON pca.pFAM_FAMILIA = pgc.pFAM_FAMILIA  
				                       LEFT JOIN DBXSCHEMA.PMARCA PM ON PCA.PMAR_CODIGO = PM.PMAR_CODIGO  
		                        ON pca.pcat_codigo = McV.pcat_codigo  
	                    ON MCV.MCAT_VIN = MV.MCAT_VIN  
                    WHERE MV.TEST_TIPOESTA <= 20  
                    GROUP BY MCV.PCAT_CODIGO,  
                                MV.MCAT_VIN,  
                                PM.PMAR_NOMBRE,  
                                PCAT_DESCRIPCION,  
                                PCOL_DESCRIPCION,  
                                mv.Mveh_NUMED_O,  
                                mcv. MCAT_ANOMODE,  
                                TEST_NOMBESTA,  
                                MV.MVEH_FECHRECE,  
                                PCA.PMAR_CODIGO,  
                                pp.ppre_precio,  
                                pca.piva_porciva, 
                                pca.pipo_porcipoc,  
                                TC.TCOM_CODIGO,  
                                TC.TCOM_TIPOCOMPRA,  
                                pgc.pFAM_nombre,  
                                MV.TCLA_CODIGO,  
                                mv.mveh_valoinfl,  
                                mcv.mcat_placa,  
                                mcv.mcat_numeultikilo,  
                                mfp.pdoc_codiordepago,  
                                mfp.mfac_numeordepago,  
                                mfp.mfac_pago,
                                CV.TCLA_CODIGO  
                       ) AS A  
                        LEFT JOIN DBXSCHEMA.MUBICACIONVEHICULO mU  
                                    LEFT JOIN DBXSCHEMA.PUBICACION pU ON mu.pubi_CODIGO = pu.pubi_codigo  
		                        ON UBICACION = mU.MUBI_CODIGO 
                      order by MARCA, FAMILIA, CATALOGO, VEHICULO;");

                Grid2.DataSource = dsPrueba;
                Grid2.DataBind();
            }
		}

        protected void OnGridRowDataBound(object sender, GridRowEventArgs args)
        {
            if (args.Row.RowType == GridRowType.DataRow)
            {
                try
                {
                    int dias = int.Parse(args.Row.Cells[9].Text);
               
                

                if (dias >= 25 && args.Row.Cells[10].Text == "Custodia")
                {
                    args.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                    args.Row.Cells[10].BackColor = System.Drawing.Color.Maroon;
                }
            }
                catch { }
            }
        }
               
	}
}