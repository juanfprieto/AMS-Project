using System;
using System.Web.UI.WebControls;
using AMS.DB;
using AMS.Forms;
using System.Web;
using System.Data.SqlClient;

namespace AMS.Tools 
{
	/// <summary>
	/// Clase que permite realizar métodos generales
	/// </summary>
	public class General : System.Web.UI.UserControl
	{
		public General()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		//Método que permite cargar los documentos en un dropdown.
		//ddlist: dropdown que entra por referencia para ser cargado
		//boton: Boton que se carga por refereencia para ser activado-desactivado
		//tipodocumento: el tipo de documento je: 'FC','NC','RC'
		//actividad: proceso que se realiza, ej: 'Taller cliente'='TC', 'Taller Garantías'='TG', 'Vehiculo Nuevo'='VN'
		//almacen: sede o almacen a la cual pertenece la transacción
		public static void cargarDocumentos(DropDownList ddlist, Button boton, string tipodocumento, string actividad, string almacen)
		{
			DatasToControls bind = new DatasToControls();
			String consulta;
			string nombreActividad;
			nombreActividad=DBFunctions.SingleData("Select tpro_nombre from DBXSCHEMA.TPROCESODOCUMENTO where TPRO_PROCESO='"+actividad+"'");
			if(nombreActividad == string.Empty)
			{
				General.mostrarMensaje("No está definido el proceso "+actividad);
				nombreActividad=actividad;
			}
			
			consulta="SELECT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = '"+tipodocumento+"' and PH.tpro_proceso = '"+actividad+"' AND P.PDOC_CODIGO = PH.PDOC_CODIGO and palm_almacen='"+almacen+"'";
				bind.PutDatasIntoDropDownList(ddlist,consulta);
				if(ddlist.Items.Count==0)
				{
					boton.Enabled=false;
					General.mostrarMensaje("No ha parametrizado ningún documento del tipo "+tipodocumento+" para la actividad "+nombreActividad+" en la sede "+almacen);
				}
				else
				{
					boton.Enabled=true;
				}			
		}
		

		//Método que permite mostrar algún mensaje en una caja de mensajes
		//mensaje: mensaje que se va a mostrar
		public static void mostrarMensaje(string mensaje)
		{
			HttpContext.Current.Response.Write("<script language:javascript>alert('"+mensaje+"');</script>");
		}
		


		/*public static void cargarDQuincena(int ano, int mes, string periodoLiquidacion)
		{
			DataSet retorna= new DataSet();
			string query;
			DBFunctions.Request(retorna,query);
			rerurn retorna;
		}*/

		//Método que inserta valores en la tabla dpagaafeceps con base en los valores de dquincena
		//ano: el año en el que se desea realizar el procedimiento
		//mes: el mes en el que se desea realizar el procedimiento
		//periodoLiquidacion: el periodo de liquidación en el que se desea realizar el procedimiento
		/*public static void cargarPagaAfecEPS(int ano, int mes, string periodoLiquidacion)
		{			
			Dataset quincena=new dataset();
			quincena=General.cargarDQuincena(ano,mes,periodoLiquidacion);
			
		}*/

        //                                          yyyy-mm-dd
        public static bool validarCierreFinanzas(string fecha, string opcion)
        {
            //param opcion es la opcion que define si la validacion va para la vigencia de cartera o tesoreria
            bool rta;
            //string fechaVigencia;
            string[] fechaVigencia;

            //proceso de conversión a entero y posterior comparación
            int mes, ano;
            ano = Convert.ToInt16(fecha.Split('-')[0]);// la fecha debe estar en formato yyyy-mm-dd
            mes = Convert.ToInt16(fecha.Split('-')[1]);// la fecha debe estar en formato yyyy-mm-dd

            switch (opcion)
            {
                case "C":
                    fechaVigencia = DBFunctions.SingleData("SELECT PANO_ANOVIGENTE || '-' || PMES_MESVIGENTE FROM CCARTERA;").Split('-');
                    //rta = fecha.Split('-')[0] != fechaVigencia.Split('-')[0] || fecha.Split('-')[1] != fechaVigencia.Split('-')[1] ? false : true;
                    rta = ano.Equals(Convert.ToInt16(fechaVigencia[0])) && mes.Equals(Convert.ToInt16(fechaVigencia[1])) ? true : false;
                    break;
                case "T":
                    fechaVigencia = DBFunctions.SingleData("SELECT CTES_ANOVIGE || '-' || CTES_MESVIGE FROM CTESORERIA;").Split('-');
                    //rta = fecha.Split('-')[0] != fechaVigencia.Split('-')[0] || fecha.Split('-')[1] != fechaVigencia.Split('-')[1] ? false : true;
                    rta = ano.Equals(Convert.ToInt16(fechaVigencia[0])) && mes.Equals(Convert.ToInt16(fechaVigencia[1])) ? true : false;
                    break;
                default:
                    rta = false;
                    break;
            }
            return rta;
        }
	}
}
