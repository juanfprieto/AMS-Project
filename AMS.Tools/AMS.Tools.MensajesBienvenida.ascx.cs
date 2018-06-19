using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.DB;


namespace AMS.Tools
{
    public partial class AMS_Tools_MensajesBienvenida : System.Web.UI.UserControl
    {
        string user = HttpContext.Current.User.Identity.Name.ToLower();
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                recargaDatos(sender, e);
            }
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Tools_MensajesBienvenida));
        }
        protected void recargaDatos(object sender, EventArgs e)
        {
            DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT PMEN_ID AS ID, PMEN_AUTOR AS AUTOR, PMEN_COMENTARIO AS MENSAJE, 
                                                            (CASE WHEN GACT_MOSTRAR = 'S' THEN 'SI' ELSE 'NO' END) AS MOSTRAR_MENSAJE, 
                                                            PMEN_FECHA AS FECHA, SUSU_LOGIN AS USUARIO FROM PMENSAJE WHERE SUSU_LOGIN = '" + user + "' ");
            miGrid.DataSource = ds.Tables[0];
            miGrid.DataBind();
        }

        protected void grabarDatos(object sender, EventArgs e)
        {
            int rta = 0;
            if(txtAreaMensaje.Text.Length == 0 )
            {
                Utils.MostrarAlerta(Response, "Es necesario escribir un mensaje para poder grabarlo");
                return;
            }
            string autor = txtAutor.Text;
            string mensaje = txtAreaMensaje.Text;
            if(autor.ToString().Trim() == "")
            {
                rta = DBFunctions.NonQuery(@"INSERT INTO DBXSCHEMA.PMENSAJE
                                                (
                                                  PMEN_COMENTARIO,
                                                  PMEN_AUTOR,
                                                  SUSU_LOGIN,
                                                  GACT_MOSTRAR,
                                                  PMEN_FECHA
                                                )
                                                VALUES
                                                (
                                                  '" + mensaje + "',default,'" + user + "','S',DEFAULT ); ");
            }
            else
            {
                rta = DBFunctions.NonQuery(@"INSERT INTO DBXSCHEMA.PMENSAJE
                                                (
                                                  PMEN_COMENTARIO,
                                                  PMEN_AUTOR,
                                                  SUSU_LOGIN,
                                                  GACT_MOSTRAR,
                                                  PMEN_FECHA
                                                )
                                                VALUES
                                                (
                                                  '" + mensaje + "','" + autor + "','" + user + "','S',DEFAULT ); ");
            }
            if(rta != 0)
            {
                Utils.MostrarAlerta(Response, "Se creó el registro correctamente.");
                txtAreaMensaje.Text = "";
                txtAutor.Text = "";
                //DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT PMEN_ID AS ID, PMEN_AUTOR AS AUTOR, PMEN_COMENTARIO AS MENSAJE, 
                //                                            (CASE WHEN GACT_MOSTRAR = 'S' THEN 'SI' ELSE 'NO' END) AS MOSTRAR_MENSAJE, 
                //                                            DATE(PMEN_FECHA) AS FECHA, SUSU_LOGIN AS USUARIO FROM PMENSAJE WHERE SUSU_LOGIN = '" + user + "' ");
                //miGrid.DataSource = ds.Tables[0];
                //miGrid.DataBind();
                recargaDatos(sender, e);
            }
            else
            {
                Utils.MostrarAlerta(Response, "Se generó un problema al intentar guardar el nuevo mensaje.");

            }
        }

        [Ajax.AjaxMethod()]
        public string elimina_Fila(string pos)
        {
            string valor = "";
            if(DBFunctions.NonQuery("DELETE FROM PMENSAJE WHERE PMEN_ID=" + pos + "") != 0)
            {
                valor = "ok";
            }
            return valor;
        }
    }
}