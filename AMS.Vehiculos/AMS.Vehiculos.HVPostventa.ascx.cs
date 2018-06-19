namespace AMS.Vehiculos
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;


	/// <summary>
	///		Descripción breve de AMS_Vehiculos_HVPostventa.
	/// </summary>
	public partial class AMS_Vehiculos_HVPostventa : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		//Seleccionar el vehiculo
		public void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			plcResultados.Visible=true;
			DataSet dsRevAnt=new DataSet();
			DBFunctions.Request(dsRevAnt,IncludeSchema.NO,
                "Select "+
                "pk.pkit_codigo codigo, pk.pkit_nombre nombre, mo.mord_creacion fecha, mo.mord_kilometraje kilometraje, mp1.tproc_codigo tipo,  mo.mord_obseclie obsecliente, mo.mord_obserece obsetaller, "+  
                "mp1.MNIT_NITCONC concat '-' concat (select mn.mnit_nombres concat mn.mnit_nombre2 concat ' ' concat mn.mnit_apellidos concat ' ' "+ 
                "concat mn.mnit_apellido2 from mnit mn, mordenpostventa mp where mp.mnit_nitconc = mp1.MNIT_NITCONC and mp.mnit_nitconc = mn.MNIT_NIT "+
                "group by mn.mnit_nombres  concat mn.mnit_nombre2 concat ' ' concat mn.mnit_apellidos concat ' ' "+ 
                "concat mn.mnit_apellido2 ) CONCESIONARIO "+
                "from pkit pk, dordenkit dor, morden mo, mordenpostventa mp1 "+
                "where pk.pkit_codigo=dor.pkit_codigo and dor.pdoc_codigo=mo.pdoc_codigo and "+
                "mp1.pdoc_codigo=mo.pdoc_codigo and mp1.mord_numeorde=mo.mord_numeorde and " + 
                "dor.mord_numeorde=mo.mord_numeorde and mcat_vin= '" + txtVINVehiculo.Text + "';");
			dgrMantenimientosAnt.DataSource=dsRevAnt.Tables[0];
			dgrMantenimientosAnt.DataBind();
			DataSet dsOperAnt=new DataSet();
			DBFunctions.Request(dsOperAnt,IncludeSchema.NO,
                "Select pt.ptem_operacion codigo, pt.ptem_descripcion nombre, mo.mord_creacion fecha, mo.mord_kilometraje kilometraje, pt.ptem_valooper precio, mp.tproc_codigo tipo, mp.MNIT_NITCONC CONCESIONARIO " +
				"from dordenoperacion dor, morden mo, ptempario pt, mordenpostventa mp "+
				"where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and mcat_vin='"+txtVINVehiculo.Text+"' and "+
				"pt.ptem_operacion=dor.ptem_operacion and mp.pdoc_codigo=mo.pdoc_codigo and mp.mord_numeorde=mo.mord_numeorde order by mord_creacion desc;");
			dgrOperacionesAnt.DataSource=dsOperAnt.Tables[0];
			dgrOperacionesAnt.DataBind();
			DataSet dsRepAnt=new DataSet();
			DBFunctions.Request(dsRepAnt,IncludeSchema.NO,
                "Select mcat_vin,mi.mite_codigo codigo, mi.mite_NOMBRE nombre, mo.mord_creacion fecha, mo.mord_kilometraje kilometraje, dor.mite_precio precio, dor.mite_cantidad cantidad, mp.tproc_codigo tipo, mp.MNIT_NITCONC CONCESIONARIO " +
				"from dordenitemspostventa dor, morden mo, mitems mi, mordenpostventa mp "+
				"where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and mcat_vin='"+txtVINVehiculo.Text+"' and "+
				"mi.mite_codigo=dor.mite_codigo and mp.pdoc_codigo=mo.pdoc_codigo and mp.mord_numeorde=mo.mord_numeorde order by mord_creacion desc;");
			dgrRepuestosAnt.DataSource=dsRepAnt.Tables[0];
			dgrRepuestosAnt.DataBind();

            #region Datos Vehiculo
            //Datos Vehiculo
            DataSet dsVehiculo = new DataSet();
            string catalogo;
            DBFunctions.Request(dsVehiculo, IncludeSchema.NO, "SELECT MC.MCAT_PLACA, MV.MCAT_VIN,MC.MCAT_MOTOR, MC.PCAT_CODIGO,PC.PCOL_DESCRIPCION,MC.MCAT_ANOMODE, MC.MCAT_NUMEULTIKILO, MV.MNIT_NIT FROM MCATALOGOVEHICULO MC,MVEHICULO MV,PCOLOR PC WHERE MC.MCAT_VIN=MV.MCAT_VIN AND PC.PCOL_CODIGO=MC.PCOL_CODIGO AND MV.MCAT_VIN='" + txtVINVehiculo.Text + "';");
            lblVINVehiculo.Text = dsVehiculo.Tables[0].Rows[0]["MCAT_VIN"].ToString();
            lblMotorVehiculo.Text = dsVehiculo.Tables[0].Rows[0]["MCAT_MOTOR"].ToString();
            catalogo = dsVehiculo.Tables[0].Rows[0]["PCAT_CODIGO"].ToString();
            lblCatalogoVehiculo.Text = catalogo;
            lblColorVehiculo.Text = dsVehiculo.Tables[0].Rows[0]["PCOL_DESCRIPCION"].ToString();
            lblAnoCatalogo.Text = dsVehiculo.Tables[0].Rows[0]["MCAT_ANOMODE"].ToString();
            lblPlacaVehiculo.Text = dsVehiculo.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
            lblKilometraje.Text = Convert.ToUInt32(dsVehiculo.Tables[0].Rows[0]["MCAT_NUMEULTIKILO"]).ToString("#,##0");
            ViewState["PLACA"] = lblPlacaVehiculo.Text;
            //Garantia
            DataSet dsGarantia = new DataSet();
            int mesesG, kilosG;
            DBFunctions.Request(dsGarantia, IncludeSchema.NO, "SELECT PCAT_KILOMETRAJEGARANTIA KILOM,PCAT_MESESGARANTIA MESES FROM PCATALOGOVEHICULO PC, MCATALOGOVEHICULO MC WHERE MC.MCAT_VIN='" + txtVINVehiculo.Text + "' AND MC.PCAT_CODIGO=PC.PCAT_CODIGO;");
            if (dsGarantia.Tables[0].Rows.Count > 0)
            {
                mesesG = Convert.ToInt16(dsGarantia.Tables[0].Rows[0]["MESES"]);
                kilosG = Convert.ToInt16(dsGarantia.Tables[0].Rows[0]["KILOM"]);
                lblMesesGarantia.Text = mesesG.ToString();
                lblKiloGarantia.Text = kilosG.ToString();
            }
            #endregion

            #region Datos Propietario
            //Datos Propietario

            DataSet dsPropietario = new DataSet();
            string nitPropietario;
            nitPropietario = dsVehiculo.Tables[0].Rows[0]["MNIT_NIT"].ToString();

            //muestra al propietario actual del vehiculo, sino hay muestra al propietario consesecionario que compró.
            DBFunctions.Request(dsPropietario, IncludeSchema.NO,
                "SELECT * FROM MVEHICULOPOSTVENTA where mcat_vin='" + txtVINVehiculo.Text + "';");
            if (dsPropietario.Tables[0].Rows.Count == 0)
            {
                DBFunctions.Request(dsPropietario, IncludeSchema.NO,
                "SELECT * FROM MNIT mn, PCIUDAD pc where pc.pciu_codigo=mn.pciu_codigo and mn.mnit_nit='" + nitPropietario + "';");

                lblNITPropietario.Text = nitPropietario;
                lblNITPropietarioa.Text = dsPropietario.Tables[0].Rows[0]["MNIT_NOMBRES"].ToString() + " " + dsPropietario.Tables[0].Rows[0]["MNIT_NOMBRE2"].ToString() + " " + dsPropietario.Tables[0].Rows[0]["MNIT_APELLIDOS"].ToString() + " " + dsPropietario.Tables[0].Rows[0]["MNIT_APELLIDO2"].ToString();
                lblNITPropietariob.Text = dsPropietario.Tables[0].Rows[0]["MNIT_DIRECCION"].ToString();
                lblNITPropietarioc.Text = dsPropietario.Tables[0].Rows[0]["MNIT_TELEFONO"].ToString();
                lblNITPropietariod.Text = dsPropietario.Tables[0].Rows[0]["MNIT_CELULAR"].ToString();
                lblNITPropietarioe.Text = dsPropietario.Tables[0].Rows[0]["PCIU_NOMBRE"].ToString();
            }
            else
            {
                lblNITPropietario.Text = dsPropietario.Tables[0].Rows[0]["MNIT_NITCLIE"].ToString();
                lblNITPropietarioa.Text = dsPropietario.Tables[0].Rows[0]["MNIT_NOMBRECLIE"].ToString();
                lblNITPropietariob.Text = dsPropietario.Tables[0].Rows[0]["MNIT_DIRECCIONCLIE"].ToString();
                lblNITPropietarioc.Text = dsPropietario.Tables[0].Rows[0]["MNIT_TELEFONOCLIE"].ToString();
                lblNITPropietariod.Text = dsPropietario.Tables[0].Rows[0]["MNIT_CELULARCLIE"].ToString();
                lblNITPropietarioe.Text = dsPropietario.Tables[0].Rows[0]["PCIU_CODIGOCLIE"].ToString();
            }
            
            #endregion
		}
	}
}
