// created on 24/11/2004 at 17:52
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Mail;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;
using System.Collections.Generic;

namespace AMS.Marketing
{
	public partial class SeguimientoOrdenes : System.Web.UI.UserControl
	{
		protected string prefijoOrdenSeleccionada, numeroOrdenSeleccionada;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected DataTable tablaAcciones;
        protected AMS_Tools_Email miMail = new AMS_Tools_Email();

        protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
                if(Request.QueryString["procTer"] =="1")
                {
                    Utils.MostrarAlerta(Response, "Se ha guardado la acción y se ha enviado el informe al correo");
                }
                //string[] filePaths = Directory.GetFiles(Server.MapPath("~/Img/"));
                //List<ListItem> files = new List<ListItem>();
                //foreach (string filePath in filePaths)
                //{
                //    string fileName = Path.GetFileName(filePath);
                //    files.Add(new ListItem(fileName, "~/Img/" + fileName));
                //}
                //GridView1.DataSource = files;
                //GridView1.DataBind();


                DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(tipoDocumento,"SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='OT'");
                Utils.llenarPrefijos(Response, ref tipoDocumento , "%", "%", "OT");
				bind.PutDatasIntoDropDownList(numeroOrden,"SELECT mord_numeorde FROM morden WHERE pdoc_codigo='"+tipoDocumento.SelectedValue+"' and test_estado = 'A' ");				
				Session.Clear();
				Session["prefijoOrdenSeleccionada"] = "";
				Session["numeroOrdenSeleccionada"] = numeroOrdenSeleccionada;
			}
			if(Session["prefijoOrdenSeleccionada"]==null)
				prefijoOrdenSeleccionada = "";
			else if(Session["prefijoOrdenSeleccionada"]!=null)
				prefijoOrdenSeleccionada = Session["prefijoOrdenSeleccionada"].ToString();
			if(Session["numeroOrdenSeleccionada"]==null)
				numeroOrdenSeleccionada = "";
			else if(Session["numeroOrdenSeleccionada"]!=null)
				numeroOrdenSeleccionada = Session["numeroOrdenSeleccionada"].ToString();
		}

        protected void habilitaCorreo(object sender, EventArgs e)
        {
            if (chkCorreo.Checked == true)
            {
                fileUpload1.Visible = true;
                lbCorreo.Visible = true;
                txtCorreo.Visible = true;
            }
            else
            {
                lbCorreo.Visible = false;
                fileUpload1.Visible = false;
                txtCorreo.Visible = false;
            }
                
        }


        protected void Confirmar_Orden(Object  Sender, EventArgs e)
		{
			prefijoOrdenSeleccionada = tipoDocumento.SelectedValue;
			Session["prefijoOrdenSeleccionada"] = prefijoOrdenSeleccionada;
			numeroOrdenSeleccionada = numeroOrden.SelectedValue;
			Session["numeroOrdenSeleccionada"] = numeroOrdenSeleccionada;
			string nitPropietarioCarro = DBFunctions.SingleData("SELECT mnit_nit FROM morden WHERE pdoc_codigo='"+prefijoOrdenSeleccionada+"' AND mord_numeorde="+numeroOrdenSeleccionada+"");
			labelPropietario.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+nitPropietarioCarro+"'");
            txtCorreo.Text = DBFunctions.SingleData("SELECT mnit_EMAIL FROM mnit WHERE mnit_nit='" + nitPropietarioCarro + "';");
            this.Llenar_Tabla_Acciones(prefijoOrdenSeleccionada,numeroOrdenSeleccionada);
			tipoActividad.Visible = true;
			resultadoActividad.Visible = true;
			labelTipoActividad.Visible = true;
			labelResultadoAccion.Visible = true;
			labelDetalleAccion.Visible = true;
			labelEjecutor.Visible = true;
			labelFechaAccion.Visible = true;
			detalleAccion.Visible = true;
			ejecutor.Visible = true;
			calendario.Visible = true;
			grabarAccion.Visible = true;
            //fileUpload1.Visible = true;
            fldContent.Visible = true;
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(tipoActividad,"SELECT pact_nombmark FROM pactividadmarketing");
			bind.PutDatasIntoDropDownList(resultadoActividad,"SELECT pres_descripcion FROM presultadoactividad");
			calendario.SelectedDate = DateTime.Now;
		}
        protected void Grabar_Accion(Object  Sender, EventArgs e)
		{
            DataSet dsDatos = new DataSet();
            bool proceso = true;
            ArrayList imagenes = new ArrayList();
            ArrayList nombres = new ArrayList();
            int numImagenes = fileUpload1.PostedFiles.Count;

            string codigoAccion = DBFunctions.SingleData("SELECT pact_codimark FROM pactividadmarketing WHERE pact_nombmark='" + tipoActividad.SelectedItem.ToString() + "'");
            if ((detalleAccion.Text == "") || (ejecutor.Text == ""))
            {
                Utils.MostrarAlerta(Response, "Algun Campo Falto por Llenar");
                return;
            }
            else
            {
                if (DBFunctions.RecordExist("SELECT * FROM dordenactividad WHERE pdoc_codigo='" + prefijoOrdenSeleccionada + "' AND mord_numeorde=" + numeroOrdenSeleccionada + " AND pact_codimark='" + codigoAccion + "'"))
                {
                    Utils.MostrarAlerta(Response, "Esta Acción ya fue grabada HOY para esta Orden");
                    return;
                }
                if (chkCorreo.Checked == true && txtCorreo.Text == "")
                {
                    Utils.MostrarAlerta(Response, "llene el campo del correo");
                    return;
                }
                ArrayList sqlStrings = new ArrayList();
                sqlStrings.Add("INSERT INTO dordenactividad VALUES('" + prefijoOrdenSeleccionada + "'," + numeroOrdenSeleccionada + ",'" + codigoAccion + "',DEFAULT,'" + DBFunctions.SingleData("SELECT pres_codigo FROM presultadoactividad WHERE pres_descripcion='" + resultadoActividad.SelectedItem.ToString() + "'") + "','" + detalleAccion.Text + "','" + calendario.SelectedDate.ToString("yyyy-MM-dd") + "','" + ejecutor.Text + "')");
                if (DBFunctions.Transaction(sqlStrings))
                {
                    proceso = true;
                }
                else
                {
                    lb.Text += DBFunctions.exceptions;
                    return;
                }
                    
            }
            if (proceso == true && chkCorreo.Checked == true)
            {
                string nit = DBFunctions.SingleData("select MNIT_NIT from morden WHERE PDOC_CODIGO = '" + tipoDocumento.SelectedValue + "' AND MORD_NUMEORDE = " + numeroOrden.SelectedValue + ";");
                string vin = DBFunctions.SingleData("select MCAT_VIN from morden WHERE PDOC_CODIGO = '" + tipoDocumento.SelectedValue + "' AND MORD_NUMEORDE = " + numeroOrden.SelectedValue + ";");
                string nombre = DBFunctions.SingleData("select nombre from vmnit where mnit_nit = '" + nit + "'");
                string jefeTaller = DBFunctions.SingleData("select ctal_nombgere from ctaller;");
                string empresa = DBFunctions.SingleData("SELECT cemp_nombre FROM CEMPRESA;");
                DBFunctions.Request(dsDatos, IncludeSchema.NO, "SELECT MCV.PCAT_CODIGO, PCV.PCAT_DESCRIPCION, MCV.MCAT_PLACA, MCV.MCAT_ANOMODE " + 
                                                                "FROM MCATALOGOVEHICULO MCV, PCATALOGOVEHICULO PCV " + 
                                                                "WHERE MCV.PCAT_CODIGO = PCV.PCAT_CODIGO AND MNIT_NIT = '" + nit + "' AND MCAT_VIN = '" + vin + "';");
                string mensajeImg =
                    @"
                    <div style = 'width: 95%; height: 100%; z-index: 6; background-color: white;' >
                        <div style = 'position: relative; background-color:#EEEFD9;width:95%;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888; display: block;' >
                            <img style = 'width: 15%; position: absolute; right: 2%;' src = '../img/AMS.LogoEmpresa.PNG' />
                            <b ><font size = '5' > Correo Generado:</font ></b >
                            <br > Seguimiento a órdenes de Taller <br ><br >
                            <b > Señor: </b ><br >" +
                            nombre + "<br >" +
                            @"<b > Reciba un cordial saludo </b >, <br >
                            Ha recibido un correo con imágenes adjuntas usando el sistema eCas <br >
                            Dichas imágenes se encuentran disponibles como archivos adjuntos en este correo.
                            <br>
                            <div style = ' margin: auto; position: relative;width: auto;background-color: gainsboro;box-shadow: 2px 2px 2px dimgrey; margin-left: auto;margin-right: auto;' >
                                <p style = 'font-size: large;font-family: sans-serif;' > 
                                    Su vehículo: " + dsDatos.Tables[0].Rows[0]["PCAT_DESCRIPCION"] + ", de placa: " + dsDatos.Tables[0].Rows[0]["MCAT_PLACA"] + 
                                    ", Año modelo: " + dsDatos.Tables[0].Rows[0]["MCAT_ANOMODE"] + @" que se encuentra en servicio en nuestro taller
                                    bajo la órden [" + tipoDocumento.SelectedValue + " - " + numeroOrden.SelectedValue + "], presenta el siguiente detalle: <br /> <i> " +
                                   detalleAccion.Text +
                                @" </i></p >
                            </div >
                            <b> Gracias por su atención.</b >
                            <br>
                            <i> Sistemas eCAS - AMS.</i > <br>
                            ATT: <b>" + jefeTaller + " </b> <br> <b>" +
                            empresa + @"</b>
                        </div >
                    </div > ";

                foreach(HttpPostedFile f in fileUpload1.PostedFiles)
                {

                    BinaryReader b = new BinaryReader(f.InputStream);
                    byte[] pp = fileUpload1.FileBytes;
                    //sacamos lectura de bytes
                    byte[] buffer = b.ReadBytes(f.ContentLength);
                    //En la lista agregamos la lista de bytes 
                    imagenes.Add(buffer);
                    nombres.Add(f.FileName);
                }
                try
                {
                    miMail.enviarMail(txtCorreo.Text, "Seguimiento al servicio de Taller de su vehículo: " + dsDatos.Tables[0].Rows[0]["MCAT_PLACA"], mensajeImg, TipoCorreo.HTML, imagenes, nombres);
                    Response.Redirect("" + indexPage + "?process=Marketing.SeguimientoOrdenes&procTer=1");
                }
                catch(Exception z)
                {
                    lb.Text = "No se puedo enviar el correo Razón: " + z.Message;
                }
            }
            else if(proceso == true)
            {
                Utils.MostrarAlerta(Response, "Se ha guardado la acción!");
            }
		}
		
		protected void Preparar_Tabla_Acciones()
		{
			tablaAcciones = new DataTable();
			tablaAcciones.Columns.Add(new DataColumn("CODIGOACCION",System.Type.GetType("System.String")));
			tablaAcciones.Columns.Add(new DataColumn("DESCRIPCIONACCION",System.Type.GetType("System.String")));
			tablaAcciones.Columns.Add(new DataColumn("RESULTADOACCION",System.Type.GetType("System.String")));
			tablaAcciones.Columns.Add(new DataColumn("DETALLERESULTADO",System.Type.GetType("System.String")));
			tablaAcciones.Columns.Add(new DataColumn("EJECUTORACCION",System.Type.GetType("System.String")));
			tablaAcciones.Columns.Add(new DataColumn("FECHAACCION",System.Type.GetType("System.String")));
		}
		
		protected void Llenar_Tabla_Acciones(string prefijo, string numero)
		{
			this.Preparar_Tabla_Acciones();
			DataSet accionesOrden = new DataSet();
			DBFunctions.Request(accionesOrden,IncludeSchema.NO,"SELECT pact_codimark, pres_codigo, dord_detalle, dord_fecha, dord_ejecutor FROM dordenactividad WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numero+"");
			for(int i=0;i<accionesOrden.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaAcciones.NewRow();
				fila["CODIGOACCION"]      = accionesOrden.Tables[0].Rows[i][0].ToString();
				fila["DESCRIPCIONACCION"] = DBFunctions.SingleData("SELECT pact_nombmark FROM pactividadmarketing WHERE pact_codimark='"+accionesOrden.Tables[0].Rows[i][0].ToString()+"'");
				fila["RESULTADOACCION"]   = DBFunctions.SingleData("SELECT pres_descripcion FROM presultadoactividad WHERE pres_codigo='"+accionesOrden.Tables[0].Rows[i][1].ToString()+"'");
				fila["DETALLERESULTADO"]  = accionesOrden.Tables[0].Rows[i][2].ToString();
				fila["EJECUTORACCION"]    = accionesOrden.Tables[0].Rows[i][4].ToString();
				fila["FECHAACCION"]       = (System.Convert.ToDateTime(accionesOrden.Tables[0].Rows[i][3].ToString())).ToString("yyyy-MM-dd");
				tablaAcciones.Rows.Add(fila);
			}
			acciones.DataSource = tablaAcciones;
			acciones.DataBind();
		}

		protected void tipoDocumento_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroOrden,"SELECT mord_numeorde FROM morden WHERE pdoc_codigo='"+tipoDocumento.SelectedValue+"' AND TEST_ESTADO = 'A' ");				
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
