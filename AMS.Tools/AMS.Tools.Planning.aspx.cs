using AMS.DB;
using Ajax;
using System;
using System.Collections;
using System.Configuration;
using System.Data;

namespace AMS.Tools
{
    public partial class Planning : System.Web.UI.Page
    {
        private string index = ConfigurationManager.AppSettings["MainIndexPage"] + "?process=";
        private int paginaActual = 1;
        private string nombreProcedimiento = "";
        private ArrayList arrParametrosPlanning = new ArrayList();
        private DataSet dsEstadosPlanning = new DataSet();
        private DataSet dsTiempoPlanning = new DataSet();

        private DataSet dsContenido = new DataSet();
        private int numFilasMaxPorPag = 0;
        private int numColumMaxPorPag = 0;
        private ArrayList idsDescriptor = new ArrayList();
        private ArrayList cntDescriptor = new ArrayList();

        private string campoDescriptorY = "";
        private string contDescriptorY = "";
        private int maxPaginas = 1;

        private string campoDescriptorX = "";
        private string contDescriptorX = "";
        private string htmlContenido = "";
        private string campoLineaTiempo = "";
        private string campoFichaContenido = "";
        private string campoHorasTarea = "";

        private string urlImgLeft = "";
        private string urlImgBanner = "";
        private string urlImgRigth = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Tools.Planning));

            nombreProcedimiento = Request.QueryString["select"].ToString().ToUpper();

            if (ViewState["paginaActual"] != null)
                paginaActual = Convert.ToInt32(ViewState["paginaActual"].ToString());

            if (ViewState["maxPaginas"] != null)
                maxPaginas = (int)ViewState["maxPaginas"];

            if (!IsPostBack)
            {
                CargarDatos();
                CrearDescriptor(paginaActual);
            }
        }

        private void CargarDatos()
        {
            if (paginaActual == 1)
            {
                arrParametrosPlanning = DBFunctions.RequestGlobalAsCollection("SELECT * FROM DBXSCHEMA.SPLANNINGENERAL WHERE SPLAN_PROCEDIMIENTO = '" + nombreProcedimiento + "'");
                DBFunctions.RequestGlobal(dsEstadosPlanning, IncludeSchema.NO,"", "select * from SDPLANNINGENERAL where splan_procedimiento = '" + nombreProcedimiento + "' order by sdplan_secuencia;");
                DBFunctions.RequestGlobal(dsTiempoPlanning, IncludeSchema.NO, "", "select * from SPLINEATIEMPO where splan_procedimiento = '" + nombreProcedimiento + "' order by splin_id;");
                DBFunctions.Request(dsContenido, IncludeSchema.NO, ((Hashtable)arrParametrosPlanning[0])["SPLAN_SQL"].ToString().Replace("@NOHTML","").Replace("`", "'"));

                ViewState["dsContenido"] = dsContenido;
                ViewState["arrParametrosPlanning"] = arrParametrosPlanning;
                ViewState["dsEstadosPlanning"] = dsEstadosPlanning;
                ViewState["dsTiempoPlanning"] = dsTiempoPlanning;

                campoDescriptorY = ((Hashtable)arrParametrosPlanning[0])["SPLAN_PARAMY"].ToString();
                ViewState["campoDescriptorY"] = campoDescriptorY;
                contDescriptorY = ((Hashtable)arrParametrosPlanning[0])["SPLAN_PARAMYDESC"].ToString();
                ViewState["contDescriptorY"] = contDescriptorY;

                campoDescriptorX = ((Hashtable)arrParametrosPlanning[0])["SPLAN_PARAMX"].ToString();
                ViewState["campoDescriptorX"] = campoDescriptorX;
                contDescriptorX = ((Hashtable)arrParametrosPlanning[0])["SPLAN_PARAMXDESC"].ToString();
                ViewState["contDescriptorX"] = contDescriptorX;

                campoLineaTiempo = ((Hashtable)arrParametrosPlanning[0])["SPLAN_TIEMPO"].ToString();
                ViewState["campoLineaTiempo"] = campoLineaTiempo;

                campoFichaContenido = ((Hashtable)arrParametrosPlanning[0])["SPLAN_FICHACONTE"].ToString();
                ViewState["campoFichaContenido"] = campoFichaContenido;
                campoHorasTarea = ((Hashtable)arrParametrosPlanning[0])["SPLAN_HORASTAREA"].ToString();
                ViewState["campoHorasTarea"] = campoHorasTarea;

                numFilasMaxPorPag = Convert.ToInt32(((Hashtable)arrParametrosPlanning[0])["SPLAN_MAXFILAS"].ToString());
                ViewState["numFilasMaxPorPag"] = numFilasMaxPorPag;

                //Carga de Logos.
                urlImgLeft = ((Hashtable)arrParametrosPlanning[0])["SPLAN_URLLOGO1"].ToString();
                ViewState["urlImgLeft"] = urlImgLeft;
                urlImgBanner = ((Hashtable)arrParametrosPlanning[0])["SPLAN_URLBANNER"].ToString();
                ViewState["urlImgBanner"] = urlImgBanner;
                urlImgRigth = ((Hashtable)arrParametrosPlanning[0])["SPLAN_URLLOGO2"].ToString();
                ViewState["urlImgRigth"] = urlImgRigth;

                if (dsContenido.Tables[0].Rows.Count > 0)
                {
                    string idAnt = dsContenido.Tables[0].Rows[0][campoDescriptorY].ToString();
                    string cntAnt = dsContenido.Tables[0].Rows[0][contDescriptorY].ToString();
                    idsDescriptor.Add(idAnt);
                    cntDescriptor.Add(cntAnt);
                    for (int h = 1; h < dsContenido.Tables[0].Rows.Count; h++)
                    {
                        string id = dsContenido.Tables[0].Rows[h][campoDescriptorY].ToString();
                        string cnt = dsContenido.Tables[0].Rows[h][contDescriptorY].ToString();
                        if (idAnt != id)
                        {
                            idsDescriptor.Add(id);
                            cntDescriptor.Add(cnt);
                            idAnt = id;
                        }
                    }
                    ViewState["idsDescriptor"] = idsDescriptor;
                    ViewState["cntDescriptor"] = cntDescriptor;
                }
            }
            else
            {
                dsContenido = (DataSet)ViewState["dsContenido"];
                arrParametrosPlanning = (ArrayList)ViewState["arrParametrosPlanning"];
                dsEstadosPlanning = (DataSet)ViewState["dsEstadosPlanning"];
                dsTiempoPlanning = (DataSet)ViewState["dsTiempoPlanning"];
                idsDescriptor = (ArrayList)ViewState["idsDescriptor"];
                cntDescriptor = (ArrayList)ViewState["cntDescriptor"];

                campoDescriptorY = (string)ViewState["campoDescriptorY"];
                contDescriptorY = (string)ViewState["contDescriptorY"];

                campoDescriptorX = (string)ViewState["campoDescriptorX"];
                contDescriptorX = (string)ViewState["contDescriptorX"];
                numFilasMaxPorPag = (int)ViewState["numFilasMaxPorPag"];

                campoFichaContenido = (string)ViewState["campoFichaContenido"];
                campoLineaTiempo = (string)ViewState["campoLineaTiempo"];
                campoHorasTarea = (string)ViewState["campoHorasTarea"];

                urlImgLeft = (string)ViewState["urlImgLeft"];
                urlImgBanner = (string)ViewState["urlImgBanner"];
                urlImgRigth = (string)ViewState["urlImgRigth"];
            }
            CargarHeader();
        }

        private void CargarHeader()
        {
            //Creacion fichas de los diferentes estados parametrizados y su color.
            string htmlEstados = "";
            for (int y = 0; y < dsEstadosPlanning.Tables[0].Rows.Count; y++)
            {
                htmlEstados += "<div class='fichaEstado' style='background-color:" + dsEstadosPlanning.Tables[0].Rows[y]["SDPLAN_COLOR"].ToString() + ";'>"
                                + dsEstadosPlanning.Tables[0].Rows[y]["SDPLAN_ESTADONOMBRE"].ToString() + "</div>";
            }
            divEstados.InnerHtml = htmlEstados;

            //Creacion fichas de la linea de tiempo parametrizada y su color.
            string htmlLineaTiempo = "";
            for (int y = 0; y < dsTiempoPlanning.Tables[0].Rows.Count; y++)
            {
                htmlLineaTiempo += "<div class='fichaEstado' style='background-color:" + dsTiempoPlanning.Tables[0].Rows[y]["SPLAN_COLOR"].ToString() + ";'>"
                                + dsTiempoPlanning.Tables[0].Rows[y]["SPLIN_DESCRIPCION"].ToString() + "</div>";
            }

            divLineaTiempo.InnerHtml = htmlLineaTiempo;

            imgLeft.ImageUrl = urlImgLeft;
            imgBanner.ImageUrl = urlImgBanner;
            imgRight.ImageUrl = urlImgRigth;
        }

        private void CrearDescriptor(int pActual)
        {
            string htmlDescriptor = "";
            string nombreColumnaDescriptor = dsContenido.Tables[0].Columns[campoDescriptorY].ColumnName;

            htmlDescriptor += "<table style='border: 2px solid black;width:100%;background-color:#C7EFAB;'>";
            htmlDescriptor += "<tr><th>" + nombreColumnaDescriptor + "</th></tr>";

            //Obtener numero de filas reales (distinct de datos descriptor).
            maxPaginas = idsDescriptor.Count / numFilasMaxPorPag;// páginas necesarias para cargar todos los registros
            int residuo = idsDescriptor.Count % numFilasMaxPorPag; // si esto es mayor a 0 quiere decir que hay que agregar una paginación más
            if (residuo > 0) maxPaginas++;
            int registro = (pActual - 1) * numFilasMaxPorPag;//POSICIÓN ACTUAL DE LSO REGISTROS

            for (int i = 0; i < numFilasMaxPorPag; i++)
            {
                string registroDescriptorY = idsDescriptor[registro].ToString(); //dsContenido.Tables[0].Rows[registro++][descriptorY].ToString();
                string contenidoDescriptorX = cntDescriptor[registro].ToString();
                registro++;
                htmlDescriptor += "<tr><td><div class='ficha fichaDescriptor'><b style='font-size: 22px;'>" + registroDescriptorY + "</b><br>" +
                                   "<div style='font-size: 14px;'>" + contenidoDescriptorX + "</div>"
                                    + "</div></td></tr>";

                CrearFila(registroDescriptorY);

                if (registro >= idsDescriptor.Count) break;
            }

            //Header_Contenido Horas.
            string htmlContenidoAUX = "<table style='border: 2px solid black;width:100%;background-color:#CBD8D8;'><tr>";
            int horaActual = DateTime.Now.Hour;
            string diaAdd = "";
            int contD = 1;
            for (int y = 0; y < numColumMaxPorPag; y++)
            {
                if (horaActual >= 19)
                {
                    horaActual = 7;
                    diaAdd = "+" + contD + " Día: ";
                    contD++;
                }

                htmlContenidoAUX += "<th>" + diaAdd + horaActual + "-" + (horaActual + 1) + "</th>";
                horaActual++;
            }
            htmlContenidoAUX += "</tr>";

            htmlContenido = htmlContenidoAUX + htmlContenido;
            htmlContenido += "</table>";
            divContenido.InnerHtml = htmlContenido;

            htmlDescriptor += "</table>";
            divDescriptor.InnerHtml = htmlDescriptor;

            ViewState["paginaActual"] = paginaActual;
            lbPagina.Text = "Pág: " + pActual + "/" + maxPaginas;
            ViewState["maxPaginas"] = maxPaginas;
        }

        private void CrearFila(string registroDescriptorY)
        {
            DataRow[] registrosPorId = dsContenido.Tables[0].Select(campoDescriptorY + " = '" + registroDescriptorY + "'");
            htmlContenido += "<tr>";
            int adicionHorasFicha = 0;

            for (int t = 0; t < registrosPorId.Length; t++)
            {
                string colorEstado = "";
                //Comparacion de parametros de estado con la tabla DPLANNINGENERAL, para definir color.
                for (int y = 0; y < dsEstadosPlanning.Tables[0].Rows.Count; y++)
                {
                    if (dsEstadosPlanning.Tables[0].Rows[y]["SDPLAN_ESTADOID"].ToString() == registrosPorId[t][campoDescriptorX].ToString())
                    {
                        colorEstado = dsEstadosPlanning.Tables[0].Rows[y]["SDPLAN_COLOR"].ToString();
                        break;
                    }
                }

                string colorTiempo = "";
                //Comparacion de parametros de tiempo con la tabla PLINEATIEMPO, para definir color de linea de tiempo.
                DateTime fechaEntrega = Convert.ToDateTime(registrosPorId[t][campoLineaTiempo].ToString());
                double diaDiferencia = (fechaEntrega - DateTime.Now).TotalDays;

                for (int y = 0; y < dsTiempoPlanning.Tables[0].Rows.Count; y++)
                {
                    int diasReferencia = Convert.ToInt32(dsTiempoPlanning.Tables[0].Rows[y]["SPLIN_VALOR"].ToString());
                    if (diaDiferencia >= diasReferencia)
                    {
                        colorTiempo = dsTiempoPlanning.Tables[0].Rows[y]["SPLAN_COLOR"].ToString();
                    }
                    else
                    {
                        break;
                    }
                }

                int repetirHoras = Convert.ToInt32(registrosPorId[t][campoHorasTarea].ToString());
                for (int r = 0; r < repetirHoras; r++)
                {
                    htmlContenido +=
                    "<td><div class='ficha' style='background-color:" + colorTiempo + "'><div class='fichaHeader' style='background-color:" + colorEstado + ";'>"
                    + registrosPorId[t][contDescriptorX] + "</div>" +
                    "<div class='fichaBody' style='font-size: 14px;'>" + registrosPorId[t][campoFichaContenido] + "</div></div></td>";

                    if (r >= 1)
                        adicionHorasFicha++;
                }
            }
            htmlContenido += "</tr>";

            if (numColumMaxPorPag == 0)
            {
                numColumMaxPorPag = registrosPorId.Length + adicionHorasFicha;
            }
            else
            {
                if (numColumMaxPorPag < (registrosPorId.Length + adicionHorasFicha))
                    numColumMaxPorPag = (registrosPorId.Length + adicionHorasFicha);
            }
        }

        protected void CambiarPagina(object sender, EventArgs z)
        {
            paginaActual = Convert.ToInt32(ViewState["paginaActual"].ToString());
            paginaActual++;

            if (paginaActual > maxPaginas)
                paginaActual = 1;

            CargarDatos();
            CrearDescriptor(paginaActual);
        }

        protected void RetrocederPagina(object sender, EventArgs z)
        {
            paginaActual = Convert.ToInt32(ViewState["paginaActual"].ToString());
            paginaActual--;

            if (paginaActual == 0)
                paginaActual = maxPaginas;

            CargarDatos();
            CrearDescriptor(paginaActual);
        }
    }
}