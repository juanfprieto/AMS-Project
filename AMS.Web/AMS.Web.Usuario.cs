using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Xml;
using AMS.Tools;
using AMS.DB;


namespace AMS.Web
{
	public class Usuario
	{
		private XmlDocument archivoUsuarios;
		
		public Usuario()
		{
			archivoUsuarios = new XmlDocument();
			archivoUsuarios.Load(ConfigurationManager.AppSettings["PathToConf"]+"Ucnf.xml");
		}


        public string AutenticarUsuario(string nombreUsuario, string clave)
		{

            string passMD5 = Generar_MD5(clave);
			XmlNode raiz = archivoUsuarios.ChildNodes[1];
			string existe = "false";
			for(int i=0;i<raiz.ChildNodes.Count;i++)
			{
				XmlNode actual = raiz.ChildNodes[i];
				string usuario = actual.Attributes["name"].Value;
				string password = actual.Attributes["password"].Value;
                if (usuario.ToLower().Trim() == nombreUsuario.ToLower().Trim() && passMD5 == password)
				{
                    string estadoUsu = DBFunctions.SingleData("SELECT SUSU_TIPCREA FROM SUSUARIO WHERE SUSU_LOGIN = '" + nombreUsuario.ToLower().Trim().Split('.')[1] + "'");
                    if (estadoUsu == "") existe = "noEncontrado";
                    else if(estadoUsu != "C")
                    {
                        existe = "Borrado";
                        break;
                    }
                    else
                    {
                        existe = "true";
                        break;
                    }
					
				}
			}
			return existe;
		}

		private string Generar_MD5(string contrasena)
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
	}
}
