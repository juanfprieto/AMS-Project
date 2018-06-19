using AMS.DB;
using AMS.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Vehiculos
{
    public partial class AMS_Vehiculos_AnfitrionVentas : System.Web.UI.UserControl
    {
        protected DataTable tablaVendedores;
        protected DataRow[] vendedores;
        protected string tablaNit, codVen, sede, fecha, usuaio;
        protected string index = ConfigurationManager.AppSettings["MainIndexPage"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                
                string nombImg = DBFunctions.SingleData("SELECT CEMP_NOMBRE FROM CEMPRESA;");
                imgEmpresa.ImageUrl = "http://ecas.co/images/" + GlobalData.EMPRESA.ToLower() + ".png";//"../img/imgEmpresasReportes/" + nombImg + ".png";
                lbEmpresa.Text = nombImg;
                //almacenes
                DataSet ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT PALM_ALMACEN,PALM_DESCRIPCION FROM PALMACEN WHERE PCEN_CENTVVN != '' OR PCEN_CENTVVU != ''");
                for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ListItem newItem = new ListItem(ds.Tables[0].Rows[i]["PALM_DESCRIPCION"].ToString(), ds.Tables[0].Rows[i]["PALM_ALMACEN"].ToString());
                    rdbAlmacenes.Items.Add(newItem);
                }
                if (Request["exito"] == "1")
                    Utils.MostrarAlerta(Response, "Se ha actualizado la información correctamente!!");
                else if (Request["error"] == "1")
                    Utils.MostrarAlerta(Response, "Se ha generado un error desconocido. \\nPor Favor vuelva a realizar el proceso");
                //rdbAlmacenes.Items[0].Selected = true;
                //rdbAlmacenes.Width = 100;
            }
            else
            {
                /*
                    string tales = Request["__EVENTARGUMENT"];
                    if (tales != null && tales != "")
                    {
                        txtNit.Text = tales;
                        cargarInfoNit(sender, e);
                    }
                */
                if (Session["tablaVend"] != null)
                {
                    tablaVendedores = (DataTable)Session["tablaVend"];
                }
                else
                    cargarVendedores();
            }

        }
        protected void cargarInfoNit(object sender, EventArgs z)
        {
            bool noExiste = true;
            string tipoNit = "";//mnit o mnitcotizacion
            noExiste = existeNit(txtNit.Text, ref tipoNit);
            if (noExiste || tipoNit.Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe digitar un NIT existente");
                divInfo.Visible = false;
                txtNit.Text = "";
            } 
            else
            {
                if(chkVendedores.Items.Count == 0)
                {
                    Utils.MostrarAlerta(Response, "Debe escoger un almacen con vendedores!");
                    divInfo.Visible = false;
                    txtNit.Text = "";
                }
                else
                {
                    string nombreCompleto = DBFunctions.SingleData("SELECT MNIT_NOMBRES || \'~\' || MNIT_NOMBRE2 || \'~\' || MNIT_APELLIDOS || \'~\' || MNIT_APELLIDO2|| \'~\' || MNIT_DIRECCION || \'~\' || MNIT_TELEFONO || \'~\' || MNIT_CELULAR || \'~\' || MNIT_EMAIL FROM " + tipoNit + " WHERE MNIT_NIT = '" + txtNit.Text + "'");
                    txtNomb1.Text = nombreCompleto.Split('~')[0];
                    txtNomb2.Text = nombreCompleto.Split('~')[1];
                    txtApe1.Text = nombreCompleto.Split('~')[2];
                    txtApe2.Text = nombreCompleto.Split('~')[3];
                    txtDireccion.Text = nombreCompleto.Split('~')[4];
                    txtTelefono.Text = nombreCompleto.Split('~')[5];
                    txtCelular.Text = nombreCompleto.Split('~')[6];
                    txtCorreo.Text = nombreCompleto.Split('~')[7];
                    divInfo.Visible = true;
                    Session["TablaNit"] = tipoNit;
                }
                /*
                    if(tipoNit== "MNIT")
                    {

                    }else if(tipoNit == "MNITCOTIZACION")
                    {

                    }
                */
            }
        }
        protected bool existeNit(string nit, ref string tipoNit)
        {
            //magia
            bool rta = true;
            if (DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT = '" + nit + "'"))
            {
                tipoNit = "MNIT";
                rta = false;
            }
            else if (DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNITCOTIZACION WHERE MNIT_NIT = '" + nit + "'"))
            {
                tipoNit = "MNITCOTIZACION";
                rta = false;
            }
            return rta;
        }

        protected void cambioVendedor(object sender, EventArgs z)
        {
            //chkVendedores.ClearSelection();
            chkVendedores.Items.Clear();
            vendedores = tablaVendedores.Select("ALMACEN = '" + rdbAlmacenes.SelectedValue + "'");
            for (int i = 0; i < vendedores.Length; i++)
            {
                ListItem newItem = new ListItem(vendedores[i].ItemArray[1].ToString(), vendedores[i].ItemArray[0].ToString());
                chkVendedores.Items.Add(newItem);
            }
        }

        protected void guardarInfo(object sender, EventArgs z)
        {
            if (txtNit.Text == "" || chkVendedores.Items.Count == 0 || chkVendedores.SelectedValue == "")
                Utils.MostrarAlerta(Response, "El nit no puede estár vacio y no olvide elegir un vendedor!");
            else
            {
                try
                {
                    tablaNit = Session["TablaNit"].ToString();
                }catch
                {
                    Session.Clear();
                    Response.Redirect(index + "?process=Vehiculos.AnfitrionVentas&error=1");
                }
                codVen = chkVendedores.SelectedValue;
                sede = rdbAlmacenes.SelectedValue;
                fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                usuaio = HttpContext.Current.User.Identity.Name.ToLower();
                ArrayList sql = new ArrayList();
                sql.Add("UPDATE " + tablaNit + " SET MNIT_NOMBRES = '" + txtNomb1.Text + "', MNIT_NOMBRE2 = '" + txtNomb2.Text
                        + "', MNIT_APELLIDOS = '" + txtApe1.Text + "', MNIT_APELLIDO2 = '" + txtApe2.Text + "', MNIT_DIRECCION = '" + txtDireccion.Text 
                        + "', MNIT_TELEFONO = '" + txtTelefono.Text + "', MNIT_CELULAR = '" + txtCelular.Text + "' WHERE MNIT_NIT = '" + txtNit.Text + "';");
                sql.Add("INSERT INTO DANFITRION (DANF_ID, PVEN_CODIGO, PALM_ALMACEN, MNIT_NIT, DANF_FECHA, SUSU_LOGIN) VALUES ( " +
                        "DEFAULT, " +
                        "'" + codVen + "', " +
                        "'" + sede + "', " +
                        "'" + txtNit.Text + "', " +
                        "'" + fecha + "', " +
                        "'" + usuaio + "' );");

                if(DBFunctions.Transaction(sql))
                {
                    Session.Clear();
                    Response.Redirect(index + "?process=Vehiculos.AnfitrionVentas&exito=1");
                }
                else
                {
                    lbError.Text = "Se generó un problema al insertar guardar los datos: " + DBFunctions.exceptions;
                    Utils.MostrarAlerta(Response, "Falló el proceso, por favor verifique que haya llenado toda la información de forma correcta. \\nDebe elegir un almacen. \\nEscoger un vendedor. \\nAsignar un NIT.");
                }

            }
        }

        protected void cargarVendedores()
        {
            tablaVendedores = DBFunctions.Request(new DataSet(), IncludeSchema.NO, "SELECT P.PVEN_CODIGO AS CODIGO, P.PVEN_NOMBRE AS NOMBRE, PA.PALM_ALMACEN AS ALMACEN FROM DBXSCHEMA.PVENDEDOR P, DBXSCHEMA.PVENDEDORALMACEN PA WHERE P.PVEN_CODIGO = PA.PVEN_CODIGO AND P.PVEN_VIGENCIA = 'V' AND TVEND_CODIGO IN('VV','TT');").Tables[0];
            Session["tableVend"] = tablaVendedores;
        }
    }
}