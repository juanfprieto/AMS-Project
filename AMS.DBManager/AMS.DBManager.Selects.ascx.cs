using System;
using System.IO;
using System.Text;
using System.Web.Mail;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using AMS.DB;
using AMS.Forms;
using AMS.Reportes;
using AMS.Documentos;
using AMS.Tools;

namespace AMS.DBManager
{
    public partial class Selects : System.Web.UI.UserControl
    {
        #region Controles, variables
        protected int i, j;
        protected string table;
        protected Table tabPreHeader;
        protected Label lbProcessTitle;
        protected DataColumn[] primaryKeys;
        protected OdbcDataReader dr;
        protected DataSet ds = new DataSet();
        protected int Tam;
        protected int pagSize;
        protected ScrollingGridDemo.ScrollingGrid sg1;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected AMS_Tools_Email miMail = new AMS_Tools_Email();
        #endregion Controles

        protected void Page_Load(object sender, EventArgs e)
        {
            //Ajax
            Ajax.Utility.RegisterTypeForAjax(typeof(Selects));

            lbInfo.Text = "";
            if (Request.QueryString["table"] != null)
            {
                table = Request.QueryString["table"];
                if (!IsPostBack)
                {

                    //Alertas
                    if (Request.QueryString["error"] != null)
                        Utils.MostrarAlerta(Response, "Problema detectado al editar tabla. Por favor contactar al administrador de sistema.");
                    if (Request.QueryString["borrarS"] != null && Request.QueryString["borrarS"] != "NO")
                        Session.Clear();
                    if (Request.QueryString["imp"] == "Y")
                        Response.Write("<script language='javascript' src='../js/AMS.Web.DialogBox.js' type='text/javascript'></script><script language:javascript>DialogBox('Desea Imprimir?','DBManager.Preview&table=" + table + "&pks=" + Request.QueryString["pksImp"] + "');</script>");
                    if (Request.QueryString["reg"] == "1")
                        Utils.MostrarAlerta(Response, "Registro Actualizado ..!!");
                    else if (Request.QueryString["reg"] == "0")
                    {
                        Utils.MostrarAlerta(Response, Request.QueryString["val"]);
                    }
                    //Busqueda/filtro
                    ViewState["SEARCH"] = "";
                    //tbWord.Text="%";
                    txtFilt.Text = "";
                    txtSort.Text = "";

                    //Pagina inicial
                    ViewState["PAGE_NUM"] = 1;

                    //Recuperar filtro
                    if (Request.QueryString["ret"] != null)
                        ReloadFilter(table);

                    if (ConfigurationManager.AppSettings["DBMNGR_Page_Size"] != null)
                        try { pagSize = Convert.ToInt16(ConfigurationManager.AppSettings["DBMNGR_PAGE_SIZE"]); }
                        catch { pagSize = 10; }
                    if (pagSize < 1)
                        pagSize = 10;
                    ViewState["PAGE_SIZE"] = pagSize;

                    //Estructura tabla
                    DataSet dsTable = (DataSet)Cache.Get("DT_COLUMNS_" + table);
                    if (dsTable == null)
                    {
                        dsTable = new DataSet();
                        //DBFunctions.Request(dsTable, IncludeSchema.NO, "SELECT name, remarks, colno, coltype FROM sysibm.syscolumns WHERE tbname='" + table + "' ORDER BY colno ASC");
                        DBFunctions.Request(dsTable, IncludeSchema.NO, SQLParser.GetTableStructure(table) );

                        //Cache.Add("DT_COLUMNS_"+table,dsTable,null, System.Web.Caching.Cache.NoAbsoluteExpiration,new TimeSpan(0, 10, 0));
                        Cache.Insert("DT_COLUMNS_" + table, dsTable, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero, CacheItemPriority.High, null);
                    }

                    //Tabla
                    ViewState["TABLA"] = dsTable;

                    //Campos consulta
                    string campos = "", coltype = "", camposAUX = "";
                    Boolean mitecodigo = false;

                    //llaves foraneas.
                    DataSet dsTableForeign = new DataSet();
                    //DBFunctions.Request(dsTableForeign, IncludeSchema.NO, "SELECT fkcolnames, reftbname, pkcolnames FROM sysibm.sysrels WHERE tbname='" + table.ToUpper().Trim() + "';");
                    DBFunctions.Request(dsTableForeign, IncludeSchema.NO, SQLParser.GetTableForeign(table) );

                    //llaves primarias.
                    DataSet dsTablePrimary = new DataSet();
                    //DBFunctions.Request(dsTablePrimary, IncludeSchema.NO, "SELECT NAME FROM SYSIBM.SYSCOLUMNS WHERE TBNAME = '" + table.ToUpper().Trim() + "' AND KEYSEQ > 0 ORDER BY KEYSEQ ASC ");
                    DBFunctions.Request(dsTablePrimary, IncludeSchema.NO, SQLParser.GetTablePrimary(table) );

                    for (int cN = 0; cN < dsTable.Tables[0].Rows.Count; cN++)
                    {
                        coltype = dsTable.Tables[0].Rows[cN][3].ToString().Trim();
                        String nomCampo = dsTable.Tables[0].Rows[cN][0].ToString().Trim();
                        if ((table.ToUpper().Trim() == "MITEMS" && cN == 0))
                        {
                            campos += "DBXSCHEMA.EDITARREFERENCIAS(" + dsTable.Tables[0].Rows[cN][0].ToString() + "," +
                                "DBXSCHEMA.obtener_tipo_linea(" + dsTable.Tables[0].Rows[cN + 2][0].ToString() + ")" +
                                ") AS " + dsTable.Tables[0].Rows[cN][0].ToString() + ",";
                            camposAUX = campos;
                        }
                        else if (nomCampo.ToUpper().Trim() == "MITE_CODIGO")
                        {
                            campos += "DBXSCHEMA.EDITARREFERENCIAS(H." + dsTable.Tables[0].Rows[cN][0].ToString() + "," +
                                "DBXSCHEMA.obtener_tipo_linea( (select m.plin_codigo from mitems as m where m.mite_codigo=H.MITE_CODIGO) )" +
                                ") AS " + dsTable.Tables[0].Rows[cN][0].ToString() + ",";
                            mitecodigo = true;
                            camposAUX = campos;
                        }
                        else if (mitecodigo == true)
                        {
                            campos += "H." + dsTable.Tables[0].Rows[cN][0].ToString() + ",";
                            camposAUX = campos;
                        }
                        else
                        {
                            string colForanea = "";
                            nomCampo = nomCampo.ToUpper().Trim();
                            string miniSelectForaneo = "";
                            DataRow[] fkRow = dsTableForeign.Tables[0].Select("FKCOLNAMES = ' " + nomCampo + "'");
                            ViewState["errorForanea"] = "0";

                            bool primaryCombinada = false;
                            for (int k = 0; k < dsTablePrimary.Tables[0].Rows.Count; k++)
                            {
                                if (nomCampo == dsTablePrimary.Tables[0].Rows[k][0].ToString())
                                {
                                    primaryCombinada = true;
                                    break;
                                }
                            }

                            if (fkRow.Length > 0 && (ViewState["errorForanea"] != null && ViewState["errorForanea"].ToString() == "0"))
                            {
                                if (fkRow[0][1].ToString() == "MNIT")
                                {
                                    miniSelectForaneo = "COALESCE(PT." + nomCampo + " || ' - ' || " + "(select NOMBRE from dbxschema.VMNIT where " + fkRow[0][2] + "= PT." + nomCampo + "),PT." + nomCampo + " CONCAT '---') AS " + nomCampo + " ";
                                }
                                else if (fkRow[0][1].ToString() == "MQUINCENAS")
                                {
                                    miniSelectForaneo = "COALESCE(PT." + nomCampo + " || ' - ' || " + "(select MQUI_ANOQUIN || ' - ' || MQUI_MESQUIN || ' - ' || MQUI_TPERNOMI from dbxschema.MQUINCENAS where " + fkRow[0][2] + "= PT.MQUI_CODIQUIN), ' ') AS " + nomCampo + " ";
                                }
                                else if (fkRow[0][1].ToString() == "MEMPLEADO")
                                {
                                    miniSelectForaneo = "COALESCE(PT." + nomCampo + " || ' - ' || " + "(select NOMBRE from dbxschema.VMNIT where MNIT_NIT= PT." + nomCampo + "),PT." + nomCampo + " CONCAT '---') AS " + nomCampo + " ";
                                    //miniSelectForaneo = "COALESCE(PT." + nomCampo + " || ' - ' || " + "(select NOMBRE from dbxschema.VMNIT_NOMBRE where MNIT_NIT= PT." + nomCampo + "),PT." + nomCampo + " CONCAT '---') AS " + nomCampo + " ";
                                }
                                else
                                {
                                    try
                                    {
                                        //colForanea = DBFunctions.SingleData("SELECT COLNAME FROM SYSCAT.COLUMNS WHERE TABNAME = '" + fkRow[0][1] + "' and COLNO = 1 order by colno;");
                                        colForanea = DBFunctions.SingleData( SQLParser.GetTableColumnByIndex(fkRow[0][1].ToString(), 1) );
                                    }
                                    catch (Exception er)
                                    {
                                        //colForanea = DBFunctions.SingleData("SELECT COLNAME FROM SYSCAT.COLUMNS WHERE TABNAME = '" + fkRow[0][1] + "' and COLNO = 0 order by colno;");
                                        colForanea = DBFunctions.SingleData(SQLParser.GetTableColumnByIndex(fkRow[0][1].ToString(), 0));
                                    }
                                    miniSelectForaneo = "COALESCE(PT." + nomCampo + " || ' - ' || " + "(select " + colForanea + " from dbxschema." + fkRow[0][1] + " where " + fkRow[0][2] + "= PT." + nomCampo + "), ' ') AS " + nomCampo + " ";
                                }
                            }

                            if (miniSelectForaneo != "" && table.ToUpper().Trim() != "MORDEN" && primaryCombinada == false) //NOTA: Esta condicion a MORDEN se ha asignado debido a incompatibilidades al traer las llaves foraneas para la descripcion.
                                campos += miniSelectForaneo + ",";
                            else
                            {
                                campos += dsTable.Tables[0].Rows[cN][0].ToString() + ",";
                            }

                            if (miniSelectForaneo != "" && table.ToUpper().Trim() != "MORDEN") //NOTA: Esta condicion a MORDEN se ha asignado debido a incompatibilidades al traer las llaves foraneas para la descripcion.
                                camposAUX += miniSelectForaneo + ",";
                            else
                                camposAUX += dsTable.Tables[0].Rows[cN][0].ToString() + ",";

                        }
                        if ( coltype.Equals("CLOB"))
                        {
                            ViewState["CLOBTABLA"] = table.ToUpper().Trim();
                            ViewState["CLOBCAMPO"] = dsTable.Tables[0].Rows[cN][0].ToString().Trim();
                            ViewState["CLOBPRIMA"] = dsTable.Tables[0].Rows[0][0].ToString().Trim();
                            ViewState["CLOBPOSICION"] = cN;
                        }
                          // if (coltype.Equals("CHAR") || coltype.Equals("CLOB") || coltype.Equals("LONGVAR") || coltype.Equals("VARCHAR"))
                          //	campos+=dsTable.Tables[0].Rows[cN][0].ToString()+",";
                          //else if(coltype.Equals("DECIMAL"))
                          //		campos+="REPLACE(LTRIM(REPLACE(CHAR("+dsTable.Tables[0].Rows[cN][0].ToString()+"), '0', ' ')), ' ', '0') AS "+dsTable.Tables[0].Rows[cN][0].ToString()+",";
                          //	else
                          //		campos+="RTRIM(CHAR("+dsTable.Tables[0].Rows[cN][0].ToString()+")) AS "+dsTable.Tables[0].Rows[cN][0].ToString()+",";
                    }
                    if (campos.EndsWith(","))
                        campos = campos.Substring(0, campos.Length - 1);
                    if (camposAUX.EndsWith(","))
                        camposAUX = camposAUX.Substring(0, camposAUX.Length - 1);

                    ViewState["CAMPOS"] = campos;
                    ViewState["CAMPOSAUX"] = camposAUX;
                    ViewState["MITEMCOD"] = mitecodigo;

                    //Columnas Filtro
                    ddlCols.DataSource = dsTable.Tables[0];
                    ddlCols.DataTextField = "remarks";
                    ddlCols.DataValueField = "name";
                    ddlCols.DataBind();

                    /*DataRow[] drCols=dsTable.Tables[0].Select("coltype='VARCHAR' or coltype='CHAR'");
					ddlCols.Items.Clear();
					for(int nc=0;nc<drCols.Length;nc++)
						ddlCols.Items.Add(new ListItem(drCols[nc]["remarks"].ToString(),drCols[nc]["name"].ToString()));*/

                    if (ddlCols.Items.Count > 0)
                    {
                        if (txtFilt.Text.Length == 0) txtFilt.Text = ddlCols.SelectedItem.Value;
                        if (txtSort.Text.Length == 0) txtSort.Text = ddlCols.SelectedItem.Value;
                        //if (txtSort.Text.Length == 0) txtSort.Text = ddlCols.Items[0].Value + "," + ddlCols.Items[1].Value;
                    }
                    CalcularTamano();
                    PrepareDataSource();
                    LoadData();
                    ArmarPaginador();
                }
                else
                {
                    pagSize = Convert.ToInt16(ViewState["PAGE_SIZE"]);
                    ArmarPaginador();
                }
            }
        }

        //Restablecer fltro
        private void ReloadFilter(string tablaA)
        {
            if (Session["DBMNGR_TABLE"] == null || !tablaA.ToUpper().Equals(Session["DBMNGR_TABLE"].ToString().ToUpper()))
                return;

            ViewState["PAGE_NUM"] = Convert.ToInt16(Session["DBMNGR_PAGE"]);
            txtSort.Text = Session["DBMNGR_SORT"].ToString();
            tbWord.Text = Session["DBMNGR_FILTER"].ToString();
            likestr.Text = tbWord.Text.Replace("%", "*");
            txtFilt.Text = Session["DBMNGR_COLUMN"].ToString();
            ViewState["SEARCH"] = Session["DBMNGR_SEARCH"].ToString();

        }

        //Calcular tamaño y paginas
        protected void CalcularTamano()
        {
            String condicion = ViewState["SEARCH"].ToString();
            if (condicion.IndexOf("'%'") != -1)
            {
                condicion = ";";
            }
            double tamano = Convert.ToDouble(DBFunctions.SingleData("SELECT COUNT(*) FROM " + table + " " + condicion));
            int totalP = (int)Math.Ceiling(tamano / pagSize);
            ViewState["TAMANO"] = tamano;
            ViewState["PAGINAS"] = totalP;
        }

        //Crea el paginador
        protected void ArmarPaginador()
        {
            double tamano = Convert.ToDouble(ViewState["TAMANO"]);
            int totalP = Convert.ToInt16(ViewState["PAGINAS"]);
            int pagAct = Convert.ToInt16(ViewState["PAGE_NUM"]);
            int pagIni = pagAct - 5;
            int pagFin = 1;

            if (pagAct > totalP) pagAct = 1;
            if (pagIni < 1) pagIni = 1;
            pagFin = pagAct + 5;
            if (pagFin > totalP) pagFin = totalP;

            plcPaginacionIS.Controls.Clear();
            plcPaginacionDS.Controls.Clear();
            plcPaginacionII.Controls.Clear();
            plcPaginacionDI.Controls.Clear();

            lblPaginaActual.Font.Size = FontUnit.Large;
            lblPaginaActual.Attributes.Add("class", "pagination");
            lblPaginaActual.Text = "&nbsp;" + pagAct + "&nbsp;";

            if (pagIni > 1)
            {
                AgregarPagina(1, plcPaginacionIS);
                plcPaginacionIS.Controls.Add(new LiteralControl("<a class='pagination'>... <a>"));
            }

            for (int n = pagIni; n < pagAct; n++)
                AgregarPagina(n, plcPaginacionIS);

            for (int n = pagAct + 1; n <= pagFin; n++)
                AgregarPagina(n, plcPaginacionDS);

            if (pagFin < totalP)
            {
                plcPaginacionDS.Controls.Add(new LiteralControl("<a class='pagination'>...<a>"));
                AgregarPagina(totalP, plcPaginacionDS);
            }

            //Naveg rapida menor
            int numA = (int)Math.Ceiling((double)(pagIni) / 6);
            if (numA > 1)
            {
                for (int n = 1; n <= 5; n++)
                {
                    AgregarPagina(1 + numA * n, plcPaginacionII);
                    plcPaginacionII.Controls.Add(new LiteralControl("&nbsp;"));
                }
            }

            //Naveg rapida mayor
            numA = (int)Math.Ceiling((double)(totalP - pagFin) / 6);
            if (numA > 1)
            {
                for (int n = 1; n <= 5; n++)
                {
                    plcPaginacionDI.Controls.Add(new LiteralControl("&nbsp;"));
                    AgregarPagina(pagFin + numA * n, plcPaginacionDI);
                }
            }

            lblResult.Text = ViewState["TAMANO"] + " filas encontradas (" + ViewState["PAGINAS"] + " paginas). Procesado " + DateTime.Now.ToString() + ".";
        }


        //Agregar pagina
        protected void AgregarPagina(int numPagina, PlaceHolder plhActual)
        {
            LinkButton hlPag = new LinkButton();
            hlPag.Text = numPagina.ToString();
            hlPag.Attributes.Add("class", "pagination");
            hlPag.Click += new System.EventHandler(this.PagerLinkButton_Click);
            if (plhActual.FindControl("cntPagina" + numPagina) != null)
                numPagina *= 10;
            hlPag.ID = "cntPagina" + numPagina;
            plhActual.Controls.Add(new LiteralControl("&nbsp;"));
            plhActual.Controls.Add(hlPag);
            plhActual.Controls.Add(new LiteralControl("&nbsp;"));
        }


        //Evento paginacion
        protected void PagerLinkButton_Click(Object sender, EventArgs e)
        {
            ViewState["PAGE_NUM"] = Convert.ToInt16(((LinkButton)sender).Text.Trim());
            PrepareDataSource();
            LoadData();
            ArmarPaginador();
        }


        //Carga informacion consultada
        protected void LoadData()
        {
            //Se reemplaza la tabla sin formato, por la tabla con formato. (esto para que pueda mostrar el formato sin perder llaves primarias y foraneas)
            ds.Clear();

            ViewState["AUX"] = 1;
            ds = DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTAAUX"].ToString());
            try
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ViewState["AUX"] = 0;
                    ds.Clear();
                    //ds.Reset();
                    DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTA"].ToString());
                }
            }
            catch
            {
                ViewState["AUX"] = 0;
                ds.Clear();
                //ds.Reset();
                DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTA"].ToString());
            }

            miMail.DsExcel = ds;
            miMail.DsExcel.DataSetName = Request.QueryString["table"];

            try
            {
                dgTable.Visible = true;
                dgTable.DataSource = ds.Tables[0];
                dgTable.DataBind();
                dgAux.DataSource = ds.Tables[0];
                dgAux.DataBind();
                ViewState["dataGrid"] = ds;
            }
            catch { }

            string[] pr = { " ", " " };
            tabPreHeader = new Table();
            Press frontEnd = new Press(ds, "Reporte de " + Request.QueryString["path"]);
            frontEnd.PreHeader(tabPreHeader, dgTable.Width, pr);
            StringBuilder SB = new StringBuilder();
            StringWriter SW = new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            tabPreHeader.RenderControl(htmlTW);
            dgAux.RenderControl(htmlTW);

            string strRep;
            strRep = SB.ToString();
            if (Request.QueryString["borrarS"] != null && Request.QueryString["borrarS"] != "NO")
                Session.Clear();
            Session["Rep"] = strRep;
            toolsHolder.Visible = true;
            dgAux.Visible = false;
            Tam = dgTable.Items.Count;

            if (dgTable.Items.Count == 0)
            {
                toolsHolder.Visible = false;
                btClasifica.Visible = false;
                dgTable.Visible = false;
            }
            else
            {
                btClasifica.Visible = true;
                toolsHolder.Visible = true;
            }

            //Guardar busqueda realizada
            Session["DBMNGR_PAGE"] = Convert.ToInt16(ViewState["PAGE_NUM"]);
            Session["DBMNGR_SORT"] = txtSort.Text;
            Session["DBMNGR_FILTER"] = tbWord.Text;
            Session["DBMNGR_COLUMN"] = txtFilt.Text;
            Session["DBMNGR_TABLE"] = Request.QueryString["table"];
            Session["DBMNGR_SEARCH"] = ViewState["SEARCH"].ToString();

        }


        //Crea un elemento de filtro segun su tipo de dato
        protected string GetFilter(string strFilter, string strLike)
        {
            string flt = "", coltype = "";
            DataSet dsTable = (DataSet)ViewState["TABLA"];
            DataRow[] colActual = dsTable.Tables[0].Select("name='" + strFilter + "'");
            if (colActual.Length > 0)
            {
                if (table.ToUpper().Trim() == "MITEMS" && strFilter.ToUpper() == "MITE_CODIGO")
                {

                    /*
                     * No han manera de conocer la linea del item en este punto... por lo que para
                     * lograr rehacer la referencia, se hara un where con las posibles combinaciones
                     * de referencias... o.O'
                     */
                    DataSet dsAux = new DataSet();
                    DBFunctions.Request(dsAux, IncludeSchema.NO, "SELECT TLIN_CODILINE FROM TLINEAITEM");
                    String aux;
                    {
                        flt = " WHERE UCASE(" + strFilter + ") LIKE '" + strLike + "' ";
                    }

                    /* 
                     * para no perder los comodines (%) se hacen banderas de inicio y fin... si hay comodines en
                     * algún lugar en el medio sería ineficiente volver a colocarlos, por lo de que se cambia el orden
                     * de la referencia...
                     */
                    bool beg = strLike[0] == '%',
                         end = strLike[strLike.Length - 1] == '%';
                    strLike = strLike.Replace("%", "");

                    for (int k = 0; k < dsAux.Tables[0].Rows.Count; k++)
                    {
                        aux = "";
                        if (Referencias.Guardar(strLike, ref aux, dsAux.Tables[0].Rows[k][0].ToString()))
                        {
                            if (beg) aux = "%" + aux;
                            if (end) aux += "%";

                            {
                                flt += "OR UCASE(" + strFilter + ") LIKE '" + aux + "' ";
                            }
                        }
                    }
                }
                else
                {
                    coltype = colActual[0][3].ToString().Trim();
                    if (coltype.Equals("CHAR") || coltype.Equals("CLOB") || coltype.Equals("LONGVAR") || coltype.Equals("VARCHAR"))
                    {

                        {
                            if (strFilter == "MNIT_NIT") // || strFilter == "MEMP_CODIEMPL")
                            {
                                flt = " WHERE UCASE(" + strFilter + ") in (select vm.mnit_nit from dbxschema.VMNIT vm where vm.nombre like '" + strLike + "' or vm.mnit_nit like '" + strLike + "')  ";
                            }
                            else
                            {
                                flt = " WHERE UCASE(" + strFilter + ") LIKE '" + strLike + "'";
                            }
                        }
                    }
                    else if (coltype.Equals("BIGINT") || coltype.Equals("INT") || coltype.Equals("DECIMAL") || coltype.Equals("DOUBLE") || coltype.Equals("INTEGER") || coltype.Equals("SMALLINT"))
                    {
                        try
                        {
                            //Double.Parse(strLike);
                            //{
                            //    flt = " WHERE " + strFilter + " = " + strLike + " ";
                            //}
                            flt = " WHERE " + strFilter + " LIKE '" + strLike + "' ";
                        }
                        catch { }
                    }
                    else
                        if (coltype.Equals("DATE") || coltype.Equals("TIME") || coltype.Equals("TIMESTMP"))
                    {
                        {
                            flt = " WHERE " + strFilter + " = '" + strLike + "'";
                        }
                    }
                    /*else
                        if(coltype.Equals("DECIMAL"))
                            flt=" WHERE REPLACE(LTRIM(REPLACE(CHAR("+strFilter+"), '0', ' ')), ' ', '0') LIKE '" + strLike + "'";
                        else 
                            flt=" WHERE UCASE(RTRIM(CHAR("+strFilter + "))) LIKE '" + strLike + "'";*/
                }
            }
            return (flt);
        }


        //Prepara consulta
        protected void PrepareDataSource()
        {
            string pkDatas = "";
            string sql;
            string sqlAUX;
            string sqlAUXExcel;
            string campos = ViewState["CAMPOS"].ToString();
            string camposAUX = ViewState["CAMPOSAUX"].ToString();
            string filtroAct = "";
            int pagInit = ((Convert.ToInt16(ViewState["PAGE_NUM"]) - 1) * pagSize) + 1;
            if ((Boolean)ViewState["MITEMCOD"] == false)
            {
                sql = "SELECT * FROM(SELECT rownumber() over() pag_num_db_mngr,tblAux.* FROM( SELECT " + campos + " FROM " + table + " PT ";
                sqlAUX = "SELECT * FROM(SELECT rownumber() over() pag_num_db_mngr,tblAux.* FROM( SELECT " + camposAUX + " FROM " + table + " PT "; // Hay que revisar por que cambia el value seleccionado en la tabla PDOCUMENTOHECHO
                //sqlAUX = "SELECT * FROM(SELECT rownumber() over() pag_num_db_mngr,tblAux.* FROM( SELECT " + campos + " FROM " + table + " H ";

            }
            else
            {
                campos = campos.Replace("PT.", "H.");
                camposAUX = camposAUX.Replace("PT.", "H.");
                sql = "SELECT * FROM(SELECT rownumber() over() pag_num_db_mngr,tblAux.* FROM( SELECT " + campos + " FROM " + table + " H ";
                sqlAUX = "SELECT * FROM(SELECT rownumber() over() pag_num_db_mngr,tblAux.* FROM( SELECT " + camposAUX + " FROM " + table + " H ";
               
            }

            if (txtFilt.Text.Length > 0 && tbWord.Text.Length > 0)
                filtroAct += GetFilter(txtFilt.Text, tbWord.Text.ToUpper());
            if (tbWord.Text.ToUpper().Equals("%"))
            {
                filtroAct = " ";
            }
            sql += filtroAct;
            sqlAUX += filtroAct;

            ViewState["SEARCH"] = filtroAct;

            //llaves primarias.
            DataSet dsTablePrimary = new DataSet();
            //DBFunctions.Request(dsTablePrimary, IncludeSchema.NO, "SELECT NAME FROM SYSIBM.SYSCOLUMNS WHERE TBNAME = '" + table.ToUpper().Trim() + "' AND KEYSEQ > 0 ORDER BY KEYSEQ ASC ");
            DBFunctions.Request(dsTablePrimary, IncludeSchema.NO, SQLParser.GetTablePrimary(table) );

            string pkFilter = " ";
            for (int k = 0; k < dsTablePrimary.Tables[0].Rows.Count; k++)
            {
                pkFilter += "," + dsTablePrimary.Tables[0].Rows[k][0].ToString();
            }

            if (txtSort.Text.Length > 0)
            {
                if ((Boolean)ViewState["MITEMCOD"] == false)
                {
                    if(pkFilter.Contains(txtSort.Text))
                    {
                        pkFilter = "";
                    }

                    sqlAUXExcel = sqlAUX + " ORDER BY " + txtSort.Text + ") as tblAux) as tblRes ;";
                    sql += " ORDER BY " + txtSort.Text + pkFilter + ") as tblAux) as tblRes where tblRes.pag_num_db_mngr>=" + pagInit + " fetch first " + pagSize + " rows only;";
                    sqlAUX += " ORDER BY " + txtSort.Text + pkFilter + ") as tblAux) as tblRes where tblRes.pag_num_db_mngr>=" + pagInit + " fetch first " + pagSize + " rows only;";
                    ViewState["CONSULTAExcel"] = sqlAUXExcel;
                }
                else
                {
                    sqlAUXExcel = sqlAUX + " ORDER BY " + txtSort.Text + ") as tblAux) as tblRes ;";
                    sql += " ORDER BY H." + txtSort.Text + ") as tblAux) as tblRes where tblRes.pag_num_db_mngr>=" + pagInit + " fetch first " + pagSize + " rows only;";
                    sqlAUX += " ORDER BY H." + txtSort.Text + ") as tblAux) as tblRes where tblRes.pag_num_db_mngr>=" + pagInit + " fetch first " + pagSize + " rows only;";
                    ViewState["CONSULTAExcel"] = sqlAUXExcel;
                }
            }
            ViewState["CONSULTA"] = sql;
            ViewState["CONSULTAAUX"] = sqlAUX;


            try
            {
                //Hago la consulta con los filtros escogidos
                ds = DBFunctions.Request(ds, IncludeSchema.YES, sql);
                //Asigno llave primaria, y si es MITEMS la fuerzo ya que la llave primaria es una columna calculada por una funcion
                if (table.ToUpper().Trim() == "MITEMS")
                {
                    DataColumn[] aux = new DataColumn[1];
                    aux[0] = ds.Tables[0].Columns[0];
                    ds.Tables[0].PrimaryKey = aux;
                }
                else if (ds.Tables[0].Columns[1].ColumnName.ToUpper().Trim() == "MITE_CODIGO")
                {
                    //Arreglo para tablas con MITE_CODIGO relacionado.
                    //Carga la tabla especifica, de manera normal, y mediante esta toma el 
                    //esquema para compararlo con la tabla obtendia con la funcion
                    //de esta manera se pueden seleccionar las llaves primarias
                    DataSet dsAux = new DataSet();
                    dsAux = DBFunctions.Request(dsAux, IncludeSchema.YES, "SELECT * FROM DBXSCHEMA." + table.ToUpper().Trim() + " fetch first 50 rows only;");
                    DataColumn[] dCol = new DataColumn[dsAux.Tables[0].PrimaryKey.Length];
                    int h = 0;
                    for (int j = 1; j < ds.Tables[0].Columns.Count && h < dsAux.Tables[0].PrimaryKey.Length; j++)
                    {
                        if (ds.Tables[0].Columns[j].ColumnName.ToUpper().Trim() == dsAux.Tables[0].PrimaryKey[h].ColumnName.ToUpper().Trim())
                        {
                            dCol[h] = ds.Tables[0].Columns[j];
                            h++;
                        }
                    }
                    ds.Tables[0].PrimaryKey = dCol;
                }

                primaryKeys = ds.Tables[0].PrimaryKey;
                //Si no hay llave primaria, pongo todas las columnas como parte de
                //la llave primaria y asigno ese arreglo como llave primaria del
                //datatable
                try
                {
                    if (ds.Tables[0].PrimaryKey.Length == 0)
                    {
                        DataColumn[] dCol = new DataColumn[ds.Tables[0].Columns.Count];
                        for (i = 0; i < ds.Tables[0].Columns.Count; i++)
                            dCol[i] = ds.Tables[0].Columns[i];
                        ds.Tables[0].PrimaryKey = dCol;
                    }
                }
                catch { };
                //Si la llave primaria tiene una sola columna, se la asigno al datagid
                //como llave primaria del mismo
                if (ds.Tables[0].PrimaryKey.Length == 1)
                    dgTable.DataKeyField = ds.Tables[0].PrimaryKey[0].ToString();
                //Si la llave primaria es de mas de un solo campo, creo un datacolumn
                //al cual a la propiedad expresion (la de filtrar) le paso las llaves
                //primarias y la agrego al datatable y al datagrid le digo q esa
                //columna va a ser la llave primaria
                if (ds.Tables[0].PrimaryKey.Length > 1)
                {
                    DataColumn dc = new DataColumn("primaryKey", typeof(string));
                    for (i = 0; i < ds.Tables[0].PrimaryKey.Length; i++)
                    {
                        pkDatas += ds.Tables[0].PrimaryKey[i];
                        if (i != ds.Tables[0].PrimaryKey.Length - 1)
                            pkDatas += " + '|' + ";
                    }
                    dc.Expression = pkDatas;
                    ds.Tables[0].Columns.Add(dc);
                    dgTable.DataKeyField = "primaryKey";
                }
                dgTable.AutoGenerateColumns = false;
                BuildDataGridColumns();
                //BuildDataGridColumnsHeader();
                toolsHolder.Visible = true;
                ViewState["TABLAPRIMARY"] = ds;
            }
            catch (Exception e)
            {
                ViewState["errorForanea"] = "1";
                lbError.Text += e.ToString();
                lbError.Text += DBFunctions.exceptions;
                //Response.Redirect(indexPage + "?process=DBManager.Selects&table=" + table );
            }
            /*			if(table.ToUpper().Trim()=="MITEMS")   //no funciona... ni idea porqué -_-' (asumo que uno no puede editar los datos del ds...) pero ahora está al momento de cargar las columnas ^_
                        {
                            string edItm="";
                            for(int n=0;n<ds.Tables[0].Rows.Count;n++)
                                if(Referencias.Editar(ds.Tables[0].Rows[n][0].ToString(),ref edItm,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+ds.Tables[0].Rows[n][2].ToString().Trim()+"'")))
                                    ds.Tables[0].Rows[n][0] = edItm;

                        }
                        else if(table.ToUpper().Trim()=="MPRECIOITEM")
                        {
                            string edItm="";
                            for(int n=0;n<ds.Tables[0].Rows.Count;n++)
                                if(Referencias.Editar(ds.Tables[0].Rows[n][0].ToString(),ref edItm,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem P,mitems M WHERE P.plin_codigo=M.plin_codigo AND M.mite_codigo='"+ds.Tables[0].Rows[n][0].ToString()+"'")))
                                    ds.Tables[0].Rows[n][0] = edItm;
                        }
            */
        }


        //Agregar columnas al datagrid con descripciones correspondientes
        protected void BuildDataGridColumns()
        {
            string names = "";
            DataRow[] drsNames;
            //DbFunctions dbf = new DbFunctions();
            for (i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                BoundColumn bc = new BoundColumn();
                bc.DataField = ds.Tables[0].Columns[i].ColumnName;
                if (ds.Tables[0].Columns[i].DataType.ToString() == "System.DateTime")
                    bc.DataFormatString = "{0:dd/MM/yyyy}";
                //lbInfo.Text += ds.Tables[0].Columns[i].ColumnName + ":::" + ds.Tables[0].Columns[i].DataType.ToString() + "<br>";
                drsNames = ((DataSet)ViewState["TABLA"]).Tables[0].Select("name='" + ds.Tables[0].Columns[i].ColumnName + "'");
                if (drsNames.Length > 0)
                    names = drsNames[0]["remarks"].ToString();
                //DBFunctions.SingleData("SELECT remarks FROM SYSIBM.SYSCOLUMNS WHERE tbname='" + table + "' AND name='" + ds.Tables[0].Columns[i].ColumnName + "'");
                if (names != null)
                    bc.HeaderText = names;
                else
                    bc.HeaderText = ds.Tables[0].Columns[i].ColumnName;
                if (ds.Tables[0].Columns[i].ColumnName == "primaryKey")
                    bc.Visible = false;
                if (!bc.DataField.ToLower().Equals("pag_num_db_mngr"))
                    dgTable.Columns.Add(bc);
            }
        }


        //Verifica la existencia de la tabla y que no sea de 1 registro??
        protected bool VerificarTablasC()
        {
            bool error = false;
            if (DBFunctions.RecordExist("SELECT * FROM " + table + " fetch first 1 rows only;"))
                error = true;
            return error;
        }


        //Alerta eliminar
        protected void validar(object sender, DataGridItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {

                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                case ListItemType.EditItem:
                    ((ImageButton)e.Item.FindControl("btDelete")).Attributes.Add("onclick", "return confirm('Esta Seguro de Borrar este Registro ?');");
                    break;
            }
        }


        //Insertar
        protected void DgTable_Insert(object sender, EventArgs e)
        {
            string processTitle = "";
            string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
            if (table.ToUpper().StartsWith("C"))
            {
                if (VerificarTablasC())
                {
                    Utils.MostrarAlerta(Response, "La tabla solicitada solo permite un registro, imposible crear uno nuevo");
                    PrepareDataSource();
                    LoadData();
                }
                else
                {
                    if (Request.QueryString["processTitle"] != null)
                        processTitle = Request.QueryString["processTitle"];
                    Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=insert&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + processTitle + "&path=" + Request.QueryString["path"] + "");
                }
            }
            else
            {
                if (Request.QueryString["processTitle"] != null)
                    processTitle = Request.QueryString["processTitle"];
                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=insert&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + processTitle + "&path=" + Request.QueryString["path"] + "");
            }
        }


        //Editar, copiar, eliminar, imprimir
        protected void dgTable_Procesos(object sender, DataGridCommandEventArgs e)
        {
            string campos = ViewState["CAMPOS"].ToString();
            #region Editar
            if (e.CommandName == "Edit")
            {

                lbInfo.Text = e.CommandName;
                string processTitle = "";
                string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

                table = Request.QueryString["table"];

                if (Request.QueryString["processTitle"] != null)
                    processTitle = Request.QueryString["processTitle"];

                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=update&pks=" + GenerarLlave(e.Item.DataSetIndex) + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + processTitle + "&path=" + Request.QueryString["path"] + "");
            }
            #endregion Editar
            #region Copiar
            if (e.CommandName == "Copiar")
            {
                lbInfo.Text = e.CommandName;
                string processTitle = "";
                string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

                table = Request.QueryString["table"];

                if (Request.QueryString["processTitle"] != null)
                    processTitle = Request.QueryString["processTitle"];

                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=copiar&pks=" + GenerarLlave(e.Item.DataSetIndex) + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=" + processTitle + "&path=" + Request.QueryString["path"] + "");
            }
            #endregion Copiar
            #region Borrar
            if (e.CommandName == "Borrar")
            {
                string sql;
                string processTitle = "";
                int ind = e.Item.DataSetIndex;

                if (table.ToUpper().StartsWith("C"))
                    Utils.MostrarAlerta(Response, "Imposible eliminar estos datos, operaciones subyacentes dependen de estos");
                else
                {
                    string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                    table = Request.QueryString["table"];

                    if (Request.QueryString["processTitle"] != null)
                        processTitle = Request.QueryString["processTitle"];

                    ds = DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTA"].ToString());
                    sql = "DELETE FROM " + table + " WHERE " + GenerarLlave(ind);

                    sql = sql.Trim();
                    if (sql.EndsWith("AND")) sql = sql.Substring(0, sql.Length - 3) + ";";

                    if (table == "MITEMS")
                        sql = "DELETE FROM MITEMS WHERE mite_codigo='" + ds.Tables[0].Rows[ind][1].ToString().Trim() + "'";

                    ArrayList sqlD = new ArrayList();
                    sqlD.Add(sql);

                    //Guardar historial
                    string user = HttpContext.Current.User.Identity.Name.ToString().ToLower();
                    string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string valId = "";

                    string valoresBorrados = "";
                    for (int cT = 0; cT < ds.Tables[0].Columns.Count; cT++)
                    {
                        valoresBorrados += ds.Tables[0].Rows[ind][cT].ToString() + ",";
                        if (cT == 0 && table.ToUpper().Equals("MVIPFACTURA"))
                            valId = ds.Tables[0].Rows[ind][cT].ToString();
                    }
                    if (valoresBorrados.EndsWith(",")) valoresBorrados = valoresBorrados.Substring(0, valoresBorrados.Length - 1);

                    if (valoresBorrados.Length > 0)
                        sqlD.Add("INSERT INTO DBXSCHEMA.MHISTORIAL_CAMBIOS VALUES(DEFAULT,'" + table + "','D','" + valoresBorrados + "','" + user + "','" + now + "');");

                    if (table.ToUpper().Equals("MVIPFACTURA"))
                        sqlD.Add(String.Format("INSERT INTO MHISTORIAL_FACTURA (FACTURA, OPERACION, SUSU_USUARIO, FECHA) " +
                                          "VALUES ('{0}', 'D', '{1}', '{2}')"
                                          , valId, user, now));

                    if (!DBFunctions.Transaction(sqlD))
                    {
                        if (table != "MITEMS")
                            Utils.MostrarAlerta(Response, "No se pudo eliminar el registro ya que se encuentra restringido por una relación padre - hijo");

                        else
                        {
                            string codI = "";
                            Referencias.Editar(ds.Tables[0].Rows[ind][0].ToString().Trim(), ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + ds.Tables[0].Rows[ind][2].ToString().Trim() + "'"));
                            Utils.MostrarAlerta(Response, "No se puede eliminar el registro " + codI + ".\\nPosible Relación de dependencia!");
                        }
                    }
                }
                ViewState["PAGE_NUM"] = 1;
                ds.Clear();
                PrepareDataSource();
                LoadData();
                CalcularTamano();
                ArmarPaginador();
            }
            #endregion Borrar
            #region Imprimir
            if (e.CommandName == "Imprimir")
            {
                lbInfo.Text = e.CommandName;
                string processTitle = "";
                string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

                table = Request.QueryString["table"];

                if (Request.QueryString["processTitle"] != null)
                    processTitle = Request.QueryString["processTitle"];

                Response.Redirect(indexPage + "?process=DBManager.Preview&table=" + table + "&pks=" + GenerarLlave(e.Item.DataSetIndex).Replace("'", "*") + "&path=" + Request.QueryString["path"]);
            }
            #endregion Imprimir
        }

        //Generar llave primeria fila
        private string GenerarLlave(int indice)
        {
            DataSet dsTable = (DataSet)ViewState["TABLA"];
            string pk = "", coltype = "";
            int ind = indice;

            if (ViewState["AUX"].ToString() == "0")
                ds = DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTA"].ToString());
            else
                ds = DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTAAUX"].ToString());

            if (table.ToUpper().Trim() == "MITEMS")
            { //si es MITEMS le paso el cod del item reorganizado
                String aux = "";
                string linCodigo = ds.Tables[0].Rows[ind][3].ToString();
                if (linCodigo.Contains(" - "))
                {
                    linCodigo = linCodigo.Substring(0, linCodigo.IndexOf(" "));
                }

                Referencias.Guardar(ds.Tables[0].Rows[ind][1].ToString(), ref aux,
                    DBFunctions.SingleData("SELECT PLIN_TIPO FROM PLINEAITEM WHERE PLIN_CODIGO='" + linCodigo + "'"));

                pk = ds.Tables[0].Columns[1].ColumnName + "='" + aux + "'";
            }
            else if (ds.Tables[0].Columns[1].ColumnName.ToUpper().Trim() == "MITE_CODIGO")
            {
                //Arreglo para traer los codigos de items de tablas sin referencia al tipo de
                //linea. Ya que no se puede aplicar la funcion de conversion de codigo item.
                DataSet dsAux = new DataSet();
                dsAux = DBFunctions.Request(dsAux, IncludeSchema.YES, "SELECT * FROM DBXSCHEMA." + table.ToUpper().Trim() + " fetch first 50 rows only;");
                DataColumn[] dCol = new DataColumn[dsAux.Tables[0].PrimaryKey.Length];
                int h = 0;
                for (int j = 1; j < ds.Tables[0].Columns.Count && h < dsAux.Tables[0].PrimaryKey.Length; j++)
                {
                    if (ds.Tables[0].Columns[j].ColumnName.ToUpper().Trim() == dsAux.Tables[0].PrimaryKey[h].ColumnName.ToUpper().Trim())
                    {
                        if (ds.Tables[0].Columns[j].ColumnName.ToUpper().Trim() == "MITE_CODIGO")
                        {
                            String aux = "";
                            String consultaAux = ObenerConsultaCodigo(ViewState["CONSULTA"].ToString(), ds.Tables[0].Rows[ind][j].ToString());
                            aux = DBFunctions.SingleData(consultaAux);

                            pk += ds.Tables[0].Columns[j].ColumnName + "='" + aux + "'";
                        }
                        else
                        {
                            coltype = dsTable.Tables[0].Select("name='" + ds.Tables[0].Columns[j].ColumnName + "'")[0]["coltype"].ToString().Trim();
                            if (coltype.Equals("TIME") || coltype.Equals("CHAR") || coltype.Equals("CLOB") || coltype.Equals("LONGVAR") || coltype.Equals("VARCHAR"))
                            {
                                pk += ds.Tables[0].Columns[j].ColumnName + "='" + ds.Tables[0].Rows[ind][ds.Tables[0].Columns[j].ColumnName].ToString() + "'";
                            }
                            else if (coltype.Equals("DATE"))
                            {
                                DateTime fecha = (DateTime)ds.Tables[0].Rows[ind][ds.Tables[0].Columns[j].ColumnName];
                                pk += ds.Tables[0].Columns[j].ColumnName + "='" + fecha.ToString("yyy-MM-dd") + "'";
                            }
                            else
                                pk += ds.Tables[0].Columns[j].ColumnName + "=" + ds.Tables[0].Rows[ind][ds.Tables[0].Columns[j].ColumnName].ToString();
                        }

                        h++;
                        if (h < dsAux.Tables[0].PrimaryKey.Length)
                            pk += " AND ";

                    }
                }

            }
            else
            {
                if (ds.Tables[0].PrimaryKey.Length == 0)
                {
                    ds.Clear();
                    ds = DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTA"].ToString());
                }
                for (i = 0; i < ds.Tables[0].PrimaryKey.Length; i++)
                {
                    coltype = dsTable.Tables[0].Select("name='" + ds.Tables[0].PrimaryKey[i].ColumnName + "'")[0]["coltype"].ToString().Trim();
                    if (coltype.Equals("TIME") || coltype.Equals("CHAR") || coltype.Equals("CLOB") || coltype.Equals("LONGVAR") || coltype.Equals("VARCHAR"))
                    {
                        string valorCampo = ds.Tables[0].Rows[ind][ds.Tables[0].PrimaryKey[i].ColumnName].ToString();
                        valorCampo = valorCampo.Replace("+", "'\'");
                        try { valorCampo = valorCampo.Substring(0, valorCampo.IndexOf(" - ")); }
                        catch (Exception e) { }
                        pk += ds.Tables[0].PrimaryKey[i].ColumnName + "='" + valorCampo + "'";
                    }
                    else if (coltype.Equals("DATE"))
                    {
                        DateTime fecha = (DateTime)ds.Tables[0].Rows[ind][ds.Tables[0].PrimaryKey[i].ColumnName];
                        pk += ds.Tables[0].PrimaryKey[i].ColumnName + "='" + fecha.ToString("yyy-MM-dd") + "'";
                    }
                    else
                    {
                        //pk += ds.Tables[0].PrimaryKey[i].ColumnName + "=" + ds.Tables[0].Rows[ind][ds.Tables[0].PrimaryKey[i].ColumnName].ToString();
                        string valorCampo = ds.Tables[0].Rows[ind][ds.Tables[0].PrimaryKey[i].ColumnName].ToString();
                        valorCampo = valorCampo.Replace("+", "'\'");
                        try { valorCampo = valorCampo.Substring(0, valorCampo.IndexOf(" - ")); }
                        catch (Exception e) { }
                        pk += ds.Tables[0].PrimaryKey[i].ColumnName + "=" + valorCampo + "";
                    }
                    if (i != ds.Tables[0].PrimaryKey.Length - 1)
                        pk += " AND ";
                }
            }
            if (pk == "")
            {
                Response.Redirect(indexPage + "?process=DBManager.Selects&table=" + table + "&error=1");
            }
            return (pk);
        }


        //Buscar
        public void Search(Object sender, EventArgs e)
        {
            if (tbWord.Text.Length == 0)
                tbWord.Text = "%";
            likestr.Text = tbWord.Text;
            likestr.Text = likestr.Text.Replace("%", "*");
            if (ddlCols.Items.Count > 0)
                txtFilt.Text = ddlCols.SelectedItem.Value;
            ViewState["PAGE_NUM"] = 1;


            PrepareDataSource();
            LoadData();
            CalcularTamano();
            ArmarPaginador();
        }

        //Ordenar
        public void Clasifica(Object sender, EventArgs e)
        {
            ViewState["PAGE_NUM"] = 1;
            if (ddlCols.Items.Count > 0)
                txtSort.Text = ddlCols.SelectedItem.Value;
            PrepareDataSource();
            LoadData();
            CalcularTamano();
            ArmarPaginador();
        }

        public void Listar(Object sender, EventArgs e)
        {
            Page_Load(sender, e);
        }


        public void SendMail(Object Sender, ImageClickEventArgs E)
        {
            /*MakeReport(Sender, E);*/
            PrepareDataSource();
            LoadData();
            dgAux.Visible = true;
            dgAux.DataSource = ds.Tables[0];
            dgAux.DataBind();
            //lbInfo.Text = Press.PressOnEmail( "Reporte de "+Request.QueryString["path"], tbEmail.Text, tabPreHeader, dgAux);
            dgAux.Visible = false;
        }

        //Agrega a la cadena inicial de consulta una columna mas en comparacion con el
        //codigo original antes de ser editado con la funcion de referencia.
        //Esto ya que las tablas deribadas a MITEMS no poseen relacion con la linea.
        private string ObenerConsultaCodigo(String consulta, String codigo)
        {
            String consultaCodigo = "";

            consultaCodigo = "SELECT MITE_CODIGONORMAL ";
            consultaCodigo += consulta.Substring(8, (consulta.IndexOf("AS MITE_CODIGO") - 8));
            consulta = consulta.Substring((consulta.IndexOf("AS MITE_CODIGO") + 15));
            consultaCodigo += " AS MITE_CODIGO,H.MITE_CODIGO AS MITE_CODIGONORMAL, ";
            consultaCodigo += consulta.Substring(0, consulta.IndexOf("fetch first"));
            consultaCodigo += " AND MITE_CODIGO = '" + codigo + "' ";
            consulta = consulta.Substring(consulta.IndexOf("fetch first"));
            consultaCodigo += consulta;

            return consultaCodigo;
        }

        protected void ImprimirExcelGrid(Object Sender, EventArgs e)
        {
            try
            {
                ds = DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTAExcel"].ToString());
                Utils.ImprimeExcel(ds, "ConsultaTabla");
            }
            catch (Exception ex)
            {
                lblResult.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
                return;
            }

            //try
            //{
            //     ds = DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTAExcel"].ToString());
            //    DateTime fecha = DateTime.Now;
            //    string nombreArchivo = "ConsultaTabla" + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
            //    base.Response.Clear();
            //    base.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xls");
            //    base.Response.Charset = "Unicode";
            //    base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    base.Response.ContentType = "application/vnd.xls";
            //    StringWriter stringWrite = new StringWriter();
            //    HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            //    //ds = (DataSet)ViewState["dataGrid"];
            //    DataGrid dgAux = new DataGrid();
            //    dgAux.DataSource = ds;
            //    dgAux.DataBind();
            //    dgAux.RenderControl(htmlWrite);

            //    base.Response.Write(stringWrite.ToString());
            //    base.Response.End();
            //}
            //catch (Exception ex)
            //{
            //    lblResult.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
            //    return;
            //}
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
        protected void DgDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink doc = new HyperLink();
                doc.NavigateUrl = "";
                if(ViewState["CLOBPOSICION"] != null)
                {
                    int posClob = Convert.ToInt16(ViewState["CLOBPOSICION"]) + 4;  //Se aumenta 4 por los 4 primeros iconos que ocupan columnas en la tabla de SELECTS.
                    if (e.Item.Cells[posClob].Text != "&nbsp;" && e.Item.Cells[posClob].Text != "")
                    {
                        //abrirDocumento(valor, primaria, campo, tabla)  funcion javascript
                        e.Item.Cells[posClob].Text = "<a href='javascript: abrirDocumento(\"" + e.Item.Cells[4].Text + "\",\"" + ViewState["CLOBPRIMA"] + "\",\"" + ViewState["CLOBCAMPO"] + "\",\"" + ViewState["CLOBTABLA"] + "\"); '>Ver Documento</a>"; // "Ver Documento";
                    }
                    else
                        e.Item.Cells[posClob].Text = "--";
                }
            }

        }

        [Ajax.AjaxMethod()]
        public string Abrir_Documento(string valor, string primaria, string campo, string tabla)
        {
            string sql = "select " + campo + " from " + tabla + " where " + primaria + " = '" + valor + "'";
            return DBFunctions.SingleData(sql);
        }

    }
}
