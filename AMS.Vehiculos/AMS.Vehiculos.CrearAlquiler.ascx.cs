using AMS.Contabilidad;
using AMS.Documentos;
using AMS.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Tools;
using AMS.DB;
using System.Collections;
using System.Speech.Recognition;

namespace AMS.Vehiculos
{
    public partial class CrearAlquiler : System.Web.UI.UserControl
    {
        protected DataTable tablaElementos, tablaRetoma;
        DatasToControls bind = new DatasToControls();
        ProceHecEco contaOnline = new ProceHecEco();
        protected FormatosDocumentos formatoFactura;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        private FormatosDocumentos formatoRecibo = new FormatosDocumentos();
        string usuario = HttpContext.Current.User.Identity.Name;
        double totalOtrosVenta = 0;
        double costoElementos = 0;
        double ivaElementos = 0;
        private SpeechRecognitionEngine escucha = new SpeechRecognitionEngine(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT PALM_ALMACEN, PALM_DESCRIPCION FROM PALMACEN WHERE TVIG_VIGENCIA = 'V';");
                ddlAlmacen.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                txtFecha.Text = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                txtFEchini.Text = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM PVENDEDOR WHERE TVEND_CODIGO = 'TT' AND PVEN_VIGENCIA = 'V'");
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
        protected void Preparar_Tabla_Elementos()
        {
            tablaElementos = new DataTable();
            tablaElementos.Columns.Add(new DataColumn("CODIGO", System.Type.GetType("System.String")));//0
            tablaElementos.Columns.Add(new DataColumn("DESCRIPCION", System.Type.GetType("System.String")));//1
            tablaElementos.Columns.Add(new DataColumn("PLACA", System.Type.GetType("System.String")));//2
            tablaElementos.Columns.Add(new DataColumn("TIEMPO", System.Type.GetType("System.String")));//3
            tablaElementos.Columns.Add(new DataColumn("PERIODO", System.Type.GetType("System.String")));//4
            tablaElementos.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.String")));//5
            tablaElementos.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.Double")));//6
            tablaElementos.Columns.Add(new DataColumn("TOTAL", System.Type.GetType("System.String")));//7

        }

        protected void Binding_Grilla()
        {
            Session["tablaElementos"] = tablaElementos;
            grillaElementos.DataSource = tablaElementos;
            grillaElementos.DataBind();
          
            for (int i = 0; i < tablaElementos.Rows.Count; i++)
            {
                totalOtrosVenta += (Convert.ToDouble(tablaElementos.Rows[i][5]) * Convert.ToDouble(tablaElementos.Rows[i][3]) + (Convert.ToDouble(tablaElementos.Rows[i][5]) * Convert.ToDouble(tablaElementos.Rows[i][3]) *  (Convert.ToDouble(tablaElementos.Rows[i][6]) *0.01)));
                costoElementos += Convert.ToDouble(tablaElementos.Rows[i][5]) * Convert.ToDouble(tablaElementos.Rows[i][3]);
                ivaElementos += Convert.ToDouble(tablaElementos.Rows[i][5]) * Convert.ToDouble(tablaElementos.Rows[i][3]) * (Convert.ToDouble(tablaElementos.Rows[i][6]) * 0.01);
                grillaElementos.Items[i].Cells[2].HorizontalAlign = HorizontalAlign.Right;
            }
            costoOtrosElementos.Text = (costoElementos.ToString("C")).Replace(")", "");
            txtIva.Text = (ivaElementos.ToString("C")).Replace(")", "");
            try { totalVenta.Text = totalOtrosVenta.ToString("C"); }
            catch { }
        }

        protected void dgAccesorioBound(object sender, DataGridItemEventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (e.Item.ItemType == ListItemType.Footer)
            { 
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[6].Controls[1]), "SELECT piva_porciva FROM piva");
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[4].Controls[1]), "SELECT TUNI_CODIGO, TUNI_DESCRIPCION FROM TUNIDADTIEMPO");
            }
        }
        protected void Documentos_Alquiler(object sender, EventArgs e)
        {
            bind.PutDatasIntoDropDownList(ddlPrefijo, "SELECT P.PDOC_CODIGO, P.PDOC_NOMBRE FROM PDOCUMENTO P, PDOCUMENTOHECHO PH WHERE P.PDOC_CODIGO = PH.PDOC_CODIGO AND PH.PALM_ALMACEN = '"+ddlAlmacen.SelectedValue+"' AND PH.TPRO_PROCESO = 'AA' AND P.TDOC_TIPODOCU = 'FC'");
            ddlPrefijo.Items.Insert(0, "Seleccione..");
            if (ddlPrefijo.Items.Count < 2)
            {
                Utils.MostrarAlerta(Response, "No ha parametrizado documentos del tipo FC para el proceso AA (Alquiler Activos) y la SEDE " + ddlAlmacen.SelectedValue + "");
                return;
            }
        }
        protected void Consecutivo_Alquiler(object sender, EventArgs e)
        {
            txtNumero.Text = DBFunctions.SingleData("SELECT PDOC_ULTIDOCU +1 FROM PDOCUMENTO WHERE PDOC_CODIGO = '"+ddlPrefijo.SelectedValue+"'");
        }
        protected void dgEvento_Grilla(object sender, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName == "AgregarObsequios")
            {

                //Primero verificamos que los campos no sean vacios
                if (((((TextBox)e.Item.Cells[0].Controls[1]).Text) == "") || ((((TextBox)e.Item.Cells[3].Controls[1]).Text) == ""))
                    Utils.MostrarAlerta(Response, "No existe ningun elemento para adicionar O Existe algun problema con el valor");
                else
                {
                    //debemos agregar una fila a la tabla asociada y luego volver a pintar la tabla
                    DataRow fila = tablaElementos.NewRow();
                    fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                    fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                    fila[2] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
                    fila[3] = ((TextBox)e.Item.Cells[3].Controls[1]).Text;
                    fila[4] = (((DropDownList)e.Item.Cells[4].Controls[1]).SelectedValue);
                    fila[5] = ((TextBox)e.Item.Cells[5].Controls[1]).Text;
                    fila[6] = Convert.ToDouble(((DropDownList)e.Item.Cells[6].Controls[1]).SelectedValue.Trim());
                    fila[7] = (Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text) * Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text)  + (Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text) * Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text) * (Convert.ToDouble(((DropDownList)e.Item.Cells[6].Controls[1]).SelectedValue.Trim()) * 0.01))); ;
                    if (tablaElementos.Select("CODIGO='" + fila[0].ToString() + "'").Length == 0)//Rows[e.Item.TabIndex][4].ToString().Length > 0)
                    {
                        tablaElementos.Rows.Add(fila);
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "El item contiene un código de accesorio-trámite ya definido, revice por favor");
                        return;
                    }
                    Binding_Grilla();
                }
            }
            else if (((Button)e.CommandSource).CommandName == "QuitarObsequios")
            {
                tablaElementos.Rows[e.Item.ItemIndex].Delete();
                Binding_Grilla();
            }
        }

        protected void Crear_alquiler(object sender, EventArgs e)
        {
            ArrayList sqlRefs = new ArrayList();
            string horaInicio = txtHHInicio.Text +':'+ txtMMInicio.Text + ':' + "00" ;
            string horaEntrega = txtHHEntrega.Text + ':' + txtMMEntrega.Text+ ':' + "00";
            DataTable dtElementos = Session["tablaElementos"] as DataTable;
            string centrocosto = DBFunctions.SingleData("select PCCO_CENTCOST from MACTIVOFIJO WHERE MAFJ_CODIACTI = '"+dtElementos.Rows[0][0].ToString()+"'");
            //validamos que los campos de las horas tengan datos
            if (horaInicio.ToString() == "" || horaEntrega.ToString()=="")
            {
                Utils.MostrarAlerta(Response, "No ha llenado los campos de horas");
                return;
            }
            //Ahora revisamos si la clave del vendedor es valida o no
            if ((DBFunctions.SingleData("SELECT pven_clave FROM pvendedor WHERE pven_codigo='" + ddlVendedor.SelectedValue + "'") != tbClaveVend.Text.Trim()) || (ddlVendedor.SelectedValue == "0"))
            {
                Utils.MostrarAlerta(Response, "La clave de " + ddlVendedor.SelectedItem.Text + " es invalida.\\nRevise Por Favor!");
                return;
            }
            if (txtObserv.Text == "") txtObserv.Text = "Orden de Alquiler";
            sqlRefs.Add("INSERT INTO MORDENALQUILER VALUES('" + ddlPrefijo.SelectedValue + "'," + txtNumero.Text.ToString() + ",'" + txtNit.Text + "', '2', '"+txtFecha.Text+ "' , '" + txtFEchini.Text+ "', '"+horaInicio+ "', '" + txtFechentrg.Text + "', '"+horaEntrega+"', '"+ddlAlmacen.SelectedValue+"', '"+txtObserv.Text+"')");

            for (int i = 0; i < dtElementos.Rows.Count; i++)
            { 
                sqlRefs.Add("INSERT INTO DORDENALQUILER VALUES('" + ddlPrefijo.SelectedValue + "'," + txtNumero.Text.ToString() + ",'" + dtElementos.Rows[i][0] + "', '"+ dtElementos.Rows[i][5] + "', '" + dtElementos.Rows[i][6] + "' , '2', '" + dtElementos.Rows[i][3] + "', '" + dtElementos.Rows[i][4] + "', null)");
            }
            for (int i = 0; i < dtElementos.Rows.Count; i++)
            {
                sqlRefs.Add("UPDATE MALQUILERACTIVOS SET TEST_ESTADO = 2 WHERE MAFJ_CODIACTI = '"+dtElementos.Rows[i][0]+"'");
            }
            double valorVenta = Math.Round(Convert.ToDouble(totalVenta.Text.Substring(1)), 0);
            double valorIva = Math.Round(Convert.ToDouble(txtIva.Text.Substring(1)), 0);

            sqlRefs.Add("INSERT INTO mfacturacliente VALUES('" + ddlPrefijo.SelectedValue + "'," + txtNumero.Text.ToString() + ",'" + txtNit.Text + "','" + ddlAlmacen.SelectedValue + "','F','V'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Convert.ToDouble(valorVenta) + ", " + Convert.ToDouble(valorIva) + ", 0, 0" +
                        ", 0, 0, 0, 0,'" + centrocosto + "'," +
                        "'Factura Alquiler Vehiculos','" + ddlVendedor.SelectedValue + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
            sqlRefs.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU+1 WHERE PDOC_CODIGO = '" + ddlPrefijo.SelectedValue + "';");
            if (DBFunctions.Transaction(sqlRefs))
            {
                contaOnline.contabilizarOnline(ddlPrefijo.SelectedValue.ToString(), Convert.ToInt32(txtNumero.Text.ToString()), Convert.ToDateTime(txtFecha.Text), "");
                Utils.MostrarAlerta(Response, "Se ha creado la factura con prefijo " + ddlPrefijo.SelectedValue.ToString() + " y número " + txtNumero.Text.ToString() + "");
                formatoFactura = new FormatosDocumentos();
                try
                {
                    formatoFactura.Prefijo = ddlPrefijo.SelectedValue.ToString();
                    formatoFactura.Numero = Convert.ToInt32(txtNumero.Text);
                    formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue + "'");
                    if (formatoFactura.Codigo != string.Empty)
                    {
                        if (formatoFactura.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                    }
                    formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue + "'");
                    if (formatoFactura.Codigo != string.Empty)
                    {
                        if (formatoFactura.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=500,WIDTH=700');</script>");
                    }
                    string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                    Session.Clear();
                    Response.Redirect("" + indexPage + "?process=Vehiculos.FacturacionTramite&prefD=" + ddlPrefijo.SelectedValue + "&numD=" + txtNumero.Text + "");
                }
                catch
                {
                    lberror.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                }
            }
            else
            {
                lberror.Text = "Error ejecutando:" + DBFunctions.exceptions;
            }

        }

        private void Escuchar_Click(object sender, EventArgs e)
        {
            try
            {
                escucha.SetInputToDefaultAudioDevice();//abirmos el puerto de audio
                escucha.LoadGrammar(new DictationGrammar());//Diccionario que reconoce las palabras
                escucha.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Lector); //recoje las palabras y las envia al metodo
                escucha.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (InvalidOperationException)
            {
                Utils.MostrarAlerta(Response, "No se puede abrir el puerto de Voz mas de una Vez.");
            }
        }
        public void Lector(object sender, SpeechRecognizedEventArgs e)
        {
            foreach (RecognizedWordUnit palabra in e.Result.Words)
            {
                txtObserv.Text += palabra.Text;
            }
        }
    }
}