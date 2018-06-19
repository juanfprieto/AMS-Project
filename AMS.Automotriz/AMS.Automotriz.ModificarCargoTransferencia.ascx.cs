using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using System.Data;
using AMS.DB;
using System.Collections;
using System.Configuration;
using AMS.Tools;

namespace AMS.Automotriz
{
    public partial class AMS_Automotriz_ModificarCargoTransferencia : System.Web.UI.UserControl
    {
        protected DataTable tablaDatos;
        protected DatasToControls bind = new DatasToControls();
        DataSet ds = new DataSet();
        DataSet cargos = new DataSet();
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        bool tienfactura = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                plctransferencia.Visible = false;
                bind.PutDatasIntoDropDownList(ddlprefijoorden, "select distinct mo.pdoc_codigo, mo.pdoc_codigo concat ' - ' concat pd.pdoc_nombre from morden mo, mordentransferencia mot, pdocumento pd where mo.pdoc_codigo = mot.pdoc_codigo and mo.pdoc_codigo=pd.pdoc_codigo;");
                bind.PutDatasIntoDropDownList(ddlnumeroorden, "select distinct mot.mord_numeorde from morden mo, mordentransferencia mot where mo.pdoc_codigo = mot.pdoc_codigo and mo.mord_numeorde=mot.mord_numeorde and mo.pdoc_codigo='" + ddlprefijoorden.SelectedValue + "' and test_estado='A';");
                string prefijo = Request.QueryString["codor"];
                string numero = Request.QueryString["numord"];
                if (prefijo!= null && numero != null)
                    Utils.MostrarAlerta(Response, "Se ha modificado los cargos de las transferencias asociadas a la orden " + prefijo + " con numero" + numero);
            }
        }
        protected void CargarNumeroOrden(Object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlnumeroorden, "select mot.mord_numeorde from morden mo, mordentransferencia mot where mo.pdoc_codigo = mot.pdoc_codigo and mo.mord_numeorde=mot.mord_numeorde and mo.pdoc_codigo='" + ddlprefijoorden.SelectedValue + "' and test_estado='A';");
        }
        protected void RevisarTransferencia(Object Sender, EventArgs e)
        {
            plctransferencia.Visible = true;
            plcorden.Visible = false;
            BindDatas();
        }
        protected void Cargar_Tabla_Transferencia(DataTable dtable)
        {
            tablaDatos = new DataTable();
            tablaDatos.Columns.Add("CODIGO ORDEN", typeof(string));
            tablaDatos.Columns.Add("NUMERO ORDEN", typeof(int));
            tablaDatos.Columns.Add("CARGO_TRANSFERENCIA", typeof(string));
            tablaDatos.Columns.Add("CODIGO TRANSFERENCIA", typeof(string));
            tablaDatos.Columns.Add("NUMERO TRANSFERENCIA", typeof(int));
        }
        public void DgInserts_Edit(object sender, DataGridCommandEventArgs e)
        {
           
                if (tienfactura == false)
                {
                    dgtransferencia.EditItemIndex = e.Item.ItemIndex;
                    dgtransferencia.DataSource = tablaDatos;
                    dgtransferencia.DataBind();
                }
                else { return; }
        }
        protected void DgInserts_Databound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                tablaDatos = (DataTable)Session["tablaDatos"];
                if (!DBFunctions.RecordExist("select TCAR_CARGO from DBXSCHEMA.mfacturaclientetaller where PDOC_PREFORDETRAB = '"+ tablaDatos.Rows[e.Item.ItemIndex]["CODIGO ORDEN"].ToString() + "' AND MORD_NUMEORDE = "+ tablaDatos.Rows[e.Item.ItemIndex]["NUMERO ORDEN"].ToString()))
                {
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]), "SELECT tca.TCAR_CARGO, tca.TCAR_CARGO || ' - ' || tca.TCAR_NOMBRE FROM DBXSCHEMA.tCARGOorden tca ORDER BY tca.TCAR_NOMBRE ASC");
                }
                else
                {
                    ArrayList nocargo = new ArrayList();
                    string cadena="";
                    for (int i = 0; i < cargos.Tables[0].Rows.Count; i++)
                    {
                        nocargo.Add(cargos.Tables[0].Rows[i][0].ToString());
                        cadena += ",'" + nocargo[i].ToString() + "'";
                    }
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]), "SELECT tca.TCAR_CARGO, tca.TCAR_CARGO || ' - ' || tca.TCAR_NOMBRE  FROM DBXSCHEMA.tCARGOorden tca where tca.TCAR_CARGO NOT IN(''"+ cadena + ") ORDER BY tca.TCAR_NOMBRE ASC");
                }
            }
        }
        protected void DgInserts_Item(object sender, DataGridCommandEventArgs e)
        {
            tablaDatos = (DataTable)Session["tablaDatos"];
            if (((Button)e.CommandSource).CommandName == "Update")
            {
                tablaDatos.Rows[e.Item.ItemIndex]["CARGO_TRANSFERENCIA"] = ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue;
                tablaDatos.AcceptChanges();
                Session["tablaDatos"] = tablaDatos;
                dgtransferencia.DataSource = tablaDatos;
                dgtransferencia.EditItemIndex = -1;
                dgtransferencia.DataBind();
            }
            else if (((Button)e.CommandSource).CommandName == "Edit")
            {
                DBFunctions.Request(cargos,IncludeSchema.NO,"select TCAR_CARGO from DBXSCHEMA.mfacturaclientetaller where PDOC_PREFORDETRAB = '" + tablaDatos.Rows[e.Item.ItemIndex]["CODIGO ORDEN"].ToString() + "' AND MORD_NUMEORDE = " + tablaDatos.Rows[e.Item.ItemIndex]["NUMERO ORDEN"].ToString());
                
                for (int i = 0; i < cargos.Tables[0].Rows.Count; i++)
                {                
                    if (tablaDatos.Rows[e.Item.ItemIndex]["CARGO_TRANSFERENCIA"].ToString() == cargos.Tables[0].Rows[i][0].ToString())
                    {
                        tienfactura = true;
                        DataSet factura = new DataSet();
                        DBFunctions.Request(factura, IncludeSchema.NO, "select PDOC_CODIGO AS FACTURA, MFAC_NUMEDOCU AS NUMERO from DBXSCHEMA.mfacturaclientetaller where PDOC_PREFORDETRAB = '" + tablaDatos.Rows[e.Item.ItemIndex]["CODIGO ORDEN"].ToString() + "' AND MORD_NUMEORDE = " + tablaDatos.Rows[e.Item.ItemIndex]["NUMERO ORDEN"].ToString());
                        Utils.MostrarAlerta(Response, "El cargo de esta transferencia ya fue facturado con el prefijo " + factura.Tables[0].Rows[0][0].ToString() + " y numero " + factura.Tables[0].Rows[0][1].ToString());
                                      
                    }
                }
            }
            else if (((Button)e.CommandSource).CommandName == "Cancel")
            {
                Session["tablaDatos"] = tablaDatos;
                dgtransferencia.DataSource = tablaDatos;
                dgtransferencia.EditItemIndex = -1;
                dgtransferencia.DataBind();
            }
        }
        public void BindDatas()
        {
            // se llena el grid                                   
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT MOT.PDOC_CODIGO AS \"CODIGO ORDEN\",MOT.MORD_NUMEORDE AS \"NUMERO ORDEN\",MOT.TCAR_CARGO AS \"CARGO_TRANSFERENCIA\",MOT.PDOC_FACTURA AS \"CODIGO TRANSFERENCIA\",MOT.MFAC_NUMERO AS \"NUMERO TRANSFERENCIA\" FROM mordentransferencia MOT WHERE PDOC_CODIGO='" + ddlprefijoorden.SelectedValue + "' AND MORD_NUMEORDE=" + ddlnumeroorden.SelectedValue + ";");
            Session["Datos"] = ds.Tables[0];
            string prfijo, preftrans;
            int numero, numetransf;
            string cargo;
            if (ds.Tables[0].Rows.Count != 0)
                Cargar_Tabla_Transferencia(ds.Tables[0]);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++){
                prfijo = "";numero = 0; cargo = "" ;  preftrans = ""; numetransf = 0;
                try { prfijo = Convert.ToString(ds.Tables[0].Rows[i][0]); }
                catch {  }
                try { numero = Convert.ToInt32(ds.Tables[0].Rows[i][1]); }
                catch {  }
                try { cargo = Convert.ToString(ds.Tables[0].Rows[i][2]); }
                catch {  }
                try { preftrans = Convert.ToString(ds.Tables[0].Rows[i][3]); }
                catch {  }
                try { numetransf = Convert.ToInt32 (ds.Tables[0].Rows[i][4]); }
                catch { }
                DataRow nr = tablaDatos.NewRow();
                nr[0] = prfijo; nr[1] = numero; nr[2] = cargo; nr[3] = preftrans; nr[4] = numetransf;
                tablaDatos.Rows.Add(nr);
            }     
            Session["tablaDatos"] = tablaDatos;
            dgtransferencia.DataSource = tablaDatos;
            dgtransferencia.DataBind();            
        }
        public void CargarCargo(DataGridCommandEventArgs e)
        {
            bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]), "select mot.mord_numeorde from morden mo, mordentransferencia mot where mo.pdoc_codigo = mot.pdoc_codigo and mo.mord_numeorde=mot.mord_numeorde and mo.pdoc_codigo='" + ddlprefijoorden.SelectedValue + "' and test_estado='A';");
        }
        public void GuardarNuevoCargo(Object Sender, EventArgs e)
        {
            ArrayList sqlLista = new ArrayList();
            tablaDatos = (DataTable)Session["tablaDatos"];
            for (int i = 0; i < tablaDatos.Rows.Count; i++)
            sqlLista.Add("UPDATE DBXSCHEMA.MORDENTRANSFERENCIA SET TCAR_CARGO = '" + tablaDatos.Rows[i][2] + "' WHERE PDOC_CODIGO = '"+ tablaDatos.Rows[i][0] + "' AND   MORD_NUMEORDE = " + tablaDatos.Rows[i][1] + " AND   PDOC_FACTURA = '" + tablaDatos.Rows[i][3] + "' AND   MFAC_NUMERO = " + tablaDatos.Rows[i][4] + "; ");
            bool procesoSatisfactorio = DBFunctions.Transaction(sqlLista);
            if (procesoSatisfactorio)
                    Response.Redirect(indexPage + "?process=Automotriz.ModificarCargoTransferencia&codor=" + tablaDatos.Rows[0][0] + "&numord=" + tablaDatos.Rows[0][1], false);
            else
            {
                lberror.Text = DBFunctions.exceptions;
                return;
            }
        }
    }
}