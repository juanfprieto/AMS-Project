using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Timers;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;
using System.Globalization;
using AMS.Documentos;



namespace AMS.VIP
{
    public partial class Prueba1 : System.Web.UI.UserControl
    {
       
        DataTable tblInfoUsuario = new DataTable();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Session.Remove("seciones");
            }
            DatasToControls bind = new DatasToControls();           
            bind.PutDatasIntoDropDownList(checkUser, "SELECT SUSU_CODIGO, SUSU_NOMBRE FROM SUSUARIO");
           checkUser.Items.Insert(0, "Seleccione un Usuario ...");
            nit_usuario.ReadOnly = true;

            if (Session["seciones"] == null)
            {
                LoadDataTable();
            }
            else
            {
                tblInfoUsuario = (DataTable)Session["seciones"];

            }
        }
        protected void ddldetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cdgLlave = checkUser.SelectedValue;
            string nombreCodigo = DBFunctions.SingleData("SELECT TTIPE_DESCRIPCION FROM TTIPOPERFIL WHERE TTIPE_CODIGO = (SELECT TTIPE_CODIGO FROM SUSUARIO WHERE SUSU_CODIGO =" + cdgLlave + ")");
            nit_usuario.Text = nombreCodigo;
            nit_usuario.Enabled = false;
        }
        protected void ValidarClave(object sender, EventArgs e)
        {
            string contra = "A123";
            
            if (password.Text.Equals(contra, StringComparison.CurrentCulture))
            {
                contenedor1.Visible = false;
                contenedor2.Visible = true;
                txtBienvenido.BackColor = Color.SkyBlue;
                txtBienvenido.Text = "Bienvenido mr. "  + checkUser.SelectedItem + " ";
                DataSet dbVisitaCliente = new DataSet();
                DBFunctions.Request(dbVisitaCliente, IncludeSchema.NO, "SELECT DV.PDOC_CODIGO, DV.DVIS_NUMEVISI, DV.DVIS_NOMBRE, DVC.DVISC_OBSERCONTACTO FROM DVISITADIARIACLIENTES DV, DVISITADIARIACLIENTESCONTACTO DVC WHERE DV.PCLAS_CODIGOVENTA ='RE' AND DV.DVIS_NUMEVISI = DVC.DVIS_NUMEVISI AND DV.PDOC_CODIGO = DVC.PDOC_CODIGO;");
                dgItems.DataSource = dbVisitaCliente.Tables[0];
                dgItems.DataBind();
                
            }
            else if(password.Text == "")
            {
                Utils.MostrarAlerta(Response, "Por favor, digite una contraseña");
                return;
            }
            else
            {
                Utils.MostrarAlerta(Response, "Contraseña Incorrecta!");
            }
        }
        protected void LoadDataTable()
        {
            tblInfoUsuario.Columns.Add("cedula");
            tblInfoUsuario.Columns.Add("nombre");
            tblInfoUsuario.Columns.Add("ciudad");
            tblInfoUsuario.Columns.Add("ahorrosActuales");
        }
        protected void RecibirDatos(object sender, EventArgs e)
        {
            contenedor2.Visible = false;
            contenedor3.Visible = true;
            dgInfoUsuario.DataSource = tblInfoUsuario;
            dgInfoUsuario.DataBind();                       
        }

        protected void AgregarInfoPersonal(object sender, DataGridCommandEventArgs e)
        {
            
            if (((Button)e.CommandSource).CommandName == "Delete")
            {
                int posicion = e.Item.ItemIndex;
                tblInfoUsuario.Rows[posicion].Delete();
                dgInfoUsuario.DataSource = tblInfoUsuario;
                dgInfoUsuario.DataBind();
            }
            else if (((Button)e.CommandSource).CommandName == "AddDatasRow")
            {
                if (((TextBox)e.Item.Cells[0].FindControl("txbCedula")).Text == "" || ((TextBox)e.Item.Cells[1].FindControl("txbNombre")).Text == "" || ((TextBox)e.Item.Cells[3].FindControl("txbAhorros")).Text == "")
                {
                    Utils.MostrarAlerta(Response, "Llene todos los campos, Porfavor..");
                    return;
                }

                DataRow filasInfoUsuario = tblInfoUsuario.NewRow();
                filasInfoUsuario[0] = ((TextBox)e.Item.Cells[0].FindControl("txbCedula")).Text;
                filasInfoUsuario[1] = ((TextBox)e.Item.Cells[1].FindControl("txbNombre")).Text;
                filasInfoUsuario[2] = ((DropDownList)e.Item.Cells[2].FindControl("ddlCiudades")).SelectedValue;
                filasInfoUsuario[3] = "$ " + ((TextBox)e.Item.Cells[3].FindControl("txbAhorros")).Text;
                tblInfoUsuario.Rows.Add(filasInfoUsuario); 
                dgInfoUsuario.DataSource = tblInfoUsuario;
                dgInfoUsuario.DataBind();
                Session["seciones"] = tblInfoUsuario;
                
            }
        }

        protected void DgInfoDataBound(object sender, DataGridItemEventArgs e)
        {
            
            if(e.Item.ItemType == ListItemType.EditItem)
                ((DropDownList)e.Item.Cells[2].FindControl("ddlCiudadesEdit")).SelectedValue = tblInfoUsuario.Rows[dgInfoUsuario.EditItemIndex][2].ToString();
        }

        public void DGInfoUsuarioCancel(Object sender, DataGridCommandEventArgs e)  
        {
            dgInfoUsuario.EditItemIndex = -1;
            dgInfoUsuario.DataSource = tblInfoUsuario;
            dgInfoUsuario.DataBind();
            return;
            
        }

        public void DGInfoUsuarioEdit(Object sender, DataGridCommandEventArgs e)
        {
            //if (tblInfoUsuario.Rows.Count > 0)
            dgInfoUsuario.EditItemIndex = e.Item.ItemIndex;
            dgInfoUsuario.DataSource = tblInfoUsuario;
            dgInfoUsuario.DataBind();
         
        }
        protected void DGInfoUsuarioUpdate(Object sender, DataGridCommandEventArgs e)
        {

            tblInfoUsuario.Rows[dgInfoUsuario.EditItemIndex][0] = ((TextBox)e.Item.Cells[0].FindControl("txbCedulaEdit")).Text;
            tblInfoUsuario.Rows[dgInfoUsuario.EditItemIndex][1] = ((TextBox)e.Item.Cells[1].FindControl("txbNombreEdit")).Text;
            tblInfoUsuario.Rows[dgInfoUsuario.EditItemIndex][2] = ((DropDownList)e.Item.Cells[2].FindControl("ddlCiudadesEdit")).SelectedValue;
            tblInfoUsuario.Rows[dgInfoUsuario.EditItemIndex][3] = ((TextBox)e.Item.Cells[3].FindControl("txbAhorrosEdit")).Text;
            dgInfoUsuario.EditItemIndex = -1;
            dgInfoUsuario.DataSource = tblInfoUsuario;
            dgInfoUsuario.DataBind();
        }
        
    }
}