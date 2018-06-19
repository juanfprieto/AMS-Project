using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Automotriz
{
    public partial class AMS_Automotriz_PlanningEntregas : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            }
        }
        protected void abrirPlaning(object sender, EventArgs z)
        {
            string nombreProcedure = "";
            {
                //llenar string
                /*
                 * Para llenar este string hay que poner el nombre del procedimiento
                 * EL procedimiento:
                 * Debe tener los campos que se mostrarán como información característica de cada div.
                 * tiene que haber un campo llamado 'DEFINICIONCOL' este campo debe ir al final y tendra el valor que asignará a su respectivo color(un campo IGUAL estará en al tabla de planningcolor)
                 * procure que el procedimiento no muestre más de 10 campos, ya que es un tablero electrónico, es bueno mostrar información concreta y concatenada
                 * */
                nombreProcedure = ddlProcedimiento.SelectedValue;
            }

            //el dataset y procedimiento se validan primero acá :S y pues manejar sesiones porque que

            Response.Write("<script language:javascript> w=window.open('AMS.Automotriz.TableroEntregaVehiculos.aspx?select=" + nombreProcedure + "');</script>");//?tal=" + "algo" + "&pag=1&rdrt=0','','scrollbars=1,fullscreen=yes,width=1000,height=710,left=0');</script>");
        }
    }
}