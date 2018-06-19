using System;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Comercial
{
	/// <summary>
	/// Descripción breve de AMS_Comercial_Anticipo.
	/// </summary>
	public class AMS_Comercial_Anticipo : System.Web.UI.Page
	{
		public string txtAnticipo;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(Request.QueryString["ant"]!=null)
				this.Generar_Anticipo(Request.QueryString["ant"]);
		}

		protected void Generar_Anticipo(string numero)
		{
			string plantilla="";
			string nlchar="`",redChar="^";
			int anchoTiquete=Tiquetes.anchoTiquete;
			try
			{
				string strLinea="";
				StreamReader strArchivo;
				strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaAnticipo.txt");
				strLinea=strArchivo.ReadLine();
				//La primera linea puede contener el ancho del tiquete
				try
				{
					anchoTiquete=Int16.Parse(strLinea);
					strLinea=strArchivo.ReadLine();}
				catch{}

				while(strLinea!=null)
				{
					plantilla+=strLinea+nlchar;
					strLinea=strArchivo.ReadLine();
				}
				strArchivo.Close();
			}
			catch
			{
                Utils.MostrarAlerta(Response, "No se ha creado la plantilla de anticipos, no se pudo imprimir.");
				return;
			}


			
			//descripcion=Tiquetes.CortarTexto(dsAnticipo.Tables[0].Rows[0]["DESCRIPCION"].ToString(),anchoTiquete);

			txtAnticipo=Plantillas.GenerarAnticipo(numero, plantilla, nlchar, redChar, anchoTiquete);

			//plantilla=plantilla.Replace("<DESCRIPCION>",descripcion);

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
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
