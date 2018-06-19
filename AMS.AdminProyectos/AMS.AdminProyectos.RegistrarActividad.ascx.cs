using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using System.Configuration;
using System.Data;
using AMS.DB;
using System.Collections;
using AMS.Tools;

namespace AMS.AdminProyectos
{
    public partial class RegistrarActividad : System.Web.UI.UserControl
    {
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        private DatasToControls bind = new DatasToControls();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Definicion del AJAX para los metodos CALL_BACK
            Ajax.Utility.RegisterTypeForAjax(typeof(RegistrarActividad));

            if (!IsPostBack)
            { //se cargan todos los GAD que estan debeidamente parametrizados en la tabla MGAD
                bind.PutDatasIntoDropDownList(ddlGad, "select MGAD_CODIGO, MGAD_NOMBRE from MGAD;");
                //Si No trae datos se indica que se debe parametrizar la tabla MGAD
                if (ddlGad.Items.Count < 1)
                    Utils.MostrarAlerta(Response, "No ha definido los GAD revice por favor.");
                else
                    ddlGad.Items.Insert(0, "Seleccione..");
            }
        }
        protected void ddlgad_CargarProyecto(object sender, System.EventArgs e)
        {  //Cargo todos los proyectos parametrizados en MPROYECTO
            bind.PutDatasIntoDropDownList(ddlProyecto, " select mpro_codigo, mpro_nombre from mproyecto where mgad_codigo = '"+ddlGad.SelectedValue+"'");
            //Si No trae datos se indica que se debe parametrizar la tabla MPROYECTO
            if (ddlProyecto.Items.Count < 1)
                Utils.MostrarAlerta(Response, "No ha definido PROYECTOS revice por favor.");
            else
                ddlProyecto.Items.Insert(0, "Seleccione..");
        }

        protected void ddlproyecto_CargarActividad(object sender, System.EventArgs e)
        {   //Cargo las actividades relacionadas a un proyecto
            bind.PutDatasIntoDropDownList(ddlActividad, "SELECT  M.MACT_CODIGO, M.MACT_NOMBRE FROM MACTIVIDADES M, MPROYECTO MP WHERE M.MPRO_CODIGO = MP.MPRO_CODIGO AND MP.MPRO_CODIGO = '"+ddlProyecto.SelectedValue+"'");
            //Si No trae datos se indica que se debe parametrizar la tabla MACTIVIDADES
            if (ddlActividad.Items.Count < 1)
                Utils.MostrarAlerta(Response, "No ha definido ACTIVIDADES revice por favor.");
            else
                ddlActividad.Items.Insert(0, "Seleccione..");
        }
        protected void ddlproyecto_CargarDatos(object sender, System.EventArgs e)
        {  //Cargo los objetivos paratrizados en la tabla MACTIVIDADES
            bind.PutDatasIntoDropDownList(ddlObj, "SELECT M.POBJ_NUMERO,P.POBJ_DESCRIPCION FROM MACTIVIDADES M, POBJETIVO P WHERE P.POBJ_NUMERO = M.POBJ_NUMERO AND MACT_CODIGO = '"+ddlActividad.SelectedValue+"'");
            //Si No trae datos se indica que se debe parametrizar la tabla POBJETIVO y defininirlos en MACTIVIDADES
            if (ddlObj.Items.Count < 1)
                Utils.MostrarAlerta(Response, "No ha definido OBJETIVOS PARA EL PREOYECTO revice por favor.");
            else
                ddlObj.Items.Insert(0, "Seleccione..");
            //Cargo los indicadores paratrizados en la tabla MACTIVIDADES
            bind.PutDatasIntoDropDownList(ddlIndicador, "SELECT M.MIND_NUMERO,MI.MIND_DESCRIPCION FROM MACTIVIDADES M, MINDICADOR MI WHERE MI.MIND_NUMERO = M.MIND_NUMERO AND MACT_CODIGO = '" + ddlActividad.SelectedValue + "'");
            //Si No trae datos se indica que se debe parametrizar la tabla MINDICADOR y defininirlos en MACTIVIDADES
            if (ddlIndicador.Items.Count < 1)
                Utils.MostrarAlerta(Response, "No ha definido INDICADORES revice por favor.");
            else
                ddlIndicador.Items.Insert(0, "Seleccione..");
            //Cargo los resultados paratrizados en la tabla MACTIVIDADES
            bind.PutDatasIntoDropDownList(ddlResult, @"SELECT MO.PRES_CODIGO AS CODIGO, PRES_DESCRIPCION AS RESULTADO FROM MACTIVIDADES M, MOBJETIVORESULTADO MO, POBJETIVO P, PRESULTADO PR  WHERE MO.POBJ_NUMERO = M.POBJ_NUMERO AND MO.POBJ_NUMERO = P.POBJ_NUMERO 
                                                        AND MO.PRES_CODIGO = PR.PRES_CODIGO AND MACT_CODIGO = '" + ddlActividad.SelectedValue + "'; ");
            //Si No trae datos se indica que se debe parametrizar la tabla  PRESULTADO, MOBJETIVORESULTADO y defininirlos en MACTIVIDADES
            if (ddlResult.Items.Count < 1)
                Utils.MostrarAlerta(Response, "No ha definido RESULTADOS PARA LOS OBJETIVOS revice por favor.");
            else
                ddlResult.Items.Insert(0, "Seleccione..");
        }

        //Invocacion del metodo AJAX para poder leer la funcion
        [Ajax.AjaxMethod]
        public DataSet Verificar_Cliente(string codigo)
        {
            DataSet clientes = new DataSet();
            //Cargo los datos de un Beneficiario con respecto al CODIGO
            DBFunctions.Request(clientes, IncludeSchema.NO, @"SELECT VM.NOMBRE AS NOMBRE, M.MNIT_NIT AS CEDULA, PGRA_DESCRIPCION AS ESCOLARIDAD,PENT_DESCRIPCION AS ENTIDADP,  VM2.NOMBRE AS RESPONSABLE  FROM MBENEFICIARIO M, VMNIT VM, VMNIT VM2,PGRADOESCOLARIDAD P, PENTIDAD PE 
                                                             WHERE M.MNIT_NIT = VM.MNIT_NIT AND P.PGRA_CODIGO = M.PGRA_CODIGO AND PE.PENT_CODIGO = M.PENT_CODIGO AND M.MNIT_NIT = VM2.MNIT_NIT AND MBEN_ID = '" + codigo+"' ");
            return clientes;
        }
        //Invocacion del metodo AJAX para poder leer la funcion
        [Ajax.AjaxMethod]
        public DataSet Verificar_Cedula(string codigo)
        {
            DataSet cedula = new DataSet();
            //Cargo los datos de un Beneficiario con respecto a la CEDULA
            DBFunctions.Request(cedula, IncludeSchema.NO, @"SELECT VM.NOMBRE AS NOMBRE, M.MBEN_ID AS CODIGO, PGRA_DESCRIPCION AS ESCOLARIDAD,PENT_DESCRIPCION AS ENTIDADP,  VM2.NOMBRE AS RESPONSABLE  FROM MBENEFICIARIO M, VMNIT VM, VMNIT VM2,PGRADOESCOLARIDAD P, PENTIDAD PE 
                                                            WHERE M.MNIT_NIT = VM.MNIT_NIT AND P.PGRA_CODIGO = M.PGRA_CODIGO AND PE.PENT_CODIGO = M.PENT_CODIGO AND M.MNIT_NIT = VM2.MNIT_NIT AND M.MNIT_NIT = '" + codigo + "'");
            return cedula;
        }

        protected void Registrar_Actividad(object sender, System.EventArgs e)
        {
            ArrayList sqlLista = new ArrayList();
            //Creo el insert de datos para generar el registo
            sqlLista.Add("INSERT INTO DACTIVIDADES VALUES( DEFAULT,  '" + ddlActividad.SelectedValue + "',  " + txtCodigo.Text + ",  '" + HttpContext.Current.User.Identity.Name.ToLower() + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')");
            //Ejecuto el Insert
            bool procesoSatisfactorio = DBFunctions.Transaction(sqlLista);
            //Valido la transaccion del insert en caso de que el proceso sea satisfactorio muestro la alerta y retorno sin datos.
            if (procesoSatisfactorio) { 
                Utils.MostrarAlerta(Response, "Registro Creado Satisfactoriaminte.");
                Response.Redirect(indexPage + "?process=AdminProyectos.RegistrarActividad", false);
            }
            else
            {   //en caso de error muestro el problema en un label para saber que ocurrio.
                lError.Text = DBFunctions.exceptions;
                return;
            }
        }

    }
}