using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AMS.DB;
using AMS.Inventarios;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Inventarios
{
    /// <summary>
    ///		Descripción breve de AMS_Inventarios_RolesInventario.
    /// </summary>
    public partial class AMS_Inventarios_RolesInventario : System.Web.UI.UserControl
    {
        #region Atributos
        protected System.Web.UI.WebControls.Panel Panel1;
        protected DataTable dt;
        protected DataTable dt1;
        protected DataTable dt2;
        protected DataTable dt3;
        protected int numerodig,numeropat,numerocont,numerocor;
        protected int band;
        protected int band1;
        protected int band2;
        protected int band3;
        protected string pruebas;
        #endregion

        #region Eventos

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                dig.Text = "0";
                pat.Text = "0";
                cont.Text = "0";
                cor.Text = "0";
                DatasToControls bind  = new DatasToControls();
                //bind.PutDatasIntoDropDownList(ddlprefijo, "select pdoc_codigo,pdoc_nombre from pdocumento where tdoc_tipodocu='IF' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref ddlprefijo  , "%", "%", "IF");
                //ddlprefijo_OnSelectedIndexChanged(sender, e);

                bind.PutDatasIntoDropDownList(dllRolGer, "Select prinf_codigo,prinf_nombrerol from PROLINVENTARIOFISICO");
                dllRolGer.SelectedValue = "1";
                bind.PutDatasIntoDropDownList(dllRolAudi, "Select prinf_codigo,prinf_nombrerol from PROLINVENTARIOFISICO");
                dllRolAudi.SelectedValue = "2";
                bind.PutDatasIntoDropDownList(ddlDigRol, "Select prinf_codigo,prinf_nombrerol from PROLINVENTARIOFISICO");
                ddlDigRol.SelectedValue = "3";
                bind.PutDatasIntoDropDownList(dllRolPat, "Select prinf_codigo,prinf_nombrerol from PROLINVENTARIOFISICO");
                dllRolPat.SelectedValue = "4";
                bind.PutDatasIntoDropDownList(dllRolCont, "Select prinf_codigo,prinf_nombrerol from PROLINVENTARIOFISICO");
                dllRolCont.SelectedValue = "5";
                bind.PutDatasIntoDropDownList(dllRolCoor, "Select prinf_codigo,prinf_nombrerol from PROLINVENTARIOFISICO");
                dllRolCoor.SelectedValue = "6";

                dllRolGer.Enabled = dllRolAudi.Enabled = ddlDigRol.Enabled = dllRolPat.Enabled = dllRolCont.Enabled = dllRolCoor.Enabled = false;
                
                if (Request["tiqs"] != null && Request["tiq"] != null && Request["tiqs"] != string.Empty && Request["tiq"] != string.Empty)
                {
                    ddlprefijo.SelectedValue = Request["tiqs"];
                    ddlprefijo_OnSelectedIndexChanged(sender, e);
                    ddlNumero.SelectedValue = Request["tiq"];
                    aceptar_Click(sender, e);
                }
            }
        }

        protected void ddlprefijo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT minf_numeroinv \n" +
             "FROM minventariofisico \n" +
             "WHERE minf_fechacierre IS NULL \n" +
             "AND   pdoc_codigo = '{0}'"
             , ddlprefijo.SelectedValue);

            Utils.FillDll(ddlNumero, sql, false);
        }

        protected void Cargar_Tabla_Rtns(object sender, EventArgs e)
        {
            if (numdig.Text != "" && Convert.ToInt32(dig.Text) != Convert.ToInt32(numdig.Text))
            {
                dig.Text = numdig.Text;
                dig.Visible = true;
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("Nombre", typeof(string)));
                dt.Columns.Add(new DataColumn("Nit", typeof(string)));
                for (int i=0; i < Convert.ToInt32(numdig.Text); i++)
                {
                    DataRow fila = dt.NewRow();
                    dt.Rows.Add(fila);
                }
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                DataGrid1.Visible = true;
            }
            if (numpat.Text != "" && Convert.ToInt32(pat.Text) != Convert.ToInt32(numpat.Text))
            {
                pat.Text = numpat.Text;
                pat.Visible = true;
                numeropat = Convert.ToInt32(numpat.Text);
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("Nombre", typeof(string)));
                dt.Columns.Add(new DataColumn("Nit", typeof(string)));
                for (int i=0; i < Convert.ToInt32(numpat.Text); i++)
                {
                    DataRow fila = dt.NewRow();
                    dt.Rows.Add(fila);
                }
                DataGrid2.DataSource = dt;
                DataGrid2.DataBind();
                DataGrid2.Visible = true;
            }
            if (numcont.Text != "" && Convert.ToInt32(cont.Text) != Convert.ToInt32(numcont.Text))
            {
                cont.Text = numcont.Text;
                cont.Visible = true;
                numerocont = Convert.ToInt32(numcont.Text);
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("Nombre", typeof(string)));
                dt.Columns.Add(new DataColumn("Nit", typeof(string)));
                for (int i=0; i < Convert.ToInt32(numcont.Text); i++)
                {
                    DataRow fila = dt.NewRow();
                    dt.Rows.Add(fila);
                }
                DataGrid3.DataSource = dt;
                DataGrid3.DataBind();
                DataGrid3.Visible = true;

            }
            if (numcor.Text != "" && Convert.ToInt32(cor.Text) != Convert.ToInt32(numcor.Text))
            {
                cor.Text = numcor.Text;
                cor.Visible = true;
                numerocor = Convert.ToInt32(numcor.Text);
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("Nombre", typeof(string)));
                dt.Columns.Add(new DataColumn("Nit", typeof(string)));
                for (int i=0; i < Convert.ToInt32(numcor.Text); i++)
                {
                    DataRow fila = dt.NewRow();
                    dt.Rows.Add(fila);
                }
                DataGrid4.DataSource = dt;
                DataGrid4.DataBind();
                DataGrid4.Visible = true;
            }
        }

        protected void Guardar_Roles(object sender, EventArgs e)
        {
            string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

            InventarioFisico inst = new InventarioFisico(hdPrefSelec.Value, Convert.ToInt32(hdNumSelec.Value));
            inst.RolesEspecificos.Clear(); // se agregarán todos de nuevo :)

            //Se agrega el rol del gerente del inventario fisico
            if (TextBox1.Text != "")
                inst.RolesEspecificos.Add(new RolesInventarioFisico(dllRolGer.SelectedValue, TextBox1a.Value));

            if (TextBox2.Text != "")
                inst.RolesEspecificos.Add(new RolesInventarioFisico(dllRolAudi.SelectedValue, TextBox2a.Value));

            if (Convert.ToInt32(dig.Text) != 0)
            {
                for (int i=0; i < DataGrid1.Items.Count; i++)
                    if (((TextBox)DataGrid1.Items[i].Cells[1].Controls[1]).Text != "")
                        inst.RolesEspecificos.Add(new RolesInventarioFisico(ddlDigRol.SelectedValue, ((TextBox)DataGrid1.Items[i].Cells[1].Controls[1]).Text));
            }

            if (Convert.ToInt32(pat.Text) != 0)
            {
                for (int j=0; j < DataGrid2.Items.Count; j++)
                    if (((TextBox)DataGrid2.Items[j].Cells[1].Controls[1]).Text != "")
                        inst.RolesEspecificos.Add(new RolesInventarioFisico(dllRolPat.SelectedValue, ((TextBox)DataGrid2.Items[j].Cells[1].Controls[1]).Text, Convert.ToInt32(((DropDownList)DataGrid2.Items[j].Cells[2].FindControl("grupo")).SelectedValue)));
            }

            if (Convert.ToInt32(cont.Text) != 0)
            {
                for (int k=0; k < DataGrid3.Items.Count; k++)
                    if (((TextBox)DataGrid3.Items[k].Cells[1].Controls[1]).Text != "")
                        inst.RolesEspecificos.Add(new RolesInventarioFisico(dllRolCont.SelectedValue, ((TextBox)DataGrid3.Items[k].Cells[1].Controls[1]).Text, Convert.ToInt32(((DropDownList)DataGrid3.Items[k].Cells[2].FindControl("grupo3")).SelectedValue)));
            }

            if (Convert.ToInt32(cor.Text) != 0)
            {
                for (int g=0; g < DataGrid4.Items.Count; g++)
                    if (((TextBox)DataGrid4.Items[g].Cells[1].Controls[1]).Text != " ")
                        inst.RolesEspecificos.Add(new RolesInventarioFisico(dllRolCoor.SelectedValue, ((TextBox)DataGrid4.Items[g].Cells[1].Controls[1]).Text, Convert.ToInt32(((DropDownList)DataGrid4.Items[g].Cells[2].FindControl("grupo2")).SelectedValue)));
            }

            if (inst.CrearRolesInventarioFisico())
            {
                string tipo = inst.CodigoTipoInventarioUbicacion;
                if (tipo.Equals("A"))
                    Response.Redirect("" + indexPage + "?process=Inventarios.InventarioFisico");
                if (tipo.Equals("T"))
                    Response.Redirect("" + indexPage + "?process=Inventarios.InventarioFisicoTodos");
            }
            else
                Utils.MostrarAlerta(Response, "Se ha presentado un error al actualizar los roles!");
        }

        protected void aceptar_Click(object sender, System.EventArgs e)
        {
            if (ddlNumero.SelectedItem.Text != null && Utils.EsNumero(ddlNumero.SelectedItem.Text))
            {
                hdPrefSelec.Value = ddlprefijo.SelectedValue;
                hdNumSelec.Value = ddlNumero.SelectedValue;
                LoadInitData(ddlprefijo.SelectedValue, ddlNumero.SelectedValue);
                Panel2.Visible = true;
            }
            else
                Utils.MostrarAlerta(Response, "Seleccione un Prefijo y un Número de Inventario");
        }

        #endregion

        #region Metodos

        protected void LoadInitData(string prefInventario, string numInventario)
        {
            string sql = String.Format(
             "SELECT mrol.prinf_codigo AS Rol, \n" +
             "       MNI.mnit_nit AS Nit, \n" +
             "       MNI.mnit_nombres concat ' ' concat COALESCE(MNI.mnit_nombre2,' ') concat ' ' concat MNI.mnit_apellidos concat ' ' concat COALESCE(MNI.mnit_apellido2,' ') AS Nombre \n" +
             "FROM mrolinventariofisico mrol  \n" +
             "  LEFT JOIN mnit MNI ON mrol.mnit_nit = MNI.mnit_nit \n" +
             "WHERE pdoc_codigo = '{0}' \n" +
             "AND   minf_numeroinf = {1}"
             , prefInventario
             , numInventario);

            DataSet ds = new DataSet();
            DataTable dtGerente = new DataTable();
            DataTable dtAuditor = new DataTable();
            DataTable dtDigitadores = new DataTable();
            DataTable dtPatinadores = new DataTable();
            DataTable dtCoordinadores = new DataTable();
            DataTable dtContadores = new DataTable();

            dtGerente.Columns.Add(new DataColumn("Rol", typeof(string)));
            dtGerente.Columns.Add(new DataColumn("Nit", typeof(string)));
            dtGerente.Columns.Add(new DataColumn("Nombre", typeof(string)));
            dtAuditor.Columns.Add(new DataColumn("Rol", typeof(string)));
            dtAuditor.Columns.Add(new DataColumn("Nit", typeof(string)));
            dtAuditor.Columns.Add(new DataColumn("Nombre", typeof(string)));
            dtDigitadores.Columns.Add(new DataColumn("Rol", typeof(string)));
            dtDigitadores.Columns.Add(new DataColumn("Nit", typeof(string)));
            dtDigitadores.Columns.Add(new DataColumn("Nombre", typeof(string)));
            dtPatinadores.Columns.Add(new DataColumn("Rol", typeof(string)));
            dtPatinadores.Columns.Add(new DataColumn("Nit", typeof(string)));
            dtPatinadores.Columns.Add(new DataColumn("Nombre", typeof(string)));
            dtCoordinadores.Columns.Add(new DataColumn("Rol", typeof(string)));
            dtCoordinadores.Columns.Add(new DataColumn("Nit", typeof(string)));
            dtCoordinadores.Columns.Add(new DataColumn("Nombre", typeof(string)));
            dtContadores.Columns.Add(new DataColumn("Rol", typeof(string)));
            dtContadores.Columns.Add(new DataColumn("Nit", typeof(string)));
            dtContadores.Columns.Add(new DataColumn("Nombre", typeof(string)));

            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);

            foreach (DataRow dr in ds.Tables[0].Select("ROL=1")) dtGerente.Rows.Add(dr.ItemArray);
            foreach (DataRow dr in ds.Tables[0].Select("ROL=2")) dtAuditor.Rows.Add(dr.ItemArray);
            foreach (DataRow dr in ds.Tables[0].Select("ROL=3")) dtDigitadores.Rows.Add(dr.ItemArray);
            foreach (DataRow dr in ds.Tables[0].Select("ROL=4")) dtPatinadores.Rows.Add(dr.ItemArray);
            foreach (DataRow dr in ds.Tables[0].Select("ROL=5")) dtCoordinadores.Rows.Add(dr.ItemArray);
            foreach (DataRow dr in ds.Tables[0].Select("ROL=6")) dtContadores.Rows.Add(dr.ItemArray);

            if (dtGerente.Rows.Count > 0)
            {
                DataRow drGerente = dtGerente.Rows[0];
                TextBox1.Text = drGerente["NOMBRE"].ToString();
                TextBox1a.Value = drGerente["NIT"].ToString();
            }
            if (dtAuditor.Rows.Count > 0)
            {
                DataRow drAuditor = dtAuditor.Rows[0];
                TextBox2.Text = drAuditor["NOMBRE"].ToString();
                TextBox2a.Value = drAuditor["NIT"].ToString();
            }

            DataGrid1.DataSource = dtDigitadores;
            DataGrid2.DataSource = dtPatinadores;
            DataGrid3.DataSource = dtCoordinadores;
            DataGrid4.DataSource = dtContadores;
            DataGrid1.Visible = dtDigitadores.Rows.Count > 0;
            DataGrid2.Visible = dtPatinadores.Rows.Count > 0;
            DataGrid3.Visible = dtCoordinadores.Rows.Count > 0;
            DataGrid4.Visible = dtContadores.Rows.Count > 0;
            DataGrid1.DataBind();
            DataGrid2.DataBind();
            DataGrid3.DataBind();
            DataGrid4.DataBind();
            dig.Text = numdig.Text = dtDigitadores.Rows.Count.ToString();
            pat.Text = numpat.Text = dtPatinadores.Rows.Count.ToString();
            cont.Text = numcont.Text = dtCoordinadores.Rows.Count.ToString();
            cor.Text = numcor.Text = dtContadores.Rows.Count.ToString();
        }

        #endregion

        #region Código generado por el Diseñador de Web Forms
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Método necesario para admitir el Diseñador. No se puede modificar
        ///		el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}

