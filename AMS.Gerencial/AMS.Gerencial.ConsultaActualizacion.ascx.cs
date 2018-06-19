using AMS.DB;
using AMS.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Gerencial
{
    public partial class AMS_Gerencial_ConsultaActualizacion : System.Web.UI.UserControl
    {
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            DBFunctions.RequestGlobal(ds, IncludeSchema.NO, "", @"SELECT GA.GACT_ID AS ID, GA.GACT_MODULO AS RUTA, GA.GACT_COMENTARIO AS RAZON, GA.GACT_FECHA AS FECHA, SU.SUSU_NOMBRE AS USUARIO 
                                                                FROM GACTUALIZACION GA, SUSUARIO SU
                                                                WHERE GA.SUSU_CODIGO = SU.SUSU_CODIGO ORDER BY GA.GACT_ID;");
            miGrid.DataSource = ds.Tables[0];
            miGrid.DataBind();
        }

        protected void buscar(object sender, EventArgs e)
        {
            //ds.Clear();
            ds.Reset();
            DBFunctions.RequestGlobal(ds, IncludeSchema.NO, "", @"SELECT GA.GACT_ID AS ID, GA.GACT_MODULO AS RUTA, GA.GACT_COMENTARIO AS RAZON, GA.GACT_FECHA AS FECHA, SU.SUSU_NOMBRE AS USUARIO 
                                                                                FROM GACTUALIZACION GA, SUSUARIO SU
                                                                                WHERE GA.SUSU_CODIGO = SU.SUSU_CODIGO AND DATE(GA.GACT_FECHA) BETWEEN '" + txtFechaDesde.Text + "' AND '" + txtFechaHasta.Text + "' ORDER BY GA.GACT_ID;");
            miGrid.DataSource = ds.Tables[0];
            miGrid.DataBind();
        }

        protected void dataGrid_Paging(object sender, DataGridPageChangedEventArgs e)
        {
            this.miGrid.CurrentPageIndex = e.NewPageIndex;
            this.miGrid.DataSource = ds.Tables[0];
            this.miGrid.DataBind();
            //DatasToControls.JustificacionGrilla(dgNew, dtNew);
        }
    }
}