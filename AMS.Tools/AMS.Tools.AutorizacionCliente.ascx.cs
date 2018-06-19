using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.DB;
using System.Collections;
using System.Configuration;
using System.Data;

namespace AMS.Tools
{

	public partial class AutorizacionCliente : System.Web.UI.UserControl
	{
        string nit, tipoDB, nombre, nombreBD;
        private ArrayList sqlStrings;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        
        protected void Page_Load(object sender, EventArgs e)
		{
            DataSet dsRegistroUsuario = new DataSet();
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Tools.AutorizacionCliente));

            if (!IsPostBack)
            {
                if (Session["nit"] != null && Session["nombre"] != null && Session["tipoDB"] != null)
                {
                    nit = Session["nit"].ToString();
                    nombre = Session["nombre"].ToString();
                    tipoDB = Session["tipoDB"].ToString();
                    if (DBFunctions.SingleData("SELECT TNIT_TIPONIT FROM MNIT WHERE MNIT_NIT = '" + nit + "'") == "N")
                    {
                        chkCambioNit.Visible = true;
                    }
                    else
                        chkCambioNit.Visible = false;
                    nombreBD = DBFunctions.SingleData("select TBAS_NOMBRE from tbasedatos where TBAS_CODIGO = '" + tipoDB + "'");
                    DBFunctions.Request(dsRegistroUsuario, IncludeSchema.NO, "SELECT PBAS_CODIGO, TBAS_CODIGO, PBAS_DESCRIPCION FROM PBASEDATOS WHERE TBAS_CODIGO IN ('E', '" + tipoDB + "');");
                    ViewState["dsRegistro"] = dsRegistroUsuario;
                    if (dsRegistroUsuario.Tables[0].Rows.Count == 0)
                    {
                        grillaElementos.Visible = false;
                        lbRazon.Text = "Aún no ha parametrizado ninguna pregunta para el tipo de Base de Datos genérico(E) y " + nombreBD + "(" + tipoDB + "). No olvide parametrizarlo";
                        return;
                    }
                    Autorizacion();
                }
                else
                {
                    Utils.MostrarAlerta(Response, "No se puede acceder a este menú sin los respectivos parámetros de acceso.");
                    lbRazon.Text = "Aún no ha parametrizado ninguna pregunta para el tipo de Base de Datos genérico(E). No olvide parametrizarlo";
                    btnGuardar.Enabled = false;
                }
            }
            Session["finAutorizar"] = "1";
        }

        public void Autorizacion()
        {
            string tipoBase = DBFunctions.SingleData("select TBAS_NOMBRE from tbasedatos where TBAS_CODIGO = '" + tipoDB + "'");
            if (tipoBase != "")
            {
                txtTipoBase.Text = tipoBase;
                txtNit.Text = nit;
                txtNombre.Text = nombre;
            }
            if (((DataSet)ViewState["dsRegistro"]).Tables[0].Rows.Count > 0)
            {
                grillaElementos.DataSource = ((DataSet)ViewState["dsRegistro"]).Tables[0];
                grillaElementos.DataBind();
            }
            else
            {
                Utils.MostrarAlerta(Response, "No se ha parametrizado ninguna autorización Habeas Data para este tipo de Base de Datos: " + tipoBase);
                btnGuardar.Enabled = false;
            }

        }

        public void GuardarDatos(object sender, EventArgs e)
        {
            if(txtNit.Text.Length == 0)
            {
                Utils.MostrarAlerta(Response, "No ingresó ningún NIT");
                return;
            }
            else if(!DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT = '" + txtNit.Text + "'"))
            {
                Utils.MostrarAlerta(Response, "El NIT digitado no existe");
                return;
            }
            bool razon1, chkS, chkN;

            sqlStrings = new ArrayList();
            string stringTres = "";
            string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

            for (int i = 0; i < grillaElementos.Items.Count; i++)
            {
                razon1 = ((CheckBox)grillaElementos.Items[i].Cells[3].Controls[5]).Checked;
                chkS = ((CheckBox)grillaElementos.Items[i].Cells[3].Controls[1]).Checked;
                chkN = ((CheckBox)grillaElementos.Items[i].Cells[3].Controls[3]).Checked;

                if (!razon1 && (chkS || chkN))
                {
                    stringTres = chkS ? "S" : "N";
                    sqlStrings.Add
                             ("INSERT INTO DBXSCHEMA.MBASEDATOS(MNIT_NIT, PBAS_CODIGO, TRES_SINO, MBAS_FECHA ) VALUES ( '" + txtNit.Text + "', " + ((DataSet)ViewState["dsRegistro"]).Tables[0].Rows[i][0].ToString() + ", " + "'" + stringTres + "', " + "'" + fechaHoy + "'" + ");");
                }
            }
            if (sqlStrings.Count == 0)
            {
                Utils.MostrarAlerta(Response, "El cliente no respondió a ninguna pregunta, por tanto no se ingresará ningún registro a la base de Datos");
                return;
            }
            if (DBFunctions.Transaction(sqlStrings))
            {
                Utils.MostrarAlerta(Response, "La autorización ha sido ingresada correctamente!");
            }
            else 
            {
                Utils.MostrarAlerta(Response, "Se ha producido un error al realizar el registro de autorización, contacte al Administrador del sistema por favor.");
                string error = DBFunctions.exceptions;
            }
        }

        public void IgnorarDatos(object sender, EventArgs e)
        {
            Session["nit"] = null;
            Session["nombre"] = null;
            Session["tipoDB"] = null;
            return;
        }

        #region Ajax
        [Ajax.AjaxMethod()]
        public string obtener_Nombre(string nitResponse)
        {
            string rta = "";
            rta = DBFunctions.SingleData("SELECT NOMBRE FROM VMNIT WHERE MNIT_NIT = '" + nitResponse + "'");
            return rta;
        }

        #endregion
    }
}