using AMS.Forms;
using System;
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
            //bind.PutDatasIntoDropDownList(ddlCodigo, "@SELECT PC.PCOM_CODIGO as Código, PC.PCOM_NOMBRE as Tipo de Comprobante, PC.PCOn_CODITRAN as Código de Transacción, 
            //                                          RTIP_NOMBRE as Tipo de Transacción, tnit_nombre as Tipo de Identificación, rs.rsus_codigo || ' ' || rsus_nombre as Sustento Tributario
            //                                          FROM dbxschema.RTIPOTRANSACCION RT, dbxschema.tnit tn, dbxschema.pcomprobanteautorizado PC
            //                                            left join dbxschema.rsustentotributario rs on pc.rsus_codigo = rs.rsus_codigo where pc.rtip_codigo = rt.rtip_codigo and pc.tnit_tiponit = tn.tnit_tiponit");//sustento Tributario
            //bind.PutDatasIntoDropDownList(ddlCodigo, "SELECT pped_codigo, pped_nombre concat ' - ' concat pped_codigo FROM ppedido WHERE tped_codigo NOT IN ('T','Q','E') and tped_codiuso IN ('P','M') order by pped_nombre");//Tipo de pago

        }
    }
}