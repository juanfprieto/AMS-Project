
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Collections.Generic;
using AMS.Tools;

namespace AMS.Automotriz
{
	/// <summary>
	/// Descripción breve de AMS_Automotriz_CitasTaller.
	/// </summary>
	public partial class AMS_Automotriz_CitasTaller : System.Web.UI.Page
	{
		public string strEmpresa,strFecha,strHora,strFondo,strImagen;
		protected System.Web.UI.WebControls.Label lblFecha;
        private DataTable filasCitas = new DataTable();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			bool vuelveR = false;
                
			if(!IsPostBack)
			{
                Session["INICIAROTAR"] = 0;
                if (Request.QueryString["retorno"] == null)
                    Session["FECHACUADRO"] = null;
				DatasToControls bind=new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlTaller, "select pa.PALM_ALMACEN, pa.PALM_DESCRIPCION from DBXSCHEMA.PALMACEN pa where (pa.pcen_centtal is not null  or pcen_centcoli is not null) and pa.TVIG_VIGENCIA = 'V' order by pa.PALM_DESCRIPCION;");
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				ViewState["EMPRESA"]=DBFunctions.SingleData("SELECT CEMP_NOMBRE FROM CEMPRESA;");
				//Recorrer archivos de fondo
				try
				{
					DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["PathToBgsCuadroTaller"]);
					ArrayList arlFondos=new ArrayList();
					FileInfo[] rgFiles = di.GetFiles("*.jpg");
					foreach(FileInfo fi in rgFiles)
					{
						arlFondos.Add(fi.Name);
					}
					ViewState["FONDOS"]=arlFondos;
				}
				catch{}
				//Imagenes muestra
				try
				{
					DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["PathToImgsCuadroTaller"]);
					ArrayList arlFondos=new ArrayList();
					FileInfo[] rgFiles = di.GetFiles("*.jpg");
					foreach(FileInfo fi in rgFiles)
					{
						arlFondos.Add(fi.Name);
					}
					ViewState["IMAGENES"]=arlFondos;
				}
				catch{}
				ViewState["TEMPSEGS"] = ConfigurationManager.AppSettings["TempCuadroTallerSegs"];
				
			}

            if (Request.QueryString["rdr"] != null && Session["FECHACUADRO"] != null && Session["TALLERCUADRO"] != null) 
            {
                vuelveR = true;
                txtFecha.Text = Convert.ToDateTime(Session["FECHACUADRO"]).ToString("yyyy-MM-dd");
                ddlTaller.SelectedIndex = ddlTaller.Items.IndexOf(ddlTaller.Items.FindByValue(Session["TALLERCUADRO"].ToString()));
                ViewState["FechaConsulta"] = Session["FECHACUADRO"];
                imgSeleccionar_Click(sender, null);
            }

            if (Request.QueryString["todos"] != null)
            {
                vuelveR = true;
                imgSeleccionar_Click(sender, null);
            }

            if ((vuelveR || IsPostBack ))
            {
				Session["TALLERCUADRO"] = ddlTaller.SelectedValue;
                //prueba:
                CargarTabla(Convert.ToDateTime(txtFecha.Text));

                
                //Cuadro citas
				if(ViewState["CONTIMAGEN"]==null) ViewState["CONTIMAGEN"] = 0;
				int conteo=(int)ViewState["CONTIMAGEN"];
				ArrayList arlImagenes = (ArrayList)ViewState["IMAGENES"];
				if(conteo==0)
				{
					ViewState["TEMPSEGS"] = ConfigurationManager.AppSettings["TempCuadroTallerSegs"];
					//CargarTabla(Convert.ToDateTime(txtFecha.Text));
				}
					//Imagenes
                else if (arlImagenes != null && conteo <= arlImagenes.Count && Request.QueryString["serv"] != null)
				{
					ViewState["TEMPSEGS"] = ConfigurationManager.AppSettings["TempImgsCuadroTallerSegs"];
					pnlFecha.Visible = pnlCitas.Visible = false;
					pnlImagenes.Visible = true;
					strImagen = "<img src=\"../img/CuadroTaller/imagenes/" + arlImagenes[conteo - 1].ToString() + "\" />";
				}
				//Redireccionar a planning
				else
				{
                    if(Request.QueryString["serv"]!=null)
					Response.Redirect("AMS.Automotriz.PlanificacionTaller.aspx?tal="+ddlTaller.SelectedValue+"&pag=1&rdrt=1");
				}
				ViewState["CONTIMAGEN"] = conteo + 1;
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
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

        protected void Do_PostBack(object sender, EventArgs e)
        {}

		private void CargarTabla(DateTime fecha)
		{
			//Armar grilla
			int dia,mes,ano;
			string color="",estado="";
			bool hoy=false,futura=false,marcado=false;
			Session["FECHACUADRO"]=fecha;
			dia=fecha.Day;
			mes=fecha.Month;
			ano=fecha.Year;
			if(dia==DateTime.Now.Day && mes==DateTime.Now.Month && ano==DateTime.Now.Year)
				hoy=true;
			else if(DateTime.Now<fecha)
				futura=true;

			IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
			strFondo="";
			strEmpresa=ViewState["EMPRESA"].ToString();
			strFecha=fecha.ToString("dddd dd MMM",culture);
			strHora=DateTime.Now.ToString("hh : mm : ss");
			if(ViewState["FONDOS"]!=null)
			{
				ArrayList arlFondos=(ArrayList)ViewState["FONDOS"];
				if(arlFondos.Count>0)
					strFondo="../img/CuadroTaller/"+arlFondos[new Random((int)DateTime.Now.Ticks).Next(0,arlFondos.Count)].ToString();
			}

			DataSet dsCitas=new DataSet();
            DataSet dsCitasConAux = new DataSet();
			DataTable dtCitas=new DataTable();
            string almacen = Session["TALLERCUADRO"].ToString();
            String sqlContadorAux = string.Format(
                 "select A.HORA, A.MINUTO, COUNT(*) AS CONTADOR from  \n" +
                 "   (SELECT HOUR(MCIT_HORA) AS HORA,  \n" +
                 "           MINUTE(MCIT_HORA) AS MINUTO  \n" +
                 "  FROM DBXSCHEMA.MCITATALLER MC  \n" +
                 "  LEFT JOIN PVENDEDOR PV on PV.PVEN_CODIGO = MC.MCIT_CODVEN  \n" +
                 "  LEFT JOIN PALMACEN PAL ON PAL.PALM_ALMACEN = MC.PALM_ALMACEN  \n" +
                 "  LEFT JOIN PKIT PK ON PK.pkit_codigo = MC.pkit_codigo  \n" +
                 "WHERE DAY(MC.MCIT_FECHA) = {0}  \n" +
                 "AND   MONTH(MC.MCIT_FECHA) = {1}  \n" +
                 "AND   YEAR(MC.MCIT_FECHA) = {2}  \n" +
                 "AND   PV.PVEN_CODIGO = MC.MCIT_CODVEN  \n"
                 , dia
                 , mes
                 , ano); 

            String sql = string.Format(
                 "SELECT HOUR(MCIT_HORA) AS HORA,  \n" +
                 "       MINUTE(MCIT_HORA) AS MINUTO,  \n" +
                 "       PAL.PALM_ALMACEN,  \n" +
                 "       PV.PVEN_CODIGO,  \n" +
                 "       PV.PVEN_NOMBRE,  \n" +
                 "       PK.PKIT_NOMBRE,  \n" +
                 "       MC.MCIT_OBSERVACION,  \n" +
                 "       MCIT_PLACA,  \n" +
                 "       MCIT_NOMBRE,  \n" +
                 "       TESTCIT_ESTACITA,  \n" +
                 "       MC.MCIT_ESTADOCLI    \n"  +
                 "FROM DBXSCHEMA.MCITATALLER MC  \n" +
                 "  LEFT JOIN PVENDEDOR PV on PV.PVEN_CODIGO = MC.MCIT_CODVEN  \n" +
                 "  LEFT JOIN PALMACEN PAL ON PAL.PALM_ALMACEN = MC.PALM_ALMACEN  \n" +
                 "  LEFT JOIN PKIT PK ON PK.pkit_codigo = MC.pkit_codigo  \n" +
                 "WHERE DAY(MC.MCIT_FECHA) = {0}  \n" +
                 "AND   MONTH(MC.MCIT_FECHA) = {1}  \n" +
                 "AND   YEAR(MC.MCIT_FECHA) = {2}  \n" +
                 "AND   PV.PVEN_CODIGO = MC.MCIT_CODVEN  \n"
                 , dia
                 , mes
                 , ano);

            if (Request.QueryString["todos"] == null)
            {
                sql += string.Format(
                    "AND   MC.PALM_ALMACEN = '{0}'  \n" +
                    "ORDER BY HORA,  \n" +
                    "         MINUTO"
                    , almacen);

                sqlContadorAux += string.Format(
                    "AND   MC.PALM_ALMACEN = '{0}'  \n" +
                    "ORDER BY HORA,  \n" +
                    "         MINUTO ) as A GROUP BY HORA, MINUTO;"
                    , almacen);
            }
            else
            {
                sql += "ORDER BY HORA,  \n" +
                         "         MINUTO";

                sqlContadorAux += "ORDER BY HORA,  \n" +
                         "         MINUTO ) as A GROUP BY HORA, MINUTO;";
            }
                

			DBFunctions.Request(dsCitas,IncludeSchema.NO, sql);
            DBFunctions.Request(dsCitasConAux, IncludeSchema.NO, sqlContadorAux);
            String empresa = "GENERAL";
            if (Request.QueryString["todos"] == null)
                empresa = DBFunctions.SingleData("SELECT palm_descripcion FROM DBXSCHEMA.PALMACEN where TVIG_VIGENCIA = 'V' and Palm_almacen='" + almacen + "'");

            //citasTitulo
            string codigoTablaHTML = "";
            string columnas = "";
            codigoTablaHTML += CrearCitasTitulo(empresa);

            if (dsCitas.Tables[0].Rows.Count != 0)
			{
				//Columnas
                filasCitas.Columns.Add("colHora");

                Hashtable colsVendedores = new Hashtable();
                DataView view = new DataView(dsCitas.Tables[0]);
                DataTable dtVendedores = view.ToTable(true, "PVEN_CODIGO", "PVEN_NOMBRE");

                for (int i = 0; i < dtVendedores.Rows.Count; i++)
				{
                    string vendedor = dtVendedores.Rows[i]["PVEN_NOMBRE"].ToString() + "<br>(" +
                            dtVendedores.Rows[i]["PVEN_CODIGO"].ToString() + ")";

                    filasCitas.Columns.Add("col_"+ i);
                    columnas += CrearColumna(vendedor);
                    colsVendedores.Add(dtVendedores.Rows[i]["PVEN_CODIGO"].ToString(), i + 1);
				}

                //citasEncabezado
                codigoTablaHTML += CrearCitasEncabezado(columnas);

                //Contador de Tecnicos
                int contTec = 0;

                //Contenido de tabla de Citas
                for (int f = 1; f <= dsCitas.Tables[0].Rows.Count; f++)
                {
                    //Ajuste del contador para no saltarse Tecnicos cuando regresa del primer ciclo
                    f--;

                    //Extraer fila de la agenda de Tecnicos
                    DataRow rowCita = dsCitas.Tables[0].Rows[f];

                    //Obtener hora de la Operación
                    int hora = Convert.ToInt32(rowCita["HORA"]);
                    int minuto = Convert.ToInt32(rowCita["MINUTO"]);
                    string horaCita = hora.ToString("00") + ":" + minuto.ToString("00");

                    //Crear fila del Tablero de Citas ------------->
                    DataRow nFila = filasCitas.NewRow();

                    //Validar que hallan campos para operar
                    if (nFila.ItemArray.Length > 0)
                    {
                        //Llenar todas las celdas de la fila con columnas en blanco "td" HTML
                        for (int i = 1; i < nFila.ItemArray.Length; i++)
                        {
                            nFila[i] = "<td></td>";
                        }

                        //Definir el primer campo siempre como la hora en cuestion
                        nFila[0] = "<td style=\"width: 62px\">" + horaCita + "</td>";

                        //Obtener la cantidad de Tecnicos operando a la hora en cuestion
                        int numTecnicos = Convert.ToInt32(dsCitasConAux.Tables[0].Rows[contTec][2]);

                        //Hacer llenado de las celdas con respecto al numero de Tecnicos
                        for (int k = 0; k < numTecnicos; k++){
                            //Traer Datos del Tecnico
                            string idVendedor = rowCita["PVEN_CODIGO"].ToString();
                            string placa = rowCita["MCIT_PLACA"].ToString();
                            string nomCliente = rowCita["MCIT_NOMBRE"].ToString();
                            string kit = rowCita["PKIT_NOMBRE"].ToString();
                            string observacion = rowCita["MCIT_OBSERVACION"].ToString();
                            string estadoCita = rowCita["TESTCIT_ESTACITA"].ToString();
                            string estadoCliente = rowCita["MCIT_ESTADOCLI"].ToString();
                            color = "GhostWhite";

                            //Definir Color para el estado actual de la Cita
                            if (!marcado && hoy && ((hora == DateTime.Now.Hour && minuto > DateTime.Now.Minute) || (hora > DateTime.Now.Hour))){
                                marcado = true;
                                color = "PaleTurquoise";
                            }
                            if (!futura){
                                //No ha llegado la hora?
                                if (hoy && (hora > DateTime.Now.Hour || (hora == DateTime.Now.Hour && minuto > DateTime.Now.Minute)))
                                    color = "amarillo";
                                else{
                                    color = "amarillo";
                                    estado = estadoCita;
                                    if (estado.Equals("C")) color = "verde";
                                    if (estado.Equals("R")) color = "naranja";
                                    if (estado.Equals("N")) color = "rojo";
                                }
                            }
                            else{
                                color = "amarillo";
                            }

                            //Obtener la posicion de la columna a la cual pertenece el Tecnico actual
                            int index = (int)colsVendedores[idVendedor];

                            //Descripcion de estado de cliente
                            string rojoFondo = "";
                            string amarilloFondo = "";
                            string verdeFondo = "";

                            if (estadoCliente == "R") //Rojo
                                rojoFondo = "background-color:#FF0000;";
                            else if (estadoCliente == "A") //Amarillo
                                amarilloFondo = "background-color:#FFFF00;";
                            else if (estadoCliente == "V") //Verde
                                verdeFondo = "background-color:#00CC00;";

                            //Definir contenido de la celda en formato HTML
                            string semaforo = "<div class='cajaSemaforo'><div class='bombillo' style='left: 89%;" + rojoFondo + "'></div><div class='bombillo' style='left: 92%;" + amarilloFondo + "'></div><div class='bombillo' style='left: 95%;" + verdeFondo + "'></div></div>";
                            string texto = semaforo + "<font size='6'><b>" + placa + "</b></font><br><b>" + nomCliente + "</b><br>" +
                                            (kit != "" ? kit + "<br>" : "") + observacion;

                            //Ubicar el contenido y formato en la celda según la posicion de la columna del Tecnico
                            nFila[index] = "<td class=\"" + color + "\">" + texto + "</td>";

                            //Aumentar contador y extraer fila de la agenda de Tecnicos
                            f++;
                            if (f < dsCitas.Tables[0].Rows.Count)
                            {
                                rowCita = dsCitas.Tables[0].Rows[f];
                            }
                        }

                        //Agregar fila editada a la tabla de Citas
                        filasCitas.Rows.Add(nFila);

                        //Aumentar contador Auxiliar de número de Tecnicos
                        contTec++;
                    }
                }

                //citasCuerpo
                codigoTablaHTML += CrearCitasCuerpo();
                citasTaller.InnerHtml = codigoTablaHTML;
			}
			else
			{
				citasTaller.InnerHtml = "No existen citas para la fecha seleccionada";
			}
		}

		protected void imgVolver_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect("AMS.Automotriz.CitasTaller.aspx");
		}

		protected void imgActualizar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
		}

		protected void imgSeleccionar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			DateTime fecha=new DateTime();
			try
			{
				fecha=Convert.ToDateTime(txtFecha.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Fecha no valida!");
				return;
			}
			pnlFecha.Visible=false;
			pnlCitas.Visible=true;
            Session["INICIAROTAR"] = 1;
			ViewState["FechaConsulta"]=fecha;
		}

        //citasTitulo
        private string CrearCitasTitulo(string cede)
        {
            string codTitulo = "";
            codTitulo += "<table class='citasTitulo'><tr><td style='font-size: 18px;'>";
            codTitulo += "CITAS PROGRAMADAS: " + cede;
		    codTitulo += "</td><td align=\"right\">";
			codTitulo += "<input type=\"button\" id=\"camb\" onClick=\"cambioScroll(this)\" value=\"Parar\">";
            codTitulo += "</td></tr></table>";
			
            return codTitulo;
        }

        //citasEncabezado
        private string CrearCitasEncabezado(string columnas)
        {
            string codTitulo = "";
            codTitulo += "<table class='citasEncabezado'><tr>";
            codTitulo += "<th style=\"width: 50px\">HORA</th>";
            codTitulo += columnas;
            codTitulo += "</tr></table>";
		
            return codTitulo;
        }
        private string CrearColumna(string titulo)
        {
            string codColumna = "";
            codColumna += "<th>" + titulo + "</th>";
            
            return codColumna;
        }

        //citasCuerpo
        private string CrearCitasCuerpo()
        {
            string codCuerpo = "";
            codCuerpo += "<table class='citasTaller'>";

            for (int i = 0; i < filasCitas.Rows.Count; i++)
            {
                codCuerpo += "<tr>";
                for (int j = 0; j < filasCitas.Columns.Count; j++)
                {
                    codCuerpo += filasCitas.Rows[i][j];
                }
                codCuerpo += "</tr>";
            }

            codCuerpo += "</table>";

            return codCuerpo;
        }
        private void CrearFila(string hora, string texto, string color, int indice)
        {
            DataRow nFila = filasCitas.NewRow();
            int posF = filasCitas.Rows.Count;

            if (posF > 0 && filasCitas.Rows[posF - 1][0] == hora)
            {
                filasCitas.Rows[posF - 1][indice] = "<td class=\"" + color + "\">" + texto + "</td>";
            }
            else
            {
                nFila[0] = "<td style=\"width: 62px\">" + hora + "</td>";
                for (int i = 1; i < nFila.ItemArray.Length; i++)
                {
                    if (i != indice)
                    {
                        nFila[i] = "<td></td>";
                    }
                    else
                    {
                        nFila[i] = "<td class=\"" + color + "\">" + texto + "</td>";
                    }
                }
                filasCitas.Rows.Add(nFila);
            }
        }

	}
}
