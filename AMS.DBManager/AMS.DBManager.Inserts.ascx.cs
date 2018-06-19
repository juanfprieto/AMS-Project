// created on 20/02/2004 at 12:48
using System;
using System.Linq;
using System.Linq.Expressions;
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
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using System.Text.RegularExpressions;
using AMS.Tools;
using Ajax;


namespace AMS.DBManager
{

    public partial class Inserts : System.Web.UI.UserControl
    {
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected int i, j, fkCount = 0;
        protected string table;
        protected string fksComps = "", ddlSec = "";
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string sels = "", filtroSels = "";
        protected string images = ConfigurationManager.AppSettings["PathToImages"];
        protected string uploads = ConfigurationManager.AppSettings["PathToUploads"];
        protected Table tbForm = new Table();
        protected DataSet ds = new DataSet();
        protected DataSet ds1 = new DataSet();
        protected TextBox tbRefItem = new TextBox();
        protected Button pruebabtn = new Button();
        protected DropDownList ddlRefItem = new DropDownList();
        protected Label lbBusqueda = new Label();
        protected System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        protected System.Web.UI.WebControls.Image imghlp = new System.Web.UI.WebControls.Image();
        protected ArrayList arlFkNombres, arlFkValores, arlFkINombres, arlFkIValores;
        public string tabla2 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.DBManager.Inserts));
            
            arlFkNombres = new ArrayList();
            arlFkValores = new ArrayList();
            arlFkINombres = new ArrayList();
            arlFkIValores = new ArrayList();

            table = Request.QueryString["table"];

            GetDependencies();
            if (Request.QueryString["table"] != null && Request.QueryString["action"] != null && !IsPostBack)
            {
                //if(Request.QueryString["imp"]=="Y")
                //	Response.Write("<script language='javascript' src='../js/AMS.Web.DialogBox.js' type='text/javascript'></script><script language:javascript>DialogBox('Desea Imprimir?','DBManager.Preview&table="+table+"&pks="+Request.QueryString["pksImp"]+"');</script>");
                tbForm.BackColor = Color.FromArgb(183, 183, 226);
                tbForm.Font.Name = "tahoma";
                tbForm.ID = "tbdata";

                Button pruebabtn = new Button();
                pruebabtn.Text = "prueba";
                //tbForm.Controls.Add(pruebabtn);




                GetSchema();
                SetForm();

                //Manejo de ModalDialog inserts.
               if (Request.QueryString["modal"] != null && Request.QueryString["tableRef"] == null)
                {
                    if (Request.QueryString["fks"] == null)
                    {
                        btnRegresarModal.Visible = true;
                        lnkVerTabla.Visible = false;
                    }
                    else
                    {
                        btnRegresarModal.Visible = false;
                        lnkVerTabla.Visible = false;
                    }
                }
                //else
                //{
                //    btnRegresarModal.Visible = false;
                //}


                if (Request.QueryString["reg"] == "2")
                    Utils.MostrarAlerta(Response, "Registro Creado Satisfactoriamente..!!");  

                if (Request.QueryString["fks"] != null)
                {
                    btVolver.Visible = false;
                    lnkVerTablaPadre.Visible = true;
                    lnkVerTablaPadre.Text = "Ver " + Request.QueryString["pathRef"];
                    if (Request.QueryString["action"] == "insert")
                        CheckExists();
                }
                if (Request.QueryString["vlr"] != null)
                {
                    if (Request.QueryString["action"] == "insert")
                        ValorObjeto();
                }
                if (Request.QueryString["action"] == "update")
                {
                    PopulateForm();
                    btInsert.Visible = false;
                    ShowDependencies();
                }
                else
                    if (Request.QueryString["action"] == "copiar")
                {
                    PopulateFormCopiar();
                    btInsert.Visible = true;
                    btUpdate.Visible = false;
                }
                else
                {
                    PopulateFormNew();
                    btInsert.Visible = true;
                    btUpdate.Visible = false;
                }
                if (Request.QueryString["msg"] != null)
                    lbInfo.Text = Request.QueryString["msg"];
            }
            else
            {
                GetSchema();
                SetForm();
            }
        }

        protected void VerTabla(Object sender, EventArgs e)
        {
            string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
            Response.Redirect(indexPage + "?process=DBManager.Selects&ret=1&table=" + table + "&path=" + Request.QueryString["path"]);
        }
        protected void VerTablaPadre(Object sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=DBManager.Selects&table=" + Request.QueryString["tableRef"] + "&path=" + Request.QueryString["path"]);
        }

        protected void ValorObjeto()
        {
            string[] valorFk = Request.QueryString["vlr"].ToString().Split('!');
            ViewState["hijaValorPK"] = valorFk;
        }
        protected void CheckExists()
        {
            //Utils.MostrarAlerta(Response, "checkexist!!");   // este mensaje es innecesario....
            //Si existe la fila, redireccionar a edicion
            //Esquema
            string sql = "SELECT * FROM " + table + " FETCH FIRST 1 ROWS ONLY;";
            DataSet ds1 = new DataSet();
            if (arlFkNombres.Count == 0) return;
            DBFunctions.Request(ds1, IncludeSchema.YES, sql);
            string sqlExiste = "";
            //Revisar que todos los primary keys esten
            for (int i = 0; i < ds1.Tables[0].PrimaryKey.Length; i++)
            {
                //  if (!arlFkNombres.Contains(ds1.Tables[0].PrimaryKey[i].ColumnName)) return;  // LO SACA DEL BUCLE y no sigue buscando la llave primaria ?
                if (arlFkNombres.Contains(ds1.Tables[0].PrimaryKey[i].ColumnName))
                {
                    if (ds1.Tables[0].PrimaryKey[i].DataType.ToString() == "System.String")
                        sqlExiste += ds1.Tables[0].PrimaryKey[i].ColumnName + "='" + arlFkValores[arlFkNombres.IndexOf(ds1.Tables[0].PrimaryKey[i].ColumnName)].ToString() + "' AND ";
                    else
                        sqlExiste += ds1.Tables[0].PrimaryKey[i].ColumnName + "=" + arlFkValores[arlFkNombres.IndexOf(ds1.Tables[0].PrimaryKey[i].ColumnName)].ToString() + " AND ";
                }
            }
            sqlExiste = sqlExiste.Substring(0, sqlExiste.Length - 4);
            //Existe?
            if (DBFunctions.RecordExist("SELECT * FROM " + table + " WHERE " + sqlExiste))
                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + sqlExiste + "&path=" + Request.QueryString["path"] + "&fks=" + Request.QueryString["fks"] + "&tableRef=" + Request.QueryString["tableRef"] + "&pksRef=" + Request.QueryString["pksRef"] + "&pathRef=" + Request.QueryString["pathRef"] + "&path=" + Request.QueryString["path"]);
            else
            {
                string[] valorFk = Request.QueryString["fks"].ToString().Split('!');
                ViewState["hijaValorPK"] = valorFk;
            }

        }

        protected void GetSchema()
        {
            ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='" +
                table + "'; SELECT fkcolnames, reftbname, pkcolnames, relname, refkeyname FROM sysibm.sysrels WHERE tbname='" +
                table + "'");
            //ds = DBFunctions.Request(ds, IncludeSchema.NO, SQLParser.GetTableStructureExtended(table) + ";" + 
            //                                                SQLParser.GetTableForeignExtended(table) );

            //ds1 = DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT constname, colname, colseq FROM sysibm.syskeycoluse WHERE tbname='" + table + "' AND constname NOT IN(SELECT name FROM sysibm.sysindexes WHERE tbname='" + table + "')");
            ds1 = DBFunctions.Request(ds1, IncludeSchema.NO, SQLParser.GetTableForeignKeySequence(table) );

            string camposConsulta = "*";
            if (table == "MNIT")  //Organiza orden de campos para mostar MNIT.
            {
                camposConsulta = @"TNIT_TIPONIT,
                                    MNIT_NIT,
                                    MNIT_DIGITO,
                                    MNIT_NOMBRES,
                                    MNIT_NOMBRE2,
                                    MNIT_APELLIDOS,
                                    MNIT_APELLIDO2,
                                    PCIU_CODIGOEXPNIT,
                                    TNAC_TIPONACI,
                                    MNIT_DIRECCION,
                                    PCIU_CODIGO,
                                    MNIT_TELEFONO,
                                    MNIT_CELULAR,
                                    MNIT_EMAIL,
                                    MNIT_WEB,
                                    TVIG_VIGENCIA,
                                    TSOC_SOCIEDAD,
                                    TREG_REGIIVA,
                                    PSEC_ACTIVIDAD,
                                    MNIT_REPRESENTANTE, 
                                    MNIT_NITREPRESENTANTE";
                //if (ds.Tables[0].Rows.Count == 21)
                //{
                //    camposConsulta += ",MNIT_REPRESENTANTE, MNIT_NITREPRESENTANTE";
                //}
            }

            if (Request.QueryString["action"] == "update")
            {
                string prueba = Request.QueryString["pks"];
                string tokenID = Server.UrlEncode(Request.QueryString["pks"]);
                string tokenID2 = Server.UrlEncode(tokenID);
                string cadenaPks = Request.QueryString.ToString();

                string pks = ObtenerLlavesPrimarias(cadenaPks);

                if (pks.IndexOf("PCAT_CODIGO", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT " + camposConsulta + " FROM " + table + " WHERE " + pks);
                    if (ds.Tables[2].Rows.Count == 0)
                    {
                        ds.Tables.RemoveAt(2);
                        pks = pks.Replace("+", " ");
                        ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT " + camposConsulta + " FROM " + table + " WHERE " + pks);
                    }
                    //revisar caso mazco cali
                    //pks = pks.Trim().Replace(" ", "+");
                }
                else
                {
                    string sql = "SELECT  " + camposConsulta + "  FROM " + table + " WHERE " + pks;
                    if (sql.Substring(sql.Length - 1, 1) == "+")
                        sql = sql.Substring(0, sql.Length - 1);
                    ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
                    if (ds.Tables[2].Rows.Count == 0)
                    {
                        ds.Tables.RemoveAt(2);
                        pks = pks.Replace("+", " ");
                        ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT  " + camposConsulta + "  FROM " + table + " WHERE " + pks);
                    }
                }
                ViewState["updatedRow"] = ds;
                //ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM " + table + " WHERE " + pks);
            }
            if (Request.QueryString["action"] == "copiar")
            {
                string tokenID = Server.UrlEncode(Request.QueryString["pks"]);
                string tokenID2 = Server.UrlEncode(tokenID);
                string cadenaPks = Request.QueryString.ToString();


                string pks = ObtenerLlavesPrimarias(cadenaPks);

                if (pks.IndexOf("PCAT_CODIGO", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT  " + camposConsulta + "  FROM " + table + " WHERE " + pks);
                }
                else
                {
                    ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT  " + camposConsulta + "  FROM " + table + " WHERE " + pks);
                }
            }
            if (Request.QueryString["action"] == "insert")
            {
                ds = DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT " + camposConsulta + " FROM " + table + " FETCH FIRST 1 ROWS ONLY ;");
                ViewState["updatedRow"] = ds;
            }

        }

        protected string ObtenerLlavesPrimarias(string cadenaPks)
        {
            cadenaPks = cadenaPks.Substring(cadenaPks.IndexOf("pks=") + 4, cadenaPks.Length - (cadenaPks.IndexOf("pks=") + 5));
            cadenaPks = cadenaPks.Substring(0, cadenaPks.IndexOf('&'));
            cadenaPks = cadenaPks.Replace("%3d", "=");
            cadenaPks = cadenaPks.Replace("%27", "'");
            cadenaPks = cadenaPks.Replace("%2f", "/");
            cadenaPks = cadenaPks.Replace("%25", "%");
            cadenaPks = cadenaPks.Replace("+", " ");
            cadenaPks = cadenaPks.Replace("'\'", "+");


            if (cadenaPks.Contains("AND"))
            {

                String prueba = cadenaPks;
                string[] partesLlave = cadenaPks.Split(new string[] { " AND " }, StringSplitOptions.None);
                cadenaPks = "";

                for (int k = 0; k < partesLlave.Length; k++)
                {
                    if (partesLlave[k].IndexOf(" - ") != -1)
                    {
                        partesLlave[k] = partesLlave[k].Substring(0, partesLlave[k].IndexOf(" - "));
                        if (partesLlave[k].IndexOf("'") != -1)
                        {
                            partesLlave[k] += "'";
                        }
                    }
                    cadenaPks += partesLlave[k] + " AND ";
                }
                cadenaPks += "@";
                cadenaPks = cadenaPks.Replace("AND @", " ");
            }

            return cadenaPks;
        }

        protected void SetForm()
        {
            for (i = 0; i < ds.Tables[ds.Tables.Count - 1].Columns.Count; i++)
            {
                DataRow[] scRow = ds.Tables[0].Select("NAME = '" + ds.Tables[ds.Tables.Count - 1].Columns[i].ColumnName + "'");
                if (scRow.Length > 0)
                {
                    DataRow[] fkRow = ds.Tables[1].Select("FKCOLNAMES = ' " + scRow[0][1].ToString() + "'");
                    DataRow[] fkRowComposite = ds1.Tables[0].Select("COLNAME = '" + scRow[0][1].ToString() + "'");

                    //string tabla = fkRow[0][0].ToString();
                    if ((fkRow.Length > 0) && (Convert.ToInt32(fkRowComposite[0][2]) == 1))
                        MapFKType(scRow[0], fkRow[0]);
                    else
                    {
                        DataRow[] fkRow2 = null;
                        if (fkRowComposite.Length > 0)
                            fkRow2 = ds.Tables[1].Select("RELNAME = '" + fkRowComposite[0][0].ToString().Trim() + "'");
                        if (fkRowComposite.Length > 0 && fkRow2.Length > 0)
                            MapFKType(scRow[0], fkRow2[0], fkRowComposite[0]);
                        else
                        {
                            if (scRow[0][2].ToString().IndexOf("CHAR") != -1 || scRow[0][2].ToString().IndexOf("VAR") != -1 || scRow[0][2].ToString().IndexOf("VARCHAR") != -1 )
                                MapStringType(scRow[0]);
                            if (scRow[0][2].ToString().IndexOf("CLOB") != -1)
                            {
                                ViewState["CLOB"] = "CLOB";
                                ViewState["CLOBINDEX"] = i;
                                MapStringType(scRow[0]);
                            }
                            if (scRow[0][2].ToString().IndexOf("INT") != -1 || scRow[0][2].ToString().IndexOf("INTEGER") != -1 || scRow[0][2].ToString().IndexOf("SMALLINT") != -1)
                                MapIntType(scRow[0]);
                            if (scRow[0][2].ToString().IndexOf("DECIMAL") != -1 || scRow[0][2].ToString().IndexOf("DOUBLE") != -1)
                                MapDecimalType(scRow[0]);
                            if (scRow[0][2].ToString().IndexOf("DATE") != -1)
                                MapDateType(scRow[0]);
                            if (scRow[0][2].ToString().Trim() == "TIME")
                                MapTimeType(scRow[0]);
                            if (scRow[0][2].ToString().Trim() == "TIMESTMP" || scRow[0][2].ToString().Trim() == "TIMESTAMP")
                                MapTimeStampType(scRow[0]);
                        }
                    }
                }
            }
            AddValidationSummary();
            phForm.Controls.Add(tbForm);
            
            //Manejo de las referencias de repuestos
            if (table == "MITEMS")
            {
                tbRefItem.Attributes.Add("onkeyup", "ItemMask(" + tbRefItem.ClientID + "," + ddlRefItem.ClientID + ");");
                ddlRefItem.Attributes.Add("onchange", "ChangeLine(" + ddlRefItem.ClientID + "," + tbRefItem.ClientID + ");");
            }
            else if (table == "MNIT")
            {
                //((TextBox)tbForm.FindControl("control_0")).Attributes.Add("onblur", "Cargar_Nombre(this)");
                //((TextBox)tbForm.FindControl("control_2")).Attributes.Add("onblur", "aMayusculas(this)");
                //((TextBox)tbForm.FindControl("control_3")).Attributes.Add("onblur", "aMayusculas(this)");
                //((TextBox)tbForm.FindControl("control_4")).Attributes.Add("onblur", "aMayusculas(this)");
                //((TextBox)tbForm.FindControl("control_5")).Attributes.Add("onblur", "aMayusculas(this)");

                //Arreglo para que opere la modificación de estructura de MNIT...
                ((DropDownList)tbForm.FindControl("control_0")).Attributes.Add("onchange", "ValidarIdentificacion(this," + ((TextBox)tbForm.FindControl("control_3")).ClientID + "," + ((TextBox)tbForm.FindControl("control_4")).ClientID + "," + ((TextBox)tbForm.FindControl("control_6")).ClientID + "," + ((DropDownList)tbForm.FindControl("control_16")).ClientID + "," + ((DropDownList)tbForm.FindControl("control_17")).ClientID + ");");
                ((TextBox)tbForm.FindControl("control_1")).Attributes.Add("onblur", "Cargar_Nombre(this)");
                ((TextBox)tbForm.FindControl("control_2")).Attributes.Add("onblur", "ValidarDigitoVerificacion(this," + ((TextBox)tbForm.FindControl("control_1")).ClientID + "," + ((DropDownList)tbForm.FindControl("control_0")).ClientID + ")");
                ((TextBox)tbForm.FindControl("control_3")).Attributes.Add("onblur", "aMayusculas(this)");
                ((TextBox)tbForm.FindControl("control_4")).Attributes.Add("onblur", "aMayusculas(this)");
                ((TextBox)tbForm.FindControl("control_5")).Attributes.Add("onblur", "aMayusculas(this)");
                ((TextBox)tbForm.FindControl("control_6")).Attributes.Add("onblur", "aMayusculas(this)");

                ((DropDownList)tbForm.FindControl("control_16")).SelectedValue = "P";
                ((DropDownList)tbForm.FindControl("control_17")).SelectedValue = "N";

                tbRefItem.ReadOnly = true;
                tbRefItem.Attributes.Add("onclick", "WizardDirection(this);");
            }
            else if (table == "CEMPRESA" || table == "MEMPLEADO" || table == "MNOMINACAPITAL" || table == "MNOMINAFAMILIA" || table == "PALMACEN")
            {
                tbRefItem.ReadOnly = true;
                tbRefItem.Attributes.Add("onclick", "WizardDirection(this);");
            }
        }

        [Ajax.AjaxMethod]
        public string Cargar_Nombre(string nit)
        {

            string n = DBFunctions.SingleData(String.Format("select mnit_nit from dbxschema.mnit where mnit_nit='" + nit + "'"));

            return n;

        }




        protected void AddValidationSummary()
        {
            ValidationSummary sum = new ValidationSummary();
            sum.ShowMessageBox = true;
            sum.ShowSummary = false;
            sum.HeaderText = "Existe Algún Problema con los siguientes campos :";
            TableCell tc1 = new TableCell();
            tc1.Controls.Add(sum);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tbForm.Rows.Add(tr);
        }

        protected void MapTimeType(DataRow dr)
        {
            Label lb = PutLabel(dr[0].ToString());
            TextBox tb = PutTextBox(8);
            tb.Text = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
            RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{2}:[0-9]{2}:[0-9]{2}", dr[0].ToString());
            System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();
            tc2.Controls.Add(tb);
            tc2.Controls.Add(new LiteralControl("   "));
            tc2.Controls.Add(img2);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbForm.Rows.Add(tr);
        }

        protected void MapTimeStampType(DataRow dr)
        {
            Label lb = PutLabel(dr[0].ToString());
            TextBox tb = PutTextBox(25);
            tb.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{2}:[0-9]{2}:[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}", dr[0].ToString());
            System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();
            tc2.Controls.Add(tb);
            tc2.Controls.Add(new LiteralControl("   "));
            tc2.Controls.Add(img2);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbForm.Rows.Add(tr);
        }

        protected void MapStringType(DataRow dr)
        {
            FileUpload fu;
            String campo = dr[1].ToString();
            bool isFileUpload = DBFunctions.SingleData(String.Format("select stab_esupload from stablacampo where stab_nombre='{0}' and stab_campo='{1}'", table, campo)) == "S";
            int length = Convert.ToInt32(dr[3]);
            if (length <= 5)
                length += 2;
            if (ViewState["CLOB"] == "CLOB")
                length = 680;

            RequiredFieldValidator rfv;
            Label lb = PutLabel(dr[0].ToString());
            TextBox tb = PutTextBox(length);
            tb.MaxLength = length;
            
            if(ViewState["CLOB"] == "CLOB")
            {
                tb.TextMode = TextBoxMode.MultiLine;
                tb.Wrap = true;

                //tb.ValidateRequestMode = ValidateRequestMode.Disabled;
                Response.Write("<script language:javascript>var parametroJS = 'control_" + ViewState["CLOBINDEX"]  + "'</script>");
                ViewState["CLOB"] = "";
                ViewState["CLOBINDEX"] = "";
            }

            System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();

            if (isFileUpload)
            {
                System.Web.UI.WebControls.Image imgData = this.PutImageData();
                //System.Web.UI.WebControls.Button btnCargar = PutButton();
                fu = PutFileUpload(imgData.ClientID);

                tc2.Controls.Add(fu);
                tc2.Controls.Add(imgData);
                //tc2.Controls.Add(btnCargar);
            }
            else
            {
                tc2.Controls.Add(tb);
                if (dr[4].ToString() == "N")
                {
                    rfv = RequiredValidator(tb.ID, dr[0].ToString());
                    tc2.Controls.Add(rfv);
                }
                if (dr[6].ToString() != "")
                {
                    tb = PutTextBox(7);
                    //tb.Text = "DEFAULT";
                    tb.Text = dr[6].ToString();
                    tb.ReadOnly = true;
                }
            }

            tc2.Controls.Add(new LiteralControl("   "));
            tc2.Controls.Add(img2);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbForm.Rows.Add(tr);
            if (table == "MITEMS" && dr[1].ToString() == "MITE_CODIGO")
                tbRefItem = tb;
            else if ((table == "MNIT" && dr[1].ToString() == "MNIT_DIRECCION") || (table == "CEMPRESA" && dr[1].ToString() == "CEMP_DIRECCION") || (table == "MEMPLEADO" && dr[1].ToString() == "MEMP_DIRECCION") || (table == "MNOMINACAPITAL" && dr[1].ToString() == "MNOM_DIRECBIEN") || (table == "MNOMINAFAMILIA" && dr[1].ToString() == "MNOM_DIRECCION") || (table == "PALMACEN" && dr[1].ToString() == "PALM_DIRECCION") || (table == "PALMACEN" && dr[1].ToString() == "PALM_DIRECCION"))
                tbRefItem = tb;
        }

        protected void MapIntType(DataRow dr)
        {
            Label lb = PutLabel(dr[0].ToString());
            TextBox tb = PutTextBox(8);
            tb.MaxLength = 10;
            System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();

            if (dr[6].ToString() != "")
            {
                tb.Text = dr[6].ToString();
            }
            else if (dr[4].ToString() != "Y")
            {
                tb.Text = "0";
            }

            if (dr[7].ToString() == "A" || dr[7].ToString() == "D")
            {
                tb = PutTextBox(7);
                tb.Text = "DEFAULT";
                tb.ReadOnly = true;
                tc1.Controls.Add(lb);
                tc2.Controls.Add(tb);
            }
            else
            {
                RegularExpressionValidator rev = Validator(tb.ID, "[0-9]+", dr[0].ToString());
                tc1.Controls.Add(lb);
                tc2.Controls.Add(tb);
                tc2.Controls.Add(rev);
            }

            if (dr[4].ToString() == "N")
            {
                RequiredFieldValidator rfv = RequiredValidator(tb.ID, dr[0].ToString());
                tc2.Controls.Add(rfv);
            }

            tc2.Controls.Add(new LiteralControl("   "));
            tc2.Controls.Add(img2);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tb.CssClass = "AlineacionDerecha";
            tbForm.Rows.Add(tr);
        }

        protected void MapDecimalType(DataRow dr)
        {
            Label lb = PutLabel(dr[0].ToString());
            TextBox tb = PutTextBox(20);
            RegularExpressionValidator rev = Validator(tb.ID, "[- .]?[0-9\\,\\.]+", dr[0].ToString());
            System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
            tb.Attributes.Add("onkeyup", "NumericMaskE(this,event)");
            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();

            if (dr[6].ToString() != "")
            {
                tb.Text = dr[6].ToString();
            }
            else if (dr[4].ToString() != "Y")
            {
                tb.Text = "0";
            }

            tc2.Controls.Add(tb);
            tc2.Controls.Add(rev);
            if (dr[4].ToString() == "N")
            {
                RequiredFieldValidator rfv = RequiredValidator(tb.ID, dr[0].ToString());
                tc2.Controls.Add(rfv);
            }
            tc2.Controls.Add(new LiteralControl("   "));
            tc2.Controls.Add(img2);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tb.CssClass = "AlineacionDerecha";
            tbForm.Rows.Add(tr);
        }

        protected void MapDateType(DataRow dr)
        {
            Label lb = PutLabel(dr[0].ToString());
            TextBox tb = PutTextBox(20);
            RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{4}-[0-9]{2}-[0-9]{2}", dr[0].ToString());
            /*IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
            tb.Text = DateTime.Now.GetDateTimeFormats('d', culture)[0];*/
            tb.Attributes.Add("onkeyup", "DateMask(this)");
            System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();
            if (dr[4].ToString() == "N")
            {
                tb.Text = DateTime.Now.ToString("yyyy-MM-dd");
                RequiredFieldValidator rfv = RequiredValidator(tb.ID, dr[0].ToString());
                tc2.Controls.Add(rfv);
            }
            else
                tb.Text = "";
            tc2.Controls.Add(tb);
            tc2.Controls.Add(rev);
            tc2.Controls.Add(new LiteralControl("   "));
            tc2.Controls.Add(img2);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbForm.Rows.Add(tr);
        }

        protected void MapFKType(DataRow dr, DataRow drFK)
        {
            string tableName = drFK[1].ToString();
            string tamaño = "";
            Label lb;
            TableCell tc1;
            TableCell tc2;
            TableRow tr = new TableRow();
            System.Web.UI.WebControls.Image img2 = PutImageFK(drFK[1].ToString());
            System.Web.UI.WebControls.Image img3 = this.PutImageHelp(dr);
            if (tableName == "MNIT" || tableName == "MITEMS")
            {
                lb = PutLabel(dr[0].ToString());
                TextBox tb = PutTextBox(20);
                tc1 = new TableCell();
                tc1.Controls.Add(lb);
                tc2 = new TableCell();
                tc2.Controls.Add(tb);
                tc2.Controls.Add(img2);
                tc2.Controls.Add(new LiteralControl("   "));
                tc2.Controls.Add(img3);
                tr.Cells.Add(tc1);
                tr.Cells.Add(tc2);
            }
            else
            {
                lb = PutLabel(dr[0].ToString());
                DropDownList ddl = PutDropDownList(drFK);
                if (ddl.Items.Count == 0 || ddl.SelectedItem.ToString() == null)
                {
                    tamaño = "";
                }
                else
                {
                    tamaño = ddl.SelectedItem.ToString();
                }
                if (tamaño.Length <= 10)
                {
                    ddl.CssClass = "dpequeno";
                }
                else if (tamaño.Length > 10 & tamaño.Length <= 50)
                {
                    ddl.CssClass = "dmediano";
                }
                else
                {
                    ddl.CssClass = "dgrande";
                }

                if (dr[4].ToString() == "Y")
                {
                    ddl.Items.Add(new ListItem("No requiere", "null"));
                    ddl.SelectedIndex = ddl.Items.Count - 1;
                }
                //Foraneas preasignadas

                if (arlFkNombres.Contains(dr[1]))
                {
                    if (ddl.Items.FindByValue(arlFkValores[arlFkNombres.IndexOf(dr[1].ToString())].ToString()) != null)
                    {
                        ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(arlFkValores[arlFkNombres.IndexOf(dr[1].ToString())].ToString()));
                        ListItem lstBloq = new ListItem(ddl.SelectedItem.Text, ddl.SelectedItem.Value);
                        ddl.Items.Clear();
                        ddl.Items.Add(lstBloq);
                        ddl.SelectedIndex = 0;
                        //ddl.Enabled=false;
                        img2.Visible = false;
                    }
                }
                if (arlFkINombres.Contains(dr[1]))
                {
                    if (ddl.Items.FindByValue(arlFkIValores[arlFkINombres.IndexOf(dr[1].ToString())].ToString()) != null)
                    {
                        ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(arlFkIValores[arlFkINombres.IndexOf(dr[1].ToString())].ToString()));
                        ListItem lstBloq = new ListItem(ddl.SelectedItem.Text, ddl.SelectedItem.Value);
                        ddl.Items.Clear();
                        ddl.Items.Add(lstBloq);
                        ddl.SelectedIndex = 0;
                        //ddl.Enabled=false;
                        img2.Visible = false;
                    }
                }

                tc1 = new TableCell();
                tc1.Controls.Add(lb);
                tc2 = new TableCell();
                tc2.Controls.Add(ddl);
                tc2.Controls.Add(img2);
                tc2.Controls.Add(new LiteralControl("   "));
                tc2.Controls.Add(img3);
                tr.Cells.Add(tc1);
                tr.Cells.Add(tc2);

                if (table == "MITEMS" && dr[1].ToString() == "PLIN_CODIGO")
                    ddlRefItem = ddl;
                img = img2;
            }
            tbForm.Rows.Add(tr);
        }

        protected void MapFKType(DataRow dr, DataRow drFK, DataRow drFKComp)
        {
            Label lb = PutLabel(dr[0].ToString());
            DropDownList ddl = new DropDownList();
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            System.Web.UI.WebControls.Image img3 = this.PutImageHelp(dr);
            if (Convert.ToInt32(drFKComp[2]) == 1)
            {
                //ddl = PutDropDownList(drFK[1].ToString(), DBFunctions.SingleData("SELECT colname FROM sysibm.syskeycoluse WHERE constname='" + drFK[4] + "' AND colseq=" + drFKComp[2] + ""), "");
                ddl = PutDropDownList(drFK[1].ToString(), DBFunctions.SingleData(SQLParser.GetKeyColumnByForeignName(drFK[4].ToString(), drFKComp[2].ToString()) ), "");

                ddl.SelectedIndexChanged += new EventHandler(this.ChangedFK);
                ddl.AutoPostBack = true;

                //fksComps += drFK[3] + "-" + ddl.ID + "-" + drFK[1] + "-" + DBFunctions.SingleData("SELECT colname FROM sysibm.syskeycoluse WHERE constname='" + drFK[4] + "' AND colseq=" + drFKComp[2] + "") + "-" + DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name=(SELECT colname FROM sysibm.syskeycoluse WHERE constname='" + drFK[4] + "' AND colseq=" + drFKComp[2] + " AND tbname='" + drFK[1] + "')") + "*";
                fksComps += drFK[3] + "-" + ddl.ID + "-" + drFK[1] + "-" + DBFunctions.SingleData(SQLParser.GetKeyColumnByForeignName(drFK[4].ToString(), drFKComp[2].ToString()) ) + "-" + DBFunctions.SingleData(SQLParser.GetDataTypeByForeignName(drFK[1].ToString() , drFK[4].ToString(), drFKComp[2].ToString()) ) + "*";

                img2 = PutImage(drFK[1].ToString());
            }
            else
            {
                string condicional = "";
                string[] sepMain = fksComps.Split('*');
                for (int k = 0; k < sepMain.Length; k++)
                {
                    string[] sepSec = sepMain[k].Split('-');
                    if (sepSec[0].ToString().Trim() == drFK[3].ToString().Trim())
                    {
                        if (sepSec[4].ToString().Trim() == "CHARACTER" || sepSec[4].ToString().Trim() == "VARCHAR" || sepSec[4].ToString().Trim() == "TIME" || sepSec[4].ToString().Trim() == "DATE" || sepSec[4].ToString().Trim() == "CHAR")
                            condicional = sepSec[3] + " = '" + this.ValueDdl(sepSec[1].ToString(), tbForm) + "' AND";
                        else
                            condicional = sepSec[3] + " = '" + this.ValueDdl(sepSec[1].ToString(), tbForm) + "' AND";
                    }
                }
                //ddl = PutDropDownList(drFK[1].ToString(), DBFunctions.SingleData("SELECT colname FROM sysibm.syskeycoluse WHERE constname='" + drFK[4] + "' AND colseq=" + drFKComp[2] + ""), condicional.Substring(0, condicional.Length - 3));
                ddl = PutDropDownList(drFK[1].ToString(), DBFunctions.SingleData( SQLParser.GetKeyColumnByForeignName(drFK[4].ToString(), drFKComp[2].ToString()) ), condicional.Substring(0, condicional.Length - 3));
                
                img2 = PutImage(drFK[1].ToString());
                //ddlSec += drFK[3] + "-" + ddl.ID + "-" + drFK[1] + "-" + DBFunctions.SingleData("SELECT colname FROM sysibm.syskeycoluse WHERE constname='" + drFK[4] + "' AND colseq=" + drFKComp[2] + "") + "-" + DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name=(SELECT colname FROM sysibm.syskeycoluse WHERE constname='" + drFK[4] + "' AND colseq=" + drFKComp[2] + " AND tbname='" + drFK[1] + "')") + "*";
                ddlSec += drFK[3] + "-" + ddl.ID + "-" + drFK[1] + "-" + 
                    DBFunctions.SingleData( SQLParser.GetKeyColumnByForeignName(drFK[4].ToString(), drFKComp[2].ToString()) ) + 
                                            "-" + DBFunctions.SingleData( SQLParser.GetDataTypeByForeignName(drFK[1].ToString(), drFK[4].ToString(), drFKComp[2].ToString()) ) + "*";
            }

            //Foraneas preasignadas
            if (arlFkNombres.Contains(dr[1]))
                if (ddl.Items.FindByValue(arlFkValores[arlFkNombres.IndexOf(dr[1].ToString())].ToString()) != null)
                {
                    ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(arlFkValores[arlFkNombres.IndexOf(dr[1].ToString())].ToString()));
                    ListItem lstBloq = new ListItem(ddl.SelectedItem.Text, ddl.SelectedItem.Value);
                    ddl.Items.Clear();
                    ddl.Items.Add(lstBloq);
                    ddl.SelectedIndex = 0;
                    //ddl.Enabled=false;
                    img2.Visible = false;
                }

            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();
            tc2.Controls.Add(ddl);
            tc2.Controls.Add(img2);
            tc2.Controls.Add(new LiteralControl("   "));
            tc2.Controls.Add(img3);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbForm.Rows.Add(tr);
            img = img2;
        }

        protected System.Web.UI.WebControls.Image PutImage(string tableName)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ID = "img_" + i.ToString();
            img.ImageUrl = images + "AMS.Search.png";
            string newsels = sels.Replace("'", "\\'");
            if (newsels.IndexOf("MITEMS") != -1)
                img.Attributes.Add("onClick", "ModalDialog(_ctl1_control_" + i.ToString() + ",'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, PLIN.plin_tipo as LINEA FROM mitems MIT, plineaitem PLIN  WHERE MIT.plin_codigo=PLIN.plin_codigo ORDER BY MIT.mite_codigo',new Array(),null,1);");
            else
                img.Attributes.Add("onClick", "ModalDialog(_ctl1_control_" + i.ToString() + ",\"" + newsels + "\",new Array());");
            img.ToolTip = "Busqueda";
            return img;
        }

        protected System.Web.UI.WebControls.Image PutImageFK(string tableName)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            int n, l;
            img.ID = "imgLupa_" + i.ToString();
            img.ImageUrl = images + "AMS.Search.png";
            string newsels = sels.Replace("'", "\\'"), fks = "", colN, colSel = "";
            //Armar select para que la llave primaria este en la primera columna
            ArrayList arrFKCols = new ArrayList();
            DataSet dsInfoFK = new DataSet();
            //DBFunctions.Request(dsInfoFK, IncludeSchema.NO,
            //    "SELECT NAME FROM sysibm.SYSCOLUMNS where TBNAME ='" + tableName + "' order by colno;" +
            //    "SELECT PKCOLNAMES FROM SYSIBM.SYSRELS WHERE TBNAME='" + table + "' AND REFTBNAME='" + tableName + "';");
            DBFunctions.Request(dsInfoFK, IncludeSchema.NO,
                SQLParser.GetColumnsTable(tableName) + ";" +
                SQLParser.GetPrimaryKeysTable(table, tableName) + ";");

            if (dsInfoFK.Tables[1].Rows.Count > 0)
                fks = dsInfoFK.Tables[1].Rows[0]["PKCOLNAMES"].ToString();
            for (n = 0; n < fks.Length; n += 20)
            {
                l = 20;
                if ((n + l) > fks.Length)
                    l = fks.Length - n;
                arrFKCols.Add(fks.Substring(n, l).Trim().ToUpper());
            }
            for (n = 0; n < dsInfoFK.Tables[0].Rows.Count; n++)
            {
                colN = dsInfoFK.Tables[0].Rows[n]["NAME"].ToString();
                if (!arrFKCols.Contains(colN))
                    arrFKCols.Add(colN.Trim().ToUpper());
            }
            for (n = 0; n < arrFKCols.Count; n++)
                colSel += arrFKCols[n].ToString() + ",";
            if (colSel.EndsWith(","))
                colSel = colSel.Substring(0, colSel.Length - 1);
            colSel = "SELECT " + colSel + " FROM DBXSCHEMA." + tableName + " " + filtroSels + ";";

            if (newsels.IndexOf("MITEMS") != -1)
                img.Attributes.Add("onClick", "ModalDialog(_ctl1_control_" + i.ToString() + ",'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, PLIN.plin_tipo as LINEA FROM mitems MIT, plineaitem PLIN  WHERE MIT.plin_codigo=PLIN.plin_codigo ORDER BY MIT.mite_codigo',new Array(),null,1);");
            else
            {
                if (tableName == "MNIT")
                    img.Attributes.Add("onClick", "ModalDialog(_ctl1_control_" + i.ToString() + ",\"" + colSel + "\",new Array(),1);");
                else
                    img.Attributes.Add("onClick", "ModalDialog(_ctl1_control_" + i.ToString() + ",\"" + colSel + "\",new Array());");
            }

            img.ToolTip = "Busqueda";
            return img;
        }

        protected System.Web.UI.WebControls.Image PutImageHelp(DataRow dr)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ID = "imghelp_" + i.ToString();
            img.ImageUrl = images + "AMS.Help.png";
            img.Attributes.Add("onClick", "ModalDialogAyuda('" + Request.QueryString["table"] + "','" + dr[1].ToString() + "');");
            img.ToolTip = "Haga Click para Ver la Ayuda";
            return img;
        }

        protected System.Web.UI.WebControls.Image PutImageData()
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ID = "imgdata_" + i.ToString();
            img.Width = 5;
            img.Height = 5;
            if (Request.QueryString["action"] == "update")
            {
                img.Width = 170;
                img.Height = 180;
            }
            img.Visible = true;

            return img;
        }

        protected System.Web.UI.WebControls.Button PutButton()
        {
            System.Web.UI.WebControls.Button btn = new System.Web.UI.WebControls.Button();
            btn.ID = "btn_" + i.ToString();
            btn.Text = "Cargar";
            //btn.Attributes.Add("onClick", "cargarImagen(imgdata_" + i.ToString() + ", control_" + i.ToString() + ");");
            //btn.Click += CargarImagen;

            return btn;
        }

        protected void CargarImagen(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            string cbname = ctrl.ClientID;
        }

        protected Label PutLabel(string text)
        {
            Label lb = new Label();
            lb.ID = "label_" + i.ToString();
            lb.Text = text + ": ";
            return lb;
        }

        protected TextBox PutTextBox(int l)
        {
            TextBox tb = new TextBox();
            tb.ID = "control_" + i.ToString();
            if (l <= 20 && l > 1)
                //tb.Width = new Unit(l*11);
                tb.CssClass = "tpequeno";
            if (l <= 100 && l > 20)
                //tb.Width = new Unit(l*7);
                tb.CssClass = "tmediano";
            if (l > 100 && l < 1000)
            {
                tb.CssClass = "tgrande";
                tb.Height = new Unit(100);
                tb.TextMode = TextBoxMode.MultiLine;
            }
            if (l >= 1000)
            {
                int h = (int)Math.Floor((double)l / 1000);
                tb.CssClass = "tgrande";
                tb.Height = new Unit(h * 100);
                tb.TextMode = TextBoxMode.MultiLine;
            }
            return tb;
        }

        protected FileUpload PutFileUpload(string imagenID)
        {
            FileUpload fu = new FileUpload();
            fu.ID = "control_" + i.ToString();
            fu.Attributes.Add("onchange", "readURL(this, '_ctl1_" + imagenID + "');");

            return fu;
        }

        protected DropDownList PutDropDownList(DataRow drT)
        {
            DataSet tempDS = new DataSet();
            string tableName = drT[1].ToString();
            string fieldName = drT[0].ToString();
            fieldName = fieldName.Replace(" ", "");



            //if (tableName == "PFONDOCESANTIAS" || tableName == "PFONDOPENSION")
            //{
            //        DataSet dsC = new DataSet();
            //        DBFunctions.Request(dsC, IncludeSchema.NO, "select " + fieldName + " from " + tableName);

            //        if (dsC.Tables[0].Rows.Count == 0)
            //        {
            //            Utils.MostrarAlerta(Response, "Faltan parámentros...");
            //            btInsert.Visible = false;
            //        }

            //}
            string filtro = DBFunctions.SingleData("select stab_sentenciaprev from stablacampo where stab_nombre='" + table + "' and stab_campo='" + fieldName.Trim() + "' and stab_sentenciaprev is not null;");

            filtro = filtro.Replace("@", "'");

            if (filtro.Length > 0) filtro = " WHERE " + filtro + " ";

            if (tableName == "MNIT")
            {
                sels = "SELECT mnit_nit,mnit_nit CONCAT ' - ' CONCAT mnit_apellidos concat ' ' concat coalesce(mnit_apellido2,'') concat ' ' concat mnit_nombres concat ' ' concat coalesce(mnit_nombre2 ,'') FROM " + tableName + " " + filtro + "ORDER BY mnit_nit";
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, sels);
                //sels="SELECT NIT,NOMBRES FROM DBXSCHEMA.VTESORERIA_MNIT ORDER BY NIT";
            }
            else if (tableName == "MCUENTA")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT mcue_codipuc,  mcue_codipuc CONCAT ' - ' CONCAT mcue_nombre FROM " + tableName + " WHERE timp_codigo <> 'N' order by mcue_codipuc");
                sels = "SELECT MCUE_CODIPUC AS CODIGO,MCUE_NOMBRE AS NOMBRE FROM DBXSCHEMA.MCUENTA ORDER BY CODIGO";
            }
            else if (tableName == "MPROVEEDOR")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT MPR.mnit_nit, MPR.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos CONCAT ' - ' CONCAT MNI.mnit_nombres FROM mproveedor MPR, mnit MNI WHERE MNI.mnit_nit = MPR.mnit_nit");
                sels = "SELECT MPR.mnit_nit, MPR.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos CONCAT ' - ' CONCAT MNI.mnit_nombres FROM mproveedor MPR, mnit MNI WHERE MNI.mnit_nit = MPR.mnit_nit";
            }
            else if (tableName == "MCLIENTE")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT MCL.mnit_nit, MCL.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos CONCAT ' ' CONCAT MNI.mnit_nombres FROM mcliente MCL, mnit MNI WHERE MNI.mnit_nit = MCL.mnit_nit");
                sels = "SELECT MCL.mnit_nit, MCL.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos CONCAT ' ' CONCAT MNI.mnit_nombres FROM mcliente MCL, mnit MNI WHERE MNI.mnit_nit = MCL.mnit_nit";
            }
            else if (tableName == "PIVA")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT piva_porciva, SUBSTR(CAST(piva_porciva AS character(5)),2,LENGTH(CAST(piva_porciva AS character(5)))-1) CONCAT '- ' CONCAT piva_decreto FROM piva order by piva_decreto");
                sels = "SELECT piva_porciva, SUBSTR(CAST(piva_porciva AS character(5)),2,LENGTH(CAST(piva_porciva AS character(5)))-1) CONCAT '- ' CONCAT piva_decreto FROM piva order by piva_decreto";
            }
            else if (tableName == "PLINEAITEM")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT RTRIM(plin_codigo) CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem ORDER BY plin_nombre");
                sels = "SELECT RTRIM(plin_codigo) CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem ORDER BY plin_nombre";
            }
            else if (tableName == "PARANCEL")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT para_codigo, para_codigo CONCAT '-' CONCAT para_nombre FROM parancel ORDER BY para_codigo ASC");
                sels = "SELECT para_codigo, para_codigo CONCAT '-' CONCAT para_nombre FROM parancel ORDER BY para_codigo ASC";
            }
            else if (tableName == "PESPACIONOMBRES")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT pesp_nombre FROM pespacionombres ORDER BY pesp_nombre ASC");
                sels = "SELECT pesp_nombre FROM pespacionombres ORDER BY pesp_nombre ASC";
            }
            else if (tableName == "MEMPLEADO")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT MEMP.memp_codiempl, MEMP.memp_codiempl CONCAT ' - ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNIT.MNIT_APELLIDO2,'') CONCAT ' ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_NOMBRE2,'') FROM dbxschema.mempleado MEMP, dbxschema.mnit MNIT WHERE MEMP.mnit_nit=MNIT.mnit_nit  ORDER BY MEMP.memp_codiempl ASC");
                sels = "SELECT MEMP.memp_codiempl, MEMP.memp_codiempl CONCAT ' - ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNIT.MNIT_APELLIDO2,'') CONCAT ' ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_NOMBRE2,'') FROM dbxschema.mempleado MEMP, dbxschema.mnit MNIT WHERE MEMP.mnit_nit=MNIT.mnit_nit  ORDER BY MEMP.memp_codiempl ASC";
                //tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT MEMP.memp_codiempl, MEMP.memp_codiempl CONCAT ' - ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNIT.MNIT_APELLIDO2,'') CONCAT ' ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_NOMBRE2,'') FROM dbxschema.mempleado MEMP, dbxschema.mnit MNIT WHERE MEMP.mnit_nit=MNIT.mnit_nit AND MEMP.TEST_ESTADO <> '4' ORDER BY MEMP.memp_codiempl ASC");
                //sels = "SELECT MEMP.memp_codiempl, MEMP.memp_codiempl CONCAT ' - ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNIT.MNIT_APELLIDO2,'') CONCAT ' ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_NOMBRE2,'') FROM dbxschema.mempleado MEMP, dbxschema.mnit MNIT WHERE MEMP.mnit_nit=MNIT.mnit_nit AND MEMP.TEST_ESTADO <> '4' ORDER BY MEMP.memp_codiempl ASC";
            }
            else if (tableName == "MITEMS")
            {
                string filtroMitems = filtro;
                if (filtroMitems.Length > 0)
                    filtroMitems = filtroMitems.Replace("WHERE", "AND");
                string stMitem = "SELECT MITE.mite_codigo,DBXSCHEMA.EDITARREFERENCIAS(MITE.mite_codigo,PLIN.plin_tipo) CONCAT ' - ' CONCAT MITE.mite_nombre FROM dbxschema.mitems MITE,dbxschema.plineaitem PLIN WHERE PLIN.plin_codigo=MITE.plin_codigo " + filtroMitems + " ";
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, stMitem);
                // AQUI FALTA EDITAR LAS REFERENCIAS DE ACUERDO A LA LINEA
                sels = "SELECT * FROM " + tableName + " " + filtro + " ORDER BY " + DBFunctions.SingleData("SELECT NAME FROM sysibm.syscolumns WHERE tbname='" + tableName + "' AND colno=1 ") + " ASC";
            }
            else if (tableName == "PTEMPARIO")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT ptem_operacion,ptem_operacion CONCAT ' - ' CONCAT ptem_descripcion FROM ptempario ORDER BY ptem_operacion");
                sels = "SELECT ptem_operacion,ptem_descripcion FROM dbxschema.ptempario";
            }
            else if (tableName == "PCENTROCOSTO")
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT PCEN_CODIGO, PCEN_NOMBRE FROM PCENTROCOSTO WHERE TIMP_CODIGO<>'N' ORDER BY PCEN_NOMBRE");
                sels = "SELECT * FROM PCENTROCOSTO WHERE TIMP_CODIGO<>'N' ORDER BY PCEN_NOMBRE";
            }
            else
            {
                //sels = "SELECT * FROM " + tableName + " " + filtro + " ORDER BY " + DBFunctions.SingleData("SELECT NAME FROM sysibm.syscolumns WHERE tbname='" + tableName + "' AND colno=1") + " ASC";
                sels = "SELECT * FROM " + tableName + " " + filtro + " ORDER BY " + DBFunctions.SingleData(SQLParser.GetColumnNameByTableIndex(tableName, "1")) + " ASC";

                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, sels);
            }
            DropDownList ddl = new DropDownList();
            ddl.ID = "control_" + i.ToString();
            try
            {
                if (tempDS.Tables[0].Rows.Count == 0)
                {
                    //ViewState["tbFaltantes"] += tableName + "\\n ";
                    Utils.MostrarAlerta(Response, "Falta parametrizar  la tabla  " + tableName + " por tanto, no se puede añadir un nuevo registro");
                    //btInsert.Visible = false;
                    btInsert.Enabled = false;
                }
                int nume = tempDS.Tables[0].Rows.Count;
                ddl.DataSource = tempDS.Tables[0];
                if (tableName != "PLINEAITEM")
                {
                    if (tempDS.Tables[0].PrimaryKey.Length > 0)
                        ddl.DataValueField = tempDS.Tables[0].PrimaryKey[0].ToString();
                    if (tempDS.Tables[0].Columns.Count > 1)
                        ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
                }
                else
                {
                    ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
                    ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
                }
                ddl.DataBind();
            }
            catch (Exception e)
            {
                lbInfo.Text += e.ToString() + "<br>" + DBFunctions.exceptions;
            }
            filtroSels = filtro;
            tempDS.Clear();
            fkCount++;
            return ddl;
        }

        protected DropDownList PutDropDownList(string tableName, string fieldName, string condicional)
        {
            DataSet tempDS = new DataSet();
            if (condicional == "")
            {
                if (tableName == "MFACTURAPROVEEDOR")
                {
                    tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT DISTINCT MFAC." + fieldName + ",MFAC." + fieldName + " CONCAT ' - ' CONCAT PDOC.pdoc_descripcion FROM " + tableName + " MFAC,pdocumento PDOC WHERE MFAC.pdoc_codiordepago=PDOC.pdoc_codigo ORDER BY " + fieldName + " ASC");
                    sels = "SELECT DISTINCT MFAC." + fieldName + ",MFAC." + fieldName + " CONCAT ' - ' CONCAT PDOC.pdoc_descripcion FROM " + tableName + " MFAC,pdocumento PDOC WHERE MFAC.pdoc_codiordepago=PDOC.pdoc_codigo ORDER BY " + fieldName + " ASC";
                }
                else
                {
                    tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT DISTINCT " + fieldName + " FROM " + tableName + " ORDER BY " + fieldName + " ASC");
                    sels = "SELECT DISTINCT " + fieldName + " FROM " + tableName + " ORDER BY " + fieldName + " ASC";
                }

            }
            else
            {
                tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT DISTINCT " + fieldName + " FROM " + tableName + " WHERE " + condicional + " ORDER BY " + fieldName + " ASC");
                sels = "SELECT DISTINCT " + fieldName + " FROM " + tableName + " WHERE " + condicional + " ORDER BY " + fieldName + " ASC";
            }

            DropDownList ddl = new DropDownList();
            ddl.ID = "control_" + i.ToString();
            try
            {
                if (tempDS.Tables[0].Columns.Count == 1)
                {
                    ddl.DataSource = tempDS.Tables[0];
                    ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
                    ddl.DataTextField = tempDS.Tables[0].Columns[0].ToString();
                    ddl.DataBind();
                }
                else
                {
                    ddl.DataSource = tempDS.Tables[0];
                    ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
                    ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
                    ddl.DataBind();
                }
            }
            catch (Exception e)
            {
                lbInfo.Text += "<br>" + e.ToString() + "<br>";
            }
            tempDS.Clear();
            fkCount++;
            return ddl;
        }

        protected void PutDropDownList(string idDdl, string idDdlConditional, string tableName, string fieldName, string fieldConditional, string typeConditional)
        {
            DatasToControls bind = new DatasToControls();
            string select = "SELECT " + fieldName + " FROM " + tableName + " WHERE " + fieldConditional + "=";
            if (typeConditional == "CHARACTER" || typeConditional == "VARCHAR" || typeConditional == "TIME" || typeConditional == "DATE")
                select += "'" + this.ValueDdl(idDdlConditional, tbForm) + "'";
            else
                select += "" + this.ValueDdl(idDdlConditional, tbForm) + "";
            for (int m = 0; m < tbForm.Rows.Count; m++)
            {
                if (tbForm.Rows[m].Cells.Count > 1)
                    if (tbForm.Rows[m].Cells[1].Controls[0].GetType() == typeof(DropDownList))
                    {
                        if (tbForm.Rows[m].Cells[1].Controls[0].ID.Trim() == idDdl)
                        {
                            bind.PutDatasIntoDropDownList(((DropDownList)tbForm.Rows[m].Cells[1].Controls[0]), select);
                            sels = select;

                        }
                    }
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
            phForm.Controls.Add(new LiteralControl("<br>"));
        }

        protected void GetDependencies()
        {
            //Llaves foraneas?
            if (Request.QueryString["fks"] != null)
            {
                string[] prts = Request.QueryString["fks"].ToString().Split('!');
                if (prts.Length == 2)
                {
                    arlFkNombres.AddRange(prts[0].Split('|'));
                    arlFkValores.AddRange(prts[1].Split('|'));
                }
            }

            //Llaves foraneas de insercion?
            if (Request.QueryString["fksI"] != null)
            {
                string[] prts = Request.QueryString["fksI"].ToString().Split('!');
                if (prts.Length == 2)
                {
                    arlFkINombres.AddRange(prts[0].Split('|'));
                    arlFkIValores.AddRange(prts[1].Split('|'));
                }
            }

        }

        protected void ShowDependencies()
        {
            DataSet dsHijas = new DataSet();
            string strLlavePadre;
            ArrayList arrPadre = new ArrayList();
            ArrayList arrHijo = new ArrayList();

            //Consultar tabla padre
            //strLlavePadre = DBFunctions.SingleData("SELECT COLNAMES FROM SYSIBM.SYSINDEXES WHERE TBNAME='" + table + "' AND UNIQUERULE='P'");
            strLlavePadre = DBFunctions.SingleData(SQLParser.GetPrimaryKeysString(table) );
            
            if (strLlavePadre.Length == 0) return;
            arrPadre.AddRange(strLlavePadre.Split('+'));

            GetDependencies(dsHijas, table);
            if (dsHijas.Tables[0].Rows.Count == 0)
                return;

            //Crear tabla
            string campos = Request.QueryString["pks"];
            string fksN = "", fksV = "", fks = "";
            DataTable dtReferencias = new DataTable();
            DataRow drN;
            dtReferencias.Columns.Add("TABLA", typeof(string));
            dtReferencias.Columns.Add("URL", typeof(string));

            int tP = arrPadre.Count;

            for (int nP = 0; nP < tP; nP++)
            {
                if (arrPadre[nP].ToString().Length > 0)
                {
                    fksN += arrPadre[nP].ToString() + "|";
                    fksV += ds.Tables[2].Rows[0][arrPadre[nP].ToString()] + "|";
                }
            }
            if (fksN.EndsWith("|")) fksN = fksN.Substring(0, fksN.Length - 1);
            if (fksV.EndsWith("|")) fksV = fksV.Substring(0, fksV.Length - 1);
            fks = fksN + "!" + fksV;

            //Agregar las hijas que incluyan la llave del padre en su llave primaria
            for (int nH = 0; nH < dsHijas.Tables[0].Rows.Count; nH++)
            {
                bool agregar = true;
                arrHijo.Clear();
                arrHijo.AddRange(dsHijas.Tables[0].Rows[nH]["LLAVE_PRIMARIA"].ToString().Split('+'));

                for (int nP = 0; nP < tP; nP++)
                {
                    if (arrPadre[nP].ToString().Length > 0)
                    {
                        if (!arrHijo.Contains(arrPadre[nP].ToString()))
                        {
                            agregar = false;
                            break;
                        }
                    }
                }

                if (agregar)
                {
                    drN = dtReferencias.NewRow();
                    drN["TABLA"] = dsHijas.Tables[0].Rows[nH]["DESCRIPCION"].ToString();
                    if (drN["TABLA"].ToString().Trim().Length == 0) drN["TABLA"] = dsHijas.Tables[0].Rows[nH]["NOMBRE"].ToString();
                    if (Request.QueryString["modal"] != null)
                        drN["URL"] = "table=" + dsHijas.Tables[0].Rows[nH]["NOMBRE"].ToString() + "&action=insert&cod=&name=&processTitle=&path=" + dsHijas.Tables[0].Rows[nH]["DESCRIPCION"].ToString() + "&fks=" + fks + "&tableRef=" + Request.QueryString["table"] + "&pksRef=" + Request.QueryString["pks"] + "&pathRef=" + Request.QueryString["path"] + "&modal=1";
                    else
                        drN["URL"] = "table=" + dsHijas.Tables[0].Rows[nH]["NOMBRE"].ToString() + "&action=insert&cod=&name=&processTitle=&path=" + dsHijas.Tables[0].Rows[nH]["DESCRIPCION"].ToString() + "&fks=" + fks + "&tableRef=" + Request.QueryString["table"] + "&pksRef=" + Request.QueryString["pks"] + "&pathRef=" + Request.QueryString["path"];
                    dtReferencias.Rows.Add(drN);
                }
            }
            if (dtReferencias.Rows.Count > 0)
            {
                dgrDependencias.DataSource = dtReferencias;
                dgrDependencias.DataBind();
                pnlDependencias.Visible = true;
            }
        }

        private void GetDependencies(DataSet dsH, string tblD)
        {
            //Consultar tablas hijas
            //DBFunctions.Request(dsH, IncludeSchema.NO,
            //    "SELECT DISTINCT ST.NAME AS NOMBRE, ST.REMARKS AS DESCRIPCION, SIH.COLNAMES AS LLAVE_PRIMARIA " +
            //    "FROM SYSIBM.SYSRELS SR, SYSIBM.SYSTABLES ST, SYSIBM.SYSINDEXES SIH " +
            //    "WHERE SR.CREATOR='DBXSCHEMA' AND SR.REFTBNAME='" + tblD + "' AND ST.NAME=SR.TBNAME AND " +
            //    "SIH.TBNAME=ST.NAME AND SIH.UNIQUERULE='P' AND " +
            //    "UPPER(ST.NAME) IN(SELECT UPPER(TABLA) FROM DBXSCHEMA.STABLA_DEPENDENCIAS);");
            DBFunctions.Request(dsH, IncludeSchema.NO, SQLParser.GetChildTables(tblD) );
        }

        protected void PopulateForm()
        {
            string valoresOriginales = "";
            bool bloquear;



            for (i = 0; i < ds.Tables[ds.Tables.Count - 1].Columns.Count; i++)
            {

                string p = Convert.ToString(ds.Tables[ds.Tables.Count - 1].Rows[0][i]);

                Control ctl = tbForm.FindControl("control_" + i.ToString());
                valoresOriginales += ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString() + ",";
                if (ctl != null)
                {
                    bloquear = (DBFunctions.SingleData("SELECT stab_modificable from STABLACAMPO where stab_modificable is not null AND stab_nombre='" + table + "' and stab_campo='" + ds.Tables[ds.Tables.Count - 1].Columns[i].ColumnName.ToString() + "';") == "N");
                    if (ctl.GetType().Equals(typeof(TextBox)))
                    {
                        if (ds.Tables[ds.Tables.Count - 1].Columns[i].DataType.ToString() == "System.DateTime" && ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString() != "")
                        {
                            string valordt = ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString();
                            string[] valordtsp = valordt.Split(' ');
                            if (valordtsp[1] == "12:00:00" && valordtsp[2] != null && valordtsp[2] == "AM")
                                valordt = Convert.ToDateTime(ds.Tables[ds.Tables.Count - 1].Rows[0][i]).ToString("yyyy-MM-dd");
                            else
                                valordt = Convert.ToDateTime(ds.Tables[ds.Tables.Count - 1].Rows[0][i]).ToString("yyyy-MM-dd HH:mm:ss");
                            try { ((TextBox)ctl).Text = valordt; }
                            catch { ((TextBox)ctl).Text = "";/*IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);((TextBox)ctl).Text = DateTime.Now.GetDateTimeFormats('d', culture)[0];*/}
                        }
                        else if (ds.Tables[ds.Tables.Count - 1].Columns[i].DataType.ToString() == "System.Decimal" || ds.Tables[ds.Tables.Count - 1].Columns[i].DataType.ToString() == "System.Double")
                        {
                            try
                            {
                                NumberFormatInfo info = new CultureInfo("en-US", false).NumberFormat;
                                string format = ds.Tables[0].Rows[i][5].ToString();
                                info.NumberDecimalDigits = Convert.ToInt32(format);
                                string usarDecimales = ConfigurationManager.AppSettings["UsarDecimales"];
                                if (usarDecimales == null || usarDecimales == "")
                                    usarDecimales = "false";
                                bool siUsaDecimales = Convert.ToBoolean(usarDecimales);

                                if (siUsaDecimales)
                                {
                                    ((TextBox)ctl).Text = Convert.ToDouble(ds.Tables[ds.Tables.Count - 1].Rows[0][i]).ToString("F3", info);
                                }
                                else
                                {
                                    ((TextBox)ctl).Text = Convert.ToDouble(ds.Tables[ds.Tables.Count - 1].Rows[0][i]).ToString("F2", info);
                                }

                            }
                            catch
                            {
                                ((TextBox)ctl).Text = "";
                            }
                        }
                        else if (ds.Tables[ds.Tables.Count - 1].Columns[i].DataType.ToString() == "System.DateTime" && ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString() == "" && ds.Tables[ds.Tables.Count - 1].Columns[i].AllowDBNull == false)
                        {
                            ((TextBox)ctl).Text = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            if (table == "MITEMS" && i == 0)
                            {
                                string codI = "";
                                Referencias.Editar(ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString(), ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + ds.Tables[ds.Tables.Count - 1].Rows[0][2].ToString().Trim() + "'"));
                                ((TextBox)ctl).Text = codI;
                            }
                            else
                                ((TextBox)ctl).Text = ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString();
                        }
                        if (bloquear)
                            ((TextBox)ctl).Attributes.Add("readonly", "true");
                    }
                    else if (ctl.GetType().Equals(typeof(DropDownList)))
                    {
                        try
                        {
                            if (table == "MITEMS" && i == 2)
                                ((DropDownList)ctl).SelectedValue = ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString().Trim() + "-" + DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString().Trim() + "'");
                            else
                            {
                                if (ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString() != "")
                                {
                                    ((DropDownList)ctl).SelectedValue = ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString();
                                    if (((DropDownList)ctl).AutoPostBack)
                                        this.ChangedFK((DropDownList)ctl, new EventArgs());
                                }
                                else
                                    ((DropDownList)ctl).SelectedIndex = ((DropDownList)ctl).Items.Count - 1;
                            }
                            if (bloquear)
                            {
                                ListItem lstDdl = ((DropDownList)ctl).SelectedItem;
                                ((DropDownList)ctl).Items.Clear();
                                ((DropDownList)ctl).Items.Add(lstDdl);
                            }
                        }
                        catch (Exception e)
                        {
                            //catch{
                            lbInfo.Text += "Advertencia: " + e.ToString();
                        }
                    }
                    else if (ctl.GetType().Equals(typeof(FileUpload)))
                    {
                        string data = ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString();

                        if (data != "")
                            if (data.Contains("."))
                            {
                                string[] split = data.Split('.');
                                string format = split[split.Length - 1].ToLower();

                                if (format == "jpg" || format == "gif" || format == "png")
                                {
                                    System.Web.UI.WebControls.Image img =
                                        (System.Web.UI.WebControls.Image)tbForm.FindControl("imgdata_" + i.ToString());

                                    img.Visible = true;
                                    img.ImageUrl = "../uploads/" + data;
                                }
                                else if (format == "pdf")
                                {
                                    System.Web.UI.WebControls.Image img =
                                        (System.Web.UI.WebControls.Image)tbForm.FindControl("imgdata_" + i.ToString());
                                    img.Visible = true;
                                    img.ImageUrl = "../img/pdf_icon.png";
                                    img.Width = 45;
                                    img.Height = 60;
                                    img.Style["cursor"] = "pointer";
                                    img.Attributes.Add("onclick", "window.open('../uploads/" + data + "', '_blank', 'toolbar=0');");
                                }
                            }
                    }
                }
                else
                    lbInfo.Text = "control_" + i.ToString();
            }
            if (valoresOriginales.EndsWith(","))
                valoresOriginales = valoresOriginales.Substring(0, valoresOriginales.Length - 1);
            ViewState["ORIGINALES"] = valoresOriginales;
            //REVISAR SI LA TABLA ES MITEMS, cambiar valor de codigo con Referencia.Editar
            //Referencias Items
            try
            {
                if (table.ToUpper().Trim() == "MITEMS")
                {
                    string itm = ((TextBox)tbForm.FindControl("control_0")).Text.Trim();
                    string bdga = ((DropDownList)tbForm.FindControl("control_2")).SelectedValue;
                    string edItm = "";
                    if (!Referencias.Editar(itm, ref edItm, bdga))
                    {
                        Utils.MostrarAlerta(Response, "Advertencia, el codigo del item no cumple con el formato de la bodega!");
                        edItm = itm;
                    }
                    ((TextBox)tbForm.FindControl("control_0")).Text = edItm;
                    //Response.Write("<script language='javascript'>alert('Item:"+itm+"   Bdga:"+bdga+"    NItem:"+edItm+"');</script>");
                }
            }
            catch (Exception e)
            {
                lbInfo.Text = e.ToString();
            }
        }

        protected void PopulateFormCopiar()
        {
            for (i = 0; i < ds.Tables[ds.Tables.Count - 1].Columns.Count; i++)
            {
                Control ctl = tbForm.FindControl("control_" + i.ToString());
                if (ctl != null)
                {
                    if (ctl.ToString() == "System.Web.UI.WebControls.TextBox")
                    {
                        if (ds.Tables[ds.Tables.Count - 1].Columns[i].DataType.ToString() == "System.DateTime" && ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString() != "")
                        {
                            string valordt = ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString();
                            string[] valordtsp = valordt.Split(' ');
                            if (valordtsp[1] == "12:00:00" && valordtsp[2] != null && valordtsp[2] == "AM")
                                valordt = Convert.ToDateTime(ds.Tables[ds.Tables.Count - 1].Rows[0][i]).ToString("yyyy-MM-dd");
                            else
                                valordt = Convert.ToDateTime(ds.Tables[ds.Tables.Count - 1].Rows[0][i]).ToString("yyyy-MM-dd HH:mm:ss");
                            try { ((TextBox)ctl).Text = valordt; }
                            catch { ((TextBox)ctl).Text = "";/*IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);((TextBox)ctl).Text = DateTime.Now.GetDateTimeFormats('d', culture)[0];*/}
                        }
                        else if (ds.Tables[ds.Tables.Count - 1].Columns[i].DataType.ToString() == "System.Decimal" || ds.Tables[ds.Tables.Count - 1].Columns[i].DataType.ToString() == "System.Double")
                        {
                            try
                            {
                                NumberFormatInfo info = new CultureInfo("en-US", false).NumberFormat;
                                string format = ds.Tables[0].Rows[i][5].ToString();
                                info.NumberDecimalDigits = Convert.ToInt32(format);
                                ((TextBox)ctl).Text = Convert.ToDouble(ds.Tables[ds.Tables.Count - 1].Rows[0][i]).ToString("F", info);
                            }
                            catch
                            {
                                ((TextBox)ctl).Text = "";
                            }
                        }
                        else
                        {
                            if (table == "MITEMS" && i == 0)
                            {
                                string codI = "";
                                DataSet dsprueba = ds;


                                Referencias.Editar(ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString(), ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + ds.Tables[ds.Tables.Count - 1].Rows[0][2].ToString().Trim() + "'"));
                                ((TextBox)ctl).Text = codI;
                            }
                            else
                                if (((TextBox)ctl).Text.Trim().Length == 0)
                                ((TextBox)ctl).Text = ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString();
                        }
                    }
                    else if (ctl.ToString() == "System.Web.UI.WebControls.DropDownList")
                    {
                        try
                        {
                            if (table == "MITEMS" && i == 2)
                                ((DropDownList)ctl).SelectedValue = ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString().Trim() + "-" + DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString().Trim() + "'");
                            else
                            {
                                if (ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString() != "")
                                    ((DropDownList)ctl).SelectedValue = ds.Tables[ds.Tables.Count - 1].Rows[0][i].ToString();
                                else
                                    ((DropDownList)ctl).SelectedIndex = ((DropDownList)ctl).Items.Count - 1;
                            }
                        }
                        catch (Exception e)
                        {
                            //catch{
                            lbInfo.Text += "Advertencia: " + e.ToString();
                        }
                    }
                }
                else
                    lbInfo.Text = "control_" + i.ToString();
            }
            //REVISAR SI LA TABLA ES MITEMS, cambiar valor de codigo con Referencia.Editar
            //Referencias Items
            try
            {
                if (table.ToUpper().Trim() == "MITEMS")
                {
                    string itm = ((TextBox)tbForm.FindControl("control_0")).Text.Trim();
                    string bdga = ((DropDownList)tbForm.FindControl("control_2")).SelectedValue;
                    string edItm = "";
                    if (!Referencias.Editar(itm, ref edItm, bdga))
                    {
                        Utils.MostrarAlerta(Response, "Advertencia, el codigo del item no cumple con el formato de la bodega!");
                        edItm = itm;
                    }
                    ((TextBox)tbForm.FindControl("control_0")).Text = edItm;
                    //Response.Write("<script language='javascript'>alert('Item:"+itm+"   Bdga:"+bdga+"    NItem:"+edItm+"');</script>");
                }
            }
            catch (Exception e)
            {
                lbInfo.Text = e.ToString();
            }
        }

        protected void PopulateFormNew()
        {
            DataSet dsBind = null;
            try
            {
                String sql = String.Format("SELECT stab_campo, " +
                 "       stab_binding " +
                 "FROM stablacampo " +
                 "WHERE stab_nombre = '{0}'", table);

                dsBind = new DataSet();             
                dsBind = DBFunctions.Request(dsBind, IncludeSchema.NO, sql);

                if (ViewState["hijaValorPK"] != null)
                {
                    DataRow rFK = dsBind.Tables[0].NewRow();
                    string[] sFK = ViewState["hijaValorPK"] as string[];
                    rFK[0] = sFK[0];
                    rFK[1] = sFK[1];
                    dsBind.Tables[0].Rows.Add(rFK);
                }

                if (dsBind.Tables[0].Rows.Count == 0) return;  //speeding up :3
            }
            catch { return; } // en caso de que los campos no esten en la BD

            try
            {
                for (int i = 0; i < ds.Tables[ds.Tables.Count - 1].Columns.Count; i++)
                {
                    Control ctl = tbForm.FindControl("control_" + i.ToString());

                    if (ctl != null)
                    {
                        string colName = ds.Tables[ds.Tables.Count - 1].Columns[i].ColumnName;

                        string bind = (from DataRow c in dsBind.Tables[0].Rows
                                       where c["STAB_CAMPO"].ToString() == colName
                                       select c["STAB_BINDING"].ToString()).FirstOrDefault();

                        if (bind == null || bind == "") continue;

                        string[] bind2 = bind.Split('.');
                        string result = bind;

                        if (bind2.Length == 2 &&
                            bind2[0] != "" && bind2[1] != "")
                        {
                            string bindTable = bind2[0];
                            string bindField = bind2[1];

                            string sql = String.Format("select {0} from {1}", bindField, bindTable);

                            if (bindTable.ToUpper() == "SUSUARIO")
                                sql += String.Format(" where susu_login='{0}'", HttpContext.Current.User.Identity.Name);

                            sql += " fetch first row only";
                            result = DBFunctions.SingleData(sql);
                        }

                        if (ctl.GetType().Equals(typeof(TextBox)))
                            ((TextBox)ctl).Text = result;

                        else if (ctl.GetType().Equals(typeof(DropDownList)))
                            ((DropDownList)ctl).SelectedValue = result;
                    }
                    else
                        lbInfo.Text = "control_" + i.ToString();
                }
            }
            catch (Exception e)
            {
                lbInfo.Text = String.Format("Error de formato de binding revise STABLACAMPO\n{0}", e.ToString());
            }
        }


        protected void InsertRecords(Object sender, EventArgs e)
         {
            NumberFormatInfo format = new System.Globalization.NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            format.NumberGroupSeparator = ",";

            //ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='" + table + "'");
            ds = DBFunctions.Request(ds, IncludeSchema.NO, SQLParser.GetTableStructureExtended(table) );
            ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM " + table + " FETCH FIRST 1 ROWS ONLY ");
            ArrayList arlCampos = new ArrayList();
            ArrayList arlValores = new ArrayList();

            //campos MNIT para estructura...
            string camposTablaMod = "";
            if (table == "MNIT")
            {
                camposTablaMod = @"(TNIT_TIPONIT,
                                MNIT_NIT,
                                MNIT_DIGITO,
                                MNIT_NOMBRES,
                                MNIT_NOMBRE2,
                                MNIT_APELLIDOS,
                                MNIT_APELLIDO2,
                                PCIU_CODIGOEXPNIT,
                                TNAC_TIPONACI,
                                MNIT_DIRECCION,
                                PCIU_CODIGO,
                                MNIT_TELEFONO,
                                MNIT_CELULAR,
                                MNIT_EMAIL,
                                MNIT_WEB,
                                TVIG_VIGENCIA,
                                TSOC_SOCIEDAD,
                                TREG_REGIIVA,
                                PSEC_ACTIVIDAD,
                                MNIT_REPRESENTANTE, 
                                MNIT_NITREPRESENTANTE";
                //if (ds.Tables[0].Rows.Count == 21)
                //{
                //    camposTablaMod += ",MNIT_REPRESENTANTE, MNIT_NITREPRESENTANTE";
                //}
                camposTablaMod += ")";
            }

            string sql = "INSERT INTO " + table + " " + camposTablaMod + " VALUES (";
            //Solo numeros en nits
            if (table.ToUpper().Trim() == "MNIT")
            {
                //string nNit = Request.Form["_ctl1:control_0"].Trim();
                //areglo para estructura de MNIT...
                string ntipoNit = Request.Form["_ctl1:control_0"].Trim();
                string nNit = Request.Form["_ctl1:control_1"].Trim();
                string nFijo = Request.Form["_ctl1:control_11"].Trim();
                string nMovil = Request.Form["_ctl1:control_12"].Trim();
                string email = Request.Form["_ctl1:control_13"].Trim();
                nFijo = nFijo.Replace(" ", "");
                nMovil = nMovil.Replace(" ", "");

                if (ntipoNit != "P") // PASAPORTE ES EL UNICO QUE PERMITE LETRAS
                {
                    foreach (char c in nNit)
                        if (!Char.IsDigit(c))
                        {
                            Utils.MostrarAlerta(Response, "El Nit sólo puede contener números!");
                            return;
                        }
                }
                foreach (char d in nFijo)
                    if (!Char.IsDigit(d))
                    {
                        Utils.MostrarAlerta(Response, "El Teléfono Fijo sólo puede contener números!");
                        return;
                    }
                foreach (char f in nMovil)
                    if (!Char.IsDigit(f))
                    {
                        Utils.MostrarAlerta(Response, "El Teléfono movil sólo puede contener números!");
                        return;
                    }

                if (nNit == "")
                {
                    Utils.MostrarAlerta(Response, "Debe llenar el campo de NIT!");
                    return;
                }
                if (nFijo == "")
                {
                    Utils.MostrarAlerta(Response, "Debe llenar el campo de Número Fijo y este debe contener sólo números");
                    return;
                }
                if (nMovil == "")
                {
                    Utils.MostrarAlerta(Response, "Debe llenar el campo Teléfono Móvil!");
                    return;
                }
                if (email == "" || !email.Contains("@"))
                {
                    Utils.MostrarAlerta(Response, "Debe llenar el campo Email con un correo válido");
                    return;
                }
            }
            //Referencias Items
            string nitm = "", updateValue, updatePost = "", updateField;
            int a = 0;

            if (table.ToUpper().Trim() == "MITEMS")
            {
                string itm = Request.Form["_ctl1:control_0"].Trim();
                string bdga = (Request.Form["_ctl1:control_2"].Trim().Split('-'))[1];
                if (!Referencias.Guardar(itm, ref nitm, bdga))
                {
                    Utils.MostrarAlerta(Response, "Codigo de item no valido!");
                    //btInsert.Visible = false;
                    return;
                }
                sql += "'" + nitm + "', ";
                a++;

                arlCampos.Add(ds.Tables[ds.Tables.Count - 1].Columns[0].ColumnName);
                arlValores.Add(nitm);
            }

            //Consultar acciones despues del insert
            DataSet dsAccionesPost = new DataSet();
            DBFunctions.Request(dsAccionesPost, IncludeSchema.NO,
                "select COALESCE(stab_sentenciapost,'') from stablacampo where stab_nombre='" + table + "' and stab_sentenciapost is not null;");
            if (dsAccionesPost.Tables.Count > 0)
                for (i = 0; i < dsAccionesPost.Tables[0].Rows.Count; i++)
                {
                    if (dsAccionesPost.Tables[0].Rows[i][0].ToString() != "")
                        updatePost += dsAccionesPost.Tables[0].Rows[i][0].ToString() + ";";
                }

            int lastTableIndex = ds.Tables.Count - 1;
            for (i = a; i < ds.Tables[lastTableIndex].Columns.Count; i++)
            {
                DataRow[] scRow = ds.Tables[0].Select("NAME = '" + ds.Tables[lastTableIndex].Columns[i].ColumnName + "'");
                updateValue = "";
                updateField = ds.Tables[lastTableIndex].Columns[i].ColumnName;
                if (scRow.Length > 0)
                {
                    string nomCampo = ds.Tables[ds.Tables.Count - 1].Columns[i].ColumnName;
                    bool isFileUpload = DBFunctions.SingleData(String.Format("select stab_esupload from stablacampo where stab_nombre='{0}' and stab_campo='{1}'", table, nomCampo)) == "S";

                    if (isFileUpload)
                    {
                        HttpPostedFile file = (HttpPostedFile)Request.Files[0];

                        string fileName = file.FileName;

                        if (fileName != "")
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                            updateValue = String.Format("'{0}'", fileName);

                            string ruta = DBFunctions.SingleData(String.Format("select stab_rutaupload from stablacampo where stab_nombre='{0}' and stab_campo='{1}'", table, nomCampo));
                            if (ruta == "") ruta = uploads;

                            file.SaveAs(ruta + fileName);
                            file.SaveAs(uploads + fileName);
                        }
                        else updateValue = "null";
                    }
                    else if (scRow[0][2].ToString().IndexOf("CHAR") != -1 || scRow[0][2].ToString().IndexOf("CLOB") != -1 || scRow[0][2].ToString().IndexOf("VAR") != -1 || scRow[0][2].ToString().IndexOf("VARCHAR") != -1 || scRow[0][2].ToString().IndexOf("TIME") != -1)
                    {
                        if (scRow[0][2].ToString().IndexOf("CLOB") != -1)
                            ViewState["CLOBINSERT"] = 1;
                        if (nomCampo == "PLIN_CODIGO" && table.ToUpper().Trim() == "MITEMS")
                            updateValue = "'" + (Request.Form["_ctl1:control_" + i.ToString().Trim() + ""].Split('-'))[0] + "'";
                        //sql += "'"+ (Request.Form["_ctl1:control_" + i.ToString().Trim() + ""].Split('-'))[0]+"',";
                        else
                        {
                            if (Request.Form["_ctl1:control_" + i.ToString().Trim() + ""] == "null")
                                updateValue = "" + Request.Form["_ctl1:control_" + i.ToString().Trim() + ""];
                            //sql += ""+ Request.Form["_ctl1:control_" + i.ToString().Trim() + ""]+", ";
                            else
                                try
                                {
                                    updateValue = ("'" + (Request.Form["_ctl1:control_" + i.ToString().Trim() + ""].Replace("'", "`")).TrimEnd() + "'");
                                }
                                catch (Exception ee)
                                {
                                    updateValue = "''";
                                }
                            //sql += "'"+ Request.Form["_ctl1:control_" + i.ToString().Trim() + ""]+"', ";
                        }
                    }
                    else if (scRow[0][2].ToString().IndexOf("DATE") != -1)
                    {
                        if (Request.Form["_ctl1:control_" + i.ToString() + ""] != "")
                        {
                            string[] splitDate = Request.Form["_ctl1:control_" + i.ToString() + ""].Split('-');
                            DateTime dt = new DateTime(Convert.ToInt16(splitDate[0]), Convert.ToInt16(splitDate[1]), Convert.ToInt16(splitDate[2]));
                            updateValue = "'" + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + "'";
                            //sql+="'"+dt.Year.ToString()+"-"+dt.Month.ToString()+"-"+dt.Day.ToString()+"', ";
                        }
                        else
                            updateValue = "null";
                        //sql+="null, ";
                    }
                    else if (scRow[0][2].ToString().IndexOf("INT") != -1)
                    {
                        if (Request.Form["_ctl1:control_" + i.ToString().Trim() + ""] != "")
                            updateValue = Request.Form["_ctl1:control_" + i.ToString().Trim() + ""];
                        //sql+=Request.Form["_ctl1:control_" + i.ToString().Trim() + ""]+", ";
                        else
                            updateValue = "null";
                        //sql+="null, ";
                    }
                    else if ((scRow[0][2].ToString().IndexOf("DECIMAL") != -1) || (scRow[0][2].ToString().IndexOf("DOUBLE") != -1))
                    {
                        if (Request.Form["_ctl1:control_" + i.ToString().Trim() + ""] != "" && Request.Form["_ctl1:control_" + i.ToString().Trim() + ""] != "null")
                            updateValue = Convert.ToDouble(Request.Form["_ctl1:control_" + i.ToString().Trim() + ""].Replace(",", ""), format).ToString();

                        //sql+=Convert.ToDouble(Request.Form["_ctl1:control_" + i.ToString().Trim() + ""]).ToString()+", ";
                        else
                            updateValue = "null";
                        //sql+="null, ";
                    }
                    if (updateValue.Length > 0) sql += updateValue + ", ";
                    updatePost = updatePost.Replace("@" + updateField + "@", updateValue);
                }

                // Aqui valida que los campos updataField ó updataValue NO tengan las palabras DELETE UPDATE DROP TRUNCATE ??
                if (updateField.ToUpper().Contains("DELETE ") == true || updateField.ToUpper().Contains("UPDATE ") == true || updateField.ToUpper().Contains("DROP ") == true || updateField.ToUpper().Contains("TRUNCATE ") == true
                || updateValue.ToUpper().Contains("DELETE ") == true || updateValue.ToUpper().Contains("UPDATE ") == true || updateValue.ToUpper().Contains("DROP ") == true || updateValue.ToUpper().Contains("TRUNCATE ") == true
                    )
                {
                    Utils.MostrarAlerta(Response, "Error de integridad, favor informar a SISTEMAS por violación de seguridad !");
                    lbInfo.Text = "Error de integridad, favor informar a SISTEMAS por violación de seguridad !.<br>";
                    btInsert.Visible = false;
                    return;
                }
                else
                {
                    arlCampos.Add(updateField);
                    arlValores.Add(updateValue);
                }
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += ");";

            // validacion de MCUENTA
            bool errorCampo = false;
            string detalleError = "";

            if (table == "MCUENTA")
            {
                validar_mcuenta(arlValores, ref errorCampo, ref detalleError);
                if (errorCampo)
                {
                    Utils.MostrarAlerta(Response, detalleError);
                    lbInfo.Text = "Por Favor verifique !.<br>";
                    return;
                }
            }

            ArrayList sqls = new ArrayList();
            sqls.Add(sql);
            if (updatePost.Length > 0)
                sqls.Add(updatePost);


            //Guardar historial Creación Registro

            string valoresCreados = "";
            for (int cT = 0; cT < arlValores.Count; cT++)
                valoresCreados += arlValores[cT].ToString() + ",";
            if (valoresCreados.EndsWith(",")) valoresCreados = valoresCreados.Substring(0, valoresCreados.Length - 1);
            valoresCreados = valoresCreados.Replace("'", "");
            if (valoresCreados.Length > 0 && ViewState["CLOBINSERT"] == null)
                sqls.Add("INSERT INTO DBXSCHEMA.MHISTORIAL_CAMBIOS VALUES(DEFAULT,'" + table + "','I','" + valoresCreados + "','" + HttpContext.Current.User.Identity.Name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');");
            else
                sqls.Add("INSERT INTO DBXSCHEMA.MHISTORIAL_CAMBIOS VALUES(DEFAULT,'" + table + "','I','CLOB','" + HttpContext.Current.User.Identity.Name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');");

            string fks = "";
            if (Request.QueryString["fks"] != null) fks = Request.QueryString["fks"];
            if (!DBFunctions.Transaction(sqls))
            {
                if (DBFunctions.exceptions.IndexOf("SQL0803N") != -1)
                    //Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks="+pksImp.Replace("*","'")+"&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + Request.QueryString["processTitle"]+"&path="+Request.QueryString["path"]+"&reg=0&imp=N&val=Campo Repetido");
                    Utils.MostrarAlerta(Response, "Uno o mas valores de la sentencia de inserción no son validos \\nporque estan restringidos por una llave primaria o una llave única");
                else
                {
                    //Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks="+Request.QueryString["pks"]+"&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + Request.QueryString["processTitle"]+"&path="+Request.QueryString["path"]+"&reg=0&imp=N&val="+DBFunctions.exceptions.Substring(DBFunctions.exceptions.IndexOf("ERROR"),DBFunctions.exceptions.Substring(DBFunctions.exceptions.IndexOf("ERROR")).IndexOf("SQLSTATE")).Replace("\"","*")+"");
                    //Utils.MostrarAlerta(Response, "Ocurrio un error desconocido");
                    lbInfo.Text = "Error " + DBFunctions.exceptions;
                    return;
                }
            }
            else
            {
                QueryCache.removeData(table);
                //Redireccionar a insercion del primer hijo que encuentre
                #region Insertar Hijo si existe
                /*DataSet dsDep=new DataSet();
				GetDependencies(dsDep,table);
				if(dsDep.Tables[0].Rows.Count>0)
				{
					string strLlavePadre;
					ArrayList arrPadre=new ArrayList();
					//Consultar tabla padre
					strLlavePadre=DBFunctions.SingleData(
						"SELECT COLNAMES FROM SYSIBM.SYSINDEXES WHERE TBNAME='"+table+"' AND UNIQUERULE='P'");
					if(strLlavePadre.Length==0) return;
					arrPadre.AddRange(strLlavePadre.Split('+'));
					string fksN="",fksV="",fksA="";
					int tP=arrPadre.Count;
					for(int nP=0;nP<tP;nP++)
					{
						if(arrPadre[nP].ToString().Length>0)
						{
							if(arlValores[arlCampos.IndexOf(arrPadre[nP].ToString())].ToString().Equals("DEFAULT"))
								arlValores[arlCampos.IndexOf(arrPadre[nP].ToString())]=
									DBFunctions.SingleData("SELECT MAX("+arrPadre[nP].ToString()+") "+
									"FROM "+table+";");
							fksN+=arrPadre[nP].ToString()+"|";
							fksV+=arlValores[arlCampos.IndexOf(arrPadre[nP].ToString())].ToString()+"|";
						}
					}
					if(fksN.EndsWith("|"))fksN=fksN.Substring(0,fksN.Length-1);
					if(fksV.EndsWith("|"))fksV=fksV.Substring(0,fksV.Length-1);
					fksA=fksN+"!"+fksV;
					string pk="",valCol="";
					DataSet dsPK=new DataSet();
					DBFunctions.Request(dsPK, IncludeSchema.YES,"SELECT * FROM "+table+" FETCH FIRST 1 ROWS ONLY;");
					for(i=0; i<dsPK.Tables[0].PrimaryKey.Length; i++)
					{
						valCol=arlValores[arlCampos.IndexOf(dsPK.Tables[0].PrimaryKey[i].ColumnName)].ToString();
						if(dsPK.Tables[0].PrimaryKey[i].DataType.ToString()=="System.String")
							pk += dsPK.Tables[0].PrimaryKey[i].ColumnName + "='" +  valCol + "'"; 
						else
							pk += dsPK.Tables[0].PrimaryKey[i].ColumnName + "=" + valCol;
						if(i != dsPK.Tables[0].PrimaryKey.Length-1)
							pk += " AND ";
					}
					if(dsDep.Tables[0].Rows.Count>0)
					{
						string urlH="../aspx/AMS.Web.Index.aspx?process=DBManager.Inserts&table="+
							dsDep.Tables[0].Rows[0]["NOMBRE"].ToString()+
							"&action=insert&cod=&name=&processTitle=&path="+dsDep.Tables[0].Rows[0]["DESCRIPCION"].ToString()+
							"&fks="+fksA+"&tableRef="+table+"&pksRef="+pk+
							"&pathRef="+Request.QueryString["path"];
						Response.Redirect(urlH);
					}
				}*/
                #endregion

                string pk = "", valCol = "";
                DataSet dsPK = new DataSet();

                if (Request.QueryString["tableRef"] != null)
                {
                    table = Request.QueryString["tableRef"];
                }

                //Areglo para estructura de mnit...
                if (table == "MNIT")
                {
                    ArrayList campoAux = (ArrayList)arlCampos.Clone();
                    arlCampos[0] = campoAux[6];
                    arlCampos[1] = campoAux[0];
                    arlCampos[2] = campoAux[1];
                    arlCampos[3] = campoAux[2];
                    arlCampos[4] = campoAux[3];
                    arlCampos[5] = campoAux[4];
                    arlCampos[6] = campoAux[5];
                }

                DBFunctions.Request(dsPK, IncludeSchema.YES, "SELECT * FROM " + table + " FETCH FIRST 1 ROWS ONLY;");
                for (i = 0; i < dsPK.Tables[0].PrimaryKey.Length; i++)
                {
                    valCol = arlValores[arlCampos.IndexOf(dsPK.Tables[0].PrimaryKey[i].ColumnName)].ToString();
                    if(valCol == "DEFAULT")
                    {
                        valCol = DBFunctions.SingleData("SELECT " + dsPK.Tables[0].PrimaryKey[i].ColumnName + " FROM " + table +
                                               " order  by " + dsPK.Tables[0].PrimaryKey[i].ColumnName + " desc FETCH FIRST 1 ROWS ONLY ;");
                    }
                    if (dsPK.Tables[0].PrimaryKey[i].DataType.ToString() == "System.String")
                    {
                        if (valCol.IndexOf("'") == -1)
                        {
                            valCol = "'" + valCol + "'";
                        }
                        valCol = valCol.Replace("+", "'\'");
                        pk += dsPK.Tables[0].PrimaryKey[i].ColumnName + "=" + valCol;
                    }
                    else
                        pk += dsPK.Tables[0].PrimaryKey[i].ColumnName + "=" + valCol;
                    if (i != dsPK.Tables[0].PrimaryKey.Length - 1)
                        pk += " AND ";
                }

                DataSet dsDep = new DataSet();
                GetDependencies(dsDep, table);

                if (Request.QueryString["pksRef"] != null || pk == "")
                {
                    pk = Request.QueryString["pksRef"];
                }


                if (Request.QueryString["modal"] != null) // || (dsDep.Tables[0].Rows.Count > 0 || Request.QueryString["tableRef"] != null))
                {
                    if (Request.QueryString["fks"] == null)
                    { 
                        if (Request.QueryString["modal"] == "1")
                        Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + pk + "&path=" + Request.QueryString["pathRef"] + "&modal=1");
                        else if (Request.QueryString["modal"] == "2")
                            RegresarModal(null, null);
                        //Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + pk + "&path=" + Request.QueryString["pathRef"] + "&modal=2");
                    }
                    else
                    {
                        string[] datos = Request.QueryString["fks"].ToString().Split('!');
                        string codigo = datos[1].Trim(); 
                        Response.Redirect("AMS.Web.ModalDialog.aspx?Vals=" + codigo + "&params=" + Request.QueryString["sql"] + "&Ins=1&Reg=1");
                    }
                }

                //else if (Request.QueryString["modal"] != null)
                //    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=insert&pks=" + pk + "&path=" + Request.QueryString["pathRef"] + "&modal=1");


                if (dsDep.Tables[0].Rows.Count > 0 || Request.QueryString["tableRef"] != null)
                    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + pk + "&path=" + Request.QueryString["pathRef"]);
                else
                    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=insert&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + Request.QueryString["processTitle"] + "&path=" + Request.QueryString["path"] + "&reg=2&imp=Y&pks=" + pk);


                //if (fks.Length == 0)
                //{
                //    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=insert&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + Request.QueryString["processTitle"] + "&path=" + Request.QueryString["path"] + "&reg=2&imp=Y&pks=" + pk);
                //}
                //else
                //{
                //    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + Request.QueryString["tableRef"] + "&action=update&pks=" + pk + "&path=" + Request.QueryString["pathRef"]);
                //}
            }

            //Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=insert");
        }

        protected void CancelReturn(Object sender, EventArgs e)
        {
            if (Request.QueryString["modal"] != null)
                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + Request.QueryString["tableRef"] + "&action=update&pks=" + Request.QueryString["pksRef"] + "&path=" + Request.QueryString["pathRef"] + "&modal=1");
            else
                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + Request.QueryString["tableRef"] + "&action=update&pks=" + Request.QueryString["pksRef"] + "&path=" + Request.QueryString["pathRef"]);
        }

        protected void RegresarModal(Object sender, EventArgs e)
        {
            DataSet dsa = (DataSet)ViewState["updatedRow"];
            string codigo = "";
            table = Request.QueryString["table"];
            string[] campos = null;

            if (Request.QueryString["campos"] != null)
            {
                campos = Request.QueryString["campos"].ToString().Split(',');
                for (int k = 0; k < campos.Length - 1; k++)
                {
                    codigo += dsa.Tables[2].Rows[0][campos[k]].ToString() + "*";
                }
                codigo += ";";
                codigo = codigo.Replace("*;", "");
            }
            else
            {
                try
                {
                    if (table == "MNIT")
                    {
                        codigo += dsa.Tables[2].Rows[0][1].ToString() + "*";
                        codigo += dsa.Tables[2].Rows[0][3].ToString() + " " + dsa.Tables[2].Rows[0][4].ToString() + " " + dsa.Tables[2].Rows[0][5].ToString();
                    }
                    else
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            codigo += dsa.Tables[2].Rows[0][k].ToString() + "*";
                        }
                        codigo += ";";
                        codigo = codigo.Replace("*;", "");
                    }
                }
                catch (Exception er)
                {
                }
            }

            if (Request.QueryString["modal"] == "2")
                Response.Redirect("AMS.Web.ModalDialogIns.aspx?Vals=" + codigo + "&params=" + Request.QueryString["sql"] + "&Ins=1&Reg=1&modal=2");
            else
                Response.Redirect("AMS.Web.ModalDialog.aspx?Vals=" + codigo + "&params=" + Request.QueryString["sql"] + "&Ins=1&Reg=1");
        }

        protected void UpdateRecord(Object sender, EventArgs e)
        {
            NumberFormatInfo format = new System.Globalization.NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            format.NumberGroupSeparator = ",";
            string valId = "";
            ArrayList arlValores = new ArrayList();

            //ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='" + table + "'");
            ds = DBFunctions.Request(ds, IncludeSchema.NO, SQLParser.GetTableStructureExtended(table) );
            
            string cadenaPks = Request.QueryString.ToString();


            string pks1 = ObtenerLlavesPrimarias(cadenaPks);

            if (pks1.IndexOf("PCAT_CODIGO", StringComparison.OrdinalIgnoreCase) != -1)
            {
                ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM " + table + " WHERE " + pks1);
                if (ds.Tables[4].Rows.Count == 0)
                {
                    ds.Tables.RemoveAt(4);
                    pks1 = pks1.Replace("+", " ");
                    ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM " + table + " WHERE " + pks1);
                }
            }
            else
            {
                //campos MNIT para estructura...
                string camposTablaMod = "*";
                if (table == "MNIT")
                {
                    camposTablaMod = @"TNIT_TIPONIT,
                                        MNIT_NIT,
                                        MNIT_DIGITO,
                                        MNIT_NOMBRES,
                                        MNIT_NOMBRE2,
                                        MNIT_APELLIDOS,
                                        MNIT_APELLIDO2,
                                        PCIU_CODIGOEXPNIT,
                                        TNAC_TIPONACI,
                                        MNIT_DIRECCION,
                                        PCIU_CODIGO,
                                        MNIT_TELEFONO,
                                        MNIT_CELULAR,
                                        MNIT_EMAIL,
                                        MNIT_WEB,
                                        TVIG_VIGENCIA,
                                        TSOC_SOCIEDAD,
                                        TREG_REGIIVA,
                                        PSEC_ACTIVIDAD,
                                        MNIT_REPRESENTANTE, 
                                        MNIT_NITREPRESENTANTE";

                    //if (ds.Tables[0].Rows.Count == 21)
                    //{
                    //    camposTablaMod += ",MNIT_REPRESENTANTE, MNIT_NITREPRESENTANTE";
                    //}
                }

                if (pks1.Substring(pks1.Length - 1, 1) == "+")
                    pks1 = pks1.Substring(0, pks1.Length - 1);
                ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT " + camposTablaMod + " FROM " + table + " WHERE " + pks1);
                if (ds.Tables[4].Rows.Count == 0)
                {
                    ds.Tables.RemoveAt(4);
                    pks1 = pks1.Replace("+", " ");
                    ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT " + camposTablaMod + " FROM " + table + " WHERE " + pks1);
                }
            }

            string sql0 = "UPDATE " + table + " SET ";
            string sql = "";
            string nitm = "";
            bool insItm = false;
            DataSet dsCondicionesE = new DataSet();

            if (table.ToUpper().Trim() == "MITEMS")
            {
                string itm = Request.Form["_ctl1:control_0"].Trim();
                string bdga = (Request.Form["_ctl1:control_2"].Trim().Split('-'))[1];
                if (!Referencias.Guardar(itm, ref nitm, bdga))
                {
                    Utils.MostrarAlerta(Response, "El codigo del item no cumple con el formato de la bodega!");
                    lbInfo.Text = "No se pudo actualizar el registro.<br>";
                    btUpdate.Visible = false;
                    return;
                }
                insItm = true;
            }


            int a = 0;
            if (insItm)
            {
                sql += "MITE_CODIGO='" + nitm + "', ";
                a++;
            }


            //Cargar condiciones existencia: deben retornar un valor
            DBFunctions.Request(dsCondicionesE, IncludeSchema.NO, "SELECT STAB_CAMPO, STAB_CONDEXISTE, STAB_ERRCONDEXISTE FROM STABLACAMPO WHERE STAB_CONDEXISTE IS NOT NULL aND STAB_ERRCONDEXISTE IS NOT NULL AND STAB_NOMBRE='" + table + "';");

            for (i = a; i < ds.Tables[ds.Tables.Count - 1].Columns.Count; i++)
            {
                string valor = "";
                if (Request.Form["_ctl1:control_" + i.ToString()] != null)
                    valor = Request.Form["_ctl1:control_" + i.ToString().Trim()];
                
                arlValores.Add(valor);

                //Actualizar condiciones de existencia con valores ingresados
                for (int ce = 0; ce < dsCondicionesE.Tables[0].Rows.Count; ce++)
                    dsCondicionesE.Tables[0].Rows[ce]["STAB_CONDEXISTE"] =
                        dsCondicionesE.Tables[0].Rows[ce]["STAB_CONDEXISTE"].ToString().Replace(
                            "@" + ds.Tables[ds.Tables.Count - 1].Columns[i].ColumnName + "@",
                            valor);

                if (!ds.Tables[ds.Tables.Count - 1].Columns[i].AutoIncrement)
                {
                    sql += ds.Tables[ds.Tables.Count - 1].Columns[i].ColumnName + " = ";

                    DataRow[] scRow = ds.Tables[0].Select("NAME = '" + ds.Tables[ds.Tables.Count - 1].Columns[i].ColumnName + "'");
                    if (scRow.Length > 0)
                    {
                        string nomCampo = ds.Tables[ds.Tables.Count - 1].Columns[i].ColumnName;

                        bool isFileUpload = DBFunctions.SingleData(String.Format("select stab_esupload from stablacampo where stab_nombre='{0}' and stab_campo='{1}'", table, nomCampo)) == "S";

                        if (isFileUpload)
                        {
                            HttpPostedFile file = (HttpPostedFile)Request.Files[0];

                            string fileName = file.FileName;

                            if (fileName != "")
                            {
                                fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                                sql += String.Format("'{0}', ", fileName);

                                string ruta = DBFunctions.SingleData(String.Format("select stab_rutaupload from stablacampo where stab_nombre='{0}' and stab_campo='{1}'", table, nomCampo));
                                if (ruta == "") ruta = uploads;

                                file.SaveAs(ruta + fileName);
                                file.SaveAs(uploads + fileName);
                            }
                            else sql = sql.Substring(0, sql.LastIndexOf(',') + 2);

                        }
                        else if (scRow[0][2].ToString().IndexOf("CHAR") != -1 || scRow[0][2].ToString().IndexOf("CLOB") != -1 || scRow[0][2].ToString().IndexOf("VAR") != -1 || scRow[0][2].ToString().IndexOf("TIME") != -1)
                        {
                            if (nomCampo == "MVFAC_CODIGO" && table.ToUpper().Trim() == "MVIPFACTURA")
                                valId = valor;

                            if (nomCampo == "PLIN_CODIGO" && table.ToUpper().Trim() == "MITEMS")
                                sql += "'" + (valor.Split('-'))[0] + "',";
                            else
                            {
                                if (valor == "null")
                                {
                                    sql += "" + valor + ", ";
                                }
                                else
                                    if (valor == "" && scRow[0][2].ToString().IndexOf("TIME") != -1)
                                    sql += "null, ";
                                else
                                {
                                    if(valor.Contains("@NOHTML")) //Limpiar codigo HTML
                                    {
                                        valor = LimpiarHTML(valor);
                                    }

                                    sql += "'" + valor.Replace("'", "`") + "', ";               
                                }
                            }
                        }
                        else if (scRow[0][2].ToString().IndexOf("DATE") != -1)
                        {
                            if (valor != "")
                            {
                                string[] splitDate = valor.Split('-');
                                DateTime dt = new DateTime(Convert.ToInt16(splitDate[0]), Convert.ToInt16(splitDate[1]), Convert.ToInt16(splitDate[2]));
                                sql += "'" + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + "', ";
                            }
                            else
                                sql += "null, ";
                        }
                        else if (scRow[0][2].ToString().IndexOf("INT") != -1)
                        {
                            if (valor != "")
                                sql += valor + ", ";
                            else
                                sql += "null, ";
                        }
                        else if ((scRow[0][2].ToString().IndexOf("DECIMAL") != -1) || (scRow[0][2].ToString().IndexOf("DOUBLE") != -1))
                        {
                            if (valor != "")
                                sql += Convert.ToDouble(valor, format).ToString() + ", ";
                            else
                                sql += "null, ";
                        }
                    }
                }
                else
                {
                    DataRow[] scRow = ds.Tables[0].Select("NAME = '" + ds.Tables[ds.Tables.Count - 1].Columns[i].ColumnName + "'");
                }
            }
            // Aqui valida que los campos NO tengan las palabras reservadas DELETE UPDATE DROP TRUNCATE !
            if (sql.ToUpper().Contains("DELETE ") == true || sql.ToUpper().Contains("UPDATE ") == true || sql.ToUpper().Contains("DROP ") == true || sql.ToUpper().Contains("TRUNCATE ") == true)
            {
                Utils.MostrarAlerta(Response, "Error de integridad, favor informar a SISTEMAS por violación de seguridad !");
                lbInfo.Text = "Error de integridad, favor informar a SISTEMAS por violación de seguridad !.<br>";
                btUpdate.Visible = false;
                return;
            }
            else
            {

                sql = sql.Substring(0, sql.Length - 2);
                sql = sql0 + sql + " WHERE " + pks1;
                //sql+=" WHERE "+Request.QueryString["pks"];
            }
            ArrayList sqls = new ArrayList();
            sqls.Add(sql);

            //Verificar que se cumplan las condiciones de existencia
            for (int ce = 0; ce < dsCondicionesE.Tables[0].Rows.Count; ce++)
                if (!DBFunctions.RecordExist(dsCondicionesE.Tables[0].Rows[ce]["STAB_CONDEXISTE"].ToString()))
                {
                    Utils.MostrarAlerta(Response, dsCondicionesE.Tables[0].Rows[ce]["STAB_ERRCONDEXISTE"].ToString());
                    lbInfo.Text = "No se pudo actualizar el registro.<br>";
                    btUpdate.Visible = false;
                    return;
                }

            // validacion de MCUENTA
            bool errorCampo = false;
            string detalleError = "";

            if (table == "MCUENTA")
            {
                validar_mcuenta(arlValores, ref errorCampo, ref detalleError);
                if (errorCampo)
                {
                    Utils.MostrarAlerta(Response, detalleError);
                    lbInfo.Text = "Por Favor verifique !.<br>";
                    return;
                }
            }

            //Guardar historial
            string user = HttpContext.Current.User.Identity.Name.ToString().ToLower();
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (ViewState["ORIGINALES"] != null)
            {
                if(ViewState["CLOB"] == "")
                    sqls.Add("INSERT INTO DBXSCHEMA.MHISTORIAL_CAMBIOS VALUES(DEFAULT,'" + table + "','U','" + ViewState["CLOB"] + "','" + user + "','" + now + "');");
                else
                    sqls.Add("INSERT INTO DBXSCHEMA.MHISTORIAL_CAMBIOS VALUES(DEFAULT,'" + table + "','U','" + ViewState["ORIGINALES"] + "','" + user + "','" + now + "');");
            }
            if (table.ToUpper().Equals("MVIPFACTURA"))
                sqls.Add(String.Format("INSERT INTO MHISTORIAL_FACTURA (FACTURA, OPERACION, SUSU_USUARIO, FECHA) " +
                                  "VALUES ('{0}', 'U', '{1}', '{2}')"
                                  , valId, user, now));

            string fks = "";
            if (Request.QueryString["fks"] != null) fks = Request.QueryString["fks"];
            if (!DBFunctions.Transaction(sqls))
            {
                if (DBFunctions.exceptions.IndexOf("SQL0803N") != -1)
                    //Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + Request.QueryString["pks"] + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + Request.QueryString["processTitle"] + "&path=" + Request.QueryString["path"] + "&reg=0&imp=N&val=Campo Repetido");
                    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + pks1 + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + Request.QueryString["processTitle"] + "&path=" + Request.QueryString["path"] + "&reg=0&imp=N&val=Campo Repetido");
                else
                {
                    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + pks1 + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + Request.QueryString["processTitle"] + "&path=" + Request.QueryString["path"] + "&reg=0&imp=N&val=Error al guardar. Verifique sus datos. Si esta actualizando el codigo, verifique que no este ligado a otros registros en el sistema.");
                    //Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + Request.QueryString["pks"] + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + Request.QueryString["processTitle"] + "&path=" + Request.QueryString["path"] + "&reg=0&imp=N&val=" + DBFunctions.exceptions.Substring(DBFunctions.exceptions.IndexOf("ERROR"), DBFunctions.exceptions.Substring(DBFunctions.exceptions.IndexOf("ERROR")).IndexOf("SQLSTATE")).Replace("\"", "*") + "");
                    lbInfo.Text = "Error " + DBFunctions.exceptions;
                }
            }
            else
            {
                QueryCache.removeData(table);

                if (Session["HREFRETURN"] != null)
                {
                    string retRef = Session["HREFRETURN"].ToString();
                    Session.Remove("HREFRETURN");
                    Response.Redirect(indexPage + "?" + retRef);

                }

                if (Request.QueryString["modal"] != null)
                    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + pks1 + "&path=" + Request.QueryString["pathRef"] + "&modal=1");

                if (fks.Length == 0)
                    Response.Redirect(indexPage + "?process=DBManager.Selects&ret=1&table=" + table + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + Request.QueryString["processTitle"] + "&path=" + Request.QueryString["path"] + "&reg=1&imp=N");
                else
                    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + Request.QueryString["tableRef"] + "&action=update&pks=" + Request.QueryString["pksRef"] + "&path=" + Request.QueryString["pathRef"]);
            }
            //Response.Redirect(indexPage + "?process=DBManager.Selects&table=" + table);
        }

        private void validar_mcuenta(ArrayList arlValores, ref bool errorCuenta, ref string detalleError)
        {
            string cuenta = arlValores[0].ToString().Trim().Replace("'", "");
            if (cuenta == "")
            {
                detalleError += "Falta del Código de la cuenta \\n";
                errorCuenta = true;
            }
            if (cuenta.Substring(0, 1) == "0" || cuenta.Length > 16)
            {
                detalleError += "Código de la cuenta no permitido \\n";
                errorCuenta = true;
            }
            string nombre = arlValores[1].ToString().Trim().Replace("'", "");
            if (nombre == "" || nombre.Length < 3)
            {
                detalleError += "Falta el nombre de la cuenta o no es válido \\n";
                errorCuenta = true;
            }
            if ((cuenta.Length == 1 && (arlValores[2].ToString() != "'1'" && arlValores[2].ToString() != "1")) ||
                (cuenta.Length == 2 && (arlValores[2].ToString() != "'2'" && arlValores[2].ToString() != "2")) ||
                (cuenta.Length == 4 && (arlValores[2].ToString() != "'3'" && arlValores[2].ToString() != "3")) ||
                (cuenta.Length == 6 && (arlValores[2].ToString() != "'4'" && arlValores[2].ToString() != "4")) ||
                (cuenta.Length == 8 && (arlValores[2].ToString() != "'5'" && arlValores[2].ToString() != "5")) ||
                (cuenta.Length == 10 && (arlValores[2].ToString() != "'6'" && arlValores[2].ToString() != "6")) ||
                (cuenta.Length == 12 && (arlValores[2].ToString() != "'7'" && arlValores[2].ToString() != "7")) ||
                (cuenta.Length == 14 && (arlValores[2].ToString() != "'8'" && arlValores[2].ToString() != "8"))
                )
            {
                detalleError += "Nivel de imputación errado \\n";
                errorCuenta = true;
            }
            if (arlValores[3].ToString().Replace("'", "") != "N" && cuenta.Length < 6)
            {
                detalleError += "Imputabilidad no es válida a este nivel \\n";
                errorCuenta = true;
            }
            if (arlValores[3].ToString().Replace("'", "") == "A" || arlValores[3].ToString().Replace("'", "") == "N")
            {
                string tipoCuenta = "Auxiliar más Libro Saldos de Terceros";
                if (arlValores[3].ToString().Replace("'", "") == "N")
                    tipoCuenta = "NO Imputable";
                DataSet DSsaldo = new DataSet();
                string saldo = @"SELECT * FROM (select PANO_ANO as ano, COALESCE(sum(msal_valodebi),0) as debito, COALESCE(sum(msal_valocred),0) as credito, COALESCE(sum(msal_niifdebi),0) as debitoniif, COALESCE(SUM(msal_niifcred),0) as CREDITOniif 
                                   from msaldocuenta MS, MCUENTA M 
                                  where M.MCUE_CODIPUC = MS.MCUE_CODIPUC 
                                    and ((TIMP_CODIGO <> 'A' and '" + arlValores[3].ToString() +"' = 'A') OR (TIMP_CODIGO <> 'N' and '"+ arlValores[3].ToString() +"' = 'N')) AND M.mcue_codipuc = '" + cuenta + @"'
                                  group by pano_ano
                                  order by 3) AS A WHERE DEBITO <> 0 OR CREDITO <> 0 OR DEBITONIIF <> 0 OR CREDITONIIF <> 0";
                DBFunctions.Request(DSsaldo, IncludeSchema.NO, saldo);
                if (DSsaldo.Tables.Count > 0)
                {
                    detalleError += "La cuenta NO PUEDE SER del tipo " + tipoCuenta + ", porque ";
                    for (int i = 0; i < DSsaldo.Tables[0].Rows.Count; i++)
                    { 
                        if (Convert.ToDouble(DSsaldo.Tables[0].Rows[i][1].ToString()) != 0 || Convert.ToDouble(DSsaldo.Tables[0].Rows[i][2].ToString()) != 0 || Convert.ToDouble(DSsaldo.Tables[0].Rows[i][3].ToString()) != 0 || Convert.ToDouble(DSsaldo.Tables[0].Rows[i][4].ToString()) != 0)
                        {
                            detalleError += "en el año " + DSsaldo.Tables[0].Rows[i][0].ToString() + " tiene saldo DEBITO FISCAL por " + DSsaldo.Tables[0].Rows[i][1].ToString() + ", CREDITO FISCAL por " + DSsaldo.Tables[0].Rows[i][2] + ", DEBITO NIIF por " + DSsaldo.Tables[0].Rows[i][3].ToString() + " y CREDITO NIIF por " + DSsaldo.Tables[0].Rows[i][4] + "  !!! \\n";
                            errorCuenta = true;
                        }
                    }
                }
            }

            if (arlValores[5].ToString().Replace("'", "") != "F" && arlValores[5].ToString().Replace("'", "") != "G" && arlValores[5].ToString().Replace("'", "") != "N")
            {
                detalleError += "Imputación de cuenta errada, no está definida como Fiscal o Genérica o NIIF \\n";
                errorCuenta = true;
            }
            if (arlValores[6].ToString().Replace("'", "") == "B" && Convert.ToDecimal(arlValores[7].ToString()) == 0)
            {
                detalleError += "La cuenta está definida como Si Pide Base Gravable y no le ha definido el PORCENTAJE de validación de la base \\n";
                errorCuenta = true;
            }
            if (arlValores[6].ToString().Replace("'", "") != "B" && Convert.ToDecimal(arlValores[7].ToString()) != 0)
            {
                detalleError += "La cuenta está definida como NO Pide Base Gravable y tiene definido un PORCENTAJE de validación de la base diferente a cero\\n";
                errorCuenta = true;
            }
            if ((arlValores[9].ToString().Replace("'", "") != "D") && (cuenta.Substring(0, 1) == "1" || cuenta.Substring(0, 1) == "5" || cuenta.Substring(0, 1) == "6" || cuenta.Substring(0, 1) == "7") ||
                (arlValores[9].ToString().Replace("'", "") != "C") && (cuenta.Substring(0, 1) == "2" || cuenta.Substring(0, 1) == "3" || cuenta.Substring(0, 1) == "4"))
            {
                detalleError += "Naturaleza errada de la cuenta \\n";
                errorCuenta = true;
            }
            if ((arlValores[10].ToString().Replace("'", "") != "R") && (cuenta.Substring(0, 1) == "1" || cuenta.Substring(0, 1) == "2" || cuenta.Substring(0, 1) == "3") ||
                (arlValores[10].ToString().Replace("'", "") != "N") && (cuenta.Substring(0, 1) == "4" || cuenta.Substring(0, 1) == "5" || cuenta.Substring(0, 1) == "6" || cuenta.Substring(0, 1) == "7") ||
                (arlValores[10].ToString().Replace("'", "") != "O") && (cuenta.Substring(0, 1) == "8" || cuenta.Substring(0, 1) == "9"))
            {
                detalleError += "Clase de la cuenta errada \\n";
                errorCuenta = true;
            }
            if (arlValores[12].ToString() == null && arlValores[12].ToString() == "null")
            {
                detalleError += "Falta definir el campo para el nombre equivalente en NIIF cuando aplique \\n";
                errorCuenta = true;
            }
            if (arlValores[13].ToString() != null && arlValores[13].ToString() != "null" && arlValores[13].ToString().Length > 2 && (arlValores[5].ToString() != "'F'" && arlValores[5].ToString() != "F"))
            {
                detalleError += "Código de cuenta equivalente errado, solo es permitido para cuentas Fiscales \\n";
                errorCuenta = true;
            }
            string cuentaPadre = cuenta.Substring(0, cuenta.ToString().Length - 1);
            if (cuenta.Length > 2)
                cuentaPadre = cuenta.Substring(0, cuenta.ToString().Length - 2);
            else
                if (cuenta.Length > 1)
                cuentaPadre = cuenta.Substring(0, cuenta.ToString().Length - 1);
            string imputable = DBFunctions.SingleData("SELECT coalesce(timp_codigo,'N') FROM MCUENTA WHERE MCUE_CODIPUC = '" + cuentaPadre + "' ");
            if (imputable != "N" && cuentaPadre.Length > 1)
            {
                detalleError += "Falta la cuenta Padre para esta cuenta o la cuenta padre NO esta definida como NO Imputable \\n";
                errorCuenta = true;
            }
            if (arlValores[5].ToString().Replace("'", "") == "F")
            {
                int registros = Convert.ToInt16(DBFunctions.SingleData("select count(*) from dcuenta where mcue_codipuc = '" + cuenta + "' and (dcue_NIIFdebi <> 0 OR dcue_NIIFcred <> 0)"));
                if (registros > 0)
                {
                    detalleError += "Esta cuenta NO puede ser de imputación SOLO FISCAL porque hay " + registros + " en el movimiento contable con imputación en las columnas de valores NIIF \\n";
                    errorCuenta = true;
                }
            }
            if (arlValores[5].ToString().Replace("'", "") == "N")
            {
                int registros = Convert.ToInt16(DBFunctions.SingleData("select count(*) from dcuenta where mcue_codipuc = '" + cuenta + "' and (dcue_VALOdebi <> 0 OR dcue_VALOcred <> 0)"));
                if (registros > 0)
                {
                    detalleError += "Esta cuenta NO puede ser de imputación SOLO NIIF porque hay " + registros + " en el movimiento contable con imputación en las columnas de valores FISCALES \\n";
                    errorCuenta = true;
                }
            }
        }

        protected bool ReviewPK(string colname)
        {
            bool find = false;
            DataSet ds1 = new DataSet();
            ds1 = DBFunctions.Request(ds1, IncludeSchema.YES, "SELECT * FROM " + table + "");
            if (ds1.Tables[0].PrimaryKey.Length > 0)
            {
                for (int k = 0; k < ds1.Tables[0].PrimaryKey.Length; k++)
                {
                    if (ds1.Tables[0].PrimaryKey[k].ColumnName == colname)
                    {
                        find = true;
                        break;
                    }
                }
            }
            return find;
        }

        protected string ReturnPK(string tableName)
        {
            string pk = "";
            DataSet ds1 = new DataSet();
            ds1 = DBFunctions.Request(ds1, IncludeSchema.YES, "SELECT * FROM " + table + "");
            if (ds1.Tables[0].PrimaryKey.Length > 0)
            {
                for (int k = 0; k < ds1.Tables[0].PrimaryKey.Length; k++)
                    pk += ds1.Tables[0].PrimaryKey[k].ColumnName + "*";
            }
            if (pk != "")
                pk = pk.Substring(0, pk.Length - 1);
            return pk;
        }

        protected string ValueDdl(string idDdl, Table tbl)
        {
            string valor = "";
            for (int m = 0; m < tbl.Rows.Count; m++)
            {
                //lbInfo.Text+="<br>"+tbl.Rows[m-1].Cells[1].Controls[0].ToString();
                if (tbl.Rows[m].Cells.Count > 1)
                    if (tbl.Rows[m].Cells[1].Controls[0].GetType() == typeof(DropDownList))
                    {
                        if (tbl.Rows[m].Cells[1].Controls[0].ID.Trim() == idDdl.Trim())
                            valor = ((DropDownList)tbl.Rows[m].Cells[1].Controls[0]).SelectedValue;
                    }
            }
            return valor;
        }

        protected void ChangedFK(Object Sender, EventArgs e)
        {
            this.FindDdlSecundary(((DropDownList)Sender).ID.Trim());
        }

        protected void FindDdlSecundary(string idDdlMain)
        {
            string idSecundaryControl = "";
            string[] sepMain1 = fksComps.Split('*');
            string tableMain = "", fieldMain = "", fieldCondition = "", typeCondition = "";
            for (int m = 0; m < sepMain1.Length - 1; m++)
            {
                string[] sepSec1 = sepMain1[m].Split('-');
                if (sepSec1[1].Trim() == idDdlMain)
                {
                    string[] sepMain2 = ddlSec.Split('*');
                    for (int k = 0; k < sepMain2.Length - 1; k++)
                    {
                        string[] sepSec2 = sepMain2[k].Split('-');
                        if (sepSec1[0].Trim() == sepSec2[0].Trim())
                        {
                            idSecundaryControl = sepSec2[1].Trim();
                            tableMain = sepSec2[2].Trim();
                            fieldMain = sepSec2[3].Trim();
                            fieldCondition = sepSec1[3].Trim();
                            typeCondition = sepSec1[4].Trim();
                        }
                    }
                }
            }
            this.PutDropDownList(idSecundaryControl, idDdlMain, tableMain, fieldMain, fieldCondition, typeCondition);
        }

        protected string LimpiarHTML(string valor)
        {
            valor = valor.Replace("<div>", " ").Replace("</div>", " ")
                          .Replace("<br>", " ").Replace("&nbsp;", " ")
                          .Replace("&lt;","<").Replace("&gt;",">");

            return valor;
        }

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
    }
}
