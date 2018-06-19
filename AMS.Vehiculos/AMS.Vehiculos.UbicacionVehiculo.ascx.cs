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
using AMS.Documentos;

namespace AMS.Vehiculos
{
	public partial class UbicacionVehiculo : System.Web.UI.UserControl
	{
        protected bool tieneUbicacion = false;
        protected FormatosDocumentos formatoFactura;
        protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
                //	bind.PutDatasIntoDropDownList(catalogo,CatalogoVehiculos.CATALOGOVEHICULOSENALMACEN);
                //    catalogo.Items.Insert(0, new ListItem("Seleccione el Catálogo...", ""));

                try
                {
                    bind.PutDatasIntoDropDownList(prefijo, "SELECT PDOC_CODIGO,PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'MV'");
                    if(prefijo.Items.Count > 0)
                        prefijo.Items.Insert(0, new ListItem("Seleccione el Prefijo...", ""));
                }
                catch
                {
                    Utils.MostrarAlerta(Response, "No ha parametrizado ningun documento del tipo MV");
                }

                bind.PutDatasIntoDropDownList(vinVehiculo,string.Format(Vehiculos.VEHICULOSENALMACEN));
                vinVehiculo.Items.Insert(0, new ListItem("Seleccione el V I N...", ""));

                bind.PutDatasIntoDropDownList(ubicacionVehiculo, "SELECT pubi_codigo, pubi_nombre FROM pubicacion where PUBI_VIGENCIA = 'V' ");
                ubicacionVehiculo.Items.Insert(0, new ListItem("Seleccione la Ubicación...", ""));

                calendar.Text = DateTime.Now.ToString("yyyy-MM-dd");
			//	this.Cargar_Ubicacion_Actual(vinVehiculo.SelectedItem.Text);

                //if (Session["actualizado"] != null)
                if (Request.QueryString["vin"] != null)
                {
                    Utils.MostrarAlerta(Response, "Actualización completa! La nueva ubicación del vehículo: " + Request.QueryString["vin"] + " se ha definido en: " + Request.QueryString["ubicacion"]);
                    Utils.MostrarAlerta(Response, "Se ha creado la factura con prefijo " + Request.QueryString["prefijo"] + " y número " + Request.QueryString["numero"] + "");
                    formatoFactura = new FormatosDocumentos();                    
                    try
                    {
                        formatoFactura.Prefijo = Request.QueryString["prefijo"];
                        formatoFactura.Numero = Convert.ToInt32(Request.QueryString["numero"]);
                        formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefijo"] + "'");
                        if (formatoFactura.Codigo != string.Empty)
                        {
                            if (formatoFactura.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                        }
                        formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefijo"] + "'");
                        if (formatoFactura.Codigo != string.Empty)
                        {
                            if (formatoFactura.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=500,WIDTH=700');</script>");
                        }
                    }
                    catch
                    {
                        lbInfo.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                    }
                    Session.Clear();
                }
			}
		}

        protected void Cargar_Numero(Object sender, EventArgs e)
        {
            txtNumero.Text = DBFunctions.SingleData("select PDOC_ULTIDOCU from PDOCUMENTO WHERE PDOC_CODIGO = '"+prefijo.SelectedValue+"'");
        }
        protected void Cambio_Catalogo(Object sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(vinVehiculo,string.Format(Vehiculos.VEHICULOSENALMACEN));
			this.Cargar_Ubicacion_Actual(vinVehiculo.SelectedItem.Text);
		}
		
		protected void Cambio_VIN(Object sender, EventArgs e)
		{
			this.Cargar_Ubicacion_Actual(vinVehiculo.SelectedItem.Text);
		}
		 
		protected void Confirmar_Ubicacion(Object sender, EventArgs e)
		{
            if (nombreAutoriza.Text != "" && nombreTransporta.Text != "")
            {
                //Primero verificamos si se ha elegido algun vehiculo para la ubicacion
                if (vinVehiculo.Items.Count > 0)
                {
                    //Ahora creamos el registro dentro de la tabla mubicacionvehiculo
                    //DBFunctions.NonQuery("INSERT INTO mubicacionvehiculo VALUES(default,'"+catalogo.SelectedValue+"','"+vinVehiculo.SelectedItem.Text+"','"+ubicacionVehiculo.SelectedValue+"','"+calendar.Text+"','"+nombreAutoriza.Text+"','"+nombreTransporta.Text+"')");
                    bool tieneUbicacion = Convert.ToBoolean(ViewState["tieneUbicacion"]);
                    string [] vin = vinVehiculo.SelectedItem.Text.Split(' ');
                    if (tieneUbicacion)
                    {
                        DBFunctions.NonQuery("UPDATE MUBICACIONVEHICULO SET PUBI_CODIGO='" + ubicacionVehiculo.SelectedValue + "', MUBI_FECHPROC='" + calendar.Text + "', MUBI_NOMBAUTOR='" + nombreAutoriza.Text + "', MUBI_NOMBTRANS='" + nombreTransporta.Text + "' " +
                                        "WHERE mcat_vin='" + vin[0] + "';");
                        DBFunctions.NonQuery("UPDATE pdocumento SET pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo= '" + prefijo.SelectedValue + "'");
                    }
                    else
                    {
                        DBFunctions.NonQuery("INSERT INTO MUBICACIONVEHICULO VALUES(DEFAULT,'','" + vin [0] + "','" + ubicacionVehiculo.SelectedValue + "'," +
                                             "'" + calendar.Text + "','" + nombreAutoriza.Text + "','" + nombreTransporta.Text + "', '"+prefijo.SelectedValue+"',"+txtNumero.Text+");");
                        DBFunctions.NonQuery("UPDATE pdocumento SET pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo= '" + prefijo.SelectedValue + "'");
                    }
                    
                    // AQUI debe llamar el FORMATO A IMPRIMIR, lo ideal es que se pueda ver en ese formato la ubicacion original (antigua) y la nueva ubicacion (a la cual se va llevar el vehiculo)
                    //Session["actualizado"] = 1;
                    Response.Redirect("" + ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Vehiculos.UbicacionVehiculo&vin=" + vin[0] + "&ubicacion=" + ubicacionVehiculo.SelectedItem + "&prefijo=" +prefijo.SelectedValue + "&numero=" +txtNumero.Text );
                }
                else
                    Utils.MostrarAlerta(Response, "No se ha seleccionado ningún vehiculo");
            }
            else 
            {
                Utils.MostrarAlerta(Response, "Verifique los campos no pueden ir vacios");
            }
		}
		
		protected void Cargar_Ubicacion_Actual(string vinS)
		{
            string[] vin = vinS.Split(' ');
			//la ubicacion actual es la del ultimo id de este vehiculo seleccionado si no existe simplemente no se coloca
            ubicacionActual.Text = DBFunctions.SingleData(@"select  p.pubi_nombre || '  ' || mubi_nombautor || '  ' || mubi_nombtrans || '  ' || cast(mubi_fechproc as char(10))
                                                            from mubicacionVEHICULO m, pubicacion p
                                                            where mcat_vin = '"+ vin [0] +@"' and m.pubi_codigo = p.pubi_codigo
                                                            order by mubi_codigo desc 
                                                            fetch first rows only ;");
            /*
            if (DBFunctions.RecordExist("SELECT * FROM mubicacionvehiculo WHERE mcat_vin='" + vin[0] + "'"))
            {
                DatasToControls.EstablecerDefectoDropDownList(ubicacionVehiculo, DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacion WHERE pubi_codigo='" + DBFunctions.SingleData("SELECT pubi_codigo FROM mubicacionvehiculo WHERE mcat_vin='" + vinS + "' ORDER BY mubi_codigo DESC") + "'"));
                nombreAutoriza.Text = DBFunctions.SingleData("SELECT mubi_nombautor FROM mubicacionvehiculo WHERE mcat_vin='" + vinS + "' ORDER BY mubi_codigo DESC");
                nombreTransporta.Text = DBFunctions.SingleData("SELECT mubi_nombtrans FROM mubicacionvehiculo WHERE mcat_vin='" + vinS + "' ORDER BY mubi_codigo DESC");
                calendar.Text = Convert.ToDateTime(DBFunctions.SingleData("SELECT mubi_fechproc FROM mubicacionvehiculo WHERE mcat_vin='" + vinS + "' ORDER BY mubi_codigo DESC")).ToString("yyyy-MM-dd");
                ViewState["tieneUbicacion"] = true;
            }
            else 
           */
            {
                ViewState["tieneUbicacion"] = false;
                calendar.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
		}
		
		////////////////////////////////////////////////
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
