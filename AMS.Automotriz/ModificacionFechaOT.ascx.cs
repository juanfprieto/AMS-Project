using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using AMS.Tools;
using AMS.DB;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;

namespace AMS.Automotriz
{
    public partial class ModificacionFechaOT : System.Web.UI.UserControl
    {
        DatasToControls bind = new DatasToControls();
        ArrayList sqlUpdate = new ArrayList();
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind.PutDatasIntoDropDownList(ddlTipoOT, "SELECT PDOC_CODIGO, PDOC_CODIGO CONCAT ' - ' CONCAT PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'OT';");
                if (ddlTipoOT.Items.Count > 0)
                {
                    ddlTipoOT.Items.Insert(0, new ListItem("Seleccione.."));
                }
                if (Request.QueryString["sucess"] == "1")
                {
                    Utils.MostrarAlerta(Response, "Se ha actualizado la Fecha de Entrega de la orden " + Request.QueryString["tipoOrden"] + " - " + Request.QueryString["numOrden"] + " !");
                }
            }
        }

        //Onchange
        public void cambioOT(object sender, EventArgs E)
        {
            bind.PutDatasIntoDropDownList(ddlNumeroOT, "SELECT MORD_NUMEORDE, PDOC_CODIGO CONCAT ' - ' CONCAT MORD_NUMEORDE FROM MORDEN WHERE PDOC_CODIGO = '" + ddlTipoOT.SelectedValue + "' AND TEST_ESTADO = 'A' ");
        }

        //método cargar la fecha y hora de la OT seleccionada
        public void cargaFecha(object sender, EventArgs E)
        {
            if (ddlTipoOT.Items.Count == 0 || ddlNumeroOT.Items.Count == 0)
            {
                Utils.MostrarAlerta(Response, "Debe seleccionar el Tipo OT y el Número de la OT");
                return;
            }
            else
            {
                ddlTipoOT.Enabled = false;
                ddlNumeroOT.Enabled = false;
                fldFecha.Visible = true;

                string fecha = DBFunctions.SingleData("SELECT MORD_ENTREGAR FROM MORDEN WHERE PDOC_cODIGO = '" + ddlTipoOT.SelectedValue + "' AND MORD_NUMEORDE = " + ddlNumeroOT.SelectedValue);
                DateTime fecha1 = Convert.ToDateTime(fecha);
                txtFechaActualOT.Text = fecha1.ToString("yyyy-MM-dd");
                txtHoraActuaLOT.Text = DBFunctions.SingleData("SELECT MORD_HORAENTG FROM MORDEN WHERE PDOC_cODIGO = '" + ddlTipoOT.SelectedValue + "' AND MORD_NUMEORDE = " + ddlNumeroOT.SelectedValue);
                btnCargar.Enabled = false;
            }
        }

        //botón Editar para modificar fechas - habilita tetxBox
        public void habilitaTxt(object sender, EventArgs E)
        {
            string fecha = DBFunctions.SingleData("SELECT MORD_ENTREGAR FROM MORDEN WHERE PDOC_cODIGO = '" + ddlTipoOT.SelectedValue + "' AND MORD_NUMEORDE = " + ddlNumeroOT.SelectedValue);
            DateTime fecha1 = Convert.ToDateTime(fecha);
            tablaNueva.Visible = true;
            txtFechaNuevaOT.Text = fecha1.ToString("yyyy-MM-dd");
            txtHoraNuevaOT.Text = DBFunctions.SingleData("SELECT MORD_HORAENTG FROM MORDEN WHERE PDOC_cODIGO = '" + ddlTipoOT.SelectedValue + "' AND MORD_NUMEORDE = " + ddlNumeroOT.SelectedValue);
            btnEditar.Enabled = false;
            btnGuardar.Visible = true;

        }

        //Guardar
        public void guardarDatos(object sender, EventArgs E)
        {
            int leng = txtHoraNuevaOT.Text.Length;
            string tabla = " MORDEN";
            //Guardar el registro en MhistorialCambios.
            //creamos e inicializamos variables que serán insertadas:
            var operacion = "U";
            string consulta = "SELECT 'TIPO ORDEN: ' CONCAT PDOC_cODIGO CONCAT ' / NUM ORDEN: ' CONCAT MORD_NUMEORDE CONCAT ' / NIT: ' CONCAT MNIT_NIT CONCAT ' / FECHA ENTREGA: ' CONCAT MORD_ENTREGAR CONCAT ' / HORA ENTREGA: ' CONCAT MORD_HORAENTG FROM MORDEN WHERE PDOC_CODIGO = '" + ddlTipoOT.SelectedValue + "' AND   MORD_NUMEORDE = " + ddlNumeroOT.SelectedValue + "";
            var originales = DBFunctions.SingleData(consulta);
            var usuario = HttpContext.Current.User.Identity.Name.ToLower();
            var fecha = DateTime.Now.ToString("yyyy-MM-dd");

            //insert en mhistorialcambios 
            //al borrar una orden, el registro debe de guardarse en esa tabla.
            sqlUpdate.Add("INSERT INTO MHISTORIAL_CAMBIOS (" +
                        "TABLA, OPERACION, ORIGINALES, SUSU_USUARIO, FECHA) " +
                        " VALUES( '" +
                        tabla + "', '" + operacion + "', '" + originales + "', '" + usuario + "', '" + fecha + "')");
            if (leng != 8 || !txtHoraNuevaOT.Text.Substring(2, 1).Contains(":") || !txtHoraNuevaOT.Text.Substring(5, 1).Contains(":"))
            {
                Utils.MostrarAlerta(Response, "Revise la hora teniendo en cuenta el Formato en el que debe ir:  HH:mm:ss (00:00:00)");
                return;
            }
            else
            {
                //DialogResult dr = MessageBox.Show("Message.", "Title", MessageBoxButtons.YesNoCancel,
                //MessageBoxIcon.Information);

                ////Utils.MostrarPregunta(Response, "Está seguro?");


                //if (dr == DialogResult.Yes)
                //{
                //    Utils.MostrarAlerta(Response, "aceptó");
                //}
                //else
                //{
                //    Utils.MostrarAlerta(Response, "Canceló");
                //}
                //So desea continuar entonces realizamos la operación ..
                sqlUpdate.Add("UPDATE MORDEN SET MORD_ENTREGAR = '" + txtFechaNuevaOT.Text + "', MORD_HORAENTG = '" + txtHoraNuevaOT.Text + "' WHERE PDOC_CODIGO = '" + ddlTipoOT.SelectedValue + "' AND MORD_NUMEORDE = " + ddlNumeroOT.SelectedValue);
                if (DBFunctions.Transaction(sqlUpdate))
                {

                    Response.Redirect(indexPage + "?process=Automotriz.ModificacionFechaOT&sucess=1&tipoOrden=" + ddlTipoOT.SelectedValue + "&numorden=" + ddlNumeroOT.SelectedValue);
                }
                else
                {
                    Utils.MostrarAlerta(Response, "Ocurrió un probema Actualizando la base de datos..");
                }
            }
        }
    }
}