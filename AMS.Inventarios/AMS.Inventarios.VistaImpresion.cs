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
using System.Xml;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Forms;

namespace AMS.Inventarios
{
	public partial class VistaImpresion : System.Web.UI.UserControl
	{
		protected string prefijoFactura, numeroFactura, cliente, origen;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			prefijoFactura = Request.QueryString["prefFact"];
			numeroFactura = Request.QueryString["numFact"];
			cliente = Request.QueryString["cliente"];
			origen = Request.QueryString["orig"];
			CargarHeader();
			CargarInfoFact();
			CargarDgRepuestos();
		}
		
		protected void CargarHeader()
		{
			int i,j;
			int numRows = 3, numCells = 3;
			for(i=0;i<numRows;i++)
			{
				TableRow r = new TableRow();
				for(j=0;j<numCells;j++)
				{
					TableCell c = new TableCell();
					if(i==0 && j == 1)
					{
						c.Text = DBFunctions.SingleData("SELECT cemp_nombre FROM cempresa");
						c.Text+= "<br>NIT: "+DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
						c.HorizontalAlign = HorizontalAlign.Center;
					}
					if(i==1 && j == 1)
					{
						c.HorizontalAlign = HorizontalAlign.Center;
						c.Text = "Factura de Repuestos : "+prefijoFactura+"-"+numeroFactura;
					}
					if(i==2 && j==0)
					{
						c.CssClass = "LetraMenuda";
						c.Text = "<br>IVA REGIMEN COMUN";
						c.Text += "<br>SOMOS GRANDES CONTRIBUYENTES RETENEDOR DE IVA";
						c.Text += "<br>NO SOMOS AUTO-RETENEDORES. SOMOS GRANDES CONTRIBUYENTES";
						c.Text += "<br>AUTO-RETENEDOR DE ICA RESOLUCION Nº";
					}
					if(i==2 && j==1)
					{
						c.HorizontalAlign = HorizontalAlign.Center;
						if(cliente == "C")
						{
                            c.Text += "Dirección : " + DBFunctions.SingleData("SELECT palm_direccion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + ")");
                            c.Text += "<br>" + DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + "))") + " - " + DBFunctions.SingleData("SELECT ppai_nombre FROM ppais WHERE ppai_pais=(SELECT ppai_pais FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + ")))");
                            c.Text += "<br>Telefonos : " + DBFunctions.SingleData("SELECT palm_telefono1 FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + ")") + " - " + DBFunctions.SingleData("SELECT palm_telefono2 FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + ")");
						}
						else if(cliente == "P")
						{
							c.Text += "Dirección : "+DBFunctions.SingleData("SELECT mnit_direccion FROM mnit WHERE mnit_nit IN (SELECT mnit_nit FROM cempresa)");
							c.Text += "<br>"+DBFunctions.SingleData("SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM mnit WHERE mnit_nit IN (SELECT mnit_nit FROM cempresa))")+" - "+DBFunctions.SingleData("SELECT ppai_nombre FROM ppais WHERE ppai_pais=(SELECT ppai_pais FROM pciudad WHERE pciu_codigo=(SELECT pciu_nombre FROM pciudad WHERE pciu_codigo=(SELECT pciu_codigo FROM mnit WHERE mnit_nit IN (SELECT mnit_nit FROM cempresa))))");
							c.Text += "<br>Telefono : "+DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit IN (SELECT mnit_nit FROM cempresa)");
						}
					}
					if(i==2 && j==2)
					{
						c.Text = "<img src='../img/AMS.LogoEmpresa.png'>";
						c.HorizontalAlign = HorizontalAlign.Right;
					}
					c.Width = new Unit("33%");
					r.Cells.Add(c);
				}
				tabHeaderEmpresa.Rows.Add(r);
			}
		}
		
		protected void CargarInfoFact()
		{
			int i;
			int numCells = 2;
			TableRow r = new TableRow();
			string nitCliente = "";
			if(cliente == "C")
				nitCliente = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+"");
			else if(cliente == "P")
				nitCliente = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
			for(i=0;i<numCells;i++)
			{
				TableCell c = new TableCell();
				if(i == 0)
				{
					c.Text += "INFORMACIÓN CLIENTE :";
					c.Text += "<br>Nombre Cliente : " + DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+nitCliente+"'");
					c.Text += "<br>Nit Cliente : "+nitCliente;
					c.Text += "<br>Telefonos : " + DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit='"+nitCliente+"'");
					c.Text += "<br>Dirección : " + DBFunctions.SingleData("SELECT mnit_direccion FROM mnit WHERE mnit_nit='"+nitCliente+"'");
					c.Text += "<br>Email : " + DBFunctions.SingleData("SELECT mnit_email FROM mnit WHERE mnit_nit='"+nitCliente+"'");
				}
				if(i == 1)
				{
					c.Text += "INFORMACIÓN FACTURA :";
					if(cliente == "C")
					{
						c.Text += "<br>Fecha Factura : " + Convert.ToDateTime(DBFunctions.SingleData("SELECT mfac_factura FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+""));
						c.Text += "<br>Fecha Vencimiento : " + Convert.ToDateTime(DBFunctions.SingleData("SELECT mfac_vence FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+""));
						c.Text += "<br>Observación : " + DBFunctions.SingleData("SELECT mfac_observacion FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+"");
					}
					else if(cliente == "P")
					{
						c.Text += "<br>Fecha Factura : " + Convert.ToDateTime(DBFunctions.SingleData("SELECT mfac_factura FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+""));
						c.Text += "<br>Fecha Vencimiento : " + Convert.ToDateTime(DBFunctions.SingleData("SELECT mfac_vence FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+""));
						c.Text += "<br>Observación : " + DBFunctions.SingleData("SELECT mfac_observacion FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
					}
				}
				c.Width = new Unit("50%");
				r.Cells.Add(c);
			}
			tabHeaderEmpresa.Rows.Add(r);
		}

		private void CargarDgRepuestos()
		{
			DataTable dtDetalle = Items.ConsultarDetalleFactura(prefijoFactura,numeroFactura);
			dgRepuestos.DataSource = dtDetalle;
			dgRepuestos.DataBind();
			double valorSinIva = 0, valorIva = 0;
			//Ahora calculamos el total de la factura, el total del iva y la suma de los dos para obtener el gran total
			for(int i=0;i<dtDetalle.Rows.Count;i++)
			{
				valorSinIva += Convert.ToDouble(dtDetalle.Rows[i]["CANTIDAD"])*Convert.ToDouble(dtDetalle.Rows[i]["VALOR_UNIDAD"]);
				valorIva += Convert.ToDouble(dtDetalle.Rows[i]["VALOR_IVA"]);
			}
			int numRows = 3, numCells = 2;
			for(int i=0;i<numRows;i++)
			{
				TableRow r = new TableRow();
				for(int j=0;j<numCells;j++)
				{
					TableCell c = new TableCell();
					if(i==0 && j==0)
					{
						c.Width = new Unit("70%");
						c.Text = "SubTotal :&nbsp;&nbsp;&nbsp;";
					}
					if(i==0 && j==1)
					{
						c.Width = new Unit("30%");
						c.HorizontalAlign = HorizontalAlign.Right;
						c.Text = valorSinIva.ToString("C");
					}
					if(i==1 && j==0)
						c.Text = "Valor Iva :&nbsp;&nbsp;&nbsp;";
					if(i==1 && j==1)
					{
						c.HorizontalAlign = HorizontalAlign.Right;
						c.Text = valorIva.ToString("C");
					}
					if(i==2 && j==0)
						c.Text = "Total :&nbsp;&nbsp;&nbsp;";
					if(i==2 && j==1)
					{
						c.HorizontalAlign = HorizontalAlign.Right;
						c.Text = (valorSinIva + valorIva).ToString("C");
					}
					r.Cells.Add(c);
				}
				tabHeaderFactura.Rows.Add(r);
			}
			tabHeaderFactura.Width = dgRepuestos.Width;
			tabHeaderFactura.BorderWidth = new Unit("1");
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
