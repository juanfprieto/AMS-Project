// created on 15/03/2005 at 13:30
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
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class PostVenta : System.Web.UI.UserControl
	{
		protected DataTable tablaProcesos;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(proceso,"SELECT ppos_codigo, ppos_descripcion FROM ppostventavehiculo");
				bind.PutDatasIntoDropDownList(catalogo,"SELECT pcat_codigo FROM pcatalogovehiculo");
                bind.PutDatasIntoDropDownList(vinVehiculo, "SELECT mc.mcat_vin " +
                                         "FROM mcatalogovehiculo mc  " +
                                         "  INNER JOIN mvehiculo mv ON mc.mcat_vin = mv.mcat_vin " +
                                         "WHERE mc.pcat_codigo = '" + catalogo.SelectedValue + "' " +
                                         "AND   mv.test_tipoesta = 60");
				bind.PutDatasIntoDropDownList(estado,"SELECT test_codigo,test_descripcion FROM testadopostventa");
				fechaProceso.SelectedDate = DateTime.Now;
			}
		}
		
		protected void Cambio_Tipo_Documento(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(vinVehiculo, "SELECT mv.mcat_vin FROM mvehiculo mv, mcatalogovehiculo mc WHERE pcat_codigo='" + catalogo.SelectedValue + "' AND mv.test_tipoesta=60 and mv.mcat_vin = mc.mcat_vin");
		}
		
		protected void Realizar_Proceso(Object  Sender, EventArgs e)
		{
			if((vinVehiculo.Items.Count>0)&&(proceso.Items.Count>0)&&(observaciones.Text!=""))
			{
				if(DBFunctions.RecordExist("SELECT * FROM mpostventavehiculo WHERE pcat_codigo='"+catalogo.SelectedValue+"' AND mcat_vin='"+vinVehiculo.SelectedValue+"' AND ppos_codigo="+proceso.SelectedValue+""))
                    Utils.MostrarAlerta(Response, "Este proceso ya fue realizado para este vehiculo");
				else
					DBFunctions.NonQuery("INSERT INTO mpostventavehiculo VALUES('"+catalogo.SelectedValue+"','"+vinVehiculo.SelectedValue+"',"+proceso.SelectedValue+",'"+fechaProceso.SelectedDate.Date.ToString("yyyy-MM-dd")+"','"+observaciones.Text+"',"+estado.SelectedValue+")");
				//Ahora mostramos en la grilla todas las acciones de postventa y decimos cuales han sido realizadas y cuales no
				this.Llenar_Tabla_Procesos(catalogo.SelectedItem.ToString(),vinVehiculo.SelectedItem.ToString());
				
			}
			else
            Utils.MostrarAlerta(Response, "No hay ningun vehiculo o proceso seleccionado, o observaciones vacias");
		}
		
		protected void Volver(Object  Sender, EventArgs e)
		{
			string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
			Response.Redirect("" + indexPage + "?process=Vehiculos.PostVenta");
		}
		
		protected void Preparar_Tabla_Proceso()
		{
			tablaProcesos = new DataTable();
			tablaProcesos.Columns.Add(new DataColumn("CODIGOPROCESO",System.Type.GetType("System.String")));
			tablaProcesos.Columns.Add(new DataColumn("DESCPROCESO",System.Type.GetType("System.String")));
			tablaProcesos.Columns.Add(new DataColumn("ESTADO",System.Type.GetType("System.String")));
			tablaProcesos.Columns.Add(new DataColumn("OBSERVACION",System.Type.GetType("System.String")));
			tablaProcesos.Columns.Add(new DataColumn("FECHA",System.Type.GetType("System.String")));
		}
		
		protected void Llenar_Tabla_Procesos(string catalogoSeleccionado, string vinSeleccionado)
		{
			this.Preparar_Tabla_Proceso();
			//Tomamos el kilometraje promedio y lo dividimos en 365 dandonos un kilometraje diario
			double kilometrajeDiario = System.Convert.ToDouble(ConfigurationManager.AppSettings["KilometrajePromedioAno"])/365;
			//Traemos la fecha de entrega del vehiculo
			DataSet listaProcesos = new DataSet();
			DBFunctions.Request(listaProcesos,IncludeSchema.NO,"SELECT ppos_codigo, ppos_descripcion, ppos_kilometraje FROM ppostventavehiculo");
			for(int i=0;i<listaProcesos.Tables[0].Rows.Count;i++)
			{
				DateTime fechaEntrega = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mveh_fechentr FROM mvehiculo WHERE mcat_vin='"+vinSeleccionado+"' AND pcat_codigo='"+catalogoSeleccionado+"'"));
				DataRow fila = tablaProcesos.NewRow();
				fila["CODIGOPROCESO"] = listaProcesos.Tables[0].Rows[i][0].ToString();
				fila["DESCPROCESO"] = listaProcesos.Tables[0].Rows[i][1].ToString();
				if(DBFunctions.RecordExist("SELECT * FROM mpostventavehiculo WHERE pcat_codigo='"+catalogoSeleccionado+"' AND mcat_vin='"+vinSeleccionado+"' AND ppos_codigo="+listaProcesos.Tables[0].Rows[i][0].ToString()+""))
				{
					fila["ESTADO"] = DBFunctions.SingleData("SELECT test_descripcion FROM testadopostventa WHERE test_codigo=(SELECT test_codigo FROM mpostventavehiculo WHERE pcat_codigo='"+catalogoSeleccionado+"' AND mcat_vin='"+vinSeleccionado+"' AND ppos_codigo="+listaProcesos.Tables[0].Rows[i][0].ToString()+")");
					fila["OBSERVACION"] = DBFunctions.SingleData("SELECT mpos_obsr FROM mpostventavehiculo WHERE pcat_codigo='"+catalogoSeleccionado+"' AND mcat_vin='"+vinSeleccionado+"' AND ppos_codigo="+listaProcesos.Tables[0].Rows[i][0].ToString()+"");
					fila["FECHA"] = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mpos_fecha FROM mpostventavehiculo WHERE pcat_codigo='"+catalogoSeleccionado+"' AND mcat_vin='"+vinSeleccionado+"' AND ppos_codigo="+listaProcesos.Tables[0].Rows[i][0].ToString()+"")).Date.ToString("yyyy-MM-dd");
				}
				else
				{
					fila["ESTADO"] = "Aun No Realizada";
					fila["OBSERVACION"] = "";
					//Ahora dividimos el kilometraje del proceso por el diario y nos da la cantidad de dias;
					double cantidadDias = System.Convert.ToDouble(listaProcesos.Tables[0].Rows[i][2].ToString())/kilometrajeDiario;
					fila["FECHA"] = fechaEntrega.AddDays(cantidadDias).Date.ToString("yyyy-MM-dd");
				}
				tablaProcesos.Rows.Add(fila);				
			}
			procesosPostVenta.DataSource = tablaProcesos;
			procesosPostVenta.DataBind();
			btnVolver.Visible = true;
			controls.Visible = false;
			lbInfo.Text = "Catalogo :"+catalogoSeleccionado+"<br>VIN : "+vinSeleccionado+"<br>Placa : "+DBFunctions.SingleData("SELECT mcat_placa FROM mcatalogovehiculo WHERE pcat_codigo='"+catalogoSeleccionado+"' AND mcat_vin='"+vinSeleccionado+"'");
		}
		
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


