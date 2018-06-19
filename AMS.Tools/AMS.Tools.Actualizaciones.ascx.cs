using AMS.DB;
using AMS.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Tools
{
    public partial class AMS_Tools_Actualizaciones : System.Web.UI.UserControl
    {
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string usuario = HttpContext.Current.User.Identity.Name.ToLower();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Ajax
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Tools_Actualizaciones));
            if(!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                //bind.PutDatasIntoDropDownList(ddlPadre, "SELECT SMEN_PRINCI, SMEN_NOMBRE_PRINCIPAL AS PRINCIPAL FROM SMENUPRINCIPAL");
                bind.PutDatasIntoDropDownList(ddlPadre,
                    @"select DISTINCT
                    SUBSTR(
                    REPLACE(mobj_nombre, 'AMS.', ''),
                    1,
                    LOCATE('.', REPLACE(mobj_nombre, 'AMS.', ''), 1) - 1
                    ) from MOBJETOECAS where tpro_id = 'ASCX'; "
                );

                ddlPadre.Items.Insert(0, "Seleccione...");
                bind.PutDatasIntoDropDownList(ddlVersion, "SELECT GVER_VERSION, GVER_VERSION from GVERSION order by GVER_VERSION desc;");
            }
        }

        [Ajax.AjaxMethod()]
        public DataSet cargar_Opcion(string value)
        {
            DataSet rta = new DataSet();
            //DBFunctions.RequestGlobal(rta, IncludeSchema.NO,"", "SELECT SMEN_CARPETA, SMEN_NOMBRE_CARPETA AS CARPETA FROM SMENUCARPETA WHERE SMEN_PRINCI = '" + value + "';");
            DBFunctions.RequestGlobal(rta, IncludeSchema.NO, "",
                "select REPLACE(mobj_nombre,  'AMS." + value + ".', '') as SMEN_CARPETA, REPLACE(mobj_nombre,  'AMS." + value + ".', '') AS CARPETA " +
                "from MOBJETOECAS where tpro_id = 'ASCX' and mobj_nombre like '%." + value + "%' ");
                
            //ddlHijo.Items.Insert(0, "Seleccione..."); por ser ajax.
            return rta;
        }

        [Ajax.AjaxMethod()]
        public DataSet cargar_Menu(string value)
        {
            DataSet rta = new DataSet();
            DBFunctions.RequestGlobal(rta, IncludeSchema.NO,"", "SELECT SMEN_OPCION, SMEN_NOMBRE_OPCION AS OPCION FROM SMENUOPCION WHERE SMEN_CARPETA = '" + value + "';");
            //ddlMenu.Items.Insert(0, "Seleccione...");
            return rta;
        }

        [Ajax.AjaxMethod()]
        public string guardar(string value, string mensaje, string version, string pTabla)
        {
            string rta = "";
            int insert = 0;
            string opcion = DBFunctions.SingleData("select mobj_id from mobjetoecas where mobj_nombre like '%" + value + "%' and tpro_id = 'ASCX'");
            string tabla = "";
            if (opcion == "1960" || opcion == "1940")
            {
                tabla = " and GMAN_PTABLA='" + pTabla + "'";
            }
            else
            {
                pTabla = "";
            }
            
            bool existeRegistro = DBFunctions.RecordExist("select smen_opcion from gmanual where smen_opcion =" + opcion + tabla);

            if(existeRegistro == true)
            {
                if (mensaje.Length <= 30000)
                {
                    insert = DBFunctions.NonQuery(
                    "UPDATE DBXSCHEMA.GMANUAL SET GMAN_CONTENIDO = '" + mensaje + "' WHERE SMEN_OPCION = " + opcion + tabla);
                }
                else
                {
                    string mensajeAux = mensaje.Substring(0, 30000);
                    mensaje = mensaje.Substring(30000, mensaje.Length - 30000);

                    insert = DBFunctions.NonQuery(
                    "UPDATE DBXSCHEMA.GMANUAL SET GMAN_CONTENIDO = '" + mensajeAux + "' WHERE SMEN_OPCION = " + opcion + tabla);

                    while (mensaje.Length > 30000)
                    {
                        mensajeAux = mensaje.Substring(0, 30000);
                        mensaje = mensaje.Substring(30000, mensaje.Length - 30000);

                        insert = DBFunctions.NonQuery(
                        "UPDATE DBXSCHEMA.GMANUAL SET GMAN_CONTENIDO = GMAN_CONTENIDO CONCAT '" + mensajeAux + "' WHERE SMEN_OPCION = " + opcion + tabla);
                        
                    }

                    insert = DBFunctions.NonQuery(
                        "UPDATE DBXSCHEMA.GMANUAL SET GMAN_CONTENIDO = GMAN_CONTENIDO CONCAT '" + mensaje + "' WHERE SMEN_OPCION = " + opcion + tabla);

                }
            }
            else
            {
                insert = DBFunctions.NonQuery(
                "INSERT INTO DBXSCHEMA.GMANUAL(SMEN_OPCION, GMAN_CONTENIDO, SUSU_LOGIN, GMAN_MODIFECHA, gver_version, gman_ptabla) " +
                "VALUES(" + opcion + ", '" + mensaje + "','" + usuario + "', DEFAULT,'" + version + "','" + pTabla + "'); ");
            }
            
            //int insert = DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.GACTUALIZACION(GACT_MODULO, GACT_COMENTARIO, SUSU_CODIGO, GACT_FECHA)VALUES('" + value + "', '" + mensaje + "'," + Codusuario + ", DEFAULT); ");
            rta = insert.ToString();
            return rta;
        }

        [Ajax.AjaxMethod()]
        public string[] cargar_Manual(string value, string pTabla, string index)
        {
            string[] arrayResp = new string[2];
            string opcion = DBFunctions.SingleData("select mobj_id from mobjetoecas where mobj_nombre like '%" + value  + "%' and tpro_id = 'ASCX'");

            if ((opcion == "1960" || opcion == "1940") && index == "0")
            {
                arrayResp[0] = "";
                arrayResp[1] = "T";
            }
            else
            {
                arrayResp[0] = DBFunctions.SingleData("select gman_contenido from gmanual where smen_opcion =" + opcion);
                arrayResp[1] = DBFunctions.SingleData("select gman_modifecha from gmanual where smen_opcion =" + opcion);
            }
            
            return arrayResp;
        }

    }
}