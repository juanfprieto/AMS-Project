using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.CriptoServiceProvider;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{

	/// <summary>
	///		Este es el control que maneja la configuración inicial de la
	///		aplicación, aca se asignan/cambian los valores del archivo
	///		Gcnf.config, se puede cargar desde una opción del menú o desde
	///		el Global.asax
	/// </summary>
	public partial class ConfiguracionInicial : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected string path=ConfigurationManager.AppSettings["MainPath"];
		private XmlDocument conf;
		private XmlNode nodo;
		private XmlNodeList nodosReemplazar;
				
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				if(Request.QueryString["first"]!=null)
					btnCancelar.Visible=false;
				ddlcentavos.Items.Add(new ListItem("Si","S"));
				ddlcentavos.Items.Add(new ListItem("No","N"));
				ddlbd.Items.Add(new ListItem("DB2",DBAdapters.DB2.ToString()));
				ddlbd.Items.Add(new ListItem("MySQL",DBAdapters.MYSQL.ToString()));
				ddlbd.Items.Add(new ListItem("ODBC",DBAdapters.ODBC.ToString()));
				ddlbd.Items.Add(new ListItem("ORACLE",DBAdapters.ORACLE.ToString()));
				ddlbd.Items.Add(new ListItem("SQL Server",DBAdapters.SQLSERVER.ToString()));
				Llenar_Controles(tbcorreo,tbcont,tbservidor,ddlcentavos,tbmoneda,ddlbd,tbserver,tbbd,tbusuario,tbcontbd,tbesq,Get_Parametros());
			}
			try
			{
				conf=new XmlDocument();
				conf.Load(path+"Gcnf.config");
				nodo=conf.DocumentElement;
				nodosReemplazar=nodo.SelectNodes("child::*");
			}
			catch(Exception ex)
			{
				lb.Text="Error al inicializar archivo de configuración : "+ex.Message;
			}
		}

		/// <summary>
		///	Get_Parametros : función que permite obtener los valores de las
		///	variables de configuración existentes dentro del archivo Gcnf.config
		///	y las almacena en un arreglo de strings
		/// </summary>
		/// <returns>
		/// Retorna un arreglo de string con los valores de las variables de
		/// configuración
		/// </returns>
		private string[] Get_Parametros()
		{
			string iv=ConfigurationManager.AppSettings["VectorInicialEncriptacion"];//1
			string comp=ConfigurationManager.AppSettings["ValorConcatClavePrivada"];//2
			string email=ConfigurationManager.AppSettings["EMailFrom"];//3
			string pwdemail=ConfigurationManager.AppSettings["PasswordEMail"];//4
			string mserver=ConfigurationManager.AppSettings["MailServer"];//5
			string centavos=ConfigurationManager.AppSettings["ManejoCentavos"];//6
			string moneda=ConfigurationManager.AppSettings["MonedaNacional"];//7
			string dbcon=ConfigurationManager.AppSettings["DBConnectionType"];//8
			string server=ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];//9
			string bd=ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];//10
			string user=ConfigurationManager.AppSettings["UID"];//11
			string pwd=ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];//12
			string schema=ConfigurationManager.AppSettings["Schema"];//13
			string[] parametros=new string[13];
			parametros[0]=iv;
			parametros[1]=comp;
			parametros[2]=email;
			parametros[3]=pwdemail;
			parametros[4]=mserver;
			parametros[5]=centavos;
			parametros[6]=moneda;
			parametros[7]=dbcon;
			parametros[8]=server;
			parametros[9]=bd;
			parametros[10]=user;
			parametros[11]=pwd;
			parametros[12]=schema;
			return parametros;
		}
		
		/// <summary>
		/// Llenar_Controles : función que llena los controles del formulario
		/// con los valores que se traen en el arreglo que devuelve la función
		/// Get_Parametros
		/// </summary>
		/// <param name="tb1">TextBox que muestra el Correo</param>
		/// <param name="tb2">TextBox que muestra la contraseña del correo</param>
		/// <param name="tb3">TextBox que muestra el servidor de correo</param>
		/// <param name="ddl4">DropDownList para el manejo de centavos</param>
		/// <param name="tb5">TextBox que muestra la moneda nacional</param>
		/// <param name="ddl6">DropDownList para el manejo del adaptador de la BD</param>
		/// <param name="tb7">TextBox que muestra el nombre del servidor de la BD</param>
		/// <param name="tb8">TextBox que muestra el nombre de la BD</param>
		/// <param name="tb9">TextBox que muestra el nombre del usuario de la BD</param>
		/// <param name="tb10">TextBox que muestra el password de la BD</param>
		/// <param name="tb11">TextBox que muestra el esquema usado en la BD</param>
		/// <param name="param">Arreglo de tipo string que trae las variables almacenadas en el Gcnf.config</param>
		private void Llenar_Controles(TextBox tb1,TextBox tb2,TextBox tb3,DropDownList ddl4,TextBox tb5,DropDownList ddl6,TextBox tb7,TextBox tb8,TextBox tb9,TextBox tb10,TextBox tb11,params string[] param)
		{
			tb1.Text=param[2];
			tb2.Text=param[3];
			tb3.Text=param[4];
			if(param[5]!=string.Empty && param[5]!=null)
				ddl4.SelectedValue=param[5];
			tb5.Text=param[6];
			if(param[7]!=string.Empty && param[7]!=null)
				ddl6.SelectedValue=param[7];
			tb7.Text=param[8];
			tb8.Text=param[9];
			tb9.Text=param[10];
			tb10.Text=param[11];
			tb11.Text=param[12];
		}

		/// <summary>
		/// Guardar : Función relacionada con el evento OnClick del botón btnGuardar 
		/// Modifica o inserta si es necesario los valores de las variables de configuración
		/// del archivo Gcnf.config, si no existe arreglo inicializador o vector de
		/// concatenación para la encriptación los genera automaticamente
		/// </summary>
		/// <param name="Sender">object que hizo el envio al servidor</param>
		/// <param name="e">EventArgs que maneja la información de los argumentos del evento</param>
		protected void Guardar(object Sender,EventArgs e)
		{
			string[] pars=Get_Parametros();
			string iv="",key="",correo="",password="",servidorCorreo="",centavos="",moneda="",adaptador="",conString="",servidorBD="",bd="",usuario="",passwordBD="",schema="";
			XmlElement vector=null,clave=null,email=null,pwd=null,mailserver=null,cent=null,mon=null,adapter=null,server=null,database=null,usuarioBD=null,passwordDB=null,esquema=null;
			bool exito=false;
			Crypto miCripto=new Crypto(CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            //Arreglo IV y Key pa encripción
			if(pars[0]==string.Empty && pars[1]==string.Empty)
			{
				Generar_Claves(ref iv,ref key);
				vector=conf.CreateElement("add");
				clave=conf.CreateElement("add");
				vector.SetAttribute("key","VectorInicialEncriptacion");
				vector.SetAttribute("value",iv);
				clave.SetAttribute("key","ValorConcatClavePrivada");
				clave.SetAttribute("value",key);
				try
				{
					nodo.ReplaceChild(vector,nodosReemplazar[0]);
					nodo.ReplaceChild(clave,nodosReemplazar[1]);
					conf.Save(path+"Gcnf.config");
					conf.Load(path+"Gcnf.config");
					pars=Get_Parametros();
					nodo=conf.DocumentElement;
					nodosReemplazar=nodo.SelectNodes("child::*");
					pars=Get_Parametros();
				}
				catch(Exception ex)
				{
					lb.Text="Error Interno. Detalles : <br>"+ex.Message;
					return;
				}
			}
			//Correo
			if(tbcorreo.Text!=string.Empty)
			{
				correo=tbcorreo.Text;
				email=conf.CreateElement("add");
				email.SetAttribute("key","EmailFrom");
				email.SetAttribute("value",correo);
			}
			//Password correo
			if(tbcont.Text!=string.Empty)
			{
				password=tbcont.Text;
				miCripto.IV=pars[0];
				miCripto.Key=pars[1];
				string passwCrip=miCripto.CifrarCadena(password);
				pwd=conf.CreateElement("add");
				pwd.SetAttribute("key","PasswordEMail");
				pwd.SetAttribute("value",passwCrip);
			}
			//Servidor de correo
			if(tbservidor.Text!=string.Empty)
			{
				servidorCorreo=tbservidor.Text;
				mailserver=conf.CreateElement("add");
				mailserver.SetAttribute("key","MailServer");
				mailserver.SetAttribute("value",servidorCorreo);
			}
			//Manejo de centavos
			if(ddlcentavos.SelectedValue!=pars[5])
			{
				centavos=ddlcentavos.SelectedValue;
				cent=conf.CreateElement("add");
				cent.SetAttribute("key","ManejoCentavos");
				cent.SetAttribute("value",centavos);
			}
            //Moneda nacional
			if(tbmoneda.Text!=string.Empty)
			{
				moneda=tbmoneda.Text;
				mon=conf.CreateElement("add");
				mon.SetAttribute("key","MonedaNacional");
				mon.SetAttribute("value",moneda.ToUpper());
			}
			//Adaptador
			if(ddlbd.SelectedValue!=pars[7])
			{
				adaptador=ddlbd.SelectedValue;
				adapter=conf.CreateElement("add");
				adapter.SetAttribute("key","DBConnectionType");
				adapter.SetAttribute("value",adaptador);
			}
			//Servidor de base de datos
			if(tbserver.Text!=string.Empty)
			{
				servidorBD=tbserver.Text;
				server=conf.CreateElement("add");
				server.SetAttribute("key","Server");
				server.SetAttribute("value",servidorBD);
			}
            //Base de datos
			if(tbbd.Text!=string.Empty)
			{
				bd=tbbd.Text;
				database=conf.CreateElement("add");
				database.SetAttribute("key","DataBase");
				database.SetAttribute("value",bd);
			}
			//Usuario de la BD
			if(tbusuario.Text!=string.Empty)
			{
				usuario=tbusuario.Text;
				usuarioBD=conf.CreateElement("add");
				usuarioBD.SetAttribute("key","UID");
				usuarioBD.SetAttribute("value",usuario);
			}
			//Password del usuario de la BD
			if(tbcontbd.Text!=string.Empty)
			{
				passwordBD=tbcontbd.Text;
				miCripto.IV=pars[0];
				miCripto.Key=pars[1];
				string passwDBEnc=miCripto.CifrarCadena(passwordBD);
				passwordDB=conf.CreateElement("add");
				passwordDB.SetAttribute("key","PWD");
				passwordDB.SetAttribute("value",passwDBEnc);
			}
			//Esquema
			if(tbesq.Text!=string.Empty)
			{
				schema=tbesq.Text;
				esquema=conf.CreateElement("add");
				esquema.SetAttribute("key","Schema");
				esquema.SetAttribute("value",schema);
			}
            //Teniendo toda la configuración, procedemos a reemplazar
			//valores en el Gcnf.config
			try
			{
				if(email!=null)
					nodo.ReplaceChild(email,nodosReemplazar[2]);
				if(pwd!=null)
					nodo.ReplaceChild(pwd,nodosReemplazar[3]);
				if(mailserver!=null)
					nodo.ReplaceChild(mailserver,nodosReemplazar[4]);
				if(cent!=null)
					nodo.ReplaceChild(cent,nodosReemplazar[5]);
				if(mon!=null)
					nodo.ReplaceChild(mon,nodosReemplazar[6]);
				if(adapter!=null)
					nodo.ReplaceChild(adapter,nodosReemplazar[7]);
				if(server!=null)
					nodo.ReplaceChild(server,nodosReemplazar[8]);
				if(database!=null)
					nodo.ReplaceChild(database,nodosReemplazar[9]);
				if(usuarioBD!=null)
					nodo.ReplaceChild(usuarioBD,nodosReemplazar[10]);
				if(passwordDB!=null)
					nodo.ReplaceChild(passwordDB,nodosReemplazar[11]);
				if(esquema!=null)
					nodo.ReplaceChild(esquema,nodosReemplazar[12]);
				conf.Save(path+"Gcnf.config");
				exito=true;
			}
			catch(Exception ex)
			{
				lb.Text="Error Interno. Detalles : <br>"+ex.Message;
			}
			finally
			{
				if(exito)
					Response.Redirect(indexPage+"?process=Tools.ConfiguracionInicial&cnf=1&first=1&eds=0");
			}
		}

		/// <summary>
		/// Cancelar : función relacionada con el evento onClick del botón btnCancelar
		/// Redirecciona a la página de inicio de la aplicación
		/// </summary>
		/// <param name="Sender">object que hizo el envio al servidor</param>
		/// <param name="e">EventArgs que maneja la información de los argumentos del evento</param>
		protected void Cancelar(object Sender,EventArgs e)
		{
			Response.Redirect(indexPage);
		}

		/// <summary>
		/// Generar_Claves : función que genera el vector de inicialización y la clave de concatenación
		/// para el algoritmo TripleDes
		/// </summary>
		/// <param name="iv">string por referencia donde se almacenara el valor del vector de inicialización</param>
		/// <param name="key">string por referencia donde se almacenara el valor de la clave de concatenación</param>
		private void Generar_Claves(ref string iv,ref string key)
		{
			TripleDESCryptoServiceProvider alg=new TripleDESCryptoServiceProvider();
			alg.GenerateIV();
			alg.GenerateKey();
			iv=ASCIIEncoding.ASCII.GetString(alg.IV);
			key=ASCIIEncoding.ASCII.GetString(alg.Key);
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
	}
}
