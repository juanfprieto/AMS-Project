// created on 29/04/2005 at 14:00
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
	public partial class CierreMensual : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				if(Request.QueryString["ext"]!=null)
                Utils.MostrarAlerta(Response, "Cierre efectuado satisfactoriamente");
				mesVigente.Text = DBFunctions.SingleData("SELECT pmes_nombre FROM pmes WHERE pmes_mes=(SELECT pmes_mes FROM cvehiculos)");
				anoVigente.Text = DBFunctions.SingleData("SELECT pano_ano FROM cvehiculos");
			}
		}
		
		protected void Realizar_Proceso(Object  Sender, EventArgs e)
		{
			//Primero debemos comprobar que todos los vehiculos vendidos durante el mes tengan una factura
			int i;
			DataTable tablaVehiculos = new DataTable();
			tablaVehiculos = this.Tabla_Vehiculos(tablaVehiculos);
			DataSet vehiculosVendidos = new DataSet();
            DBFunctions.Request(vehiculosVendidos, IncludeSchema.NO, "SELECT MCV.pcat_codigo, MVEH.mcat_vin, MAS.pdoc_codigo, MAS.mped_numepedi FROM mvehiculo MVEH, mpedidovehiculo MPED, masignacionvehiculo MAS, MCATALOGOVEHICULO MCV WHERE MCV.MCAT_VIN = MVEH.MCAT_VIN AND (MVEH.test_tipoesta=40 OR MVEH.test_tipoesta=60) AND MVEH.mveh_inventario=MAS.mveh_inventario AND MAS.pdoc_codigo=MPED.pdoc_codigo AND MAS.mped_numepedi=MPED.mped_numepedi AND (MPED.mped_pedido>='" + anoVigente.Text + "-" + DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre ='" + mesVigente.Text + "'") + "-01'AND MPED.mped_pedido<='" + anoVigente.Text + "-" + DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre ='" + mesVigente.Text + "'") + "-" + DBFunctions.SingleData("SELECT pmes_dias FROM pmes WHERE pmes_nombre ='" + mesVigente.Text + "'") + "')");
			for(i=0;i<vehiculosVendidos.Tables[0].Rows.Count;i++)
			{
				if(!DBFunctions.RecordExist("SELECT * FROM mfacturapedidovehiculo WHERE mped_codigo='"+vehiculosVendidos.Tables[0].Rows[i][2].ToString()+"' AND mped_numepedi="+vehiculosVendidos.Tables[0].Rows[i][3].ToString()+""))
				{
					DataRow fila = tablaVehiculos.NewRow();
					fila[0] = vehiculosVendidos.Tables[0].Rows[i][0].ToString();
					fila[1] = vehiculosVendidos.Tables[0].Rows[i][1].ToString();
					fila[2] = vehiculosVendidos.Tables[0].Rows[i][2].ToString();
					fila[3] = vehiculosVendidos.Tables[0].Rows[i][3].ToString();
					tablaVehiculos.Rows.Add(fila);
				}
			}
			if(tablaVehiculos.Rows.Count>0)
			{
				DataGrid dg = new DataGrid();
				this.Controls.Add(dg);
				dg.DataSource = tablaVehiculos;
				dg.DataBind();
            }
			else
            {
               	//Aqui continuamos el proceso de cierre de vehiculos
                   /*  proceso de ajustes por Inflación por ahora derogado
                   //Primero comprobamos que exista un valor del PAAG para el mes vigente
                   if(!DBFunctions.RecordExist("SELECT * FROM ppaag WHERE pano_ano="+anoVigente.Text+" AND pmes_mes="+DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre ='"+mesVigente.Text+"'")+""))
                       Response.Redirect("" + indexPage + "?process=Vehiculos.CierreMensual");
                   else
                   {
                       //Ahora debemos de revisar cuales son los vehículos nuevos, debemos de revisar cuales fueron comprados el antes del mes vigente
                       double valorPaag = System.Convert.ToDouble(DBFunctions.SingleData("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano="+anoVigente.Text+" AND pmes_mes="+DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre ='"+mesVigente.Text+"'")+""));
                       DataSet vehiculosAjustar = new DataSet();
                       DBFunctions.Request(vehiculosAjustar,IncludeSchema.NO,"SELECT MVEH.pcat_codigo, MVEH.mcat_vin, MFAC.mfac_valofact, MVEH.mveh_valoinfl, MVEH.mveh_valoinfl FROM mvehiculo MVEH, mfacturaproveedor MFAC WHERE MVEH.pdoc_codiordepago=MFAC.pdoc_codiordepago AND MVEH.mfac_numeordepago=MFAC.mfac_numeordepago AND MFAC.mfac_factura <'"+anoVigente.Text+"-"+DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre ='"+mesVigente.Text+"'")+"-01' AND MVEH.test_tipoesta = 20 AND MVEH.tcla_codigo = 'N' ");
                    */	
                    ArrayList sqlStrings = new ArrayList();
				//	for(i=0;i<vehiculosAjustar.Tables[0].Rows.Count;i++)
				//		sqlStrings.Add("UPDATE mvehiculo SET mveh_valoinfl = mveh_valoinfl + "+((System.Convert.ToDouble(vehiculosAjustar.Tables[0].Rows[i][2])+System.Convert.ToDouble(vehiculosAjustar.Tables[0].Rows[i][3]))*valorPaag).ToString()+" WHERE pcat_codigo='"+vehiculosAjustar.Tables[0].Rows[i][0].ToString()+"' AND mcat_vin='"+vehiculosAjustar.Tables[0].Rows[i][1].ToString()+"'");
					int mes = System.Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes FROM cvehiculos"));
					int ano = System.Convert.ToInt32(DBFunctions.SingleData("SELECT pano_ano FROM cvehiculos"));
					if(mes==12)
					{
						mes=1;
						ano+=1;
					}
					else
						mes+=1;
					sqlStrings.Add("UPDATE cvehiculos SET pmes_mes="+mes.ToString()+", pano_ano="+ano.ToString()+"");
                    if (DBFunctions.Transaction(sqlStrings))
                    {
                        Utils.MostrarAlerta(Response, "Proceso de Cierre Mensual de Vehículos finalizado exitosamente ...!!!");
                        btnProceso.Enabled = false;
                   //    Response.Redirect(indexPage + "?process=AMS.web.Inicio.aspx");
                    }
                    else
                        lb.Text = DBFunctions.exceptions;
			//	}
			}
		}
		
		protected DataTable Tabla_Vehiculos(DataTable dt)
		{
			dt.Columns.Add(new DataColumn("CATALOGO",System.Type.GetType("System.String")));
			dt.Columns.Add(new DataColumn("VIN",System.Type.GetType("System.String")));
			dt.Columns.Add(new DataColumn("PREFIJO PEDIDO",System.Type.GetType("System.String")));
			dt.Columns.Add(new DataColumn("NUMERO PEDIDO",System.Type.GetType("System.String")));
			return dt;
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

