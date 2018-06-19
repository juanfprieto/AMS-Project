using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Inventarios
{

	public partial class Guias : System.Web.UI.UserControl
	{

        #region atributos
        
        protected Seleccionar facturas  = new Seleccionar();
        protected DatasToControls bind = new DatasToControls();

        #endregion atributos


		protected void Page_Load(object sender, EventArgs e)
		{
            facturas.Visible = true;

            facturas.ECargarG += new Seleccionar.DCargarG(Facturas_ECargarG);
            facturas.AsignarEstados(true, true, true, true);
            facturas.AsignarTextoLabels("Facturas disponibles", "Factuas Seleccionadas");

            if(!IsPostBack)
            {
                facturas.BotonAccion.Text = "Guardar";
                facturas.BotonAccion.Enabled = false;

                cargarDdlNit();
                cargarDespachadores();
			}
		}

        private void cargarDespachadores()
        {
            //cargamos los vendedores todo tipo y de mostrador

            string sql = "";

            sql =
                "SELECT pven_codigo,pven_nombre " +
                "FROM pvendedor " +
                "WHERE (tvend_codigo='TT' OR tvend_codigo='VM') and pven_vigencia = 'V' ;";

            bind.PutDatasIntoDropDownList(ddlDespachador, sql);
        }

        private void cargarDdlNit()
        {
            //cargamos los nits de provedor que tengan facturas en los ultimos 5 dias

            string sql = "";

            sql =
                "SELECT DISTINCT NIT.mnit_nit, NIT.NOMBRE " +
                "FROM Vmnit NIT " +
                "INNER JOIN mfacturacliente FAC " +
                "ON (NIT.mnit_nit=FAC.mnit_nit) " +
                "WHERE (DAYS(SYSDATE)-DAYS(FAC.mfac_factura))<=5;";

            bind.PutDatasIntoDropDownList(ddlNit,sql);
            if (ddlNit.Items.Count > 1)
                ddlNit.Items.Insert(0, "Seleccione:..");
            else
                if (ddlNit.SelectedValue != null) cargarDatos();
        }

        private void cargarCiudad(string nit)
        {
            String sql =
                "SELECT CIU.pciu_codigo,CIU.pciu_nombre " +
                "FROM pciudad CIU ;";

            bind.PutDatasIntoDropDownList(ddlCiudad, sql);

            //de todas las ciudades elegimos la asignada al nit

            sql =
                "SELECT CIU.pciu_codigo " +
                "FROM mnit NIT, pciudad CIU " +
                "WHERE NIT.mnit_nit='" + nit + "' AND " +
                "CIU.pciu_codigo=NIT.pciu_codigo;";

            ddlCiudad.SelectedValue = DBFunctions.SingleData(sql);
        }

        private void cargarDireccion(string nit)
        {
            //cargamos la dirección asiciada al nit

            string sql =
                "SELECT mnit_direccion " +
                "FROM mnit " +
                "WHERE mnit_nit='" + nit + "';";

            txtDireccion.Text = DBFunctions.SingleData(sql);
        }

        private void cargarFacturas(string nit)
        {
            DataSet ds = new DataSet();

            string Sql = "SELECT pdoc_codigo, mfac_numedocu " +
                "FROM mfacturacliente " +
                "WHERE MNIT_NIT='" + nit + "' AND " +
                "PDOC_CODIGO ||mfac_numedocu NOT IN (" +
                "SELECT PDOC_CODIGO ||mfac_numedocu " +
                "FROM dguia) AND MFAC_TIPODOCU = 'F';";
            
            try
            {
                DBFunctions.Request(ds, IncludeSchema.NO, Sql);

                string fac;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //de esta manera lo que se muestra es el prefijo seguido del numero
                    //lo que se muestra es igual al dato que se puede obtener. Alto acoplamiento =(
                    fac = ds.Tables[0].Rows[i][0].ToString() + "-" + ds.Tables[0].Rows[i][1].ToString();

                    facturas.CargarOrigen(i,fac,fac,"white");
                }
            }
            catch (Exception Ex)
            {
                lbInfo.Text = "Error : " + Ex.Message;
            }
            
            ds.Dispose();
        }

        private void cargarDatos()
        {
            string nit = ddlNit.SelectedValue;

            cargarDireccion(nit);
            cargarCiudad(nit);
            cargarFacturas(nit);
        }

        private void Facturas_ECargarG(ListBox Inicio, ListBox Final)
		{

        }

        protected void ddlNitIndexChanged(Object Sender, EventArgs E)
        {
            string nit = ddlNit.SelectedValue;

            cargarDireccion(nit);
            cargarCiudad(nit);
            cargarFacturas(nit);
        }
	}
}