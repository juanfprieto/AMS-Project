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
	public partial class CrearUsuario : System.Web.UI.UserControl 
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string mainPath = ConfigurationManager.AppSettings["MainPath"];
		protected string pathToConfig= ConfigurationManager.AppSettings["PathToConf"];
		protected XmlDocument webConfig;
		protected XmlNode autorizacion;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();

                bool usuarioDependencia = Convert.ToBoolean(ConfigurationManager.AppSettings["CrearUsuarioConDependenciaRol"]);

                if (usuarioDependencia)
                {
                    string NIT_ususario = DBFunctions.SingleData("SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
                    txtNit.Text = NIT_ususario;

                    if (txtNit.Text != "")
                    {
                        txtNit.ReadOnly = true;
                        txtNit.Attributes.Remove("ondblclick");
                        bind.PutDatasIntoDropDownList(tipoPerfil, "SELECT DISTINCT t.ttipe_codigo, t.ttipe_descripcion FROM TTIPOPERFIL t where t.ttipe_rol <> 'A' order by ttipe_descripcion;");
                    }
                    else
                    {
                        bind.PutDatasIntoDropDownList(tipoPerfil, "SELECT ttipe_codigo, ttipe_descripcion FROM ttipoperfil order by ttipe_descripcion");
                    }
                }
                else
                {
                    txtNit.ReadOnly = false;
                    txtNit.Text = "";
                    bind.PutDatasIntoDropDownList(tipoPerfil, "SELECT ttipe_codigo, ttipe_descripcion FROM ttipoperfil order by ttipe_descripcion");
                }
			}
			webConfig = new XmlDocument();
			try
			{
				webConfig.Load(pathToConfig+"Ucnf.xml");
				autorizacion = webConfig.ChildNodes[1];
			}
			catch(Exception exc)
			{
				lb.Text = exc.ToString();
			}
		}
		
		protected  void Cancelar(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Tools.Usuarios");
		}
		
		protected  void Aceptar_Nuevo_Usuario(Object  Sender, EventArgs e)
		{
			int error=0;
			if((nombreUsuario.Text=="")||(loginUsuario.Text=="")||(contrasenaUsuario.Text=="")||(verificarContrasena.Text==""))
			{
                Utils.MostrarAlerta(Response, "Falta Algun Dato Por Escribir, Por Favor Verifique");
				error=1;
			}
            if(loginUsuario.Text.Contains("."))
            {
                Utils.MostrarAlerta(Response, "En el campo de Login usuario no se permiten puntos, revise por favor");
                error = 1;
            }
			if (txtNit.Text!="")
			{
				if (!DBFunctions.RecordExist("Select mnit_nit from DBXSCHEMA.MNIT where mnit_nit='"+txtNit.Text+"'"))
				{
                    Utils.MostrarAlerta(Response, "El nit ingresado es invalido,Revise por favor!");
					error=1;
				}
				txtNit.Text="'"+txtNit.Text.Trim()+"'";
			}
			else
				txtNit.Text="null";
			if (error==0)
			{
				//primero verificamos que los dos campos de texto destinados a la contraseña sean iguales
				if(contrasenaUsuario.Text!=verificarContrasena.Text)
                    Utils.MostrarAlerta(Response, "Contraseña No Valida, Por Favor Verifique Los Campos de Constraseña");
				else
				{
                    //Identificador de empresa para cada usuario.
                    string userLogin = loginUsuario.Text.ToLower(); 
                    if (!DBFunctions.RecordExist("SELECT * FROM susuario WHERE susu_login='" + userLogin + "'"))
					{
						//Primero Realizamos el cambio en el Web.Config
						XmlNode credenciales = autorizacion;
						XmlElement credencial = webConfig.CreateElement("user");
                        credencial.SetAttribute("name", GlobalData.getEMPRESA() + "." + userLogin);  //para 2015
                        //credencial.SetAttribute("name", userLogin);
                        credencial.SetAttribute("password",this.Generar_MD5(contrasenaUsuario.Text));
						credenciales.AppendChild(credencial);
						webConfig.Save(pathToConfig+"Ucnf.xml");
						//Luego grabamos en la base de datos la informacion del usuario
						DataSet usuarios = new DataSet();
						DBFunctions.Request(usuarios,IncludeSchema.NO,"SELECT * FROM susuario ORDER BY susu_codigo ASC");
						int numero = System.Convert.ToInt32(usuarios.Tables[0].Rows[(usuarios.Tables[0].Rows.Count-1)][0].ToString());
                        
                        try
                        {
                            //Validacion de usuarios por IP, modifica tabla SUSUARIO -> SUSU_IPVALIDA VARCHAR(100)
                            DBFunctions.NonQuery("INSERT INTO susuario VALUES(" + (numero + 1).ToString() + ",'" + nombreUsuario.Text + "','" + userLogin + "','" + tipoPerfil.SelectedValue.ToString() + "',default,null,default," + txtNit.Text + ",'" + txtIPs.Text + "')");
                        }
                        catch (Exception er)
                        {
                            DBFunctions.NonQuery("INSERT INTO susuario VALUES(" + (numero + 1).ToString() + ",'" + nombreUsuario.Text + "','" + userLogin + "','" + tipoPerfil.SelectedValue.ToString() + "',default,null,default," + txtNit.Text + ")");
                        }

                        if (txtNit.Text != "null")
                        {
                            DBFunctions.NonQuery("UPDATE dbxschema.mcontacto set SUSU_CODIGO=" + (numero + 1).ToString() + " WHERE MNIT_NITCON=" + txtNit.Text + "; ");
                        }
                        Response.Redirect("" + indexPage + "?process=Tools.Usuarios&creado=1");
					}
					else
                    Utils.MostrarAlerta(Response, "Este Login es Usado Por otro usuario, Por Favor Cambielo");
				}
			}
			txtNit.Text="";
		}
		
		protected string Generar_MD5(string contrasena)
		{
			string contrasenaHexadecimal = "";
			byte []entrada = System.Text.Encoding.UTF8.GetBytes(contrasena);
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte []salida = md5.ComputeHash(entrada);
			Converter myConverter = new Converter();
			for(int i=0;i<salida.Length;i++)
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
	}
}
