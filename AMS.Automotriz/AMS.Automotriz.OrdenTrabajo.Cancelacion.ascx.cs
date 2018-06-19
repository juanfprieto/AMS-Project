using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.DBManager;
using AMS.Forms;
using AMS.Documentos;
using Ajax;
using AMS.Tools;

namespace AMS.Automotriz.OrdenTrabajo
{
	public partial class Cancelacion : System.Web.UI.UserControl
	{
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string tipoProceso;

		protected void Page_Load(object sender, EventArgs e)
		{
            if(!IsPostBack)
			{
                DatasToControls bind = new DatasToControls();

                Utils.llenarPrefijos(Response, ref tipoDocumento1, "%", "%", "OT");

                tipoProceso = Request.QueryString["actor"]; //Debe definir el tipo de proceso: Torre de Control / Liquidacion OT
                if (tipoProceso == "R")
                {
                    Utils.MostrarAlerta(Response, "La Orden NO se puede cancelar porque tiene operaciones o repuestos asociados");
                }
                else if (tipoProceso == "C")
                {
                    Utils.MostrarAlerta(Response, "La Orden de trabajo se canceló correctamente");
                }
            }
		}

        [Ajax.AjaxMethod()]
        protected void Cambio_Documento1(Object Sender, EventArgs e)
        {
            Llenar_Ordenes_Liquidar(ordenesPreliquidar, tipoDocumento1);
        }

        [Ajax.AjaxMethod()]
        protected void Llenar_Ordenes_Liquidar(DropDownList objetivo, DropDownList fuente)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(objetivo, "SELECT mord_numeorde FROM dbxschema.morden WHERE pdoc_codigo='" + fuente.SelectedValue + "' AND mord_estaliqu IN ('A','P') and test_estado='A' ORDER BY mord_numeorde");
        }

        protected void cancelarOperacion(Object Sender, EventArgs e)
        {
             ArrayList sqls = new ArrayList();
            string operacion = DBFunctions.SingleData(@"SELECT * FROM (
                        select m.pdoc_codigo, m.mord_numeorde, count(*) AS ITEMS
                        from morden m, dordenoperacion d
                        where m.pdoc_codigo = d.pdoc_codigo AND m.mord_numeorde = d.mord_numeorde
                        AND  m.pdoc_codigo = '" + tipoDocumento1.SelectedValue + @"' 
                        and m.mord_numeorde = " + ordenesPreliquidar.SelectedItem + @"
                        group by m.pdoc_codigo, m.mord_numeorde
                         union
                        select m.pdoc_codigo, m.mord_numeorde, sum(di.dite_cantidad - coalesce(did.dite_cantidad,0)) as items
                        from morden m, mordentransferencia d left join ditems di
                           left join ditems did on di.pdoc_codigo = did.dite_prefdocurefe and di.dite_numedocu = did.dite_numedocurefe
                        on d.pdoc_factura = di.pdoc_codigo and d.mfac_numero = di.dite_numedocu
                        where m.pdoc_codigo = d.pdoc_codigo AND m.mord_numeorde = d.mord_numeorde
                        and  m.pdoc_codigo = 'prefijo' and m.mord_numeorde = 1
                        group by m.pdoc_codigo, m.mord_numeorde ) AS A WHERE ITEMS > 0;");

            if (operacion == "")
                {
                    sqls.Add(@"UPDATE dbxschema.MORDEN   SET TEST_ESTADO = 'X', MORD_OBSERECE = '" + textObser.Text + @"' WHERE PDOC_CODIGO = 
                                '" + tipoDocumento1.SelectedValue + "' AND   MORD_NUMEORDE = " + ordenesPreliquidar.SelectedValue + ";");

                        if (DBFunctions.Transaction(sqls))
                        {                            
                        }
                        else
                            lb.Text = "Error " + DBFunctions.exceptions;
                    Response.Redirect("" + indexPage + "?process=Automotriz.OrdenTrabajo.Cancelacion&actor=C ");
                }
            else
                {                    
                    Response.Redirect("" + indexPage + "?process=Automotriz.OrdenTrabajo.Cancelacion&actor=R ");
                }          
        }
    }
}
