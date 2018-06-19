// created on 12/04/2005 at 14:37
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
	public partial class ModificacionFormulario : System.Web.UI.UserControl
	{
		string user = HttpContext.Current.User.Identity.Name.ToLower();
        protected bool validarVIN = Convert.ToBoolean(ConfigurationManager.AppSettings["validarVIN"]);
     	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(estadoVehiculo,"SELECT test_tipoesta, test_nombesta FROM testadovehiculo order by test_tipoesta");
                bind.PutDatasIntoDropDownList(ubicacion,   "SELECT pubi_codigo, pubi_nombre       FROM pubicacion where PUBI_VIGENCIA = 'V' ORDER BY pubi_nombre");
                bind.PutDatasIntoDropDownList(tipoCompra,  "SELECT tcom_codigo, tcom_tipocompra   FROM tcompravehiculo order by tcom_tipocompra");
				bind.PutDatasIntoDropDownList(tipoVehiculo,"SELECT tcla_clase, tcla_nombre        FROM tclasevehiculo ORDER BY tcla_clase");
				bind.PutDatasIntoDropDownList(ddlCataVehi, "SELECT pcat_codigo                    FROM pcatalogovehiculo order by pcat_codigo");
                //var catalogo = HttpUtility.UrlEncode(Request.QueryString["cat"]);

                this.Cargar_Datos(HttpUtility.UrlEncode(Request.QueryString["cat"]), Request.QueryString["vin"]);
            }
		}
		
		protected void Cargar_Datos(string catalogo, string vin)
		{
			//Posible cambio : utilizar el request y traer todos los datos en una sola llamada a la base de datos o seguir haciendo llamada a llamada
			cataVehiculo.Text   = catalogo;
			vinVehiculo.Text    = vin;
			txtVinVehiculo.Text = vin;
            DataSet ds = new DataSet();
            numeInventario.Text = DBFunctions.SingleData("SELECT max(mveh_inventario) FROM mvehiculo WHERE mcat_vin='" + vin + "'");
            DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT max(mveh_inventario) as inventario,  mped_numero, 
                                                        mveh_valoinfl, mveh_numerece, mveh_fechrece, mveh_fechdisp, mveh_kilometr, mveh_numemani, mveh_fechmani, 
                                                        mveh_aduana, mveh_numed_o, mveh_numelevante, mveh_valogast, mnit_nit, mveh_fechentr FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text + @" group by mveh_inventario ,  mped_numero, 
                                                        mveh_valoinfl, mveh_numerece, mveh_fechrece, mveh_fechdisp, mveh_kilometr, mveh_numemani, mveh_fechmani, 
                                                        mveh_aduana, mveh_numed_o, mveh_numelevante, mveh_valogast, mnit_nit, mveh_fechentr;");

            
            prefPedido.Text = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo=(SELECT pdoc_codigopediprov FROM mvehiculo WHERE mcat_vin='" + vin + "')");
            numePedido.Text = ds.Tables[0].Rows[0]["mped_numero"].ToString();
            DatasToControls.EstablecerDefectoDropDownList(ddlCataVehi, cataVehiculo.Text);//DBFunctions.SingleData("SELECT pcat_codigo FROM mvehiculo mv, mCATALOGOVEHICULO MC WHERE pcat_codigo='" + catalogo + "' AND MV.mcat_vin='" + vin + "' AND MC.MCAT_VIN = MV.MCat_VIN"));
            DatasToControls.EstablecerDefectoDropDownList(estadoVehiculo, DBFunctions.SingleData("SELECT test_nombesta FROM testadovehiculo WHERE test_tipoesta=(SELECT test_tipoesta FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text + ")"));
            DatasToControls.EstablecerDefectoDropDownList(ubicacion, DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacion WHERE pubi_codigo='" + DBFunctions.SingleData("SELECT pubi_codigo FROM mubicacionvehiculo WHERE pcat_codigo='" + catalogo + "' AND mcat_vin='" + vin + "' ORDER BY mubi_codigo DESC") + "'"));
            DatasToControls.EstablecerDefectoDropDownList(tipoCompra, DBFunctions.SingleData("SELECT tcom_tipocompra FROM tcompravehiculo WHERE tcom_codigo=(SELECT tcom_codigo FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text + ")"));
            DatasToControls.EstablecerDefectoDropDownList(tipoVehiculo, DBFunctions.SingleData("SELECT tcla_nombre FROM tclasevehiculo WHERE tcla_clase=(SELECT tcla_codigo FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text + ")"));
            if (tipoVehiculo.SelectedValue.ToString() == "U")
            {
                lblPrecioPublico.Text = "Precio de Venta a Público";
                txtPrecioPublico.Enabled = true;
            }
            else
            {
                lblPrecioPublico.Text = "";
                txtPrecioPublico.Enabled = false;
            }
            txtPrecioPublico.Text = ds.Tables[0].Rows[0]["mveh_valoinfl"].ToString();
            numeRecepcion.Text = ds.Tables[0].Rows[0]["mveh_numerece"].ToString();
            fechRecepcion.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["mveh_fechrece"].ToString()).GetDateTimeFormats()[5];
            fechDisponible.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["mveh_fechdisp"].ToString()).GetDateTimeFormats()[5];
            kiloRecepcion.Text = ds.Tables[0].Rows[0]["mveh_kilometr"].ToString();
            numeManifiesto.Text = ds.Tables[0].Rows[0]["mveh_numemani"].ToString();
            fechManifiesto.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["mveh_fechmani"].ToString()).GetDateTimeFormats()[5];
            numeAduana.Text = ds.Tables[0].Rows[0]["mveh_aduana"].ToString();
            numeDO.Text = ds.Tables[0].Rows[0]["mveh_numed_o"].ToString();
            numeLevante.Text = ds.Tables[0].Rows[0]["mveh_numelevante"].ToString();
            valorGastos.Text = ds.Tables[0].Rows[0]["mveh_valogast"].ToString();
            nitPropietario.Text = ds.Tables[0].Rows[0]["mnit_nit"].ToString();
            txtPlaca.Text = DBFunctions.SingleData("SELECT MCAT_PLACA FROM MCATALOGOVEHICULO WHERE MCAT_VIN = '" + vin + "'");
            txtFechaMatriInicial.Text = Convert.ToDateTime(DBFunctions.SingleData("SELECT MCAT_VENTA FROM MCATALOGOVEHICULO WHERE MCAT_VIN = '" + vin + "'")).GetDateTimeFormats()[5];
            //txtNumMatriInicial.Text = ;
            try
            {
                fechEntrega.Text = ds.Tables[0].Rows[0]["mveh_fechentr"].ToString();
            }
            catch {
                Utils.MostrarAlerta(Response, "Este vehículo tiene asignada una fecha de entrega");
            }
            /*
			numeInventario.Text = DBFunctions.SingleData("SELECT max(mveh_inventario) FROM mvehiculo WHERE mcat_vin='"+vin+"'");
			prefPedido.Text     = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo=(SELECT pdoc_codigopediprov FROM mvehiculo WHERE mcat_vin='"+vin+"')");			
			numePedido.Text     = DBFunctions.SingleData("SELECT mped_numero FROM mvehiculo WHERE mcat_vin='"+vin+"'");
            DatasToControls.EstablecerDefectoDropDownList(ddlCataVehi, DBFunctions.SingleData("SELECT pcat_codigo FROM mvehiculo mv, mCATALOGOVEHICULO MC WHERE pcat_codigo='" + catalogo + "' AND MV.mcat_vin='" + vin + "' AND MC.MCAT_VIN = MV.MCat_VIN"));
            DatasToControls.EstablecerDefectoDropDownList(estadoVehiculo, DBFunctions.SingleData("SELECT test_nombesta FROM testadovehiculo WHERE test_tipoesta=(SELECT test_tipoesta FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text + ")"));
			DatasToControls.EstablecerDefectoDropDownList(ubicacion,DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacion WHERE pubi_codigo='"+DBFunctions.SingleData("SELECT pubi_codigo FROM mubicacionvehiculo WHERE pcat_codigo='"+catalogo+"' AND mcat_vin='"+vin+"' ORDER BY mubi_codigo DESC")+"'"));
            DatasToControls.EstablecerDefectoDropDownList(tipoCompra, DBFunctions.SingleData("SELECT tcom_tipocompra FROM tcompravehiculo WHERE tcom_codigo=(SELECT tcom_codigo FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text + ")"));
            DatasToControls.EstablecerDefectoDropDownList(tipoVehiculo, DBFunctions.SingleData("SELECT tcla_nombre FROM tclasevehiculo WHERE tcla_clase=(SELECT tcla_codigo FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text + ")"));
            if (tipoVehiculo.SelectedValue.ToString() == "U")
            {
                lblPrecioPublico.Text = "Precio de Venta a Público";
                txtPrecioPublico.Enabled = true;
            }
            else
            {
                lblPrecioPublico.Text = "";
                txtPrecioPublico.Enabled = false;
            }
            txtPrecioPublico.Text = DBFunctions.SingleData("SELECT mveh_valoinfl FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text);
            numeRecepcion.Text  = DBFunctions.SingleData("SELECT mveh_numerece FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text);
            fechRecepcion.Text  = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mveh_fechrece FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text)).ToString("yyyy-MM-dd");
            fechDisponible.Text = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mveh_fechdisp FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text)).ToString("yyyy-MM-dd");
            kiloRecepcion.Text  = DBFunctions.SingleData("SELECT mveh_kilometr FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text);
            numeManifiesto.Text = DBFunctions.SingleData("SELECT mveh_numemani FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text);
            fechManifiesto.Text = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mveh_fechmani FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text)).ToString("yyyy-MM-dd");
            numeAduana.Text     = DBFunctions.SingleData("SELECT mveh_aduana FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text);
            numeDO.Text         = DBFunctions.SingleData("SELECT mveh_numed_o FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text);
            numeLevante.Text    = DBFunctions.SingleData("SELECT mveh_numelevante FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text);
            valorGastos.Text    = DBFunctions.SingleData("SELECT mveh_valogast FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text);
            nitPropietario.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text);
			try
			{
                fechEntrega.Text = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mveh_fechentr FROM mvehiculo WHERE mveh_inventario=" + numeInventario.Text)).ToString("yyyy-MM-dd");
			}
			catch{}
            */

        }

		protected string FechasNulas(string fecha)
		{
			if (fecha!="")
				fecha="'"+Convert.ToDateTime(fecha).ToString("yyyy-MM-dd")+"'";
			else
				fecha="null";
			return fecha;
		}
		
		protected string CamposNulos(string campo)
		{
			if (campo!="")
				campo="'"+campo+"'";
			else
				campo="null";
			return campo;
		}

		protected string FechasNulasTimeS(string fecha)
		{
			if (fecha!="")
				fecha="'"+Convert.ToDateTime(fecha).ToString("yyyy-MM-dd HH:mm:ss")+"'";
			else
				fecha="null";
			return fecha;
		}

		protected bool validarVin(string vin)
		{
            if (!validarVIN)
                return true;
            else
            {
                if (vin.Length == 17)
                    return true;
                else
                    return false;
            }
		}

		protected void Grabar_Cambio(Object sender, EventArgs e)
		{
            if(validarDatos())
            {
                ArrayList sqlStrings = new ArrayList();
                sqlStrings.Add("UPDATE mvehiculo SET test_tipoesta=" + estadoVehiculo.SelectedValue + ", tcom_codigo='" + tipoCompra.SelectedValue + "', " +
                        " tcla_codigo   ='" + tipoVehiculo.SelectedValue + "', mveh_numerece=" + numeRecepcion.Text + ", mveh_fechrece='" + fechRecepcion.Text + "', " +
                        " mveh_fechdisp ='" + fechDisponible.Text + "', mveh_kilometr=" + kiloRecepcion.Text.Replace(",", ".") + ", mveh_numemani='" + numeManifiesto.Text + "', " +
                        " mveh_fechmani ='" + fechManifiesto.Text + "', mveh_aduana='" + numeAduana.Text + "', mveh_numed_o='" + numeDO.Text + "', mveh_numelevante='" + numeLevante.Text + "', " +
                        " mveh_valogast =" + valorGastos.Text.Replace(",", "") + ",mveh_valoinfl =" + txtPrecioPublico.Text.Replace(",", "") + ", mnit_nit='" + nitPropietario.Text + "'" + (fechEntrega.Text == ""?"": ", mveh_fechentr = '" + fechEntrega.Text + "'") +
                        " WHERE mveh_inventario=" + numeInventario.Text + "; ");
                sqlStrings.Add("UPDATE MCATALOGOVEHICULO SET " /*PCAT_CODIGO = '" + ddlCataVehi.SelectedValue + "',*/ + " MCAT_PLACA = '" + txtPlaca.Text + "', MCAT_VENTA = '" + txtFechaMatriInicial.Text + "' WHERE MCAT_VIN = '" + txtVinVehiculo.Text + "'");
                if(DBFunctions.Transaction(sqlStrings))
                {
                    Response.Redirect("" + ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Vehiculos.ModificacionDatosVehiculo&exito=1");
                }
                else
                {
                    Utils.MostrarAlerta(Response, "La modificación ha fallado. Revise que toda la información sea la adecuada");
                    lb.Text = DBFunctions.exceptions;
                }
            }
            else
            {
                Utils.MostrarAlerta(Response, "No puede haber ningún campo vacio, por favo revise");
            }



            /*
			if (validarVin(txtVinVehiculo.Text))
			{
                string vin_existe = DBFunctions.SingleData("select mcat_vin from mcatalogovehiculo where mcat_vin = '" + txtVinVehiculo.Text + "' AND MCAT_VIN <> '"+ vinVehiculo.Text +"' ");
				if  (vin_existe != "")
                    Utils.MostrarAlerta(Response, "Este VIN YA EXISTE en el sistema, Revise por favor");
                else
				{
                    //acá empezamos a llenar las tablas, recordar que en este proceso no es necesario cambiar el V.I.N
                    if()
                    {

                    }


					//if (vinVehiculo.Text!=txtVinVehiculo.Text) // or catalogo es diferente
					//{
				 //// 1. quitar llave primaria y llave unica a mcatalogovehiculo
				 //  		DBFunctions.NonQuery("ALTER TABLE DBXSCHEMA.MCATALOGOVEHICULO DROP PRIMARY KEY;");
				 //// 2. actualizar catalogo y vin en mcatalogovehiculo 
				 //		DBFunctions.NonQuery("update dbxschema.mcatalogovehiculo set PCAT_CODIGO='"+ddlCataVehi.SelectedValue+"',mcat_vin='"+txtVinVehiculo.Text+"' where mcat_vin='"+vinVehiculo.Text+"' ");
				 //// 3. actualizar catalogo y vin en mvehiculo
				 //		DBFunctions.NonQuery("update dbxschema.mvehiculo set mcat_vin='"+txtVinVehiculo.Text+"' where mcat_vin='"+vinVehiculo.Text+"' ");
				 //// 4. actualizar catalogo y vin en mubicacionvehiculo
				 //		DBFunctions.NonQuery("update dbxschema.mubicacionvehiculo set PCAT_CODIGO='"+ddlCataVehi.SelectedValue+"',mcat_vin='"+txtVinVehiculo.Text+"' where mcat_vin='"+vinVehiculo.Text+"' ");
				 //// 5. actualizar catalogo y vin en morden
				 // 		DBFunctions.NonQuery("update dbxschema.morden set mcat_vin='"+txtVinVehiculo.Text+"' where mcat_vin='"+vinVehiculo.Text+"' ");
				 //// -   revisar tabla mauditoriavehiculos que hace?
				 //// 6. restablecer llave primaria y llave unica a mcatalogovehiculo
					//	DBFunctions.NonQuery("ALTER TABLE DBXSCHEMA.MCATALOGOVEHICULO ADD PRIMARY KEY (MCAT_VIN);");
     //           //  7. Crea la llave Foranea Con MVEHICULO 
     //                   DBFunctions.NonQuery("ALTER TABLE DBXSCHEMA.MVEHICULO ADD CONSTRAINT FK_MVEHIC_MCATALOGOVEHICULO FOREIGN KEY (MCAT_VIN) REFERENCES DBXSCHEMA.MCATALOGOVEHICULO (MCAT_VIN) ON UPDATE NO ACTION ON DELETE NO ACTION;");
					//}

                    string fechaEntrega = fechEntrega.Text == "" ? "" : ", mveh_fechentr=('" + fechEntrega.Text + "' concat ' 12:00:00.0') ";
										
					string sql = "UPDATE mvehiculo SET test_tipoesta="+estadoVehiculo.SelectedValue+", tcom_codigo='"+tipoCompra.SelectedValue+"', "+
						" tcla_codigo   ='"+tipoVehiculo.SelectedValue+"', mveh_numerece="+numeRecepcion.Text+", mveh_fechrece='"+fechRecepcion.Text+"', "+
                        " mveh_fechdisp ='" + fechDisponible.Text + "', mveh_kilometr=" + kiloRecepcion.Text.Replace(",", ".") + ", mveh_numemani='" + numeManifiesto.Text + "', " +
						" mveh_fechmani ='"+fechManifiesto.Text+"', mveh_aduana='"+numeAduana.Text+"', mveh_numed_o='"+numeDO.Text+"', mveh_numelevante='"+numeLevante.Text+"', "+
                        " mveh_valogast =" + valorGastos.Text.Replace(",", "") + ",mveh_valoinfl =" + txtPrecioPublico.Text.Replace(",", "") + ", mnit_nit='" + nitPropietario.Text + "'" + fechaEntrega +
						" WHERE mveh_inventario="+numeInventario.Text+"; ";

                    try
                    {
                        DBFunctions.NonQuery(sql);
                        Response.Redirect("" + ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Vehiculos.ModificacionDatosVehiculo");
                    }
                    catch
                    {
                        lb.Text = "Error al actualizar datos: " + DBFunctions.exceptions;
                    }
				}
			}
            lb.Text = "Error al actualizar datos, este VIN NO cumple la regla de Validación: " + DBFunctions.exceptions;
            */
		}
        protected bool validarDatos()
        {
            valorGastos.Text = valorGastos.Text.Trim() == "" ? "0.00" : valorGastos.Text;
            kiloRecepcion.Text = kiloRecepcion.Text.Trim() == "" ? "0.00" : kiloRecepcion.Text;
            if (//txtPrecioPublico.Text == ""
                numeRecepcion.Text == ""
                || fechRecepcion.Text == ""
                || fechDisponible.Text == ""
                || kiloRecepcion.Text == ""
                || numeManifiesto.Text == ""
                || fechManifiesto.Text == ""
                || numeAduana.Text == ""
                || numeDO.Text == ""
                || numeLevante.Text == ""
                || valorGastos.Text == ""
                || nitPropietario.Text == ""
                || txtPlaca.Text == "")
                //|| txtFechaMatriInicial.Text == ""
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
        protected void cambioVin(object sender, EventArgs z)
        {
            Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Automotriz.ModificarVIN&vin=" + txtVinVehiculo.Text);
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
