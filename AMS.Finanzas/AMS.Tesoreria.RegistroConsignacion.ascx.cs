using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Tools;
using AMS.Forms;

namespace AMS.Finanzas
{
    public partial class AMS_Tesoreria_RegistroConsignacion : System.Web.UI.UserControl
    {
        private DatasToControls bind = new DatasToControls();
        protected void Page_Load(object sender, EventArgs e)
        {
            bind.PutDatasIntoDropDownList(ddlCodigoCuenta, "SELECT PCUE_NUMERO, '[' CONCAT PCUE_NUMERO CONCAT ']' CONCAT ' - ' CONCAT PCUE_NOMBRE FROM PCUENTACORRIENTE;");
            bind.PutDatasIntoDropDownList(ddlEstadoConsignacion, "SELECT TCONS_CODIGO, TCONS_DESCRIPCION FROM TCONFIRMACIONCONSIGNACION ORDER BY TCONS_CODIGO DESC;");
            bind.PutDatasIntoDropDownList(ddlTipoVenta, "SELECT PFLU_DESCRIPCION,PFLU_DESCRIPCION FROM PFLUJOCAJAESPECIFICO;");
            bind.PutDatasIntoDropDownList(ddlTipoCompra, "SELECT TTIP_CODIGO, TTIP_NOMBRE FROM TTIPOPAGO;");
            bind.PutDatasIntoDropDownList(ddlUsuario, "SELECT SUSU_CODIGO, SUSU_NOMBRE FROM SUSUARIO WHERE SUSU_TIPCREA != 'D';");
        }

        protected void guardarCondignacion(object sender, EventArgs e)
        {
            Utils.MostrarAlerta(Response, "Estamos probando!!");
        }
    }
}