// created on 24/01/2005 at 11:21

using System;
using System.Text;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Mail;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Automotriz
{

	public partial class ConsultarHojaUsuario : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataSet lineas;
		protected DataSet lineas2;
		protected DataTable resultado;
		protected System.Web.UI.WebControls.Label repuestos;
		string vinFinal=null;
		#endregion
				
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Session["HREFRETURN"] != null)
                ViewState["HREFRETURN"]=Session["HREFRETURN"];
            Session.Remove("Rep");
			string placaVehiculo = Request.QueryString["placa"];
			this.Distribuir_Datos_Cliente(placaVehiculo);
			this.Distribuir_Datos_Vehiculo(placaVehiculo);
			this.Distribuir_Grilla_Operaciones(placaVehiculo);
			this.generar(sender,e);
			Session["Rep"] = this.Cuerpo_Correo();
		}
		
		protected void Volver(Object  Sender, EventArgs e)
		{
			string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
            if (ViewState["HREFRETURN"] != null)
            {
                string retRef = ViewState["HREFRETURN"].ToString();
                Response.Redirect(indexPage + "?" + retRef);
            }
            else
                Response.Redirect("" + indexPage + "?process=Automotriz.HojaVidaSinPrecio");
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			MailMessage MyMail = new MailMessage();
   		  	MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
     		MyMail.To = tbEmail.Text;
			MyMail.Subject = "Hoja de Vida de Automovil";
			MyMail.Body = ((string)Session["Rep"]);
      		MyMail.BodyFormat = MailFormat.Html;
			try{
    	   		SmtpMail.Send(MyMail);}
    		catch(Exception e){
    	 	  lb.Text = e.ToString();
    		}
		}

		protected void generar(Object  Sender, EventArgs e)
		{			
			string prefijoNumeroOrden = String.Empty;
			double valorSumatoria = 0;
			PrepararTabla();
			lineas = new DataSet();
			lineas = DBFunctions.Request(lineas,IncludeSchema.NO,"SELECT codigo_orden,numero_orden,codigo_factura,numero_factura,codigo_editado,nombre_item,cantidad_original-COALESCE(cantidad_devuelta,0),valor_unitario FROM vtaller_consrephv WHERE vin_relacionado = '"+vinFinal+"' AND (cantidad_original-COALESCE(cantidad_devuelta,0)) > 0 ORDER BY codigo_orden,numero_orden ASC");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				if(prefijoNumeroOrden != String.Empty && prefijoNumeroOrden != lineas.Tables[0].Rows[i][0]+"-"+lineas.Tables[0].Rows[i][1])
				{
					DataRow drTotal = resultado.NewRow();
					drTotal["REPUESTO"] = "------";
					drTotal["CODREPUESTO"] = "------";
					drTotal["ORDENOP"] = "------";
					drTotal["CANTIDAD"] = "------";
					//drTotal["OTRELAC"] = "<span style='color:#FF0000'>TOTAL "+prefijoNumeroOrden+"</span>";
					drTotal["VALORU"] = valorSumatoria.ToString("C");
					resultado.Rows.Add(drTotal);
					valorSumatoria = 0;
				}
				DataRow fila = resultado.NewRow();
				fila["REPUESTO"] = lineas.Tables[0].Rows[i][5].ToString();
				fila["CODREPUESTO"] = lineas.Tables[0].Rows[i][4].ToString();
				fila["ORDENOP"] = lineas.Tables[0].Rows[i][2]+"-"+lineas.Tables[0].Rows[i][3];
				fila["VALORU"] = Convert.ToDouble(lineas.Tables[0].Rows[i][7]).ToString("C");
				fila["CANTIDAD"] = lineas.Tables[0].Rows[i][6].ToString();
				fila["OTRELAC"] = prefijoNumeroOrden = lineas.Tables[0].Rows[i][0]+"-"+lineas.Tables[0].Rows[i][1];
				valorSumatoria += Convert.ToDouble(lineas.Tables[0].Rows[i][6])*Convert.ToDouble(lineas.Tables[0].Rows[i][7]);
				resultado.Rows.Add(fila);
			}
			if(prefijoNumeroOrden != String.Empty)
			{
				DataRow drTotal = resultado.NewRow();
				drTotal["REPUESTO"] = "------";
				drTotal["CODREPUESTO"] = "------";
				drTotal["ORDENOP"] = "------";
				drTotal["CANTIDAD"] = "------";
				//drTotal["OTRELAC"] = "<span style='color:#FF0000'>TOTAL "+prefijoNumeroOrden+"</span>";
				drTotal["VALORU"] = valorSumatoria.ToString("C");
				resultado.Rows.Add(drTotal);
				valorSumatoria = 0;
			}
			Grid.DataSource = resultado;
			Grid.DataBind();
		}
		#endregion
		
		#region Metodos
		protected void Distribuir_Datos_Cliente(string placaVehiculo)
		{
			string nitCliente = DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			nombres.Text = DBFunctions.SingleData("SELECT mnit_nombres FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			apellidos.Text = DBFunctions.SingleData("SELECT mnit_apellidos FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			nit.Text = nitCliente;
			ciudadExpedicion.Text = DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigoexpnit FROM mnit WHERE mnit_nit='"+nitCliente+"')");
			tipoNacionalidad.Text = DBFunctions.SingleData("SELECT tnac_nombre FROM tnacionalidad WHERE tnac_tiponaci=(SELECT tnac_tiponaci FROM mnit WHERE mnit_nit='"+nitCliente+"')");
			direccion.Text = DBFunctions.SingleData("SELECT mnit_direccion FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			ciudad.Text = DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM mnit WHERE mnit_nit='"+nitCliente+"')");
			telefono.Text = DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			celular.Text = DBFunctions.SingleData("SELECT mnit_celular FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			email.Text = DBFunctions.SingleData("SELECT mnit_email FROM mnit WHERE mnit_nit='"+nitCliente+"'");
			website.Text = DBFunctions.SingleData("SELECT mnit_web FROM mnit WHERE mnit_nit='"+nitCliente+"'");
		}
		
		protected void Distribuir_Datos_Vehiculo(string placaVehiculo)
		{
			catalogo.Text = DBFunctions.SingleData("SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			vin.Text = DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			vinFinal=DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			placa.Text = placaVehiculo;
            marca.Text = DBFunctions.SingleData("SELECT pmar_nombre FROM mcatalogovehiculo MCAT,pcatalogovehiculo PCAT,pmarca PMAR WHERE MCAT.mcat_placa='" + placaVehiculo + "' AND MCAT.pcat_codigo=PCAT.pcat_codigo and PCAT.pmar_codigo=PMAR.pmar_codigo");
            detalle.Text = DBFunctions.SingleData("SELECT pcat_descripcion FROM mcatalogovehiculo MCAT,pcatalogovehiculo PCAT WHERE MCAT.mcat_placa='" + placaVehiculo + "' AND MCAT.pcat_codigo=PCAT.pcat_codigo");
			numeroMotor.Text = DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			serie.Text = DBFunctions.SingleData("SELECT mcat_serie FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			chasis.Text = DBFunctions.SingleData("SELECT mcat_chasis FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			color.Text = DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo=(SELECT pcol_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"')");
			anoModelo.Text = DBFunctions.SingleData("SELECT mcat_anomode FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			tipoServicio.Text = DBFunctions.SingleData("SELECT tser_nombserv FROM tserviciovehiculo WHERE tser_tiposerv=(SELECT tser_tiposerv FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"')");
			conscVendedor.Text = DBFunctions.SingleData("SELECT mcat_concvend FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			fechaVenta.Text = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mcat_venta FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'")).ToString("yyyy-MM-dd");
			ultimoKilometraje.Text = DBFunctions.SingleData("SELECT mcat_numeultikilo FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			kilometrajePromedio.Text = DBFunctions.SingleData("SELECT mcat_numekiloprom FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
		}
		
		protected DataTable Preparar_Tabla_Operaciones(DataTable tablaAsociada)
		{
			tablaAsociada.Columns.Add(new DataColumn("PREFNUMORD",System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("CODIGOOPERACION",System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("PRECIO",System.Type.GetType("System.Double")));
			tablaAsociada.Columns.Add(new DataColumn("REPUESTOS",System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("FECHATERM",System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("OPERACION", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("KILOMETRAJE", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("FECHA", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("MECANICO", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("ESTADO", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("OBSERECE", System.Type.GetType("System.String")));
            tablaAsociada.Columns.Add(new DataColumn("OBSECLIE", System.Type.GetType("System.String")));
			return tablaAsociada;
		}
		
		protected void Distribuir_Grilla_Operaciones(string placaVehiculo)
		{
			string prefijoNumeroOrden = String.Empty;
			double valorSumatoria = 0;
            int aux = 0;
			string vin = DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='"+placaVehiculo+"'");
			DataTable tablaOperacionesRealizadas = new DataTable();
			tablaOperacionesRealizadas = Preparar_Tabla_Operaciones(tablaOperacionesRealizadas);
			DataSet operacionesRealizadas = new DataSet();
            DBFunctions.Request(operacionesRealizadas, IncludeSchema.NO, "SELECT DORP.pdoc_codigo CONCAT '-' CONCAT CAST(DORP.mord_numeorde AS character(20)), DORP.ptem_operacion, DORP.dord_valooper, DORP.dord_fechcump, MOR.MORD_KILOMETRAJE KILOMETRAJE, MOR.MORD_ENTRADA FECHA, MOR.PVEN_CODIGO, TESTO.test_nombre ESTADO, MOR.MORD_OBSERECE OBSERECE, MOR.MORD_OBSECLIE OBSECLIE FROM dordenoperacion DORP, morden MOR, TESTADOOPERACION TESTO WHERE testo.TEST_ESTAOPER=DORP.test_estado AND MOR.mcat_vin='" + vin + "' AND MOR.pdoc_codigo=DORP.pdoc_codigo AND MOR.mord_numeorde=DORP.mord_numeorde ORDER BY DORP.pdoc_codigo,DORP.mord_numeorde ASC");
			for(int i=0;i<operacionesRealizadas.Tables[0].Rows.Count;i++)
			{
				if(prefijoNumeroOrden != String.Empty && prefijoNumeroOrden != operacionesRealizadas.Tables[0].Rows[i][0].ToString())
				{
					DataRow drTotal = tablaOperacionesRealizadas.NewRow();
                    drTotal["OBSERECE"] = "<span style='color:#FF0000'>OBS RECEPCION:</span>";
                    //drTotal["CODIGOOPERACION"] = drTotal["DESCRIPCION"] = drTotal["OPERACION"] = drTotal["KILOMETRAJE"] = drTotal["FECHA"] = drTotal["MECANICO"] = drTotal["ESTADO"] = drTotal["OBSERECE"] = drTotal["OBSECLIE"] = "------";
                    drTotal["OBSERECE"] = "<span style='color:#FF0000'> "+ operacionesRealizadas.Tables[0].Rows[i]["OBSERECE"].ToString() + "</span>";
                    drTotal["OBSECLIE"] = "<span style='color:#FF0000'> "+ operacionesRealizadas.Tables[0].Rows[i]["OBSECLIE"].ToString() + "</span>";
                    tablaOperacionesRealizadas.Rows.Add(drTotal);
					valorSumatoria = 0;
                    aux = i;
				}
				DataRow fila = tablaOperacionesRealizadas.NewRow();
				fila["PREFNUMORD"] = prefijoNumeroOrden = operacionesRealizadas.Tables[0].Rows[i][0].ToString();
				fila["CODIGOOPERACION"] = operacionesRealizadas.Tables[0].Rows[i][1].ToString();
				fila["DESCRIPCION"] = DBFunctions.SingleData("SELECT ptem_descripcion FROM ptempario WHERE ptem_operacion='"+operacionesRealizadas.Tables[0].Rows[i][1].ToString()+"'");
                fila["OPERACION"] = fila["CODIGOOPERACION"].ToString() + " - " + fila["DESCRIPCION"].ToString();
				fila["PRECIO"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][2]);
                try { fila["KILOMETRAJE"] = Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i]["KILOMETRAJE"]).ToString("#,##0"); }catch { }
                try { fila["FECHA"] = Convert.ToDateTime(operacionesRealizadas.Tables[0].Rows[i]["FECHA"]).ToString("yyyy-MM-dd"); }catch { }
                fila["MECANICO"] = DBFunctions.SingleData("SELECT PVEN_CODIGO CONCAT ' - ' CONCAT PVEN_NOMBRE FROM PVENDEDOR WHERE PVEN_CODIGO='" + operacionesRealizadas.Tables[0].Rows[i]["PVEN_CODIGO"].ToString() + "';");
                fila["ESTADO"] = operacionesRealizadas.Tables[0].Rows[i]["ESTADO"].ToString();
				valorSumatoria += Convert.ToDouble(operacionesRealizadas.Tables[0].Rows[i][2]);
				try{fila["FECHATERM"] = Convert.ToDateTime(operacionesRealizadas.Tables[0].Rows[i][3]).ToString("yyyy-MM-dd");}catch{}
				tablaOperacionesRealizadas.Rows.Add(fila);
			}
			if(prefijoNumeroOrden != String.Empty)
			{
				DataRow drTotal = tablaOperacionesRealizadas.NewRow();
				//drTotal["PREFNUMORD"] = "<span style='color:#FF0000'>TOTAL "+prefijoNumeroOrden+"</span>";
                drTotal["OBSERECE"] = "<span style='color:#FF0000'> " + operacionesRealizadas.Tables[0].Rows[aux]["OBSERECE"].ToString() + "</span>";
                drTotal["OBSECLIE"] = "<span style='color:#FF0000'> " + operacionesRealizadas.Tables[0].Rows[aux]["OBSECLIE"].ToString() + "</span>"; 
                tablaOperacionesRealizadas.Rows.Add(drTotal);
				valorSumatoria = 0;
			}
			operacionesRealizadasGrilla.DataSource = tablaOperacionesRealizadas;
			operacionesRealizadasGrilla.DataBind();
		}
		
		protected string Cuerpo_Correo()
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			controlesConsulta.RenderControl(htmlTW);
			return SB.ToString();
		}

		public void PrepararTabla()
		{
			resultado = new DataTable();
			resultado.Columns.Add(new DataColumn("ORDENOP",typeof(string)));
			resultado.Columns.Add(new DataColumn("REPUESTO",typeof(string)));
			resultado.Columns.Add(new DataColumn("VALORU",typeof(string)));
			resultado.Columns.Add(new DataColumn("CANTIDAD",typeof(string)));
			resultado.Columns.Add(new DataColumn("CODREPUESTO",typeof(string)));
			resultado.Columns.Add(new DataColumn("OTRELAC",typeof(string)));
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
