using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Tools;
using AMS.DB;
using System.Data;
using System.Collections;
using System.Configuration;

namespace AMS.Asousados
{
	public partial class AsociacionVendedores : System.Web.UI.UserControl
    {

#region Eventos

        protected void Page_Load(object sender, EventArgs e)
		{
            if(!IsPostBack)
            {
                DatosIniciales();
            }
		}

        protected void ddlAsociado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string asociado = ddlAsociado.SelectedValue;
            if (asociado == "" || asociado == "0")
            {
                ddlSede.Enabled = btnAsignarVends.Enabled = btnDesasignarVends.Enabled = false;
                return;
            }

            LlenarDatosAsociado(asociado);
            ddlSede_SelectedIndexChanged(null, null);
        }

        protected void ddlSede_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sede = ddlSede.SelectedValue;
            if (sede == "" || sede == "0") return;

            BindVendedores(sede);
        }

        protected void btnAsignarVends_Click(object sender, EventArgs e)
        {
            bool vendsAsignados = VendedorController.AsignarVendedoresEnCarrusel(ddlAsociado.SelectedValue);

            if (vendsAsignados)
                Utils.MostrarAlerta(Response, "Vendedores Asignados!");

            else
                Utils.MostrarAlerta(Response, "Ucurrió un porblema al asignar los vendedores. Por favor revise los logs para más información");

            btnAsignarVends.Enabled = false;
            btnDesasignarVends.Enabled = true;
        }

        protected void btnDesasignarVends_Click(object sender, EventArgs e)
        {
            bool vendsDesasignados = VendedorController.DesasignarVendedoresPorCarrusel(ddlAsociado.SelectedValue);

            if (vendsDesasignados)
                Utils.MostrarAlerta(Response, "Vendedores desasignados!");

            else
                Utils.MostrarAlerta(Response, "Ucurrió un porblema al desasignar los vendedores. Por favor revise los logs para más información");

            btnAsignarVends.Enabled = true;
            btnDesasignarVends.Enabled = false;
        }

#endregion

#region eventos grid

        public void dgVendedores_ItemCommand(Object sender, DataGridCommandEventArgs e)
        {
            string commandName = ((Button)e.CommandSource).CommandName;

            if (commandName == "remover")
            {
                string codigoVend = dgVendedores.DataKeys[e.Item.ItemIndex].ToString();

                VendedorController.eliminarVendedor(codigoVend);
                VendedorController.AsignarCarrosPorUbicacion(ddlSede.SelectedValue);
            }
            else if (commandName == "agregar")
            {
                string nombreVend = ((TextBox)e.Item.FindControl("txtNom")).Text;
                string ubicVend = ((DropDownList)e.Item.FindControl("ddlUbicacion")).SelectedValue;
                string telfVend = ((TextBox)e.Item.FindControl("txtTel")).Text;
                string mailVend = ((TextBox)e.Item.FindControl("txtEmail")).Text;
                HttpPostedFile fotoVend = Request.Files[0] as HttpPostedFile;

                VendedorController.nuevoVendedor(nombreVend, ubicVend, telfVend, mailVend, fotoVend);
            }
            BindVendedores(ddlSede.SelectedValue);
        }

        public void dgVendedores_Cancel(Object sender, DataGridCommandEventArgs e)
		{
            dgVendedores.EditItemIndex = -1;
			BindVendedores(ddlSede.SelectedValue);
		}

        public void dgVendedores_Edit(Object sender, DataGridCommandEventArgs e)
		{
            dgVendedores.EditItemIndex = e.Item.ItemIndex;
            BindVendedores(ddlSede.SelectedValue);
		}

        public void dgVendedores_Update(Object sender, DataGridCommandEventArgs e)
        {
            string codigoVend = dgVendedores.DataKeys[e.Item.ItemIndex].ToString();
            string nombreVend = ((TextBox)e.Item.FindControl("txtNomEdit")).Text;
            string ubicVend = ((DropDownList)e.Item.FindControl("ddlUbicacionEdit")).SelectedValue;
            string telfVend = ((TextBox)e.Item.FindControl("txtTelEdit")).Text;
            string mailVend = ((TextBox)e.Item.FindControl("txtEmailEdit")).Text;
            HttpPostedFile fotoVend = Request.Files[0] as HttpPostedFile;

            if (ubicVend != ddlSede.SelectedValue)
                DBFunctions.NonQuery("update mvehiculousado set pven_asignado=null where pven_asignado=" + codigoVend);

            VendedorController.updateVendedor(codigoVend, nombreVend, ubicVend, telfVend, mailVend, fotoVend);
            VendedorController.AsignarCarrosPorUbicacion(ddlSede.SelectedValue);

            dgVendedores.EditItemIndex = -1;
            BindVendedores(ddlSede.SelectedValue);
        }

        protected void dgVendedores_Bound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                string sede = ddlSede.SelectedValue;

                DropDownList ddlUbicacion = (DropDownList)e.Item.FindControl("ddlUbicacion");
                string sql = String.Format("select pubi_secuencia, pubi_nombre from pubicacion where pubi_secuencia={0}", sede);
                Utils.FillDll(ddlUbicacion, sql, false);
            }
            if(e.Item.ItemType == ListItemType.EditItem)
            {
                string sede = ddlSede.SelectedValue;
                string asociado = ddlAsociado.SelectedValue;

                DropDownList ddlUbicacionEdit = (DropDownList)e.Item.FindControl("ddlUbicacionEdit");
                string sql = String.Format("select pubi_secuencia,pubi_nombre from pubicacion where mnit_nitconcesionario='{0}'", asociado);
                Utils.FillDll(ddlUbicacionEdit, sql, sede, false);
            }
        }

#endregion

#region Métodos

        private void DatosIniciales()
        {
            string user = HttpContext.Current.User.Identity.Name.ToLower();

            string sql = String.Format(
                 "select mnit_nit from susuario where LOWER(susu_login)='{0}'"
                 , user);

            string asociado = DBFunctions.SingleData(sql);

            if (asociado != "")
            {
                sql = String.Format("select * from vmnit where mnit_nit='{0}'", asociado);
                Utils.FillDll(ddlAsociado, sql, false);
                ddlAsociado_SelectedIndexChanged(null, null);
            }
            else
            {
                sql = "SELECT vmn.* \n" +
                    "FROM vmnit vmn  \n" +
                    "  INNER JOIN mconcesionario mco ON mco.mnit_nit = vmn.mnit_nit";
                Utils.FillDll(ddlAsociado, sql, true);
                ddlSede.Enabled = btnAsignarVends.Enabled = btnDesasignarVends.Enabled = false;
            }
        }

        private void BindVendedores(string sede)
        {
            string sql = string.Format(
                "SELECT pve.pven_secuencia AS CODIGO, \n" +
                 "       pve.pven_nombre AS NOMBRE, \n" +
                 "       pve.pubi_secuencia AS IDUBICACION, \n" +
                 "       pub.pubi_nombre AS UBICACION, \n" +
                 "       pve.pven_telefono AS TELEFONO, \n" +
                 "       pve.pven_email AS MAIL, \n" +
                 "       '../uploads/' || pve.pven_foto AS FOTO \n" +
                 "FROM pvendedor pve  \n" +
                 "  INNER JOIN pubicacion pub ON pve.pubi_secuencia = pub.pubi_secuencia \n" +
                 "WHERE pve.pubi_secuencia = {0}"
                 , sede);

            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
            dgVendedores.DataSource = ds;
            dgVendedores.DataBind();
        }

        private void LlenarDatosAsociado(string asociado)
        {
            ddlSede.Enabled = true;

            string sql = String.Format("select pubi_secuencia,pubi_nombre from pubicacion where mnit_nitconcesionario='{0}'", asociado);
            Utils.FillDll(ddlSede, sql, true);

            sql = String.Format("SELECT mcon_autovend FROM mconcesionario where mnit_nit='{0}'", asociado);
            string autoVend = DBFunctions.SingleData(sql);
            if (autoVend == "S")
                btnDesasignarVends.Enabled = true;
            else
                btnAsignarVends.Enabled = true;
        }

#endregion

    }
}