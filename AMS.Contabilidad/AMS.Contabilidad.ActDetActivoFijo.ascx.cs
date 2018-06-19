using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AMS.Forms;
using AMS.Tools;
using AMS.DB;
using System.Collections;
using System.Configuration;

namespace AMS.Contabilidad
{
    public partial class ActDetActivoFijo : System.Web.UI.UserControl
    {

        DatasToControls bind = new DatasToControls();
        ArrayList sqlUpdate = new ArrayList();
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected static ArrayList archivos = new ArrayList();
        protected string path = ConfigurationSettings.AppSettings["PathToUploads"];


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bind.PutDatasIntoDropDownList(ddlActivoFijo, "SELECT MAFJ_CODIACTI, MAFJ_CODIACTI CONCAT ' - ' CONCAT MAFJ_DESCRIPCION FROM MACTIVOFIJO WHERE TVIG_VIGENCIA = 'V';");
                if (ddlActivoFijo.Items.Count > 0)
                {
                    ddlActivoFijo.Items.Insert(0, new ListItem("Seleccione.."));
                }
                else
                {
                    Utils.MostrarAlerta(Response, "Debe parametrizar la tabla de Activos fijos");
                    btnCargar.Enabled = false;
                }
                if (Request.QueryString["sucess"] == "1")
                {
                    Utils.MostrarAlerta(Response, "Se han actualizado los valores del Activo Fijo " + Request.QueryString["actFijo"] + " !");
                }
            }
        }
        //Carga deterioro y Residual
        public void cargaTexto(object sender, EventArgs E)
        {
            if(ddlActivoFijo.SelectedIndex == 0)
            {
                Utils.MostrarAlerta(Response, "Debe seleccionar un activo fijo, Revise por favor..");
                return;
            }
            fldActivos.Visible = true;
            txtDeterioroActual.Text = DBFunctions.SingleData("SELECT MAFJ_VALODETENIIF FROM MACTIVOFIJO WHERE MAFJ_CODIACTI = '" + ddlActivoFijo.SelectedValue + "'");
            txtResidualActual.Text = DBFunctions.SingleData("SELECT MAFJ_VALORECINIIF FROM MACTIVOFIJO WHERE MAFJ_CODIACTI = '" + ddlActivoFijo.SelectedValue + "'");
            txtMercado.Text = DBFunctions.SingleData("SELECT MAFJ_VALOACTNIIF FROM MACTIVOFIJO WHERE MAFJ_CODIACTI = '" + ddlActivoFijo.SelectedValue + "'");
            lbDepreciacion.Text = DBFunctions.SingleData("SELECT MAFJ_DEPRACUM FROM MACTIVOFIJO WHERE MAFJ_CODIACTI = '" + ddlActivoFijo.SelectedValue + "'");
            lbAvaluador.Text = DBFunctions.SingleData("SELECT MAFJ_AVALUADOR FROM MACTIVOFIJO WHERE MAFJ_CODIACTI = '" + ddlActivoFijo.SelectedValue + "'");
            lbFechaA.Text = DBFunctions.SingleData("SELECT MAFJ_FECHAVAL FROM MACTIVOFIJO WHERE MAFJ_CODIACTI = '" + ddlActivoFijo.SelectedValue + "'");
            btnCargar.Enabled = false;
        }

        //botón Editar para modificar fechas - habilita tetxBox
        public void habilitaFld(object sender, EventArgs E)
        {
            fldEdit.Visible = true;
            btnEditar.Enabled = false;
            btnGuardar.Visible = true;
            try
            {
                txtDeterioroNuevo.Text = txtDeterioroActual.Text.Substring(0, txtDeterioroActual.Text.Length - 3);
            }catch(Exception e)
            {

            }
            try
            {
                txtResidualNuevo.Text = txtResidualActual.Text.Substring(0, txtResidualActual.Text.Length - 3);
            }
            catch (Exception e)
            {

            }
            try
            {
                txtNuevoMercado.Text = txtMercado.Text.Substring(0, txtMercado.Text.Length - 3);
            }
            catch (Exception e)
            {

            }
            
            
        }

        //Guardar
        public void guardarDatos(object sender, EventArgs E)
        {

            string anexos = lbArchivos.Text.Replace("<br>", "; ") ;
            var usuario = HttpContext.Current.User.Identity.Name.ToLower();
            var fecha = DateTime.Now.ToString("yyyy-MM-dd");
            ArrayList sqlReady = new ArrayList();
            sqlReady.Add("UPDATE MACTIVOFIJO SET MAFJ_VALORECINIIF = " + txtResidualNuevo.Text.Replace(",", "") + ", MAFJ_VALODETENIIF = " + txtDeterioroNuevo.Text.Replace(",", "") + ", MAFJ_VALOACTNIIF = " + txtNuevoMercado.Text.Replace(",", "") + ", MAFJ_AVALUADOR = '" + txtAvaluador.Text + "', MAFJ_FECHAVAL = '" + TxtFechAval.Text + "' WHERE MAFJ_CODIACTI = '" + ddlActivoFijo.SelectedValue + "';");
            sqlReady.Add("INSERT INTO DACTIVOFIJOVALORES " +
                        " ( MAFJ_CODIACTI, " +
                            "DAFJ_FECHA, " +
                            "MAFJ_VALORECIORIGNIIF, " +
                            "MAFJ_VALODETEORIGNIIF, " +
                            "MAFJ_VALOACTORIGNIIF, " +
                            "MAFJ_VALORECINIIF, " +
                            "MAFJ_VALODETENIIF, " +
                            "MAFJ_VALOACTNIIF, " +
                            "DAFJ_OBSERVACION, " +
                            "DAFJ_USUARIO, " +
                            "DAFJ_AVALUADOR, " +
                            "DAFJ_FECHAVAL, " +
                            "DAFJ_ANEXO" +
                        " )VALUES ('" + ddlActivoFijo.SelectedValue + "', '" +
                                        fecha + "', " +
                                        txtResidualActual.Text.Replace(",", "") + ", " +
                                        txtDeterioroActual.Text.Replace(",", "") + ", " +
                                        txtMercado.Text.Replace(",", "") + ", " +
                                        txtResidualNuevo.Text.Replace(",", "") + ", " + 
                                        txtDeterioroNuevo.Text.Replace(",", "") + ", " +
                                        txtNuevoMercado.Text.Replace(",", "") + ", '" +
                                        txtObs.Value + "', '" + 
                                        usuario + "', '" + 
                                        txtAvaluador.Text + "', '" + 
                                        TxtFechAval.Text + "', '" + 
                                        anexos + "' )");
            //Actualizamos registro y confirmamos que se haya hecho el update
            if (DBFunctions.Transaction(sqlReady))
            {
                //Si guarda los registro, pues guarda el archivo 
                //Se inserta el registro.
                for (int i = 0; i < archivos.Count; i++)
                {
                    string[] nombre = ((HttpPostedFile)archivos[i]).FileName.Split('\\');
                    ((HttpPostedFile)archivos[i]).SaveAs(path + new Random().Next(0, 9999) + nombre[nombre.Length - 1]);
                }
                Response.Redirect(indexPage + "?process=Contabilidad.ActDetActivoFijo&sucess=1&actFijo=" + ddlActivoFijo.SelectedValue);
            }
            else
            {
                Utils.MostrarAlerta(Response, "Ha ocurrido un problema con el registro, Revise que los datos estén bien ingresados");
                return;
            }

        }

        //Agregar elementos en el label que verá el clientey en el arrayList
        protected void btnAgregar_Click(object Sender, EventArgs e)
        {
            if (this.uplFile.PostedFile.FileName.Length == 0)
                Utils.MostrarAlerta(Response, "No ha especificado un nombre de archivo");
            else
            {
                HttpPostedFile file = uplFile.PostedFile;
                archivos.Add(file);
                Session["archivos"] = archivos;
                lbArchivos.Text +=  file.FileName + "<br>";
            }
            //Ln 221
        }

    }
}