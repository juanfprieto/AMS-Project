using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using AMS.Tools;
using System.Data;
using Ajax;
using AMS.DB;
using System.Collections;
using System.Configuration;

namespace AMS.Automotriz
{
    public partial class EliminaOrden : System.Web.UI.UserControl
    {
        DatasToControls bind = new DatasToControls();
        public ArrayList sqlRels = new ArrayList();
        public string tabla = "";
        string tipoOrden;
        string numeroOrden;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Automotriz.EliminaOrden));

            if(!IsPostBack)
            {
                if (Request.QueryString["sucess"] == "1")
                {
                    Utils.MostrarAlerta(Response, "La orden " + Request.QueryString["tipoOrden"] + " - " + Request.QueryString["numOrden"] + " Ha sido eliminada!");
                }
                else if (Request.QueryString["sucess"] == "2")
                {
                    Utils.MostrarAlerta(Response, "No se pudo eliminar la órden" + Request.QueryString["tipoOrden"] + " - " + Request.QueryString["numOrden"] + " verifique que no tenga facturas o repuestos");
                }

                bind.PutDatasIntoDropDownList(ddlTipoOrden, "SELECT PDOC_CODIGO, PDOC_CODIGO CONCAT ' - ' CONCAT PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'OT'");
                if (ddlTipoOrden.Items.Count > 0)
                {
                    ddlTipoOrden.Items.Insert(0, new ListItem("Seleccione.."));
                }
                btnBorrar.Enabled = false;
            }
            
        }
        
        //Cargar órdenes
        protected void cargarNumOrdenes(object sender, EventArgs e)
        {
            bind.PutDatasIntoDropDownList(ddlNumeroOrden, "select MORD_NUMEORDE AS NUMERO, MORD_NUMEORDE CONCAT ' - ' CONCAT PDOC_CODIGO AS ORDEN from morden where pdoc_codigo = '" + ddlTipoOrden.SelectedValue.ToString() + "' AND TEST_ESTADO = 'A'");
            if (ddlNumeroOrden.Items.Count > 0)
            {
                ddlNumeroOrden.Items.Insert(0, new ListItem("Seleccione.."));
                ddlNumeroOrden.Enabled = true;
            }
            else
            {
                Utils.MostrarAlerta(Response, "No hay órdenes abiertas para el prefijo: " + ddlTipoOrden.SelectedValue.ToString());
            }
        }

        //Método que buscará si existen facturas o repuestos pendientes para la orden
        //De ser así, no se podrá eliminar la orden de trabajo.
        protected void buscarFacturas(object sender, EventArgs e)
        {
            btnBorrar.Enabled = true;
            DataSet ds = new DataSet();
            tipoOrden = ddlTipoOrden.SelectedValue.ToString();
            numeroOrden = ddlNumeroOrden.SelectedValue.ToString();
            string sql = "SELECT mo.pdoc_codigo as codiORde, mot.pdoc_codigo as codiTrans, mfct.pdoc_codigo as codiFact FROM MORDEN MO " +
                                                        "left join MORDENTRANSFERENCIA MOT " +
                                                        "on ( MO.PDOC_CODIGO = MOT.PDOC_CODIGO and mo.MORD_NUMEORDe = mot.MORD_NUMEORDe) " +
                                                        "left join MFACTURACLIENTETALLER MFCT " +
                                                        "ON (MO.PDOC_CODIGO = MFCT.PDOC_PREFORDETRAB and mo.MORD_NUMEORDe = mfct.MORD_NUMEORDe) " +
                                                        "WHERE MO.PDOC_CODIGO = '" + tipoOrden + "' " +
                                                        "AND MO.MORD_NUMEORDE= " + numeroOrden + " ;";
            DBFunctions.Request(ds,IncludeSchema.NO, sql);


            if (ds.Tables[0].Rows[0]["codiTrans"].ToString() != "" || ds.Tables[0].Rows[0]["codiFact"].ToString() != "")
            {
                Utils.MostrarAlerta(Response, "La orden: " + tipoOrden + " - " + numeroOrden + " tiene Repuestos o Facturas, por lo que no se puede eliminar.");
                btnBorrar.Enabled = false;
            }
            else
            {
                ddlNumeroOrden.Enabled = false;
                ddlTipoOrden.Enabled = false;
                Utils.MostrarAlerta(Response, "Ahora puede borrar la Orden de Trabajo: " + tipoOrden + " - " + numeroOrden);
               
            }
        }
        


        //Método que eliminará la órden en cuestión y sus referencias
        protected void borrarOrden(object sender, EventArgs e)
        {
            if (DBFunctions.RecordExist("Select PDOC_CODIGO FROM DORDENOPERACION FETCH FIRST  ROW ONLY"))
            {
                sqlRels.Add("DELETE FROM DORDENOPERACION WHERE PDOC_CODIGO = '" + ddlTipoOrden.SelectedValue + "' AND   MORD_NUMEORDE = " + ddlNumeroOrden.SelectedValue + ";");
                //tabla += "DORDENOPERACION, ";
            }
            if (DBFunctions.RecordExist("Select PDOC_CODIGO FROM DORDENACCESORIO FETCH FIRST  ROW ONLY"))
            {
                sqlRels.Add("DELETE FROM DORDENACCESORIO WHERE PDOC_CODIGO = '" + ddlTipoOrden.SelectedValue + "' AND   MORD_NUMEORDE = " + ddlNumeroOrden.SelectedValue + ";");
                //tabla += "DORDENACCESORIO, ";
            }
            if (DBFunctions.RecordExist("Select PDOC_CODIGO FROM DORDENSEGUROS FETCH FIRST  ROW ONLY"))
            {
                sqlRels.Add("DELETE FROM DORDENSEGUROS WHERE PDOC_CODIGO = '" + ddlTipoOrden.SelectedValue + "' AND   MORD_NUMEORDE = " + ddlNumeroOrden.SelectedValue + ";");
                //tabla += "DORDENSEGUROS";
            }
            sqlRels.Add("DELETE FROM MORDEN WHERE PDOC_CODIGO = '" + ddlTipoOrden.SelectedValue + "' AND   MORD_NUMEORDE = " + ddlNumeroOrden.SelectedValue + ";");
            tabla = " MORDEN";
            //Guardar el registro en MhistorialCambios.
            //creamos e inicializamos variables que serán insertadas:
            var operacion = "D";
            string consulta = "SELECT PDOC_CODIGO CONCAT ', ' CONCAT MORD_NUMEORDE CONCAT ', ' CONCAT PVEN_MERCADERISTA CONCAT ', ' CONCAT MCAT_VIN CONCAT ', ' CONCAT MNIT_NIT CONCAT ', ENTRADA: ' CONCAT MORD_ENTRADA CONCAT ', CREACION: ' CONCAT MORD_CREACION FROM MORDEN WHERE PDOC_CODIGO = '" + ddlTipoOrden.SelectedValue + "' AND   MORD_NUMEORDE = " + ddlNumeroOrden.SelectedValue + "";
            var originales = DBFunctions.SingleData(consulta);
            var usuario = HttpContext.Current.User.Identity.Name.ToLower();
            var fecha = DateTime.Now.ToString("yyyy-MM-dd");

            //insert en mhistorialcambios 
            //al borrar una orden, el registro debe de guardarse en esa tabla.
            sqlRels.Add("INSERT INTO MHISTORIAL_CAMBIOS (" +
                        "TABLA, OPERACION, ORIGINALES, SUSU_USUARIO, FECHA) " +
                        " VALUES( '" + 
                        tabla + "', '" + operacion + "', '" + originales + "', '" + usuario + "', '" + fecha + "')");
            if (DBFunctions.Transaction(sqlRels))
            {

                Response.Redirect(indexPage + "?process=Automotriz.EliminaOrden&sucess=1&tipoOrden=" + ddlTipoOrden.SelectedValue + "&numorden=" + ddlNumeroOrden.SelectedValue);
            }
            else
            {
                Response.Redirect(indexPage + "?process=Automotriz.EliminaOrden&sucess=2&tipoOrden=" + ddlTipoOrden.SelectedValue + "&numorden=" + ddlNumeroOrden.SelectedValue);
            }
        }

        
    }
}