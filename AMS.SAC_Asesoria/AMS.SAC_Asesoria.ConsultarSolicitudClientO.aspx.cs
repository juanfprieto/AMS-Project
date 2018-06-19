using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using System.Text;
using System;
using Obout.Grid;
using AMS.Tools;

namespace AMS.SAC_Asesoria
{
    public partial class ConsultarSolicitudClientO : System.Web.UI.Page
    {
        Obout.Grid.Grid grid1;
        TemplateContainer c;
        DataSet dsDatos;
        DataTable dsTabla;

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarEncabezado();
            ConfigurarGrid();

            if (!Page.IsPostBack)
            {

                dsTabla = new DataTable();

                CargarDatos();

                plcReporte.Controls.Clear();
                plcReporte.Controls.Add(grid1);

                grid1.DataSource = dsDatos;
                grid1.DataBind();
            }
        }

        //Carga encabezado de Ecas con usuario en sesion en el sistema.
        protected void CargarEncabezado()
        {
            String nameSystem = ConfigurationManager.AppSettings["SystemName"] + ".";
            String pathToControls = ConfigurationManager.AppSettings["PathToControls"];

            plcEncabezado.Controls.Add(LoadControl("" + pathToControls + nameSystem + "Tools.Encabezado.ascx"));
        }

        //Realiza la configuracion general de todo el diseño del Grid.
        protected void ConfigurarGrid()
        {
            grid1 = new Obout.Grid.Grid();

            //Configuracion del Template para grupos.
            GridRuntimeTemplate TemplateEditAddress = new GridRuntimeTemplate();
            TemplateEditAddress.ID = "TemplateEditAddress";
            TemplateEditAddress.UseQuotes = true;
            TemplateEditAddress.Template = new Obout.Grid.RuntimeTemplate();
            TemplateEditAddress.Template.CreateTemplate +=
                new Obout.Grid.GridRuntimeTemplateEventHandler(CreateEditAddressTemplate);

            //Parametros de Grid generales.
            grid1.Templates.Add(TemplateEditAddress);
            grid1.TemplateSettings.GroupHeaderTemplateId = "TemplateEditAddress";
            grid1.AllowAddingRecords = false;
            grid1.AllowColumnReordering = true;
            grid1.AllowColumnResizing = true;
            grid1.AllowGrouping = true;
            grid1.AutoGenerateColumns = true;
            grid1.CallbackMode = true;
            grid1.EnableRecordHover = true;
            grid1.FolderLocalization = "../grid/localization";
            grid1.FolderStyle = "../grid/styles/style_4";
            grid1.ID = "grid1";
            grid1.Language = "es";
            grid1.PageSize = 200;
            grid1.Serialize = true;
            grid1.ShowCollapsedGroups = true;
            grid1.ShowGroupsInfo = true;
            grid1.ShowMultiPageGroupsInfo = true;
            grid1.KeepSelectedRecords = true;

        }

        //Evento para la carga del Template para la condiguracion del Grid.
        void CreateEditAddressTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
        {
            Literal oLiteral = new Literal();
            e.Container.Controls.Add(oLiteral);
            c = e.Container;
            oLiteral.DataBinding += new EventHandler(DataBindEditAddressTemplate);
        }

        //Evento para configuracion del Template del Grid.
        void DataBindEditAddressTemplate(Object sender, EventArgs e)
        {
            Literal oLiteral = sender as Literal;
            Obout.Grid.TemplateContainer oContainer =
                oLiteral.NamingContainer as Obout.Grid.TemplateContainer;

            oLiteral.Text = "<u>" + c.Column.HeaderText + "</u> : <b><i>" + c.Value + "</i></b> (" + c.Group.PageRecordsCount + " " + (c.Group.PageRecordsCount > 1 ? "registros" : "registro") + ")";
        }

        //LLena el dataSet con la consulta especifica a solicitudes. Valida si es 
        //usuario interno o externo a la empresa para los filtros.
        private void CargarDatos()
        {
            dsDatos = new DataSet();
            string nitUsu = DBFunctions.SingleData("SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "';");
            string tipoUsu = DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "';");
            string consulta = "";
            if (!tipoUsu.Equals("AS") && !tipoUsu.Equals("UE"))
            {
                if (!nitUsu.Equals(""))
                {
                    consulta = "AND MSOL.mnit_nitcon=" + nitUsu;
                }
                else
                {
                    Utils.MostrarAlerta(Response, "Debe relacionar un Nit o Cedula a su Usuario mediante la opción de Modificación de Usuario.");
                    return;
                }
            }

            DBFunctions.Request(dsDatos, IncludeSchema.NO, "SELECT MSOL.msol_numero AS NUMERO,MNIT.mnit_nit AS NITCLI,(MNIT.mnit_nombres || MNIT.mnit_nombre2 || MNIT.mnit_apellidos || MNIT.mnit_apellido2 )AS CLIENTE,MNIT2.mnit_nit AS NITCON,(MNIT2.mnit_nombres || MNIT2.mnit_nombre2 || " +
                                                        "MNIT2.mnit_apellidos || MNIT2.mnit_apellido2 ) AS CONTACTO, " +
                                                        "MSOL.msol_fecha AS FECHA,MSOL.msol_hora AS HORA,TSOL.tsol_nombre AS VIA " +
                                                        "FROM dbxschema.msolicitud MSOL,dbxschema.mnit MNIT,dbxschema.mnit MNIT2,dbxschema.tsolicitud TSOL " +
                                                        "WHERE MSOL.mnit_nitcli=MNIT.mnit_nit AND MSOL.mnit_nitcon=MNIT2.mnit_nit AND MSOL.tsol_codigo=TSOL.tsol_codigo " +
                                                        consulta + " ORDER BY MSOL.msol_numero DESC ");

            if (dsDatos.Tables[0].Rows.Count == 0)
            {
                Utils.MostrarAlerta(Response, "Actualmente no ha creado ninguna solicitud.");
                //ViewState["solicitudes"] = 0;
                //return;
            }
        }



    }
}