using System;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

 

namespace AMS.DB
{
	public enum TipoRegistro
	{
		Actividad,
		Error,
		Informacion,
		ErrorCrystalReports,
		InformacionCrystalRepots
	}

	public class CarpetaRegistro
	{
		private const string formatoCarpetaRaizCompleto = "{0}\\{1}\\{2}";
		private const string formatoNombreArchivoCompleto = "{0}\\{1}\\{2}\\{3}{4}{5}";
		private const string extension = ".xml";

		private string carpetaRaiz;
		private string usuario;
		private DateTime fecha;
		private string nombreArchivo;

		public CarpetaRegistro(string carpetaRaiz, string usuario, string nombreArchivo)
		{
			this.CarpetaRaiz = carpetaRaiz;
			this.Usuario = usuario;
			this.fecha = DateTime.Now;
			this.NombreArchivo = nombreArchivo;
		}

		public string CarpetaRaiz{get{return this.carpetaRaiz;}set{this.carpetaRaiz = value;}}
		public string Usuario{get{return usuario;}set{usuario = value;}}
		public string Mes{get{return this.ObtenerNombreMes(fecha.Month);}}
		public string Dia{get{return fecha.Day.ToString();}}
		public string NombreArchivo{get{return this.nombreArchivo;}set{this.nombreArchivo = value;}}
		public string CarpetaRaizCompleto{get{return string.Format(formatoCarpetaRaizCompleto,this.CarpetaRaiz,this.Usuario,this.Mes);}}
		public string NombreArchivoCompleto{get{return string.Format(formatoNombreArchivoCompleto,this.CarpetaRaiz,this.Usuario,this.Mes,this.NombreArchivo,this.Dia,extension);}}

		private string ObtenerNombreMes(int mes)
		{
			switch(mes)			
			{
				case 1:
					return "Enero";
				case 2:
					return "Febrero";
				case 3:
					return "Marzo";
				case 4:
					return "Abril";
				case 5:
					return "Mayo";
				case 6:
					return "Junio";
				case 7:
					return "Julio";
				case 8:
					return "Agosto";
				case 9:
					return "Septiembre";
				case 10:
					return "Octubre";
				case 11:
					return "Noviembre";
				case 12:
					return "Diciembre";
				default:
					return string.Empty;
			}
		}
	}

	public class Registro
	{
		private DateTime fecha;
		private string usuario;
		private TipoRegistro tipoRegistro;
		private string sql;
		private string nombreExcepcion;
		private string mensajeExcepcion;
		private string pilaLlamados;
		private string observaciones;

		public Registro() { }
		public Registro(string usuario, TipoRegistro tipoRegistro, string sql, Exception excepcion, string observaciones)
		{
			this.Fecha = DateTime.Now;
			this.usuario = usuario;
			this.TipoRegistro = tipoRegistro;
			this.Sql = sql;
			this.CargarPilaLlamados(tipoRegistro, excepcion);
			this.Observaciones = Utilidades.CambiarSaltosLinea(observaciones);
		}

		public DateTime Fecha {get{return this.fecha;}set{this.fecha = value;}}
		public string Dia {get{return this.fecha.ToShortDateString();}set{}}
		public string Hora {get{return this.fecha.ToShortTimeString();}set{}}
		public string Usuario {get{return this.usuario;}set{this.usuario = value;}}
		public TipoRegistro TipoRegistro {get{return this.tipoRegistro;}set{this.tipoRegistro = value;}}
		public string Sql {get{return this.sql;}set{this.sql = value;}}
		public string NombreExcepcion {get{return this.nombreExcepcion;}set{this.nombreExcepcion = value;}}
		public string MensajeExcepcion {get{return this.mensajeExcepcion;}set{this.mensajeExcepcion = value;}}
		public string PilaLLamados {get{return this.pilaLlamados;}set{this.pilaLlamados = value;}}
		public string Observaciones {get{return this.observaciones;}set{this.observaciones = value;}}

		private void CargarPilaLlamados(TipoRegistro tipoRegistro, Exception excepcion)
		{
			if(excepcion == null)
			{
				if (tipoRegistro == DB.TipoRegistro.Actividad)
					this.PilaLLamados = string.Empty;
				else
					this.PilaLLamados = new StackTrace().ToString();
			}
			else
			{
				this.NombreExcepcion = excepcion.GetType().FullName;
				this.MensajeExcepcion = excepcion.Message;
				this.PilaLLamados = new StackTrace().ToString();
			}
		}
	}

	public class ListaRegistro : ArrayList
	{
		public int Add(Registro value)
		{
			return base.Add(value);
		}

		public new Registro this[int index]
		{
			get
			{
				return (Registro)base[index];
			}
			set
			{
				base[index] = value;
			}
		}
	}

	public class ArchivoRegistro
	{
		private ListaRegistro listaRegistroActividad;
		private ListaRegistro listaRegistroError;
		private ListaRegistro listaRegistroInformacion;
		private ListaRegistro listaRegistroErrorCrystalReports;
		private ListaRegistro listaRegistroInformacionCrystalRepots;

		public ArchivoRegistro()
		{
			this.listaRegistroActividad = new ListaRegistro();
			this.listaRegistroError = new ListaRegistro();
			this.listaRegistroInformacion = new ListaRegistro();
			this.ListaRegistroErrorCrystalReports = new ListaRegistro();
			this.ListaRegistroInformacionCrystalRepots = new ListaRegistro();
		}

		public ListaRegistro ListaRegistroActividad {get{return this.listaRegistroActividad;}set{this.listaRegistroActividad = value;}}
		public ListaRegistro ListaRegistroError {get{return this.listaRegistroError;}set{this.listaRegistroError = value;}}
		public ListaRegistro ListaRegistroInformacion {get{return this.listaRegistroInformacion;}set{this.listaRegistroInformacion = value;}}
		public ListaRegistro ListaRegistroErrorCrystalReports {get{return this.listaRegistroErrorCrystalReports;}set{this.listaRegistroErrorCrystalReports = value;}}
		public ListaRegistro ListaRegistroInformacionCrystalRepots {get{return this.listaRegistroInformacionCrystalRepots;}set{this.listaRegistroInformacionCrystalRepots = value;}}

		public void AgregarRegistro(Registro registro)
		{
			switch(registro.TipoRegistro)
			{
				case TipoRegistro.Actividad:
					this.ListaRegistroActividad.Add(registro);
					break;
				case TipoRegistro.Error:
					this.ListaRegistroError.Add(registro);
					break;
				case TipoRegistro.Informacion:
					this.ListaRegistroInformacion.Add(registro);
					break;
				case TipoRegistro.ErrorCrystalReports:
					this.ListaRegistroErrorCrystalReports.Add(registro);
					break;
				case TipoRegistro.InformacionCrystalRepots:
					this.ListaRegistroInformacionCrystalRepots.Add(registro);
					break;
			}
		}
	}

	public class SerializacionXML
	{
		public static void SerializarObjetoArchivo(object objeto, string nombreArchivo)
		{
			FileStream  flujoArchivo = new FileStream(nombreArchivo, FileMode.Create, FileAccess.Write);

			SerializarObjeto(objeto,flujoArchivo);
		}

		public static void SerializarObjeto(object objeto, Stream flujo)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(objeto.GetType());

			xmlSerializer.Serialize(flujo,objeto);

			flujo.Close();
		}

		public static object DeserializarObjetoArchivo(Type tipoDatos, string nombreArchivo)
		{
			FileStream  flujoArchivo = new FileStream(nombreArchivo, FileMode.Open, FileAccess.Read);

			return DeserializarObjeto(tipoDatos,flujoArchivo);
		}

		public static object DeserializarObjeto(Type tipoDatos, Stream flujo)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(tipoDatos);
            object objeto = xmlSerializer.Deserialize(flujo);
            
			flujo.Close();

			return objeto;
		}
	}

	public class AdministradorCarpetasRegistro
	{
		public static void CrearCarpetaRegistro(CarpetaRegistro carpetaRegistro)
		{
			if(!AdministradorCarpetasRegistro.ExisteCarpetaRegistro(carpetaRegistro))
				Directory.CreateDirectory(carpetaRegistro.CarpetaRaizCompleto);
		}

		public static bool ExisteCarpetaRegistro(CarpetaRegistro carpetaRegistro)
		{
			return Directory.Exists(carpetaRegistro.CarpetaRaizCompleto);
		}
	
		public static bool ExisteArchivoRegistro(CarpetaRegistro carpetaRegistro)
		{
			return File.Exists(carpetaRegistro.NombreArchivoCompleto);
		}

		public static ArchivoRegistro CargarArchivoRegistro(CarpetaRegistro carpetaRegistro)
		{
			if(AdministradorCarpetasRegistro.ExisteArchivoRegistro(carpetaRegistro))
				return (ArchivoRegistro)SerializacionXML.DeserializarObjetoArchivo(typeof(ArchivoRegistro),carpetaRegistro.NombreArchivoCompleto);
			else
				return new ArchivoRegistro();
		}

		public static void GuardarArchivoRegistro(CarpetaRegistro carpetaRegistro, ArchivoRegistro archivoRegistro)
		{
			AdministradorCarpetasRegistro.CrearCarpetaRegistro(carpetaRegistro);

			SerializacionXML.SerializarObjetoArchivo(archivoRegistro,carpetaRegistro.NombreArchivoCompleto);
		}
		
		public static void GrabarLogs(TipoRegistro tipoRegistro, string sql, Exception excepcion, string observaciones)
		{
			string pathlogs = ConfigurationManager.AppSettings["PathToLogs"];
			string aplicalogs = ConfigurationManager.AppSettings["AplicaLogs"];
			string usuario;
			string nombreArchivo = "Registro";

            if(HttpContext.Current!=null)
                usuario = HttpContext.Current.User.Identity.Name.ToString().ToLower();
            else usuario="automatico";
			
			bool registrarLog = false;

            if (tipoRegistro != TipoRegistro.Actividad || aplicalogs == "true")
				registrarLog = true;
			
			if (registrarLog)
			{
                if (pathlogs == "") throw new Exception(excepcion.ToString()); //return;

				CarpetaRegistro carpetaRegistro = new CarpetaRegistro(pathlogs,usuario,nombreArchivo);

				ArchivoRegistro archivoRegistro = AdministradorCarpetasRegistro.CargarArchivoRegistro(carpetaRegistro);

				Registro registro = new Registro(usuario, tipoRegistro, sql, excepcion, observaciones);

				archivoRegistro.AgregarRegistro(registro);

				AdministradorCarpetasRegistro.GuardarArchivoRegistro(carpetaRegistro,archivoRegistro);
			}
		}
	}

	public class Utilidades
	{
		public static string CambiarSaltosLinea(string cadena)
		{
			return cadena.Replace("<br>","\n");
		}
	}
}
