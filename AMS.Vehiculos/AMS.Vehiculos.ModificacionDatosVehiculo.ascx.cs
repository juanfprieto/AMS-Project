// created on 12/04/2005 at 13:20
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
	public partial class ModificacionDatosVehiculo : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
            string sql = String.Format(
                    "SELECT max(mveh_inventario) NUMERO, MV.mcat_vin VIN " +
                     "FROM  MCATALOGOVEHICULO MC " +
                     "inner join MVEHICULO MV ON MV.mcat_vin = MC.mcat_vin " +
                     "WHERE MC.pcat_codigo='{0}' and mv.test_tipoesta <= 40 " +
                     "GROUP BY MV.mcat_vin " +
                     "ORDER BY MV.mcat_vin"
                     , catalogoVehiculo.SelectedValue);

			if(!IsPostBack)
			{
                Utils.FillDll(catalogoVehiculo, CatalogoVehiculos.CATALOGOVEHICULOSCOMERCIAN, false);
                if (catalogoVehiculo.Items.Count > 0)
                    catalogoVehiculo.Items.Insert(0, "Seleccione:..");
                else
                    Utils.MostrarAlerta(Response, "NO hay vehículos para Modificar !");
                Utils.FillDll(vinVehiculo, sql, false);
                if (Request.QueryString["exito"] != null)
                    Utils.MostrarAlerta(Response, "Cambios realizados con éxito");
            }
			imglupa.Attributes.Add("OnClick","ModalDialog("+vinVehiculo.ClientID+",'"+Utilidades.EscaparCadenaAJavascript(sql)+"' ,new Array() );");
		}
		
		protected void Cambio_Catalogo(Object sender, EventArgs e)
		{

            string sql = String.Format(
                    "SELECT max(mveh_inventario) NUMERO, MV.mcat_vin VIN " +
                     "FROM  MCATALOGOVEHICULO MC " +
                     "inner join MVEHICULO MV ON MV.mcat_vin = MC.mcat_vin " +
                     "WHERE MC.pcat_codigo='{0}' and mv.test_tipoesta <= 40 " +
                     "GROUP BY MV.mcat_vin " +
                     "ORDER BY MV.mcat_vin"
                     , catalogoVehiculo.SelectedValue);
            Utils.FillDll(vinVehiculo, sql, false);

            imglupa.Attributes.Add("OnClick", "ModalDialog(" + vinVehiculo.ClientID + ",'" + Utilidades.EscaparCadenaAJavascript(sql) + "' ,new Array() );");
		}
		
		protected void Editar_Datos_Vehiculo(Object sender, EventArgs e)
		{
            //var parsedString = HttpUtility.HtmlDecode();
            //var root = HttpUtility.ParseQueryString(parsedString)["root"];

            if (vinVehiculo.Items.Count>0)
                    Response.Redirect("" + indexPage + "?process=Vehiculos.ModificacionFormulario&cat=" + catalogoVehiculo.SelectedValue + 
                        "&vin=" + vinVehiculo.SelectedItem.Text);
			else
                Utils.MostrarAlerta(Response, "No se ha seleccionado ningún vehículo");
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
