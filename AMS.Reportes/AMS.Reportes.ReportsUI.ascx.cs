
using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using AMS.CriptoServiceProvider;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Reportes
{

    public partial class CrystalReportUI : System.Web.UI.UserControl
    {
        #region Atributos

        protected int i;
        protected string numRep;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string sels = "";
        protected string images = ConfigurationManager.AppSettings["PathToImages"];
        protected string CodigoReporte = "";
        protected string pathToReports = ConfigurationManager.AppSettings["PathToPreviews"];
        protected string ConString = ConfigurationManager.AppSettings["ConnectionString"];
        protected string Modulo = "";
        protected string NombreReporte = "";
        protected string Vista = "";
        protected string Seleccion1 = "";
        protected string Seleccion2 = "";
        protected DataRow dr = null;
        protected Table tbForm = new Table();
        protected Table tbParams = new Table();
        protected PlaceHolder phForm;
        protected DataGrid dgTest;
        protected DataSet ds = new DataSet();
        protected DataSet ds1 = new DataSet();
        protected TextBox tbRefItem = new TextBox();
        protected DropDownList ddlRefItem = new DropDownList();
        protected System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        protected System.Web.UI.WebControls.Image imghlp = new System.Web.UI.WebControls.Image();
        protected System.Web.UI.WebControls.Calendar cal;
        protected AMS_Tools_Email miMail = new AMS_Tools_Email();

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            if (Request.QueryString["CodRep"] == null)
            {
                Utils.MostrarAlerta(Response, "No se ha definido el Reporte");
                return;
            }

            tbForm.BackColor = Color.FromArgb(255, 255, 255);
            tbForm.BorderWidth = new Unit(1);
            tbForm.Font.Name = "arial";

            if (IsValidSchema())
            {
                SetForm();
                ConstruirControlesParametros();

                ListarPrinter listprint = new ListarPrinter();
                listprint.mirar();
                ArrayList Lista = listprint.Valores;
                Impresoras.DataSource = Lista;
                Impresoras.DataBind();
            }
            else
            {
                Utils.MostrarAlerta(Response, "No se han definido datos de la configuracion de empresa, o parametros para el informe");
            }


        }

        protected void Cambiar_Fecha(object Sender, EventArgs e)
        {
            string ind = ((System.Web.UI.WebControls.Calendar)Sender).ID;
            string[] partes = ind.Split('_');
            ((TextBox)phForm1.FindControl("param_" + partes[partes.Length - 1])).Text = ((System.Web.UI.WebControls.Calendar)Sender).SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void Cambiar_Combos(object Sender, EventArgs e)
        {
            string[] id = ((DropDownList)Sender).ID.Split('_');
            DataSet tempDS = new DataSet();
            DataRow[] tienenPadre = ds.Tables[1].Select("SFIL_CODIGOPADRE <> ''");
            ArrayList hijos = new ArrayList();
            if (tienenPadre.Length != 0)
            {
                for (int i = 0; i < tienenPadre.Length; i++)
                {
                    string[] partes = tienenPadre[i][4].ToString().Split(',');
                    for (int j = 0; j < partes.Length; j++)
                    {
                        if (partes[j] == id[id.Length - 1])
                            hijos.Add(tienenPadre[i][5].ToString());
                    }
                }
            }
            if (hijos.Count != 0)
            {
                bool reporteODBC = (Request.QueryString["CodRepG"] != null);
                for (int i = 0; i < hijos.Count; i++)
                {
                    DatasToControls bind = new DatasToControls();
                    string p = "";
                    if (reporteODBC == false)
                        p = DBFunctions.SingleData("SELECT SFIL_CODIGOPADRE FROM SFILTROREPORTECRYSTAL WHERE SREP_CODIGO=" + Request.QueryString["CodRep"] + " AND SFIL_CODIGO=" + hijos[i].ToString() + "");
                    else
                        p = DBFunctions.SingleDataGlobal("SELECT SFIL_CODIGOPADRE FROM GFILTROREPORTECRYSTAL WHERE SREP_CODIGO=" + Request.QueryString["CodRep"] + " AND SFIL_CODIGO=" + hijos[i].ToString() + "");

                    bind.PutDatasIntoDropDownList(((DropDownList)tbForm.FindControl("param_" + hijos[i].ToString())), Establecer_Select(hijos[i].ToString(), p.Split(',')));
                }
            }
        }

        protected void btnImprimir_Click(object sender, System.EventArgs e)
        {
            //lbInfo.Visible = true;
            //tbMail.Visible = true;
            //imgBtnMail.Visible = true;

            this.Llenar_Seleccion();
            string[] Formulas = new string[8];
            string[] ValFormulas = new string[8];
            string header = "", footer = "";
            bool reporteODBC = Convert.ToBoolean(ViewState["reporteODBC"]);

            if (reporteODBC == false)
            {
                if (DBFunctions.SingleData("SELECT srep_header FROM sreportecrystal WHERE srep_codigo=" + Request.QueryString["CodRep"] + "") == "S")
                    header = "AMS_HEADER.rpt";
                if (DBFunctions.SingleData("SELECT srep_footer FROM sreportecrystal WHERE srep_codigo=" + Request.QueryString["CodRep"] + "") == "S")
                    footer = "AMS_FOOTER.rpt";
            }
            else
            {
                if (DBFunctions.SingleDataGlobal("SELECT srep_header FROM greportecrystal WHERE srep_codigo=" + ds.Tables[0].Rows[0][0] + "") == "S")
                    header = "AMS_HEADER.rpt";
                if (DBFunctions.SingleDataGlobal("SELECT srep_footer FROM greportecrystal WHERE srep_codigo=" + ds.Tables[0].Rows[0][0] + "") == "S")
                    footer = "AMS_FOOTER.rpt";
            }

            DataSet tempDS = new DataSet();
            string where = "", filtro = "";
            string[] filtros;
            string sql = "";
            Formulas[0] = "CLIENTE";
            Formulas[1] = "NIT";
            Formulas[2] = "TITULO";
            Formulas[3] = "TITULO1";
            Formulas[4] = "SELECCION";
            Formulas[5] = "SELECCION2";
            Formulas[6] = "VERSION";
            Formulas[7] = "REPORTE";

            ValFormulas[0] = Empresa.Text;
            ValFormulas[1] = "NIT : " + Nit.Text;
            ValFormulas[2] = Titulo.Text;
            ValFormulas[3] = Subtitulo.Text;
            ValFormulas[4] = Seleccion1;
            ValFormulas[5] = Seleccion2;
            ValFormulas[6] = "AMS " + ConfigurationManager.AppSettings["Version"];
            if (this.CodigoRep.Text != string.Empty)
                ValFormulas[7] = "REPORTE : " + this.Modulo + " " + CodigoRep.Text;
            else
                ValFormulas[7] = "REPORTE : " + this.Modulo + " " + Request.QueryString["CodRep"];
            int copias = 1;
            int paginainicial = 1;
            int paginafinal = 1;
            try
            {
                string vistaO = Vista.Trim();
                string[] vistas = null;
                if (vistaO.Length > 0)
                {
                    vistas = Vista.Split(';');
                    DataTable dtVistas = new DataTable();
                    dtVistas = DBFunctions.Request(new DataSet(), IncludeSchema.NO, "SELECT NAME FROM SYSIBM.SYSTABLES WHERE TYPE = 'V' AND  CREATOR = 'DBXSCHEMA';").Tables[0];
                    for (int i = 0; i < vistas.Length; i++)
                        //if (!vistas[i].ToUpper().Equals("VRELLENO") && DBFunctions.RecordExist("select NAME from sysibm.SYSTABLES where NAME='" + vistas[i].ToString().Trim().ToUpper() + "_R';"))
                        if (dtVistas.Select("NAME = '" + vistas[i].ToString().Trim().ToUpper() + "_R'").Length > 0)//DBFunctions.RecordExist("select NAME from sysibm.SYSTABLES where NAME='" + vistas[i].ToString().Trim().ToUpper() + "_R';"))
                            sql += "DROP VIEW DBXSCHEMA." + vistas[i].ToString().Trim().ToUpper() + "_R;";
                }
                if (sql.Length == 0 || DBFunctions.NonQuery(sql) == -1)
                {
                    DataRow[] drchb = ds.Tables[1].Select("TTIP_FILTRO='B'");
                    filtros = ds.Tables[0].Rows[0][10].ToString().Split(';');
                    //Si hay algun filtro de tipo boolean
                    if (drchb.Length != 0)
                    {
                        if (filtros.Length != 1 && Obtener_Valor(drchb[0][5].ToString()).ToLower() == "true")
                        {
                            filtro = filtros[filtros.Length - 1];
                            filtro = filtro.Replace("@", "'");
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                filtro = filtro.Replace("#" + ds.Tables[1].Rows[i][5].ToString(), Obtener_Valor(ds.Tables[1].Rows[i][5].ToString()));
                                if (i == ds.Tables[1].Rows.Count - 1)
                                    where += filtro;
                            }
                        }
                        else
                        {
                            filtro = filtros[0].Replace("@", "'");
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                filtro = filtro.Replace("#" + ds.Tables[1].Rows[i][5].ToString(), Obtener_Valor(ds.Tables[1].Rows[i][5].ToString()));
                                if (i == ds.Tables[1].Rows.Count - 1)
                                    where += filtro;
                            }
                        }
                    }
                    else
                    {
                        filtro = filtros[0].Replace("@", "'");
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            filtro = filtro.Replace("#" + ds.Tables[1].Rows[i][5].ToString(), Obtener_Valor(ds.Tables[1].Rows[i][5].ToString()));
                            if (i == ds.Tables[1].Rows.Count - 1)
                                where += filtro;
                        }
                    }
                    sql = "";
                    if (vistaO.Length > 0)
                    {
                        if (where.Length == 0)
                        {

                            for (int i = 0; i < vistas.Length; i++)
                                //if (!vistas[i].ToUpper().Equals("VRELLENO"))  
                                sql += "CREATE VIEW DBXSCHEMA." + vistas[i].ToString().Trim().ToUpper() + "_R AS SELECT * FROM DBXSCHEMA." + vistas[i].ToString().Trim().ToUpper() + ";";
                        }
                        else
                        {
                            for (int i = 0; i < vistas.Length; i++)
                                //if (!vistas[i].ToUpper().Equals("VRELLENO"))  
                                sql += "CREATE VIEW DBXSCHEMA." + vistas[i].ToString().Trim().ToUpper() + "_R AS SELECT * FROM DBXSCHEMA." + vistas[i].ToString().Trim().ToUpper() + " WHERE " + where + ";";
                        }
                    }
                    if (sql.Length == 0 || DBFunctions.NonQuery(sql) == -1)
                    {
                        Imprimir Impr = new Imprimir(NombreReporte);
                        //						string servidor=ConfigurationManager.AppSettings["Server"];
                        //						string database=ConfigurationManager.AppSettings["DataBase"];
                        //						string usuario=ConfigurationManager.AppSettings["UID"];
                        //						string password=ConfigurationManager.AppSettings["PWD"];
                        //						AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
                        //						miCripto.IV=ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
                        //						miCripto.Key=ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
                        //						string newPwd=miCripto.DescifrarCadena(password);

                        Impr.DtValPar = RetornarValoresParametros();

                        string usuario = ConfigurationManager.AppSettings["UID"];
                        //string password = ConfigurationManager.AppSettings["PWD"];
                        string password = ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()]; //para 2015
                        AMS.CriptoServiceProvider.Crypto miCripto = new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
                        miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
                        miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
                        string newPwd = miCripto.DescifrarCadena(password);

                        Impr.OdbcReport = reporteODBC;
                        Impr.PreviewReport2(NombreReporte, header, footer, copias, paginainicial, paginafinal, "", "", NombreReporte, rdbImpresion.SelectedValue.ToString(), usuario, newPwd, Formulas, ValFormulas, lb);

                        if (Impr.Exito)
                        {
                            Ver.NavigateUrl = Impr.Documento;
                            Ver.Visible = true;
                            miMail.RutaArchivo =  ConfigurationManager.AppSettings["PathToReports"] + NombreReporte + "_" + HttpContext.Current.User.Identity.Name.ToLower() + "." + rdbImpresion.SelectedValue.ToString();
                            miMail.NombreReporte = NombreReporte;
                            miMail.Visible = true;
                            //lbInfo.Visible = true;
                            //tbMail.Visible = true;
                            //imgBtnMail.Visible = true;
                            //rfv1.Visible = true;
                        }
                        else
                        {
                            lb.Text += "Error al generar reporte. Detalles <br>" + Impr.Mensajes;
                            miMail.Visible = false;
                            //lbInfo.Visible = false;
                            //tbMail.Visible = false;
                            //imgBtnMail.Visible = false;
                            //rfv1.Visible = false;
                        }

                        Impr.ReportUnload();
                    }
                    else
                        lb.Text += "Error al generar consultas : <br>" + DBFunctions.exceptions;
                }
                else
                    lb.Text += "Error al recrear vistas : <br>" + DBFunctions.exceptions;
            }
            catch (Exception ex)
            {
                lb.Text += "Error general del modulo. Detalles : <br>" + ex.InnerException.Message;
                lb.Text += "<br><br>" + ex.StackTrace;
            }
        }

        #endregion

        #region Métodos

        protected bool IsValidSchema()
        {
            bool retorno = true;
            bool reporteODBC = (Request.QueryString["CodRepG"] != null);
            ViewState["reporteODBC"] = reporteODBC;

            if(reporteODBC == false)
            {
                ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM SREPORTECRYSTAL WHERE SREP_CODIGO=" + Request.QueryString["CodRep"] + ";" +
                "SELECT SFIL_TEXTO,TTIP_FILTRO,SFIL_VISTA,SFIL_CONDICION,SFIL_CODIGOPADRE,SFIL_CODIGO,SFIL_VALORAMOSTRAR,SFIL_VALORCOMBO, SFIL_LUPA FROM SFILTROREPORTECRYSTAL WHERE SREP_CODIGO =" + Request.QueryString["CodRep"] + " ORDER BY SFIL_CODIGO");
            }
            else
            {
                ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM GREPORTECRYSTAL WHERE SREP_CODIGO=" + Request.QueryString["CodRep"] + ";" +
                "SELECT SFIL_TEXTO,TTIP_FILTRO,SFIL_VISTA,SFIL_CONDICION,SFIL_CODIGOPADRE,SFIL_CODIGO,SFIL_VALORAMOSTRAR,SFIL_VALORCOMBO, SFIL_LUPA FROM GFILTROREPORTECRYSTAL WHERE SREP_CODIGO =" + Request.QueryString["CodRep"] + " ORDER BY SFIL_CODIGO");
            }

            DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT * FROM VENCABEZADOEMPRESA");
            
            if (ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dr = ds1.Tables[0].Rows[0];
                    Empresa.Text = dr[1].ToString();
                    Nit.Text = dr[3].ToString();
                    dr = ds.Tables[0].Rows[0];
                    Titulo.Text = dr[6].ToString();
                    Subtitulo.Text = dr[7].ToString();
                    CodigoRep.Text = dr[5].ToString();
                    CodigoReporte = dr[0].ToString();
                    Modulo = dr[2].ToString();
                    NombreReporte = dr[3].ToString();
                    Vista = dr[4].ToString();
                }
                else
                {
                    retorno = false;
                }
            }
            else
            {
                retorno = false;
            }
            return retorno;
        }
        
        private string Establecer_Select(string id, params string[] padres)
        {
            string select = "";
            DataRow[] datos = ds.Tables[1].Select("SFIL_CODIGO=" + id + "");
            string where = "WHERE ";
            string reemplazo = "";
            bool valor = false, texto = false;
            if (datos.Length != 0)
            {
                string reemplazoArrobas = datos[0][3].ToString().Replace("@", "'");
                reemplazo = reemplazoArrobas;
                for (int i = 0; i < padres.Length; i++)
                {
                    reemplazo = reemplazo.Replace("#" + padres[i], Obtener_Valor(padres[i]));
                    if (i == padres.Length - 1)
                        where += reemplazo;
                }
                select += "SELECT ";
                if (datos[0][7].ToString() != "")
                {
                    select += datos[0][7].ToString() + ",";
                    valor = true;
                }
                if (datos[0][6].ToString() != "")
                {
                    select += datos[0][6].ToString();
                    texto = true;
                }
                if (!valor && !texto)
                    select += "* ";
                select += " FROM " + datos[0][2].ToString() + " " + where;
            }
            return select;
        }

        protected void SetForm()
        {
            string tipo = "";
            for (i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[1].Rows[i];
                tipo = dr[1].ToString();
                switch (tipo)
                {
                    case "S"://Si es string pongo un textbox
                        MapStringType(dr);
                        break;
                    case "C"://Si es un  combo pues pongo un combo jeje
                        MapComboType(dr);
                        break;
                    case "N"://Si es un numero pongo un textbox
                        MapIntType(dr);
                        break;
                    case "B"://Si es un booleano pongo un check box
                        MapBooleanType(dr);
                        break;
                    case "F"://Si es una fecha pongo el calendar acompañado de un textbox
                        MapDateType(dr);
                        break;
                }
            }
            if (ds.Tables[1].Rows.Count > 0)
                phForm1.Controls.Add(tbForm);
            //else
            //    phForm1.Visible = false;
        }

        protected void MapStringType(DataRow dr)
        {
            //Como es un string pongo el label con el texto a mostrar y un textbox
            RequiredFieldValidator rfv;
            Label lb = PutLabel(dr[0].ToString(), dr[5].ToString());
            TextBox tb = PutTextBox(90, dr[5].ToString());

            System.Web.UI.WebControls.Image lupa = PutImageLupa(dr[8].ToString(), tb);

            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();
            tc2.Controls.Add(tb);

            TableCell tc3 = new TableCell();
            tc3.Controls.Add(lupa);

            rfv = RequiredValidator(tb.ID, dr[0].ToString());
            tc2.Controls.Add(rfv);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tbForm.Rows.Add(tr);
        }

        protected System.Web.UI.WebControls.Image PutImageLupa(string tableName, TextBox tb)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            //img.ID = "img_" + i.ToString();
            if (tableName != "")
            {
                img.ImageUrl = images + "AMS.Search.png";
                img.Attributes.Add("onClick", "ModalDialog(_ctl1_" + tb.ClientID + ",\"SELECT * FROM " + tableName + ";\",new Array());");
                tb.Attributes.Add("onDblClick", "ModalDialog(_ctl1_" + tb.ClientID + ",\"SELECT * FROM " + tableName + ";\",new Array());");
                img.ToolTip = "Busqueda";
            }

            return img;
        }

        protected void MapComboType(DataRow dr)
        {
            //Como es un combo, le paso al combo la vista con la q se debe cargar
            Label lb = PutLabel(dr[0].ToString(), dr[5].ToString());
            DropDownList ddl = new DropDownList();
            //CheckBox chb=PutCheckBox(dr[5].ToString());
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();
            tc1.Controls.Add(lb);
            //Si no es hijo
            if (dr[4].ToString() == "")
            {
                ddl = PutDropDownList(dr[2].ToString() + " " + dr[3].ToString(), dr[5].ToString(), dr);
                //						vista y condicion					indice
                tc2.Controls.Add(ddl);
                //tc2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                //tc2.Controls.Add(chb);
            }
            //Si es hijo
            else
            {
                string[] padres = dr[4].ToString().Split(',');
                ddl = PutDropDownList(dr[2].ToString(), dr[3].ToString(), dr[5].ToString(), dr, padres);
                //						vista			condicion		indice			padres
                tc2.Controls.Add(ddl);
                //tc2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                //tc2.Controls.Add(chb);
            }
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbForm.Rows.Add(tr);
        }

        protected void MapIntType(DataRow dr)
        {
            Label lb = PutLabel(dr[0].ToString(), dr[5].ToString());
            TextBox tb = PutTextBox(8, dr[5].ToString());
            tb.MaxLength = 6;
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();
            RegularExpressionValidator rev = Validator(tb.ID, "[0-9]+", dr[0].ToString());
            tc1.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tc2.Controls.Add(rev);
            RequiredFieldValidator rfv = RequiredValidator(tb.ID, dr[0].ToString());
            tc2.Controls.Add(rfv);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tb.CssClass = "AlineacionDerecha";
            tbForm.Rows.Add(tr);
            ViewState["Datoscalendar"] = tb.Text;
            tb.Text = ViewState["Datoscalendar"].ToString();
        }

        protected void MapBooleanType(DataRow dr)
        {
            Label lb = PutLabel(dr[0].ToString(), dr[5].ToString());
            CheckBox chb = PutCheckBox(dr[5].ToString());
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();
            TableRow tr = new TableRow();
            tc1.Controls.Add(lb);
            tc2.Controls.Add(chb);
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbForm.Rows.Add(tr);
        }

        protected void MapDateType(DataRow dr)
        {

            Label lb = PutLabel(dr[0].ToString(), dr[5].ToString());
            TextBox tb = PutTextBox(10, dr[5].ToString());
            RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{4}-[0-9]{2}-[0-9]{2}", dr[0].ToString());
            tb.Attributes.Add("onkeyup", "DateMask(this)");
            tb.CssClass = "calendario"; 
            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();
            RequiredFieldValidator rfv = RequiredValidator(tb.ID, dr[0].ToString());
            tc2.Controls.Add(tb);
            tc2.Controls.Add(rfv);
            tc2.Controls.Add(rev);
            tc2.Controls.Add(this.PutImage(dr[5].ToString()));
            //tc2.Controls.Add(this.PutTableCalendar(dr[5].ToString()));
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbForm.Rows.Add(tr);



            //Label lb = PutLabel(dr[0].ToString(), dr[5].ToString());
            //TextBox tb = PutTextBox(10, dr[5].ToString());
            //RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{4}-[0-9]{2}-[0-9]{2}", dr[0].ToString());

            //CalendarExtender calendar = new CalendarExtender();
            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            //img.ID = "imgFilterControl_" + dr[0].ToString().Replace(" ","");
            //img.ImageUrl = images + "AMS.Calendar.png";
            //calendar.TargetControlID = tb.ID;
            //calendar.Format = "yyyy-MM-dd";
            //calendar.PopupButtonID = img.ID;
            //calendar.Animated = true;

            //tb.Attributes.Add("onkeyup", "DateMask(this)");
            //TableCell tc1 = new TableCell();

            //tc1.Controls.Add(lb);
            //TableCell tc2 = new TableCell();
            //tb.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //RequiredFieldValidator rfv = RequiredValidator(tb.ID, dr[0].ToString());

            //tc2.Controls.Add(tb);
            //tc2.Controls.Add(img);
            //tc2.Controls.Add(calendar);
            ////tc2.Controls.Add(rfv);
            ////tc2.Controls.Add(rev);

            //TableRow tr = new TableRow();
            //tr.Cells.Add(tc1);
            //tr.Cells.Add(tc2);

            //tbForm.Rows.Add(tr);
        }

        protected CheckBox PutCheckBox(string indice)
        {
            CheckBox chb = new CheckBox();
            chb.ID = "param_" + indice;
            chb.Checked = false;
            //chb.Text="Todos";
            return chb;
        }

        protected System.Web.UI.WebControls.Image PutImage(string indice)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ID = "img_" + indice;
            img.ImageUrl = images + "AMS.Icon.Calendar.png";
            img.Attributes.Add("onmouseover", "_ctl1_" + "tabcal_" + indice + ".style.display='inline'");
            img.Attributes.Add("onmouseout", "_ctl1_" + "tabcal_" + indice + ".style.display='none'");
            img.BorderWidth = 0;
            return img;
        }

        //protected Table PutTableCalendar(string indice)
        //{
        //    Table tbl = new Table();
        //    tbl.ID = "tabcal_" + indice;
        //    tbl.Attributes.Add("onmouseover", "_ctl1_" + "tabcal_" + indice + ".style.display='inline'");
        //    tbl.Attributes.Add("onmouseout", "_ctl1_" + "tabcal_" + indice + ".style.display='none'");
        //    tbl.Style.Add("DISPLAY", "none");
        //    tbl.Style.Add("WIDTH", "109");
        //    tbl.Style.Add("POSITION", "absolute");
        //    TableCell tc = new TableCell();
        //    TableRow tr = new TableRow();
        //    tc.Controls.Add(this.PutCalendar(indice));
        //    tr.Cells.Add(tc);
        //    tbl.Rows.Add(tr);
        //    return tbl;
        //}

        protected System.Web.UI.WebControls.Calendar PutCalendar(string indice)
        {
            System.Web.UI.WebControls.Calendar cln = new System.Web.UI.WebControls.Calendar();
            cln.ID = "calendarioFecha_" + indice;
            cln.SelectionChanged += new EventHandler(this.Cambiar_Fecha);
            return cln;
        }

        protected Label PutLabel(string text, string indice)
        {
            Label lb = new Label();
            lb.ID = "lb_" + indice;
            lb.Text = text + ": ";
            return lb;
        }

        protected TextBox PutTextBox(int l, string indice)
        {
            TextBox tb = new TextBox();
            tb.ID = "param_" + indice;
            //tb.CssClass = "calendario";

            if (l < 5)
                tb.Width = new Unit(25);
            if (l <= 10 && l >= 5)
                tb.Width = new Unit(l * 11);
            if (l <= 100 && l > 10)
                tb.Width = new Unit(l * 7);
            if (l > 100 && l < 5000)
            {
                tb.Width = new Unit(600);
                tb.Height = new Unit(60);
                tb.TextMode = TextBoxMode.MultiLine;
            }
            if (l >= 5000)
            {
                tb.Width = new Unit(400);
                tb.Height = new Unit(120);
                tb.TextMode = TextBoxMode.MultiLine;
            }
            return tb;
        }

        protected DropDownList PutDropDownList(string vista, string indice, DataRow dr)
        {
            DataSet tempDS = new DataSet();
            string hijos = "";
            DataRow[] valores = ds.Tables[1].Select("SFIL_CODIGO=" + indice + "");
            string select = "SELECT ";
            bool valor = false, texto = false;
            if (valores[0][7].ToString() != "")
            {
                select += valores[0][7].ToString() + ",";
                valor = true;
            }
            if (valores[0][6].ToString() != "")
            {
                select += valores[0][6].ToString();
                texto = true;
            }
            if (!valor && !texto)
                select += "* ";
            //select+=" FROM "+datos[0][2].ToString()+" "+where;
            tempDS = DBFunctions.Request(tempDS, IncludeSchema.NO, select + " FROM " + vista);
            DropDownList ddl = new DropDownList();
            ddl.ID = "param_" + indice;
            try
            {
                ddl.DataSource = tempDS.Tables[0];
                if (tempDS.Tables[0].Columns.Count == 1)
                {
                    ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
                    ddl.DataTextField = tempDS.Tables[0].Columns[0].ToString();
                }
                else if (tempDS.Tables[0].Columns.Count > 1)
                {
                    ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
                    ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
                }
                ddl.DataBind();
                hijos = EsPadre(indice);
                if (hijos.EndsWith(","))
                {
                    hijos = hijos.Substring(0, hijos.Length - 2);
                    string[] separarHijos = hijos.Split(',');
                    if (separarHijos.Length != 0)
                        ddl.AutoPostBack = true;
                    ddl.SelectedIndexChanged += new EventHandler(this.Cambiar_Combos);
                }
            }
            catch (Exception e)
            {
                lb.Text += e.ToString();
            }
            tempDS.Clear();
            return ddl;
        }

        protected DropDownList PutDropDownList(string vista, string condicion, string indice, DataRow dr, params string[] padres)
        {
            //Asi viene la condicion desde la BD
            //memp_codiempl=@#1@ and mquin_mesquin=#2
            DataSet tempDS = new DataSet();
            string where = "WHERE ";
            string reemplazo = "";
            string reemplazoArrobas = condicion.Replace("@", "'");
            reemplazo = reemplazoArrobas;
            DataRow[] valores = ds.Tables[1].Select("SFIL_CODIGO=" + indice + "");
            string select = "SELECT ";
            bool valor = false, texto = true;
            if (valores[0][7].ToString() != "")
            {
                select += valores[0][7].ToString() + ",";
                valor = true;
            }
            if (valores[0][6].ToString() != "")
            {
                select += valores[0][6].ToString();
                texto = true;
            }
            if (!valor && !texto)
                select += "* ";
            for (int i = 0; i < padres.Length; i++)
            {
                reemplazo = reemplazo.Replace("#" + padres[i], Obtener_Valor(padres[i]));
                if (i == padres.Length - 1)
                    where += reemplazo;
            }
            DropDownList ddl = new DropDownList();
            ddl.ID = "param_" + indice;
            //lb.Text+=where;
            tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, select + " FROM " + vista + " " + where);
            try
            {
                ddl.DataSource = tempDS.Tables[0];
                if (tempDS.Tables[0].Columns.Count > 1)
                {
                    ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
                    ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
                }
                else if (tempDS.Tables[0].Columns.Count == 1)
                {
                    ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
                    ddl.DataTextField = tempDS.Tables[0].Columns[0].ToString();
                }
                ddl.DataBind();
            }
            catch (Exception e)
            {
                lb.Text += e.ToString() + "<br>" + DBFunctions.exceptions;
            }
            tempDS.Clear();
            return ddl;
        }

        protected string EsPadre(string indice)
        {
            string hijos = "";
            DataSet sel = new DataSet();
            bool reporteODBC = Convert.ToBoolean(ViewState["reporteODBC"]);

            if(reporteODBC == false)
                DBFunctions.Request(sel, IncludeSchema.NO, "SELECT SFIL_CODIGO,SFIL_CODIGOPADRE FROM SFILTROREPORTECRYSTAL WHERE SREP_CODIGO =" + Request.QueryString["CodRep"] + "");
            else
                DBFunctions.Request(sel, IncludeSchema.NO, "SELECT SFIL_CODIGO,SFIL_CODIGOPADRE FROM GFILTROREPORTECRYSTAL WHERE SREP_CODIGO =" + Request.QueryString["CodRep"] + "");

            if (sel.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < sel.Tables[0].Rows.Count; i++)
                {
                    string[] temp = sel.Tables[0].Rows[i][1].ToString().Split(',');
                    if (temp.Length != 0)
                    {
                        for (int j = 0; j < temp.Length; j++)
                        {
                            if (indice == temp[j])
                                hijos += sel.Tables[0].Rows[i][0].ToString() + ",";
                        }
                    }
                }
            }
            return hijos;
        }

        protected string Obtener_Valor(string indPad)
        {
            string valor = "";
            for (int m = 0; m < tbForm.Rows.Count; m++)
            {
                //lbInfo.Text+="<br>"+tbl.Rows[m-1].Cells[1].Controls[0].ToString();
                if (tbForm.Rows[m].Cells[1].Controls[0].GetType() == typeof(DropDownList))
                {
                    if (tbForm.Rows[m].Cells[1].Controls[0].ID.Trim() == "param_" + indPad)
                    {
                        valor = ((DropDownList)tbForm.Rows[m].Cells[1].Controls[0]).SelectedValue;
                        break;
                    }
                }
                else if (tbForm.Rows[m].Cells[1].Controls[0].GetType() == typeof(TextBox))
                {
                    if (tbForm.Rows[m].Cells[1].Controls[0].ID.Trim() == "param_" + indPad)
                    {
                        valor = ((TextBox)tbForm.Rows[m].Cells[1].Controls[0]).Text;
                        break;
                    }
                }
                else if (tbForm.Rows[m].Cells[1].Controls[0].GetType() == typeof(CheckBox))
                {
                    if (tbForm.Rows[m].Cells[1].Controls[0].ID.Trim() == "param_" + indPad)
                    {
                        valor = ((CheckBox)tbForm.Rows[m].Cells[1].Controls[0]).Checked.ToString();
                        break;
                    }
                }
            }
            return valor;
        }

        protected void Llenar_Seleccion()
        {
            ArrayList llaves = new ArrayList();
            ArrayList valores = new ArrayList();
            try
            {
                for (int i = 0; i < tbForm.Rows.Count; i++)
                {
                    llaves.Add(((Label)tbForm.Rows[i].Cells[0].Controls[0]).Text);
                    string val = "";
                    val = Obtener_Valor((i + 1).ToString());
                    if (val == "True")
                        valores.Add("TODOS");
                    else
                        valores.Add(Obtener_Valor((i + 1).ToString()));
                }
                if (valores.Contains("TODOS"))
                {
                    int indice = valores.IndexOf("TODOS");
                    valores.RemoveAt(indice - 1);
                    llaves.RemoveAt(indice);
                }
                else
                {
                    int indice = valores.IndexOf("False");
                    if (indice != -1)
                    {
                        valores.RemoveAt(indice);
                        llaves.RemoveAt(indice);
                    }
                }
                for (int i = 0; i < llaves.Count; i++)
                    Seleccion1 += llaves[i].ToString() + " " + valores[i].ToString() + " ";
            }
            catch
            {
                Seleccion1 = "";
            }
        }

        protected RegularExpressionValidator Validator(string controlID, string regex, string textError)
        {
            RegularExpressionValidator rev = new RegularExpressionValidator();
            rev.ErrorMessage = textError + ". No Valido .";
            rev.ControlToValidate = controlID;
            rev.ValidationExpression = regex;
            rev.Display = ValidatorDisplay.None;
            return rev;
        }

        protected RequiredFieldValidator RequiredValidator(string controlID, string textError)
        {
            RequiredFieldValidator rfv = new RequiredFieldValidator();
            rfv.ErrorMessage = textError + ". Requerido.";
            rfv.ControlToValidate = controlID;
            rfv.Display = ValidatorDisplay.None;
            return rfv;
        }

        private void BR()
        {
            phForm1.Controls.Add(new LiteralControl("<br>"));
        }

        private DataTable ObtenerParametros()
        {
            Imprimir miImpresion = new Imprimir(ds.Tables[0].Rows[0][3].ToString());
            DataTable dt = miImpresion.RetornarInformacionParametros();
            miImpresion.ReportUnload();

            return dt;

        }

        protected void ConstruirControlesParametros()
        {
            DataTable dt = ObtenerParametros();
            Session["params"] = dt;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Si es un parametro boooelano
                if (dt.Rows[i][3].ToString() == "BooleanField")
                {
                    SetBooleanParameter(i.ToString(), dt.Rows[i][1].ToString());
                }
                //Si es un parametro de fecha
                else if (dt.Rows[i][3].ToString() == "DateField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                        SetDateRangeParameter(i.ToString(), dt.Rows[i][1].ToString());
                    else
                        SetDateParameter(i.ToString(), dt.Rows[i][1].ToString());
                }
                //Si es un parametro de fecha/hora
                else if (dt.Rows[i][3].ToString() == "DateTimeField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                        SetDateTimeRangeParameter(i.ToString(), dt.Rows[i][1].ToString());
                    else
                        SetDateTimeParameter(i.ToString(), dt.Rows[i][1].ToString());
                }
                //Si es un parametro numérico entero
                else if (dt.Rows[i][3].ToString() == "Int16sField" || dt.Rows[i][3].ToString() == "Int16uField" || dt.Rows[i][3].ToString() == "Int32sField" || dt.Rows[i][3].ToString() == "Int32uField" || dt.Rows[i][3].ToString() == "Int8sField" || dt.Rows[i][3].ToString() == "Int8uField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                        SetIntegerRangeParameter(i.ToString(), dt.Rows[i][1].ToString());
                    else
                        SetIntegerParameter(i.ToString(), dt.Rows[i][1].ToString());
                }
                //Si es un parametro numérico con decimales
                else if (dt.Rows[i][3].ToString() == "NumberField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                        SetDecimalRangeParameter(i.ToString(), dt.Rows[i][1].ToString());
                    else
                        SetDecimalParameter(i.ToString(), dt.Rows[i][1].ToString());
                }
                //Si es un parametro de texto
                else if (dt.Rows[i][3].ToString() == "StringField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                        SetStringRangeParameter(i.ToString(), dt.Rows[i][1].ToString());
                    else
                        SetStringParameter(i.ToString(), dt.Rows[i][1].ToString());
                }
                //Si es un parametro de hora
                else if (dt.Rows[i][3].ToString() == "TimeField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                        SetDateTimeRangeParameter(i.ToString(), dt.Rows[i][1].ToString());
                    else
                        SetTimeParameter(i.ToString(), dt.Rows[i][1].ToString());
                }
            }
            if (dt.Rows.Count > 0)
            {
                if (phForm1.Controls.Count == 0)
                {
                    phForm1.Controls.Clear();
                    phForm1.Controls.Add(tbParams);
                    phForm1.Visible = false;
                    plcParamAUX.Visible = true;
                }
            }

           phForm2.Controls.Add(tbParams);  //Estaba comentado
        }

        private Label BuildParameterLabel(string texto, string indice)
        {
            Label lb = new Label();
            lb.ID = "lbPar_" + indice;
            lb.Text = texto + " : ";
            return lb;
        }

        private Label[] BuildRangeParameterLabel(string texto, string indice)
        {
            Label[] labels = new Label[2];
            Label lb = new Label();
            Label lb1 = new Label();
            lb.ID = "lbRanPar_" + indice;
            lb1.ID = "lbRanPar0_" + indice;
            lb.Text = texto + " Desde : ";
            lb1.Text = " Hasta : ";
            labels[0] = lb;
            labels[1] = lb1;
            return labels;
        }

        private TextBox BuildParameterTextBox(string indice)
        {
            TextBox tb = new TextBox();
            tb.ID = "tbPar_" + indice;
            tb.Width = 100;
            return tb;
        }

        private TextBox[] BuildRangeParameterTextBox(string indice)
        {
            TextBox[] textboxs = new TextBox[2];
            TextBox tb = new TextBox();
            TextBox tb1 = new TextBox();
            tb.ID = "tbRanPar_" + indice;
            tb1.ID = "tbRanPar0_" + indice;
            tb.Width = 100;
            tb1.Width = 100;
            textboxs[0] = tb;
            textboxs[1] = tb1;
            return textboxs;
        }

        private CheckBox BuildParameterBoolean(string indice)
        {
            CheckBox cb = new CheckBox();
            cb.ID = "cb_" + i.ToString();
            cb.Checked = false;
            return cb;
        }

        private void SetBooleanParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = BuildParameterLabel(texto, indice);
            CheckBox cb = BuildParameterBoolean(indice);
            tc.Controls.Add(lb);
            tc2.Controls.Add(cb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tbParams.Rows.Add(tr);
        }

        private void SetDateParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = BuildParameterLabel(texto, indice);
            TextBox tb = BuildParameterTextBox(indice);
            tb.Attributes.Add("onKeyUp", "DateMask(this)");
            tb.Text = DateTime.Now.ToString("yyyy-MM-dd");
            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tbParams.Rows.Add(tr);
        }

        private void SetDateRangeParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = BuildRangeParameterLabel(texto, indice);
            TextBox[] textboxs = BuildRangeParameterTextBox(indice);
            for (int i = 0; i < textboxs.Length; i++)
            {
                textboxs[i].Attributes.Add("onKeyUp", "DateMask(this)");
                textboxs[i].Text = DateTime.Now.ToString("yyyy-MM-dd");                
            }
            tc.Controls.Add(labels[0]);
            tc2.Controls.Add(textboxs[0]);
            tc3.Controls.Add(labels[1]);
            tc4.Controls.Add(textboxs[1]);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);
            tbParams.Rows.Add(tr);
        }

        private void SetDateTimeParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = BuildParameterLabel(texto, indice);
            TextBox tb = BuildParameterTextBox(indice);
            tb.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tbParams.Rows.Add(tr);
        }

        private void SetDateTimeRangeParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = BuildRangeParameterLabel(texto, indice);
            TextBox[] textboxs = BuildRangeParameterTextBox(indice);
            for (int i = 0; i < textboxs.Length; i++)
                textboxs[i].Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            tc.Controls.Add(labels[0]);
            tc2.Controls.Add(textboxs[0]);
            tc3.Controls.Add(labels[1]);
            tc4.Controls.Add(textboxs[1]);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);
            tbParams.Rows.Add(tr);
        }

        private void SetIntegerParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = BuildParameterLabel(texto, indice);
            TextBox tb = BuildParameterTextBox(indice);
            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tbParams.Rows.Add(tr);
        }

        private void SetIntegerRangeParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = BuildRangeParameterLabel(texto, indice);
            TextBox[] textboxs = BuildRangeParameterTextBox(indice);
            tc.Controls.Add(labels[0]);
            tc2.Controls.Add(textboxs[0]);
            tc3.Controls.Add(labels[1]);
            tc4.Controls.Add(textboxs[1]);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);
            tbParams.Rows.Add(tr);
        }

        private void SetDecimalParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = BuildParameterLabel(texto, indice);
            TextBox tb = BuildParameterTextBox(indice);
            tb.Attributes.Add("onKeyUp", "NumericMaskE(this,event)");
            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tbParams.Rows.Add(tr);
        }

        private void SetDecimalRangeParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = BuildRangeParameterLabel(texto, indice);
            TextBox[] textboxs = BuildRangeParameterTextBox(indice);
            for (int i = 0; i < textboxs.Length; i++)
                textboxs[i].Attributes.Add("onKeyUp", "NumericMaskE(this,event)");
            tc.Controls.Add(labels[0]);
            tc2.Controls.Add(textboxs[0]);
            tc3.Controls.Add(labels[1]);
            tc4.Controls.Add(textboxs[1]);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);
            tbParams.Rows.Add(tr);
        }

        private void SetStringParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = BuildParameterLabel(texto, indice);
            TextBox tb = BuildParameterTextBox(indice);
            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tbParams.Rows.Add(tr);
        }

        private void SetStringRangeParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = BuildRangeParameterLabel(texto, indice);
            TextBox[] textboxs = BuildRangeParameterTextBox(indice);
            tc.Controls.Add(labels[0]);
            tc2.Controls.Add(textboxs[0]);
            tc3.Controls.Add(labels[1]);
            tc4.Controls.Add(textboxs[1]);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);
            tbParams.Rows.Add(tr);
        }

        private void SetTimeParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = BuildParameterLabel(texto, indice);
            TextBox tb = BuildParameterTextBox(indice);
            tb.Text = DateTime.Now.ToString("HH:mm:ss");
            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tbParams.Rows.Add(tr);
        }

        private void SetTimeRangeParameter(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = BuildRangeParameterLabel(texto, indice);
            TextBox[] textboxs = BuildRangeParameterTextBox(indice);
            for (int i = 0; i < textboxs.Length; i++)
                textboxs[i].Text = DateTime.Now.ToString("HH:mm:ss");
            tc.Controls.Add(labels[0]);
            tc2.Controls.Add(textboxs[0]);
            tc3.Controls.Add(labels[1]);
            tc4.Controls.Add(textboxs[1]);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);
            tbParams.Rows.Add(tr);
        }

        private DataTable RetornarValoresParametros()
        {
            //DataTable con la información de los parametros del reporte
            DataTable dt = (DataTable)Session["params"];
            //DataTable con los valores que van a tener los parametros
            DataTable dtPars = new DataTable();
            dtPars.Columns.Add("NOMBRE", typeof(string));
            dtPars.Columns.Add("VALOR", typeof(object));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Si es un parametro boooelano
                if (dt.Rows[i][3].ToString() == "BooleanField")
                {
                    dtPars.Rows.Add(GetBooleanParameter(i, dt, dtPars));
                }
                //Si es un parametro de fecha
                else if (dt.Rows[i][3].ToString() == "DateField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                    {
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[0]);
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[1]);
                    }
                    else
                        dtPars.Rows.Add(GetParameter(i, dt, dtPars));
                }
                //Si es un parametro de fecha/hora
                else if (dt.Rows[i][3].ToString() == "DateTimeField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                    {
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[0]);
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[1]);
                    }
                    else
                        dtPars.Rows.Add(GetParameter(i, dt, dtPars));
                }
                //Si es un parametro numérico entero
                else if (dt.Rows[i][3].ToString() == "Int16sField" || dt.Rows[i][3].ToString() == "Int16uField" || dt.Rows[i][3].ToString() == "Int32sField" || dt.Rows[i][3].ToString() == "Int32uField" || dt.Rows[i][3].ToString() == "Int8sField" || dt.Rows[i][3].ToString() == "Int8uField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                    {
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[0]);
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[1]);
                    }
                    else
                        dtPars.Rows.Add(GetParameter(i, dt, dtPars));
                }
                //Si es un parametro numérico con decimales
                else if (dt.Rows[i][3].ToString() == "NumberField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                    {
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[0]);
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[1]);
                    }
                    else
                        dtPars.Rows.Add(GetParameter(i, dt, dtPars));
                }
                //Si es un parametro de texto
                else if (dt.Rows[i][3].ToString() == "StringField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                    {
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[0]);
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[1]);
                    }
                    else
                        dtPars.Rows.Add(GetParameter(i, dt, dtPars));
                }
                //Si es un parametro de hora
                else if (dt.Rows[i][3].ToString() == "TimeField")
                {
                    if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                    {
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[0]);
                        dtPars.Rows.Add((GetRangeParameter(i, dt, dtPars))[1]);
                    }
                    else
                        dtPars.Rows.Add(GetParameter(i, dt, dtPars));
                }
            }
            return dtPars;
        }

        private DataRow GetBooleanParameter(int indice, DataTable dt, DataTable dtPars)
        {
            DataRow fila;
            CheckBox cb = new CheckBox();
            fila = dtPars.NewRow();
            fila[0] = dt.Rows[indice][0].ToString();
            cb = ((CheckBox)tbParams.FindControl("cb_" + indice.ToString()));
            if (cb.Checked)
                fila[1] = true;
            else
                fila[1] = false;
            return fila;
        }

        private DataRow GetParameter(int indice, DataTable dt, DataTable dtPars)
        {
            DataRow fila = dtPars.NewRow();
            fila[0] = dt.Rows[indice][0].ToString();
            try
            {
                if (tbForm.Rows[indice].Cells[1].Controls[0].GetType() == typeof(TextBox))
                {
                    ((TextBox)tbParams.FindControl("tbPar_" + indice.ToString())).Text = ((TextBox)tbForm.Rows[indice].Cells[1].Controls[0]).Text;
                }
                else if (tbForm.Rows[indice].Cells[1].Controls[0].GetType() == typeof(DropDownList))
                {
                    //((TextBox)tbParams.FindControl("tbPar_" + indice.ToString())).Text = ((DropDownList)tbForm.Rows[indice].Cells[1].Controls[0]).SelectedValue;
                    ((TextBox)tbParams.Rows[indice].Cells[1].Controls[0]).Text = ((DropDownList)tbForm.Rows[indice].Cells[1].Controls[0]).SelectedValue; ;
                }
            }
            catch (Exception err)
            { }
            //fila[1]=((TextBox)tbParams.FindControl("tbPar_"+indice.ToString())).Text;
            fila[1] = ((TextBox)tbParams.Rows[indice].Cells[1].Controls[0]).Text;

            string val = Obtener_Valor("0");
            return fila;
        }

        private DataRow[] GetRangeParameter(int indice, DataTable dt, DataTable dtPars)
        {
            DataRow[] filas = new DataRow[2];
            for (int i = 0; i < filas.Length; i++)
            {
                DataRow fila = dtPars.NewRow();
                if (i == 0)
                {
                    fila[0] = dt.Rows[indice][0].ToString();
                    try
                    {
                        ((TextBox)tbParams.FindControl("tbRanPar_" + indice.ToString())).Text = ((TextBox)tbForm.Rows[indice * 2].Cells[1].Controls[0]).Text;
                    }
                    catch (Exception err)
                    { }
                    fila[1] = ((TextBox)tbParams.FindControl("tbRanPar_" + indice.ToString())).Text;
                    filas[i] = fila;
                }
                else
                {
                    fila[0] = dt.Rows[indice][0].ToString();
                    try
                    {
                        ((TextBox)tbParams.FindControl("tbRanPar0_" + indice.ToString())).Text = ((TextBox)tbForm.Rows[(indice * 2) + 1].Cells[1].Controls[0]).Text;
                    }
                    catch (Exception err)
                    { }
                    fila[1] = ((TextBox)tbParams.FindControl("tbRanPar0_" + indice.ToString())).Text;
                    filas[i] = fila;
                }
            }
            return filas;
        }

        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

        }
        #endregion

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Mail miMail = new Mail();
            AMS.CriptoServiceProvider.Crypto miCripto = new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string password = ConfigurationManager.AppSettings["PasswordEMail"];
            try
            {
                //string newPwd=miCripto.DescifrarCadena(password);
                string newPwd = password;
                string rutaArchivo = ConfigurationManager.AppSettings["PathToReports"] + NombreReporte + "_" + HttpContext.Current.User.Identity.Name.ToLower() + "." + rdbImpresion.SelectedValue.ToString();

                string mensaje =
                    @"<div style='position: absolute; background-color:#EEEFD9;width: 35%;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888;'>
	                	<img style='width: 20%; position: absolute; right: 2%;' src='http://ams.ecas.co/sac/img/SAC.logoReport.sac.png' /><br><br>
		                <b><font size='5'>Reporte Generado:</font></b>
		                <br>" + NombreReporte +
                       @"<br><br>
		                <b>Reciba un cordial saludo</b>, <br>
		                Ha recibido un Reporte usando el Sistema A.M.S. <br>
                        Dicho reporte se encuentra disponible como archivo <br>
                        adjunto en este correo.
		                <br><br>
	                    <b>Gracias por su atención.</b>
		                <br>
		                <i>eCAS-AMS.</i>
	                </div>
                    <br><br>";

                //Tools.Utils.EnviarMail(tbMail.Text, "Ha Recibido un Reporte: " + NombreReporte, mensaje, TipoCorreo.HTML, rutaArchivo);
                //Utils.MostrarAlerta(Response, "Email con Reporte ha sido enviado correctamente a: " + tbMail.Text);
            }
            catch (Exception er)
            {
                lb.Text = "El Servidor de correos no ha sido configurado. Contactar Administrador de sistemas.";
            }
        }
    }
}
