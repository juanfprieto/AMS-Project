using AMS.DB;
using AMS.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Tools
{
    public partial class SustentoTributario : System.Web.UI.UserControl
    {
        private DatasToControls bind = new DatasToControls();
        protected void Page_Load(object sender, EventArgs e)
        {
            string tipoProceso = Request.QueryString["Tproc"];
            string tipoPersona = Request.QueryString["Tper"];

            bind.PutDatasIntoDropDownList(ddlSustento, @"SELECT PC.PCOM_CODIGO as Código, PC.PCOM_NOMBRE as Tipo de Comprobante, PC.PCOn_CODITRAN as Código de Transacción, 
                                                        RTIP_NOMBRE as Tipo de Transacción, tnit_nombre as Tipo de Identificación, rs.rsus_codigo || ' ' || rsus_nombre as Sustento Tributario
                                                        FROM dbxschema.RTIPOTRANSACCION RT,
                                                        dbxschema.tnit tn, dbxschema.pcomprobanteautorizado PC
                                                        left join dbxschema.rsustentotributario rs on pc.rsus_codigo = rs.rsus_codigo 
                                                        where pc.rtip_codigo = rt.rtip_codigo 
                                                        and pc.tnit_tiponit = tn.tnit_tiponit
                                                        AND tn.tnit_tiponit = '"+tipoPersona+ @"'
                                                        and PC.PCOn_CODITRAN = '"+tipoProceso+"' "); //sustento Tributario
            bind.PutDatasIntoDropDownList(ddlPago, "Select tfor_codigo, tform_nombre from tforma pago");//Tipo de pago

            

        }
        protected void GuardarSustento()
        {
            ArrayList sqlRefs = new ArrayList();
            sqlRefs.Add("INSERT INTO MDOCUMENTOUSUARIO VALUES('" + ddlPago + "'," + ddlPago + ",'" + ddlPago + "', 'C','D','',null, '', '' , null, ''," + ddlPago + ", null, '" + ddlPago + "')");
            if (DBFunctions.Transaction(sqlRefs))
            {
                try
                {
                    Utils.MostrarAlerta(Response, "Informacion Guarda Satisfactoriamente");
                }
                catch 
                {
                    lbError.Text = "Error " + DBFunctions.exceptions;
                }
            }
       }
    }
}