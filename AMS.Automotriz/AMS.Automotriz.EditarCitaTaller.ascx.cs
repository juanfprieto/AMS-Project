
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using Ajax;
using eWorld.UI;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class AMS_Automotriz_EditarCitaTaller : System.Web.UI.UserControl
	{
		#region Propiedades
		protected System.Web.UI.WebControls.Button btnAceptar;
		private DatasToControls bind = new DatasToControls();
        protected System.Web.UI.WebControls.Label fechaCita;
        protected static string nvoRecepcionista;
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Automotriz_EditarCitaTaller));

            if (!IsPostBack)
            {
                bind.PutDatasIntoDropDownList(ddlTaller, "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centtal is not null  or pcen_centcoli is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion");
                BindDdlRecepcionista(ddlTaller.SelectedValue);
                btnAceptarEdicion.Attributes.Add("onclick", "return ValidarProcesoEdicion();");
                //BindDdlHoras(DateTime.Now.ToString("yyyy-MM-dd"), ddlRecepcionista.SelectedValue);
            }
		}

		protected void btnConsultar_Click(object sender, System.EventArgs e)
		{
			ConsultarCitas();
		}

        protected void btnConsultarPlaca_Click(object sender, System.EventArgs e)
        {
            ConsultarCitasPlaca();
        }
        [Ajax.AjaxMethod]
        public string verificar_Almacen(string fecha, string hora, string placa) {
            string nombre = DBFunctions.SingleData("SELECT MCIT_NOMBRE FROM MCITATALLER WHERE MCIT_FECHA= '" + fecha.Substring(0, 10) + "' AND MCIT_HORA = '" + hora + "'AND MCIT_PLACA = '" + placa + "'" );
            string codTaller = DBFunctions.SingleData("SELECT PALM_ALMACEN FROM MCITATALLER WHERE MCIT_FECHA= '" + fecha + "' AND MCIT_HORA = '" + hora + "' AND MCIT_NOMBRE = '" + nombre + "'");
            //tallerAlm = codTaller;
            //string nombreTaller = DBFunctions.SingleData("SELECT palm_descripcion from palmacen where palm_almacen = '" + codTaller + "'");
            return codTaller;
        }
		private void dgCitasDia_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{

               // HtmlInputHidden fecha = 

                DateTime fecCita = Convert.ToDateTime(((HtmlInputHidden)e.Item.Cells[0].FindControl("hdFecCita")).Value);
                //fechaCita.Text = fecCita.ToString("yyyy-MM-dd");
                ((Label)e.Item.Cells[0].FindControl("fechaCita")).Text = fecCita.ToString("yyyy-MM-dd");
                string horaCita = ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdHorCit")).Value;
				string recepcionistaNombre = ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdNombRec")).Value;
               
                // DateTime fechaCita = Convert.ToDateTime(hdFechProc.Value + " " + horaCita);
               

                string a = ((System.Data.DataRowView)(e.Item.DataItem)).Row.ItemArray[11].ToString();             
                if (a == "C")
                {
                    ((Button)e.Item.Cells[0].FindControl("btnEliminar")).Enabled = false;
                    ((HtmlInputButton)e.Item.Cells[0].FindControl("btnEditar")).Disabled = true;
                }
                else
                {
                    //((Button)e.Item.Cells[0].FindControl("btnEliminar")).Attributes.Add("onclick", "return confirm('Esta seguro de eliminar la cita del día " + Convert.ToDateTime(hdFechProc.Value).GetDateTimeFormats(new CultureInfo("es-CO"))[7] + " de la hora " + horaCita + " del recepcionista " + recepcionistaNombre + " ?');");
                    //((HtmlInputButton)e.Item.Cells[0].FindControl("btnEditar")).Attributes.Add("onclick", "CargarFormEdicion('" + hdFechProc.Value + "','" + ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdHorCit")).Value + "','" + ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdCodRcp")).Value + "');");
                    string fechaCita = ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdFecCita")).Value;
                    var fechaCita1 = Convert.ToDateTime(fechaCita).ToString("MM-dd-yyyy");
                    
                    ((Button)e.Item.Cells[0].FindControl("btnEliminar")).Attributes.Add("onclick", "return confirm('Esta seguro de eliminar la cita del día " + Convert.ToDateTime(fechaCita).GetDateTimeFormats(new CultureInfo("es-CO"))[7] + " de la hora " + horaCita + " del recepcionista " + recepcionistaNombre + " ?');");
                    ((HtmlInputButton)e.Item.Cells[0].FindControl("btnEditar")).Attributes.Add("onclick", "CargarFormEdicion('" + fechaCita1 + "','" + ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdHorCit")).Value + "','" + ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdCodRcp")).Value + "','" + ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdPlaca")).Value + "','" + ((HtmlInputHidden)e.Item.Cells[0].FindControl("inptCodVend")).Value + "');");
                }                  
                
			}
		}

		private void dgCitasDia_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName == "delete")
			{
				//DBFunctions.NonQuery("DELETE FROM mcitataller WHERE mcit_fecha='"+Convert.ToDateTime(hdFechProc.Value).ToString("yyyy-MM-dd")+"' AND mcit_hora='"+((HtmlInputHidden)e.Item.Cells[0].FindControl("hdHorCit")).Value+"' AND mcit_codven='"+((HtmlInputHidden)e.Item.Cells[0].FindControl("hdCodRcp")).Value+"'");
                DBFunctions.NonQuery("DELETE FROM mcitataller WHERE mcit_fecha='" + Convert.ToDateTime(((HtmlInputHidden)e.Item.Cells[0].FindControl("hdFecCita")).Value).ToString("yyyy-MM-dd") + "' AND mcit_hora='" + ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdHorCit")).Value + "' AND mcit_codven='" + ((HtmlInputHidden)e.Item.Cells[0].FindControl("hdCodRcp")).Value + "'");
				ConsultarCitas();
			}
		}

		protected void btnAceptarEdicion_Click(object sender, System.EventArgs e)
		{
			DateTime fechaSeleccionada = Convert.ToDateTime(cpFechaNuevaCita.SelectedDate.ToString("yyyy-MM-dd") + " "+ddlHorario.SelectedValue);
			if(fechaSeleccionada <= DateTime.Now)
			{
                Utils.MostrarAlerta(Response, "La fecha seleccionada es menor a la fecha registrada del sistema. Por favor seleccione una fecha superior.");
                ConsultarCitas();
				return;
			}
			else if(ConsultarExistenciaCita(fechaSeleccionada.ToString("dd-MM-yyyy"),ddlHorario.SelectedValue,ddlRecepcionista.SelectedValue))
			{
                Utils.MostrarAlerta(Response, "Este espacio ya se encuentra reservado, por favor seleccione otro");
				ConsultarCitas();
				return;
			}
            var ci = System.Globalization.CultureInfo.GetCultureInfo("en-us");
            DateTime fechaAntiguaDate = DateTime.Parse(hdFechAnt.Value, ci);
            ActualizarCita(fechaAntiguaDate.ToString("yyyy-MM-dd"), hdHorAnt.Value, hdRecAnt.Value, cpFechaNuevaCita.SelectedDate.ToString("yyyy-MM-dd"), ddlHorario.SelectedValue);
            ConsultarCitas();
		}

		#endregion

		#region Metodos

		public void ConsultarCitas()
		{
			//Se genera la consulta las citas programadas para un dia especifico 

            String sql = String.Format("SELECT MCIT.mcit_fecha, " +
             "       MCIT.mcit_hora, " +
             "       MCIT.mcit_codven, " +
             "       PVEN.pven_nombre," +
             "       MCIT.pcat_codigo, " +
             "       PCAT.pcat_descripcion, " +
             "       MCIT.mcit_nombre, " +
             "       MCIT.pkit_codigo, " +
             "       PKI.pkit_nombre, " +
             "       MCIT.mcit_placa, " +
             "       MCIT.mcit_observacion, " +
              "      MCIT.TESTCIT_ESTACITA " +
             "FROM mcitataller MCIT  " +
             "  LEFT JOIN pvendedor PVEN ON MCIT.mcit_codven = PVEN.pven_codigo  " +
             "  LEFT JOIN pcatalogovehiculo PCAT ON MCIT.pcat_codigo = PCAT.pcat_codigo  " +
             "  LEFT JOIN pkit PKI ON MCIT.pkit_codigo = PKI.pkit_codigo " +
             "WHERE mcit_fecha = '{0}' " +
             "ORDER BY MCIT.mcit_hora, " +
             "         PVEN.pven_nombre ASC", cpFechaCita.SelectedDate.ToString("yyyy-MM-dd"));
			DataSet ds = new DataSet();
			ds = DBFunctions.Request(ds,IncludeSchema.NO, sql);
			if(ds.Tables[0].Rows.Count == 0)
			{
				dgCitasDia.DataSource = null;
                Utils.MostrarAlerta(Response, "Este espacio ya se encuentra reservado, por favor seleccione otro");
			}
			else
			{
				dgCitasDia.DataSource = ds.Tables[0];
				hdFechProc.Value = cpFechaCita.SelectedDate.ToString("yyyy-MM-dd");
                BindDdlHoras(cpFechaCita.SelectedDate.ToString("yyyy-MM-dd"), ddlRecepcionista.SelectedValue);

			}
			dgCitasDia.DataBind();
		}

        public void ConsultarCitasPlaca()
        {
            //Se genera la consulta las citas programadas para una PLACA  especifica
            String sql = String.Format("SELECT MCIT.mcit_fecha, " +
             "       MCIT.mcit_hora, " +
             "       MCIT.mcit_codven, " +
             "       PVEN.pven_nombre, " +
             "       MCIT.pcat_codigo, " +
             "       PCAT.pcat_descripcion, " +
             "       MCIT.mcit_nombre, " +
             "       MCIT.pkit_codigo, " +
             "       PKI.pkit_nombre, " +
             "       MCIT.mcit_placa, " +
             "       MCIT.mcit_observacion, " +
              "      MCIT.TESTCIT_ESTACITA " +
             "FROM mcitataller MCIT  " +
             "  LEFT JOIN pvendedor PVEN ON MCIT.mcit_codven = PVEN.pven_codigo  " +
             "  LEFT JOIN pcatalogovehiculo PCAT ON MCIT.pcat_codigo = PCAT.pcat_codigo  " +
             "  LEFT JOIN pkit PKI ON MCIT.pkit_codigo = PKI.pkit_codigo " +
             "WHERE MCIT_PLACA = '" + TextBoxPlaca.Text+ " ' " +
             "ORDER BY MCIT.mcit_fecha desc,"+ cpFechaCita.SelectedDate.ToString("yyyy-MM-dd"));
            
            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                dgCitasDia.DataSource = null;
                Utils.MostrarAlerta(Response, "Este espacio ya se encuentra reservado, por favor seleccione otro");
            }
            else
            {
                dgCitasDia.DataSource = ds.Tables[0];
                hdFechProc.Value = cpFechaCita.SelectedDate.ToString("yyyy-MM-dd");
                BindDdlHoras(cpFechaCita.SelectedDate.ToString(), ddlRecepcionista.SelectedValue);
            }
            dgCitasDia.DataBind();
        }

		protected void BindDdlHoras(string fecha, string codRecep)
		{
            
			DataSet dsIntervalo = ConstruirHorarioAtencionRecepcionista(fecha,codRecep);
			ddlHorario.DataSource = dsIntervalo.Tables[0];
			ddlHorario.DataValueField = dsIntervalo.Tables[0].Columns[0].ColumnName;
			ddlHorario.DataTextField = dsIntervalo.Tables[0].Columns[0].ColumnName;
			ddlHorario.DataBind();
		}

		protected  double CalculoIntervalo(double cantidad)
		{
			double factor = (1/cantidad);
			return (factor*60);
		}

		private void BindDdlRecepcionista(string codigoTaller)
		{
			//bind.PutDatasIntoDropDownList(ddlRecepcionista,"SELECT pven_codigo, pven_nombre FROM pvendedor WHERE tvend_codigo='RT' AND pven_vigencia='V' AND palm_almacen = '"+codigoTaller+"' ORDER BY pven_nombre ASC");
            bind.PutDatasIntoDropDownList(ddlRecepcionista, "SELECT pven_codigo, pven_nombre FROM pvendedor WHERE tvend_codigo='RT' AND pven_vigencia='V' ORDER BY pven_nombre ASC");
        }
		
		private DataTable InitializeDtIntervalo()
		{
			DataTable dtIntervalo = new DataTable();
			dtIntervalo.Columns.Add(new DataColumn("HORA",typeof(string)));
			return dtIntervalo;
		}

		private void ActualizarCita(string fechaAntigua, string horarioAntiguo, string codRecepAntiguo, string fechaNueva, string horarioNuevo)
		{
            string taller = ddlTaller.SelectedValue.ToString();
            int proceso = DBFunctions.NonQuery("UPDATE mcitataller SET mcit_fecha='" + fechaNueva + "', mcit_hora='" + horarioNuevo + "', mcit_codven='" + nvoRecepcionista + "', palm_almacen='" + taller + "' WHERE mcit_fecha='" + fechaAntigua + "' AND mcit_hora='" + horarioAntiguo + "' AND mcit_codven='" + codRecepAntiguo + "'");
		}
        	
		#endregion

		#region Metodos Ajax
		
		[Ajax.AjaxMethod()]
		public DataSet CambioTallerCarga(string idAlmacen)
		{
			DataSet dsConsulta = new DataSet();
            //DBFunctions.Request(dsConsulta,IncludeSchema.NO,"SELECT pven_codigo, pven_nombre FROM pvendedor WHERE tvend_codigo='RT' AND palm_almacen = '"+idAlmacen+"' ORDER BY pven_nombre ASC");
            DBFunctions.Request(dsConsulta, IncludeSchema.NO, "SELECT PVA.pven_codigo, PV.pven_nombre FROM pvendedoralmacen PVA, PVENDEDOR PV WHERE PV.tvend_codigo='RT' AND PVA.palm_almacen = '" + idAlmacen + "' AND PVA.PVEN_CODIGO = PV.PVEN_CODIGO ORDER BY pven_nombre ASC;");
            return dsConsulta;
		}

		[Ajax.AjaxMethod()]
		public DataSet ConstruirHorarioAtencionRecepcionista(string fecha, string codRecep)
		{
            nvoRecepcionista = codRecep;
            DateTime fechaP;
            try
            {
                fechaP = Convert.ToDateTime(fecha);
            }
            catch
            {
                //corregir
                fechaP = Convert.ToDateTime(fecha.Replace("/","-"));
            }
			DataSet intervalo = new DataSet();
			DBFunctions.Request(intervalo,IncludeSchema.NO,"SELECT ctal_hirec, ctal_hfrec, ctal_citaporhora FROM ctaller");
			DateTime inicio = Convert.ToDateTime(intervalo.Tables[0].Rows[0][0].ToString());
			DateTime final = Convert.ToDateTime(intervalo.Tables[0].Rows[0][1].ToString());
			DateTime horAlm = new DateTime();
            try
            {
                horAlm = Convert.ToDateTime(DBFunctions.SingleData("SELECT pven_horalmuerzo FROM pvendedor WHERE pven_codigo='"+codRecep+"'"));
			}
            catch { horAlm = Convert.ToDateTime("12:00:00"); };
            double tamIntervalo = CalculoIntervalo(Convert.ToDouble(intervalo.Tables[0].Rows[0][2]));
			DataTable dtIntervalo = InitializeDtIntervalo();
            
			while(DateTime.Compare(final,inicio)>0)
			{
				try
				{
                    DateTime horFinTem = inicio.AddMinutes(tamIntervalo);
                    if(!DBFunctions.RecordExist("SELECT pcat_codigo FROM mcitataller WHERE mcit_fecha='"+fechaP.ToString("yyyy-MM-dd")+"' AND mcit_hora='"+inicio.TimeOfDay.ToString()+"' AND mcit_codven='"+codRecep+"'") && (inicio<horAlm || inicio>=(horAlm.AddMinutes(60))))
                    {
						DataRow dr = dtIntervalo.NewRow();
						dr[0] = inicio.TimeOfDay.ToString();
						dtIntervalo.Rows.Add(dr);
					}
				}
				catch(Exception ex){string s = ex.ToString();}
				inicio = inicio.AddMinutes(tamIntervalo);
			}
			DataSet dsReturn = new DataSet();
			dsReturn.Tables.Add(dtIntervalo);
			return dsReturn;
		}

		[Ajax.AjaxMethod()]
		public bool ConsultaFechaHoraVsSistema(string fecha, string hora)
		{
			DateTime fechHorSel = Convert.ToDateTime(fecha+" "+hora,new CultureInfo("es-CO"));
			if(fechHorSel <= DateTime.Now)
				return false;
			else
				return true;
		}

		[Ajax.AjaxMethod()]
		public bool ConsultarExistenciaCita(string fecha, string hora, string codRecep)
		{
			DateTime fechHorSel = Convert.ToDateTime(fecha+" "+hora,new CultureInfo("es-CO"));
            //ddlHorario.SelectedValue = hora;
			return DBFunctions.RecordExist("SELECT mcit_placa FROM mcitataller WHERE mcit_fecha='"+fechHorSel.ToString("yyyy-MM-dd")+"' AND mcit_hora='"+hora+"' AND mcit_codven='"+codRecep+"'");
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
			this.dgCitasDia.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgCitasDia_ItemCommand);
			this.dgCitasDia.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgCitasDia_ItemDataBound);

		}
		#endregion
	}
}
