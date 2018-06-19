using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AMS.DB;

namespace AMS.SAC_Asesoria
{
	public partial class VerAgendaAsesores : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                // prueba usuario PortatilA
                String p = " prueba sesion PortatilA";
                ListarAgendaAsesores();
            }
		}

        //Crea todo el codigo de las tablas HTML para mostrar las agendas de los asesores.
        protected void ListarAgendaAsesores()
        {
            
            
            DataSet dsAsesores = new DataSet();

            DBFunctions.Request(dsAsesores, IncludeSchema.NO, 
                @"SELECT PASE.mnit_nit, MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_nombre2 CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' 
                CONCAT MNIT.mnit_apellido2 as nombre , COALESCE(g.contador, 0) as cont
                FROM pasesor PASE left join (SELECT MOR.pas_mnit_nit, count(MOR.pas_mnit_nit) as contador
                FROM dbxschema.dsolicitud DSOL,  MORDENSERVICIO MOR, msolicitud MSOL, mnit mn
                WHERE  MOR.msol_numero = DSOL.msol_numero  and MOR.test_codigo != 'CE'
                AND  msol.msol_numero=dsol.msol_numero AND msol.mnit_nitcli = mn.Mnit_nit group by MOR.pas_mnit_nit order by contador desc) as g on g.pas_mnit_nit = PASE.mnit_nit,mnit MNIT    
                WHERE MNIT.mnit_nit=PASE.mnit_nit ORDER BY cont DESC;  "); 

            string tablaAsesorHTML = "";
            for (int i = 0; i < dsAsesores.Tables[0].Rows.Count; i++)
            {
                tablaAsesorHTML += GenerarTablaAsesor(dsAsesores.Tables[0].Rows[i][0].ToString(), dsAsesores.Tables[0].Rows[i][1].ToString(), i);
            }

            contenedorAgendas.InnerHtml = tablaAsesorHTML;
        }

        //Genera el codigo HTML para crear una tabla con la informacion de las ordenes agendadas por un asesor.
        protected string GenerarTablaAsesor(string nit, string nombre, int secuencia)
        {
            DataSet dsAsesorOrdenes = new DataSet();
//            DBFunctions.Request(dsAsesorOrdenes, IncludeSchema.NO,
//                @"SELECT MOR.msol_numero, DSOL.dsol_detalle AS Detalle, 
//                (MN.mnit_nombres || MN.mnit_nombre2 || MN.mnit_apellidos || MN.mnit_apellido2) AS CLIENTE, COALESCE(MOR.test_codigo, 'AB') AS VIA
//                FROM dbxschema.dsolicitud DSOL,  MORDENSERVICIO MOR, msolicitud MSOL, mnit mn
//                WHERE  MOR.msol_numero = DSOL.msol_numero AND MOR.pas_mnit_nit='" + nit + @"' and MOR.test_codigo != 'CE'
//                AND  msol.msol_numero=dsol.msol_numero AND msol.mnit_nitcli = mn.Mnit_nit;");

            DBFunctions.Request(dsAsesorOrdenes, IncludeSchema.NO,
              @"SELECT MOR.msol_numero, DSOL.dsol_detalle AS Detalle, 
                (MN.mnit_nombres || MN.mnit_nombre2 || MN.mnit_apellidos || MN.mnit_apellido2) AS CLIENTE, COALESCE(MOR.test_codigo, 'AB') AS VIA
                FROM dbxschema.dsolicitud DSOL,  MORDENSERVICIO MOR, msolicitud MSOL, mnit mn
                WHERE  MOR.msol_numero = DSOL.msol_numero AND MOR.pas_mnit_nit='" + nit + @"' and MOR.test_codigo != 'CE'
                AND  msol.msol_numero=dsol.msol_numero AND msol.mnit_nitcli = mn.Mnit_nit
                union
                SELECT MOR.msol_numero, DSOL.dsol_detalle AS Detalle, 
                (MN.mnit_nombres || MN.mnit_nombre2 || MN.mnit_apellidos || MN.mnit_apellido2) AS CLIENTE, COALESCE(MOR.test_codigo, 'AB') AS VIA
                FROM dbxschema.dsolicitud DSOL,  MORDENSERVICIO MOR, msolicitud MSOL, mnit mn
                WHERE  MOR.msol_numero = DSOL.msol_numero AND MOR.pas_mnit_nit='" + nit + @"' and MOR.test_codigo = 'CE'
                AND  msol.msol_numero=dsol.msol_numero AND msol.mnit_nitcli = mn.Mnit_nit
                AND MORD_FECHACIERRE BETWEEN CURRENT DATE- 15 DAY AND CURRENT DATE;");

            string codigoHTMLBody = "";
            int countCerradas = 0, countAbiertas = 0;
            for (int j = 0; j < dsAsesorOrdenes.Tables[0].Rows.Count; j++)
            {
                string claseTipoOrden = "";
                if (dsAsesorOrdenes.Tables[0].Rows[j][3].ToString() == "AS")
                {
                    claseTipoOrden = "class='ordenAsignada'";
                    countAbiertas++;
                }
                else if (dsAsesorOrdenes.Tables[0].Rows[j][3].ToString() == "ED")
                {
                    claseTipoOrden = "class='ordenDesarrollo'";
                    countAbiertas++;
                }
                else if(dsAsesorOrdenes.Tables[0].Rows[j][3].ToString() == "CC")
                {
                    claseTipoOrden = "class='ordenCCalidad'";
                    //countAbiertas++;
                }
                else if (dsAsesorOrdenes.Tables[0].Rows[j][3].ToString() == "IM")
                {
                    claseTipoOrden = "class='ordenImplementada'";
                    //countAbiertas++;
                }
                else if (dsAsesorOrdenes.Tables[0].Rows[j][3].ToString() == "CE")
                {
                    claseTipoOrden = "class='ordenCerrada'";
                    countCerradas++;
                }

                codigoHTMLBody += "<tr " + claseTipoOrden + "><td><font size=5>" + dsAsesorOrdenes.Tables[0].Rows[j][0].ToString() + "</font></td>";
                codigoHTMLBody += "<td>" + dsAsesorOrdenes.Tables[0].Rows[j][1].ToString() + "</td>";
                codigoHTMLBody += "<td><center>" + dsAsesorOrdenes.Tables[0].Rows[j][2].ToString() + "</center></td></tr>";
            }

            string codigoHTML = "<div id=\"asesorCabeza_" + secuencia + "\" onClick=\"desplegar(\'asesorCuerpo_" + secuencia + "\');\" style=\"cursor:pointer\"><table class='tablaAsesor'>";
            codigoHTML += "<tr><td colspan='2' style='width:70%'><font size=4>" + nombre + "</font></td><td style='width:30%'><font size=3>Ordenes en Proceso: " + countAbiertas + "<br>Ordenes Cerradas: " + countCerradas + "</font></td></tr>";
            codigoHTML += "</table></div>";

            codigoHTML += "<div id='asesorCuerpo_" + secuencia + "' class='cuerpoTablaAsesor'><table class='tablaAsesor'>";
            codigoHTML += @"<tr><td style='width:15%'><font size=5>Número Orden</font></td>
                                <td style='width:55%'><font size=5>Descripción</font></td>
                                <td style='width:30%'><font size=5>Empresa</font></td></tr>";
            
            codigoHTML += codigoHTMLBody;

            codigoHTML += "</table></div><br><br>";
            return codigoHTML;
        }
	}
}
