// created on 17/12/2004 at 10:54
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Estadisticos;

namespace AMS.Automotriz
{
	public partial class ProgramacionTaller : System.Web.UI.UserControl
	{
		#region Atributos
		protected Label      enPatio ;
		protected DataTable tablaRecepcionistas, tablaMecanicos, tablaOperacionesOrdenes;
		protected PlaceHolder graficosEstad;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected eWorld.UI.CalendarPopup CalendarPopup1;
        
        #endregion
		
		#region Eventos
        
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(taller, "SELECT palm_descripcion FROM palmacen pa where pa.pcen_centtal is not null or pcen_centcoli is not null order by pa.PALM_DESCRIPCION;");
				//bind.PutDatasIntoDropDownList(taller,"SELECT palm_almacen,palm_descripcion FROM palmacen ORDER BY palm_almacen ASC");
			}
            Cargar_Cantidades_Ordenes(taller.SelectedValue,FechaInicial.SelectedDate.ToString("yyyy-MM-dd"), FechaFinal.SelectedDate.ToString("yyyy-MM-dd"));
            Llenar_Grilla_Recepcionistas(taller.SelectedValue, FechaInicial.SelectedDate.ToString("yyyy-MM-dd"), FechaFinal.SelectedDate.ToString("yyyy-MM-dd"));
            Llenar_Grilla_Mecanicos(taller.SelectedValue, FechaInicial.SelectedDate.ToString("yyyy-MM-dd"), FechaFinal.SelectedDate.ToString("yyyy-MM-dd"));
            LLenar_Grilla_TotalOrdenes(taller.SelectedValue, FechaInicial.SelectedDate.ToString("yyyy-MM-dd"), FechaFinal.SelectedDate.ToString("yyyy-MM-dd"));
            
			/*fechaConsulta.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
			horaConsulta.Text = DateTime.Now.TimeOfDay.ToString().Substring(0,8);*/
		}
       		
		protected void Cambio_Grafico(Object  Sender, EventArgs e)
		{
			if(tipGraficos.SelectedValue == "M")
				Crear_Grafico_Mecanicos();
			else if(tipGraficos.SelectedValue == "O")
				Crear_Grafico_Operaciones();
		}
		
		protected  void  Cambio_Taller(Object  Sender, EventArgs e)
		{
            Cargar_Cantidades_Ordenes(DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE palm_descripcion='" + taller.SelectedItem.ToString().Trim() + "'"), FechaInicial.SelectedDate.ToString("yyyy-MM-dd"), FechaFinal.SelectedDate.ToString("yyyy-MM-dd"));
            Llenar_Grilla_Recepcionistas(DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE palm_descripcion='" + taller.SelectedItem.ToString().Trim() + "'"), FechaInicial.SelectedDate.ToString("yyyy-MM-dd"), FechaFinal.SelectedDate.ToString("yyyy-MM-dd"));
            Llenar_Grilla_Mecanicos(DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE palm_descripcion='" + taller.SelectedItem.ToString().Trim() + "'"), FechaInicial.SelectedDate.ToString("yyyy-MM-dd"), FechaFinal.SelectedDate.ToString("yyyy-MM-dd"));
            LLenar_Grilla_TotalOrdenes(DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE palm_descripcion='" + taller.SelectedItem.ToString().Trim() + "'"), FechaInicial.SelectedDate.ToString("yyyy-MM-dd"), FechaFinal.SelectedDate.ToString("yyyy-MM-dd"));
            
		}
		#endregion
		
		#region Metodos
		protected void Cargar_Cantidades_Ordenes(string codigoTaller,string FechaInicial, string FechaFinal)
		{
            enProceso.Text = DBFunctions.SingleData("SELECT COALESCE(COUNT(MOR1.mord_numeorde),0) FROM pvendedor PVE LEFT JOIN morden MOR1 ON PVE.pven_codigo = MOR1.pven_codigo WHERE PVE.palm_almacen = '" + codigoTaller + "' AND PVE.tvend_codigo IN ('RT','RA','RC') AND MOR1.test_estado IN ('A') AND MOR1.MORD_ENTRADA BETWEEN '" +FechaInicial+ "' AND '"+FechaFinal+ "' and PVEN_VIGENCIA = 'V' ");
            preliquidadas.Text = DBFunctions.SingleData("SELECT COUNT(MOR1.mord_numeorde) FROM pvendedor PVE LEFT JOIN morden MOR1 ON PVE.pven_codigo = MOR1.pven_codigo WHERE PVE.palm_almacen = '" + codigoTaller + "' AND PVE.tvend_codigo IN ('RT','RA','RC') AND MOR1.mord_estaliqu IN ('P') AND MOR1.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' AND PVEN_VIGENCIA = 'V' ");
            facturadasSinSalida.Text = DBFunctions.SingleData("SELECT COUNT(MOR1.mord_numeorde) as OTPC FROM pvendedor PVE LEFT JOIN morden MOR1 ON PVE.pven_codigo = MOR1.pven_codigo WHERE PVE.palm_almacen = '" + codigoTaller + "' AND PVE.tvend_codigo IN ('RT','RA','RC') AND MOR1.test_estado IN ('F') AND MOR1.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' AND PVEN_VIGENCIA = 'V' ");
		}
		
		protected double Calcular_Ordenes_Facturadas_Sin_Salir_Recepcionista(string codigoRecepcionista)
		{
			double cantidadOrdenes = 0;
            try { cantidadOrdenes = Convert.ToDouble(DBFunctions.SingleData("SELECT mord_numeorde, mord_salida FROM morden WHERE test_estado='F' AND pven_codigo='" + codigoRecepcionista + "' AND PVEN_VIGENCIA = 'V' ")); }
			catch{}
			return cantidadOrdenes;
		}
		
		protected void Preparar_Tabla_Recepcionistas()
		{
			tablaRecepcionistas = new DataTable();
			tablaRecepcionistas.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//0
			tablaRecepcionistas.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));//1
			tablaRecepcionistas.Columns.Add(new DataColumn("OTPC",System.Type.GetType("System.Double")));//2
			tablaRecepcionistas.Columns.Add(new DataColumn("OTPI",System.Type.GetType("System.Double")));//3
			tablaRecepcionistas.Columns.Add(new DataColumn("OTPRC",System.Type.GetType("System.Double")));//4
			tablaRecepcionistas.Columns.Add(new DataColumn("OTPRI",System.Type.GetType("System.Double")));//5
			tablaRecepcionistas.Columns.Add(new DataColumn("OTFSS",System.Type.GetType("System.Double")));//6
			tablaRecepcionistas.Columns.Add(new DataColumn("TOTALORDENES",System.Type.GetType("System.Double")));//7
			tablaRecepcionistas.Columns.Add(new DataColumn("ULTIMASORDENES",System.Type.GetType("System.String")));//8
		}

        protected void Llenar_Grilla_Recepcionistas(string codigoTaller, string FechaInicial, string FechaFinal)
		{
			this.Preparar_Tabla_Recepcionistas();
			//Traemos un Dataset con todos los recepcionistas disponibles para este taller
			DataSet recepcionistasTaller = new DataSet();
            DBFunctions.Request(recepcionistasTaller, IncludeSchema.NO, "SELECT pven_codigo, pven_nombre FROM pvendedor WHERE palm_almacen = '" + codigoTaller + "' AND tvend_codigo IN ('RT','RA','RC') AND PVEN_VIGENCIA = 'V';" + //CODIGO-NOMBRE 0-1 //0
                "SELECT PVE.pven_codigo, COUNT(MOR1.mord_numeorde) as OTPC FROM pvendedor PVE LEFT JOIN morden MOR1 ON PVE.pven_codigo = MOR1.pven_codigo WHERE PVE.palm_almacen = '" + codigoTaller + "' AND PVE.tvend_codigo IN ('RT','RA','RC') AND MOR1.tcar_cargo IN ('C','G','S') AND MOR1.test_estado IN ('A') AND MOR1.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' AND PVEN_VIGENCIA = 'V' GROUP BY PVE.pven_codigo,PVE.pven_nombre;" +//OTPC - 2 //1
                "SELECT PVE.pven_codigo, COUNT(MOR1.mord_numeorde) as OTPC FROM pvendedor PVE LEFT JOIN morden MOR1 ON PVE.pven_codigo = MOR1.pven_codigo WHERE PVE.palm_almacen = '" + codigoTaller + "' AND PVE.tvend_codigo IN ('RT','RA','RC') AND MOR1.tcar_cargo IN ('I','A','T') AND MOR1.test_estado IN ('A') AND MOR1.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' AND PVEN_VIGENCIA = 'V' GROUP BY PVE.pven_codigo,PVE.pven_nombre;" +//OTPI - 3 //2
                "SELECT PVE.pven_codigo, COUNT(MOR1.mord_numeorde) as OTPC FROM pvendedor PVE LEFT JOIN morden MOR1 ON PVE.pven_codigo = MOR1.pven_codigo WHERE PVE.palm_almacen = '" + codigoTaller + "' AND PVE.tvend_codigo IN ('RT','RA','RC') AND MOR1.tcar_cargo IN ('C','G','S') AND MOR1.mord_estaliqu IN ('P') AND MOR1.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' AND PVEN_VIGENCIA = 'V' GROUP BY PVE.pven_codigo,PVE.pven_nombre;" +//OTPRC - 4 //3
                "SELECT PVE.pven_codigo, COUNT(MOR1.mord_numeorde) as OTPC FROM pvendedor PVE LEFT JOIN morden MOR1 ON PVE.pven_codigo = MOR1.pven_codigo WHERE PVE.palm_almacen = '" + codigoTaller + "' AND PVE.tvend_codigo IN ('RT','RA','RC') AND MOR1.tcar_cargo IN ('I','A','T') AND MOR1.mord_estaliqu IN ('P') AND MOR1.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' AND PVEN_VIGENCIA = 'V' GROUP BY PVE.pven_codigo,PVE.pven_nombre;" +//OTPRI - 5 //4
                "SELECT PVE.pven_codigo, COUNT(MOR1.mord_numeorde) as OTPC FROM pvendedor PVE LEFT JOIN morden MOR1 ON PVE.pven_codigo = MOR1.pven_codigo WHERE PVE.palm_almacen = '" + codigoTaller + "' AND PVE.tvend_codigo IN ('RT','RA','RC') AND MOR1.test_estado IN ('F') AND MOR1.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' AND PVEN_VIGENCIA = 'V' GROUP BY PVE.pven_codigo,PVE.pven_nombre;" +//OTFSS - 6 //5
                "SELECT PVE.pven_codigo, MOR1.pdoc_codigo AS PREFIJO, MOR1.mord_numeorde AS NUMERO FROM pvendedor PVE LEFT JOIN morden MOR1 ON PVE.pven_codigo = MOR1.pven_codigo WHERE PVE.palm_almacen = '" + codigoTaller + "' AND PVE.tvend_codigo IN ('RT','RA','RC') AND MOR1.test_estado IN ('A') AND MOR1.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' AND PVEN_VIGENCIA = 'V'");//ULTIMASORDENES - 8 //6
			for(int i=0;i<recepcionistasTaller.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaRecepcionistas.NewRow();
				fila[0] = recepcionistasTaller.Tables[0].Rows[i][0].ToString();
				fila[1] = recepcionistasTaller.Tables[0].Rows[i][1].ToString();
				try{fila[2] = Convert.ToDouble((recepcionistasTaller.Tables[1].Select("pven_codigo='"+recepcionistasTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[2] = 0;}
				try{fila[3] = Convert.ToDouble((recepcionistasTaller.Tables[2].Select("pven_codigo='"+recepcionistasTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[3] = 0;}
				//Lo que  tiene que ver con preliquidacion aun esta pendiente
				try{fila[4] = Convert.ToDouble((recepcionistasTaller.Tables[3].Select("pven_codigo='"+recepcionistasTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[4] = 0;}				
				try{fila[5] = Convert.ToDouble((recepcionistasTaller.Tables[4].Select("pven_codigo='"+recepcionistasTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[5] = 0;}
				try{fila[6] = Convert.ToDouble((recepcionistasTaller.Tables[5].Select("pven_codigo='"+recepcionistasTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[6] = 0;}
				fila[7] = Convert.ToDouble(fila["OTPC"]) + Convert.ToDouble(fila["OTPI"]) + Convert.ToDouble(fila["OTFSS"]);
				fila[8] = Ultimas_Ordenes_Recepcionista(recepcionistasTaller.Tables[6].Select("pven_codigo='"+recepcionistasTaller.Tables[0].Rows[i][0]+"'"),recepcionistasTaller.Tables[0].Rows[i][0].ToString());
				tablaRecepcionistas.Rows.Add(fila);
				/*double cantidadOrdenes = 0;
			try{cantidadOrdenes = Convert.ToDouble(DBFunctions.SingleData("SELECT mord_numeorde, mord_salida FROM morden WHERE test_estado='F' AND pven_codigo='"+codigoRecepcionista+"'"));}catch{}
			return cantidadOrdenes;*/
			}
			actividadesRecepcionistas.DataSource = tablaRecepcionistas;
			actividadesRecepcionistas.DataBind();
		}
		
		protected void Preparar_Tabla_Mecanicos()
		{
			tablaMecanicos = new DataTable();
			tablaMecanicos.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//0
			tablaMecanicos.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));//1
			tablaMecanicos.Columns.Add(new DataColumn("OPRASIG",System.Type.GetType("System.Double")));//2
			tablaMecanicos.Columns.Add(new DataColumn("HORASIG",System.Type.GetType("System.Double")));//3
			tablaMecanicos.Columns.Add(new DataColumn("OPRCUMP",System.Type.GetType("System.Double")));//4
			tablaMecanicos.Columns.Add(new DataColumn("HORCUMP",System.Type.GetType("System.Double")));//5
			tablaMecanicos.Columns.Add(new DataColumn("OPRPARA",System.Type.GetType("System.Double")));//6
			tablaMecanicos.Columns.Add(new DataColumn("HORPARA",System.Type.GetType("System.Double")));//7
			tablaMecanicos.Columns.Add(new DataColumn("HORTOTAL",System.Type.GetType("System.Double")));//8
			tablaMecanicos.Columns.Add(new DataColumn("HORDISPO",System.Type.GetType("System.Double")));//9
			tablaMecanicos.Columns.Add(new DataColumn("NUMORDEN",System.Type.GetType("System.Double")));//10
			tablaMecanicos.Columns.Add(new DataColumn("ULTIMASORDENES",System.Type.GetType("System.String")));//11
		}
		
		protected void Llenar_Grilla_Mecanicos(string codigoTaller, string FechaInicial, string FechaFinal)
		{
			DateTime horaFinal = Convert.ToDateTime(DBFunctions.SingleData("SELECT ctal_hfmec FROM ctaller"));
			double diferencia = (horaFinal.TimeOfDay - DateTime.Now.TimeOfDay).TotalHours;
			Preparar_Tabla_Mecanicos();
			DataSet mecanicosTaller = new DataSet();
            DBFunctions.Request(mecanicosTaller, IncludeSchema.NO, 
                "SELECT pv.pven_codigo, pven_nombre FROM pvendedor pv, pvendedoralmacen pa WHERE pa.palm_almacen = '" + codigoTaller + "' AND tvend_codigo='MG' and pven_vigencia = 'V' and pv.pven_codigo = pa.pven_codigo; "+ // +//CODIGO-NOMBRE // 0-1 --0
 
                "SELECT DOR.pven_codigo,COUNT(DOR.ptem_operacion) "+
                "FROM dordenoperacion DOR "+
                " INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde,pvendedor PV "+
                " WHERE DOR.test_estado NOT in ('X','C','S') AND MOR.test_estado = 'A' AND PV.PVEN_CODIGO = DOR.PVEN_CODIGO and pven_vigencia = 'V' and MOR.palm_almacen = '" + codigoTaller + "' "+
                " AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "'  GROUP BY DOR.pven_codigo; "+ // +//OPRASIG //2 --1

                "SELECT pven_codigo, SUM(CARGA) FROM (SELECT DOR.pven_codigo, CASE WHEN DORD_TIEMPOPER = 0 OR DORD_TIEMPOPER IS NULL THEN 1 ELSE DORD_TIEMPOPER END AS CARGA "+
                "FROM dordenoperacion DOR "+
                " INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde,pvendedor PV "+
                " WHERE DOR.test_estado NOT in ('X','C','S') AND MOR.test_estado = 'A' AND PV.PVEN_CODIGO = DOR.PVEN_CODIGO and pven_vigencia = 'V' and MOR.palm_almacen = '" + codigoTaller + "' "+
                " AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "')   GROUP BY pven_codigo; "+ // +//HORASIG //2 --1

                "SELECT DOR.pven_codigo,COUNT(DOR.ptem_operacion) "+
                " FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde, pvendedor PV "+
                " WHERE DOR.test_estado = 'C' AND DOR.PVEN_CODIGO = PV.PVEN_CODIGO AND MOR.PALM_ALMACEN = '" + codigoTaller + "' "+
                " AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' GROUP BY DOR.pven_codigo;"+  // +//OPRCUMP //4 --3

                "SELECT DOR.pven_codigo, SUM (DORD_TIEMLIQU), SUM (DORD_TIEMPOPER) "+
                "FROM dordenoperacion DOR "+
                " INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde,pvendedor PV "+
                " WHERE DOR.test_estado = 'C' AND PV.PVEN_CODIGO = DOR.PVEN_CODIGO and pven_vigencia = 'V' and MOR.palm_almacen = '" + codigoTaller + "' "+
                " AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' GROUP BY DOR.pven_codigo; "+ // +//HORCUMP //5 --4

                "SELECT DOR.pven_codigo,COUNT(DOR.ptem_operacion) "+
                " FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde, PVENDEDOR PV "+
                " WHERE DOR.test_estado IN ('M','R','T','E','G') AND MOR.test_estado = 'A' AND   PV.PVEN_CODIGO = DOR.PVEN_CODIGO AND MOR.PALM_ALMACEN = '" + codigoTaller + "' "+
                " AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '2013-10-03' GROUP BY DOR.pven_codigo; "+ // +//OPRPARA //6 --5

                "SELECT pven_codigo,SUM(CARGA) FROM (SELECT DOR.pven_codigo, CASE WHEN DORD_TIEMPOPER = 0 OR DORD_TIEMPOPER IS NULL THEN 1 ELSE DORD_TIEMPOPER END AS CARGA "+
                "FROM dordenoperacion DOR "+
                " INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde,pvendedor PV "+
                " WHERE DOR.test_estado IN ('M','R','T','E','G') AND MOR.test_estado = 'A' AND PV.PVEN_CODIGO = DOR.PVEN_CODIGO and pven_vigencia = 'V' and MOR.palm_almacen = '" + codigoTaller + "' "+
                " AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "')   GROUP BY pven_codigo;"+ // +//HORPARA //7 --6

                "SELECT pven_codigo,COUNT(ORDEN) FROM ( "+
                " SELECT DISTINCT DOR.pven_codigo,DOR.PDOC_CODIGO || DOR.mord_numeorde AS ORDEN "+
                " FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde,pvendedor PV "+ 
                " WHERE MOR.test_estado = 'A' AND  PV.PVEN_CODIGO = DOR.PVEN_CODIGO AND MOR.PALM_ALMACEN = '" + codigoTaller + "' and pven_vigencia = 'V' "+ 
                " AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "') GROUP BY pven_codigo;"+ // +//NUMORDEN //10 --7

                "SELECT DISTINCT DOR.pven_codigo,DOR.pdoc_codigo,DOR.mord_numeorde "+
                " FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde,pvendedor PV "+ 
                " WHERE MOR.test_estado = 'A' and PV.PVEN_CODIGO = DOR.PVEN_CODIGO AND MOR.PALM_ALMACEN = '" + codigoTaller + "' and pven_vigencia = 'V' "+ 
                " AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' ORDER BY DOR.pdoc_codigo,DOR.mord_numeorde ASC ; " //ULTIMASORDENES //11 --8
                );
			for(int i=0;i<mecanicosTaller.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaMecanicos.NewRow();
				fila[0] = mecanicosTaller.Tables[0].Rows[i][0].ToString();
				fila[1] = mecanicosTaller.Tables[0].Rows[i][1].ToString();
				try{fila[2] = Convert.ToDouble((mecanicosTaller.Tables[1].Select("pven_codigo='"+mecanicosTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[2] = 0;}
				try{fila[3] = Convert.ToDouble((mecanicosTaller.Tables[2].Select("pven_codigo='"+mecanicosTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[3] = 0;}
				try{fila[4] = Convert.ToDouble((mecanicosTaller.Tables[3].Select("pven_codigo='"+mecanicosTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[4] = 0;}
				try{fila[5] = Convert.ToDouble((mecanicosTaller.Tables[4].Select("pven_codigo='"+mecanicosTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[5] = 0;}
				try{fila[6] = Convert.ToDouble((mecanicosTaller.Tables[5].Select("pven_codigo='"+mecanicosTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[6] = 0;}
				try{fila[7] = Convert.ToDouble((mecanicosTaller.Tables[6].Select("pven_codigo='"+mecanicosTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[7] = 0;}
				fila[8] = Convert.ToDouble(fila["HORASIG"]) + Convert.ToDouble(fila["HORCUMP"]) + Convert.ToDouble(fila["HORPARA"]);
				fila[9] = diferencia - Convert.ToDouble(fila["HORASIG"]);
				try{fila[10] = Convert.ToDouble((mecanicosTaller.Tables[7].Select("pven_codigo='"+mecanicosTaller.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[10] = 0;}
				fila[11] = Ultimas_Ordenes_Mecanico(mecanicosTaller.Tables[8].Select("pven_codigo='"+mecanicosTaller.Tables[0].Rows[i][0]+"'"), mecanicosTaller.Tables[0].Rows[i][0].ToString());
				tablaMecanicos.Rows.Add(fila);
			}
			actividadesMecanicos.DataSource = tablaMecanicos;
			actividadesMecanicos.DataBind();
		}
		
		protected void Preparar_Tabla_TotalOperaciones()
		{
			tablaOperacionesOrdenes = new DataTable();
			tablaOperacionesOrdenes.Columns.Add(new DataColumn("TIPOTRABAJO",System.Type.GetType("System.String")));//0
			tablaOperacionesOrdenes.Columns.Add(new DataColumn("HACL",System.Type.GetType("System.Double")));//1
			tablaOperacionesOrdenes.Columns.Add(new DataColumn("HAIN",System.Type.GetType("System.Double")));//2
			tablaOperacionesOrdenes.Columns.Add(new DataColumn("HCCL",System.Type.GetType("System.Double")));//3
			tablaOperacionesOrdenes.Columns.Add(new DataColumn("HCIN",System.Type.GetType("System.Double")));//4
			tablaOperacionesOrdenes.Columns.Add(new DataColumn("HPRCL",System.Type.GetType("System.Double")));//5
			tablaOperacionesOrdenes.Columns.Add(new DataColumn("HPRIN",System.Type.GetType("System.Double")));//6
			tablaOperacionesOrdenes.Columns.Add(new DataColumn("HPTCL",System.Type.GetType("System.Double")));//7
			tablaOperacionesOrdenes.Columns.Add(new DataColumn("HPTIN",System.Type.GetType("System.Double")));//8
		}
		
		protected void LLenar_Grilla_TotalOrdenes(string codigoTaller, string FechaInicial, string FechaFinal)
		{
			Preparar_Tabla_TotalOperaciones();
			DataSet trabajoOrdenes = new DataSet();
			DBFunctions.Request(trabajoOrdenes,IncludeSchema.NO,"SELECT tope_codigo,tope_nombre FROM ttipooperaciontaller;"+//TIPOTRABAJO //0
                "SELECT tope_codigo , SUM (tiempo_calculado) FROM (SELECT PTE.tope_codigo, CASE WHEN ptie_tiemclie IS NOT NULL AND ptie_tiemclie > 0 THEN sum (ptie_tiemclie) ELSE sum (PTE.ptem_tiempoestandar) END AS tiempo_calculado FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo INNER JOIN PVENDEDOR PV ON DOR.PVEN_CODIGO = PV.PVEN_CODIGO WHERE MOR.test_estado = 'A' AND 	DOR.test_estado  = 'A' AND PV.PVEN_VIGENCIA = 'V' AND PV.palm_almacen = '" + codigoTaller + "' AND DOR.tcar_cargo IN ('C','G','S') AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' group by tope_codigo,ptie_tiemclie,PTE.ptem_tiempoestandar) A GROUP BY tope_codigo;" + //HACL //1
                "SELECT tope_codigo , SUM (tiempo_calculado) FROM (SELECT PTE.tope_codigo, CASE WHEN ptie_tiemclie IS NOT NULL AND ptie_tiemclie > 0 THEN sum (ptie_tiemclie) ELSE sum (PTE.ptem_tiempoestandar) END AS tiempo_calculado FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo INNER JOIN PVENDEDOR PV ON DOR.PVEN_CODIGO = PV.PVEN_CODIGO WHERE MOR.test_estado = 'A' AND 	DOR.test_estado  = 'A' AND PV.PVEN_VIGENCIA = 'V' AND PV.palm_almacen = '" + codigoTaller + "' AND DOR.tcar_cargo IN ('I','A','T') AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' group by tope_codigo,ptie_tiemclie,PTE.ptem_tiempoestandar) A GROUP BY tope_codigo;" + //HAIN //2
                "SELECT tope_codigo , SUM (tiempo_calculado) FROM (SELECT PTE.tope_codigo, CASE WHEN ptie_tiemclie IS NOT NULL AND ptie_tiemclie > 0 THEN sum (ptie_tiemclie) ELSE sum (PTE.ptem_tiempoestandar) END AS tiempo_calculado FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo INNER JOIN PVENDEDOR PV ON DOR.PVEN_CODIGO = PV.PVEN_CODIGO WHERE MOR.test_estado = 'A' AND 	DOR.test_estado  = 'C' AND PV.PVEN_VIGENCIA = 'V' AND PV.palm_almacen = '" + codigoTaller + "' AND DOR.tcar_cargo IN ('C','G','S') AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' group by tope_codigo,ptie_tiemclie,PTE.ptem_tiempoestandar) A GROUP BY tope_codigo;" + //HCCL //3
                "SELECT tope_codigo , SUM (tiempo_calculado) FROM (SELECT PTE.tope_codigo, CASE WHEN ptie_tiemclie IS NOT NULL AND ptie_tiemclie > 0 THEN sum (ptie_tiemclie) ELSE sum (PTE.ptem_tiempoestandar) END AS tiempo_calculado FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo INNER JOIN PVENDEDOR PV ON DOR.PVEN_CODIGO = PV.PVEN_CODIGO WHERE MOR.test_estado = 'A' AND 	DOR.test_estado  = 'C' AND PV.PVEN_VIGENCIA = 'V' AND PV.palm_almacen = '" + codigoTaller + "' AND DOR.tcar_cargo IN ('I','A','T') AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' group by tope_codigo,ptie_tiemclie,PTE.ptem_tiempoestandar) A GROUP BY tope_codigo;" + //HCIN //4
                "SELECT tope_codigo , SUM (tiempo_calculado) FROM (SELECT PTE.tope_codigo, CASE WHEN ptie_tiemclie IS NOT NULL AND ptie_tiemclie > 0 THEN sum (ptie_tiemclie) ELSE sum (PTE.ptem_tiempoestandar) END AS tiempo_calculado FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo INNER JOIN PVENDEDOR PV ON DOR.PVEN_CODIGO = PV.PVEN_CODIGO WHERE MOR.test_estado = 'A' AND 	DOR.test_estado  = 'R' AND PV.PVEN_VIGENCIA = 'V' AND PV.palm_almacen = '" + codigoTaller + "' AND DOR.tcar_cargo IN ('C','G','S') AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' group by tope_codigo,ptie_tiemclie,PTE.ptem_tiempoestandar) A GROUP BY tope_codigo;" + //HPRCL //5
                "SELECT tope_codigo , SUM (tiempo_calculado) FROM (SELECT PTE.tope_codigo, CASE WHEN ptie_tiemclie IS NOT NULL AND ptie_tiemclie > 0 THEN sum (ptie_tiemclie) ELSE sum (PTE.ptem_tiempoestandar) END AS tiempo_calculado FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo INNER JOIN PVENDEDOR PV ON DOR.PVEN_CODIGO = PV.PVEN_CODIGO WHERE MOR.test_estado = 'A' AND 	DOR.test_estado  = 'R' AND PV.PVEN_VIGENCIA = 'V' AND PV.palm_almacen = '" + codigoTaller + "' AND DOR.tcar_cargo IN ('I','A','T') AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' group by tope_codigo,ptie_tiemclie,PTE.ptem_tiempoestandar) A GROUP BY tope_codigo;" + //HPRIN //6
                "SELECT tope_codigo , SUM (tiempo_calculado) FROM (SELECT PTE.tope_codigo, CASE WHEN ptie_tiemclie IS NOT NULL AND ptie_tiemclie > 0 THEN sum (ptie_tiemclie) ELSE sum (PTE.ptem_tiempoestandar) END AS tiempo_calculado FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo INNER JOIN PVENDEDOR PV ON DOR.PVEN_CODIGO = PV.PVEN_CODIGO WHERE MOR.test_estado = 'A' AND 	DOR.test_estado  IN ('M','S','T') AND PV.PVEN_VIGENCIA = 'V' AND PV.palm_almacen = '" + codigoTaller + "' AND DOR.tcar_cargo IN ('C','G','S') AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' group by tope_codigo,ptie_tiemclie,PTE.ptem_tiempoestandar) A GROUP BY tope_codigo;" + //HPTCL //7
                "SELECT tope_codigo , SUM (tiempo_calculado) FROM (SELECT PTE.tope_codigo, CASE WHEN ptie_tiemclie IS NOT NULL AND ptie_tiemclie > 0 THEN sum (ptie_tiemclie) ELSE sum (PTE.ptem_tiempoestandar) END AS tiempo_calculado FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo INNER JOIN PVENDEDOR PV ON DOR.PVEN_CODIGO = PV.PVEN_CODIGO WHERE MOR.test_estado = 'A' AND 	DOR.test_estado  IN ('M','S','T') AND PV.PVEN_VIGENCIA = 'V' AND PV.palm_almacen = '" + codigoTaller + "' AND DOR.tcar_cargo IN ('I','A','T') AND MOR.MORD_ENTRADA BETWEEN '" + FechaInicial + "' AND '" + FechaFinal + "' group by tope_codigo,ptie_tiemclie,PTE.ptem_tiempoestandar) A GROUP BY tope_codigo;"); //HPTIN //8
			for(int i=0;i<trabajoOrdenes.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaOperacionesOrdenes.NewRow();
				fila[0] = trabajoOrdenes.Tables[0].Rows[i][1].ToString();
				try{fila[1] = Convert.ToDouble((trabajoOrdenes.Tables[1].Select("tope_codigo='"+trabajoOrdenes.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[1] = 0;}
				try{fila[2] = Convert.ToDouble((trabajoOrdenes.Tables[2].Select("tope_codigo='"+trabajoOrdenes.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[2] = 0;}
				try{fila[3] = Convert.ToDouble((trabajoOrdenes.Tables[3].Select("tope_codigo='"+trabajoOrdenes.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[3] = 0;}
				try{fila[4] = Convert.ToDouble((trabajoOrdenes.Tables[4].Select("tope_codigo='"+trabajoOrdenes.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[4] = 0;}
				try{fila[5] = Convert.ToDouble((trabajoOrdenes.Tables[5].Select("tope_codigo='"+trabajoOrdenes.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[5] = 0;}
				try{fila[6] = Convert.ToDouble((trabajoOrdenes.Tables[6].Select("tope_codigo='"+trabajoOrdenes.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[6] = 0;}
				try{fila[7] = Convert.ToDouble((trabajoOrdenes.Tables[7].Select("tope_codigo='"+trabajoOrdenes.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[7] = 0;}
				try{fila[8] = Convert.ToDouble((trabajoOrdenes.Tables[8].Select("tope_codigo='"+trabajoOrdenes.Tables[0].Rows[i][0]+"'"))[0][1]);}
				catch{fila[8] = 0;}
				tablaOperacionesOrdenes.Rows.Add(fila);				
			}
			totalOperacionesOrdenes.DataSource = tablaOperacionesOrdenes;
			totalOperacionesOrdenes.DataBind();
		}
		
		//Funcion que nos retorna la cantidad de horas que tiene un mecanico con un estado especifico
		protected double Cantidad_Horas(string codigoVendedor, string estado)
		{
			double cantidadHoras = 0;
			DataSet operacionesMecanico = new DataSet();
			if(estado != "P" && estado != "A")
				DBFunctions.Request(operacionesMecanico,IncludeSchema.NO,"SELECT DISTINCT DOR.ptem_operacion,DOR.pdoc_codigo,DOR.mord_numeorde,PTE.ptem_descripcion,PTE.ptem_tiempoestandar,PTI.ptie_tiemclie "+
					"FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo "+
					"WHERE DOR.pven_codigo = '"+codigoVendedor+"' AND DOR.test_estado='"+estado+"' AND MOR.test_estado = 'A'");
			else if(estado == "P")
				DBFunctions.Request(operacionesMecanico,IncludeSchema.NO,"SELECT DISTINCT DOR.ptem_operacion,DOR.pdoc_codigo,DOR.mord_numeorde,PTE.ptem_descripcion,PTE.ptem_tiempoestandar,PTI.ptie_tiemclie "+
					"FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo "+
					"WHERE DOR.pven_codigo = '"+codigoVendedor+"' AND DOR.test_estado IN ('M','R','T') AND MOR.test_estado = 'A'");
			else if(estado == "A")
				DBFunctions.Request(operacionesMecanico,IncludeSchema.NO,"SELECT DISTINCT DOR.ptem_operacion,DOR.pdoc_codigo,DOR.mord_numeorde,PTE.ptem_descripcion,PTE.ptem_tiempoestandar,PTI.ptie_tiemclie "+
					"FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo = MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion INNER JOIN mcatalogovehiculo MCV ON MOR.mcat_vin = MCV.mcat_vin INNER JOIN pcatalogovehiculo PCV ON PCV.pcat_codigo = MCV.pcat_codigo LEFT JOIN ptiempotaller PTI ON PTE.ptem_operacion = PTI.ptie_tempario AND PTI.ptie_grupcata = PCV.pgru_grupo "+
					"WHERE DOR.pven_codigo = '"+codigoVendedor+"' AND DOR.test_estado NOT IN ('C','S','X') AND MOR.test_estado = 'A'");
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			for(int i=0;i<operacionesMecanico.Tables[0].Rows.Count;i++)
			{
				try{cantidadHoras += Convert.ToDouble(operacionesMecanico.Tables[0].Rows[i][5]);}
				catch{cantidadHoras += Convert.ToDouble(operacionesMecanico.Tables[0].Rows[i][4]);}
			}
			return cantidadHoras;
		}
		
		protected double Cantidad_Horas_Trabajo(string codigoTrabajo, string tipoCliente, string estado, string codigoTaller)
		{
			double cantidadHoras = 0;
			string consulta = "";
			DataSet totalOrdenesTrabajo = new DataSet();
			if(tipoCliente=="C")
				consulta = "SELECT DOR.ptem_operacion, DOR.pdoc_codigo, DOR.mord_numeorde  FROM dordenoperacion DOR, morden MOR WHERE DOR.pdoc_codigo=MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde AND MOR.ttra_codigo='"+codigoTrabajo+"' AND (DOR.tcar_cargo='C' OR DOR.tcar_cargo='G' OR DOR.tcar_cargo='S') AND MOR.palm_almacen='"+codigoTaller+"'";				
			else
				consulta = "SELECT DOR.ptem_operacion, DOR.pdoc_codigo, DOR.mord_numeorde  FROM dordenoperacion DOR, morden MOR WHERE DOR.pdoc_codigo=MOR.pdoc_codigo AND DOR.mord_numeorde = MOR.mord_numeorde AND MOR.ttra_codigo='"+codigoTrabajo+"' AND (DOR.tcar_cargo='A' OR DOR.tcar_cargo='I' OR DOR.tcar_cargo='T') AND DOR.test_estado='"+estado+"' AND MOR.palm_almacen='"+codigoTaller+"'";
			if(estado=="PR")
				consulta += " AND (DOR.test_estado='M' OR DOR.test_estado='R' OR DOR.test_estado='T')";
			else
				consulta += " AND DOR.test_estado='"+estado+"'";
			DBFunctions.Request(totalOrdenesTrabajo,IncludeSchema.NO,consulta);
			for(int i=0;i<totalOrdenesTrabajo.Tables[0].Rows.Count;i++)
			{
				string grupoCatalogo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM morden WHERE pdoc_codigo='"+totalOrdenesTrabajo.Tables[0].Rows[i][1].ToString()+"' AND mord_numeorde="+totalOrdenesTrabajo.Tables[0].Rows[i][2].ToString()+"))");
				if(codigoTrabajo!="P")
				{
					try{cantidadHoras += Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+totalOrdenesTrabajo.Tables[0].Rows[i][0].ToString()+"'"));}
					catch{cantidadHoras += Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM ptempario WHERE ptem_operacion='"+totalOrdenesTrabajo.Tables[0].Rows[i][0].ToString()+"'"));}
				}
			}
			return cantidadHoras;
		}
		
		protected string Ultimas_Ordenes_Recepcionista(DataRow[] draOrdenes, string codigoRecepcionista)
		{
			string ultimasOrdenes = "";
			if(draOrdenes.Length == 0)
				return string.Empty;
			for(int i=0;i<draOrdenes.Length;i++)
				ultimasOrdenes += draOrdenes[i][1]+" "+draOrdenes[i][2]+" </br>";
			return "<span onclick=\"MostrarOcultarDiv('ult_ord_rec_"+codigoRecepcionista+"')\" style=\"cursor: pointer; FONT-WEIGHT: bold; COLOR: red\">Mostrar Ordenes Abiertas</span><div id='ult_ord_rec_"+codigoRecepcionista+"' style='display:none'>"+ultimasOrdenes.Substring(0,ultimasOrdenes.Length-1)+"<div>";
		}
		
		protected string Ultimas_Ordenes_Mecanico(DataRow[] draUltimasOrdenes,string codigoMecanico)
		{
			string ultimasOrdenes = "";
			if(draUltimasOrdenes.Length == 0)
				return string.Empty;
			for(int i=0;i<draUltimasOrdenes.Length;i++)
				ultimasOrdenes += draUltimasOrdenes[i][1]+" "+draUltimasOrdenes[i][2]+" </br>";
			return "<span onclick=\"MostrarOcultarDiv('ult_ord_mec_"+codigoMecanico+"')\" style=\"cursor: pointer; FONT-WEIGHT: bold; COLOR: red\">Mostrar Ordenes Abiertas</span><div id='ult_ord_mec_"+codigoMecanico+"' style='display:none'>"+ultimasOrdenes.Substring(0,ultimasOrdenes.Length-1)+"<div>";
		}
		
		//Funcion que permite crear grafico para estadistica de mecanicos midiendo en horas
		protected void Crear_Grafico_Mecanicos()
		{
			System.Web.UI.WebControls.Image imagenEst= new System.Web.UI.WebControls.Image();
			imagenEst = Graficos.Generador_Grafico(imagenEst,"Gráfico de Actividades de Mecánicos",TipoGraficos.COLUMNAS,"Mecánicos","Horas",this.Cadena_Datos_Mecanicos(3,5,7,9));
			Response.Write("<script language:javascript>w=window.open('../est/esttemp.gif','','HEIGHT="+imagenEst.Height+",WIDTH="+imagenEst.Width+"');</script>");
		}
		
		protected string[] Cadena_Datos_Mecanicos(params int[] cantidadSeries)
		{
			//Debemos consultar la tabla que esta asociada al datagrid actividadesMecanicos que se llama tablaMecanicos
			string[] salida = new string[cantidadSeries.Length*3];
			for(int i=0;i<cantidadSeries.Length;i++)
			{
				salida[(i*3)] = "serie,"+(i+1).ToString()+","+actividadesMecanicos.Columns[cantidadSeries[i]].HeaderText;
				salida[(i*3)+1] = "categoria,"+(i+1).ToString()+",";
				salida[(i*3)+2] = "valores,"+(i+1).ToString()+",";
				for(int j=0;j<tablaMecanicos.Rows.Count;j++)
				{
					if(j==0)
					{
						salida[(i*3)+1] += tablaMecanicos.Rows[j][1].ToString();
						salida[(i*3)+2] += tablaMecanicos.Rows[j][cantidadSeries[i]].ToString();
					}
					else
					{
						salida[(i*3)+1] += "\t" + tablaMecanicos.Rows[j][1].ToString();
						salida[(i*3)+2] += "\t" + tablaMecanicos.Rows[j][cantidadSeries[i]].ToString();
					}
				}
			}
			return salida;
		}
		
		protected void Crear_Grafico_Operaciones()
		{
			System.Web.UI.WebControls.Image imagenOper= new System.Web.UI.WebControls.Image();
			imagenOper = Graficos.Generador_Grafico(imagenOper,"gráfico de operaciones en taller",TipoGraficos.COLUMNAS,"Tipo de Trabajo","Horas",this.Cadena_Datos_Operaciones(1,2,3,4,5,6));
			Response.Write("<script language:javascript>w=window.open('../est/esttemp.gif','','HEIGHT=420,WIDTH=540');</script>");
		}
		
		protected string[] Cadena_Datos_Operaciones(params int[] cantidadSeries)
		{
			string[] salida = new string[cantidadSeries.Length*3];
			for(int i=0;i<cantidadSeries.Length;i++)
			{
				salida[(i*3)] = "serie,"+(i+1).ToString()+","+totalOperacionesOrdenes.Columns[cantidadSeries[i]].HeaderText;
				salida[(i*3)+1] = "categoria,"+(i+1).ToString()+",";
				salida[(i*3)+2] = "valores,"+(i+1).ToString()+",";
				for(int j=0;j<tablaOperacionesOrdenes.Rows.Count;j++)
				{
					if(j==0)
					{
						salida[(i*3)+1] += tablaOperacionesOrdenes.Rows[j][0].ToString();
						salida[(i*3)+2] += tablaOperacionesOrdenes.Rows[j][cantidadSeries[i]].ToString();
					}
					else
					{
						salida[(i*3)+1] += "\t" + tablaOperacionesOrdenes.Rows[j][0].ToString();
						salida[(i*3)+2] += "\t" + tablaOperacionesOrdenes.Rows[j][cantidadSeries[i]].ToString();
					}
				}
			}
			return salida;
		}
		#endregion
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}