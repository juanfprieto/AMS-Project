using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.DB;
using System.Data;
using System.Data.Odbc;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Collections;
using AMS.Forms;
using AMS.Documentos;
using AMS.CriptoServiceProvider;
//using AjaxControlToolkit;

namespace AMS.Reportes
{
    public class FabricaRepProductorCrystal : ReporteFiltros
    {
        private string codigoReporte;
        private DataSet dsFiltrosReporte;
        private Table tablaFiltros ;
        private Table tablaFiltrosCrystal;

        protected string images = ConfigurationManager.AppSettings["PathToImages"];
        public PlaceHolder phForm;

        public Table TablaFiltros()
        {
            return tablaFiltros;
        }

        public Table TablaFiltrosCrystal()
        {
            return tablaFiltrosCrystal;
        }

        public DataSet DsFiltrosReporte()
        {
            return dsFiltrosReporte;
        }

        public FabricaRepProductorCrystal(string codReporte, PlaceHolder phFormulario)
        {
            codigoReporte = codReporte;
            dsFiltrosReporte = new DataSet();
            tablaFiltros  = new Table();
            tablaFiltrosCrystal = new Table();
            phForm = phFormulario;
        }

        public void CargarFiltros()
        {
            DBFunctions.Request(dsFiltrosReporte, IncludeSchema.NO, 
                "SELECT SFIL_TEXTO,TTIP_FILTRO,SFIL_VISTA,SFIL_CONDICION,SFIL_CODIGOPADRE,SFIL_CODIGO,SFIL_VALORAMOSTRAR,SFIL_VALORCOMBO " +
                "FROM SFILTROREPORTECRYSTAL WHERE SREP_CODIGO =" + codigoReporte + " ORDER BY SFIL_CODIGO");

            if (dsFiltrosReporte.Tables[0].Rows.Count != 0)
            {
                tablaFiltros = ObtenerFiltrosSqlReporte();
            }

            string nomReporte = DBFunctions.SingleData("SELECT srep_nombrerpt FROM SREPORTECRYSTAL WHERE srep_codigo=" + codigoReporte);
            Imprimir miImpresion = new Imprimir(nomReporte);
            DataTable dtParametros = miImpresion.RetornarInformacionParametros();
            miImpresion.ReportUnload();

            dsFiltrosReporte.Tables.Add(dtParametros);

            if (dtParametros.Rows.Count != 0)
            {
                tablaFiltrosCrystal = ObtenerFiltrosCrystalReporte(dtParametros);
            }
            
        }

        protected Table ObtenerFiltrosSqlReporte()
        {
            Table tablaFiltrosSQL = new Table();
            string tipo = "";

            for (int i = 0; i < dsFiltrosReporte.Tables[0].Rows.Count; i++)
            {
                TableRow trow = new TableRow();
                DataRow dr = dsFiltrosReporte.Tables[0].Rows[i];
                tipo = dr[1].ToString();
                
                switch (tipo)
                {
                    case "S"://Si es string pongo un textbox
                        trow = MapStringType(dr[5].ToString(), dr[0].ToString());
                        break;
                    case "F"://Si es una fecha pongo el calendar acompañado de un textbox
                        trow = MapDateType(dr[5].ToString(), dr[0].ToString());
                        break;
                    case "B"://Si es un booleano pongo un check box
                        trow = MapBooleanType(dr[5].ToString(), dr[0].ToString());
                        break;
                    case "N"://Si es un numero pongo un textbox
                        trow = MapIntegerType(dr[5].ToString(), dr[0].ToString());
                        break;
                    case "C"://Si es un  combo pues pongo un combo jeje
                        trow = MapComboType(dr);
                        break;
                }

                tablaFiltrosSQL.Rows.Add(trow);
            }

            return tablaFiltrosSQL;
        }

        protected Table ObtenerFiltrosCrystalReporte(DataTable dtParams)
        {
            Table tablaFiltrosCrys = new Table();
            DataTable dt = dtParams;
            string tipo = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TableRow trowCry = new TableRow();
                tipo = dt.Rows[i][3].ToString();

                switch (tipo)
                {
                    case "StringField":  //Si es parametro texto pongo un textbox
                        if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                            trowCry = MapStringRangeType(i.ToString(), dt.Rows[i][1].ToString());
                        else
                            trowCry = MapStringType(i.ToString(), dt.Rows[i][1].ToString());
                        break;

                    case "DateField":  //Si es un parametro de fecha
                        if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                            trowCry = MapDatesRangeType(i.ToString(), dt.Rows[i][1].ToString());
                        else
                            trowCry = MapDateType(i.ToString(), dt.Rows[i][1].ToString());
                        break;

                    case "BooleanField":  //Si es un parametro boooelano
                        trowCry = MapBooleanType(i.ToString(), dt.Rows[i][1].ToString());
                        break;

                    case "NumberField":  //Si es un parametro numérico con decimales
                        if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                            trowCry = MapDecimalRangeType(i.ToString(), dt.Rows[i][1].ToString());
                        else
                            trowCry = MapDecimalType(i.ToString(), dt.Rows[i][1].ToString());
                        break;

                    case "Int8sField": //Si es un parametro numérico entero
                    case "Int8uField":
                    case "Int16sField":  
                    case "Int16uField":
                    case "Int32sField":
                    case "Int32uField":
                        if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                            trowCry = MapIntegerRangeType(i.ToString(), dt.Rows[i][1].ToString());
                        else
                            trowCry = MapIntegerType(i.ToString(), dt.Rows[i][1].ToString());
                        break;

                    case "DateTimeField":  //Si es un parametro de fecha/hora
                        if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                            trowCry = MapDateTimeRangeType(i.ToString(), dt.Rows[i][1].ToString());
                        else
                            trowCry = MapDateTimeType(i.ToString(), dt.Rows[i][1].ToString());
                        break;

                    case "TimeField": //Si es un parametro de hora
                        if (dt.Rows[i][4].ToString() == "DiscreteAndRangeValue" || dt.Rows[i][4].ToString() == "RangeValue")
                            trowCry = MapTimeRangeType(i.ToString(), dt.Rows[i][1].ToString());
                        else
                            trowCry = MapTimeType(i.ToString(), dt.Rows[i][1].ToString());
                        break;
                }

                tablaFiltrosCrys.Rows.Add(trowCry);
            }

            return tablaFiltrosCrys;
        }


        //---------------------Metodos de mapeo de los controles---------------------------------- //

        protected TableRow MapStringType(string indice, string texto)
        {
            //Como es un string pongo el label con el texto a mostrar y un textbox PutLabel( dr[5].ToString(indice), dr[0].ToString(texto));
            TableRow tr = new TableRow();
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();

            RequiredFieldValidator rfv;
            Label lb = PutLabel( indice, texto);
            TextBox tb = PutTextBox(2, indice);
            
            tc1.Controls.Add(lb);
            tc2.Controls.Add(tb);

            rfv = RequiredValidator(tb.ID, texto);
            tc2.Controls.Add(rfv);
            
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);

            return tr;
        }

        private TableRow MapStringRangeType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = PutLabelsRange(indice, texto);
            TextBox[] textboxs = PutTextBoxesRange(indice);

            tc.Controls.Add(labels[0]);
            tc2.Controls.Add(textboxs[0]);
            tc3.Controls.Add(labels[1]);
            tc4.Controls.Add(textboxs[1]);

            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);

            return tr;
        }

        private TableRow MapDecimalType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = PutLabel(indice, texto);
            TextBox tb = PutTextBox(2, indice);

            tb.Attributes.Add("onKeyUp", "NumericMaskE(this,event)");
            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);

            return tr;
        }

        private TableRow MapDecimalRangeType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = PutLabelsRange(indice, texto);
            TextBox[] textboxs = PutTextBoxesRange(indice);
            
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

            return tr;
        }

        private TableRow MapIntegerType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = PutLabel(indice, texto);
            TextBox tb = PutTextBox(2, indice);

            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            
            return tr;
        }

        private TableRow MapIntegerRangeType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = PutLabelsRange(indice, texto);
            TextBox[] textboxs = PutTextBoxesRange(indice);

            tc.Controls.Add(labels[0]);
            tc2.Controls.Add(textboxs[0]);
            tc3.Controls.Add(labels[1]);
            tc4.Controls.Add(textboxs[1]);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);

            return tr;
        }

        protected TableRow MapDateType(string indice, string texto)
        {
            //Label lb = PutLabel(indice, texto);
            //TextBox tb = PutTextBox(1, indice);
            //RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{4}-[0-9]{2}-[0-9]{2}", texto);

            //CalendarExtender calendar = new CalendarExtender();
            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            //img.ID = "imgFilterControl_" + indice;
            //img.ImageUrl = images + "AMS.Calendar.png";
            //calendar.TargetControlID = tb.ID;
            //calendar.Format = "yyyy-MM-dd";
            //calendar.PopupButtonID = img.ID;

            //tb.Attributes.Add("onkeyup", "DateMask(this)");
            //TableCell tc1 = new TableCell();

            //tc1.Controls.Add(lb);
            //TableCell tc2 = new TableCell();
            //tb.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //RequiredFieldValidator rfv = RequiredValidator(tb.ID, texto);

            //tc2.Controls.Add(tb);
            //tc2.Controls.Add(img);
            //tc2.Controls.Add(calendar);
            //tc2.Controls.Add(rfv);
            //tc2.Controls.Add(rev);

            TableRow tr = new TableRow();

            //tr.Cells.Add(tc1);
            //tr.Cells.Add(tc2);
            
            return tr;
        }

        private TableRow MapDatesRangeType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();

            Label[] labels = PutLabelsRange(indice, texto);
            TextBox[] textboxs = PutTextBoxesRange(indice);

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

            return tr;
        }

        protected TableRow MapBooleanType(string indice, string texto)
        {
            Label lb = PutLabel(indice, texto);
            CheckBox chb = PutCheckBox(indice);
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();
            TableRow tr = new TableRow();

            tc1.Controls.Add(lb);
            tc2.Controls.Add(chb);
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);

            return tr;
        }

        private TableRow MapDateTimeType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = PutLabel(indice, texto);
            TextBox tb = PutTextBox(1, indice);

            tb.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            
            return tr;
        }

        private TableRow MapDateTimeRangeType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = PutLabelsRange(indice, texto);
            TextBox[] textboxs = PutTextBoxesRange(indice);

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

            return tr;
        }

        private TableRow MapTimeType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            Label lb = PutLabel(indice, texto);
            TextBox tb = PutTextBox(1, indice);
            tb.Text = DateTime.Now.ToString("HH:mm:ss");

            tc.Controls.Add(lb);
            tc2.Controls.Add(tb);
            tr.Cells.Add(tc);
            tr.Cells.Add(tc2);
            
            return tr;
        }

        private TableRow MapTimeRangeType(string indice, string texto)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            Label[] labels = PutLabelsRange(indice, texto);
            TextBox[] textboxs = PutTextBoxesRange(indice);

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

            return tr;
        }

        protected TableRow MapComboType(DataRow dr)
        {
            //Como es un combo, le paso al combo la vista con la q se debe cargar
            Label lb = PutLabel(dr[0].ToString(), dr[5].ToString());
            DropDownList ddl = new DropDownList();
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();
            tc1.Controls.Add(lb);

            if (dr[4].ToString() == "")  //Si no es hijo
            {
                //					 (	     vista       -         condicion   ,      indice     , dr)
                ddl = PutDropDownList(dr[2].ToString() + " " + dr[3].ToString(), dr[5].ToString(), dr);
                tc2.Controls.Add(ddl);
            }
            else  //Si es hijo
            {
                string[] padres = dr[4].ToString().Split(',');
                //					 (	     vista	  ,     condicion	,	  indice	  ,	dr, padres)
                ddl = PutDropDownList(dr[2].ToString(), dr[3].ToString(), dr[5].ToString(), dr, padres);
                tc2.Controls.Add(ddl);
            }
            
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);

            return tr;
        }

        

        //--------------------------------------Metodos de creación de Controles ------------------------------------//

        protected Label PutLabel(string indice, string texto)
        {
            Label lb = new Label();
            lb.ID = "lb_" + indice;
            lb.Text = texto + ": ";
            return lb;
        }

        private Label[] PutLabelsRange(string indice, string texto)
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

        protected TextBox PutTextBox(int largo, string indice)
        {
            TextBox tb = new TextBox();
            tb.ID = "param_" + indice;

            switch (largo)
            {
                case 1:  // Tamaño pequeño
                    tb.Width = new Unit(100);
                    break;
                case 2:  // Tamaño mediano
                    tb.Width = new Unit(250);
                    break;
                case 3:  // Tamaño grande
                    tb.Width = new Unit(400);
                    break;
                case 4:  // Tamaño mediano multiline
                    tb.Width = new Unit(400);
                    tb.Height = new Unit(60);
                    tb.TextMode = TextBoxMode.MultiLine;
                    break;
            }

            return tb;
        }

        private TextBox[] PutTextBoxesRange(string indice)
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

        protected CheckBox PutCheckBox(string indice)
        {
            CheckBox chb = new CheckBox();
            chb.ID = "param_" + indice;
            chb.Checked = false;
            return chb;
        }

        protected DropDownList PutDropDownList(string vista, string indice, DataRow dr)
        {
            DataSet tempDS = new DataSet();
            string hijos = "";
            DataRow[] valores = dsFiltrosReporte.Tables[0].Select("SFIL_CODIGO=" + indice + "");
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
                    ddl.SelectedIndexChanged += new EventHandler(Cambiar_Combos);
                }
            }
            catch (Exception e)
            {}

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
            DataRow[] valores = dsFiltrosReporte.Tables[0].Select("SFIL_CODIGO=" + indice + "");
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
            {}

            tempDS.Clear();
            return ddl;
        }

        protected System.Web.UI.WebControls.Image PutImage(string indice)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ID = "img_" + indice;
            img.ImageUrl = images + "AMS.Icon.Calendar.gif";
            img.Attributes.Add("onmouseover", "_ctl1_" + "tabcal_" + indice + ".style.display='inline'");
            img.Attributes.Add("onmouseout", "_ctl1_" + "tabcal_" + indice + ".style.display='none'");
            img.BorderWidth = 0;
            return img;
        }

        protected Table PutTableCalendar(string indice)
        {
            Table tbl = new Table();
            tbl.ID = "tabcal_" + indice;
            tbl.Attributes.Add("onmouseover", "_ctl1_" + "tabcal_" + indice + ".style.display='inline'");
            tbl.Attributes.Add("onmouseout", "_ctl1_" + "tabcal_" + indice + ".style.display='none'");
            tbl.Style.Add("DISPLAY", "none");
            tbl.Style.Add("WIDTH", "109");
            tbl.Style.Add("POSITION", "absolute");
            TableCell tc = new TableCell();
            TableRow tr = new TableRow();
            tc.Controls.Add(this.PutCalendar(indice));
            tr.Cells.Add(tc);
            tbl.Rows.Add(tr);
            return tbl;
        }

        protected System.Web.UI.WebControls.Calendar PutCalendar(string indice)
        {
            System.Web.UI.WebControls.Calendar cln = new System.Web.UI.WebControls.Calendar();
            cln.ID = "calendarioFecha_" + indice;
            cln.SelectionChanged += new EventHandler(this.Cambiar_Fecha);
            return cln;
        }

        
        //----------------Metodos de validación dinamico para los controles creados --------------------------//

        protected RequiredFieldValidator RequiredValidator(string controlID, string textError)
        {
            RequiredFieldValidator rfv = new RequiredFieldValidator();
            rfv.ErrorMessage = textError + ". Requerido.";
            rfv.ControlToValidate = controlID;
            rfv.Display = ValidatorDisplay.None;
            return rfv;
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


        // ---------------------- General   ------------------------------ //

        protected void Cambiar_Fecha(object Sender, EventArgs e)
        {
            string ind = ((System.Web.UI.WebControls.Calendar)Sender).ID;
            string[] partes = ind.Split('_');
            ((TextBox)phForm.FindControl("param_" + partes[partes.Length - 1])).Text = ((System.Web.UI.WebControls.Calendar)Sender).SelectedDate.ToString("yyyy-MM-dd");
        }

        protected string EsPadre(string indice)
        {
            string hijos = "";
            DataSet sel = new DataSet();
            DBFunctions.Request(sel, IncludeSchema.NO, "SELECT SFIL_CODIGO,SFIL_CODIGOPADRE FROM SFILTROREPORTECRYSTAL WHERE SREP_CODIGO =" + codigoReporte + "");
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

        protected void Cambiar_Combos(object Sender, EventArgs e)
        {
            string[] id = ((DropDownList)Sender).ID.Split('_');
            DataSet tempDS = new DataSet();
            DataRow[] tienenPadre = dsFiltrosReporte.Tables[0].Select("SFIL_CODIGOPADRE <> ''");
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
                for (int i = 0; i < hijos.Count; i++)
                {
                    DatasToControls bind = new DatasToControls();
                    string p = DBFunctions.SingleData("SELECT SFIL_CODIGOPADRE FROM SFILTROREPORTECRYSTAL WHERE SREP_CODIGO=" + codigoReporte + " AND SFIL_CODIGO=" + hijos[i].ToString() + "");
                    bind.PutDatasIntoDropDownList(((DropDownList)phForm.FindControl("param_" + hijos[i].ToString())), Establecer_Select(hijos[i].ToString(), p.Split(',')));
                }
            }
        }

        private string Establecer_Select(string id, params string[] padres)
        {
            string select = "";
            DataRow[] datos = dsFiltrosReporte.Tables[0].Select("SFIL_CODIGO=" + id + "");
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

        protected string Obtener_Valor(string indPad)
        {
            string valor = "";
            for (int m = 0; m < tablaFiltros.Rows.Count; m++)
            {
                //lbInfo.Text+="<br>"+tbl.Rows[m-1].Cells[1].Controls[0].ToString();
                if (tablaFiltros.Rows[m].Cells[1].Controls[0].GetType() == typeof(DropDownList))
                {
                    if (tablaFiltros.Rows[m].Cells[1].Controls[0].ID.Trim() == "param_" + indPad)
                    {
                        valor = ((DropDownList)tablaFiltros.Rows[m].Cells[1].Controls[0]).SelectedValue;
                        break;
                    }
                }
                else if (tablaFiltros.Rows[m].Cells[1].Controls[0].GetType() == typeof(TextBox))
                {
                    if (tablaFiltros.Rows[m].Cells[1].Controls[0].ID.Trim() == "param_" + indPad)
                    {
                        valor = ((TextBox)tablaFiltros.Rows[m].Cells[1].Controls[0]).Text;
                        break;
                    }
                }
                else if (tablaFiltros.Rows[m].Cells[1].Controls[0].GetType() == typeof(CheckBox))
                {
                    if (tablaFiltros.Rows[m].Cells[1].Controls[0].ID.Trim() == "param_" + indPad)
                    {
                        valor = ((CheckBox)tablaFiltros.Rows[m].Cells[1].Controls[0]).Checked.ToString();
                        break;
                    }
                }
            }
            return valor;
        }

        

    }
}