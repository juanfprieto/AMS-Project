using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.DBManager;
using System.Configuration;
using System.Collections;
using AMS.Documentos;
using AMS.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.Services;


namespace AMS.Web
{
    public partial class Bienvenida : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //mensajeAct.Text = DBFunctions.SingleData("SELECT GACT_MODULO CONCAT ' --> ' CONCAT GACT_COMENTARIO AS COMENTARIO, DATE(GACT_FECHA) AS FECHA, RAND() AS INDX FROM GACTUALIZACION ORDER BY IDX FETCH FIRST 1 ROWS ONLY");
                string usuario = HttpContext.Current.User.Identity.Name;
                lblUserWelcome.Text = "Bienvenido ";
                lblUserWelcome.Text += DBFunctions.SingleData("select SUSU_NOMBRE  from susuario where susu_login ='" + usuario + "'");

                Ajax.Utility.RegisterTypeForAjax(typeof(Web.Bienvenida));
            }

            //Mostrar el mensaje de acuerdo a la hora del día 
            mensajeAct.Text = mostrarMensaje();
        }

        //Función que devuelve un mensaje conforme la hora del día
        [Ajax.AjaxMethod()]
        public string mostrarMensaje()
        {
            string primerFecha = DateTime.Today.AddDays(-5).GetDateTimeFormats()[5];
            string segundaFecha = DateTime.Today.GetDateTimeFormats()[5];
            string texto = "";
            TimeSpan hora = DateTime.Now.TimeOfDay;
            //int dia, mes, ano;
            //dia = DateTime.Now.Day;
            //mes = DateTime.Now.Month;
            //ano = DateTime.Now.Year;
            //string fecha2 = ano + "-" + mes + "-" + dia;
            //string fecha1 = ano + "-" + mes + "-" + (dia - 5);

            if (hora > new TimeSpan(06, 00, 00) && hora < new TimeSpan(13, 00, 00))
            {
                //AQUI VAN LOS MENSAJES DE LOS USUARIOS   ¤¤¤¤
                texto = DBFunctions.SingleData("SELECT PMEN_COMENTARIO CONCAT ' » ' CONCAT PMEN_AUTOR CONCAT ' « ', RAND() AS INDX FROM PMENSAJE ORDER BY INDX FETCH FIRST 1 ROWS ONLY");
                if (texto == "")
                    texto = "Buenos días !!";
            }
            /*else if (hora >= new TimeSpan(13, 00, 00) && hora <= new TimeSpan(13, 29, 59))
            {
                texto = "SE ESTAN REALIZANDO ACTUALIZACIONES, FAVOR NO INGRESE AL SISTEMA";
            }*/
            else
            {
                //AQUI VAN LOS MENSAJES DE LAS ACTUALIZACIONES
                //texto = DBFunctions.SingleDataGlobal("SELECT GACT_MODULO CONCAT ' --> ' CONCAT GACT_COMENTARIO AS COMENTARIO, DATE(GACT_FECHA) AS FECHA, RAND() AS INDX FROM GACTUALIZACION WHERE DATE(GACT_FECHA) BETWEEN '" + primerFecha + "' AND '" + segundaFecha + "' ORDER BY INDX FETCH FIRST 1 ROWS ONLY");
                //test sin global, luego poner global
                texto = DBFunctions.SingleData(
                    @"select (select pr.smen_nombre_principal CONCAT ' - ' CONCAT ca.smen_nombre_carpeta CONCAT ' - ' CONCAT op.smen_nombre_opcion AS menu
                    from SMENUOPCION op, SMENUCARPETA ca, SMENUPRINCIPAL pr where op.smen_opcion = dsol.ppro_id
                    and op.smen_carpeta = ca.smen_carpeta and ca.smen_princi = pr.smen_princi)  CONCAT ' --> ' CONCAT
                    do.dord_respcliente as COMENTARIO, RAND() AS INDX
                    from dordensolucion do, mordenservicio m, dsolicitud dsol
                    where do.ttipo_resp = 'CL' and do.mord_numero = m.mord_numero 
                    and dsol.msol_numero = do.mord_numero and dsol.dsol_numero = do.dord_iddet
                    and DATE(mord_fechacierre) BETWEEN '" + primerFecha + "' AND '" + segundaFecha + "' " +
                    "ORDER BY INDX FETCH FIRST 1 ROWS ONLY;");

                if (texto == "")
                {
                    texto = DBFunctions.SingleDataGlobal("SELECT GACT_MODULO CONCAT ' --> ' CONCAT GACT_COMENTARIO AS COMENTARIO, DATE(GACT_FECHA) AS FECHA, RAND() AS INDX FROM GACTUALIZACION WHERE DATE(GACT_FECHA) BETWEEN '" + primerFecha + "' AND '" + segundaFecha + "' ORDER BY INDX FETCH FIRST 1 ROWS ONLY");
                    if(texto == "")
                        texto = "Cualquier cambio significativo será notificado por este panel.";
                }
                    
            }
            return texto;
        }

        [Ajax.AjaxMethod()]
        public string carga_Mensaje_Texto()
        {
            TimeSpan hora = DateTime.Now.TimeOfDay;
            string mensaje = "";
            //int dia, mes, ano;
            //DateTime prueba = DateTime.Today.AddDays(-5);
            string primerFecha = DateTime.Today.AddDays(-5).GetDateTimeFormats()[5];
            string segundaFecha = DateTime.Today.GetDateTimeFormats()[5];

            //string fecha112 = prueba.GetDateTimeFormats()[5];
            //string fecha13 = prueba.Year + "-" + prueba.Month + "-" + prueba.Day;

            //dia = (DateTime.Now.Day - 5) == 0? 1: DateTime.Now.Day;
            //mes = DateTime.Now.Month;
            //ano = DateTime.Now.Year;
            //string fecha2 = ano + "-" + mes + "-" + dia;
            //string fecha1 = ano + "-" + mes + "-" + (dia - 5);
            //DataSet ds = new DataSet();
            //DBFunctions.RequestGlobal(ds, IncludeSchema.NO, "", "SELECT GACT_MODULO CONCAT ' --> ' CONCAT GACT_COMENTARIO AS COMENTARIO, DATE(GACT_FECHA) AS FECHA FROM GACTUALIZACION WHERE DATE(GACT_FECHA) BETWEEN '" + fecha1 + "' AND '" + fecha2 + "'");
            
            if (hora > new TimeSpan(06, 00, 00) && hora < new TimeSpan(13, 00, 00))
            {
                //AQUI VAN LOS MENSAJES DE LOS USUARIOS
                mensaje = DBFunctions.SingleData("SELECT PMEN_COMENTARIO CONCAT ' » ' CONCAT PMEN_AUTOR CONCAT ' « ', RAND() AS INDX FROM PMENSAJE ORDER BY INDX FETCH FIRST 1 ROWS ONLY");
                if(mensaje == "")
                {
                    mensaje = "Configuración - Administración Usuarios - mensajes --> En este menú podrá escribir mensajes para que los vean todos los usuarios que se encuentren en la página principal";
                }
            }
            else if (hora >= new TimeSpan(13, 00, 00) && hora <= new TimeSpan(13, 29, 59))
            {
                mensaje = "SE ESTAN REALIZANDO ACTUALIZACIONES, FAVOR NO INGRESE AL SISTEMA";
            }
            else
            {
                //AQUI VAN LOS MENSAJES DE LAS ACTUALIZACIONES
                mensaje = DBFunctions.SingleDataGlobal("SELECT GACT_MODULO CONCAT ' --> ' CONCAT GACT_COMENTARIO AS COMENTARIO, DATE(GACT_FECHA) AS FECHA, RAND() AS INDX FROM GACTUALIZACION WHERE DATE(GACT_FECHA) BETWEEN '" + primerFecha + "' AND '" + segundaFecha + "' ORDER BY INDX FETCH FIRST 1 ROWS ONLY");
                if (mensaje == "")
                {
                    mensaje = "Despues de la 1:30 pm se podrán leer todos los cambios realizados y desarrollos implementados.";
                }
            }
            return mensaje;
        }
        
    }
}