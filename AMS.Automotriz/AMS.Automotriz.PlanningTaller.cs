using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;

namespace AMS.Automotriz
{
	public class EstudioOperacion
	{
		#region Atributos
		private string codigoMecanico, codigoOperacion, prefijoOrdenTrabajo;
		private int numeroOrdenTrabajo;
		private double tiempoPlanificado, tiempoGastado;
		private DateTime fechorInicio, fechorFin;
		#endregion

		#region Propiedades
		public string CodigoMecanico{set{codigoMecanico=value;}get{return codigoMecanico;}}
		public string CodigoOperacion{set{codigoOperacion=value;}get{return codigoOperacion;}}
		public string PrefijoOrdenTrabajo{set{prefijoOrdenTrabajo=value;}get{return prefijoOrdenTrabajo;}}
		public int NumeroOrdenTrabajo{set{numeroOrdenTrabajo=value;}get{return numeroOrdenTrabajo;}}
		public double TiempoPlanificado{set{tiempoPlanificado=value;}get{return tiempoPlanificado;}}
		public double TiempoGastado{set{tiempoGastado=value;}get{return tiempoGastado;}}
		public DateTime FechorInicio{set{fechorInicio=value;}get{return fechorInicio;}}
		public DateTime FechorFin{set{fechorFin=value;}get{return fechorFin;}}
		#endregion

		#region Constructor
		
		public EstudioOperacion(string codigoMecanico, string codigoOperacion, string prefijoOrdenTrabajo, int numeroOrdenTrabajo,
								double tiempoPlanificado, double tiempoGastado, DateTime fechorInicio, DateTime fechorFin)
		{
			this.codigoMecanico = codigoMecanico;
			this.codigoOperacion = codigoOperacion;
			this.prefijoOrdenTrabajo = prefijoOrdenTrabajo;
			this.numeroOrdenTrabajo = numeroOrdenTrabajo;
			this.tiempoPlanificado = tiempoPlanificado;
			this.tiempoGastado = tiempoGastado;
			this.fechorInicio = fechorInicio;
			this.fechorFin = fechorFin;
		}

		#endregion

		#region Metodos
		#endregion
	}	
	
	public class CollectionEstudioOperacion : CollectionBase
	{
		#region Sobrecarga Metodos Heredados
		public virtual void Add(EstudioOperacion NewEstudioOperacion)
		{
			this.List.Add(NewEstudioOperacion);
		}

		public virtual EstudioOperacion this[int Index]
		{
			get{return (EstudioOperacion)this.List[Index];}
		}
		#endregion
	}

	/// <summary>
	/// Descripción breve de PlanningTaller.
	/// </summary>
	public class PlanningTaller
	{
		#region Atributos
		private DataTable dtSalida, dtMecanicos, dtParalizadas, dtSinAsignar, dtCumplidas, dtNoAutorizadas;
		private DateTime horaInicioTaller,horaFinalTaller;
		#endregion Atributos

		#region Propiedades
		public DataTable DtConsultaPlanningAsignadas{get{return dtSalida;}}
		public DataTable DtMecanicosPlanning{get{return dtMecanicos;}}
		public DataTable DtOperacionesParalizadas{get{return dtParalizadas;}}
		public DataTable DtOperacionesSinAsignar{get{return dtSinAsignar;}}
		public DataTable DtOperacionesCumplidas{get{return dtCumplidas;}}
		public DataTable DtOperacionesNoAutorizadas{get{return dtNoAutorizadas;}}
		public DateTime HoraInicioTaller{get{return horaInicioTaller;}}
		public DateTime HoraFinalTaller{get{return horaFinalTaller;}}
		public DataTable FechasConsulta
		{
			get
			{
				DataTable dtFechas = new DataTable();
				dtFechas.Columns.Add(new DataColumn("fechaVal",typeof(string)));
				dtFechas.Columns.Add(new DataColumn("fechaTex",typeof(string)));
				DataRow[] draSalida = dtSalida.Select("","fecha_hora_inicio ASC");
				for(int i=0;i<draSalida.Length;i++)
				{
					string strFechaInicio = Convert.ToDateTime(draSalida[i][6]).ToString("yyyy-MM-dd");
					string strFechaFin = Convert.ToDateTime(draSalida[i][7]).ToString("yyyy-MM-dd");
					if(dtFechas.Select("fechaVal='"+strFechaInicio+"'").Length == 0)
					{
						DataRow dr = dtFechas.NewRow();
						dr[0] = strFechaInicio;
						dr[1] = Convert.ToDateTime(draSalida[i][6]).GetDateTimeFormats(new CultureInfo("es-CO"))[7];
						dtFechas.Rows.Add(dr);
					}
					if(dtFechas.Select("fechaVal='"+strFechaFin+"'").Length == 0)
					{
						DataRow dr = dtFechas.NewRow();
						dr[0] = strFechaFin;
						dr[1] = Convert.ToDateTime(draSalida[i][7]).GetDateTimeFormats(new CultureInfo("es-CO"))[7];
						dtFechas.Rows.Add(dr);
					}
				}
				return dtFechas;
			}
		}
		#endregion 
		
		#region Constructor
		public PlanningTaller(string codigoTaller)
		{
			// Consultas
			DataSet dsConsulta = new DataSet();
			DBFunctions.Request(dsConsulta,IncludeSchema.NO,"SELECT DISTINCT DOR.pven_codigo,PVE.pven_nombre FROM dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo=MOR.pdoc_codigo AND DOR.mord_numeorde=MOR.mord_numeorde INNER JOIN pvendedor PVE ON DOR.pven_codigo=PVE.pven_codigo WHERE MOR.test_estado = 'A' AND MOR.palm_almacen='"+codigoTaller+"' AND PVE.pven_vigencia = 'V';"+ //0
															"SELECT DOR.pdoc_codigo, DOR.mord_numeorde, DOR.ptem_operacion, DOR.pven_codigo, MOR.mord_entrada, MOR.mord_horaentr, DOR.tiempo_calculado FROM vtaller_dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo=MOR.pdoc_codigo AND DOR.mord_numeorde=MOR.mord_numeorde WHERE DOR.test_estado = 'A' AND MOR.test_estado = 'A' AND MOR.palm_almacen = '"+codigoTaller+"' ORDER BY MOR.mord_entrada,DOR.pven_codigo ASC;"+ //1
															"SELECT DOR.pdoc_codigo, DOR.mord_numeorde, DOR.ptem_operacion, DOR.pven_codigo, DEO.test_estado, DEO.destoper_hora, DOR.tiempo_calculado FROM vtaller_dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo=MOR.pdoc_codigo AND DOR.mord_numeorde=MOR.mord_numeorde LEFT JOIN destadisticaoperacion DEO ON DOR.pdoc_codigo=DEO.pdoc_codigo AND DOR.mord_numeorde=DEO.mord_numeorde AND DOR.ptem_operacion=DEO.ptem_operacion WHERE DOR.test_estado = 'A' AND MOR.test_estado = 'A' AND MOR.palm_almacen = '"+codigoTaller+"' ORDER BY MOR.mord_entrada,DOR.pven_codigo ASC;"+ //2
															"SELECT DOR.pdoc_codigo, DOR.mord_numeorde, DOR.ptem_operacion, PTE.ptem_descripcion, DOR.pven_codigo as codigo_mecanico, DOR.test_estado, TES.test_nombre as estado_operacion FROM vtaller_dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo=MOR.pdoc_codigo AND DOR.mord_numeorde=MOR.mord_numeorde INNER JOIN testadooperacion TES ON DOR.test_estado = TES.test_estaoper INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion WHERE DOR.test_estado IN ('M','R','T') AND MOR.test_estado = 'A' AND MOR.palm_almacen = '"+codigoTaller+"' ORDER BY MOR.mord_entrada,DOR.pven_codigo ASC;"+ //3
															"SELECT DOR.pdoc_codigo as prefijo_orden, DOR.mord_numeorde as numero_orden, DOR.ptem_operacion as codigo_operacion, PTE.ptem_descripcion as descripcion_operacion FROM vtaller_dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo=MOR.pdoc_codigo AND DOR.mord_numeorde=MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion WHERE DOR.test_estado = 'S' AND MOR.test_estado = 'A' AND MOR.palm_almacen = '"+codigoTaller+"' ORDER BY MOR.mord_entrada,DOR.pven_codigo ASC;"+ //4
															"SELECT DOR.pdoc_codigo, DOR.mord_numeorde, DOR.ptem_operacion, PTE.ptem_descripcion, DOR.pven_codigo as codigo_mecanico, DOR.test_estado, TES.test_nombre as estado_operacion FROM vtaller_dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo=MOR.pdoc_codigo AND DOR.mord_numeorde=MOR.mord_numeorde INNER JOIN testadooperacion TES ON DOR.test_estado = TES.test_estaoper INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion WHERE DOR.test_estado = 'C' AND MOR.test_estado = 'A' AND MOR.palm_almacen = '"+codigoTaller+"' ORDER BY MOR.mord_entrada,DOR.pven_codigo ASC;"+ //5
															"SELECT DOR.pdoc_codigo as prefijo_orden, DOR.mord_numeorde as numero_orden, DOR.ptem_operacion as codigo_operacion, PTE.ptem_descripcion as descripcion_operacion FROM vtaller_dordenoperacion DOR INNER JOIN morden MOR ON DOR.pdoc_codigo=MOR.pdoc_codigo AND DOR.mord_numeorde=MOR.mord_numeorde INNER JOIN ptempario PTE ON DOR.ptem_operacion = PTE.ptem_operacion WHERE DOR.test_estado = 'X' AND MOR.test_estado = 'A' AND MOR.palm_almacen = '"+codigoTaller+"' ORDER BY MOR.mord_entrada,DOR.pven_codigo ASC;"); //6
			PrepararDtSalida();
			// Fin Consultas
			// Inicio Construcción de Intervalo de Atención
			horaInicioTaller = Convert.ToDateTime(DBFunctions.SingleData("SELECT ctal_himec FROM ctaller"));
			horaFinalTaller = Convert.ToDateTime(DBFunctions.SingleData("SELECT ctal_hfmec FROM ctaller"));
			// Fin Construcción de Intervalo de Atención
			// Inicio Construccion Asignaciones Mecanico
			for(int i=0;i<dsConsulta.Tables[0].Rows.Count;i++)
			{
				CollectionEstudioOperacion a = RealizarEstudioOperaciones(dsConsulta.Tables[0].Rows[i][0].ToString(),dsConsulta.Tables[1].Select("pven_codigo='"+dsConsulta.Tables[0].Rows[i][0]+"'","mord_entrada,mord_horaentr ASC"),dsConsulta.Tables[2]);
				for(int j=0;j<a.Count;j++)
				{
					DataRow dr = dtSalida.NewRow();
					dr[0] = a[j].CodigoMecanico;
					dr[1] = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+a[j].CodigoMecanico+"'");
					dr[2] = a[j].PrefijoOrdenTrabajo;
					dr[3] = a[j].NumeroOrdenTrabajo;
					dr[4] = a[j].CodigoOperacion;
					dr[5] = DBFunctions.SingleData("SELECT ptem_descripcion FROM ptempario WHERE ptem_operacion='"+a[j].CodigoOperacion+"'");
					dr[6] = a[j].FechorInicio;
					dr[7] = a[j].FechorFin;
					dr[8] = a[j].TiempoPlanificado;
					dr[9] = a[j].TiempoGastado;
					dtSalida.Rows.Add(dr);
				}
			}
			// Fin Construccion Asignaciones Mecanico
			// Inicio de Asignación de tabla de mecanicos general
			dtMecanicos = dsConsulta.Tables[0].Copy();
			// Fin de Asignación de tabla de mecanicos general
			// Inicio de Asignación de tabla de operaciones paralizadas
			dtParalizadas = dsConsulta.Tables[3].Copy();
			// Fin de Asignación de tabla de operaciones paralizadas
			// Inicio de Asignación de tabla de operaciones sin asignar
			dtSinAsignar = dsConsulta.Tables[4].Copy();
			// Fin de Asignación de tabla de operaciones sin asignar
			// Inicio de Asignación de tabla de operaciones cumplidas
			dtCumplidas = dsConsulta.Tables[5].Copy();
			// Fin de Asignación de tabla de operaciones cumplidas
			// Inicio de Asignación de tabla de operaciones cumplidas
			dtNoAutorizadas = dsConsulta.Tables[6].Copy();
			// Fin de Asignación de tabla de operaciones cumplidas
		}
		#endregion

		#region Metodos
		private void PrepararDtSalida()
		{
			dtSalida = new DataTable();
			dtSalida.Columns.Add(new DataColumn("codigo_mecanico",typeof(string)));//0
			dtSalida.Columns.Add(new DataColumn("nombre_mecanico",typeof(string)));//1
			dtSalida.Columns.Add(new DataColumn("prefijo_orden",typeof(string)));//2
			dtSalida.Columns.Add(new DataColumn("numero_orden",typeof(int)));//3
			dtSalida.Columns.Add(new DataColumn("codigo_operacion",typeof(string)));//4
			dtSalida.Columns.Add(new DataColumn("nombre_operacion",typeof(string)));//5
			dtSalida.Columns.Add(new DataColumn("fecha_hora_inicio",typeof(DateTime)));//6
			dtSalida.Columns.Add(new DataColumn("fecha_hora_fin",typeof(DateTime)));//7
			dtSalida.Columns.Add(new DataColumn("duracion_operacion",typeof(double)));//8
			dtSalida.Columns.Add(new DataColumn("tiempo_gastado",typeof(double)));//9
		}

		private CollectionEstudioOperacion RealizarEstudioOperaciones(string codigoMecanico, DataRow[] draOperacionesAsignadas, DataTable dtEstadisticasOperaciones)
		{
			int i = 0;
			ArrayList operacionesParalizadas = new ArrayList();			
			CollectionEstudioOperacion salida = new CollectionEstudioOperacion();
			DateTime fechaPivote = DateTime.Now;
			if(draOperacionesAsignadas.Length > 0)
			{
				for(i=0;i<draOperacionesAsignadas.Length;i++)
				{
					double tiempoGastado = 0;
					bool estadoOperacionParalizado = false;
					DateTime fechaEntrada = Convert.ToDateTime(draOperacionesAsignadas[i][4]);
					string horaEntrada = draOperacionesAsignadas[i][5].ToString();
					DateTime fechaPivoteAnt = fechaPivote;
					DateTime fechaSalida = fechaPivote;
					DateTime fechaUltimaEstadistica = fechaPivote;
					if(i==0)
					{
						fechaPivote = Convert.ToDateTime(fechaEntrada.ToString("yyyy-MM-dd")+" "+horaEntrada);
						fechaPivoteAnt = fechaPivote;
						fechaSalida = CalculoFechafinalizacion(fechaPivote,false,ref tiempoGastado,dtEstadisticasOperaciones.Select("pdoc_codigo='"+draOperacionesAsignadas[i][0]+"' AND mord_numeorde="+draOperacionesAsignadas[i][1]+" AND ptem_operacion='"+draOperacionesAsignadas[i][2]+"'","destoper_hora ASC"),Convert.ToDateTime(fechaEntrada.ToString("yyyy-MM-dd")+" "+horaEntrada),ref estadoOperacionParalizado, ref fechaUltimaEstadistica);
					}
					else
						fechaSalida = CalculoFechafinalizacion(fechaPivote,true,ref tiempoGastado,dtEstadisticasOperaciones.Select("pdoc_codigo='"+draOperacionesAsignadas[i][0]+"' AND mord_numeorde="+draOperacionesAsignadas[i][1]+" AND ptem_operacion='"+draOperacionesAsignadas[i][2]+"'","destoper_hora ASC"),Convert.ToDateTime(fechaEntrada.ToString("yyyy-MM-dd")+" "+horaEntrada),ref estadoOperacionParalizado, ref fechaUltimaEstadistica);
					if(estadoOperacionParalizado)
						operacionesParalizadas.Add(new EstudioOperacion(codigoMecanico,draOperacionesAsignadas[i][2].ToString(),draOperacionesAsignadas[i][0].ToString(),Convert.ToInt32(draOperacionesAsignadas[i][1]),Convert.ToDouble(draOperacionesAsignadas[i][6]),tiempoGastado,fechaSalida,fechaSalida));
					else
					{
						fechaPivote = fechaSalida;
						//Ahora se revisa si la fecha de pivoteAnt es mayor a la fecha de entrada de la orden de trabajo asociada a la operacion
						if(fechaPivoteAnt >= fechaUltimaEstadistica)
							salida.Add(new EstudioOperacion(codigoMecanico,draOperacionesAsignadas[i][2].ToString(),draOperacionesAsignadas[i][0].ToString(),Convert.ToInt32(draOperacionesAsignadas[i][1]),Convert.ToDouble(draOperacionesAsignadas[i][6]),tiempoGastado,fechaPivoteAnt,fechaPivote));
						else
						{
							if(Convert.ToDouble(draOperacionesAsignadas[i][6])-tiempoGastado > 0)
								salida.Add(new EstudioOperacion(codigoMecanico,draOperacionesAsignadas[i][2].ToString(),draOperacionesAsignadas[i][0].ToString(),Convert.ToInt32(draOperacionesAsignadas[i][1]),Convert.ToDouble(draOperacionesAsignadas[i][6]),tiempoGastado,fechaUltimaEstadistica,AjustarFechaHorario(fechaUltimaEstadistica.AddHours(Convert.ToDouble(draOperacionesAsignadas[i][6])-tiempoGastado),Convert.ToDateTime(fechaUltimaEstadistica.ToString("yyyy-MM-dd")+" "+horaFinalTaller.ToString("HH:mm:ss")))));
							else
								salida.Add(new EstudioOperacion(codigoMecanico,draOperacionesAsignadas[i][2].ToString(),draOperacionesAsignadas[i][0].ToString(),Convert.ToInt32(draOperacionesAsignadas[i][1]),Convert.ToDouble(draOperacionesAsignadas[i][6]),tiempoGastado,fechaUltimaEstadistica,AjustarFechaHorario(fechaUltimaEstadistica.AddHours(0.5),Convert.ToDateTime(fechaUltimaEstadistica.ToString("yyyy-MM-dd")+" "+horaFinalTaller.ToString("HH:mm:ss")))));
						}
					}	
				}
				for(i=0;i<operacionesParalizadas.Count;i++)
				{
					double tiempo = 0;
					DateTime fechaInicio = ((EstudioOperacion)operacionesParalizadas[i]).FechorInicio;
					DateTime fechaFin = ((EstudioOperacion)operacionesParalizadas[i]).FechorFin;
					if((((EstudioOperacion)operacionesParalizadas[i]).TiempoPlanificado - ((EstudioOperacion)operacionesParalizadas[i]).TiempoGastado) <= 0)
					{
						tiempo = 0.5;
						//((EstudioOperacion)operacionesParalizadas[i]).FechorFin = fechaPivote = AjustarFechaHorario(((EstudioOperacion)operacionesParalizadas[i]).FechorInicio.AddHours(tiempo),Convert.ToDateTime(((EstudioOperacion)operacionesParalizadas[i]).FechorInicio.ToString("yyyy-MM-dd")+" "+horaFinalTaller.ToString("HH:mm:ss")));
						fechaFin = fechaPivote = AjustarFechaHorario(((EstudioOperacion)operacionesParalizadas[i]).FechorInicio.AddHours(tiempo),Convert.ToDateTime(((EstudioOperacion)operacionesParalizadas[i]).FechorInicio.ToString("yyyy-MM-dd")+" "+horaFinalTaller.ToString("HH:mm:ss")));
					}
					else
					{
						tiempo = ((EstudioOperacion)operacionesParalizadas[i]).TiempoPlanificado - ((EstudioOperacion)operacionesParalizadas[i]).TiempoGastado;
						//((EstudioOperacion)operacionesParalizadas[i]).FechorFin = fechaPivote = AjustarFechaHorario(((EstudioOperacion)operacionesParalizadas[i]).FechorInicio.AddHours(tiempo), Convert.ToDateTime(((EstudioOperacion)operacionesParalizadas[i]).FechorInicio.ToString("yyyy-MM-dd")+" "+horaFinalTaller.ToString("HH:mm:ss")));
						fechaFin = fechaPivote = AjustarFechaHorario(((EstudioOperacion)operacionesParalizadas[i]).FechorInicio.AddHours(tiempo), Convert.ToDateTime(((EstudioOperacion)operacionesParalizadas[i]).FechorInicio.ToString("yyyy-MM-dd")+" "+horaFinalTaller.ToString("HH:mm:ss")));
					}
					RevisionColisionTiemposSecundarios(salida, tiempo, ref fechaInicio, ref fechaFin);
					((EstudioOperacion)operacionesParalizadas[i]).FechorInicio = fechaInicio;
					((EstudioOperacion)operacionesParalizadas[i]).FechorFin = fechaFin;
					salida.Add((EstudioOperacion)operacionesParalizadas[i]);
				}
			}
			return salida;
		}

		private DateTime CalculoFechafinalizacion(DateTime fechaPivote, bool usoFechaPivote, ref double tiempoGastado, DataRow[] draEstadisticas, DateTime fechaEntradaOrden, ref bool operacionParalizada, ref DateTime fechaUltimaEstadistica)
		{
			string estadoAnterior = "A";
			double tiempoCalculado = 0;
			operacionParalizada = false;
			tiempoGastado = 0;
			DateTime fechaOperacion = DateTime.Now;
			if(usoFechaPivote && fechaPivote < fechaEntradaOrden)
				fechaOperacion = fechaPivote;
			else
				fechaOperacion = fechaEntradaOrden;
			for(int i=0;i<draEstadisticas.Length;i++)
			{
				if(tiempoGastado == 0)
					tiempoCalculado = Convert.ToDouble(draEstadisticas[i][6]);
				if(draEstadisticas[i][4].ToString() != "A" && estadoAnterior == "A")
				{
					DateTime fechaEvento = Convert.ToDateTime(draEstadisticas[i][5]);
					TimeSpan diferencia = fechaEvento - fechaOperacion;
					if(diferencia.TotalHours > 0)
						tiempoGastado += diferencia.TotalHours;
					operacionParalizada = true;
				}
				estadoAnterior = draEstadisticas[i][4].ToString();
				if(operacionParalizada)
					fechaOperacion = Convert.ToDateTime(draEstadisticas[i][5]);
				fechaUltimaEstadistica = Convert.ToDateTime(draEstadisticas[i][5]);
			}
			if(!operacionParalizada)
			{
				DateTime fechaFinHorarioDia = Convert.ToDateTime(fechaOperacion.ToString("yyyy-MM-dd")+" "+horaFinalTaller.ToString("HH:mm:ss"));
				if((tiempoCalculado - tiempoGastado) <= 0)
					fechaOperacion = fechaOperacion.AddHours(0.5);
				else
					fechaOperacion = fechaOperacion.AddHours((tiempoCalculado - tiempoGastado));
				fechaOperacion = AjustarFechaHorario(fechaOperacion,fechaFinHorarioDia);
			}
			return fechaOperacion;
		}

		private DateTime AjustarFechaHorario(DateTime fechaAjuste, DateTime fechaCota)
		{
			if(fechaAjuste <= fechaCota)
				return fechaAjuste;
			else
			{
				double cantidadTiempoAjuste = (fechaAjuste - fechaCota).TotalHours;
				fechaAjuste = Convert.ToDateTime(fechaAjuste.AddDays(1).ToString("yyyy-MM-dd")+" "+horaInicioTaller.ToString("HH:mm:ss")).AddHours(cantidadTiempoAjuste);
				fechaCota = Convert.ToDateTime(fechaAjuste.ToString("yyyy-MM-dd")+" "+horaFinalTaller.ToString("HH:mm:ss"));
				return AjustarFechaHorario(fechaAjuste,fechaCota);
			}
		}

		private void RevisionColisionTiemposSecundarios(CollectionEstudioOperacion salida, double tiempo, ref DateTime fechaInicio, ref DateTime fechaFin)
		{
			for(int i=0;i<salida.Count;i++)
			{
				if((fechaInicio >= salida[i].FechorInicio && fechaInicio <= salida[i].FechorFin) || (fechaFin >= salida[i].FechorInicio && fechaFin <= salida[i].FechorFin))
				{
					fechaInicio = salida[i].FechorFin;
					fechaFin = fechaInicio.AddHours(tiempo);
				}
			}
		}

		public ArrayList ConstruirIntervalo(DateTime dtInicio, DateTime dtFin, double tamIntervalo)
		{
			ArrayList intervalos = new ArrayList();
			while(dtInicio <= dtFin)
			{
				intervalos.Add(dtInicio.ToString("HH:mm")+"-"+dtInicio.AddHours(tamIntervalo).ToString("HH:mm"));
				dtInicio = dtInicio.AddHours(tamIntervalo);
			}
			return intervalos;
		}

		public ArrayList ConstruirIntervalo(double tamIntervalo)
		{
			DateTime dtInicio = horaInicioTaller;
			DateTime dtFin = horaFinalTaller;
			ArrayList intervalos = new ArrayList();
			while(dtInicio < dtFin)
			{
				intervalos.Add(dtInicio.ToString("HH:mm")+"-"+dtInicio.AddHours(tamIntervalo).ToString("HH:mm"));
				dtInicio = dtInicio.AddHours(tamIntervalo);
			}
			return intervalos;
		}
		#endregion
	}

	public class UtilitarioPlanning	
	{
		#region Metodos
		public static string ReturnRGBParalizado(string codigoEstado)
		{
			if(codigoEstado == "T")
				return "9900FF";
			else if(codigoEstado == "R")
				return "FF0033";
			else if(codigoEstado == "M")
				return "FF9900";
			else if(codigoEstado == "C")
				return "66CC33";
			return "33CC00";
		}

		public static DateTime ValidarParametrosFecha(DateTime fechaConsulta)
		{
			DateTime fechaArreglo = fechaConsulta;
			if(UtilitarioPlanning.GetWeekDayNumber(fechaConsulta.DayOfWeek) == 7 || DBFunctions.RecordExist("SELECT pdf_secuencia FROM pdiafestivo WHERE pdf_fech='"+fechaConsulta.ToString("yyyy-MM-dd")+"'"))
				fechaArreglo = fechaArreglo.AddDays(1);
			return fechaArreglo;
		}

		public static int GetWeekDayNumber(DayOfWeek day)
		{
			int number = -1;
			if(day == DayOfWeek.Monday)
				number = 1;
			else if(day == DayOfWeek.Tuesday)
				number = 2;
			else if(day == DayOfWeek.Wednesday)
				number = 3;
			else if(day == DayOfWeek.Thursday)
				number = 4;
			else if(day == DayOfWeek.Friday)
				number = 5;
			else if(day == DayOfWeek.Saturday)
				number = 6;
			else if(day == DayOfWeek.Sunday)
				number = 7;
			return number;
		}
		#endregion
	}
}
