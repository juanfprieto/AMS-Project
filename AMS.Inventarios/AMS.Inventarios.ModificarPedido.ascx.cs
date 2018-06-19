using AMS.DB;
using AMS.Forms;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AMS.Documentos;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class ModificarPedido : System.Web.UI.UserControl
	{
        protected DataTable dtInserts;
        protected DataSet ds;
        protected DataView dvInserts;
        protected ArrayList types = new ArrayList();
        protected ArrayList lbFields = new ArrayList();
        private string valorReal;
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlCodigo, "SELECT PPED_CODIGO, PPED_NOMBRE FROM PPEDIDO ORDER BY PPED_NOMBRE;");
                CargarPedidos();
                if (Request.QueryString["prefPed"] != null && Request.QueryString["numPed"] != null)
                    Utils.MostrarAlerta(Response, "Se ha modificado el pedido " + Request.QueryString["prefPed"] + "-" + Request.QueryString["numPed"] + "");
            }
			if(Session["dtInsertsCP"]==null){
                LoadDataColumns();
				LoadDataTable();
            }
			else
				dtInserts = (DataTable)Session["dtInsertsCP"];
            valorReal = Request.Form[hdValor.UniqueID];
		}
        protected void LoadDataColumns()
        {
            lbFields.Add("mite_codigo");//0
            types.Add(typeof(string));
            lbFields.Add("mite_nombre");//1
            types.Add(typeof(string));
            lbFields.Add("mite_cantidad");//2
            types.Add(typeof(double));
            lbFields.Add("mite_cantasig");//3
            types.Add(typeof(double));
            lbFields.Add("mite_precio");//4
            types.Add(typeof(double));
            lbFields.Add("mite_iva");//5
            types.Add(typeof(double));
            lbFields.Add("mite_desc");//6
            types.Add(typeof(double));
            lbFields.Add("mite_tot");//7
            types.Add(typeof(double));
            lbFields.Add("mite_disp");//8
            types.Add(typeof(double));
            lbFields.Add("mite_totA");//9
            types.Add(typeof(double));
            lbFields.Add("mite_precioinicial");//10
            types.Add(typeof(double));
            lbFields.Add("plin_codigo");//11
            types.Add(typeof(string));
            lbFields.Add("mite_color");//12
            types.Add(typeof(string));
        }
        protected void LoadDataTable()
        {
            int i;
            dtInserts = new DataTable();
            for (i = 0; i < lbFields.Count; i++)
                dtInserts.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
            Session["dtInsertsCP"] = dtInserts;
        }
        private void CargarPedidos()
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlNumero, "SELECT MPED_NUMEPEDI FROM MPEDIDOITEM WHERE PPED_CODIGO='" + ddlCodigo.SelectedValue + "' AND ( (PPED_CODIGO,MPED_NUMEPEDI) NOT IN (SELECT PPED_CODIGO,MPED_NUMEPEDI FROM DPEDIDOITEM WHERE DPED_CANTFACT>0 GROUP BY PPED_CODIGO,MPED_NUMEPEDI)) ORDER BY MPED_NUMEPEDI;");
        }
        protected void ddlCodigo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPedidos();
        }
        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            DataSet dsPedido = new DataSet();
            DataRow drPedido;
            DBFunctions.Request(dsPedido, IncludeSchema.NO,
                "SELECT * FROM MPEDIDOITEM " +
                "WHERE PPED_CODIGO='" + ddlCodigo.SelectedValue + "' AND MPED_NUMEPEDI=" + ddlNumero.SelectedValue + ";");
            if (dsPedido.Tables[0].Rows.Count == 0)
            {
                Utils.MostrarAlerta(Response, "No se encontró el pedido!");
                return;
            }
            drPedido=dsPedido.Tables[0].Rows[0];
            lblTipoPedido.Text = drPedido["mped_claseregi"].ToString();
            lblNIT.Text = drPedido["mnit_nit"].ToString();
            lblNITa.Text = DBFunctions.SingleData("SELECT mnit_apellidos concat ' ' CONCAT COALESCE(mnit_apellido2,'') concat ' ' CONCAT mnit_nombres concat ' ' concat COALESCE(mnit_nombre2,'') as Nombre FROM mnit where mnit_nit='" + drPedido["mnit_nit"].ToString() + "';");
            lblAlmacen.Text = drPedido["palm_almacen"].ToString();
            lblAlmacena.Text = DBFunctions.SingleData("SELECT PALM_DESCRIPCION FROM PALMACEN WHERE tvig_vigencia='V' and PALM_ALMACEN='" + drPedido["palm_almacen"].ToString() + "';");
            lblFecha.Text = Convert.ToDateTime(drPedido["MPED_PEDIDO"]).ToString("yyyy-MM-dd");
            lblObservacion.Text = drPedido["MPED_OBSERVACION"].ToString();
            plcPedido.Visible = true;
            btnSeleccionar.Visible = false;
            ddlCodigo.Enabled = false;
            ddlNumero.Enabled = false;
            ViewState["TIPOPEDIDO"] = DBFunctions.SingleData("SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + ddlCodigo.SelectedValue + "'").Trim();
            ViewState["NACIONALIDAD"] = DBFunctions.SingleData("SELECT TNAC_TIPONACI FROM mnit where MNIT_NIT='" + lblNIT.Text + "'");
            //Cargar Detalle
            dsPedido=new DataSet();
            DBFunctions.Request(dsPedido, IncludeSchema.NO,
                "SELECT DP.*, coalesce(DP.piva_porciva,0) AS IVA, MI.mite_nombre, MI.plin_codigo "+
                "FROM DPEDIDOITEM DP, MITEMS MI WHERE MI.MITE_CODIGO=DP.MITE_CODIGO AND DP.PPED_CODIGO='" + ddlCodigo.SelectedValue + "' AND DP.MPED_NUMEPEDI=" + ddlNumero.SelectedValue + ";");
            double totA;
            dtInserts.Rows.Clear();
            foreach (DataRow drD in dsPedido.Tables[0].Rows)
            {
                DataRow drDetalle = dtInserts.NewRow();
                totA = Convert.ToDouble(drD["DPED_CANTPEDI"]) * Convert.ToDouble(drD["DPED_VALOUNIT"]);
                totA = totA + Math.Round(totA * (Convert.ToDouble(drD["IVA"]) / 100), 0);
                drDetalle["mite_codigo"] = drD["mite_codigo"];//0  0
                drDetalle["mite_nombre"] = drD["mite_nombre"];//1
                drDetalle["mite_cantidad"] = drD["DPED_CANTPEDI"];//2  5
                drDetalle["mite_cantasig"] = drD["DPED_CANTASIG"];//3
                drDetalle["mite_precio"] = drD["DPED_VALOUNIT"];//4  2
                drDetalle["mite_iva"] = drD["IVA"];//5  3
                drDetalle["mite_desc"] = drD["DPED_PORCDESC"];//6  4
                drDetalle["mite_tot"] = totA;//7
                drDetalle["mite_disp"] = drD["DPED_CANTASIG"];//8
                drDetalle["mite_totA"] = totA;//9
                drDetalle["mite_precioinicial"] = drD["DPED_VALOUNIT"];//10
                drDetalle["plin_codigo"] = drD["plin_codigo"];//11
                drDetalle["mite_color"] = "";//12
                dtInserts.Rows.Add(drDetalle);
            }
            BindDatas();
        }
        protected void BindDatas()
        {
            int i, j;
            dgItems.EnableViewState = true;
            dvInserts = new DataView(dtInserts);
            dgItems.DataSource = dtInserts;
            Session["dtInsertsCP"] = dtInserts;
            string tipoPedido = ViewState["TIPOPEDIDO"].ToString();
            dgItems.DataBind();
            for (i = 0; i < dgItems.Columns.Count; i++)
                if (i >= 3 && i <= 9)
                    for (j = 0; j < dgItems.Items.Count; j++)
                        dgItems.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
            //Debgemos revisar si es tipo cliente y asi colocar los respectivos colores del semaforo
            if (tipoPedido != "P")
            {
                for (i = 0; i < dtInserts.Rows.Count; i++)
                {
                    if (dtInserts.Rows[i][12].ToString() == "0")
                        dgItems.Items[i].Cells[4].BackColor = Color.Red;
                    else if (dtInserts.Rows[i][12].ToString() == "1")
                        dgItems.Items[i].Cells[4].BackColor = Color.Yellow;
                    else if (dtInserts.Rows[i][12].ToString() == "2")
                        dgItems.Items[i].Cells[4].BackColor = Color.Green;
                    else
                        lbInfo.Text += "<br>" + dtInserts.Rows[i][12].ToString();
                }
            }
            txtNumItem.Text = dtInserts.Rows.Count.ToString();
            double t = 0, ta = 0;
            int n;
            if (dtInserts.Rows.Count > 0)
            {
                for (n = 0; n < dtInserts.Rows.Count; n++)
                {
                    t += Convert.ToDouble(dtInserts.Rows[n][7]);
                    ta += Convert.ToDouble(dtInserts.Rows[n][9]);
                }
            }
            txtTotal.Text = t.ToString("C");
            txtTotAsig.Text = ta.ToString("C");
            if (dtInserts.Rows.Count == 0)
                dgItems.EditItemIndex = -1;
        }
        protected void DgInsertsDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                //((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Remove("OnBlur");
                //((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("OnBlur", "ConsultarInfoReferencia(this,'" + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + "','" + ((TextBox)e.Item.Cells[1].Controls[1]).ClientID + "','" + Request.QueryString["actor"] + "','" + ddlCodigo.ClientID + "','" + ((TextBox)e.Item.Cells[3].Controls[1]).ClientID + "','" + ddlTipoOrden.ClientID + "','" + ddlNumOrden.ClientID + "','" + hdTipoPed.ClientID + "','" + ddlPrecios.ClientID + "','" + ddlAlmacen.ClientID + "','" + ((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).ClientID + "','" + ((Label)e.Item.Cells[6].FindControl("lbPrecMin")).ClientID + "','" + ddlCodigo.ClientID + "');");
                //                                                                                  (objSender,     idDDLLinea,                                                   idObjNombre,                                           tipoCliente,					idDDLTipoPedido,                  idObjCantidad,									idDDLOTPref,				idDDLOTNum,				idhdTipPed,						idDDLLista,			idDDLalmacen,					idobjValor																idlbPre														idPrefPed		))
                if (hdDescCli.Value == "")
                    ((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text = "0";
                else
                    ((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text = hdDescCli.Value;
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]), "SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
                /*((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onkeyup", "CargaNIT2(" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "," + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + ");");
                if (Tipo == "P")
                {
                    ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("ondblclick", "CargaNITP(" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "," + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + ",'" + DBFunctions.SingleData("SELECT pano_ano FROM cinventario") + "');");
                }
                else
                    ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("ondblclick", "CargaNIT(" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "," + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + ",'" + DBFunctions.SingleData("SELECT pano_ano FROM cinventario") + "'," + ddlPrecios.ClientID + ");");
                ((DropDownList)e.Item.Cells[2].Controls[1]).Attributes.Add("onchange", "ChangeLine(" + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + "," + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + ");");
                if (Tipo == "P")
                {
                    ((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Visible = true;
                }*/
                //Se va agregar al boton de reiniciar un confirm de javascript para el boton de reiniciar
                ((Button)e.Item.Cells[10].FindControl("btnClear")).Attributes.Add("onclick", "return confirm('Esta seguro de reiniciar la información del pedido?');");
                /*if (Tipo == "M")
                {
                    if (ViewState["CASA_MATRIZ"].ToString() == "S")
                    {
                        ((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Attributes.Add("readonly", "readonly");
                        //((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Attributes.Add("readonly","readonly");
                    }
                    try
                    {
                        if (tipoPedido == "P")
                            ((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text = "0";
                        else ((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text = Convert.ToDouble(DBFunctions.SingleData(
                            "SELECT mcli_porcdescinv FROM mcliente WHERE mnit_nit='" + lblNIT.Text + "';"
                            )).ToString();
                    }
                    catch
                    {
                        ((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text = "0";
                    }
                }*/
            }

            /*if (e.Item.ItemType == ListItemType.EditItem)
            {
                if (Tipo == "M")
                    if (ViewState["CASA_MATRIZ"].ToString() == "S")
                    {
                        ((TextBox)e.Item.Cells[6].FindControl("edit_precio")).Attributes.Add("readonly", "readonly");
                        //((TextBox)e.Item.Cells[8].FindControl("edit_2")).Attributes.Add("readonly","readonly");
                    }
            }*/
        }
        protected void DgInserts_Delete(Object sender, DataGridCommandEventArgs e)
        {
            try
            {
                //Solo si la cantidad asignada es cero
                if (Convert.ToDouble(dtInserts.Rows[e.Item.ItemIndex][3]) > 0)
                {
                    Utils.MostrarAlerta(Response, "No puede eliminarse un item con cantidad asignada!");
                    return;
                }
                dtInserts.Rows.Remove(dtInserts.Rows[e.Item.ItemIndex]);
                dgItems.EditItemIndex = -1;
            }
            catch { };
            BindDatas();
        }
        public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            dgItems.EditItemIndex = -1;
            BindDatas();
        }
        protected void DgInserts_AddAndDel(object sender, DataGridCommandEventArgs e)
        {
            string tipoPedido = ViewState["TIPOPEDIDO"].ToString();
            if (((Button)e.CommandSource).CommandName == "AddDatasRow")
            {
                if (CheckValues(e))
                {
                    int ivm = 1;
                    double cant = 0;
                    double prec = 0;
                    double desc = 0, descM = 0;
                    string Tipo=lblTipoPedido.Text;

                    if (tipoPedido != "T")
                        cant = Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
                    else
                    {
                        double cantidadIngresada = Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
                        cant = cantidadIngresada;
                    }
                    if (Tipo == "C")
                    {
                        prec = Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text);
                        if (tipoPedido == "P")
                            desc = 0;
                        else desc = Convert.ToDouble(((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text);
                    }
                    else if (Tipo == "P")
                    {
                        prec = Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text);
                        desc = Convert.ToDouble(((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text);
                    }
                    else if (Tipo == "M")
                    {
                        if (tipoPedido == "P")
                        {
                            desc = 0;
                            descM = 0;
                        }
                        else
                        {
                            descM = Convert.ToDouble(DBFunctions.SingleData("SELECT mcli_porcdescinv FROM mcliente WHERE mnit_nit='" + lblNIT.Text + "';"));
                            desc = Convert.ToDouble(((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text);
                        }
                        prec = Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text);
                        if (desc > descM)
                        {
                            desc = descM;
                            Utils.MostrarAlerta(Response, "El descuento se ha modificado ya que supera el máximo permitido para el cliente!");
                        }

                    }
                    InsertaItem(((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim(), (((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[0], cant, prec, desc, ivm);//Se le pasa el codigo, cantidad solicitada,precio,descuento y un indicativo si es una solicitud de taller
                }
                else
                    Utils.MostrarAlerta(Response, "Algun valor no es valido para la inserción o el precio es cero!");
            }
            BindDatas();
        }
        public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
        {	//Este selecciona la fila- Nos permite la edicion de un item agregado
            if (dtInserts.Rows.Count == 0)//Como no hay nada, no se pone a actualizar nada
                return;
            double cant = 0;
            string Tipo = lblTipoPedido.Text;
            string tipoPedido = ViewState["TIPOPEDIDO"].ToString();
            string nNacionalidad = ViewState["NACIONALIDAD"].ToString();
            if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text))
            {
                Utils.MostrarAlerta(Response, "Cantidad Invalida!");
                dgItems.EditItemIndex = -1;
                BindDatas();
                return;
            }
            //Se debe revisar si el pedido es tipo transferencia se debe restringir el numero de items a que sea menor o igual que la cantidad configurada
            cant = Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
            if (tipoPedido == "T")
            {
                string codI = "";
                //Referencias.Guardar(((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim(),ref codI,(((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[1]);
                Referencias.Guardar(dtInserts.Rows[e.Item.DataSetIndex][0].ToString(), ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtInserts.Rows[e.Item.DataSetIndex][11].ToString() + "'"));
            }
            if (cant <= 0)//Si la cantidad pedida es menor o igual que cero se le asigna 1
                cant = 1;
            double pr = 0;
            if (Tipo != "P")
                //pr = Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][4]); //Precio
                try { pr = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text); }
                catch { pr = Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][4]); }
            else if (Tipo == "P")
            {
                //Validamos si el valor digitado es valido o no
                if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))
                {
                    Utils.MostrarAlerta(Response, "Precio Ingresado Invalido!");
                    dgItems.EditItemIndex = -1;
                    BindDatas();
                    return;
                }
                pr = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);//Precio
            }
            double desc = 0, descM = 0;
            //if(Tipo=="C")
            desc = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);//Porcentaje de descuento ¿Este no se debe de cargar automaticamente?
            if (desc < 0)
                desc = 0;
            if (desc > 100)
                desc = 100;
            if (Tipo == "M")
            {
                if (tipoPedido == "P")
                    descM = 0;  // LOS PEDIDOS PROMOCION NO TIENEN DESCUENTO
                else descM = Convert.ToDouble(DBFunctions.SingleData("SELECT mcli_porcdescinv FROM mcliente WHERE mnit_nit='" + lblNIT.Text + "';"));
                if (desc > descM)
                {
                    desc = descM;
                    Utils.MostrarAlerta(Response, "El descuento se ha modificado ya que supera el máximo permitido para el cliente!");
                }
            }
            double cantD = Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][8]);//Cantidad Disponible
            double cantA = 0;//Cantidad Asignada
            // Para las cotizaciones solo se graba el pedido sin asignacion ni facturacion ni lista de empaque ni back-order en msaldoitems
            // Las cotizaciones deben tener un formato asociado que debe pedir   si o no  imprime los codigos de los productos..
            // En algun momento, una cotizacion se convierte en un caso para facturar, ahi seria aplicar el procedimiento regular de pedidos
            // pero partiendo de la cotizacion y cargandola en la grilla de los pedidos
            if (tipoPedido != "C")
            {
                if (Tipo != "P")
                {
                    if (cant > cantD)
                        cantA = cantD;
                    else
                        cantA = cant;
                }
                else if (Tipo == "P")
                    cantA = cant;
            }
            double iva;
            if (nNacionalidad == "E") iva = 0;
            else
            {
                iva = Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][5]);//Iva
            }
            double tot = cant * pr;
            if (Tipo != "P" && tipoPedido != "E" && tipoPedido != "G" && tipoPedido != "P")//Pedido Cliente y distinto de Interno, Garantia y Promoción aplico descuento
                tot = tot - Math.Round((desc / 100) * tot, 0);
            else if (Tipo == "P")//Si es proveedor, aplic descuento
                tot = tot - Math.Round((desc / 100) * tot, 0);
            else
            {
                Utils.MostrarAlerta(Response, "El tipo de pedido escogido no permite aplicar descuento");
                desc = 0;
            }
            tot = tot + Math.Round(tot * (iva / 100), 0);
            double totA = cantA * pr;
            totA = totA + Math.Round(totA * (iva / 100), 0);
            if (Tipo != "P" && tipoPedido != "E" && tipoPedido != "G" && tipoPedido != "P")
                totA = totA - Math.Round((desc / 100) * totA, 0);
            dtInserts.Rows[dgItems.EditItemIndex][2] = cant;
            dtInserts.Rows[dgItems.EditItemIndex][3] = cantA;
            dtInserts.Rows[dgItems.EditItemIndex][4] = pr;
            dtInserts.Rows[dgItems.EditItemIndex][6] = desc;
            dtInserts.Rows[dgItems.EditItemIndex][7] = tot;
            dtInserts.Rows[dgItems.EditItemIndex][8] = cantD;
            dtInserts.Rows[dgItems.EditItemIndex][9] = totA;
            dtInserts.Rows[dgItems.EditItemIndex][10] = pr;
            if (Tipo == "P")
                dtInserts.Rows[dgItems.EditItemIndex][4] = pr;
            dgItems.EditItemIndex = -1;
            BindDatas();
        }
        public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
        {
            string Tipo = lblTipoPedido.Text;
            if (dtInserts.Rows.Count > 0)
                dgItems.EditItemIndex = (int)e.Item.ItemIndex;
            BindDatas();
            if (Tipo == "P")
                ((TextBox)dgItems.Items[dgItems.EditItemIndex].Cells[6].Controls[1]).ReadOnly = false;
        }
        protected void InsertaItem(string CodNIT, string linea, double cant, double prec, double descuento, int ivm)
        {
            ds = new DataSet();
            if (CodNIT.Length > 0)
            {
                string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
                string mes_cinv = DBFunctions.SingleData("SELECT pmes_mes from cinventario");
                string codI = "";
                double prIni = 0, pr = 0;
                string Tipo = lblTipoPedido.Text;
                string tipoPedido = ViewState["TIPOPEDIDO"].ToString();
                string nNacionalidad = ViewState["NACIONALIDAD"].ToString();
                if (!Referencias.Guardar(CodNIT, ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + linea + "'")))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " no es valido para la linea de bodega " + DBFunctions.SingleData("SELECT plin_nombre FROM plineaitem WHERE plin_codigo='" + linea + "'") + ".\\nRevise Por Favor!");
                    return;
                }
                if (!Referencias.RevisionSustitucion(ref codI, linea))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " NO se encuentra registrado. " + codI + "\\nRevise Por Favor!");
                    return;
                }
                if (Tipo == "M" && !DBFunctions.RecordExist("SELECT mnit_nit FROM mcliente WHERE mnit_nit='" + lblNIT.Text + "';"))
                {
                    Utils.MostrarAlerta(Response, "No se encuentra registrado el cliente!");
                    return;
                }
                string CodNIT2 = "";
                Referencias.Editar(codI, ref CodNIT2, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + linea + "'"));
                if (CodNIT2 != CodNIT)
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " se ha sustituido.\\nEl codigo actual es " + CodNIT2 + "!");
                //Revisar que no este ya el item en lista
                if ((this.dtInserts.Select("mite_codigo='" + CodNIT + "'")).Length > 0)
                {
                    BindDatas();
                    Utils.MostrarAlerta(Response, "El item ya esta en la lista, intente actualizarlo!\\nCodigo Item :" + CodNIT + "  \\nDescripción:" + DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mitems.mite_codigo='" + codI + "'") + "");
                    return;
                }
                //Que tipo de pedido es, si el pedido es tipo Garantia(G) o Interno(I) el precio del item no se toma de la lista de precios sino es el costo promedio + el factor de garantia o interno
                //Cantidad: Si la cantidad es menor o igual que cero le asigna 1
                if (cant <= 0)
                    cant = 1;
                //Datos del item cod, nom, iva, porcentaje de ganancia
                DBFunctions.Request(ds, IncludeSchema.NO, "select mitems.mite_codigo, mitems.mite_nombre, mitems.piva_porciva, mitems.plin_codigo, pdescuentoitem.pdes_porcdesc from mitems, pdescuentoitem where mitems.mite_codigo='" + codI + "' and pdescuentoitem.pdes_codigo=mitems.pdes_codigo");
                //															0					1				2					3                         4
                //Precio del item segun lista de precios
                if (Tipo != "P")
                {
                    if (Convert.ToDouble(valorReal) != prec)
                    {
                        if (tipoPedido == "E")
                            prec = prec + (prec * (Convert.ToDouble(DBFunctions.SingleData("SELECT cinv_factorinterno FROM cinventario")) / 100));
                    }
                }
                prIni = pr = prec;
                //Descuento del cliente
                double desc = 0;
                if (Tipo != "P")
                {
                    /*string descs = DBFunctions.SingleData("SELECT MCLI_PCDESC FROM MCLIENTE WHERE MNIT_NIT='" +lblNIT.Text+"'");
                    if(descs.Length > 0)
                        desc = Convert.ToDouble(descs);*/
                    if (tipoPedido == "E" || tipoPedido == "P") // PEDIDOS de Emergencia y Promocion NO TIENEN DESCUENTO
                        desc = 0;
                    else
                        desc = descuento;
                }
                else if (Tipo == "P")//Aqui traemos el porcentaje de ganancia en este item y sugerimos el precio de compra de este item al proveedor
                    desc = descuento;
                double cantA = 0;						//Cantidad Asignada
                double cantD = Referencias.ConsultarDisponibilidad(codI, lblAlmacen.Text, ano_cinv, 0); //Cantidad Disponible
                if (tipoPedido != "C")
                {
                    if (Tipo != "P")
                    {
                        if (cant > cantD)
                            cantA = cantD;
                        else

                            cantA = cant;
                    }
                    else if (Tipo == "P")
                        cantA = cant;
                    //Iva
                }
                if (nNacionalidad == "E") ivm = 0;  // los extranejros no pagan iva
                double iva = Convert.ToDouble(ds.Tables[0].Rows[0][2]) * ivm;//Si se esta realizando una transferencia de taller no se liquida el iva todavia
                //Total
                double tot = cant * pr;
                tot = tot + Math.Round(tot * (iva / 100), 0);
                tot = tot - Math.Round((desc / 100) * tot, 0);
                double totA = cantA * pr;
                totA = totA + Math.Round(totA * (iva / 100), 0);
                totA = totA - Math.Round((desc / 100) * totA, 0);
                //Llenar nueva fila
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dtInserts.NewRow();
                    dr[0] = CodNIT;//Codigo
                    dr[1] = ds.Tables[0].Rows[0][1];//Nombre
                    dr[2] = cant;					//Cantidad
                    dr[3] = cantA;					//CantidadAsig
                    //dr[4] = pr;						//Precio
                    dr[4] = prIni;						//Precio
                    dr[5] = iva;					//IVA
                    dr[6] = desc;					//Descuento
                    //dr[7] = tot;					//Total
                    dr[7] = totA;					//Total
                    dr[8] = cantD;					//Disponible
                    dr[9] = totA;					//Total Asignado
                    dr[10] = prIni;					//Precio Inicial
                    dr[11] = ds.Tables[0].Rows[0][3];//Linea
                    //Vamos a determinar cual es el color del semaforo
                    dr[12] = Referencias.ConsultaSemaforoDisponibilidad(codI, lblAlmacen.Text, Convert.ToUInt32(mes_cinv), Convert.ToInt32(ano_cinv)).ToString();
                    dtInserts.Rows.Add(dr);
                }
                if (hdDescCli.Value == string.Empty)
                    hdDescCli.Value = desc.ToString();
            }
        }
        protected bool CheckValues(DataGridCommandEventArgs e)
        {
            bool check = true;
            string Tipo = lblTipoPedido.Text;
            if (((TextBox)e.Item.Cells[0].Controls[1]).Text == "" || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text))
                check = false;
            else if (Tipo == "P" && !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text))
                check = false;
            else if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text) || Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text) == 0)
                check = false;
            else if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].FindControl("tbfdesc")).Text) || Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("tbfdesc")).Text) > 100)
                check = false;
            return check;
        }
        protected void NewAjust(Object Sender, EventArgs E)
        {
            string Tipo = lblTipoPedido.Text;
            PedidoFactura pedfac = new PedidoFactura(
                ViewState["TIPOPEDIDO"].ToString(), // 1 Tipo de Pedido
                ddlCodigo.SelectedValue, // 2 Prefijo Documento
                lblNIT.Text, // 3 Nit
                lblAlmacen.Text, // Almacen
                "", // 5 Vendedor
                Convert.ToUInt32(ddlNumero.SelectedValue), // 6 Numero Pedido
                "", // 7 Prefijo OT
                0, // 8 Numero OT
                Tipo, // 9 Tipo de Pedido
                "", // 10 Cargo
                DateTime.Now,  // 11 Fecha
                "", // 12 Observaciones
                null
                );
            for (int n = 0; n < dtInserts.Rows.Count; n++) //Se agregan las filas que detallan el pedido
            {
                string codI = "";
                Referencias.Guardar(dtInserts.Rows[n][0].ToString(), ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtInserts.Rows[n][11].ToString() + "'"));
                pedfac.InsertaFila(
                    codI, // Codigo de Item
                    0, // Cantidad Facturada
                    Convert.ToDouble(dtInserts.Rows[n][4]), // Precio
                    Convert.ToDouble(dtInserts.Rows[n][5]), // Porcentaje IVA
                    Convert.ToDouble(dtInserts.Rows[n][6]), // Porcentaje Descuento
                    Convert.ToDouble(dtInserts.Rows[n][2]), // Cantidad Pedida
                    "", // Codigo del pedido
                    ""//Numero del pedido
                    );
            }
            bool status = true;
            status = pedfac.ActualizarPed(true);
            if (status)
            {
                Session.Clear();
                string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                Response.Redirect("" + indexPage + "?process=Inventarios.ModificarPedido&path=" + Request.QueryString["path"] + "&prefPed=" + pedfac.Coddocumento + "&numPed=" + pedfac.Numpedido + "");
                lbInfo.Text += pedfac.ProcessMsg;
            }
            /*
            if (!ValidarDatos())
                return;

            string PD = "", ND = "";//Prefijo y Numero de OT (en caso que sea una transferencia a taller)
            uint numD = 0;

            string tp = DBFunctions.SingleData("SELECT TPED_CODIGO FROM ppedido where pped_codigo='" + ddlCodigo.SelectedValue + "'");
            string tVIG = DBFunctions.SingleData("SELECT TVIG_VIGENCIA FROM MNIT where MNIT_NIT ='" + txtNIT.Text + "'");
            if (tVIG == "B")
            {
                Response.Write("<script language='javascript'>alert('Este Nit NO está Vigente !!!');</script>");
                return;
            }
            try
            {
                if (tp == "T")
                {
                    PD = ddlTipoOrden.SelectedValue;
                    ND = ddlNumOrden.SelectedValue;
                    if (this.ddlNumOrden.SelectedIndex == 0)
                    {
                        Response.Write("<script language='javascript'>alert('Debe Seleccionar una orden para la transferencia!');</script>");
                        return;
                    }
                    if (DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='" + PD + "' ") == "OT")
                    {
                        if (DBFunctions.SingleData("SELECT test_estado FROM MORDEN WHERE pdoc_codigo='" + PD + "' AND MORD_NUMEORDE=" + ND + " ") != "A")
                        {
                            Response.Write("<script language='javascript'>alert('El tipo, numero o estado de la Orden de Trabajo NO es valido!');</script>");
                            return;
                        }
                    }
                    else if (DBFunctions.SingleData("SELECT test_estado FROM MORDENPRODUCCION WHERE pdoc_codigo='" + PD + "' AND MORD_NUMEORDE=" + ND + " ") != "A")
                    {
                        Response.Write("<script language='javascript'>alert('El tipo, numero o estado de la Orden de Produccion NO es valido!');</script>");
                        return;
                    }

                    try { numD = Convert.ToUInt32(ND); }
                    catch { };
                }
            }
            catch (Exception ex) { lbInfo.Text = ex.ToString(); }
            //Constructor Tipo 2 Solo Pedido
            PedidoFactura pedfac = new PedidoFactura(
                tp, // 1 Tipo de Pedido
                ddlCodigo.SelectedValue, // 2 Prefijo Documento
                txtNIT.Text, // 3 Nit
                ddlAlmacen.SelectedValue, // Almacen
                this.ddlVendedor.SelectedValue, // 5 Vendedor
                Convert.ToUInt32(txtNumPed.Text), // 6 Numero Pedido
                PD, // 7 Prefijo OT
                numD, // 8 Numero OT
                Tipo, // 9 Tipo de Pedido
                ddlCargo.SelectedValue, // 10 Cargo
                calDate.SelectedDate,  // 11 Fecha
                txtObs.Text // 12 Observaciones
                );
            int n;
            for (n = 0; n < dtInserts.Rows.Count; n++) //Se agregan las filas que detallan el pedido
            {
                string codI = "";
                Referencias.Guardar(dtInserts.Rows[n][0].ToString(), ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtInserts.Rows[n][11].ToString() + "'"));
                pedfac.InsertaFila(
                    codI, // Codigo de Item
                    0, // Cantidad Facturada
                    Convert.ToDouble(dtInserts.Rows[n][4]), // Precio
                    Convert.ToDouble(dtInserts.Rows[n][5]), // Porcentaje IVA
                    Convert.ToDouble(dtInserts.Rows[n][6]), // Porcentaje Descuento
                    Convert.ToDouble(dtInserts.Rows[n][2]) // Cantidad Pedida
                    );
            }
            bool status = true;
            facRealizado = pedfac.RealizarPed(ref status, true);
            if (status)
            {
                Session.Clear();
                if (!procesoCombinado)
                {
                    string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                    Response.Redirect("" + indexPage + "?process=Inventarios.CrearPedido&path=Crear Pedido" + "&actor=" + Tipo + "&numLis=" + pedfac.Numerolista + "&pedCre=0&prefPed=" + pedfac.Coddocumento + "&numPed=" + pedfac.Numpedido + "");
                    lbInfo.Text += pedfac.ProcessMsg;
                }
            }
            else
                lbInfo.Text += pedfac.ProcessMsg;
            txtNumPed.Text = DBFunctions.SingleData("SELECT pped_ultipedi+1 FROM ppedido WHERE pped_codigo = '" + ddlCodigo.SelectedValue + "'");
           */
        }
    }
}