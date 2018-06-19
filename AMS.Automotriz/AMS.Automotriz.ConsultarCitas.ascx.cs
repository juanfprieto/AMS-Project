// created on 02/12/2004 at 12:14

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

	public partial class ConsultarCitas : System.Web.UI.UserControl
	{
		protected DataSet citasAnteriores, citasPosteriores;
		protected DataTable tablaCitasAnteriores;
		
		protected void Realizar_Consulta(Object  Sender, EventArgs e)
		{
			if(placa.Text!="")
			{
				resultadoConsulta.Controls.Clear();
				Label citaActual = new Label();
				Label labelHistorial = new Label();
				Label informacionAdicional = new Label();
				DataGrid historialCitas = new DataGrid();
				resultadoConsulta.Controls.Add(citaActual);
				resultadoConsulta.Controls.Add(new LiteralControl("<br><br>"));
				resultadoConsulta.Controls.Add(informacionAdicional);
				resultadoConsulta.Controls.Add(new LiteralControl("<br>"));
				resultadoConsulta.Controls.Add(historialCitas);
				resultadoConsulta.Controls.Add(new LiteralControl("<br>"));
				resultadoConsulta.Controls.Add(labelHistorial);
				informacionAdicional.Text = "<center>HISTORIAL DE CITAS</center>";
				DateTime fechaMomento = DateTime.Now;
				this.Consultar_Proxima_Cita(citaActual,fechaMomento);

				if(this.LLenar_Grilla_Historial_Citas(historialCitas,fechaMomento,labelHistorial))
				{
					//System.Web.UI.WebControls.Image imagenEst= new System.Web.UI.WebControls.Image();
					//imagenEst = Graficos.Generador_Grafico(imagenEst,"Estadística citas",TipoGraficos.PIE,"","","serie,"+this.Crear_Categoria(),"datos,"+this.Traer_Datos());
					resultadoConsulta.Controls.Add(new LiteralControl("<br><br><br>"));
					//resultadoConsulta.Controls.Add(imagenEst);					
					lb.Text = Graficos.exceptions;
				}
			}
		}
		
		protected void Preparar_Grilla_Consulta_Citas()
		{
			tablaCitasAnteriores = new DataTable();
			tablaCitasAnteriores.Columns.Add(new DataColumn("SEDE",System.Type.GetType("System.String")));
			tablaCitasAnteriores.Columns.Add(new DataColumn("RECEPCIONISTA",System.Type.GetType("System.String")));
			tablaCitasAnteriores.Columns.Add(new DataColumn("FECHA",System.Type.GetType("System.String")));
			tablaCitasAnteriores.Columns.Add(new DataColumn("HORA",System.Type.GetType("System.String")));
			tablaCitasAnteriores.Columns.Add(new DataColumn("SERVICIO",System.Type.GetType("System.String")));
			tablaCitasAnteriores.Columns.Add(new DataColumn("ESTADO DE LA CITA",System.Type.GetType("System.String")));			
		}
		
		protected bool LLenar_Grilla_Historial_Citas(DataGrid grilla, DateTime fechaMomento, Label labelHistorial)
		{
			int i;
			bool retorno = false;
			citasAnteriores = new DataSet();
			this.Preparar_Grilla_Consulta_Citas();
			//DBFunctions.Request(citasAnteriores,IncludeSchema.NO,"SELECT mcit_fecha, mcit_hora, mcit_codven, pkit_codigo, testcit_estacita FROM mcitataller WHERE mcit_fecha<'"+fechaMomento.Date.ToString("yyyy-MM-dd")+"' AND mcit_hora < '"+(fechaMomento.TimeOfDay.ToString()).Substring(0,8)+"' AND mcit_placa ='"+placa.Text+"'");
			DBFunctions.Request(citasAnteriores,IncludeSchema.NO,"SELECT mcit_fecha, mcit_hora, mcit_codven, pkit_codigo, testcit_estacita FROM mcitataller WHERE mcit_fecha<'"+fechaMomento.Date.ToString("yyyy-MM-dd")+"' AND mcit_placa ='"+placa.Text+"'");
			for(i=0;i<citasAnteriores.Tables[0].Rows.Count;i++)
			{
				retorno = true;
				DataRow fila = tablaCitasAnteriores.NewRow();
                fila["SEDE"] = DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE TVIG_VIGENCIA='V' and palm_almacen=(SELECT palm_almacen FROM pvendedor WHERE pven_codigo='" + citasAnteriores.Tables[0].Rows[i][2].ToString() + "')");
				fila["RECEPCIONISTA"] = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+citasAnteriores.Tables[0].Rows[i][2].ToString()+"'");
				fila["FECHA"] = (System.Convert.ToDateTime(citasAnteriores.Tables[0].Rows[i][0].ToString())).Date.ToString("yyyy-MM-dd");
				fila["HORA"] = citasAnteriores.Tables[0].Rows[i][1].ToString();
                fila["SERVICIO"] = DBFunctions.SingleData("SELECT pkit_nombre FROM pkit WHERE PKIT_VIGENCIA = 'V' and pkit_codigo='" + citasAnteriores.Tables[0].Rows[i][3].ToString() + "'");
				fila["ESTADO DE LA CITA"] = DBFunctions.SingleData("SELECT testcit_desccita FROM testadocita WHERE testcit_estacita='"+citasAnteriores.Tables[0].Rows[i][4].ToString()+"'");
				tablaCitasAnteriores.Rows.Add(fila);
			}
			grilla.DataSource = tablaCitasAnteriores;
			grilla.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(grilla);
			labelHistorial.Text = "Su carro ha tenido un total de "+citasAnteriores.Tables[0].Rows.Count.ToString()+" en nuestros talleres";
			return retorno;
		}
		
		protected void Consultar_Proxima_Cita(Label citaActual, DateTime fechaMomento)
		{
			citasPosteriores = new DataSet();
			//DBFunctions.Request(citasPosteriores,IncludeSchema.NO,"SELECT mcit_fecha, mcit_hora, mcit_codven, pkit_codigo, testcit_estacita FROM mcitataller WHERE mcit_fecha>='"+fechaMomento.Date.ToString("yyyy-MM-dd")+"' AND mcit_hora>='"+(fechaMomento.TimeOfDay.ToString()).Substring(0,8)+"' AND mcit_placa ='"+placa.Text+"' ORDER BY mcit_fecha ASC");
			DBFunctions.Request(citasPosteriores,IncludeSchema.NO,"SELECT mcit_fecha, mcit_hora, mcit_codven, pkit_codigo, testcit_estacita FROM mcitataller WHERE mcit_fecha>='"+fechaMomento.Date.ToString("yyyy-MM-dd")+"' AND mcit_placa ='"+placa.Text+"' ORDER BY mcit_fecha ASC");
			if(citasPosteriores.Tables[0].Rows.Count==0)
				citaActual.Text = "Para este vehiculo no existen citas programadas";
			else
			{
				citaActual.Text = "La proxima cita que tiene este vehiculo es el "+(System.Convert.ToDateTime(citasPosteriores.Tables[0].Rows[0][0].ToString())).Date.ToString("yyyy-MM-dd")+" a las "+citasPosteriores.Tables[0].Rows[0][1].ToString();
                citaActual.Text += "<br>en la sede " + DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE TVIG_VIGENCIA='V' and palm_almacen=(SELECT palm_almacen FROM pvendedor WHERE pven_codigo='" + citasPosteriores.Tables[0].Rows[0][2].ToString() + "')");
				citaActual.Text += " y sera antendido por "+DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+citasPosteriores.Tables[0].Rows[0][2].ToString()+"'");
				//Ahora calculamos la diferencia de tiempo				
				DateTime fechaCita = System.Convert.ToDateTime((System.Convert.ToDateTime(citasPosteriores.Tables[0].Rows[0][0].ToString())).Date.ToString("yyyy-MM-dd")+" "+citasPosteriores.Tables[0].Rows[0][1].ToString());
				citaActual.Text += "<br>Tiempo Restante para su cita : "+(fechaCita - fechaMomento).Days.ToString()+" dias con "+(fechaCita - fechaMomento).Hours.ToString()+" horas";
			}
		}
		
		protected string Crear_Categoria()
		{
			string salida = "";
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM testadocita");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				salida += ds.Tables[0].Rows[i][1].ToString()+'\t';
			return salida;		
		}
		
		protected string Traer_Datos()
		{
			string salida = "";
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM testadocita");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				salida += DBFunctions.SingleData("SELECT COUNT(*) FROM mcitataller WHERE mcit_fecha<'"+DateTime.Now.Date.ToString("yyyy-MM-dd")+"' AND mcit_placa ='"+placa.Text+"' AND testcit_estacita='"+ds.Tables[0].Rows[i][0].ToString()+"'")+'\t';
			return salida;
		}
		
	}
}
