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
using System.Linq;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;
using System;
using Obout.Grid;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OboutInc.Window;
using System.IO;
using System.Web.UI.DataVisualization.Charting;

////////////////////////////////////////////////////////////////////////////////
/////////////////////////////F.R.O.G.///////////////////////////////////////////
////////////////Formato Reporte Obout Gráfico///////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

namespace AMS.Reportes
{
    enum TipoGrafica { Spline };
    
    public partial class FrogReports : System.Web.UI.Page
    {
        Obout.Grid.Grid grid1;
        TemplateContainer c;
     //   DataSet dsPrueba;
     //   ArrayList tipo;
        Boolean todasFilas;
        ArrayList seleccionados;
        Table tabVentana;
        ConsultaDescriptor descriptor;
        ArrayList filtros;
        protected Table tbFormHeader = new Table();
        Table filTabla = new Table();
        
        //HyperLink hDescarga;

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarEncabezado();

            ImprimirTod.ImageUrl = "../img/AMS.Icon.PrinterF.png";
            BtnExcel.ImageUrl = "../img/AMS.Icon.ExcelF.png";
            BtnGraficar.ImageUrl = "../img/AMS.Icon.GraphF.png";
            BtnCorreo.ImageUrl = "../img/AMS.Icon.CorreoF.png";
            BtnPlano.ImageUrl = "../img/AMS.Icon.plane_icon.png";
            BtnWord.ImageUrl = "../img/AMS.Icon.word_icon.png";
            BtnPDF.ImageUrl = "../img/AMS.Icon.pdf_icon.png";
            ImprimirTod.Enabled = true;
            BtnExcel.Enabled = true;
            BtnGraficar.Enabled = true;
            BtnCorreo.Enabled = true;
            BtnPlano.Enabled = true;

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ",";
            customCulture.DateTimeFormat.DateSeparator = "-";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            if (!Page.IsPostBack)
            {
                //Implementacion para llamar consultas globales. Cambiar parametro de idReporte->GReport
                string idReporte = Request.QueryString["SReporte"];
                if (idReporte == null)
                {
                    idReporte = Request.QueryString["idReporte"];
                    ViewState["descriptor"] = new ConsultaDescriptor(idReporte, false);
                }
                else
                {
                    ViewState["descriptor"] = new ConsultaDescriptor(idReporte, true);  //indicador para determinar que se trata de un reporte global.
                }

                ImprimirTod.ImageUrl = "../img/AMS.Icon.PrinterShdw.png";
                BtnExcel.ImageUrl = "../img/AMS.Icon.ExcelShdw.png";
                BtnGraficar.ImageUrl = "../img/AMS.Icon.GraphShdw.png";
                BtnCorreo.ImageUrl = "../img/AMS.Icon.CorreoShdw.png";
                BtnPlano.ImageUrl = "../img/AMS.Icon.plane_iconShdw.png";
                BtnWord.ImageUrl = "../img/AMS.Icon.word_iconShdw.png";
                BtnPDF.ImageUrl = "../img/AMS.Icon.pdf_iconShdw.png";
                ImprimirTod.Enabled = false;
                BtnExcel.Enabled = false;
                BtnGraficar.Enabled = false;
                BtnCorreo.Enabled = false;
                BtnPlano.Enabled = false;
            }

            descriptor = (ConsultaDescriptor)ViewState["descriptor"];
            filtros = new ArrayList();
            

            filTabla.CellPadding = 20;
            filTabla.CssClass = "tableRoundBorder";
            filTabla.ControlStyle.CssClass = "tableRoundBorder";
            filTabla.HorizontalAlign = HorizontalAlign.Left;
            int contadorFiltros = 1;
            if (Request.QueryString["vendedor"] != null)
                btnRegresar.Visible = true;
            foreach (FiltroDescriptor filtro in descriptor.Filtros)
            {
                ConsultaFiltroUI filtroUI = filtro.GetFiltroUI(Page, contadorFiltros);
                TableRow trow = new TableRow();
                trow = ((Table)filtroUI.PlaceHolder.Controls[0]).Rows[0];
                if (filtro.Label == "cod_vendedor_implicito")
                {
                    string codUsuario = DBFunctions.SingleData("SELECT SUSU_CODIGO FROM SUSUARIO WHERE SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
                    ((TextBox)trow.Cells[1].Controls[0]).Text = DBFunctions.SingleData("SELECT PVEN_CODIGO FROM PVENDEDOR WHERE SUSU_CODIGO = '" + codUsuario + "'");
                    ((TextBox)trow.Cells[1].Controls[0]).Enabled = false;
                    btnRegresar.Visible = true;
                    //((TextBox)filtroUI.Control.Controls[0]).Text = DBFunctions.SingleData("SELECT PVEN_CODIGO FROM PVENDEDOR WHERE SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
                    //((TextBox)filtroUI.Control.Controls[0]).Enabled = false;
                }
                filTabla.Rows.Add(trow);
                filtros.Add(filtroUI);
                contadorFiltros++;
            }

            plcFiltros.Controls.Add(filTabla); //Tabla de filtros


            if (ViewState["descriptorGrid"] != null && Request.QueryString["Grid"] != "2")
            {
                descriptor = (ConsultaDescriptor)ViewState["descriptorGrid"];
                ConstruiurColumnas(descriptor.Columnas);
                plcReporte.Controls.Add(grid1);
            }
            
            ConfigurarVentanaGrafica();
            ConfigurarVentanaGrafica2(null);
            
            if(Request.QueryString["idReporte"] != null)
            {
               // lblTitulo.Text = DBFunctions.SingleData("select UPPER(srep_nombre) from sreporte where srep_id=" + Request.QueryString["idReporte"] + ";");
            }

            //Para la rotación en gestión de vehículos.
            if (Request.QueryString["tim"] == "1")
            {
                GenerarReporte(sender, e);
                Response.AddHeader("REFRESH", "14;URL=" + "AMS.Reportes.FrogReports.aspx?idReporte=1013&tim=2&tall=" + Request.QueryString["tall"]);
            }
            //else if (Request.QueryString["tim"] == "2")
            //{
            //    GenerarReporte(sender, e);
            //    Response.AddHeader("REFRESH", "14;URL=" + "AMS.Reportes.FrogReports.aspx?idReporte=1014&tim=3&tall=" + Request.QueryString["tall"]);
            //}
            //else if (Request.QueryString["tim"] == "3")
            //{
            //    GenerarReporte(sender, e);
            //    Response.AddHeader("REFRESH", "14;URL=" + "AMS.Reportes.FrogReports.aspx?idReporte=7650&tim=4&tall=" + Request.QueryString["tall"]);
            //}
            else if (Request.QueryString["tim"] == "2")
            {
                    GenerarReporte(sender, e);
                    Response.AddHeader("REFRESH", "14;URL=" + "AMS.Automotriz.PlanificacionTallerVeh.aspx?tal=" + Request.QueryString["tall"] + "&pag=1&rdrt=1");
                }
        }

        //Carga encabezado de Ecas con usuario en sesion en el sistema.
        protected void CargarEncabezado()
        {
            String nameSystem = ConfigurationManager.AppSettings["SystemName"] + ".";
            String pathToControls = ConfigurationManager.AppSettings["PathToControls"];

            plcEncabezado.Controls.Add(LoadControl("" + pathToControls + nameSystem + "Tools.Encabezado.ascx"));
        }

        //Configuracion general de la ventana emergente para opciones de datos de Grafica.
        public void ConfigurarVentanaGrafica()
        {
            myWindow.IsDraggable = true;
            myWindow.IsModal = false;
            myWindow.Status = "Selección de datos a graficar";
            myWindow.Left = 200;
            myWindow.Top = 100;
            myWindow.Height = 500;
            myWindow.Width = 500;
            myWindow.Title = "Parámetros de Gráfica";
            myWindow.StyleFolder = "../grid/wdstyles/geryon";
            myWindow.PageOpacity = 0;
            myWindow.VisibleOnLoad = false;
            myWindow.ShowCloseButton = false;
            myWindow.Overflow = OverflowStyles.SCROLL;
            

            if (ViewState["descriptorGrid"] != null && Request.QueryString["Grid"] != "2")
            {
                tabVentana = ConfigurarVentanaObjetos();
                myWindow.Controls.Add(tabVentana);
            }
        }


        //Configuracion general de la ventana emergente para opciones de datos de Grafica.
        public void ConfigurarVentanaGrafica2(Chart grafica)
        {
            vntGrafica.IsDraggable = true;
            vntGrafica.ShowCloseButton = true;
            vntGrafica.Status = "Selección de datos a graficar";
            vntGrafica.Left = 100;
            vntGrafica.Top = 100;
            vntGrafica.Height = 520;
            vntGrafica.Width = 650;
            vntGrafica.Title = "Parámetros de Gráfica";
            vntGrafica.StyleFolder = "../grid/wdstyles/geryon";
            vntGrafica.VisibleOnLoad = false;
            vntGrafica.ShowCloseButton = false;
            vntGrafica.Overflow = OverflowStyles.SCROLL;

            if (grafica != null)
            {
                TableCell grafCell = new TableCell();
                grafCell.Controls.Add(grafica);

                TableCell botonCell = new TableCell();

                ImageButton btnCerrar = new ImageButton();
                btnCerrar.ID = "cVentana";
                btnCerrar.ImageUrl = "../img/AMS.Icon.Close.png";
                //btnCerrar.Click += new ImageClickEventHandler(this.CerrarVentana2);
                btnCerrar.Attributes.Add("onClick", "vntGrafica.Close();");
                botonCell.Controls.Add(btnCerrar);

                TableRow trows = new TableRow();
                trows.Cells.Add(grafCell);
                Table tabGrafica = new Table();
                tabGrafica.Rows.Add(trows);

                trows = new TableRow();
                trows.Cells.Add(botonCell);
                trows.HorizontalAlign = HorizontalAlign.Center;
                tabGrafica.Rows.Add(trows);

                trows = new TableRow();
                botonCell = new TableCell();
                botonCell.Text = "<font size='1' face='Georgia, Arial'>Cerrar</font>";
                trows.Cells.Add(botonCell);
                trows.HorizontalAlign = HorizontalAlign.Center;
                tabGrafica.Rows.Add(trows);

                tabGrafica.HorizontalAlign = HorizontalAlign.Center;
                tabGrafica.CssClass = "filtersIn";

                vntGrafica.Controls.Add(tabGrafica);
            }
        }

        //Configuracion de los objetos que muestra la ventana de graficacion.
        public Table ConfigurarVentanaObjetos()
        {
            Table tabla = new Table();
            Table topciones = new Table();
            Table tseleccion = new Table();
            Table tbotones = new Table();
            TableRow trow = new TableRow();
            TableCell tcellL;
            TableCell tcellCh1;
            TableCell tcellCh2;

            tabla.ID = "tabla1";
            trow = new TableRow();

            tcellL = new TableCell();
            tcellL.Text = "Columna";
            tcellL.BackColor = Color.Aquamarine;
            trow.Cells.Add(tcellL);

            tcellL = new TableCell();
            tcellL.Text = "Graficar Cabecera";
            tcellL.BackColor = Color.Aquamarine;
            trow.Cells.Add(tcellL);

            tcellL = new TableCell();
            tcellL.Text = "Graficar Datos*";
            tcellL.BackColor = Color.Aquamarine;
            trow.Cells.Add(tcellL);

            topciones.Rows.Add(trow);
            ArrayList tipoDatos = descriptor.Columnas;

            for (int i = 0; i < grid1.Columns.Count - 1; i++)
            {
                //celda para los nombres de columnas
                tcellL = new TableCell();
                tcellL.Text = grid1.Columns[i].HeaderText;

                //Celda para los Checklist de los encabezados de la grafica
                CheckBox chkEnc1 = new CheckBox();
                chkEnc1.ID = "chkEnc_" + i;
                chkEnc1.Attributes.Add("onClick", "unSoloCheck(this," + grid1.Columns.Count + ")");
                tcellCh1 = new TableCell();
                tcellCh1.Controls.Add(chkEnc1);

                //Celda para los Checklist de los datos a graficar (Numerico)
                CheckBox chk = new CheckBox();
                chk.ID = "chk_" + i;
                tcellCh2 = new TableCell();

                ColumnaDescriptor entry = (ColumnaDescriptor)tipoDatos[i];
                TipoCampo campo = (TipoCampo)entry.TipoCampo;

                if (campo != TipoCampo.Moneda && campo != TipoCampo.Numero)
                    chk.Enabled = false;

                tcellCh2.Controls.Add(chk);

                trow = new TableRow();
                trow.Cells.Add(tcellL);
                trow.Cells.Add(tcellCh1);
                trow.Cells.Add(tcellCh2);

                topciones.Rows.Add(trow);
            }

            tcellL = new TableCell();
            tcellL.Text = "<font size='1' face='Georgia, Arial'>*Valores númericos.</font>";
            trow = new TableRow();
            trow.Cells.Add(new TableCell());
            trow.Cells.Add(new TableCell());
            trow.Cells.Add(tcellL);
            topciones.Rows.Add(trow);

            trow = new TableRow();
            tcellL = new TableCell();
            tcellL.Text = "Seleccione el tipo de gráfica: ";
            trow.Cells.Add(tcellL);

            tcellL = new TableCell();
            DropDownList ddlTipoGrafica = new DropDownList();
            ddlTipoGrafica.ID = "ddlTipoGrafica";
            ddlTipoGrafica.Items.Add("Spline");
            ddlTipoGrafica.Items.Add("Line");
            ddlTipoGrafica.Items.Add("Barras");
            ddlTipoGrafica.Items.Add("Columnas");
            ddlTipoGrafica.Items.Add("Columnas 3D");
            ddlTipoGrafica.Items.Add("Torta");
            ddlTipoGrafica.Items.Add("Piramide");

            tcellL.Text = "Tipo de Gráfica";
            tcellL.Controls.Add(ddlTipoGrafica);
            trow.Cells.Add(tcellL);
            tseleccion.Rows.Add(trow);

            trow = new TableRow();
            tcellL = new TableCell();
            Label lFilas = new Label();
            lFilas.Text = "Todos las filas";
            tcellL.Controls.Add(lFilas);
            trow.Cells.Add(tcellL);

            CheckBox chkT = new CheckBox();
            chkT.ID = "chkTodas";
            tcellL = new TableCell();
            tcellL.Controls.Add(chkT);
            trow.Cells.Add(tcellL);
            tseleccion.Rows.Add(trow);

            ImageButton imgGraficar = new ImageButton();
            imgGraficar.ID = "grafVentana";
            imgGraficar.ImageUrl = "../img/AMS.Icon.GraphF.png";
            imgGraficar.Click += new ImageClickEventHandler(this.Graficar2);
            imgGraficar.OnClientClick = "animarBoton(this,'../img/grafica.gif');";

            ImageButton imgCerrar = new ImageButton();
            imgCerrar.ID = "closeVentana";
            imgCerrar.ImageUrl = "../img/AMS.Icon.Close.png";
            //imgCerrar.Click += new ImageClickEventHandler(this.CerrarVentana);
            imgCerrar.Attributes.Add("onClick", "myWindow.Close();");

            trow = new TableRow();
            tcellL = new TableCell();
            tcellL.Controls.Add(imgGraficar);
            trow.Cells.Add(tcellL);

            tcellL = new TableCell();
            tcellL.Controls.Add(imgCerrar);
            trow.Cells.Add(tcellL);
            trow.HorizontalAlign = HorizontalAlign.Center;

            tbotones.Rows.Add(trow);

            trow = new TableRow();
            tcellL = new TableCell();
            tcellL.Text = "<font size='1' face='Georgia, Arial'>Graficar</font>";
            trow.Cells.Add(tcellL);
            tcellL = new TableCell();
            tcellL.Text = "<font size='1' face='Georgia, Arial'>Cerrar</font>";
            trow.HorizontalAlign = HorizontalAlign.Center;
            trow.Cells.Add(tcellL);

            tbotones.Rows.Add(trow);

            topciones.CellPadding = 1;
            topciones.Width = 450;
            topciones.HorizontalAlign = HorizontalAlign.Center;
            topciones.CssClass = "tableRoundBorder";
            topciones.ControlStyle.CssClass = "tableRoundBorder";

            tseleccion.CellPadding = 1;
            tseleccion.Width = 450;
            tseleccion.HorizontalAlign = HorizontalAlign.Center;
            tseleccion.CssClass = "tableRoundBorder";
            tseleccion.ControlStyle.CssClass = "tableRoundBorder";

            tbotones.CellPadding = 2;
            tbotones.CellSpacing = 1;
            tbotones.CssClass = "tableRoundBorder";
            tbotones.ControlStyle.CssClass = "tableRoundBorder";

            tcellL = new TableCell();
            tcellL.Controls.Add(topciones);
            trow = new TableRow();
            trow.Cells.Add(tcellL);
            tabla.Rows.Add(trow);

            tcellL = new TableCell();
            tcellL.Controls.Add(tseleccion);
            trow = new TableRow();
            trow.Cells.Add(tcellL);
            tabla.Rows.Add(trow);

            tcellL = new TableCell();
            tcellL.Controls.Add(tbotones);
            trow = new TableRow();
            trow.Cells.Add(tcellL);
            tabla.Rows.Add(trow);

            tabla.HorizontalAlign = HorizontalAlign.Center;
            tabla.CellPadding = 10;

            return tabla;
        }

        protected void setHeader()
        { 
            int numRows = 2;
            int numCells = 3;
            string logoEmpresa = DBFunctions.SingleDataGlobal("SELECT GEMP_iCONO FROM GEMPRESA WHERE GEMP_NOMBRE='" + GlobalData.getEMPRESA()+ "';");
            string logo = logoEmpresa;
                for (int k = 0; k<numRows; k++)
                {
                    TableRow r = new TableRow();
                    for (int m = 0; m<numCells; m++)
                    {
                        TableCell c = new TableCell();
                        if (k == 0 && m == 0)
                        {
                            c.VerticalAlign = VerticalAlign.Top;
                            if (logo.Length > 0)
                                //c.Text = "<IMG src=\"../rpt/"+logo+"\" border=\"0\">";
                                c.Text = "<IMG src=\"" + logo + "\" border=\"0\">";
                            else
                                c.Text = "<p>&nbsp;</p>";
                        }
                        if (k == 0 && m == 1)
                        {
                            c.Text = "<center style='font-size: 18px;'>" + DBFunctions.SingleData("SELECT cemp_nombre FROM cempresa") + "</center>";
                            c.Text += "<center>NIT: " + DBFunctions.SingleData("SELECT mnit_nit FROM cempresa") + "</center>";
                            c.Text += "<center>" + DBFunctions.SingleData("SELECT mn.mnit_direccion FROM cempresa ce, mnit mn where ce.mnit_nit=mn.mnit_nit;") + "</center>";
                            c.Text += "<center>Ciudad: " + DBFunctions.SingleData("SELECT pc.pciu_nombre FROM cempresa ce, pciudad pc where ce.cemp_ciudad=pc.pciu_codigo;") + "</center>";
                            c.Text += "<center>PBX: " + DBFunctions.SingleData("SELECT mn.mnit_telefono FROM cempresa ce, mnit mn where ce.mnit_nit=mn.mnit_nit;") + "</center>";
                        }
                        if (k == 0 && m == 2)
                        if (filTabla.Rows.Count > 0)
                        {
                            for (int j = 0; j < filTabla.Rows.Count; j++)
                            {
                                string texto;
                                if (filTabla.Rows[j].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    texto = c.Text;
                                    c.Text = texto + ((Label)filTabla.Rows[j].Cells[0].Controls[0]).Text + " " + ((DropDownList)filTabla.Rows[j].Cells[1].Controls[0]).SelectedItem.Text + "<br>";
                                }
                                else if (filTabla.Rows[j].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                {
                                        texto = c.Text;
                                        c.Text = texto + ((Label)filTabla.Rows[j].Cells[0].Controls[0]).Text + " " + ((TextBox)filTabla.Rows[j].Cells[1].Controls[0]).Text + "<br>";
                                }
                            }
                            try
                            {
                                c.Text = c.Text.Substring(0, c.Text.Length - 4);
                            }
                            catch (Exception)
                            { }

                        };
                    if (k == 1 && m == 0)
                        c.Text = "Procesado en:" + Convert.ToDateTime(DateTime.Now.ToShortDateString()).GetDateTimeFormats()[87] + "";
                        if (k == 1 && m == 1)
                            c.Text = "<center>" + DBFunctions.SingleData("select UPPER(srep_nombre) from sreporte where srep_id=" + Request.QueryString["idReporte"] + ";") + "</center>";
                        if (k == 1 && m == 2)
                        {
                            //c.Text = "<div style='text-align:right'>";
                           
                           // c.Text = c.Text + "</div>";
                        }                            
                        c.Width = new Unit("33%");
                        r.Cells.Add(c);
                    }
                    tbFormHeader.Rows.Add(r);
                }
                phForm.Controls.Add(tbFormHeader);
        }

        //Generacion del reporte.
        public void GenerarReporte(Object Sender, EventArgs E)
        {
            plcBotones.Visible = true;
            //BtnGenerar.ImageUrl = "../img/AMS.Icon.GenF.png";
            BtnGenerar.Enabled = true;
            setHeader();

            Hashtable valsFitros = new Hashtable();

            foreach (ConsultaFiltroUI filtroUI in filtros)
            {
                DictionaryEntry de = filtroUI.GetValorFiltro();
                valsFitros.Add(de.Key, de.Value);
            }

            descriptor.GenerarReporte(valsFitros);
            ViewState["descriptorGrid"] = descriptor;
            GenerarCheckVisibilidad(descriptor.Columnas);

            if (Request.QueryString["Grid"] == "2" && ViewState["descriptorGrid"] != null)
            {
                GenerarRporteGrid(descriptor.Datos);

            }
            else
            {                 
                ConstruiurColumnas(descriptor.Columnas);
                plcReporte.Controls.Clear();
                plcReporte.Controls.Add(grid1);
                ConfigurarVentanaGrafica();
                //post back posible sino aparecen datos...
                CargarDatosGrid(descriptor.Datos);
            }
        }

        public void GenerarRporteGrid(DataTable datos)
        {
            //traigo los datos iniciales y los copio en un DATASET            
            DataTable dtCopy = datos.Copy();
            DataSet ds = new DataSet();
            ds.Tables.Add(dtCopy);
            DataRow[] dtFiltro = ds.Tables[0].Select("");
            DataTable dt1 = dtFiltro.CopyToDataTable();
            DataTable dtCopy1 = new DataTable();
            int i = 0;
            DataRow fila = dtCopy1.NewRow();     
            //GENERACION DE TITULOS DE COLUMNAS       
            for (int j=0; j < dt1.Columns.Count ; j++)
            {                
                dtCopy1.Columns.Add(new DataColumn (dtCopy.Columns[j].ToString(), System.Type.GetType("System.String")));
            }           
            //GENEREACION DINAMICA DE FILAS
            for (int filas = 0; filas < dtFiltro[0].Table.Rows.Count; filas++)
            {
                
                for (i = 0; i < dtFiltro[filas].Table.Columns.Count; i++)
                {
                    fila[i] = (dtFiltro[filas]).ItemArray[i].ToString();
                }
                 
                if (dtCopy.Rows[filas]["ISNIVEL"].ToString() == "1")
                {
                    dtCopy.Rows[filas][0] = "\\TOTAL\\";                    
                }
                if (dtCopy.Rows[filas]["ISNIVEL"].ToString() == "2")
                {
                    dtCopy.Rows[filas][0] = "\\TOTAL\\" + fila.ItemArray[0].ToString().Trim();                    
                }
                else if (dtCopy.Rows[filas]["ISNIVEL"].ToString() == "3")
                {
                    dtCopy.Rows[filas][0] = "\\TOTAL\\" + fila.ItemArray[0].ToString().Trim() + "\\" + fila.ItemArray[1].ToString().Trim();                   
                }
                else if(dtCopy.Rows[filas]["ISNIVEL"].ToString() == "4")
                {
                    dtCopy.Rows[filas][0] = "\\TOTAL\\" + fila.ItemArray[0].ToString().Trim() + "\\" + fila.ItemArray[1].ToString().Trim() + "\\" + fila.ItemArray[2].ToString().Trim();                    
                }
                else if (dtCopy.Rows[filas]["ISNIVEL"].ToString() == "5")
                {
                    dtCopy.Rows[filas][0] = "\\TOTAL\\" + fila.ItemArray[0].ToString().Trim() + "\\" + fila.ItemArray[1].ToString().Trim() + "\\" + fila.ItemArray[2].ToString().Trim() + "\\" + fila.ItemArray[3].ToString().Trim();                    
                }
                else if (dtCopy.Rows[filas]["ISNIVEL"].ToString() == "6")
                {
                    dtCopy.Rows[filas][0] = "\\TOTAL\\" + fila.ItemArray[0].ToString().Trim() + "\\" + fila.ItemArray[1].ToString().Trim() + "\\" + fila.ItemArray[2].ToString().Trim() + "\\" + fila.ItemArray[3].ToString().Trim() + "\\" + fila.ItemArray[4].ToString().Trim();
                }
                else if (dtCopy.Rows[filas]["ISNIVEL"].ToString() == "7")
                {
                    dtCopy.Rows[filas][0] = "\\TOTAL\\" + fila.ItemArray[0].ToString().Trim() + "\\" + fila.ItemArray[1].ToString().Trim() + "\\" + fila.ItemArray[2].ToString().Trim() + "\\" + fila.ItemArray[3].ToString().Trim() + "\\" + fila.ItemArray[4].ToString().Trim() + "\\" + fila.ItemArray[5].ToString().Trim();
                }
                else if (dtCopy.Rows[filas]["ISNIVEL"].ToString() == "8")
                {
                    dtCopy.Rows[filas][0] = "\\TOTAL\\" + fila.ItemArray[0].ToString().Trim() + "\\" + fila.ItemArray[1].ToString().Trim() + "\\" + fila.ItemArray[2].ToString().Trim() + "\\" + fila.ItemArray[3].ToString().Trim() + "\\" + fila.ItemArray[4].ToString().Trim() + "\\" + fila.ItemArray[5].ToString().Trim() + "\\" + fila.ItemArray[6].ToString().Trim();
                }
                //SEPARACION DE DATOS POR COLUMNAS
                for (int k = 1; k < dtFiltro[filas].Table.Columns.Count; k++)
                {
                    dtCopy.Rows[filas][k] = (dtFiltro[filas]).ItemArray[k].ToString().Trim();                    
                }
            }

            //CARGAR Y MOSTRAR EL DATATABLE
            dgConsulta2.FullPathDataColumn = dtCopy.Columns[0].ToString();
            dtCopy.Columns.Remove("ISNIVEL");
            dtCopy.AcceptChanges();
            dgConsulta2.DataSource = dtCopy;
            dgConsulta2.DataBind();
            //this.dgConsulta2.Columns["CustomerID"].Visible = false;
            dgConsulta2.DeleteRow(8);
        }

        private void muestraColumnasGrid()
        {
            
        }
        protected void Show_Hide_ChildGrid(object sender, EventArgs e)
        {
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer); //(imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                row.FindControl("pnlOrders").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "../img/minus.png";

                //string nivel = dgBusqueda.DataKeys[row.RowIndex].Values[0].ToString();                
                GridView gvOrders = row.FindControl("gvOrders") as GridView;
                //gvOrders.ToolTip = nivel;
                DataSet dsFiltro2 = new DataSet();
                DataSet ds = (DataSet)ViewState["dsConsulta"];
                DataRow[] dtFiltro2 = ds.Tables[0].Select("NIVEL=2");
                DataTable dt2 = dtFiltro2.CopyToDataTable();
                dsFiltro2.Tables.Add(dt2);
                gvOrders.DataSource = dsFiltro2.Tables[0];
                gvOrders.DataBind();

            }
            else
            {
                row.FindControl("pnlOrders").Visible = false;
                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "../img/plus.png";
            }

        }
        protected void GenerarCheckVisibilidad(ArrayList tipoDatos)
        {
            int k = 0;
            plcVisibilityPanel.Controls.Clear();
            hdCheckBoxes.Value = "";
            foreach (ColumnaDescriptor entry in tipoDatos)
            {
                CheckBox chkV = new CheckBox();
                chkV.ID = "chk_V" + k;
                chkV.Text = entry.Nombre;
                chkV.Checked = true;
                chkV.Attributes.Add("onclick", "ocultarColumnaChk(this," + k + ");");
                plcVisibilityPanel.Controls.Add(chkV);
                hdCheckBoxes.Value += "true,";
                k++;
            }

            //Adicion para columna final de checkList Graficador.
            CheckBox chkV2 = new CheckBox();
            chkV2.ID = "chk_V" + k;
            chkV2.Text = "Graficador";
            chkV2.Checked = false;
            chkV2.Attributes.Add("onclick", "ocultarColumnaChk(this," + k + ");");
            plcVisibilityPanel.Controls.Add(chkV2);
            hdCheckBoxes.Value += "true";
        }

        //Realiza la configuracion general de todo el diseño del Grid.
        protected void ConstruiurColumnas(ArrayList tipoDatos)
        {
            grid1 = new Obout.Grid.Grid();
            string datoConsulta = "";
            ConsultaDescriptor consulta = (ConsultaDescriptor)ViewState["descriptorGrid"];

            //Configuracion del Template para grupos.
            GridRuntimeTemplate TemplateEditAddress = new GridRuntimeTemplate();
            TemplateEditAddress.ID = "TemplateEditAddress";
            TemplateEditAddress.UseQuotes = true;
            TemplateEditAddress.Template = new Obout.Grid.RuntimeTemplate();
            TemplateEditAddress.Template.CreateTemplate +=
                new Obout.Grid.GridRuntimeTemplateEventHandler(CreateEditAddressTemplate);

            //Parametros de Grid generales.
            //grid1.Attributes.Add("position", "absolute");
            //grid1.Attributes.CssStyle.Add("position", "fixed");
            //grid1.Attributes.CssStyle.Add("float", "top");

            grid1.Templates.Add(TemplateEditAddress);
            grid1.TemplateSettings.GroupHeaderTemplateId = "TemplateEditAddress";
            grid1.AllowAddingRecords = false;
            grid1.AllowColumnReordering = true;
            grid1.AllowColumnResizing = true;
            grid1.AllowGrouping = true;
            grid1.AutoGenerateColumns = false;
            grid1.AllowFiltering = true;
            grid1.CallbackMode = true;
            grid1.AllowSorting = true;
            grid1.EnableRecordHover = true;
            grid1.FolderLocalization = "../grid/localization";
            grid1.FolderStyle = "../grid/styles/style_4";
            grid1.HideColumnsWhenGrouping = true;           
            grid1.KeepSelectedRecords = true;
            grid1.ID = "grid1";
            grid1.Language = "es";
            string pageSize = ConfigurationManager.AppSettings["frogPageSize"];
            if (pageSize == null)
                pageSize = "400";
            grid1.PageSize = Convert.ToInt16(pageSize);
            grid1.PageSizeOptions = "100,500,1000,5000,10000,50000";
            grid1.ShowCollapsedGroups = false;
            grid1.ShowColumnsFooter = true;
            grid1.ShowGroupsInfo = true; //True
            grid1.ShowMultiPageGroupsInfo = true;
            grid1.Serialize = true;
            grid1.Width = Unit.Percentage(100);

            //Creacion de Columnas
            string columnsToExport = "";
            string grupoCols = "";
            int cont = 0;
            foreach (ColumnaDescriptor entry in tipoDatos)
            {
                Column oCol1 = new Column();
                oCol1.DataField = entry.Nombre;
                oCol1.HeaderText = entry.Nombre;
                oCol1.Wrap = true;
                oCol1.AllowGroupBy = true;
                oCol1.Width = "" + entry.TamanoPromedio;//"150";

                if (entry.Agrupa == "1")
                    grupoCols += entry.Nombre + ",";

                TipoCampo campo = (TipoCampo)entry.TipoCampo;
                TipoCampo filtro = (TipoCampo)entry.TipoFiltro;

                if (campo == TipoCampo.Moneda)
                    oCol1.DataFormatString = "{0:C}";
                else if (campo == TipoCampo.Fecha)
                    oCol1.DataFormatString = "{0:dd/MM/yyyy}";
                else if (campo == TipoCampo.Entero || campo == TipoCampo.Numero)
                {
                    oCol1.Align = "right";
                }

                if (filtro == TipoCampo.Moneda)
                {
                    oCol1.DataFormatString = "{0:C}";
                    oCol1.Align = "right";
                }
                else if (filtro == TipoCampo.Numero)
                {
                    oCol1.DataFormatString = "{0:N}";
                    oCol1.Align = "right";
                }
                else if (filtro == TipoCampo.Entero)
                {
                    oCol1.DataFormatString = "{0:N0}";
                    oCol1.Align = "right";
                }

                columnsToExport += oCol1.HeaderText + ",";
                

                //if (consulta.Datos.Rows[0][cont].ToString() != "")
                    //datoConsulta += consulta.Datos.Rows[0][cont].ToString();
                //columnsToExport += datoConsulta;
                grid1.Columns.Add(oCol1);
                //columnsToExport += " aqui estoy";
                //grid1.Columns[cont].HeaderText += " aqui estoy";

                grid1.Columns[cont].AllowFilter = true;
                grid1.ShowColumnsFooter = true;
                grid1.Columns[cont].AllowSorting = true;
                cont++;
            }

            CheckBoxSelectColumn oCol2 = new CheckBoxSelectColumn();
            oCol2.ShowHeaderCheckBox = true;
            oCol2.ControlType = GridControlType.Standard;
            oCol2.ID = "ChkID";
            grid1.Columns.Add(oCol2);
            
            
            if (grupoCols.Length > 0)
                grupoCols = grupoCols.Substring(0, grupoCols.Length - 1);

            grid1.GroupBy = grupoCols;
            ConfiguracionExportarExcel(columnsToExport);
        }

        //Evento para la carga del Template para la condiguracion del Grid.
        void CreateEditAddressTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
        {
            Literal oLiteral = new Literal();
            e.Container.Controls.Add(oLiteral);
            c = e.Container;
            oLiteral.DataBinding += new EventHandler(DataBindEditAddressTemplate);
        }

        //Evento para configuracion del Template del Grid.
        void DataBindEditAddressTemplate(Object sender, EventArgs e)
        {
            Literal oLiteral = sender as Literal;
            Obout.Grid.TemplateContainer oContainer =
                oLiteral.NamingContainer as Obout.Grid.TemplateContainer;

            oLiteral.Text = "<u>" + c.Column.HeaderText + "</u> : <b><i>" + c.Value + "</i></b> (" + c.Group.PageRecordsCount + " " + (c.Group.PageRecordsCount > 1 ? "registros" : "registro") + ")";
        }

        //Configuracion de parametros para hacer exportacion del Grid a Excel y a Word.
        protected void ConfiguracionExportarExcel(string columnsToExport)
        {
            grid1.ExportingSettings.AppendTimeStamp = true;
            grid1.ExportingSettings.ExportAllPages = true;
            grid1.ExportingSettings.ExportHiddenColumns = false;
            grid1.ExportingSettings.FileName = "Archivo";
            grid1.ExportingSettings.KeepColumnSettings = true;
            grid1.ExportingSettings.ExportGroupHeader = true;

            
            grid1.FolderExports = "../dwl";

            if (!string.IsNullOrEmpty(columnsToExport))
            {
                grid1.ExportingSettings.ColumnsToExport = columnsToExport.Substring(0, columnsToExport.Length - 1);
            }
            else
            {
                grid1.ExportingSettings.ColumnsToExport = "";
            }
        }

        //Exportar excel desde servidor
        protected void EnviarExcel(Object sender, EventArgs E)
        {
            string archivo = grid1.ExportToExcel();
           
            hDescarga.NavigateUrl = "../dwl/" + archivo;
            hDescarga.Visible = true;
            //hDescarga.Target = "_blank";
        }

        //Carga de Excel via servidor
        protected void Excel(object sender, EventArgs e)
        {
            MemoryStream fileStream = new MemoryStream();
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            DateTime fecha = DateTime.Now;
            //Formato para el nombre del Archivo.
            string nombreArchivo = "Reporte" + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
            nombreArchivo = nombreArchivo.Replace(" ", "");
            nombreArchivo = nombreArchivo.Replace("/", "");
            nombreArchivo = nombreArchivo.Replace(":", "-");
            string[] hdChecks = hdCheckBoxes.Value.Split(',');

            int k = 0;
            foreach (Column col in grid1.Columns)
            {
                try
                {
                    if (hdChecks[k] == "false" && (grid1.Columns.Count > k))
                    {
                        descriptor.Datos.Columns.RemoveAt(k);
                    }
                }
                catch (Exception err)
                { }
                k++;
            }

            DataSet dsDatos = new DataSet();
            dsDatos.Tables.Add(descriptor.Datos);
            Utils.ImprimeExcel(dsDatos, nombreArchivo);

            //base.Response.Clear();
            ////base.Response.Charset = Encoding.UTF8.WebName;
            //Encoding encoding = Encoding.UTF8;
            //base.Response.Charset = encoding.EncodingName;
            //base.Response.ContentEncoding = Encoding.Default;
            ////base.Response.ContentEncoding = Encoding.Unicode;
            //base.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //base.Response.ClearContent();
            //base.Response.Buffer = false;
            //base.Response.BufferOutput = false;

            ////base.Response.ContentType = "application/vnd.xls";
            //base.Response.ContentType = "application/vnd.ms-excel";
            //base.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xls");
            ////base.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xlsx");

            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            PdfWriter wri = PdfWriter.GetInstance(doc, fileStream); 

            ////Adición del Título
            TextWriter innerTextWriter = htmlWrite.InnerWriter;
            innerTextWriter.Write(fileStream);
            innerTextWriter.Write("<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'><tr><td colspan=2><b>" + lblTitulo.Text + "</b></td></tr></table>");

            ////Adicion de parametros
            string filtrosHTML = "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>";
            //for (int h = 0; h < descriptor.Filtros.Count; h++)
            //{
            //    filtrosHTML += "<tr><td>" + ((Label)plcFiltros.FindControl("lbl_" + (h + 1))).Text + "</td>";
            //    try
            //    {
            //        switch (((FiltroDescriptor)descriptor.Filtros[0]).TipoCampo)
            //        {
            //            case TipoCampo.RelacionForanea:
            //                filtrosHTML += "<td>" + ((DropDownList)plcFiltros.FindControl("ddl_" + (h + 1))).SelectedItem + "</td></tr>";
            //                break;
            //            case TipoCampo.Cadena:
            //                filtrosHTML += "<td>" + ((TextBox)plcFiltros.FindControl("txtCadena_" + (h + 1))).Text + "</td></tr>";
            //                break;
            //            case TipoCampo.Numero:
            //                filtrosHTML += "<td>" + ((TextBox)plcFiltros.FindControl("txtFilterControl_" + (h + 1))).Text + "</td></tr>";
            //                break;
            //            case TipoCampo.Fecha:
            //                DateTime fechaCampo = Convert.ToDateTime(((TextBox)plcFiltros.FindControl("txtFilterControlF_" + (h + 1))).Text);
            //                filtrosHTML += "<td>" + fechaCampo.ToString("yyyy-MM-dd") + "</td></tr>";
            //                break;
            //        }

            //    }
            //    catch (Exception er) { }
            //}
            filtrosHTML += "<tr><td colspan=2></td></tr></table>";
            innerTextWriter.Write(filtrosHTML);

            //DataGrid dgAux = new DataGrid();
            //dgAux.DataSource = descriptor.Datos;
            //dgAux.DataBind();
            //dgAux.RenderControl(htmlWrite);

            //base.Response.Write(stringWrite.ToString());
            //base.Response.End();
        }

        //Generar archivo plano .txt
        protected void GenerarPlanoTXT(object sender, EventArgs e)
        {
            //Formato para el nombre del Archivo.
            DateTime fecha = DateTime.Now;
            string nombreArchivo = "ArchPlano" + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString() + ".txt";
            nombreArchivo = nombreArchivo.Replace(" ", "");
            nombreArchivo = nombreArchivo.Replace("/", "");
            nombreArchivo = nombreArchivo.Replace(":", "-");
            string directorioArchivo = ConfigurationManager.AppSettings["PathToDownloads"];

            StreamWriter sw = File.CreateText(directorioArchivo + nombreArchivo);

            //int numero = Convert.ToInt16(TXTNUMERO.Text.ToString());
            DataSet DatosCuerpo = new DataSet();
            DatosCuerpo.Tables.Add(descriptor.Datos);
            string textoArch = "";

            //Rows[i][h]
            //i: filas
            for (int i = 0; i < DatosCuerpo.Tables[0].Rows.Count; i++)
            {
                //h:columnas
                for (int h = 0; h < DatosCuerpo.Tables[0].Columns.Count; h++)
                {
                    if (DatosCuerpo.Tables[0].Rows[i][h].ToString() != "")
                    {
                        //textoArch += "\"" + DatosCuerpo.Tables[0].Rows[i][h].ToString().Replace(",",".") + "\",";
                        textoArch += DatosCuerpo.Tables[0].Rows[i][h].ToString().Replace(",", ".") + "!";
                    }
                    else 
                    {
                        textoArch += "!";
                    }
                    
                }
                textoArch += "@";
                textoArch = textoArch.Replace("!@", "");
                sw.WriteLine(textoArch);
                textoArch = "";
            }

            sw.Close();

            Utils.MostrarAlerta(Response, "Archivo Plano Generado con Exito. \\n \\n Instrucciones: \\n - Haga click en Descargar Archivo Plano. \\n - Revise en la parte inferior izquierda de su pantalla \\n" +
                                          "- Encontrará el archivo plano descargado. Gracias!");
            hlDescargaPlano.Visible = true;
            hlDescargaPlano.NavigateUrl = "../dwl/" + nombreArchivo;
            hlDescargaPlano.Attributes.Add("download", "../dwl/" +  nombreArchivo);

        }

        //Generar archivo PDF
        protected void GenerarPDF(object sender, EventArgs e)
        {
            //// Export current page
            //ExportGridToPDF();

            // Export all pages
            grid1.PageSize = -1;
            grid1.DataBind();
            ExportGridToPDF();
        }

        private void ExportGridToPDF()
        {
            // Stream which will be used to render the data
            MemoryStream fileStream = new MemoryStream();

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            try
            {
                //Create Document class object and set its size to letter and give space left, right, Top, Bottom Margin
                PdfWriter wri = PdfWriter.GetInstance(doc, fileStream);
                wri.PageEvent = new MyHeaderFooterEvent();

                doc.Open();//Open Document to write

                iTextSharp.text.Font fontT = FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font font8 = FontFactory.GetFont("ARIAL", 7);
                iTextSharp.text.Font fontFil = FontFactory.GetFont("ARIAL", 7);
                
                //Write some content in Title
                Paragraph paragraph = new Paragraph(lblTitulo.Text, fontFil);
                paragraph.Alignment = Element.ALIGN_CENTER;

                //Adicion de parametros
                Paragraph paragraphFiltros = new Paragraph();
                paragraphFiltros.Font = fontFil;
                string filtro = "";
                for (int h = 0; h < descriptor.Filtros.Count; h++)
                {
                    filtro = ((Label)plcFiltros.FindControl("lbl_" + (h + 1))).Text + ": ";
                    try
                    {
                        switch (((FiltroDescriptor)descriptor.Filtros[0]).TipoCampo)
                        {
                            case TipoCampo.RelacionForanea:
                                filtro += ((DropDownList)plcFiltros.FindControl("ddl_" + (h + 1))).SelectedItem ;
                                break;
                            case TipoCampo.Cadena:
                                filtro += ((TextBox)plcFiltros.FindControl("txtCadena_" + (h + 1))).Text ;
                                break;
                            case TipoCampo.Numero:
                                filtro += ((TextBox)plcFiltros.FindControl("txtFilterControl_" + (h + 1))).Text ;
                                break;
                            case TipoCampo.Fecha:
                                DateTime fechaCampo = Convert.ToDateTime(((TextBox)plcFiltros.FindControl("txtFilterControlF_" + (h + 1))).Text);
                                filtro += fechaCampo.ToString("yyyy-MM-dd");
                                break;
                        }
                        paragraphFiltros.Add(filtro);
                        paragraphFiltros.IndentationLeft = 50;
                    }
                    catch (Exception er) { }
                }
                
                //Estado de checklist via hidden value
                string[] hdChecks = hdCheckBoxes.Value.Split(',');
                int contColumnas = 0;
                for (int y = 0; y < hdChecks.Length; y++)
                {
                    if (hdChecks[y] == "true")
                    {
                        contColumnas++;
                    }
                }

                //Craete instance of the pdf table and set the number of column in that table
                PdfPTable PdfTable = new PdfPTable(contColumnas); // grid1.Columns.Count);
                PdfPCell PdfPCell = null;
                
                //Add headers of the pdf table
                int j = 0;
                foreach (Column col in grid1.Columns)
                {
                    if (hdChecks[j] == "true")
                    {
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(col.HeaderText, fontT)));
                        PdfPCell.BorderColor = new BaseColor(200, 200, 200);
                        PdfPCell.BackgroundColor = new BaseColor(210, 239, 255);
                        PdfTable.AddCell(PdfPCell);
                    }
                    j++;
                } 

                //How add the data from the Grid to pdf table
                for (int i = 0; i < grid1.Rows.Count; i++)
                {
                    Hashtable dataItem = grid1.Rows[i].ToHashtable();
                    int k = 0;
                    foreach (Column col in grid1.Columns)
                    {
                        try
                        {
                            if(hdChecks[k] == "true" && (grid1.Columns.Count > k+1))
                            {
                                PdfPCell = new PdfPCell(new Phrase(new Chunk(dataItem[col.DataField].ToString(), font8)));
                                PdfPCell.BorderColor = new BaseColor(200, 200, 200);
                                PdfTable.AddCell(PdfPCell);
                            }
                        }
                        catch(Exception err)
                        { }
                        k++;
                    }
                }

                PdfTable.SpacingBefore = 15f;

                doc.Add(paragraph);
                doc.Add(paragraphFiltros);
                doc.Add(PdfTable);
            }
            catch (DocumentException docEx)
            {
                //handle pdf document exception if any
            }
            catch (IOException ioEx)
            {
                // handle IO exception
            }
            catch (Exception ex)
            {
                // ahndle other exception if occurs
            }
            finally
            {
                //Close document and writer
                doc.Close();
            }

            //Salvar archivo en el servidor.
            File.WriteAllBytes(ConfigurationManager.AppSettings["PathToReports"] + lblTitulo.Text.Replace(" ", "") + "_" + GlobalData.getEMPRESA() + ".pdf", fileStream.ToArray());
            try
            {
                Utils.MostrarPDF(Response, lblTitulo.Text.Replace(" ", "") + "_" + GlobalData.getEMPRESA() + ".pdf", 800);
            }
            catch {  }
            fileStream.Close();
            ////Salvar Archivo en el cliente y descargar.
            //// Send the data and the appropriate headers to the browser
            //Response.Clear();
            //Response.AddHeader("content-disposition", "attachment;filename=oboutGrid.pdf");
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(fileStream.ToArray());
            //Response.End();
        }

        //Carga de datos al Grid. Solo debe hacerce una vez sin PostBack.
        protected void CargarDatosGrid(DataTable datos)
        {
            grid1.DataSource = datos;
            grid1.DataBind();
        }

        //Evento en icono Graficar para mostrar ventana de opciones de Grafica.
        public void Graficar(Object Sender, EventArgs E)
        {
            ViewState["seleccionados"] = grid1.SelectedRecords;
            myWindow.VisibleOnLoad = true;
        }

        //Configura y crea la grafica de las columnas y filas relacionadas.
        public void Graficar2(Object Sender, EventArgs E)
        {
            //DataSet dsPrueba2 = new DataSet();
            //DBFunctions.Request(dsPrueba2, IncludeSchema.NO,
            //"select mcat_vin as AnswerText, CAST ((mveh_numerece/1000) AS VARCHAR(5)) as Votes from dbxschema.mvehiculo where mveh_inventario >= 1 and mveh_inventario <= 9;");

            ArrayList tipo2 = new ArrayList();
            tipo2 = obtenerTiposGrafica();
            Graficas g = new Graficas();

            seleccionados = new ArrayList();
            seleccionados = (ArrayList)ViewState["seleccionados"];

            DropDownList ddlTipoGraf = (DropDownList)myWindow.FindControl("ddlTipoGrafica");

            Chart graf = g.ObtenerGrafica(descriptor.Datos, ddlTipoGraf.SelectedItem.ToString(), tipo2, false, todasFilas, seleccionados);

            grid1.SelectedRecords = new ArrayList();
            ViewState.Remove("seleccionados");

            ConfigurarVentanaGrafica2(graf);
            vntGrafica.VisibleOnLoad = true;
        }

        //Retorna un ArrayList con la columna de referencia[0], y columnas de datos numericos relacionados[n>0] para diferentes series en la grafica.
        public ArrayList obtenerTiposGrafica()
        {
            ArrayList tipoGraf = new ArrayList();
            ArrayList tipo = descriptor.Columnas;

            for (int y = 0; y < grid1.Columns.Count - 1; y++)
            {
                CheckBox c = (CheckBox)myWindow.FindControl("chkEnc_" + y);
                if (c.Checked)
                {
                    ColumnaDescriptor entry = (ColumnaDescriptor)tipo[y];
                    tipoGraf.Add(new DictionaryEntry(entry.Nombre, y));
                    break;
                }
            }

            for (int y = 0; y < grid1.Columns.Count - 1; y++)
            {
                CheckBox c = (CheckBox)myWindow.FindControl("chk_" + y);
                if (c.Checked)
                {
                    ColumnaDescriptor entry = (ColumnaDescriptor)tipo[y];
                    tipoGraf.Add(new DictionaryEntry(entry.Nombre, y));
                }
            }

            CheckBox cTodas = (CheckBox)myWindow.FindControl("chkTodas");
            todasFilas = cTodas.Checked;

            return tipoGraf;
        }

        //Cierra ventana de opciones para configurar la Grafica.
        public void CerrarVentana(Object Sender, EventArgs E)
        {
            myWindow.VisibleOnLoad = false;
        }

        //Cierra ventana de opciones para configurar la Grafica.
        public void CerrarVentana2(Object Sender, EventArgs E)
        {
            vntGrafica.VisibleOnLoad = false;
        }

        //Abre ventana emergente de envio por correo.
        public void EnviarCorreo(Object Sender, EventArgs E)
        {
            vntCorreo.VisibleOnLoad = true;
        }

        //Cierra ventana emergente de envio por correo.
        public void CerrarCorreo(Object Sender, EventArgs E)
        {
            vntCorreo.VisibleOnLoad = false;
        }

        //Enviar correo con adjunto de grilla seleccionada.
        public void EnviarCorreo2(Object Sender, EventArgs E)
        {
            vntCorreo.VisibleOnLoad = false;
            string archivo = grid1.ExportToExcel();
            string path = ConfigurationManager.AppSettings["PathToDownloads"];

            string urlImagen = "http://ecas.co/images/" + GlobalData.getEMPRESA() + ".png";
            string nReporte = lblTitulo.Text;
            string empresa = DBFunctions.SingleDataGlobal("select gemp_descripcion from gempresa where gemp_nombre='" + GlobalData.getEMPRESA() + "';");

            string mensaje =
                @"<div style='position: absolute; background-color:#EEEFD9;width: 35%;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888;'>
	                <img style='width: 20%; position: absolute; right: 2%;' src='" + urlImagen + @"' /><br><br>
		            <b><font size='5'>Reporte Generado:</font></b>
		            <br>" + nReporte +
                    @"<br><br>" +
                    txtArea.Text + "  " + @"
		            < br>
                    --Ha recibido un Reporte usando el Sistema Ecas. <br>
                    --Dicho reporte se encuentra disponible como archivo <br>
                    --adjunto en este correo.
		            <br><br>
	                <b>-Gracias por su atención.</b>
		            <br>
		            <i>eCAS. www.ecas.co</i>
	            </div>
                <br><br>";

            int a = Utils.EnviarMail(txtPara.Text, "Reporte de: " + empresa + ". " + txtAsunto.Text, mensaje, TipoCorreo.HTML, path + archivo);
        }
        protected void btnRegresar_Click(object sender, EventArgs z)
        {
            string codUsuario = DBFunctions.SingleData("SELECT SUSU_CODIGO FROM SUSUARIO WHERE SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
            string tipo_cod_vendedor = DBFunctions.SingleData("SELECT PVEN_CODIGO || '~' || TVEND_CODIGO FROM PVENDEDOR WHERE SUSU_CODIGO = '" + codUsuario + "'");
            Session.Clear();
            if (tipo_cod_vendedor != "" && tipo_cod_vendedor.Split('~')[1] == "VV")
            {
                //regresarPrincipalVendedores.Visible = false;
                Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Vehiculos.GestionVendedor&cod_vend=" + tipo_cod_vendedor.Split('~')[0]);
            }
            else
            {
                Response.Redirect("AMS.Web.CerrarSesion.aspx&errorVendedor=1");
            }
        }

        class MyHeaderFooterEvent : PdfPageEventHelper
        {
            iTextSharp.text.Font FONT = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9); //, iTextSharp.text.Font.BOLD);
            string empresa = DBFunctions.SingleData("select cemp_nombre from cempresa;");
            string nit = DBFunctions.SingleData("select mnit_nit from cempresa;");
            string fechaGeneracion = DateTime.Now.ToString();
            string logoEmpresa = DBFunctions.SingleDataGlobal("select gemp_icono from gempresa where gemp_nombre = '" + GlobalData.getEMPRESA() + "'");

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfContentByte canvas = writer.DirectContent;
                Phrase headerText = new Phrase("", FONT);
                headerText.Add(empresa + " - " + nit);

                Phrase dateText = new Phrase("", FONT);
                dateText.Add(fechaGeneracion);

                string logoURL = HttpContext.Current.Server.MapPath(logoEmpresa.Replace("..", ""));
                logoURL = logoURL.Replace("\\aspx", "");
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(logoURL);
                
                //Resize image depend upon your need
                jpg.ScaleToFit(50, 50f);
                //Give space before image
                jpg.SpacingBefore = 10f;
                //Give some space after the image
                jpg.SpacingAfter = 1f;
                jpg.Alignment = Element.ALIGN_LEFT;

                Paragraph asw = new Paragraph();
                asw.Alignment = 30;
                
                asw.Add(new Chunk(jpg, 30, 30, true));
                //asw.Add(empresa + " - " + nit);
                asw.IndentationLeft = 30;
                asw.SpacingAfter = 15;
                
                ColumnText.ShowTextAligned( canvas, Element.ALIGN_LEFT,
                    asw, 40, 770, 180
                );

                ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT,
                    headerText, 70, 770, 0
                );

                ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT,
                    dateText, 500, 770, 0
                );

                ColumnText.ShowTextAligned(
                  canvas, Element.ALIGN_LEFT,                                                            //continua texto->
                  new Phrase("Generado por Software de Sistemas Ecas S.A.S.                                                                                                                                       Pag." + document.PageNumber, FONT), 10, 10, 0
                );
                
            }
        }
    }
}
