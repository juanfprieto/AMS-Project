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
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{
	public partial class Usuarios : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string mainPath = ConfigurationManager.AppSettings["PathToConf"];
		protected DropDownList modusuario;
		protected XmlDocument webConfig;
		protected System.Web.UI.WebControls.Button modificar;
		protected XmlNode autorizacion;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
                string NIT_ususario = DBFunctions.SingleData("SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
				DatasToControls bind = new DatasToControls();
                bool usuarioDependencia = Convert.ToBoolean(ConfigurationManager.AppSettings["CrearUsuarioConDependenciaRol"]);
                if (NIT_ususario != "" && usuarioDependencia)
                {
                    //Campo ttipe_rol debe existir en susuario.
                    bind.PutDatasIntoDropDownList(usuario, "SELECT susu_login,susu_nombre FROM susuario s, TTIPOPERFIL t WHERE s.susu_tipcrea<>'D' AND s.susu_login <>'" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "' and s.mnit_nit = '" + NIT_ususario + "' and t.ttipe_codigo=s.ttipe_codigo and t.ttipe_rol <> 'A' and susu_login not like '%_eliminado' order by s.susu_nombre");
                    bind.PutDatasIntoDropDownList(ddlmodificar, "SELECT susu_login,susu_nombre FROM susuario s, TTIPOPERFIL t WHERE s.susu_tipcrea<>'D' and mnit_nit = '" + NIT_ususario + "' and t.ttipe_codigo=s.ttipe_codigo and t.ttipe_rol <> 'A'  and susu_login not like '%_eliminado' order by susu_nombre");

                }
                else 
                {
                    bind.PutDatasIntoDropDownList(usuario, "SELECT susu_login,susu_nombre FROM susuario WHERE susu_tipcrea<>'D' AND susu_login <>'" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'  and susu_login not like '%_eliminado' order by susu_nombre");
                    bind.PutDatasIntoDropDownList(ddlmodificar, "SELECT susu_login,susu_nombre FROM susuario WHERE susu_tipcrea<>'D'  and susu_login not like '%_eliminado' order by susu_nombre");
                }
				if(Request.QueryString["el"]!=null)
                    Utils.MostrarAlerta(Response, "Usuario Eliminado Satisfactoriamente!");
                if (Request.QueryString["creado"] != null)
                    Utils.MostrarAlerta(Response, "Usuario Creado Satisfactoriamente!");

			}
			//Aqui debemos cargar el archivo xml Web.Config
			webConfig = new XmlDocument();
			try
			{
				webConfig.Load(mainPath+"Ucnf.xml");
				autorizacion = webConfig.ChildNodes[1];
			}
			catch(Exception exc)
			{
				lb.Text = exc.ToString();
			}
		}
		
		protected void Ingresar_Crear(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Tools.CrearUsuario");
		}
		
		protected void Ingresar_Administrador(Object Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Tools.SesionesAbiertas");
		}
		
		protected void Eliminar_Usuario(Object  Sender, EventArgs e)
		{
			if(usuario.Items.Count>0)
			{
				bool encontrado = false;
				string login = usuario.SelectedValue;
				XmlNode credenciales = autorizacion;
				XmlNode credencialUsuario = webConfig.DocumentElement;
				foreach(XmlNode oCurrentNode in credenciales.ChildNodes)
				{
                    if(oCurrentNode.Attributes[0].Value.ToString() == GlobalData.getEMPRESA() + "." + login) //para 2015
                    //if (oCurrentNode.Attributes[0].Value.ToString() == login)
                    {
						credencialUsuario = oCurrentNode;
						encontrado = true;
					}
				}
				if(encontrado)
				{
					credenciales.RemoveChild(credencialUsuario);
					webConfig.Save(mainPath+"Ucnf.xml");
					//Ahora lo eliminamos de la base de datos al usuario
                    {
                        //DBFunctions.NonQuery("delete from susuario where susu_login='" + login + "_eliminado'; ");
                        //DBFunctions.NonQuery("update  susuario set SUSU_TIPCREA = 'D',susu_nombre=susu_nombre||' ELIMINADO', susu_login=susu_login||'_eliminado' WHERE susu_nombre='" + usuario.SelectedItem.ToString() + "' AND susu_login='" + login + "'");
                        DBFunctions.NonQuery("update  susuario set SUSU_TIPCREA = 'D',susu_nombre=susu_nombre||' ELIMINADO' WHERE susu_nombre='" + usuario.SelectedItem.ToString() + "' AND susu_login='" + login + "'");
                    }
                    Utils.MostrarAlerta(Response, "Usuario Eliminado Con Exito");
					Response.Redirect("" + indexPage + "?process=Tools.Usuarios&el=1");
				}
				else
				{
					//Cuando no esta en el archivo.
				//	DBFunctions.NonQuery("DELETE FROM susuario WHERE susu_nombre='"+usuario.SelectedItem.ToString()+"' AND susu_login='"+login+"'");					
                    DBFunctions.NonQuery("delete from susuario where susu_login='" + login + "_eliminado'; ");
                    //DBFunctions.NonQuery("update  susuario set SUSU_TIPCREA = 'D',susu_nombre=susu_nombre||' Eliminado', susu_login=susu_login||'_eliminado' WHERE susu_nombre='" + usuario.SelectedItem.ToString() + "' AND susu_login='" + login + "'");
                    DBFunctions.NonQuery("update  susuario set SUSU_TIPCREA = 'D' WHERE susu_nombre='" + usuario.SelectedItem.ToString() + "' AND susu_login='" + login + "'");
                    Response.Redirect("" + indexPage + "?process=Tools.Usuarios&el=1");
				}
					
			}
			else
            Utils.MostrarAlerta(Response, "No Se ha Seleccionado ningun Usuario");
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

		protected void btnModificar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Tools.ModificarUsuario&usr="+ddlmodificar.SelectedValue+"");
		}
	
	}
}
