using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.DB;
using AMS.Forms;
using System.Data;
using System.Collections;
using System.Configuration;

namespace AMS.Tools
{
	public partial class AutorizacionClienteLista : System.Web.UI.UserControl
	{
        protected Table tbListaDB;
        bool yaRegistro;
        DataSet dsBaseDatosAutorizar;
        int contadorExistentes=0, contadorFilas = 0;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            plhListaDB.Visible = true;

            if (Session["baseDatos"] != null && !IsPostBack)
            {
                string texto = DBFunctions.SingleData("SELECT tbas_info FROM tbasedatos where tbas_codigo = '" + Session["baseDatos"].ToString() + "'");
                divMendaje.InnerText = texto;
            }

            if (Session["parametro"] != null)
            {
                txtNit.Text = (String)Session["parametro"];
                ConsultarRegistroBaseDatos(sender, e);
            }
		}

        public void ConsultarRegistroBaseDatos(object sender, EventArgs e)
        {
            if (txtNit.Text != "")
            {
                DataSet dsRegistroUsuario = new DataSet();

                //DBFunctions.Request(dsRegistroUsuario, IncludeSchema.NO, "select tb.tbas_codigo, mb.tres_sino, tb.tbas_nombre from mbasedatos mb, tbasedatos tb where mb.mnit_nit='" + txtNit.Text + "' and tb.tbas_codigo=mb.tbas_codigo;");
                DBFunctions.Request(dsRegistroUsuario, IncludeSchema.NO, "SELECT MB.PBAS_CODIGO, MB.TRES_SINO, TB.TBAS_NOMBRE FROM TBASEDATOS TB, PBASEDATOS PB, MBASEDATOS MB WHERE MB.MNIT_NIT = '" + txtNit.Text + "'PB.PBAS_CODIGO = MB.PBAS_CODIGO AND PB.TBAS_CODIGO = TB.TBAS_CODIGO;");
                ViewState["dsRegistro"] = dsRegistroUsuario;
                yaRegistro = dsRegistroUsuario != null && dsRegistroUsuario.Tables[0].Rows.Count > 0;
                ViewState["yaRegistro"] = yaRegistro;

                if (yaRegistro)
                {
                    Utils.MostrarAlerta(Response, "Este usuario ya ha sido registrado antes.");
                }

                dsBaseDatosAutorizar = new DataSet();
                DBFunctions.Request(dsBaseDatosAutorizar, IncludeSchema.NO, "select TBAS_CODIGO, TBAS_NOMBRE from tbasedatos;");
                ViewState["BaseDatosAutorizar"] = dsBaseDatosAutorizar;

                if (dsBaseDatosAutorizar != null && dsBaseDatosAutorizar.Tables[0].Rows.Count > 0)
                {
                    grillaElementos.DataSource = dsBaseDatosAutorizar.Tables[0];
                    grillaElementos.DataBind();

                    plhListaDB.Visible = true;
                    btnGuardar.Visible = true;
                }
                else
                {
                    Utils.MostrarAlerta(Response, "No se han parametrizado las bases de datos para el registro de Autorización del Cliente. Por favor, parametrizar para continuar.");
                }
                
            }
            else
            {
                Utils.MostrarAlerta(Response, "Por favor ingrese el NIT del usuario antes de realizar la consulta.");
            }
        }

        public void GuardarRegistro(object sender, EventArgs e)
        {
            ArrayList sqlStrings = new ArrayList();
            dsBaseDatosAutorizar = (DataSet)ViewState["BaseDatosAutorizar"];
            yaRegistro = (bool)ViewState["yaRegistro"];
            DataSet dsRegistro = (DataSet)ViewState["dsRegistro"];

            for (int i = 0; i < grillaElementos.Items.Count; i++)
            {
                string resulSiNo = "N";
                bool insertar = true;
                if (((RadioButton)grillaElementos.Items[i].Cells[1].FindControl("rdSi")).Checked == true)
                {
                    resulSiNo = "S";
                }

                for (int j = 0; j < dsRegistro.Tables[0].Rows.Count; j++)
                {
                    if (dsBaseDatosAutorizar.Tables[0].Rows[i][0].ToString() == dsRegistro.Tables[0].Rows[j][0].ToString())
                    {
                        insertar = false;
                        j = dsRegistro.Tables[0].Rows.Count + 1;
                    }
                }

                if (yaRegistro == false || insertar)
                    sqlStrings.Add("INSERT INTO MBASEDATOS VALUES ('" + dsBaseDatosAutorizar.Tables[0].Rows[i][0] + "' , '" + txtNit.Text + "' , '" + resulSiNo + "')");
                else
                    sqlStrings.Add("UPDATE MBASEDATOS SET TRES_SINO = '" + resulSiNo + "' WHERE TBAS_CODIGO = '" + dsBaseDatosAutorizar.Tables[0].Rows[i][0] + "' AND  MNIT_NIT = '" + txtNit.Text + "';");
            }

            if (DBFunctions.Transaction(sqlStrings))
            {
                Utils.MostrarAlerta(Response, "El registro de autroizaciones ha sido creado correctamente!");
                plhListaDB.Visible = false;
                btnGuardar.Visible = false;
                //Response.Redirect("" + indexPage + "?process=Tools.AutorizacionClienteLista");
            }
            else
            {
                Utils.MostrarAlerta(Response, "Se ha producido un error al realizar el registro de autorización!");
            }

                
        }

        protected void dgElementosBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem ) && yaRegistro)
            {
                DataSet dsRegistro = (DataSet)ViewState["dsRegistro"];
                dsBaseDatosAutorizar = (DataSet)ViewState["BaseDatosAutorizar"];

                string estado = "";

                if (contadorExistentes < dsRegistro.Tables[0].Rows.Count &&
                    dsBaseDatosAutorizar.Tables[0].Rows[contadorFilas][0].ToString() == dsRegistro.Tables[0].Rows[contadorExistentes][0].ToString())
                {
                    estado = dsRegistro.Tables[0].Rows[contadorExistentes][1].ToString();
                    contadorExistentes++;
                }
                
                contadorFilas++;

                if(estado  == "S")
                    ((RadioButton)e.Item.Cells[1].Controls[1]).Checked = true;
                else if (estado == "N")
                    ((RadioButton)e.Item.Cells[1].Controls[3]).Checked = true;
            }
        }

	}
}