using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using AMS.DB;
using System.Collections;
using AMS.Tools;

namespace AMS.Automotriz
{
    public partial class DatosCliente : System.Web.UI.UserControl
	{

        public string Nombre { get { return nombre.Text; } }
        public string Nombre2 { get { return nombre2.Text; } }
        public string Apellido { get { return apellido.Text; } }
        public string Apellido2 { get { return apellido2.Text; } }
        public string Telefono { get { return telefono.Text; } }
        public string Celular { get { return celular.Text; } }
        public string Email { get { return correo.Text; } }
        public string Nombrecompleto { get { return nombre.Text + " " + nombre2.Text +" " + apellido.Text + " " + apellido2.Text; } }
        public string Colores { get { return HiColores.Value; } }

        private string nit;

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                
            }

            nit = ViewState["nit"] != null ? ViewState["nit"].ToString() : null;
            
		}
        public void CargarDatosIniciales(string nit)
        {
            this.nit = nit;
            ViewState["nit"] = nit;

            string sql = String.Format(
                "SELECT mnit_nombres, \n" +
                "mnit_nombre2, \n" +
                "mnit_apellidos, \n" +
                "mnit_apellido2, \n" +
                "       mnit_telefono, \n" +
                "       mnit_celular, \n" +
                "       mnit_email \n" +
                "FROM mnit \n" +
                "WHERE mnit_nit = '{0}'"
                , nit);

            ArrayList clienteArray = DBFunctions.RequestAsCollection(sql);

            if (clienteArray.Count <= 0)
                return;

            Hashtable cliente = (Hashtable) clienteArray[0];
            nombre.Text = cliente["MNIT_NOMBRES"].ToString();
            nombre2.Text = cliente["MNIT_NOMBRE2"].ToString();
            apellido.Text = cliente["MNIT_APELLIDOS"].ToString();
            apellido2.Text = cliente["MNIT_APELLIDO2"].ToString();
            telefono.Text = cliente["MNIT_TELEFONO"].ToString();
            celular.Text = cliente["MNIT_CELULAR"].ToString();
            correo.Text = cliente["MNIT_EMAIL"].ToString();
            ValidarNit(); 
                              
        }

        public bool TieneSuficientesDatos()
        {
            return nombre.Text != "" && (telefono.Text != "" || celular.Text != "" || correo.Text != "");
        }
        public void ValidarNit()
        {
            string sql = "SELECT TNIT_TIPONIT FROM mnit WHERE mnit_nit = '"+nit+"'";
            ArrayList Validar = DBFunctions.RequestAsCollection(sql);
            
            if (Validar.Count <= 0)
                return;

            Hashtable validar = (Hashtable)Validar[0];

            if (validar["TNIT_TIPONIT"].ToString() == "N")  // EMPRESA es nit "N"
            {
                lblSegundoNombre.Visible = false;
                lblSegundoApellido.Visible = false;
                nombre2.Visible = false;
                apellido2.Visible = false;
                lblPrimerNombre.Text = "Sigla";
                lblPrimerApellido.Text = "Nombre Empresa";
            }
            
            
        }
        //protected void Rojo_OnClick(Object Sender, EventArgs e) 
        //{                       
        //        //Rojo.InnerHtml = "<img src = '../img/semaforo/rojo2.png' alt = 'imagen' />";
        //        Utils.MostrarAlerta(Response, "ROJO");      
        //}

        protected void GuardarCliente_OnClick(Object Sender, EventArgs e)
        {
            Hashtable datosUpdate = new Hashtable();
            Hashtable datosPk = new Hashtable();
            string tabla = "MNIT";

            datosUpdate.Add("MNIT_NOMBRES", Utils.ToVarchar(nombre.Text));
            datosUpdate.Add("MNIT_NOMBRE2", Utils.ToVarchar(nombre2.Text));
            datosUpdate.Add("MNIT_APELLIDOS", Utils.ToVarchar(apellido.Text));
            datosUpdate.Add("MNIT_APELLIDO2", Utils.ToVarchar(apellido2.Text));
            datosUpdate.Add("MNIT_TELEFONO", Utils.ToVarchar(telefono.Text));
            datosUpdate.Add("MNIT_CELULAR", Utils.ToVarchar(celular.Text));
            datosUpdate.Add("MNIT_EMAIL", Utils.ToVarchar(correo.Text));
            datosPk.Add("MNIT_NIT", Utils.ToVarchar(nit));
                       
       //     DBFunctions.UpdateHashtable(tabla, datosUpdate, datosPk);   // no se actualiza la tabla de MNIT porque no trae el nit correcto, esta actualizando el nit que NO es.    

           }

	}
}
