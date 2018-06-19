using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using AMS.DB;
using AMS.Tools;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;

namespace AMS.Asousados
{
	public partial class IngresoVehiculo : System.Web.UI.UserControl
	{
        private string pathToUploads = ConfigurationManager.AppSettings["PathToUploads"];
        private string pathToUploadsLarge = ConfigurationManager.AppSettings["PathToUploadsLargeImg"];
        private string pathToUploadsSmall = ConfigurationManager.AppSettings["PathToUploadsSmallImg"];
        private string pathToUploadsThumb = ConfigurationManager.AppSettings["PathToUploadsThumbImg"];

        private ArrayList images;
        private string idVehiculo;
        private string idOferente;
        private string idUbicacionActual;
        private bool esOferente, modifica, Consultar;
        public string indexPage = ConfigurationManager.AppSettings["MainAjaxPage"];
        private Int32 llaveVehiculoOfertado;
        Button btnEliminar = new Button();
        protected string mainPage=ConfigurationSettings.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            DataSet ds = new DataSet();
            string sql = String.Format("SELECT MNIT_NIT, MNIT_NIT2  FROM MCONCESIONARIO WHERE MNIT_NIT = (SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')");
            ds =  DBFunctions.Request(ds, IncludeSchema.NO, sql);;
            if (ds.Tables[0].Rows.Count != 0)
            {
                string valu = ds.Tables[0].Rows[0][0].ToString();
                string valu2 = ds.Tables[0].Rows[0][1].ToString();

                if (valu != valu2)
                    Response.Redirect(mainPage + "?process=Asousados.ConsultaDeVehiculosOfertados&codAso=1");
            }

            esOferente = Request.QueryString["oferente"] != null && Request.QueryString["oferente"] != "";
            modifica = Request.QueryString["modifica"] != null && Request.QueryString["modifica"] != "";
            Consultar = Request.QueryString["Consultar"] != null && Request.QueryString["Consultar"] != "";
            if (modifica)
            {
                btnInsertar.Visible = false;
                btnEditar.Visible = true;
            }
            else if (Consultar)
            {
                btnInsertar.Visible = false;
                btnEditar.Visible = false;
                btnUpload.Visible = false;
                btnEliminar.Visible = false;
                upFotos.Visible = false;
                txtPlaca.ReadOnly = true;
                txtOferenteNombre.ReadOnly = true;
                txtOferenteTelefono.ReadOnly = true;
                txtOferenteCelular.ReadOnly = true;
                txtOferenteMail.ReadOnly = true;
                txtKilometraje.ReadOnly = true;
                txtPrecio.ReadOnly = true;
                txtInfoAdicional.ReadOnly = true;
                ddlCiudad.Enabled = false;
                ddlColor.Enabled = false;
                ddlModelo.Enabled = false;
                ddlServicio.Enabled = false;
                LbAgregar.Visible = false;
                btnVolver.Visible = true;
            }

            if (!IsPostBack)
            {
                ViewState["id"] = Request.QueryString["id"];
                idVehiculo = ViewState["id"] != null ? ViewState["id"].ToString() : null;
                ViewState["images"] = images = new ArrayList();
                //agregarFiltros();
                llenarDDLs();

                CargarDatosEdicion(idVehiculo);
                llenarddlEstado();
                habilitarPlaceHolders();
                ddlClaseVeh_OnSelectedIndexChanged(sender, e); //gestionar opcciones de catalogo
            }
            //else
            //{
            //    var result = Request.Params["__EVENTARGUMENT"];
            //}

            idVehiculo = ViewState["id"] != null ? ViewState["id"].ToString() : null;
            images = (ArrayList)ViewState["images"];
            idOferente = (string)ViewState["idOferente"];
            CargarImagenes();
            if (Request.QueryString["OKplaca"] != null)
                txtPlaca.Text = Request.QueryString["OKplaca"];
		}

        
        protected void ddlAsociado_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string NIT_ususario = DBFunctions.SingleData("SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
            if (ddlAsociado.SelectedValue == "0")
                if (NIT_ususario == "")
                {
                    //ddlUbicacion.Items.Clear();
                    Utils.FillDll(
                          ddlUbicacion,
                          String.Format("select pubi_secuencia, pubi_nombre from pubicacion order by pubi_nombre"),
                          true);
                }
                else
                {
                    Utils.FillDll(
                         ddlUbicacion,
                         String.Format("select pubi_secuencia, pubi_nombre from pubicacion where mnit_nitconcesionario= '"+NIT_ususario+"' order by pubi_nombre"),
                         true);
                }

            else
                Utils.FillDll(
                    ddlUbicacion, 
                    String.Format("select pubi_secuencia, pubi_nombre from pubicacion where mnit_nitconcesionario='{0}' order by pubi_nombre", 
                                    ddlAsociado.SelectedValue), 
                    true);
        }

        protected void ddlEstado_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            habilitarPlaceHolders();
        }

        protected void ddlClaseVeh_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT PMV.PMAR_CODIGO, \n" +
             "       PMV.PMAR_NOMBRE \n" +
             "FROM PMARCA PMV, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE PMV.PMAR_CODIGO = PCV.PMAR_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}'"
             , ddlClaseVeh.SelectedValue);

            Utils.FillDll(ddlMarca, sql, true);
            ddlMarca_OnSelectedIndexChanged(sender, e); //reiniciará los ddl a partir de la selección
        }

        protected void ddlMarca_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT PGC.PGRU_GRUPO, \n" +
             "       PGC.PGRU_NOMBRE \n" +
             "FROM PGRUPOCATALOGO PGC, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE PGC.PGRU_GRUPO = PCV.PGRU_GRUPO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue);

            Utils.FillDll(ddlRefPrincipal, sql, true);

            ddlRefPrincipal_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlRefPrincipal_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT PSGC.PSGRU_GRUPO, " +
             "       PSGC.PSGRU_NOMBRE " +
             "FROM PCATALOGOVEHICULO PCV  " +
             "  LEFT JOIN PSUBGRUPOVEHICULO PSGC ON PSGC.PSGRU_GRUPO = PCV.PSGRU_CODIGO " +
             "WHERE PCV.PGRU_GRUPO = '{0}'  AND PCV.PMAR_CODIGO = '{1}' and PCV.pcla_codigo = {2}"
             , ddlRefPrincipal.SelectedValue, ddlMarca.SelectedValue,  ddlClaseVeh.SelectedValue);

            Utils.FillDll(ddlRefComplementaria, sql, true);

            ddlRefComplementaria_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlRefComplementaria_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             "SELECT DISTINCT PC.PCAR_CODIGO, \n" +
             "       PC.PCAR_NOMBRE \n" +
             "FROM PCARROCERIA PC, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE PC.PCAR_CODIGO = PCV.PCAR_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}' \n" +
             "AND   PCV.PGRU_GRUPO = '{2}' \n"+
             "AND   PCV.PSGRU_CODIGO = '{3}' "
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             );

            Utils.FillDll(ddlCarroceria, sql, true);
            ddlCarroceria_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlCarroceria_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlCarroceria.SelectedValue == "")
            //    return;
            string sql = String.Format(
             "SELECT DISTINCT RC.RCIL_CODIGO, \n" +
             "       RC.RCIL_NOMBRE \n" +
             "FROM RCILINDRAJEMOTOR RC, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE RC.RCIL_CODIGO = PCV.RCIL_CODIGO \n" +
             "AND   PCV.PCLA_CODIGO = '{0}' \n" +
             "AND   PCV.PMAR_CODIGO = '{1}' \n" +
             "AND   PCV.PGRU_GRUPO = '{2}' \n" +
             "AND   PCV.PSGRU_CODIGO = '{3}' \n" +
             "AND   PCV.PCAR_CODIGO = '{4}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue);

            Utils.FillDll(ddlCilindraje, sql, true);
            ddlCilindraje_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlCilindraje_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             @"SELECT DISTINCT pcm.PCOM_CODIGO, 
                    pcm.PCOM_NOMBRE 
             FROM PcombustibleMOTOR pcm, 
                  PCATALOGOVEHICULO PCV 
             WHERE pcm.PCOM_CODIGO = PCV.PCOM_CODIGO 
             AND   PCV.PCLA_CODIGO = '{0}' 
             AND   PCV.PMAR_CODIGO = '{1}' 
             AND   PCV.PGRU_GRUPO = '{2}' 
             AND   PCV.PsGRU_CODIGO = '{3}' 
             AND   PCV.PCAR_CODIGO = '{4}'
            AND   PCV.RCIL_CODIGO = '{5}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue
             , ddlCilindraje.SelectedValue);

            Utils.FillDll(ddlTipoCombustible, sql, true);
            ddlTipoCombustible_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlTipoCombustible_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             @"SELECT DISTINCT Ra.Rali_CODIGO, 
                    Ra.Rali_NOMBRE 
            FROM RalimentacionMOTOR Ra, 
                  PCATALOGOVEHICULO PCV 
             WHERE Ra.Rali_CODIGO = PCV.Rali_CODIGO 
             AND   PCV.PCLA_CODIGO = '{0}' 
             AND   PCV.PMAR_CODIGO = '{1}' 
            AND   PCV.PGRU_GRUPO = '{2}' 
             AND   PCV.PSGRU_CODIGO = '{3}' 
             AND   PCV.PCAR_CODIGO = '{4}' 
             AND   PCV.RCIL_CODIGO = '{5}'
            AND   PCV.PCOM_CODIGO = '{6}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue
             , ddlCilindraje.SelectedValue
             , ddlTipoCombustible.SelectedValue);

            Utils.FillDll(ddlAspiracion, sql, true);
            ddlAspiracion_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlAspiracion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             @"SELECT DISTINCT Rt.Rtra_CODIGO, 
                    Rt.Rtra_DESCRIPC 
             FROM RtraccionVEHICULO Rt, 
                  PCATALOGOVEHICULO PCV 
             WHERE Rt.Rtra_CODIGO = PCV.Rtra_CODIGO 
             AND   PCV.PCLA_CODIGO = '{0}' 
             AND   PCV.PMAR_CODIGO = '{1}' 
             AND   PCV.PGRU_GRUPO = '{2}' 
             AND   PCV.PSGRU_CODIGO = '{3}' 
             AND   PCV.PCAR_CODIGO = '{4}'
             AND   PCV.RCIL_CODIGO = '{5}'
            AND   PCV.PCOM_CODIGO = '{6}'
            AND   PCV.RALI_CODIGO = '{7}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue
             , ddlCilindraje.SelectedValue
             , ddlTipoCombustible.SelectedValue
             , ddlAspiracion.SelectedValue);

            Utils.FillDll(ddlTraccion, sql, true);
            ddlTraccion_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlTraccion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
             @"SELECT DISTINCT RCC.RCAJ_CODIGO, 
                    RCC.RCAJ_NOMBRE 
             FROM RcajaCAMBIOS RCC, 
                  PCATALOGOVEHICULO PCV 
             WHERE RCC.RCAJ_CODIGO = PCV.RCAJ_CODIGO 
             AND   PCV.PCLA_CODIGO = '{0}' 
             AND   PCV.PMAR_CODIGO = '{1}' 
             AND   PCV.PGRU_GRUPO = '{2}' 
             AND   PCV.PSGRU_CODIGO = '{3}' 
             AND   PCV.PCAR_CODIGO = '{4}'
            AND   PCV.RCIL_CODIGO = '{5}'
            AND   PCV.PCOM_CODIGO = '{6}'
            AND   PCV.RALI_CODIGO = '{7}'
            AND   PCV.RTRA_CODIGO = '{8}'"
             , ddlClaseVeh.SelectedValue
             , ddlMarca.SelectedValue
             , ddlRefPrincipal.SelectedValue
             , ddlRefComplementaria.SelectedValue
             , ddlCarroceria.SelectedValue
              , ddlCilindraje.SelectedValue
             , ddlTipoCombustible.SelectedValue
             , ddlAspiracion.SelectedValue
             , ddlTraccion.SelectedValue);

            Utils.FillDll(ddlCaja, sql, true);
            DatosCatalogo_OnSelectedIndexChanged(sender, e);
        }

        protected void DatosCatalogo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CambiarVisibilidadPHCatalogo();
            if (IsCatalogoFilled()) LlenarCatalogo();
        }

        protected void btnInsertar_Click(object sender, EventArgs e)
        {
            if (!validarDatos())
            {
                Utils.MostrarAlerta(Response, "Datos erroneos, por favor verifique");
                return;
            }

            string nit = ddlAsociado.SelectedValue == "0" ? "null" : String.Format("'{0}'", ddlAsociado.SelectedValue);
            string sql = "";

            if (esOferente)
            {           
                sql = String.Format(
                     "INSERT INTO DBXSCHEMA.MOFERENTE \n" +
                     "( \n" +
                     "  MOFE_NOMBRE, \n" +
                     "  MOFE_TELEFONO, \n" +
                     "  MOFE_CELULAR, \n" +
                     "  MOFE_EMAIL \n" +
                     ") \n" +
                     "VALUES ('{0}', '{1}', '{2}', '{3}')"
                     , txtOferenteNombre.Text
                     , txtOferenteTelefono.Text
                     , txtOferenteCelular.Text
                     , txtOferenteMail.Text);

                if (DBFunctions.NonQuery(sql) != 1)
                    lblError.Text = DBFunctions.exceptions;
                else
                {
                    idOferente = DBFunctions.SingleData("select max(MOFE_SECUENCIA) from MOFERENTE");
                    nit = "null";
                }
            }

            sql = String.Format(
             "INSERT INTO MVEHICULOUSADO (" +
             "  MNIT_NIT, " +
             "  PCAT_CODIGO, " +
             "  MCAT_PLACA, " +
             "  PCIU_CODIGO, " +
             "  PCOL_CODIGO, " +
             "  PANO_ANOMODE, " +
             "  TSER_TIPOSERV, " +
             "  MCAT_NUMEULTIKILO, " +
             "  MVEH_PRECVENT, " +
             "  PUBI_CODIGO, " +
             "  TEST_TIPOESTA, " +
             "  MVEH_FECHINGR, " +
             "  MVEH_OBSERVACION, " +
             "  TORI_CODIGO, " +
             "  MVEH_OFERENTE, " +
             "  PVEN_ASIGNADO, " +
             "  MVEH_SECUENCIA " +
             ") VALUES ( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, " +
             "  {10}, {11}, {12}, {13}, {14}, {15}, DEFAULT)"
             , nit
             , String.Format("'{0}'", ddlCatalogo.SelectedValue)
             , String.Format("'{0}'", txtPlaca.Text)
             , String.Format("'{0}'", ddlCiudad.SelectedValue)
             , String.Format("'{0}'", ddlColor.SelectedValue)
             , ddlModelo.SelectedValue
             , String.Format("'{0}'", ddlServicio.SelectedValue)
             , txtKilometraje.Text.Replace(",","")
             , txtPrecio.Text == "" ? "null" : txtPrecio.Text.Replace(",", "")
             , esOferente ? "null" : ddlUbicacion.SelectedValue
             , esOferente ? "20" : ddlEstado.SelectedValue
             , String.Format("'{0}'", esOferente ? DateTime.Now.ToString("yyyy-MM-dd") : txtFechaIngreso.Text)
             , txtInfoAdicional.Text == "" ? "null" : String.Format("'{0}'", txtInfoAdicional.Text)
             , esOferente ? "'P'" : String.Format("'{0}'", ddlPropiedad.SelectedValue)
             , esOferente ? idOferente : "null"
             , ObtenerVendedorPorCarrusel());


            if (DBFunctions.NonQuery(sql) != 1)
                lblError.Text = DBFunctions.exceptions;
            else
            {
                try
                {
                    if (ViewState["id"].ToString() != null)
                    {
                        llaveVehiculoOfertado = Convert.ToInt32(ViewState["id"].ToString());
                        sql = "UPDATE MVEHICULOUSADO SET TEST_TIPOESTA = 90 where mveh_secuencia = " + llaveVehiculoOfertado;
                        DBFunctions.NonQuery(sql);
                    }
                }
                catch (Exception erro)
                { }

                sql = String.Format("select MAX(mveh_secuencia) from MVEHICULOUSADO where mcat_placa='{0}' and pciu_codigo='{1}'",
                            txtPlaca.Text, ddlCiudad.SelectedValue);

                string vehiculo = DBFunctions.SingleData(sql);

                if (Request.QueryString["ingresoVeh"] != null)
                {
                    sql = "DELETE FROM MVEHICULOUSADOFOTO WHERE mveh_vehiculo = " + vehiculo;
                    DBFunctions.NonQuery(sql);
                }

                foreach(string imgData in images)
                {
                    if (images.IndexOf(imgData) == 0)
                        sql = String.Format(
                            "INSERT INTO MVEHICULOUSADOFOTO \n" +
                             "(MVEH_VEHICULO, MVEH_FOTO, MVEH_ESPORTADA) \n" +
                             "VALUES \n" +
                             "({0}, '{1}', 'S')"
                             , vehiculo
                             , imgData);
                    else
                        sql = String.Format(
                            "INSERT INTO MVEHICULOUSADOFOTO \n" +
                             "(MVEH_VEHICULO, MVEH_FOTO) \n" +
                             "VALUES \n" +
                             "({0}, '{1}')"
                             , vehiculo
                             , imgData);

                    DBFunctions.NonQuery(sql);
                }
                Utils.MostrarAlerta(Response, "Vehículo Guardado!");
                limpiarFormulario();
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            if (!validarDatos())
            {
                Utils.MostrarAlerta(Response, "Datos erroneos, por favor verifique");
                return;
            }

            ArrayList sqls = new ArrayList();
            string sql;
            string nit = ddlAsociado.SelectedValue == "0" || ddlAsociado.SelectedValue == "" ? "null" : String.Format("'{0}'", ddlAsociado.SelectedValue);

            if (esOferente)
            {
                sql = String.Format(
                    "UPDATE DBXSCHEMA.MOFERENTE \n" +
                     "   SET MOFE_NOMBRE = '{0}', \n" +
                     "       MOFE_TELEFONO = '{1}', \n" +
                     "       MOFE_CELULAR = '{2}', \n" +
                     "       MOFE_EMAIL = '{3}' \n" +
                     "WHERE MOFE_SECUENCIA = {4}"
                     , txtOferenteNombre.Text
                     , txtOferenteTelefono.Text
                     , txtOferenteCelular.Text
                     , txtOferenteMail.Text
                     , idOferente);

                sqls.Add(sql);
            }

            sql = String.Format(
             "UPDATE MVEHICULOUSADO \n" +
             "   SET MNIT_NIT = {0}, \n" +
             "       PCAT_CODIGO = {1}, \n" +
             "       MCAT_PLACA = {2}, \n" +
             "       PCIU_CODIGO = {3}, \n" +
             "       PCOL_CODIGO = {4}, \n" +
             "       PANO_ANOMODE = {5}, \n" +
             "       TSER_TIPOSERV = {6}, \n" +
             "       MCAT_NUMEULTIKILO = {7}, \n" +
             "       MVEH_PRECVENT = {8}, \n" +
             "       PUBI_CODIGO = {9}, \n" +
             "       TEST_TIPOESTA = {10}, \n" +
             (esOferente ? "{11}" : "MVEH_FECHINGR = {11}, \n") +
             "       MVEH_FECHVENT = {12}, \n" +
             "       MVEH_OBSERVACION = {13}, \n" +
             "       TORI_CODIGO = {14}, \n" +
             "       MVEH_NUMEFACVEN = {15}, \n" +
             "       MVEH_VALFACVEN = {16}, \n" +
             "       MVEH_FECHRETIRO = {17}, \n" +
             (!esOferente ? "MVEH_OFERENTE = null, \n" : "") + 
             (idUbicacionActual == ddlUbicacion.SelectedValue ? "{18}" : "PVEN_ASIGNADO = {18}, \n" ) +
             "       MVEH_MOTIRETIRO = {19} \n" +
             "WHERE MVEH_SECUENCIA = {20}"
             , nit
             , String.Format("'{0}'", ddlCatalogo.SelectedValue)
             , String.Format("'{0}'", txtPlaca.Text)
             , String.Format("'{0}'", ddlCiudad.SelectedValue)
             , String.Format("'{0}'", ddlColor.SelectedValue)
             , ddlModelo.SelectedValue
             , String.Format("'{0}'", ddlServicio.SelectedValue)
             , txtKilometraje.Text.Replace(",", "")
             , txtPrecio.Text == "" ? "null" : txtPrecio.Text.Replace(",", "")
             , esOferente ? "null" : ddlUbicacion.SelectedValue
             , esOferente ? "20" : ddlEstado.SelectedValue
             , esOferente ? "" : String.Format("'{0}'", txtFechaIngreso.Text)
             , txtFechaVenta.Text == "" ? "null" : String.Format("'{0}'", txtFechaVenta.Text)
             , txtInfoAdicional.Text == "" ? "null" : String.Format("'{0}'", txtInfoAdicional.Text)
             , esOferente ? "'P'" : String.Format("'{0}'", ddlPropiedad.SelectedValue)
             , txtNumFactura.Text == "" ? "null" : String.Format("'{0}'", txtNumFactura.Text)
             , txtValorVenta.Text == "" ? "null" : txtValorVenta.Text.Replace(",", "")
             , txtFechaRetiro.Text == "" ? "null" : String.Format("'{0}'", txtFechaRetiro.Text)
             , idUbicacionActual == ddlUbicacion.SelectedValue ? "" : ObtenerVendedorPorCarrusel()
             , txtMotivoRetiro.Text == "" ? "null" : String.Format("'{0}'", txtMotivoRetiro.Text)
             , idVehiculo);

            sqls.Add(sql);

            sql = String.Format(
                     "SELECT MVEH_SECUENCIA, \n" +
                     "       MVEH_FOTO, \n" +
                     "       MVEH_ESPORTADA \n" +
                     "FROM MVEHICULOUSADOFOTO \n" +
                     "WHERE MVEH_VEHICULO = {0}"
                     , idVehiculo);

            ArrayList fotos = DBFunctions.RequestAsCollection(sql);
            ArrayList fotosToAdd = new ArrayList();
            //ArrayList fotosToDel = new ArrayList();
            bool portadaEliminada = false;

            foreach (Hashtable foto in fotos)
            {
                string fotoDB = (string)foto["MVEH_FOTO"];
                bool found = false;

                foreach (string imgData in images)
                {
                    found = imgData == fotoDB;
                    if (found) break;
                }

                if (!found)
                {
                    int secuencia = (int)foto["MVEH_SECUENCIA"];
                    portadaEliminada = (string)foto["MVEH_ESPORTADA"] == "S";

                    sql = String.Format(
                     "DELETE FROM MVEHICULOUSADOFOTO \n" +
                     "WHERE MVEH_SECUENCIA = {0}"
                     , secuencia);

                    sqls.Add(sql);
                }
            }

            foreach (string imgData in images)
            {
                bool found = false;

                foreach (Hashtable foto in fotos)
                {
                    string fotoDB = (string)foto["MVEH_FOTO"];
                    found = imgData == fotoDB;
                    if (found) break;
                }

                if (!found)
                {
                    if (fotos.Count == 0 && images.IndexOf(imgData) == 0)
                        sql = String.Format(
                            "INSERT INTO MVEHICULOUSADOFOTO \n" +
                             "(MVEH_VEHICULO, MVEH_FOTO, MVEH_ESPORTADA) \n" +
                             "VALUES \n" +
                             "({0}, '{1}', 'S')"
                             , idVehiculo
                             , imgData);
                    else
                        sql = String.Format(
                            "INSERT INTO MVEHICULOUSADOFOTO \n" +
                             "(MVEH_VEHICULO, MVEH_FOTO) \n" +
                             "VALUES \n" +
                             "({0}, '{1}')"
                             , idVehiculo
                             , imgData);

                    sqls.Add(sql);
                }
            }
            if (portadaEliminada)
            {
                sql = String.Format("UPDATE (SELECT * \n" +
                 "FROM mvehiculousadofoto \n" +
                 "WHERE mveh_vehiculo = {0} fetch FIRST ROW ONLY) SET MVEH_ESPORTADA = 'S'"
                 , idVehiculo);
                sqls.Add(sql);
            }

            if (!DBFunctions.Transaction(sqls))
            {
                Utils.MostrarAlerta(Response, "Error al Guardar el Vehículo");
                lblError.Text = DBFunctions.exceptions;
                return;
            }
            

            string msj = "Vehículo Guardado!";

            String redirect = String.Format("{0}?process=Asousados.ListaUsadosEditar&eMsj={1}", indexPage, msj);
            if (esOferente)
                redirect += "&oferente=1";

            Response.Redirect(redirect);

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = (HttpPostedFile)Request.Files[0];

            if (!file.FileName.ToLower().Contains("jpg"))
            {
                Utils.MostrarAlerta(Response, "Seleccione una imagen con formato .jpg");
                return;
            }

            string fileName = "";
            
            do
                fileName = Path.GetRandomFileName().Split('.')[0] +".jpg";
            while (File.Exists(pathToUploads + fileName));

            Bitmap imgLarge = new Bitmap(file.InputStream);
            int width = imgLarge.Width;
            int height = imgLarge.Height;

            int heightSmall = 500 * height / width;
            int heightThumb = 100 * height / width;

            Bitmap imgSmall = Utils.ResizeImage(imgLarge, 500, heightSmall);
            Bitmap imgThumb = Utils.ResizeImage(imgLarge, 100, heightThumb);

            //imgLarge.Save(pathToUploadsLarge + fileName);
            //imgSmall.Save(pathToUploadsSmall + fileName);
            //imgSmall.Save(pathToUploads + fileName);
            //imgThumb.Save(pathToUploadsThumb + fileName);

            System.Drawing.Image imagenL = imgLarge;
            System.Drawing.Image imagenS = imgSmall;
            System.Drawing.Image imagenT = imgThumb;

            SaveJpeg(pathToUploadsLarge + fileName, imagenL, 45);
            SaveJpeg(pathToUploadsSmall + fileName, imagenS, 45);
            SaveJpeg(pathToUploads + fileName, imagenS, 45);
            SaveJpeg(pathToUploadsThumb + fileName, imagenT, 45);

            //Bitmap bmp = (Bitmap)Bitmap.FromFile(pathToUploadsLarge + "TH" + fileName);
            //SaveJpeg(pathToUploadsLarge + "LaTH" + fileName, Bitmap.FromFile(pathToUploadsLarge + "TH" + fileName), 40);
            
            images.Add(fileName);
            agregarImagen(fileName);

            ViewState["images"] = images;
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(((Button)sender).CommandName);

            images.RemoveAt(index);
            //phFotos.Controls.Clear();

            CargarImagenes();
        }

        protected void txtPlaca_OnTextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Z]{3}\d{3}$");
            Match m = regex.Match(txtPlaca.Text);
            if (!m.Success) 
            {
                string strA = String.Format("Por favor ingrese una placa valida!");
                string redirect = String.Format("{0}?process=Asousados.IngresoVehiculo&eMsj={1}", indexPage, strA);
                Response.Redirect(redirect);
            }

            if (esOferente || modifica) return;

            string placa = txtPlaca.Text;
            string sql = String.Format(
                "SELECT mv.mveh_secuencia AS ID,  \n" +
                "			 mv.test_tipoesta as ESTADO, \n" +
                "       mo.mofe_nombre AS NOMBRE,  \n" +
                "       mo.mofe_telefono AS TELEFONO,  \n" +
                "       mo.mofe_celular AS CELULAR,  \n" +
                "       mo.mofe_email AS MAIL, \n" +
                "       mn.mnit_nombres || ' ' || mn.mnit_apellidos AS ASOCIADO \n" +
                "FROM mvehiculousado mv   \n" +
                "  LEFT JOIN moferente mo ON mo.mofe_secuencia = mv.mveh_oferente  \n" +
                "  LEFT JOIN mnit mn ON mn.mnit_nit = mv.mnit_nit \n" +
                "WHERE mcat_placa = '{0}' ORDER BY  mv.test_tipoesta"
                , placa);

            ArrayList arrayVehiculos = DBFunctions.RequestAsCollection(sql);
            Response.Clear();

            if (arrayVehiculos != null && arrayVehiculos.Count > 0)
            {
                Hashtable vehiculo = (Hashtable)arrayVehiculos[0];
                string id = vehiculo["ID"].ToString();
                string estado = vehiculo["ESTADO"].ToString();
                string asociado = vehiculo["ASOCIADO"].ToString();
                string nombreOfertante = vehiculo["NOMBRE"].ToString();
                string strAlerta = "";
                llaveVehiculoOfertado = 0;

                if (estado == "20" && asociado != "") // vehículo en vitrina por otro asociado
                {
                    strAlerta = String.Format("Este vehículo ya está en la base de datos a nombre de {0} y no podrá ingresarlo. Por favor verifique la placa", asociado);

                    string redirect = String.Format("{0}?process=Asousados.IngresoVehiculo&eMsj={1}", indexPage, strAlerta);
                    Response.Redirect(redirect);
                }
                else
                {
                    ViewState["id"] = id;
                    llenarDDLs();
                    CargarDatosEdicion(id);
                    LimpiarCamposCompraOfertado();
                    //btnInsertar.Visible = false;
                    //btnEditar.Visible = true;

                    if (asociado != "") // vehículo alguna vez ingresado, pero ya no en venta
                    {
                        strAlerta = "Esta placa ya existe en la base de datos. Puede ser ingresado por usted, verifique datos y diligencie precio de venta, kilometraje y fecha de ingreso";
                        txtInfoAdicional.Text = "";
                    }
                    else // vehículo ofertado
                    {
                        btnEditar.Text = "Comprar";
                        strAlerta = String.Format("Este vehículo está ofertado a todos los asociados en el portal de OkCar por {0} (Datos de contacto: telf: {1}, cel: {2}, mail {3}). Si usted lo está ingresando, es porque lo acaba de comprar"
                            , nombreOfertante
                            , vehiculo["TELEFONO"].ToString()
                            , vehiculo["CELULAR"].ToString()
                            , vehiculo["MAIL"].ToString());
                    }
                }

                Utils.MostrarAlerta(Response, strAlerta);
            }
            else 
            {
                phVenta.Visible = true;
                string redirect = String.Format("{0}?process=Asousados.IngresoVehiculo&OKplaca={1}", indexPage, txtPlaca.Text);
                Response.Redirect(redirect); 
            }
        }

        private void LimpiarCamposCompraOfertado()
        {
            txtKilometraje.Text = "";
            txtPrecio.Text = "";
            ddlPropiedad.SelectedIndex = 0;
            ddlUbicacion.SelectedIndex = 0;
            //ddlUbicacion.Items.Clear();
            ddlEstado.SelectedIndex = 3;
            txtFechaIngreso.Text = "";
            txtFechaVenta.Text = "";
            txtNumFactura.Text = "";
            txtValorVenta.Text = "";
        }

        private void CargarDatosEdicion(string idVehiculo)
        {
            if (idVehiculo == null) return;

            //btnInsertar.Visible = false;
            // phVenta.Visible = true;

            string sql = String.Format(
             "SELECT MNIT_NIT, \n" +
             "       PCAT_CODIGO, \n" +
             (esOferente && Consultar ? "SUBSTR(MCAT_PLACA,6,1) AS MCAT_PLACA, \n" : 
             "       MCAT_PLACA, \n") +
             "       PCIU_CODIGO, \n" +
             "       PCOL_CODIGO, \n" +
             "       PANO_ANOMODE, \n" +
             "       TSER_TIPOSERV, \n" +
             "       MCAT_NUMEULTIKILO, \n" +
             "       MVEH_PRECVENT, \n" +
             "       PUBI_CODIGO, \n" +
             "       TEST_TIPOESTA, \n" +
             "       MVEH_FECHINGR, \n" +
             "       MVEH_FECHVENT, \n" +
             "       MVEH_OBSERVACION, \n" +
             "       TORI_CODIGO, \n" +
             "       MVEH_OFERENTE \n" +
             "FROM DBXSCHEMA.MVEHICULOUSADO \n" +
             "WHERE mveh_secuencia = {0}"
             , idVehiculo);

            ArrayList data = DBFunctions.RequestAsCollection(sql);
            if (data == null)
            {
                Utils.MostrarAlerta(Response, "Vehículo no Encontrado!");
                return;
            }

            //btnEditar.Visible = true;
            Hashtable vehiculo = (Hashtable)data[0];
            string nit = !Utils.IsNullOrEmpty(vehiculo["MNIT_NIT"]) ? vehiculo["MNIT_NIT"].ToString() : "0";

            if (esOferente)
            {
                idOferente = vehiculo["MVEH_OFERENTE"].ToString();
                ViewState["idOferente"] = idOferente;

                sql = String.Format(
                 "SELECT MOFE_NOMBRE, \n" +
                 "       MOFE_TELEFONO, \n" +
                 "       MOFE_CELULAR, \n" +
                 "       MOFE_EMAIL \n" +
                 "FROM DBXSCHEMA.MOFERENTE \n" +
                 "WHERE MOFE_SECUENCIA = '{0}'"
                 , idOferente);

                ArrayList dataOferente = DBFunctions.RequestAsCollection(sql);
                Hashtable oferente = (Hashtable)dataOferente[0];

                txtOferenteNombre.Text = !Utils.IsNullOrEmpty(oferente["MOFE_NOMBRE"]) ? oferente["MOFE_NOMBRE"].ToString() : "";
                txtOferenteTelefono.Text = !Utils.IsNullOrEmpty(oferente["MOFE_TELEFONO"]) ? oferente["MOFE_TELEFONO"].ToString() : "";
                txtOferenteCelular.Text = !Utils.IsNullOrEmpty(oferente["MOFE_CELULAR"]) ? oferente["MOFE_CELULAR"].ToString() : "";
                txtOferenteMail.Text = !Utils.IsNullOrEmpty(oferente["MOFE_EMAIL"]) ? oferente["MOFE_EMAIL"].ToString() : "";
            }
            else
            {
                if (ddlAsociado.Items.FindByValue(nit) != null) ddlAsociado.SelectedValue = nit;
                ddlAsociado_OnSelectedIndexChanged(null, null); // llena las ubicaciones del asociado
                ddlPropiedad.SelectedValue = !Utils.IsNullOrEmpty(vehiculo["TORI_CODIGO"]) ? vehiculo["TORI_CODIGO"].ToString() : "0";
                if (ddlUbicacion.Items.Count > 1)
                    ddlUbicacion.SelectedValue = !Utils.IsNullOrEmpty(vehiculo["PUBI_CODIGO"]) && ddlUbicacion.Items.FindByValue(vehiculo["PUBI_CODIGO"].ToString()) != null ? vehiculo["PUBI_CODIGO"].ToString() : "0";
                ddlEstado.SelectedValue = !Utils.IsNullOrEmpty(vehiculo["TEST_TIPOESTA"]) ? vehiculo["TEST_TIPOESTA"].ToString() : "0";
                txtFechaIngreso.Text = !Utils.IsNullOrEmpty(vehiculo["MVEH_FECHINGR"]) ? ((DateTime)vehiculo["MVEH_FECHINGR"]).ToString("yyyy-MM-dd") : "";
            }

            CargarCatalogo(vehiculo["PCAT_CODIGO"].ToString());
            txtPlaca.Text = !Utils.IsNullOrEmpty(vehiculo["MCAT_PLACA"]) ? vehiculo["MCAT_PLACA"].ToString() : "";
            ddlCiudad.SelectedValue = !Utils.IsNullOrEmpty(vehiculo["PCIU_CODIGO"]) ? vehiculo["PCIU_CODIGO"].ToString() : "0";
            ddlColor.SelectedValue = !Utils.IsNullOrEmpty(vehiculo["PCOL_CODIGO"]) ? vehiculo["PCOL_CODIGO"].ToString() : "0";
            ddlModelo.SelectedValue = !Utils.IsNullOrEmpty(vehiculo["PANO_ANOMODE"]) ? vehiculo["PANO_ANOMODE"].ToString() : "0";
            ddlServicio.SelectedValue = !Utils.IsNullOrEmpty(vehiculo["TSER_TIPOSERV"]) ? vehiculo["TSER_TIPOSERV"].ToString() : "0";
            txtKilometraje.Text = !Utils.IsNullOrEmpty(vehiculo["MCAT_NUMEULTIKILO"]) ? vehiculo["MCAT_NUMEULTIKILO"].ToString() : "";
            txtPrecio.Text = !Utils.IsNullOrEmpty(vehiculo["MVEH_PRECVENT"]) ? vehiculo["MVEH_PRECVENT"].ToString() : "";
            txtFechaVenta.Text = !Utils.IsNullOrEmpty(vehiculo["MVEH_FECHVENT"] ) ? ((DateTime)vehiculo["MVEH_FECHVENT"]).ToString("yyyy-MM-dd") : "";
            txtInfoAdicional.Text = !Utils.IsNullOrEmpty(vehiculo["MVEH_OBSERVACION"]) ? vehiculo["MVEH_OBSERVACION"].ToString() : "";

            idUbicacionActual = ddlUbicacion.SelectedValue;

            CargarImagenes(idVehiculo);
        }

        private void CargarImagenes(string idVehiculo)
        {
            string sql = String.Format(
             "SELECT MVEH_FOTO \n" +
             "FROM MVEHICULOUSADOFOTO \n" +
             "WHERE MVEH_VEHICULO = {0}"
             , idVehiculo);

            ArrayList fotos = DBFunctions.RequestAsCollection(sql);

            if (Request.QueryString["ingresoVeh"] == null)
                foreach (Hashtable foto in fotos)
                    images.Add(foto["MVEH_FOTO"]);

            //CargarImagenes();
        }

        //private void agregarImagen(byte[] imgData)
        //{
        //    Table table = new Table();
        //    TableRow row = new TableRow();
        //    TableCell cell = new TableCell();

        //    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //    img.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String(imgData);

        //    img.Width = 400;
        //    img.Height = 400;

        //    cell.Controls.Add(img);
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);
        //    row = new TableRow();
        //    cell = new TableCell();

        //    Button btnEliminar = new Button();
        //    btnEliminar.Text = "Eliminar";
        //    btnEliminar.CommandName = images.IndexOf(imgData).ToString();
        //    btnEliminar.Click += new EventHandler(btnEliminar_Click);

        //    cell.Controls.Add(btnEliminar);
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    phFotos.Controls.Add(table);
        //}

        private void agregarImagen(string filename)
        {
            Table table = new Table();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();

            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = "../uploads/" + filename;

            img.Width = 400;
            //img.Height = 400;

            cell.Controls.Add(img);
            row.Cells.Add(cell);
            table.Rows.Add(row);
            TableRow rowEl = new TableRow();
            TableCell cellEl = new TableCell();

            if (!Consultar) 
            {
                btnEliminar = new Button();
                btnEliminar.Text = "Eliminar";
                btnEliminar.CommandName = images.IndexOf(filename).ToString();
                btnEliminar.Click += new EventHandler(btnEliminar_Click);
            }

            cellEl.Controls.Add(btnEliminar);
            rowEl.Cells.Add(cellEl);
            table.Rows.Add(rowEl);

            phFotos.Controls.Add(table);
        }

        private void CargarImagenes()
        {
            phFotos.Controls.Clear();
            
            if(Request.QueryString["ingresoVeh"] == null)
                foreach (string imgData in images)
                    agregarImagen(imgData);
        }

        private bool validarDatos()
        {
            LimpiarcamposInvisibles();

            string campos = "";

            string sqlPlaca = "";
            if (idVehiculo != null && idVehiculo != "")
                sqlPlaca = String.Format("select * from mvehiculousado where mcat_placa='{0}' and mveh_secuencia<>{1} and test_tipoesta=20", txtPlaca.Text, idVehiculo);
            else
                sqlPlaca = String.Format("select * from mvehiculousado where mcat_placa='{0}' and test_tipoesta=20", txtPlaca.Text);

            if (esOferente)
            {
                if (txtOferenteNombre.Text == "") campos += "Ingrese un nombre\n";
                if (txtOferenteTelefono.Text != "" && !Utils.EsEntero(txtOferenteTelefono.Text)) campos += "Ingrese un teléfono valido (solo números)\n";
                if (txtOferenteCelular.Text != "" && !Utils.EsEntero(txtOferenteCelular.Text)) campos += "Ingrese un celular valido (solo números)\n";
                if (txtOferenteMail.Text != "" && !Utils.EsMail(txtOferenteMail.Text)) campos += "Ingrese un e-mail válido\n";
            }
            else
            {
                //if (ddlAsociado.Items.Count == 0 || ddlAsociado.SelectedValue == "0") campos += "Seleccione un asociado\n";
                if (ddlPropiedad.SelectedValue == "0") campos += "Seleccione la propiedad\n";
                if (ddlUbicacion.SelectedValue == "0") campos += "Seleccione la ubicación\n";
                if (ddlEstado.SelectedValue == "0") campos += "Seleccione el estado\n";
                if (!Utils.EsFecha(txtFechaIngreso.Text) || Utils.ParseDate(txtFechaIngreso.Text) > DateTime.Now) 
                    campos += "Ingrese una fecha de ingreso válida (solo se admiten fechas actuales o pasadas)\n";
            }

            if (ddlCatalogo.Items.Count == 0 || ddlCatalogo.SelectedValue == "0") campos += "Seleccione los detalles de su vehículo\n";
            if (txtPlaca.Text == "" || txtPlaca.Text.Length != 6) campos += "Ingrese una placa válida (6 dígitos)\n";
            if (ddlCiudad.SelectedValue == "0") campos += "Seleccione la ciudad\n";
            if (ddlColor.SelectedValue == "0") campos += "Seleccione el color\n";
            if (ddlModelo.SelectedValue == "0") campos += "Seleccione el modelo\n";
            if (ddlServicio.SelectedValue == "0") campos += "Seleccione el servicio\n";
            if (!Utils.EsEntero(txtKilometraje.Text.Replace(",",""))) campos += "Ingrese un kilometraje válido\n";
            if (txtFechaVenta.Text != "" && !Utils.EsFecha(txtFechaVenta.Text)) campos += "Ingrese una fecha de venta válida\n";
            if (txtValorVenta.Text != "" && !Utils.EsEntero(txtValorVenta.Text.Replace(",", ""))) campos += "Ingrese un valor de venta válido\n";
            if (txtFechaRetiro.Text != "" && !Utils.EsFecha(txtFechaRetiro.Text)) campos += "Ingrese una fecha de retiro válida\n";
            if (DBFunctions.RecordExist(sqlPlaca)) campos += "Ya existe un carro con esta placa en vitrina\n";

            if (Request.QueryString["modifica"] != null && (ddlEstado.SelectedValue == "90" || ddlEstado.SelectedValue == "40"))
            {
                if (phVenta.Visible)
                {
                    if (txtFechaVenta.Text == "") campos += "Ingrese una Fecha de Venta del Vehículo\n";
                    if (txtNumFactura.Text == "") campos += "Ingrese una Número de Factura\n";
                    if (txtValorVenta.Text == "") campos += "Ingrese un Valor de Venta\n";
                }
                else
                {
                    if (txtFechaRetiro.Text == "") campos += "Ingrese una Fecha de Retiro\n";
                    if (txtMotivoRetiro.Text == "") campos += "Ingrese un Motivo de Retiro\n";
                }            
            }
            
            lblError.Text = campos;

            return campos.Length == 0;
        }

        private void LimpiarcamposInvisibles()
        {
            //if (!phAsignado.Visible)
            //{
            //    txtProspectoCli.Text = "";
            //}
            if (!phRetirado.Visible)
            {
                txtFechaRetiro.Text = "";
                txtMotivoRetiro.Text = "";
            }
            if (!phVenta.Visible)
            {
                txtNumFactura.Text = "";
                txtValorVenta.Text = "";
                txtFechaVenta.Text = "";
            }
        }

        private void limpiarFormulario()
        {
            if(ddlAsociado.Items.Count != 0) ddlAsociado.SelectedIndex = 0;
            ddlClaseVeh.SelectedIndex = 0;
            ddlMarca.Items.Clear();
            ddlRefPrincipal.Items.Clear();
            ddlRefComplementaria.Items.Clear();
            ddlCarroceria.Items.Clear();
            ddlCilindraje.Items.Clear();
            ddlTipoCombustible.Items.Clear();
            ddlAspiracion.Items.Clear();
            ddlTraccion.Items.Clear();
            ddlCaja.Items.Clear();
            ddlCatalogo.Items.Clear();
            txtPlaca.Text = "";
            ddlCiudad.SelectedIndex = 0;
            ddlColor.SelectedIndex = 0;
            ddlModelo.SelectedIndex = 0;
            ddlServicio.SelectedIndex = 0;
            txtKilometraje.Text = "";
            txtPrecio.Text = "";
            if (ddlUbicacion.Items.Count != 0) ddlUbicacion.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            txtFechaIngreso.Text = "";
            txtInfoAdicional.Text = "";
            ddlPropiedad.SelectedIndex = 0;

            txtOferenteNombre.Text = "";
            txtOferenteTelefono.Text = "";
            txtOferenteCelular.Text = "";
            txtOferenteMail.Text = "";

            phFotos.Controls.Clear();
            images.Clear();

            lblError.Text = "";

            CambiarVisibilidadPHCatalogo();
            ddlClaseVeh_OnSelectedIndexChanged(null, null); //gestionar opcciones de catalogo
        }

        private void llenarDDLs()
        {
            string sql;
            if (!esOferente)
            {
                string nit = DBFunctions.SingleData(String.Format("SELECT mnit_nit FROM susuario WHERE lower(susu_login) = '{0}'", HttpContext.Current.User.Identity.Name.ToLower()));

                sql = "select mc.mnit_nit, mn.mnit_nombres || ' ' || mn.mnit_apellidos from MCONCESIONARIO mc inner join mnit mn on mc.mnit_nit = mn.mnit_nit {0} order by mcon_gerente";
                sql = String.Format(sql, nit != "" ? String.Format("WHERE mn.mnit_nit = '{0}'", nit) : "");
                Utils.FillDll(ddlAsociado, sql, true);

                sql = "select pubi_secuencia, pubi_nombre from pubicacion {0} order by pubi_nombre";
                sql = String.Format(sql, nit != "" ? String.Format("where mnit_nitconcesionario='{0}'", nit) : "");
                Utils.FillDll(ddlUbicacion, sql, true);
            }

            Utils.FillDll(ddlColor, "select pcol_codigo, pcol_descripcion from pcolor order by pcol_descripcion", true);
            Utils.FillDll(ddlModelo, "select pano_ano, pano_detalle from pano order by pano_detalle", true);
            Utils.FillDll(ddlServicio, "select tser_tiposerv, tser_nombserv from tserviciovehiculo order by tser_nombserv", true);
            Utils.FillDll(ddlEstado, "select test_tipoesta, test_nombesta from testadovehiculo order by test_nombesta", true);
            Utils.FillDll(ddlPropiedad, "select tori_codigo, tori_descripc from torigenvehiculo order by tori_descripc", true);
            Utils.FillDll(ddlCiudad, "select pciu_codigo, pciu_nombre from pciudad order by pciu_nombre", true);

            sql = "SELECT DISTINCT PCC.PCLA_CODIGO, \n" +
             "       PCC.PCLA_NOMBRE \n" +
             "FROM PCLASEVEHICULO PCC, \n" +
             "     PCATALOGOVEHICULO PCV \n" +
             "WHERE PCC.PCLA_CODIGO = PCV.PCLA_CODIGO";

            Utils.FillDll(ddlClaseVeh, sql, true);
        }

        private void llenarddlEstado()
        {
            string estadoActual = ddlEstado.SelectedValue;
            string filter = "";

            if (estadoActual == "0") //nuevo ingreso
                filter = "where test_tipoesta = 20";

            Utils.FillDll(ddlEstado,
                String.Format("select test_tipoesta, test_nombesta from testadovehiculo {0} order by test_nombesta", filter), true);
            
            ddlEstado.SelectedValue = estadoActual;
        }

        private void habilitarPlaceHolders() 
        {
            string estadoActual = ddlEstado.SelectedValue;
            //phAsignado.Visible = false;
            phRetirado.Visible = false;
            phVenta.Visible = false;

            switch (estadoActual)
            {
                case "40": //Vendido
                    phVenta.Visible = true;
                    break;
                //case "30": //Asignado
                //    phAsignado.Visible = true;
                //    break;
                case "90": //Retirado
                    phRetirado.Visible = true;
                    break;
            }

            if (esOferente)
            {
                phAsociado.Visible = false;
                phOpcionesNoOferente.Visible = false;
                phOferente.Visible = true;
            }
        }

        private ArrayList ObtenerCatalogos() 
        {
            string sql =
             "SELECT PCV.PCAT_CODIGO " +
             "FROM PCATALOGOVEHICULO PCV " +
             String.Format("WHERE PCV.PCLA_CODIGO = '{0}' ", ddlClaseVeh.SelectedValue);

            if (ddlMarca.Items.Count > 0)
                sql += String.Format("AND   PCV.PMAR_CODIGO = '{0}' ", ddlMarca.SelectedValue);
            if (ddlRefPrincipal.Items.Count > 0)
                sql += String.Format("AND   PCV.PGRU_GRUPO = '{0}' ", ddlRefPrincipal.SelectedValue);
            if (ddlRefComplementaria.Items.Count > 0 && ddlRefComplementaria.SelectedValue != "")
                sql += String.Format("AND	PCV.PSGRU_CODIGO = '{0}' ", ddlRefComplementaria.SelectedValue);
            else if (ddlRefComplementaria.SelectedValue == "")
                sql += "AND	PCV.PSGRU_CODIGO is null ";
            if (ddlCarroceria.Items.Count > 0)
                sql += String.Format("AND   PCV.PCAR_CODIGO = '{0}' ", ddlCarroceria.SelectedValue);
            if (ddlCilindraje.Items.Count > 0)
                sql += String.Format("AND   PCV.RCIL_CODIGO = '{0}' ", ddlCilindraje.SelectedValue);
            if (ddlTipoCombustible.Items.Count > 0)
                sql += String.Format("AND   PCV.PCOM_CODIGO = '{0}' ", ddlTipoCombustible.SelectedValue);
            if (ddlAspiracion.Items.Count > 0)
                sql += String.Format("AND   PCV.Rali_CODIGO = '{0}' ", ddlAspiracion.SelectedValue);
            if (ddlTraccion.Items.Count > 0)
                sql += String.Format("AND   PCV.Rtra_CODIGO = '{0}' ", ddlTraccion.SelectedValue);
            if (ddlCaja.Items.Count > 0)
                sql += String.Format("AND   PCV.RCAJ_CODIGO = '{0}' ", ddlCaja.SelectedValue);
             
            return DBFunctions.RequestAsCollection(sql);
        }

        private void CargarCatalogo(string catalogo)
        {
            string sql = String.Format(
             "SELECT PCV.PCLA_CODIGO, \n" +
             "       PCV.PMAR_CODIGO, \n" +
             "       PCV.PGRU_GRUPO, \n" +
             "       PCV.PSGRU_CODIGO, \n" +
             "       PCV.PCAR_CODIGO, \n" +
             "       PCV.RCIL_CODIGO, \n" +
             "       PCV.PCOM_CODIGO, \n" +
             "       PCV.Rali_CODIGO, \n" +
             "       PCV.Rtra_CODIGO, \n" +
             "       PCV.RCAJ_CODIGO \n" +
             "FROM PCATALOGOVEHICULO PCV \n" +
             "WHERE PCV.PCAT_CODIGO = '{0}'"
             , catalogo);

            Hashtable catalogoTable = (Hashtable)DBFunctions.RequestAsCollection(sql)[0];

            ddlClaseVeh.SelectedValue = catalogoTable["PCLA_CODIGO"] as string;
            ddlClaseVeh_OnSelectedIndexChanged(null, null);

            ddlMarca.SelectedValue = catalogoTable["PMAR_CODIGO"] as string;
            ddlMarca_OnSelectedIndexChanged(null, null);

            ddlRefPrincipal.SelectedValue = catalogoTable["PGRU_GRUPO"] as string;
            ddlRefPrincipal_OnSelectedIndexChanged(null, null);

            ddlRefComplementaria.SelectedValue = catalogoTable["PSGRU_CODIGO"] as string;
            ddlRefComplementaria_OnSelectedIndexChanged(null, null);

            ddlCarroceria.SelectedValue = catalogoTable["PCAR_CODIGO"] as string;
            ddlCarroceria_OnSelectedIndexChanged(null, null);

            ddlCilindraje.SelectedValue = catalogoTable["RCIL_CODIGO"] as string;
            ddlCilindraje_OnSelectedIndexChanged(null, null);

            ddlTipoCombustible.SelectedValue = catalogoTable["PCOM_CODIGO"] as string;
            ddlTipoCombustible_OnSelectedIndexChanged(null, null);

            ddlAspiracion.SelectedValue = catalogoTable["RALI_CODIGO"] as string;
            ddlAspiracion_OnSelectedIndexChanged(null, null);

            ddlTraccion.SelectedValue = catalogoTable["RTRA_CODIGO"] as string;
            ddlTraccion_OnSelectedIndexChanged(null, null);

            ddlCaja.SelectedValue = catalogoTable["RCAJ_CODIGO"] as string;
            DatosCatalogo_OnSelectedIndexChanged(null, null);
            ddlCatalogo.SelectedValue = catalogo;

            ddlClaseVeh.Enabled = false;
            ddlMarca.Enabled = false;
            ddlRefPrincipal.Enabled = false;
            ddlRefComplementaria.Enabled = false;
            ddlCarroceria.Enabled = false;
            ddlCilindraje.Enabled = false;
            ddlTipoCombustible.Enabled = false;
            ddlAspiracion.Enabled = false;
            ddlTraccion.Enabled = false;
            ddlCaja.Enabled = false;
            ddlCatalogo.Enabled = false;
        }

        private bool IsCatalogoFilled()
        {
            bool resp = true;

            resp &= ddlClaseVeh.Items.Count > 0 ? ddlClaseVeh.SelectedValue != "0" : true;
            resp &= ddlMarca.Items.Count > 0 ? ddlMarca.SelectedValue != "0" : true;
            resp &= ddlRefPrincipal.Items.Count > 0 ? ddlRefPrincipal.SelectedValue != "0" : true;
            resp &= ddlRefComplementaria.Items.Count > 0 ? ddlRefComplementaria.SelectedValue != "0" : true;
            resp &= ddlCarroceria.Items.Count > 0 ? ddlCarroceria.SelectedValue != "0" : true;
            resp &= ddlCilindraje.Items.Count > 0 ? ddlCilindraje.SelectedValue != "0" : true;
            resp &= ddlTipoCombustible.Items.Count > 0 ? ddlTipoCombustible.SelectedValue != "0" : true;
            resp &= ddlAspiracion.Items.Count > 0 ? ddlAspiracion.SelectedValue != "0" : true;
            resp &= ddlCaja.Items.Count > 0 ? ddlCaja.SelectedValue != "0" : true;

            return resp;
        }

        private void LlenarCatalogo()
        {

            ArrayList catalogos = ObtenerCatalogos();
            string catalogosStr = "(";

            foreach (Hashtable hash in catalogos)
                catalogosStr += String.Format("'{0}',", hash["PCAT_CODIGO"].ToString());

            if (catalogosStr.Length > 1)
                catalogosStr = 
                    catalogosStr.Substring(0, catalogosStr.Length - 1) + ")";

            string sql = String.Format(
             "SELECT PV.PCAT_CODIGO,  \n" +
             "       pcv.PCLA_NOMBRE CONCAT ' ' CONCAT   \n" +
             "       pm.PMAR_NOMBRE CONCAT ' ' CONCAT   \n" +
             "       pg.PGRU_NOMBRE CONCAT ' ' CONCAT   \n" +
             ((ddlRefComplementaria.Items.Count > 1 || ddlRefComplementaria.SelectedValue != "NT") ?
             "       COALESCE(psg.PSGRU_NOMBRE,'') CONCAT ' ' CONCAT   \n" : "") +
             "       pc.PCAR_NOMBRE CONCAT '\n' CONCAT   \n" +
             "       COALESCE(rc.RCIL_NOMBRE,'') CONCAT ' ' CONCAT   \n" +
             "       pcm.PCOM_NOMBRE CONCAT ' ' CONCAT   \n" +
             "       COALESCE(ram.RALI_NOMBRE,'') CONCAT   \n" +
             "       ' Caja ' CONCAT COALESCE(rcc.RCAJ_NOMBRE,'') CONCAT ' ' CONCAT   \n" +
             "       COALESCE(rtv.RTRA_DESCRIPC,'')  \n" +
             "FROM PCLASEVEHICULO PCV,  \n" +
             "     PMARCA PM,  \n" +
             "     PGRUPOCATALOGO PG,  \n" +
             "     PCARROCERIA PC,  \n" +
             "     PCOMBUSTIBLEMOTOR PCM,  \n" +
             "     PCATALOGOVEHICULO PV   \n" +
             "  LEFT JOIN PSUBGRUPOVEHICULO PSG ON PV.PSGRU_CODIGO = PSG.PSGRU_GRUPO   \n" +
             "  LEFT JOIN RALIMENTACIONMOTOR RAM ON PV.RALI_CODIGO = RAM.RALI_CODIGO   \n" +
             "  LEFT JOIN RCILINDRAJEMOTOR RC ON PV.RCIL_CODIGO = RC.RCIL_CODIGO   \n" +
             "  LEFT JOIN RCAJACAMBIOS RCC ON PV.RCAJ_CODIGO = RCC.RCAJ_CODIGO   \n" +
             "  LEFT JOIN RTRACCIONVEHICULO RTV ON PV.RTRA_CODIGO = RTV.RTRA_CODIGO  \n" +
             "WHERE PV.PCLA_CODIGO = PCV.PCLA_CODIGO  \n" +
             "AND   PV.PMAR_CODIGO = PM.PMAR_CODIGO  \n" +
             "AND   PV.PGRU_GRUPO = PG.PGRU_GRUPO  \n" +
             "AND   PV.PCAR_CODIGO = PC.PCAR_CODIGO  \n" +
             "AND   PV.PCOM_CODIGO = PCM.PCOM_CODIGO  \n" +
             "AND   PV.PCAT_CODIGO in {0} \n" +
             "ORDER BY 2"
             , catalogosStr);

            ddlCatalogo.Items.Clear();
            Utils.FillDll(ddlCatalogo, sql, true);

            if (ddlCatalogo.Items.Count > 0)
                phDatosVehiculo.Visible = true;
            else
                phDatosVehiculo.Visible = false;
        }

        private void CambiarVisibilidadPHCatalogo()
        {
            phMarca.Visible = false;
            phRefPrincipal.Visible = false;
            phCarroceria.Visible = false;
            phRefComplementaria.Visible = false;
            phDatosCatalogo.Visible = false;
            phCatalogo.Visible = false;

            if(ddlClaseVeh.Items.Count > 0 && ddlClaseVeh.SelectedValue != "0")
                phMarca.Visible = true;

            if (ddlMarca.Items.Count > 0 && ddlMarca.SelectedValue != "0")
                phRefPrincipal.Visible = true;


            if (ddlRefPrincipal.Items.Count > 0 && ddlRefPrincipal.SelectedValue != "0")
            {
                if (ddlRefComplementaria.Items.Count > 1 ||
                    ddlRefComplementaria.SelectedValue != "NT") // Referencia Complementaria diferente de "NO TIENE"
                    phRefComplementaria.Visible = true;

                phCarroceria.Visible = true;
            }

            if (ddlCarroceria.Items.Count > 0 && ddlCarroceria.SelectedValue != "0")
                phDatosCatalogo.Visible = true;

            if(IsCatalogoFilled())
                phCatalogo.Visible = true;
        }

        private bool OferenteExiste(string cc)
        {
            return DBFunctions.RecordExist(String.Format(
                "select * from MOFERENTE where MOFE_CC='{0}'", cc));
        }

        private string ObtenerVendedorPorCarrusel()
        {
            string vendedor = "null";

            if (esOferente)
                return vendedor;
            
            string ubicacion = ddlUbicacion.SelectedValue;
            string asociado = ddlAsociado.SelectedValue;
            string sql = String.Format("select mcon_autovend from mconcesionario where mnit_nit='{0}'", asociado);
            
            if (DBFunctions.SingleData(sql) == "N")
                return vendedor;

            sql = String.Format(
             "SELECT pv.pven_secuencia vendedor, \n" +
             "       pubi_vendasig asignaciones \n" +
             "FROM pvendedor pv  \n" +
             "  LEFT JOIN pubicacion pu ON pu.pubi_secuencia = pv.pubi_secuencia \n" +
             "WHERE pv.pubi_secuencia = {0} \n" +
             "ORDER BY 1"
             , ubicacion);
            ArrayList vendedores = DBFunctions.RequestAsCollection(sql);

            if (vendedores.Count > 0)
            {
                int index = (int)((Hashtable)vendedores[0])["ASIGNACIONES"];
                vendedor = ((Hashtable)vendedores[index % vendedores.Count])["VENDEDOR"].ToString();

                sql = String.Format("update pubicacion set pubi_vendasig={0} where pubi_secuencia={1}"
                    , (index + 1) % vendedores.Count
                    , ubicacion);
                DBFunctions.NonQuery(sql);

            }

            return vendedor;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            string redirect = null;
            redirect = String.Format("{0}?process=Asousados.ConsultaDeVehiculosOfertados",indexPage);
            Response.Redirect(redirect);
        }

        // Saves an image as a jpeg image, with the given quality 
        // <param name="path">Path to which the image would be saved.</param> 
        // <param name="quality">An integer from 0 to 100, with 100 being the highest quality</param> 
        public static void SaveJpeg(string path, System.Drawing.Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


            // Encoder parameter for image quality 
            EncoderParameter qualityParam =
                new EncoderParameter(Encoder.Quality, quality);
            // Jpeg image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        // Returns the image codec with the given mime type 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        } 

	}
}
