
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.DBManager;
using AMS.Tools;
	
namespace AMS.Automotriz
{ 
	/// <summary>
	///		Descripción breve de AMS_Automotriz_PlanificarTaller.
	/// </summary>
	public partial class AMS_Automotriz_PlanificacionTallerVeh : System.Web.UI.Page
	{
        private DataTable dtInserItem = new DataTable();
        private DataTable dtInserts = new DataTable();
        private int horaAct;
        protected DataSet lineas;
        private int pagina;
        protected DataTable resultado;
        private string taller;
        private int ultimaHora;
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];
        private DataSet dsMecanicos = new DataSet();
        private DataSet dsVehiculosPlacas = new DataSet();

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!IsPostBack)
            {
                ViewState["TEMPSEGS"] = ConfigurationManager.AppSettings["TempPlaningSegs"];
                if (Request.QueryString["rdrt"] != null && Request.QueryString["rdrt"].ToString().Equals("1")) Session["REDIRCUADRO"] = false;
                else if (Request.QueryString["rdrt"] != null && Request.QueryString["rdrt"].ToString().Equals("0")) Session.Contents.Remove("REDIRCUADRO");
            }

            taller = base.Request.QueryString["tal"];
            pagina = Convert.ToInt32(base.Request["pag"]);
            horaAct = DateTime.Now.Hour;
            LabelT.Text = DBFunctions.SingleData("SELECT palm_descripcion FROM DBXSCHEMA.PALMACEN where tvig_vigencia='V' and Palm_almacen='" + this.taller + "'");

            //if (Session["REDIRCUADRO"] != null && Convert.ToBoolean(Session["REDIRCUADRO"]) && Session["TALLERCUADRO"] != null)
            //    Response.Redirect("" + indexPage + "?process=Reportes.FormatoReporte&idReporte=7650&tim=1&tall=" + taller);

            GenerarTablaPlanning();
            GenerarTabla2(sender, e);

		}

        public void GenerarTablaPlanning()
        {
            DBFunctions.Request(dsMecanicos, IncludeSchema.NO,
                "SELECT pvend.PVEN_CODIGO, pvend.PVEN_NOMBRE FROM DBXSCHEMA.PVENDEDOR pvend, DBXSCHEMA.PALMACEN palm " +
                "WHERE palm.PALM_ALMACEN='" + this.taller + "'  and pvend.pven_vigencia='V' order by pvend.PVEN_CODIGO");

            DBFunctions.Request(dsVehiculosPlacas, IncludeSchema.NO,
                "SELECT DISTINCT MCV.MCAT_PLACA " +
                "FROM DBXSCHEMA.DORDENOPERACION porden, DBXSCHEMA.PALMACEN alm, DBXSCHEMA.MORDEN mord,  " +
                "DBXSCHEMA.MCATALOGOVEHICULO MCV, DBXSCHEMA.ptempario pt,DBXSCHEMA.PVENDEDOR pven, DBXSCHEMA.tcargoorden tcar  " +
                "WHERE alm.PALM_ALMACEN='" + this.taller + "' AND mord.PDOC_CODIGO=porden.PDOC_CODIGO AND mord.palm_almacen=alm.palm_almacen  " +
                "AND mord.TEST_ESTADO='A' AND porden.MORD_NUMEORDE=mord.MORD_NUMEORDE AND MCV.MCAT_VIN=MORD.MCAT_VIN  " +
                "AND pt.PTEM_OPERACION=porden.PTEM_OPERACION AND pven.PVEN_CODIGO=mord.PVEN_CODIGO AND tcar.TCAR_CARGO=porden.TCAR_CARGO and mord.tcar_cargo not in 'A';");

            if (dsVehiculosPlacas.Tables[0].Rows.Count != 0)
            {
                pagina = validarPaginaActual(dsVehiculosPlacas.Tables[0].Rows.Count);
                contenido.InnerHtml = CrearTablaOrdenes(pagina);
            }
            else
            {
                Utils.MostrarAlerta(Response, "No hay vehiculos disponibles.");
                if (Session["REDIRCUADRO"] != null)
                    base.Response.Redirect(string.Concat(new object[] { "AMS.Automotriz.CitasTaller.aspx?serv=1&a=1&rdr=1&retorno=1" }));
            }
        }

        public int validarPaginaActual(int tamRegistrosMec)
        {
            int regXpag = 4;   //Registros por pagina
            int paginasTotales = 0;
            double paginasAproxDecimal = Convert.ToDouble(tamRegistrosMec) / Convert.ToDouble(regXpag);
            int paginasAproxEntero = tamRegistrosMec / regXpag;
            if (paginasAproxDecimal > paginasAproxEntero)
                paginasTotales = paginasAproxEntero + 1;
            else
                paginasTotales = paginasAproxEntero;

            paginasPie.InnerHtml = CrearPaginador(paginasTotales);

            if (pagina > paginasTotales)
            {
                if (Session["REDIRCUADRO"] == null)
                    base.Response.Redirect(string.Concat(new object[] { "AMS.Automotriz.PlanificacionTallerVeh.aspx?tal=", base.Request.QueryString["tal"], "&pag=1" }));
                else
                    base.Response.Redirect(string.Concat(new object[] { "AMS.Automotriz.CitasTaller.aspx?serv=1&a=1&rdr=1&retorno=1" }));
                return 1;
            }
            else
                return pagina;
        }

        public string CrearPaginador(int paginasTotales)
        {
            string paginadorHTML = "";
            int separador = 20;
            string urlRedi = "";

            for (int i = 1; i <= paginasTotales; i++)
            {
                //urlRedi = "AMS.Automotriz.PlanificacionTaller.aspx?tal=" + base.Request.QueryString["tal"] + "&pag=" + i;

                paginadorHTML += "<div style='left: " + separador + "px;";

                if (pagina == i)
                    paginadorHTML += "border-style: solid;' class='paginador' ";
                else
                    paginadorHTML += "' class='paginador' ";

                paginadorHTML += "onClick='espera();window.location = cambioURL(" + pagina + "," + i + ");'>";
                paginadorHTML += i + "</div>";
                separador += 25;
            }

            return paginadorHTML;
        }

        public string CrearTablaOrdenes(int pagActual)
        {
            DataSet vendedorM = new DataSet();
            DataSet dsOrdenes = new DataSet();
            
            DBFunctions.Request(vendedorM, IncludeSchema.NO, "SELECT hm.CTAL_HIMEC AS LLEGADA,hm.CTAL_HFMEC AS SALIDA,hour(hm.CTAL_HFMEC-hm.CTAL_HIMEC)FROM DBXSCHEMA.CTALLER hm;");
            DBFunctions.Request(dsOrdenes, IncludeSchema.NO, "SELECT porden.PTEM_OPERACION, porden.TEST_ESTADO, porden.PDOC_CODIGO, porden.MORD_NUMEORDE, mord.mord_entregar, mord.mord_horaentg, mord.MORD_NUMEENTR NUMERO, porden.PVEN_CODIGO, cast(month (mord.mord_entregar) as char(2)) concat '/' concat cast(day(mord.mord_entregar) as char(2)) concat ' ' concat replace(cast(mord.mord_horaentg as char(5)),'.',':') ENTREGA, coalesce(porden.dord_tiemliqu,1) HORAS_PROC, mord.mord_entregar FECHA_ENTREGA, mord.pven_codigo RECEP, mord.mcat_vin VIN, (select test.test_nombre from testadooperacion test where test.test_estaoper = porden.TEST_ESTADO) TNOMBRE, MCV.MCAT_PLACA, MCV.PCAT_CODIGO, pt.ptem_descripcion, pven.PVEN_NOMBRE, tcar.TCAR_NOMBRE, mord.MORD_ENTRADA  FROM DBXSCHEMA.DORDENOPERACION porden, DBXSCHEMA.PALMACEN alm, DBXSCHEMA.MORDEN mord, DBXSCHEMA.MCATALOGOVEHICULO MCV, DBXSCHEMA.ptempario pt,DBXSCHEMA.PVENDEDOR pven, DBXSCHEMA.tcargoorden tcar WHERE alm.PALM_ALMACEN='" + this.taller + "' AND mord.PDOC_CODIGO=porden.PDOC_CODIGO AND mord.palm_almacen=alm.palm_almacen AND mord.TEST_ESTADO='A' AND porden.MORD_NUMEORDE=mord.MORD_NUMEORDE AND MCV.MCAT_VIN=MORD.MCAT_VIN AND pt.PTEM_OPERACION=porden.PTEM_OPERACION AND pven.PVEN_CODIGO=mord.PVEN_CODIGO AND tcar.TCAR_CARGO=porden.TCAR_CARGO order by porden.PVEN_CODIGO, mord.MORD_ENTREGAR, mord.mord_horaentg;");
            int filasXpag = 4;   //Número de filas.
            int registros = pagActual * filasXpag;
            int tamRegistroMec = dsVehiculosPlacas.Tables[0].Rows.Count; //Tamaño por tabla de vehiculos.
            int columnas = 0;
            int procedimientoAct = 0;
            int columnaAnteior = 1;
            int horaInicio = Convert.ToInt32(vendedorM.Tables[0].Rows[0][0].ToString().Split(new char[] { ':' })[0]);
            ultimaHora = Convert.ToInt32(vendedorM.Tables[0].Rows[0][2]);
            ultimaHora += horaInicio;
            string htmlTablaOrden = "";
            string nombreCol = "";
            string placa = "";
            string strOperacion = "";
            string strEntrada = "";
            string color = "";
            string colorEstado = "";
            double horasProceso = 0.0;
            double horasUsadasProcesos = 0.0;
            bool terminaMecanico = false;
            DateTime fechaHoy = DateTime.Now;

            //Construcción encabezados tabla Ordenes. Para Hoy.
            htmlTablaOrden += "<table class='cont'><tr>";
            for (int i = horaAct; i <= ultimaHora; i++)
            {
                int y = i + 1;
                nombreCol = i.ToString() + "-" + y;
                htmlTablaOrden += "<th>" + nombreCol + "</th>";
                columnas++;
            }
            //Construcción encabezados tabla Ordenes. Para Mañana.
            for (int i = horaInicio; i <= ultimaHora; i++)
            {
                int y = i + 1;
                nombreCol = "Mañ. " + i.ToString() + "-" + y;
                htmlTablaOrden += "<th>" + nombreCol + "</th>";
                columnas++;
            }

            htmlTablaOrden += "</tr>";
            mecanicos.InnerHtml = "<table class='meca'><tr><th>Vehículo</th></tr>";

            //Construcción contenido tabla Ordenes.
            for (int i = (registros - filasXpag); (i < registros && i < tamRegistroMec); i++)
            {
                DataRow drMec = dsVehiculosPlacas.Tables[0].Rows[i];
                DataRow[] drOrdenesMec = dsOrdenes.Tables[0].Select("MCAT_PLACA='" + drMec["MCAT_PLACA"].ToString() + "'");
                //DataRow[] tecnico = dsMecanicos.Tables[0].Select("PVEN_CODIGO='" + drMec["PVEN_CODIGO"].ToString() + "'");
                int columnaActual = 1;
                Boolean fichas = false;
                procedimientoAct = 0;
                DateTime dAct = DateTime.Now;
                int anio = ((DateTime)drOrdenesMec[procedimientoAct]["MORD_ENTREGAR"]).Year;
                int mes = ((DateTime)drOrdenesMec[procedimientoAct]["MORD_ENTREGAR"]).Month;
                int dia = ((DateTime)drOrdenesMec[procedimientoAct]["MORD_ENTREGAR"]).Day;
                String[] hor = drOrdenesMec[procedimientoAct]["MORD_HORAENTG"].ToString().Split(':');
                DateTime dFin = new DateTime(anio, mes, dia, Int32.Parse(hor[0]), Int32.Parse(hor[1]), 0);
                TimeSpan interval = dFin - dAct;
                String restantes2 = interval.Days + "d. " + interval.Hours + ":" + interval.Minutes;

                mecanicos.InnerHtml += CrearTablaMecanicos(pagina, drOrdenesMec[0], restantes2, drOrdenesMec[procedimientoAct]["PVEN_NOMBRE"].ToString());
                
                //String codTecnico;
                //if (tecnico.Length == 0)
                //    codTecnico = "No registrado";
                //else
                //    codTecnico = tecnico[0]["PVEN_CODIGO"] + "-" + tecnico[0]["PVEN_NOMBRE"];

                htmlTablaOrden += "<tr>";
                while (drOrdenesMec.Length > 0 && procedimientoAct < drOrdenesMec.Length
                        && columnaActual <= columnas && horaAct <= ultimaHora
                        && !terminaMecanico)
                {
                    placa = DBFunctions.SingleData("SELECT MCAT_PLACA FROM MCATALOGOVEHICULO WHERE MCAT_VIN='" + drOrdenesMec[procedimientoAct]["VIN"] + "';");
                    strEntrada = drOrdenesMec[procedimientoAct]["PTEM_DESCRIPCION"].ToString();
                    
                    horasProceso = Convert.ToDouble(drOrdenesMec[procedimientoAct]["HORAS_PROC"]);
                    DateTime fechaProc = Convert.ToDateTime(drOrdenesMec[procedimientoAct]["FECHA_ENTREGA"]);

                    //Colores de fondo para estado de tiempo en ordenes.
                    if (fechaProc < fechaHoy.AddDays(-30))
                        color = "rojo";
                    else if (fechaProc < fechaHoy)
                        color = "naranja";
                    else if (fechaProc == fechaHoy)
                        color = "amarillo";
                    else
                        color = "verde";

                    //Colores de fondo para estado de operación en ordenes.
                    if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("R"))
                        colorEstado = "morado";
                    else if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("T"))
                        colorEstado = "azul";
                    else if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("M"))
                        colorEstado = "cafe";
                    else if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("I"))
                        colorEstado = "verdeClaro";
                    else if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("C"))
                        colorEstado = "verdemuyClaro";

                    //Contenido de ficha de operación.
                    strOperacion = "[" + drOrdenesMec[procedimientoAct]["PTEM_OPERACION"].ToString() + "]" + drOrdenesMec[procedimientoAct]["HORAS_PROC"].ToString() + "&nbsp;" + drOrdenesMec[procedimientoAct]["TNOMBRE"].ToString() + "&nbsp;" + drOrdenesMec[procedimientoAct]["TCAR_NOMBRE"].ToString() + "&nbsp;";
                    
                    //if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("C"))
                    //{
                    //    String sql2 = "select pdoc_codigo concat '-' concat cast(mfac_numedocu AS CHAR(9)) from mfacturaclientetaller where PDOC_PREFORDETRAB='" + drOrdenesMec[0][2] + "' " +
                    //            "AND mord_numeorde=" + drOrdenesMec[0][3] + " AND TCAR_CARGO=(select tcar_cargo from tcargoorden " +
                    //            "where tcar_nombre = '" + drOrdenesMec[0][18] + "');";
                    //    String mfac = DBFunctions.SingleData(sql2);

                    //    if (!mfac.Equals(""))
                    //    {
                    //        strOperacion = drOrdenesMec[procedimientoAct]["HORAS_PROC"].ToString() + "&nbsp;&nbsp;" + drOrdenesMec[procedimientoAct]["TNOMBRE"].ToString() + "<BR>" + codTecnico + "<BR><span style=\"background-color:#70DD20\">" + drOrdenesMec[procedimientoAct]["TCAR_NOMBRE"].ToString() + "</span> " + mfac;
                    //    }
                    //}

                    columnaActual = ((int)Math.Floor(horasUsadasProcesos)) + 1;
                    htmlTablaOrden += AgregarOperacion(strOperacion, strEntrada, color, colorEstado);
                    fichas = true;

                    int ultimaC = 0;
                    //for (int n = columnaActual; n <= Math.Ceiling((double)(horasProceso + horasUsadasProcesos)); n++)
                    //{
                    //    if (n > columnas)
                    //    {
                    //        terminaMecanico = true;
                    //        break;
                    //    }

                    //    htmlTablaOrden += AgregarOperacion(strOperacion, strEntrada, color, colorEstado, placa);
                    //    fichas = true;
                    //    ultimaC = n;
                    //}

                    horasUsadasProcesos += 1;  //horasProceso...
                    columnaAnteior = columnaActual;
                    procedimientoAct++;
                }

                if (fichas)
                    columnaActual++;

                //Completa fichas vacias en la tabla del planning.
                for (int k = columnaActual; k <= columnas; k++)
                {
                    htmlTablaOrden += AgregarOperacion("", "", "", "");
                }
                htmlTablaOrden += "</tr>";
                horasUsadasProcesos = 0;
            }

            mecanicos.InnerHtml += "</table>";
            htmlTablaOrden += "</table>";

            return htmlTablaOrden;
        }

        public string CrearTablaMecanicos(int pagActual, DataRow drMec, String restantes, String nombreMecanico)
        {
            int regXpag = 4;   //Registros por pagina
            int registros = pagActual * regXpag;
            string htmlTablaMec = "";
            int tamRegistros = dsMecanicos.Tables[0].Rows.Count;

            DateTime fechaEntrada =  Convert.ToDateTime(drMec["MORD_ENTRADA"]);
            htmlTablaMec += "<tr><td>";
            htmlTablaMec += "<b>" + drMec["MCAT_PLACA"].ToString() + " - " + fechaEntrada.ToString("yyyy-MM-dd") + "</b><BR>Entregar:" + drMec["ENTREGA"].ToString() + "<BR>Tmp. Rest: " + restantes + "<BR>" + nombreMecanico;
            htmlTablaMec += "</td></tr>";

            return htmlTablaMec;
        }

        public string AgregarOperacion(string strOperacion, string strEntrada, string color, string colorEstado)
        {
            string celda = "";

            if (strOperacion != "")
            {
                celda += "<td class='" + color + "'>";
                celda += "<TABLE class='ficha'><tr><th class='" + colorEstado + "'>";
                celda += strEntrada + "</th></tr>";
                celda += "<tr><td>" + strOperacion + "</td></tr></TABLE></td>";
            }
            else
            {
                celda += "<td><div class='vacio'></div></td>";
            }

            return celda;
        }

		protected void GenerarTabla2(object sender, EventArgs e)
		{
			this.PrepararTabla();
			this.lineas = new DataSet();
			DBFunctions.Request(this.lineas, IncludeSchema.NO,
                "SELECT DISTINCT MORDEN.PDOC_CODIGO concat ' ' concat rtrim(char(MORDEN.MORD_NUMEORDE)) NUMORDEN, MORDEN.MORD_ENTRADA FECHAENTRADA, " +
                "MORDEN.MORD_HORAENTR HORAENTRADA, MORDEN.MORD_HORAENTG HORAENTREGA, MORDEN.MORD_ENTREGAR FECHAENTREGA, MORDEN.MORD_NUMEENTR,  " +
                "COALESCE(MCATV.MCAT_PLACA,'') AS PLACA  " +
                "FROM DBXSCHEMA.MORDEN MORDEN left join DBXSCHEMA.DORDENOPERACION DORDEN on MORDEN.PDOC_CODIGO=DORDEN.PDOC_CODIGO  " +
                "AND MORDEN.MORD_NUMEORDE=DORDEN.MORD_NUMEORDE left outer join MCATALOGOVEHICULO MCATV ON MCATV.MCAT_VIN=MORDEN.MCAT_VIN, " +
                "DBXSCHEMA.PALMACEN alm WHERE DORDEN.TEST_ESTADO ='S' AND MORDEN.TEST_ESTADO='A' AND MORDEN.palm_almacen=alm.palm_almacen " +
                "and alm.PALM_ALMACEN='" + taller + "';");

			this.Grid.DataSource = this.lineas.Tables[0];
			this.Grid.DataBind();
		}

		public void PrepararTabla()
		{
			this.resultado = new DataTable();
			DataColumn numorden = new DataColumn();
			numorden.DataType = Type.GetType("System.String");
			numorden.ColumnName = "NUMORDEN";
			numorden.ReadOnly = true;
			this.resultado.Columns.Add(numorden);
			DataColumn fechaentrada = new DataColumn();
			fechaentrada.DataType = Type.GetType("System.String");
			fechaentrada.ColumnName = "FECHAENTRADA";
			fechaentrada.ReadOnly = true;
			this.resultado.Columns.Add(fechaentrada);
			DataColumn horaentrada = new DataColumn();
			horaentrada.DataType = Type.GetType("System.String");
			horaentrada.ColumnName = "HORAENTRADA";
			horaentrada.ReadOnly = true;
			this.resultado.Columns.Add(horaentrada);
			DataColumn horaentrega = new DataColumn();
			horaentrega.DataType = Type.GetType("System.String");
			horaentrega.ColumnName = "HORAENTREGA";
			horaentrega.ReadOnly = true;
			this.resultado.Columns.Add(horaentrega);
			DataColumn fechaentrega = new DataColumn();
			fechaentrega.DataType = Type.GetType("System.String");
			fechaentrega.ColumnName = "FECHAENTREGA";
			fechaentrega.ReadOnly = true;
			this.resultado.Columns.Add(fechaentrega);
		}


		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			base.OnInit(e);
		}
		
		#endregion

		

		
	}
}