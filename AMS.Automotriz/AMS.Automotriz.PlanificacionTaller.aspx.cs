
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	
namespace AMS.Automotriz
{ 
	/// <summary>
	///		Descripción breve de AMS_Automotriz_PlanificarTaller.
	/// </summary>
	public partial class AMS_Automotriz_PlanificacionTaller : System.Web.UI.Page
	{
		private DataTable dtInserItem = new DataTable();
		private DataTable dtInserts = new DataTable();
		private int horaAct;
		protected DataSet lineas;
		private int pagina;
		protected DataTable resultado;
        private string taller, tallerPrim;
		private int ultimaHora;
		protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];
        private DataSet dsMecanicos = new DataSet();
        string tipoPlanning = "";


        protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				ViewState["TEMPSEGS"] = ConfigurationManager.AppSettings["TempPlaningSegs"];
                string tipoPlanning = Request.QueryString["planning"];
                if(Session["planning"] == null)
                    Session["planning"] = tipoPlanning;
                if (Request.QueryString["rdrt"]!=null && Request.QueryString["rdrt"].ToString().Equals("1")) Session["REDIRCUADRO"] = false;
				else if (Request.QueryString["rdrt"] != null && Request.QueryString["rdrt"].ToString().Equals("0")) Session.Contents.Remove("REDIRCUADRO");

                
            }

			taller = tallerPrim = base.Request.QueryString["tal"];
            DataSet dsTalleresPlanning = new DataSet();
            DBFunctions.Request(dsTalleresPlanning, IncludeSchema.NO, "select pt.PALM_ALMA2, pa.palm_descripcion from PTALLERESPLANNING pt, PALMACEN pa where pt.PALM_ALMA1 = '" + this.taller + "' and pt.PALM_ALMA2 = pa.PALM_ALMACEN and pa.tvig_vigencia='V';");
            try
            {
                if (dsTalleresPlanning.Tables[0].Rows.Count > 1)
                {
                    taller = "";
                    for (int k = 0; k < dsTalleresPlanning.Tables[0].Rows.Count; k++)
                    {
                        taller += dsTalleresPlanning.Tables[0].Rows[k][0] + "' OR palm.palm_almacen='";
                        LabelT.Text += dsTalleresPlanning.Tables[0].Rows[k][1] + "-";
                    }

                    taller = taller.Substring(0, (taller.Length - 24));
                    LabelT.Text = LabelT.Text.Substring(0, (LabelT.Text.Length - 1));
                }
                else
                {
                    LabelT.Text = DBFunctions.SingleData("SELECT palm.palm_descripcion FROM DBXSCHEMA.PALMACEN palm where palm.tvig_vigencia='V' and palm.Palm_almacen='" + this.taller + "'");
                }
            }
            catch (Exception err)
            {
                LabelT.Text = "Parametrizar tabla de plannings";
            }
            
            pagina = Convert.ToInt32(base.Request["pag"]);
            horaAct = DateTime.Now.Hour;
            
            GenerarTablaPlanning();
            GenerarTabla2(sender, e);

            //iconos empresas " + GlobalData.getEMPRESA() + "
            try
            {
                string urlIconoEmpresa = DBFunctions.SingleDataGlobal("SELECT GEMP_iCONO FROM GEMPRESA WHERE GEMP_NOMBRE='" + GlobalData.getEMPRESA() + "';");
                Image1.ImageUrl = urlIconoEmpresa;
            }
            catch { }
        }

        public void GenerarTablaPlanning()
        {
            //DBFunctions.Request(dsMecanicos, IncludeSchema.NO,
            //    "SELECT distinct pvend.PVEN_CODIGO, pvend.PVEN_NOMBRE FROM DBXSCHEMA.PVENDEDOR pvend, DBXSCHEMA.PvendedorALMACEN palm " +
            //    "WHERE PVEND.PVEN_CODIGO = PAlm.PVEN_CODIGO and palm.PALM_ALMACEN='" + taller + "' AND " +
            //    "TVEND_CODIGO='MG' and pvend.pven_vigencia='V' order by pvend.PVEN_nombre");
            //try
            //{ 
           
                tipoPlanning = Session["planning"].ToString();                
                string strTipoPlanning = "";
                if (tipoPlanning != null)
                {
                
                    if (tipoPlanning == "LATPIN")
                        strTipoPlanning = " AND (pt.tope_codigo = 'PIN' OR pt.tope_codigo = 'LAT') ";
                    else
                        strTipoPlanning = " AND (pt.tope_codigo != 'PIN' AND pt.tope_codigo != 'LAT') ";
                }
                else
                    strTipoPlanning = "";
           
                DBFunctions.Request(dsMecanicos, IncludeSchema.NO,
                @"SELECT distinct porden.PVEN_CODIGO, pvend.PVEN_NOMBRE
                    FROM DBXSCHEMA.DORDENOPERACION porden, DBXSCHEMA.PALMACEN palm, DBXSCHEMA.MORDEN mord, ptempario pt, DBXSCHEMA.PVENDEDOR pvend
                   WHERE palm.tvig_vigencia = 'V' and (palm.PALM_ALMACEN = '" + taller + @"')
                     AND mord.PDOC_CODIGO = porden.PDOC_CODIGO AND mord.palm_almacen = palm.palm_almacen AND mord.TEST_ESTADO = 'A' AND porden.MORD_NUMEORDE = mord.MORD_NUMEORDE AND porden.TEST_ESTADO
                     not in ('C','S','X','G') and pt.ptem_operacion = porden.PTEM_OPERACION 
                     " + strTipoPlanning + @" and porden.pven_codigo = pvend.pven_codigo
                   order by porden.PVEN_CODIGO;");

                pagina = validarPaginaActual(dsMecanicos.Tables[0].Rows.Count);
                mecanicos.InnerHtml = CrearTablaMecanicos(pagina);
                contenido.InnerHtml = CrearTablaOrdenes(pagina);
            //}
            //catch { };
        }

        public string CrearTablaMecanicos(int pagActual)
        {
            int regXpag = 4;   //Registros por pagina
            int registros = pagActual * regXpag;
            string htmlTablaMec = "";
            int tamRegistros = dsMecanicos.Tables[0].Rows.Count;

            htmlTablaMec += "<table class='meca'><tr><th>Técnico</th></tr>";
            for(int i = (registros-regXpag); (i < registros && i < tamRegistros); i++)
            {
                htmlTablaMec += "<tr><td><b><font size='5'>";
                htmlTablaMec += dsMecanicos.Tables[0].Rows[i][0];
                htmlTablaMec += "</font></b><BR>";
                htmlTablaMec += dsMecanicos.Tables[0].Rows[i][1];
                htmlTablaMec += "</td></tr>";
            }
            htmlTablaMec += "</table>";

            return htmlTablaMec;
        }

        public string CrearTablaOrdenes(int pagActual)
        {
            DataSet vendedorM = new DataSet();
            DataSet dsOrdenes = new DataSet();
            DBFunctions.Request(vendedorM, IncludeSchema.NO, "SELECT hm.CTAL_HIMEC AS LLEGADA,hm.CTAL_HFMEC - 1 hour AS SALIDA,hour(hm.CTAL_HFMEC-hm.CTAL_HIMEC) FROM DBXSCHEMA.CTALLER hm;");

            string tipoPlanning = Session["planning"].ToString();
            string srtTipoPlanning = "";
            if (tipoPlanning != null)
            {
                if (tipoPlanning == "LATPIN")
                    srtTipoPlanning = " AND (pt.tope_codigo = 'PIN' OR pt.tope_codigo = 'LAT') ";
                else
                    srtTipoPlanning = " AND (pt.tope_codigo != 'PIN' AND pt.tope_codigo != 'LAT') ";
            }
            else
                srtTipoPlanning = "";

            DBFunctions.Request(dsOrdenes, IncludeSchema.NO, "SELECT porden.PTEM_OPERACION, porden.TEST_ESTADO, porden.PDOC_CODIGO, porden.MORD_NUMEORDE, mord.mord_entregar, mord.mord_horaentg, mord.MORD_NUMEENTR NUMERO, porden.PVEN_CODIGO, cast(month (mord.mord_entregar) as char(2)) concat '/' concat cast(day(mord.mord_entregar) as char(2)) concat ' ' concat replace(cast(mord.mord_horaentg as char(5)),'.',':') ENTREGA, coalesce(porden.dord_tiemliqu,1) HORAS_PROC, mord.mord_entregar FECHA_ENTREGA, mord.pven_codigo RECEP, mord.mcat_vin VIN, (select test.test_nombre from testadooperacion test where test.test_estaoper = porden.TEST_ESTADO) TNOMBRE, MORD.TTRA_CODIGO, coalesce(mcv.mcat_placa,'* ERROR *') AS MCAT_PLACA FROM DBXSCHEMA.DORDENOPERACION porden, DBXSCHEMA.PALMACEN palm, ptempario pt, DBXSCHEMA.MORDEN mord left join MCATALOGOVEHICULO mcv on mord.MCAT_VIN = mcv.mcat_vin WHERE palm.tvig_vigencia='V' and (palm.PALM_ALMACEN='" + this.taller + "') AND mord.PDOC_CODIGO=porden.PDOC_CODIGO AND mord.palm_almacen=palm.palm_almacen AND mord.TEST_ESTADO='A' AND porden.MORD_NUMEORDE=mord.MORD_NUMEORDE AND porden.TEST_ESTADO not in ('C','S','X','G') and pt.ptem_operacion = porden.PTEM_OPERACION " + srtTipoPlanning + " order by porden.PVEN_CODIGO, mord.MORD_ENTREGAR, mord.mord_horaentg");
            int filasXpag = 4;   //Número de filas.
            int registros = pagActual * filasXpag;
            int tamRegistroMec = dsMecanicos.Tables[0].Rows.Count;
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

            //Construcción contenido tabla Ordenes.
            for(int i = (registros-filasXpag); (i < registros && i < tamRegistroMec); i++)
            {
                DataRow drMec =  dsMecanicos.Tables[0].Rows[i];
                DataRow[] drOrdenesMec = dsOrdenes.Tables[0].Select("PVEN_CODIGO='" + drMec["PVEN_CODIGO"].ToString() + "'");
                int columnaActual = 1;
                Boolean fichas = false;
                terminaMecanico = false;
                procedimientoAct = 0;

                htmlTablaOrden += "<tr>";
                while (drOrdenesMec.Length > 0 && procedimientoAct < drOrdenesMec.Length
                        && columnaActual <= columnas &&  horaAct <= ultimaHora 
                        && !terminaMecanico)
                {
                  //  placa = DBFunctions.SingleData("SELECT MCAT_PLACA FROM MCATALOGOVEHICULO WHERE MCAT_VIN='" + drOrdenesMec[procedimientoAct]["VIN"] + "';");
                    placa = drOrdenesMec[procedimientoAct]["MCAT_PLACA"].ToString().Trim();
                    strEntrada = drOrdenesMec[procedimientoAct]["NUMERO"].ToString().Trim();

                    //bool servicio = dsOrdenes.Tables[0].Rows[i]["TTRA_CODIGO"].Equals("S")? true : false;
                    bool servicio = drOrdenesMec[procedimientoAct]["TTRA_CODIGO"].Equals("S") ? true : false;

                    strOperacion = "[" + drOrdenesMec[procedimientoAct]["PTEM_OPERACION"].ToString() + "] " + drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString() + "<BR>" + 
                                    "(" + drOrdenesMec[procedimientoAct]["PDOC_CODIGO"].ToString() + "-" + drOrdenesMec[procedimientoAct]["MORD_NUMEORDE"].ToString() + ")<BR>" + 
                                    drOrdenesMec[procedimientoAct]["HORAS_PROC"].ToString() + "  " + 
                                    drOrdenesMec[procedimientoAct]["TNOMBRE"].ToString() + " " + 
                                    drOrdenesMec[procedimientoAct]["ENTREGA"].ToString() + " Rcp:" + drOrdenesMec[procedimientoAct]["RECEP"].ToString();
                    

                    if (servicio)
                    {
                        strOperacion += "&nbsp;&nbsp;<img src = '../img/espera2.png' title = 'Serv. Inmediato' />";
                    }

                    if (strEntrada.Length == 0)
                    {
                        strEntrada = "?";
                    }

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
                    if(drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("A"))
                        colorEstado = "blanco";
                    else if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("R"))
                        colorEstado = "morado";
                    else if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("T"))
                        colorEstado = "azul";
                    else if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("M"))
                        colorEstado = "cafe";
                    else if (drOrdenesMec[procedimientoAct]["TEST_ESTADO"].ToString().Equals("I"))
                        colorEstado = "verdeClaro";
                    else 
                        colorEstado = "metal";

                    columnaActual = ((int)Math.Floor(horasUsadasProcesos)) + 1;
                    
                    int ultimaC = 0;
                    for (int n = columnaActual; n <= Math.Ceiling((double)(horasProceso + horasUsadasProcesos)); n++)
                    {
                        if (n > columnas)
                        {
                            terminaMecanico = true;
                            break;
                        }

                        htmlTablaOrden += AgregarOperacion(strOperacion, strEntrada, color, colorEstado, placa);
                        fichas = true;
                        ultimaC = n;
                       
                    }
                    
                    horasUsadasProcesos += horasProceso;
                    columnaAnteior = columnaActual;
                    procedimientoAct++;
                }

                if (fichas)
                    columnaActual++;

                //Completa fichas vacias en la tabla del planning.
                for (int k = columnaActual; k <= columnas; k++ )
                {
                    htmlTablaOrden += AgregarOperacion("", "", "", "", "");
                }
                htmlTablaOrden += "</tr>";
                horasUsadasProcesos = 0;
            }

            htmlTablaOrden += "</table>";

            return htmlTablaOrden;
        }

        public string AgregarOperacion(string strOperacion, string strEntrada, string color, string colorEstado, string placa)
        {
            string celda = "";

            if (strOperacion != "")
            {
                celda += "<td class='" + color + "'>";
                celda += "<TABLE class='ficha'><tr><th class='" + colorEstado  + "'>";
                celda += strEntrada + " - " + placa + "</th></tr>";
                celda += "<tr><td>" + strOperacion + "</td></tr></TABLE></td>";
            }
            else
            {
                celda += "<td><div class='vacio'></div></td>";
            }

            return celda;
        }

        public int validarPaginaActual(int tamRegistrosMec)
        {
            int regXpag = 4;   //Registros por pagina
            int paginasTotales = 0;
            double paginasAproxDecimal = Convert.ToDouble(tamRegistrosMec)/Convert.ToDouble(regXpag);
            int paginasAproxEntero = tamRegistrosMec / regXpag;
            if (paginasAproxDecimal > paginasAproxEntero)
                paginasTotales = paginasAproxEntero + 1;
            else
                paginasTotales = paginasAproxEntero;

            paginasPie.InnerHtml = CrearPaginador(paginasTotales);


             if (pagina > paginasTotales)
            {
                if (Session["REDIRCUADRO"] == null)
                {
                    string tipoPlanning = Session["planning"].ToString();
                    if (tipoPlanning == null)
                        base.Response.Redirect(string.Concat(new object[] { "AMS.Automotriz.PlanificacionTaller.aspx?tal=", base.Request.QueryString["tal"], "&pag=1&rdrt=0" }));
                    else
                        base.Response.Redirect(string.Concat(new object[] { "AMS.Automotriz.PlanificacionTaller.aspx?tal=", base.Request.QueryString["tal"], "&pag=1&rdrt=0&planning=" + tipoPlanning }));
                }
                else
                    base.Response.Redirect("AMS.Reportes.FrogReports.aspx?idReporte=7650&tim=1&tall=" + tallerPrim);
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

            for (int i = 1; i <= paginasTotales; i++ )
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

		protected void GenerarTabla2(object sender, EventArgs e)
		{
			PrepararTabla();
			lineas = new DataSet();
			DBFunctions.Request(lineas, IncludeSchema.NO, 
                "SELECT DISTINCT MORDEN.PDOC_CODIGO concat ' ' concat rtrim(char(MORDEN.MORD_NUMEORDE)) NUMORDEN, MORDEN.MORD_ENTRADA FECHAENTRADA, " +
                "MORDEN.MORD_HORAENTR HORAENTRADA, MORDEN.MORD_HORAENTG HORAENTREGA, MORDEN.MORD_ENTREGAR FECHAENTREGA, MORDEN.MORD_NUMEENTR,  " +
                "COALESCE(MCATV.MCAT_PLACA,'') AS PLACA  " +
                "FROM DBXSCHEMA.MORDEN MORDEN left join DBXSCHEMA.DORDENOPERACION DORDEN on MORDEN.PDOC_CODIGO=DORDEN.PDOC_CODIGO  " +
                "AND MORDEN.MORD_NUMEORDE=DORDEN.MORD_NUMEORDE left outer join MCATALOGOVEHICULO MCATV ON MCATV.MCAT_VIN=MORDEN.MCAT_VIN, " +
                "DBXSCHEMA.PALMACEN palm WHERE DORDEN.TEST_ESTADO ='S' AND MORDEN.TEST_ESTADO='A' AND MORDEN.palm_almacen=palm.palm_almacen " +
                "and palm.PALM_ALMACEN='" + taller + "';");
			Grid.DataSource = lineas.Tables[0];
			Grid.DataBind();
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
