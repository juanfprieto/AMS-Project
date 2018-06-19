using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using Ajax;

namespace AMS.Automotriz
{
	/// <summary>
	///		Descripción breve de AMS_Automotriz_PlanningTaller.
	/// </summary>
	public partial class AMS_Automotriz_PlanningTaller : System.Web.UI.UserControl
	{
		#region Atributos
		private DataTable dtFuentePlanning;
		private DatasToControls bind = new DatasToControls();
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Automotriz_PlanningTaller));
			if(!IsPostBack)
			{
				lbFechaProcesoConsulta.Text += DateTime.Now.GetDateTimeFormats(new CultureInfo("es-Co"))[9];
                bind.PutDatasIntoDropDownList(ddlTaller, "SELECT palm_almacen,palm_descripcion FROM palmacen pa where (pa.pcen_centtal is not null  or pcen_centcoli is not null) and pa.tvig_vigencia='V' order by pa.PALM_DESCRIPCION;");
				CargarInfoTaller();
			}
		}

		protected void ddlFechaConsulta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PlanningTaller inst = new PlanningTaller(ddlTaller.SelectedValue);
			PrepararDtFuentePlanning(inst.ConstruirIntervalo(1));
			ConstruirDtFuentePlanning(inst.ConstruirIntervalo(1),inst.DtMecanicosPlanning,inst.DtConsultaPlanningAsignadas, inst.DtOperacionesParalizadas, inst.DtOperacionesCumplidas);
			BindDgPlanning();
			BindDgSinAsignar(inst.DtOperacionesSinAsignar);
			BindDgNoAutorizar(inst.DtOperacionesNoAutorizadas);
		}

		protected void ddlTaller_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CargarInfoTaller();
		}
		#endregion

		#region Metodos
		private void CargarInfoTaller()
		{
			PlanningTaller inst = new PlanningTaller(ddlTaller.SelectedValue);
			DataTable dtFechas = inst.FechasConsulta.Copy();
			ddlFechaConsulta.DataSource = dtFechas;
			ddlFechaConsulta.DataValueField = dtFechas.Columns[0].ColumnName;
			ddlFechaConsulta.DataTextField = dtFechas.Columns[1].ColumnName;
			ddlFechaConsulta.DataBind();
			PrepararDtFuentePlanning(inst.ConstruirIntervalo(1));
			ConstruirDtFuentePlanning(inst.ConstruirIntervalo(1),inst.DtMecanicosPlanning,inst.DtConsultaPlanningAsignadas,inst.DtOperacionesParalizadas, inst.DtOperacionesCumplidas);
			BindDgPlanning();
			BindDgSinAsignar(inst.DtOperacionesSinAsignar);
			BindDgNoAutorizar(inst.DtOperacionesNoAutorizadas);
		}

		private void PrepararDtFuentePlanning(ArrayList intervalosMecanico)
		{
			dtFuentePlanning = new DataTable();
			//Se agrega la columna donde se almacena la información relacionada con el mecanico
			dtFuentePlanning.Columns.Add(new DataColumn("Técnico",typeof(string)));
			//Ahora se agregan las columnas relacionadas con los intervalos de consulta de los tecnicos
			for(int i=0;i<intervalosMecanico.Count;i++)
				dtFuentePlanning.Columns.Add(new DataColumn(intervalosMecanico[i].ToString(),typeof(string)));
			//Se agrega la columna correspondiente a operaciones paralizadas
			dtFuentePlanning.Columns.Add(new DataColumn("Operaciones Paralizadas",typeof(string)));
			//Se agrega la columna correspondiente a operaciones cumplidas de ordenes abiertas
			dtFuentePlanning.Columns.Add(new DataColumn("Operaciones Cumplidas",typeof(string)));
		}

		private void BindDgPlanning()
		{
			dgPlanning.DataSource = dtFuentePlanning;
			dgPlanning.DataBind();
		}

		private void BindDgSinAsignar(DataTable dtSource)
		{
			lbTotalSinAsignar.Text = dtSource.Rows.Count.ToString();
			dgSinAsignar.DataSource = dtSource;
			dgSinAsignar.DataBind();
		}

		private void BindDgNoAutorizar(DataTable dtSource)
		{
			lbTotalNoAutorizada.Text = dtSource.Rows.Count.ToString();
			dgNoAutorizada.DataSource = dtSource;
			dgNoAutorizada.DataBind();
		}

		private void ConstruirDtFuentePlanning(ArrayList intervalos, DataTable dtMecanicos, DataTable dtAsignaciones, DataTable dtOperacionesParalizadas, DataTable dtOperacionesCumplidas)
		{
			//Se crea el datarow correspondiente a cada mecanico
			for(int i=0;i<dtMecanicos.Rows.Count;i++)
			{
				DataRow dr = dtFuentePlanning.NewRow();
				dr["Técnico"] = dtMecanicos.Rows[i][0]+" - "+dtMecanicos.Rows[i][1];
				for(int j=0;j<intervalos.Count;j++)
					dr[intervalos[j].ToString()] = ConstruirTextoIntervalo(intervalos[j].ToString(),dtAsignaciones.Select("codigo_mecanico='"+dtMecanicos.Rows[i][0]+"'"));
				dr["Operaciones Paralizadas"] = ConstruirTextoParalizadas(dtOperacionesParalizadas.Select("codigo_mecanico='"+dtMecanicos.Rows[i][0]+"'","estado_operacion ASC"));
				dr["Operaciones Cumplidas"] = "<span id='span_"+dtMecanicos.Rows[i][0]+"' style='color:Blue;cursor:pointer' onclick=\"MostrarCumplidas('"+dtMecanicos.Rows[i][0]+"');\">Ver</span><div id='div_cump_"+dtMecanicos.Rows[i][0]+"' style='display:none'>"+ConstruirTextoCumplidas(dtOperacionesCumplidas.Select("codigo_mecanico='"+dtMecanicos.Rows[i][0]+"'","pdoc_codigo,mord_numeorde ASC"))+"</div>";
				dtFuentePlanning.Rows.Add(dr);
			}
		}

		private string ConstruirTextoIntervalo(string intervalo, DataRow[] draOperacionesMecanico)
		{
			string salida = "<table class='tablewhite2' cellSpacing='0' cellPadding='2' width='100%' border='0'>";
			DateTime fechaInicioIntervalo = Convert.ToDateTime(ddlFechaConsulta.SelectedValue+" "+(intervalo.Split('-'))[0]);
			DateTime fechaFinIntervalo = Convert.ToDateTime(ddlFechaConsulta.SelectedValue+" "+(intervalo.Split('-'))[1]);
			for(int i=0;i<draOperacionesMecanico.Length;i++)
			{
				DateTime fechaInicioOperacion = Convert.ToDateTime(draOperacionesMecanico[i][6]);
				DateTime fechaFinOperacion = Convert.ToDateTime(draOperacionesMecanico[i][7]);
				//Caso 1: Cuando la fecha de inicio y fin de la operacion son el mismo dia de la consulta
				if(fechaInicioOperacion.Date == fechaInicioIntervalo.Date && fechaFinOperacion.Date == fechaInicioIntervalo.Date)
				{
					if((fechaInicioOperacion.TimeOfDay >= fechaInicioIntervalo.TimeOfDay && fechaInicioOperacion.TimeOfDay < fechaFinIntervalo.TimeOfDay) || (fechaInicioOperacion.TimeOfDay <= fechaInicioIntervalo.TimeOfDay && fechaFinOperacion.TimeOfDay >= fechaFinIntervalo.TimeOfDay) || (fechaFinOperacion.TimeOfDay >= fechaInicioIntervalo.TimeOfDay && fechaFinOperacion.TimeOfDay < fechaFinIntervalo.TimeOfDay))
						salida += "<tr><td><span style='cursor:pointer' onclick=\"ConsOT2('"+draOperacionesMecanico[i][2]+"',"+draOperacionesMecanico[i][3]+","+draOperacionesMecanico[i][8]+","+draOperacionesMecanico[i][9]+");\">"+draOperacionesMecanico[i][4]+" - "+draOperacionesMecanico[i][5]+"</span></td></tr>";
				}
					//Caso 2 : Cuando la fecha de inicio de operacion es igual a la consulta y fecha de fin de operacion es mayor
				else if(fechaInicioOperacion.Date == fechaInicioIntervalo.Date && fechaFinOperacion.Date > fechaInicioIntervalo.Date)
				{
					if(fechaInicioOperacion.TimeOfDay <= fechaFinIntervalo.TimeOfDay)
						salida += "<tr><td><span style='cursor:pointer' onclick=\"ConsOT2('"+draOperacionesMecanico[i][2]+"',"+draOperacionesMecanico[i][3]+","+draOperacionesMecanico[i][8]+","+draOperacionesMecanico[i][9]+");\">"+draOperacionesMecanico[i][4]+" - "+draOperacionesMecanico[i][5]+"</span></td></tr>";
				}
				//Caso 3 : Cuando la fecha de inicio de operacion es menor a la consulta y fecha de fin de operacion es igual
				else if(fechaInicioOperacion.Date < fechaInicioIntervalo.Date && fechaFinOperacion.Date == fechaInicioIntervalo.Date)
				{
					if(fechaFinOperacion.TimeOfDay >= fechaFinIntervalo.TimeOfDay)
						salida += "<tr><td><span style='cursor:pointer' onclick=\"ConsOT2('"+draOperacionesMecanico[i][2]+"',"+draOperacionesMecanico[i][3]+","+draOperacionesMecanico[i][8]+","+draOperacionesMecanico[i][9]+");\">"+draOperacionesMecanico[i][4]+" - "+draOperacionesMecanico[i][5]+"</span></td></tr>";
				}
			}
			salida += "</table>";
			return salida;
		}

		private string ConstruirTextoParalizadas(DataRow[] draOperacionesParalizadas)
		{
			string salida = "<table class='tablewhite2' cellSpacing='0' cellPadding='1' width='100%' border='0'>";
			for(int i=0;i<draOperacionesParalizadas.Length;i++)
				salida += "<tr><td><span style='cursor:pointer' onclick=\"ConsOT('"+draOperacionesParalizadas[i][0]+"',"+draOperacionesParalizadas[i][1]+");\">"+draOperacionesParalizadas[i][2]+" - "+draOperacionesParalizadas[i][3]+"</span></td><td><span style='color:#"+UtilitarioPlanning.ReturnRGBParalizado(draOperacionesParalizadas[i][5].ToString())+";font-weight:bold'>"+draOperacionesParalizadas[i][6]+"</span></td></tr>";
			salida += "</table>";
			return salida;
		}

		private string ConstruirTextoCumplidas(DataRow[] draOperacionesCumplidas)
		{
			string salida = "<table class='tablewhite2' cellSpacing='0' cellPadding='1' width='100%' border='0'>";
			for(int i=0;i<draOperacionesCumplidas.Length;i++)
				salida += "<tr><td><span style='cursor:pointer' onclick=\"ConsOT('"+draOperacionesCumplidas[i][0]+"',"+draOperacionesCumplidas[i][1]+");\">"+draOperacionesCumplidas[i][2]+" - "+draOperacionesCumplidas[i][3]+"</span></td><td><span style='color:#"+UtilitarioPlanning.ReturnRGBParalizado(draOperacionesCumplidas[i][5].ToString())+";font-weight:bold'>"+draOperacionesCumplidas[i][6]+"</span></td></tr>";
			salida += "</table>";
			return salida;
		}
		#endregion

		#region Metodos AJAX
		[Ajax.AjaxMethod]
		public string ConsultarInfoOT(string prefijoOT, int numeroOT)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MOR.mnit_nit || ' - ' || MNI.mnit_apellidos || ' ' || MNI.mnit_nombres, MCA.mcat_placa, MOR.mord_entrada, MOR.mord_horaentr, MOR.mord_entregar, MOR.mord_horaentg "+
												    "FROM morden MOR INNER JOIN mnit MNI ON MOR.mnit_nit = MNI.mnit_nit INNER JOIN mcatalogovehiculo MCA ON MOR.pcat_codigo = MCA.pcat_codigo AND MOR.mcat_vin = MCA.mcat_vin "+
													"WHERE MOR.pdoc_codigo = '"+prefijoOT+"' AND MOR.mord_numeorde="+numeroOT);
			string htmlQuery = "<table border=\"0\" class=\"tablewhiteletrawhite\" cellpadding=\"2\" cellspacing=\"0\"><tr><td>INFORMACIÓN ORDEN DE TRABAJO</td><td align=\"right\" valign='top'><img src=\"../img/close.gif\" style=\"cursor:pointer\" onclick=\"t1.Hide(event);\"/></td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Prefijo Orden de Trabajo :</td><td align='right'>"+prefijoOT+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Número Orden de Trabajo :</td><td align='right'>"+numeroOT+"</td></tr>";			
			htmlQuery += "<tr><td width=\"45%\">Cliente :</td><td align='right'>"+ds.Tables[0].Rows[0][0]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Placa :</td><td align='right'>"+ds.Tables[0].Rows[0][1]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Fecha Entrada Orden :</td><td align='right'>"+Convert.ToDateTime(ds.Tables[0].Rows[0][2]).GetDateTimeFormats(new CultureInfo("es-CO"))[7]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Hora Entrada Orden :</td><td align='right'>"+ds.Tables[0].Rows[0][3]+"</td></tr>";
 			htmlQuery += "<tr><td width=\"45%\">Fecha Estimada Salida Orden :</td><td align='right'>"+Convert.ToDateTime(ds.Tables[0].Rows[0][4]).GetDateTimeFormats(new CultureInfo("es-CO"))[7]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Hora Estimada Salida Orden :</td><td align='right'>"+ds.Tables[0].Rows[0][5]+"</td></tr>";
			htmlQuery += "</table>";
			return htmlQuery;
		}

		[Ajax.AjaxMethod]
		public string ConsultarInfoOT2(string prefijoOT, int numeroOT, double duracionOperacion, double tiempoGastado)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MOR.mnit_nit || ' - ' || MNI.mnit_apellidos || ' ' || MNI.mnit_nombres, MCA.mcat_placa, MOR.mord_entrada, MOR.mord_horaentr, MOR.mord_entregar, MOR.mord_horaentg "+
				"FROM morden MOR INNER JOIN mnit MNI ON MOR.mnit_nit = MNI.mnit_nit INNER JOIN mcatalogovehiculo MCA ON MOR.pcat_codigo = MCA.pcat_codigo AND MOR.mcat_vin = MCA.mcat_vin "+
				"WHERE MOR.pdoc_codigo = '"+prefijoOT+"' AND MOR.mord_numeorde="+numeroOT);
			string htmlQuery = "<table border=\"0\" class=\"tablewhiteletrawhite\" cellpadding=\"2\" cellspacing=\"0\"><tr><td>INFORMACIÓN ORDEN DE TRABAJO</td><td align=\"right\" valign='top'><img src=\"../img/close.gif\" style=\"cursor:pointer\" onclick=\"t1.Hide(event);\"/></td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Prefijo Orden de Trabajo :</td><td align='right'>"+prefijoOT+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Número Orden de Trabajo :</td><td align='right'>"+numeroOT+"</td></tr>";			
			htmlQuery += "<tr><td width=\"45%\">Cliente :</td><td align='right'>"+ds.Tables[0].Rows[0][0]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Placa :</td><td align='right'>"+ds.Tables[0].Rows[0][1]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Fecha Entrada Orden :</td><td align='right'>"+Convert.ToDateTime(ds.Tables[0].Rows[0][2]).GetDateTimeFormats(new CultureInfo("es-CO"))[7]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Hora Entrada Orden :</td><td align='right'>"+ds.Tables[0].Rows[0][3]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Fecha Estimada Salida Orden :</td><td align='right'>"+Convert.ToDateTime(ds.Tables[0].Rows[0][4]).GetDateTimeFormats(new CultureInfo("es-CO"))[7]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Hora Estimada Salida Orden :</td><td align='right'>"+ds.Tables[0].Rows[0][5]+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Duración de la Operación :</td><td align='right'>"+duracionOperacion+"</td></tr>";
			htmlQuery += "<tr><td width=\"45%\">Tiempo gastado calculado :</td><td align='right'>"+tiempoGastado+"</td></tr>";
			htmlQuery += "</table>";
			return htmlQuery;
		}
		#endregion

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
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion						
	}
}
