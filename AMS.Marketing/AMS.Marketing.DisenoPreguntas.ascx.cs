using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;
using System.Data;

namespace AMS.Marketing
{
	public partial class DisenoPreguntas : System.Web.UI.UserControl
	{
        protected Seleccionar Seleccion = new Seleccionar();

        protected void Page_Load(object sender, EventArgs e)
        {
            Seleccion.ECargarG += new Seleccionar.DCargarG(Seleccion_ECargarG);
            Seleccion.AsignarEstados(true, true, true, true);
            Seleccion.AsignarTextoLabels("Sin Asignar", "Asignados");

            if (!IsPostBack)
            {
                Cargar();
                BindGrid();
            }
        }

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

        /// <summary>
        /// Carga los ListBox del control de usuario Seleccionar con las consultas
        /// especificadas
        /// </summary>
        private void BindGrid()
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            string Sql = "Select pres_codiresp as Codigo, pres_descresp as Nombre from DBXSCHEMA.PRESPUESTAENCUESTA where " +
                         "pres_codiresp not in (select pres_codiresp from DBXSCHEMA.dpreguntarespuesta  where ppre_codipreg = '" + Perfil.SelectedValue.ToString() + "') order by Nombre; ";
                
            DBFunctions.Request(ds, IncludeSchema.NO, Sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Seleccion.CargarOrigen(i, ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][0].ToString(), "Rojo");
            }

            Sql = "Select dp.pres_codiresp as Codigo, pr.pres_descresp as Nombre from DBXSCHEMA.dpreguntarespuesta dp, DBXSCHEMA.prespuestaencuesta pr Where dp.pres_codiresp=pr.pres_codiresp " +
                  "and dp.ppre_codipreg='" + Perfil.SelectedValue.ToString() + "' order by Nombre;";
               
            DBFunctions.Request(ds1, IncludeSchema.NO, Sql);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                Seleccion.CargarDestino(i, ds1.Tables[0].Rows[i][1].ToString(), ds1.Tables[0].Rows[i][0].ToString(), "Rojo");
            }
            ds.Dispose();
            ds1.Dispose();
        }

        /// <summary>
        /// Enlaza el dropdownlist Perfil con la consulta especificada
        /// </summary>
        private void Cargar()
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(Perfil, "SELECT ppre_codipreg ,ppre_descpreg FROM dbxschema.ppreguntaencuesta where ttip_codigo='C' order by ppre_descpreg");
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia el perfil seleccionado
        /// </summary>
        /// <param name="sender">object Objeto que hizo la petición</param>
        /// <param name="e">EventArgs manejador de argumentos de eventos</param>
        protected void Entidad_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Seleccion.LimpiarTodo();
            BindGrid();
        }

        /// <summary>
        /// Función que ejecutara el delegado DCargarG del control de usuario
        /// Seleccionar. Actualiza los permisos asignados y desasignados
        /// contenidos en los ListBox
        /// </summary>
        /// <param name="Inicio">ListBox contiene los permisos a los cuales no tendra acceso el perfil seleccionado</param>
        /// <param name="Final">ListBox contiene los permisos a los cuales tendra acceso el perfil seleccionado</param>
        private void Seleccion_ECargarG(ListBox Inicio, ListBox Final)
        {
            string sql = "";
            string estaban = "";
            string restantes = "";
            string errQuit = "";
            string okAdd = "";
            int x,y;
            int retorno = 0;
            int totinsert = 0;

            Mensajes.Text = "";

            for (y = 0; y < Inicio.Items.Count; y++)
            {
                restantes = Inicio.Items[y].Value.ToString().Trim();
                try
                {
                    sql = "DELETE FROM dbxschema.DPREGUNTARESPUESTA WHERE PPRE_CODIPREG='" + Perfil.SelectedValue.ToString() + "' AND PRES_CODIRESP='" + restantes + "' ;";
                    retorno = DBFunctions.NonQuery(sql);
                }
                catch (Exception Ex)
                {
                    errQuit += "La respuesta asociada: " + Inicio.Items[y].Text + " no se pudo retirar. <BR>";
                }

            }


            for (x = 0; x < Final.Items.Count; x++)
            {
                estaban = Final.Items[x].Value.ToString().Trim();
                try
                {
                    if (!DBFunctions.RecordExist("select ppre_codipreg from dbxschema.dpreguntarespuesta where ppre_codipreg='" + Perfil.SelectedValue.ToString() + "' and pres_codiresp='" + estaban + "';"))
                    {
                        sql = "INSERT INTO dbxschema.DPREGUNTARESPUESTA VALUES (DEFAULT,'" + Perfil.SelectedValue.ToString() + "','" + estaban + "');";
                        retorno = DBFunctions.NonQuery(sql);
                    }
                    
                    okAdd += "Se ha relacionado correctamente la respuesta: " + Final.Items[x].Text + ". <BR>";
                }
                catch (Exception Ex)
                {
                    //Mensajes.Text = "Error " + Ex.Message;
                }

            }

            if (!errQuit.Equals(""))
            {
                Mensajes.Text = "Error... Una o mas respuestas retiradas de la asociacion estan presententes en la definicion de una Encuesta. <BR>" + errQuit;
            }
            if (!okAdd.Equals(""))
            {
                Mensajes.Text += "<BR>Respuestas relacionadas correctamente: <BR>" + okAdd;
            }

        }

       
	}
}