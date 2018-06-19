// created on 03/11/2004 at 10:58
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Xml;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Inventarios;
using AMS.DBManager;
using AMS.Tools;
using AMS.CriptoServiceProvider;

namespace AMS.Automotriz
{
	public partial class VistaImpresion : System.Web.UI.UserControl
	{
		protected double totalParcial = 0;
        protected double totalManoObra = 0;
        protected double totalRepuestos = 0;
        protected double totalIva = 0;
        protected double totalIvaRepuestos = 0; 
        protected double totalIvaOperaciones = 0;
        protected double totalDescuentosMO, descPreliqMob = 0;
        protected double totalDescuentosRep, descPreliqRep = 0;
        protected double totalIvaDescMO = 0;
        protected double totalIVADescRep = 0;
        protected int numDecimales = 0;

        protected bool estadoTransferencia;
        protected string numeroOrden;
        protected string codigoPrefijo;
        protected string tipoProceso;
        protected string cargoEspecial;
        protected string numeroFactura;
        protected string prefijoFactura;
        protected string cantidadTrans;
        protected string mordenNum;
        protected string mordenN;
		protected string numeroOrdenFAC;
        protected string prefijoOrdenFAC;
        protected string destinoCerrar;
        protected string mensaje;
        protected string revisionAseguradora;

        private string Tipo;
        private string actividad;
        private string nit = "";
        private string cargarTabs = "";

        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected DataTable operacionesPeritajeGrabar;
        protected DataTable tablaOperaciones;
        protected DataTable tablaRepuestos;
		protected PlaceHolder toolsFormats;
		protected Button btnImpBaj;
		protected PlaceHolder plcCOM;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private string nacionalidad="";
		protected FormatosDocumentos formatoRecibo = new FormatosDocumentos();
		protected Crypto myCrypto = new Crypto(Crypto.CryptoProvider.TripleDES);
			
		protected void Page_Load(object sender, System.EventArgs e)
		{
            myCrypto.IV   = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            myCrypto.Key  = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            numeroOrden   = Request.QueryString["numOT"];
            codigoPrefijo = Request.QueryString["prefOT"];
            tipoProceso   = Request.QueryString["tipVis"];
            cargoEspecial = Request.QueryString["cargo"];
            destinoCerrar = Request.QueryString["dest"];
            mensaje       = Request.QueryString["msg"];
            revisionAseguradora = Request["revAse"]; //Revisamos si viene una señal que me obligue a revisar el cargo de nuevo. La seña esperada es llamada revAse=S
            cargarTabs    = Request.QueryString["tabs"];
            operacionesPeritaje.Visible = true;
            
            operacionesPeritaje.Controls.Add(LoadControl(pathToControls + "AMS.Automotriz.OrdenTrabajo.OperacionesPeritaje.ascx"));

            if (cargarTabs == "N" || mensaje == "1" )
            {
                
                tab2.Visible = false;
                tab3.Visible = false;
            }
            else if (cargarTabs == "S")
            {
                tab2.Visible = true;
                tab3.Visible = true;
            }

            if (revisionAseguradora != String.Empty && revisionAseguradora == "S")
                cargoEspecial = DBFunctions.SingleData("SELECT tcar_cargo FROM morden WHERE pdoc_codigo='" + codigoPrefijo + "' AND mord_numeorde=" + numeroOrden);

            string centavos = DBFunctions.SingleData("SELECT COALESCE(CEMP_LIQUDECI,'N') FROM CEMPRESA"); //ConfigurationManager.AppSettings["ManejoCentavos"];
            if (Utils.EsEntero(centavos))
                numDecimales = Convert.ToInt32(centavos);

            bool decimales = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarDecimales"]);
            if (decimales)
                numDecimales = 2;

            if (tipoProceso == "orden" || tipoProceso == "prel")
                nit = DBFunctions.SingleData("SELECT mnit_nit FROM morden WHERE pdoc_codigo='" + codigoPrefijo + "' AND mord_numeorde=" + numeroOrden + "");
            
            if (tipoProceso == "liquidar")
            {
                prefijoOrdenFAC = DBFunctions.SingleData("SELECT pdoc_prefordetrab FROM mfacturaclientetaller WHERE pdoc_codigo='" + codigoPrefijo + "' AND mfac_numedocu=" + numeroOrden + "");
                numeroOrdenFAC = DBFunctions.SingleData("SELECT mord_numeorde FROM mfacturaclientetaller WHERE pdoc_codigo='" + codigoPrefijo + "' AND mfac_numedocu=" + numeroOrden + "");
                nacionalidad = IVA.NacionalidadNit(DBFunctions.SingleData("SELECT mnit_nit FROM morden WHERE pdoc_codigo='" + prefijoOrdenFAC + "' AND mord_numeorde=" + numeroOrdenFAC + ""));
                nit = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='" + codigoPrefijo + "' AND mfac_numedocu=" + numeroOrden + "");
            }

            
                
            if (tipoProceso == "prel" && mensaje != "1")
               cerrar.Text = "Ir a Liquidación";

            if (!IsPostBack)
            {
                Construccion_Header();
                LLenar_Grilla_Operaciones();
                Llenar_Grilla_Repuestos();
                Construccion_Footer_Terminos();
                Construccion_Footer();
                LlenarRegistroVisita();
             
                /*          
                 * toolsFormats no se usa en ningun otro lado =S
                 * 
                            if(tipoProceso == "orden")
                                toolsFormats.Visible = false;
                */
                Llenar_Grillas_Peritaje();
                Session["Rep"] = Orden_Lista();
                //Mostrar_Formatos();
                if (mensaje == "1")
                    Utils.MostrarAlerta(Response, "No se preliquido la orden de trabajo, las operaciones no estan cumplidas en su totalidad");
                    
                
                /*if(tipoProceso == "orden")
                    btnImpBaj.Attributes.Add("onclick","DialogBox('"+ddlFormatos.SelectedValue+"','"+DBFunctions.SingleData("SELECT pdoc_codigo FROM mfacturaclientetaller WHERE pdoc_prefordetrab='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+" ORDER BY mfac_fechcrea DESC")+"','"+DBFunctions.SingleData("SELECT mfac_numedocu FROM mfacturaclientetaller WHERE pdoc_prefordetrab='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+" ORDER BY mfac_fechcrea DESC")+"');");
                else if(tipoProceso == "liquidar")
                    btnImpBaj.Attributes.Add("onclick","DialogBox('"+ddlFormatos.SelectedValue+"','"+codigoPrefijo+"','"+numeroOrden+"');");*/

            RevisionImpresionPDF();
            }
		}
				
		/*protected void Cambio_Formato(Object  Sender, EventArgs e)
		{
			btnImpBaj.Attributes.Add("onclick",ddlFormatos.SelectedValue);
		}*/
			
		protected void CerrarVentana(Object  Sender, EventArgs e)
		{
			if(destinoCerrar == "0")
			{
				if(tipoProceso == "orden")
					Response.Redirect("" + indexPage + "?process=Automotriz.OrdenTrabajo");
				else if(tipoProceso == "liquidar" || tipoProceso=="prel")
					    Response.Redirect("" + indexPage + "?process=Automotriz.LiquidaOrden");
			}
			else if(destinoCerrar == "1")
			{
				if(tipoProceso == "orden")
					Response.Redirect("" + indexPage + "?process=Automotriz.OrdenTrabajo");
				else if(tipoProceso == "liquidar" || tipoProceso=="prel")
					    Response.Redirect("" + indexPage + "?process=Automotriz.ProcesosLiquidacion&pref="+prefijoOrdenFAC+"&num="+numeroOrdenFAC+"");
			}
			else if(destinoCerrar == "2")
			{
				if(tipoProceso == "orden")
				{
					Response.Redirect("" + indexPage + "?process=Automotriz.LiquidaOrden");
				}
				else if(tipoProceso=="prel" && mensaje!="1")
					    Response.Redirect(indexPage+"?process=Automotriz.ProcesosLiquidacion&pref="+codigoPrefijo+"&num="+numeroOrden+"");
				else if(tipoProceso=="prel" && mensaje=="1")
					    Response.Redirect("" + indexPage + "?process=Automotriz.LiquidaOrden");
			}
		}

        public void SendMail(Object Sender, ImageClickEventArgs E)
        {


            numeroOrden = Request.QueryString["numeroOrden"];
            codigoPrefijo = Request.QueryString["codigoPrefijo"];
            string subject = "";
            if (tipoProceso == "orden")
                subject = "ORDEN DE TRABAJO";
            else if (tipoProceso == "prel") //"preliquidar")
                subject = "PRELIQUIDACION";
            else if (tipoProceso == "liquidar")
                subject = "LIQUIDACION";
            //Mail miMail = new Mail();
            //miMail.EnviarMail(ConfigurationManager.AppSettings["EmailFrom"], tbEmail.Text, subject, ((string)Session["Rep"]), TipoCorreo.HTML, ConfigurationManager.AppSettings["MailServer"], string.Empty, string.Empty, myCrypto.DescifrarCadena(ConfigurationManager.AppSettings["PasswordEMail"]));
            //int t = miMail.EnviarMail(tbEmail.Text, subject, ((string)Session["Rep"]), TipoCorreo.HTML, "");
            int t = Utils.EnviarMail(tbEmail.Text, subject, ((string)Session["Rep"]), TipoCorreo.HTML, "");
            if (t == 1)
                Utils.MostrarAlerta(Response, "Se ha enviado correctamente el reporte a: " + tbEmail.Text);
            else
                Utils.MostrarAlerta(Response, "No fue posible enviar el reporte a: " + tbEmail.Text);
            Construccion_Header();
            LLenar_Grilla_Operaciones();
            Llenar_Grilla_Repuestos();
            Construccion_Footer_Terminos();
            Construccion_Footer();
            LlenarRegistroVisita();
            Llenar_Grillas_Peritaje();
        }

        protected string Orden_Lista()
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			tabHeaderEmpresa.RenderControl(htmlTW);
			tabHeaderPropietario.RenderControl(htmlTW);
			tabHeaderVehiculo.RenderControl(htmlTW);
			tabFooterDiscriminacionOperaciones.RenderControl(htmlTW);
			tabFooterTotal.RenderControl(htmlTW);
			tituloOperaciones.RenderControl(htmlTW);
			operaciones.RenderControl(htmlTW);
			tituloRepuestos.RenderControl(htmlTW);
			repuestos.RenderControl(htmlTW);
			tituloPeritaje.RenderControl(htmlTW);
			peritaje.RenderControl(htmlTW);
			tabFooterTerminos.RenderControl(htmlTW);
			string orden = SB.ToString();
			return orden;
		}
		
		protected void Construccion_Header()
		{
            numeroOrden = Request.QueryString["numOT"];
            codigoPrefijo = Request.QueryString["prefOT"];

            if (codigoPrefijo != string.Empty && numeroOrden != string.Empty)
			{
				int i,j;
				int numRows, numCells;
				//constriumos los datos de la empresa
				numRows = 3;
				numCells = 3;
				string codigoTaller = "";
				if(tipoProceso == "orden" || tipoProceso=="prel")
					codigoTaller =DBFunctions.SingleData("SELECT palm_almacen FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
				else if(tipoProceso == "liquidar")
					codigoTaller =DBFunctions.SingleData("SELECT palm_almacen FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+"");
				for(i=0;i<numRows;i++)
				{
					TableRow r = new TableRow();
					for(j=0;j<numCells;j++)
					{
						TableCell c = new TableCell();
						if(i==0 && j == 1)
						{
							c.Text = "<center>"+DBFunctions.SingleData("SELECT cemp_nombre FROM cempresa")+"</center>";
							c.Text+= "<center>NIT: "+DBFunctions.SingleData("SELECT MNIT.mnit_nit CONCAT '-' CONCAT MNIT.mnit_digito FROM dbxschema.cempresa CEMP,dbxschema.mnit MNIT WHERE MNIT.mnit_nit=CEMP.mnit_nit")+"</center>";
						}
                        if (i == 0 && j == 2)
                        {
                            if (tipoProceso.Contains("prel"))// == "preliquidar")
                                c.Text = "<center>PRELIQUIDACION</center>";
                            else if (tipoProceso == "liquidar")
                                c.Text = "<center>LIQUIDACION</center>";
                        }
						if(i==1 && j == 1)
						{
							if(tipoProceso=="orden")
								c.Text = "<br><center>Orden de Trabajo "+codigoPrefijo+"-"+numeroOrden+"</center>";
							else if(tipoProceso=="liquidar")
								c.Text = "<br><center>Factura de Venta Nº "+codigoPrefijo+"-"+numeroOrden+"</center>";
						}
						if(i==2 && j==0)
						{
							c.CssClass = "LetraMenuda";
							c.Text = "<br>IVA "+DBFunctions.SingleData("SELECT treg_nombre FROM tregimeniva WHERE treg_regiiva=(SELECT cemp_indiregiiva FROM cempresa)");
							if(DBFunctions.SingleData("SELECT cemp_indiretefuen FROM cempresa")=="A" || DBFunctions.SingleData("SELECT cemp_indiretefuen FROM cempresa")=="B")
								c.Text += "<br>SOMOS AUTO-RETENEDORES.";
							else
								c.Text += "<br>NO SOMOS AUTO-RETENEDORES.";
							if(DBFunctions.SingleData("SELECT cemp_indigrancont FROM cempresa")=="S")
								c.Text += "<br>SOMOS GRANDES CONTRIBUYENTES";
							else
								c.Text += "<br>NO SOMOS GRANDES CONTRIBUYENTES";
							c.Text += "<br>"+DBFunctions.SingleData("SELECT 'Numeración autorizada por la DIAN Resolución Nº ' CONCAT pdoc_numeresofact CONCAT ' Fecha ' CONCAT CAST(pdoc_fechresofact AS char(10)) CONCAT ' Rango con prefijo ' CONCAT pdoc_codigo CONCAT ' Nº ' CONCAT CAST(pdoc_numeinic AS char(10)) CONCAT ' al Nº ' CONCAT CAST(pdoc_numefina AS char(10)) FROM dbxschema.pdocumento WHERE pdoc_codigo='"+codigoPrefijo+"'");
						}
						if(i==2 && j==1)
						{
                            c.Text += "<br><center>" + DBFunctions.SingleData("SELECT palm_direccion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codigoTaller + "'") + "</center>";
                            c.Text += "<center>TELEFONOS:</center><center>" + DBFunctions.SingleData("SELECT palm_telefono1 FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codigoTaller + "'") + " - " + DBFunctions.SingleData("SELECT palm_telefono2 FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codigoTaller + "'") + "</center>";
                            c.Text += "<center>" + DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codigoTaller + "')") + " - " + DBFunctions.SingleData("SELECT ppai_nombre FROM ppais WHERE ppai_pais=(SELECT ppai_pais FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codigoTaller + "'))") + "</center>";
						}
						if(i==2 && j==2)
						{
							c.Text = "<img src='../img/AMS.LogoEmpresa.png'>";
							c.HorizontalAlign = HorizontalAlign.Center;
						}
						c.Width = new Unit("33%");
						r.Cells.Add(c);
					}
					tabHeaderEmpresa.Rows.Add(r);
				}
				tabHeaderEmpresa.BorderWidth = new Unit("1");
				//tabHeaderEmpresa.Width = operaciones.Width;
				//construimos los datos del propietario
				numRows = 7;
				numCells = 2;
				string nit = "";
				if(tipoProceso == "orden" || tipoProceso=="prel")
					nit = DBFunctions.SingleData("SELECT mnit_nit FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
				else if(tipoProceso == "liquidar")
					    nit = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+"");
				for(i=0;i<numRows;i++)
				{
					TableRow r = new TableRow();
					for(j=0;j<numCells;j++)
					{
						TableCell c = new TableCell();
						if(i==0 && j==0)
							c.Text = "DATOS DEL CLIENTE";
						if(i==0 && j==1)
							c.Text = "DATOS DE LA ORDEN";
						if(i==1 && j==0)
                        {
                            String contacto = DBFunctions.SingleData("SELECT MORD_CONTACTO FROM morden WHERE pdoc_codigo='" + codigoPrefijo + "' AND mord_numeorde=" + numeroOrden + "");

                            c.Text = "<br>Cliente : " + DBFunctions.SingleData("SELECT mnit_apellidos || ' ' || mnit_nombres FROM mnit WHERE mnit_nit='" + nit + "'") +
                                ((contacto!=null) ? "<br>Contacto : " + contacto : "");
						    
                        }
                        if(i==1 && j==1)
						{
							if(tipoProceso == "orden" || tipoProceso=="prel")
							{
								DateTime fechaEntrada = Convert.ToDateTime(DBFunctions.SingleData("SELECT mord_entrada FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+""));
								c.Text = "Orden : " + codigoPrefijo+"-"+numeroOrden;
								c.Text += "<br>Fecha y Hora de Entrada : "+fechaEntrada.ToString("yyyy-MM-dd")+ " a las "+DBFunctions.SingleData("SELECT mord_horaentr FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
							}
							else if(tipoProceso == "liquidar")
							{
								c.Text = "Orden : " + prefijoOrdenFAC+"-"+numeroOrdenFAC;
								DateTime fechaEntrada = DateTime.Now;
								try{fechaEntrada = Convert.ToDateTime(DBFunctions.SingleData("SELECT mord_entrada FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+""));}
								catch{lb.Text += "SELECT mord_entrada FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+"";}
								c.Text += "<br>Fecha y Hora de Entrada : "+fechaEntrada.ToString("yyyy-MM-dd")+ " a las "+DBFunctions.SingleData("SELECT mord_horaentr FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
								c.Text += "<br>Fecha Factura : "+Convert.ToDateTime(DBFunctions.SingleData("SELECT mfac_factura FROM mfacturacliente WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+"")).ToString("yyyy-MM-dd");
							}
						}						
						if(i==2 && j==0)
							c.Text = "N.I.T : "+nit;
						if(i==2 && j==1)
							c.Text = "Nº de Entrada : "+DBFunctions.SingleData("SELECT mord_numeentr FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
						if(i==3 && j==0)
							c.Text = "Telefonos :"+DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit='"+nit+"'")+" - "+DBFunctions.SingleData("SELECT mnit_celular FROM mnit WHERE mnit_nit='"+nit+"'");
						if(i==3 && j==1)
						{
							if(tipoProceso == "orden" || tipoProceso=="prel")
								c.Text = "Recepcionista : "+DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+")");
							else if(tipoProceso == "liquidar")
								    c.Text = "Recepcionista : "+DBFunctions.SingleData("SELECT pven_nombre FROM dbxschema.pvendedor WHERE pven_codigo=(select morden.pven_codigo from dbxschema.mfacturaclientetaller mfac, dbxschema.morden morden where  mfac.pdoc_prefordetrab=morden.pdoc_codigo and mfac.mord_numeorde=morden.mord_numeorde and mfac.mord_numeorde="+numeroOrdenFAC+" and mfac.pdoc_prefordetrab='"+prefijoOrdenFAC+"' )");
								//c.Text = "Recepcionista : "+DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+mordenNum+")");
						}
						if(i==4 && j==0)
							c.Text = "Dirección : "+DBFunctions.SingleData("SELECT mnit_direccion FROM mnit WHERE mnit_nit='"+nit+"'")+" "+DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM mnit WHERE mnit_nit='"+nit+"')");
						if(i==4 && j==1)
						{
                            if (tipoProceso == "orden" || tipoProceso == "prel")
                            {
                                String cargo = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo=(SELECT tcar_cargo FROM morden WHERE pdoc_codigo='" + codigoPrefijo + "' AND mord_numeorde=" + numeroOrden + ")");
                                c.Text = "Cargo : " + cargo;
                                if(cargo.Trim() == "Seguros")
                                    c.Text += " - " + DBFunctions.SingleData("select mnit_nombres concat ' ' concat  mnit_nombre2 concat ' ' concat mnit_apellidos concat ' ' concat mnit_apellido2 from mnit where mnit_nit= (select mnit_nitseguros from dordenseguros where pdoc_codigo = '" + codigoPrefijo + "' and mord_numeorde=" + numeroOrden + ")");
                            }
                            else if (tipoProceso == "liquidar")
                                c.Text = "Cargo Factura : " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + cargoEspecial + "'");
						}

						if(i==5 && j==0)
							c.Text = "E-mail : "+DBFunctions.SingleData("SELECT mnit_email FROM mnit WHERE mnit_nit='"+nit+"'")+" "+DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT mnit_direccion FROM mnit WHERE mnit_nit='"+nit+"')");
						if(i==5 && j==1)
						{
							if(tipoProceso == "orden" || tipoProceso=="prel")
							{
								DateTime fechaSalida = Convert.ToDateTime(DBFunctions.SingleData("SELECT mord_entregar FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+""));
								c.Text = "Fecha y Hora de Salida : "+fechaSalida.ToString("yyyy-MM-dd")+ " a las "+DBFunctions.SingleData("SELECT mord_horaentg FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
								c.Text += "<br> (Estas son estimaciones y estan sujetas a cambios por imprevisto)";
							}
							else if(tipoProceso == "liquidar")
							{
								DateTime fechaSalida = DateTime.Now;
								try{fechaSalida = Convert.ToDateTime(DBFunctions.SingleData("SELECT mord_entregar FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+""));}
								catch{lb.Text += "<br>"+"SELECT mord_entregar FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+"";}
								c.Text = "Fecha y Hora de Salida : "+fechaSalida.ToString("yyyy-MM-dd")+ " a las "+DBFunctions.SingleData("SELECT mord_horaentg FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
								c.Text += "<br> (Estas son estimaciones y estan sujetas a cambios por imprevisto)";
							}
						}
						if(i==6 && j==0)
						{
							if(tipoProceso == "orden" || tipoProceso=="prel")
								c.Text +="Observaciones del Cliente:<br>"+DBFunctions.SingleData("SELECT mord_obseclie FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
							else if(tipoProceso == "liquidar")
							{
								c.Text +="Observaciones del Cliente OT :<br>"+DBFunctions.SingleData("SELECT mord_obseclie FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+"");
								c.Text +="<br>Observaciones del Cliente Factura :<br>"+DBFunctions.SingleData("SELECT mfac_observacion FROM mfacturacliente WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+"");
							}
						}
						if(i==6 && j==1)
						{
							if(tipoProceso == "orden"|| tipoProceso=="prel")
								c.Text +="Observaciones del Recepcionista:<br>"+DBFunctions.SingleData("SELECT mord_obserece FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
							else if(tipoProceso == "liquidar")
								c.Text +="Observaciones del Recepcionista:<br>"+DBFunctions.SingleData("SELECT mord_obserece FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+"");
						}
						c.Width = new Unit("50%");
						r.Cells.Add(c);
					}
					tabHeaderPropietario.Rows.Add(r);
				}
				tabHeaderPropietario.BorderWidth = new Unit("1");
				//tabHeaderPropietario.Width = operaciones.Width;
				//construimos los datos del vehiculo
				numRows = 5;
				numCells = 4;
				string placa = "";
				if(tipoProceso == "orden" || tipoProceso=="prel")
					placa = DBFunctions.SingleData("SELECT mcat_placa FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+")");
				else if(tipoProceso == "liquidar")
					    placa = DBFunctions.SingleData("SELECT mcat_placa FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+")");
				if (placa=="")
                {
                    Utils.MostrarAlerta(Response, "NO ENCONTRE PLACA del vehículo, revice el VIN y ACTUALICELO antes de continuar con el proceso !!!");
                    return;
                }

                DataSet dsDatosVehiculo = new DataSet();
                string sqlDatosVehiculo = "Select PC.PCAT_CODIGO, PMAR_NOMBRE, PC.PCAT_DESCRIPCION, MC.MCAT_VIN, pcol_descripcion, MC.MCAT_ANOMODE, MC.MCAT_MOTOR, MC.MCAT_SERIE, COALESCE(mcat_numeultikilo,0), tser_nombserv, mcat_concvend" +
                       " FROM PCATALOGOVEHICULO PC, MCATALOGOVEHICULO MC, PMARCA PM, PCOLOR PCL, tserviciovehiculo TS  " +
                       " WHERE PC.PCAT_CODIGO = MC.PCAT_CODIGO AND PC.PMAR_CODIGO = PM.PMAR_CODIGO AND MC.PCOL_CODIGO = PCL.PCOL_CODIGO AND TS.tser_tiposerv = MC.tser_tiposerv" +
                       " AND MC.MCAT_PLACA = '"+placa+"' ";
                DBFunctions.Request(dsDatosVehiculo, IncludeSchema.NO, sqlDatosVehiculo);
                for(i=0;i<numRows;i++)
				{
					TableRow r = new TableRow();
					for(j=0;j<numCells;j++)
					{
						TableCell c = new TableCell();
						if(i==0 && j==0)
							c.Text = "DATOS DEL VEHICULO";
						if(i==1 && j==0)
							c.Text = "Catálogo : "+dsDatosVehiculo.Tables[0].Rows[0][0].ToString();
                                //DBFunctions.SingleData("SELECT pgru_nombre FROM pgrupocatalogo WHERE pgru_grupo=(SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'))");
						if(i==1 && j==1)
                            c.Text = "Marca : " + dsDatosVehiculo.Tables[0].Rows[0][1].ToString() +
                                //DBFunctions.SingleData("SELECT pmar_nombre FROM pmarca WHERE pmar_codigo=(SELECT pmar_codigo FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'))") +
                            "<br>Detalle : " + dsDatosVehiculo.Tables[0].Rows[0][2].ToString();
                                //DBFunctions.SingleData("SELECT pcat_descripcion FROM mcatalogovehiculo MCAT,pcatalogovehiculo PCAT WHERE MCAT.mcat_placa='" + placa + "' AND MCAT.pcat_codigo=PCAT.pcat_codigo");
						if(i==1 && j==2)
                            c.Text = "Fabricación : " + dsDatosVehiculo.Tables[0].Rows[0][3].ToString();
                                //DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='" + placa + "'");
						if(i==1 && j==3)
                            c.Text = "Color : " + dsDatosVehiculo.Tables[0].Rows[0][4].ToString();
                                //DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo=(SELECT pcol_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"')");
						if(i==2 && j==0)
                            c.Text = "Modelo : " + dsDatosVehiculo.Tables[0].Rows[0][5].ToString();
                                //DBFunctions.SingleData("SELECT mcat_anomode FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
						if(i==2 && j==1)
                            c.Text = "Motor : " + dsDatosVehiculo.Tables[0].Rows[0][6].ToString();
                                //DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
						if(i==2 && j==2)
                            c.Text = "Serie : " + dsDatosVehiculo.Tables[0].Rows[0][7].ToString();
                                //DBFunctions.SingleData("SELECT mcat_serie FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
						if(i==2 && j==3)
							c.Text = "Placa : "+placa;
						if(i==3 && j==0)
                            //c.Text = "Kilometraje : " + Convert.ToDouble(dsDatosVehiculo.Tables[0].Rows[0][8].ToString()).ToString("N");
                            c.Text = "Kilometraje : " + DBFunctions.SingleData("SELECT MORD_KILOMETRAJE FROM morden WHERE pdoc_codigo='" + codigoPrefijo + "' AND mord_numeorde=" + numeroOrden + "");
                                //Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(mcat_numeultikilo,0) FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'")).ToString("N");
						if(i==3 && j==1)
                            c.Text = "Servicio : " + dsDatosVehiculo.Tables[0].Rows[0][9].ToString();
                                //DBFunctions.SingleData("SELECT tser_nombserv FROM tserviciovehiculo WHERE tser_tiposerv=(SELECT tser_tiposerv FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"')");
						if(i==3 && j==2)
						{
							if(tipoProceso == "orden"|| tipoProceso=="prel")
								c.Text = "N/Combustible : "+DBFunctions.SingleData("SELECT tnivcomb_descripcion FROM tnivelcombustible WHERE tnivcomb_codigo=(SELECT tnivcomb_codigo FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+")");
							else if(tipoProceso == "liquidar")
								c.Text = "N/Combustible : "+DBFunctions.SingleData("SELECT tnivcomb_descripcion FROM tnivelcombustible WHERE tnivcomb_codigo=(SELECT tnivcomb_codigo FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+")");
						}
						if(i==3 && j==3)
                            c.Text = "Conce. Vendedor : " + dsDatosVehiculo.Tables[0].Rows[0][10].ToString();
                                //DBFunctions.SingleData("SELECT mcat_concvend FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
						if(i==4 && j==0)
						{
							DateTime fechaVenta = DateTime.Now;
							try{fechaVenta = Convert.ToDateTime(DBFunctions.SingleData("SELECT mcat_venta FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'"));}
							catch{lb.Text += "<br>"+"SELECT mcat_venta FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'";}
							c.Text = "Fecha de Venta : "+fechaVenta.ToString("yyyy-MM-dd");
						}						
						c.Width = new Unit("25%");
						r.Cells.Add(c);
					}
					tabHeaderVehiculo.Rows.Add(r);
				}
				//vamos a meter dentro de la tabla que carga los datos del vehiculo los accesorios que estan guardados dentro de la tabla
				DataSet accesorios = new DataSet();
				if(tipoProceso == "orden" || tipoProceso=="prel")
					DBFunctions.Request(accesorios,IncludeSchema.NO,"SELECT pacc_codigo, mord_estado FROM dordenaccesorio WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+"");
				else if(tipoProceso == "liquidar")
					    DBFunctions.Request(accesorios,IncludeSchema.NO,"SELECT pacc_codigo, mord_estado FROM dordenaccesorio WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+"");
				numRows = accesorios.Tables[0].Rows.Count + 1;
				numCells = 4;
				int controlBucle = 0;
				for(i=0;i<numRows;i++)
				{
					TableRow r = new TableRow();
					for(j=0;j<numCells;j++)
					{
						TableCell c = new TableCell();
						if(controlBucle < accesorios.Tables[0].Rows.Count)
						{
							c.Text = DBFunctions.SingleData("SELECT pacc_descripcion FROM paccesorio WHERE pacc_codigo='"+accesorios.Tables[0].Rows[controlBucle][0].ToString()+"'") + " : "+accesorios.Tables[0].Rows[controlBucle][1].ToString();
							controlBucle+=1;
							c.Width = new Unit("25%");
							r.Cells.Add(c);
						}
					}
					tabHeaderVehiculo.Rows.Add(r);
				}
				tabHeaderVehiculo.BorderWidth = new Unit("1");
				//tabHeaderVehiculo.Width = operaciones.Width;
				//Se construyen los titulos de los diferentes grillas
				TableRow row = new TableRow();
				TableCell cell = new TableCell();
				cell.Text = "DESCRIPCION DE OPERACIONES";
				cell.HorizontalAlign=HorizontalAlign.Center;
				cell.ColumnSpan=1;
				row.Cells.Add(cell);
				tituloOperaciones.Rows.Add(row);
				//tituloOperaciones.Width=operaciones.Width;
				/////////////////////////////////////////////
				row = new TableRow();
				cell = new TableCell();
				cell.Text = "REPUESTOS";
				cell.HorizontalAlign=HorizontalAlign.Center;
				cell.ColumnSpan=1;
				row.Cells.Add(cell);
				tituloRepuestos.Rows.Add(row);
				//tituloRepuestos.Width=operaciones.Width;
			}
		}
		
		protected void Construccion_Footer()
		{
			double porcDescIvaMO=0,porcDescIvaRep=0;
			try
			{
                try
                {
                    descPreliqMob = Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE (MORD_VALODESCMOB,0) FROM MORDENDESCUENTO WHERE PDOC_CODIGO ='" + codigoPrefijo + "' AND MORD_NUMEORDE = " + numeroOrden + ""));
                }
                catch
                {
                    descPreliqMob = 0;
                }

                totalDescuentosMO =Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_descoperaciones FROM mfacturaclientetaller WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+""));
				if(totalDescuentosMO!=0)
				{
					if(nacionalidad!="E")
					{
						porcDescIvaMO=(100*totalDescuentosMO)/totalManoObra;
						totalIvaDescMO=totalIvaOperaciones*(porcDescIvaMO/100);
					}
				}
			}
			catch{}
			try
			{
                try
                {
                    descPreliqRep = Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE (MORD_VALODESCREP,0) FROM MORDENDESCUENTO WHERE PDOC_CODIGO ='" + codigoPrefijo + " AND MORD_NUMEORDE = " + numeroOrden + ""));
                }
                catch
                {
                    descPreliqRep = 0;
                }
                totalDescuentosRep =Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_descrepuestos FROM mfacturaclientetaller WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+""));
				if(totalDescuentosRep!=0)
				{
					if(nacionalidad!="E")
					{
						porcDescIvaRep=(100*totalDescuentosRep)/totalRepuestos;
						totalIVADescRep=totalIvaRepuestos*(porcDescIvaRep/100);
					}
				}
			}
			catch{}
			
			int i,j,numRows,numCells;
			if(cargoEspecial!="S")
			{
				numRows = 7;
				numCells = 2;
				//Construccion de totales
				TableRow tbr=new TableRow();
				TableCell tbc=new TableCell();
				tbc.Text="TOTALES";
				tbc.HorizontalAlign=HorizontalAlign.Center;
				tbc.ColumnSpan=2;
				tbr.Cells.Add(tbc);
				tabFooterTotal.Rows.Add(tbr);
				for(i=0;i<numRows;i++)
				{
					TableRow r = new TableRow();
					for(j=0;j<numCells;j++)
					{
						TableCell c = new TableCell();
						if(i==0 && j==0)
							c.Text = "<div style='text-align:right'>Valor Mano de Obra Mecanica :&nbsp;&nbsp;&nbsp;</div>";
						if(i==0 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round(totalManoObra, numDecimales).ToString("C");
						}
						if(i==1 && j==0)
							c.Text = "<div style='text-align:right'>Valor Repuestos :&nbsp;&nbsp;&nbsp;</div>";
						if(i==1 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round(totalRepuestos, numDecimales).ToString("C");
						}
						if(i==2 && j==0)
							c.Text="<div style='text-align:right'>Total Descuentos Mano de Obra :&nbsp;&nbsp;&nbsp;</div>";
						if(i==2 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
							c.Text = Math.Round(totalDescuentosMO - descPreliqMob, numDecimales).ToString("C");
						}
						if(i==3 && j==0)
							c.Text="<div style='text-align:right'>Total Descuentos Repuestos :&nbsp;&nbsp;&nbsp;</div>";
						if(i==3 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round(totalDescuentosRep - descPreliqRep, numDecimales).ToString("C");
						}
						if(i==4 && j==0)
							c.Text = "<div style='text-align:right'>SUB - TOTAL :&nbsp;&nbsp;&nbsp;</div>";
						if(i==4 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round((totalManoObra + totalRepuestos - totalDescuentosMO - totalDescuentosRep - descPreliqMob - descPreliqRep), numDecimales).ToString("C");
						}
						if(i==5 && j==0)
							c.Text = "<div style='text-align:right'> I.V.A :&nbsp;&nbsp;&nbsp;</div>";
						if(i==5 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
							c.Text = Math.Round((totalIva-totalIvaDescMO-totalIVADescRep),numDecimales).ToString("C");
						}
						if(i==6 && j==0)
							c.Text = "<div style='text-align:right'>Valor TOTAL :&nbsp;&nbsp;&nbsp;</div>";
						if(i==6 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round((totalManoObra + totalRepuestos + (totalIva - totalIvaDescMO - totalIVADescRep) - totalDescuentosMO - totalDescuentosRep - descPreliqMob - descPreliqRep), numDecimales).ToString("C");
						}
						c.Width = new Unit("50%");
						r.Cells.Add(c);
					}
					tabFooterTotal.Rows.Add(r);
				}
				tabFooterTotal.BorderWidth = new Unit("1");
        		//tabFooterTotal.Width = operaciones.Width;
				//Construccion de la otra parte
			}			
			else if(cargoEspecial == "S")
			{
				numRows = 9;
				numCells = 2;
				double porcentajeDeducible = 0;
				if(tipoProceso == "orden")
					porcentajeDeducible = Convert.ToDouble(DBFunctions.SingleData("SELECT mord_porcdeducible FROM dordenseguros WHERE pdoc_codigo='"+ codigoPrefijo +"' AND mord_numeorde="+numeroOrden+""));
				else if(tipoProceso == "liquidar")
					    try{porcentajeDeducible = Convert.ToDouble(DBFunctions.SingleData("SELECT mord_porcdeducible FROM dordenseguros WHERE pdoc_codigo='"+ prefijoOrdenFAC +"' AND mord_numeorde="+numeroOrdenFAC+""));}
					    catch{porcentajeDeducible = 0;}
				double valorMinimoDeducible = 0;
				double valorDeducibleSuministros = 0;
				for(i=0;i<numRows;i++)
				{
					TableRow r = new TableRow();
					for(j=0;j<numCells;j++)
					{
						TableCell c = new TableCell();
						if(i==0 && j==0)
							c.Text = "<div style='text-align:right'>Valor Mano de Obra Mecanica :&nbsp;&nbsp;&nbsp;</div>";
						if(i==0 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round(totalManoObra, numDecimales).ToString("C");
						}
						if(i==1 && j==0)
							c.Text = "<div style='text-align:right'>Valor Repuestos :&nbsp;&nbsp;&nbsp;</div>";
						if(i==1 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round(totalRepuestos, numDecimales).ToString("C");
						}
						if(i==2 && j==0)
							c.Text = "<div style='text-align:right'>SUB - TOTAL :&nbsp;&nbsp;&nbsp;</div>";
						if(i==2 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round((totalManoObra + totalRepuestos), numDecimales).ToString("C");
						}
						if(i==3 && j==0)
							c.Text = "<div style='text-align:right'>Valor I.V.A :&nbsp;&nbsp;&nbsp;</div>";
						if(i==3 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round((totalIva), numDecimales).ToString("C");
						}
						if(i==4 && j==0)
							c.Text = "<div style='text-align:right'>Valor TOTAL :&nbsp;&nbsp;&nbsp;</div>";
						if(i==4 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
                            c.Text = Math.Round((totalManoObra + totalRepuestos + totalIva), numDecimales).ToString("C");
						}
						if(i==5 && j==0)
							c.Text = "<div style='text-align:right'>Valor Deducible Aseguradora:&nbsp;&nbsp;&nbsp;</div>";
						if(i==5 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
							try
							{
								if(tipoProceso == "orden")
									valorMinimoDeducible = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_deduminimo FROM mfacturaclientetaller WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+" AND pdoc_prefordetrab='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+""));
								else if(tipoProceso == "liquidar")
									valorMinimoDeducible = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_deduminimo FROM mfacturaclientetaller WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+" AND pdoc_prefordetrab='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+""));
							}
							catch{valorMinimoDeducible = 0;}
                            c.Text = Math.Round(valorMinimoDeducible, numDecimales).ToString("C");
						}
						if(i==6 && j==0)
							c.Text = "<div style='text-align:right'>Valor Deducible Suministros Aseguradora:&nbsp;&nbsp;&nbsp;</div>";
						if(i==6 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
							try
							{
								if(tipoProceso == "orden" || tipoProceso=="prel")
									valorDeducibleSuministros = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_dedusumase FROM mfacturaclientetaller WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+" AND pdoc_prefordetrab='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+""));
								else if(tipoProceso == "liquidar")
									    valorDeducibleSuministros = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_dedusumase FROM mfacturaclientetaller WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+" AND pdoc_prefordetrab='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+""));
							}
							catch{
								valorDeducibleSuministros = 0;
							}
                            c.Text = Math.Round(valorDeducibleSuministros, numDecimales).ToString("C");
						}
						if(i==7 && j==0)
							c.Text = "<div style='text-align:right'>VALOR A CANCELAR :&nbsp;&nbsp;&nbsp;</div>";
						if(i==7 && j==1)
						{
							c.HorizontalAlign = HorizontalAlign.Right;
							if(tipoProceso == "orden" || tipoProceso=="prel")
							{
								c.Text = "Sin Liquidar";
							}
							else if(tipoProceso == "liquidar")
							{
								    c.Text = Math.Round(Convert.ToDouble(
                                    DBFunctions.SingleData("SELECT mfac_valofact + mfac_valoiva + mfac_valoflet + mfac_valoivaflet - mfac_valorete FROM mfacturacliente WHERE pdoc_codigo='" + codigoPrefijo + "' AND mfac_numedocu=" + numeroOrden + ""))
                                    , numDecimales).ToString("C");
								
							}
						}
						c.Width = new Unit("50%");
						r.Cells.Add(c);
					}
					tabFooterTotal.Rows.Add(r);
				}
				tabFooterTotal.BorderWidth = new Unit("1");
        		//tabFooterTotal.Width = operaciones.Width;
			}
		}
		
		protected void Construccion_Footer_Terminos()
		{
			if(tipoProceso=="liquidar")
			{
				int numRows = 6, numCells = 2;
				string diasVigencia = DBFunctions.SingleData("SELECT mfac_diasplaz FROM mfacturacliente WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+"");
				string centroCosto = DBFunctions.SingleData("SELECT pcen_nombre FROM pcentrocosto WHERE pcen_codigo=(SELECT pcen_codigo FROM mfacturacliente WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+")");
				//string recepcionista = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+")");
				string recepcionista = DBFunctions.SingleData("SELECT pven_nombre FROM dbxschema.pvendedor WHERE pven_codigo=(select morden.pven_codigo from dbxschema.mfacturaclientetaller mfac, dbxschema.morden morden where  mfac.pdoc_prefordetrab=morden.pdoc_codigo and mfac.mord_numeorde=morden.mord_numeorde and mfac.mord_numeorde="+numeroOrdenFAC+" and mfac.pdoc_prefordetrab='"+prefijoOrdenFAC+"' )");
				string observaciones = DBFunctions.SingleData("SELECT mfac_observacion FROM mfacturacliente WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+"");
				DateTime fechaVencimiento = Convert.ToDateTime(DBFunctions.SingleData("SELECT mfac_vence FROM mfacturacliente WHERE pdoc_codigo='"+codigoPrefijo+"' AND mfac_numedocu="+numeroOrden+""));
				for(int i=0;i<numRows;i++)
				{
					TableRow r = new TableRow();
					for(int j=0;j<numCells;j++)
					{
						TableCell c = new TableCell();
						if(i==0 && j==0)
							c.Text = "Dias de Vigencia : ";
						if(i==0 && j==1)
							c.Text = diasVigencia;
						if(i==1 && j==0)
							c.Text = "Fecha de Vencimiento : ";
						if(i==1 && j==1)
							c.Text = fechaVencimiento.ToString("yyyy-MM-dd");
						if(i==2 && j==0)
							c.Text = "Centro de Costos : ";
						if(i==2 && j==1)
							c.Text = centroCosto;
						if(i==3 && j==0)
							c.Text = "Observaciones : ";
						if(i==3 && j==1)
							c.Text = observaciones;
						if(i==4 && j==0)
							c.Text = "Recepcionista : ";
						if(i==4 && j==1)
							c.Text = recepcionista;
						if(i==5 && j==0)
							c.Text = "Terminos : ";
						if(i==5 && j==1)
							c.Text = DBFunctions.SingleData("SELECT pdoc_observacion FROM pdocumento WHERE pdoc_codigo='"+codigoPrefijo+"'");
						c.Width = new Unit("50%");
						r.Cells.Add(c);
					}
					tabFooterTerminos.Rows.Add(r);
				}
				tabFooterTerminos.BorderWidth = new Unit("1");
        		//tabFooterTerminos.Width = operaciones.Width;
			}
		}
				
		protected void Preparar_Tabla_Operaciones()
		{
			tablaOperaciones = new DataTable();
			tablaOperaciones.Columns.Add(new DataColumn("DESCRIPCIÓN",System.Type.GetType("System.String")));//0
			tablaOperaciones.Columns.Add(new DataColumn("CARGO",System.Type.GetType("System.String")));//1
			tablaOperaciones.Columns.Add(new DataColumn("CODIGO TEMPARIO",System.Type.GetType("System.String")));//2
			tablaOperaciones.Columns.Add(new DataColumn("TIEMPO",System.Type.GetType("System.Double")));//3
			tablaOperaciones.Columns.Add(new DataColumn("EXCENCIÓN IVA",System.Type.GetType("System.String")));//4
			tablaOperaciones.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));//5
			tablaOperaciones.Columns.Add(new DataColumn("MECANICO",System.Type.GetType("System.String")));//6
		}
		
		protected void Preparar_Tabla_Repuestos()
		{
			tablaRepuestos = new DataTable();
			tablaRepuestos.Columns.Add(new DataColumn("TRANSFERENCIA",System.Type.GetType("System.String")));//0
			tablaRepuestos.Columns.Add(new DataColumn("REFERENCIA",System.Type.GetType("System.String")));//1
			tablaRepuestos.Columns.Add(new DataColumn("DESCRIPCIÓN",System.Type.GetType("System.String")));//2
			tablaRepuestos.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.Double")));//3
			tablaRepuestos.Columns.Add(new DataColumn("VALOR UNITARIO",System.Type.GetType("System.String")));//4
			tablaRepuestos.Columns.Add(new DataColumn("PORCENTAJE IVA",System.Type.GetType("System.String")));//5
			tablaRepuestos.Columns.Add(new DataColumn("DESCUENTO",System.Type.GetType("System.String")));//6
			tablaRepuestos.Columns.Add(new DataColumn("TOTAL ITEM",System.Type.GetType("System.String")));//7
			tablaRepuestos.Columns.Add(new DataColumn("CARGO",System.Type.GetType("System.String")));//8
		}
		
		protected DataTable Preparar_Tabla_Peritaje(DataTable tablaAsociada, string grupoPeritaje)
		{
			tablaAsociada.Columns.Add(new DataColumn(grupoPeritaje,System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("ESTADO",System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("DETALLE",System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("COSTO",System.Type.GetType("System.String")));
			return tablaAsociada;
		}
		
		protected void LLenar_Grilla_Operaciones()
		{
			int i;
			Preparar_Tabla_Operaciones();
			DataSet operacionesOrden = new DataSet();
            Hashtable tipoOeraciones = new Hashtable();
            if (tipoProceso == "orden" || tipoProceso == "prel")
                DBFunctions.Request(operacionesOrden, IncludeSchema.NO, "SELECT ptem_operacion, tcar_cargo, dord_valooper, dord_tiemliqu, pven_codigo FROM dordenoperacion WHERE pdoc_codigo='" + codigoPrefijo + "' AND mord_numeorde=" + numeroOrden + " ");
            else if (tipoProceso == "liquidar")
                    DBFunctions.Request(operacionesOrden, IncludeSchema.NO, "SELECT ptem_operacion, tcar_cargo, dord_valooper, dord_tiemliqu, pven_codigo FROM dordenoperacion WHERE pdoc_codigo='" + prefijoOrdenFAC + "' AND mord_numeorde=" + numeroOrdenFAC + " AND tcar_cargo='" + cargoEspecial + "'");


            //if (tipoProceso == "orden" || tipoProceso == "prel")
            //{
            //    double sumando = 0;
            //    DBFunctions.Request(operacionesOrden, IncludeSchema.NO, "SELECT ptem_operacion, tcar_cargo, dord_valooper, dord_tiemliqu, pven_codigo FROM dordenoperacion WHERE pdoc_codigo='" + codigoPrefijo + "' AND mord_numeorde=" + numeroOrden + " ");
            //    //DataRow sumatoria = operacionesOrden.Tables[0].NewRow();
                
            //    //operacionesOrden.Tables[0].Rows.Add(sumatoria);
            //    for (int j = 0; j < operacionesOrden.Tables[0].Rows.Count ; j++)
            //    {
            //        if (operacionesOrden.Tables[0].Rows[j]["DORD_VALOOPER"].ToString() == null || operacionesOrden.Tables[0].Rows[j]["DORD_VALOOPER"].ToString() == "") sumando += 0;

            //        else
            //        sumando += Convert.ToDouble(operacionesOrden.Tables[0].Rows[j]["DORD_VALOOPER"]);
            //    }
                //sumatoria[0] = "TOTAL";
                //sumatoria[2] = sumando;
            //}
            for(i=0;i<operacionesOrden.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaOperaciones.NewRow();
                fila[0] = DBFunctions.SingleData(@"select TRIM(ptem_descripcion CONCAT ' ' CONCAT COALESCE(DOBSOT_OBSERVACIONES,'')) 
                                                    from PTEMPARIO P
                                                     LEFT JOIN DOBSERVACIONESOT D ON D.PDOC_CODIGO = '"+codigoPrefijo+@"' AND D.MORD_NUMEORDE = "+numeroOrden+@" AND P.PTEM_OPERACION = D.PTEM_OPERACION 
                                                   where P.PTEM_OPERACION = '" +operacionesOrden.Tables[0].Rows[i][0].ToString()+"' ");
				fila[1] = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='"+operacionesOrden.Tables[0].Rows[i][1].ToString()+"'");
				fila[2] =  operacionesOrden.Tables[0].Rows[i][0].ToString();
				string grupoCatalogo = DBFunctions.SingleData("SELECT pgru_grupo FROM pgrupocatalogo WHERE pgru_grupo=(SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa=(SELECT mcat_placa FROM mcatalogovehiculo WHERE mcat_vin=(SELECT mcat_vin FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+"))))");
				//Cambio hecho para que funcione con la operacion de peritaje
				if(tipoProceso=="liquidar")
				{
					if((DBFunctions.SingleData("SELECT ttra_codigo FROM morden WHERE pdoc_codigo='"+prefijoOrdenFAC+"' AND mord_numeorde="+numeroOrdenFAC+""))!="P")
					{
						try{fila[3] = Convert.ToDouble(operacionesOrden.Tables[0].Rows[i][3].ToString());}
						catch{fila[3] = 0;}
					}
					else
					{
						try{fila[3] = Convert.ToDouble(operacionesOrden.Tables[0].Rows[i][3].ToString());}
						catch{fila[3] = 0;}
					}
				}
				else
				{
					try{fila[3] = Convert.ToDouble(operacionesOrden.Tables[0].Rows[i][3].ToString());}catch{fila[3] = 0;}
				}
				//Fin Cambio
				string excencionIva = DBFunctions.SingleData("SELECT ptem_exceiva FROM ptempario WHERE ptem_operacion='"+operacionesOrden.Tables[0].Rows[i][0].ToString()+"'");
                if (fila[1].ToString() == "Alistamiento" || fila[1].ToString() == "Interno" || fila[1].ToString() == "Gtía Taller")
                    excencionIva = "S";
                double valorOperacion = Convert.ToDouble(operacionesOrden.Tables[0].Rows[i][2].ToString());
                if (operacionesOrden.Tables[0].Rows[i][1].ToString() == "G")
                {
                    string liqGarantiaFabrica = "S";
                    try { liqGarantiaFabrica = DBFunctions.SingleData("SELECT COALESCE(CTAL_IVALIQUGARA,'S') FROM ctaller"); }
                    catch { liqGarantiaFabrica = "S"; };
                    if (liqGarantiaFabrica == "N")
                        excencionIva="S";
                }
				double valorSalida = valorOperacion;
				if(excencionIva=="S")
					fila[4] = "SI";
				else if(excencionIva=="N")
				{
					double porcentajeIva = Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
                    
                  //  valorOperacion = valorSalida = (valorOperacion * 100)/(porcentajeIva+100);
                    double ivaOperacion = valorOperacion * ((porcentajeIva / 100));
                    totalIvaOperaciones += ivaOperacion;
                    totalIva += ivaOperacion;
                    valorSalida += ivaOperacion;
					fila[4] = "NO";	
				}
				totalParcial += valorOperacion;
				totalManoObra += valorOperacion;
                //totalManoObra += Math.Round(valorOperacion, numDecimales);
                fila[5] = Math.Round(valorOperacion, numDecimales).ToString("C");
				fila[6] = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+operacionesOrden.Tables[0].Rows[i][4].ToString()+"'");
				tablaOperaciones.Rows.Add(fila);
				Tools.DeterminarTipoOperacionSuma(operacionesOrden.Tables[0].Rows[i][0].ToString(), valorSalida,ref tipoOeraciones);
			}
			operaciones.DataSource = tablaOperaciones;			
			operaciones.DataBind();
			DatasToControls.JustificacionGrilla(operaciones,tablaOperaciones);
			if(nacionalidad=="E")
			{
				totalIvaOperaciones=0;
				totalIva=0;
			}
			//Ahora agregamos la informacion sobre la discrimacion de los valores de las operaciones
			TableRow tbr=new TableRow();
			TableCell tbc=new TableCell();
			tbc.Text="DISCRIMINACION DE COSTOS OPERACIONES";
			tbc.HorizontalAlign=HorizontalAlign.Center;
			tbc.ColumnSpan=4;
			tbr.Cells.Add(tbc);
			tabFooterDiscriminacionOperaciones.Rows.Add(tbr);

            TableRow r = new TableRow();
            TableCell c;
            int cellCount = 0;
            foreach (string key in tipoOeraciones.Keys)
            {
                if (cellCount % 4 == 0 && cellCount > 0)
                {
                    tabFooterDiscriminacionOperaciones.Rows.Add(r);
                    r = new TableRow();
                }
                c = new TableCell();
                c.Text = key + " : " + Math.Round((double)tipoOeraciones[key], numDecimales).ToString("C");

                c.Width = new Unit("25%");
                r.Cells.Add(c);
                cellCount++;
            }
            tabFooterDiscriminacionOperaciones.Rows.Add(r);
		}
		
		protected void Llenar_Grilla_Repuestos()
		{
			if((DBFunctions.SingleData("SELECT ttra_codigo FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+""))!="P")
			{
				Preparar_Tabla_Repuestos();
				DataSet repuestosOrden = new DataSet();

                string ivaExclLubricantes = "";
                try { ivaExclLubricantes = Convert.ToString(DBFunctions.SingleData("SELECT CINV_IVAEXCLLUBR FROM CINVENTARIO WHERE CINV_IVAEXCLLUBR = 'S'; ")); }
                catch { ivaExclLubricantes = "N"; }

                if (ivaExclLubricantes == "S")
                {
                    // MANEJO DE LOS ITEMS EXCLUIDOS DE INVENTARIO de IVA, como los aceites cuando NO tienen MANO DE OBRA son excludidos de IVA
                    // se detecta si hay alguna operacion del carga facturado con valor para cobrar IVA en los lubricantes
                    if (DBFunctions.RecordExist("SELECT DORD_VALOOPER FROM DORDENOPERACION, CINVENTARIO WHERE tcar_cargo = '" + cargoEspecial + @"' AND pdoc_codigo = '" + codigoPrefijo + @"' AND mord_numeorde = " + numeroOrden + @" and  DORD_VALOOPER > 0 ; "))
                        ivaExclLubricantes = "N";
                    if (ivaExclLubricantes == "S")
                        DBFunctions.NonQuery(@"UPDATE DITEMS SET PIVA_PORCIVA = 0 
                                WHERE pdoc_codigo||dite_numedocu||mite_codigo in 
                              (select dit.pdoc_codigo || dit.dite_numedocu || dit.mite_codigo  
                                 FROM mitems MIT, mordentransferencia MOT, ditems DIT
                                WHERE DIT.mite_codigo = MIT.mite_codigo  and mit.tori_codigo = 'Y' 
                                  AND DIT.pdoc_codigo = MOT.pdoc_factura AND DIT.dite_numedocu = MOT.mfac_numero AND DIT.piva_porciva > 0 AND MOT.pdoc_codigo = '" + codigoPrefijo + @"' AND MOT.mord_numeorde = " + numeroOrden + @" AND MOT.tcar_cargo = '" + cargoEspecial + @"' ) ");
                    else
                        DBFunctions.NonQuery(@"UPDATE DITEMS SET PIVA_PORCIVA = (SELECT CEMP_PORCIVA FROM CEMPRESA) 
                                WHERE pdoc_codigo||dite_numedocu||mite_codigo in 
                              (select dit.pdoc_codigo || dit.dite_numedocu || dit.mite_codigo  
                                 FROM mitems MIT, mordentransferencia MOT, ditems DIT
                                WHERE DIT.mite_codigo = MIT.mite_codigo  and mit.tori_codigo = 'Y' 
                                  AND DIT.pdoc_codigo = MOT.pdoc_factura AND DIT.dite_numedocu = MOT.mfac_numero AND DIT.piva_porciva = 0 AND MOT.pdoc_codigo = '" + codigoPrefijo + @"' AND MOT.mord_numeorde = " + numeroOrden + @" AND MOT.tcar_cargo = '" + cargoEspecial + @"' ) ");
                }

                if (tipoProceso=="orden")
				{
					DataSet pedidos = new DataSet();
					DBFunctions.Request(pedidos,IncludeSchema.NO,"SELECT * FROM mpedidotransferencia WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+" ORDER BY mped_numero ASC");
					string sqlQueries = "";
					for(int i=0;i<pedidos.Tables[0].Rows.Count;i++)
					{
						string prefijoTransferencia = pedidos.Tables[0].Rows[i][3].ToString();
						string codigoTransferencia = pedidos.Tables[0].Rows[i][4].ToString();
						string nitTransferencia = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidoitem WHERE pped_codigo='"+prefijoTransferencia+"' AND mped_numepedi="+codigoTransferencia+"");
						string claseRegistro = DBFunctions.SingleData("SELECT mped_claseregi FROM mpedidoitem WHERE pped_codigo='"+prefijoTransferencia+"' AND mped_numepedi="+codigoTransferencia+"");
						sqlQueries += @"SELECT DBXSCHEMA.EDITARREFERENCIAS(DPED.mite_codigo,PLIN.plin_tipo), DPED.dped_cantpedi, MORD.tcar_cargo, DPED.dped_valounit, DPED.pped_codigo CONCAT '-' CONCAT CAST(DPED.mped_numepedi AS character(20)),PLIN.plin_tipo 
                                        FROM  dpedidoitem DPED, mpedidoitem MPED, mpedidotransferencia MORD, mitems MIT, plineaitem PLIN  
                                        WHERE DPED.mped_clasregi='" + claseRegistro + @"' AND DPED.mnit_nit='" + nitTransferencia + @"' AND DPED.pped_codigo='" + prefijoTransferencia + @"' AND DPED.mped_numepedi=" + codigoTransferencia + @" AND MORD.pdoc_codigo='" + codigoPrefijo + @"' AND MORD.mord_numeorde=" + numeroOrden + @" AND MORD.pped_codigo=MPED.pped_codigo  
                                         AND  MORD.mped_numero=MPED.mped_numepedi AND MPED.mped_claseregi=DPED.mped_clasregi AND MPED.mnit_nit=DPED.mnit_nit AND MPED.pped_codigo=DPED.pped_codigo AND MPED.mped_numepedi=DPED.mped_numepedi AND DPED.mite_codigo=MIT.mite_codigo AND MIT.plin_codigo=PLIN.plin_codigo;";
						//													0										1				2					3					4																			5
                        /*
                        sqlQueries += "SELECT DBXSCHEMA.EDITARREFERENCIAS(DPED.mite_codigo,PLIN.plin_tipo), DPED.dped_cantpedi, MORD.tcar_cargo, DPED.dped_valounit, "+
                        "di.pdoc_codigo concat'-' CONCAT CAST(Di.dite_numedocu AS character(10)) concat DPED.pped_codigo CONCAT '-' CONCAT CAST(DPED.mped_numepedi AS character(20)),PLIN.plin_tipo,di.tcar_cargo, "+
                         "di.dite_cantidad - coalesce(did.dite_cantidad,0) as transferida "+
                        "FROM mpedidoitem mPED, mpedidotransferencia MORD, mitems MIT, plineaitem PLIN, dpedidoitem dPED "+
                        "left join ditems di on dped.pped_codigo = di.dite_prefdocurefe and dped.mped_numepedi = di.dite_numedocurefe and dped.mite_codigo = di.mite_codigo "+
                        "left join ditems did on di.pdoc_codigo = did.dite_prefdocurefe and di.dite_numedocu = did.dite_numedocurefe and di.mite_codigo = did.mite_codigo "+
                        "WHERE DPED.mped_clasregi='"+claseRegistro+"' AND DPED.mnit_nit='"+nitTransferencia+"' AND DPED.pped_codigo='"+prefijoTransferencia+"' "+
                        " AND DPED.mped_numepedi="+codigoTransferencia+" AND MORD.pdoc_codigo='"+codigoPrefijo+"' AND MORD.mord_numeorde="+numeroOrden+" "+
                        " AND MORD.pped_codigo=MPED.pped_codigo AND MORD.mped_numero=MPED.mped_numepedi AND MPED.mped_claseregi=DPED.mped_clasregi "+
                        " AND MPED.mnit_nit=DPED.mnit_nit AND MPED.pped_codigo=DPED.pped_codigo AND MPED.mped_numepedi=DPED.mped_numepedi AND DPED.mite_codigo=MIT.mite_codigo "+
                        " AND MIT.plin_codigo=PLIN.plin_codigo; ";

                         */
                    }
					DBFunctions.Request(repuestosOrden,IncludeSchema.NO,sqlQueries);
				}
				else if(tipoProceso=="liquidar")
					    DBFunctions.Request(repuestosOrden,IncludeSchema.NO, "SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo),DIT.dite_cantidad, MOT.tcar_cargo, DIT.dite_valounit,DIT.pdoc_codigo CONCAT '-' CONCAT CAST(DIT.dite_numedocu AS CHARACTER(7)), PLIN.plin_tipo,DIT.mite_codigo,DIT.piva_porciva,DIT.dite_porcdesc, mite_nombre FROM ditems DIT, mordentransferencia MOT, mitems MIT, plineaitem PLIN WHERE DIT.mite_codigo = MIT.mite_codigo AND MIT.plin_codigo = PLIN.plin_codigo AND MOT.pdoc_factura = DIT.pdoc_codigo AND MOT.mfac_numero = DIT.dite_numedocu AND MOT.pdoc_codigo = '" + prefijoOrdenFAC+"' AND MOT.mord_numeorde = "+numeroOrdenFAC+" AND MOT.tcar_cargo = '"+cargoEspecial+"'");
				else if(tipoProceso=="prel")
					    DBFunctions.Request(repuestosOrden,IncludeSchema.NO, "SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo),DIT.dite_cantidad, MOT.tcar_cargo, DIT.dite_valounit,DIT.pdoc_codigo CONCAT '-' CONCAT CAST(DIT.dite_numedocu AS CHARACTER(7)), PLIN.plin_tipo,DIT.mite_codigo,DIT.piva_porciva,DIT.dite_porcdesc, mite_nombre FROM ditems DIT, mordentransferencia MOT, mitems MIT, plineaitem PLIN WHERE DIT.mite_codigo = MIT.mite_codigo AND MIT.plin_codigo = PLIN.plin_codigo AND MOT.pdoc_factura = DIT.pdoc_codigo AND MOT.mfac_numero = DIT.dite_numedocu AND MOT.pdoc_codigo = '" + codigoPrefijo+"' AND MOT.mord_numeorde = "+numeroOrden+"");
					//																					0											1					2				3												4											5				6				7				8
					//"SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo),DIT.dite_cantidad, MOT.tcar_cargo, DIT.dite_valounit,DIT.pdoc_codigo CONCAT '-' CONCAT CAST(DIT.dite_numedocu AS CHARACTER(7)), PLIN.plin_tipo,DIT.mite_codigo,DIT.piva_porciva,DIT.dite_porcdesc FROM ditems DIT, mordentransferencia MOT, mitems MIT, plineaitem PLIN WHERE DIT.mite_codigo = MIT.mite_codigo AND MIT.plin_codigo = PLIN.plin_codigo AND MOT.pdoc_factura = DIT.pdoc_codigo AND MOT.mfac_numero = DIT.dite_numedocu AND MOT.pdoc_codigo = '"+prefijoOrdenFAC+"' AND MOT.mord_numeorde = "+numeroOrdenFAC+" AND MOT.tcar_cargo = '"+cargoEspecial+"'"
				//																					0												1					2				3								4															5				6				7				8		
				for(int j=0;j<repuestosOrden.Tables.Count;j++)
				{
					////////////////////////////////////////////////////////////////////////
					for(int i=0;i<repuestosOrden.Tables[j].Rows.Count;i++)
					{
						if(tipoProceso=="liquidar" || tipoProceso=="prel")
						{
							double cantidad = Items.ConsultaRelacionItemsTransferencias(
                                repuestosOrden.Tables[j].Rows[i][6].ToString(),
                                (repuestosOrden.Tables[j].Rows[i][4].ToString().Split('-'))[0],
								Convert.ToInt64((repuestosOrden.Tables[j].Rows[i][4].ToString().Split('-'))[1].Trim()),
                                80,81 );
							if(cantidad>0)
							{
								//Esta sujeto a cambios dependiendo  de como se guarden las transferencias
								DataRow fila = tablaRepuestos.NewRow();
								fila[0] = repuestosOrden.Tables[j].Rows[i][4].ToString();
								fila[1] = repuestosOrden.Tables[j].Rows[i][0].ToString();
								string codI = "";
								Referencias.Guardar(repuestosOrden.Tables[j].Rows[i][0].ToString(), ref codI,repuestosOrden.Tables[j].Rows[i][5].ToString());
								fila[2] = repuestosOrden.Tables[j].Rows[i][9].ToString();
                                fila[3] = cantidad;

								double precioItem = 0,precioItemDesc=0,precioItemIva=0;
							    //precioItem = Math.Round(Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][3].ToString()), numDecimales);
                                precioItem = Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][3].ToString());
								fila[4] = precioItem.ToString("C");
                                double porcentajeIva = 0;
                                string liqGarantiaFabrica = "S";
                                try { liqGarantiaFabrica = DBFunctions.SingleData("SELECT COALESCE(CTAL_IVALIQUGARA,'S') FROM ctaller"); }
                                catch { liqGarantiaFabrica = "S"; };
                                if (repuestosOrden.Tables[j].Rows[i][2].ToString() == "G" && liqGarantiaFabrica == "N")
                                     porcentajeIva = 0;
                                else porcentajeIva = Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][7].ToString());
                                fila[5] = porcentajeIva.ToString()+"%";
								//Precio*cantidad
								precioItem=precioItem*Convert.ToDouble(cantidad);
								//Precio*cantidad-descuento
								precioItemDesc=precioItem-(precioItem*(Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][8])/100));
								//Precio*cantidad-descuento+iva
								if(nacionalidad!="E")
									precioItemIva=precioItemDesc + (precioItemDesc*(porcentajeIva/100));
								else
									precioItemIva=precioItemDesc;
								//fila[6] = ((precioItem + (precioItem*(porcentajeIva/100))) * Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][1])).ToString("C");
								if(nacionalidad!="E")
									fila[6]=Convert.ToDouble(repuestosOrden.Tables[0].Rows[i][8]).ToString()+"%";
								else
									fila[6] = (0).ToString()+"%";
								if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
                                    fila[7] = Math.Round(precioItemIva, numDecimales).ToString("C");
								else
									fila[7] = precioItemIva.ToString("C");
								fila[8] = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo = '"+repuestosOrden.Tables[0].Rows[i][2].ToString()+"'");
                                
                                totalRepuestos += precioItemDesc;
								totalIvaRepuestos += (precioItemDesc*(porcentajeIva/100));
								totalIva += (precioItemDesc*(porcentajeIva/100));
								tablaRepuestos.Rows.Add(fila);
							}
							if(nacionalidad=="E")
							{
								totalIvaRepuestos=0;
								totalIva=0;
							}
						}
						else if(tipoProceso=="orden")
						{
							//Esta sujeto a cambios dependiendo  de como se guarden las transferencias
							DataRow fila = tablaRepuestos.NewRow();
							fila[0] = repuestosOrden.Tables[j].Rows[i][4].ToString();
							fila[1] = repuestosOrden.Tables[j].Rows[i][0].ToString();
							string codI = "";
							Referencias.Guardar(repuestosOrden.Tables[j].Rows[i][0].ToString(), ref codI,repuestosOrden.Tables[j].Rows[i][5].ToString());
							fila[2] = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
							fila[3] = Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][1]);
							double precioItem = 0;
							precioItem = Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][3].ToString());
							fila[4] = precioItem.ToString("C");
							double porcentajeIva = Convert.ToDouble(DBFunctions.SingleData("SELECT piva_porciva FROM mitems WHERE mite_codigo='"+codI+"'"));
							fila[5] = porcentajeIva.ToString()+"%";
							fila[6]="0%";
							fila[7] = (((precioItem + (precioItem*(porcentajeIva/100))) * Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][1]))).ToString("C");
							totalRepuestos += precioItem * Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][1]);
							totalIvaRepuestos += (precioItem*(porcentajeIva/100)) * Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][1]);
							totalIva += (precioItem*(porcentajeIva/100)) * Convert.ToDouble(repuestosOrden.Tables[j].Rows[i][1]);
							tablaRepuestos.Rows.Add(fila);

						}
                        
					}
                    //DataRow sumatoriaRespuestos = tablaRepuestos.NewRow();
                    //tablaRepuestos.Rows.Add(sumatoriaRespuestos);
                    //double sumandoRespuestos = 0;
                    //double sumandoRespuestosIva = 0;
                    //for (int b = 0; b < tablaRepuestos.Rows.Count - 1; b++)
                    //{
                    //    if (tablaRepuestos.Rows[b][4].ToString() == "" && tablaRepuestos.Rows[b][7].ToString() == "")
                    //    {
                    //        sumandoRespuestos += 0;
                    //        sumandoRespuestosIva += 0;
                    //    }
                    //    else
                    //    {
                    //        sumandoRespuestos += Convert.ToDouble(tablaRepuestos.Rows[b][4].ToString().Substring(1));
                    //        sumandoRespuestosIva += Convert.ToDouble(tablaRepuestos.Rows[b][7].ToString().Substring(1));
                    //    }
                    //}
                    //sumatoriaRespuestos[0] = "TOTAL";
                    //sumatoriaRespuestos[4] = sumandoRespuestos.ToString("C");
                    //sumatoriaRespuestos[7] = sumandoRespuestosIva.ToString("C");
					////////////////////////////////////////////////////////////////////////
				}
				repuestos.DataSource = tablaRepuestos;
				//Aplicar_Formato(repuestos,tablaRepuestos);
				repuestos.DataBind();
				DatasToControls.JustificacionGrilla(repuestos,tablaRepuestos);
			}
		}



      


        //Esta funcion nos permite cargar la tabla especifica para poder guardar las operaciones especificas 
        protected void Preparar_Tabla_Peritaje()
        {
            operacionesPeritajeGrabar = new DataTable();
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("CODIGOPREFIJO", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("NUMEROORDEN", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("GRUPOPERITAJE", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("ITEMPERITAJE", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("ESTADO", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("DETALLE", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("COSTO", System.Type.GetType("System.String")));
        }


        protected void Distribuir_Tabla_Peritaje(PlaceHolder gruposPeritaje, DataTable[] tablasAsociadas, ArrayList codigosGruposPeritaje, int cantidadGrillas, string codigoPrefijo, string numeroOrden)
        {
            this.Preparar_Tabla_Peritaje();
            for (int i = 0; i < cantidadGrillas; i++)
            {
                int cantidadOperaciones = tablasAsociadas[i].Rows.Count;
                for (int j = 0; j < cantidadOperaciones; j++)
                {
                    DataRow fila = operacionesPeritajeGrabar.NewRow();
                    fila[0] = codigoPrefijo;
                    fila[1] = numeroOrden;
                    fila[2] = codigosGruposPeritaje[i].ToString();
                    fila[3] = DBFunctions.SingleData("SELECT pitp_codigo FROM pitemperitaje WHERE pitp_descripcion='" + tablasAsociadas[i].Rows[j][0].ToString() + "' AND pgrp_codigo='" + codigosGruposPeritaje[i].ToString() + "'");
                    fila[4] = DBFunctions.SingleData("SELECT tespe_codigo FROM testadoperitaje WHERE tespe_descripcion='" + ((RadioButtonList)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[1].Controls[0]).SelectedItem.ToString() + "'");
                    if ((((TextBox)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[2].Controls[0]).Text) == "")
                        fila[5] = "";
                    else
                        fila[5] = ((TextBox)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[2].Controls[0]).Text;
                    if ((((TextBox)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[3].Controls[0]).Text) == "")
                        fila[6] = "null";
                    else
                        fila[6] = ((TextBox)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[3].Controls[0]).Text;
                    operacionesPeritajeGrabar.Rows.Add(fila);
                }
            }
        }
		
		protected void Llenar_Grillas_Peritaje()
		{
			if((DBFunctions.SingleData("SELECT ttra_codigo FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+""))=="P")
			{
				double totalPeritaje = 0;
				peritaje.Visible = true;
				//Creamos un DataSet que contenga los grupos de peritaje
				DataSet gruposPeritajeConstruccion = new DataSet();
				DBFunctions.Request(gruposPeritajeConstruccion,IncludeSchema.NO,"SELECT * FROM pgrupoperitaje");
				for(int i=0;i<gruposPeritajeConstruccion.Tables[0].Rows.Count;i++)
				{
					//Ahora Creamos Un DataSet que contenga los items que se encuentran en la tabla dordenperitaje que cumplan con la condicion del grupo
					DataSet itemsPeritajeEspecificos = new DataSet();
					DBFunctions.Request(itemsPeritajeEspecificos,IncludeSchema.NO,"SELECT pitp_codigo, tespe_codigo, dorpe_detalle, COALESCE(dorpe_costo,0) AS COSTO FROM dordenperitaje WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden+" AND pgrp_codigo='"+gruposPeritajeConstruccion.Tables[0].Rows[i][0].ToString()+"' ORDER BY pitp_codigo");
					//Ahora creamos la tabla asociada y el datagrid que vamos a mostrar en pantalla
					DataTable tablaAsociada = new DataTable();
					tablaAsociada = this.Preparar_Tabla_Peritaje(tablaAsociada,gruposPeritajeConstruccion.Tables[0].Rows[i][1].ToString());
					DataGrid grilla = new DataGrid();
					//grilla.Width = new Unit(780);
					peritaje.Controls.Add(new LiteralControl("<br>"));
					peritaje.Controls.Add(grilla);
					for(int j=0;j<itemsPeritajeEspecificos.Tables[0].Rows.Count;j++)
					{
						DataRow fila = tablaAsociada.NewRow();
						fila[gruposPeritajeConstruccion.Tables[0].Rows[i][1].ToString()] = DBFunctions.SingleData("SELECT pitp_descripcion FROM pitemperitaje WHERE pitp_codigo='"+itemsPeritajeEspecificos.Tables[0].Rows[j][0].ToString()+"'");
						fila["ESTADO"]  = DBFunctions.SingleData("SELECT tespe_descripcion FROM testadoPeritaje WHERE tespe_codigo='"+itemsPeritajeEspecificos.Tables[0].Rows[j][1].ToString()+"'");
						fila["DETALLE"] = itemsPeritajeEspecificos.Tables[0].Rows[j][2].ToString();
						fila["COSTO"]   = 0;
                        if ((itemsPeritajeEspecificos.Tables[0].Rows[j][3].ToString()) != "")
                        {
                            fila["COSTO"] = System.Convert.ToDouble(itemsPeritajeEspecificos.Tables[0].Rows[j][3]).ToString("C");
                            totalPeritaje += System.Convert.ToDouble(itemsPeritajeEspecificos.Tables[0].Rows[j][3].ToString().Trim());
                        }
						tablaAsociada.Rows.Add(fila);
					}
					DatasToControls.Aplicar_Formato_Grilla(grilla);
					grilla.DataSource = tablaAsociada;
					grilla.DataBind();
				}
				//Ahora Construimos el Label que contiene el total del peritaje					
				Label total = new Label();
				double porcentajeIva = System.Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
				double iva = totalPeritaje*(porcentajeIva/100);
				peritaje.Controls.Add(new LiteralControl("<br>"));
				peritaje.Controls.Add(total);
				total.Text = "SUB-TOTAL : "+totalPeritaje.ToString("C");
				total.Text += "<br>IVA : "+iva.ToString("C");
				total.Text += "<br>TOTAL : "+(totalPeritaje + iva).ToString("C");
				//Fin de construccion
				//Titulo de Peritaje
				TableRow row = new TableRow();
				TableCell cell = new TableCell();
				cell.Text = "OPERACIONES DE PERITAJE";
				row.Cells.Add(cell);
				tituloPeritaje.Rows.Add(row);
				tituloPeritaje.Visible = true;
			}
		}
		
		protected void Aplicar_Formato(DataGrid dgSource, DataTable dtSource)
		{
			for(int i=0;i<dtSource.Columns.Count;i++)
			{
				if(dtSource.Columns[i].DataType == typeof(double))
				{
					for(int j=0;j<dgSource.Items.Count;j++)
						dgSource.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
				}
			}
		}

		private void RevisionImpresionPDF()
		{
			if(Request["ImpFac"] != null && Request["ImpFac"] != string.Empty)
			{
				try
				{
					formatoRecibo.Prefijo = Request["prefOT"];
					formatoRecibo.Numero = Convert.ToInt32(Request["numOT"]);
					formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+ Request["prefOT"] +"'");
					if(formatoRecibo.Codigo!=string.Empty)
					{
						if(formatoRecibo.Cargar_Formato())
							Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=600');</script>");
					}
				}
				catch
				{
                    Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
				}
			}
            else if (Request["ImpOrd"] != null && Request["ImpOrd"] != string.Empty)
            {
                try
                {
                    if (ConfigurationManager.AppSettings["ImprimirPreliquidacionOT"] == "true")
                    {
                        Hashtable parametros = new Hashtable();
                        parametros.Add("PREFIJO", codigoPrefijo);
                        parametros.Add("NUMERO", numeroOrden);
                        parametros.Add("CARGO", cargoEspecial);

                        //string documento = Imprimir.ImprimirRPT("ams.taller.preliquidacion", parametros);
                        string preliq = "PRELIQ"; //INSERTAR EN TODAS LAS BASES DE DATOS EL DOCUMENTO DE LA PRELIQUIDACION DE LA OT 

                        string documento = "";
                        try
                        {
                            string formato = DBFunctions.SingleData("SELECT SFOR_NOMBRPT FROM dbxschema.SFORMATODOCUMENTOCRYSTAL WHERE sfor_codigo='" + preliq + "'");
                            documento = Imprimir.ImprimirRPT(formato, parametros);
                        }
                        catch { documento = Imprimir.ImprimirRPT("ams.taller.preliquidacion", parametros); };                        
                        Response.Write("<script language:javascript>w=window.open('" + documento + "','','HEIGHT=600,WIDTH=600');</script>");
                    }
                }
                catch (Exception ex)
                {
                    Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + ex.InnerException + "");
                }
            }
		}

        private void LlenarRegistroVisita()
        {

            ViewState["tipo"] = Request.QueryString["tipo"];
            Tipo = (string)ViewState["tipo"];
            ViewState["tiempoIni"] = DateTime.Now;
            actividad = Request.QueryString["Activ"];

            lblNumero.Text = DBFunctions.SingleData("SELECT MAX(dmar_numeacti)+1 from dBxschema.dmarketing;");
            Tipo = (string)ViewState["tipo"];

            if (tipoProceso == "orden" || tipoProceso == "prel")
                lblVendedor.Text = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM morden WHERE pdoc_codigo='" + codigoPrefijo + "' AND mord_numeorde=" + numeroOrden + ")");
            else if (tipoProceso == "liquidar")
                lblVendedor.Text = DBFunctions.SingleData("SELECT pven_nombre FROM dbxschema.pvendedor WHERE pven_codigo=(select morden.pven_codigo from dbxschema.mfacturaclientetaller mfac, dbxschema.morden morden where  mfac.pdoc_prefordetrab=morden.pdoc_codigo and mfac.mord_numeorde=morden.mord_numeorde and mfac.mord_numeorde=" + numeroOrdenFAC + " and mfac.pdoc_prefordetrab='" + prefijoOrdenFAC + "' )");

            lblCliente.Text = DBFunctions.SingleData("SELECT mnit_apellidos || ' ' || mnit_nombres FROM mnit WHERE mnit_nit='" + nit + "'");

            DatasToControls bind = new DatasToControls();
            //lblVendedor.Text = DBFunctions.SingleData("SELECT pven_nombre FROM dbxschema.pvendedor WHERE pven_codigo=(select morden.pven_codigo from dbxschema.mfacturaclientetaller mfac, dbxschema.morden morden where  mfac.pdoc_prefordetrab=morden.pdoc_codigo and mfac.mord_numeorde=morden.mord_numeorde and mfac.mord_numeorde="+numeroOrdenFAC+" and mfac.pdoc_prefordetrab='"+prefijoOrdenFAC+"' )");
            lblFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (actividad != null)
                bind.PutDatasIntoDropDownList(ddlActividad, "SELECT PACT_CODIMARK, PACT_NOMBMARK  FROM DBXSCHEMA.PACTIVIDADMARKETING WHERE PACT_VIGENTE='S' AND TACT_TIPOACTI='OT' ORDER BY PACT_NOMBMARK;");
            else
                bind.PutDatasIntoDropDownList(ddlActividad, "SELECT PACT_CODIMARK, PACT_NOMBMARK  FROM DBXSCHEMA.PACTIVIDADMARKETING WHERE PACT_VIGENTE='S' AND TACT_TIPOACTI = 'OT'  ORDER BY PACT_NOMBMARK;");

            bind.PutDatasIntoDropDownList(ddlResultado, "SELECT PRES_CODIGO, PRES_DESCRIPCION FROM DBXSCHEMA.PRESULTADOACTIVIDAD WHERE PRES_VIGENTE='S' ORDER BY PRES_DESCRIPCION;");

          
            if (Tipo == "C")
                bind.PutDatasIntoDropDownList(ddlMercadeista, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM DBXSCHEMA.PVENDEDOR           WHERE TVEND_CODIGO='MC' AND PVEN_VIGENCIA='V' ORDER BY PVEN_NOMBRE;");
            else
                bind.PutDatasIntoDropDownList(ddlMercadeista, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM DBXSCHEMA.PVENDEDOR           WHERE TVEND_CODIGO<>'MG' AND PVEN_VIGENCIA='V' ORDER BY PVEN_NOMBRE;");

        }
        protected void btnGuardarRegistroC_Click(object sender, System.EventArgs e)
		{
			ArrayList sqlUpd=new ArrayList();
			string numero=DBFunctions.SingleData("SELECT MAX(dmar_numeacti)+1 from dBxschema.dmarketing;");
			string fechaProx=(txtFechaProx.PostedDate == "" ? "NULL" : "'" + txtFechaProx.SelectedDate.ToString("yyyy-MM-dd") + "'");
            DateTime fchProx = DateTime.Now; 

            TimeSpan tRes = DateTime.Now - (DateTime)ViewState["tiempoIni"];
            int minD = (int) Math.Round(tRes.TotalMinutes);
            minD = minD == 0 ? 1 : minD;

            if (txtFechaProx.PostedDate.Length > 0)
            {
                fchProx = txtFechaProx.SelectedDate;
                if (fchProx < DateTime.Now.AddDays(-1))
                {
                    Utils.MostrarAlerta(Response, "Fecha próxima no válida, debe ser menor o igual a la fecha actual.");
                    return;
                }
            }

            //Insertar MCLIENTE si no existe
            string sqlCliente = "INSERT INTO MCLIENTE VALUES(" +
                    "'{0}',NULL,NULL,0,0,NULL,NULL," +
                    "NULL,NULL,NULL,NULL,NULL,NULL,'N',0,NULL,'{1}'," +
                    "NULL,NULL,NULL,'1','1',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);";

            if (!DBFunctions.RecordExist("SELECT * FROM MCLIENTE WHERE MNIT_NIT='" + nit + "';"))
                sqlUpd.Add(String.Format(sqlCliente, nit, DateTime.Now.ToString("yyyy-MM-dd")));

            string idVen = Request.QueryString["lblVendedor.Text"];
            if (idVen == null || idVen == "") idVen = ddlMercadeista.SelectedValue;

            sqlUpd.Add("INSERT INTO DBXSCHEMA.DMARKETING VALUES(DEFAULT,NULL,'" + nit + "','" + ddlActividad.SelectedValue + "','"
            + txtDetalle.Text.Replace("'", "''").Trim() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + idVen + "','" + ddlResultado.SelectedValue + "'," + 
            fechaProx + ",'" + ddlMercadeista.SelectedValue + "'," + minD + ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + codigoPrefijo + "'," + numeroOrden + ");");

			if(DBFunctions.Transaction(sqlUpd))
			{
                Utils.MostrarAlerta(Response, "Registro Guardado");
			}   
			else
				lb.Text+="Error "+DBFunctions.exceptions;
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
