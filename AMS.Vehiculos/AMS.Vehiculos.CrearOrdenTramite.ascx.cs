using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Tools;
using AMS.DB;
using System.Data;
using AMS.Forms;
using AMS.Documentos;
using System.Collections;
using AMS.Contabilidad;
using System.Configuration;

namespace AMS.Vehiculos
{
    public partial class CrearOrdenTramite : System.Web.UI.UserControl
    {
        protected DataTable tablaElementos, tablaRetoma, tablaDatos;
        DatasToControls bind = new DatasToControls();
        ProceHecEco contaOnline = new ProceHecEco();
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        private FormatosDocumentos formatoRecibo = new FormatosDocumentos();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //Session.Clear();
                if (ddlOpciVehiDetalle.Items.Count <= 0)
                {
                    ddlOpciVehiDetalle.Enabled = false;
                    ddlOpciVehiDetalle.Items.Insert(0, new ListItem("Sin detalle..", "0"));
                }
                try
                {
                    bind.PutDatasIntoDropDownList(ddlPrefijo, "SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'OS'");
                    ddlPrefijo.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                    bind.PutDatasIntoDropDownList(ddlPrefEdit, "SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'OS'");
                    ddlPrefEdit.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                }
                catch { }
                string prefijo = Request.QueryString["codor"];
                string numero = Request.QueryString["numord"];
                string eliminado = Request.QueryString["Elim"];
                string editado = Request.QueryString["Edit"];
                if (prefijo != null && numero != null)
                {
                    try
                    {
                        Utils.MostrarAlerta(Response, "se ha creado la orden de tramite con preijo " + Request.QueryString["codor"] + " y numero " + Request.QueryString["numord"] + "");
                        formatoRecibo.Prefijo = Request.QueryString["codor"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numord"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["codor"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                        }
                    }
                    catch
                    {
                        lberror.Text += "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>";
                    }
                }
                if (eliminado == "OK")
                {
                    Utils.MostrarAlerta(Response, "Proceso Realizado Satisfactoriamente");
                }
                if (editado == "ok")
                {
                    Utils.MostrarAlerta(Response, "Proceso Realizado Satisfactoriamente");
                } 
                
                this.Preparar_Tabla_Elementos();
                this.Binding_Grilla();
            }
            else
            {
                if (Session["tablaElementos"] == null)
                {
                    this.Preparar_Tabla_Elementos();
                    this.Binding_Grilla();
                }
                else
                {
                    tablaElementos = (DataTable)Session["tablaElementos"];
                    Session["tablaElementos"] = tablaElementos;
                }
            }
        }
        protected void Realizar_Tramite(Object Sender, EventArgs e)
        {
            if (tipoTramite.SelectedValue == "CP")
            {
                PlOpcion.Visible = false;
                PlEliminar.Visible = false;
                PlEditar.Visible = false;
                Pltramite.Visible = true;
                txtNit.Visible = false;
                txtNita.Visible = false;
                lbNit.Visible = false;
                lbNumero.Visible = false;
                imglupa1.Visible = false;
                bind.PutDatasIntoDropDownList(ddlTramite, "SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'OS'");
                ddlTramite.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                bind.PutDatasIntoDropDownList(ddlPedido, "SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'PC'");
                ddlPedido.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                bind.PutDatasIntoDropDownList(ddlOpciVehiDetalle, "SELECT POPC_OPCIVEHI, POPC_NOMBOPCI FROM POPCIONVEHICULO");
            }
            else if (tipoTramite.SelectedValue == "SP")
            {
                PlOpcion.Visible = false;
                PlEliminar.Visible = false;
                PlEditar.Visible = false;
                Pltramite.Visible = true;
                bind.PutDatasIntoDropDownList(ddlTramite, "SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'OS'");
                ddlTramite.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                ddlNumPedi.Items.Insert(0, new ListItem("0", "0"));
                ddlPedido.Visible = false;
                ddlNumPedi.Visible = false;
                lbNumeroPed.Visible = false;
                lbPrefijoPed.Visible = false;
                Image1.Visible = false;

            }
        }

        protected void Editar_Tramite (object sender, EventArgs e)
        {
            PlOpcion.Visible = false;
            PlEliminar.Visible = false;
            PlEditar.Visible = false;
            Pltramite.Visible = true;
            ddlTramite.Enabled = false;
            txtNita.Enabled = false;
            ddlPedido.Visible = false;
            ddlNumPedi.Visible = false;
            lbNumeroPed.Visible = false;
            lbPrefijoPed.Visible = false;
            Image1.Visible = false;
            btnAccion.Visible = false;
            btnEdicion.Visible = true;


            DataSet ds = new DataSet();
            DataRow fila;
            int i = 0;
            DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT P.PITE_NOMBRE, D.DORD_VALOTRAM, D.PIVA_PORCIVA,P.TITE_CODIGO, D.PTRA_CODIGO, D.DORD_DOCUREFE, M.PDOC_CODIGO, M.MORD_NUMEORDE, M.MNIT_NIT, VM.NOMBRE
                                                        FROM DORDENTRAMITE D, PITEMVENTAVEHICULO P, MORDENTRAMITE M,  VMNIT VM
                                                        WHERE D.PDOC_CODIGO = '" + ddlPrefEdit.SelectedValue+"' AND D.MORD_NUMEORDE = "+ddlNumEdit.SelectedValue+ " AND  P.PITE_CODIGO = D.PTRA_CODIGO AND D.PDOC_CODIGO = M.PDOC_CODIGO AND M.MORD_NUMEORDE = D.MORD_NUMEORDE AND M.MNIT_NIT = VM.mNIT_NIT");

            if (ds.Tables[0].Rows.Count != 0)
            {
                Preparar_Tabla_Elementos();
                bind.PutDatasIntoDropDownList(ddlTramite, "SELECT M.PDOC_CODIGO, PDOC_DESCRIPCION FROM MORDENTRAMITE M, PDOCUMENTO P WHERE M.PDOC_CODIGO = '" + ddlPrefEdit.SelectedValue + "' AND MORD_NUMEORDE = " + ddlNumEdit.SelectedValue + " AND M.PDOC_CODIGO = P.PDOC_CODIGO;");
                txtNumero.Text = ds.Tables[0].Rows[i][7].ToString();
                txtNit.Text = ds.Tables[0].Rows[i][8].ToString();
                txtNita.Text = ds.Tables[0].Rows[i][9].ToString();
            }                         
            else { Utils.MostrarAlerta(Response, "No existen Ordenes de tramite en Proceso para este Nit"); return; };
            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                fila = tablaElementos.NewRow();
                fila[0] = ds.Tables[0].Rows[i][0].ToString();
                fila[1] = ds.Tables[0].Rows[i][1].ToString();
                fila[2] = ds.Tables[0].Rows[i][2].ToString();
                fila[3] = ds.Tables[0].Rows[i][3].ToString();
                fila[4] = ds.Tables[0].Rows[i][4].ToString();
                fila[5] = ds.Tables[0].Rows[i][5].ToString();
                tablaElementos.Rows.Add(fila);
                Session["tablaElementos"] = tablaElementos;
                grillaElementos.DataSource = tablaElementos;
                grillaElementos.DataBind();
            }
            Binding_Grilla();
        }
        protected void Eliminar_Tramite(Object Sender, EventArgs e)
        {
            ArrayList sqlRefs = new ArrayList();
            sqlRefs.Add("UPDATE DORDENTRAMITE SET TEST_ESTADO = 'D' WHERE PDOC_CODIGO = '" + ddlPrefijo.SelectedValue + "' AND MORD_NUMEORDE = " + ddlNumInic.SelectedValue + "");
            sqlRefs.Add("UPDATE MORDENTRAMITE SET TEST_ESTADO = 'D' WHERE PDOC_CODIGO = '" + ddlPrefijo.SelectedValue + "' AND MORD_NUMEORDE = " + ddlNumInic.SelectedValue + "");
            if (DBFunctions.Transaction(sqlRefs))
            {
                Response.Redirect(indexPage + "?process=Vehiculos.CrearOrdenTramite&Elim=OK", false);
            }

        }

        protected void Cargar_Numeros(Object Sender, EventArgs e)
        {
                bind.PutDatasIntoDropDownList(ddlNumEdit, "SELECT M.MORD_NUMEORDE FROM MORDENTRAMITE M WHERE M.TEST_ESTADO = 'A' AND M.PDOC_CODIGO = '" + ddlPrefEdit.SelectedValue + "' AND MORD_NUMEORDE NOT IN (SELECT MORD_NUMEORDE FROM MFACTURACLIENTETRAMITE MF) ORDER BY 1;");
                ddlNumEdit.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                bind.PutDatasIntoDropDownList(ddlNumInic, "SELECT M.MORD_NUMEORDE FROM MORDENTRAMITE M WHERE M.TEST_ESTADO = 'A' AND M.PDOC_CODIGO = '" + ddlPrefijo.SelectedValue + "' AND MORD_NUMEORDE NOT IN (SELECT MORD_NUMEORDE FROM MFACTURACLIENTETRAMITE MF) ORDER BY 1;");
                ddlNumInic.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
        }
        protected void Cambio_PrefijoTramite(Object Sender, EventArgs e)
        {
            txtNumero.Text = DBFunctions.SingleData("SELECT PDOC_ULTIDOCU+1 FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'OS' AND PDOC_CODIGO = '" + ddlTramite.SelectedValue + "'");
        }
        protected void Cambio_PrefijoPedido(Object Sender, EventArgs e)
        {
            bind.PutDatasIntoDropDownList(ddlNumPedi, @"SELECT MP.MPED_NUMEPEDI, MP.MPED_NUMEPEDI FROM MPEDIDOVEHICULO MP 
                                                        LEFT JOIN MORDENTRAMITE M ON MP.PDOC_CODIGO = M.MPED_CODIGO AND MP.MPED_NUMEPEDI = M.MPED_NUMEPEDI
                                                        WHERE TEST_TIPOESTA < '30'
                                                        AND M.MPED_CODIGO IS NULL
                                                        AND M.MPED_NUMEPEDI IS NULL
                                                         AND MP.PDOC_CODIGO = '" + ddlPedido.SelectedValue + "'");
        }
        protected void dgEvento_Grilla(object sender, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName == "AgregarObsequios")
            {

                //Primero verificamos que los campos no sean vacios
                if (((((TextBox)e.Item.Cells[0].Controls[1]).Text) == "") || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text)))
                    Utils.MostrarAlerta(Response, "No existe ningun elemento para adicionar O Existe algun problema con el valor");
                else
                {
                    //debemos agregar una fila a la tabla asociada y luego volver a pintar la tabla
                    DataRow fila = tablaElementos.NewRow();
                    fila[0] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                    fila[1] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
                    fila[2] = Convert.ToDouble(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim());
                    fila[3] = DBFunctions.SingleData("SELECT TITE_CODIGO FROM DBXSCHEMA.pitemventavehiculo where PITE_CODIGO = '" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + "'");
                    fila[4] = DBFunctions.SingleData("SELECT PITE_CODIGO FROM DBXSCHEMA.pitemventavehiculo where PITE_CODIGO = '" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + "'");
                    fila[5] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
                    //if (tablaElementos.Select("CODIGO='" + fila[4].ToString() + "'").Length == 0)//Rows[e.Item.TabIndex][4].ToString().Length > 0)
                    //{
                    tablaElementos.Rows.Add(fila);
                    //}
                    //else
                    //{
                    //    Utils.MostrarAlerta(Response, "El item contiene un código de accesorio-trámite ya definido, revice por favor");
                    //    return;
                    //}
                    Binding_Grilla();
                }
            }
            else if (((Button)e.CommandSource).CommandName == "QuitarObsequios")
            {
                tablaElementos.Rows[e.Item.ItemIndex].Delete();
                tablaElementos.AcceptChanges();
                Binding_Grilla();
            }
           else if (((Button)e.CommandSource).CommandName == "Update")
            {
                try
                {
                    tablaElementos.Rows[e.Item.ItemIndex]["CODIGO"] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                    tablaElementos.Rows[e.Item.ItemIndex]["DESCRIPCION"] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                    tablaElementos.Rows[e.Item.ItemIndex]["COSTO"] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;                   
                    tablaElementos.Rows[e.Item.ItemIndex]["OBSERVACION"] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
                    tablaElementos.Rows[e.Item.ItemIndex]["IVA"] = ((DropDownList)e.Item.FindControl("ddlIVA")).SelectedValue;                    
                }
                catch
                {
                   
                }
                tablaElementos.AcceptChanges();
                Session["tablaElementos"] = tablaElementos;
                grillaElementos.DataSource = tablaElementos;
                grillaElementos.EditItemIndex = -1;
                Binding_Grilla();
                grillaElementos.DataBind();
            }
            else if (((Button)e.CommandSource).CommandName == "Cancel")
            {
                grillaElementos.DataSource = tablaElementos;
                grillaElementos.EditItemIndex = -1;
                grillaElementos.DataBind();
            }
        }
        protected void Preparar_Tabla_Elementos()
        {
            tablaElementos = new DataTable();            
            tablaElementos.Columns.Add(new DataColumn("DESCRIPCION", System.Type.GetType("System.String")));//1
            tablaElementos.Columns.Add(new DataColumn("COSTO", System.Type.GetType("System.Double")));//2
            tablaElementos.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.Double")));//3
            tablaElementos.Columns.Add(new DataColumn("TIPO_TRAMITE", System.Type.GetType("System.String")));//4
            tablaElementos.Columns.Add(new DataColumn("CODIGO", System.Type.GetType("System.String")));//0
            tablaElementos.Columns.Add(new DataColumn("OBSERVACION", System.Type.GetType("System.String")));//5

        }
        protected void Binding_Grilla()
        {
            Session["tablaElementos"] = tablaElementos;
            grillaElementos.DataSource = tablaElementos;
            grillaElementos.DataBind();
            double totalOtrosVenta = 0;
            for (int i = 0; i < tablaElementos.Rows.Count; i++)
            {
                try
                {
                    totalOtrosVenta += (Convert.ToDouble(tablaElementos.Rows[i][1]) + (Convert.ToDouble(tablaElementos.Rows[i][1]) * (Convert.ToDouble(tablaElementos.Rows[i][2]) / 100)));
                    grillaElementos.Items[i].Cells[2].HorizontalAlign = HorizontalAlign.Right;
                }
                catch { };
            }
            costoOtrosElementos.Text = (totalOtrosVenta.ToString("C")).Replace(")", "");
            try { totalVenta.Text = totalOtrosVenta.ToString("C"); }
            catch { }
        }
        protected void dgAccesorioBound(object sender, DataGridItemEventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (e.Item.ItemType == ListItemType.Footer)
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[3].Controls[1]), "SELECT piva_porciva FROM piva");
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                ((TextBox)e.Item.Cells[0].Controls[1]).Text = tablaElementos.Rows[e.Item.ItemIndex]["CODIGO"].ToString();                
                ((TextBox)e.Item.Cells[1].Controls[1]).Text = tablaElementos.Rows[e.Item.ItemIndex]["DESCRIPCION"].ToString();
                ((TextBox)e.Item.Cells[2].Controls[1]).Text = tablaElementos.Rows[e.Item.ItemIndex]["COSTO"].ToString();
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[3].FindControl("ddlIVA")), "SELECT piva_porciva FROM piva");
                try {((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue = tablaElementos.Rows[e.Item.ItemIndex]["IVA"].ToString(); } catch { }
                ((TextBox)e.Item.Cells[4].Controls[1]).Text = tablaElementos.Rows[e.Item.ItemIndex]["OBSERVACION"].ToString(); 
            }
        }

        protected void dgServicios_Edicion(object sender, DataGridCommandEventArgs e)
        {
            grillaElementos.EditItemIndex = e.Item.ItemIndex;
            grillaElementos.DataSource = tablaElementos;
            grillaElementos.DataBind();
        }

        protected void EjecutarAccion(object sender, EventArgs e)
        {
            ArrayList sqlRefs = new ArrayList();
            DataTable datos = new DataTable();
            string nitPrincipal = DBFunctions.SingleData("SELECT MNIT_NIT FROM MPEDIDOVEHICULO WHERE PDOC_CODIGO = '" + ddlPedido.SelectedValue + "' AND MPED_NUMEPEDI = '" + ddlNumPedi.SelectedValue + "'");
            if (nitPrincipal == "")
                nitPrincipal = txtNit.Text;
            Pedidos.PedidoVehiculosCliente(ddlPedido.SelectedValue, Convert.ToUInt32(ddlNumPedi.SelectedValue),
            "C", 0, tablaElementos, datos, ref sqlRefs, 1, "OrdenTramite", 0.00, 0.00, descripcionObsequios.Text, nitPrincipal, ddlTramite.SelectedValue, txtNumero.Text);
            bool procesoSatisfactorio = DBFunctions.Transaction(sqlRefs);
            if (procesoSatisfactorio)
              { 
                 Response.Redirect(indexPage + "?process=Vehiculos.CrearOrdenTramite&codor=" + ddlTramite.SelectedValue + "&numord=" + txtNumero.Text, false);
              }
            else
              {
                  lberror.Text = DBFunctions.exceptions;
                  return;
              }           
        }

        protected void Guardar_Edicion(object sender, EventArgs e)
        {
            ArrayList sqlRefs = new ArrayList();
            DataTable datos = new DataTable();
            string nitPrincipal = DBFunctions.SingleData("SELECT MNIT_NIT FROM MPEDIDOVEHICULO WHERE PDOC_CODIGO = '" + ddlPedido.SelectedValue + "' AND MPED_NUMEPEDI = '" + ddlNumPedi.SelectedValue + "'");
            if (nitPrincipal == "")
                nitPrincipal = txtNit.Text;
            if (ddlNumPedi.SelectedValue == "") ddlNumPedi.Items.Insert(0, new ListItem("0", "0"));
            Pedidos.PedidoVehiculosCliente(ddlPedido.SelectedValue, Convert.ToUInt32(ddlNumPedi.SelectedValue),
            "C", 0, tablaElementos, datos, ref sqlRefs, 2, "OrdenTramite", 0.00, 0.00, descripcionObsequios.Text, nitPrincipal, ddlTramite.SelectedValue, txtNumero.Text);
            bool procesoSatisfactorio = DBFunctions.Transaction(sqlRefs);
            if (procesoSatisfactorio)
            {
                Response.Redirect(indexPage + "?process=Vehiculos.CrearOrdenTramite&Edit=Ok", false);
            }
            else
            {
                lberror.Text = DBFunctions.exceptions;
                return;
            }
        }
    }
}