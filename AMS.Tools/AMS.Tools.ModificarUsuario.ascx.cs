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
using System.Security.Cryptography;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{
    public partial class ModificarUsuario : System.Web.UI.UserControl
    {
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string mainPath = ConfigurationManager.AppSettings["PathToConf"];
        protected XmlDocument webConfig;
        protected XmlNode autorizacion;

        protected void Page_Load(object sender, System.EventArgs e)
        {

            //HtmlControl myControl = (HtmlControl)plcClaves.FindControl("txtPassActual");
            if (!IsPostBack)
            {
                String rolUsuario = DBFunctions.SingleData("SELECT ttipe_Codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "' AND susu_tipcrea<>'D'");
                if (rolUsuario != "AS")
                {
                    string tipoRol = DBFunctions.SingleData("select ttipe_rol from ttipoperfil where ttipe_codigo='" + rolUsuario + "';");
                    if (tipoRol == "A")
                        rolUsuario = "AS";
                }

                if (Request.QueryString["usr"] != null && rolUsuario != "AS")
                {
                    if (Request.QueryString["usr"] != HttpContext.Current.User.Identity.Name.ToLower())
                    {
                        plcClaves.Visible = false;
                    }
                }
                if (rolUsuario == "AS")
                {
                    txtPassActual.Visible = false;
                    txtIPs.Enabled = true;
                }

                DatasToControls bind = new DatasToControls();
                string NIT_USUARIOMODIFICAR = DBFunctions.SingleData("SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
                bool usuarioDependencia = Convert.ToBoolean(ConfigurationManager.AppSettings["CrearUsuarioConDependenciaRol"]);
                txtNit.Text = NIT_USUARIOMODIFICAR;
                if (NIT_USUARIOMODIFICAR != "" && usuarioDependencia && rolUsuario != "AS")
                {
                    txtNit.ReadOnly = true;
                    txtNit.Attributes.Remove("ondblclick");
                    bind.PutDatasIntoDropDownList(ddlperfil, " SELECT DISTINCT t.ttipe_codigo, t.ttipe_descripcion FROM TTIPOPERFIL t where t.ttipe_rol <> 'A' order by ttipe_descripcion;");
                }
                else
                {
                    bind.PutDatasIntoDropDownList(ddlperfil, " SELECT ttipe_codigo ,ttipe_descripcion FROM ttipoperfil order by 2");
                }
             
                //Si se ingresa como administrador, solo puedo cambiar el perfil
                if (Request.QueryString["usr"] != null && rolUsuario == "AS")
                {
                    
                    lbLogin.Text = Request.QueryString["usr"];
                    tbnombre.Text = DBFunctions.SingleData("SELECT susu_nombre FROM susuario WHERE susu_login='" + lbLogin.Text + "'");
                    DatasToControls.EstablecerValueDropDownList(ddlperfil, DBFunctions.SingleData("SELECT ttipe_codigo FROM susuario WHERE susu_login='" + lbLogin.Text + "'"));

                    //dejamos que pueda cambiar los datos, ya que un admin debe poder administrar los datos de usuario
                    //tbcont.Enabled=tbcontn.Enabled=tbcontnc.Enabled=tbnombre.Enabled=false;
                    String nitAsignado = DBFunctions.SingleData("SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + lbLogin.Text + "'");
                    txtNit.Text = nitAsignado;
                    String ipsValidas = DBFunctions.SingleData("SELECT susu_ipvalida FROM dbxschema.susuario WHERE susu_login='" + lbLogin.Text + "'");
                    txtIPs.Text = ipsValidas;
                    btnConfirmar.CausesValidation = false;
                }
                //Si ingreso como el usuario propio, puedo cambiar todos los datos menos el perfil
                else
                {
                    String ipsValidas = DBFunctions.SingleData("SELECT susu_ipvalida FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
                    txtIPs.Text = ipsValidas;
                    bind.PutDatasIntoDropDownList(ddlperfil, "SELECT ttipe_codigo ,ttipe_descripcion FROM ttipoperfil");
                    lbLogin.Text = HttpContext.Current.User.Identity.Name.ToLower();
                    tbnombre.Text = DBFunctions.SingleData("SELECT susu_nombre FROM susuario WHERE susu_login='" + lbLogin.Text + "'");
                    DatasToControls.EstablecerValueDropDownList(ddlperfil, DBFunctions.SingleData("SELECT ttipe_codigo FROM susuario WHERE susu_login='" + lbLogin.Text + "'"));
                    ddlperfil.Enabled = false;
                    txtNit.Enabled = false;
                    txtIPs.Enabled = false;
                    txtPassActual.Visible = true;
                    tbnombre.Enabled = false;
                }
                if (Request.QueryString["md"] != null)
                Utils.MostrarAlerta(Response, "Información Modificada Satisfactoriamente");
            }
            webConfig = new XmlDocument();
            try
            {
                webConfig.Load(mainPath + "Ucnf.xml");
                autorizacion = webConfig.ChildNodes[1];
            }
            catch (Exception exc)
            {
                lb.Text = exc.ToString();
            }
        }

        protected string Generar_MD5(string contrasena)
        {
            string contrasenaHexadecimal = "";
            byte []entrada = System.Text.Encoding.UTF8.GetBytes(contrasena);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte []salida = md5.ComputeHash(entrada);
            Converter myConverter = new Converter();
            for (int i=0; i < salida.Length; i++)
            {
                contrasenaHexadecimal += myConverter.Convertir_Decimal_Hexadecimal(System.Convert.ToInt32(salida[i]));
            }
            return contrasenaHexadecimal;
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

        protected void Modificar_Datos()
		{
			if (txtNit.Text!="")
				txtNit.Text="'"+txtNit.Text.Trim()+"'";
			else
				txtNit.Text="null";
			bool encontrado = false;
			XmlNode credenciales = autorizacion;
			XmlNode credencialUsuario = webConfig.DocumentElement;
			foreach(XmlNode oCurrentNode in credenciales.ChildNodes)
			{
                if (oCurrentNode.Attributes[0].Value.ToString().ToLower() == GlobalData.EMPRESA + "." + lbLogin.Text)
                {
                    credencialUsuario = oCurrentNode;
                    encontrado = true;
                    break;
                }
			}
			if(encontrado)
			{
                if (tbcontn.Text == tbcontnc.Text)
                {
                    if (Request.QueryString["usr"] != null)
                    {
                        //Si se cambio la contraseña
                        if (tbcontn.Text != string.Empty)
                        {
                            //Se cambia Ucnf.xml
                            credenciales.RemoveChild(credencialUsuario);
                            XmlElement credencial = webConfig.CreateElement("user");
                            credencial.SetAttribute("name", GlobalData.EMPRESA + "." + lbLogin.Text);
                            credencial.SetAttribute("password", this.Generar_MD5(tbcontn.Text));
                            credenciales.AppendChild(credencial);
                            webConfig.Save(mainPath + "Ucnf.xml");
                        }

                        if (DBFunctions.NonQuery("UPDATE susuario SET susu_nombre='" + tbnombre.Text + "',ttipe_codigo='" + ddlperfil.SelectedValue + "',mnit_nit=" + txtNit.Text + ", SUSU_IPVALIDA= '" + txtIPs.Text + "' WHERE susu_login='" + lbLogin.Text + "'") == 1)
                        {
                            if (txtNit.Text != "null")
                            {
                                DBFunctions.NonQuery("UPDATE dbxschema.mcontacto set SUSU_CODIGO=(select susu_codigo from dbxschema.susuario where mnit_nit=" + txtNit.Text + ") WHERE MNIT_NITCON=" + txtNit.Text + "; ");
                            }
                            Response.Redirect(indexPage + "?process=Tools.ModificarUsuario&usr=" + Request.QueryString["usr"] + "&md=1");
                        }
                        else
                            lb.Text = "Error : " + DBFunctions.exceptions;
                    }
                    else
                    {
                        //Si la contraseña digitada es la del usuario
                        if (Generar_MD5(tbcont.Text) == credencialUsuario.Attributes["password"].Value)
                        {
                            //Se cambia tanto base de datos como Ucnf.xml
                            credenciales.RemoveChild(credencialUsuario);
                            XmlElement credencial = webConfig.CreateElement("user");
                            credencial.SetAttribute("name", GlobalData.EMPRESA + "." + lbLogin.Text);
                            //Si se cambio la contraseña
                            if (tbcontn.Text != string.Empty)
                                credencial.SetAttribute("password", this.Generar_MD5(tbcontn.Text));
                            //Si se dejo la misma
                            else
                                credencial.SetAttribute("password", this.Generar_MD5(tbcont.Text));
                            credenciales.AppendChild(credencial);
                            webConfig.Save(mainPath + "Ucnf.xml");

                            if (DBFunctions.NonQuery("UPDATE susuario SET susu_nombre='" + tbnombre.Text + "',ttipe_codigo='" + ddlperfil.SelectedValue + "',mnit_nit=" + txtNit.Text + " WHERE susu_login='" + lbLogin.Text + "'") == 1)
                            {
                                Response.Redirect(indexPage + "?process=Tools.ModificarUsuario&md=1");
                            }
                            else
                                lb.Text = "Error : " + DBFunctions.exceptions;
                        }
                        else
                            Utils.MostrarAlerta(Response, "Contraseña Incorrecta");
                    }
                }
                else
                Utils.MostrarAlerta(Response, "Las contraseñas no coinciden");
            }
            txtNit.Text = txtNit.Text.Replace("'","");
		}

        protected void btnConfirmar_Click(object sender, System.EventArgs e)
        {
            Modificar_Datos();
        }

        protected void btnCancelar_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(indexPage + "?process=Tools.Usuarios");
        }
    }
}
